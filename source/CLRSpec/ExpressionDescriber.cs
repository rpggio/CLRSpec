using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CLRSpec
{
    /// <summary>
    /// Provides a readable description from an expression tree.
    /// </summary>
    public class ExpressionDescriber : ExpressionVisitor
    {
        private bool _prependNextWord;
        private readonly List<string> _words = new List<string>();

        public bool IncludeArgumentNames { get; set; }

        /// <summary>
        /// Overridable rule for determining whether a method reads in reverse order.
        /// </summary>
        public Func<MethodInfo, bool> IsReverseOrderMethod =
            m => m.Name.StartsWith("should", StringComparison.OrdinalIgnoreCase)
            || m.Name.StartsWith("be", StringComparison.OrdinalIgnoreCase);

        public static string Describe(Expression<Action> expr, ExpressionDescriberOptions options = null)
        {
            return Describe((Expression)expr, options);
        }

        public static string Describe<T>(Expression<Func<T>> expr, ExpressionDescriberOptions options = null)
        {
            return Describe((Expression)expr, options);
        }

        public static string Describe(Expression expr, ExpressionDescriberOptions options = null)
        {
            var describer = new ExpressionDescriber();
            if (options != null)
            {
                describer.IncludeArgumentNames = options.IncludeArgumentNames;
            }
            return describer.DescribeExpression(expr);
        }

        public string DescribeExpression(Expression expr)
        {
            base.Visit(expr);
            return string.Join(" ", _words.ToArray());
        }

        protected override void Visit(Expression expr)
        {
            if (expr != null && Debugger.IsAttached)
            {
                Debug.WriteLine("Visiting ({0}){1}", expr.NodeType, expr);
            }
            base.Visit(expr);
        }

        protected override void VisitMemberAccess(MemberExpression expr)
        {
            switch (expr.Member.MemberType)
            {
                case MemberTypes.Property:
                case MemberTypes.Field:
                    ApplyWord(TextHelper.Unpack(expr.Member.Name), true);
                    break;
            }
            base.VisitMemberAccess(expr);
        }
        
        protected override void VisitMethodCall(MethodCallExpression expr)
        {
            ParameterInfo[] parameters = expr.Method.GetParameters();
            ReadOnlyCollection<Expression> arguments = expr.Arguments;

            var paramAndArgs = from i in Enumerable.Range(0, arguments.Count)
                let p = parameters[i]
                let a = arguments[i]
                select IncludeArgumentNames 
                    ? string.Format("{0} {1}", p.Name, a)
                    : a.ToString();
            if (expr.Method.GetCustomAttributes(typeof (ExtensionAttribute), false).Any())
            {
                paramAndArgs = paramAndArgs.Skip(1);
            }
            string argDescription = string.Join(" and ", paramAndArgs.ToArray());
            string result = TextHelper.Unpack(expr.Method.Name);
            result = argDescription.Length == 0
                ? result
                : string.Format("{0} {1}", result, argDescription);

            ApplyWord(result, true);
            if (IsReverseOrderMethod(expr.Method))
            {
                _prependNextWord = true;
            }

            base.VisitMethodCall(expr);
        }

        private void ApplyWord(string text, bool prepend = false)
        {
            if (prepend || _prependNextWord)
            {
                _words.Insert(0, text);
            }
            else
            {
                _words.Add(text);
            }
            _prependNextWord = false;

            if (Debugger.IsAttached)
            {
                Debug.WriteLine("description: {0}", (object)string.Join(" ", _words));
            }
        }
    }

    public class ExpressionDescriberOptions
    {
        public bool IncludeArgumentNames { get; set; }
    }
}