using AxiomFX.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AxiomFX.Runtime.Modules;

public sealed class ModuleContext : IModuleContext
{
    public IAppContext App { get; }
    public IServiceCollection Services { get; }
    public IConfiguration Configuration { get; }
    public string ModuleId { get; }

    public ModuleContext(
        IAppContext app,
        IServiceCollection services,
        IConfiguration configuration,
        string moduleId)
    {
        App = app;
        Services = services;
        Configuration = configuration;
        ModuleId = moduleId;
    }
}