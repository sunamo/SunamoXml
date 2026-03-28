using System;

/// <summary>
/// Unit tests for <see cref="XHelper"/> XML helper methods.
/// </summary>
public class XHelperTests
{
    /// <summary>
    /// Tests that InnerTextOfNode correctly extracts the text content of a named descendant element.
    /// </summary>
    [Fact]
    public void InnerTextOfNode()
    {
        var expected = "1.40.2.1593";
        var xml = "<PackageReference Include=\"Google.Apis.YouTube.v3\"><Version>1.40.2.1593</Version></PackageReference>";
        var element = XElement.Parse(xml);

        var actual = XHelper.InnerTextOfNode(element, "Version");
        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Tests that Attr correctly returns the attribute value from an XElement.
    /// </summary>
    [Fact]
    public void Attr_ReturnsAttributeValue()
    {
        var xml = "<PackageReference Include=\"SunamoXml\" Version=\"1.0.0\" />";
        var element = XElement.Parse(xml);

        var result = XHelper.Attr(element, "Include");
        Assert.Equal("SunamoXml", result);
    }

    /// <summary>
    /// Tests that Attr returns null when the specified attribute does not exist.
    /// </summary>
    [Fact]
    public void Attr_ReturnsNull_WhenAttributeNotFound()
    {
        var xml = "<PackageReference Include=\"SunamoXml\" />";
        var element = XElement.Parse(xml);

        var result = XHelper.Attr(element, "NonExistent");
        Assert.Null(result);
    }

    /// <summary>
    /// Tests that Minify removes whitespace and newlines from XML content.
    /// </summary>
    [Fact]
    public void Minify_RemovesWhitespaceAndNewlines()
    {
        var xml = "<root>" + Environment.NewLine + "  <child>text</child>" + Environment.NewLine + "</root>";

        var result = XHelper.Minify(xml);
        Assert.Equal("<root><child>text</child></root>", result);
    }

    /// <summary>
    /// Tests that GetElementOfName finds a direct child element by tag name.
    /// </summary>
    [Fact]
    public void GetElementOfName_FindsDirectChild()
    {
        var xml = "<root><Version>1.0</Version><Name>Test</Name></root>";
        var element = XElement.Parse(xml);

        var result = XHelper.GetElementOfName(element, "Version");
        Assert.NotNull(result);
        Assert.Equal("1.0", result.Value);
    }

    /// <summary>
    /// Tests that GetElementOfName returns null when the element is not found.
    /// </summary>
    [Fact]
    public void GetElementOfName_ReturnsNull_WhenNotFound()
    {
        var xml = "<root><Version>1.0</Version></root>";
        var element = XElement.Parse(xml);

        var result = XHelper.GetElementOfName(element, "NonExistent");
        Assert.Null(result);
    }
}
