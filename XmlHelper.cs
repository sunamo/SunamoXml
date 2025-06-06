namespace SunamoXml;

public static class XmlHelper
{
    public static XmlAttribute foundedNode;

    public static XmlNode GetAttributeWithName(XmlNode item, string p)
    {
        foreach (XmlAttribute item2 in item.Attributes)
            if (item2.Name == p)
            {
                foundedNode = item2;
                return item2;
            }

        return null;
    }

    public static bool IsXml(string str)
    {
        if (!string.IsNullOrEmpty(str) && str.TrimStart().StartsWith("<")) return true;
        return false;
    }

    /// <summary>
    ///     Usage: FubuCsprojFile
    ///     A2 is used only in exception
    /// </summary>
    /// <param name="xmlContent"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string FormatXmlInMemory(string xmlContent, string path = "")
    {
        MemoryStream mStream = new();
        XmlTextWriter writer = new(mStream, Encoding.Unicode);
        //XmlNamespacesHolder h = new XmlNamespacesHolder();

        //document = h.ParseAndRemoveNamespacesXmlDocument(xml);
        XmlDocument document = new();

        string result;
        try
        {
            document.LoadXml(xmlContent);

            writer.Formatting = Formatting.Indented;

            // Write the XML into a formatting XmlTextWriter
            document.WriteContentTo(writer);
            writer.Flush();
            mStream.Flush();

            // Have to rewind the MemoryStream in order to read
            // its contents.
            mStream.Position = 0;

            // Read MemoryStream contents into a StreamReader.
            StreamReader sReader = new(mStream);

            // Extract the text from the StreamReader.
            var formattedXml = sReader.ReadToEnd();

            result = formattedXml;
        }
        catch (XmlException ex)
        {
            var nl = Environment.NewLine;


            return "Exception:" + path + nl + nl + ex.Message;
            //ThrowEx.CustomWithStackTrace(ex);
        }

        mStream.Close();
        // 'Cannot access a closed Stream.'
        //writer.Close();

        return result;
    }

    public static string GetAttrValueOrInnerElement(XmlNode item, string v)
    {
        var attr = item.Attributes[v];

        if (attr != null) return attr.Value;

        var childNodes = ChildNodes(item);
        if (childNodes.Count != 0)
        {
            var el = childNodes.First(d => d.Name == v);
            return el?.Value;
        }

        Debugger.Break();
        return null;
    }

    public static string GetAttributeWithNameValue(XmlNode item, string p)
    {
        foreach (XmlAttribute item2 in item.Attributes)
            if (item2.Name == p)
            {
                foundedNode = item2;
                return item2.InnerXml;
            }

        return null;
    }

    /// <summary>
    ///     because return type is Object and can't use item.ChildNodes.First(d => d.) etc.
    ///     XmlNodeList dědí jen z IEnumerable, IDisposable
    /// </summary>
    /// <returns></returns>
    public static List<XmlNode> ChildNodes(XmlNode xml)
    {
        // TODO: až přilinkuji SunamoExtensions tak .COunt
        var result = new List<XmlNode>();

        foreach (XmlNode item in xml.ChildNodes) result.Add(item);

        return result;
    }

    /// <summary>
    ///     WOrkaround for error The node to be removed is not a child of this node.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    public static XmlNode ReplaceChildNodeByOuterHtml(XmlNode from, XmlNode to)
    {
        var pn = from.ParentNode;
        var chn = pn.ChildNodes;

        if (chn.Contains(from))
        {
            from = from.ParentNode.ReplaceChild(to, from);
        }
        else
        {
            var toOx = to.OuterXml;
            for (var i = 0; i < chn.Count; i++)
            {
                var ox = chn[i].OuterXml;
                if (ox == toOx)
                {
                    from = pn.ReplaceChild(to, chn[i]);
                    break;
                }
            }
        }

        return from;
    }

    #region Same funcionality as in XHelper

