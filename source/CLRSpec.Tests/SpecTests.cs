using System;
using System.IO;
using System.Text.RegularExpressions;
using FluentAssertions;
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
                .Given(calc.PowerOnIs(true))
                .And(calc.Display.Should().BeEmpty())
                .When(calc.Button(5).IsPressed())
                .And(calc.PlusButton.IsPressed())
                .And(calc.Button(6).IsPressed())
                .And(calc.EqualsButton.IsPressed())
                .Then(calc.Display.Should().Be("11"))
                );
            const string expected = @"PassingSpec
                as a student                        => Passed
                given calc power on is True         => Passed
                and calc display should be empty    => Passed
                when calc button 5 is pressed       => Passed
                and calc plus button is pressed     => Passed
                and calc button 6 is pressed        => Passed
                and calc equals button is pressed   => Passed
                then calc display should be ""11""  => Passed";
            string actual = output.ToString();
            Assert.That(Clean(actual), Is.EqualTo(Clean(expected)));
            Console.WriteLine(actual);
        }

        [Test]
        public void FailingSpec()
        {
            Console.WriteLine();
            var calc = new Calculator();
            var output = new StringWriter();
            Spec.Output = output;
            Assert.Throws<AssertionException>(() =>
                Spec.Run(s => s
                    .Given(calc.PowerOnIs(true))
                    .Then(calc.Display.Should().Be("1"))
                    )
                );
            const string expected = @"
                given calc power on is True     => Passed
                then calc display should be ""1""    => Failed";
            string actual = output.ToString();
            // upcase to make it easier to troubleshoot
            Assert.That(Clean(actual.ToUpper()), Is.StringContaining(Clean(expected.ToUpper())));
            Assert.That(actual.ToUpper(), Is.StringContaining("Expected string".ToUpper()));
        }

        [Test]
        public void CanDescribeLambdas()
        {
            var calc = new Calculator();
            var output = new StringWriter();
            Spec.Output = output;
            Spec.Run(s => s
                .Given(() => calc.Clear())
                );
            const string expected = @"CanDescribeLambdas
                given calc clear                    => Passed";
            string actual = output.ToString();
            Assert.That(Clean(actual), Is.EqualTo(Clean(expected)));
            Console.WriteLine(actual);
        }

        [Test]
        public void ExecutesLambdas()
        {
            var output = new StringWriter();
            Spec.Output = output;
            Assert.Throws<Exception>(() =>
                Spec.Run(s => s
                    .Given(() => Boom())
                    )
            );
            const string expected = @"given boom                    => Failed";
            string actual = output.ToString();
            // upcase to make it easier to troubleshoot
            Assert.That(Clean(actual.ToUpper()), Is.StringContaining(Clean(expected.ToUpper())));
            Assert.That(actual.ToUpper(), Is.StringContaining("boom happened".ToUpper()));
        }

        private object Student()
        {
            return null;
        }

        private void Boom()
        {
            throw new Exception("boom happened");
        }

        private static string Clean(string value)
        {
            return Regex.Replace(value, "\\s+", " ").Trim();
        }
    }
}