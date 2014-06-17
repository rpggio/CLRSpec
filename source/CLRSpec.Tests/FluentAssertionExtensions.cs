//using System;
//using FluentAssertions;
//using FluentAssertions.Numeric;
//using FluentAssertions.Primitives;

//namespace CLRSpec.Tests
//{
//    public static class FluentAssertionExtensions
//    {
//        public static NumericAssertionsAdapter<int> Should(this int actualValue)
//        {
//            return new NumericAssertionsAdapter<int>(actualValue);
//        }

//        public static StringAssertionsAdapter Should(this string actualValue)
//        {
//            return new StringAssertionsAdapter(actualValue);
//        }
//    }

//    public class StringAssertionsAdapter : StringAssertions
//    {
        

//        public StringAssertionsAdapter(string value) : base(value)
//        {
//        }

//        public AndConstraintAdapter<StringAssertionsAdapter> Be(string expected)
//        {

//            new StringEqualityValidator(Subject, expected, StringComparison.CurrentCulture, because, reasonArgs).Validate();

//            // not correct
//            return new AndConstraintAdapter<StringAssertionsAdapter>(this);
//        }
//    }

//    public class AndConstraintAdapter<T> : AndConstraint<T>
//    {
//        public AndConstraintAdapter(T parentConstraint) : base(parentConstraint)
//        {
//        }
//    }

//    public class NumericAssertionsAdapter<T> : NumericAssertions<T> where T : struct
//    {
//        public NumericAssertionsAdapter(object value) : base(value)
//        {
//        }

//        public AndConstraint<NumericAssertions<T>> Be(T expected)
//        {
//            return base.Be(expected);
//        }
//    }
//}