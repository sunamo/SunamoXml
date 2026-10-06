namespace SunamoXml.Generators;

public class XmlGeneratorNS2
{
    private readonly string xmlNamespace;

    protected StringBuilder stringBuilder = new();

    public XmlGeneratorNS2(string xmlNamespace)
    {
        this.xmlNamespace = xmlNamespace;
    }

    public void WriteCData(string innerCData)
    {
        WriteRaw($"<![CDATA[{innerCData}]]>");
    }

    public void WriteElementObject(string elementName, object value)
    {
        if (value != null) WriteElement(elementName, value.ToString() ?? string.Empty);
    }

    public void WriteTagWithAttr(string tagName, string attributeName, string attributeValue)
    {
        stringBuilder.AppendFormat("<" + xmlNamespace + "{0} {1}=\"{2}\">", tagName, attributeName, attributeValue);
    }

    public void WriteRaw(string rawContent)
    {
        stringBuilder.Append(rawContent);
    }

    public void TerminateTag(string tagName)
    {
        stringBuilder.AppendFormat("</" + xmlNamespace + "{0}>", tagName);
    }

    public void WriteTag(string tagName)
    {
        stringBuilder.AppendFormat("<" + xmlNamespace + "{0}>", tagName);
    }

    public override string ToString()
    {
        return stringBuilder.ToString().Replace("  />", " />");
    }

    public void WriteTagWithAttrs(string tagName, params string[] attributes)
    {
        stringBuilder.AppendFormat("<" + xmlNamespace + "{0} ", tagName);
        for (var i = 0; i < attributes.Length; i++) stringBuilder.AppendFormat("{0}=\"{1}\"", attributes[i], attributes[++i]);
        stringBuilder.Append(">");
    }

    public void WriteElement(string elementName, string inner)
    {
        stringBuilder.AppendFormat("<" + xmlNamespace + "{0}>{1}</" + xmlNamespace + "{0}>", elementName, inner);
    }

    public void WriteElementCData(string elementName, string cdata)
    {
        stringBuilder.AppendFormat("<" + xmlNamespace + "{0}><![CDATA[{1}]]></" + xmlNamespace + "{0}>", elementName, cdata);
    }

    public void WriteXmlDeclaration()
    {
        stringBuilder.Append(XmlTemplates.Xml);
    }

    public void WriteTagWith2Attrs(string tagName, string firstName, string firstValue, string secondName, string secondValue)
    {
        stringBuilder.AppendFormat("<" + xmlNamespace + "{0} {1}=\"{2}\" {3}=\"{4}\">", tagName, firstName, firstValue, secondName, secondValue);
    }

    public static string WriteSimpleTagS(string xmlNamespace, string tagName, params string[] attributes)
    {
        var generator = new XmlGeneratorNS2(xmlNamespace);
        if (attributes.Length == 0)
            generator.WriteSimpleTag(tagName);
        else
            generator.WriteSimpleTag(tagName, attributes);
        return generator.ToString();
    }

    public void WriteSimpleTag(string tagName)
    {
        stringBuilder.AppendFormat("<" + xmlNamespace + "{0} />", tagName);
    }

    public void WriteSimpleTag(string tagName, params string[] attributes)
    {
        stringBuilder.AppendFormat("<" + xmlNamespace + "{0} ", tagName);
        for (var i = 0; i < attributes.Length; i++) stringBuilder.AppendFormat("{0}=\"{1}\"", attributes[i], attributes[++i]);
        stringBuilder.Append(" />");
    }
}
