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
            Assert.IsTrue(Units.One / Units.One  == Units.One);
            
            Assert.IsTrue(Units.One == Units.Shares / Units.Shares);
            Assert.IsTrue(Units.One == Units.GBP / Units.GBP);
            Assert.IsTrue(Units.One / Units.GBP == Units.Shares / (Units.Shares * Units.GBP));
        }

        [Test]
        public void TestSimplifyForSimpleCancels()
        {
            var units = new Units(new[] { Unit._Shares }, new[] { Unit._Shares });
            units.Simplify();
            Assert.IsTrue(Units.One == units);
        }
    }
}