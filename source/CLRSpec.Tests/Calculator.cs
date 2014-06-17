using System;

namespace CLRSpec.Tests
{
    internal class Calculator
    {
        public bool PowerOn { get; private set; }

        public bool PowerOnIs(bool value)
        {
            return true;
        }

        public string Display { get; private set; }

        public Calculator()
        {
            Display = "";
        }

        public Element Button(int number)
        {
            return new Element();
        }

        public void Add(int a, int b)
        {
        }

        public Element Find(string type, string name)
        {
            return new Element();
        }

        public Element PlusButton
        {
            get { return new Element(); }
        }

        public Element EqualsButton
        {
            get
            {
                return new Element(() => Display = "11");
            }
        }
    }

    internal class Element
    {
        private readonly Action _action;

        public Element()
        {
        }

        public Element(Action action)
        {
            _action = action;
        }

        public bool IsPressed()
        {
            if(_action != null) _action();
            return true;
        }
    }
}