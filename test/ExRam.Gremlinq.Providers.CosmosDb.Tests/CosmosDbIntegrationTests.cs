#if RELEASE && NET5_0 && RUNCOSMOSDBEMULATORINTEGRATIONTESTS
using System;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using Microsoft.Azure.Cosmos;
using Xunit;
using Xunit.Abstractions;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public class CosmosDbIntegrationTests : QueryIntegrationTest, IClassFixture<CosmosDbIntegrationTests.Fixture>
    {
        public new sealed class Fixture : QueryIntegrationTest.Fixture
        {
            private readonly Task _task;

            public Fixture() : base(Gremlinq.Core.GremlinQuerySource.g
                .UseCosmosDb(builder => builder
                    .At(new Uri("ws://localhost:8901"), "db", "graph")
                    .AuthenticateBy(
                        "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="))
                .ConfigureEnvironment(env => env
                    .AddFakePartitionKey()))
            {
                _task = CreateImpl();
            }

            public Task Create()
            {
                return _task;
            }

            private static async Task CreateImpl()
            {
                var cosmosClient = new CosmosClient("https://localhost:8081", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");

                var database = await cosmosClient.CreateDatabaseIfNotExistsAsync("db", ThroughputProperties.CreateAutoscaleThroughput(40000));
                await database.Database.CreateContainerIfNotExistsAsync("graph", "/PartitionKey");
            }
        }

        private static readonly Regex IdRegex2 = new("\"[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}([|]PartitionKey)?\"", RegexOptions.IgnoreCase);

        public CosmosDbIntegrationTests(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
            fixture.Create().Wait();
        }

        public override IImmutableList<Func<string, string>> Scrubbers()
        {
            return base.Scrubbers()
                .Add(x => IdRegex2.Replace(x, "\"scrubbed id\""));
        }

        [Fact(Skip = "Gremlin Query Compilation Error: AddE should follow by a Vertex")]
        public override async Task AddE_With_Ignored() { }

        [Fact(Skip = "Gremlin Query Compilation Error: AddE should follow by a Vertex")]
        public override async Task AddE_from_to() { }

        [Fact(Skip = "Gremlin Query Compilation Error: AddE should follow by a Vertex")]
        public override async Task AddE_to_from() { }

        [Fact(Skip = "Gremlin Query Compilation Error: Unable to bind to method 'aggregate', with arguments of type: (Scope, String) @ line 1, column 16.")]
        public override async Task Aggregate_Global() { }

        [Fact(Skip = "Gremlin Query Compilation Error: Unable to resolve symbol 'none' in the current context. @ line 1, column 57.")]
        public override async Task Choose_only_default_case() { }

        [Fact(Skip = "Gremlin Query Compilation Error: Unable to resolve symbol 'none' in the current context. @ line 1, column 113.")]
        public override async Task Choose_two_cases_default() { }

        [Fact(Skip = "Entity with the specified id does not exist in the system. More info: https://aka.ms/cosmosdb-tsg-not-found")]
        public override async Task Drop() { }

        [Fact(Skip = "Gremlin query syntax error: Unsupported groovy language rule: 'closure' text: '{it.get().property('Name').value().length()==2}' @ line 1, column 35.")]
        public override async Task FilterWithLambda() { }

        [Fact(Skip = "Gremlin query syntax error: Unsupported groovy language rule: 'closure' text: '{it.property('Name').value().length()}' @ line 1, column 60.")]
        public override async Task OrderBy_ThenBy_lambda() { }

        [Fact(Skip = "Gremlin query syntax error: Unsupported groovy language rule: 'closure' text: '{it.property('Name').value().length()}' @ line 1, column 39.")]
        public override async Task OrderBy_lambda() { }

        [Fact(Skip = "Gremlin Query Execution Error: All Values: Cross Apply: The input of values() cannot be a meta or edge property.")]
        public override async Task Properties_Meta_Values() { }

        [Fact(Skip = "Gremlin Query Execution Error: All Values: Cross Apply: The input of values() cannot be a meta or edge property.")]
        public override async Task Properties_Values_typed() { }

        [Fact(Skip = "Gremlin Query Execution Error: All Values: Cross Apply: The input of values() cannot be a meta or edge property.")]
        public override async Task Properties_Values_untyped() { }

        [Fact(Skip = "Object reference not set to an instance of an object.")]
        public override async Task Where_property_traversal() { }

        [Fact(Skip = "Gremlin Query Compilation Error: Unable to resolve symbol 'SubgraphStrategy' in the current context. @ line 1, column 21.")]
        public override async Task WithoutStrategies1() { }

        [Fact(Skip = "Gremlin Query Compilation Error: Unable to resolve symbol 'SubgraphStrategy' in the current context. @ line 1, column 21. Unable to resolve symbol 'ElementIdStrategy' in the current context. @ line 1, column 39.")]
        public override async Task WithoutStrategies2() { }

        [Fact(Skip = "Gremlin Query Compilation Error: Unable to resolve symbol 'SubgraphStrategy' in the current context. @ line 1, column 21. Unable to resolve symbol 'ElementIdStrategy' in the current context. @ line 1, column 39.")]
        public override async Task WithoutStrategies3() { }

        [Fact(Skip = "OrderBy Id no good idea")]
        public override async Task Order_Fold_Unfold() { }

        [Fact(Skip="Unable to find any method 'withSideEffect'")]
        public override async Task WithSideEffect1() { }

        [Fact(Skip = "Unable to find any method 'withSideEffect'")]
        public override async Task WithSideEffect2() { }

        [Fact(Skip = "Cannot create ValueField on non-primitive type GraphTraversal.")]
        public override async Task Property_single_traversal() { }

        [Fact(Skip = "Cannot create ValueField on non-primitive type GraphTraversal.")]
        public override async Task Property_single_from_stepLabel() { }
    }
}
#endif
