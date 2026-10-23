using System.Collections.Immutable;

using ExRam.Gremlinq.Core.Steps;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    /// <summary>Extension methods for <see cref="IGremlinQueryEnvironment"/>.</summary>
    public static class GremlinQueryEnvironmentExtensions
    {
        /// <summary>
        /// Determines whether the environment supports the given type, either natively or via a known conversion.
        /// </summary>
        /// <param name="environment">The query environment.</param>
        /// <param name="type">The type to check.</param>
        public static bool SupportsType(this IGremlinQueryEnvironment environment, Type type)
        {
            ArgumentNullException.ThrowIfNull(environment);
            ArgumentNullException.ThrowIfNull(type);

            if (environment.SupportsTypeNatively(type))
                return true;

            if (type == typeof(byte[]))
                return environment.SupportsTypeNatively(typeof(string));

            if (type == typeof(TimeSpan))
                return environment.SupportsTypeNatively(typeof(double));

            return false;
        }

        /// <summary>
        /// Determines whether the environment natively supports the given type.
        /// </summary>
        /// <param name="environment">The query environment.</param>
        /// <param name="type">The type to check.</param>
        public static bool SupportsTypeNatively(this IGremlinQueryEnvironment environment, Type type)
        {
            ArgumentNullException.ThrowIfNull(environment);
            ArgumentNullException.ThrowIfNull(type);

            return environment.NativeTypes.Contains(type);
        }

        // The step that filters out all traversers on the server: none() as long as the server understands
        // TinkerPop versions below 3.8, discard() once it does not.
        internal static Step GetDiscardStep(this IGremlinQueryEnvironment environment) => environment.ServerCompatibilityRange.IsAtLeast(TinkerPopVersions.V3_8_0)
            ? DiscardStep.Instance
            : NoneStep.Instance;

        // The step that filters by a traversal on the value of a key: has(key, traversal) where the server understands it,
        // otherwise the filter a 3.7 server executes for it, where() with the traversal behind the step that takes the
        // value of the key. 3.8 removed has(key, traversal). A 3.8 server still accepts it, but the traversal never matches.
        internal static Step GetHasTraversalStep(this IGremlinQueryEnvironment environment, Key key, Traversal traversal)
        {
            if (!environment.ServerCompatibilityRange.IsAtLeast(TinkerPopVersions.V3_8_0))
                return new HasTraversalStep(key, traversal);

            Step valueStep = key.RawKey switch
            {
                T t when T.Id.Equals(t) => IdStep.Instance,
                T t when T.Label.Equals(t) => LabelStep.Instance,
                T t => new ValuesStep(ImmutableArray.Create(t.EnumValue)),
                var name => new ValuesStep(ImmutableArray.Create((string)name))
            };

            return new WhereTraversalStep(Traversal.Empty
                .Push(valueStep)
                .Push(traversal.Steps));
        }

        // The step that aggregates into a side-effect: aggregate(scope, x) as long as the server understands TinkerPop versions
        // below 3.8. 3.8 removed aggregate(local, x), so from then on the local scope becomes local(aggregate(x)).
        internal static Step GetAggregateStep(this IGremlinQueryEnvironment environment, Scope scope, StepLabel stepLabel) => Scope.Local.Equals(scope) && environment.ServerCompatibilityRange.IsAtLeast(TinkerPopVersions.V3_8_0)
            ? new LocalStep(Traversal.Empty.Push(new AggregateStep(Scope.Global, stepLabel)))
            : new AggregateStep(scope, stepLabel);
    }
}
