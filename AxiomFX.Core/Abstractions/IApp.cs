namespace AxiomFX.Core.Abstractions;

public interface IApp
{
    /// <summary>
    /// Represents a running AxiomFX application.
    /// This the root runtime object produced by the hosting layer.
    /// <param name="cancellationToken">Token used to request shutdown.</param>
    /// </summary>
    Task RunAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Requests a graceful shutdown of the application.
    /// </summary>
    void Stop();

    /// <summary>
    /// Provide access to the application context, including services
    /// configuration, logging, and registered features.
    /// </summary>
    IAppContext Context { get; }
}