using NodaTime;
using NUnit.Framework;
using QuantLibrary;

namespace QuantLibraryTest
{
    [TestFixture]
    public class TestPortfolioPricing
    {
        private static readonly LocalDate _today = new LocalDate(2021, 12, 6); 
        
        private IMarketSnapshot BuildSingleStockMarketSnapshot(Stock stock)
        {
            var snapshot = new SimpleMarketSnapshot();
            snapshot.PricingDate = _today;
            snapshot.SetPrice(MarketKey.StockPrice(stock), new Amount(90, stock.Currency));
            snapshot.SetItem(MarketKey.DiscountCurve(stock.Currency), new FlatDiscountCurve(_today, 0.05));
            snapshot.SetItem(MarketKey.VolSurface(stock), new FlatVolSurface(0.20));
            return snapshot;
        }
        
        [Test]
        public void TestSimplePortfolio()
        {
            var ccy = Units.GBP;
            
            var stock = new Stock("ABCD.L", ccy);
            var stockPosition = new SimplePosition(stock, 50);

            var option = new VanillaEquityOption(stock, 100, PutCall.Call, _today + Period.FromDays(180));
            var optionPosition = new SimplePosition(option, -100);
            
            var portfolio = new Portfolio();
            portfolio.Add(stockPosition);
            portfolio.Add(optionPosition);

            var snapshot = BuildSingleStockMarketSnapshot(stock);

            var risksByItem = portfolio.CalculatePositionRisks(snapshot, RiskParameters.Default);
            var aggregatedRisk = Portfolio.AggregatePositionRisks(risksByItem, snapshot, RiskParameters.Default);
            
            Assert.IsTrue(risksByItem[0].Value + risksByItem[1].Value == aggregatedRisk.Value);
        }

        [Test]
        public void TestMultiCurrencyPortfolio()
        {
            // TODO           
        }

        [Test]
        public void TestNestedPortfolio()
        {
            // TODO 
        }
    }
}