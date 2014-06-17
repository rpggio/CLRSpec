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
            var parser = new SpecExpressionParser();
            var steps = parser.Parse(action);
            foreach (var step in steps)
            {
                output.Write("{0,-50} => ", step);
                try
                {
                    var stepAction = Expression.Lambda<Action>(step.Expression).Compile();
                    stepAction();
                    output.Write("Passed");
                }
                catch
                {
                    output.Write("Failed");
                    throw;
                }
                output.WriteLine();
            }
        }

        private string _actor;

        public Spec AsA(string actor)
        {
            _actor = actor;
            return this;
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