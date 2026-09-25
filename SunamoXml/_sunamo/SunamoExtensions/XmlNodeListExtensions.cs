namespace SunamoXml._sunamo.SunamoExtensions;

internal static class XmlNodeListExtensions
{
    internal static bool Contains(this XmlNodeList nodeList, XmlNode node)
    {
        foreach (var item in nodeList)
            if (item == node)
                return true;
        return false;
    }

    internal static XmlNode? First(this XmlNodeList nodeList, string tagName)
    {
        foreach (XmlNode item in nodeList)
            if (item.Name == tagName)
                return item;
        return null;
    }

    internal static List<XmlNode> WithName(this XmlNodeList nodeList, string tagName)
    {
        var result = new List<XmlNode>();
        foreach (XmlNode item in nodeList)
            if (item.Name == tagName)
                result.Add(item);
        return result;
    }
}
