
namespace SunamoXml;
using System.Xml;
using System.Xml.Linq;

/// <summary>
/// XH = XmlElement
/// XHelper = XElement
/// </summary>
public partial class XHelper
{
    public static Dictionary<string, string> XmlNamespaces(XmlNamespaceManager nsmgr, bool withPrexixedXmlnsColon)
    {
        Dictionary<string, string> ns = new Dictionary<string, string>();
        foreach (string item2 in nsmgr)
        {
            var item = item2;

            if (withPrexixedXmlnsColon)
            {
                if (item == string.Empty || item == Consts.xmlns)
                {
                    item = Consts.xmlns;
                }
                else
                {
                    item = "xmlns:" + item;
                }

            }

            // Jak� je typ item, at nemus�m pou��vat slovn�k
            var v = nsmgr.LookupNamespace(item2);

            if (!ns.ContainsKey(item))
            {
                ns.Add(item, v);
            }
        }

        return ns;
    }

    /// <summary>
    /// If A1 is file, output will be save to file and return null
    /// Otherwise return string
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
        {
            xmlFormat =
#if ASYNC
            await
#endif
            TFSE.ReadAllText(pathOrContent);
        }
        XmlNamespacesHolder h = new XmlNamespacesHolder();
        XDocument doc = h.ParseAndRemoveNamespacesXDocument(xmlFormat);
        var formatted = doc.ToString();
        formatted = formatted.Replace(" xmlns=\"\"", string.Empty);
        //HReplace.ReplaceAll2(formatted, string.Empty, " xmlns=\"\"");
        if (File.Exists(pathOrContent))
        {
#if ASYNC
            await
#endif
            TFSE.WriteAllText(pathOrContent, formatted);
            //ThisApp.Success(sess.i18n(XlfKeys.ChangesSavedToFile));
            return null;
        }
        else
        {
            //ThisApp.Success(sess.i18n(XlfKeys.ChangesSavedToClipboard));
            return formatted;
        }
    }
    public static string FormatXmlInMemory(string xml)
    {
        try
        {
            XDocument doc = XDocument.Parse(xml);
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
    public static List<XElement> GetElementsOfNameWithAttr(System.Xml.Linq.XElement xElement, string tag, string attr, string value, bool caseSensitive)
    {
        return GetElementsOfNameWithAttrWorker(xElement, tag, attr, value, false, caseSensitive);
    }

    public static List<XElement> GetElementsOfNameWithAttrWorker(System.Xml.Linq.XElement xElement, string tag, string attr, string value, bool enoughIsContainsAttribute, bool caseSensitive)
    {
        List<XElement> vr = new List<XElement>();
        List<XElement> e = XHelper.GetElementsOfNameRecursive(xElement, tag);
        foreach (XElement item in e)
        {
            var attrValue = XHelper.Attr(item, attr);
            if (attrValue.Contains(value) /*SH.ContainsBoolBool(attrValue, value, enoughIsContainsAttribute, caseSensitive)*/)
            {
                vr.Add(item);
            }
        }

        return vr;
    }

    /// <param name = "item"></param>
    /// <param name = "p"></param>
    public static XElement GetElementOfNameRecursive(XElement node, string nazev)
    {
        //bool ns = true;
        if (nazev.Contains(AllStrings.colon))
        {
            var (p, z) = SH.GetPartsByLocationNoOut(nazev, AllChars.colon);
            p = XHelper.ns[p];
            foreach (XElement item in node.DescendantsAndSelf())
            {
                if (item.Name.LocalName == z && item.Name.NamespaceName == p)
                {
                    return item;
                }
            }
        }
        else
        {
            foreach (XElement item in node.DescendantsAndSelf())
            {
                if (item.Name.LocalName == nazev)
                {
                    return item;
                }
            }
        }
        return null;
    }
    /// <summary>
    /// Is usage only in _Uap/SocialNetworksManager -> open for find out how looks input data and then move to RegexHelper
    /// </summary>
    /// <param name = "p"></param>
    /// <param name = "deli"></param>
    public static string ReturnValueAllSubElementsSeparatedBy(XElement p, string deli)
    {
        StringBuilder sb = new StringBuilder();
        string xml = GetXml(p);
        MatchCollection mc = Regex.Matches(xml, "<(?:\"[^\"]*\"['\"]*|'[^']*'['\"]*|[^'\">])+>");
        List<string> nahrazeno = new List<string>();
        foreach (Match item in mc)
        {
            if (!nahrazeno.Contains(item.Value))
            {
                nahrazeno.Add(item.Value);
                xml = xml.Replace(item.Value, deli);
            }
        }
        sb.Append(xml);
        return sb.ToString().Replace(deli + deli, deli);
    }
    public static string GetXml(XElement node)
    {
        StringWriter sw = new StringWriter();
        XmlWriter xtw = XmlWriter.Create(sw);
        node.WriteTo(xtw);
        return sw.ToString();
    }
    public static XElement GetElementOfSecondLevel(XElement var, string first, string second)
    {
        XElement f = var.Element(XName.Get(first));
        if (f != null)
        {
            XElement s = f.Element(XName.Get(second));
            return s;
        }
        return null;
    }
    public static string GetValueOfElementOfSecondLevelOrSE(XElement var, string first, string second)
    {
        XElement xe = GetElementOfSecondLevel(var, first, second);
        if (xe != null)
        {
            return xe.Value.Trim();
        }
        return "";
    }
    /// <param name = "var"></param>
    /// <param name = "p"></param>
    public static string GetValueOfElementOfNameOrSE(XElement var, string nazev)
    {
        XElement xe = GetElementOfName(var, nazev);
        if (xe == null)
        {
            return "";
        }
        return xe.Value.Trim();
    }
}
