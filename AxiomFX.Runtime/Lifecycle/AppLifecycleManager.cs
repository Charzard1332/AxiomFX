using AxiomFX.Core;
using Microsoft.Extensions.Logging;

namespace AxiomFX.Runtime.Lifecycle;

/// <summary>
/// Coordinates all registered IAppLifecycle handlers,
/// ensuring lifecycle events are executed in order and fully async.
/// </summary>
public sealed class AppLifecycleManager
{
    private readonly IReadOnlyList<IAppLifecycle> _handlers;

    public AppLifecycleManager(IEnumerable<IAppLifecycle> handlers)
    {
        _handlers = handlers.ToList();
    }

    /// <summary>
    /// Called before modules initialize and before the startup pipeline runs.
    /// </summary>
    public async Task OnStartingAsync(CancellationToken cancellationToken)
    {
        foreach (var handler in _handlers)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                await handler.OnStartingAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                // Lifecycle errors should not crash the host immediately.
                // They are logged by the host.
                throw new AppLifecycleException(
                    $"Lifecycle handler {handler.GetType().Name} failed during OnStartingAsync.",
                    ex);
            }
        }
    }

    /// <summary>
    /// Called after modules initialize and after the startup pipeline completes.
    /// </summary>
    public async Task OnStartedAsync(CancellationToken cancellationToken)
    {
        foreach (var handler in _handlers)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                await handler.OnStartedAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AppLifecycleException(
                    $"Lifecycle handler {handler.GetType().Name} failed during OnStartedAsync.",
                    ex);
            }
        }
    }

    /// <summary>
    /// Called when the application begins shutting down.
    /// </summary>
    public async Task OnStoppingAsync(CancellationToken cancellationToken)
    {
        foreach (var handler in _handlers)
        {
            try
            {
                await handler.OnStoppingAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Shutdown cancellation is normal
            }
            catch (Exception ex)
            {
                throw new AppLifecycleException(
                    $"Lifecycle handler {handler.GetType().Name} failed during OnStoppingAsync.",
                    ex);
            }
        }
    }

    /// <summary>
    /// Called after background tasks stop and the application is fully shutting down.
    /// </summary>
    public async Task OnStoppedAsync(CancellationToken cancellationToken)
    {
        foreach (var handler in _handlers)
        {
            try
            {
                await handler.OnStoppedAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Normal during shutdown
            }
            catch (Exception ex)
            {
                throw new AppLifecycleException(
                    $"Lifecycle handler {handler.GetType().Name} failed during OnStoppedAsync.",
                    ex);
            }
        }
    }
}

/// <summary>
/// Represents an error thrown by a lifecycle handler.
/// </summary>
public sealed class AppLifecycleException : Exception
{
    public AppLifecycleException(string message, Exception inner)
        : base(message, inner)
    {
    }
}