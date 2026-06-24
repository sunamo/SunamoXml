namespace SunamoXml;

public static partial class XmlHelper
{
    public static IList<XmlNode> GetElementsOfName(XmlNode node, string tagName)
    {
        return node.ChildNodes.WithName(tagName);
    }

    public static string? Attr(XmlNode node, string attributeName)
    {
        var attributeNode = GetAttributeWithName(node, attributeName);
        if (attributeNode != null)
            return attributeNode.Value;
        return null;
    }

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
