#pragma warning disable IDE0060 // parameters kept because of the public API
namespace SunamoXml;

/// <summary>
/// Represents the xml helper type.
/// </summary>
public static partial class XmlHelper
{
    /// <summary>
    /// Gets or sets the found attribute.
    /// </summary>
    public static XmlAttribute? FoundAttribute { get; set; }

    /// <summary>
    /// Get attribute with name.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="attributeName">The attribute name.</param>
    /// <returns>The get attribute with name result.</returns>
    public static XmlNode? GetAttributeWithName(XmlNode node, string attributeName)
    {
        if (node.Attributes == null) return null;
        foreach (XmlAttribute item in node.Attributes)
            if (item.Name == attributeName)
            {
                FoundAttribute = item;
                return item;
            }

        return null;
    }

    /// <summary>
    /// Is xml.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>The is xml result.</returns>
    public static bool IsXml(string text)
    {
        if (!string.IsNullOrEmpty(text) && text.TrimStart().StartsWith("<"))
            return true;
        return false;
    }

    /// <summary>
    /// Format xml in memory.
    /// </summary>
    /// <param name="xmlContent">The xml content.</param>
    /// <param name="path">The path.</param>
    /// <returns>The format xml in memory result.</returns>
    public static string FormatXmlInMemory(string xmlContent, string path = "")
    {
        MemoryStream memoryStream = new();
        XmlTextWriter writer = new(memoryStream, Encoding.Unicode);
        XmlDocument document = new();
        string result;
        try
        {
            document.LoadXml(xmlContent);
            writer.Formatting = Formatting.Indented;
            document.WriteContentTo(writer);
            writer.Flush();
            memoryStream.Flush();
            memoryStream.Position = 0;
            StreamReader streamReader = new(memoryStream);
            var formattedXml = streamReader.ReadToEnd();
            result = formattedXml;
        }
        catch (XmlException exception)
        {
            return "Exception:" + path + Environment.NewLine + Environment.NewLine + exception.Message;
        }

        memoryStream.Close();
        return result;
    }

    /// <summary>
    /// Get attr value or inner element.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="attributeName">The attribute name.</param>
    /// <returns>The get attr value or inner element result.</returns>
    public static string? GetAttrValueOrInnerElement(XmlNode node, string attributeName)
    {
        var attribute = node.Attributes?[attributeName];
        if (attribute != null)
            return attribute.Value;
        var childNodeList = ChildNodes(node);
        if (childNodeList.Count != 0)
        {
            var element = childNodeList.First(childNode => childNode.Name == attributeName);
            return element?.Value;
        }

        Debugger.Break();
        return null;
    }

    /// <summary>
    /// Get attribute with name value.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="attributeName">The attribute name.</param>
    /// <returns>The get attribute with name value result.</returns>
    public static string? GetAttributeWithNameValue(XmlNode node, string attributeName)
    {
        if (node.Attributes == null) return null;
        foreach (XmlAttribute item in node.Attributes)
            if (item.Name == attributeName)
            {
                FoundAttribute = item;
                return item.InnerXml;
            }

        return null;
    }

    /// <summary>
    /// Child nodes.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <returns>The child nodes result.</returns>
    public static List<XmlNode> ChildNodes(XmlNode node)
    {
        var result = new List<XmlNode>();
        foreach (XmlNode item in node.ChildNodes)
            result.Add(item);
        return result;
    }

