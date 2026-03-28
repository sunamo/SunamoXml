namespace SunamoXml;

/// <summary>
/// Additional XElement-based XML helper methods for namespace extraction, formatting, element search, and value retrieval.
/// </summary>
public partial class XHelper
{
    /// <summary>
    /// Extracts namespace declarations from an XmlNamespaceManager into a dictionary, optionally prefixed with "xmlns:".
    /// </summary>
    /// <param name="namespaceManager">The namespace manager to read from.</param>
    /// <param name="isWithPrefixedXmlnsColon">Whether to prefix keys with "xmlns:" (or "xmlns" for the default namespace).</param>
    public static Dictionary<string, string> XmlNamespaces(XmlNamespaceManager namespaceManager, bool isWithPrefixedXmlnsColon)
    {
        var namespaceDictionary = new Dictionary<string, string>();
        foreach (string namespacePrefix in namespaceManager)
        {
            var key = namespacePrefix;
            if (isWithPrefixedXmlnsColon)
            {
                if (key == string.Empty || key == "xmlns")
                    key = "xmlns";
                else
                    key = "xmlns:" + key;
            }

            var namespaceValue = namespaceManager.LookupNamespace(namespacePrefix) ?? string.Empty;
            if (!namespaceDictionary.ContainsKey(key))
                namespaceDictionary.Add(key, namespaceValue);
        }

        return namespaceDictionary;
    }

    /// <summary>
    /// Formats XML content with proper indentation. If input is a file path, saves the result back to the file and returns null. Otherwise returns the formatted string.
    /// </summary>
    /// <param name="pathOrContent">The file path or XML content string.</param>
    public static
#if ASYNC
        async Task<string?>
#else
    string?
#endif
    FormatXml(string pathOrContent)
    {
        var xmlFormat = pathOrContent;
        if (File.Exists(pathOrContent))
            xmlFormat =
#if ASYNC
                await
#endif
            File.ReadAllTextAsync(pathOrContent);
        var namespacesHolder = new XmlNamespacesHolder();
        var document = namespacesHolder.ParseAndRemoveNamespacesXDocument(xmlFormat);
        var formatted = document.ToString();
        formatted = formatted.Replace(" xmlns=\"\"", string.Empty);
        if (File.Exists(pathOrContent))
        {
#if ASYNC
            await
#endif
            File.WriteAllTextAsync(pathOrContent, formatted);
            return null;
        }

        return formatted;
    }

    /// <summary>
    /// Formats XML content in memory using XDocument parsing. Returns the original string on parse failure.
    /// </summary>
    /// <param name="xml">The XML string to format.</param>
    public static string FormatXmlInMemory(string xml)
    {
        try
        {
            var document = XDocument.Parse(xml);
            return document.ToString();
        }
        catch (Exception)
        {
            return xml;
        }
    }

    /// <summary>
    /// Returns the inner XML content of an XElement.
    /// </summary>
    /// <param name="parent">The parent XElement to read from.</param>
    public static string GetInnerXml(XElement parent)
    {
        var reader = parent.CreateReader();
        reader.MoveToContent();
        return reader.ReadInnerXml();
    }

    /// <summary>
    /// Returns elements matching the specified tag name whose attribute has the exact specified value.
    /// </summary>
    /// <param name="element">The parent XElement to search.</param>
    /// <param name="tagName">The tag name to match.</param>
    /// <param name="attributeName">The attribute name to check.</param>
    /// <param name="attributeValue">The expected attribute value.</param>
    public static List<XElement> GetElementsOfNameWithAttr(XElement element, string tagName, string attributeName, string attributeValue)
    {
        return GetElementsOfNameWithAttrWorker(element, tagName, attributeName, attributeValue);
    }

    /// <summary>
    /// Worker method that finds elements by tag name whose attribute value contains the specified text.
    /// </summary>
    /// <param name="element">The parent XElement to search.</param>
    /// <param name="tagName">The tag name to match.</param>
    /// <param name="attributeName">The attribute name to check.</param>
    /// <param name="attributeValue">The text that the attribute value must contain.</param>
    public static List<XElement> GetElementsOfNameWithAttrWorker(XElement element, string tagName, string attributeName, string attributeValue)
    {
        var result = new List<XElement>();
        var elements = GetElementsOfNameRecursive(element, tagName);
        foreach (var item in elements)
        {
            var foundAttributeValue = Attr(item, attributeName);
            if (foundAttributeValue != null && foundAttributeValue.Contains(attributeValue))
                result.Add(item);
        }

        return result;
    }

