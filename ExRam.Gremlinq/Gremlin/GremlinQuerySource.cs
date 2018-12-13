using System;
using System.Collections.Immutable;
using LanguageExt;

namespace ExRam.Gremlinq
{
    internal sealed class GremlinQuerySource : IGremlinQuerySource
    {
        private readonly string _name;
        private readonly IGraphModel _model;
        private readonly IGremlinQueryProvider _queryProvider;
        private readonly ImmutableList<IGremlinQueryStrategy> _strategies;

        public GremlinQuerySource(string name, IGraphModel model, IGremlinQueryProvider queryProvider, ImmutableList<IGremlinQueryStrategy> strategies)
        {
            _name = name;
            _model = model;
            _strategies = strategies;
            _queryProvider = queryProvider;
        }

        public IVGremlinQuery<TVertex> AddV<TVertex>(TVertex vertex)
        {
            return Create()
                .AddV(vertex);
        }

        public IVGremlinQuery<TVertex> AddV<TVertex>() where TVertex : new()
        {
            return Create()
                .AddV<TVertex>();
        }

        public IEGremlinQuery<TEdge> AddE<TEdge>(TEdge edge)
        {
            return Create()
                .AddE(edge);
        }

        public IEGremlinQuery<TEdge> AddE<TEdge>() where TEdge : new()
        {
            return Create()
                .AddE<TEdge>();
        }

        public IVGremlinQuery<Vertex> V(params object[] ids)
        {
            return Create()
                .V(ids);
        }

        public IVGremlinQuery<TVertex> V<TVertex>(params object[] ids)
        {
            return Create()
                .V<TVertex>(ids);
        }

        public IEGremlinQuery<Edge> E(params object[] ids)
        {
            return Create()
                .E(ids);
        }

        public IGremlinQuery<TElement> Inject<TElement>(params TElement[] elements)
        {
            return Create()
                .Cast<TElement>()
                .Inject(elements);
        }

        public IGremlinQuerySource WithStrategies(params IGremlinQueryStrategy[] strategies)
        {
            return new GremlinQuerySource(_name, _model, _queryProvider, strategies.ToImmutableList());
        }

        public IGremlinQuerySource WithModel(IGraphModel model)
        {
            return new GremlinQuerySource(_name, model, _queryProvider, _strategies);
        }

        public IGremlinQuerySource SetQueryProvider(IGremlinQueryProvider queryProvider)
        {
            return new GremlinQuerySource(_name, _model, queryProvider, _strategies);
        }

        private IGremlinQuery<Unit> Create()
        {
            var ret = new GremlinQueryImpl<Unit, Unit, Unit>(_model, ImmutableList<Step>.Empty, ImmutableDictionary<StepLabel, string>.Empty)
                .AddStep(new IdentifierStep(_name))
                .SetTypedGremlinQueryProvider(_queryProvider);

            foreach (var strategy in _strategies)
            {
                ret = strategy.Apply(ret);
            }

            return ret;
        }
    }
}
