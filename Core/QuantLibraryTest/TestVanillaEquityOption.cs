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
            var ccy = "GBP";
            var stock = new Stock(ticker, ccy);
            
            var strike = 100;
            var today = new LocalDate(2021, 12, 3);
            var expiry = new LocalDate(2022, 3, 3);
            
            var mktEnv = new SimpleMarketEnv();
            mktEnv.PricingDate = today;

            var option = new VanillaEquityOption(stock, strike, PutCall.Call, expiry);

            var price = option.Value(mktEnv);
            // TODO 
        }
    }
}