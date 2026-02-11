using System;
using System.Collections.Generic;
using System.Linq;
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

            var sb = new StringBuilder();
            sb.AppendLine("#nullable enable");
            sb.AppendLine("using System.Linq.Expressions;");
            sb.AppendLine("using Gremlin.Net.Process.Traversal;");
            sb.AppendLine();
            sb.AppendLine("namespace ExRam.Gremlinq.Core");
            sb.AppendLine("{");
            sb.AppendLine("    partial class GremlinQuery<T1, T2, T3, T4>");
            sb.AppendLine("    {");

            GenerateSelectOverloads(sb);
            GenerateCastOverloads(sb, substitutedBaseInterfaces);
            GenerateTypedInterfaceImplementations(sb, substitutedBaseInterfaces);
            GenerateOfTypeOverloads(sb, substitutedBaseInterfaces);
            GenerateElementOverloads(sb, substitutedBaseInterfaces);

            sb.AppendLine("   }");
            sb.AppendLine("}");

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

                sb.AppendLine(string.Format(
                    "        IMapGremlinQuery<({0})> IGremlinQueryBase.Select<{0}>({1}) => Project<({0})>(p => p.ToTuple(){2});",
                    typeArgs, labelParams, byChain));
                sb.AppendLine();

                var projectionParams = GetArgumentList("Expression<Func<T1, TItem{0}>> projection{0}", i);
                var projByChain = GetArgumentList(".By(__ => __.Select<IGremlinQuery<TItem{0}>>(projection{0}))", "", i);

                sb.AppendLine(string.Format(
                    "        IMapGremlinQuery<({0})> IMapGremlinQueryBase<T1>.Select<{0}>({1}) => Project<({0})>(p => p.ToTuple(){2});",
                    typeArgs, projectionParams, projByChain));
            }

            sb.AppendLine();
        }

        private static void GenerateCastOverloads(StringBuilder sb, string[] substitutedBaseInterfaces)
        {
            foreach (var iface in Untyped(substitutedBaseInterfaces))
            {
                var changed = ChangeType(iface, "TResult").Replace("Base", "");
                sb.AppendLine(string.Format(
                    "        {0} {1}.Cast<TResult>() => Cast<TResult>();",
                    changed, iface));
            }

            sb.AppendLine();
        }

        private static void GenerateTypedInterfaceImplementations(StringBuilder sb, string[] substitutedBaseInterfaces)
        {
            foreach (var iface in Typed(substitutedBaseInterfaces))
            {
                sb.AppendLine(string.Format(
                    "        TTargetQuery IGremlinQueryBaseRec<T1, {0}>.Aggregate<TTargetQuery>(Func<{0}, StepLabel<IArrayGremlinQuery<T1[], T1, {0}>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, continuation);",
                    iface));
                sb.AppendLine(string.Format(
                    "        TTargetQuery IGremlinQueryBaseRec<T1, {0}>.AggregateLocal<TTargetQuery>(Func<{0}, StepLabel<IArrayGremlinQuery<T1[], T1, {0}>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, continuation);",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<T1, {0}>.Aggregate(StepLabel<T1[]> stepLabel) => Aggregate(Scope.Global, stepLabel);",
                    iface));
                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<T1, {0}>.AggregateLocal(StepLabel<T1[]> stepLabel) => Aggregate(Scope.Local, stepLabel);",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        TTargetQuery IGremlinQueryBaseRec<T1, {0}>.As<TTargetQuery>(Func<{0}, StepLabel<{0}, T1>, TTargetQuery> continuation) => As<StepLabel<{0}, T1>, TTargetQuery>(continuation);",
                    iface));
                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<T1, {0}>.As(StepLabel<T1> stepLabel) => As(stepLabel);",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<{0}>.And(params Func<{0}, IGremlinQueryBase>[] andTraversals) => And(andTraversals);",
                    iface));
                sb.AppendLine();
                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<{0}>.And(params ReadOnlySpan<Func<{0}, IGremlinQueryBase>> andTraversals) => And(andTraversals.Cast().To<Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase>>());",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<{0}>.Barrier() => Barrier();",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        TTargetQuery IGremlinQueryBaseRec<{0}>.Choose<TTargetQuery>(Func<{0}, IGremlinQueryBase> traversalPredicate, Func<{0}, TTargetQuery> trueChoice, Func<{0}, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(traversalPredicate, trueChoice, falseChoice);",
                    iface));
                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<{0}>.Choose(Func<{0}, IGremlinQueryBase> traversalPredicate, Func<{0}, {0}> trueChoice) => Choose<{0}, {0}, {0}>(traversalPredicate, trueChoice);",
                    iface));
                sb.AppendLine(string.Format(
                    "        IGremlinQuery<object> IGremlinQueryBaseRec<{0}>.Choose(Func<{0}, IGremlinQueryBase> traversalPredicate, Func<{0}, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(traversalPredicate, trueChoice);",
                    iface));
                sb.AppendLine("        ");
                sb.AppendLine(string.Format(
                    "        TTargetQuery IGremlinQueryBaseRec<{0}>.Choose<TTargetQuery>(Func<IChooseBuilder<{0}>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        TTargetQuery IGremlinQueryBaseRec<T1, {0}>.Choose<TTargetQuery>(Expression<Func<T1, bool>> predicate, Func<{0}, TTargetQuery> trueChoice, Func<{0}, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(predicate, trueChoice, falseChoice);",
                    iface));
                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<T1, {0}>.Choose(Expression<Func<T1, bool>> predicate, Func<{0}, {0}> trueChoice) => Choose<{0}, {0}, {0}>(predicate, trueChoice);",
                    iface));
                sb.AppendLine(string.Format(
                    "        IGremlinQuery<object> IGremlinQueryBaseRec<T1, {0}>.Choose(Expression<Func<T1, bool>> predicate, Func<{0}, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(predicate, trueChoice);",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        TTargetQuery IGremlinQueryBaseRec<{0}>.Coalesce<TTargetQuery>(params Func<{0}, TTargetQuery>[] traversals) => Coalesce<TTargetQuery, TTargetQuery>(traversals);",
                    iface));
                sb.AppendLine(string.Format(
                    "        IGremlinQuery<object> IGremlinQueryBaseRec<{0}>.Coalesce(params Func<{0}, IGremlinQueryBase>[] traversals) => Coalesce<IGremlinQueryBase, IGremlinQuery<object>>(traversals);",
                    iface));
                sb.AppendLine();
                sb.AppendLine(string.Format(
                    "        TTargetQuery IGremlinQueryBaseRec<{0}>.Coalesce<TTargetQuery>(params ReadOnlySpan<Func<{0}, TTargetQuery>> traversals) => Coalesce<TTargetQuery, TTargetQuery>(traversals.Cast().To<Func<GremlinQuery<T1, T2, T3, T4>, TTargetQuery>>());",
                    iface));
                sb.AppendLine(string.Format(
                    "        IGremlinQuery<object> IGremlinQueryBaseRec<{0}>.Coalesce(params ReadOnlySpan<Func<{0}, IGremlinQueryBase>> traversals) => Coalesce<IGremlinQueryBase, IGremlinQuery<object>>(traversals.Cast().To<Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase>>());",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<{0}>.Coin(double probability) => Coin(probability);",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<{0}>.CyclicPath() => CyclicPath();",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<{0}>.Dedup() => DedupGlobal();",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<{0}>.DedupLocal() => DedupLocal();",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        TTargetQuery IGremlinQueryBaseRec<{0}>.FlatMap<TTargetQuery>(Func<{0}, TTargetQuery> mapping) => FlatMap(mapping);",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        IArrayGremlinQuery<T1[], T1, {0}> IGremlinQueryBaseRec<T1, {0}>.ForceArray() => CloneAs<IArrayGremlinQuery<T1[], T1, {0}>>();",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        IArrayGremlinQuery<T1[], T1, {0}> IGremlinQueryBaseRec<T1, {0}>.Fold() => Fold<{0}>();",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        IMapGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<T1, {0}>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<{0}>, IGroupBuilderWithKeyAndValue<TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);",
                    iface));
                sb.AppendLine(string.Format(
                    "        IMapGremlinQuery<IDictionary<TNewKey, T1[]>> IGremlinQueryBaseRec<T1, {0}>.Group<TNewKey>(Func<IGroupBuilder<{0}>, IGroupBuilderWithKey<{0}, TNewKey>> groupBuilder) => Group(groupBuilder);",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<{0}>.Identity() => Identity();",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<T1, {0}>.Inject(params T1[] elements) => Inject<T1>(elements);",
                    iface));
                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<T1, {0}>.Inject(params ReadOnlySpan<T1> elements) => Inject(elements);",
                    iface));
                sb.AppendLine("        ");

                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<{0}>.Limit(long count) => LimitGlobal(count);",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        TTargetQuery IGremlinQueryBaseRec<{0}>.Local<TTargetQuery>(Func<{0} , TTargetQuery> localTraversal) => Local(localTraversal);",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<{0}>.Loop(Func<IStartLoopBuilder<{0}>, IFinalLoopBuilder<{0}>> loopBuilderTransformation) => Loop(loopBuilderTransformation);",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        TTargetQuery IGremlinQueryBaseRec<{0}>.Map<TTargetQuery>(Func<{0}, TTargetQuery> mapping) => Map(mapping);",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<{0}>.Max() => MaxGlobal();",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<{0}>.Mean() => MeanGlobal();",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<{0}>.Min() => MinGlobal();",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<{0}>.Not(Func<{0}, IGremlinQueryBase> notTraversal) => Not(static (__, notTraversal) => notTraversal(__), notTraversal);",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<{0}>.None() => None();",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<{0}>.Optional(Func<{0}, {0}> optionalTraversal) => Optional(optionalTraversal);",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<{0}>.Or(params Func<{0}, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);",
                    iface));
                sb.AppendLine();
                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<{0}>.Or(params ReadOnlySpan<Func<{0}, IGremlinQueryBase>> orTraversals) => Or(orTraversals.Cast().To<Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase>>());",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<T1, {0}>.Order(Func<IOrderBuilder<T1, {0}>, IOrderBuilderWithBy<T1, {0}>> projection) => OrderGlobal(projection);",
                    iface));
                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<T1, {0}>.OrderLocal(Func<IOrderBuilder<T1, {0}>, IOrderBuilderWithBy<T1, {0}>> projection) => OrderLocal(projection);",
                    iface));
                sb.AppendLine();
                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<{0}>.Order(Func<IOrderBuilder<{0}>, IOrderBuilderWithBy<{0}>> projection) => OrderGlobal(projection);",
                    iface));
                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<{0}>.OrderLocal(Func<IOrderBuilder<{0}>, IOrderBuilderWithBy<{0}>> projection) => OrderLocal(projection);",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        IGremlinQuery<dynamic> IGremlinQueryBaseRec<T1, {0}>.Project(Func<IProjectBuilder<{0}, T1>, IProjectDynamicResult> continuation) => Project(continuation);",
                    iface));
                sb.AppendLine(string.Format(
                    "        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, {0}>.Project<TResult>(Func<IProjectBuilder<{0}, T1>, IProjectMapResult<TResult>> continuation) => Project(continuation);",
                    iface));
                sb.AppendLine(string.Format(
                    "        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, {0}>.Project<TResult>(Func<IProjectBuilder<{0}, T1>, IProjectTupleResult<TResult>> continuation) => Project<TResult>(continuation);",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<{0}>.Range(long low, long high) => RangeGlobal(low, high);",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<{0}>.SideEffect(Func<{0}, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<{0}>.SimplePath() => SimplePath();",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<{0}>.Skip(long count) => Skip(count, Scope.Global);",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<{0}>.Sum() => SumGlobal();",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<{0}>.Tail(long count) => TailGlobal(count);",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        TTargetQuery IGremlinQueryBaseRec<{0}>.Union<TTargetQuery>(params Func<{0}, TTargetQuery>[] unionTraversals) => Union<TTargetQuery, TTargetQuery>(unionTraversals);",
                    iface));
                sb.AppendLine(string.Format(
                    "        IGremlinQuery<object> IGremlinQueryBaseRec<{0}>.Union(params Func<{0}, IGremlinQueryBase>[] unionTraversals) => Union<IGremlinQueryBase, IGremlinQuery<object>>(unionTraversals);",
                    iface));
                sb.AppendLine();
                sb.AppendLine(string.Format(
                    "        TTargetQuery IGremlinQueryBaseRec<{0}>.Union<TTargetQuery>(params ReadOnlySpan<Func<{0}, TTargetQuery>> unionTraversals) => Union<TTargetQuery, TTargetQuery>(unionTraversals.Cast().To<Func<GremlinQuery<T1, T2, T3, T4>, TTargetQuery>>());",
                    iface));
                sb.AppendLine(string.Format(
                    "        IGremlinQuery<object> IGremlinQueryBaseRec<{0}>.Union(params ReadOnlySpan<Func<{0}, IGremlinQueryBase>> unionTraversals) => Union<IGremlinQueryBase, IGremlinQuery<object>>(unionTraversals.Cast().To<Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase>>());",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<T1, {0}>.Where(Expression<Func<T1, bool>> predicate) => Where(predicate);",
                    iface));
                sb.AppendLine(string.Format(
                    "        {0} IGremlinQueryBaseRec<{0}>.Where(Func<{0}, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);",
                    iface));
            }

            sb.AppendLine();
        }

        private static void GenerateOfTypeOverloads(StringBuilder sb, string[] substitutedBaseInterfaces)
        {
            foreach (var iface in Untyped(EdgeOrVertex(substitutedBaseInterfaces)))
            {
                var changed = ChangeType(iface, "TTarget").Replace("Base", "");
                var model = iface.Contains("VertexGremlinQuery")
                    ? "Environment.Model.VerticesModel"
                    : "Environment.Model.EdgesModel";
                sb.AppendLine(string.Format(
                    "        {0} {1}.OfType<TTarget>() => OfType<TTarget, {0}>({2});",
                    changed, iface, model));
            }

            sb.AppendLine();
        }

        private static void GenerateElementOverloads(StringBuilder sb, string[] substitutedBaseInterfaces)
        {
            foreach (var iface in Typed(Element(substitutedBaseInterfaces)))
            {
                sb.AppendLine(string.Format(
                    "        {0} IElementGremlinQueryBaseRec<{0}>.Property(string key, object? value) => Property(key, value);",
                    iface));
                sb.AppendLine(string.Format(
                    "        {0} IElementGremlinQueryBaseRec<{0}>.Property(string key, Func<{0}, IGremlinQueryBase> valueTransformation) => Property(key, valueTransformation);",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        {0} IElementGremlinQueryBaseRec<T1, {0}>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, TProjectedValue value) => Property(projection, value);",
                    iface));
                sb.AppendLine(string.Format(
                    "        {0} IElementGremlinQueryBaseRec<T1, {0}>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, StepLabel<TProjectedValue> stepLabel) => Property(projection, __ => __.Select(stepLabel));",
                    iface));
                sb.AppendLine(string.Format(
                    "        {0} IElementGremlinQueryBaseRec<T1, {0}>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, Func<{0}, IGremlinQueryBase<TProjectedValue>> valueTraversal) => Property(projection, valueTraversal);",
                    iface));
                sb.AppendLine();

                sb.AppendLine(string.Format(
                    "        {0} IElementGremlinQueryBaseRec<T1, {0}>.Where<TProjection>(Expression<Func<T1, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQueryBase> propertyTraversal) => Where(projection, propertyTraversal);",
                    iface));
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

            return iface + "<" + newType + ">";
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
