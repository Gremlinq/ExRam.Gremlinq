namespace ExRam.Gremlinq.Providers.Neptune
{
    public interface IDisabledAWSSigner : IAWSSigner
    {
        IAWSSigner UseSigV4();
    }
}
