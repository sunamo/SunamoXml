namespace SunamoXml.Generators;

public class XmlGeneratorSelective : XmlGenerator
{
    public void WriteTagWithAttrsSelective(string tagName, List<string> excludedAttributes, List<string> attributes)
    {
        StringBuilder.AppendFormat("<{0} ", tagName);
        for (var i = 0; i < attributes.Count / 2; i++)
        {
            var attributeName = attributes[i * 2];
            if (!excludedAttributes.Contains(attributeName)) StringBuilder.AppendFormat("{0}=\"{1}\"", attributeName, attributes[i * 2 + 1]);
        }

        StringBuilder.Append(">");
    }

    public override string ToString()
    {
        return StringBuilder.ToString();
    }
}
