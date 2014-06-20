using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            Assert.That(
                ExpressionDescriber.Describe(() => _calc.PlusButton),
                Is.EqualTo("calc plus button"));
        }

        [Test]
        public void DescribesMethodCall()
        {
            Assert.That(
                ExpressionDescriber.Describe(() => _calc.Button(5)),
                Is.EqualTo("calc button 5"));
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
            Assert.That(
                ExpressionDescriber.Describe(() => _calc.Add(5, 6)),
                Is.EqualTo("calc add 5 and 6"));
        }

        [Test]
        public void DescribesChainedMethodCall()
        {
            Assert.That(
                ExpressionDescriber.Describe(() => _calc.Button(5).IsPressed()),
                Is.EqualTo("calc button 5 is pressed"));
        }

        [Test]
        public void DescribesFluentAssertions()
        {
            int x = 0;
            Assert.That(
                ExpressionDescriber.Describe(() => x.Should().Be(5)),
                Is.EqualTo("x should be 5"));
            Assert.That(
                ExpressionDescriber.Describe(() => x.Should().BeInRange(4, 6)),
                Is.EqualTo("x should be in range 4 and 6"));
        }

        [Test]
        public void DescribesExtensionMethod()
        {
            IEnumerable<string> names= new string[0];
            Assert.That(
                ExpressionDescriber.Describe(() => names.Count().Should().Be(5)),
                Is.EqualTo("names count should be 5"));
        }
    }
}
