using BenchmarkDotNet.Attributes;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Tests.Entities;
using Newtonsoft.Json.Linq;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public class DeserializerBenchmarks
    {
        private readonly ITransformer _oldDeserializer;
        private readonly ITransformer _newDeserializer;
        private readonly JObject _source = JObject.Parse("{ \"id\": 13, \"label\": \"Person\", \"type\": \"vertex\", \"properties\": { \"Age\": [ { \"id\": 1, \"value\": \"36\" } ], \"RegistrationDate\": [ { \"id\": 2, \"value\": 1481750076295 } ], \"Gender\": [ { \"id\": 3, \"value\": 1 } ], \"PhoneNumbers\": [ { \"id\": 4, \"value\": \"+123456\" }, { \"id\": 5, \"value\": \"+234567\" } ] } }");

        public DeserializerBenchmarks()
        {
            _oldDeserializer = GremlinQueryEnvironment.Invalid
               .UseNewtonsoftJson()
               .Deserializer;

            _newDeserializer = GremlinQueryEnvironment.Invalid
               ./*ShinyAndNew*/UseNewtonsoftJson()
               .Deserializer;
        }

        [Benchmark]
        public void Old()
        {
            _oldDeserializer
                .TryTransform<JObject, Person>(_source, GremlinQueryEnvironment.Invalid, out _);
        }

        [Benchmark]
        public void New()
        {
            _newDeserializer
                .TryTransform<JObject, Person>(_source, GremlinQueryEnvironment.Invalid, out _);
        }
    }
}
