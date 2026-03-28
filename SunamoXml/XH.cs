namespace SunamoXml;

/// <summary>
/// XML helper class operating on <see cref="XmlDocument"/> and <see cref="XDocument"/> for loading, sanitizing, and manipulating XML content.
/// </summary>
public class XH
{
    /// <summary>
    /// Shared XmlDocument instance for general-purpose XML operations.
    /// </summary>
    public static XmlDocument XmlDocument { get; set; } = new();

    /// <summary>
    /// Adds or removes an XML namespace from a .csproj or XML file.
    /// </summary>
    /// <param name="csprojPath">The path to the file to modify.</param>
    /// <param name="xmlNamespace">The namespace to add or remove.</param>
    /// <param name="isAdding">True to add the namespace, false to remove it.</param>
    public static
#if ASYNC
        async Task
#else
void
#endif
        AddXmlns(string csprojPath, XNamespace xmlNamespace, bool isAdding)
    {
        if (isAdding)
        {
            var document =
#if ASYNC
                XDocument.Load(csprojPath);
#else
XDocument.Load(csprojPath);
#endif
            AddNamespace(xmlNamespace, document);
            document.Save(csprojPath);
        }
        else
        {
            var text =
#if ASYNC
                await
#endif
                    File.ReadAllTextAsync(csprojPath);
            text = RemoveNamespace(xmlNamespace, text);
#if ASYNC
            await
#endif
                File.WriteAllTextAsync(csprojPath, text);
        }
    }

    /// <summary>
    /// Adds the specified namespace to all elements and sets it as the root xmlns attribute.
    /// </summary>
    /// <param name="xmlNamespace">The namespace to add.</param>
    /// <param name="document">The XDocument to modify.</param>
    private static void AddNamespace(XNamespace xmlNamespace, XDocument document)
    {
        foreach (var element in document.Descendants().ToList()) element.Name = xmlNamespace + element.Name.LocalName;
        document.Root?.SetAttributeValue("xmlns", xmlNamespace.ToString());
    }

    /// <summary>
    /// Removes the xmlns declaration for the specified namespace from the text.
    /// </summary>
    /// <param name="xmlNamespace">The namespace to remove.</param>
    /// <param name="text">The XML text to process.</param>
    private static string RemoveNamespace(XNamespace xmlNamespace, string text)
    {
        var xmlnsDeclaration = "xmlns=\"" + xmlNamespace + "\"";
        text = SH.ReplaceOnce(text, xmlnsDeclaration, string.Empty);
        return text;
    }

    /// <summary>
    /// Adds or removes an XML namespace from an in-memory XML string.
    /// </summary>
    /// <param name="content">The XML content string.</param>
    /// <param name="xmlNamespace">The namespace to add or remove.</param>
    /// <param name="isAdding">True to add the namespace, false to remove it.</param>
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

    /// <summary>
    /// Returns the outer XML representation of an XDocument.
    /// </summary>
    /// <param name="document">The XDocument to serialize.</param>
    private static string OuterXml(XDocument document)
    {
        var stringBuilder = new StringBuilder();
        var xmlWriter = XmlWriter.Create(stringBuilder);
        document.Document?.WriteTo(xmlWriter);
        return stringBuilder.ToString();
    }

    /// <summary>
    /// Returns the inner XML of the document element parsed from the specified XML string.
    /// </summary>
    /// <param name="xml">The XML string to parse.</param>
    public static string InnerXml(string xml)
    {
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xml);
        return xmlDocument.DocumentElement?.InnerXml ?? string.Empty;
    }

    /// <summary>
    /// Replaces special HTML entity references (right/left single quotation marks) with their literal equivalents.
    /// </summary>
    /// <param name="text">The text containing HTML entities to replace.</param>
    public static string ReplaceSpecialHtmlEntity(string text)
    {
        text = text.Replace("&rsquo;", "'");
        text = text.Replace("&lsquo;", "'");
        return text;
    }

    /// <summary>
    /// Replaces standalone ampersands (not part of valid XML entities) with the XML-safe &amp;amp; entity.
    /// </summary>
    /// <param name="xml">The XML string to process.</param>
    public static string ReplaceAmpInString(string xml)
    {
        var badAmpersand = new Regex("&(?![a-zA-Z]{2,6};|#[0-9]{2,4};)");
        const string goodAmpersand = "&amp;";
        return badAmpersand.Replace(xml, goodAmpersand);
    }

    /// <summary>
    /// Parses the XML string and returns the root node (last child of the document).
    /// </summary>
    /// <param name="xml">The XML string to parse.</param>
    public static XmlNode? ReturnXmlRoot(string xml)
    {
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xml);
        return xmlDocument.LastChild;
    }

    /// <summary>
    /// Parses the XML string preserving whitespace and returns the first child node (typically the XML declaration).
    /// </summary>
    /// <param name="xml">The XML string to parse.</param>
    public static XmlNode? ReturnXmlNode(string xml)
    {
        var xmlDocument = new XmlDocument();
        xmlDocument.PreserveWhitespace = true;
        xmlDocument.LoadXml(xml);
        return xmlDocument.FirstChild;
    }

    /// <summary>
    /// Removes illegal XML characters from a string, keeping only characters allowed by XML 1.0.
    /// </summary>
    /// <param name="xml">The XML string to sanitize.</param>
    public static string SanitizeXmlString(string xml)
    {
        if (xml == null) { ThrowEx.IsNull("xml"); return string.Empty; }
        var buffer = new StringBuilder(xml.Length);
        foreach (var character in xml)
            if (IsLegalXmlChar(character))
                buffer.Append(character);
        return buffer.ToString();
    }

    /// <summary>
    /// Determines whether a given character is allowed by XML 1.0 specification.
    /// </summary>
    /// <param name="character">The Unicode code point to check.</param>
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

    /// <summary>
    /// Loads XML from a string or file path and returns the parsed XmlDocument. Returns null on parse failure.
    /// </summary>
    /// <param name="xml">The XML content string or file path.</param>
    public static
#if ASYNC
        async Task<XmlDocument?>
#else
XmlDocument?
#endif
        LoadXml(string xml)
    {
        if (File.Exists(xml))
            xml =
#if ASYNC
                await
#endif
                    File.ReadAllTextAsync(xml);
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

    /// <summary>
    /// Removes all forms of XML declaration from the input text.
    /// </summary>
    /// <param name="text">The text containing XML declarations to remove.</param>
    public static string RemoveXmlDeclaration(string text)
    {
        text = Regex.Replace(text, @"<\?xml.*?\?>", "");
        text = Regex.Replace(text, @"<\?xml.*?\>", "");
        text = Regex.Replace(text, @"<\?xml.*?\/>", "");
        return text;
    }
}
