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
            Assert.IsTrue(Unit.GBP == Unit.Shares / Unit.GBP * (Unit.GBP / Unit.Shares) * Unit.GBP);
            Assert.IsTrue(Unit.One * Unit.GBP == Unit.GBP);
            Assert.IsTrue(Unit.GBP * Unit.One == Unit.GBP);
            Assert.IsTrue(Unit.One / Unit.One  == Unit.One);
            
            Assert.IsTrue(Unit.One == Unit.Shares / Unit.Shares);
            Assert.IsTrue(Unit.One == Unit.GBP / Unit.GBP);
            Assert.IsTrue(Unit.One / Unit.GBP == Unit.Shares / (Unit.Shares * Unit.GBP));
        }

        [Test]
        public void TestSimplifyForSimpleCancels()
        {
            var units = new Units(new[] { Unit.Shares }, new[] { Unit.Shares });
            units.Simplify();
            Assert.IsTrue(Unit.One == units);
        }
    }
}