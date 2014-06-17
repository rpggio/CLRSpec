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
                Is.EqualTo("plus button"));
        }

        [Test]
        public void DescribesMethodCall()
        {
            Assert.That(
                ExpressionDescriber.Describe(() => _calc.Button(5)),
                Is.EqualTo("button 5"));
        }

        [Test]
        public void DescribesMethodCallWithParamNames()
        {
            Assert.That(
                ExpressionDescriber.Describe(() => _calc.Find("function", "POW"), 
                    new ExpressionDescriberOptions(){IncludeArgumentNames = true}),
                Is.EqualTo("find type \"function\" and name \"POW\""));
        }

        [Test]
        public void DescribesMethodCallWithMultipleParams()
        {
            Assert.That(
                ExpressionDescriber.Describe(() => _calc.Add(5, 6)),
                Is.EqualTo("add 5 and 6"));
        }

        [Test]
        public void DescribesChainedMethodCall()
        {
            Assert.That(
                ExpressionDescriber.Describe(() => _calc.Button(5).Push()),
                Is.EqualTo("push button 5"));
        }

        [Test]
        public void DescribesExtensionMethod()
        {
            Assert.That(
                ExpressionDescriber.Describe(() => _calc.Display.ShouldBe("11")),
                Is.EqualTo("display should be \"11\""));
        }
    }
}
