using ExRam.Gremlinq.Core.Serialization;
using FluentAssertions;
using Gremlin.Net.Process.Traversal;

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
    }
}
