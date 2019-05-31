using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Serialization
{
    public sealed class GremlinQueryElementVisitorCollection : IGremlinQueryElementVisitorCollection
    {
        public static readonly IGremlinQueryElementVisitorCollection Empty = new GremlinQueryElementVisitorCollection(ImmutableDictionary<Type, Func<object>>.Empty);

        public static readonly IGremlinQueryElementVisitorCollection Default = Empty
            .Set<SerializedGremlinQuery, GroovyGremlinQueryElementVisitor>();

        private readonly ImmutableDictionary<Type, Func<object>> _visitors;

        private GremlinQueryElementVisitorCollection(ImmutableDictionary<Type, Func<object>> visitors)
        {
            _visitors = visitors;
        }

        public IGremlinQueryElementVisitorCollection Set<TSerializedQuery, TVisitor>() where TVisitor : IGremlinQueryElementVisitor<TSerializedQuery>, new()
        {
            return new GremlinQueryElementVisitorCollection(_visitors.SetItem(
                typeof(TSerializedQuery),
                () => new TVisitor()));
        }

        public IGremlinQueryElementVisitor<TSerializedQuery> Get<TSerializedQuery>()
        {
            return (IGremlinQueryElementVisitor<TSerializedQuery>)_visitors[typeof(TSerializedQuery)]();
        }
    }
}