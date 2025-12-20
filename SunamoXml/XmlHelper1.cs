// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoXml;
public static partial class XmlHelper
{
    public static IList<XmlNode> GetElementsOfName(XmlNode e, string v)
    {
        return e.ChildNodes.WithName(v);
    }

    public static string Attr(XmlNode data, string v)
    {
        var argument = GetAttributeWithName(data, v);
        if (argument != null)
            return argument.Value;
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
}