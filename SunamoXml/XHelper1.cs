// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoXml;
public partial class XHelper
{
    public static Dictionary<string, string> XmlNamespaces(XmlNamespaceManager nsmgr, bool withPrexixedXmlnsColon)
    {
        var ns = new Dictionary<string, string>();
        foreach (string item2 in nsmgr)
        {
            var item = item2;
            if (withPrexixedXmlnsColon)
            {
                if (item == string.Empty || item == "xmlns")
                    item = "xmlns";
                else
                    item = "xmlns:" + item;
            }

            // Jak� je typ item, at nemus�m pou��vat slovn�k
            var value = nsmgr.LookupNamespace(item2);
            if (!ns.ContainsKey(item))
                ns.Add(item, value);
        }

        return ns;
    }

    /// <summary>
    ///     If A1 is file, output will be save to file and return null
    ///     Otherwise return string
    /// </summary>
    /// <param name = "pathOrContent"></param>
    public static 
#if ASYNC
        async Task<string>
#else
    string 
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
        var h = new XmlNamespacesHolder();
        var doc = h.ParseAndRemoveNamespacesXDocument(xmlFormat);
        var formatted = doc.ToString();
        formatted = formatted.Replace(" xmlns=\"\"", string.Empty);
        //HReplace.ReplaceAll2(formatted, string.Empty, " xmlns=\"\"");
        if (File.Exists(pathOrContent))
        {
#if ASYNC
            await
#endif
            File.WriteAllTextAsync(pathOrContent, formatted);
            //ThisApp.Success(Translate.FromKey(XlfKeys.ChangesSavedToFile));
            return null;
        }

        //ThisApp.Success(Translate.FromKey(XlfKeys.ChangesSavedToClipboard));
        return formatted;
    }

    public static string FormatXmlInMemory(string xml)
    {
        try
        {
            var doc = XDocument.Parse(xml);
            return doc.ToString();
        }
        catch (Exception)
        {
            // Handle and throw if fatal exception here; don't just ignore them
            return xml;
        }
    }

    public static string GetInnerXml(XElement parent)
    {
        var reader = parent.CreateReader();
        reader.MoveToContent();
        return reader.ReadInnerXml();
    }

    public static List<XElement> GetElementsOfNameWithAttr(XElement xElement, string tag, string attr, string value /*,
        bool caseSensitive*/)
    {
        return GetElementsOfNameWithAttrWorker(xElement, tag, attr, value /*, false, caseSensitive*/);
    }

    public static List<XElement> GetElementsOfNameWithAttrWorker(XElement xElement, string tag, string attr, string value /*, bool enoughIsContainsAttribute, bool caseSensitive*/)
    {
        var vr = new List<XElement>();
        var element = GetElementsOfNameRecursive(xElement, tag);
        foreach (var item in element)
        {
            var attrValue = Attr(item, attr);
            if (attrValue.Contains(value) /*SH.ContainsBoolBool(attrValue, value, enoughIsContainsAttribute, caseSensitive)*/)
                vr.Add(item);
        }

        return vr;
    }

    /// <param name = "item"></param>
    /// <param name = "p"></param>
    public static XElement GetElementOfNameRecursive(XElement node, string nazev)
    {
        //bool ns = true;
        if (nazev.Contains(":"))
        {
            var(p, z) = SH.GetPartsByLocationNoOut(nazev, ':');
            p = ns[p];
            foreach (var item in node.DescendantsAndSelf())
                if (item.Name.LocalName == z && item.Name.NamespaceName == p)
                    return item;
        }
        else
        {
            foreach (var item in node.DescendantsAndSelf())
                if (item.Name.LocalName == nazev)
                    return item;
        }

        return null;
    }

    /// <summary>
    ///     Is usage only in _Uap/SocialNetworksManager -> open for find out how looks input data and then move to RegexHelper
    /// </summary>
    /// <param name = "p"></param>
    /// <param name = "deli"></param>
    public static string ReturnValueAllSubElementsSeparatedBy(XElement p, string deli)
    {
        var stringBuilder = new StringBuilder();
        var xml = GetXml(p);
        var mc = Regex.Matches(xml, "<(?:\"[^\"]*\"['\"]*|'[^']*'['\"]*|[^'\">])+>");
        var nahrazeno = new List<string>();
        foreach (Match item in mc)
            if (!nahrazeno.Contains(item.Value))
            {
                nahrazeno.Add(item.Value);
                xml = xml.Replace(item.Value, deli);
            }

        stringBuilder.Append(xml);
        return stringBuilder.ToString().Replace(deli + deli, deli);
    }

    public static string GetXml(XElement node)
    {
        var sw = new StringWriter();
        var xtw = XmlWriter.Create(sw);
        node.WriteTo(xtw);
        return sw.ToString();
    }

    public static XElement GetElementOfSecondLevel(XElement var, string first, string second)
    {
        var f = var.Element(XName.Get(first));
        if (f != null)
        {
            var text = f.Element(XName.Get(second));
            return text;
        }

        return null;
    }

    public static string GetValueOfElementOfSecondLevelOrSE(XElement var, string first, string second)
    {
        var xe = GetElementOfSecondLevel(var, first, second);
        if (xe != null)
            return xe.Value.Trim();
        return "";
    }

    /// <param name = "var"></param>
    /// <param name = "p"></param>
    public static string GetValueOfElementOfNameOrSE(XElement var, string nazev)
    {
        var xe = GetElementOfName(var, nazev);
        if (xe == null)
            return "";
        return xe.Value.Trim();
    }
}