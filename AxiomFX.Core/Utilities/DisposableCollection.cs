namespace AxiomFX.Core.Utilities;

/// <summary>
/// A lightweight container for managing multiple <see cref="IDisposable"/> instances.
/// Ensures all items are disposed exactly once, even if added after partial disposal.
/// </summary>
public sealed class DisposableCollection : IDisposable
{
    private readonly List<IDisposable> _items = new();
    private bool _disposed;

    /// <summary>
    /// Adds a disposable item to the collection.
    /// If the collection has already been disposed, the item is disposed immediately.
    /// </summary>
    public void Add(IDisposable disposable)
    {
        if (disposable is null)
            throw new ArgumentNullException(nameof(disposable));

        if (_disposed)
        {
            // If already disposed, dispose immediately
            disposable.Dispose();
            return;
        }

        _items.Add(disposable);
    }

    /// <summary>
    /// Disposes all items in the collection in reverse order of addition.
    /// Ensures each item is disposed exactly once.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;

        // Dispose in reverse order (stack semantics)
        for (int i = _items.Count - 1; i >= 0; i--)
        {
            try
            {
                _items[i].Dispose();
            }
            catch
            {
                // Swallow exceptions to ensure all disposables are attempted
                // You may later add logging here via an injected logger
            }
        }

        _items.Clear();
    }
}