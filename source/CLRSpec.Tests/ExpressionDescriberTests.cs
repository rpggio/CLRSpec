using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using NUnit.Framework;

namespace CLRSpec.Tests
{
    [TestFixture]
    public class ExpressionDescriberTests
    {
        readonly Calculator _calc = new Calculator();

        [Test]
        public void DescribesProperty()
        {
            AssertDescription(
                () => _calc.PlusButton,
                "calc plus button");
        }

        [Test]
        public void DescribesMethodCall()
        {
             AssertDescription(
                () => _calc.Button(5),
                "calc button 5");
        }

        [Test]
        public void DescribesMethodCallWithParamNames()
        {
            Assert.That(
                ExpressionDescriber.Describe(() => _calc.Find("function", "POW"), 
                    new ExpressionDescriberOptions(){IncludeArgumentNames = true}),
                Is.EqualTo("calc find type \"function\" and name \"POW\""));
        }

        [Test]
        public void DescribesMethodCallWithMultipleParams()
        {
            AssertDescription(
                () => _calc.Add(5, 6),
                "calc add 5 and 6");
        }

        [Test]
        public void DescribesChainedMethodCall()
        {
            AssertDescription(
                () => _calc.Button(5).IsPressed(),
                "calc button 5 is pressed");
        }

        [Test]
        public void DescribesFluentAssertions()
        {
            int x = 0;
            AssertDescription(
                () => x.Should().Be(5),
                "x should be 5");
            AssertDescription(
                () => x.Should().BeInRange(4, 6),
                "x should be in range 4 and 6");
        }

        [Test]
        public void DescribesExtensionMethod()
        {
            IEnumerable<string> names= new string[0];
            AssertDescription(
                () => names.Count().Should().Be(5),
                "names count should be 5"
                );
        }

        private void AssertDescription(Expression<Action> expr, string expected)
        {
            AssertDescription((Expression)expr, expected);
        }

        private void AssertDescription<T>(Expression<Func<T>> expr, string expected)
        {
            AssertDescription((Expression)expr, expected);
        }

        private void AssertDescription(Expression expr, string expected)
        {
            try
            {
                var result = ExpressionDescriber.Describe(expr);
                Assert.AreEqual(expected, result);
            }
            catch
            {
                new ExpressionDiagnosticWriter(Console.Out).Write(expr);
                Console.WriteLine();
                throw;
            }
        }
    }
}
