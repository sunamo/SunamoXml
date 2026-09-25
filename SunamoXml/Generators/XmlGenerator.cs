namespace SunamoXml.Generators;

/// <summary>
/// Builds XML content using a StringBuilder. Supports tag writing, attributes, CDATA sections, and optional stack-based tracking of opened tags.
/// </summary>
public class XmlGenerator
{
    private readonly Stack<string>? stack;
    private readonly bool isUsingStack;

    /// <summary>
    /// The underlying StringBuilder that holds the generated XML content.
    /// </summary>
    public StringBuilder StringBuilder { get; set; } = new();

    /// <summary>
    /// Initializes a new instance without stack tracking.
    /// </summary>
    public XmlGenerator() : this(false)
    {
    }

    /// <summary>
    /// Initializes a new instance with optional stack tracking of opened tags.
    /// </summary>
    /// <param name="isUsingStack">Whether to track opened tags on a stack.</param>
    public XmlGenerator(bool isUsingStack)
    {
        this.isUsingStack = isUsingStack;
        if (isUsingStack) stack = new Stack<string>();
    }

    /// <summary>
    /// Returns the current length of the generated XML content.
    /// </summary>
    public int Length()
    {
        return StringBuilder.Length;
    }

    /// <summary>
    /// Writes a self-closing tag with two attributes.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    /// <param name="firstName">The first attribute name.</param>
    /// <param name="firstValue">The first attribute value.</param>
    /// <param name="secondName">The second attribute name.</param>
    /// <param name="secondValue">The second attribute value.</param>
    public void WriteNonPairTagWith2Attrs(string tagName, string firstName, string firstValue, string secondName, string secondValue)
    {
        StringBuilder.AppendFormat("<{0} {1}=\"{2}\" {3}=\"{4}\" />", tagName, firstName, firstValue, secondName, secondValue);
    }

    /// <summary>
    /// Writes a self-closing tag with one attribute.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    /// <param name="attrName">The attribute name.</param>
    /// <param name="attrValue">The attribute value.</param>
    public void WriteNonPairTagWithAttr(string tagName, string attrName, string attrValue)
    {
        StringBuilder.AppendFormat("<{0} {1}=\"{2}\" />", tagName, attrName, attrValue);
    }

    /// <summary>
    /// Inserts text at the specified index in the generated content.
    /// </summary>
    /// <param name="index">The position to insert at.</param>
    /// <param name="text">The text to insert.</param>
    public void Insert(int index, string text)
    {
        StringBuilder.Insert(index, text);
    }

    /// <summary>
    /// Appends a new line to the generated content.
    /// </summary>
    public void AppendLine()
    {
        StringBuilder.AppendLine();
    }

    /// <summary>
    /// Writes an XML comment opening marker.
    /// </summary>
    public void StartComment()
    {
        StringBuilder.Append("<!--");
    }

    /// <summary>
    /// Writes an XML comment closing marker.
    /// </summary>
    public void EndComment()
    {
        StringBuilder.Append("-->");
    }

    /// <summary>
    /// Writes a self-closing tag with multiple attributes from a list.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    /// <param name="attributes">List of alternating attribute names and values.</param>
    public void WriteNonPairTagWithAttrs(string tagName, List<string> attributes)
    {
        WriteNonPairTagWithAttrs(tagName, attributes.ToArray());
    }

    /// <summary>
    /// Writes a self-closing tag with multiple attributes.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    /// <param name="attributes">Alternating attribute names and values.</param>
    public void WriteNonPairTagWithAttrs(string tagName, params string[] attributes)
    {
        StringBuilder.AppendFormat("<{0} ", tagName);
        for (var i = 0; i < attributes.Length; i++)
        {
            var attributeName = attributes[i];
            object attributeValue = attributes[++i];
            StringBuilder.AppendFormat("{0}=\"{1}\" ", attributeName, attributeValue);
        }

        StringBuilder.Append(" />");
    }

