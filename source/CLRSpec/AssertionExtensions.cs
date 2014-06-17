using NUnit.Framework;

namespace CLRSpec
{
    public static class AssertionExtensions
    {
        public static bool ShouldBe<T>(this T actual, T value)
        {
            Assert.AreEqual(value, actual);
            return true;
        }
    }
}
