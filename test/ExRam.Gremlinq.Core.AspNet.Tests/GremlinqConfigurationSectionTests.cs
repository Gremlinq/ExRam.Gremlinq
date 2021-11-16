using System;
using System.Collections.Generic;

using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ExRam.Gremlinq.Core.AspNet.Tests
{
    public class GremlinqConfigurationSectionTests
    {
        public GremlinqConfigurationSectionTests() : base()
        {

        }

        [Fact]
        public void Indexer_can_be_null()
        {
            var serviceCollection = new ServiceCollection()
                .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                    .AddInMemoryCollection()
                    .Build())
                .AddGremlinq(s => { })
                .BuildServiceProvider();

            var section = serviceCollection
                .GetRequiredService<IGremlinqConfigurationSection>();

            section["Key"]
                .Should()
                .BeNull();
        }
    }
}
