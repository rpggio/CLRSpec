using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CLRSpec
{
    /// <summary>
    /// Parses spec expression tree into steps.
    /// </summary>
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

        protected override bool VisitMethodCall(MethodCallExpression expr)
        {
            if (expr.Method.DeclaringType == typeof (Spec) && !expr.Method.IsStatic)
            {
                var child = expr.Arguments[0];
                if (child.NodeType == ExpressionType.Quote)
                {
                    child = ((UnaryExpression) child).Operand;
                }
                _steps.Add(new Step()
                {
                    StepType = (StepType)Enum.Parse(typeof(StepType), expr.Method.Name),
                    Expression = child
                });
            }

            base.VisitMethodCall(expr);
            return true;
        }
    }
}