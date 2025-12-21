namespace SunamoXml._sunamo.SunamoDictionary;

internal class DictionaryHelper
{
    internal static List<string> GetListStringFromDictionary(Dictionary<string, string> p)
    {
        var vr = new List<string>();

        foreach (var item in p)
        {
            vr.Add(item.Key);
            vr.Add(item.Value);
        }

        return vr;
    }
}