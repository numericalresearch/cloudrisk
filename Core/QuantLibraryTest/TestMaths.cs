using System;
using NUnit.Framework;
using QuantLibrary;

namespace QuantLibraryTest
{
    [TestFixture]
    public class TestMaths
    {
        private static readonly double Tolerance = 1 / Math.Pow(10, 6);
        [TestCase(-5, 0)]
        [TestCase(-2, 0.02275013)]
        [TestCase(-1, 0.15865525)]
        [TestCase(0, 0.5)]
        [TestCase(1, 0.841344746)]
        [TestCase(2, 0.977249868)]
        [TestCase(5, 1)]
        public void TestCumulativeNormal(double x, double y)
        {
            double actual = Maths.N(x);
            Assert.That(actual, Is.EqualTo(y).Within(Tolerance));
        }
        
    }
}