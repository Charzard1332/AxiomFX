namespace AxiomFX.Hosting;

/// <summary>
/// Represents environment information for an AxiomFX application.
/// </summary>
public interface IHostEnvironment
{
    /// <summary>
    /// The friendly name of the application.
    /// </summary>
    string ApplicationName { get; }

    /// <summary>
    /// The environment name (e.g., Development, Production, Staging).
    /// </summary>
    string EnvironmentName { get; }

    /// <summary>
    /// The root directory of the application.
    /// </summary>
    string ContentRootPath { get; }

    /// <summary>
    /// A unique identifier for this running instance of the application.
    /// </summary>
    string InstanceId { get; }
}