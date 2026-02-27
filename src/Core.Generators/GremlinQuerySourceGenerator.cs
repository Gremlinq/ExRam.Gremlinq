using Microsoft.CodeAnalysis;
using static ExRam.Gremlinq.Core.Generators.ArgumentListExtensions;

namespace ExRam.Gremlinq.Core.Generators
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
            "IEdgeGremlinQuery<TEdge, TAdjacentVertex>",
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

        public void Initialize(IncrementalGeneratorInitializationContext context) => context.RegisterPostInitializationOutput(Execute);

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

            var code = CodeWriter
                .Create()
                .WriteLine("#nullable enable")
                .WriteLine("using System.Linq.Expressions;")
                .WriteLine("using Gremlin.Net.Process.Traversal;")
                .WriteLine()
                .WriteLine("namespace ExRam.Gremlinq.Core")
                .Block(writer => writer
                    .WriteLine("partial class GremlinQuery<T1, T2, T3, T4>")
                    .Block(w => w
                        .Do(w2 => GenerateSelectOverloads(w2))
                        .Do(w2 => GenerateCastOverloads(w2, substitutedBaseInterfaces))
                        .Do(w2 => GenerateTypedInterfaceImplementations(w2, substitutedBaseInterfaces))
                        .Do(w2 => GenerateOfTypeOverloads(w2, substitutedBaseInterfaces))
                        .Do(w2 => GenerateElementOverloads(w2, substitutedBaseInterfaces))
                        .Do(w2 => GenerateVertexTraversalOverloads(w2))))
                .Code();

            context.AddSource("GremlinQuery.generated.cs", code);
            context.CancellationToken.ThrowIfCancellationRequested();
        }

        private static CodeWriter GenerateSelectOverloads(CodeWriter writer)
        {
            for (var i = 2; i <= 16; i++)
            {
                var typeArgs = GetArgumentList("TItem{0}", i);
                var labelParams = GetArgumentList("StepLabel<TItem{0}> label{0}", i);
                var byChain = GetArgumentList(".By(__ => __.Select(label{0}))", "", i);

                writer = writer
                    .WriteLine($"IMapGremlinQuery<({typeArgs})> IGremlinQueryBase.Select<{typeArgs}>({labelParams}) => Project<({typeArgs})>(p => p.ToTuple(){byChain});")
                    .WriteLine();

                var projectionParams = GetArgumentList("Expression<Func<T1, TItem{0}>> projection{0}", i);
                var projByChain = GetArgumentList(".By(__ => __.Select<IGremlinQuery<TItem{0}>>(projection{0}))", "", i);

                writer = writer
                    .WriteLine($"IMapGremlinQuery<({typeArgs})> IMapGremlinQueryBase<T1>.Select<{typeArgs}>({projectionParams}) => Project<({typeArgs})>(p => p.ToTuple(){projByChain});");
            }

            return writer
                .WriteLine();
        }

        private static CodeWriter GenerateCastOverloads(CodeWriter writer, string[] substitutedBaseInterfaces)
        {
            foreach (var iface in Untyped(substitutedBaseInterfaces))
            {
                var changed = ChangeType(iface, "TResult").Replace("Base", "");
                writer = writer
                    .WriteLine($"{changed} {iface}.Cast<TResult>() => Cast<TResult>();");
            }

            return writer
                .WriteLine();
        }

        private static CodeWriter GenerateTypedInterfaceImplementations(CodeWriter writer, string[] substitutedBaseInterfaces)
        {
            foreach (var iface in Typed(substitutedBaseInterfaces))
            {
                writer = writer
                    .WriteLine($"TTargetQuery IGremlinQueryBaseRec<T1, {iface}>.Aggregate<TTargetQuery>(Func<{iface}, StepLabel<IArrayGremlinQuery<T1[], T1, {iface}>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, continuation);")
                    .WriteLine($"TTargetQuery IGremlinQueryBaseRec<T1, {iface}>.AggregateLocal<TTargetQuery>(Func<{iface}, StepLabel<IArrayGremlinQuery<T1[], T1, {iface}>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, continuation);")
                    .WriteLine()
                    .WriteLine($"{iface} IGremlinQueryBaseRec<T1, {iface}>.Aggregate(StepLabel<T1[]> stepLabel) => Aggregate(Scope.Global, stepLabel);")
                    .WriteLine($"{iface} IGremlinQueryBaseRec<T1, {iface}>.AggregateLocal(StepLabel<T1[]> stepLabel) => Aggregate(Scope.Local, stepLabel);")
                    .WriteLine()
                    .WriteLine($"TTargetQuery IGremlinQueryBaseRec<T1, {iface}>.As<TTargetQuery>(Func<{iface}, StepLabel<{iface}, T1>, TTargetQuery> continuation) => As<StepLabel<{iface}, T1>, TTargetQuery>(continuation);")
                    .WriteLine($"{iface} IGremlinQueryBaseRec<T1, {iface}>.As(StepLabel<T1> stepLabel) => As(stepLabel);")
                    .WriteLine()
                    .WriteLine($"{iface} IGremlinQueryBaseRec<{iface}>.And(params Func<{iface}, IGremlinQueryBase>[] andTraversals) => And(andTraversals);")
                    .WriteLine()
                    .WriteLine($"{iface} IGremlinQueryBaseRec<{iface}>.And(params ReadOnlySpan<Func<{iface}, IGremlinQueryBase>> andTraversals) => And(andTraversals.Cast().To<Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase>>());")
                    .WriteLine()
                    .WriteLine($"{iface} IGremlinQueryBaseRec<{iface}>.Barrier() => Barrier();")
                    .WriteLine()
                    .WriteLine($"TTargetQuery IGremlinQueryBaseRec<{iface}>.Choose<TTargetQuery>(Func<{iface}, IGremlinQueryBase> traversalPredicate, Func<{iface}, TTargetQuery> trueChoice, Func<{iface}, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(traversalPredicate, trueChoice, falseChoice);")
                    .WriteLine($"{iface} IGremlinQueryBaseRec<{iface}>.Choose(Func<{iface}, IGremlinQueryBase> traversalPredicate, Func<{iface}, {iface}> trueChoice) => Choose<{iface}, {iface}, {iface}>(traversalPredicate, trueChoice);")
                    .WriteLine($"IGremlinQuery<object> IGremlinQueryBaseRec<{iface}>.Choose(Func<{iface}, IGremlinQueryBase> traversalPredicate, Func<{iface}, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(traversalPredicate, trueChoice);")
                    .WriteLine()
                    .WriteLine($"TTargetQuery IGremlinQueryBaseRec<{iface}>.Choose<TTargetQuery>(Func<IChooseBuilder<{iface}>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);")
                    .WriteLine()
                    .WriteLine($"TTargetQuery IGremlinQueryBaseRec<T1, {iface}>.Choose<TTargetQuery>(Expression<Func<T1, bool>> predicate, Func<{iface}, TTargetQuery> trueChoice, Func<{iface}, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(predicate, trueChoice, falseChoice);")
                    .WriteLine($"{iface} IGremlinQueryBaseRec<T1, {iface}>.Choose(Expression<Func<T1, bool>> predicate, Func<{iface}, {iface}> trueChoice) => Choose<{iface}, {iface}, {iface}>(predicate, trueChoice);")
                    .WriteLine($"IGremlinQuery<object> IGremlinQueryBaseRec<T1, {iface}>.Choose(Expression<Func<T1, bool>> predicate, Func<{iface}, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(predicate, trueChoice);")
                    .WriteLine()
                    .WriteLine($"TTargetQuery IGremlinQueryBaseRec<{iface}>.Coalesce<TTargetQuery>(params Func<{iface}, TTargetQuery>[] traversals) => Coalesce<TTargetQuery, TTargetQuery>(traversals);")
                    .WriteLine($"IGremlinQuery<object> IGremlinQueryBaseRec<{iface}>.Coalesce(params Func<{iface}, IGremlinQueryBase>[] traversals) => Coalesce<IGremlinQueryBase, IGremlinQuery<object>>(traversals);")
                    .WriteLine()
                    .WriteLine($"TTargetQuery IGremlinQueryBaseRec<{iface}>.Coalesce<TTargetQuery>(params ReadOnlySpan<Func<{iface}, TTargetQuery>> traversals) => Coalesce<TTargetQuery, TTargetQuery>(traversals.Cast().To<Func<GremlinQuery<T1, T2, T3, T4>, TTargetQuery>>());")
                    .WriteLine($"IGremlinQuery<object> IGremlinQueryBaseRec<{iface}>.Coalesce(params ReadOnlySpan<Func<{iface}, IGremlinQueryBase>> traversals) => Coalesce<IGremlinQueryBase, IGremlinQuery<object>>(traversals.Cast().To<Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase>>());")
                    .WriteLine()
                    .WriteLine($"{iface} IGremlinQueryBaseRec<{iface}>.Coin(double probability) => Coin(probability);")
                    .WriteLine()
                    .WriteLine($"{iface} IGremlinQueryBaseRec<{iface}>.CyclicPath() => CyclicPath();")
                    .WriteLine()
                    .WriteLine($"{iface} IGremlinQueryBaseRec<{iface}>.Dedup() => DedupGlobal();")
                    .WriteLine()
                    .WriteLine($"{iface} IGremlinQueryBaseRec<{iface}>.DedupLocal() => DedupLocal();")
                    .WriteLine()
                    .WriteLine($"TTargetQuery IGremlinQueryBaseRec<{iface}>.FlatMap<TTargetQuery>(Func<{iface}, TTargetQuery> mapping) => FlatMap(mapping);")
                    .WriteLine()
                    .WriteLine($"IArrayGremlinQuery<T1[], T1, {iface}> IGremlinQueryBaseRec<T1, {iface}>.ForceArray() => CloneAs<IArrayGremlinQuery<T1[], T1, {iface}>>();")
                    .WriteLine()
                    .WriteLine($"IArrayGremlinQuery<T1[], T1, {iface}> IGremlinQueryBaseRec<T1, {iface}>.Fold() => Fold<{iface}>();")
                    .WriteLine()
                    .WriteLine($"IMapGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<T1, {iface}>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<{iface}>, IGroupBuilderWithKeyAndValue<TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);")
                    .WriteLine($"IMapGremlinQuery<IDictionary<TNewKey, T1[]>> IGremlinQueryBaseRec<T1, {iface}>.Group<TNewKey>(Func<IGroupBuilder<{iface}>, IGroupBuilderWithKey<{iface}, TNewKey>> groupBuilder) => Group(groupBuilder);")
                    .WriteLine()
                    .WriteLine($"{iface} IGremlinQueryBaseRec<{iface}>.Identity() => Identity();")
                    .WriteLine()
                    .WriteLine($"{iface} IGremlinQueryBaseRec<T1, {iface}>.Inject(params T1[] elements) => Inject<T1>(elements);")
                    .WriteLine($"{iface} IGremlinQueryBaseRec<T1, {iface}>.Inject(params ReadOnlySpan<T1> elements) => Inject(elements);")
                    .WriteLine("        ")
                    .WriteLine($"{iface} IGremlinQueryBaseRec<{iface}>.Limit(long count) => LimitGlobal(count);")
                    .WriteLine()
                    .WriteLine($"TTargetQuery IGremlinQueryBaseRec<{iface}>.Local<TTargetQuery>(Func<{iface} , TTargetQuery> localTraversal) => Local(localTraversal);")
                    .WriteLine()
                    .WriteLine($"{iface} IGremlinQueryBaseRec<{iface}>.Loop(Func<IStartLoopBuilder<{iface}>, IFinalLoopBuilder<{iface}>> loopBuilderTransformation) => Loop(loopBuilderTransformation);")
                    .WriteLine()
                    .WriteLine($"TTargetQuery IGremlinQueryBaseRec<{iface}>.Map<TTargetQuery>(Func<{iface}, TTargetQuery> mapping) => Map(mapping);")
                    .WriteLine()
                    .WriteLine($"{iface} IGremlinQueryBaseRec<{iface}>.Max() => MaxGlobal();")
                    .WriteLine()
                    .WriteLine($"{iface} IGremlinQueryBaseRec<{iface}>.Mean() => MeanGlobal();")
                    .WriteLine()
                    .WriteLine($"{iface} IGremlinQueryBaseRec<{iface}>.Min() => MinGlobal();")
                    .WriteLine()
                    .WriteLine($"{iface} IGremlinQueryBaseRec<{iface}>.Not(Func<{iface}, IGremlinQueryBase> notTraversal) => Not(static (__, notTraversal) => notTraversal(__), notTraversal);")
                    .WriteLine()
                    .WriteLine($"{iface} IGremlinQueryBaseRec<{iface}>.None() => None();")
                    .WriteLine()
                    .WriteLine($"{iface} IGremlinQueryBaseRec<{iface}>.Optional(Func<{iface}, {iface}> optionalTraversal) => Optional(optionalTraversal);")
                    .WriteLine()
                    .WriteLine($"{iface} IGremlinQueryBaseRec<{iface}>.Or(params Func<{iface}, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);")
                    .WriteLine()
                    .WriteLine($"{iface} IGremlinQueryBaseRec<{iface}>.Or(params ReadOnlySpan<Func<{iface}, IGremlinQueryBase>> orTraversals) => Or(orTraversals.Cast().To<Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase>>());")
                    .WriteLine()
                    .WriteLine($"{iface} IGremlinQueryBaseRec<T1, {iface}>.Order(Func<IOrderBuilder<T1, {iface}>, IOrderBuilderWithBy<T1, {iface}>> projection) => OrderGlobal(projection);")
                    .WriteLine($"{iface} IGremlinQueryBaseRec<T1, {iface}>.OrderLocal(Func<IOrderBuilder<T1, {iface}>, IOrderBuilderWithBy<T1, {iface}>> projection) => OrderLocal(projection);")
                    .WriteLine()
                    .WriteLine($"{iface} IGremlinQueryBaseRec<{iface}>.Order(Func<IOrderBuilder<{iface}>, IOrderBuilderWithBy<{iface}>> projection) => OrderGlobal(projection);")
                    .WriteLine($"{iface} IGremlinQueryBaseRec<{iface}>.OrderLocal(Func<IOrderBuilder<{iface}>, IOrderBuilderWithBy<{iface}>> projection) => OrderLocal(projection);")
                    .WriteLine()
                    .WriteLine($"IGremlinQuery<dynamic> IGremlinQueryBaseRec<T1, {iface}>.Project(Func<IProjectBuilder<{iface}, T1>, IProjectDynamicResult> continuation) => Project(continuation);")
                    .WriteLine($"IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, {iface}>.Project<TResult>(Func<IProjectBuilder<{iface}, T1>, IProjectMapResult<TResult>> continuation) => Project(continuation);")
                    .WriteLine($"IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, {iface}>.Project<TResult>(Func<IProjectBuilder<{iface}, T1>, IProjectTupleResult<TResult>> continuation) => Project<TResult>(continuation);")
                    .WriteLine()
                    .WriteLine($"{iface} IGremlinQueryBaseRec<{iface}>.Range(long low, long high) => RangeGlobal(low, high);")
                    .WriteLine()
                    .WriteLine($"{iface} IGremlinQueryBaseRec<{iface}>.SideEffect(Func<{iface}, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);")
                    .WriteLine()
                    .WriteLine($"{iface} IGremlinQueryBaseRec<{iface}>.SimplePath() => SimplePath();")
                    .WriteLine()
                    .WriteLine($"{iface} IGremlinQueryBaseRec<{iface}>.Skip(long count) => Skip(count, Scope.Global);")
                    .WriteLine()
                    .WriteLine($"{iface} IGremlinQueryBaseRec<{iface}>.Sum() => SumGlobal();")
                    .WriteLine()
                    .WriteLine($"{iface} IGremlinQueryBaseRec<{iface}>.Tail(long count) => TailGlobal(count);")
                    .WriteLine()
                    .WriteLine($"TTargetQuery IGremlinQueryBaseRec<{iface}>.Union<TTargetQuery>(params Func<{iface}, TTargetQuery>[] unionTraversals) => Union<TTargetQuery, TTargetQuery>(unionTraversals);")
                    .WriteLine($"IGremlinQuery<object> IGremlinQueryBaseRec<{iface}>.Union(params Func<{iface}, IGremlinQueryBase>[] unionTraversals) => Union<IGremlinQueryBase, IGremlinQuery<object>>(unionTraversals);")
                    .WriteLine()
                    .WriteLine($"TTargetQuery IGremlinQueryBaseRec<{iface}>.Union<TTargetQuery>(params ReadOnlySpan<Func<{iface}, TTargetQuery>> unionTraversals) => Union<TTargetQuery, TTargetQuery>(unionTraversals.Cast().To<Func<GremlinQuery<T1, T2, T3, T4>, TTargetQuery>>());")
                    .WriteLine($"IGremlinQuery<object> IGremlinQueryBaseRec<{iface}>.Union(params ReadOnlySpan<Func<{iface}, IGremlinQueryBase>> unionTraversals) => Union<IGremlinQueryBase, IGremlinQuery<object>>(unionTraversals.Cast().To<Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase>>());")
                    .WriteLine()
                    .WriteLine($"{iface} IGremlinQueryBaseRec<T1, {iface}>.Where(Expression<Func<T1, bool>> predicate) => Where(predicate);")
                    .WriteLine($"{iface} IGremlinQueryBaseRec<{iface}>.Where(Func<{iface}, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);");
            }

            return writer
                .WriteLine();
        }

        private static CodeWriter GenerateOfTypeOverloads(CodeWriter writer, string[] substitutedBaseInterfaces)
        {
            foreach (var iface in Untyped(EdgeOrVertex(substitutedBaseInterfaces)))
            {
                var changed = ChangeType(iface, "TTarget").Replace("Base", "");
                var model = iface.Contains("VertexGremlinQuery")
                    ? "Environment.Model.VerticesModel"
                    : "Environment.Model.EdgesModel";
                writer = writer
                    .WriteLine($"{changed} {iface}.OfType<TTarget>() => OfType<{changed}>(SanitizedFilterTypesCache<T1, TTarget>.Types, {model});");

                var multiChanged = ChangeType(iface, "object").Replace("Base", "");

                for (var i = 2; i <= 16; i++)
                {
                    var typeArgs = GetArgumentList("TTarget{0}", i);
                    writer = writer
                        .WriteLine($"{multiChanged} {iface}.OfType<{typeArgs}>() => OfType<{multiChanged}>(SanitizedFilterTypesCache<T1, {typeArgs}>.Types, {model});")
                        .WriteLine();
                }
            }

            return writer
                .WriteLine();
        }

        private static CodeWriter GenerateElementOverloads(CodeWriter writer, string[] substitutedBaseInterfaces)
        {
            foreach (var iface in Typed(Element(substitutedBaseInterfaces)))
            {
                writer = writer
                    .WriteLine($"{iface} IElementGremlinQueryBaseRec<{iface}>.Property(string key, object? value) => Property(key, value);")
                    .WriteLine($"{iface} IElementGremlinQueryBaseRec<{iface}>.Property(string key, Func<{iface}, IGremlinQueryBase> valueTransformation) => Property(key, valueTransformation);")
                    .WriteLine()
                    .WriteLine($"{iface} IElementGremlinQueryBaseRec<T1, {iface}>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, TProjectedValue value) => Property(projection, value);")
                    .WriteLine($"{iface} IElementGremlinQueryBaseRec<T1, {iface}>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, StepLabel<TProjectedValue> stepLabel) => Property(projection, __ => __.Select(stepLabel));")
                    .WriteLine($"{iface} IElementGremlinQueryBaseRec<T1, {iface}>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, Func<{iface}, IGremlinQueryBase<TProjectedValue>> valueTraversal) => Property(projection, valueTraversal);")
                    .WriteLine()
                    .WriteLine($"{iface} IElementGremlinQueryBaseRec<T1, {iface}>.Where<TProjection>(Expression<Func<T1, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQueryBase> propertyTraversal) => Where(projection, propertyTraversal);");
            }

            return writer;
        }

        private static CodeWriter GenerateVertexTraversalOverloads(CodeWriter writer)
        {
            foreach (var method in new[] { "Both", "In", "Out" })
            {
                writer = writer
                    .WriteLine($"IVertexGremlinQuery<object> IVertexGremlinQueryBase.{method}() => {method}(FilterTypes.None);")
                    .WriteLine()
                    .WriteLine($"IVertexGremlinQuery<object> IVertexGremlinQueryBase.{method}<TEdge>() => {method}(SanitizedFilterTypesCache<object, TEdge>.Types);")
                    .WriteLine();

                for (var i = 2; i <= 16; i++)
                {
                    var typeArgs = GetArgumentList("TEdge{0}", i);
                    writer = writer
                        .WriteLine($"IVertexGremlinQuery<object> IVertexGremlinQueryBase.{method}<{typeArgs}>() => {method}(SanitizedFilterTypesCache<object, {typeArgs}>.Types);")
                        .WriteLine();
                }
            }

            foreach (var method in new[] { "BothE", "InE", "OutE" })
            {
                writer = writer
                    .WriteLine($"IEdgeGremlinQuery<object> IVertexGremlinQueryBase.{method}() => {method}<object>(FilterTypes.None);")
                    .WriteLine()
                    .WriteLine($"IEdgeGremlinQuery<TEdge> IVertexGremlinQueryBase.{method}<TEdge>() => {method}<TEdge>(FilterTypesCache<TEdge>.Types);")
                    .WriteLine();

                for (var i = 2; i <= 16; i++)
                {
                    var typeArgs = GetArgumentList("TEdge{0}", i);
                    writer = writer
                        .WriteLine($"IEdgeGremlinQuery<object> IVertexGremlinQueryBase.{method}<{typeArgs}>() => {method}<object>(FilterTypesCache<{typeArgs}>.Types);")
                        .WriteLine();
                }
            }

            writer = writer
                .WriteLine("IEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.BothE() => BothE<object>(FilterTypes.None);")
                .WriteLine()
                .WriteLine("IEdgeGremlinQuery<TEdge, T1> IVertexGremlinQueryBase<T1>.BothE<TEdge>() => BothE<TEdge>(FilterTypesCache<TEdge>.Types);")
                .WriteLine();

            for (var i = 2; i <= 16; i++)
            {
                var typeArgs = GetArgumentList("TEdge{0}", i);
                writer = writer
                    .WriteLine($"IEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.BothE<{typeArgs}>() => BothE<object>(FilterTypesCache<{typeArgs}>.Types);")
                    .WriteLine();
            }

            writer = writer
                .WriteLine("IInEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.InE() => InE<object>(FilterTypes.None);")
                .WriteLine()
                .WriteLine("IInEdgeGremlinQuery<TEdge, T1> IVertexGremlinQueryBase<T1>.InE<TEdge>() => InE<TEdge>(FilterTypesCache<TEdge>.Types);")
                .WriteLine();

            for (var i = 2; i <= 16; i++)
            {
                var typeArgs = GetArgumentList("TEdge{0}", i);
                writer = writer
                    .WriteLine($"IInEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.InE<{typeArgs}>() => InE<object>(FilterTypesCache<{typeArgs}>.Types);")
                    .WriteLine();
            }

            writer = writer
                .WriteLine("IOutEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.OutE() => OutE<object>(FilterTypes.None);")
                .WriteLine()
                .WriteLine("IOutEdgeGremlinQuery<TEdge, T1> IVertexGremlinQueryBase<T1>.OutE<TEdge>() => OutE<TEdge>(FilterTypesCache<TEdge>.Types);")
                .WriteLine();

            for (var i = 2; i <= 16; i++)
            {
                var typeArgs = GetArgumentList("TEdge{0}", i);
                writer = writer
                    .WriteLine($"IOutEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.OutE<{typeArgs}>() => OutE<object>(FilterTypesCache<{typeArgs}>.Types);")
                    .WriteLine();
            }

            return writer;
        }

        private static string[] Untyped(IEnumerable<string> interfaces) => interfaces
            .Where(iface => GetTypeParameters(iface).Length == 0)
            .ToArray();

        private static string[] Typed(IEnumerable<string> interfaces) => interfaces
            .Where(iface => GetTypeParameters(iface).Length > 0)
            .ToArray();

        private static string[] Element(IEnumerable<string> interfaces) => interfaces
            .Where(iface => iface.Contains("IElement") || iface.Contains("IVertex") || iface.Contains("Edge"))
            .ToArray();

        private static string[] EdgeOrVertex(IEnumerable<string> interfaces) => interfaces
            .Where(iface => iface.Contains("IVertex") || iface.Contains("EdgeGremlinQuery"))
            .ToArray();

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
    }
}
