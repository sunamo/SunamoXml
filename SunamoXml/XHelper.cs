namespace SunamoXml;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public partial class XHelper
{
    public static Dictionary<string, string> ns = new();
    public static string InnerTextOfNode(XElement xe, string value)
    {
        var desc = xe.Descendants(XName.Get(value));
        if (desc.Count() == 0)
            return string.Empty;
        var first = desc.First();
        return first.Value;
    }

    /// <summary>
    ///     Při nenalezení vrací null
    /// </summary>
    /// <param name = "item"></param>
    /// <param name = "attr"></param>
    public static string Attr(XElement item, string attr)
    {
        var xa = item.Attribute(XName.Get(attr));
        if (xa != null)
            return xa.Value;
        return null;
    }

    public static XElement GetElementOfNameWithAttr(XElement node, string nazev, string attr, string value)
    {
        if (nazev.Contains(":"))
        {
            var(p, z) = SH.GetPartsByLocationNoOut(nazev, ':');
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
        foreach (var element in settings.DescendantsAndSelf())
            //element.Name =  element.Name.LocalName;
            element.Name = XName.Get(element.Name.LocalName, ns2);
        //foreach (var element in settings.Attributes())
        //{
        //    //element.Name = XName.Get(element.Name.LocalName, ns2);
        //    toInsert.Add(element);
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

    public static IList<XElement> GetElementsOfNameWithAttrContains(XElement group, string tag, string attr, string value /*, bool caseSensitive = false*/)
    {
        return GetElementsOfNameWithAttrWorker(group, tag, attr, value /*, true, caseSensitive*/);
    }

    public static void AddXmlNamespaces(XmlNamespaceManager nsmgr)
    {
        foreach (string item in nsmgr)
        {
            // Jaký je typ item, at nemusím používat slovník
            var value = nsmgr.LookupNamespace(item);
            if (!ns.ContainsKey(item))
                ns.Add(item, value);
        }
    }

    /// <param name = "p"></param>
    public static void AddXmlNamespaces(params string[] p)
    {
        for (var i = 0; i < p.Length; i++)
            //.TrimEnd('/') + "/"
            ns.Add(p[i].Replace("xmlns:", ""), p[++i]);
    }

    public static void AddXmlNamespaces(Dictionary<string, string> dictionary)
    {
        foreach (var item in dictionary)
            ns.Add(item.Key, item.Value);
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
    ///     Získá element jména A2 value A1.
    ///     Umí pracovat value NS, stačí zadat zkratku namepsace jako ns:tab
    /// </summary>
    /// <param name = "node"></param>
    /// <param name = "nazev"></param>
    public static XElement GetElementOfName(XContainer node, string nazev)
    {
        if (nazev.Contains(":"))
        {
            var(p, z) = SH.GetPartsByLocationNoOut(nazev, ':');
            p = ns[p];
            foreach (var item in node.Elements())
            {
                if (IsRightTag(item, z, p))
                {
                }

                if (item.Name.LocalName == z && item.Name.NamespaceName == p)
                    return item;
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
    /// <param name = "xName"></param>
    /// <param name = "nazev"></param>
    public static bool IsRightTag(XName xName, string nazev)
    {
        var(p, z) = SH.GetPartsByLocationNoOut(nazev, ':');
        p = ns[p];
        if (xName.LocalName == z && xName.NamespaceName == p)
            return true;
        return false;
    }

    public static bool IsRightTag(XElement xName, string localName, string namespaceName)
    {
        return IsRightTag(xName.Name, localName, namespaceName);
    }

    /// <summary>
    ///     Into A3 is passing shortcut
    /// </summary>
    /// <param name = "xName"></param>
    /// <param name = "localName"></param>
    /// <param name = "namespaceName"></param>
    public static bool IsRightTag(XName xName, string localName, string namespaceName)
    {
        if (xName.LocalName == localName && xName.NamespaceName == namespaceName)
            return true;
        return false;
    }

    public static List<XElement> GetElementsOfNameRecursive(XElement node, string nazev)
    {
        var vr = new List<XElement>();
        if (nazev.Contains(":"))
        {
            var(p, z) = SH.GetPartsByLocationNoOut(nazev, ':');
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
}