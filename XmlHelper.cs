using System.Diagnostics;
using System.Xml;

public static partial class XmlHelper
{
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
