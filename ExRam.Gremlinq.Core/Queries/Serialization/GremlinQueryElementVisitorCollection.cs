using System;
using System.Collections.Immutable;
using LanguageExt;

namespace ExRam.Gremlinq.Core.Serialization
{
    public sealed class GremlinQueryElementVisitorCollection : IGremlinQueryElementVisitorCollection
    {
        public static readonly IGremlinQueryElementVisitorCollection Empty = new GremlinQueryElementVisitorCollection(ImmutableDictionary<Type, Func<IGremlinQueryElementVisitor>>.Empty);

        public static readonly IGremlinQueryElementVisitorCollection Default = Empty
            .Set<SerializedGremlinQuery, GroovyGremlinQueryElementVisitor>();

        private readonly IImmutableDictionary<Type, Func<IGremlinQueryElementVisitor>> _visitors;

        private GremlinQueryElementVisitorCollection(IImmutableDictionary<Type, Func<IGremlinQueryElementVisitor>> visitors)
        {
            _visitors = visitors;
        }

        public IGremlinQueryElementVisitorCollection TryAdd<TSerializedQuery, TVisitor>() where TVisitor : IGremlinQueryElementVisitor<TSerializedQuery>, new()
        {
            return new GremlinQueryElementVisitorCollection(_visitors.ContainsKey(typeof(TSerializedQuery))
                ? _visitors
                : _visitors.SetItem(
                    typeof(TSerializedQuery),
                    () => new TVisitor()));
        }

        public IGremlinQueryElementVisitorCollection Set<TSerializedQuery, TVisitor>() where TVisitor : IGremlinQueryElementVisitor<TSerializedQuery>, new()
        {
            return new GremlinQueryElementVisitorCollection(_visitors.SetItem(
                typeof(TSerializedQuery),
                () => new TVisitor()));
        }

        public Option<IGremlinQueryElementVisitor<TSerializedQuery>> TryGet<TSerializedQuery>()
        {
            return _visitors
                .TryGetValue(typeof(TSerializedQuery))
                .Map(x => (IGremlinQueryElementVisitor<TSerializedQuery>)x());
        }
    }
}
