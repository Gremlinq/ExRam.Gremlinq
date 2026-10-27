using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Core.Transformation;
using FluentAssertions;
using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Process.Traversal.Strategy.Verification;

namespace ExRam.Gremlinq.Core.Tests
{
    public class BytecodeExtensionsTest
    {
        [Fact]
        public Task GroovyExpression_is_handled_correctly()
        {
            var bytecode = new Bytecode()
            {
                StepInstructions =
                {
                    new Instruction(
                        "outerInstruction",
                        GroovyExpression.From(
                            "StaticType",
                            [
                                new Instruction("innerInstruction1"),
                                new Instruction("innerInstruction2"),
                                new Instruction("innerInstruction3")
                            ]))
                }
            };

            return Verify(bytecode.ToGroovyScript(GremlinQueryEnvironment.Invalid));
        }

        [Fact]
        public Task ToGroovyScript_with_bindings()
        {
            var bytecode = new Bytecode()
            {
                StepInstructions =
                {
                    new Instruction(
                        "instruction",
                        "a",
                        1)
                }
            };

            return Verify(bytecode.ToGroovyScript(GremlinQueryEnvironment.Invalid));
        }

        [Fact]
        public Task ToGroovyScript_without_bindings()
        {
            var bytecode = new Bytecode()
            {
                StepInstructions =
                {
                    new Instruction(
                        "instruction",
                        "a",
                        1)
                }
            };

            return Verify(bytecode.ToGroovyScript(GremlinQueryEnvironment.Invalid, false));
        }

        [Fact]
        public void GroovyExpression_rejects_an_empty_identifier() => new Action(() => GroovyExpression.From("", []))
            .Should()
            .ThrowExactly<ArgumentException>()
            .WithMessage("A GroovyExpression must have a non-empty identifier.");

        [Fact]
        public void ToGroovyScript_writes_a_type_by_its_name() => new Bytecode
            {
                StepInstructions =
                {
                    new Instruction("withoutStrategies", typeof(ReadOnlyStrategy))
                }
            }
            .ToGroovyScript(GremlinQueryEnvironment.Invalid, false)
            .Script
            .Should()
            .Be("g.withoutStrategies(ReadOnlyStrategy)");

        [Fact]
        public void ToGroovyScript_writes_a_strategy_by_its_GroovyExpression() => new Bytecode
            {
                SourceInstructions =
                {
                    new Instruction("withStrategies", new ReadOnlyStrategy())
                }
            }
            .ToGroovyScript(
                GremlinQueryEnvironment.Invalid
                    .ConfigureSerializer(serializer => serializer
                        .Add(ConverterFactory.Create<ReadOnlyStrategy, GroovyExpression>(static (_, _, _, _) => GroovyExpression.From(nameof(ReadOnlyStrategy), [new Instruction("instance")])))),
                false)
            .Script
            .Should()
            .Be("g.withStrategies(ReadOnlyStrategy.instance())");

        [Fact]
        public void Groovy_debugger_writes_a_DateTime_quoted() => GremlinQueryDebugger.Groovy
            .Debug(
                new Bytecode
                {
                    StepInstructions =
                    {
                        new Instruction("constant", new DateTime(2026, 10, 27, 12, 0, 0, DateTimeKind.Utc))
                    }
                },
                GremlinQueryEnvironment.Invalid)
            .Should()
            .Be("g.constant('2026-10-27T12:00:00.0000000Z')");
    }
}
