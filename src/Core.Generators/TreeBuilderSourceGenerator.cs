using System.Text;
using Microsoft.CodeAnalysis;

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
            var sb = new StringBuilder()
                .AppendLine("using ExRam.Gremlinq.Core.Steps;")
                .AppendLine("using System.Linq.Expressions;")
                .AppendLine()
                .AppendLine("namespace ExRam.Gremlinq.Core")
                .AppendLine("{");

            GenerateInterfaces(sb);
            GenerateTreeBuilderClass(sb);

            sb
                .AppendLine("}");

            context.AddSource("TreeBuilder.generated.cs", sb.ToString());
            context.CancellationToken.ThrowIfCancellationRequested();
        }

        private static void GenerateInterfaces(StringBuilder sb)
        {
            for (var i = 0; i <= MaxParameters; i++)
            {
                var genericArgList = GetGenericArgumentList("TNode{0}", i);

                sb
                    .AppendLine($"    public interface ITreeBuilder{genericArgList}");

                if (i >= 1)
                {
                    sb
                        .AppendLine($"        : ITreeBuilderResult<{GetTreeTypeName(i)}>");
                }

                for (var j = 1; j <= i; j++)
                {
                    sb
                        .AppendLine($"            where TNode{j} : notnull");
                }

                sb
                    .AppendLine("    {");

                if (i < MaxParameters)
                {
                    var ofArgs = GetArgumentList("TNode{0}", i, hasFollowingArguments: true) + "TNewNode";
                    sb
                        .AppendLine($"        ITreeBuilder<{ofArgs}> Of<TNewNode>() where TNewNode : notnull;");
                }

                if (i >= 1)
                {
                    var byArgs = GetArgumentList("TNode{0}", i - 1, hasFollowingArguments: true) + "TNewNode";
                    sb
                        .AppendLine()
                        .AppendLine($"        ITreeBuilder<{byArgs}> By<TNewNode>(Expression<Func<TNode{i}, TNewNode>> expression) where TNewNode : notnull;");
                }

                sb
                    .AppendLine("    }")
                    .AppendLine();
            }
        }

        private static void GenerateTreeBuilderClass(StringBuilder sb)
        {
            var classTypeArgs = GetArgumentList("TNode{0}", MaxParameters);

            sb
                .AppendLine()
                .AppendLine("    partial class GremlinQuery<T1, T2, T3, T4>")
                .AppendLine("    {")
                .AppendLine($"        private sealed partial class TreeBuilder<{classTypeArgs}> :");

            for (var i = 1; i <= MaxParameters; i++)
            {
                sb
                    .AppendLine($"            ITreeBuilder<{GetArgumentList("TNode{0}", i)}>,");
            }

            sb
                .AppendLine("            ITreeBuilder");

            for (var i = 1; i <= MaxParameters; i++)
            {
                sb
                    .AppendLine($"                where TNode{i} : notnull");
            }

            sb
                .AppendLine("        {");

            GenerateOfOverloads(sb);
            GenerateByOverloads(sb);
            GenerateBuildOverloads(sb);

            sb
                .AppendLine("        }")
                .AppendLine("    }");
        }

        private static void GenerateOfOverloads(StringBuilder sb)
        {
            for (var i = 0; i < MaxParameters; i++)
            {
                var interfaceGenericArgs = GetGenericArgumentList("TNode{0}", i);
                var returnArgs = GetArgumentList("TNode{0}", i, hasFollowingArguments: true) + "TNewNode";
                var ctorArgs = GetArgumentList("TNode{0}", i, hasFollowingArguments: true) + "TNewNode" + GetArgumentList("object", MaxParameters - 1 - i, hasPreceedingArguments: true);

                sb
                    .AppendLine()
                    .AppendLine($"            ITreeBuilder<{returnArgs}> ITreeBuilder{interfaceGenericArgs}.Of<TNewNode>()")
                    .AppendLine("            {")
                    .AppendLine($"                return new TreeBuilder<{ctorArgs}>(_sourceQuery, _bySteps.Push(TreeStep.ByIdentityStep.Instance));")
                    .AppendLine("            }");
            }
        }

        private static void GenerateByOverloads(StringBuilder sb)
        {
            for (var i = 1; i <= MaxParameters; i++)
            {
                var interfaceArgs = GetArgumentList("TNode{0}", i);
                var returnArgs = GetArgumentList("TNode{0}", i - 1, hasFollowingArguments: true) + " TNewNode";
                var ctorArgs = GetArgumentList("TNode{0}", i - 1, hasFollowingArguments: true) + "TNewNode" + GetArgumentList("object", MaxParameters - i, hasPreceedingArguments: true);

                sb
                    .AppendLine()
                    .AppendLine($"            ITreeBuilder<{returnArgs}> ITreeBuilder<{interfaceArgs}>.By<TNewNode>(Expression<Func<TNode{i}, TNewNode>> expression)")
                    .AppendLine("            {")
                    .AppendLine($"                return new TreeBuilder<{ctorArgs}>(_sourceQuery, _bySteps.Pop().Push(new TreeStep.ByKeyStep(_sourceQuery.GetKey(expression))));")
                    .AppendLine("            }");
            }
        }

        private static void GenerateBuildOverloads(StringBuilder sb)
        {
            for (var i = 1; i <= MaxParameters; i++)
            {
                var treeType = GetTreeTypeName(i);

                sb
                    .AppendLine()
                    .AppendLine($"            IGremlinQuery<{treeType}> ITreeBuilderResult<{treeType}>.Build() => Build<{treeType}>();");
            }
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

        private static string GetGenericArgumentList(string template, int argumentCount)
        {
            if (argumentCount > 0)
                return $"<{GetArgumentList(template, argumentCount)}>";

            return "";
        }

        private static string GetArgumentList(string template, int argumentCount, bool hasPreceedingArguments = false, bool hasFollowingArguments = false)
        {
            return GetArgumentList(template, ", ", argumentCount, hasPreceedingArguments, hasFollowingArguments);
        }

        private static string GetArgumentList(string template, string separator, int argumentCount, bool hasPreceedingArguments = false, bool hasFollowingArguments = false)
        {
            var sb = new StringBuilder();

            if (argumentCount > 0 && hasPreceedingArguments)
                sb.Append(separator);

            for (var i = 1; i <= argumentCount; i++)
            {
                if (i > 1)
                    sb.Append(separator);

                sb.Append(string.Format(template, i));
            }

            if (argumentCount > 0 && hasFollowingArguments)
                sb.Append(separator);

            return sb.ToString();
        }
    }
}
