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
