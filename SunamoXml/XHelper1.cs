namespace SunamoXml;

public partial class XHelper
{
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

    public static
        async Task<string?>
    FormatXml(string pathOrContent)
    {
        var xmlFormat = pathOrContent;
        if (File.Exists(pathOrContent))
            xmlFormat =
                await FileAsync.ReadAllTextAsync(pathOrContent);
        var namespacesHolder = new XmlNamespacesHolder();
        var document = namespacesHolder.ParseAndRemoveNamespacesXDocument(xmlFormat);
        var formatted = document.ToString();
        formatted = formatted.Replace(" xmlns=\"\"", string.Empty);
        if (File.Exists(pathOrContent))
        {
            await FileAsync.WriteAllTextAsync(pathOrContent, formatted);
            return null;
        }

        return formatted;
    }

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

    public static string GetInnerXml(XElement parent)
    {
        var reader = parent.CreateReader();
        reader.MoveToContent();
        return reader.ReadInnerXml();
    }

    public static List<XElement> GetElementsOfNameWithAttr(XElement element, string tagName, string attributeName, string attributeValue)
    {
        return GetElementsOfNameWithAttrWorker(element, tagName, attributeName, attributeValue);
    }

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

    public static string GetXml(XElement node)
    {
        var stringWriter = new StringWriter();
        var xmlWriter = XmlWriter.Create(stringWriter);
        node.WriteTo(xmlWriter);
        return stringWriter.ToString();
    }

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

    public static string GetValueOfElementOfSecondLevelOrSE(XElement parentElement, string firstLevelName, string secondLevelName)
    {
        var element = GetElementOfSecondLevel(parentElement, firstLevelName, secondLevelName);
        if (element != null)
            return element.Value.Trim();
        return "";
    }

    public static string GetValueOfElementOfNameOrSE(XElement parentElement, string tagName)
    {
        var element = GetElementOfName(parentElement, tagName);
        if (element == null)
            return "";
        return element.Value.Trim();
    }
}
