namespace SunamoXml._sunamo.SunamoString;

/// <summary>
/// String helper methods for basic string manipulation operations.
/// </summary>
internal class SH
{
    /// <summary>
    /// Replaces only the first occurrence of a pattern in the input string.
    /// </summary>
    /// <param name="input">The input string to search in.</param>
    /// <param name="what">The pattern to find.</param>
    /// <param name="replacement">The replacement string.</param>
    internal static string ReplaceOnce(string input, string what, string replacement)
    {
        return new Regex(what).Replace(input, replacement, 1);
    }

    /// <summary>
    /// Splits a string at the first occurrence of the separator and returns both parts as a tuple.
    /// </summary>
    /// <param name="text">The string to split.</param>
    /// <param name="separator">The character to split at.</param>
    internal static (string, string) GetPartsByLocationNoOut(string text, char separator)
    {
        GetPartsByLocation(out var before, out var after, text, separator);
        return (before, after);
    }

    /// <summary>
    /// Splits a string at the first occurrence of the separator character.
    /// </summary>
    /// <param name="before">The substring before the separator.</param>
    /// <param name="after">The substring after the separator.</param>
    /// <param name="text">The string to split.</param>
    /// <param name="separator">The character to split at.</param>
    internal static void GetPartsByLocation(out string before, out string after, string text, char separator)
    {
        var index = text.IndexOf(separator);
        GetPartsByLocation(out before, out after, text, index);
    }

    /// <summary>
    /// Splits a string at the specified position index.
    /// </summary>
    /// <param name="before">The substring before the position.</param>
    /// <param name="after">The substring after the position.</param>
    /// <param name="text">The string to split.</param>
    /// <param name="position">The index at which to split. If -1, returns the entire string as before.</param>
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
