using System;
using Castle.Core.Configuration;
using NodaTime;
using NUnit.Framework;
using QuantLibrary;

// test key errors etc https://docs.nunit.org/articles/nunit/writing-tests/assertions/classic-assertions/Assert.Throws.html

namespace QuantLibraryTest
{
    [TestFixture]
    public class TestSimpleMarketSnapshot
    {
        [Test]
        public void TestThatWeThrowOnMissingFxRates()
        {
            var env = new SimpleMarketSnapshot();
            Assert.Throws<MarketDataException>(() => env.GetFXRate(Units.GBP, Units.EUR));
            Assert.Throws<MarketDataException>(() => env.GetFXRate(Units.GBP, Units.USD));
            Assert.Throws<MarketDataException>(() => env.Convert(new Amount(100, Units.GBP), Units.EUR));
        }

        [Test]
        public void TestGettingSimpleAndInvertedFxRates()
        {
            var env = new SimpleMarketSnapshot();
            env.SetFXRate(Units.GBP, Units.EUR, 1.1m);
            Assert.That(env.GetFXRate(Units.GBP, Units.EUR) == 1.1m * Units.EUR / Units.GBP);
            Assert.That(env.GetFXRate(Units.EUR, Units.GBP) == 1 / 1.1m * Units.GBP / Units.EUR);
        }

        [Test]
        public void TestSimpleUnitFxConversion()
        {
            var env = new SimpleMarketSnapshot();
            env.SetFXRate(Units.GBP, Units.EUR, 1.1m);
            var converted = env.Convert(new Amount(100, Units.GBP), Units.EUR);
            Assert.That(converted.Value == 110.0m);
            Assert.AreEqual(converted.Units, Units.EUR);
        }

        [Test]
        public void TestFxConversionWithComplexUnits()
        {
            var env = new SimpleMarketSnapshot();
            env.SetFXRate(Units.DKK, Units.EUR, 0.1m);

            Assert.AreEqual(1000 * Units.DKK, env.Convert(100 * Units.EUR, Units.DKK));
            Assert.AreEqual(10 * Units.EUR, env.Convert(100 * Units.DKK, Units.EUR));

            Assert.AreEqual(10 * Units.One / Units.DKK, env.Convert(100m * (Units.One / Units.EUR), Units.DKK));
            Assert.AreEqual(1000 * (Units.One / Units.EUR), env.Convert(100m * (Units.One / Units.DKK), Units.EUR));

        }

        [Test]
        public void TestThatWeCanGetAndSetPrices()
        {
            var key = MarketKey.StockPrice(new Stock("ZZZZ.F", Units.EUR));
            var price = new Amount(789, Units.EUR);

            var env = new SimpleMarketSnapshot();
            env.SetPrice(key, price);
            Assert.IsTrue(env.GetPrice(key, out var retrievedPrice));
            Assert.IsTrue(price == retrievedPrice);

            Assert.IsTrue(price == env.GetPrice(key));
        }

        [Test]
        public void TestThatWeGetTheRightErrorsForMissingPrices()
        {
            var key = MarketKey.StockPrice(new Stock("ZZZZ.F", Units.EUR));
            var env = new SimpleMarketSnapshot();
            Assert.IsFalse(env.GetPrice(key, out var _));
            Assert.Throws<MissingMarketDataException>(() => env.GetPrice(key));
        }


        [Test]
        public void TestGettingAndRetrievingObjects()
        {
            var env = new SimpleMarketSnapshot();
            var key = MarketKey.DiscountCurve(Units.DKK);
            Assert.IsFalse(env.GetItem(key, out IDiscountCurve? _));

            var curve = new FlatDiscountCurve(new LocalDate(2021, 12, 12), 0.01);
            env.SetItem(key, curve);
            Assert.IsTrue(env.GetItem(key, out IDiscountCurve? retrievedCurve));
            Assert.IsNotNull(retrievedCurve);

            var futureDate = new LocalDate(2022, 12, 12);
            Assert.IsTrue(curve.df(futureDate) == retrievedCurve?.df(futureDate));
            
            


        }

        [Test]
        public void TestThatWeGetTheRightErrorBadlyTypedObjects()
        {
            var env = new SimpleMarketSnapshot();
            var key = MarketKey.DiscountCurve(Units.DKK);
            env.SetItem(key, new FlatVolSurface(0.25));
            Assert.Throws<MarketDataException>(
                () => env.GetItem(key, out IDiscountCurve? _));
        }
    }
}