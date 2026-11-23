namespace SunamoXml.Generators;

/// <summary>
/// Represents the xml generator type.
/// </summary>
public class XmlGenerator
{
    private readonly Stack<string>? stack;
    private readonly bool isUsingStack;

    /// <summary>
    /// Gets or sets the string builder.
    /// </summary>
    public StringBuilder StringBuilder { get; set; } = new();

    /// <summary>
    /// Xml generator.
    /// </summary>
    /// <returns>The xml generator result.</returns>
    public XmlGenerator() : this(false)
    {
    }

    /// <summary>
    /// Xml generator.
    /// </summary>
    /// <param name="isUsingStack">The is using stack.</param>
    /// <returns>The xml generator result.</returns>
    public XmlGenerator(bool isUsingStack)
    {
        this.isUsingStack = isUsingStack;
        if (isUsingStack) stack = new Stack<string>();
    }

    /// <summary>
    /// Initializes a new instance of the length class.
    /// </summary>
    /// <param name="isUsingStack">The is using stack.</param>
    public int Length()
    {
        return StringBuilder.Length;
    }

    /// <summary>
    /// Write non pair tag with 2 attrs.
    /// </summary>
    /// <param name="tagName">The tag name.</param>
    /// <param name="firstName">The first name.</param>
    /// <param name="firstValue">The first value.</param>
    /// <param name="secondName">The second name.</param>
    /// <param name="secondValue">The second value.</param>
    /// <returns>The write non pair tag with 2 attrs result.</returns>
    public void WriteNonPairTagWith2Attrs(string tagName, string firstName, string firstValue, string secondName, string secondValue)
    {
        StringBuilder.AppendFormat("<{0} {1}=\"{2}\" {3}=\"{4}\" />", tagName, firstName, firstValue, secondName, secondValue);
    }

    /// <summary>
    /// Initializes a new instance of the write non pair tag with attr class.
    /// </summary>
    /// <param name="{1}">The 1.</param>
    /// <param name="tagName">The tag name.</param>
    /// <param name="firstName">The first name.</param>
    /// <param name="firstValue">The first value.</param>
    /// <param name="secondName">The second name.</param>
    /// <param name="secondValue">The second value.</param>
    public void WriteNonPairTagWithAttr(string tagName, string attrName, string attrValue)
    {
        StringBuilder.AppendFormat("<{0} {1}=\"{2}\" />", tagName, attrName, attrValue);
    }

    /// <summary>
    /// Initializes a new instance of the insert class.
    /// </summary>
    /// <param name="{1}">The 1.</param>
    /// <param name="tagName">The tag name.</param>
    /// <param name="attrName">The attr name.</param>
    /// <param name="attrValue">The attr value.</param>
    public void Insert(int index, string text)
    {
        StringBuilder.Insert(index, text);
    }

    /// <summary>
    /// Initializes a new instance of the append line class.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <param name="text">The text.</param>
    public void AppendLine()
    {
        StringBuilder.AppendLine();
    }

    /// <summary>
    /// Initializes a new instance of the start comment class.
    /// </summary>
    public void StartComment()
    {
        StringBuilder.Append("<!--");
    }

    /// <summary>
    /// Initializes a new instance of the end comment class.
    /// </summary>
    /// <param name=""<!--"">The "&lt;!--".</param>
    public void EndComment()
    {
        StringBuilder.Append("-->");
    }

    /// <summary>
    /// Initializes a new instance of the write non pair tag with attrs class.
    /// </summary>
    /// <param name=""-->"">The "--&gt;".</param>
    public void WriteNonPairTagWithAttrs(string tagName, List<string> attributes)
    {
        WriteNonPairTagWithAttrs(tagName, attributes.ToArray());
    }

    /// <summary>
    /// Initializes a new instance of the write non pair tag with attrs class.
    /// </summary>
    /// <param name="tagName">The tag name.</param>
    /// <param name="attributes.ToArray()">The attributes to array.</param>
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
    /// Initializes a new instance of the write c data class.
    /// </summary>
    /// <param name="/>"">The /&gt;".</param>
    public void WriteCData(string innerCData)
    {
        WriteRaw(string.Format("<![CDATA[{0}]]>", innerCData));
    }

    /// <summary>
    /// Initializes a new instance of the write tag with attr class.
    /// </summary>
    /// <param name="innerCData)">The inner c data.</param>
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
    /// Initializes a new instance of the write raw class.
    /// </summary>
    /// <param name="result">The result.</param>
    public void WriteRaw(string rawContent)
    {
        StringBuilder.Append(rawContent);
    }

