namespace SunamoXml._sunamo.SunamoString;

internal class SH
{
    //    internal static Func<string, string> ReplaceAllDoubleSpaceToSingle;
    //    internal static Func<string, string, string, string> ReplaceAll2;
    //    internal static Func<string, string, string, string> ReplaceOnce;
    //    internal static Func<string, char, (string, string)> GetPartsByLocationNoOut;
    //    internal static Func<string, string, SearchStrategy, bool> Contains;
    //    internal static Func<string, string, bool, bool, bool> ContainsBoolBool;
    //    internal static Func<string, string> ReplaceAllWhitecharsForSpace;

    internal static string ReplaceOnce(string input, string what, string zaco)
    {
        return new Regex(what).Replace(input, zaco, 1);
    }

    internal static (string, string) GetPartsByLocationNoOut(string text, char or)
    {
        GetPartsByLocation(out var pred, out var za, text, or);
        return (pred, za);
    }

    internal static void GetPartsByLocation(out string pred, out string za, string text, char or)
    {
        var dex = text.IndexOf(or);
        GetPartsByLocation(out pred, out za, text, dex);
    }

    internal static void GetPartsByLocation(out string pred, out string za, string text, int pozice)
    {
        if (pozice == -1)
        {
            pred = text;
            za = "";
        }
        else
        {
            pred = text.Substring(0, pozice);
            if (text.Length > pozice + 1)
                za = text.Substring(pozice + 1);
            else
                za = string.Empty;
        }
    }






}