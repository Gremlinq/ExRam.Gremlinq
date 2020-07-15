using System;
using ExRam.Gremlinq.Providers.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        private sealed class UseWebSocketGremlinQueryEnvironmentBuilderTransformation : IWebSocketGremlinQueryEnvironmentBuilderTransformation
        {
            private readonly Func<IWebSocketGremlinQueryEnvironmentBuilder, IWebSocketGremlinQueryEnvironmentBuilder> _transformation;

            public UseWebSocketGremlinQueryEnvironmentBuilderTransformation(Func<IWebSocketGremlinQueryEnvironmentBuilder, IWebSocketGremlinQueryEnvironmentBuilder> transformation)
            {
                _transformation = transformation;
            }

            public IWebSocketGremlinQueryEnvironmentBuilder Transform(IWebSocketGremlinQueryEnvironmentBuilder builder)
            {
                return _transformation(builder);
            }
        }

        public static GremlinqSetup ConfigureWebSocket(this GremlinqSetup setup, Func<IWebSocketGremlinQueryEnvironmentBuilder, IWebSocketGremlinQueryEnvironmentBuilder> transformation)
        {
            return setup.RegisterTypes(serviceCollection => serviceCollection.AddSingleton<IWebSocketGremlinQueryEnvironmentBuilderTransformation>(_ => new UseWebSocketGremlinQueryEnvironmentBuilderTransformation(transformation)));
        }
    }
}
