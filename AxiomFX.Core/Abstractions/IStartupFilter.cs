namespace AxiomFX.Core.Abstractions;

/// <summary>
/// Represents a startup filter that can wrap or extend the application's
/// startup pipeline. Startup filters allow modules and components to
/// insert logic before or after the application's main startup routine.
/// </summary>
public interface IStartupFilter
{
    /// <summary>
    /// Wraps the next startup action in the pipeline.
    /// Implementations may run logic before and/or after invoking <paramref name="next"/>.
    /// </summary>
    /// <param name="next">The next startup delegate in the pipeline</param>
    /// <returns>A wrapped startup delegate.</returns>
    Func<IAppContext, CancellationToken, Task> Configure(Func<IAppContext, CancellationToken, Task> next);
}