namespace SunamoXml;

public partial class XHelper
{
    public static Dictionary<string, string> Namespaces { get; set; } = new();

    public static string InnerTextOfNode(XElement element, string elementName)
    {
        var descendants = element.Descendants(XName.Get(elementName));
        if (!descendants.Any())
            return string.Empty;
        var first = descendants.First();
        return first.Value;
    }

    public static string? Attr(XElement element, string attributeName)
    {
        var xmlAttribute = element.Attribute(XName.Get(attributeName));
        if (xmlAttribute != null)
            return xmlAttribute.Value;
        return null;
    }

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

    public static XElement MakeAllElementsWithDefaultNs(XElement element)
    {
        var defaultNamespace = Namespaces[string.Empty];
        foreach (var descendant in element.DescendantsAndSelf())
            descendant.Name = XName.Get(descendant.Name.LocalName, defaultNamespace);
        var result = new XElement(XName.Get(element.Name.LocalName, defaultNamespace), element.Attributes(), element.Descendants());
        return result;
    }

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

    public static IList<XElement> GetElementsOfNameWithAttrContains(XElement element, string tagName, string attributeName, string attributeValue)
    {
        return GetElementsOfNameWithAttrWorker(element, tagName, attributeName, attributeValue);
    }

    public static void AddXmlNamespaces(XmlNamespaceManager namespaceManager)
    {
        foreach (string item in namespaceManager)
        {
            var namespaceValue = namespaceManager.LookupNamespace(item) ?? string.Empty;
            if (!Namespaces.ContainsKey(item))
                Namespaces.Add(item, namespaceValue);
        }
    }

    public static void AddXmlNamespaces(params string[] namespacePairs)
    {
        for (var i = 0; i < namespacePairs.Length; i++)
            Namespaces.Add(namespacePairs[i].Replace("xmlns:", ""), namespacePairs[++i]);
    }

    public static void AddXmlNamespaces(Dictionary<string, string> dictionary)
    {
        foreach (var item in dictionary)
            Namespaces.Add(item.Key, item.Value);
    }

    public static
        async Task<XDocument>
    CreateXDocument(string contentOrFilePath)
    {
        if (File.Exists(contentOrFilePath))
            contentOrFilePath =
                await FileAsync.ReadAllTextAsync(contentOrFilePath);
        var encodedBytes = Encoding.UTF8.GetBytes(contentOrFilePath).ToList();
        XDocument document;
        using (var memoryStream = new MemoryStream(encodedBytes.ToArray()))
        using (var xmlReader = XmlReader.Create(memoryStream))
        {
            document = XDocument.Load(xmlReader);
        }

        return document;
    }

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

    public static bool IsRightTag(XElement element, string tagName)
    {
        return IsRightTag(element.Name, tagName);
    }

    public static bool IsRightTag(XName xName, string tagName)
    {
        var (namespaceName, localName) = SH.GetPartsByLocationNoOut(tagName, ':');
        namespaceName = Namespaces[namespaceName];
        if (xName.LocalName == localName && xName.NamespaceName == namespaceName)
            return true;
        return false;
    }

    public static bool IsRightTag(XElement element, string localName, string namespaceName)
    {
        return IsRightTag(element.Name, localName, namespaceName);
    }

    public static bool IsRightTag(XName xName, string localName, string namespaceName)
    {
        if (xName.LocalName == localName && xName.NamespaceName == namespaceName)
            return true;
        return false;
    }

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

    public static string Minify(string text)
    {
        text = text.Replace(Environment.NewLine, string.Empty);
        text = SHReplace.ReplaceAllWhitecharsForSpace(text);
        text = SHReplace.ReplaceAllDoubleSpaceToSingle(text);
        text = text.Replace("> <", "><");
        return text;
    }
}
