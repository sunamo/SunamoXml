namespace SunamoXml;

public static partial class XmlHelper
{
    /// <summary>
    /// Get elements of name.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="tagName">The tag name.</param>
    /// <returns>The get elements of name result.</returns>
    public static IList<XmlNode> GetElementsOfName(XmlNode node, string tagName)
    {
        return node.ChildNodes.WithName(tagName);
    }

    /// <summary>
    /// Attr.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="attributeName">The attribute name.</param>
    /// <returns>The attr result.</returns>
    public static string? Attr(XmlNode node, string attributeName)
    {
        var attributeNode = GetAttributeWithName(node, attributeName);
        if (attributeNode != null)
            return attributeNode.Value;
        return null;
    }

    /// <summary>
    /// Set attribute.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="attributeName">The attribute name.</param>
    /// <param name="attributeValue">The attribute value.</param>
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
