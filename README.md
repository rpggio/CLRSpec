CLRSpec
=======

Generate BDD-style spec descriptions from tests written in normal C# code.

### Example

This test:
```
[Test]
public void AddTwoNumbers()
{
  var calc = new Calculator();
  Spec.Run(s => s
      .AsA(Student())
      .Given(calc.SetPowerTo(true))
      .When(calc.Button(5).Push())
      .And(calc.PlusButton.Push())
      .And(calc.Button(6).Push())
      .And(calc.EqualsButton.Push())
      .Then(calc.Display.ShouldBe("11"))
      );
}
```
produces this output:
```
PassingSpec
    as a student                                           => Passed
        given I set power to True                          => Passed
        when I push button 5                               => Passed
        and push plus button                               => Passed
        and push button 6                                  => Passed
        and push equals button                             => Passed
        then display should be "11"                        => Passed
```

### What's the catch?
* No optional parameters allowed for methods inside the `Spec.Run` block (it's a lambda thing).
* Your helper methods, like `SetPowerTo` and `Push` above, must return a value in order to compile. (The value will be ignored.)
* Due to these constraints, this library seems most appropriate to UI testing, where you will already be creating your own custom model to access the UI.
* Only supports NUnit right now.

### You might also like
These other libraries can add BDD mojo to your .NET projects, at varying levels of eyeball strain.

(In no particular order)
* [StoryQ](http://storyq.codeplex.com/) - The bossy big brother to this project.
* [SpecFlow](http://www.specflow.org/) - Full cuke-itude
* [NSpec](http://nspec.org/) - ["it"] = () => is_so_simple_to("learn");
* [MSpec](https://github.com/machine/machine.specifications) - Because = () => it = great;
* [NBehave](https://github.com/nbehave/NBehave) - Why couldn't it start with 'Oh'?


