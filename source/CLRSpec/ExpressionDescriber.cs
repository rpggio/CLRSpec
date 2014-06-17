using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace CLRSpec
{
    public class ExpressionDescriber : ExpressionVisitor
    {
        private bool _prependNextWord;
        private readonly List<string> _words = new List<string>();

        public bool IncludeArgumentNames { get; set; }

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

        protected override void VisitMemberAccess(MemberExpression expr)
        {
            switch (expr.Member.MemberType)
            {
                case MemberTypes.Property:
                    ApplyWord(Unpack(expr.Member.Name));
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
            string result = Unpack(expr.Method.Name);
            result = argDescription.Length == 0
                ? result
                : string.Format("{0} {1}", result, argDescription);

            ApplyWord(result);
            if (MethodIsAssertion(expr.Method))
            {
                _prependNextWord = true;
            }

            base.VisitMethodCall(expr);
        }

        private bool MethodIsAssertion(MethodInfo method)
        {
            var name = method.Name;
            return name.StartsWith("should", StringComparison.OrdinalIgnoreCase);
        }

        private void ApplyWord(string text)
        {
            if (_prependNextWord)
            {
                _prependNextWord = false;
                _words.Insert(0, text);
            }
            else
            {
                _words.Add(text);
            }
        }

        private static string Unpack(string input)
        {
            return Regex.Replace(input, @"[\p{Lu}_]", x => " " + x.Value.ToLowerInvariant()).Trim();
        }
    }

    public class ExpressionDescriberOptions
    {
        public bool IncludeArgumentNames { get; set; }
    }
}