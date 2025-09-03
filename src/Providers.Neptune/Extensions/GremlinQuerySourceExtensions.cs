using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Providers.Neptune
{
    public static class GremlinQuerySourceExtensions
    {
        private static readonly StepLabel<bool> UseDFEStepLabel = new ();

        public static IGremlinQuerySource UseDFE(this IGremlinQuerySource source, bool enabled = true) => source
            .WithSideEffect(UseDFEStepLabel, enabled);
    }
}
