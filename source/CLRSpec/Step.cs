using System.Linq.Expressions;

namespace CLRSpec
{
    /// <summary>
    /// Represents a single step of the spec (given, when, etc).
    /// </summary>
    public class Step
    {
        public StepType StepType { get; set; }
        public Expression Expression { get; set; }

        public override string ToString()
        {
            string format = (StepType == StepType.Given || StepType == StepType.When)
                ? "{0} I {1}"
                : "{0} {1}";
            return string.Format(format, 
                TextHelper.Unpack(StepType.ToString()), 
                ExpressionDescriber.Describe(Expression));
        }
    }

    public enum StepType
    {
        AsA,
        Given,
        And,
        When,
        Then
    }
}