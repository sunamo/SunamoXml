namespace SunamoXml._sunamo.SunamoInterfaces.Interfaces;

internal interface IXmlGenerator
{
    void AppendLine();
    void EndComment();
    void Insert(int index, string text);
    int Length();
    void StartComment();
    void TerminateTag(string tagName);
    string ToString();
    void WriteCData(string innerCData);
    void WriteElement(string elementName, string inner);
    void WriteNonPairTag(string tagName);
    void WriteNonPairTagWith2Attrs(string tagName, string firstName, string firstValue, string secondName, string secondValue);
    void WriteNonPairTagWithAttr(string tagName, string attrName, string attrValue);
    void WriteNonPairTagWithAttrs(bool appendNull, string tagName, params string[] attributes);
    void WriteNonPairTagWithAttrs(string tag, List<string> attributes);
    void WriteNonPairTagWithAttrs(string tag, params string[] attributes);
    void WriteRaw(string rawContent);
    void WriteTag(string tagName);
    void WriteTagNamespaceManager(object element, XmlNamespaceManager namespaceManager, string namespaceUri, string prefix);
    void WriteTagNamespaceManager(string nameTag, XmlNamespaceManager namespaceManager, params string[] attributes);
    void WriteTagWith2Attrs(string tagName, string firstName, string firstValue, string secondName, string secondValue);
    void WriteTagWithAttr(string tag, string attributeName, string attributeValue, bool skipEmptyOrNull = false);
    void WriteTagWithAttrs(string tagName, List<string> attributes);
    void WriteTagWithAttrs(string tagName, params string[] attributes);
    void WriteTagWithAttrsCheckNull(string tagName, params string[] attributes);
    void WriteXmlDeclaration();
}