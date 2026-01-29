namespace AxiomFX.Core.Results;

/// <summary>
/// Represents the outcome of an operation that may succeed or fail.
/// Contains no value payload. For value-carrying results, use <see cref="Result{T}"/>.
/// </summary>
public readonly struct Result
{
    /// <summary>
    /// Indicates whether the operation succeeded.
    /// </summary>
    public bool Success { get; }

    /// <summary>
    /// Contains error information when the operation fails.
    /// </summary>
    public Error? Error { get; }

    private Result(bool success, Error? error)
    {
        Success = success;
        Error = error;
    }

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    /// <returns></returns>
    public static Result Ok() => new(true, null);

    /// <summary>
    /// Create a failed result with the specified error.
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Result Fail(Error error)
    {
        if (error is null)
            throw new ArgumentNullException(nameof(error));
        return new Result(false, error);
    }


    /// <summary>
    /// Implicit conversion from <see cref="Error"/> to a failed <see cref="Result"/>.
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static implicit operator Result(Error error) => Fail(error);

    /// <summary>
    /// Throws the contained error as an exception if the result is a failure.
    /// Useful for bridging imperative and result-based code.
    /// </summary>
    public void ThrowIfFailed()
    {
        if (!Success)
            throw Error!.Exception ?? new InvalidOperationException(Error.ToString());
    }

    public override string ToString() => Success ? "Success" : $"Failure: {Error}";
}