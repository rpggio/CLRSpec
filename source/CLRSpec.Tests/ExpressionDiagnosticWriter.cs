using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using NUnit.Framework;

namespace CLRSpec.Tests
{
    public class ExpressionDiagnosticWriter : ExpressionVisitor
    {
        private readonly IndentedTextWriter _writer;

        public ExpressionDiagnosticWriter(TextWriter writer)
        {
            _writer = new IndentedTextWriter(writer);
        }

        public void Write<T>(Expression<Func<T>> expr)
        {
            Write((Expression)expr);
        }

        public void Write<T>(Expression<Action<T>> expr)
        {
            Write((Expression)expr);
        }

        public void Write(Expression expr)
        {
            _writer.Indent = 0;
            Visit(expr);
        }

        //protected override bool Visit(Expression expr)
        //{
        //    if (expr == null)
        //    {
        //        _writer.WriteLine("[null]");
        //        return;
        //    }
        //    _writer.WriteLine("({0}) {1} {2}", expr.NodeType, expr.Type.Name, expr);
        //    _writer.Indent++;
        //    base.Visit(expr);
        //    _writer.Indent--;
        //}

        protected override bool Visit(Expression expr)
        {
            if (expr == null)
            {
                _writer.WriteLine("[null]");
                return true;
            }

            if (!base.Visit(expr))
            {
                throw new Exception(string.Format("Expression ({0}) {1} was not handled", expr.NodeType, expr));
            }

            return true;
        }

        private void WriteExpr(Expression expr, string description = null, Action baseVisit = null)
        {
            _writer.WriteLine("({0}) {1} {2}", expr.NodeType, expr.Type.Name, description ?? expr.ToString());
            _writer.Indent++;
            if (baseVisit != null)
            {
                baseVisit();
            }
            _writer.Indent--;
        }

        protected override bool VisitConstant(ConstantExpression expr)
        {
            WriteExpr(expr, expr.Value.ToString(), () => base.VisitConstant(expr));
            return true;
        }

        protected override bool VisitMemberAccess(MemberExpression expr)
        {
            WriteExpr(expr, expr.Member.Name, () => base.VisitMemberAccess(expr));
            return true;
        }

        protected override bool VisitMethodCall(MethodCallExpression expr)
        {
            WriteExpr(expr, expr.Method.Name, () => base.VisitMethodCall(expr));
            return true;
        }

        protected override bool VisitLambda(LambdaExpression expr)
        {
            WriteExpr(expr, expr.Name, () => base.VisitLambda(expr));
            return true;
        }

        protected override bool VisitInvocation(InvocationExpression expr)
        {
            WriteExpr(expr, 
                string.Format("invoke([{0}])", expr.Arguments.Count), 
                () => base.VisitInvocation(expr));
            return true;
        }

        protected override bool VisitUnary(UnaryExpression expr)
        {
            WriteExpr(expr, expr.Operand.ToString(), () => base.VisitUnary(expr));
            return true;
        }
    }
}
