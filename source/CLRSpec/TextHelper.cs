using System.Text.RegularExpressions;

namespace CLRSpec
{
    internal static class TextHelper
    {
        internal static string Unpack(string input)
        {
            return Regex.Replace(input, @"[\p{Lu}_]", x => " " + x.Value.ToLowerInvariant()).Trim();
        }
    }
}