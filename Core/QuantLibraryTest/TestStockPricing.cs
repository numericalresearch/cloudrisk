using NUnit.Framework;
using QuantLibrary;

namespace QuantLibraryTest
{
    [TestFixture]
    public class TestStockPricing
    {
        [Test]
        public void TestPricingAStock()
        {
            var ticker = "ABCD.L";
            var ccy = Units.GBP;
            var stock = new Stock(ticker, ccy);
           
            var mkt = new SimpleMarketSnapshot();
            var price = new Amount(123.45m, Units.GBP);
            mkt.SetPrice(MarketKey.StockPrice(stock), price);

            var risks = stock.CalculateRisk(mkt, RiskParameters.Default);
            Assert.AreEqual(risks.Value, price);
            Assert.AreEqual(risks.DollarGreeks.PV, price);
            Assert.AreEqual(risks.DollarGreeks.Delta, 1 * Units.One);
            Assert.AreEqual(risks.DollarGreeks.Gamma, 0 * Units.One / stock.Currency);
        }

        [Test]
        public void TestPricingAStockWithMissingMarketData()
        {
            var ticker = "ABCD.L";
            var ccy = Units.GBP;
            var stock = new Stock(ticker, ccy);
           
            var mkt = new SimpleMarketSnapshot();
            Assert.Throws<MarketDataException>(() => stock.CalculateRisk(mkt, RiskParameters.Default));
        }
    }
}