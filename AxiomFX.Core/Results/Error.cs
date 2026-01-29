namespace AxiomFX.Core.Results;

/// <summary>
/// Represents a structured error used by the AxiomFX result system.
/// Errors contain a code, a human-readable message, and an optional exception.
/// </summary>
public sealed class Error
{
    /// <summary>
    /// A machine-readable error code.
    /// Typicall formatted as: "Category.Subcategory.Code"
    /// Example: "Config.MissingKey", "Db.ConnectionFailed"
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// A human-readable descrption of the error.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// An optional exception associated with the error.
    /// Useful for diagnostics, logging, and debugging.
    /// </summary>
    public Exception? Exception { get; }

    public Error(string code, string message, Exception? exception = null)
    {
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Message = message ?? throw new ArgumentNullException(nameof(message));
        Exception = exception;
    }

    /// <summary>
    /// Creates a new error with the same code and message but a different exception.
    /// Useful when wrapping exceptions without losing semantic meaning.
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    public Error WithException(Exception exception) => new(Code, Message, Exception);

    /// <summary>
    /// Creates a new error with the same code and exception but a different message.
    /// Useful for adding context.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Exception is null
        ? $"{Code}: {Message}"
        : $"{Code}: {Message} ({Exception.GetType().Name} : {Exception.Message})";
}