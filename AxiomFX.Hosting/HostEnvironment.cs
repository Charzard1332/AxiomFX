namespace AxiomFX.Hosting;

/// <summary>
/// Represents the environment in which an AxiomFX application is running.
/// This is the public-facing environment abstraction exposed by the hosting layer.
/// </summary>
public sealed class IHostEnvironment : IHostEnvironment
{
    /// <summary>
    /// The friendly name of the application.
    /// </summary>
    public string ApplicationName { get; }

    /// <summary>
    /// The environment name (e.g., Development, Production, Staging).
    /// </summary>
    public string EnvironmentName { get; }

    /// <summary>
    /// The root directory of the application.
    /// </summary>
    public string ContentRootPath { get; }

    /// <summary>
    /// A unique identifier for this running instance of the application.
    /// Useful for diagnostics, clustering, and distributed tracing.
    /// </summary>
    public string InstanceId { get; }

    public IHostEnvironment(
        string applicationName,
        string environmentName,
        string contentRootPath)
    {
        ApplicationName = applicationName;
        EnvironmentName = environmentName;
        ContentRootPath = contentRootPath;

        // Generate a unique instance ID for this host
        InstanceId = Guid.NewGuid().ToString("N");
    }
}