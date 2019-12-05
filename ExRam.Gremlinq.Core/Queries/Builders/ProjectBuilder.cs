using System;
using System.Collections.Immutable;
using System.Linq.Expressions;
using Gremlin.Net.Process.Traversal;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public static partial class ProjectBuilder
    {
        private sealed partial class ProjectBuilderImpl<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16> :
            IProjectBuilder<TSourceQuery, TElement>,
            IProjectDynamicBuilder<TSourceQuery, TElement>
            where TSourceQuery : IGremlinQuery
        { 
            private readonly TSourceQuery _sourceQuery;

            public ProjectBuilderImpl(TSourceQuery sourceQuery, IImmutableDictionary<string, IGremlinQuery> projections)
            {
                _sourceQuery = sourceQuery;
                Projections = projections;
            }
            
            private ProjectBuilderImpl<TSourceQuery, TElement, TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16> By<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(Func<TSourceQuery, IGremlinQuery> projection)
            {
                return By<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>($"Item{Projections.Count + 1}", projection);
            }

            private ProjectBuilderImpl<TSourceQuery, TElement, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit> By(string name, Func<TSourceQuery, IGremlinQuery> projection)
            {
                return By<Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit>(name, projection);
            }

            private ProjectBuilderImpl<TSourceQuery, TElement, TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16> By<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(string name, Func<TSourceQuery, IGremlinQuery> projection)
            {
                return new ProjectBuilderImpl<TSourceQuery, TElement, TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(
                    _sourceQuery,
                    Projections.SetItem(name, projection(_sourceQuery)));
            }

            IProjectTupleBuilder<TSourceQuery, TElement> IProjectBuilder<TSourceQuery, TElement>.ToTuple()
            {
                return this;
            }

            IProjectDynamicBuilder<TSourceQuery, TElement> IProjectBuilder<TSourceQuery, TElement>.ToDynamic()
            {
                return this;
            }

            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TNewItem16> IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15>.By<TNewItem16>(Func<TSourceQuery, IGremlinQuery<TNewItem16>> projection)
            {
                return By<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TNewItem16>(projection);
            }

            IProjectTupleBuilder<TSourceQuery, TElement, TNewItem1> IProjectTupleBuilder<TSourceQuery, TElement>.By<TNewItem1>(Func<TSourceQuery, IGremlinQuery<TNewItem1>> projection)
            {
                return By<TNewItem1, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit>(projection);
            }

            IProjectDynamicBuilder<TSourceQuery, TElement> IProjectDynamicBuilder<TSourceQuery, TElement>.By(Func<TSourceQuery, IGremlinQuery> projection)
            {
                return By($"Item{Projections.Count + 1}", projection);
            }

            IProjectDynamicBuilder<TSourceQuery, TElement> IProjectDynamicBuilder<TSourceQuery, TElement>.By(string name, Func<TSourceQuery, IGremlinQuery> projection)
            {
                return By(name, projection);
            }

            public IImmutableDictionary<string, IGremlinQuery> Projections { get; }
        }

        internal static IProjectBuilder<TSourceQuery, TElement> Create<TSourceQuery, TElement>(TSourceQuery sourceQuery) where TSourceQuery : IGremlinQuery<TElement>
        {
            return new ProjectBuilderImpl<TSourceQuery, TElement, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit>(
                sourceQuery,
                ImmutableDictionary<string, IGremlinQuery>.Empty);
        }

        public static IProjectDynamicBuilder<TSourceQuery, TElement> By<TSourceQuery, TElement>(this IProjectDynamicBuilder<TSourceQuery, TElement> projectBuilder, Expression<Func<TElement, object>> projection)
            where TSourceQuery : IElementGremlinQuery<TElement>
        {
            if (projection.Body.StripConvert() is MemberExpression memberExpression)
                return projectBuilder.By(memberExpression.Member.Name, _ => _.Values(projection));

            throw new ExpressionNotSupportedException(projection);
        }
    }
}
