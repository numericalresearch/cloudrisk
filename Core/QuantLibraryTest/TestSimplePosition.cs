using Moq;
using NUnit.Framework;
using QuantLibrary;

namespace QuantLibraryTest
{
    class UnitEuroValueInstrument : IInstrument
    {
        public Amount Value(IMarketSnapshot marketSnapshot)
        {
            return new Amount(1, "EUR");
        }

        public CalcResults CalculateRisk(IMarketSnapshot marketSnapshot, RiskParameters riskParameters)
        {
            var res = new CalcResults("EUR");
            res.BlackScholesGreeks.PV = 1;
            res.BlackScholesGreeks.Delta = 1;
            res.BlackScholesGreeks.Gamma = 0;
            res.BlackScholesGreeks.Vega = 0;
            res.BlackScholesGreeks.Theta = 0;
            res.BlackScholesGreeks.Rho = 0;

            res.DollarGreeks.PV = new Amount(1, "EUR");
            res.DollarGreeks.Delta = new Amount(1, "EUR");
            res.DollarGreeks.Gamma = new Amount(0, "EUR");
            res.DollarGreeks.Vega = new Amount(0, "EUR");
            res.DollarGreeks.Theta = new Amount(0, "EUR");
            res.DollarGreeks.Rho = new Amount(0, "EUR");

            return res;            
        }
    }

    [TestFixture]
    public class TestSimplePosition
    {
        [Test]
        public void TestThatSimpleRisksScaleWithQuantity()
        {
            var position = new SimplePosition(new UnitEuroValueInstrument(), 50);
            var mockMarketEnv = new Mock<IMarketSnapshot>();

            var risks = position.CalculateRisk(mockMarketEnv.Object, RiskParameters.Default); 
            Assert.AreEqual(new Amount(50, "EUR"), risks.DollarGreeks.PV); 
            // TODO - how do I want to define risks 
        }
    }
}