using System.Text;
using Microsoft.CodeAnalysis;

#nullable enable

namespace ExRam.Gremlinq.Core.Generator
{
    [Generator]
    public class GremlinQuerySourceGenerator : IIncrementalGenerator
    {
        private static readonly string[] BaseInterfaces = new[]
        {
            "IGremlinQueryBase",
            "IElementGremlinQueryBase",
            "IEdgeOrVertexGremlinQueryBase",
            "IVertexGremlinQueryBase",
            "IEdgeGremlinQueryBase",
            "IPropertyGremlinQueryBase",

            "IGremlinQuery<T1>",
            "IMapGremlinQuery<T1>",
            "IArrayGremlinQuery<TArray, TArrayItem, TOriginalQuery>",
            "IElementGremlinQuery<T1>",
            "IEdgeOrVertexGremlinQuery<T1>",
            "IVertexGremlinQuery<TVertex>",
            "IEdgeGremlinQuery<TEdge>",
            "IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>",
            "IEdgeGremlinQuery<TEdge, T2, T3>",
            "IInEdgeGremlinQuery<TEdge, T3>",
            "IOutEdgeGremlinQuery<TEdge, T2>",
            "IVertexPropertyGremlinQuery<TProperty, TValue>",
            "IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>",
            "IPropertyGremlinQuery<T1>",
            "IStringGremlinQuery<T1>",
            "IDateGremlinQuery<T1>"
        };

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(Execute);
        }

        private static void Execute(IncrementalGeneratorPostInitializationContext context)
        {
            var substitutedBaseInterfaces = BaseInterfaces
                .Select(x => x
                    .Replace("TVertex", "T1")
                    .Replace("TEdge", "T1")
                    .Replace("TAdjacentVertex", "T2")
                    .Replace("TProperty", "T1")
                    .Replace("TValue", "T2")
                    .Replace("TArrayItem", "T2")
                    .Replace("TArray", "T1")
                    .Replace("TMeta", "T3")
                    .Replace("TOriginalQuery", "T4"))
                .ToArray();

            var sb = new StringBuilder()
                .AppendLine("#nullable enable")
                .AppendLine("using System.Linq.Expressions;")
                .AppendLine("using Gremlin.Net.Process.Traversal;")
                .AppendLine()
                .AppendLine("namespace ExRam.Gremlinq.Core")
                .AppendLine("{")
                .AppendLine("    partial class GremlinQuery<T1, T2, T3, T4>")
                .AppendLine("    {");

            GenerateSelectOverloads(sb);
            GenerateCastOverloads(sb, substitutedBaseInterfaces);
            GenerateTypedInterfaceImplementations(sb, substitutedBaseInterfaces);
            GenerateOfTypeOverloads(sb, substitutedBaseInterfaces);
            GenerateElementOverloads(sb, substitutedBaseInterfaces);

            sb
                .AppendLine("   }")
                .AppendLine("}");

            context.AddSource("GremlinQuery.generated.cs", sb.ToString());
            context.CancellationToken.ThrowIfCancellationRequested();
        }

        private static void GenerateSelectOverloads(StringBuilder sb)
        {
            for (var i = 2; i <= 16; i++)
            {
                var typeArgs = GetArgumentList("TItem{0}", i);
                var labelParams = GetArgumentList("StepLabel<TItem{0}> label{0}", i);
                var byChain = GetArgumentList(".By(__ => __.Select(label{0}))", "", i);

                sb
                    .AppendLine($"        IMapGremlinQuery<({typeArgs})> IGremlinQueryBase.Select<{typeArgs}>({labelParams}) => Project<({typeArgs})>(p => p.ToTuple(){byChain});")
                    .AppendLine();

                var projectionParams = GetArgumentList("Expression<Func<T1, TItem{0}>> projection{0}", i);
                var projByChain = GetArgumentList(".By(__ => __.Select<IGremlinQuery<TItem{0}>>(projection{0}))", "", i);

                sb.AppendLine($"        IMapGremlinQuery<({typeArgs})> IMapGremlinQueryBase<T1>.Select<{typeArgs}>({projectionParams}) => Project<({typeArgs})>(p => p.ToTuple(){projByChain});");
            }

            sb
                .AppendLine();
        }

