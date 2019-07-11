using System;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    internal static class ChooseBuilder
    {
        private sealed class ChooseBuilderImpl<TSourceQuery, TTargetQuery, TPickElement> :
            IChooseBuilder<TSourceQuery>,
            IChooseBuilderWithCondition<TSourceQuery, TPickElement>,
            IChooseBuilderWithCase<TSourceQuery, TPickElement, TTargetQuery>
            where TSourceQuery : IGremlinQuery
            where TTargetQuery : IGremlinQuery
        {
            private readonly TSourceQuery _sourceQuery;
            private readonly IGremlinQuery _targetQuery;

            public ChooseBuilderImpl(TSourceQuery sourceQuery, IGremlinQuery targetQuery)
            {
                _sourceQuery = sourceQuery;
                _targetQuery = targetQuery;
            }

            public IChooseBuilderWithCondition<TSourceQuery, TNewPickElement> On<TNewPickElement>(Func<TSourceQuery, IGremlinQuery<TNewPickElement>> chooseTraversal)
            {
                return new ChooseBuilderImpl<TSourceQuery, TTargetQuery, TNewPickElement>(
                    _sourceQuery,
                    _targetQuery.AsAdmin().AddStep(new ChooseOptionTraversalStep(chooseTraversal(_sourceQuery))));
            }

            public IChooseBuilderWithCase<TSourceQuery, TPickElement, TNewTargetQuery> Case<TNewTargetQuery>(TPickElement element, Func<TSourceQuery, TNewTargetQuery> continuation) where TNewTargetQuery : IGremlinQuery
            {
                return new ChooseBuilderImpl<TSourceQuery, TNewTargetQuery, TPickElement>(
                    _sourceQuery,
                    _targetQuery.AsAdmin().AddStep(new OptionTraversalStep(element, continuation(_sourceQuery))));
            }

            public IChooseBuilderWithCaseOrDefault<TNewTargetQuery> Default<TNewTargetQuery>(Func<TSourceQuery, TNewTargetQuery> continuation) where TNewTargetQuery : IGremlinQuery
            {
                return new ChooseBuilderImpl<TSourceQuery, TNewTargetQuery, TPickElement>(
                    _sourceQuery,
                    _targetQuery.AsAdmin().AddStep(new OptionTraversalStep(default, continuation(_sourceQuery))));
            }

            public IChooseBuilderWithCase<TSourceQuery, TPickElement, TTargetQuery> Case(TPickElement element, Func<TSourceQuery, TTargetQuery> continuation) => Case<TTargetQuery>(element, continuation);

            public IChooseBuilderWithCaseOrDefault<TTargetQuery> Default(Func<TSourceQuery, TTargetQuery> continuation) => Default<TTargetQuery>(continuation);

            public TTargetQuery TargetQuery
            {
                get
                {
                    if (_targetQuery == null)
                        throw new InvalidOperationException();

                    return _targetQuery
                        .AsAdmin()
                        .ChangeQueryType<TTargetQuery>();
                }
            }
        }

        public static IChooseBuilder<TSourceQuery> Create<TSourceQuery>(TSourceQuery sourceQuery, IGremlinQuery targetQuery)
            where TSourceQuery : IGremlinQuery
        {
            return new ChooseBuilderImpl<TSourceQuery, TSourceQuery, Unit>(sourceQuery, targetQuery);
        }
    }
}
