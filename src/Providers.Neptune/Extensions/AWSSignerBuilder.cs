namespace ExRam.Gremlinq.Providers.Neptune
{
    public readonly struct AWSSignerBuilder
    {
        public IAWSSigner UseSigV4() => AWSSigner.EmptySigV4;
    }
}