    /// <summary>
    /// Replace child node by outer html.
    /// </summary>
    /// <param name="sourceNode">The source node.</param>
    /// <param name="targetNode">The target node.</param>
    /// <returns>The replace child node by outer html result.</returns>
    public static XmlNode ReplaceChildNodeByOuterHtml(XmlNode sourceNode, XmlNode targetNode)
    {
        var parentNode = sourceNode.ParentNode!;
        var childNodes = parentNode.ChildNodes;
        if (childNodes.Contains(sourceNode))
        {
            sourceNode = sourceNode.ParentNode!.ReplaceChild(targetNode, sourceNode);
        }
        else
        {
            var targetOuterXml = targetNode.OuterXml;
            for (var i = 0; i < childNodes.Count; i++)
            {
                var childNode = childNodes[i];
                if (childNode != null)
                {
                    var outerXml = childNode.OuterXml;
                    if (outerXml == targetOuterXml)
                    {
                        sourceNode = parentNode.ReplaceChild(targetNode, childNode);
                        break;
                    }
                }
            }
        }

        return sourceNode;
    }

    /// <summary>
    /// Get inner xml.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <returns>The get inner xml result.</returns>
    public static string GetInnerXml(XmlNode node)
    {
        var innerContent = "";
        if (node is XmlCDataSection cdataSection)
        {
            innerContent = cdataSection.Value ?? string.Empty;
        }
        else
        {
            if (node != null)
                innerContent = node.InnerXml;
        }

        return innerContent;
    }

    /// <summary>
    /// Remove attrs from root.
    /// </summary>
    /// <param name="xmlDocument">The xml document.</param>
    /// <param name="newRootElementName">The new root element name.</param>
    /// <param name="attributeNames">The attribute names.</param>
    /// <returns>The remove attrs from root result.</returns>
    public static void RemoveAttrsFromRoot(ref XmlDocument xmlDocument, string newRootElementName, params string[] attributeNames)
    {
        var newDocument = new XmlDocument();
        var newRoot = newDocument.CreateElement(newRootElementName);
        foreach (XmlAttribute item in xmlDocument.DocumentElement!.Attributes)
            if (!attributeNames.Contains(item.Name))
            {
                var importedNode = newDocument.ImportNode(item, true);
                var xmlAttribute = (XmlAttribute)importedNode;
                newRoot.Attributes.Append(xmlAttribute);
            }

        if (newRoot.Attributes.Count == 0)
        {
            var xmlAttribute = newDocument.CreateAttribute("xmlns");
            xmlAttribute.Value = "http://";
            newRoot.Attributes.Append(xmlAttribute);
        }

        newDocument.AppendChild(newRoot);
        newRoot.InnerXml = xmlDocument.DocumentElement.InnerXml;
        xmlDocument = newDocument;
    }

    /// <summary>
    /// Add attrs to root.
    /// </summary>
    /// <param name="xmlDocument">The xml document.</param>
    /// <param name="newRootElementName">The new root element name.</param>
    /// <param name="attributes">The attributes.</param>
    /// <returns>The add attrs to root result.</returns>
    public static void AddAttrsToRoot(ref XmlDocument xmlDocument, string newRootElementName, params string[] attributes)
    {
        if (!ThrowEx.HasOddNumberOfElements("attributes", attributes))
            return;
    }

    /// <summary>
    /// Inner text of node.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <returns>The inner text of node result.</returns>
    public static string InnerTextOfNode(XmlNode node)
    {
        return node.InnerText;
    }

    /// <summary>
    /// Create xml document.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <returns>The create xml document result.</returns>
    public static XmlDocument CreateXmlDocument(string content)
    {
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(content);
        xmlDocument.PreserveWhitespace = true;
        return xmlDocument;
    }

    /// <summary>
    /// Get child node with name.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="tagName">The tag name.</param>
    /// <returns>The get child node with name result.</returns>
    public static XmlNode? GetChildNodeWithName(XmlNode node, string tagName)
    {
        foreach (XmlNode item in node.ChildNodes)
            if (item.Name == tagName)
                return item;
        return null;
    }

    /// <summary>
    /// Get element of name.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="tagName">The tag name.</param>
    /// <returns>The get element of name result.</returns>
    public static XmlNode? GetElementOfName(XmlNode node, string tagName)
    {
        return node.ChildNodes.First(tagName);
    }
}
