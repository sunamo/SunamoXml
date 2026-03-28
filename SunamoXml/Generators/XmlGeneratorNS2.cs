namespace SunamoXml.Generators;

/// <summary>
/// Builds namespaced XML content. All generated tags are prefixed with the specified namespace.
/// </summary>
public class XmlGeneratorNS2
{
    private readonly string xmlNamespace;

    /// <summary>
    /// The underlying StringBuilder that holds the generated XML content.
    /// </summary>
    protected StringBuilder stringBuilder = new();

    /// <summary>
    /// Initializes a new instance with the specified namespace prefix.
    /// </summary>
    /// <param name="xmlNamespace">The namespace prefix to prepend to all tags.</param>
    public XmlGeneratorNS2(string xmlNamespace)
    {
        this.xmlNamespace = xmlNamespace;
    }

    /// <summary>
    /// Writes a CDATA section with the specified content.
    /// </summary>
    /// <param name="innerCData">The content to wrap in CDATA.</param>
    public void WriteCData(string innerCData)
    {
        WriteRaw($"<![CDATA[{innerCData}]]>");
    }

    /// <summary>
    /// Writes a complete element with the object's string representation as inner content. Skips if the value is null.
    /// </summary>
    /// <param name="elementName">The element name.</param>
    /// <param name="value">The object whose string representation becomes the inner content.</param>
    public void WriteElementObject(string elementName, object value)
    {
        if (value != null) WriteElement(elementName, value.ToString() ?? string.Empty);
    }

    /// <summary>
    /// Writes an opening tag with one attribute.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    /// <param name="attributeName">The attribute name.</param>
    /// <param name="attributeValue">The attribute value.</param>
    public void WriteTagWithAttr(string tagName, string attributeName, string attributeValue)
    {
        stringBuilder.AppendFormat("<" + xmlNamespace + "{0} {1}=\"{2}\">", tagName, attributeName, attributeValue);
    }

    /// <summary>
    /// Writes raw content directly to the output.
    /// </summary>
    /// <param name="rawContent">The raw content to append.</param>
    public void WriteRaw(string rawContent)
    {
        stringBuilder.Append(rawContent);
    }

    /// <summary>
    /// Writes a closing tag for the specified element.
    /// </summary>
    /// <param name="tagName">The element name to close.</param>
    public void TerminateTag(string tagName)
    {
        stringBuilder.AppendFormat("</" + xmlNamespace + "{0}>", tagName);
    }

    /// <summary>
    /// Writes an opening tag for the specified element.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    public void WriteTag(string tagName)
    {
        stringBuilder.AppendFormat("<" + xmlNamespace + "{0}>", tagName);
    }

    /// <summary>
    /// Returns the generated XML content as a string with normalized spacing.
    /// </summary>
    public override string ToString()
    {
        return stringBuilder.ToString().Replace("  />", " />");
    }

    /// <summary>
    /// Writes an opening tag with multiple attributes.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    /// <param name="attributes">Alternating attribute names and values.</param>
    public void WriteTagWithAttrs(string tagName, params string[] attributes)
    {
        stringBuilder.AppendFormat("<" + xmlNamespace + "{0} ", tagName);
        for (var i = 0; i < attributes.Length; i++) stringBuilder.AppendFormat("{0}=\"{1}\"", attributes[i], attributes[++i]);
        stringBuilder.Append(">");
    }

    /// <summary>
    /// Writes a complete element with the specified inner content.
    /// </summary>
    /// <param name="elementName">The element name.</param>
    /// <param name="inner">The inner content of the element.</param>
    public void WriteElement(string elementName, string inner)
    {
        stringBuilder.AppendFormat("<" + xmlNamespace + "{0}>{1}</" + xmlNamespace + "{0}>", elementName, inner);
    }

    /// <summary>
    /// Writes a complete element with the inner content wrapped in a CDATA section.
    /// </summary>
    /// <param name="elementName">The element name.</param>
    /// <param name="cdata">The content to wrap in CDATA.</param>
    public void WriteElementCData(string elementName, string cdata)
    {
        stringBuilder.AppendFormat("<" + xmlNamespace + "{0}><![CDATA[{1}]]></" + xmlNamespace + "{0}>", elementName, cdata);
    }

    /// <summary>
    /// Writes the standard XML declaration to the output.
    /// </summary>
    public void WriteXmlDeclaration()
    {
        stringBuilder.Append(XmlTemplates.Xml);
    }

    /// <summary>
    /// Writes an opening tag with two attributes.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    /// <param name="firstName">The first attribute name.</param>
    /// <param name="firstValue">The first attribute value.</param>
    /// <param name="secondName">The second attribute name.</param>
    /// <param name="secondValue">The second attribute value.</param>
    public void WriteTagWith2Attrs(string tagName, string firstName, string firstValue, string secondName, string secondValue)
    {
        stringBuilder.AppendFormat("<" + xmlNamespace + "{0} {1}=\"{2}\" {3}=\"{4}\">", tagName, firstName, firstValue, secondName, secondValue);
    }

    /// <summary>
    /// Creates a self-closing tag string with the specified namespace, tag name, and optional attributes.
    /// </summary>
    /// <param name="xmlNamespace">The namespace prefix.</param>
    /// <param name="tagName">The element name.</param>
    /// <param name="attributes">Alternating attribute names and values.</param>
    public static string WriteSimpleTagS(string xmlNamespace, string tagName, params string[] attributes)
    {
        var generator = new XmlGeneratorNS2(xmlNamespace);
        if (attributes.Length == 0)
            generator.WriteSimpleTag(tagName);
        else
            generator.WriteSimpleTag(tagName, attributes);
        return generator.ToString();
    }

    /// <summary>
    /// Writes a self-closing tag without attributes.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    public void WriteSimpleTag(string tagName)
    {
        stringBuilder.AppendFormat("<" + xmlNamespace + "{0} />", tagName);
    }

    /// <summary>
    /// Writes a self-closing tag with multiple attributes.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    /// <param name="attributes">Alternating attribute names and values.</param>
    public void WriteSimpleTag(string tagName, params string[] attributes)
    {
        stringBuilder.AppendFormat("<" + xmlNamespace + "{0} ", tagName);
        for (var i = 0; i < attributes.Length; i++) stringBuilder.AppendFormat("{0}=\"{1}\"", attributes[i], attributes[++i]);
        stringBuilder.Append(" />");
    }
}
