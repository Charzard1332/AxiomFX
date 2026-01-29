namespace AxiomFX.Runtime.Hosting;

/// <summary>
/// Provides configuration options for the AxiomFX application host.
/// These options influence how the runtime initializes, loads modules,
/// and manages application behavior.
/// </summary>
public sealed class AppOptions
{
    /// <summary>
    /// The application name used for logging, diagnostics, and metadata.
    /// Defaults to "AxiomFX Application".
    /// </summary>
    public string ApplicationName { get; set; } = "AxiomFX Application";

    /// <summary>
    /// Enables or disables automatic module initialization during startup.
    /// Defaults to true.
    /// </summary>
    public bool AutoInitializeModules { get; set; } = true;

    /// <summary>
    /// Enables or disables automatic discovery of modules via reflection.
    /// Defaults to false. When enabled, the runtime will scan assemblies
    /// for types implementing IModule.
    /// </summary>
    public bool EnableModuleDiscovery { get; set; } = false;

    /// <summary>
    /// When true, background tasks will start automatically after the
    /// application has completed its startup pipeline.
    /// Defaults to true.
    /// </summary>
    public bool AutoStartBackgroundTasks { get; set; } = true;

    /// <summary>
    /// Optional path for module discovery or plugin loading.
    /// Not used unless EnableModuleDiscovery is true.
    /// </summary>
    public string? ModuleSearchPath { get; set; }

    /// <summary>
    /// Optional environment name (e.g., Development, Production).
    /// Useful for diagnostics, logging, and conditional module behavior.
    /// </summary>
    public string Environment { get; set; } = "Production";
}