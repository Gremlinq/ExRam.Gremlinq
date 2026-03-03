using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Providers.Neptune
{
    /// <summary>
    /// Provides Neptune-specific extension methods for <see cref="IGremlinQuerySource"/>.
    /// </summary>
    public static class GremlinQuerySourceExtensions
    {
        private static readonly StepLabel<bool> UseDFEStepLabel = "Neptune#useDFE";

        /// <summary>
        /// Enables or disables the Neptune DFE (Deep Feature Engine) query engine.
        /// </summary>
        /// <param name="source">The query source to configure.</param>
        /// <param name="enabled">Whether to enable DFE.</param>
        public static IGremlinQuerySource UseDFE(this IGremlinQuerySource source, bool enabled = true)
        {
            ArgumentNullException.ThrowIfNull(source);

            return source
                .WithSideEffect(UseDFEStepLabel, enabled);
        }
    }
}
