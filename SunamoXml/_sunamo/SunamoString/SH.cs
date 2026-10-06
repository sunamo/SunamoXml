namespace SunamoXml._sunamo.SunamoString;

internal class SH
{
    internal static string ReplaceOnce(string input, string what, string replacement)
    {
        return new Regex(what).Replace(input, replacement, 1);
    }

    internal static (string, string) GetPartsByLocationNoOut(string text, char separator)
    {
        GetPartsByLocation(out var before, out var after, text, separator);
        return (before, after);
    }

    internal static void GetPartsByLocation(out string before, out string after, string text, char separator)
    {
        var index = text.IndexOf(separator);
        GetPartsByLocation(out before, out after, text, index);
    }

    internal static void GetPartsByLocation(out string before, out string after, string text, int position)
    {
        if (position == -1)
        {
            before = text;
            after = "";
        }
        else
        {
            before = text.Substring(0, position);
            if (text.Length > position + 1)
                after = text.Substring(position + 1);
            else
                after = string.Empty;
        }
    }
}
