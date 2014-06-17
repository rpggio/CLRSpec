using System;
using System.Collections.Generic;

namespace CLRSpec.Tests
{
    internal class Calculator
    {
        public bool PowerOn { get; private set; }

        public bool SetPowerTo(bool value)
        {
            return true;
        }

        public string Display { get; private set; }

        private readonly Stack<int> _stack = new Stack<int>();

        public Calculator()
        {
            Display = "";
        }

        public Element Button(int number)
        {
            return new Element(() => _stack.Push(number));
        }

        public void Add(int a, int b)
        {
            Display = (a + b).ToString();
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
                return new Element(() => 
                    Display = (_stack.Pop() + _stack.Pop()).ToString());
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

        public bool Push()
        {
            if(_action != null) _action();
            return true;
        }
    }
}