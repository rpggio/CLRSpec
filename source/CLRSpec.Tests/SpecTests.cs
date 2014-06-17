using NUnit.Framework;

namespace CLRSpec.Tests
{
    [TestFixture]
    public class SpecTests
    {
        [Test]
        [Ignore("in progress")]
        public void Given()
        {
            var calc = new Calculator();
            Spec.Run(s =>
                s.Given(calc.Display.ShouldBe(""))
                 .When(calc.Button(5).Push())
                 .And(calc.Button(6).Push())
                 .And(calc.EqualsButton.Push())
                 .Then(calc.Display.ShouldBe("11"))
                );
        }

    }
}
