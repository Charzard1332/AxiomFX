namespace AxiomFX.Core.Features;

/// <summary>
/// A lightweight container for storing optional application features.
/// Features extend the application's capabilities without requiring
/// changes to core interfaces.
/// </summary>
public sealed class FeatureCollection
{
    private readonly Dictionary<Type, object> _features = new();
    private readonly object _lock = new();

    /// <summary>
    /// Adds or replaces a feature in the collection.
    /// </summary>
    /// <typeparam name="TFeature">The feature type.</typeparam>
    /// <param name="feature">The feature instance.</param>
    public void Set<TFeature>(TFeature feature)
        where TFeature : class, IAppFeature
    {
        if (feature is null)
            throw new ArgumentNullException(nameof(feature));

        lock (_lock)
        {
            _features[typeof(TFeature)] = feature;
        }
    }

    /// <summary>
    /// Retrieves a feature from the collection.
    /// Returns null if the feature is not present.
    /// </summary>
    /// <typeparam name="TFeature">The feature type.</typeparam>
    public TFeature? Get<TFeature>()
        where TFeature : class, IAppFeature
    {
        lock (_lock)
        {
            return _features.TryGetValue(typeof(TFeature), out var value)
                ? (TFeature)value
                : null;
        }
    }

    /// <summary>
    /// Checks whether a feature of the specified type exists.
    /// </summary>
    public bool Has<TFeature>()
        where TFeature : class, IAppFeature
    {
        lock (_lock)
        {
            return _features.ContainsKey(typeof(TFeature));
        }
    }

    /// <summary>
    /// Removes a feature from the collection.
    /// </summary>
    public bool Remove<TFeature>()
        where TFeature : class, IAppFeature
    {
        lock (_lock)
        {
            return _features.Remove(typeof(TFeature));
        }
    }
}