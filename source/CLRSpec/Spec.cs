using System;
using System.IO;
using System.Linq.Expressions;

namespace CLRSpec
{
    public class Spec
    {
        public static TextWriter Output { get; set; }

        public static void Run(Expression<Action<Spec>> action)
        {
            var output = Output ?? Console.Out;
            output.WriteLine(ExpressionDescriber.Describe(action));
            var spec = new Spec();
            action.Compile()(spec);
        }

        public Spec Given(object x)
        {
            return this;
        }

        public Spec When(object x)
        {
            return this;
        }

        public Spec And(object x)
        {
            return this;
        }

        public Spec Then(object x)
        {
            return this;
        }

        public Spec Then(Action action)
        {
            return this;
        }
    }
}