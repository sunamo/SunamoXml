namespace SunamoXml.Generators;

public class XmlGeneratorResources
{
    private readonly XmlGenerator xmlGenerator = new();

    public void AddText(string name, string text)
    {
        xmlGenerator.WriteTagWith2Attrs("data", "name", name, "xml:space", "preserve");
        xmlGenerator.WriteElement("value", text);
        xmlGenerator.TerminateTag("data");
    }

    public override string ToString()
    {
        return xmlGenerator.ToString();
    }
}
