namespace SunamoXml.Generators;

/// <summary>
/// Represents the xml generator selective type.
/// </summary>
public class XmlGeneratorSelective : XmlGenerator
{
    /// <summary>
    /// Write tag with attrs selective.
    /// </summary>
    /// <param name="tagName">The tag name.</param>
    /// <param name="excludedAttributes">The excluded attributes.</param>
    /// <param name="attributes">The attributes.</param>
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

    /// <summary>
    /// To string.
    /// </summary>
    /// <returns>The to string result.</returns>
    public override string ToString()
    {
        return StringBuilder.ToString();
    }
}
