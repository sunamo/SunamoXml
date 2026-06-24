namespace SunamoXml.Generators;

public static class XmlTemplates
{
    public const string Xml = "<?xml version='1.0' encoding='utf-8'?>";

    public static string GetXml2(string value1, string value2)
    {
        return "<sunamo><n1><![CDATA[" + value1 + "]]></n1><n2><![CDATA[" + value2 + "]]></n2></sunamo>";
    }

    public static string GetXml5(string value1, string value2, string value3, string value4, string value5)
    {
        return "<sunamo><n1><![CDATA[" + value1 + "]]></n1><n2><![CDATA[" + value2 + "]]></n2><n3><![CDATA[" + value3 +
               "]]></n3><n4><![CDATA[" + value4 + "]]></n4><n5><![CDATA[" + value5 + "]]></n5></sunamo>";
    }

    public static string GetXml4(string value1, string value2, string value3, string value4)
    {
        return "<sunamo><n1><![CDATA[" + value1 + "]]></n1><n2><![CDATA[" + value2 + "]]></n2><n3><![CDATA[" + value3 +
               "]]></n3><n4><![CDATA[" + value4 + "]]></n4></sunamo>";
    }

    public static string GetXml3(string value1, string value2, string value3)
    {
        return "<sunamo><n1><![CDATA[" + value1 + "]]></n1><n2><![CDATA[" + value2 + "]]></n2><n3><![CDATA[" + value3 +
               "]]></n3></sunamo>";
    }

    public static string GetXml1(string value1)
    {
        return "<sunamo><n1><![CDATA[" + value1 + "]]></n1></sunamo>";
    }
}