    /// <summary>
    /// Finds the first descendant element (including self) with the specified tag name. Supports namespace-prefixed tag names.
    /// </summary>
    /// <param name="node">The XElement to search recursively.</param>
    /// <param name="tagName">The tag name to find.</param>
    public static XElement? GetElementOfNameRecursive(XElement node, string tagName)
    {
        if (tagName.Contains(':'))
        {
            var (namespaceName, localName) = SH.GetPartsByLocationNoOut(tagName, ':');
            namespaceName = Namespaces[namespaceName];
            foreach (var item in node.DescendantsAndSelf())
                if (item.Name.LocalName == localName && item.Name.NamespaceName == namespaceName)
                    return item;
        }
        else
        {
            foreach (var item in node.DescendantsAndSelf())
                if (item.Name.LocalName == tagName)
                    return item;
        }

        return null;
    }

    /// <summary>
    /// Returns the concatenated text values of all sub-elements separated by the specified delimiter, with XML tags replaced.
    /// </summary>
    /// <param name="element">The XElement to extract values from.</param>
    /// <param name="delimiter">The delimiter to insert between values.</param>
    public static string ReturnValueAllSubElementsSeparatedBy(XElement element, string delimiter)
    {
        var stringBuilder = new StringBuilder();
        var xml = GetXml(element);
        var matches = Regex.Matches(xml, "<(?:\"[^\"]*\"['\"]*|'[^']*'['\"]*|[^'\">])+>");
        var replacedValues = new List<string>();
        foreach (Match item in matches)
            if (!replacedValues.Contains(item.Value))
            {
                replacedValues.Add(item.Value);
                xml = xml.Replace(item.Value, delimiter);
            }

        stringBuilder.Append(xml);
        return stringBuilder.ToString().Replace(delimiter + delimiter, delimiter);
    }

    /// <summary>
    /// Serializes an XElement to its XML string representation.
    /// </summary>
    /// <param name="node">The XElement to serialize.</param>
    public static string GetXml(XElement node)
    {
        var stringWriter = new StringWriter();
        var xmlWriter = XmlWriter.Create(stringWriter);
        node.WriteTo(xmlWriter);
        return stringWriter.ToString();
    }

    /// <summary>
    /// Returns the child element found at the second nesting level (firstLevelName/secondLevelName).
    /// </summary>
    /// <param name="parentElement">The root XElement to search from.</param>
    /// <param name="firstLevelName">The first-level element name.</param>
    /// <param name="secondLevelName">The second-level element name.</param>
    public static XElement? GetElementOfSecondLevel(XElement parentElement, string firstLevelName, string secondLevelName)
    {
        var firstLevelElement = parentElement.Element(XName.Get(firstLevelName));
        if (firstLevelElement != null)
        {
            var secondLevelElement = firstLevelElement.Element(XName.Get(secondLevelName));
            return secondLevelElement;
        }

        return null;
    }

    /// <summary>
    /// Returns the trimmed value of a second-level element, or an empty string if not found.
    /// </summary>
    /// <param name="parentElement">The root XElement to search from.</param>
    /// <param name="firstLevelName">The first-level element name.</param>
    /// <param name="secondLevelName">The second-level element name.</param>
    public static string GetValueOfElementOfSecondLevelOrSE(XElement parentElement, string firstLevelName, string secondLevelName)
    {
        var element = GetElementOfSecondLevel(parentElement, firstLevelName, secondLevelName);
        if (element != null)
            return element.Value.Trim();
        return "";
    }

    /// <summary>
    /// Returns the trimmed value of a named element, or an empty string if not found.
    /// </summary>
    /// <param name="parentElement">The XElement to search.</param>
    /// <param name="tagName">The element name to find.</param>
    public static string GetValueOfElementOfNameOrSE(XElement parentElement, string tagName)
    {
        var element = GetElementOfName(parentElement, tagName);
        if (element == null)
            return "";
        return element.Value.Trim();
    }
}
