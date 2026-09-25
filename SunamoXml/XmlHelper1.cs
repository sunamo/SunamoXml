namespace SunamoXml;

/// <summary>
/// Additional XML helper methods for attribute access, element search, and attribute manipulation on <see cref="XmlNode"/>.
/// </summary>
public static partial class XmlHelper
{
    /// <summary>
    /// Returns all child elements with the specified tag name.
    /// </summary>
    /// <param name="node">The parent XML node.</param>
    /// <param name="tagName">The tag name to match.</param>
    public static IList<XmlNode> GetElementsOfName(XmlNode node, string tagName)
    {
        return node.ChildNodes.WithName(tagName);
    }

    /// <summary>
    /// Returns the value of the attribute with the specified name, or null if not found.
    /// </summary>
    /// <param name="node">The XML node to search.</param>
    /// <param name="attributeName">The attribute name to find.</param>
    public static string? Attr(XmlNode node, string attributeName)
    {
        var attributeNode = GetAttributeWithName(node, attributeName);
        if (attributeNode != null)
            return attributeNode.Value;
        return null;
    }

    /// <summary>
    /// Sets or creates an attribute with the specified name and value on the given node.
    /// </summary>
    /// <param name="node">The XML node to modify.</param>
    /// <param name="attributeName">The name of the attribute to set.</param>
    /// <param name="attributeValue">The value to assign to the attribute.</param>
    public static void SetAttribute(XmlNode node, string attributeName, string attributeValue)
    {
        var xmlElement = (XmlElement)node;
        if (xmlElement != null)
        {
            xmlElement.SetAttribute(attributeName, attributeValue);
            return;
        }

        var existingValue = Attr(node, attributeName);
        if (existingValue == null)
        {
            var xmlAttribute = node.OwnerDocument!.CreateAttribute(attributeName);
            node.Attributes!.Append(xmlAttribute);
        }

        node.Attributes![attributeName]!.Value = attributeValue;
    }
}
