namespace SunamoXml.Generators;

/// <summary>
/// Extends <see cref="XmlGenerator"/> with selective attribute writing, allowing certain attributes to be excluded.
/// </summary>
public class XmlGeneratorSelective : XmlGenerator
{
    /// <summary>
    /// Writes an opening tag with attributes, excluding those whose names appear in the exclusion list.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    /// <param name="excludedAttributes">List of attribute names to exclude from the output.</param>
    /// <param name="attributes">List of alternating attribute names and values.</param>
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
    /// Returns the generated XML content as a string.
    /// </summary>
    public override string ToString()
    {
        return StringBuilder.ToString();
    }
}
