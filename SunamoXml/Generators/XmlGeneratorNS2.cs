namespace SunamoXml.Generators;

/// <summary>
///     Nasel jsem jeste tridu DotXml ale ta umoznuje vytvaret jen dokumenty ke bude root ThisApp.Name
///     A nebo moje vlastni XML tridy, ale ty umi vytvaret jen tridy bez rozsahlejesho xml vnoreni.
///     Element - prvek kteremu se zapisuje ihned i innerObsah. Muze byt i prazdne.
///     Tag - prvek kteremu to mohu zapsat pozdeji nebo vubec.
/// </summary>
public class XmlGeneratorNS2
{
    private readonly string _ns;
    protected StringBuilder stringBuilder = new();

    /// <param name="ns"></param>
    public XmlGeneratorNS2(string ns)
    {
        _ns = ns;
    }

    public void WriteCData(string innerCData)
    {
        WriteRaw($"<![CDATA[{innerCData}]]>");
    }

    public void WriteElementObject(string elementName, object value)
    {
        if (value != null) WriteElement(elementName, value.ToString());
    }

    public void WriteTagWithAttr(string tag, string attributeName, string attributeValue)
    {
        stringBuilder.AppendFormat("<" + _ns + "{0} {1}=\"{2}\">", tag, attributeName, attributeValue);
    }

    public void WriteRaw(string rawContent)
    {
        stringBuilder.Append(rawContent);
    }

    public void TerminateTag(string tagName)
    {
        stringBuilder.AppendFormat("</" + _ns + "{0}>", tagName);
    }

    public void WriteTag(string tagName)
    {
        stringBuilder.AppendFormat("<" + _ns + "{0}>", tagName);
    }

    public override string ToString()
    {
        return stringBuilder.ToString().Replace("  />", " />");
    }

    /// <summary>
    ///     NI
    /// </summary>
    /// <param name="tagName"></param>
    /// <param name="attributes"></param>
    public void WriteTagWithAttrs(string tagName, params string[] attributes)
    {
        stringBuilder.AppendFormat("<" + _ns + "{0} ", tagName);
        for (var i = 0; i < attributes.Length; i++) stringBuilder.AppendFormat("{0}=\"{1}\"", attributes[i], attributes[++i]);
        stringBuilder.Append(">");
    }

    public void WriteElement(string elementName, string inner)
    {
        stringBuilder.AppendFormat("<" + _ns + "{0}>{1}</" + _ns + "{0}>", elementName, inner);
    }

    public void WriteElementCData(string elementName, string cdata)
    {
        stringBuilder.AppendFormat("<" + _ns + "{0}><![CDATA[{1}]]></" + _ns + "{0}>", elementName, cdata);
    }

    public void WriteXmlDeclaration()
    {
        stringBuilder.Append(XmlTemplates.xml);
    }

    public void WriteTagWith2Attrs(string tagName, string firstName, string firstValue, string secondName, string secondValue)
    {
        //{0} {1}=\"{2}\" {3}=\"{4}\">
        stringBuilder.AppendFormat("<" + _ns + "{0} {1}=\"{2}\" {3}=\"{4}\">", tagName, firstName, firstValue, secondName, secondValue);
    }

    public static string WriteSimpleTagS(string ns, string tag, params string[] attributes)
    {
        var xValue = new XmlGeneratorNS2(ns);
        if (attributes.Length == 0)
            xValue.WriteSimpleTag(tag);
        else
            xValue.WriteSimpleTag(tag, attributes);
        return xValue.ToString();
    }

    public void WriteSimpleTag(string tag)
    {
        stringBuilder.AppendFormat("<" + _ns + "{0} />", tag);
    }

    public void WriteSimpleTag(string tag, params string[] attributes)
    {
        stringBuilder.AppendFormat("<" + _ns + "{0} ", tag);
        for (var i = 0; i < attributes.Length; i++) stringBuilder.AppendFormat("{0}=\"{1}\"", attributes[i], attributes[++i]);
        stringBuilder.Append(" />");
    }
}