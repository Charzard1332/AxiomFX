using AxiomFX.Core;
using AxiomFX.Core.Features;
using AxiomFX.Core.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AxiomFX.Runtime.Hosting;

/// <summary>
/// Builds and configures an AxiomFX application host.
/// Responsible for constructing the DI container, loading modules,
/// building the startup pipeline, and wiring all runtime subsystems.
/// </summary>
public sealed class AppBuilder
{
    private readonly IServiceCollection _services = new ServiceCollection();
    private readonly List<IStartupFilter> _startupFilters = new();
    private readonly List<IModule> _modules = new();
    private readonly List<IBackgroundTask> _backgroundTasks = new();
    private readonly List<IAppLifecycle> _lifecycleHandlers = new();

    private IConfiguration? _configuration;
    private AppOptions _options = new();

    public AppBuilder ConfigureConfiguration(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        return this;
    }

    public AppBuilder ConfigureOptions(Action<AppOptions> configure)
    {
        configure?.Invoke(_options);
        return this;
    }

    public AppBuilder AddModule(IModule module)
    {
        _modules.Add(module);
        return this;
    }

    public AppBuilder AddStartupFilter(IStartupFilter filter)
    {
        _startupFilters.Add(filter);
        return this;
    }

    public AppBuilder AddBackgroundTask(IBackgroundTask task)
    {
        _backgroundTasks.Add(task);
        return this;
    }

    public AppBuilder AddLifecycleHandler(IAppLifecycle lifecycle)
    {
        _lifecycleHandlers.Add(lifecycle);
        return this;
    }

    public AppHost Build()
    {
        if (_configuration is null)
            throw new InvalidOperationException("Configuration must be provided before building the application.");

        // Core runtime services
        var cancellationHub = new CancellationHub();
        var features = new FeatureCollection();

        _services.AddSingleton<IConfiguration>(_configuration);
        _services.AddSingleton(cancellationHub);
        _services.AddSingleton(features);

        // Logging
        _services.AddLogging(builder =>
        {
            builder.AddConfiguration(_configuration.GetSection("Logging"));
            builder.AddConsole();
            builder.AddDebug();
        });

        // Register modules, lifecycle handlers, background tasks
        foreach (var module in _modules)
            _services.AddSingleton(module);

        foreach (var lifecycle in _lifecycleHandlers)
            _services.AddSingleton(lifecycle);

        foreach (var task in _backgroundTasks)
            _services.AddSingleton(task);

        // Build DI container
        var provider = _services.BuildServiceProvider();

        // Create AppContext
        var context = new AppContext(
            provider,
            _configuration,
            provider.GetRequiredService<ILoggerFactory>(),
            features,
            cancellationHub);

        // Runtime subsystems
        var moduleLoader = new ModuleLoader(_modules);
        var lifecycleManager = new AppLifecycleManager(_lifecycleHandlers);
        var backgroundRunner = new BackgroundTaskRunner(_backgroundTasks);

        // Build startup pipeline
        var startupPipeline = BuildStartupPipeline();

        return new AppHost(
            provider,
            context,
            cancellationHub,
            lifecycleManager,
            backgroundRunner,
            moduleLoader,
            startupPipeline);
    }

    private Func<IAppContext, CancellationToken, Task> BuildStartupPipeline()
    {
        Func<IAppContext, CancellationToken, Task> pipeline = static (_, _) => Task.CompletedTask;

        // Apply filters in reverse order (outermost first)
        for (int i = _startupFilters.Count - 1; i >= 0; i--)
        {
            var filter = _startupFilters[i];
            pipeline = filter.Configure(pipeline);
        }

        return pipeline;
    }
}