using NUnit.Framework;
using QuantLibrary;

// test key errors etc https://docs.nunit.org/articles/nunit/writing-tests/assertions/classic-assertions/Assert.Throws.html

namespace QuantLibraryTest
{
    [TestFixture]
    public class TestSimpleMarketEnv
    {
        [Test]
        public void TestThatWeCanGetAndSetPrices()
        {
            var env = new SimpleMarketEnv();
            

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