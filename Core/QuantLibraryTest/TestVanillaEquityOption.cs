using NodaTime;
using NUnit.Framework;
using QuantLibrary;

namespace QuantLibraryTest
{
    [TestFixture]
    public class TestVanillaEquityOption
    {
        [Test]
        public void TestRiskMeasuresOnSimpleVanillaCall()
        {
            var ticker = "ABCD.L";
            var ccy = Units.GBP;
            var stock = new Stock(ticker, ccy);
            
            var strike = 100;
            var today = new LocalDate(2021, 12, 3);
            var expiry = new LocalDate(2022, 12, 3);
            
            var mkt = new SimpleMarketSnapshot();
            mkt.PricingDate = today;
            
            mkt.SetPrice(MarketKey.StockPrice(stock), new Amount(90, ccy));
            mkt.SetItem(MarketKey.DiscountCurve(ccy), new FlatDiscountCurve(today, 0.05));
            mkt.SetItem(MarketKey.VolSurface(stock), new FlatVolSurface(0.20));
            
            var option = new VanillaEquityOption(stock, strike, PutCall.Call, expiry);

            var risks = option.CalculateRisk(mkt, RiskParameters.Default);
            Assert.That(risks.BlackScholesGreeks.PV > 0.5);
            Assert.That(risks.BlackScholesGreeks.Delta > 0.0);  // Call delta should be positive
            Assert.That(risks.BlackScholesGreeks.Gamma > 0.0);  // Call Gamma should be positive
            Assert.That(risks.BlackScholesGreeks.Vega > 0.0);   // Call vega should be positive
            Assert.That(risks.BlackScholesGreeks.Theta < 0.0);   // Call theta should be negative
            Assert.That(risks.BlackScholesGreeks.Rho >  0.0);   // Call rho should be positive
                
            Assert.That(risks.DollarGreeks.PV.Value > 5.0m);
        }
    }
}