namespace SunamoXml;

/// <summary>
/// Parses XML documents while extracting and removing namespace declarations, storing them in an XmlNamespaceManager for later use.
/// </summary>
public class XmlNamespacesHolder
{
    /// <summary>
    /// The namespace manager populated during parsing with all extracted namespace declarations.
    /// </summary>
    public XmlNamespaceManager? NamespaceManager { get; set; }

    /// <summary>
    /// Parses an XML string, extracts namespace declarations into <see cref="NamespaceManager"/>, and removes them from the document.
    /// </summary>
    /// <param name="content">The XML content to parse.</param>
    public XmlDocument ParseAndRemoveNamespacesXmlDocument(string content)
    {
        var xmlDocument = new XmlDocument();
        xmlDocument = ParseAndRemoveNamespacesXmlDocument(content, xmlDocument.NameTable);
        return xmlDocument;
    }

    /// <summary>
    /// Parses an XML string using the specified name table, extracts namespace declarations, and removes them from the document.
    /// The default prefix is used for the default namespace since XPath queries require a prefix (/:Tag doesn't work, but /prefix:Tag does).
    /// </summary>
    /// <param name="content">The XML content to parse.</param>
    /// <param name="nameTable">The XmlNameTable to use for the namespace manager.</param>
    /// <param name="defaultPrefix">The prefix to assign to the default namespace.</param>
    public XmlDocument ParseAndRemoveNamespacesXmlDocument(string content, XmlNameTable nameTable, string defaultPrefix = "x")
    {
        var xmlDocument = new XmlDocument();
        NamespaceManager = new XmlNamespaceManager(nameTable);
        xmlDocument.LoadXml(content);
        foreach (XmlNode item in xmlDocument.ChildNodes)
        {
            if (item.NodeType == XmlNodeType.XmlDeclaration) continue;
            var root = item;
            for (var i = root.Attributes!.Count - 1; i >= 0; i--)
            {
                var attribute = root.Attributes[i];
                var key = defaultPrefix;
                if (attribute.Name.StartsWith("xmlns"))
                {
                    if (attribute.Name.Contains(':')) key = attribute.Name.Substring(6);
                    NamespaceManager.AddNamespace(key, attribute.Value);
                    root.Attributes.RemoveAt(i);
                }
            }
        }

        return xmlDocument;
    }

    /// <summary>
    /// Parses an XML string, removes namespace declarations, and returns the result as an XDocument.
    /// </summary>
    /// <param name="content">The XML content to parse.</param>
    public XDocument ParseAndRemoveNamespacesXDocument(string content)
    {
        var xmlDocument = ParseAndRemoveNamespacesXmlDocument(content);
        return XDocument.Parse(xmlDocument.OuterXml);
    }

    /// <summary>
    /// Parses an XML string using the specified name table, removes namespace declarations, and returns the result as an XDocument.
    /// </summary>
    /// <param name="content">The XML content to parse.</param>
    /// <param name="nameTable">The XmlNameTable to use.</param>
    /// <param name="defaultPrefix">The prefix to assign to the default namespace.</param>
    public XDocument ParseAndRemoveNamespacesXDocument(string content, XmlNameTable nameTable, string defaultPrefix = "x")
    {
        var xmlDocument = ParseAndRemoveNamespacesXmlDocument(content, nameTable, defaultPrefix);
        return new XDocument(xmlDocument.OuterXml);
    }
}
