using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace CLRSpec
{
    /// <summary>
    /// Base class for implementing a expression tree visitor. Read-only version of the example at:
    /// http://msdn.microsoft.com/en-us/library/bb882521(v=vs.90).aspx
    /// </summary>
    public abstract class ExpressionVisitor
    {
        protected virtual void Visit(Expression expr)
        {
            if (expr == null)
            {
                return;
            }

            switch (expr.NodeType)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.ArrayLength:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                    VisitUnary((UnaryExpression) expr);
                    return;
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.Coalesce:
                case ExpressionType.ArrayIndex:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                case ExpressionType.ExclusiveOr:
                    VisitBinary((BinaryExpression) expr);
                    return;
                case ExpressionType.TypeIs:
                    VisitTypeIs((TypeBinaryExpression) expr);
                    return;
                case ExpressionType.Conditional:
                    VisitConditional((ConditionalExpression) expr);
                    return;
                case ExpressionType.Constant:
                    VisitConstant((ConstantExpression) expr);
                    return;
                case ExpressionType.Parameter:
                    VisitParameter((ParameterExpression) expr);
                    return;
                case ExpressionType.MemberAccess:
                    VisitMemberAccess((MemberExpression) expr);
                    return;
                case ExpressionType.Call:
                    VisitMethodCall((MethodCallExpression) expr);
                    return;
                case ExpressionType.Lambda:
                    VisitLambda((LambdaExpression) expr);
                    return;
                case ExpressionType.New:
                    VisitNew((NewExpression) expr);
                    return;
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                    VisitNewArray((NewArrayExpression) expr);
                    return;
                case ExpressionType.Invoke:
                    VisitInvocation((InvocationExpression) expr);
                    return;
                case ExpressionType.MemberInit:
                    VisitMemberInit((MemberInitExpression) expr);
                    return;
                case ExpressionType.ListInit:
                    VisitListInit((ListInitExpression) expr);
                    return;
                default:
                    throw new Exception(string.Format("Unhandled expression type: '{0}'", expr.NodeType));
            }
        }

        protected virtual void VisitBinding(MemberBinding binding)
        {
            switch (binding.BindingType)
            {
                case MemberBindingType.Assignment:
                    VisitMemberAssignment((MemberAssignment) binding);
                    break;
                case MemberBindingType.MemberBinding:
                    VisitMemberMemberBinding((MemberMemberBinding) binding);
                    break;
                case MemberBindingType.ListBinding:
                    VisitMemberListBinding((MemberListBinding) binding);
                    break;
                default:
                    throw new Exception(string.Format("Unhandled binding type '{0}'", binding.BindingType));
            }
        }

        protected virtual void VisitElementInitializer(ElementInit init)
        {
            VisitExpressionList(init.Arguments);
        }

        protected virtual void VisitUnary(UnaryExpression expr)
        {
            Visit(expr.Operand);
        }

        protected virtual void VisitBinary(BinaryExpression expr)
        {
            Visit(expr.Left);
            Visit(expr.Right);
            Visit(expr.Conversion);
        }

        protected virtual void VisitTypeIs(TypeBinaryExpression expr)
        {
            Visit(expr.Expression);
        }

        protected virtual void VisitConstant(ConstantExpression expr)
        {
        }

        protected virtual void VisitConditional(ConditionalExpression expr)
        {
            Visit(expr.Test);
            Visit(expr.IfTrue);
            Visit(expr.IfFalse);
        }

        protected virtual void VisitParameter(ParameterExpression expr)
        {
        }

        protected virtual void VisitMemberAccess(MemberExpression expr)
        {
            Visit(expr.Expression);
        }

        protected virtual void VisitMethodCall(MethodCallExpression expr)
        {
            Visit(expr.Object);
            VisitExpressionList(expr.Arguments);
        }

        protected virtual void VisitExpressionList(ReadOnlyCollection<Expression> exprs)
        {
            foreach (var expr in exprs)
            {
                Visit(expr);
            }
        }

        protected virtual void VisitMemberAssignment(MemberAssignment assignment)
        {
            Visit(assignment.Expression);
        }

        protected virtual void VisitMemberMemberBinding(MemberMemberBinding binding)
        {
            VisitBindingList(binding.Bindings);
        }

        protected virtual void VisitMemberListBinding(MemberListBinding binding)
        {
            VisitElementInitializerList(binding.Initializers);
        }

        protected virtual void VisitBindingList(ReadOnlyCollection<MemberBinding> bindings)
        {
            foreach (var exprItem in bindings)
            {
                VisitBinding(exprItem);
            }
        }

        protected virtual void VisitElementInitializerList(ReadOnlyCollection<ElementInit> inits)
        {
            foreach (var init in inits)
            {
                VisitElementInitializer(init);
            }
        }

        protected virtual void VisitLambda(LambdaExpression expr)
        {
            Visit(expr.Body);
        }

        protected virtual void VisitNew(NewExpression expr)
        {
            VisitExpressionList(expr.Arguments);
        }

        protected virtual void VisitMemberInit(MemberInitExpression init)
        {
            VisitNew(init.NewExpression);
            VisitBindingList(init.Bindings);
        }

        protected virtual void VisitListInit(ListInitExpression init)
        {
            VisitNew(init.NewExpression);
            VisitElementInitializerList(init.Initializers);
        }

        protected virtual void VisitNewArray(NewArrayExpression expr)
        {
            VisitExpressionList(expr.Expressions);
        }

        protected virtual void VisitInvocation(InvocationExpression expr)
        {
            VisitExpressionList(expr.Arguments);
            Visit(expr.Expression);
        }
    }
}