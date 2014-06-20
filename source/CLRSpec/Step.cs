using System.Linq;
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
            string description;
            try
            {
                description = ExpressionDescriber.Describe(Expression);
            }
            catch
            {
                description = new string(Expression.ToString().Take(40).ToArray());
            }
            return string.Format("{0} {1}", 
                TextHelper.Unpack(StepType.ToString()), 
                description);
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