    /// <summary>
    /// Initializes a new instance of the terminate tag class.
    /// </summary>
    /// <param name="rawContent">The raw content.</param>
    public void TerminateTag(string tagName)
    {
        StringBuilder.AppendFormat("</{0}>", tagName);
    }

    /// <summary>
    /// Initializes a new instance of the write tag class.
    /// </summary>
    /// <param name=""</{0}>"">The 0.</param>
    /// <param name="tagName">The tag name.</param>
    public void WriteTag(string tagName)
    {
        var result = $"<{tagName}>";
        if (isUsingStack) stack?.Push(result);
        StringBuilder.Append(result);
    }

    /// <summary>
    /// Initializes a new instance of the to string class.
    /// </summary>
    /// <param name="result">The result.</param>
    public override string ToString()
    {
        return StringBuilder.ToString();
    }

    /// <summary>
    /// Write tag with attrs.
    /// </summary>
    /// <returns>The write tag with attrs result.</returns>
    public void WriteTagWithAttrs(string tagName, List<string> attributes)
    {
        WriteTagWithAttrs(tagName, attributes.ToArray());
    }

    /// <summary>
    /// Initializes a new instance of the write tag with attrs class.
    /// </summary>
    /// <param name="tagName">The tag name.</param>
    /// <param name="attributes.ToArray()">The attributes to array.</param>
    public void WriteTagWithAttrs(string tagName, params string[] attributes)
    {
        WriteTagWithAttrs(true, tagName, attributes);
    }

    private void WriteTagWithAttrs(string tagName, Dictionary<string, string> attributes)
    {
        WriteTagWithAttrs(true, tagName, DictionaryHelper.GetListStringFromDictionary(attributes).ToArray());
    }

    /// <summary>
    /// Initializes a new instance of the write tag with attrs check null class.
    /// </summary>
    /// <param name="true">The true.</param>
    /// <param name="tagName">The tag name.</param>
    /// <param name="DictionaryHelper.GetListStringFromDictionary(attributes).ToArray()">The dictionary helper get list string from dictionary attributes to array.</param>
    public void WriteTagWithAttrsCheckNull(string tagName, params string[] attributes)
    {
        WriteTagWithAttrs(false, tagName, attributes);
    }

    private bool IsNulledOrEmpty(string text)
    {
        if (string.IsNullOrEmpty(text) || text == "(null)") return true;
        return false;
    }

    /// <summary>
    /// Write tag namespace manager.
    /// </summary>
    /// <param name="tagName">The tag name.</param>
    /// <param name="namespaceManager">The namespace manager.</param>
    /// <param name="attributes">The attributes.</param>
    /// <returns>The write tag namespace manager result.</returns>
    public void WriteTagNamespaceManager(string tagName, XmlNamespaceManager namespaceManager, params string[] attributes)
    {
        var dictionary = XHelper.XmlNamespaces(namespaceManager, true);
        for (var i = 0; i < attributes.Count(); i++) dictionary.Add(attributes[i], attributes[++i]);
        WriteTagWithAttrs(tagName, dictionary);
    }

    /// <summary>
    /// Initializes a new instance of the write non pair tag with attrs class.
    /// </summary>
    /// <param name="tagName">The tag name.</param>
    /// <param name="dictionary">The dictionary.</param>
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
    /// Initializes a new instance of the write element class.
    /// </summary>
    /// <param name="result">The result.</param>
    public void WriteElement(string elementName, string inner)
    {
        StringBuilder.AppendFormat("<{0}>{1}</{0}>", elementName, inner);
    }

    /// <summary>
    /// Initializes a new instance of the write xml declaration class.
    /// </summary>
    /// <param name=""<{0}>{1}</{0}>"">The 0 1 0.</param>
    /// <param name="elementName">The element name.</param>
    /// <param name="inner">The inner.</param>
    public void WriteXmlDeclaration()
    {
        StringBuilder.Append(XmlTemplates.Xml);
    }

    /// <summary>
    /// Initializes a new instance of the write tag with 2 attrs class.
    /// </summary>
    /// <param name="XmlTemplates.Xml">The xml templates xml.</param>
    public void WriteTagWith2Attrs(string tagName, string firstName, string firstValue, string secondName, string secondValue)
    {
        var result = string.Format("<{0} {1}=\"{2}\" {3}=\"{4}\">", tagName, firstName, firstValue, secondName, secondValue);
        if (isUsingStack) stack?.Push(result);
        StringBuilder.Append(result);
    }

    /// <summary>
    /// Initializes a new instance of the write non pair tag class.
    /// </summary>
    /// <param name="result">The result.</param>
    public void WriteNonPairTag(string tagName)
    {
        StringBuilder.AppendFormat("<{0} />", tagName);
    }
}
