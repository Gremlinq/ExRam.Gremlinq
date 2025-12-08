namespace ExRam.Gremlinq.Providers.Neptune
{
    public interface IDisabledAWSSigner : IAWSSigner
    {
        ISigV4AWSSigner UseSigV4();
    }
}
