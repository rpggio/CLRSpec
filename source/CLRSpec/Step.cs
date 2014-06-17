using System.Linq.Expressions;

namespace CLRSpec
{
    public class Step
    {
        public StepType StepType { get; set; }
        public Expression Expression { get; set; }

        public override string ToString()
        {
            string format = (StepType == StepType.Given || StepType == StepType.When)
                ? "{0} I {1}"
                : "{0} {1}";
            return string.Format(format, StepType, ExpressionDescriber.Describe(Expression));
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