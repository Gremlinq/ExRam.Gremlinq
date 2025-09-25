using ExRam.Gremlinq.Core.Serialization;
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

            return Verify(bytecode.ToGroovyScript(GremlinQueryEnvironment.Invalid, true));
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
    }
}
