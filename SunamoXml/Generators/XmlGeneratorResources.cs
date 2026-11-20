namespace SunamoXml.Generators;

/// <summary>
/// Represents the xml generator resources type.
/// </summary>
public class XmlGeneratorResources
{
    private readonly XmlGenerator xmlGenerator = new();

    /// <summary>
    /// Add text.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="text">The text.</param>
    public void AddText(string name, string text)
    {
        xmlGenerator.WriteTagWith2Attrs("data", "name", name, "xml:space", "preserve");
        xmlGenerator.WriteElement("value", text);
        xmlGenerator.TerminateTag("data");
    }

    /// <summary>
    /// To string.
    /// </summary>
    /// <returns>The to string result.</returns>
    public override string ToString()
    {
        return xmlGenerator.ToString();
    }
}
