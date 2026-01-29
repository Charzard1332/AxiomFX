namespace AxiomFX.Core.Results;

/// <summary>
/// Represents a strongly-typed, structured error code.
/// Error codes are lightweight identifiers used to categorize and
/// classify errors across the framework and modules.
/// </summary>
public readonly struct ErrorCode : IEquatable<ErrorCode>
{
    /// <summary>
    /// The machine-readable identifier for the error code.
    /// Example: "Config.MissingKey", "Db.ConnectionFailed"
    /// </summary>
    public string Value { get; }

    public ErrorCode(string value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Implicit conversion from string to ErrorCode.
    /// Allows writing: ErrorCode code = "Config.MissingKey";
    /// </summary>
    public static implicit operator ErrorCode(string value) => new(value);

    /// <summary>
    /// Returns the string representation of the error code.
    /// </summary>
    public override string ToString() => Value;

    public bool Equals(ErrorCode other) => Value == other.Value;
    public override bool Equals(object? obj) => obj is ErrorCode other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(ErrorCode left, ErrorCode right) => left.Equals(right);
    public static bool operator !=(ErrorCode left, ErrorCode right) => !left.Equals(right);
}