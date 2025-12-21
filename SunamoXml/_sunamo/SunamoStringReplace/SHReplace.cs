namespace SunamoXml._sunamo.SunamoStringReplace;

internal class SHReplace
{
    internal static string ReplaceAllDoubleSpaceToSingle(string text, bool alsoHtml = false)
    {
        //text = SH.FromSpace160To32(text);

        if (alsoHtml)
        {
            text = text.Replace(" &nbsp;", " ");
            text = text.Replace("&nbsp; ", " ");
            text = text.Replace("&nbsp;", " ");
        }

        while (text.Contains("  "))
            text = text.Replace("  ", " "); // ReplaceAll2(text, "", );

        // Here it was cycling, dont know why, therefore without while
        //while (text.Contains("space160 + space"))
        //{
        //text = ReplaceAll2(text, "", "space160 + space");
        //}

        //while (text.Contains("space + space160"))
        //{
        //text = ReplaceAll2(text, "", "space + space160");
        //}

        return text;
    }


    internal static string ReplaceAllWhitecharsForSpace(string c)
    {
        WhitespaceCharService whitespaceChar = new();
        foreach (var item in whitespaceChar.whiteSpaceChars)
            if (item != ' ')
                c = c.Replace(item, ' ');

        return c;
    }
}