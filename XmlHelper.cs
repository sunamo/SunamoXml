namespace SunamoXml;


public static partial class XmlHelper
{
    /// <summary>
    ///     Usage: FubuCsprojFile
    ///
    /// A2 is used only in exception
    /// </summary>
    /// <param name="xmlContent"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string FormatXmlInMemory(string xmlContent, string path = Consts.se)
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

            writer.Formatting = System.Xml.Formatting.Indented;

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


            return Consts.Exception + path + nl + nl + ex.Message;
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

        if (attr != null)
        {
            return attr.Value;
        }

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
        {
            if (item2.Name == p)
            {
                foundedNode = item2;
                return item2.InnerXml;
            }
        }

        return null;
    }

    /// <summary>
    /// because return type is Object and can't use item.ChildNodes.First(d => d.) etc.
    /// XmlNodeList dědí jen z IEnumerable, IDisposable
    /// </summary>
    /// <returns></returns>
    public static List<XmlNode> ChildNodes(XmlNode xml)
    {
        // TODO: až přilinkuji SunamoExtensions tak .COunt
        List<XmlNode> result = new List<XmlNode>();

        foreach (XmlNode item in xml.ChildNodes)
        {
            result.Add(item);
        }

        return result;
    }

    /// <summary>
    /// WOrkaround for error The node to be removed is not a child of this node.
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
            for (int i = 0; i < chn.Count; i++)
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
}