    /// <summary>
    ///     Vrátí InnerXml nebo hodnotu CData podle typu uzlu
    /// </summary>
    /// <param name="eventDescriptionNode"></param>
    public static string GetInnerXml(XmlNode eventDescriptionNode)
    {
        var eventDescription = "";
        if (eventDescriptionNode is XmlCDataSection)
        {
            var cdataSection = eventDescriptionNode as XmlCDataSection;
            eventDescription = cdataSection.Value;
        }
        else
        {
            if (eventDescriptionNode != null) eventDescription = eventDescriptionNode.InnerXml;
        }

        return eventDescription;
    }

    private const string dummyXmlns = "https://sunamo.cz/_/dummyXmlns";

    public static void RemoveAttrsFromRoot(ref XmlDocument x, string newRootElementName, params string[] attrsName)
    {
        var docNew = new XmlDocument();
        var newRoot = docNew.CreateElement(newRootElementName);

        foreach (XmlAttribute item in x.DocumentElement.Attributes)
            if (!attrsName.Contains(item.Name))
            {
                var item2 = docNew.ImportNode(item, true);
                var xa = (XmlAttribute)item2;
                newRoot.Attributes.Append(xa);
            }

        if (newRoot.Attributes.Count == 0)
        {
            var xa = docNew.CreateAttribute("xmlns");
            xa.Value = "http://";
            newRoot.Attributes.Append(xa);
        }

        docNew.AppendChild(newRoot);
        newRoot.InnerXml = x.DocumentElement.InnerXml;
        x = docNew;
    }

    public static void AddAttrsToRoot(ref XmlDocument x, string newRootElementName, params string[] attrs)
    {
        var docNew = new XmlDocument();
        var newRoot = docNew.CreateElement(newRootElementName);

        if (!ThrowEx.HasOddNumberOfElements("attrs", attrs)) return;
        var addedAny = false;
        //for (int i = 0; i < attrs.Length; i++)
        //{
        //    //var xa =
        //    var x1 = attrs[++i];
        //    if (!string.IsNullOrEmpty(x1))
        //    {
        //        newRoot.SetAttribute(attrs[i], x1);
        //        addedAny = true;
        //    }
        //    //newRoot.Attributes.Append();
        //}

        //var x2 = XmlHelper.Attr(newRoot, "xmlns");
        //if (x2 == dummyXmlns)
        //{
        //    newRoot.Attributes.Remove(XmlHelper.foundedNode);
        //}

        if (!addedAny) return;

        docNew.AppendChild(newRoot);
        newRoot.InnerXml = x.DocumentElement.InnerXml;
        x = docNew;
    }

    public static string InnerTextOfNode(XmlNode xe)
    {
        return xe.InnerText;
    }


    public static XmlDocument CreateXmlDocument(string content)
    {
        var d = new XmlDocument();
        d.LoadXml(content);
        d.PreserveWhitespace = true;
        return d;
    }

    /// <summary>
    ///     Vrátí null pokud se nepodaří nalézt
    /// </summary>
    /// <param name="item"></param>
    /// <param name="p"></param>
    public static XmlNode GetChildNodeWithName(XmlNode item, string p)
    {
        foreach (XmlNode item2 in item.ChildNodes)
            if (item2.Name == p)
                return item2;

        return null;
    }

    public static XmlNode GetElementOfName(XmlNode e, string n)
    {
        return e.ChildNodes.First(n);
    }

    public static IList<XmlNode> GetElementsOfName(XmlNode e, string v)
    {
        return e.ChildNodes.WithName(v);
    }

    public static string Attr(XmlNode d, string v)
    {
        var a = GetAttributeWithName(d, v);
        if (a != null) return a.Value;
        return null;
    }

    public static void SetAttribute(XmlNode node, string include, string rel)
    {
        var xe = (XmlElement)node;
        if (xe != null)
        {
            xe.SetAttribute(include, rel);
            return;
        }

        // Working only when attribute
        var atrValue = Attr(node, include);
        if (atrValue == null)
        {
            var xa = node.OwnerDocument.CreateAttribute(include);
            node.Attributes.Append(xa);
        }

        node.Attributes[include].Value = rel;
    }

    #endregion
}