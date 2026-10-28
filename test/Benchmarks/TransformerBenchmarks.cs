using BenchmarkDotNet.Attributes;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Steps;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Tests.Entities;

using Gremlin.Net.Process.Traversal;

using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Benchmarks
{
    // The cost of one TryTransform call on a transformer whose converters exist already:
    // the lookup of the converter chain for the key (environment, static source type, actual
    // source type, target type) and the call of the chain, not its creation.
    [MemoryDiagnoser]
    public class TransformerBenchmarks
    {
        private const int EnvironmentCount = 20;

        private static readonly Instruction CountInstruction = new("count");

        private int _environmentIndex;
        private ITransformer _serializer = null!;
        private ITransformer _singleConverter = null!;
        private IGremlinQueryEnvironment _environment = null!;
        private IGremlinQueryEnvironment[] _environments = null!;

        [GlobalSetup]
        public void Setup()
        {
            var source = g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel.FromBaseTypes<Vertex, Edge>()));

            _environment = source.AsAdmin().Environment;
            _serializer = _environment.Serializer;
            _singleConverter = Transformer.Empty
                .Add(ConverterFactory.Create<string, string>(static (value, _, _, _) => value));

            _environments = new IGremlinQueryEnvironment[EnvironmentCount];

            for (var i = 0; i < _environments.Length; i++)
            {
                _environments[i] = source
                    .ConfigureEnvironment(env => env
                        .ConfigureOptions(options => options))
                    .AsAdmin()
                    .Environment;
            }

            // Create every converter chain once, so that the benchmarks measure the lookup only.
            Step_to_object();
            Instruction_to_instruction_array();
            Key_to_object();
            String_to_string_single_converter();

            for (var i = 0; i < _environments.Length; i++)
            {
                Step_to_object_rotating_environments();
            }
        }

        // The hottest site of the serializer: a step to its instruction.
        [Benchmark]
        public object? Step_to_object() => _serializer.TryTransform<Step, object>(CountStep.Global, _environment, out var value)
            ? value
            : null;

        // A lookup that finds no converter and ends in the fallback.
        [Benchmark]
        public object? Instruction_to_instruction_array() => _serializer.TryTransform<Instruction, Instruction[]>(CountInstruction, _environment, out var value)
            ? value
            : null;

        // A struct source.
        [Benchmark]
        public object? Key_to_object() => _serializer.TryTransform<Key, object>(T.Id, _environment, out var value)
            ? value
            : null;

        // The lookup alone, on a transformer with one converter.
        [Benchmark]
        public string? String_to_string_single_converter() => _singleConverter.TryTransform<string, string>("value", _environment, out var value)
            ? value
            : null;

        // The same site called with many environments in turn, as a test suite does.
        [Benchmark]
        public object? Step_to_object_rotating_environments()
        {
            _environmentIndex = (_environmentIndex + 1) % _environments.Length;

            return _serializer.TryTransform<Step, object>(CountStep.Global, _environments[_environmentIndex], out var value)
                ? value
                : null;
        }
    }
}
