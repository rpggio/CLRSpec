using System;
using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace CLRSpec.Tests
{
    [TestFixture]
    public class SpecTests
    {
        [Test]
        public void PassingSpec()
        {
            var calc = new Calculator();
            var output = new StringWriter();
            Spec.Output = output;
            Spec.Run(s => s
                .AsA(Student())
                .Given(calc.SetPowerTo(true))
                .When(calc.Button(5).Push())
                .And(calc.PlusButton.Push())
                .And(calc.Button(6).Push())
                .And(calc.EqualsButton.Push())
                .Then(calc.Display.ShouldBe("11"))
                );
            const string expected = @"PassingSpec
                as a student                    => Passed
                given I set power to True       => Passed
                when I push button 5            => Passed
                and push plus button            => Passed
                and push button 6               => Passed
                and push equals button          => Passed
                then display should be ""11""   => Passed";
            string actual = output.ToString();
            Assert.That(Clean(actual), Is.EqualTo(Clean(expected)));
            Console.WriteLine(actual);
        }

        [Test]
        public void FailingSpec()
        {
            Console.WriteLine("------ This will read like a failing test when it succeeds. When it fails.. ------");
            Console.WriteLine();
            var calc = new Calculator();
            var output = new StringWriter();
            Spec.Output = output;
            Spec.Run(s => s
                .Given(calc.SetPowerTo(true))
                .Then(calc.Display.ShouldBe("1"))
                );
            const string expected = @"FailingSpec
                given I set power to True     => Passed
                then display should be ""1""   => Failed";
            string actual = output.ToString();
            Assert.That(Clean(actual), Is.StringStarting(Clean(expected)));
            Assert.That(actual, Is.StringContaining("Expected string length 1 but was 0"));
            Console.WriteLine(actual);
        }

        private object Student()
        {
            return null;
        }

        private static string Clean(string value)
        {
            return Regex.Replace(value, "\\s+", " ").Trim();
        }
    }
}