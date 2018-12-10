using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExRam.Gremlinq
{
    public sealed class ValuesStep<TSource, TTarget> : NonTerminalStep
    {
        private readonly Expression<Func<TSource, TTarget>>[] _projections;

        public ValuesStep(Expression<Func<TSource, TTarget>>[] projections)
        {
            _projections = projections;
        }

        public override IEnumerable<Step> Resolve(IGraphModel model)
        {
            var keys = _projections
                .Select(projection =>
                {
                    if (projection.Body.StripConvert() is MemberExpression memberExpression)
                        return model.GetIdentifier(memberExpression.Member.Name);

                    throw new NotSupportedException();
                })
                .ToArray();

            var numberOfIdSteps = keys
                .OfType<T>()
                .Count(x => x == T.Id);

            var propertyKeys = keys
                .OfType<string>()
                .Cast<object>()
                .ToArray();

            if (numberOfIdSteps > 1 || numberOfIdSteps > 0 && propertyKeys.Length > 0)
            {
                yield return new UnionStep(
                    new IGremlinQuery[]
                    {
                        GremlinQuery.Anonymous.AddStep(MethodStep.Create("values", propertyKeys)).Resolve(model),
                        GremlinQuery.Anonymous.Id().Resolve(model)
                    });
            }
            else if (numberOfIdSteps > 0)
                yield return MethodStep.Id;
            else
            {
                yield return MethodStep.Create(
                    "values",
                    propertyKeys);
            }
        }
    }
}
