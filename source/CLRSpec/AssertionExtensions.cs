using NUnit.Framework;

namespace CLRSpec
{
    /// <summary>
    /// Our own fluent assertions are required, because the existing fluent assertion
    /// APIs either don't read correctly when parsed, or can't be used
    /// in lambdas due to optional params.
    /// todo: Add basic assertions for values, strings, collections.
    /// </summary>
    public static class AssertionExtensions
    {
        public static bool ShouldBe<T>(this T actual, T value)
        {
            Assert.AreEqual(value, actual);
            return true;
        }
    }
}