        private static void GenerateCastOverloads(StringBuilder sb, string[] substitutedBaseInterfaces)
        {
            foreach (var iface in Untyped(substitutedBaseInterfaces))
            {
                var changed = ChangeType(iface, "TResult").Replace("Base", "");
                sb.AppendLine($"        {changed} {iface}.Cast<TResult>() => Cast<TResult>();");
            }

            sb
                .AppendLine();
        }

        private static void GenerateTypedInterfaceImplementations(StringBuilder sb, string[] substitutedBaseInterfaces)
        {
            foreach (var iface in Typed(substitutedBaseInterfaces))
            {
                sb
                    .AppendLine($"        TTargetQuery IGremlinQueryBaseRec<T1, {iface}>.Aggregate<TTargetQuery>(Func<{iface}, StepLabel<IArrayGremlinQuery<T1[], T1, {iface}>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, continuation);")
                    .AppendLine($"        TTargetQuery IGremlinQueryBaseRec<T1, {iface}>.AggregateLocal<TTargetQuery>(Func<{iface}, StepLabel<IArrayGremlinQuery<T1[], T1, {iface}>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, continuation);")
                    .AppendLine()
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<T1, {iface}>.Aggregate(StepLabel<T1[]> stepLabel) => Aggregate(Scope.Global, stepLabel);")
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<T1, {iface}>.AggregateLocal(StepLabel<T1[]> stepLabel) => Aggregate(Scope.Local, stepLabel);")
                    .AppendLine()
                    .AppendLine($"        TTargetQuery IGremlinQueryBaseRec<T1, {iface}>.As<TTargetQuery>(Func<{iface}, StepLabel<{iface}, T1>, TTargetQuery> continuation) => As<StepLabel<{iface}, T1>, TTargetQuery>(continuation);")
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<T1, {iface}>.As(StepLabel<T1> stepLabel) => As(stepLabel);")
                    .AppendLine()
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<{iface}>.And(params Func<{iface}, IGremlinQueryBase>[] andTraversals) => And(andTraversals);")
                    .AppendLine()
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<{iface}>.And(params ReadOnlySpan<Func<{iface}, IGremlinQueryBase>> andTraversals) => And(andTraversals.Cast().To<Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase>>());")
                    .AppendLine()
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<{iface}>.Barrier() => Barrier();")
                    .AppendLine();

                sb
                    .AppendLine($"        TTargetQuery IGremlinQueryBaseRec<{iface}>.Choose<TTargetQuery>(Func<{iface}, IGremlinQueryBase> traversalPredicate, Func<{iface}, TTargetQuery> trueChoice, Func<{iface}, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(traversalPredicate, trueChoice, falseChoice);")
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<{iface}>.Choose(Func<{iface}, IGremlinQueryBase> traversalPredicate, Func<{iface}, {iface}> trueChoice) => Choose<{iface}, {iface}, {iface}>(traversalPredicate, trueChoice);")
                    .AppendLine($"        IGremlinQuery<object> IGremlinQueryBaseRec<{iface}>.Choose(Func<{iface}, IGremlinQueryBase> traversalPredicate, Func<{iface}, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(traversalPredicate, trueChoice);")
                    .AppendLine()
                    .AppendLine($"        TTargetQuery IGremlinQueryBaseRec<{iface}>.Choose<TTargetQuery>(Func<IChooseBuilder<{iface}>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);")
                    .AppendLine()
                    .AppendLine($"        TTargetQuery IGremlinQueryBaseRec<T1, {iface}>.Choose<TTargetQuery>(Expression<Func<T1, bool>> predicate, Func<{iface}, TTargetQuery> trueChoice, Func<{iface}, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(predicate, trueChoice, falseChoice);")
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<T1, {iface}>.Choose(Expression<Func<T1, bool>> predicate, Func<{iface}, {iface}> trueChoice) => Choose<{iface}, {iface}, {iface}>(predicate, trueChoice);")
                    .AppendLine($"        IGremlinQuery<object> IGremlinQueryBaseRec<T1, {iface}>.Choose(Expression<Func<T1, bool>> predicate, Func<{iface}, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(predicate, trueChoice);")
                    .AppendLine();

                sb
                    .AppendLine($"        TTargetQuery IGremlinQueryBaseRec<{iface}>.Coalesce<TTargetQuery>(params Func<{iface}, TTargetQuery>[] traversals) => Coalesce<TTargetQuery, TTargetQuery>(traversals);")
                    .AppendLine($"        IGremlinQuery<object> IGremlinQueryBaseRec<{iface}>.Coalesce(params Func<{iface}, IGremlinQueryBase>[] traversals) => Coalesce<IGremlinQueryBase, IGremlinQuery<object>>(traversals);")
                    .AppendLine()
                    .AppendLine($"        TTargetQuery IGremlinQueryBaseRec<{iface}>.Coalesce<TTargetQuery>(params ReadOnlySpan<Func<{iface}, TTargetQuery>> traversals) => Coalesce<TTargetQuery, TTargetQuery>(traversals.Cast().To<Func<GremlinQuery<T1, T2, T3, T4>, TTargetQuery>>());")
                    .AppendLine($"        IGremlinQuery<object> IGremlinQueryBaseRec<{iface}>.Coalesce(params ReadOnlySpan<Func<{iface}, IGremlinQueryBase>> traversals) => Coalesce<IGremlinQueryBase, IGremlinQuery<object>>(traversals.Cast().To<Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase>>());")
                    .AppendLine();

                sb
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<{iface}>.Coin(double probability) => Coin(probability);")
                    .AppendLine()
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<{iface}>.CyclicPath() => CyclicPath();")
                    .AppendLine()
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<{iface}>.Dedup() => DedupGlobal();")
                    .AppendLine()
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<{iface}>.DedupLocal() => DedupLocal();")
                    .AppendLine()
                    .AppendLine($"        TTargetQuery IGremlinQueryBaseRec<{iface}>.FlatMap<TTargetQuery>(Func<{iface}, TTargetQuery> mapping) => FlatMap(mapping);")
                    .AppendLine()
                    .AppendLine($"        IArrayGremlinQuery<T1[], T1, {iface}> IGremlinQueryBaseRec<T1, {iface}>.ForceArray() => CloneAs<IArrayGremlinQuery<T1[], T1, {iface}>>();")
                    .AppendLine()
                    .AppendLine($"        IArrayGremlinQuery<T1[], T1, {iface}> IGremlinQueryBaseRec<T1, {iface}>.Fold() => Fold<{iface}>();")
                    .AppendLine()
                    .AppendLine($"        IMapGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<T1, {iface}>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<{iface}>, IGroupBuilderWithKeyAndValue<TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);")
                    .AppendLine($"        IMapGremlinQuery<IDictionary<TNewKey, T1[]>> IGremlinQueryBaseRec<T1, {iface}>.Group<TNewKey>(Func<IGroupBuilder<{iface}>, IGroupBuilderWithKey<{iface}, TNewKey>> groupBuilder) => Group(groupBuilder);")
                    .AppendLine()
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<{iface}>.Identity() => Identity();")
                    .AppendLine();

                sb
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<T1, {iface}>.Inject(params T1[] elements) => Inject<T1>(elements);")
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<T1, {iface}>.Inject(params ReadOnlySpan<T1> elements) => Inject(elements);")
                    .AppendLine("        ")
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<{iface}>.Limit(long count) => LimitGlobal(count);")
                    .AppendLine()
                    .AppendLine($"        TTargetQuery IGremlinQueryBaseRec<{iface}>.Local<TTargetQuery>(Func<{iface} , TTargetQuery> localTraversal) => Local(localTraversal);")
                    .AppendLine()
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<{iface}>.Loop(Func<IStartLoopBuilder<{iface}>, IFinalLoopBuilder<{iface}>> loopBuilderTransformation) => Loop(loopBuilderTransformation);")
                    .AppendLine()
                    .AppendLine($"        TTargetQuery IGremlinQueryBaseRec<{iface}>.Map<TTargetQuery>(Func<{iface}, TTargetQuery> mapping) => Map(mapping);")
                    .AppendLine()
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<{iface}>.Max() => MaxGlobal();")
                    .AppendLine()
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<{iface}>.Mean() => MeanGlobal();")
                    .AppendLine()
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<{iface}>.Min() => MinGlobal();")
                    .AppendLine()
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<{iface}>.Not(Func<{iface}, IGremlinQueryBase> notTraversal) => Not(static (__, notTraversal) => notTraversal(__), notTraversal);")
                    .AppendLine()
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<{iface}>.None() => None();")
                    .AppendLine()
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<{iface}>.Optional(Func<{iface}, {iface}> optionalTraversal) => Optional(optionalTraversal);")
                    .AppendLine()
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<{iface}>.Or(params Func<{iface}, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);")
                    .AppendLine()
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<{iface}>.Or(params ReadOnlySpan<Func<{iface}, IGremlinQueryBase>> orTraversals) => Or(orTraversals.Cast().To<Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase>>());")
                    .AppendLine();

                sb
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<T1, {iface}>.Order(Func<IOrderBuilder<T1, {iface}>, IOrderBuilderWithBy<T1, {iface}>> projection) => OrderGlobal(projection);")
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<T1, {iface}>.OrderLocal(Func<IOrderBuilder<T1, {iface}>, IOrderBuilderWithBy<T1, {iface}>> projection) => OrderLocal(projection);")
                    .AppendLine()
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<{iface}>.Order(Func<IOrderBuilder<{iface}>, IOrderBuilderWithBy<{iface}>> projection) => OrderGlobal(projection);")
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<{iface}>.OrderLocal(Func<IOrderBuilder<{iface}>, IOrderBuilderWithBy<{iface}>> projection) => OrderLocal(projection);")
                    .AppendLine()
                    .AppendLine($"        IGremlinQuery<dynamic> IGremlinQueryBaseRec<T1, {iface}>.Project(Func<IProjectBuilder<{iface}, T1>, IProjectDynamicResult> continuation) => Project(continuation);")
                    .AppendLine($"        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, {iface}>.Project<TResult>(Func<IProjectBuilder<{iface}, T1>, IProjectMapResult<TResult>> continuation) => Project(continuation);")
                    .AppendLine($"        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, {iface}>.Project<TResult>(Func<IProjectBuilder<{iface}, T1>, IProjectTupleResult<TResult>> continuation) => Project<TResult>(continuation);")
                    .AppendLine()
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<{iface}>.Range(long low, long high) => RangeGlobal(low, high);")
                    .AppendLine()
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<{iface}>.SideEffect(Func<{iface}, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);")
                    .AppendLine()
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<{iface}>.SimplePath() => SimplePath();")
                    .AppendLine()
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<{iface}>.Skip(long count) => Skip(count, Scope.Global);")
                    .AppendLine()
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<{iface}>.Sum() => SumGlobal();")
                    .AppendLine()
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<{iface}>.Tail(long count) => TailGlobal(count);")
                    .AppendLine()
                    .AppendLine($"        TTargetQuery IGremlinQueryBaseRec<{iface}>.Union<TTargetQuery>(params Func<{iface}, TTargetQuery>[] unionTraversals) => Union<TTargetQuery, TTargetQuery>(unionTraversals);")
                    .AppendLine($"        IGremlinQuery<object> IGremlinQueryBaseRec<{iface}>.Union(params Func<{iface}, IGremlinQueryBase>[] unionTraversals) => Union<IGremlinQueryBase, IGremlinQuery<object>>(unionTraversals);")
                    .AppendLine()
                    .AppendLine($"        TTargetQuery IGremlinQueryBaseRec<{iface}>.Union<TTargetQuery>(params ReadOnlySpan<Func<{iface}, TTargetQuery>> unionTraversals) => Union<TTargetQuery, TTargetQuery>(unionTraversals.Cast().To<Func<GremlinQuery<T1, T2, T3, T4>, TTargetQuery>>());")
                    .AppendLine($"        IGremlinQuery<object> IGremlinQueryBaseRec<{iface}>.Union(params ReadOnlySpan<Func<{iface}, IGremlinQueryBase>> unionTraversals) => Union<IGremlinQueryBase, IGremlinQuery<object>>(unionTraversals.Cast().To<Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase>>());")
                    .AppendLine()
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<T1, {iface}>.Where(Expression<Func<T1, bool>> predicate) => Where(predicate);")
                    .AppendLine($"        {iface} IGremlinQueryBaseRec<{iface}>.Where(Func<{iface}, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);");
            }

            sb
                .AppendLine();
        }

