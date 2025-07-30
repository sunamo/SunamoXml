namespace SunamoXml;

public class XHelper
{
    public static Dictionary<string, string> ns = new();


    public static string InnerTextOfNode(XElement xe, string v)
    {
        var desc = xe.Descendants(XName.Get(v));
        if (desc.Count() == 0) return string.Empty;
        var first = desc.First();
        return first.Value;
    }

    /// <summary>
    ///     Při nenalezení vrací null
    /// </summary>
    /// <param name="item"></param>
    /// <param name="attr"></param>
    public static string Attr(XElement item, string attr)
    {
        var xa = item.Attribute(XName.Get(attr));
        if (xa != null) return xa.Value;

        return null;
    }

    public static XElement GetElementOfNameWithAttr(XElement node, string nazev, string attr, string value)
    {
        if (nazev.Contains(":"))
        {
            var (p, z) = SH.GetPartsByLocationNoOut(nazev, ':');
            p = ns[p];
            foreach (var item in node.Elements())
                if (item.Name.LocalName == z && item.Name.NamespaceName == p)
                    if (Attr(item, attr) == value)
                        return item;
        }
        else
        {
            foreach (var item in node.DescendantsAndSelf())
                if (item.Name.LocalName == nazev)
                    if (Attr(item, attr) == value)
                        return item;
        }

        return null;
    }

    public static XElement MakeAllElementsWithDefaultNs(XElement settings)
    {
        var ns2 = ns[string.Empty];
        var toInsert = new List<object>();
        // shift ALL elements in the settings document into the target namespace
        foreach (var e in settings.DescendantsAndSelf())
            //e.Name =  e.Name.LocalName;
            e.Name = XName.Get(e.Name.LocalName, ns2);

        //foreach (var e in settings.Attributes())
        //{
        //    //e.Name = XName.Get(e.Name.LocalName, ns2);
        //    toInsert.Add(e);
        //}
        //t
        var vr = new XElement(XName.Get(settings.Name.LocalName, ns2), settings.Attributes(), settings.Descendants());
        return vr;
    }


    public static List<XElement> GetElementsOfName(XElement node, string nazev)
    {
        var result = new List<XElement>();
        if (nazev.Contains(":"))
        {
            foreach (var item in node.Elements())
                if (IsRightTag(item, nazev))
                    result.Add(item);
        }
        else
        {
            foreach (var item in node.Elements())
                if (item.Name.LocalName == nazev)
                    result.Add(item);
        }

        return result;
    }

    public static IList<XElement> GetElementsOfNameWithAttrContains(XElement group, string tag, string attr,
        string value/*, bool caseSensitive = false*/)
    {
        return GetElementsOfNameWithAttrWorker(group, tag, attr, value/*, true, caseSensitive*/);
    }

    public static void AddXmlNamespaces(XmlNamespaceManager nsmgr)
    {
        foreach (string item in nsmgr)
        {
            // Jaký je typ item, at nemusím používat slovník
            var v = nsmgr.LookupNamespace(item);
            if (!ns.ContainsKey(item)) ns.Add(item, v);
        }
    }

    /// <param name="p"></param>
    public static void AddXmlNamespaces(params string[] p)
    {
        for (var i = 0; i < p.Length; i++)
            //.TrimEnd('/') + "/"
            ns.Add(p[i].Replace("xmlns:", ""), p[++i]);
    }

    public static void AddXmlNamespaces(Dictionary<string, string> d)
    {
        foreach (var item in d) ns.Add(item.Key, item.Value);
    }

    public static
#if ASYNC
        async Task<XDocument>
#else
XDocument
#endif
        CreateXDocument(string contentOrFn)
    {
        if (File.Exists(contentOrFn))
            contentOrFn =
#if ASYNC
                await
#endif
                    File.ReadAllTextAsync(contentOrFn);

        var enB = Encoding.UTF8.GetBytes(contentOrFn).ToList();
        XDocument xd = null;
        using (var oStream = new MemoryStream(enB.ToArray()))
        using (var oReader = XmlReader.Create(oStream))
        {
            xd = XDocument.Load(oReader);
        }

        return xd;
    }

    /// <summary>
    ///     Získá element jména A2 v A1.
    ///     Umí pracovat v NS, stačí zadat zkratku namepsace jako ns:tab
    /// </summary>
    /// <param name="node"></param>
    /// <param name="nazev"></param>
    public static XElement GetElementOfName(XContainer node, string nazev)
    {
        if (nazev.Contains(":"))
        {
            var (p, z) = SH.GetPartsByLocationNoOut(nazev, ':');
            p = ns[p];
            foreach (var item in node.Elements())
            {
                if (IsRightTag(item, z, p))
                {
                }

                if (item.Name.LocalName == z && item.Name.NamespaceName == p) return item;
            }
        }
        else
        {
            foreach (var item in node.Elements())
                if (item.Name.LocalName == nazev)
                    return item;
        }

        return null;
    }

