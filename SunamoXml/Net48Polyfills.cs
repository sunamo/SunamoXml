// Kompatibilita pro net48/netstandard2.0 - File async API pridano az v .NET Core (stejny vzor jako SunamoFileSystem/Net48Polyfills.cs).
namespace SunamoXml;

internal static class FileAsync
{
#if NET5_0_OR_GREATER
    internal static System.Threading.Tasks.Task<string> ReadAllTextAsync(string path)
        => System.IO.File.ReadAllTextAsync(path);

    internal static System.Threading.Tasks.Task WriteAllTextAsync(string path, string? contents)
        => System.IO.File.WriteAllTextAsync(path, contents);
#else
    internal static async System.Threading.Tasks.Task<string> ReadAllTextAsync(string path)
    {
        using var reader = new System.IO.StreamReader(path);
        return await reader.ReadToEndAsync().ConfigureAwait(false);
    }

    internal static async System.Threading.Tasks.Task WriteAllTextAsync(string path, string? contents)
    {
        using var writer = new System.IO.StreamWriter(path, false);
        await writer.WriteAsync(contents ?? string.Empty).ConfigureAwait(false);
    }
#endif
}
