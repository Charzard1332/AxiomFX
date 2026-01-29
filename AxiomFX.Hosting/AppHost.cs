using AxiomFX.Core;
using AxiomFX.Core.Abstractions;
using AxiomFX.Core.Utilities;

using AxiomFX.Runtime.Background;
using AxiomFX.Runtime.Lifecycle;
using AxiomFX.Runtime.Modules;
using AxiomFX.Runtime.Startup;

using Microsoft.Extensions.Logging;

namespace AxiomFX.Runtime.Hosting;

/// <summary>
/// The internal runtime host responsible for orchestrating the
/// application lifecycle, module initialization, startup pipeline,
/// background tasks, and graceful shutdown.
/// </summary>
public sealed class AppHost
{
    private readonly IServiceProvider _services;
    private readonly IAppContext _context;
    private readonly CancellationHub _cancellationHub;

    private readonly AppLifecycleManager _lifecycle;
    private readonly BackgroundTaskRunner _backgroundRunner;
    private readonly ModuleLoader _moduleLoader;

    private readonly Func<IAppContext, CancellationToken, Task> _startupPipeline;

    private readonly ILogger<AppHost> _logger;

    public AppHost(
        IServiceProvider services,
        IAppContext context,
        CancellationHub cancellationHub,
        AppLifecycleManager lifecycle,
        BackgroundTaskRunner backgroundRunner,
        ModuleLoader moduleLoader,
        Func<IAppContext, CancellationToken, Task> startupPipeline)
    {
        _services = services;
        _context = context;
        _cancellationHub = cancellationHub;

        _lifecycle = lifecycle;
        _backgroundRunner = backgroundRunner;
        _moduleLoader = moduleLoader;

        _startupPipeline = startupPipeline;

        _logger = context.LoggerFactory.CreateLogger<AppHost>();
    }

    /// <summary>
    /// Starts the application host and all runtime subsystems.
    /// </summary>
    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("AxiomFX AppHost starting...");

        // 1. Notify lifecycle handlers
        await _lifecycle.OnStartingAsync(cancellationToken);

        // 2. Initialize modules
        await _moduleLoader.InitializeModulesAsync(_context, cancellationToken);

        // 3. Execute startup pipeline
        await _startupPipeline(_context, cancellationToken);

        // 4. Notify lifecycle handlers
        await _lifecycle.OnStartedAsync(cancellationToken);

        // 5. Start background tasks
        await _backgroundRunner.StartAsync(_context, cancellationToken);

        _logger.LogInformation("AxiomFX AppHost started successfully.");
    }

    /// <summary>
    /// Requests shutdown and waits for all subsystems to stop.
    /// </summary>
    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("AxiomFX AppHost stopping...");

        // 1. Trigger global cancellation
        _cancellationHub.Cancel();

        // 2. Notify lifecycle handlers
        await _lifecycle.OnStoppingAsync(cancellationToken);

        // 3. Stop background tasks
        await _backgroundRunner.StopAsync();

        // 4. Notify lifecycle handlers
        await _lifecycle.OnStoppedAsync(cancellationToken);

        _logger.LogInformation("AxiomFX AppHost stopped.");
    }
}