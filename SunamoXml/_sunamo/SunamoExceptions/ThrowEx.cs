namespace SunamoXml._sunamo.SunamoExceptions;

/// <summary>
/// Provides methods to validate conditions and throw exceptions with contextual information.
/// </summary>
internal partial class ThrowEx
{
    /// <summary>
    /// Throws if the collection has an odd number of elements.
    /// </summary>
    /// <param name="listName">The name of the collection being validated.</param>
    /// <param name="collection">The collection to validate.</param>
    internal static bool HasOddNumberOfElements(string listName, ICollection collection)
    {
        var exceptionFactory = Exceptions.HasOddNumberOfElements;
        return ThrowIsNotNull(exceptionFactory, listName, collection);
    }

    /// <summary>
    /// Throws an exception with a custom message.
    /// </summary>
    /// <param name="message">The primary message.</param>
    /// <param name="isReallyThrowing">Whether to actually throw the exception.</param>
    /// <param name="secondMessage">An optional secondary message appended to the primary.</param>
    internal static bool Custom(string message, bool isReallyThrowing = true, string secondMessage = "")
    {
        string joined = string.Join(" ", message, secondMessage);
        string? exceptionMessage = Exceptions.Custom(FullNameOfExecutedCode(), joined);
        return ThrowIsNotNull(exceptionMessage, isReallyThrowing);
    }

    /// <summary>
    /// Throws an exception containing the full exception text including inner exceptions.
    /// </summary>
    /// <param name="exception">The exception to format and throw.</param>
    internal static bool CustomWithStackTrace(Exception exception) { return Custom(Exceptions.TextOfExceptions(exception)); }

    /// <summary>
    /// Throws if the specified variable is null.
    /// </summary>
    /// <param name="variableName">The name of the variable being checked.</param>
    /// <param name="variable">The variable to check for null.</param>
    internal static bool IsNull(string variableName, object? variable = null)
    { return ThrowIsNotNull(Exceptions.IsNull(FullNameOfExecutedCode(), variableName, variable)); }

    /// <summary>
    /// Throws a "not implemented method" exception.
    /// </summary>
    internal static bool NotImplementedMethod() { return ThrowIsNotNull(Exceptions.NotImplementedMethod); }

    /// <summary>
    /// Returns the fully qualified name of the currently executing code location.
    /// </summary>
    internal static string FullNameOfExecutedCode()
    {
        Tuple<string, string, string> placeOfException = Exceptions.PlaceOfException();
        string fullName = FullNameOfExecutedCode(placeOfException.Item1, placeOfException.Item2, true);
        return fullName;
    }

    /// <summary>
    /// Returns the fully qualified name from type and method information.
    /// </summary>
    /// <param name="type">The type object (can be Type, MethodBase, string, or any object).</param>
    /// <param name="methodName">The method name.</param>
    /// <param name="isFromThrowEx">Whether the call originates from ThrowEx, affecting stack depth.</param>
    private static string FullNameOfExecutedCode(object type, string methodName, bool isFromThrowEx = false)
    {
        if (methodName == null)
        {
            int depth = 2;
            if (isFromThrowEx)
            {
                depth++;
            }

            methodName = Exceptions.CallingMethod(depth);
        }
        string typeFullName;
        if (type is Type resolvedType)
        {
            typeFullName = resolvedType.FullName ?? "Type cannot be get via type is Type type2";
        }
        else if (type is MethodBase method)
        {
            typeFullName = method.ReflectedType?.FullName ?? "Type cannot be get via type is MethodBase method";
            methodName = method.Name;
        }
        else if (type is string)
        {
            typeFullName = type.ToString() ?? "Type cannot be get via type is string";
        }
        else
        {
            Type objectType = type.GetType();
            typeFullName = objectType.FullName ?? "Type cannot be get via type.GetType()";
        }
        return string.Concat(typeFullName, ".", methodName);
    }

    /// <summary>
    /// Throws an exception if the exception message is not null.
    /// </summary>
    /// <param name="exceptionMessage">The exception message to evaluate.</param>
    /// <param name="isReallyThrowing">Whether to actually throw the exception.</param>
    internal static bool ThrowIsNotNull(string? exceptionMessage, bool isReallyThrowing = true)
    {
        if (exceptionMessage != null)
        {
            Debugger.Break();
            if (isReallyThrowing)
            {
                throw new Exception(exceptionMessage);
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Evaluates a two-parameter exception factory and throws if the result is not null.
    /// </summary>
    /// <typeparam name="A">The type of the first argument.</typeparam>
    /// <typeparam name="B">The type of the second argument.</typeparam>
    /// <param name="exceptionFactory">The factory function that produces an exception message.</param>
    /// <param name="firstArgument">The first argument to the factory.</param>
    /// <param name="secondArgument">The second argument to the factory.</param>
    internal static bool ThrowIsNotNull<A, B>(Func<string, A, B, string?> exceptionFactory, A firstArgument, B secondArgument)
    {
        string? exceptionMessage = exceptionFactory(FullNameOfExecutedCode(), firstArgument, secondArgument);
        return ThrowIsNotNull(exceptionMessage);
    }

    /// <summary>
    /// Evaluates a parameterless exception factory and throws if the result is not null.
    /// </summary>
    /// <param name="exceptionFactory">The factory function that produces an exception message.</param>
    internal static bool ThrowIsNotNull(Func<string, string?> exceptionFactory)
    {
        string? exceptionMessage = exceptionFactory(FullNameOfExecutedCode());
        return ThrowIsNotNull(exceptionMessage);
    }
}
