namespace SunamoXml;

/// <summary>
/// Helper class for XML operations on <see cref="XmlNode"/> and <see cref="XmlDocument"/>, including attribute access, node search, formatting, and manipulation.
/// </summary>
public static partial class XmlHelper
{
    /// <summary>
    /// Stores the last attribute found by <see cref="GetAttributeWithName"/>.
    /// </summary>
    public static XmlAttribute? FoundAttribute { get; set; }

    /// <summary>
    /// Finds the first attribute with the specified name on the given node.
    /// </summary>
    /// <param name="node">The XML node to search.</param>
    /// <param name="attributeName">The attribute name to find.</param>
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
    /// Determines whether the string appears to be XML content by checking if it starts with a less-than sign.
    /// </summary>
    /// <param name="text">The string to check.</param>
    public static bool IsXml(string text)
    {
        if (!string.IsNullOrEmpty(text) && text.TrimStart().StartsWith('<'))
            return true;
        return false;
    }

    /// <summary>
    /// Formats XML content with indentation. Returns the formatted result or an error message on XML parse failure.
    /// </summary>
    /// <param name="xmlContent">The XML content to format.</param>
    /// <param name="path">Optional file path for error context in exception messages.</param>
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
    /// Returns the attribute value or the inner text of a child element with the specified name.
    /// </summary>
    /// <param name="node">The XML node to search.</param>
    /// <param name="attributeName">The attribute or child element name.</param>
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
    /// Returns the inner XML value of the first attribute with the specified name.
    /// </summary>
    /// <param name="node">The XML node to search.</param>
    /// <param name="attributeName">The attribute name to find.</param>
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
    /// Returns child nodes as a generic list since XmlNodeList only implements IEnumerable.
    /// </summary>
    /// <param name="node">The parent XML node.</param>
    public static List<XmlNode> ChildNodes(XmlNode node)
    {
        var result = new List<XmlNode>();
        foreach (XmlNode item in node.ChildNodes)
            result.Add(item);
        return result;
    }

    /// <summary>
    /// Replaces a child node by matching outer XML, working around the "node to be removed is not a child" error.
    /// </summary>
    /// <param name="sourceNode">The node to be replaced.</param>
    /// <param name="targetNode">The replacement node.</param>
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
    /// Returns the inner XML or CDATA value depending on the node type.
    /// </summary>
    /// <param name="node">The XML node to read.</param>
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
    /// Removes specified attributes from the document root and re-creates it with a new element name.
    /// </summary>
    /// <param name="xmlDocument">The XML document to modify (passed by reference).</param>
    /// <param name="newRootElementName">The name for the new root element.</param>
    /// <param name="attributeNames">The attribute names to remove from the root.</param>
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
    /// Adds attributes to the document root, re-creating it with a new element name. Currently performs validation only.
    /// </summary>
    /// <param name="xmlDocument">The XML document to modify (passed by reference).</param>
    /// <param name="newRootElementName">The name for the new root element.</param>
    /// <param name="attributes">Alternating attribute names and values to add.</param>
    public static void AddAttrsToRoot(ref XmlDocument xmlDocument, string newRootElementName, params string[] attributes)
    {
        if (!ThrowEx.HasOddNumberOfElements("attributes", attributes))
            return;
    }

    /// <summary>
    /// Returns the inner text of the specified XML node.
    /// </summary>
    /// <param name="node">The XML node to read.</param>
    public static string InnerTextOfNode(XmlNode node)
    {
        return node.InnerText;
    }

    /// <summary>
    /// Creates and returns a new XmlDocument with whitespace preservation from the specified XML content.
    /// </summary>
    /// <param name="content">The XML content string to load.</param>
    public static XmlDocument CreateXmlDocument(string content)
    {
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(content);
        xmlDocument.PreserveWhitespace = true;
        return xmlDocument;
    }

    /// <summary>
    /// Returns the first child node with the specified name, or null if not found.
    /// </summary>
    /// <param name="node">The parent XML node to search.</param>
    /// <param name="tagName">The child node name to find.</param>
    public static XmlNode? GetChildNodeWithName(XmlNode node, string tagName)
    {
        foreach (XmlNode item in node.ChildNodes)
            if (item.Name == tagName)
                return item;
        return null;
    }

    /// <summary>
    /// Returns the first child element with the specified tag name.
    /// </summary>
    /// <param name="node">The parent XML node.</param>
    /// <param name="tagName">The tag name to find.</param>
    public static XmlNode? GetElementOfName(XmlNode node, string tagName)
    {
        return node.ChildNodes.First(tagName);
    }
}
