namespace SunamoXml.Generators;

/// <summary>
/// Generates XML resource entries in the standard .resx format with name, xml:space, and value elements.
/// </summary>
public class XmlGeneratorResources
{
    private readonly XmlGenerator xmlGenerator = new();

    /// <summary>
    /// Adds a text resource entry with the specified name and value.
    /// </summary>
    /// <param name="name">The resource name.</param>
    /// <param name="text">The resource value text.</param>
    public void AddText(string name, string text)
    {
        xmlGenerator.WriteTagWith2Attrs("data", "name", name, "xml:space", "preserve");
        xmlGenerator.WriteElement("value", text);
        xmlGenerator.TerminateTag("data");
    }

    /// <summary>
    /// Returns the generated XML resource content as a string.
    /// </summary>
    public override string ToString()
    {
        return xmlGenerator.ToString();
    }
}
