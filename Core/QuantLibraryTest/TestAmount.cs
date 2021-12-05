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
            var oneEuro = new Amount(1, "EUR");
            var twoEuro = new Amount(2, "EUR");
            
            Assert.That(oneEuro < twoEuro);
            Assert.That(twoEuro > oneEuro);
        }
        
        [Test]
        public void TestMultiplication()
        {
            var oneEuro = new Amount(1, "EUR");
            var twoEuro = new Amount(2, "EUR");
            
            Assert.That(2 * oneEuro == twoEuro);
            Assert.That(oneEuro * 2 == twoEuro);
        }
        
    }
}