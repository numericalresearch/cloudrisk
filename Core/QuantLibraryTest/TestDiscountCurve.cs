using NodaTime;
using NuGet.Frameworks;
using NUnit.Framework;
using QuantLibrary;

namespace QuantLibraryTest
{
    [TestFixture]
    public class TestDiscountCurve
    {
        [Test]
        public void TestDiscountFactor()
        {
            var today = new LocalDate(2021, 1, 1);
            var curve = new FlatDiscountCurve(today, 0.05);
            
            Assert.IsTrue(1 == curve.df(today));
            Assert.IsTrue(curve.df(today + Period.FromDays(1)) < 1);
            Assert.IsTrue(0 < curve.df(today + Period.FromDays(1)));
            
            Assert.IsTrue(curve.df(today + Period.FromYears(1)) < 1);
            Assert.IsTrue(0 < curve.df(today + Period.FromYears(1)));
        }
        

        [Test]
        public void TestForwardDiscountFactor()
        {
            // given t0 < t1 < t2, df(t0, t1) * df (t1, t2) == df(t0, t2)
            var d0 = new LocalDate(2021, 12, 3);
            var d1 = d0 + Period.FromYears(1);
            var d2 = d0 + Period.FromYears(2);

            var curve = new FlatDiscountCurve(d0, 0.05);

            var df_d0_d2 = curve.df(d2);
            var df_d0_d1 = curve.df(d0, d1);
            var df_d1_d2 = curve.df(d0, d1);
            
            Assert.AreEqual(df_d0_d2, df_d0_d1 * df_d1_d2);
        }

        [Test]
        public void TestDateBoundaries()
        {
            var curve = new FlatDiscountCurve(new LocalDate(2021, 12, 3), 0.05);

            Assert.Throws<QuantLibraryException>(
                () => curve.df(new LocalDate(2020, 12, 3)));
            Assert.Throws<QuantLibraryException>(
                () => curve.df(
                    new LocalDate(2021, 12, 3),
                    new LocalDate(2021, 12, 1)));
        }

    }
}