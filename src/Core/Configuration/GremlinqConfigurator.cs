namespace ExRam.Gremlinq.Core
{
    public sealed class GremlinqConfigurator : IGremlinqConfigurator<GremlinqConfigurator>
    {
        public static readonly GremlinqConfigurator Identity = new (_ => _);

        private readonly Func<IGremlinQuerySource, IGremlinQuerySource> _transformation;

        private GremlinqConfigurator(Func<IGremlinQuerySource, IGremlinQuerySource> transformation)
        {
            _transformation = transformation;
        }

        public GremlinqConfigurator ConfigureQuerySource(Func<IGremlinQuerySource, IGremlinQuerySource> transformation) => new (_ => transformation(_transformation(_)));

        public IGremlinQuerySource Transform(IGremlinQuerySource source) => _transformation(source);
    }
}
