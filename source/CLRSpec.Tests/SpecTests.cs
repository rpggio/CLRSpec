using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace CLRSpec.Tests
{
    [TestFixture]
    public class SpecTests
    {
        [Test]
        public void RunSpec()
        {
            var calc = new Calculator();
            var output = new StringWriter();
            Spec.Output = output;
            Spec.Run(s => s
                .Given(calc.SetPowerTo(true))
                .When(calc.Button(5).Push())
                .And(calc.PlusButton.Push())
                .And(calc.Button(6).Push())
                .And(calc.EqualsButton.Push())
                .Then(calc.Display.ShouldBe("11"))
                );
            const string expected =
                @"Given I set power to True       => Passed
                When I push button 5              => Passed
                And push plus button            => Passed
                And push button 6               => Passed
                And push equals button          => Passed
                Then display should be ""11""   => Passed";
            string actual = output.ToString();
            Assert.AreEqual(Clean(expected), Clean(actual));
        }

        private static string Clean(string value)
        {
            return Regex.Replace(value, "\\s+", " ").Trim();
        }
    }
}