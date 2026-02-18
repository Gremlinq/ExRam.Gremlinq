using Microsoft.CodeAnalysis;
using static ExRam.Gremlinq.Core.Generators.ArgumentListExtensions;

namespace ExRam.Gremlinq.Core.Generators
{
    [Generator]
    public class ProjectBuilderSourceGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context) => context.RegisterPostInitializationOutput(Execute);

        private static void Execute(IncrementalGeneratorPostInitializationContext context)
        {
            var code = CodeWriter
                .Create()
                .WriteLine("#pragma warning disable CS0109 // Member does not hide an inherited member; new keyword is not required")
                .WriteLine("using System.Linq.Expressions;")
                .WriteLine()
                .WriteLine("namespace ExRam.Gremlinq.Core")
                .Block(writer => writer
                    .Do(GenerateInterfaces)
                    .Do(GenerateProjectBuilderClass))
                .Code();

            context.AddSource("ProjectBuilder.generated.cs", code);
            context.CancellationToken.ThrowIfCancellationRequested();
        }

        private static CodeWriter GenerateInterfaces(CodeWriter writer)
        {
            for (var i = 0; i <= 16; i++)
            {
                var typeArgs = GetArgumentList("TItem{0}", i, hasPreceedingArguments: true);

                writer = writer
                    .Write($"public interface IProjectTupleBuilder<out TSourceQuery, TElement{typeArgs}>");

                if (i >= 2)
                {
                    var tupleArgs = GetArgumentList("TItem{0}", i);
                    writer = writer
                        .WriteLine()
                        .Indent(w => w
                            .Write($": IProjectTupleResult<({tupleArgs})>"));
                }

                writer = writer
                    .WriteLine()
                    .Indent(w => w
                        .Write("where TSourceQuery : IGremlinQueryBase"));

                if (i < 16)
                {
                    var nextTypeArgs = GetArgumentList("TItem{0}", i + 1);

                    writer = writer
                        .WriteLine()
                        .Block(w => w
                            .WriteLine($"IProjectTupleBuilder<TSourceQuery, TElement, {nextTypeArgs}> By<TItem{i + 1}>(Func<TSourceQuery, IGremlinQueryBase<TItem{i + 1}>> projection);")
                            .WriteLine($"IProjectTupleBuilder<TSourceQuery, TElement, {nextTypeArgs}> By<TItem{i + 1}>(Expression<Func<TElement, TItem{i + 1}>> projection);"));
                }
                else
                {
                    writer = writer
                        .WriteLine()
                        .WriteLine(";");
                }

                writer = writer
                    .WriteLine();
            }

            return writer;
        }

        private static CodeWriter GenerateProjectBuilderClass(CodeWriter writer) => writer
            .WriteLine()
            .WriteLine("partial class GremlinQuery<T1, T2, T3, T4>")
            .Block(w =>
            {
                w = w
                    .Write("private sealed partial class ProjectBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16> :");

                for (var i = 2; i <= 16; i++)
                {
                    var args = GetArgumentList("TItem{0}", i);
                    w = w
                        .WriteLine()
                        .Indent(w2 => w2
                            .Write($"IProjectTupleBuilder<GremlinQuery<T1, T2, T3, T4>, T1, {args}>,"));
                }

                w = w
                    .WriteLine()
                    .Indent(w2 => w2
                        .WriteLine("IProjectTupleBuilder<GremlinQuery<T1, T2, T3, T4>, T1>,")
                        .Write("IProjectTupleBuilder<GremlinQuery<T1, T2, T3, T4>, T1, TItem1>"));

                return w
                    .WriteLine()
                    .Block(w2 => w2
                        .Do(GenerateByOverloads)
                        .Do(GenerateBuildOverloads));
            });

        private static CodeWriter GenerateByOverloads(CodeWriter writer)
        {
            for (var i = 0; i < 16; i++)
            {
                var returnTypeArgs = GetArgumentList("TItem{0}", i, hasFollowingArguments: true) + $" TNewItem{i + 1}";
                var interfaceTypeArgs = GetArgumentList("TItem{0}", i, hasPreceedingArguments: true);
                var byLambdaArgs = GetArgumentList("TItem{0}", i, hasFollowingArguments: true) + $"TNewItem{i + 1}" + GetArgumentList("object", 15 - i, hasPreceedingArguments: true);
                var byExpressionArgs = GetArgumentList("TItem{0}", i, hasFollowingArguments: true) + $"TNewItem{i + 1}" + GetArgumentList("object", 15 - i, hasPreceedingArguments: true);

                writer = writer
                    .WriteLine()
                    .WriteLine($"IProjectTupleBuilder<GremlinQuery<T1, T2, T3, T4>, T1, {returnTypeArgs}> IProjectTupleBuilder<GremlinQuery<T1, T2, T3, T4>, T1{interfaceTypeArgs}>.By<TNewItem{i + 1}>(Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase<TNewItem{i + 1}>> projection)")
                    .Block(w => w
                        .WriteLine($"return ByLambda<{byLambdaArgs}>(projection);"))
                    .WriteLine()
                    .WriteLine($"IProjectTupleBuilder<GremlinQuery<T1, T2, T3, T4>, T1, {returnTypeArgs}> IProjectTupleBuilder<GremlinQuery<T1, T2, T3, T4>, T1{interfaceTypeArgs}>.By<TNewItem{i + 1}>(Expression<Func<T1, TNewItem{i + 1}>> projection)")
                    .Block(w => w
                        .WriteLine($"return ByExpression<{byExpressionArgs}>(projection);"));
            }

            return writer
                .WriteLine();
        }

        private static CodeWriter GenerateBuildOverloads(CodeWriter writer)
        {
            for (var i = 2; i <= 16; i++)
            {
                var tupleArgs = GetArgumentList("TItem{0}", i);
                writer = writer
                    .WriteLine($"IMapGremlinQuery<({tupleArgs})> IProjectTupleResult<({tupleArgs})>.Build() => Build<IMapGremlinQuery<({tupleArgs})>>();");
            }

            return writer;
        }
    }
}
