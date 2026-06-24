namespace SunamoXml.Generators;

public class XmlGenerator
{
    private readonly Stack<string>? stack;
    private readonly bool isUsingStack;

    public StringBuilder StringBuilder { get; set; } = new();

    public XmlGenerator() : this(false)
    {
    }

    public XmlGenerator(bool isUsingStack)
    {
        this.isUsingStack = isUsingStack;
        if (isUsingStack) stack = new Stack<string>();
    }

    public int Length()
    {
        return StringBuilder.Length;
    }

    public void WriteNonPairTagWith2Attrs(string tagName, string firstName, string firstValue, string secondName, string secondValue)
    {
        StringBuilder.AppendFormat("<{0} {1}=\"{2}\" {3}=\"{4}\" />", tagName, firstName, firstValue, secondName, secondValue);
    }

    public void WriteNonPairTagWithAttr(string tagName, string attrName, string attrValue)
    {
        StringBuilder.AppendFormat("<{0} {1}=\"{2}\" />", tagName, attrName, attrValue);
    }

    public void Insert(int index, string text)
    {
        StringBuilder.Insert(index, text);
    }

    public void AppendLine()
    {
        StringBuilder.AppendLine();
    }

    public void StartComment()
    {
        StringBuilder.Append("<!--");
    }

    public void EndComment()
    {
        StringBuilder.Append("-->");
    }

    public void WriteNonPairTagWithAttrs(string tagName, List<string> attributes)
    {
        WriteNonPairTagWithAttrs(tagName, attributes.ToArray());
    }

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

    public void WriteCData(string innerCData)
    {
        WriteRaw(string.Format("<![CDATA[{0}]]>", innerCData));
    }

    public void WriteTagWithAttr(string tagName, string attributeName, string attributeValue, bool isSkippingEmptyOrNull = false)
    {
        if (isSkippingEmptyOrNull)
            if (string.IsNullOrWhiteSpace(attributeValue))
                return;
        var result = string.Format("<{0} {1}=\"{2}\">", tagName, attributeName, attributeValue);
        if (isUsingStack) stack?.Push(result);
        StringBuilder.Append(result);
    }

    public void WriteRaw(string rawContent)
    {
        StringBuilder.Append(rawContent);
    }

    public void TerminateTag(string tagName)
    {
        StringBuilder.AppendFormat("</{0}>", tagName);
    }

    public void WriteTag(string tagName)
    {
        var result = $"<{tagName}>";
        if (isUsingStack) stack?.Push(result);
        StringBuilder.Append(result);
    }

    public override string ToString()
    {
        return StringBuilder.ToString();
    }

    public void WriteTagWithAttrs(string tagName, List<string> attributes)
    {
        WriteTagWithAttrs(tagName, attributes.ToArray());
    }

    public void WriteTagWithAttrs(string tagName, params string[] attributes)
    {
        WriteTagWithAttrs(true, tagName, attributes);
    }

    private void WriteTagWithAttrs(string tagName, Dictionary<string, string> attributes)
    {
        WriteTagWithAttrs(true, tagName, DictionaryHelper.GetListStringFromDictionary(attributes).ToArray());
    }

    public void WriteTagWithAttrsCheckNull(string tagName, params string[] attributes)
    {
        WriteTagWithAttrs(false, tagName, attributes);
    }

    private bool IsNulledOrEmpty(string text)
    {
        if (string.IsNullOrEmpty(text) || text == "(null)") return true;
        return false;
    }

    public void WriteTagNamespaceManager(string tagName, XmlNamespaceManager namespaceManager, params string[] attributes)
    {
        var dictionary = XHelper.XmlNamespaces(namespaceManager, true);
        for (var i = 0; i < attributes.Count(); i++) dictionary.Add(attributes[i], attributes[++i]);
        WriteTagWithAttrs(tagName, dictionary);
    }

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

    public void WriteElement(string elementName, string inner)
    {
        StringBuilder.AppendFormat("<{0}>{1}</{0}>", elementName, inner);
    }

    public void WriteXmlDeclaration()
    {
        StringBuilder.Append(XmlTemplates.Xml);
    }

    public void WriteTagWith2Attrs(string tagName, string firstName, string firstValue, string secondName, string secondValue)
    {
        var result = string.Format("<{0} {1}=\"{2}\" {3}=\"{4}\">", tagName, firstName, firstValue, secondName, secondValue);
        if (isUsingStack) stack?.Push(result);
        StringBuilder.Append(result);
    }

    public void WriteNonPairTag(string tagName)
    {
        StringBuilder.AppendFormat("<{0} />", tagName);
    }
}