        private static void GenerateOfTypeOverloads(StringBuilder sb, string[] substitutedBaseInterfaces)
        {
            foreach (var iface in Untyped(EdgeOrVertex(substitutedBaseInterfaces)))
            {
                var changed = ChangeType(iface, "TTarget").Replace("Base", "");
                var model = iface.Contains("VertexGremlinQuery")
                    ? "Environment.Model.VerticesModel"
                    : "Environment.Model.EdgesModel";
                sb.AppendLine($"        {changed} {iface}.OfType<TTarget>() => OfType<TTarget, {changed}>({model});");
            }

            sb
                .AppendLine();
        }

        private static void GenerateElementOverloads(StringBuilder sb, string[] substitutedBaseInterfaces)
        {
            foreach (var iface in Typed(Element(substitutedBaseInterfaces)))
            {
                sb
                    .AppendLine($"        {iface} IElementGremlinQueryBaseRec<{iface}>.Property(string key, object? value) => Property(key, value);")
                    .AppendLine($"        {iface} IElementGremlinQueryBaseRec<{iface}>.Property(string key, Func<{iface}, IGremlinQueryBase> valueTransformation) => Property(key, valueTransformation);")
                    .AppendLine()
                    .AppendLine($"        {iface} IElementGremlinQueryBaseRec<T1, {iface}>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, TProjectedValue value) => Property(projection, value);")
                    .AppendLine($"        {iface} IElementGremlinQueryBaseRec<T1, {iface}>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, StepLabel<TProjectedValue> stepLabel) => Property(projection, __ => __.Select(stepLabel));")
                    .AppendLine($"        {iface} IElementGremlinQueryBaseRec<T1, {iface}>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, Func<{iface}, IGremlinQueryBase<TProjectedValue>> valueTraversal) => Property(projection, valueTraversal);")
                    .AppendLine()
                    .AppendLine($"        {iface} IElementGremlinQueryBaseRec<T1, {iface}>.Where<TProjection>(Expression<Func<T1, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQueryBase> propertyTraversal) => Where(projection, propertyTraversal);");
            }
        }

