namespace SunamoXml.Generators;

/// <summary>
///     Našel jsem ještě třídu DotXml ale ta umožňuje vytvářet jen dokumenty ke bude root ThisApp.Name
///     A nebo moje vlastní XML třídy, ale ty umí vytvářet jen třídy bez rozsáhlejšího xml vnoření.
///     Element - prvek kterému se zapisují ihned i innerObsah. Může být i prázdný.
///     Tag - prvek kterému to mohu zapsat později nebo vůbec.
/// </summary>
public class XmlGenerator //: IXmlGenerator
{
    private static Type type = typeof(XmlGenerator);
    private readonly Stack<string> _stack;
    private readonly bool _useStack;
    public StringBuilder stringBuilder = new();

    public XmlGenerator() : this(false)
    {
    }

    public XmlGenerator(bool useStack)
    {
        _useStack = useStack;
        if (useStack) _stack = new Stack<string>();
    }

    public int Length()
    {
        return stringBuilder.Length;
    }

    public void WriteNonPairTagWith2Attrs(string tagName, string firstName, string firstValue, string secondName, string secondValue)
    {
        stringBuilder.AppendFormat("<{0} {1}=\"{2}\" {3}=\"{4}\" />", tagName, firstName, firstValue, secondName, secondValue);
    }

    public void WriteNonPairTagWithAttr(string tagName, string attrName, string attrValue)
    {
        stringBuilder.AppendFormat("<{0} {1}=\"{2}\" />", tagName, attrName, attrValue);
    }

    public void Insert(int index, string text)
    {
        stringBuilder.Insert(index, text);
    }

    public void AppendLine()
    {
        stringBuilder.AppendLine();
    }

    public void StartComment()
    {
        stringBuilder.Append("<!--");
    }

    public void EndComment()
    {
        stringBuilder.Append("-->");
    }

    public void WriteNonPairTagWithAttrs(string tag, List<string> attributes)
    {
        WriteNonPairTagWithAttrs(tag, attributes.ToArray());
    }

    public void WriteNonPairTagWithAttrs(string tag, params string[] attributes)
    {
        stringBuilder.AppendFormat("<{0} ", tag);
        for (var i = 0; i < attributes.Length; i++)
        {
            var text = attributes[i];
            object value = attributes[++i];
            stringBuilder.AppendFormat("{0}=\"{1}\" ", text, value);
        }

        stringBuilder.Append(" />");
    }

    public void WriteCData(string innerCData)
    {
        WriteRaw( /*string.Format("<![CDATA[{0}]]>", innerCData)*/ string.Format("<![CDATA[{0}]]>", innerCData));
    }

    public void WriteTagWithAttr(string tag, string attributeName, string attributeValue, bool skipEmptyOrNull = false)
    {
        if (skipEmptyOrNull)
            if (string.IsNullOrWhiteSpace(attributeValue))
                return;
        var result = /*string.Format*/ string.Format("<{0} {1}=\"{2}\">", tag, attributeName, attributeValue);
        if (_useStack) _stack.Push(result);
        stringBuilder.Append(result);
    }

    public void WriteRaw(string parameter)
    {
        stringBuilder.Append(parameter);
    }

    public void TerminateTag(string parameter)
    {
        stringBuilder.AppendFormat("</{0}>", parameter);
    }

    public void WriteTag(string parameter)
    {
        var result = $"<{parameter}>";
        if (_useStack) _stack.Push(result);
        stringBuilder.Append(result);
    }

    public override string ToString()
    {
        return stringBuilder.ToString();
    }

    public void WriteTagWithAttrs(string tagName, List<string> attributes)
    {
        WriteTagWithAttrs(tagName, attributes.ToArray());
    }

    /// <summary>
    ///     if will be sth null, wont be writing
    /// </summary>
    /// <param name="tagName"></param>
    /// <param name="attributes"></param>
    public void WriteTagWithAttrs(string tagName, params string[] attributes)
    {
        WriteTagWithAttrs(true, tagName, attributes);
    }

