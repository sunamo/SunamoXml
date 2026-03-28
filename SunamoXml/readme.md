# SunamoXml

A .NET library providing comprehensive XML manipulation utilities including parsing, formatting, generation, namespace management, and sanitization.

## Features

- **XML Parsing & Loading**: Load XML from strings or files with error handling (`XH.LoadXml`, `XmlHelper.CreateXmlDocument`)
- **XML Formatting**: Format and indent XML content in memory or files (`XHelper.FormatXml`, `XmlHelper.FormatXmlInMemory`)
- **XML Generation**: Fluent XML builder with tag, attribute, CDATA, and namespace support (`XmlGenerator`, `XmlGeneratorNS2`)
- **Namespace Management**: Add, remove, and manage XML namespaces (`XmlNamespacesHolder`, `XHelper.AddXmlNamespaces`)
- **Element Search**: Find elements by name, attribute, or namespace with recursive search (`XHelper.GetElementOfName`, `XHelper.GetElementsOfNameRecursive`)
- **XML Sanitization**: Remove illegal XML characters and fix ampersand entities (`XH.SanitizeXmlString`, `XH.ReplaceAmpInString`)
- **Minification**: Compact XML by removing whitespace and newlines (`XHelper.Minify`)

## Installation

```
dotnet add package SunamoXml
```

## Target Frameworks

`net10.0`, `net9.0`, `net8.0`

## Links

- [NuGet](https://www.nuget.org/profiles/sunamo)
- [GitHub](https://github.com/sunamo/PlatformIndependentNuGetPackages)
- [Developer site](https://sunamo.cz)

## License

MIT
