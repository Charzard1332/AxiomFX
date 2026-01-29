namespace AxiomFX.Core.Utilities;

/// <summary>
/// A lightweight asynchronous mutual exclusion lock.
/// Ensures that only one asynchronous operation can enter the critical section at a time.
/// </summary>
public sealed class AsyncLock
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    /// <summary>
    /// Acquires the lock asynchronously and returns an <see cref="IDisposable"/>
    /// that releases the lock when disposed.
    /// </summary>
    public async Task<IDisposable> LockAsync(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
        return new Releaser(_semaphore);
    }

    /// <summary>
    /// Synchronous lock acquisition for rare cases where async is not available.
    /// </summary>
    public IDisposable Lock()
    {
        _semaphore.Wait();
        return new Releaser(_semaphore);
    }

    private sealed class Releaser : IDisposable
    {
        private readonly SemaphoreSlim _semaphore;
        private int _released;

        public Releaser(SemaphoreSlim semaphore)
        {
            _semaphore = semaphore;
        }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _released, 1) == 0)
                _semaphore.Release();
        }
    }
}