using System.Collections.Immutable;

using ExRam.Gremlinq.Core.Serialization;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Tests
{
    public class BytecodeExtensionsTest : VerifyBase
    {
        public BytecodeExtensionsTest() : base()
        {

        }

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
                            ImmutableArray.Create(
                                new Instruction("innerInstruction1"),
                                new Instruction("innerInstruction2"),
                                new Instruction("innerInstruction3"))))
                }
            };

            return Verify(bytecode.ToGroovy());
        }
    }
}
