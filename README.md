CLRSpec
=======

Generate BDD-style spec descriptions from tests/stories/specs written in readable C# code. The entire test is specified in a single lambda expression in a fluent style. CLRSpec parses the expression tree and generates human-readable output from the code as it executes the test.
(Pronounced 'clear spec', if you like.)

### Example

This test:
```
[Test]
public void AddTwoNumbers()
{
  var calc = new Calculator();
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
}
```
produces this output:
```
AddTwoNumbers
    as a student                                           => Passed
        given calc power on is True                        => Passed
        and calc display should be empty                   => Passed
        when calc button 5 is pressed                      => Passed
        and calc plus button is pressed                    => Passed
        and calc button 6 is pressed                       => Passed
        and calc equals button is pressed                  => Passed
        then calc display should be "11"                   => Passed
```

### What's the catch?
* No optional parameters allowed for methods inside the `Spec.Run` block (it's a lambda thing).
* Your helper methods, like `PowerOnIs` and `Push` shown above, must return a value in order to compile. (The value will be ignored.)
* Due to these constraints, this library is best suited for testing against a purpose-built test model like you would use for UI testing. Once I get around to adding lambda support within the step definitions (`Given`, `And`, etc), then you could easily test against a wider variety of APIs.

### Fluent is fluent

This project is compatible with FluentAssertions, which render to English quite nicely. Currently we are using a [fork](https://github.com/ryascl/fluentassertions) of FluentAssertions due to the optional parameters issue. Hopefully our fixes will be pulled into the main project.

### You might also like

(In no particular order)
* [StoryQ](http://storyq.codeplex.com/) - Some similarities to this project. StoryQ has no lambda support, thus no rendering of chained invocations. It does have more output formatting control.
* [SpecFlow](http://www.specflow.org/)
* [NSpec](http://nspec.org/) 
* [MSpec](https://github.com/machine/machine.specifications)
* [NBehave](https://github.com/nbehave/NBehave) - Why couldn't it start with 'Oh'?

