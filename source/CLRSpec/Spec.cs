using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;

namespace CLRSpec
{
    public class Spec
    {
        public static TextWriter Output { get; set; }

        public static void Run(Expression<Action<Spec>> action)
        {
            var output = new IndentedTextWriter(Output ?? Console.Out);

            try
            {
                var callingMethod = new StackFrame(1).GetMethod().Name;
                output.WriteLine(callingMethod);
                output.Indent++;
            }
            catch{}

            var parser = new SpecExpressionParser();
            var steps = parser.Parse(action);
            foreach (var step in steps)
            {
                if (step.Expression == null)
                {
                    output.Write(step);
                }
                else
                {
                    output.Write("{0,-50} => ", step);
                    try
                    {
                        Expression<Action> stepAction = step.Expression.NodeType == ExpressionType.Lambda
                            ? (Expression<Action>) step.Expression
                            : Expression.Lambda<Action>(step.Expression);
                        var compiledAction = stepAction.Compile();
                        compiledAction();
                        output.Write("Passed");
                    }
                    catch(Exception ex)
                    {
                        output.Write("Failed");
                        output.WriteLine();
                        output.WriteLine();
                        output.WriteLine(ex.Message);
                    }
                    output.WriteLine();
                }

                if (step.StepType == StepType.AsA)
                {
                    output.Indent++;
                }
            }
        }

        public Spec AsA(object ignored)
        {
            return this;
        }

        public Spec AsA(Expression<Action> action)
        {
            return this;
        }

        public Spec Given(object ignored)
        {
            return this;
        }

        public Spec Given(Expression<Action> action)
        {
            return this;
        }

        public Spec When(object ignored)
        {
            return this;
        }

        public Spec When(Expression<Action> action)
        {
            return this;
        }

        public Spec And(object ignored)
        {
            return this;
        }

        public Spec And(Expression<Action> action)
        {
            return this;
        }

        public Spec Then(object ignored)
        {
            return this;
        }

        public Spec Then(Expression<Action> action)
        {
            return this;
        }
    }
}