    /// <summary>
    /// Writes a CDATA section with the specified content.
    /// </summary>
    /// <param name="innerCData">The content to wrap in CDATA.</param>
    public void WriteCData(string innerCData)
    {
        WriteRaw(string.Format("<![CDATA[{0}]]>", innerCData));
    }

    /// <summary>
    /// Writes an opening tag with one attribute.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    /// <param name="attributeName">The attribute name.</param>
    /// <param name="attributeValue">The attribute value.</param>
    /// <param name="isSkippingEmptyOrNull">Whether to skip writing if the attribute value is empty or null.</param>
    public void WriteTagWithAttr(string tagName, string attributeName, string attributeValue, bool isSkippingEmptyOrNull = false)
    {
        if (isSkippingEmptyOrNull)
            if (string.IsNullOrWhiteSpace(attributeValue))
                return;
        var result = string.Format("<{0} {1}=\"{2}\">", tagName, attributeName, attributeValue);
        if (isUsingStack) stack?.Push(result);
        StringBuilder.Append(result);
    }

    /// <summary>
    /// Writes raw content directly to the output.
    /// </summary>
    /// <param name="rawContent">The raw content to append.</param>
    public void WriteRaw(string rawContent)
    {
        StringBuilder.Append(rawContent);
    }

    /// <summary>
    /// Writes a closing tag for the specified element.
    /// </summary>
    /// <param name="tagName">The element name to close.</param>
    public void TerminateTag(string tagName)
    {
        StringBuilder.AppendFormat("</{0}>", tagName);
    }

    /// <summary>
    /// Writes an opening tag for the specified element.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    public void WriteTag(string tagName)
    {
        var result = $"<{tagName}>";
        if (isUsingStack) stack?.Push(result);
        StringBuilder.Append(result);
    }

    /// <summary>
    /// Returns the generated XML content as a string.
    /// </summary>
    public override string ToString()
    {
        return StringBuilder.ToString();
    }

    /// <summary>
    /// Writes an opening tag with multiple attributes from a list.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    /// <param name="attributes">List of alternating attribute names and values.</param>
    public void WriteTagWithAttrs(string tagName, List<string> attributes)
    {
        WriteTagWithAttrs(tagName, attributes.ToArray());
    }

    /// <summary>
    /// Writes an opening tag with multiple attributes, skipping null values.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    /// <param name="attributes">Alternating attribute names and values.</param>
    public void WriteTagWithAttrs(string tagName, params string[] attributes)
    {
        WriteTagWithAttrs(true, tagName, attributes);
    }

    /// <summary>
    /// Writes an opening tag with attributes from a dictionary, including null values.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    /// <param name="attributes">Dictionary of attribute names and values.</param>
    private void WriteTagWithAttrs(string tagName, Dictionary<string, string> attributes)
    {
        WriteTagWithAttrs(true, tagName, DictionaryHelper.GetListStringFromDictionary(attributes).ToArray());
    }

    /// <summary>
    /// Writes an opening tag with multiple attributes, skipping those with null values.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    /// <param name="attributes">Alternating attribute names and values.</param>
    public void WriteTagWithAttrsCheckNull(string tagName, params string[] attributes)
    {
        WriteTagWithAttrs(false, tagName, attributes);
    }

    /// <summary>
    /// Checks whether a string is null, empty, or the literal "(null)".
    /// </summary>
    /// <param name="text">The string to check.</param>
    private bool IsNulledOrEmpty(string text)
    {
        if (string.IsNullOrEmpty(text) || text == "(null)") return true;
        return false;
    }

    /// <summary>
    /// Writes an opening tag with namespace manager declarations and additional attributes.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    /// <param name="namespaceManager">The namespace manager containing namespace declarations.</param>
    /// <param name="attributes">Additional alternating attribute names and values.</param>
    public void WriteTagNamespaceManager(string tagName, XmlNamespaceManager namespaceManager, params string[] attributes)
    {
        var dictionary = XHelper.XmlNamespaces(namespaceManager, true);
        for (var i = 0; i < attributes.Count(); i++) dictionary.Add(attributes[i], attributes[++i]);
        WriteTagWithAttrs(tagName, dictionary);
    }

