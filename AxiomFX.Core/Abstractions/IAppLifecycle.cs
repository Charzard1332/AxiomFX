namespace AxiomFX.Core.Abstractions;

/// <summary>
/// Defines lifecycle events for an AxiomFX application.
/// Implementations may hook into startup, shutdown,
/// and post-start intialization phases.
/// </summary>
public interface IAppLifecycle
{
    /// <summary>
    /// Called before the application begins running.
    /// Use this to perform initialization that must occur
    /// before modules or background services start
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task OnStartingAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Called after the application has fully started.
    /// Useful for warm-up tasks, background initialization,
    /// or deferred module activation.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task OnStartedAsync(CancellationToken  cancellationToken = default);

    /// <summary>
    /// Called when the appliation is beginning a graceful shutdown.
    /// Use this to stop background tasks, flush buffers,
    /// and release external resources.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task OnStoppingAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Called after the application has fully stopped.
    /// Use this for cleanup that must occur after all services
    /// and modules have been torn down.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task OnStoppedAsync(CancellationToken cancellationToken = default);
}