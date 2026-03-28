namespace SunamoXml;

/// <summary>
/// Helper class for XElement-based XML operations including namespace management, element search, attribute access, and XML minification.
/// </summary>
public partial class XHelper
{
    /// <summary>
    /// Dictionary mapping namespace prefixes to their URI values.
    /// </summary>
    public static Dictionary<string, string> Namespaces { get; set; } = new();

    /// <summary>
    /// Returns the inner text of a descendant element with the specified name.
    /// </summary>
    /// <param name="element">The parent XElement to search.</param>
    /// <param name="elementName">The descendant element name to find.</param>
    public static string InnerTextOfNode(XElement element, string elementName)
    {
        var descendants = element.Descendants(XName.Get(elementName));
        if (!descendants.Any())
            return string.Empty;
        var first = descendants.First();
        return first.Value;
    }

    /// <summary>
    /// Returns the value of the specified attribute, or null if not found.
    /// </summary>
    /// <param name="element">The XElement to search.</param>
    /// <param name="attributeName">The attribute name to find.</param>
    public static string? Attr(XElement element, string attributeName)
    {
        var xmlAttribute = element.Attribute(XName.Get(attributeName));
        if (xmlAttribute != null)
            return xmlAttribute.Value;
        return null;
    }

    /// <summary>
    /// Finds an element by tag name that also has an attribute with the specified value. Supports namespace-prefixed tag names.
    /// </summary>
    /// <param name="node">The parent XElement to search.</param>
    /// <param name="tagName">The tag name to match, optionally with namespace prefix (e.g., "ns:tag").</param>
    /// <param name="attributeName">The attribute name to match.</param>
    /// <param name="attributeValue">The expected attribute value.</param>
    public static XElement? GetElementOfNameWithAttr(XElement node, string tagName, string attributeName, string attributeValue)
    {
        if (tagName.Contains(':'))
        {
            var (namespaceName, localName) = SH.GetPartsByLocationNoOut(tagName, ':');
            namespaceName = Namespaces[namespaceName];
            foreach (var item in node.Elements())
                if (item.Name.LocalName == localName && item.Name.NamespaceName == namespaceName)
                    if (Attr(item, attributeName) == attributeValue)
                        return item;
        }
        else
        {
            foreach (var item in node.DescendantsAndSelf())
                if (item.Name.LocalName == tagName)
                    if (Attr(item, attributeName) == attributeValue)
                        return item;
        }

        return null;
    }

    /// <summary>
    /// Re-creates the element with all descendants shifted into the default namespace.
    /// </summary>
    /// <param name="element">The XElement to transform.</param>
    public static XElement MakeAllElementsWithDefaultNs(XElement element)
    {
        var defaultNamespace = Namespaces[string.Empty];
        foreach (var descendant in element.DescendantsAndSelf())
            descendant.Name = XName.Get(descendant.Name.LocalName, defaultNamespace);
        var result = new XElement(XName.Get(element.Name.LocalName, defaultNamespace), element.Attributes(), element.Descendants());
        return result;
    }

    /// <summary>
    /// Returns all direct child elements matching the specified tag name. Supports namespace-prefixed tag names.
    /// </summary>
    /// <param name="node">The parent XElement to search.</param>
    /// <param name="tagName">The tag name to match.</param>
    public static List<XElement> GetElementsOfName(XElement node, string tagName)
    {
        var result = new List<XElement>();
        if (tagName.Contains(':'))
        {
            foreach (var item in node.Elements())
                if (IsRightTag(item, tagName))
                    result.Add(item);
        }
        else
        {
            foreach (var item in node.Elements())
                if (item.Name.LocalName == tagName)
                    result.Add(item);
        }

        return result;
    }

    /// <summary>
    /// Returns elements matching the specified tag name whose attribute value contains the specified text.
    /// </summary>
    /// <param name="element">The parent XElement to search.</param>
    /// <param name="tagName">The tag name to match.</param>
    /// <param name="attributeName">The attribute name to check.</param>
    /// <param name="attributeValue">The text that the attribute value must contain.</param>
    public static IList<XElement> GetElementsOfNameWithAttrContains(XElement element, string tagName, string attributeName, string attributeValue)
    {
        return GetElementsOfNameWithAttrWorker(element, tagName, attributeName, attributeValue);
    }

    /// <summary>
    /// Adds all namespace declarations from an XmlNamespaceManager to the shared Namespaces dictionary.
    /// </summary>
    /// <param name="namespaceManager">The namespace manager to read from.</param>
    public static void AddXmlNamespaces(XmlNamespaceManager namespaceManager)
    {
        foreach (string item in namespaceManager)
        {
            var namespaceValue = namespaceManager.LookupNamespace(item) ?? string.Empty;
            if (!Namespaces.ContainsKey(item))
                Namespaces.Add(item, namespaceValue);
        }
    }

    /// <summary>
    /// Adds namespace declarations from alternating prefix-URI pairs.
    /// </summary>
    /// <param name="namespacePairs">Alternating namespace prefix (with "xmlns:" prefix) and URI values.</param>
    public static void AddXmlNamespaces(params string[] namespacePairs)
    {
        for (var i = 0; i < namespacePairs.Length; i++)
            Namespaces.Add(namespacePairs[i].Replace("xmlns:", ""), namespacePairs[++i]);
    }

