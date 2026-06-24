namespace SunamoXml._sunamo.SunamoDictionary;

internal class DictionaryHelper
{
    internal static List<string> GetListStringFromDictionary(Dictionary<string, string> dictionary)
    {
        var result = new List<string>();

        foreach (var item in dictionary)
        {
            result.Add(item.Key);
            result.Add(item.Value);
        }

        return result;
    }
}
