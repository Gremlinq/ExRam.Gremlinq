using Microsoft.CodeAnalysis;
using static ExRam.Gremlinq.Core.Generators.ArgumentListExtensions;

namespace ExRam.Gremlinq.Core.Generators
{
    [Generator]
    public class TreeBuilderSourceGenerator : IIncrementalGenerator
    {
        private const int MaxParameters = 27;

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(Execute);
        }

        private static void Execute(IncrementalGeneratorPostInitializationContext context)
        {
            var code = CodeWriter
                .Create()
                .WriteLine("using ExRam.Gremlinq.Core.Steps;")
                .WriteLine("using System.Linq.Expressions;")
                .WriteLine()
                .WriteLine("namespace ExRam.Gremlinq.Core")
                .Block(writer => writer
                    .Do(GenerateInterfaces)
                    .Do(GenerateTreeBuilderClass))
                .Code();

            context.AddSource("TreeBuilder.generated.cs", code);
            context.CancellationToken.ThrowIfCancellationRequested();
        }

        private static CodeWriter GenerateInterfaces(CodeWriter writer)
        {
            for (var i = 0; i <= MaxParameters; i++)
            {
                var genericArgList = GetGenericArgumentList("TNode{0}", i);

                writer = writer
                    .Write($"public interface ITreeBuilder{genericArgList}");

                if (i >= 1)
                {
                    writer = writer
                        .WriteLine()
                        .Indent(w => w
                            .Write($": ITreeBuilderResult<{GetTreeTypeName(i)}>"));
                }

                for (var j = 1; j <= i; j++)
                {
                    writer = writer
                        .WriteLine()
                        .Indent(w => w
                            .Indent(w2 => w2
                                .Write($"where TNode{j} : notnull")));
                }

                writer = writer
                    .WriteLine()
                    .Block(w =>
                    {
                        if (i < MaxParameters)
                        {
                            var ofArgs = GetArgumentList("TNode{0}", i, hasFollowingArguments: true) + "TNewNode";
                            w = w
                                .WriteLine($"ITreeBuilder<{ofArgs}> Of<TNewNode>() where TNewNode : notnull;");
                        }

                        if (i >= 1)
                        {
                            var byArgs = GetArgumentList("TNode{0}", i - 1, hasFollowingArguments: true) + "TNewNode";
                            w = w
                                .WriteLine()
                                .WriteLine($"ITreeBuilder<{byArgs}> By<TNewNode>(Expression<Func<TNode{i}, TNewNode>> expression) where TNewNode : notnull;");
                        }

                        return w;
                    })
                    .WriteLine();
            }

            return writer;
        }

        private static CodeWriter GenerateTreeBuilderClass(CodeWriter writer)
        {
            var classTypeArgs = GetArgumentList("TNode{0}", MaxParameters);

            return writer
                .WriteLine()
                .WriteLine("partial class GremlinQuery<T1, T2, T3, T4>")
                .Block(w =>
                {
                    w = w
                        .Write($"private sealed partial class TreeBuilder<{classTypeArgs}> :");

                    for (var i = 1; i <= MaxParameters; i++)
                    {
                        w = w
                            .WriteLine()
                            .Indent(w2 => w2
                                .Write($"ITreeBuilder<{GetArgumentList("TNode{0}", i)}>,"));
                    }

                    w = w
                        .WriteLine()
                        .Indent(w2 => w2
                            .Write("ITreeBuilder"));

                    for (var i = 1; i <= MaxParameters; i++)
                    {
                        w = w
                            .WriteLine()
                            .Indent(w2 => w2
                                .Indent(w3 => w3
                                    .Write($"where TNode{i} : notnull")));
                    }

                    return w
                        .WriteLine()
                        .Block(w2 => w2
                            .Do(GenerateOfOverloads)
                            .Do(GenerateByOverloads)
                            .Do(GenerateBuildOverloads));
                });
        }

        private static CodeWriter GenerateOfOverloads(CodeWriter writer)
        {
            for (var i = 0; i < MaxParameters; i++)
            {
                var interfaceGenericArgs = GetGenericArgumentList("TNode{0}", i);
                var returnArgs = GetArgumentList("TNode{0}", i, hasFollowingArguments: true) + "TNewNode";
                var ctorArgs = GetArgumentList("TNode{0}", i, hasFollowingArguments: true) + "TNewNode" + GetArgumentList("object", MaxParameters - 1 - i, hasPreceedingArguments: true);

                writer = writer
                    .WriteLine()
                    .WriteLine($"ITreeBuilder<{returnArgs}> ITreeBuilder{interfaceGenericArgs}.Of<TNewNode>()")
                    .Block(w => w
                        .WriteLine($"return new TreeBuilder<{ctorArgs}>(_sourceQuery, _bySteps.Push(TreeStep.ByIdentityStep.Instance));"));
            }

            return writer;
        }

        private static CodeWriter GenerateByOverloads(CodeWriter writer)
        {
            for (var i = 1; i <= MaxParameters; i++)
            {
                var interfaceArgs = GetArgumentList("TNode{0}", i);
                var returnArgs = GetArgumentList("TNode{0}", i - 1, hasFollowingArguments: true) + " TNewNode";
                var ctorArgs = GetArgumentList("TNode{0}", i - 1, hasFollowingArguments: true) + "TNewNode" + GetArgumentList("object", MaxParameters - i, hasPreceedingArguments: true);

                writer = writer
                    .WriteLine()
                    .WriteLine($"ITreeBuilder<{returnArgs}> ITreeBuilder<{interfaceArgs}>.By<TNewNode>(Expression<Func<TNode{i}, TNewNode>> expression)")
                    .Block(w => w
                        .WriteLine($"return new TreeBuilder<{ctorArgs}>(_sourceQuery, _bySteps.Pop().Push(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));"));
            }

            return writer;
        }

        private static CodeWriter GenerateBuildOverloads(CodeWriter writer)
        {
            for (var i = 1; i <= MaxParameters; i++)
            {
                var treeType = GetTreeTypeName(i);

                writer = writer
                    .WriteLine()
                    .WriteLine($"IGremlinQuery<{treeType}> ITreeBuilderResult<{treeType}>.Build() => Build<{treeType}>();");
            }

            return writer;
        }

        private static string GetTreeTypeName(int parameterCount)
        {
            return GetTreeTypeName(1, parameterCount);
        }

        private static string GetTreeTypeName(int firstParameter, int lastParameter)
        {
            var parameterCount = (lastParameter - firstParameter) + 1;
            if (parameterCount == 1)
                return $"Tree<TNode{firstParameter}, Tree<object>>";

            return $"Tree<TNode{firstParameter}, {GetTreeTypeName(firstParameter + 1, lastParameter)}>";
        }
    }
}
