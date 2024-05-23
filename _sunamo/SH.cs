namespace SunamoXml;

//namespace SunamoXml;

public class SH
{
    //    public static Func<string, string> ReplaceAllDoubleSpaceToSingle;
    //    public static Func<string, string, string, string> ReplaceAll2;
    //    public static Func<string, string, string, string> ReplaceOnce;
    //    public static Func<string, char, (string, string)> GetPartsByLocationNoOut;
    //    public static Func<string, string, SearchStrategy, bool> Contains;
    //    public static Func<string, string, bool, bool, bool> ContainsBoolBool;
    //    public static Func<string, string> ReplaceAllWhitecharsForSpace;

    public static string ReplaceOnce(string input, string what, string zaco)
    {
        return new Regex(what).Replace(input, zaco, 1);
    }

    public static (string, string) GetPartsByLocationNoOut(string text, char or)
    {
        GetPartsByLocation(out var pred, out var za, text, or);
        return (pred, za);
    }

    public static void GetPartsByLocation(out string pred, out string za, string text, char or)
    {
        int dex = text.IndexOf(or);
        GetPartsByLocation(out pred, out za, text, dex);
    }

    public static void GetPartsByLocation(out string pred, out string za, string text, int pozice)
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
            {
                za = text.Substring(pozice + 1);
            }
            else
            {
                za = string.Empty;
            }
        }
    }
}
