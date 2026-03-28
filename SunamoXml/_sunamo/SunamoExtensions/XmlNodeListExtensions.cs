namespace SunamoXml._sunamo.SunamoExtensions;

/// <summary>
/// Extension methods for <see cref="XmlNodeList"/> to provide LINQ-like functionality.
/// </summary>
internal static class XmlNodeListExtensions
{
    /// <summary>
    /// Determines whether the node list contains the specified node.
    /// </summary>
    /// <param name="nodeList">The node list to search.</param>
    /// <param name="node">The node to find.</param>
    internal static bool Contains(this XmlNodeList nodeList, XmlNode node)
    {
        foreach (var item in nodeList)
            if (item == node)
                return true;
        return false;
    }

    /// <summary>
    /// Returns the first node with the specified tag name.
    /// </summary>
    /// <param name="nodeList">The node list to search.</param>
    /// <param name="tagName">The tag name to match.</param>
    internal static XmlNode? First(this XmlNodeList nodeList, string tagName)
    {
        foreach (XmlNode item in nodeList)
            if (item.Name == tagName)
                return item;
        return null;
    }

    /// <summary>
    /// Returns all nodes with the specified tag name.
    /// </summary>
    /// <param name="nodeList">The node list to search.</param>
    /// <param name="tagName">The tag name to match.</param>
    internal static List<XmlNode> WithName(this XmlNodeList nodeList, string tagName)
    {
        var result = new List<XmlNode>();
        foreach (XmlNode item in nodeList)
            if (item.Name == tagName)
                result.Add(item);
        return result;
    }
}
