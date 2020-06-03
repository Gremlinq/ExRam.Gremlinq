namespace ExRam.Gremlinq.Providers.CosmosDb
{
    public interface ICosmosDbConfigurationBuilderWithUri
    {
        ICosmosDbConfigurationBuilderWithAuthKey AuthenticateBy(string authKey);
    }
}
