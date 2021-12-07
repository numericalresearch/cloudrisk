using System;
using Castle.Core.Configuration;
using NUnit.Framework;
using QuantLibrary;

// test key errors etc https://docs.nunit.org/articles/nunit/writing-tests/assertions/classic-assertions/Assert.Throws.html

namespace QuantLibraryTest
{
    [TestFixture]
    public class TestSimpleMarketSnapshot
    {
        private static bool AlmostEquals(double x, double y)
        {
            return Math.Abs(x - y) < 1e-7;
        }
        
        [Test]
        public void TestFXConversions()
        {
            var env = new SimpleMarketSnapshot();
            Assert.Throws<MarketDataException>(() => env.GetFXRate(Units.GBP, Units.EUR));
            Assert.Throws<MarketDataException>(() => env.GetFXRate(Units.GBP, Units.USD));
            Assert.Throws<MarketDataException>(() => env.Convert(new Amount(100, Units.GBP), Units.EUR));
            
            env.SetFXRate(Units.GBP, Units.EUR, 1.1);
            Assert.That(AlmostEquals(env.GetFXRate(Units.GBP, Units.EUR), 1.1));
            Assert.That(AlmostEquals(env.GetFXRate(Units.EUR, Units.GBP), 1/1.1));
            var converted = env.Convert(new Amount(100, Units.GBP), Units.EUR);
            Assert.That(AlmostEquals(converted.Value, 110.0));
            Assert.AreEqual(converted.Units, Units.EUR);

        }
        
        [Test]
        public void TestThatWeCanGetAndSetPrices()
        {
            var env = new SimpleMarketSnapshot();
            

        }

        [Test]
        public void TestThatWeGetTheRightErrorsForMissingPrices()
        {
            
        }

        [Test]
        public void TestThatPricesAreCorrectlyInterpolatedBackwards()
        {
            
        }
    }
}