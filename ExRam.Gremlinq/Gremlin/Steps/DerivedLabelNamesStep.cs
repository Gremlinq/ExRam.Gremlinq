using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ExRam.Gremlinq
{
    public abstract class DerivedLabelNamesStep : NonTerminalStep
    {
        protected static readonly ConcurrentDictionary<(IGraphModel model, Type type), ImmutableList<object>> TypeLabelDict = new ConcurrentDictionary<(IGraphModel, Type), ImmutableList<object>>();
    }

    public sealed class DerivedLabelNamesStep<TElement> : DerivedLabelNamesStep
    {
        private readonly string _stepName;

        public DerivedLabelNamesStep(string stepName)
        {
            _stepName = stepName;
        }

        public override IEnumerable<TerminalStep> Resolve(IGraphModel model)
        {
            yield return new MethodStep(_stepName, GetDerivedLabelNames(model));
        }

        private static ImmutableList<object> GetDerivedLabelNames(IGraphModel model)
        {
            return TypeLabelDict
                .GetOrAdd(
                    (model, typeof(TElement)),
                    tuple => tuple.model.GetDerivedTypes(typeof(TElement), true)
                        .Select(type => tuple.model.TryGetLabelOfType(type)
                            .IfNone(() => throw new InvalidOperationException()))
                        .OrderBy(x => x)
                        .ToImmutableList<object>());
        }
    }
}