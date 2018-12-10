using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ExRam.Gremlinq
{
    public abstract class DerivedLabelNamesStep : NonTerminalStep
    {
        protected static readonly ConcurrentDictionary<(IGraphModel model, Type type), object[]> TypeLabelDict = new ConcurrentDictionary<(IGraphModel, Type), object[]>();
    }

    public sealed class DerivedLabelNamesStep<TElement> : DerivedLabelNamesStep
    {
        public static DerivedLabelNamesStep<TElement> HasLabel = new DerivedLabelNamesStep<TElement>("hasLabel");
        public static DerivedLabelNamesStep<TElement> Out = new DerivedLabelNamesStep<TElement>("out");
        public static DerivedLabelNamesStep<TElement> In = new DerivedLabelNamesStep<TElement>("in");
        public static DerivedLabelNamesStep<TElement> Both = new DerivedLabelNamesStep<TElement>("both");
        public static DerivedLabelNamesStep<TElement> OutE = new DerivedLabelNamesStep<TElement>("outE");
        public static DerivedLabelNamesStep<TElement> InE = new DerivedLabelNamesStep<TElement>("inE");
        public static DerivedLabelNamesStep<TElement> BothE = new DerivedLabelNamesStep<TElement>("bothE");

        private readonly string _stepName;

        private DerivedLabelNamesStep(string stepName)
        {
            _stepName = stepName;
        }

        public override IEnumerable<Step> Resolve(IGraphModel model)
        {
            yield return MethodStep.Create(_stepName, GetDerivedLabelNames(model));
        }

        private static object[] GetDerivedLabelNames(IGraphModel model)
        {
            return TypeLabelDict
                .GetOrAdd(
                    (model, typeof(TElement)),
                    tuple => tuple.model.GetDerivedTypes(typeof(TElement), true)
                        .Select(type => tuple.model.TryGetLabelOfType(type)
                            .IfNone(() => throw new InvalidOperationException()))
                        .OrderBy(x => x)
                        .ToArray<object>());
        }
    }
}
