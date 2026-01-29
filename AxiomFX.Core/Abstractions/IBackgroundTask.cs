namespace AxiomFX.Core.Abstractions;

/// <summary>
/// Represents a long-running or scheduled background task within an AxiomFX application.
/// Background tasks are started by the runtime after the application has initialized,
/// and are gracefully stopped during application shutdown.
/// </summary>
public interface IBackgroundTask
{
    /// <summary>
    /// Starts the background task.
    /// Implementations should run until the provided cancellation token is triggered.
    /// </summary>
    /// <param name="context">The application context.</param>
    /// <param name="cancellationToken">Token used to request shutdown.</param>
    /// <returns></returns>
    Task ExecuteAsync(IAppContext context, CancellationToken cancellationToken);
}