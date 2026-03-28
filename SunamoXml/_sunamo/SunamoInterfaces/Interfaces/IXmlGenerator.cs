namespace SunamoXml._sunamo.SunamoInterfaces.Interfaces;

/// <summary>
/// Defines a contract for XML generation operations.
/// </summary>
internal interface IXmlGenerator
{
    /// <summary>
    /// Appends a new line to the output.
    /// </summary>
    void AppendLine();

    /// <summary>
    /// Writes an XML comment closing tag.
    /// </summary>
    void EndComment();

    /// <summary>
    /// Inserts text at the specified position.
    /// </summary>
    /// <param name="index">The position to insert at.</param>
    /// <param name="text">The text to insert.</param>
    void Insert(int index, string text);

    /// <summary>
    /// Returns the current length of the generated XML content.
    /// </summary>
    int Length();

    /// <summary>
    /// Writes an XML comment opening tag.
    /// </summary>
    void StartComment();

    /// <summary>
    /// Writes a closing tag for the specified element.
    /// </summary>
    /// <param name="tagName">The element name to close.</param>
    void TerminateTag(string tagName);

    /// <summary>
    /// Returns the generated XML as a string.
    /// </summary>
    string ToString();

    /// <summary>
    /// Writes a CDATA section with the specified content.
    /// </summary>
    /// <param name="innerCData">The content to wrap in CDATA.</param>
    void WriteCData(string innerCData);

    /// <summary>
    /// Writes a complete element with the specified inner content.
    /// </summary>
    /// <param name="elementName">The element name.</param>
    /// <param name="inner">The inner content of the element.</param>
    void WriteElement(string elementName, string inner);

    /// <summary>
    /// Writes a self-closing tag without attributes.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    void WriteNonPairTag(string tagName);

    /// <summary>
    /// Writes a self-closing tag with two attributes.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    /// <param name="firstName">The first attribute name.</param>
    /// <param name="firstValue">The first attribute value.</param>
    /// <param name="secondName">The second attribute name.</param>
    /// <param name="secondValue">The second attribute value.</param>
    void WriteNonPairTagWith2Attrs(string tagName, string firstName, string firstValue, string secondName, string secondValue);

    /// <summary>
    /// Writes a self-closing tag with one attribute.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    /// <param name="attrName">The attribute name.</param>
    /// <param name="attrValue">The attribute value.</param>
    void WriteNonPairTagWithAttr(string tagName, string attrName, string attrValue);

    /// <summary>
    /// Writes a self-closing tag with multiple attributes, optionally appending null values.
    /// </summary>
    /// <param name="isAppendingNull">Whether to include attributes with null values.</param>
    /// <param name="tagName">The element name.</param>
    /// <param name="attributes">Alternating attribute names and values.</param>
    void WriteNonPairTagWithAttrs(bool isAppendingNull, string tagName, params string[] attributes);

    /// <summary>
    /// Writes a self-closing tag with multiple attributes from a list.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    /// <param name="attributes">List of alternating attribute names and values.</param>
    void WriteNonPairTagWithAttrs(string tagName, List<string> attributes);

    /// <summary>
    /// Writes a self-closing tag with multiple attributes.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    /// <param name="attributes">Alternating attribute names and values.</param>
    void WriteNonPairTagWithAttrs(string tagName, params string[] attributes);

    /// <summary>
    /// Writes raw content directly to the output without any encoding.
    /// </summary>
    /// <param name="rawContent">The raw content to write.</param>
    void WriteRaw(string rawContent);

    /// <summary>
    /// Writes an opening tag for the specified element.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    void WriteTag(string tagName);

    /// <summary>
    /// Writes an opening tag with namespace manager information and additional attributes.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    /// <param name="namespaceManager">The namespace manager containing namespace declarations.</param>
    /// <param name="attributes">Additional alternating attribute names and values.</param>
    void WriteTagNamespaceManager(string tagName, XmlNamespaceManager namespaceManager, params string[] attributes);

    /// <summary>
    /// Writes an opening tag with two attributes.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    /// <param name="firstName">The first attribute name.</param>
    /// <param name="firstValue">The first attribute value.</param>
    /// <param name="secondName">The second attribute name.</param>
    /// <param name="secondValue">The second attribute value.</param>
    void WriteTagWith2Attrs(string tagName, string firstName, string firstValue, string secondName, string secondValue);

    /// <summary>
    /// Writes an opening tag with one attribute, optionally skipping empty or null values.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    /// <param name="attributeName">The attribute name.</param>
    /// <param name="attributeValue">The attribute value.</param>
    /// <param name="isSkippingEmptyOrNull">Whether to skip writing if the attribute value is empty or null.</param>
    void WriteTagWithAttr(string tagName, string attributeName, string attributeValue, bool isSkippingEmptyOrNull = false);

    /// <summary>
    /// Writes an opening tag with multiple attributes from a list.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    /// <param name="attributes">List of alternating attribute names and values.</param>
    void WriteTagWithAttrs(string tagName, List<string> attributes);

    /// <summary>
    /// Writes an opening tag with multiple attributes.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    /// <param name="attributes">Alternating attribute names and values.</param>
    void WriteTagWithAttrs(string tagName, params string[] attributes);

    /// <summary>
    /// Writes an opening tag with multiple attributes, skipping those with null values.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    /// <param name="attributes">Alternating attribute names and values.</param>
    void WriteTagWithAttrsCheckNull(string tagName, params string[] attributes);

    /// <summary>
    /// Writes the standard XML declaration.
    /// </summary>
    void WriteXmlDeclaration();
}
