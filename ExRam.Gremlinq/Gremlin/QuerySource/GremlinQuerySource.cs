using System.Collections.Immutable;
using ExRam.Gremlinq.GraphElements;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public static class GremlinQuerySource
    {
        // ReSharper disable once InconsistentNaming
        #pragma warning disable IDE1006 // Naming Styles
        public static readonly IGremlinQuerySource g = Create("g");
        #pragma warning restore IDE1006 // Naming Styles
    
        public static IGremlinQuerySource Create(string name)
        {
            return new GremlinQuerySourceImpl(name, GraphModel.Dynamic(), GremlinQueryProvider.Invalid, ImmutableList<IGremlinQueryStrategy>.Empty);
        }

        private sealed class GremlinQuerySourceImpl : IGremlinQuerySource
        {
            private readonly string _name;
            private readonly IGraphModel _model;
            private readonly IGremlinQueryProvider _queryProvider;
            private readonly ImmutableList<IGremlinQueryStrategy> _strategies;

            public GremlinQuerySourceImpl(string name, IGraphModel model, IGremlinQueryProvider queryProvider, ImmutableList<IGremlinQueryStrategy> strategies)
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
                return new GremlinQuerySourceImpl(_name, _model, _queryProvider, _strategies.AddRange(strategies));
            }

            public IGremlinQuerySource WithModel(IGraphModel model)
            {
                return new GremlinQuerySourceImpl(_name, model, _queryProvider, _strategies);
            }

            public IGremlinQuerySource WithQueryProvider(IGremlinQueryProvider queryProvider)
            {
                return new GremlinQuerySourceImpl(_name, _model, queryProvider, _strategies);
            }

            private IGremlinQuery<Unit> Create()
            {
                var ret =
                    new GremlinQueryImpl<Unit, Unit, Unit>(
                            _model,
                            new JsonSupportGremlinQueryProvider(_queryProvider),
                            ImmutableList<Step>.Empty,
                            ImmutableDictionary<StepLabel, string>.Empty)
                        .AddStep(new IdentifierStep(_name));

                foreach (var strategy in _strategies)
                {
                    ret = strategy.Apply(ret);
                }

                return ret;
            }
        }
    }
}