        private static string[] Untyped(IEnumerable<string> interfaces)
        {
            return interfaces
                .Where(iface => GetTypeParameters(iface).Length == 0)
                .ToArray();
        }

        private static string[] Typed(IEnumerable<string> interfaces)
        {
            return interfaces
                .Where(iface => GetTypeParameters(iface).Length > 0)
                .ToArray();
        }

        private static string[] Element(IEnumerable<string> interfaces)
        {
            return interfaces
                .Where(iface => iface.Contains("IElement") || iface.Contains("IVertex") || iface.Contains("Edge"))
                .ToArray();
        }

        private static string[] EdgeOrVertex(IEnumerable<string> interfaces)
        {
            return interfaces
                .Where(iface => iface.Contains("IVertex") || iface.Contains("EdgeGremlinQuery"))
                .ToArray();
        }

        private static string ChangeType(string iface, string newType)
        {
            var elementType = GetElement(iface);
            if (elementType != null)
                return iface.Replace(elementType, newType);

            return $"{iface}<{newType}>";
        }

        private static string[] GetTypeParameters(string str)
        {
            var start = str.IndexOf('<');
            if (start == -1)
                return Array.Empty<string>();

            var end = str.IndexOf('>');
            if (end == -1)
                return Array.Empty<string>();

            return str
                .Substring(start + 1, end - start - 1)
                .Split(',')
                .Select(x => x.Trim())
                .ToArray();
        }

        private static string? GetElement(string str)
        {
            var typeParams = GetTypeParameters(str);
            return typeParams.Length > 0 ? typeParams[0] : null;
        }

        private static string GetArgumentList(string template, int argumentCount)
        {
            return GetArgumentList(template, ", ", argumentCount);
        }

        private static string GetArgumentList(string template, string separator, int argumentCount)
        {
            var sb = new StringBuilder();

            for (var i = 1; i <= argumentCount; i++)
            {
                if (i > 1)
                    sb.Append(separator);

                sb.Append(string.Format(template, i));
            }

            return sb.ToString();
        }
    }
}
