using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AxiomFX.Core.Abstractions;

/// <summary>
/// Provides a scoped environment for module initialization.
/// Each module receives its own context during startup,
/// allowing it to register services, read config,
/// and interact with the application environment.
/// </summary>
public interface IModuleContext
{
    /// <summary>
    /// The root application context.
    /// Gives modules access to services, config, logging,
    /// features, and application-wide cancellation tokens.
    /// </summary>
    IAppContext App { get; }

    /// <summary>
    /// The service collection used during application composition.
    /// Modules may register services, options, and factories here.
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    /// The configuration subtree associated with this module.
    /// Typically maps to: Configuration["Modules:{ModuleName:*"]
    /// </summary>
    IConfiguration Configuration { get; }

    /// <summary>
    /// A unique identifier for this module instance.
    /// Useful for diagnostics, logging, and plugin metadata.
    /// </summary>
    string ModuleId { get; }
}