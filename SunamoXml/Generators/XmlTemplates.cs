namespace SunamoXml.Generators;

/// <summary>
/// Provides XML template strings and methods for creating XML document wrappers.
/// </summary>
public static class XmlTemplates
{
    /// <summary>
    /// Standard XML declaration: &lt;?xml version='1.0' encoding='utf-8'?&gt;
    /// </summary>
    public const string Xml = "<?xml version='1.0' encoding='utf-8'?>";

    /// <summary>
    /// Creates an XML wrapper with two CDATA values.
    /// </summary>
    /// <param name="value1">The first value to wrap in CDATA.</param>
    /// <param name="value2">The second value to wrap in CDATA.</param>
    public static string GetXml2(string value1, string value2)
    {
        return "<sunamo><n1><![CDATA[" + value1 + "]]></n1><n2><![CDATA[" + value2 + "]]></n2></sunamo>";
    }

    /// <summary>
    /// Creates an XML wrapper with five CDATA values.
    /// </summary>
    /// <param name="value1">The first value to wrap in CDATA.</param>
    /// <param name="value2">The second value to wrap in CDATA.</param>
    /// <param name="value3">The third value to wrap in CDATA.</param>
    /// <param name="value4">The fourth value to wrap in CDATA.</param>
    /// <param name="value5">The fifth value to wrap in CDATA.</param>
    public static string GetXml5(string value1, string value2, string value3, string value4, string value5)
    {
        return "<sunamo><n1><![CDATA[" + value1 + "]]></n1><n2><![CDATA[" + value2 + "]]></n2><n3><![CDATA[" + value3 +
               "]]></n3><n4><![CDATA[" + value4 + "]]></n4><n5><![CDATA[" + value5 + "]]></n5></sunamo>";
    }

    /// <summary>
    /// Creates an XML wrapper with four CDATA values.
    /// </summary>
    /// <param name="value1">The first value to wrap in CDATA.</param>
    /// <param name="value2">The second value to wrap in CDATA.</param>
    /// <param name="value3">The third value to wrap in CDATA.</param>
    /// <param name="value4">The fourth value to wrap in CDATA.</param>
    public static string GetXml4(string value1, string value2, string value3, string value4)
    {
        return "<sunamo><n1><![CDATA[" + value1 + "]]></n1><n2><![CDATA[" + value2 + "]]></n2><n3><![CDATA[" + value3 +
               "]]></n3><n4><![CDATA[" + value4 + "]]></n4></sunamo>";
    }

    /// <summary>
    /// Creates an XML wrapper with three CDATA values.
    /// </summary>
    /// <param name="value1">The first value to wrap in CDATA.</param>
    /// <param name="value2">The second value to wrap in CDATA.</param>
    /// <param name="value3">The third value to wrap in CDATA.</param>
    public static string GetXml3(string value1, string value2, string value3)
    {
        return "<sunamo><n1><![CDATA[" + value1 + "]]></n1><n2><![CDATA[" + value2 + "]]></n2><n3><![CDATA[" + value3 +
               "]]></n3></sunamo>";
    }

    /// <summary>
    /// Creates an XML wrapper with one CDATA value.
    /// </summary>
    /// <param name="value1">The value to wrap in CDATA.</param>
    public static string GetXml1(string value1)
    {
        return "<sunamo><n1><![CDATA[" + value1 + "]]></n1></sunamo>";
    }
}
