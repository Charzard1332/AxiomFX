namespace AxiomFX.Core.Abstractions;

/// <summary>
/// Represents a single step in a composable execution pipeline.
/// Pipeline steps wrap the next step, allowing pre- and post-processing,
/// transformation, validation, or interception of exectution.
/// </summary>
public interface IPipelineStep<TContext>
{
    /// <summary>
    /// Wraps the next step in the pipeline.
    /// Implementations may run logic before and/or after invoking <paramref name="next"/>
    /// </summary>
    /// <param name="next">The next pipeline delegate</param>
    /// <returns>A wrapped pipeline delegate.</returns>
    Func<TContext, CancellationToken, Task> Invoke(Func<TContext, CancellationToken, Task> next);
}