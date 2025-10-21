// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoXml._sunamo.SunamoDictionary;
//namespace SunamoXml;

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