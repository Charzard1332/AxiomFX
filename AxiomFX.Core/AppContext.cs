using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AxiomFX.Core;

public sealed class AppContext : IAppContext
{
    public IServiceProvider Services { get; }
    public IConfiguration Configuration { get; }
    public ILoggerFactory LoggerFactory { get; }
    public FeatureCollection Features { get; }
    public CancellationHub Cancellation { get; }

    public AppContext(
        IServiceProvider services,
        IConfiguration configuration,
        ILoggerFactory loggerFactory,
        FeatureCollection features,
        CancellationHub cancellation)
    {
        Services = services;
        Configuration = configuration;
        LoggerFactory = loggerFactory;
        Features = features;
        Cancellation = cancellation;
    }
}