    /// <summary>
    /// Writes a self-closing tag with multiple attributes, optionally including null values.
    /// </summary>
    /// <param name="isAppendingNull">Whether to include attributes with null values.</param>
    /// <param name="tagName">The element name.</param>
    /// <param name="attributes">Alternating attribute names and values.</param>
    public void WriteNonPairTagWithAttrs(bool isAppendingNull, string tagName, params string[] attributes)
    {
        var localStringBuilder = new StringBuilder();
        localStringBuilder.AppendFormat("<{0} ", tagName);
        for (var i = 0; i < attributes.Length; i++)
        {
            var attributeName = attributes[i];
            var attributeValue = attributes[++i];
            if ((string.IsNullOrEmpty(attributeValue) && isAppendingNull) || !string.IsNullOrEmpty(attributeValue))
                if ((!IsNulledOrEmpty(attributeName) && isAppendingNull) || !IsNulledOrEmpty(attributeValue))
                    localStringBuilder.AppendFormat("{0}=\"{1}\" ", attributeName, attributeValue);
        }

        localStringBuilder.Append(" /");
        localStringBuilder.Append(">");
        var result = localStringBuilder.ToString();
        if (isUsingStack) stack?.Push(result);
        StringBuilder.Append(result);
    }

    /// <summary>
    /// Writes an opening tag with multiple attributes, optionally including null values.
    /// </summary>
    /// <param name="isAppendingNull">Whether to include attributes with null values.</param>
    /// <param name="tagName">The element name.</param>
    /// <param name="attributes">Alternating attribute names and values.</param>
    private void WriteTagWithAttrs(bool isAppendingNull, string tagName, params string[] attributes)
    {
        var localStringBuilder = new StringBuilder();
        localStringBuilder.AppendFormat("<{0} ", tagName);
        for (var i = 0; i < attributes.Length; i++)
        {
            var attributeName = attributes[i];
            var attributeValue = attributes[++i];
            if ((string.IsNullOrEmpty(attributeValue) && isAppendingNull) || !string.IsNullOrEmpty(attributeValue))
                if ((!IsNulledOrEmpty(attributeName) && isAppendingNull) || !IsNulledOrEmpty(attributeValue))
                    localStringBuilder.AppendFormat("{0}=\"{1}\" ", attributeName, attributeValue);
        }

        localStringBuilder.Append(">");
        var result = localStringBuilder.ToString();
        if (isUsingStack) stack?.Push(result);
        StringBuilder.Append(result);
    }

    /// <summary>
    /// Writes a complete element with the specified inner content.
    /// </summary>
    /// <param name="elementName">The element name.</param>
    /// <param name="inner">The inner content of the element.</param>
    public void WriteElement(string elementName, string inner)
    {
        StringBuilder.AppendFormat("<{0}>{1}</{0}>", elementName, inner);
    }

    /// <summary>
    /// Writes the standard XML declaration to the output.
    /// </summary>
    public void WriteXmlDeclaration()
    {
        StringBuilder.Append(XmlTemplates.Xml);
    }

    /// <summary>
    /// Writes an opening tag with two attributes.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    /// <param name="firstName">The first attribute name.</param>
    /// <param name="firstValue">The first attribute value.</param>
    /// <param name="secondName">The second attribute name.</param>
    /// <param name="secondValue">The second attribute value.</param>
    public void WriteTagWith2Attrs(string tagName, string firstName, string firstValue, string secondName, string secondValue)
    {
        var result = string.Format("<{0} {1}=\"{2}\" {3}=\"{4}\">", tagName, firstName, firstValue, secondName, secondValue);
        if (isUsingStack) stack?.Push(result);
        StringBuilder.Append(result);
    }

    /// <summary>
    /// Writes a self-closing tag without attributes.
    /// </summary>
    /// <param name="tagName">The element name.</param>
    public void WriteNonPairTag(string tagName)
    {
        StringBuilder.AppendFormat("<{0} />", tagName);
    }
}
