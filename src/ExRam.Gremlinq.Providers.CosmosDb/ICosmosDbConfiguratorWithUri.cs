namespace ExRam.Gremlinq.Providers.CosmosDb
{
    public interface ICosmosDbConfiguratorWithUri
    {
        ICosmosDbConfiguratorWithAuthKey AuthenticateBy(string authKey);
    }
}
