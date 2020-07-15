using System;
using ExRam.Gremlinq.Providers.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        private sealed class UseWebSocketGremlinQueryEnvironmentBuilderTransformation : IWebSocketGremlinQueryEnvironmentBuilderTransformation
        {
            private readonly Func<IWebSocketGremlinQueryExecutorBuilder, IWebSocketGremlinQueryExecutorBuilder> _transformation;

            public UseWebSocketGremlinQueryEnvironmentBuilderTransformation(Func<IWebSocketGremlinQueryExecutorBuilder, IWebSocketGremlinQueryExecutorBuilder> transformation)
            {
                _transformation = transformation;
            }

            public IWebSocketGremlinQueryExecutorBuilder Transform(IWebSocketGremlinQueryExecutorBuilder builder)
            {
                return _transformation(builder);
            }
        }

        public static GremlinqSetup ConfigureWebSocket(this GremlinqSetup setup, Func<IWebSocketGremlinQueryExecutorBuilder, IWebSocketGremlinQueryExecutorBuilder> transformation)
        {
            return setup.RegisterTypes(serviceCollection => serviceCollection.AddSingleton<IWebSocketGremlinQueryEnvironmentBuilderTransformation>(_ => new UseWebSocketGremlinQueryEnvironmentBuilderTransformation(transformation)));
        }
    }
}
