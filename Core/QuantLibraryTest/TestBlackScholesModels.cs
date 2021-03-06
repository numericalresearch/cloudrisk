using System;
using NUnit.Framework;
using QuantLibrary;

// Look at property based testing here!

namespace QuantLibraryTest
{
    [TestFixture]
    public class TestBlackScholesModels
    {
        private static bool AlmostEquals(double x, double y, double tolerance = 1e-2)
        {
            return Math.Abs(x - y) < tolerance;
        }

        // A somewhat cursory test, verified against https://goodcalculators.com/black-scholes-calculator/, but 
        // sufficient in the context; we're not testing the model, we're just testing that we copied the formulas
        // correctly
        [Test]
        public void TestAtTheMoneySpotLongDatedOptions()
        {
            var callGreeks = BlackScholes.Call(100, 100, 1, 0.05, 0, 0.15);
            Assert.That(AlmostEquals(callGreeks.PV, 8.592));
            Assert.That(AlmostEquals(callGreeks.Delta, 0.658));
            Assert.That(AlmostEquals(callGreeks.Gamma, 0.024));
            Assert.That(AlmostEquals(callGreeks.Vega, 36.703));
            Assert.That(AlmostEquals(callGreeks.Theta, -5.616));
            Assert.That(AlmostEquals(callGreeks.Rho, 57.257));

            var putGreeks = BlackScholes.Put(100, 100, 1, 0.05, 0, 0.15);
            Assert.That(AlmostEquals(putGreeks.PV, 3.71));
            Assert.That(AlmostEquals(putGreeks.Delta, -0.342));
            Assert.That(AlmostEquals(putGreeks.Gamma, 0.024));
            Assert.That(AlmostEquals(putGreeks.Vega, 36.703));
            Assert.That(AlmostEquals(putGreeks.Theta, -0.859));
            Assert.That(AlmostEquals(putGreeks.Rho, -37.866));
        }

        [Test]
        public void TestPnLAttribution()
        {
            const double spot1 = 100.0;
            const double spot2 = 102.0;
            var greeks1  = BlackScholes.Call(spot1, 100, 1, 0.05, 0, 0.15);
            var greeks2  = BlackScholes.Call(spot2, 100, 1, 0.05, 0, 0.15);
            
            Console.WriteLine($"{greeks2.PV}, {greeks1.PV} + {greeks1.Delta} * {(spot2 - spot1)}");
            var estimatedPrice = 
                greeks1.PV 
                + greeks1.Delta * (spot2 - spot1) 
                + 0.5 * greeks1.Gamma * Math.Pow(spot2 - spot1, 2);
            Console.WriteLine($"{greeks2.PV}, {greeks1.PV} + {greeks1.Delta} * {(spot2 - spot1)} + gamma = {estimatedPrice} ");
            Assert.That(AlmostEquals(greeks2.PV, estimatedPrice));
        }

}
}