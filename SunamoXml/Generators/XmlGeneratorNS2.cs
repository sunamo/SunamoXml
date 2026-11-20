namespace SunamoXml.Generators;

/// <summary>
/// Represents the xml generator NS 2 type.
/// </summary>
public class XmlGeneratorNS2
{
    private readonly string xmlNamespace;

    /// <summary>
    /// The string builder.
    /// </summary>
    protected StringBuilder stringBuilder = new();

    /// <summary>
    /// Xml generator NS 2.
    /// </summary>
    /// <returns>The xml generator NS 2 result.</returns>
    public XmlGeneratorNS2(string xmlNamespace)
    {
        this.xmlNamespace = xmlNamespace;
    }

    /// <summary>
    /// Write c data.
    /// </summary>
    /// <param name="innerCData">The inner c data.</param>
    /// <returns>The write c data result.</returns>
    public void WriteCData(string innerCData)
    {
        WriteRaw($"<![CDATA[{innerCData}]]>");
    }

    /// <summary>
    /// Initializes a new instance of the write element object class.
    /// </summary>
    /// <param name="$"<![CDATA[{innerCData}]]>"">The CDATA inner c data.</param>
    public void WriteElementObject(string elementName, object value)
    {
        if (value != null) WriteElement(elementName, value.ToString() ?? string.Empty);
    }

    /// <summary>
    /// Initializes a new instance of the write tag with attr class.
    /// </summary>
    /// <param name="!">The !.</param>
    public void WriteTagWithAttr(string tagName, string attributeName, string attributeValue)
    {
        stringBuilder.AppendFormat("<" + xmlNamespace + "{0} {1}=\"{2}\">", tagName, attributeName, attributeValue);
    }

    /// <summary>
    /// Initializes a new instance of the write raw class.
    /// </summary>
    /// <param name="{1}">The 1.</param>
    /// <param name="tagName">The tag name.</param>
    /// <param name="attributeName">The attribute name.</param>
    /// <param name="attributeValue">The attribute value.</param>
    public void WriteRaw(string rawContent)
    {
        stringBuilder.Append(rawContent);
    }

    /// <summary>
    /// Initializes a new instance of the terminate tag class.
    /// </summary>
    /// <param name="rawContent">The raw content.</param>
    public void TerminateTag(string tagName)
    {
        stringBuilder.AppendFormat("</" + xmlNamespace + "{0}>", tagName);
    }

    /// <summary>
    /// Initializes a new instance of the write tag class.
    /// </summary>
    /// <param name=""{0}>"">The 0.</param>
    /// <param name="tagName">The tag name.</param>
    public void WriteTag(string tagName)
    {
        stringBuilder.AppendFormat("<" + xmlNamespace + "{0}>", tagName);
    }

    /// <summary>
    /// Initializes a new instance of the to string class.
    /// </summary>
    /// <param name=""{0}>"">The 0.</param>
    /// <param name="tagName">The tag name.</param>
    public override string ToString()
    {
        return stringBuilder.ToString().Replace("  />", " />");
    }

    /// <summary>
    /// Write tag with attrs.
    /// </summary>
    /// <returns>The write tag with attrs result.</returns>
    public void WriteTagWithAttrs(string tagName, params string[] attributes)
    {
        stringBuilder.AppendFormat("<" + xmlNamespace + "{0} ", tagName);
        for (var i = 0; i < attributes.Length; i++) stringBuilder.AppendFormat("{0}=\"{1}\"", attributes[i], attributes[++i]);
        stringBuilder.Append(">");
    }

    /// <summary>
    /// Initializes a new instance of the write element class.
    /// </summary>
    /// <param name="">"">The "&gt;".</param>
    public void WriteElement(string elementName, string inner)
    {
        stringBuilder.AppendFormat("<" + xmlNamespace + "{0}>{1}</" + xmlNamespace + "{0}>", elementName, inner);
    }

    /// <summary>
    /// Initializes a new instance of the write element c data class.
    /// </summary>
    /// <param name=""{0}>"">The 0.</param>
    /// <param name="elementName">The element name.</param>
    /// <param name="inner">The inner.</param>
    public void WriteElementCData(string elementName, string cdata)
    {
        stringBuilder.AppendFormat("<" + xmlNamespace + "{0}><![CDATA[{1}]]></" + xmlNamespace + "{0}>", elementName, cdata);
    }

    /// <summary>
    /// Initializes a new instance of the write xml declaration class.
    /// </summary>
    /// <param name=""{0}>"">The 0.</param>
    /// <param name="elementName">The element name.</param>
    /// <param name="cdata">The cdata.</param>
    public void WriteXmlDeclaration()
    {
        stringBuilder.Append(XmlTemplates.Xml);
    }

    /// <summary>
    /// Initializes a new instance of the write tag with 2 attrs class.
    /// </summary>
    /// <param name="XmlTemplates.Xml">The xml templates xml.</param>
    public void WriteTagWith2Attrs(string tagName, string firstName, string firstValue, string secondName, string secondValue)
    {
        stringBuilder.AppendFormat("<" + xmlNamespace + "{0} {1}=\"{2}\" {3}=\"{4}\">", tagName, firstName, firstValue, secondName, secondValue);
    }

    /// <summary>
    /// Initializes a new instance of the write simple tag s class.
    /// </summary>
    /// <param name="{1}">The 1.</param>
    /// <param name="tagName">The tag name.</param>
    /// <param name="firstName">The first name.</param>
    /// <param name="firstValue">The first value.</param>
    /// <param name="secondName">The second name.</param>
    /// <param name="secondValue">The second value.</param>
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
    /// Write simple tag.
    /// </summary>
    /// <returns>The write simple tag result.</returns>
    public void WriteSimpleTag(string tagName)
    {
        stringBuilder.AppendFormat("<" + xmlNamespace + "{0} />", tagName);
    }

    /// <summary>
    /// Initializes a new instance of the write simple tag class.
    /// </summary>
    /// <param name="/>"">The /&gt;".</param>
    /// <param name="tagName">The tag name.</param>
    public void WriteSimpleTag(string tagName, params string[] attributes)
    {
        stringBuilder.AppendFormat("<" + xmlNamespace + "{0} ", tagName);
        for (var i = 0; i < attributes.Length; i++) stringBuilder.AppendFormat("{0}=\"{1}\"", attributes[i], attributes[++i]);
        stringBuilder.Append(" />");
    }
}