    /// <summary>
    ///     Add also null
    /// </summary>
    /// <param name="nameTag"></param>
    /// <param name="attributes"></param>
    private void WriteTagWithAttrs(string nameTag, Dictionary<string, string> attributes)
    {
        WriteTagWithAttrs(true, nameTag, DictionaryHelper.GetListStringFromDictionary(attributes).ToArray());
    }

    /// <summary>
    ///     Dont write attr with null
    /// </summary>
    /// <param name="tagName"></param>
    /// <param name="attributes"></param>
    public void WriteTagWithAttrsCheckNull(string tagName, params string[] attributes)
    {
        WriteTagWithAttrs(false, tagName, attributes);
    }

    private bool IsNulledOrEmpty(string text)
    {
        if (string.IsNullOrEmpty(text) || text == "(null)") return true;
        return false;
    }

    public void WriteTagNamespaceManager(string nameTag, XmlNamespaceManager namespaceManager, params string[] attributes)
    {
        //List<string> parameter = new List<string>(attributes);
        var dictionary = XHelper.XmlNamespaces(namespaceManager, true);
        for (var i = 0; i < attributes.Count(); i++) dictionary.Add(attributes[i], attributes[++i]);
        WriteTagWithAttrs(nameTag, dictionary);
    }

    public void WriteNonPairTagWithAttrs(bool appendNull, string tagName, params string[] attributes)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendFormat("<{0} ", tagName);
        for (var i = 0; i < attributes.Length; i++)
        {
            var attr = attributes[i];
            var val = attributes[++i];
            if ((string.IsNullOrEmpty(val) && appendNull) || !string.IsNullOrEmpty(val))
                if ((!IsNulledOrEmpty(attr) && appendNull) || !IsNulledOrEmpty(val))
                    stringBuilder.AppendFormat("{0}=\"{1}\" ", attr, val);
        }

        stringBuilder.Append(" /");
        stringBuilder.Append(">");
        var result = stringBuilder.ToString();
        if (_useStack) _stack.Push(result);
        this.stringBuilder.Append(result);
    }

    /// <summary>
    ///     if will be sth null, wont be writing
    /// </summary>
    /// <param name="appendNull"></param>
    /// <param name="tagName"></param>
    /// <param name="attributes"></param>
    private void WriteTagWithAttrs(bool appendNull, string tagName, params string[] attributes)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendFormat("<{0} ", tagName);
        for (var i = 0; i < attributes.Length; i++)
        {
            var attr = attributes[i];
            var val = attributes[++i];
            if ((string.IsNullOrEmpty(val) && appendNull) || !string.IsNullOrEmpty(val))
                if ((!IsNulledOrEmpty(attr) && appendNull) || !IsNulledOrEmpty(val))
                    stringBuilder.AppendFormat("{0}=\"{1}\" ", attr, val);
        }

        stringBuilder.Append(">");
        var result = stringBuilder.ToString();
        if (_useStack) _stack.Push(result);
        this.stringBuilder.Append(result);
    }

    public void WriteElement(string elementName, string inner)
    {
        stringBuilder.AppendFormat("<{0}>{1}</{0}>", elementName, inner);
    }

    public void WriteXmlDeclaration()
    {
        stringBuilder.Append(XmlTemplates.xml);
    }

    public void WriteTagWith2Attrs(string tagName, string firstName, string firstValue, string secondName, string secondValue)
    {
        var result = /*string.Format*/ string.Format("<{0} {1}=\"{2}\" {3}=\"{4}\">", tagName, firstName, firstValue, secondName, secondValue);
        if (_useStack) _stack.Push(result);
        stringBuilder.Append(result);
    }

    public void WriteNonPairTag(string tagName)
    {
        stringBuilder.AppendFormat("<{0} />", tagName);
    }
}