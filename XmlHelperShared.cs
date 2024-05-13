namespace SunamoXml;


/// <summary>
/// Use System.Xml NS
/// </summary>
public static partial class XmlHelper
{
    public static XmlAttribute foundedNode = null;

    public static XmlNode GetAttributeWithName(XmlNode item, string p)
    {
        foreach (XmlAttribute item2 in item.Attributes)
        {
            if (item2.Name == p)
            {
                foundedNode = item2;
                return item2;
            }
        }

        return null;
    }

    public static bool IsXml(string str)
    {
        if (!string.IsNullOrEmpty(str) && str.TrimStart().StartsWith("<"))
        {
            return true;
        }
        return false;
    }

    #region Same funcionality as in XHelper
    /// <summary>
    /// Vrátí InnerXml nebo hodnotu CData podle typu uzlu
    /// </summary>
    /// <param name = "eventDescriptionNode"></param>
    public static string GetInnerXml(XmlNode eventDescriptionNode)
    {
        string eventDescription = "";
        if (eventDescriptionNode is XmlCDataSection)
        {
            XmlCDataSection cdataSection = eventDescriptionNode as XmlCDataSection;
            eventDescription = cdataSection.Value;
        }
        else
        {
            if (eventDescriptionNode != null)
            {
                eventDescription = eventDescriptionNode.InnerXml;
            }

        }

        return eventDescription;
    }

    const string dummyXmlns = "https://sunamo.cz/_/dummyXmlns";

    public static void RemoveAttrsFromRoot(ref XmlDocument x, string newRootElementName, params string[] attrsName)
    {
        XmlDocument docNew = new XmlDocument();
        XmlElement newRoot = docNew.CreateElement(newRootElementName);

        foreach (XmlAttribute item in x.DocumentElement.Attributes)
        {
            if (!attrsName.Contains(item.Name))
            {
                var item2 = docNew.ImportNode(item, true);
                var xa = (XmlAttribute)item2;
                newRoot.Attributes.Append(xa);
            }
        }

        if (newRoot.Attributes.Count == 0)
        {
            var xa = docNew.CreateAttribute(Consts.xmlns);
            xa.Value = Consts.Schema;
            newRoot.Attributes.Append(xa);
        }

        docNew.AppendChild(newRoot);
        newRoot.InnerXml = x.DocumentElement.InnerXml;
        x = docNew;
    }

    public static void AddAttrsToRoot(ref XmlDocument x, string newRootElementName, params string[] attrs)
    {
        XmlDocument docNew = new XmlDocument();
        XmlElement newRoot = docNew.CreateElement(newRootElementName);

        if (!ThrowEx.IsOdd("attrs", attrs))
        {
            return;
        }
        bool addedAny = false;
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

        //var x2 = XmlHelper.Attr(newRoot, Consts.xmlns);
        //if (x2 == dummyXmlns)
        //{
        //    newRoot.Attributes.Remove(XmlHelper.foundedNode);
        //}

        if (!addedAny)
        {
            return;
        }

        docNew.AppendChild(newRoot);
        newRoot.InnerXml = x.DocumentElement.InnerXml;
        x = docNew;
    }

    public static string InnerTextOfNode(XmlNode xe, string version2)
    {
        return xe.InnerText;
    }

    public static void AddXmlNamespaces(XmlNamespaceManager nsmgr)
    {

    }

    public static XmlDocument CreateXmlDocument(string content)
    {
        XmlDocument d = new XmlDocument();
        d.LoadXml(content);
        d.PreserveWhitespace = true;
        return d;
    }

    /// <summary>
    /// Vrátí null pokud se nepodaří nalézt
    /// </summary>
    /// <param name = "item"></param>
    /// <param name = "p"></param>
    public static XmlNode GetChildNodeWithName(XmlNode item, string p)
    {
        foreach (XmlNode item2 in item.ChildNodes)
        {
            if (item2.Name == p)
            {
                return item2;
            }
        }

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
        if (a != null)
        {
            return a.Value;
        }
        return null;
    }

    public static void SetAttribute(XmlNode node, string include, string rel)
    {
        var xe = ((XmlElement)node);
        if (xe != null)
        {
            xe.SetAttribute(include, rel);
            return;
        }

        // Working only when attribute
        var atrValue = XmlHelper.Attr(node, include);
        if (atrValue == null)
        {
            var xa = node.OwnerDocument.CreateAttribute(include);
            node.Attributes.Append(xa);
        }
        node.Attributes[include].Value = rel;
    }
    #endregion
}
