#if RELEASE && NET5_0 && !SKIPINTEGRATIONTESTS
using System;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using Xunit;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public class CosmosDbIntegrationTests : QueryIntegrationTest
    {
        public CosmosDbIntegrationTests(ITestOutputHelper testOutputHelper) : base(
            g.ConfigureEnvironment(env => env
                .AddFakePartitionKey()
                .UseCosmosDb(builder => builder
                    .At(new Uri("ws://localhost:8901"), "db", "graph")
                    .AuthenticateBy(
                        "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="))),
            testOutputHelper)
        {
        }

        [Fact(Skip = "x")]
        public override async Task AddE_With_Ignored()
        {
            //Gremlin Query Compilation Error: AddE should follow by a Vertex
        }

        [Fact(Skip = "x")]
        public override async Task AddE_from_to()
        {
            //Gremlin Query Compilation Error: AddE should follow by a Vertex
        }

        [Fact(Skip = "x")]
        public override async Task AddE_to_from()
        {
            //Gremlin Query Compilation Error: AddE should follow by a Vertex
        }

        [Fact(Skip = "x")]
        public override async Task AddV_with_byte_array_property()
        {
            //Gremlin Malformed Request: Unknown GraphSON type encountered: gx: ByteBuffer.Path: 'args.bindings._h', Line: 1, Position: 383.
        }

        [Fact(Skip = "x")]
        public override async Task Aggregate_Global()
        {
            //Gremlin Query Compilation Error: Unable to bind to method 'aggregate', with arguments of type: (Scope, String) @ line 1, column 16.
        }

        [Fact(Skip = "x")]
        public override async Task Choose_only_default_case()
        {
            //Gremlin Query Compilation Error: Unable to resolve symbol 'none' in the current context. @ line 1, column 57.
        }

        [Fact(Skip = "x")]
        public override async Task Choose_two_cases_default()
        {
            //Gremlin Query Compilation Error: Unable to resolve symbol 'none' in the current context. @ line 1, column 113.
        }

        [Fact(Skip = "x")]
        public override async Task Drop()
        {
            //Entity with the specified id does not exist in the system. More info: https://aka.ms/cosmosdb-tsg-not-found
        }
        
        [Fact(Skip = "x")]
        public override async Task Drop_in_local()
        {
            //Entity with the specified id does not exist in the system. More info: https://aka.ms/cosmosdb-tsg-not-found
        }

        [Fact(Skip = "x")]
        public override async Task FilterWithLambda()
        {
            //Gremlin query syntax error: Unsupported groovy language rule: 'closure' text: '{it.get().property('Name').value().length()==2}' @ line 1, column 35.
        }

        [Fact(Skip = "x")]
        public override async Task OrderBy_ThenBy_lambda()
        {
            //    Gremlin query syntax error: Unsupported groovy language rule: 'closure' text: '{it.property('Name').value().length()}' @ line 1, column 60.

            //        Unsupported groovy language rule: 'closure' text: '{it.property('Age').value()}' @ line 1, column 103.
        }

        [Fact(Skip = "x")]
        public override async Task OrderBy_lambda()
        {
            //Gremlin query syntax error: Unsupported groovy language rule: 'closure' text: '{it.property('Name').value().length()}' @ line 1, column 39.
        }

        [Fact(Skip = "x")]
        public override async Task Properties_Meta_Values()
        {
            //Gremlin Query Execution Error: All Values: Cross Apply: The input of values() cannot be a meta or edge property.
        }

        [Fact(Skip = "x")]
        public override async Task Properties_Values_typed()
        {
            //Gremlin Query Execution Error: All Values: Cross Apply: The input of values() cannot be a meta or edge property.
        }

        [Fact(Skip = "x")]
        public override async Task Properties_Values_untyped()
        {
            //Gremlin Query Execution Error: All Values: Cross Apply: The input of values() cannot be a meta or edge property.
        }

        [Fact(Skip = "x")]
        public override async Task Where_property_traversal()
        {
            //Object reference not set to an instance of an object.
        }

        [Fact(Skip = "x")]
        public override async Task WithoutStrategies1()
        {
            //Gremlin Query Compilation Error: Unable to resolve symbol 'SubgraphStrategy' in the current context. @ line 1, column 21.
        }

        [Fact(Skip = "x")]
        public override async Task WithoutStrategies2()
        {
            //Gremlin Query Compilation Error: Unable to resolve symbol 'SubgraphStrategy' in the current context. @ line 1, column 21.

            //    Unable to resolve symbol 'ElementIdStrategy' in the current context. @ line 1, column 39.
        }

        [Fact(Skip = "x")]
        public override async Task WithoutStrategies3()
        {
            //Gremlin Query Compilation Error: Unable to resolve symbol 'SubgraphStrategy' in the current context. @ line 1, column 21.
            //Unable to resolve symbol 'ElementIdStrategy' in the current context. @ line 1, column 39.
        }

        [Fact(Skip = "x")]
        public override async Task Order_Fold_Unfold()
        {
            //OrderBy Id no good idea
        }
    }
}
#endif
