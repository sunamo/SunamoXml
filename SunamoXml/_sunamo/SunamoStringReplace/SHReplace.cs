namespace SunamoXml._sunamo.SunamoStringReplace;

/// <summary>
/// String replacement helper methods for whitespace and special character handling.
/// </summary>
internal class SHReplace
{
    /// <summary>
    /// Replaces all occurrences of double spaces with a single space. Optionally also replaces HTML non-breaking spaces.
    /// </summary>
    /// <param name="text">The text to process.</param>
    /// <param name="isAlsoReplacingHtml">Whether to also replace HTML non-breaking space entities.</param>
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

    /// <summary>
    /// Replaces all whitespace characters (tabs, newlines, etc.) with regular spaces.
    /// </summary>
    /// <param name="text">The text to process.</param>
    internal static string ReplaceAllWhitecharsForSpace(string text)
    {
        WhitespaceCharService whitespaceCharService = new();
        foreach (var item in whitespaceCharService.WhiteSpaceChars)
            if (item != ' ')
                text = text.Replace(item, ' ');

        return text;
    }
}
