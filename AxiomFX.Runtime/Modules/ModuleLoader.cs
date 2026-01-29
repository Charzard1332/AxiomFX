using AxiomFX.Core;
using Microsoft.Extensions.Logging;

namespace AxiomFX.Runtime.Modules;

/// <summary>
/// Responsible for initializing all registered modules in the application.
/// Ensures modules are initialized in order and provides hooks for future
/// module discovery, dependency resolution, and hot-reload support.
/// </summary>
public sealed class ModuleLoader
{
    private readonly IReadOnlyList<IModule> _modules;

    public ModuleLoader(IEnumerable<IModule> modules)
    {
        _modules = modules.ToList();
    }

    /// <summary>
    /// Initializes all modules sequentially.
    /// Each module receives the application context and a cancellation token.
    /// </summary>
    public async Task InitializeModulesAsync(IAppContext context, CancellationToken cancellationToken)
    {
        var logger = context.LoggerFactory.CreateLogger<ModuleLoader>();

        if (_modules.Count == 0)
        {
            logger.LogDebug("No modules registered.");
            return;
        }

        logger.LogInformation("Initializing {Count} module(s)...", _modules.Count);

        foreach (var module in _modules)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                logger.LogInformation("Initializing module: {Module}", module.GetType().Name);
                await module.InitializeAsync(context, cancellationToken);
                logger.LogInformation("Module initialized: {Module}", module.GetType().Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Module initialization failed: {Module}", module.GetType().Name);
                throw;
            }
        }

        logger.LogInformation("All modules initialized successfully.");
    }
}