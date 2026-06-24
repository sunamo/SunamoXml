namespace SunamoXml;

public class XH
{
    public static XmlDocument XmlDocument { get; set; } = new();

    public static
        async Task
        AddXmlns(string csprojPath, XNamespace xmlNamespace, bool isAdding)
    {
        if (isAdding)
        {
            var document =
                XDocument.Load(csprojPath);
            AddNamespace(xmlNamespace, document);
            document.Save(csprojPath);
        }
        else
        {
            var text =
                await FileAsync.ReadAllTextAsync(csprojPath);
            text = RemoveNamespace(xmlNamespace, text);
            await FileAsync.WriteAllTextAsync(csprojPath, text);
        }
    }

    private static void AddNamespace(XNamespace xmlNamespace, XDocument document)
    {
        foreach (var element in document.Descendants().ToList()) element.Name = xmlNamespace + element.Name.LocalName;
        document.Root?.SetAttributeValue("xmlns", xmlNamespace.ToString());
    }

    private static string RemoveNamespace(XNamespace xmlNamespace, string text)
    {
        var xmlnsDeclaration = "xmlns=\"" + xmlNamespace + "\"";
        text = SH.ReplaceOnce(text, xmlnsDeclaration, string.Empty);
        return text;
    }

    public static string AddXmlnsContent(string content, XNamespace xmlNamespace, bool isAdding)
    {
        if (isAdding)
        {
            var document = XDocument.Parse(content);
            AddNamespace(xmlNamespace, document);
            return OuterXml(document);
        }

        return RemoveNamespace(xmlNamespace, content);
    }

    private static string OuterXml(XDocument document)
    {
        var stringBuilder = new StringBuilder();
        var xmlWriter = XmlWriter.Create(stringBuilder);
        document.Document?.WriteTo(xmlWriter);
        return stringBuilder.ToString();
    }

    public static string InnerXml(string xml)
    {
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xml);
        return xmlDocument.DocumentElement?.InnerXml ?? string.Empty;
    }

    public static string ReplaceSpecialHtmlEntity(string text)
    {
        text = text.Replace("&rsquo;", "'");
        text = text.Replace("&lsquo;", "'");
        return text;
    }

    public static string ReplaceAmpInString(string xml)
    {
        var badAmpersand = new Regex("&(?![a-zA-Z]{2,6};|#[0-9]{2,4};)");
        const string goodAmpersand = "&amp;";
        return badAmpersand.Replace(xml, goodAmpersand);
    }

    public static XmlNode? ReturnXmlRoot(string xml)
    {
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xml);
        return xmlDocument.LastChild;
    }

    public static XmlNode? ReturnXmlNode(string xml)
    {
        var xmlDocument = new XmlDocument();
        xmlDocument.PreserveWhitespace = true;
        xmlDocument.LoadXml(xml);
        return xmlDocument.FirstChild;
    }

    public static string SanitizeXmlString(string xml)
    {
        if (xml == null) { ThrowEx.IsNull("xml"); return string.Empty; }
        var buffer = new StringBuilder(xml.Length);
        foreach (var character in xml)
            if (IsLegalXmlChar(character))
                buffer.Append(character);
        return buffer.ToString();
    }

    private static bool IsLegalXmlChar(int character)
    {
        return
            character == 0x9 ||
            character == 0xA ||
            character == 0xD ||
            (character >= 0x20 && character <= 0xD7FF) ||
            (character >= 0xE000 && character <= 0xFFFD) ||
            (character >= 0x10000 && character <= 0x10FFFF)
            ;
    }

    public static
        async Task<XmlDocument?>
        LoadXml(string xml)
    {
        if (File.Exists(xml))
            xml =
                await FileAsync.ReadAllTextAsync(xml);
        var xmlDocument = new XmlDocument();
        try
        {
            xmlDocument.LoadXml(xml);
        }
        catch (Exception exception)
        {
            ThrowEx.CustomWithStackTrace(exception);
            return null;
        }

        return xmlDocument;
    }

    public static string RemoveXmlDeclaration(string text)
    {
        text = Regex.Replace(text, @"<\?xml.*?\?>", "");
        text = Regex.Replace(text, @"<\?xml.*?\>", "");
        text = Regex.Replace(text, @"<\?xml.*?\/>", "");
        return text;
    }
}
