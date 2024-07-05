namespace SunamoXml.Generators;

public class XmlGeneratorResources
{
    private XmlGenerator _xml = new XmlGenerator();

    public void AddText(string name, string text)
    {
        _xml.WriteTagWith2Attrs("data", "name", name, "xml:space", "preserve");
        _xml.WriteElement("value", text);
        _xml.TerminateTag("data");
    }

    public override string ToString()
    {
        return _xml.ToString();
    }
}
