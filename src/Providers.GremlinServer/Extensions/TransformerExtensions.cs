using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Steps;
using ExRam.Gremlinq.Core.Transformation;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Providers.GremlinServer
{
    internal static class TransformerExtensions
    {
        public static ITransformer WorkaroundRangeInconsistency(this ITransformer transformer) => transformer
            .WorkaroundRangeInconsistency<LimitStep>(step => Scope.Local.Equals(step.Scope) && step.Count == 1)
            .WorkaroundRangeInconsistency<TailStep>(step => Scope.Local.Equals(step.Scope) && step.Count == 1)
            .WorkaroundRangeInconsistency<RangeStep>(step => Scope.Local.Equals(step.Scope) && step.Upper - step.Lower == 1);

        private static ITransformer WorkaroundRangeInconsistency<TStep>(this ITransformer transformer, Func<TStep, bool> scopeGetter)
            where TStep : Step => transformer
                .Add<TStep>((step, env, defer, recurse) =>
                {
                    var deferred = defer
                        .TransformTo<Instruction>()
                        .From(step, env);

                    return scopeGetter(step)
                        ? deferred.WorkaroundRangeInconsistency()
                        : deferred;
                });

        private static Instruction WorkaroundRangeInconsistency(this Instruction instruction) => new (
            "map",
            new Bytecode
            {
                StepInstructions =
                {
                    instruction,
                    new Instruction(
                        "choose",
                        new Bytecode
                        {
                            StepInstructions =
                            {
                                new Instruction("count", Scope.Local),
                                new Instruction("is", P.Eq(1))
                            }
                        },
                        new Bytecode
                        {
                            StepInstructions =
                            {
                                new Instruction("fold")
                            }
                        })
                }
            });


        private static ITransformer Add<TSource>(this ITransformer serializer, Func<TSource, IGremlinQueryEnvironment, ITransformer, ITransformer, Instruction?> converter) => serializer
            .Add(ConverterFactory.Create(converter));
    }
}
