namespace SunamoXml._sunamo.SunamoDictionary;

/// <summary>
/// Helper methods for converting dictionaries to other collection types.
/// </summary>
internal class DictionaryHelper
{
    /// <summary>
    /// Converts a string dictionary to a flat list of alternating keys and values.
    /// </summary>
    /// <param name="dictionary">The dictionary to convert.</param>
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
