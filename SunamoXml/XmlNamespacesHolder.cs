namespace SunamoXml;

/// <summary>
/// Represents the xml namespaces holder type.
/// </summary>
public class XmlNamespacesHolder
{
    /// <summary>
    /// Gets or sets the namespace manager.
    /// </summary>
    public XmlNamespaceManager? NamespaceManager { get; set; }

    /// <summary>
    /// Parse and remove namespaces xml document.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <returns>The parse and remove namespaces xml document result.</returns>
    public XmlDocument ParseAndRemoveNamespacesXmlDocument(string content)
    {
        var xmlDocument = new XmlDocument();
        xmlDocument = ParseAndRemoveNamespacesXmlDocument(content, xmlDocument.NameTable);
        return xmlDocument;
    }

    /// <summary>
    /// Parse and remove namespaces xml document.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <param name="nameTable">The name table.</param>
    /// <param name="defaultPrefix">The default prefix.</param>
    /// <returns>The parse and remove namespaces xml document result.</returns>
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
    /// Parse and remove namespaces x document.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <returns>The parse and remove namespaces x document result.</returns>
    public XDocument ParseAndRemoveNamespacesXDocument(string content)
    {
        var xmlDocument = ParseAndRemoveNamespacesXmlDocument(content);
        return XDocument.Parse(xmlDocument.OuterXml);
    }

    /// <summary>
    /// Parse and remove namespaces x document.
    /// </summary>
    /// <param name="xmlDocument.OuterXml">The xml document outer xml.</param>
    /// <returns>The parse and remove namespaces x document result.</returns>
    public XDocument ParseAndRemoveNamespacesXDocument(string content, XmlNameTable nameTable, string defaultPrefix = "x")
    {
        var xmlDocument = ParseAndRemoveNamespacesXmlDocument(content, nameTable, defaultPrefix);
        return new XDocument(xmlDocument.OuterXml);
    }
}
