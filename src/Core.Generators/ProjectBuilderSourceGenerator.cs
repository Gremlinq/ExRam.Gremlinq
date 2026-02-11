using System.Text;
using Microsoft.CodeAnalysis;

namespace ExRam.Gremlinq.Core.Generators
{
    [Generator]
    public class ProjectBuilderSourceGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(Execute);
        }

        private static void Execute(IncrementalGeneratorPostInitializationContext context)
        {
            var sb = new StringBuilder()
                .AppendLine("#pragma warning disable CS0109 // Member does not hide an inherited member; new keyword is not required")
                .AppendLine("using System.Linq.Expressions;")
                .AppendLine()
                .AppendLine("namespace ExRam.Gremlinq.Core")
                .AppendLine("{");

            GenerateInterfaces(sb);
            GenerateProjectBuilderClass(sb);

            sb
                .AppendLine("}");

            context.AddSource("ProjectBuilder.generated.cs", sb.ToString());
            context.CancellationToken.ThrowIfCancellationRequested();
        }

        private static void GenerateInterfaces(StringBuilder sb)
        {
            for (var i = 0; i <= 16; i++)
            {
                var typeArgs = GetArgumentList("TItem{0}", i, hasPrecedingArguments: true);

                sb
                    .AppendLine($"    public interface IProjectTupleBuilder<out TSourceQuery, TElement{typeArgs}>");

                if (i >= 2)
                {
                    var tupleArgs = GetArgumentList("TItem{0}", i);
                    sb
                        .AppendLine($"        : IProjectTupleResult<({tupleArgs})>");
                }

                sb
                    .AppendLine("        where TSourceQuery : IGremlinQueryBase");

                if (i < 16)
                {
                    var nextTypeArgs = GetArgumentList("TItem{0}", i + 1);

                    sb
                        .AppendLine("    {")
                        .AppendLine($"        IProjectTupleBuilder<TSourceQuery, TElement, {nextTypeArgs}> By<TItem{i + 1}>(Func<TSourceQuery, IGremlinQueryBase<TItem{i + 1}>> projection);")
                        .AppendLine($"        IProjectTupleBuilder<TSourceQuery, TElement, {nextTypeArgs}> By<TItem{i + 1}>(Expression<Func<TElement, TItem{i + 1}>> projection);")
                        .AppendLine("    }");
                }
                else
                {
                    sb
                        .AppendLine("    ;");
                }

                sb
                    .AppendLine();
            }
        }

        private static void GenerateProjectBuilderClass(StringBuilder sb)
        {
            sb
                .AppendLine()
                .AppendLine("    partial class GremlinQuery<T1, T2, T3, T4>")
                .AppendLine("    {")
                .AppendLine("        private sealed partial class ProjectBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16> :");

            for (var i = 2; i <= 16; i++)
            {
                var args = GetArgumentList("TItem{0}", i);
                sb
                    .AppendLine($"            IProjectTupleBuilder<GremlinQuery<T1, T2, T3, T4>, T1, {args}>,");
            }

            sb
                .AppendLine("            IProjectTupleBuilder<GremlinQuery<T1, T2, T3, T4>, T1>,")
                .AppendLine("            IProjectTupleBuilder<GremlinQuery<T1, T2, T3, T4>, T1, TItem1>")
                .AppendLine("        {");

            GenerateByOverloads(sb);
            GenerateBuildOverloads(sb);

            sb
                .AppendLine("        }")
                .AppendLine("    }");
        }

        private static void GenerateByOverloads(StringBuilder sb)
        {
            for (var i = 0; i < 16; i++)
            {
                var returnTypeArgs = GetArgumentList("TItem{0}", i, hasFollowingArguments: true) + $" TNewItem{i + 1}";
                var interfaceTypeArgs = GetArgumentList("TItem{0}", i, hasPrecedingArguments: true);
                var byLambdaArgs = GetArgumentList("TItem{0}", i, hasFollowingArguments: true) + $"TNewItem{i + 1}" + GetArgumentList("object", 15 - i, hasPrecedingArguments: true);
                var byExpressionArgs = GetArgumentList("TItem{0}", i, hasFollowingArguments: true) + $"TNewItem{i + 1}" + GetArgumentList("object", 15 - i, hasPrecedingArguments: true);

                sb
                    .AppendLine()
                    .AppendLine($"            IProjectTupleBuilder<GremlinQuery<T1, T2, T3, T4>, T1, {returnTypeArgs}> IProjectTupleBuilder<GremlinQuery<T1, T2, T3, T4>, T1{interfaceTypeArgs}>.By<TNewItem{i + 1}>(Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase<TNewItem{i + 1}>> projection)")
                    .AppendLine("            {")
                    .AppendLine($"                return ByLambda<{byLambdaArgs}>(projection);")
                    .AppendLine("            }")
                    .AppendLine()
                    .AppendLine($"            IProjectTupleBuilder<GremlinQuery<T1, T2, T3, T4>, T1, {returnTypeArgs}> IProjectTupleBuilder<GremlinQuery<T1, T2, T3, T4>, T1{interfaceTypeArgs}>.By<TNewItem{i + 1}>(Expression<Func<T1, TNewItem{i + 1}>> projection)")
                    .AppendLine("            {")
                    .AppendLine($"                return ByExpression<{byExpressionArgs}>(projection);")
                    .AppendLine("            }");
            }

            sb
                .AppendLine();
        }

        private static void GenerateBuildOverloads(StringBuilder sb)
        {
            for (var i = 2; i <= 16; i++)
            {
                var tupleArgs = GetArgumentList("TItem{0}", i);
                sb
                    .AppendLine($"            IMapGremlinQuery<({tupleArgs})> IProjectTupleResult<({tupleArgs})>.Build() => Build<IMapGremlinQuery<({tupleArgs})>>();");
            }
        }

        private static string GetArgumentList(string template, int argumentCount, bool hasPrecedingArguments = false, bool hasFollowingArguments = false)
        {
            return GetArgumentList(template, ", ", argumentCount, hasPrecedingArguments, hasFollowingArguments);
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
