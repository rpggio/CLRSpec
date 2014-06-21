using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace CLRSpec
{
    /// <summary>
    ///     Base class for implementing a expression tree visitor. Read-only version of the example at:
    ///     http://msdn.microsoft.com/en-us/library/bb882521(v=vs.90).aspx
    /// </summary>
    public abstract class ExpressionVisitor
    {
        /// <summary>
        /// Visit an expression tree, calling out to the type-specific override based on node type.
        /// </summary>
        /// <param name="expr"></param>
        /// <returns>True if method was handled by an override.</returns>
        protected virtual bool Visit(Expression expr)
        {
            if (expr == null)
            {
                return false;
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
                    return VisitUnary((UnaryExpression) expr);
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
                    return VisitBinary((BinaryExpression) expr);
                case ExpressionType.TypeIs:
                    return VisitTypeIs((TypeBinaryExpression) expr);
                case ExpressionType.Conditional:
                    return VisitConditional((ConditionalExpression) expr);
                case ExpressionType.Constant:
                    return VisitConstant((ConstantExpression) expr);
                case ExpressionType.Parameter:
                    return VisitParameter((ParameterExpression) expr);
                case ExpressionType.MemberAccess:
                    return VisitMemberAccess((MemberExpression) expr);
                case ExpressionType.Call:
                    return VisitMethodCall((MethodCallExpression) expr);
                case ExpressionType.Lambda:
                    return VisitLambda((LambdaExpression) expr);
                case ExpressionType.New:
                    return VisitNew((NewExpression) expr);
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                    return VisitNewArray((NewArrayExpression) expr);
                case ExpressionType.Invoke:
                    return VisitInvocation((InvocationExpression) expr);
                case ExpressionType.MemberInit:
                    return VisitMemberInit((MemberInitExpression) expr);
                case ExpressionType.ListInit:
                    return VisitListInit((ListInitExpression) expr);
                default:
                    throw new Exception(string.Format("Unhandled expression type: '{0}'", expr.NodeType));
            }
        }

        protected virtual bool VisitBinding(MemberBinding binding)
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
            return false;
        }

        protected virtual bool VisitElementInitializer(ElementInit init)
        {
            VisitExpressionList(init.Arguments);
            return false;
        }

        protected virtual bool VisitUnary(UnaryExpression expr)
        {
            Visit(expr.Operand);
            return false;
        }

        protected virtual bool VisitBinary(BinaryExpression expr)
        {
            Visit(expr.Left);
            Visit(expr.Right);
            Visit(expr.Conversion);
            return false;
        }

        protected virtual bool VisitTypeIs(TypeBinaryExpression expr)
        {
            Visit(expr.Expression);
            return false;
        }

        protected virtual bool VisitConstant(ConstantExpression expr)
        {
            return false;
        }

        protected virtual bool VisitConditional(ConditionalExpression expr)
        {
            Visit(expr.Test);
            Visit(expr.IfTrue);
            Visit(expr.IfFalse);
            return false;
        }

        protected virtual bool VisitParameter(ParameterExpression expr)
        {
            return false;
        }

        protected virtual bool VisitMemberAccess(MemberExpression expr)
        {
            Visit(expr.Expression);
            return false;
        }

        protected virtual bool VisitMethodCall(MethodCallExpression expr)
        {
            Visit(expr.Object);
            VisitExpressionList(expr.Arguments);
            return false;
        }

        protected virtual bool VisitExpressionList(ReadOnlyCollection<Expression> exprs)
        {
            foreach (var expr in exprs)
            {
                Visit(expr);
            }
            return false;
        }

        protected virtual bool VisitMemberAssignment(MemberAssignment assignment)
        {
            Visit(assignment.Expression);
            return false;
        }

        protected virtual bool VisitMemberMemberBinding(MemberMemberBinding binding)
        {
            VisitBindingList(binding.Bindings);
            return false;
        }

        protected virtual bool VisitMemberListBinding(MemberListBinding binding)
        {
            VisitElementInitializerList(binding.Initializers);
            return false;
        }

        protected virtual bool VisitBindingList(ReadOnlyCollection<MemberBinding> bindings)
        {
            foreach (var exprItem in bindings)
            {
                VisitBinding(exprItem);
            }
            return false;
        }

        protected virtual bool VisitElementInitializerList(ReadOnlyCollection<ElementInit> inits)
        {
            foreach (var init in inits)
            {
                VisitElementInitializer(init);
            }
            return false;
        }

        protected virtual bool VisitLambda(LambdaExpression expr)
        {
            Visit(expr.Body);
            return false;
        }

        protected virtual bool VisitNew(NewExpression expr)
        {
            VisitExpressionList(expr.Arguments);
            return false;
        }

        protected virtual bool VisitMemberInit(MemberInitExpression init)
        {
            VisitNew(init.NewExpression);
            VisitBindingList(init.Bindings);
            return false;
        }

        protected virtual bool VisitListInit(ListInitExpression init)
        {
            VisitNew(init.NewExpression);
            VisitElementInitializerList(init.Initializers);
            return false;
        }

        protected virtual bool VisitNewArray(NewArrayExpression expr)
        {
            VisitExpressionList(expr.Expressions);
            return false;
        }

        protected virtual bool VisitInvocation(InvocationExpression expr)
        {
            VisitExpressionList(expr.Arguments);
            Visit(expr.Expression);
            return false;
        }
    }
}