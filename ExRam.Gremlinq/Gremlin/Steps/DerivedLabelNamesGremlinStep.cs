using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ExRam.Gremlinq
{
    public abstract class DerivedLabelNamesGremlinStep : NonTerminalGremlinStep
    {
        protected static readonly ConcurrentDictionary<(IGraphModel model, Type type), ImmutableList<object>> TypeLabelDict = new ConcurrentDictionary<(IGraphModel, Type), ImmutableList<object>>();
    }

    public sealed class DerivedLabelNamesGremlinStep<T> : DerivedLabelNamesGremlinStep
    {
        private readonly string _stepName;

        public DerivedLabelNamesGremlinStep(string stepName)
        {
            this._stepName = stepName;
        }

        public override IEnumerable<TerminalGremlinStep> Resolve(IGraphModel model)
        {
            yield return new TerminalGremlinStep(this._stepName, GetDerivedLabelNames(model));
        }

        private static ImmutableList<object> GetDerivedLabelNames(IGraphModel model)
        {
            return TypeLabelDict
                .GetOrAdd(
                    (model, typeof(T)),
                    tuple => tuple.model.GetDerivedTypes(typeof(T), true)
                        .Select(type => tuple.model.TryGetLabelOfType(type)
                            .IfNone(() => throw new InvalidOperationException()))
                        .OrderBy(x => x)
                        .ToImmutableList<object>());
        }
    }
}