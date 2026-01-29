namespace AxiomFX.Hosting;

/// <summary>
/// Options that control the behavior of the AxiomFX hosting layer.
/// These settings apply to the public-facing host and influence
/// startup, shutdown, and environment behavior.
/// </summary>
public sealed class HostOptions
{
    /// <summary>
    /// The name of the application. Defaults to "AxiomFX Application".
    /// </summary>
    public string ApplicationName { get; set; } = "AxiomFX Application";

    /// <summary>
    /// The environment name (e.g., Development, Production, Staging).
    /// Defaults to "Production".
    /// </summary>
    public string EnvironmentName { get; set; } = "Production";

    /// <summary>
    /// The root directory of the application.
    /// Defaults to the current working directory.
    /// </summary>
    public string ContentRootPath { get; set; } = Directory.GetCurrentDirectory();

    /// <summary>
    /// Whether the host should stop automatically when the runtime
    /// cancellation token is triggered. Defaults to true.
    /// </summary>
    public bool StopOnShutdown { get; set; } = true;

    /// <summary>
    /// The timeout for graceful shutdown operations.
    /// Defaults to 5 seconds.
    /// </summary>
    public TimeSpan ShutdownTimeout { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Whether to validate the service provider on build.
    /// Useful for catching DI errors early.
    /// Defaults to false for performance.
    /// </summary>
    public bool ValidateServiceProvider { get; set; } = false;

    /// <summary>
    /// Whether to capture startup errors and continue running.
    /// Defaults to false (fail fast).
    /// </summary>
    public bool CaptureStartupErrors { get; set; } = false;
}