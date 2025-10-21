// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy

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

    public void WriteElementObject(string p, object o)
    {
        if (o != null) WriteElement(p, o.ToString());
    }

    public void WriteTagWithAttr(string tag, string atribut, string hodnota)
    {
        stringBuilder.AppendFormat("<" + _ns + "{0} {1}=\"{2}\">", tag, atribut, hodnota);
    }

    public void WriteRaw(string p)
    {
        stringBuilder.Append(p);
    }

    public void TerminateTag(string p)
    {
        stringBuilder.AppendFormat("</" + _ns + "{0}>", p);
    }

    public void WriteTag(string p)
    {
        stringBuilder.AppendFormat("<" + _ns + "{0}>", p);
    }

    public override string ToString()
    {
        return stringBuilder.ToString().Replace("  />", " />");
    }

    /// <summary>
    ///     NI
    /// </summary>
    /// <param name="p"></param>
    /// <param name="p_2"></param>
    public void WriteTagWithAttrs(string p, params string[] p_2)
    {
        stringBuilder.AppendFormat("<" + _ns + "{0} ", p);
        for (var i = 0; i < p_2.Length; i++) stringBuilder.AppendFormat("{0}=\"{1}\"", p_2[i], p_2[++i]);
        stringBuilder.Append(">");
    }

    public void WriteElement(string nazev, string inner)
    {
        stringBuilder.AppendFormat("<" + _ns + "{0}>{1}</" + _ns + "{0}>", nazev, inner);
    }

    public void WriteElementCData(string nazev, string cdata)
    {
        stringBuilder.AppendFormat("<" + _ns + "{0}><![CDATA[{1}]]></" + _ns + "{0}>", nazev, cdata);
    }

    public void WriteXmlDeclaration()
    {
        stringBuilder.Append(XmlTemplates.xml);
    }

    public void WriteTagWith2Attrs(string p, string p_2, string p_3, string p_4, string p_5)
    {
        //{0} {1}=\"{2}\" {3}=\"{4}\">
        stringBuilder.AppendFormat("<" + _ns + "{0} {1}=\"{2}\" {3}=\"{4}\">", p, p_2, p_3, p_4, p_5);
    }

    public static string WriteSimpleTagS(string ns, string tag, params string[] p)
    {
        var xValue = new XmlGeneratorNS2(ns);
        if (p.Length == 0)
            xValue.WriteSimpleTag(tag);
        else
            xValue.WriteSimpleTag(tag, p);
        return xValue.ToString();
    }

    public void WriteSimpleTag(string tag)
    {
        stringBuilder.AppendFormat("<" + _ns + "{0} />", tag);
    }

    public void WriteSimpleTag(string tag, params string[] p_2)
    {
        stringBuilder.AppendFormat("<" + _ns + "{0} ", tag);
        for (var i = 0; i < p_2.Length; i++) stringBuilder.AppendFormat("{0}=\"{1}\"", p_2[i], p_2[++i]);
        stringBuilder.Append(" />");
    }
}