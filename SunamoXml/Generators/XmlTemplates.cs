namespace SunamoXml.Generators;

/// <summary>
/// Represents the xml templates type.
/// </summary>
public static class XmlTemplates
{
    /// <summary>
    /// The xml constant.
    /// </summary>
    public const string Xml = "<?xml version='1.0' encoding='utf-8'?>";

    /// <summary>
    /// Get xml 2.
    /// </summary>
    /// <param name="value1">The value 1.</param>
    /// <param name="value2">The value 2.</param>
    /// <returns>The get xml 2 result.</returns>
    public static string GetXml2(string value1, string value2)
    {
        return "<sunamo><n1><![CDATA[" + value1 + "]]></n1><n2><![CDATA[" + value2 + "]]></n2></sunamo>";
    }

    /// <summary>
    /// Get xml 5.
    /// </summary>
    /// <param name="value1">The value 1.</param>
    /// <param name="value2">The value 2.</param>
    /// <param name="value3">The value 3.</param>
    /// <param name="value4">The value 4.</param>
    /// <param name="value5">The value 5.</param>
    /// <returns>The get xml 5 result.</returns>
    public static string GetXml5(string value1, string value2, string value3, string value4, string value5)
    {
        return "<sunamo><n1><![CDATA[" + value1 + "]]></n1><n2><![CDATA[" + value2 + "]]></n2><n3><![CDATA[" + value3 +
               "]]></n3><n4><![CDATA[" + value4 + "]]></n4><n5><![CDATA[" + value5 + "]]></n5></sunamo>";
    }

    /// <summary>
    /// Get xml 4.
    /// </summary>
    /// <param name="value1">The value 1.</param>
    /// <param name="value2">The value 2.</param>
    /// <param name="value3">The value 3.</param>
    /// <param name="value4">The value 4.</param>
    /// <returns>The get xml 4 result.</returns>
    public static string GetXml4(string value1, string value2, string value3, string value4)
    {
        return "<sunamo><n1><![CDATA[" + value1 + "]]></n1><n2><![CDATA[" + value2 + "]]></n2><n3><![CDATA[" + value3 +
               "]]></n3><n4><![CDATA[" + value4 + "]]></n4></sunamo>";
    }

    /// <summary>
    /// Get xml 3.
    /// </summary>
    /// <param name="value1">The value 1.</param>
    /// <param name="value2">The value 2.</param>
    /// <param name="value3">The value 3.</param>
    /// <returns>The get xml 3 result.</returns>
    public static string GetXml3(string value1, string value2, string value3)
    {
        return "<sunamo><n1><![CDATA[" + value1 + "]]></n1><n2><![CDATA[" + value2 + "]]></n2><n3><![CDATA[" + value3 +
               "]]></n3></sunamo>";
    }

    /// <summary>
    /// Get xml 1.
    /// </summary>
    /// <param name="value1">The value 1.</param>
    /// <returns>The get xml 1 result.</returns>
    public static string GetXml1(string value1)
    {
        return "<sunamo><n1><![CDATA[" + value1 + "]]></n1></sunamo>";
    }
}
