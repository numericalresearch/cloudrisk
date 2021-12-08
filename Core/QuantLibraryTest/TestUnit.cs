using System;
using System.Collections.Generic;
using NUnit.Framework;
using QuantLibrary;

namespace QuantLibraryTest
{

    [TestFixture]
    public class TestUnit
    {

        [Test]
        public void TestOutSyntax()
        {
            Assert.IsTrue(Units.GBP == Units.Shares / Units.GBP * (Units.GBP / Units.Shares) * Units.GBP);
            Assert.IsTrue(Units.One * Units.GBP == Units.GBP);
            Assert.IsTrue(Units.GBP * Units.One == Units.GBP);
            Assert.IsTrue(Units.One / Units.One == Units.One);

            Assert.IsTrue(Units.One == Units.Shares / Units.Shares);
            Assert.IsTrue(Units.One == Units.GBP / Units.GBP);
            Assert.IsTrue(Units.One / Units.GBP == Units.Shares / (Units.Shares * Units.GBP));
        }

        [Test]
        public void TestSimplifyForOnes()
        {
            var oneDivOne = new Units(new[] { Unit._One }, new[] { Unit._One });
            oneDivOne.Simplify();
            Assert.IsTrue(Units.One == oneDivOne);

            var oneTimesOne = new Units(new[] { Unit._One, Unit._One }, new Unit[] { });
            oneTimesOne.Simplify();
            Assert.IsTrue(Units.One == oneTimesOne);

        }

        [Test]
        public void TestSimplifyForSimpleCancels()
        {
            var units = new Units(new[] { Unit._Shares }, new[] { Unit._Shares });
            units.Simplify();
            Assert.IsTrue(Units.One == units);
        }

        [Test]
        public void TestSimplifyForHigherOrderCancels()
        {
            var units = new Units(
                new[] { Unit._EUR, Unit._Shares, Unit._EUR, Unit._EUR },
                new[] { Unit._Shares, Unit._EUR, Unit._EUR, });
            units.Simplify();
            Assert.IsTrue(Units.EUR == units);
        }

        [Test]
        public void TestToString()
        {
            Assert.AreEqual("GBP", Units.GBP.ToString());
            Assert.AreEqual("1 / GBP", (Units.One / Units.GBP).ToString());
            Assert.AreEqual("EUR / GBP", (Units.EUR / Units.GBP).ToString());
            Assert.AreEqual("[EUR Shares ] / [GBP USD ]",
                (Units.EUR * Units.Shares / (Units.GBP * Units.USD)).ToString());
            // NOTE - not perfect formatting, but good enough for now
        }

        [Test]
        public void TestRepeatedUnits()
        {
            Assert.AreEqual("1 / [GBP GBP ]", ((Units.One / Units.GBP) / Units.GBP).ToString());
            Assert.AreEqual("[DKK DKK ] / [GBP GBP ]", ((Units.DKK * Units.DKK / Units.GBP) / Units.GBP).ToString());
        }

        [Test]
        public void TestIsTrivial()
        {
            Assert.IsTrue(Units.DKK.IsTrivial());
            Assert.IsTrue(Units.Shares.IsTrivial());
            Assert.IsTrue(Units.One.IsTrivial());

            Assert.IsTrue((Units.One * Units.One).IsTrivial(out var a));
            Assert.IsTrue(a == Unit._One);
            Assert.IsTrue((Units.One / Units.One).IsTrivial(out var b));
            Assert.IsTrue(b == Unit._One);

            Assert.IsFalse((Units.DKK * Units.DKK).IsTrivial());
            Assert.IsFalse((Units.EUR * Units.DKK).IsTrivial());
            Assert.IsFalse((Units.EUR / Units.DKK).IsTrivial());
            Assert.IsFalse((Units.One / Units.DKK).IsTrivial());
            Assert.IsFalse((Units.One / (Units.DKK * Units.DKK)).IsTrivial());
        }

        [Test]
        public void TestInteractionBetweenDecimalsAndUnits()
        {
            Assert.IsTrue(new Amount(100m, Units.EUR) == 100m * Units.EUR);
            Assert.IsTrue(new Amount(100m, Units.EUR) == Units.EUR * 100m);
        }

        [Test]
        public void TestInteractionBetweenAmountsAndUnits()
        {
            
        }

        [Test]
        public void TestIdentityStuff()
        {
            var a = Units.DKK;
            var b = new Units(Unit._DKK);
            
            Assert.IsTrue(a == b);
            Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(b.Equals(a));
            
            var hash_a = a.GetHashCode();
            var hash_b = b.GetHashCode();

            // var hash_a_n = a._numerator.GetHashCode();
            // var hash_b_n = b._numerator.GetHashCode();
            // var hash_a_d = a._denominator.GetHashCode();
            // var hash_b_d = b._denominator.GetHashCode();
            // Assert.IsTrue(hash_a_n == hash_b_n);
            // Assert.IsTrue(hash_a_d == hash_b_d);
            
            Assert.IsTrue(hash_a == hash_b);
        }

        [Test]
        public void TestUnitHashing()
        {
            var x = new KeyValuePair<string, int>("wibble", 17);
            var y = new KeyValuePair<string, int>("wibble", 17);
            Assert.IsTrue(x.GetHashCode() == y.GetHashCode());
            
            var a = new Unit("wubble");
            var b = new Unit("wubble");
            Assert.IsTrue(a.GetHashCode() == b.GetHashCode());
        }

        struct A
        {
            public int value;
            public string s;
            public Dictionary<string, int> d;
        }
        
        [Test]
        public void TestStructHash()
        {
            var a = new A();
            a.value = 1;
            a.s = "wibble";
            a.d = new Dictionary<string, int>();
            a.d["foo"] = 7;
            
            var b = new A();
            b.value = 1;
            b.s = "wibble";
            b.d = new Dictionary<string, int>();
            b.d["foo"] = 7;

            var hash_a = a.GetHashCode();
            var hash_b = b.GetHashCode();
            Assert.IsTrue(hash_a == hash_b);
        }
    }
}