namespace SunamoXml._sunamo;

internal class SH
{
    internal static Func<string, string> ReplaceAllDoubleSpaceToSingle;
    internal static Func<string, string, string, string> ReplaceAll2;
    internal static Func<string, string, string, string> ReplaceOnce;
    internal static Func<string, char, (string, string)> GetPartsByLocationNoOut;
    internal static Func<string, string, SearchStrategy, bool> Contains;
    internal static Func<string, string, bool, bool, bool> ContainsBoolBool;
    internal static Func<string, string> ReplaceAllWhitecharsForSpace;
}