    public static bool IsRightTag(XElement xName, string nazev)
    {
        return IsRightTag(xName.Name, nazev);
    }

    /// <summary>
    ///     Will split A2 to LocalName and NamespaceName
    /// </summary>
    /// <param name="xName"></param>
    /// <param name="nazev"></param>
    public static bool IsRightTag(XName xName, string nazev)
    {
        var (p, z) = SH.GetPartsByLocationNoOut(nazev, ':');
        p = ns[p];
        if (xName.LocalName == z && xName.NamespaceName == p) return true;

        return false;
    }

    public static bool IsRightTag(XElement xName, string localName, string namespaceName)
    {
        return IsRightTag(xName.Name, localName, namespaceName);
    }

    /// <summary>
    ///     Into A3 is passing shortcut
    /// </summary>
    /// <param name="xName"></param>
    /// <param name="localName"></param>
    /// <param name="namespaceName"></param>
    public static bool IsRightTag(XName xName, string localName, string namespaceName)
    {
        if (xName.LocalName == localName && xName.NamespaceName == namespaceName) return true;

        return false;
    }



    public static List<XElement> GetElementsOfNameRecursive(XElement node, string nazev)
    {
        var vr = new List<XElement>();

        if (nazev.Contains(":"))
        {
            var (p, z) = SH.GetPartsByLocationNoOut(nazev, ':');
            p = ns[p];
            foreach (var item in node.DescendantsAndSelf())
                if (item.Name.LocalName == z && item.Name.NamespaceName == p)
                    vr.Add(item);
        }
        else
        {
            foreach (var item in node.DescendantsAndSelf())
                if (item.Name.LocalName == nazev)
                    vr.Add(item);
        }

        return vr;
    }

    public static string Minify(string c)
    {
        c = c.Replace(Environment.NewLine, string.Empty);
        c = SHReplace.ReplaceAllWhitecharsForSpace(c);
        c = SHReplace.ReplaceAllDoubleSpaceToSingle(c);
        c = c.Replace("> <", "><");
        return c;
    }

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
            var v = nsmgr.LookupNamespace(item2);

            if (!ns.ContainsKey(item)) ns.Add(item, v);
        }

        return ns;
    }

    /// <summary>
    ///     If A1 is file, output will be save to file and return null
    ///     Otherwise return string
    /// </summary>
    /// <param name="pathOrContent"></param>
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

    public static List<XElement> GetElementsOfNameWithAttr(XElement xElement, string tag, string attr, string value/*,
        bool caseSensitive*/)
    {
        return GetElementsOfNameWithAttrWorker(xElement, tag, attr, value/*, false, caseSensitive*/);
    }

    public static List<XElement> GetElementsOfNameWithAttrWorker(XElement xElement, string tag, string attr,
        string value/*, bool enoughIsContainsAttribute, bool caseSensitive*/)
    {
        var vr = new List<XElement>();
        var e = GetElementsOfNameRecursive(xElement, tag);
        foreach (var item in e)
        {
            var attrValue = Attr(item, attr);
            if (attrValue.Contains(
                    value) /*SH.ContainsBoolBool(attrValue, value, enoughIsContainsAttribute, caseSensitive)*/
               ) vr.Add(item);
        }

        return vr;
    }

    /// <param name="item"></param>
    /// <param name="p"></param>
    public static XElement GetElementOfNameRecursive(XElement node, string nazev)
    {
        //bool ns = true;
        if (nazev.Contains(":"))
        {
            var (p, z) = SH.GetPartsByLocationNoOut(nazev, ':');
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
    /// <param name="p"></param>
    /// <param name="deli"></param>
    public static string ReturnValueAllSubElementsSeparatedBy(XElement p, string deli)
    {
        var sb = new StringBuilder();
        var xml = GetXml(p);
        var mc = Regex.Matches(xml, "<(?:\"[^\"]*\"['\"]*|'[^']*'['\"]*|[^'\">])+>");
        var nahrazeno = new List<string>();
        foreach (Match item in mc)
            if (!nahrazeno.Contains(item.Value))
            {
                nahrazeno.Add(item.Value);
                xml = xml.Replace(item.Value, deli);
            }

        sb.Append(xml);
        return sb.ToString().Replace(deli + deli, deli);
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
            var s = f.Element(XName.Get(second));
            return s;
        }

        return null;
    }

    public static string GetValueOfElementOfSecondLevelOrSE(XElement var, string first, string second)
    {
        var xe = GetElementOfSecondLevel(var, first, second);
        if (xe != null) return xe.Value.Trim();
        return "";
    }

    /// <param name="var"></param>
    /// <param name="p"></param>
    public static string GetValueOfElementOfNameOrSE(XElement var, string nazev)
    {
        var xe = GetElementOfName(var, nazev);
        if (xe == null) return "";
        return xe.Value.Trim();
    }
}