namespace ExRam.Gremlinq.Core
{
    public interface ICosmosDbConfigurationBuilderWithUri
    {
        ICosmosDbConfigurationBuilderWithAuthKey AuthenticateBy(string authKey);
    }
}