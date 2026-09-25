#pragma warning disable IDE0060 // parametry zachovány kvůli veřejnému API
namespace SunamoXml;

public static partial class XmlHelper
{
    public static XmlAttribute? FoundAttribute { get; set; }

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

    public static bool IsXml(string text)
    {
        if (!string.IsNullOrEmpty(text) && text.TrimStart().StartsWith("<"))
            return true;
        return false;
    }

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

    public static List<XmlNode> ChildNodes(XmlNode node)
    {
        var result = new List<XmlNode>();
        foreach (XmlNode item in node.ChildNodes)
            result.Add(item);
        return result;
    }

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

    public static void AddAttrsToRoot(ref XmlDocument xmlDocument, string newRootElementName, params string[] attributes)
    {
        if (!ThrowEx.HasOddNumberOfElements("attributes", attributes))
            return;
    }

    public static string InnerTextOfNode(XmlNode node)
    {
        return node.InnerText;
    }

    public static XmlDocument CreateXmlDocument(string content)
    {
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(content);
        xmlDocument.PreserveWhitespace = true;
        return xmlDocument;
    }

    public static XmlNode? GetChildNodeWithName(XmlNode node, string tagName)
    {
        foreach (XmlNode item in node.ChildNodes)
            if (item.Name == tagName)
                return item;
        return null;
    }

    public static XmlNode? GetElementOfName(XmlNode node, string tagName)
    {
        return node.ChildNodes.First(tagName);
    }
}
