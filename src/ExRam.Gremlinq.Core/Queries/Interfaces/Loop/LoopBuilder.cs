#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier

using System;

namespace ExRam.Gremlinq.Core
{
    partial class GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>
    {
        private sealed class LoopBuilder<TQuery> :
            IEmit<TQuery>,
            IEmitRepeat<TQuery>,
            IEmitRepeatUntil<TQuery>,
            IFinalLoopBuilder<TQuery>,
            IRepeat<TQuery>,
            IRepeatEmit<TQuery>,
            IRepeatEmitUntil<TQuery>,
            IRepeatUntil<TQuery>,
            IUntil<TQuery>,
            IUntilEmit<TQuery>,
            IUntilEmitRepeat<TQuery>,
            IUntilRepeat<TQuery>,
            IUntilRepeatEmit<TQuery> where TQuery : IGremlinQueryBase
        {
            private readonly GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> _outerQuery;

            public LoopBuilder(GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> outerQuery)
            {
                _outerQuery = outerQuery;
            }

            IRepeatEmit<TQuery> IRepeat<TQuery>.Emit() => Emit();

            IUntilEmit<TQuery> IUntil<TQuery>.Emit() => Emit();

            IUntilRepeatEmit<TQuery> IUntilRepeat<TQuery>.Emit() => Emit();

            IEmitRepeat<TQuery> IEmit<TQuery>.Repeat(Func<TQuery, TQuery> loop) => Repeat(loop);

            IUntilRepeat<TQuery> IUntil<TQuery>.Repeat(Func<TQuery, TQuery> loop) => Repeat(loop);

            IUntilEmitRepeat<TQuery> IUntilEmit<TQuery>.Repeat(Func<TQuery, TQuery> loop) => Repeat(loop);

            IEmitRepeatUntil<TQuery> IEmitRepeat<TQuery>.Until(Func<TQuery, IGremlinQueryBase> condition) => Until(condition);

            IRepeatUntil<TQuery> IRepeat<TQuery>.Until(Func<TQuery, IGremlinQueryBase> condition) => Until(condition);

            IRepeatEmitUntil<TQuery> IRepeatEmit<TQuery>.Until(Func<TQuery, IGremlinQueryBase> condition) => Until(condition);

            private LoopBuilder<TQuery> Emit() => new(_outerQuery.Emit());
            private LoopBuilder<TQuery> Until(Func<TQuery, IGremlinQueryBase> condition) => new(_outerQuery.Until(_ => condition((TQuery)(object)_)));
            private LoopBuilder<TQuery> Repeat(Func<TQuery, TQuery> loop) => new ((GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>)(object)_outerQuery.Repeat(_ => loop((TQuery)(object)_)));
        }
    }
}
