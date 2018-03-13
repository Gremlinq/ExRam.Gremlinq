using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;

namespace ExRam.Gremlinq
{
    public sealed class ValuesGremlinStep<TSource, TTarget> : NonTerminalGremlinStep
    {
        private readonly Expression<Func<TSource, TTarget>>[] _projections;

        public ValuesGremlinStep(Expression<Func<TSource, TTarget>>[] projections)
        {
            this._projections = projections;
        }

        public override IEnumerable<TerminalGremlinStep> Resolve(IGraphModel model)
        {
            yield return new TerminalGremlinStep(
                "values",
                this._projections
                    .Select(projection =>
                    {
                        if (projection.Body is MemberExpression memberExpression)
                        {
                            var name = memberExpression.Member.Name;

                            return name == model.IdPropertyName
                                ? (object)T.Id 
                                : name;
                        }

                        throw new NotSupportedException();
                    })
                    .ToImmutableList<object>());
        }
    }
}