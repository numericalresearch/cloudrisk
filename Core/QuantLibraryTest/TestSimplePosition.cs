using Moq;
using NUnit.Framework;
using QuantLibrary;

namespace QuantLibraryTest
{
    class UnitEuroValueInstrument : IInstrument
    {
        public Amount Value(IMarketEnv marketEnv)
        {
            return new Amount(1, "EUR");
        }

        public CalcResults CalculateRisk(IMarketEnv marketEnv, RiskParameters riskParameters)
        {
            throw new System.NotImplementedException();
        }
    }

    [TestFixture]
    public class TestSimplePosition
    {
        [Test]
        public void TestThatValueScalesWithQuantity()
        {
            var position = new SimplePosition(new UnitEuroValueInstrument(), 50);
            var mockMarketEnv = new Mock<IMarketEnv>();
            
            
            Assert.AreEqual(new Amount(50, "EUR"), position.Value(mockMarketEnv.Object)); 
        }
        
        [Test]
        public void TestThatSimpleRisksScaleWithQuantity()
        {
            // TODO - how do I want to define risks 
        }
    }
}