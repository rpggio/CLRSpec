using System.Text.RegularExpressions;

namespace CLRSpec
{
    internal static class TextHelper
    {
        internal static string Unpack(string input)
        {
            input = Regex.Replace(input, @"[\p{Lu}_]", x => " " + x.Value.ToLowerInvariant());
            input = Regex.Replace(input, "[_]", "");
            return input.Trim();
        }
    }
}