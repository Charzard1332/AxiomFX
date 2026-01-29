namespace AxiomFX.Core.Abstractions;

/// <summary>
/// Represents a self-contained, composable unit of application functionality.
/// Modules are discovered and initialized by the runtime during startup.
/// </summary>
public interface IModule
{
    /// <summary>
    /// The unique name of the module.
    /// Used for diagnostics, logging, and feature registration.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The semantic version of the module.
    /// Helps with compatibility, upgrades, and plugin metadata.
    /// </summary>
    Version Version { get; }

    /// <summary>
    /// Called during application startup to initialize the module.
    /// Modules may register services, configure options, or hook into the app lifecycle.
    /// </summary>
    /// <param name="context">The module context containing app services and configuration</param>
    /// <param name="cancellationToken">Token used to cancel startup</param>
    /// <returns></returns>
    Task InitializeAsync(IModuleContext context, CancellationToken cancellationToken = default);
}