    /// <summary>
    /// Adds all entries from a dictionary to the shared Namespaces dictionary.
    /// </summary>
    /// <param name="dictionary">The dictionary of prefix-URI pairs to add.</param>
    public static void AddXmlNamespaces(Dictionary<string, string> dictionary)
    {
        foreach (var item in dictionary)
            Namespaces.Add(item.Key, item.Value);
    }

    /// <summary>
    /// Creates an XDocument from an XML string or file path.
    /// </summary>
    /// <param name="contentOrFilePath">The XML content string or file path.</param>
    public static
#if ASYNC
        async Task<XDocument>
#else
    XDocument
#endif
    CreateXDocument(string contentOrFilePath)
    {
        if (File.Exists(contentOrFilePath))
            contentOrFilePath =
#if ASYNC
                await
#endif
            File.ReadAllTextAsync(contentOrFilePath);
        var encodedBytes = Encoding.UTF8.GetBytes(contentOrFilePath).ToList();
        XDocument document;
        using (var memoryStream = new MemoryStream(encodedBytes.ToArray()))
        using (var xmlReader = XmlReader.Create(memoryStream))
        {
            document = XDocument.Load(xmlReader);
        }

        return document;
    }

    /// <summary>
    /// Finds an element by name within the container. Supports namespace-prefixed tag names (e.g., "ns:tag").
    /// </summary>
    /// <param name="node">The container to search.</param>
    /// <param name="tagName">The tag name to find.</param>
    public static XElement? GetElementOfName(XContainer node, string tagName)
    {
        if (tagName.Contains(':'))
        {
            var (namespaceName, localName) = SH.GetPartsByLocationNoOut(tagName, ':');
            namespaceName = Namespaces[namespaceName];
            foreach (var item in node.Elements())
            {
                if (item.Name.LocalName == localName && item.Name.NamespaceName == namespaceName)
                    return item;
            }
        }
        else
        {
            foreach (var item in node.Elements())
                if (item.Name.LocalName == tagName)
                    return item;
        }

        return null;
    }

    /// <summary>
    /// Checks if the element matches the namespace-prefixed tag name.
    /// </summary>
    /// <param name="element">The XElement to check.</param>
    /// <param name="tagName">The namespace-prefixed tag name (e.g., "ns:tag").</param>
    public static bool IsRightTag(XElement element, string tagName)
    {
        return IsRightTag(element.Name, tagName);
    }

    /// <summary>
    /// Checks if the XName matches the namespace-prefixed tag name by splitting into local name and namespace.
    /// </summary>
    /// <param name="xName">The XName to check.</param>
    /// <param name="tagName">The namespace-prefixed tag name (e.g., "ns:tag").</param>
    public static bool IsRightTag(XName xName, string tagName)
    {
        var (namespaceName, localName) = SH.GetPartsByLocationNoOut(tagName, ':');
        namespaceName = Namespaces[namespaceName];
        if (xName.LocalName == localName && xName.NamespaceName == namespaceName)
            return true;
        return false;
    }

    /// <summary>
    /// Checks if the element matches the specified local name and namespace.
    /// </summary>
    /// <param name="element">The XElement to check.</param>
    /// <param name="localName">The expected local name.</param>
    /// <param name="namespaceName">The expected namespace name.</param>
    public static bool IsRightTag(XElement element, string localName, string namespaceName)
    {
        return IsRightTag(element.Name, localName, namespaceName);
    }

    /// <summary>
    /// Checks if the XName matches the specified local name and namespace.
    /// </summary>
    /// <param name="xName">The XName to check.</param>
    /// <param name="localName">The expected local name.</param>
    /// <param name="namespaceName">The expected namespace name.</param>
    public static bool IsRightTag(XName xName, string localName, string namespaceName)
    {
        if (xName.LocalName == localName && xName.NamespaceName == namespaceName)
            return true;
        return false;
    }

    /// <summary>
    /// Returns all descendant elements (including self) matching the specified tag name. Supports namespace-prefixed tag names.
    /// </summary>
    /// <param name="node">The XElement to search recursively.</param>
    /// <param name="tagName">The tag name to match.</param>
    public static List<XElement> GetElementsOfNameRecursive(XElement node, string tagName)
    {
        var result = new List<XElement>();
        if (tagName.Contains(':'))
        {
            var (namespaceName, localName) = SH.GetPartsByLocationNoOut(tagName, ':');
            namespaceName = Namespaces[namespaceName];
            foreach (var item in node.DescendantsAndSelf())
                if (item.Name.LocalName == localName && item.Name.NamespaceName == namespaceName)
                    result.Add(item);
        }
        else
        {
            foreach (var item in node.DescendantsAndSelf())
                if (item.Name.LocalName == tagName)
                    result.Add(item);
        }

        return result;
    }

    /// <summary>
    /// Minifies XML by removing newlines, collapsing whitespace, and removing spaces between tags.
    /// </summary>
    /// <param name="text">The XML text to minify.</param>
    public static string Minify(string text)
    {
        text = text.Replace(Environment.NewLine, string.Empty);
        text = SHReplace.ReplaceAllWhitecharsForSpace(text);
        text = SHReplace.ReplaceAllDoubleSpaceToSingle(text);
        text = text.Replace("> <", "><");
        return text;
    }
}
