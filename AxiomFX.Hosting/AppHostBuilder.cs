using AxiomFX.Core.Abstractions;
using AxiomFX.Runtime.Hosting;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AxiomFX.Hosting;

/// <summary>
/// Public-facing builder for constructing an AxiomFX application host.
/// Wraps the internal runtime AppBuilder and exposes a clean, developer-friendly API.
/// </summary>
public sealed class AppHostBuilder
{
    private readonly IConfigurationBuilder _configurationBuilder = new ConfigurationBuilder();
    private readonly IServiceCollection _services = new ServiceCollection();

    private readonly List<Action<IServiceCollection>> _serviceConfigurators = new();
    private readonly List<Action<ILoggingBuilder>> _loggingConfigurators = new();

    private string _environmentName = "Production";
    private string _applicationName = "AxiomFX Application";
    private string? _contentRoot;

    private readonly AxiomFX.Runtime.Hosting.AppBuilder _runtimeBuilder = new();

    public AppHostBuilder()
    {
        _contentRoot = Directory.GetCurrentDirectory();
    }

    // ------------------------------------------------------------
    // Configuration
    // ------------------------------------------------------------

    public AppHostBuilder ConfigureAppConfiguration(Action<IConfigurationBuilder> configure)
    {
        configure?.Invoke(_configurationBuilder);
        return this;
    }

    public AppHostBuilder UseEnvironment(string environmentName)
    {
        _environmentName = environmentName;
        return this;
    }

    public AppHostBuilder UseContentRoot(string path)
    {
        _contentRoot = path;
        return this;
    }

    public AppHostBuilder UseApplicationName(string name)
    {
        _applicationName = name;
        return this;
    }

    // ------------------------------------------------------------
    // Services
    // ------------------------------------------------------------

    public AppHostBuilder ConfigureServices(Action<IServiceCollection> configure)
    {
        _serviceConfigurators.Add(configure);
        return this;
    }

    // ------------------------------------------------------------
    // Logging
    // ------------------------------------------------------------

    public AppHostBuilder ConfigureLogging(Action<ILoggingBuilder> configure)
    {
        _loggingConfigurators.Add(configure);
        return this;
    }

    // ------------------------------------------------------------
    // Modules, Background Tasks, Lifecycle Handlers
    // (Forwarded to Runtime Builder)
    // ------------------------------------------------------------

    public AppHostBuilder AddModule(IModule module)
    {
        _runtimeBuilder.AddModule(module);
        return this;
    }

    public AppHostBuilder AddStartupFilter(IStartupFilter filter)
    {
        _runtimeBuilder.AddStartupFilter(filter);
        return this;
    }

    public AppHostBuilder AddBackgroundTask(IBackgroundTask task)
    {
        _runtimeBuilder.AddBackgroundTask(task);
        return this;
    }

    public AppHostBuilder AddLifecycleHandler(IAppLifecycle lifecycle)
    {
        _runtimeBuilder.AddLifecycleHandler(lifecycle);
        return this;
    }

    // ------------------------------------------------------------
    // Build
    // ------------------------------------------------------------

    public IHost Build()
    {
        // Build configuration
        var configuration = _configurationBuilder.Build();

        // Apply configuration to runtime builder
        _runtimeBuilder.ConfigureConfiguration(configuration);

        // Apply service configurators
        foreach (var configure in _serviceConfigurators)
            configure(_services);

        // Apply logging configurators
        _services.AddLogging(builder =>
        {
            builder.AddConfiguration(configuration.GetSection("Logging"));
            builder.AddConsole();
            builder.AddDebug();

            foreach (var configure in _loggingConfigurators)
                configure(builder);
        });

        // Build service provider for hosting layer
        var hostingServices = _services.BuildServiceProvider();

        // Create environment
        var environment = new HostEnvironment(
            applicationName: _applicationName,
            environmentName: _environmentName,
            contentRootPath: _contentRoot ?? Directory.GetCurrentDirectory());

        // Build internal runtime host
        var runtimeHost = _runtimeBuilder.Build();

        // Wrap runtime host in public-facing host
        return new AppHost(
            hostingServices,
            environment,
            runtimeHost);
    }
}