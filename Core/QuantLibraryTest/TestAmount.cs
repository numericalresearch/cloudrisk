using Microsoft.FSharp.Collections;
using NUnit.Framework;
using QuantLibrary;

namespace QuantLibraryTest
{
    [TestFixture]
    public class TestAmount
    {
        [Test]
        public void TestRelationalOperators()
        {
            var oneEuro = new Amount(1, Units.EUR);
            var twoEuro = new Amount(2, Units.EUR);
            
            Assert.That(oneEuro < twoEuro);
            Assert.That(twoEuro > oneEuro);
        }
        
        [Test]
        public void TestMultiplication()
        {
            var oneEuro = new Amount(1m, Units.EUR);
            var twoEuro = new Amount(2m, Units.EUR);
            
            Assert.That(2m * oneEuro == twoEuro);
            Assert.That(oneEuro * 2m == twoEuro);
        }

        [Test]
        public void TestMultiplyingAmounts()
        {
            var a = new Amount(2m, Units.EUR);
            var b = new Amount(3m, Units.EUR);
            Assert.That(a * b == new Amount(6m, Units.EUR * Units.EUR));
        }

        [Test]
        public void TestDividingAmounts()
        {
            var a = new Amount(2m, Units.EUR);
            var b = new Amount(3m, Units.DKK);
            Assert.That(a / b == new Amount(2m / 3m , Units.EUR / Units.DKK));
        }
    }
}