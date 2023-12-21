using System.Diagnostics.CodeAnalysis;

using ExRam.Gremlinq.Core.Transformation;

namespace ExRam.Gremlinq.Core.Serialization
{
    internal sealed class StepLabelSupportConverterFactory : IConverterFactory
    {
        private sealed class DeferConverter<TQuery, TTarget> : IConverter<TQuery, TTarget>
            where TQuery : IGremlinQueryBase
        {
            private readonly IGremlinQueryEnvironment _environment;

            public DeferConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public bool TryConvert(TQuery source, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                _stepLabelNames = null;

                try
                {
                    return defer.TryTransform(source, _environment, out value);
                }
                finally
                {
                    _stepLabelNames = null;
                }
            }
        }

        private sealed class StepLabelResolutionConverter<TStepLabel, TTarget> : IConverter<TStepLabel, TTarget>
            where TStepLabel : StepLabel
        {
            public bool TryConvert(TStepLabel stepLabel, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                var stepLabelNames = _stepLabelNames ??= new Dictionary<StepLabel, Label>();

                if (!stepLabelNames.TryGetValue(stepLabel, out var stepLabelMapping))
                {
                    stepLabelMapping = stepLabel.Identity is string { Length: > 0 } stringIdentity && !stringIdentity.StartsWith('_')
                        ? stringIdentity
                        : stepLabelNames.Count;

                    stepLabelNames.Add(stepLabel, stepLabelMapping);
                }

                value = (TTarget)(object)(string)stepLabelMapping;
                return true;
            }
        }

        [ThreadStatic]
        private static Dictionary<StepLabel, Label>? _stepLabelNames;

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            if (typeof(IGremlinQueryBase).IsAssignableFrom(typeof(TSource)) && typeof(TTarget) == typeof(Traversal))
                return (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(DeferConverter<,>).MakeGenericType(typeof(TSource), typeof(TTarget)), environment);

            if (typeof(StepLabel).IsAssignableFrom(typeof(TSource)) && typeof(TTarget).IsAssignableFrom(typeof(string)))
                return (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(StepLabelResolutionConverter<,>).MakeGenericType(typeof(TSource), typeof(TTarget)));

            return default;
        }
    }
}

