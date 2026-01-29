namespace AxiomFX.Core.Utilities;

/// <summary>
/// Provides a centralized hub for creating and managing cancellation tokens.
/// Useful for coordinating shutdown across modules, background tasks,
/// pipelines, and other long-running operations.
/// </summary>
public sealed class CancellationHub : IDisposable
{
    private readonly CancellationTokenSource _rootCts = new();
    private readonly List<CancellationTokenSource> _linkedSources = new();
    private bool _disposed;

    /// <summary>
    /// A token that is cancelled when the application is shutting down.
    /// </summary>
    public CancellationToken RootToken => _rootCts.Token;

    /// <summary>
    /// Creates a new linked cancellation token source that is tied to the root token.
    /// When the application shuts down, all linked tokens are cancelled automatically.
    /// </summary>
    public CancellationTokenSource CreateLinkedSource()
    {
        ThrowIfDisposed();

        var linked = CancellationTokenSource.CreateLinkedTokenSource(_rootCts.Token);
        lock (_linkedSources)
            _linkedSources.Add(linked);

        return linked;
    }

    /// <summary>
    /// Requests cancellation of the root token and all linked tokens.
    /// </summary>
    public void Cancel()
    {
        ThrowIfDisposed();
        _rootCts.Cancel();
    }

    /// <summary>
    /// Disposes the hub and all linked cancellation token sources.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;

        _rootCts.Cancel();
        _rootCts.Dispose();

        lock (_linkedSources)
        {
            foreach (var cts in _linkedSources)
            {
                try { cts.Cancel(); }
                catch { /* swallow */ }

                cts.Dispose();
            }

            _linkedSources.Clear();
        }
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(CancellationHub));
    }
}