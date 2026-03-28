namespace SunamoXml._sunamo.SunamoExceptions;

/// <summary>
/// Provides methods for creating exception messages and inspecting the call stack.
/// </summary>
internal sealed partial class Exceptions
{
    /// <summary>
    /// Prepends a caller context prefix to a message if the prefix is not empty.
    /// </summary>
    /// <param name="before">The caller context prefix.</param>
    internal static string CheckBefore(string before)
    {
        return string.IsNullOrWhiteSpace(before) ? string.Empty : before + ": ";
    }

    /// <summary>
    /// Returns a formatted string containing all exception messages from the exception chain.
    /// </summary>
    /// <param name="exception">The exception to format.</param>
    /// <param name="isIncludingInnerExceptions">Whether to include inner exception messages.</param>
    internal static string TextOfExceptions(Exception exception, bool isIncludingInnerExceptions = true)
    {
        if (exception == null) return string.Empty;
        StringBuilder stringBuilder = new();
        stringBuilder.Append("Exception:");
        stringBuilder.AppendLine(exception.Message);
        if (isIncludingInnerExceptions)
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
                stringBuilder.AppendLine(exception.Message);
            }
        var result = stringBuilder.ToString();
        return result;
    }

    /// <summary>
    /// Inspects the call stack and returns the type name, method name, and full stack trace.
    /// </summary>
    /// <param name="isFillAlsoFirstTwo">Whether to also extract the type and method name from the first non-ThrowEx frame.</param>
    internal static Tuple<string, string, string> PlaceOfException(bool isFillAlsoFirstTwo = true)
    {
        StackTrace stackTrace = new();
        var stackTraceText = stackTrace.ToString();
        var lines = stackTraceText.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
        lines.RemoveAt(0);
        var i = 0;
        string typeName = string.Empty;
        string methodName = string.Empty;
        for (; i < lines.Count; i++)
        {
            var line = lines[i];
            if (isFillAlsoFirstTwo)
                if (!line.StartsWith("   at ThrowEx"))
                {
                    TypeAndMethodName(line, out typeName, out methodName);
                    isFillAlsoFirstTwo = false;
                }
            if (line.StartsWith("at System."))
            {
                lines.Add(string.Empty);
                lines.Add(string.Empty);
                break;
            }
        }
        return new Tuple<string, string, string>(typeName, methodName, string.Join(Environment.NewLine, lines));
    }

    /// <summary>
    /// Extracts the type name and method name from a stack trace line.
    /// </summary>
    /// <param name="line">The stack trace line to parse.</param>
    /// <param name="typeName">The extracted type name.</param>
    /// <param name="methodName">The extracted method name.</param>
    internal static void TypeAndMethodName(string line, out string typeName, out string methodName)
    {
        var trimmedLine = line.Split("at ")[1].Trim();
        var fullName = trimmedLine.Split('(')[0];
        var parts = fullName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        methodName = parts[^1];
        parts.RemoveAt(parts.Count - 1);
        typeName = string.Join(".", parts);
    }

    /// <summary>
    /// Returns the name of the calling method at the specified stack depth.
    /// </summary>
    /// <param name="depth">The stack frame depth to inspect.</param>
    internal static string CallingMethod(int depth = 1)
    {
        StackTrace stackTrace = new();
        var methodBase = stackTrace.GetFrame(depth)?.GetMethod();
        if (methodBase == null)
        {
            return "Method name cannot be get";
        }
        var methodName = methodBase.Name;
        return methodName;
    }

    /// <summary>
    /// Creates a custom exception message with an optional caller context prefix.
    /// </summary>
    /// <param name="before">The caller context prefix.</param>
    /// <param name="message">The exception message.</param>
    internal static string? Custom(string before, string message)
    {
        return CheckBefore(before) + message;
    }

    /// <summary>
    /// Creates a "not implemented method" exception message.
    /// </summary>
    /// <param name="before">The caller context prefix.</param>
    internal static string? NotImplementedMethod(string before)
    {
        return CheckBefore(before) + "Not implemented method.";
    }

    /// <summary>
    /// Returns an error message if the collection has an odd number of elements, otherwise null.
    /// </summary>
    /// <param name="before">The caller context prefix.</param>
    /// <param name="listName">The name of the collection being validated.</param>
    /// <param name="collection">The collection to validate.</param>
    internal static string? HasOddNumberOfElements(string before, string listName, ICollection collection)
    {
        return collection.Count % 2 == 1 ? CheckBefore(before) + listName + " has odd number of elements " + collection.Count : null;
    }

    /// <summary>
    /// Returns an error message if the variable is null, otherwise null.
    /// </summary>
    /// <param name="before">The caller context prefix.</param>
    /// <param name="variableName">The name of the variable being checked.</param>
    /// <param name="variable">The variable to check for null.</param>
    internal static string? IsNull(string before, string variableName, object? variable)
    {
        return variable == null ? CheckBefore(before) + variableName + " " + "is null" + "." : null;
    }
}
