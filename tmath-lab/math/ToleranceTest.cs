using tmath;

namespace tmath_lab.math
{
    [TestClass]
    public class ToleranceTest
    {
        [TestMethod("Tolerance Default")]
        public void Test_Default_Value()
        {
            Tolerance tol = default;
            Console.WriteLine(tol.ToString());
            Assert.IsNotNull(tol);
        }
    }
}
