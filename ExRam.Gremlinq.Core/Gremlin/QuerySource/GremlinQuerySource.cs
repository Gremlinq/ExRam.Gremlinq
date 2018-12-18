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
            private readonly IGremlinQueryExecutor _queryExecutor;
            private readonly ImmutableList<IGremlinQueryStrategy> _strategies;

            public GremlinQuerySourceImpl(string name, IGraphModel model, IGremlinQueryExecutor queryExecutor, ImmutableList<IGremlinQueryStrategy> strategies)
            {
                _name = name;
                _model = model;
                _strategies = strategies;
                _queryExecutor = queryExecutor;
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

            public IVGremlinQuery<IVertex> V(params object[] ids)
            {
                return Create()
                    .V(ids);
            }

            public IVGremlinQuery<TVertex> V<TVertex>(params object[] ids)
            {
                return Create()
                    .V<TVertex>(ids);
            }

            public IEGremlinQuery<IEdge> E(params object[] ids)
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
                return new GremlinQuerySourceImpl(_name, _model, _queryExecutor, _strategies.AddRange(strategies));
            }

            public IGremlinQuerySource WithModel(IGraphModel model)
            {
                return new GremlinQuerySourceImpl(_name, model, _queryExecutor, _strategies);
            }

            public IGremlinQuerySource WithExecutor(IGremlinQueryExecutor executor)
            {
                return new GremlinQuerySourceImpl(_name, _model, executor, _strategies);
            }

            private IGremlinQuery<Unit> Create()
            {
                var ret =
                    new GremlinQueryImpl<Unit, Unit, Unit>(
                            _model,
                            _queryExecutor,
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
