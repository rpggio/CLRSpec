using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CLRSpec
{
    public class SpecExpressionParser : ExpressionVisitor
    {
        private readonly List<Step> _steps = new List<Step>(); 

        public List<Step> Parse(Expression expr)
        {
            _steps.Clear();
            Visit(expr);
            _steps.Reverse();
            return _steps;
        }

        protected override void VisitMethodCall(MethodCallExpression expr)
        {
            if (expr.Method.DeclaringType == typeof (Spec) && !expr.Method.IsStatic)
            {
                _steps.Add(new Step()
                {
                    StepType = (StepType)Enum.Parse(typeof(StepType), expr.Method.Name),
                    Expression = expr.Arguments[0]
                });
            }

            base.VisitMethodCall(expr);
        }
    }
}