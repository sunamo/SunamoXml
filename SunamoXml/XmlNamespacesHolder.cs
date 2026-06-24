namespace SunamoXml;

public class XmlNamespacesHolder
{
    public XmlNamespaceManager? NamespaceManager { get; set; }

    public XmlDocument ParseAndRemoveNamespacesXmlDocument(string content)
    {
        var xmlDocument = new XmlDocument();
        xmlDocument = ParseAndRemoveNamespacesXmlDocument(content, xmlDocument.NameTable);
        return xmlDocument;
    }

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

    public XDocument ParseAndRemoveNamespacesXDocument(string content)
    {
        var xmlDocument = ParseAndRemoveNamespacesXmlDocument(content);
        return XDocument.Parse(xmlDocument.OuterXml);
    }

    public XDocument ParseAndRemoveNamespacesXDocument(string content, XmlNameTable nameTable, string defaultPrefix = "x")
    {
        var xmlDocument = ParseAndRemoveNamespacesXmlDocument(content, nameTable, defaultPrefix);
        return new XDocument(xmlDocument.OuterXml);
    }
}
