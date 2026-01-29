using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AxiomFX.Core.Features;

namespace AxiomFX.Core.Abstractions;

/// <summary>
/// Provides access to the core runtime environment of an AxiomFX application.
/// This context is created by the hosting layer and shared across the entire app.
/// </summary>
public interface IAppContext
{
    /// <summary>
    /// The root service provider for the application.
    /// All dependency-injected services are resolved from here.
    /// </summary>
    IServiceProvider Services { get; }

    /// <summary>
    /// The merged configuration for the application.
    /// Includes JSON, environment variables, user secrets, and any custom providers.
    /// </summary>
    IConfiguration Configuration { get; }

    /// <summary>
    /// The logging factory used to create loggers throught the application.
    /// </summary>
    ILoggerFactory LoggerFactory { get; }

    /// <summary>
    /// A collection of optional runtime features that extend the application's capabilities.
    /// Features may include metrics, tracing, hot-reload, plugin metadata, etc.
    /// </summary>
    FeatureCollection Features { get; }

    /// <summary>
    /// Provides access to application-wide cancellation tokens.
    /// Useful for long-running serivces, background tasks, and graceful shutdown
    /// </summary>
    CancellationToken ApplicationStopping { get; }

    /// <summary>
    /// Provides a unique identifier for this running instance of the application.
    /// Useful for diagnostics, clustering, and distributed tracing.
    /// </summary>
    string InstanceId { get; }
}