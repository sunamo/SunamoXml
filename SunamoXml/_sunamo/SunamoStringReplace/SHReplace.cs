namespace SunamoXml._sunamo.SunamoStringReplace;

internal class SHReplace
{
    internal static string ReplaceAllDoubleSpaceToSingle(string text, bool isAlsoReplacingHtml = false)
    {
        if (isAlsoReplacingHtml)
        {
            text = text.Replace(" &nbsp;", " ");
            text = text.Replace("&nbsp; ", " ");
            text = text.Replace("&nbsp;", " ");
        }

        while (text.Contains("  "))
            text = text.Replace("  ", " ");

        return text;
    }

    internal static string ReplaceAllWhitecharsForSpace(string text)
    {
        WhitespaceCharService whitespaceCharService = new();
        foreach (var item in whitespaceCharService.WhiteSpaceChars)
            if (item != ' ')
                text = text.Replace(item, ' ');

        return text;
    }
}
