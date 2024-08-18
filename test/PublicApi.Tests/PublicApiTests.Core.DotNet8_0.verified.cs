﻿namespace ExRam.Gremlinq.Core
{
    public static class BytecodeExtensions
    {
        public static ExRam.Gremlinq.Core.Serialization.GroovyGremlinScript ToGroovyScript(this Gremlin.Net.Process.Traversal.Bytecode bytecode, ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment, bool includeBindings = true) { }
    }
    [System.Flags]
    public enum DisabledTextPredicates
    {
        None = 0,
        Containing = 1,
        EndingWith = 2,
        NotContaining = 4,
        NotEndingWith = 8,
        NotStartingWith = 16,
        StartingWith = 32,
        Regex = 64,
        NotRegex = 128,
    }
    [System.Flags]
    public enum EdgeFeatures
    {
        None = 0,
        AddEdges = 1,
        Upsert = 2,
        RemoveEdges = 4,
        AnyIds = 8,
        UuidIds = 16,
        UserSuppliedIds = 32,
        CustomIds = 64,
        NumericIds = 128,
        RemoveProperty = 256,
        AddProperty = 512,
        StringIds = 1024,
        All = 2047,
    }
    [System.Flags]
    public enum EdgePropertyFeatures
    {
        None = 0,
        Properties = 1,
        SerializableValues = 2,
        UniformListValues = 4,
        BooleanArrayValues = 8,
        DoubleArrayValues = 16,
        IntegerArrayValues = 32,
        StringArrayValues = 64,
        FloatValues = 128,
        LongValues = 256,
        MixedListValues = 512,
        StringValues = 1024,
        LongArrayValues = 2048,
        MapValues = 4096,
        ByteArrayValues = 8192,
        FloatArrayValues = 16384,
        BooleanValues = 32768,
        ByteValues = 65536,
        DoubleValues = 131072,
        IntegerValues = 262144,
        All = 524287,
    }
    public static class EnumerableExtensions
    {
        public static ExRam.Gremlinq.Core.Traversal ToTraversal(this System.Collections.Generic.IEnumerable<ExRam.Gremlinq.Core.Steps.Step> source) { }
    }
    public sealed class ExpressionNotSupportedException : System.NotSupportedException
    {
        public ExpressionNotSupportedException() { }
        public ExpressionNotSupportedException(System.Exception innerException) { }
        public ExpressionNotSupportedException(System.Linq.Expressions.Expression expression) { }
        public ExpressionNotSupportedException(string message) { }
        public ExpressionNotSupportedException(System.Linq.Expressions.Expression expression, System.Exception innerException) { }
    }
    public static class FeatureSet
    {
        public static ExRam.Gremlinq.Core.IFeatureSet Full;
        public static ExRam.Gremlinq.Core.IFeatureSet None;
    }
    public static class FeatureSetExtensions
    {
        public static bool Supports(this ExRam.Gremlinq.Core.IFeatureSet featureSet, ExRam.Gremlinq.Core.EdgeFeatures edgeFeatures) { }
        public static bool Supports(this ExRam.Gremlinq.Core.IFeatureSet featureSet, ExRam.Gremlinq.Core.EdgePropertyFeatures edgePropertyFeatures) { }
        public static bool Supports(this ExRam.Gremlinq.Core.IFeatureSet featureSet, ExRam.Gremlinq.Core.GraphFeatures graphFeatures) { }
        public static bool Supports(this ExRam.Gremlinq.Core.IFeatureSet featureSet, ExRam.Gremlinq.Core.VariableFeatures variableFeatures) { }
        public static bool Supports(this ExRam.Gremlinq.Core.IFeatureSet featureSet, ExRam.Gremlinq.Core.VertexFeatures vertexFeatures) { }
        public static bool Supports(this ExRam.Gremlinq.Core.IFeatureSet featureSet, ExRam.Gremlinq.Core.VertexPropertyFeatures vertexPropertyFeatures) { }
    }
    public enum FilterLabelsVerbosity
    {
        Maximum = 0,
        Minimum = 1,
    }
    [System.Flags]
    public enum GraphFeatures
    {
        None = 0,
        Transactions = 1,
        Computer = 2,
        IoWrite = 4,
        IoRead = 8,
        ThreadedTransactions = 16,
        Persistence = 32,
        ConcurrentAccess = 64,
        All = 127,
    }
    public static class GremlinQueryAdminExtensions
    {
        public static TTargetQuery AddSteps<TTargetQuery>(this ExRam.Gremlinq.Core.IGremlinQueryAdmin admin, System.Collections.Generic.IEnumerable<ExRam.Gremlinq.Core.Steps.Step> steps)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase { }
    }
    public struct GremlinQueryAwaiter<TElement> : System.Runtime.CompilerServices.ICriticalNotifyCompletion, System.Runtime.CompilerServices.INotifyCompletion
    {
        public bool IsCompleted { get; }
        public TElement[] GetResult() { }
        public void OnCompleted(System.Action continuation) { }
        public void UnsafeOnCompleted(System.Action continuation) { }
    }
    public static class GremlinQueryDebugger
    {
        public static readonly ExRam.Gremlinq.Core.IGremlinQueryDebugger Groovy;
    }
    public static class GremlinQueryEnvironment
    {
        public static readonly ExRam.Gremlinq.Core.IGremlinQueryEnvironment Invalid;
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment UseDebugger(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment, ExRam.Gremlinq.Core.IGremlinQueryDebugger debugger) { }
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment UseDeserializer(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment, ExRam.Gremlinq.Core.Transformation.ITransformer deserializer) { }
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment UseExecutor(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment, ExRam.Gremlinq.Core.Execution.IGremlinQueryExecutor executor) { }
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment UseLogger(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment source, Microsoft.Extensions.Logging.ILogger logger) { }
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment UseModel(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment source, ExRam.Gremlinq.Core.Models.IGraphModel model) { }
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment UseSerializer(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment, ExRam.Gremlinq.Core.Transformation.ITransformer serializer) { }
    }
    public static class GremlinQueryEnvironmentExtensions
    {
        public static bool SupportsType(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment, System.Type type) { }
        public static bool SupportsTypeNatively(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment, System.Type type) { }
    }
    public static class GremlinQueryExtensions
    {
        public static System.Threading.Tasks.ValueTask<TElement> FirstAsync<TElement>(this ExRam.Gremlinq.Core.IGremlinQueryBase<TElement> query, System.Threading.CancellationToken ct = default) { }
        public static System.Threading.Tasks.ValueTask<TElement?> FirstOrDefaultAsync<TElement>(this ExRam.Gremlinq.Core.IGremlinQueryBase<TElement> query, System.Threading.CancellationToken ct = default) { }
        public static System.Threading.Tasks.ValueTask<TElement> LastAsync<TElement>(this ExRam.Gremlinq.Core.IGremlinQueryBase<TElement> query, System.Threading.CancellationToken ct = default) { }
        public static System.Threading.Tasks.ValueTask<TElement?> LastOrDefaultAsync<TElement>(this ExRam.Gremlinq.Core.IGremlinQueryBase<TElement> query, System.Threading.CancellationToken ct = default) { }
        public static System.Threading.Tasks.ValueTask<TElement> SingleAsync<TElement>(this ExRam.Gremlinq.Core.IGremlinQueryBase<TElement> query, System.Threading.CancellationToken ct = default) { }
        public static System.Threading.Tasks.ValueTask<TElement?> SingleOrDefaultAsync<TElement>(this ExRam.Gremlinq.Core.IGremlinQueryBase<TElement> query, System.Threading.CancellationToken ct = default) { }
        public static System.Threading.Tasks.ValueTask<TElement[]> ToArrayAsync<TElement>(this ExRam.Gremlinq.Core.IGremlinQueryBase<TElement> query, System.Threading.CancellationToken ct = default) { }
    }
    public static class GremlinQuerySource
    {
        public static readonly ExRam.Gremlinq.Core.IGremlinQuerySource g;
    }
    public static class GremlinqOption
    {
        public static readonly ExRam.Gremlinq.Core.GremlinqOption<string> Alias;
        public static readonly ExRam.Gremlinq.Core.GremlinqOption<ExRam.Gremlinq.Core.DisabledTextPredicates> DisabledTextPredicates;
        public static readonly ExRam.Gremlinq.Core.GremlinqOption<ExRam.Gremlinq.Core.Traversal> EdgeProjectionSteps;
        public static readonly ExRam.Gremlinq.Core.GremlinqOption<ExRam.Gremlinq.Core.Traversal> EmptyProjectionProtectionDecoratorSteps;
        public static readonly ExRam.Gremlinq.Core.GremlinqOption<bool> EnableEmptyProjectionValueProtection;
        public static readonly ExRam.Gremlinq.Core.GremlinqOption<ExRam.Gremlinq.Core.FilterLabelsVerbosity> FilterLabelsVerbosity;
        public static readonly ExRam.Gremlinq.Core.GremlinqOption<ExRam.Gremlinq.Core.QueryLogFormatting> QueryLogFormatting;
        public static readonly ExRam.Gremlinq.Core.GremlinqOption<Microsoft.Extensions.Logging.LogLevel> QueryLogLogLevel;
        public static readonly ExRam.Gremlinq.Core.GremlinqOption<ExRam.Gremlinq.Core.QueryLogVerbosity> QueryLogVerbosity;
        public static readonly ExRam.Gremlinq.Core.GremlinqOption<System.Func<ExRam.Gremlinq.Core.StepLabel, ExRam.Gremlinq.Core.Projections.Projection>> StepLabelProjectionFallback;
        public static readonly ExRam.Gremlinq.Core.GremlinqOption<ExRam.Gremlinq.Core.StringComparisonTranslationStrictness> StringComparisonTranslationStrictness;
        public static readonly ExRam.Gremlinq.Core.GremlinqOption<System.Collections.Immutable.IImmutableDictionary<Gremlin.Net.Process.Traversal.T, ExRam.Gremlinq.Core.Models.SerializationBehaviour>> TSerializationBehaviourOverrides;
        public static readonly ExRam.Gremlinq.Core.GremlinqOption<ExRam.Gremlinq.Core.Traversal> VertexProjectionSteps;
        public static readonly ExRam.Gremlinq.Core.GremlinqOption<ExRam.Gremlinq.Core.Traversal> VertexProjectionWithoutMetaPropertiesSteps;
        public static readonly ExRam.Gremlinq.Core.GremlinqOption<ExRam.Gremlinq.Core.Traversal> VertexPropertyProjectionSteps;
        public static readonly ExRam.Gremlinq.Core.GremlinqOption<ExRam.Gremlinq.Core.Traversal> VertexPropertyProjectionWithoutMetaPropertiesSteps;
        public static readonly ExRam.Gremlinq.Core.GremlinqOption<bool> WorkaroundRangeInconsistencies;
        public static ExRam.Gremlinq.Core.GremlinqOption<TValue> Create<TValue>(TValue defaultValue) { }
    }
    public sealed class GremlinqOption<TValue>
    {
        public TValue DefaultValue { get; }
    }
    public static class GremlinqOptions
    {
        public static readonly ExRam.Gremlinq.Core.IGremlinqOptions Empty;
    }
    public interface IArrayGremlinQueryBase : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IGremlinQuery<object[]> Lower();
        ExRam.Gremlinq.Core.IGremlinQuery<object> MaxLocal();
        ExRam.Gremlinq.Core.IGremlinQuery<object> MeanLocal();
        ExRam.Gremlinq.Core.IGremlinQuery<object> MinLocal();
        ExRam.Gremlinq.Core.IGremlinQuery<object> SumLocal();
        ExRam.Gremlinq.Core.IGremlinQuery<object> Unfold();
    }
    public interface IArrayGremlinQueryBaseRec<TSelf> : ExRam.Gremlinq.Core.IArrayGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IArrayGremlinQueryBaseRec<TSelf>
    {
        TSelf LimitLocal(long count);
        TSelf RangeLocal(long low, long high);
        TSelf SkipLocal(long count);
        TSelf TailLocal(long count);
    }
    public interface IArrayGremlinQueryBaseRec<TArrayItem, TSelf> : ExRam.Gremlinq.Core.IArrayGremlinQueryBase, ExRam.Gremlinq.Core.IArrayGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IArrayGremlinQueryBase<TArrayItem>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IArrayGremlinQueryBaseRec<TArrayItem, TSelf> { }
    public interface IArrayGremlinQueryBaseRec<TArray, TArrayItem, TSelf> : ExRam.Gremlinq.Core.IArrayGremlinQueryBase, ExRam.Gremlinq.Core.IArrayGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IArrayGremlinQueryBaseRec<TArrayItem, TSelf>, ExRam.Gremlinq.Core.IArrayGremlinQueryBase<TArrayItem>, ExRam.Gremlinq.Core.IArrayGremlinQueryBase<TArray, TArrayItem>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TArray, TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase<TArray>, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IArrayGremlinQueryBaseRec<TArray, TArrayItem, TSelf> { }
    public interface IArrayGremlinQueryBaseRec<TArray, TArrayItem, out TOriginalQuery, TSelf> : ExRam.Gremlinq.Core.IArrayGremlinQueryBase, ExRam.Gremlinq.Core.IArrayGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IArrayGremlinQueryBaseRec<TArrayItem, TSelf>, ExRam.Gremlinq.Core.IArrayGremlinQueryBaseRec<TArray, TArrayItem, TSelf>, ExRam.Gremlinq.Core.IArrayGremlinQueryBase<TArrayItem>, ExRam.Gremlinq.Core.IArrayGremlinQueryBase<TArray, TArrayItem>, ExRam.Gremlinq.Core.IArrayGremlinQueryBase<TArray, TArrayItem, TOriginalQuery>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TArray, TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase<TArray>, ExRam.Gremlinq.Core.IStartGremlinQuery
        where out TOriginalQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
        where TSelf : ExRam.Gremlinq.Core.IArrayGremlinQueryBaseRec<TArray, TArrayItem, TOriginalQuery, TSelf> { }
    public interface IArrayGremlinQueryBase<TArrayItem> : ExRam.Gremlinq.Core.IArrayGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IGremlinQuery<TArrayItem[]> Lower();
        ExRam.Gremlinq.Core.IGremlinQuery<TArrayItem> MaxLocal();
        ExRam.Gremlinq.Core.IGremlinQuery<TArrayItem> MeanLocal();
        ExRam.Gremlinq.Core.IGremlinQuery<TArrayItem> MinLocal();
        ExRam.Gremlinq.Core.IGremlinQuery<TArrayItem> SumLocal();
        ExRam.Gremlinq.Core.IGremlinQuery<TArrayItem> Unfold();
    }
    public interface IArrayGremlinQueryBase<TArray, TArrayItem> : ExRam.Gremlinq.Core.IArrayGremlinQueryBase, ExRam.Gremlinq.Core.IArrayGremlinQueryBase<TArrayItem>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TArray>, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        new ExRam.Gremlinq.Core.IGremlinQuery<TArray> Lower();
    }
    public interface IArrayGremlinQueryBase<TArray, TArrayItem, out TOriginalQuery> : ExRam.Gremlinq.Core.IArrayGremlinQueryBase, ExRam.Gremlinq.Core.IArrayGremlinQueryBase<TArrayItem>, ExRam.Gremlinq.Core.IArrayGremlinQueryBase<TArray, TArrayItem>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TArray>, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        TOriginalQuery MaxLocal();
        TOriginalQuery MeanLocal();
        TOriginalQuery MinLocal();
        TOriginalQuery SumLocal();
        TOriginalQuery Unfold();
    }
    public interface IArrayGremlinQuery<TArray, TArrayItem, TOriginalQuery> : ExRam.Gremlinq.Core.IArrayGremlinQueryBase, ExRam.Gremlinq.Core.IArrayGremlinQueryBaseRec<ExRam.Gremlinq.Core.IArrayGremlinQuery<TArray, TArrayItem, TOriginalQuery>>, ExRam.Gremlinq.Core.IArrayGremlinQueryBaseRec<TArrayItem, ExRam.Gremlinq.Core.IArrayGremlinQuery<TArray, TArrayItem, TOriginalQuery>>, ExRam.Gremlinq.Core.IArrayGremlinQueryBaseRec<TArray, TArrayItem, ExRam.Gremlinq.Core.IArrayGremlinQuery<TArray, TArrayItem, TOriginalQuery>>, ExRam.Gremlinq.Core.IArrayGremlinQueryBaseRec<TArray, TArrayItem, TOriginalQuery, ExRam.Gremlinq.Core.IArrayGremlinQuery<TArray, TArrayItem, TOriginalQuery>>, ExRam.Gremlinq.Core.IArrayGremlinQueryBase<TArrayItem>, ExRam.Gremlinq.Core.IArrayGremlinQueryBase<TArray, TArrayItem>, ExRam.Gremlinq.Core.IArrayGremlinQueryBase<TArray, TArrayItem, TOriginalQuery>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IArrayGremlinQuery<TArray, TArrayItem, TOriginalQuery>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TArray, ExRam.Gremlinq.Core.IArrayGremlinQuery<TArray, TArrayItem, TOriginalQuery>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TArray>, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TOriginalQuery : ExRam.Gremlinq.Core.IGremlinQueryBase { }
    public interface IChooseBuilderWithCaseOrDefault<out TTargetQuery>
        where out TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        TTargetQuery TargetQuery { get; }
    }
    public interface IChooseBuilderWithCase<out TSourceQuery, in TElement, TTargetQuery> : ExRam.Gremlinq.Core.IChooseBuilderWithCaseOrDefault<TTargetQuery>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
        where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IChooseBuilderWithCase<TSourceQuery, TElement, TTargetQuery> Case(TElement element, System.Func<TSourceQuery, TTargetQuery> continuation);
        ExRam.Gremlinq.Core.IChooseBuilderWithCaseOrDefault<TTargetQuery> Default(System.Func<TSourceQuery, TTargetQuery> continuation);
    }
    public interface IChooseBuilderWithCondition<out TSourceQuery, in TElement>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IChooseBuilderWithCase<TSourceQuery, TElement, TTargetQuery> Case<TTargetQuery>(TElement element, System.Func<TSourceQuery, TTargetQuery> continuation)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        ExRam.Gremlinq.Core.IChooseBuilderWithCaseOrDefault<TTargetQuery> Default<TTargetQuery>(System.Func<TSourceQuery, TTargetQuery> continuation)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
    }
    public interface IChooseBuilder<out TSourceQuery>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IChooseBuilderWithCondition<TSourceQuery, TElement> On<TElement>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>> chooseTraversal);
    }
    public interface IEdgeGremlinQueryBase : ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IVertexGremlinQuery<object> BothV();
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex> BothV<TVertex>();
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TResult> Cast<TResult>();
        ExRam.Gremlinq.Core.IVertexGremlinQuery<object> InV();
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex> InV<TVertex>();
        ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQuery<object> Lower();
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TTarget> OfType<TTarget>();
        ExRam.Gremlinq.Core.IVertexGremlinQuery<object> OtherV();
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex> OtherV<TVertex>();
        ExRam.Gremlinq.Core.IVertexGremlinQuery<object> OutV();
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex> OutV<TVertex>();
        ExRam.Gremlinq.Core.IPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.Property<object>> Properties();
        ExRam.Gremlinq.Core.IPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.Property<TValue>> Properties<TValue>();
    }
    public interface IEdgeGremlinQueryBaseRec<TSelf> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TSelf> { }
    public interface IEdgeGremlinQueryBaseRec<TEdge, TSelf> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TEdge, TSelf> { }
    public interface IEdgeGremlinQueryBase<TEdge> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IOutEdgeGremlinQuery<TEdge, TOutVertex> From<TOutVertex>(ExRam.Gremlinq.Core.StepLabel<TOutVertex> stepLabel);
        ExRam.Gremlinq.Core.IOutEdgeGremlinQuery<TEdge, TOutVertex> From<TOutVertex>(System.Func<ExRam.Gremlinq.Core.IVertexGremlinQueryBase, ExRam.Gremlinq.Core.IVertexGremlinQueryBase<TOutVertex>> fromVertexTraversal);
        ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQuery<TEdge> Lower();
        ExRam.Gremlinq.Core.IPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.Property<object>> Properties(params System.Linq.Expressions.Expression<System.Func<TEdge, ExRam.Gremlinq.Core.GraphElements.Property<object>>>[] projections);
        ExRam.Gremlinq.Core.IPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.Property<TValue>> Properties<TValue>(params System.Linq.Expressions.Expression<System.Func<TEdge, ExRam.Gremlinq.Core.GraphElements.Property<TValue>>>[] projections);
        ExRam.Gremlinq.Core.IPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.Property<TValue>> Properties<TValue>(params System.Linq.Expressions.Expression<System.Func<TEdge, TValue>>[] projections);
        ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TEdge, TInVertex> To<TInVertex>(ExRam.Gremlinq.Core.StepLabel<TInVertex> stepLabel);
        ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TEdge, TInVertex> To<TInVertex>(System.Func<ExRam.Gremlinq.Core.IVertexGremlinQueryBase, ExRam.Gremlinq.Core.IVertexGremlinQueryBase<TInVertex>> toVertexTraversal);
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge> Update(TEdge element);
        ExRam.Gremlinq.Core.IGremlinQuery<object> Values(params System.Linq.Expressions.Expression<System.Func<TEdge, ExRam.Gremlinq.Core.GraphElements.Property<object>>>[] projections);
        ExRam.Gremlinq.Core.IGremlinQuery<TValue> Values<TValue>(params System.Linq.Expressions.Expression<System.Func<TEdge, ExRam.Gremlinq.Core.GraphElements.Property<TValue>>>[] projections);
        new ExRam.Gremlinq.Core.IGremlinQuery<TValue> Values<TValue>(params System.Linq.Expressions.Expression<System.Func<TEdge, TValue>>[] projections);
    }
    public interface IEdgeGremlinQueryBase<TEdge, TOutVertex, TInVertex> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBase<TEdge, TInVertex>, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBase<TEdge, TOutVertex>, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        new ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge> Lower();
    }
    public interface IEdgeGremlinQuery<TEdge> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge>>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge>>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge>>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge>>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IStartGremlinQuery { }
    public interface IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge, TOutVertex, TInVertex>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBaseRec<ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBase<TEdge, TInVertex>, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBaseRec<ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBaseRec<ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBase<TEdge, TOutVertex>, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TEdge, TInVertex> AsInEdge();
        ExRam.Gremlinq.Core.IOutEdgeGremlinQuery<TEdge, TOutVertex> AsOutEdge();
        new ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge> Lower();
    }
    public interface IEdgeOrVertexGremlinQueryBase : ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQuery<TResult> Cast<TResult>();
        ExRam.Gremlinq.Core.IElementGremlinQuery<object> Lower();
    }
    public interface IEdgeOrVertexGremlinQueryBaseRec<TSelf> : ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf> { }
    public interface IEdgeOrVertexGremlinQueryBaseRec<TElement, TSelf> : ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TElement, TSelf>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TElement, TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TElement, TSelf> { }
    public interface IEdgeOrVertexGremlinQueryBase<TElement> : ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IElementGremlinQuery<TElement> Lower();
    }
    public interface IEdgeOrVertexGremlinQuery<TElement> : ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TElement, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TElement, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TElement, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IStartGremlinQuery { }
    public interface IElementGremlinQueryBase : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IElementGremlinQuery<TResult> Cast<TResult>();
        ExRam.Gremlinq.Core.IGremlinQuery<object> Id();
        ExRam.Gremlinq.Core.IGremlinQuery<string> Label();
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.Collections.Generic.IDictionary<string, object>> ValueMap();
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.Collections.Generic.IDictionary<string, TValue>> ValueMap<TValue>();
        ExRam.Gremlinq.Core.IGremlinQuery<object> Values();
        ExRam.Gremlinq.Core.IGremlinQuery<TValue> Values<TValue>();
    }
    public interface IElementGremlinQueryBaseRec<TSelf> : ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf>
    {
        TSelf Property(string key, System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase> valueTransformation);
        TSelf Property(string key, object? value);
    }
    public interface IElementGremlinQueryBaseRec<TElement, TSelf> : ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TElement, TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TElement, TSelf>
    {
        TSelf Property<TProjectedValue>(System.Linq.Expressions.Expression<System.Func<TElement, TProjectedValue>> projection, ExRam.Gremlinq.Core.StepLabel<TProjectedValue> stepLabel);
        TSelf Property<TProjectedValue>(System.Linq.Expressions.Expression<System.Func<TElement, TProjectedValue>> projection, System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase<TProjectedValue>> valueTraversal);
        TSelf Property<TProjectedValue>(System.Linq.Expressions.Expression<System.Func<TElement, TProjectedValue>> projection, TProjectedValue value);
        TSelf Where<TProjection>(System.Linq.Expressions.Expression<System.Func<TElement, TProjection>> projection, System.Func<ExRam.Gremlinq.Core.IGremlinQuery<TProjection>, ExRam.Gremlinq.Core.IGremlinQueryBase> propertyTraversal);
    }
    public interface IElementGremlinQueryBase<TElement> : ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IElementGremlinQuery<TElement> Update(TElement element);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.Collections.Generic.IDictionary<string, TValue>> ValueMap<TValue>(params System.Linq.Expressions.Expression<System.Func<TElement, TValue>>[] keys);
        ExRam.Gremlinq.Core.IGremlinQuery<TValue> Values<TValue>(params System.Linq.Expressions.Expression<System.Func<TElement, TValue>>[] projections);
        ExRam.Gremlinq.Core.IGremlinQuery<TValue> Values<TValue>(params System.Linq.Expressions.Expression<System.Func<TElement, TValue[]>>[] projections);
    }
    public interface IElementGremlinQuery<TElement> : ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<ExRam.Gremlinq.Core.IElementGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TElement, ExRam.Gremlinq.Core.IElementGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IElementGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TElement, ExRam.Gremlinq.Core.IElementGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IStartGremlinQuery { }
    public interface IEmitLoopBuilder<TQuery>
        where TQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IEmitRepeatLoopBuilder<TQuery> Repeat(System.Func<TQuery, TQuery> loop);
    }
    public interface IEmitRepeatLoopBuilder<out TQuery> : ExRam.Gremlinq.Core.IFinalLoopBuilder<TQuery>
        where out TQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IEmitRepeatUntilLoopBuilder<TQuery> Times(int loopCount);
        ExRam.Gremlinq.Core.IEmitRepeatUntilLoopBuilder<TQuery> Until(System.Func<TQuery, ExRam.Gremlinq.Core.IGremlinQueryBase> condition);
    }
    public interface IEmitRepeatUntilLoopBuilder<out TQuery> : ExRam.Gremlinq.Core.IFinalLoopBuilder<TQuery>
        where out TQuery : ExRam.Gremlinq.Core.IGremlinQueryBase { }
    public interface IFeatureSet
    {
        ExRam.Gremlinq.Core.EdgeFeatures EdgeFeatures { get; }
        ExRam.Gremlinq.Core.EdgePropertyFeatures EdgePropertyFeatures { get; }
        ExRam.Gremlinq.Core.GraphFeatures GraphFeatures { get; }
        ExRam.Gremlinq.Core.VariableFeatures VariableFeatures { get; }
        ExRam.Gremlinq.Core.VertexFeatures VertexFeatures { get; }
        ExRam.Gremlinq.Core.VertexPropertyFeatures VertexPropertyFeatures { get; }
        ExRam.Gremlinq.Core.IFeatureSet ConfigureEdgeFeatures(System.Func<ExRam.Gremlinq.Core.EdgeFeatures, ExRam.Gremlinq.Core.EdgeFeatures> config);
        ExRam.Gremlinq.Core.IFeatureSet ConfigureEdgePropertyFeatures(System.Func<ExRam.Gremlinq.Core.EdgePropertyFeatures, ExRam.Gremlinq.Core.EdgePropertyFeatures> config);
        ExRam.Gremlinq.Core.IFeatureSet ConfigureGraphFeatures(System.Func<ExRam.Gremlinq.Core.GraphFeatures, ExRam.Gremlinq.Core.GraphFeatures> config);
        ExRam.Gremlinq.Core.IFeatureSet ConfigureVariableFeatures(System.Func<ExRam.Gremlinq.Core.VariableFeatures, ExRam.Gremlinq.Core.VariableFeatures> config);
        ExRam.Gremlinq.Core.IFeatureSet ConfigureVertexFeatures(System.Func<ExRam.Gremlinq.Core.VertexFeatures, ExRam.Gremlinq.Core.VertexFeatures> config);
        ExRam.Gremlinq.Core.IFeatureSet ConfigureVertexPropertyFeatures(System.Func<ExRam.Gremlinq.Core.VertexPropertyFeatures, ExRam.Gremlinq.Core.VertexPropertyFeatures> config);
    }
    public interface IFinalLoopBuilder<out TQuery>
        where out TQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        TQuery Build();
    }
    public interface IGremlinQueryAdmin
    {
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment Environment { get; }
        System.Collections.Immutable.IImmutableDictionary<object, object?> Metadata { get; }
        ExRam.Gremlinq.Core.Traversal Steps { get; }
        TTargetQuery AddStep<TTargetQuery>(ExRam.Gremlinq.Core.Steps.Step step, System.Func<ExRam.Gremlinq.Core.Projections.Projection, ExRam.Gremlinq.Core.Projections.Projection>? projectionTransformation = null)
            where TTargetQuery : ExRam.Gremlinq.Core.IStartGremlinQuery;
        TTargetQuery ChangeQueryType<TTargetQuery>()
            where TTargetQuery : ExRam.Gremlinq.Core.IStartGremlinQuery;
        TTargetQuery ConfigureSteps<TTargetQuery>(System.Func<ExRam.Gremlinq.Core.Traversal, ExRam.Gremlinq.Core.Traversal> configurator, System.Func<ExRam.Gremlinq.Core.Projections.Projection, ExRam.Gremlinq.Core.Projections.Projection>? projectionTransformation = null)
            where TTargetQuery : ExRam.Gremlinq.Core.IStartGremlinQuery;
        ExRam.Gremlinq.Core.IGremlinQuerySource GetSource();
    }
    public interface IGremlinQueryBase : ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IArrayGremlinQuery<TElement, TArrayItem, TOriginalQuery> Cap<TElement, TArrayItem, TOriginalQuery>(ExRam.Gremlinq.Core.StepLabel<ExRam.Gremlinq.Core.IArrayGremlinQuery<TElement, TArrayItem, TOriginalQuery>, TElement> label)
            where TOriginalQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        ExRam.Gremlinq.Core.IGremlinQuery<TResult> Cast<TResult>();
        ExRam.Gremlinq.Core.IGremlinQuery<TValue> Constant<TValue>(TValue constant);
        ExRam.Gremlinq.Core.IGremlinQuery<long> Count();
        ExRam.Gremlinq.Core.IGremlinQuery<long> CountLocal();
        string Debug();
        ExRam.Gremlinq.Core.IGremlinQuery<object> Drop();
        ExRam.Gremlinq.Core.IGremlinQuery<string> Explain();
        ExRam.Gremlinq.Core.IGremlinQuery<object> Fail(string? message = null);
        System.Runtime.CompilerServices.TaskAwaiter GetAwaiter();
        ExRam.Gremlinq.Core.IGremlinQuery<object> Lower();
        ExRam.Gremlinq.Core.IGremlinQuery<ExRam.Gremlinq.Core.GraphElements.Path> Path();
        ExRam.Gremlinq.Core.IGremlinQuery<string> Profile();
        ExRam.Gremlinq.Core.IGremlinQuery<TStepElement> Select<TStepElement>(ExRam.Gremlinq.Core.StepLabel<TStepElement> label);
        TQuery Select<TQuery, TElement>(ExRam.Gremlinq.Core.StepLabel<TQuery, TElement> label)
            where TQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<T1, T2>> Select<T1, T2>(ExRam.Gremlinq.Core.StepLabel<T1> label1, ExRam.Gremlinq.Core.StepLabel<T2> label2);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<T1, T2, T3>> Select<T1, T2, T3>(ExRam.Gremlinq.Core.StepLabel<T1> label1, ExRam.Gremlinq.Core.StepLabel<T2> label2, ExRam.Gremlinq.Core.StepLabel<T3> label3);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<T1, T2, T3, T4>> Select<T1, T2, T3, T4>(ExRam.Gremlinq.Core.StepLabel<T1> label1, ExRam.Gremlinq.Core.StepLabel<T2> label2, ExRam.Gremlinq.Core.StepLabel<T3> label3, ExRam.Gremlinq.Core.StepLabel<T4> label4);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5>> Select<T1, T2, T3, T4, T5>(ExRam.Gremlinq.Core.StepLabel<T1> label1, ExRam.Gremlinq.Core.StepLabel<T2> label2, ExRam.Gremlinq.Core.StepLabel<T3> label3, ExRam.Gremlinq.Core.StepLabel<T4> label4, ExRam.Gremlinq.Core.StepLabel<T5> label5);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6>> Select<T1, T2, T3, T4, T5, T6>(ExRam.Gremlinq.Core.StepLabel<T1> label1, ExRam.Gremlinq.Core.StepLabel<T2> label2, ExRam.Gremlinq.Core.StepLabel<T3> label3, ExRam.Gremlinq.Core.StepLabel<T4> label4, ExRam.Gremlinq.Core.StepLabel<T5> label5, ExRam.Gremlinq.Core.StepLabel<T6> label6);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7>> Select<T1, T2, T3, T4, T5, T6, T7>(ExRam.Gremlinq.Core.StepLabel<T1> label1, ExRam.Gremlinq.Core.StepLabel<T2> label2, ExRam.Gremlinq.Core.StepLabel<T3> label3, ExRam.Gremlinq.Core.StepLabel<T4> label4, ExRam.Gremlinq.Core.StepLabel<T5> label5, ExRam.Gremlinq.Core.StepLabel<T6> label6, ExRam.Gremlinq.Core.StepLabel<T7> label7);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8>>> Select<T1, T2, T3, T4, T5, T6, T7, T8>(ExRam.Gremlinq.Core.StepLabel<T1> label1, ExRam.Gremlinq.Core.StepLabel<T2> label2, ExRam.Gremlinq.Core.StepLabel<T3> label3, ExRam.Gremlinq.Core.StepLabel<T4> label4, ExRam.Gremlinq.Core.StepLabel<T5> label5, ExRam.Gremlinq.Core.StepLabel<T6> label6, ExRam.Gremlinq.Core.StepLabel<T7> label7, ExRam.Gremlinq.Core.StepLabel<T8> label8);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9>>> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9>(ExRam.Gremlinq.Core.StepLabel<T1> label1, ExRam.Gremlinq.Core.StepLabel<T2> label2, ExRam.Gremlinq.Core.StepLabel<T3> label3, ExRam.Gremlinq.Core.StepLabel<T4> label4, ExRam.Gremlinq.Core.StepLabel<T5> label5, ExRam.Gremlinq.Core.StepLabel<T6> label6, ExRam.Gremlinq.Core.StepLabel<T7> label7, ExRam.Gremlinq.Core.StepLabel<T8> label8, ExRam.Gremlinq.Core.StepLabel<T9> label9);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10>>> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(ExRam.Gremlinq.Core.StepLabel<T1> label1, ExRam.Gremlinq.Core.StepLabel<T2> label2, ExRam.Gremlinq.Core.StepLabel<T3> label3, ExRam.Gremlinq.Core.StepLabel<T4> label4, ExRam.Gremlinq.Core.StepLabel<T5> label5, ExRam.Gremlinq.Core.StepLabel<T6> label6, ExRam.Gremlinq.Core.StepLabel<T7> label7, ExRam.Gremlinq.Core.StepLabel<T8> label8, ExRam.Gremlinq.Core.StepLabel<T9> label9, ExRam.Gremlinq.Core.StepLabel<T10> label10);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11>>> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(ExRam.Gremlinq.Core.StepLabel<T1> label1, ExRam.Gremlinq.Core.StepLabel<T2> label2, ExRam.Gremlinq.Core.StepLabel<T3> label3, ExRam.Gremlinq.Core.StepLabel<T4> label4, ExRam.Gremlinq.Core.StepLabel<T5> label5, ExRam.Gremlinq.Core.StepLabel<T6> label6, ExRam.Gremlinq.Core.StepLabel<T7> label7, ExRam.Gremlinq.Core.StepLabel<T8> label8, ExRam.Gremlinq.Core.StepLabel<T9> label9, ExRam.Gremlinq.Core.StepLabel<T10> label10, ExRam.Gremlinq.Core.StepLabel<T11> label11);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12>>> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(ExRam.Gremlinq.Core.StepLabel<T1> label1, ExRam.Gremlinq.Core.StepLabel<T2> label2, ExRam.Gremlinq.Core.StepLabel<T3> label3, ExRam.Gremlinq.Core.StepLabel<T4> label4, ExRam.Gremlinq.Core.StepLabel<T5> label5, ExRam.Gremlinq.Core.StepLabel<T6> label6, ExRam.Gremlinq.Core.StepLabel<T7> label7, ExRam.Gremlinq.Core.StepLabel<T8> label8, ExRam.Gremlinq.Core.StepLabel<T9> label9, ExRam.Gremlinq.Core.StepLabel<T10> label10, ExRam.Gremlinq.Core.StepLabel<T11> label11, ExRam.Gremlinq.Core.StepLabel<T12> label12);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13>>> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(ExRam.Gremlinq.Core.StepLabel<T1> label1, ExRam.Gremlinq.Core.StepLabel<T2> label2, ExRam.Gremlinq.Core.StepLabel<T3> label3, ExRam.Gremlinq.Core.StepLabel<T4> label4, ExRam.Gremlinq.Core.StepLabel<T5> label5, ExRam.Gremlinq.Core.StepLabel<T6> label6, ExRam.Gremlinq.Core.StepLabel<T7> label7, ExRam.Gremlinq.Core.StepLabel<T8> label8, ExRam.Gremlinq.Core.StepLabel<T9> label9, ExRam.Gremlinq.Core.StepLabel<T10> label10, ExRam.Gremlinq.Core.StepLabel<T11> label11, ExRam.Gremlinq.Core.StepLabel<T12> label12, ExRam.Gremlinq.Core.StepLabel<T13> label13);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13, T14>>> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(ExRam.Gremlinq.Core.StepLabel<T1> label1, ExRam.Gremlinq.Core.StepLabel<T2> label2, ExRam.Gremlinq.Core.StepLabel<T3> label3, ExRam.Gremlinq.Core.StepLabel<T4> label4, ExRam.Gremlinq.Core.StepLabel<T5> label5, ExRam.Gremlinq.Core.StepLabel<T6> label6, ExRam.Gremlinq.Core.StepLabel<T7> label7, ExRam.Gremlinq.Core.StepLabel<T8> label8, ExRam.Gremlinq.Core.StepLabel<T9> label9, ExRam.Gremlinq.Core.StepLabel<T10> label10, ExRam.Gremlinq.Core.StepLabel<T11> label11, ExRam.Gremlinq.Core.StepLabel<T12> label12, ExRam.Gremlinq.Core.StepLabel<T13> label13, ExRam.Gremlinq.Core.StepLabel<T14> label14);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13, T14, System.ValueTuple<T15>>>> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(ExRam.Gremlinq.Core.StepLabel<T1> label1, ExRam.Gremlinq.Core.StepLabel<T2> label2, ExRam.Gremlinq.Core.StepLabel<T3> label3, ExRam.Gremlinq.Core.StepLabel<T4> label4, ExRam.Gremlinq.Core.StepLabel<T5> label5, ExRam.Gremlinq.Core.StepLabel<T6> label6, ExRam.Gremlinq.Core.StepLabel<T7> label7, ExRam.Gremlinq.Core.StepLabel<T8> label8, ExRam.Gremlinq.Core.StepLabel<T9> label9, ExRam.Gremlinq.Core.StepLabel<T10> label10, ExRam.Gremlinq.Core.StepLabel<T11> label11, ExRam.Gremlinq.Core.StepLabel<T12> label12, ExRam.Gremlinq.Core.StepLabel<T13> label13, ExRam.Gremlinq.Core.StepLabel<T14> label14, ExRam.Gremlinq.Core.StepLabel<T15> label15);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13, T14, System.ValueTuple<T15, T16>>>> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
                    ExRam.Gremlinq.Core.StepLabel<T1> label1,
                    ExRam.Gremlinq.Core.StepLabel<T2> label2,
                    ExRam.Gremlinq.Core.StepLabel<T3> label3,
                    ExRam.Gremlinq.Core.StepLabel<T4> label4,
                    ExRam.Gremlinq.Core.StepLabel<T5> label5,
                    ExRam.Gremlinq.Core.StepLabel<T6> label6,
                    ExRam.Gremlinq.Core.StepLabel<T7> label7,
                    ExRam.Gremlinq.Core.StepLabel<T8> label8,
                    ExRam.Gremlinq.Core.StepLabel<T9> label9,
                    ExRam.Gremlinq.Core.StepLabel<T10> label10,
                    ExRam.Gremlinq.Core.StepLabel<T11> label11,
                    ExRam.Gremlinq.Core.StepLabel<T12> label12,
                    ExRam.Gremlinq.Core.StepLabel<T13> label13,
                    ExRam.Gremlinq.Core.StepLabel<T14> label14,
                    ExRam.Gremlinq.Core.StepLabel<T15> label15,
                    ExRam.Gremlinq.Core.StepLabel<T16> label16);
    }
    public interface IGremlinQueryBaseRec<TSelf> : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>
    {
        TSelf And(params System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase>[] andTraversals);
        TTargetQuery As<TTargetQuery>(System.Func<TSelf, ExRam.Gremlinq.Core.StepLabel<TSelf, object>, TTargetQuery> continuation)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        TSelf Barrier();
        ExRam.Gremlinq.Core.IGremlinQuery<object> Choose(System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase> traversalPredicate, System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase> trueChoice);
        TSelf Choose(System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase> traversalPredicate, System.Func<TSelf, TSelf> trueChoice);
        TTargetQuery Choose<TTargetQuery>(System.Func<ExRam.Gremlinq.Core.IChooseBuilder<TSelf>, ExRam.Gremlinq.Core.IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        TTargetQuery Choose<TTargetQuery>(System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase> traversalPredicate, System.Func<TSelf, TTargetQuery> trueChoice, System.Func<TSelf, TTargetQuery> falseChoice)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        ExRam.Gremlinq.Core.IGremlinQuery<object> Coalesce(params System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase>[] traversals);
        TTargetQuery Coalesce<TTargetQuery>(params System.Func<TSelf, TTargetQuery>[] traversals)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        TSelf Coin(double probability);
        TSelf CyclicPath();
        TSelf Dedup();
        TSelf DedupLocal();
        TTargetQuery FlatMap<TTargetQuery>(System.Func<TSelf, TTargetQuery> mapping)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        TSelf Identity();
        TSelf Limit(long count);
        TTargetQuery Local<TTargetQuery>(System.Func<TSelf, TTargetQuery> localTraversal)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        TSelf Loop(System.Func<ExRam.Gremlinq.Core.IStartLoopBuilder<TSelf>, ExRam.Gremlinq.Core.IFinalLoopBuilder<TSelf>> loopBuilderTransformation);
        TTargetQuery Map<TTargetQuery>(System.Func<TSelf, TTargetQuery> mapping)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        TSelf Max();
        TSelf Mean();
        TSelf Min();
        TSelf None();
        TSelf Not(System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase> notTraversal);
        TSelf Optional(System.Func<TSelf, TSelf> optionalTraversal);
        TSelf Or(params System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase>[] orTraversals);
        TSelf Order(System.Func<ExRam.Gremlinq.Core.IOrderBuilder<TSelf>, ExRam.Gremlinq.Core.IOrderBuilderWithBy<TSelf>> projection);
        TSelf OrderLocal(System.Func<ExRam.Gremlinq.Core.IOrderBuilder<TSelf>, ExRam.Gremlinq.Core.IOrderBuilderWithBy<TSelf>> projection);
        TSelf Range(long low, long high);
        TSelf SideEffect(System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase> sideEffectTraversal);
        TSelf SimplePath();
        TSelf Skip(long count);
        TSelf Sum();
        TSelf Tail(long count);
        ExRam.Gremlinq.Core.IGremlinQuery<object> Union(params System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase>[] traversals);
        TTargetQuery Union<TTargetQuery>(params System.Func<TSelf, TTargetQuery>[] unionTraversals)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        TSelf Where(System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase> filterTraversal);
    }
    public interface IGremlinQueryBaseRec<TElement, TSelf> : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TElement, TSelf>
    {
        TSelf Aggregate(ExRam.Gremlinq.Core.StepLabel<TElement[]> stepLabel);
        TTargetQuery Aggregate<TTargetQuery>(System.Func<TSelf, ExRam.Gremlinq.Core.StepLabel<ExRam.Gremlinq.Core.IArrayGremlinQuery<TElement[], TElement, TSelf>, TElement[]>, TTargetQuery> continuation)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        TSelf AggregateLocal(ExRam.Gremlinq.Core.StepLabel<TElement[]> stepLabel);
        TTargetQuery AggregateLocal<TTargetQuery>(System.Func<TSelf, ExRam.Gremlinq.Core.StepLabel<ExRam.Gremlinq.Core.IArrayGremlinQuery<TElement[], TElement, TSelf>, TElement[]>, TTargetQuery> continuation)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        TSelf As(ExRam.Gremlinq.Core.StepLabel<TElement> stepLabel);
        TTargetQuery As<TTargetQuery>(System.Func<TSelf, ExRam.Gremlinq.Core.StepLabel<TSelf, TElement>, TTargetQuery> continuation)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        ExRam.Gremlinq.Core.IGremlinQuery<object> Choose(System.Linq.Expressions.Expression<System.Func<TElement, bool>> predicate, System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase> trueChoice);
        TSelf Choose(System.Linq.Expressions.Expression<System.Func<TElement, bool>> predicate, System.Func<TSelf, TSelf> trueChoice);
        TTargetQuery Choose<TTargetQuery>(System.Linq.Expressions.Expression<System.Func<TElement, bool>> predicate, System.Func<TSelf, TTargetQuery> trueChoice, System.Func<TSelf, TTargetQuery> falseChoice)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        ExRam.Gremlinq.Core.IArrayGremlinQuery<TElement[], TElement, TSelf> Fold();
        ExRam.Gremlinq.Core.IArrayGremlinQuery<TElement[], TElement, TSelf> ForceArray();
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.Collections.Generic.IDictionary<TNewKey, TElement[]>> Group<TNewKey>(System.Func<ExRam.Gremlinq.Core.IGroupBuilder<TSelf>, ExRam.Gremlinq.Core.IGroupBuilderWithKey<TSelf, TNewKey>> groupBuilder);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.Collections.Generic.IDictionary<TNewKey, TNewValue>> Group<TNewKey, TNewValue>(System.Func<ExRam.Gremlinq.Core.IGroupBuilder<TSelf>, ExRam.Gremlinq.Core.IGroupBuilderWithKeyAndValue<TNewKey, TNewValue>> groupBuilder);
        TSelf Inject(params TElement[] elements);
        TSelf Order(System.Func<ExRam.Gremlinq.Core.IOrderBuilder<TElement, TSelf>, ExRam.Gremlinq.Core.IOrderBuilderWithBy<TElement, TSelf>> projection);
        TSelf OrderLocal(System.Func<ExRam.Gremlinq.Core.IOrderBuilder<TElement, TSelf>, ExRam.Gremlinq.Core.IOrderBuilderWithBy<TElement, TSelf>> projection);
        [return: System.Runtime.CompilerServices.Dynamic(new bool[] {
                false,
                true})]
        ExRam.Gremlinq.Core.IGremlinQuery<object> Project(System.Func<ExRam.Gremlinq.Core.IProjectBuilder<TSelf, TElement>, ExRam.Gremlinq.Core.IProjectDynamicResult> continuation);
        ExRam.Gremlinq.Core.IMapGremlinQuery<TResult> Project<TResult>(System.Func<ExRam.Gremlinq.Core.IProjectBuilder<TSelf, TElement>, ExRam.Gremlinq.Core.IProjectMapResult<TResult>> continuation);
        ExRam.Gremlinq.Core.IMapGremlinQuery<TResult> Project<TResult>(System.Func<ExRam.Gremlinq.Core.IProjectBuilder<TSelf, TElement>, ExRam.Gremlinq.Core.IProjectTupleResult<TResult>> continuation)
            where TResult : System.Runtime.CompilerServices.ITuple;
        TSelf Where(System.Linq.Expressions.Expression<System.Func<TElement, bool>> predicate);
    }
    public interface IGremlinQueryBase<TElement> : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IArrayGremlinQuery<TElement[], TElement, ExRam.Gremlinq.Core.IGremlinQuery<TElement>> ForceArray();
        ExRam.Gremlinq.Core.IGremlinQuery<TElement> ForceBase();
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TElement> ForceEdge();
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> ForceEdge<TOutVertex, TInVertex>();
        ExRam.Gremlinq.Core.IElementGremlinQuery<TElement> ForceElement();
        ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TElement, TInVertex> ForceInEdge<TInVertex>();
        ExRam.Gremlinq.Core.IOutEdgeGremlinQuery<TElement, TOutVertex> ForceOutEdge<TOutVertex>();
        ExRam.Gremlinq.Core.IPropertyGremlinQuery<TElement> ForceProperty();
        ExRam.Gremlinq.Core.IGremlinQuery<TElement> ForceValue();
        ExRam.Gremlinq.Core.IMapGremlinQuery<TElement> ForceValueTuple();
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TElement> ForceVertex();
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TElement, TValue> ForceVertexProperty<TValue>();
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TElement, TValue, TMeta> ForceVertexProperty<TValue, TMeta>();
        ExRam.Gremlinq.Core.GremlinQueryAwaiter<TElement> GetAwaiter();
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.Collections.Generic.IDictionary<TElement, TElement[]>> Group();
        ExRam.Gremlinq.Core.IGremlinQuery<TElement> Lower();
        System.Collections.Generic.IAsyncEnumerable<TElement> ToAsyncEnumerable();
    }
    public interface IGremlinQueryDebugger
    {
        string Debug(Gremlin.Net.Process.Traversal.Bytecode bytecode, ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment);
    }
    public interface IGremlinQueryEnvironment
    {
        ExRam.Gremlinq.Core.IGremlinQueryDebugger Debugger { get; }
        ExRam.Gremlinq.Core.Transformation.ITransformer Deserializer { get; }
        ExRam.Gremlinq.Core.Execution.IGremlinQueryExecutor Executor { get; }
        ExRam.Gremlinq.Core.IFeatureSet FeatureSet { get; }
        Microsoft.Extensions.Logging.ILogger Logger { get; }
        ExRam.Gremlinq.Core.Models.IGraphModel Model { get; }
        System.Collections.Immutable.IImmutableSet<System.Type> NativeTypes { get; }
        ExRam.Gremlinq.Core.IGremlinqOptions Options { get; }
        ExRam.Gremlinq.Core.Transformation.ITransformer Serializer { get; }
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment ConfigureDebugger(System.Func<ExRam.Gremlinq.Core.IGremlinQueryDebugger, ExRam.Gremlinq.Core.IGremlinQueryDebugger> debuggerTransformation);
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment ConfigureDeserializer(System.Func<ExRam.Gremlinq.Core.Transformation.ITransformer, ExRam.Gremlinq.Core.Transformation.ITransformer> deserializerTransformation);
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment ConfigureExecutor(System.Func<ExRam.Gremlinq.Core.Execution.IGremlinQueryExecutor, ExRam.Gremlinq.Core.Execution.IGremlinQueryExecutor> executorTransformation);
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment ConfigureFeatureSet(System.Func<ExRam.Gremlinq.Core.IFeatureSet, ExRam.Gremlinq.Core.IFeatureSet> featureSetTransformation);
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment ConfigureLogger(System.Func<Microsoft.Extensions.Logging.ILogger, Microsoft.Extensions.Logging.ILogger> loggerTransformation);
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment ConfigureModel(System.Func<ExRam.Gremlinq.Core.Models.IGraphModel, ExRam.Gremlinq.Core.Models.IGraphModel> modelTransformation);
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment ConfigureNativeTypes(System.Func<System.Collections.Immutable.IImmutableSet<System.Type>, System.Collections.Immutable.IImmutableSet<System.Type>> transformation);
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment ConfigureOptions(System.Func<ExRam.Gremlinq.Core.IGremlinqOptions, ExRam.Gremlinq.Core.IGremlinqOptions> optionsTransformation);
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment ConfigureSerializer(System.Func<ExRam.Gremlinq.Core.Transformation.ITransformer, ExRam.Gremlinq.Core.Transformation.ITransformer> serializerTransformation);
    }
    public interface IGremlinQuerySource : ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IGremlinQuerySource ConfigureEnvironment(System.Func<ExRam.Gremlinq.Core.IGremlinQueryEnvironment, ExRam.Gremlinq.Core.IGremlinQueryEnvironment> environmentTransformation);
        ExRam.Gremlinq.Core.IGremlinQuerySource ConfigureMetadata(System.Func<System.Collections.Immutable.IImmutableDictionary<object, object?>, System.Collections.Immutable.IImmutableDictionary<object, object?>> metadataTransformation);
        ExRam.Gremlinq.Core.IGremlinQuerySource WithSideEffect<TSideEffect>(ExRam.Gremlinq.Core.StepLabel<TSideEffect> label, TSideEffect value);
        TQuery WithSideEffect<TSideEffect, TQuery>(TSideEffect value, System.Func<ExRam.Gremlinq.Core.IGremlinQuerySource, ExRam.Gremlinq.Core.StepLabel<TSideEffect>, TQuery> continuation)
            where TQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
    }
    public interface IGremlinQuerySourceTransformation
    {
        ExRam.Gremlinq.Core.IGremlinQuerySource Transform(ExRam.Gremlinq.Core.IGremlinQuerySource source);
    }
    public interface IGremlinQuery<TElement> : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TElement, ExRam.Gremlinq.Core.IGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IStartGremlinQuery { }
    public interface IGremlinqConfiguratorTransformation<TConfigurator>
        where TConfigurator : ExRam.Gremlinq.Core.IGremlinqConfigurator<TConfigurator>
    {
        TConfigurator Transform(TConfigurator configurator);
    }
    public interface IGremlinqConfigurator<out TSelf> : ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation
        where out TSelf : ExRam.Gremlinq.Core.IGremlinqConfigurator<TSelf>
    {
        TSelf ConfigureQuerySource(System.Func<ExRam.Gremlinq.Core.IGremlinQuerySource, ExRam.Gremlinq.Core.IGremlinQuerySource> transformation);
    }
    public interface IGremlinqOptions
    {
        ExRam.Gremlinq.Core.IGremlinqOptions ConfigureValue<TValue>(ExRam.Gremlinq.Core.GremlinqOption<TValue> option, System.Func<TValue, TValue> configuration);
        bool Contains<TValue>(ExRam.Gremlinq.Core.GremlinqOption<TValue> option);
        TValue GetValue<TValue>(ExRam.Gremlinq.Core.GremlinqOption<TValue> option);
        ExRam.Gremlinq.Core.IGremlinqOptions Remove<TValue>(ExRam.Gremlinq.Core.GremlinqOption<TValue> option);
        ExRam.Gremlinq.Core.IGremlinqOptions SetValue<TValue>(ExRam.Gremlinq.Core.GremlinqOption<TValue> option, TValue value);
    }
    public interface IGroupBuilderWithKeyAndValue<TKey, TValue>
    {
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.Collections.Generic.IDictionary<TKey, TValue>> Build();
    }
    public interface IGroupBuilderWithKey<out TSourceQuery, TKey>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IGroupBuilderWithKeyAndValue<TKey, TValue> ByValue<TValue>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TValue>> valueSelector);
    }
    public interface IGroupBuilder<out TSourceQuery>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IGroupBuilderWithKey<TSourceQuery, TKey> ByKey<TKey>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TKey>> keySelector);
    }
    public interface IInEdgeGremlinQueryBase : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<object> Lower();
    }
    public interface IInEdgeGremlinQueryBaseRec<TSelf> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IInEdgeGremlinQueryBaseRec<TSelf> { }
    public interface IInEdgeGremlinQueryBaseRec<TEdge, TInVertex, TSelf> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBase<TEdge, TInVertex>, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IInEdgeGremlinQueryBaseRec<TEdge, TInVertex, TSelf> { }
    public interface IInEdgeGremlinQueryBase<TEdge, TInVertex> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> From<TOutVertex>(System.Func<ExRam.Gremlinq.Core.IVertexGremlinQuery<TInVertex>, ExRam.Gremlinq.Core.IVertexGremlinQueryBase<TOutVertex>> fromVertexTraversal);
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TInVertex> InV();
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge> Lower();
    }
    public interface IInEdgeGremlinQuery<TEdge, TInVertex> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TEdge, TInVertex>>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TEdge, TInVertex>>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TEdge, TInVertex>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TEdge, TInVertex>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TEdge, TInVertex>>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TEdge, TInVertex>>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TEdge, TInVertex>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TEdge, TInVertex>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBaseRec<ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TEdge, TInVertex>>, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBaseRec<TEdge, TInVertex, ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TEdge, TInVertex>>, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBase<TEdge, TInVertex>, ExRam.Gremlinq.Core.IStartGremlinQuery { }
    public interface IInOrOutEdgeGremlinQueryBase : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<object> Lower();
    }
    public interface IInOrOutEdgeGremlinQueryBaseRec<TSelf> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBaseRec<TSelf> { }
    public interface IInOrOutEdgeGremlinQueryBaseRec<TEdge, TAdjacentVertex, TSelf> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBase<TEdge, TAdjacentVertex>, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBaseRec<TEdge, TAdjacentVertex, TSelf> { }
    public interface IInOrOutEdgeGremlinQueryBase<TEdge, TAdjacentVertex> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge, TTargetVertex, TAdjacentVertex> From<TTargetVertex>(ExRam.Gremlinq.Core.StepLabel<TTargetVertex> stepLabel);
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge, TTargetVertex, TAdjacentVertex> From<TTargetVertex>(System.Func<ExRam.Gremlinq.Core.IVertexGremlinQuery<TAdjacentVertex>, ExRam.Gremlinq.Core.IVertexGremlinQueryBase<TTargetVertex>> fromVertexTraversal);
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge> Lower();
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge, TAdjacentVertex, TTargetVertex> To<TTargetVertex>(ExRam.Gremlinq.Core.StepLabel<TTargetVertex> stepLabel);
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge, TAdjacentVertex, TTargetVertex> To<TTargetVertex>(System.Func<ExRam.Gremlinq.Core.IVertexGremlinQuery<TAdjacentVertex>, ExRam.Gremlinq.Core.IVertexGremlinQueryBase<TTargetVertex>> toVertexTraversal);
    }
    public interface IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBaseRec<ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBaseRec<TEdge, TAdjacentVertex, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBase<TEdge, TAdjacentVertex>, ExRam.Gremlinq.Core.IStartGremlinQuery { }
    public interface IMapGremlinQueryBase : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery { }
    public interface IMapGremlinQueryBase<TElement> : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IMapGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IGremlinQuery<TValue> Select<TValue>(System.Linq.Expressions.Expression<System.Func<TElement, TValue>> projection);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<TItem1, TItem2>> Select<TItem1, TItem2>(System.Linq.Expressions.Expression<System.Func<TElement, TItem1>> projection1, System.Linq.Expressions.Expression<System.Func<TElement, TItem2>> projection2);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<TItem1, TItem2, TItem3>> Select<TItem1, TItem2, TItem3>(System.Linq.Expressions.Expression<System.Func<TElement, TItem1>> projection1, System.Linq.Expressions.Expression<System.Func<TElement, TItem2>> projection2, System.Linq.Expressions.Expression<System.Func<TElement, TItem3>> projection3);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<TItem1, TItem2, TItem3, TItem4>> Select<TItem1, TItem2, TItem3, TItem4>(System.Linq.Expressions.Expression<System.Func<TElement, TItem1>> projection1, System.Linq.Expressions.Expression<System.Func<TElement, TItem2>> projection2, System.Linq.Expressions.Expression<System.Func<TElement, TItem3>> projection3, System.Linq.Expressions.Expression<System.Func<TElement, TItem4>> projection4);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5>> Select<TItem1, TItem2, TItem3, TItem4, TItem5>(System.Linq.Expressions.Expression<System.Func<TElement, TItem1>> projection1, System.Linq.Expressions.Expression<System.Func<TElement, TItem2>> projection2, System.Linq.Expressions.Expression<System.Func<TElement, TItem3>> projection3, System.Linq.Expressions.Expression<System.Func<TElement, TItem4>> projection4, System.Linq.Expressions.Expression<System.Func<TElement, TItem5>> projection5);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6>> Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6>(System.Linq.Expressions.Expression<System.Func<TElement, TItem1>> projection1, System.Linq.Expressions.Expression<System.Func<TElement, TItem2>> projection2, System.Linq.Expressions.Expression<System.Func<TElement, TItem3>> projection3, System.Linq.Expressions.Expression<System.Func<TElement, TItem4>> projection4, System.Linq.Expressions.Expression<System.Func<TElement, TItem5>> projection5, System.Linq.Expressions.Expression<System.Func<TElement, TItem6>> projection6);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7>> Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7>(System.Linq.Expressions.Expression<System.Func<TElement, TItem1>> projection1, System.Linq.Expressions.Expression<System.Func<TElement, TItem2>> projection2, System.Linq.Expressions.Expression<System.Func<TElement, TItem3>> projection3, System.Linq.Expressions.Expression<System.Func<TElement, TItem4>> projection4, System.Linq.Expressions.Expression<System.Func<TElement, TItem5>> projection5, System.Linq.Expressions.Expression<System.Func<TElement, TItem6>> projection6, System.Linq.Expressions.Expression<System.Func<TElement, TItem7>> projection7);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8>>> Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8>(System.Linq.Expressions.Expression<System.Func<TElement, TItem1>> projection1, System.Linq.Expressions.Expression<System.Func<TElement, TItem2>> projection2, System.Linq.Expressions.Expression<System.Func<TElement, TItem3>> projection3, System.Linq.Expressions.Expression<System.Func<TElement, TItem4>> projection4, System.Linq.Expressions.Expression<System.Func<TElement, TItem5>> projection5, System.Linq.Expressions.Expression<System.Func<TElement, TItem6>> projection6, System.Linq.Expressions.Expression<System.Func<TElement, TItem7>> projection7, System.Linq.Expressions.Expression<System.Func<TElement, TItem8>> projection8);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9>>> Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9>(System.Linq.Expressions.Expression<System.Func<TElement, TItem1>> projection1, System.Linq.Expressions.Expression<System.Func<TElement, TItem2>> projection2, System.Linq.Expressions.Expression<System.Func<TElement, TItem3>> projection3, System.Linq.Expressions.Expression<System.Func<TElement, TItem4>> projection4, System.Linq.Expressions.Expression<System.Func<TElement, TItem5>> projection5, System.Linq.Expressions.Expression<System.Func<TElement, TItem6>> projection6, System.Linq.Expressions.Expression<System.Func<TElement, TItem7>> projection7, System.Linq.Expressions.Expression<System.Func<TElement, TItem8>> projection8, System.Linq.Expressions.Expression<System.Func<TElement, TItem9>> projection9);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9, TItem10>>> Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10>(System.Linq.Expressions.Expression<System.Func<TElement, TItem1>> projection1, System.Linq.Expressions.Expression<System.Func<TElement, TItem2>> projection2, System.Linq.Expressions.Expression<System.Func<TElement, TItem3>> projection3, System.Linq.Expressions.Expression<System.Func<TElement, TItem4>> projection4, System.Linq.Expressions.Expression<System.Func<TElement, TItem5>> projection5, System.Linq.Expressions.Expression<System.Func<TElement, TItem6>> projection6, System.Linq.Expressions.Expression<System.Func<TElement, TItem7>> projection7, System.Linq.Expressions.Expression<System.Func<TElement, TItem8>> projection8, System.Linq.Expressions.Expression<System.Func<TElement, TItem9>> projection9, System.Linq.Expressions.Expression<System.Func<TElement, TItem10>> projection10);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9, TItem10, TItem11>>> Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11>(System.Linq.Expressions.Expression<System.Func<TElement, TItem1>> projection1, System.Linq.Expressions.Expression<System.Func<TElement, TItem2>> projection2, System.Linq.Expressions.Expression<System.Func<TElement, TItem3>> projection3, System.Linq.Expressions.Expression<System.Func<TElement, TItem4>> projection4, System.Linq.Expressions.Expression<System.Func<TElement, TItem5>> projection5, System.Linq.Expressions.Expression<System.Func<TElement, TItem6>> projection6, System.Linq.Expressions.Expression<System.Func<TElement, TItem7>> projection7, System.Linq.Expressions.Expression<System.Func<TElement, TItem8>> projection8, System.Linq.Expressions.Expression<System.Func<TElement, TItem9>> projection9, System.Linq.Expressions.Expression<System.Func<TElement, TItem10>> projection10, System.Linq.Expressions.Expression<System.Func<TElement, TItem11>> projection11);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9, TItem10, TItem11, TItem12>>> Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12>(System.Linq.Expressions.Expression<System.Func<TElement, TItem1>> projection1, System.Linq.Expressions.Expression<System.Func<TElement, TItem2>> projection2, System.Linq.Expressions.Expression<System.Func<TElement, TItem3>> projection3, System.Linq.Expressions.Expression<System.Func<TElement, TItem4>> projection4, System.Linq.Expressions.Expression<System.Func<TElement, TItem5>> projection5, System.Linq.Expressions.Expression<System.Func<TElement, TItem6>> projection6, System.Linq.Expressions.Expression<System.Func<TElement, TItem7>> projection7, System.Linq.Expressions.Expression<System.Func<TElement, TItem8>> projection8, System.Linq.Expressions.Expression<System.Func<TElement, TItem9>> projection9, System.Linq.Expressions.Expression<System.Func<TElement, TItem10>> projection10, System.Linq.Expressions.Expression<System.Func<TElement, TItem11>> projection11, System.Linq.Expressions.Expression<System.Func<TElement, TItem12>> projection12);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9, TItem10, TItem11, TItem12, TItem13>>> Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13>(System.Linq.Expressions.Expression<System.Func<TElement, TItem1>> projection1, System.Linq.Expressions.Expression<System.Func<TElement, TItem2>> projection2, System.Linq.Expressions.Expression<System.Func<TElement, TItem3>> projection3, System.Linq.Expressions.Expression<System.Func<TElement, TItem4>> projection4, System.Linq.Expressions.Expression<System.Func<TElement, TItem5>> projection5, System.Linq.Expressions.Expression<System.Func<TElement, TItem6>> projection6, System.Linq.Expressions.Expression<System.Func<TElement, TItem7>> projection7, System.Linq.Expressions.Expression<System.Func<TElement, TItem8>> projection8, System.Linq.Expressions.Expression<System.Func<TElement, TItem9>> projection9, System.Linq.Expressions.Expression<System.Func<TElement, TItem10>> projection10, System.Linq.Expressions.Expression<System.Func<TElement, TItem11>> projection11, System.Linq.Expressions.Expression<System.Func<TElement, TItem12>> projection12, System.Linq.Expressions.Expression<System.Func<TElement, TItem13>> projection13);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14>>> Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14>(System.Linq.Expressions.Expression<System.Func<TElement, TItem1>> projection1, System.Linq.Expressions.Expression<System.Func<TElement, TItem2>> projection2, System.Linq.Expressions.Expression<System.Func<TElement, TItem3>> projection3, System.Linq.Expressions.Expression<System.Func<TElement, TItem4>> projection4, System.Linq.Expressions.Expression<System.Func<TElement, TItem5>> projection5, System.Linq.Expressions.Expression<System.Func<TElement, TItem6>> projection6, System.Linq.Expressions.Expression<System.Func<TElement, TItem7>> projection7, System.Linq.Expressions.Expression<System.Func<TElement, TItem8>> projection8, System.Linq.Expressions.Expression<System.Func<TElement, TItem9>> projection9, System.Linq.Expressions.Expression<System.Func<TElement, TItem10>> projection10, System.Linq.Expressions.Expression<System.Func<TElement, TItem11>> projection11, System.Linq.Expressions.Expression<System.Func<TElement, TItem12>> projection12, System.Linq.Expressions.Expression<System.Func<TElement, TItem13>> projection13, System.Linq.Expressions.Expression<System.Func<TElement, TItem14>> projection14);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, System.ValueTuple<TItem15>>>> Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15>(System.Linq.Expressions.Expression<System.Func<TElement, TItem1>> projection1, System.Linq.Expressions.Expression<System.Func<TElement, TItem2>> projection2, System.Linq.Expressions.Expression<System.Func<TElement, TItem3>> projection3, System.Linq.Expressions.Expression<System.Func<TElement, TItem4>> projection4, System.Linq.Expressions.Expression<System.Func<TElement, TItem5>> projection5, System.Linq.Expressions.Expression<System.Func<TElement, TItem6>> projection6, System.Linq.Expressions.Expression<System.Func<TElement, TItem7>> projection7, System.Linq.Expressions.Expression<System.Func<TElement, TItem8>> projection8, System.Linq.Expressions.Expression<System.Func<TElement, TItem9>> projection9, System.Linq.Expressions.Expression<System.Func<TElement, TItem10>> projection10, System.Linq.Expressions.Expression<System.Func<TElement, TItem11>> projection11, System.Linq.Expressions.Expression<System.Func<TElement, TItem12>> projection12, System.Linq.Expressions.Expression<System.Func<TElement, TItem13>> projection13, System.Linq.Expressions.Expression<System.Func<TElement, TItem14>> projection14, System.Linq.Expressions.Expression<System.Func<TElement, TItem15>> projection15);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, System.ValueTuple<TItem15, TItem16>>>> Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16>(
                    System.Linq.Expressions.Expression<System.Func<TElement, TItem1>> projection1,
                    System.Linq.Expressions.Expression<System.Func<TElement, TItem2>> projection2,
                    System.Linq.Expressions.Expression<System.Func<TElement, TItem3>> projection3,
                    System.Linq.Expressions.Expression<System.Func<TElement, TItem4>> projection4,
                    System.Linq.Expressions.Expression<System.Func<TElement, TItem5>> projection5,
                    System.Linq.Expressions.Expression<System.Func<TElement, TItem6>> projection6,
                    System.Linq.Expressions.Expression<System.Func<TElement, TItem7>> projection7,
                    System.Linq.Expressions.Expression<System.Func<TElement, TItem8>> projection8,
                    System.Linq.Expressions.Expression<System.Func<TElement, TItem9>> projection9,
                    System.Linq.Expressions.Expression<System.Func<TElement, TItem10>> projection10,
                    System.Linq.Expressions.Expression<System.Func<TElement, TItem11>> projection11,
                    System.Linq.Expressions.Expression<System.Func<TElement, TItem12>> projection12,
                    System.Linq.Expressions.Expression<System.Func<TElement, TItem13>> projection13,
                    System.Linq.Expressions.Expression<System.Func<TElement, TItem14>> projection14,
                    System.Linq.Expressions.Expression<System.Func<TElement, TItem15>> projection15,
                    System.Linq.Expressions.Expression<System.Func<TElement, TItem16>> projection16);
    }
    public interface IMapGremlinQuery<TElement> : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IMapGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TElement, ExRam.Gremlinq.Core.IMapGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IMapGremlinQueryBase, ExRam.Gremlinq.Core.IMapGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IStartGremlinQuery { }
    public interface IOrderBuilderWithBy<out TSourceQuery> : ExRam.Gremlinq.Core.IOrderBuilder<TSourceQuery>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        TSourceQuery Build();
    }
    public interface IOrderBuilderWithBy<TElement, out TSourceQuery> : ExRam.Gremlinq.Core.IOrderBuilderWithBy<TSourceQuery>, ExRam.Gremlinq.Core.IOrderBuilder<TSourceQuery>, ExRam.Gremlinq.Core.IOrderBuilder<TElement, TSourceQuery>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase<TElement> { }
    public interface IOrderBuilder<out TSourceQuery>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IOrderBuilderWithBy<TSourceQuery> By(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase> traversal);
        ExRam.Gremlinq.Core.IOrderBuilderWithBy<TSourceQuery> ByDescending(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase> traversal);
    }
    public interface IOrderBuilder<TElement, out TSourceQuery> : ExRam.Gremlinq.Core.IOrderBuilder<TSourceQuery>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>
    {
        ExRam.Gremlinq.Core.IOrderBuilderWithBy<TElement, TSourceQuery> By(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase> traversal);
        ExRam.Gremlinq.Core.IOrderBuilderWithBy<TElement, TSourceQuery> By(System.Linq.Expressions.Expression<System.Func<TElement, object?>> projection);
        ExRam.Gremlinq.Core.IOrderBuilderWithBy<TElement, TSourceQuery> ByDescending(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase> traversal);
        ExRam.Gremlinq.Core.IOrderBuilderWithBy<TElement, TSourceQuery> ByDescending(System.Linq.Expressions.Expression<System.Func<TElement, object?>> projection);
    }
    public interface IOutEdgeGremlinQueryBase : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<object> Lower();
    }
    public interface IOutEdgeGremlinQueryBaseRec<TSelf> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBaseRec<TSelf> { }
    public interface IOutEdgeGremlinQueryBaseRec<TEdge, TOutVertex, TSelf> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBase<TEdge, TOutVertex>, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBaseRec<TEdge, TOutVertex, TSelf> { }
    public interface IOutEdgeGremlinQueryBase<TEdge, TOutVertex> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge> Lower();
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TOutVertex> OutV();
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> To<TInVertex>(System.Func<ExRam.Gremlinq.Core.IVertexGremlinQuery<TOutVertex>, ExRam.Gremlinq.Core.IVertexGremlinQueryBase<TInVertex>> toVertexTraversal);
    }
    public interface IOutEdgeGremlinQuery<TEdge, TOutVertex> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<ExRam.Gremlinq.Core.IOutEdgeGremlinQuery<TEdge, TOutVertex>>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IOutEdgeGremlinQuery<TEdge, TOutVertex>>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<ExRam.Gremlinq.Core.IOutEdgeGremlinQuery<TEdge, TOutVertex>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IOutEdgeGremlinQuery<TEdge, TOutVertex>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<ExRam.Gremlinq.Core.IOutEdgeGremlinQuery<TEdge, TOutVertex>>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IOutEdgeGremlinQuery<TEdge, TOutVertex>>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IOutEdgeGremlinQuery<TEdge, TOutVertex>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IOutEdgeGremlinQuery<TEdge, TOutVertex>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBaseRec<ExRam.Gremlinq.Core.IOutEdgeGremlinQuery<TEdge, TOutVertex>>, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBaseRec<TEdge, TOutVertex, ExRam.Gremlinq.Core.IOutEdgeGremlinQuery<TEdge, TOutVertex>>, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBase<TEdge, TOutVertex>, ExRam.Gremlinq.Core.IStartGremlinQuery { }
    public interface IProjectBuilder<out TSourceQuery, TElement>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectMapBuilder<TSourceQuery, TElement, TTargetType> To<TTargetType>();
        ExRam.Gremlinq.Core.IProjectDynamicBuilder<TSourceQuery, TElement> ToDynamic();
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement> ToTuple();
        ExRam.Gremlinq.Core.IProjectBuilder<TSourceQuery, TElement> WithEmptyProjectionProtection();
    }
    public interface IProjectDynamicBuilder<out TSourceQuery, TElement> : ExRam.Gremlinq.Core.IProjectDynamicResult
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectDynamicBuilder<TSourceQuery, TElement> By(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase> projection);
        ExRam.Gremlinq.Core.IProjectDynamicBuilder<TSourceQuery, TElement> By(System.Linq.Expressions.Expression<System.Func<TElement, object>> projection);
        ExRam.Gremlinq.Core.IProjectDynamicBuilder<TSourceQuery, TElement> By(string name, System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase> projection);
        ExRam.Gremlinq.Core.IProjectDynamicBuilder<TSourceQuery, TElement> By(string name, System.Linq.Expressions.Expression<System.Func<TElement, object>> projection);
    }
    public interface IProjectDynamicResult
    {
        [return: System.Runtime.CompilerServices.Dynamic(new bool[] {
                false,
                true})]
        ExRam.Gremlinq.Core.IGremlinQuery<object> Build();
    }
    public interface IProjectMapBuilder<out TSourceQuery, TElement, TTargetType> : ExRam.Gremlinq.Core.IProjectMapResult<TTargetType>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectMapBuilder<TSourceQuery, TElement, TTargetType> By<TSourceProperty, TTargetProperty>(System.Linq.Expressions.Expression<System.Func<TTargetType, TTargetProperty>> targetExpression, System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TSourceProperty>> projection)
            where TSourceProperty : TTargetProperty;
        ExRam.Gremlinq.Core.IProjectMapBuilder<TSourceQuery, TElement, TTargetType> By<TSourceProperty, TTargetProperty>(System.Linq.Expressions.Expression<System.Func<TTargetType, TTargetProperty>> targetExpression, System.Linq.Expressions.Expression<System.Func<TElement, TSourceProperty>> projection)
            where TSourceProperty : TTargetProperty;
    }
    public interface IProjectMapResult<TTargetType>
    {
        ExRam.Gremlinq.Core.IMapGremlinQuery<TTargetType> Build();
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8> : ExRam.Gremlinq.Core.IProjectTupleResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8>>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9> By<TItem9>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem9>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9> By<TItem9>(System.Linq.Expressions.Expression<System.Func<TElement, TItem9>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9> : ExRam.Gremlinq.Core.IProjectTupleResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9>>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10> By<TItem10>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem10>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10> By<TItem10>(System.Linq.Expressions.Expression<System.Func<TElement, TItem10>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10> : ExRam.Gremlinq.Core.IProjectTupleResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9, TItem10>>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11> By<TItem11>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem11>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11> By<TItem11>(System.Linq.Expressions.Expression<System.Func<TElement, TItem11>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11> : ExRam.Gremlinq.Core.IProjectTupleResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9, TItem10, TItem11>>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12> By<TItem12>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem12>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12> By<TItem12>(System.Linq.Expressions.Expression<System.Func<TElement, TItem12>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12> : ExRam.Gremlinq.Core.IProjectTupleResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9, TItem10, TItem11, TItem12>>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13> By<TItem13>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem13>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13> By<TItem13>(System.Linq.Expressions.Expression<System.Func<TElement, TItem13>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13> : ExRam.Gremlinq.Core.IProjectTupleResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9, TItem10, TItem11, TItem12, TItem13>>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14> By<TItem14>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem14>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14> By<TItem14>(System.Linq.Expressions.Expression<System.Func<TElement, TItem14>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14> : ExRam.Gremlinq.Core.IProjectTupleResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14>>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15> By<TItem15>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem15>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15> By<TItem15>(System.Linq.Expressions.Expression<System.Func<TElement, TItem15>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15> : ExRam.Gremlinq.Core.IProjectTupleResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, System.ValueTuple<TItem15>>>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16> By<TItem16>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem16>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16> By<TItem16>(System.Linq.Expressions.Expression<System.Func<TElement, TItem16>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16> : ExRam.Gremlinq.Core.IProjectTupleResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, System.ValueTuple<TItem15, TItem16>>>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase { }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1> By<TItem1>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem1>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1> By<TItem1>(System.Linq.Expressions.Expression<System.Func<TElement, TItem1>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2> By<TItem2>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem2>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2> By<TItem2>(System.Linq.Expressions.Expression<System.Func<TElement, TItem2>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2> : ExRam.Gremlinq.Core.IProjectTupleResult<System.ValueTuple<TItem1, TItem2>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3> By<TItem3>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem3>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3> By<TItem3>(System.Linq.Expressions.Expression<System.Func<TElement, TItem3>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3> : ExRam.Gremlinq.Core.IProjectTupleResult<System.ValueTuple<TItem1, TItem2, TItem3>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4> By<TItem4>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem4>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4> By<TItem4>(System.Linq.Expressions.Expression<System.Func<TElement, TItem4>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4> : ExRam.Gremlinq.Core.IProjectTupleResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5> By<TItem5>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem5>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5> By<TItem5>(System.Linq.Expressions.Expression<System.Func<TElement, TItem5>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5> : ExRam.Gremlinq.Core.IProjectTupleResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6> By<TItem6>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem6>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6> By<TItem6>(System.Linq.Expressions.Expression<System.Func<TElement, TItem6>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6> : ExRam.Gremlinq.Core.IProjectTupleResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7> By<TItem7>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem7>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7> By<TItem7>(System.Linq.Expressions.Expression<System.Func<TElement, TItem7>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7> : ExRam.Gremlinq.Core.IProjectTupleResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8> By<TItem8>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem8>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8> By<TItem8>(System.Linq.Expressions.Expression<System.Func<TElement, TItem8>> projection);
    }
    public interface IProjectTupleResult<TTuple>
        where TTuple : System.Runtime.CompilerServices.ITuple
    {
        ExRam.Gremlinq.Core.IMapGremlinQuery<TTuple> Build();
    }
    public interface IPropertyGremlinQueryBase : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IPropertyGremlinQuery<TResult> Cast<TResult>();
    }
    public interface IPropertyGremlinQueryBase<TElement> : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IPropertyGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IGremlinQuery<string> Key();
        ExRam.Gremlinq.Core.IGremlinQuery<object> Value();
        ExRam.Gremlinq.Core.IGremlinQuery<TValue> Value<TValue>();
    }
    public interface IPropertyGremlinQuery<TElement> : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IPropertyGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TElement, ExRam.Gremlinq.Core.IPropertyGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IPropertyGremlinQueryBase, ExRam.Gremlinq.Core.IPropertyGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IStartGremlinQuery { }
    public interface IRepeatEmitLoopBuilder<out TQuery> : ExRam.Gremlinq.Core.IFinalLoopBuilder<TQuery>
        where out TQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IRepeatEmitUntilLoopBuilder<TQuery> Times(int loopCount);
        ExRam.Gremlinq.Core.IRepeatEmitUntilLoopBuilder<TQuery> Until(System.Func<TQuery, ExRam.Gremlinq.Core.IGremlinQueryBase> condition);
    }
    public interface IRepeatEmitUntilLoopBuilder<out TQuery> : ExRam.Gremlinq.Core.IFinalLoopBuilder<TQuery>
        where out TQuery : ExRam.Gremlinq.Core.IGremlinQueryBase { }
    public interface IRepeatLoopBuilder<out TQuery> : ExRam.Gremlinq.Core.IFinalLoopBuilder<TQuery>
        where out TQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IRepeatEmitLoopBuilder<TQuery> Emit();
        ExRam.Gremlinq.Core.IRepeatUntilLoopBuilder<TQuery> Times(int loopCount);
        ExRam.Gremlinq.Core.IRepeatUntilLoopBuilder<TQuery> Until(System.Func<TQuery, ExRam.Gremlinq.Core.IGremlinQueryBase> condition);
    }
    public interface IRepeatUntilLoopBuilder<out TQuery> : ExRam.Gremlinq.Core.IFinalLoopBuilder<TQuery>
        where out TQuery : ExRam.Gremlinq.Core.IGremlinQueryBase { }
    public interface IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge> AddE<TEdge>()
            where TEdge : new();
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge> AddE<TEdge>(TEdge edge);
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex> AddV<TVertex>()
            where TVertex : new();
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex> AddV<TVertex>(TVertex vertex);
        ExRam.Gremlinq.Core.IGremlinQueryAdmin AsAdmin();
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<object> E(object id);
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<object> E(params object[] ids);
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge> E<TEdge>(object id);
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge> E<TEdge>(params object[] ids);
        ExRam.Gremlinq.Core.IGremlinQuery<TElement> Inject<TElement>(params TElement[] elements);
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TNewEdge> ReplaceE<TNewEdge>(TNewEdge edge);
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TNewVertex> ReplaceV<TNewVertex>(TNewVertex vertex);
        ExRam.Gremlinq.Core.IVertexGremlinQuery<object> V(object id);
        ExRam.Gremlinq.Core.IVertexGremlinQuery<object> V(params object[] ids);
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex> V<TVertex>(object id);
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex> V<TVertex>(params object[] ids);
    }
    public interface IStartLoopBuilder<TQuery>
        where TQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IEmitLoopBuilder<TQuery> Emit();
        ExRam.Gremlinq.Core.IRepeatLoopBuilder<TQuery> Repeat(System.Func<TQuery, TQuery> loop);
        ExRam.Gremlinq.Core.IUntilLoopBuilder<TQuery> Until(System.Func<TQuery, ExRam.Gremlinq.Core.IGremlinQueryBase> condition);
    }
    public interface IUntilEmitLoopBuilder<TQuery>
        where TQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IUntilEmitRepeatLoopBuilder<TQuery> Repeat(System.Func<TQuery, TQuery> loop);
    }
    public interface IUntilEmitRepeatLoopBuilder<out TQuery> : ExRam.Gremlinq.Core.IFinalLoopBuilder<TQuery>
        where out TQuery : ExRam.Gremlinq.Core.IGremlinQueryBase { }
    public interface IUntilLoopBuilder<TQuery>
        where TQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IUntilEmitLoopBuilder<TQuery> Emit();
        ExRam.Gremlinq.Core.IUntilRepeatLoopBuilder<TQuery> Repeat(System.Func<TQuery, TQuery> loop);
    }
    public interface IUntilRepeatEmitLoopBuilder<out TQuery> : ExRam.Gremlinq.Core.IFinalLoopBuilder<TQuery>
        where out TQuery : ExRam.Gremlinq.Core.IGremlinQueryBase { }
    public interface IUntilRepeatLoopBuilder<out TQuery> : ExRam.Gremlinq.Core.IFinalLoopBuilder<TQuery>
        where out TQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IUntilRepeatEmitLoopBuilder<TQuery> Emit();
    }
    public interface IVertexGremlinQueryBase : ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IVertexGremlinQuery<object> Both();
        ExRam.Gremlinq.Core.IVertexGremlinQuery<object> Both<TEdge>();
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<object> BothE();
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge> BothE<TEdge>();
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TResult> Cast<TResult>();
        ExRam.Gremlinq.Core.IVertexGremlinQuery<object> In();
        ExRam.Gremlinq.Core.IVertexGremlinQuery<object> In<TEdge>();
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<object> InE();
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge> InE<TEdge>();
        ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQuery<object> Lower();
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TTarget> OfType<TTarget>();
        ExRam.Gremlinq.Core.IVertexGremlinQuery<object> Out();
        ExRam.Gremlinq.Core.IVertexGremlinQuery<object> Out<TEdge>();
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<object> OutE();
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge> OutE<TEdge>();
    }
    public interface IVertexGremlinQueryBase<TVertex> : ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TVertex>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TVertex>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TVertex>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IVertexGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TVertex> AddE<TEdge>()
            where TEdge : new();
        ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TVertex> AddE<TEdge>(TEdge edge);
        ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TEdge, TVertex> InE<TEdge>();
        ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQuery<TVertex> Lower();
        ExRam.Gremlinq.Core.IOutEdgeGremlinQuery<TEdge, TVertex> OutE<TEdge>();
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.VertexProperty<object>, object> Properties();
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.VertexProperty<object>, object> Properties(params System.Linq.Expressions.Expression<System.Func<TVertex, ExRam.Gremlinq.Core.GraphElements.VertexProperty<object>>>[] projections);
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue>, TValue> Properties<TValue>();
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue>, TValue> Properties<TValue>(params System.Linq.Expressions.Expression<System.Func<TVertex, ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue>>>[] projections);
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue>, TValue> Properties<TValue>(params System.Linq.Expressions.Expression<System.Func<TVertex, ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue>[]>>[] projections);
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue>, TValue> Properties<TValue>(params System.Linq.Expressions.Expression<System.Func<TVertex, TValue>>[] projections);
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue>, TValue> Properties<TValue>(params System.Linq.Expressions.Expression<System.Func<TVertex, TValue[]>>[] projections);
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue, TMeta>, TValue, TMeta> Properties<TValue, TMeta>(params System.Linq.Expressions.Expression<System.Func<TVertex, ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue, TMeta>>>[] projections);
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue, TMeta>, TValue, TMeta> Properties<TValue, TMeta>(params System.Linq.Expressions.Expression<System.Func<TVertex, ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue, TMeta>[]>>[] projections);
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex> Update(TVertex element);
        ExRam.Gremlinq.Core.IGremlinQuery<object> Values(params System.Linq.Expressions.Expression<System.Func<TVertex, ExRam.Gremlinq.Core.GraphElements.VertexProperty<object>>>[] projections);
        ExRam.Gremlinq.Core.IGremlinQuery<TValue> Values<TValue>(params System.Linq.Expressions.Expression<System.Func<TVertex, ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue>>>[] projections);
        ExRam.Gremlinq.Core.IGremlinQuery<TValue> Values<TValue>(params System.Linq.Expressions.Expression<System.Func<TVertex, ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue>[]>>[] projections);
        new ExRam.Gremlinq.Core.IGremlinQuery<TValue> Values<TValue>(params System.Linq.Expressions.Expression<System.Func<TVertex, TValue>>[] projections);
        new ExRam.Gremlinq.Core.IGremlinQuery<TValue> Values<TValue>(params System.Linq.Expressions.Expression<System.Func<TVertex, TValue[]>>[] projections);
        ExRam.Gremlinq.Core.IGremlinQuery<TValue> Values<TValue, TMeta>(params System.Linq.Expressions.Expression<System.Func<TVertex, ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue, TMeta>>>[] projections);
        ExRam.Gremlinq.Core.IGremlinQuery<TValue> Values<TValue, TMeta>(params System.Linq.Expressions.Expression<System.Func<TVertex, ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue, TMeta>[]>>[] projections);
    }
    public interface IVertexGremlinQuery<TVertex> : ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TVertex, ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TVertex>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex>>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TVertex, ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex>>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TVertex>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TVertex, ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TVertex>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IVertexGremlinQueryBase, ExRam.Gremlinq.Core.IVertexGremlinQueryBase<TVertex>
    {
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex> Property<TProjectedValue>(System.Linq.Expressions.Expression<System.Func<TVertex, TProjectedValue[]>> projection, TProjectedValue value);
    }
    public interface IVertexPropertyGremlinQueryBase : ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IElementGremlinQuery<object> Lower();
        ExRam.Gremlinq.Core.IPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.Property<object>> Properties(params string[] keys);
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.Collections.Generic.IDictionary<string, object>> ValueMap(params string[] keys);
        new ExRam.Gremlinq.Core.IMapGremlinQuery<System.Collections.Generic.IDictionary<string, TValue>> ValueMap<TValue>();
        ExRam.Gremlinq.Core.IMapGremlinQuery<System.Collections.Generic.IDictionary<string, TValue>> ValueMap<TValue>(params string[] keys);
        ExRam.Gremlinq.Core.IGremlinQuery<object> Values(params string[] keys);
        new ExRam.Gremlinq.Core.IGremlinQuery<TValue> Values<TValue>();
        ExRam.Gremlinq.Core.IGremlinQuery<TValue> Values<TValue>(params string[] keys);
    }
    public interface IVertexPropertyGremlinQueryBase<TProperty, TValue> : ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TProperty>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TProperty>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IElementGremlinQuery<TProperty> Lower();
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue, TMeta>, TValue, TMeta> Meta<TMeta>();
        ExRam.Gremlinq.Core.IPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.Property<TMetaValue>> Properties<TMetaValue>(params string[] keys);
        ExRam.Gremlinq.Core.IGremlinQuery<TValue> Value();
    }
    public interface IVertexPropertyGremlinQueryBase<TProperty, TValue, TMeta> : ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TProperty>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TProperty>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IElementGremlinQuery<TProperty> Lower();
        ExRam.Gremlinq.Core.IPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.Property<TMetaValue>> Properties<TMetaValue>(params System.Linq.Expressions.Expression<System.Func<TMeta, TMetaValue>>[] projections);
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Property<TMetaValue>(System.Linq.Expressions.Expression<System.Func<TMeta, TMetaValue>> projection, TMetaValue value);
        ExRam.Gremlinq.Core.IGremlinQuery<TValue> Value();
        ExRam.Gremlinq.Core.IGremlinQuery<TMeta> ValueMap();
        ExRam.Gremlinq.Core.IGremlinQuery<TMetaValue> Values<TMetaValue>(params System.Linq.Expressions.Expression<System.Func<TMeta, TMetaValue>>[] projections);
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Where(System.Linq.Expressions.Expression<System.Func<ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue, TMeta>, bool>> predicate);
    }
    public interface IVertexPropertyGremlinQuery<TProperty, TValue> : ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue>>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TProperty, ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue>>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TProperty>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TProperty, ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TProperty>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBase, ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBase<TProperty, TValue> { }
    public interface IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> : ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TProperty, ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TProperty>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TProperty, ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TProperty>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBase, ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBase<TProperty, TValue, TMeta> { }
    public readonly struct Key : System.IComparable<ExRam.Gremlinq.Core.Key>
    {
        public Key(Gremlin.Net.Process.Traversal.T t) { }
        public Key(string name) { }
        public object RawKey { get; }
        public int CompareTo(ExRam.Gremlinq.Core.Key other) { }
        public bool Equals(ExRam.Gremlinq.Core.Key other) { }
        public override bool Equals(object? obj) { }
        public override int GetHashCode() { }
        public static ExRam.Gremlinq.Core.Key op_Implicit(Gremlin.Net.Process.Traversal.T t) { }
        public static ExRam.Gremlinq.Core.Key op_Implicit(string name) { }
        public static bool operator !=(ExRam.Gremlinq.Core.Key key1, ExRam.Gremlinq.Core.Key key2) { }
        public static bool operator ==(ExRam.Gremlinq.Core.Key key1, ExRam.Gremlinq.Core.Key key2) { }
    }
    [System.Flags]
    public enum QueryLogFormatting
    {
        None = 0,
        Indented = 1,
    }
    [System.Flags]
    public enum QueryLogVerbosity
    {
        QueryOnly = 0,
        IncludeBindings = 1,
    }
    public abstract class StepLabel : System.IEquatable<ExRam.Gremlinq.Core.StepLabel>
    {
        protected StepLabel() { }
        public virtual ExRam.Gremlinq.Core.StepLabel<TNewValue> Cast<TNewValue>() { }
        public bool Equals(ExRam.Gremlinq.Core.StepLabel? other) { }
        public override bool Equals(object? obj) { }
        public override int GetHashCode() { }
        public static ExRam.Gremlinq.Core.StepLabel op_Implicit(string str) { }
        public static bool operator !=(ExRam.Gremlinq.Core.StepLabel? left, ExRam.Gremlinq.Core.StepLabel? right) { }
        public static bool operator ==(ExRam.Gremlinq.Core.StepLabel? left, ExRam.Gremlinq.Core.StepLabel? right) { }
    }
    public class StepLabel<TElement> : ExRam.Gremlinq.Core.StepLabel
    {
        public StepLabel() { }
        public TElement Value { get; }
        public new static ExRam.Gremlinq.Core.StepLabel<TElement> op_Implicit(string str) { }
    }
    public class StepLabel<TQuery, TElement> : ExRam.Gremlinq.Core.StepLabel<TElement>
        where TQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        public StepLabel() { }
        [System.Runtime.CompilerServices.PreserveBaseOverrides]
        public virtual ExRam.Gremlinq.Core.StepLabel<ExRam.Gremlinq.Core.IGremlinQuery<TNewValue>, TNewValue> Cast<TNewValue>() { }
        public static ExRam.Gremlinq.Core.StepLabel<TQuery, TElement> op_Implicit(string str) { }
    }
    public enum StringComparisonTranslationStrictness
    {
        Strict = 0,
        Lenient = 1,
    }
    public static class TransformerClassExtensions
    {
        public static ExRam.Gremlinq.Core.TransformerClassExtensions.TryTransformToBuilder<TTarget> TryTransformTo<TTarget>(this ExRam.Gremlinq.Core.Transformation.ITransformer transformer)
            where TTarget :  class { }
        public readonly struct TryTransformToBuilder<TTarget>
            where TTarget :  class
        {
            public TryTransformToBuilder(ExRam.Gremlinq.Core.Transformation.ITransformer transformer) { }
            public TTarget? From<TSource>(TSource source, ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment) { }
        }
    }
    public static class TransformerExtensions
    {
        public static ExRam.Gremlinq.Core.TransformerExtensions.TransformToBuilder<TTarget> TransformTo<TTarget>(this ExRam.Gremlinq.Core.Transformation.ITransformer transformer) { }
        public readonly struct TransformToBuilder<TTarget>
        {
            public TransformToBuilder(ExRam.Gremlinq.Core.Transformation.ITransformer transformer) { }
            public TTarget From<TSource>(TSource source, ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment) { }
        }
    }
    public static class TransformerStructExtensions
    {
        public static ExRam.Gremlinq.Core.TransformerStructExtensions.TryTransformToBuilder<TTarget> TryTransformTo<TTarget>(this ExRam.Gremlinq.Core.Transformation.ITransformer transformer)
            where TTarget :  struct { }
        public readonly struct TryTransformToBuilder<TTarget>
            where TTarget :  struct
        {
            public TryTransformToBuilder(ExRam.Gremlinq.Core.Transformation.ITransformer transformer) { }
            public TTarget? From<TSource>(TSource source, ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment) { }
        }
    }
    public readonly struct Traversal
    {
        public static readonly ExRam.Gremlinq.Core.Traversal Empty;
        public int Count { get; }
        public ExRam.Gremlinq.Core.Steps.Step this[int index] { get; }
        public ExRam.Gremlinq.Core.Projections.Projection Projection { get; }
        public ExRam.Gremlinq.Core.Steps.SideEffectSemantics SideEffectSemantics { get; }
        public System.ReadOnlySpan<ExRam.Gremlinq.Core.Steps.Step> Steps { get; }
        public ExRam.Gremlinq.Core.Traversal IncludeProjection(ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment) { }
        public ExRam.Gremlinq.Core.Traversal Pop() { }
        public ExRam.Gremlinq.Core.Traversal Pop(out ExRam.Gremlinq.Core.Steps.Step poppedStep) { }
        public ExRam.Gremlinq.Core.Traversal Push(ExRam.Gremlinq.Core.Steps.Step step) { }
        public ExRam.Gremlinq.Core.Traversal Push(params ExRam.Gremlinq.Core.Steps.Step[] steps) { }
        public ExRam.Gremlinq.Core.Traversal Slice(int start) { }
        public ExRam.Gremlinq.Core.Traversal Slice(int start, int length) { }
        public ExRam.Gremlinq.Core.Traversal WithProjection(ExRam.Gremlinq.Core.Projections.Projection projection) { }
        public static ExRam.Gremlinq.Core.Traversal Create<TState>(int length, TState state, System.Buffers.SpanAction<ExRam.Gremlinq.Core.Steps.Step, TState> action) { }
        public static ExRam.Gremlinq.Core.Traversal op_Implicit(ExRam.Gremlinq.Core.Steps.Step step) { }
    }
    [System.Flags]
    public enum VariableFeatures
    {
        None = 0,
        Variables = 1,
        SerializableValues = 2,
        UniformListValues = 4,
        BooleanArrayValues = 8,
        DoubleArrayValues = 16,
        IntegerArrayValues = 32,
        StringArrayValues = 64,
        FloatValues = 128,
        LongValues = 256,
        MixedListValues = 512,
        StringValues = 1024,
        LongArrayValues = 2048,
        MapValues = 4096,
        ByteArrayValues = 8192,
        FloatArrayValues = 16384,
        BooleanValues = 32768,
        ByteValues = 65536,
        DoubleValues = 131072,
        IntegerValues = 262144,
        All = 524287,
    }
    [System.Flags]
    public enum VertexFeatures
    {
        None = 0,
        MetaProperties = 1,
        Upsert = 2,
        DuplicateMultiProperties = 4,
        AddVertices = 8,
        MultiProperties = 16,
        RemoveVertices = 32,
        AnyIds = 64,
        UuidIds = 128,
        UserSuppliedIds = 256,
        CustomIds = 512,
        NumericIds = 1024,
        RemoveProperty = 2048,
        AddProperty = 4096,
        StringIds = 8192,
        All = 16383,
    }
    [System.Flags]
    public enum VertexPropertyFeatures
    {
        None = 0,
        AnyIds = 1,
        UuidIds = 2,
        UserSuppliedIds = 4,
        CustomIds = 8,
        NumericIds = 16,
        RemoveProperty = 32,
        StringIds = 64,
        Properties = 128,
        SerializableValues = 256,
        UniformListValues = 512,
        BooleanArrayValues = 1024,
        DoubleArrayValues = 2048,
        IntegerArrayValues = 4096,
        StringArrayValues = 8192,
        FloatValues = 16384,
        LongValues = 32768,
        MixedListValues = 65536,
        StringValues = 131072,
        LongArrayValues = 262144,
        MapValues = 524288,
        ByteArrayValues = 1048576,
        FloatArrayValues = 2097152,
        BooleanValues = 4194304,
        ByteValues = 8388608,
        DoubleValues = 16777216,
        IntegerValues = 33554432,
        All = 67108863,
    }
}
namespace ExRam.Gremlinq.Core.Deserialization
{
    public static class Deserializer
    {
        public static readonly ExRam.Gremlinq.Core.Transformation.ITransformer Default;
    }
}
namespace ExRam.Gremlinq.Core.Execution
{
    public readonly struct GremlinQueryExecutionContext
    {
        public System.Guid ExecutionId { get; }
        public ExRam.Gremlinq.Core.IGremlinQueryBase Query { get; }
        public ExRam.Gremlinq.Core.Execution.GremlinQueryExecutionContext TransformQuery(System.Func<ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase> transformation) { }
        public ExRam.Gremlinq.Core.Execution.GremlinQueryExecutionContext WithNewExecutionId() { }
        public static ExRam.Gremlinq.Core.Execution.GremlinQueryExecutionContext Create(ExRam.Gremlinq.Core.IGremlinQueryBase query) { }
    }
    public class GremlinQueryExecutionException : System.Exception
    {
        public GremlinQueryExecutionException(ExRam.Gremlinq.Core.Execution.GremlinQueryExecutionContext executionContext, System.Exception innerException) { }
        public GremlinQueryExecutionException(ExRam.Gremlinq.Core.Execution.GremlinQueryExecutionContext executionContext, string message, System.Exception innerException) { }
        public ExRam.Gremlinq.Core.Execution.GremlinQueryExecutionContext ExecutionContext { get; }
    }
    public static class GremlinQueryExecutor
    {
        public static readonly ExRam.Gremlinq.Core.Execution.IGremlinQueryExecutor Empty;
        public static readonly ExRam.Gremlinq.Core.Execution.IGremlinQueryExecutor Invalid;
        public static ExRam.Gremlinq.Core.Execution.IGremlinQueryExecutor RetryWithExponentialBackoff(this ExRam.Gremlinq.Core.Execution.IGremlinQueryExecutor executor, System.Func<int, ExRam.Gremlinq.Core.Execution.GremlinQueryExecutionException, bool> shouldRetry) { }
        public static ExRam.Gremlinq.Core.Execution.IGremlinQueryExecutor Serialize(this ExRam.Gremlinq.Core.Execution.IGremlinQueryExecutor executor) { }
        public static ExRam.Gremlinq.Core.Execution.IGremlinQueryExecutor TransformExecutionException(this ExRam.Gremlinq.Core.Execution.IGremlinQueryExecutor executor, System.Func<ExRam.Gremlinq.Core.Execution.GremlinQueryExecutionException, ExRam.Gremlinq.Core.Execution.GremlinQueryExecutionException> exceptionTransformation) { }
        public static ExRam.Gremlinq.Core.Execution.IGremlinQueryExecutor TransformQuery(this ExRam.Gremlinq.Core.Execution.IGremlinQueryExecutor baseExecutor, System.Func<ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase> transformation) { }
    }
    public interface IGremlinQueryExecutor
    {
        System.Collections.Generic.IAsyncEnumerable<T> Execute<T>(ExRam.Gremlinq.Core.Execution.GremlinQueryExecutionContext context);
    }
}
namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    public abstract class ConstantExpressionSemantics : ExRam.Gremlinq.Core.ExpressionParsing.ExpressionSemantics
    {
        protected ConstantExpressionSemantics() { }
    }
    public sealed class ContainsExpressionSemantics : ExRam.Gremlinq.Core.ExpressionParsing.EnumerableExpressionSemantics
    {
        public static readonly ExRam.Gremlinq.Core.ExpressionParsing.ContainsExpressionSemantics Instance;
        public override ExRam.Gremlinq.Core.ExpressionParsing.ExpressionSemantics Flip() { }
    }
    public sealed class EndsWithExpressionSemantics : ExRam.Gremlinq.Core.ExpressionParsing.StringExpressionSemantics
    {
        public static readonly ExRam.Gremlinq.Core.ExpressionParsing.EndsWithExpressionSemantics CaseInsensitive;
        public static readonly ExRam.Gremlinq.Core.ExpressionParsing.EndsWithExpressionSemantics CaseSensitive;
        public override ExRam.Gremlinq.Core.ExpressionParsing.ExpressionSemantics Flip() { }
        public static ExRam.Gremlinq.Core.ExpressionParsing.EndsWithExpressionSemantics Get(System.StringComparison comparison) { }
    }
    public abstract class EnumerableExpressionSemantics : ExRam.Gremlinq.Core.ExpressionParsing.ExpressionSemantics
    {
        protected EnumerableExpressionSemantics() { }
    }
    public sealed class EqualsExpressionSemantics : ExRam.Gremlinq.Core.ExpressionParsing.ObjectExpressionSemantics
    {
        public static readonly ExRam.Gremlinq.Core.ExpressionParsing.EqualsExpressionSemantics Instance;
        public override ExRam.Gremlinq.Core.ExpressionParsing.ExpressionSemantics Flip() { }
    }
    public abstract class ExpressionSemantics
    {
        protected ExpressionSemantics() { }
        public abstract ExRam.Gremlinq.Core.ExpressionParsing.ExpressionSemantics Flip();
    }
    public sealed class FalseExpressionSemantics : ExRam.Gremlinq.Core.ExpressionParsing.ConstantExpressionSemantics
    {
        public static readonly ExRam.Gremlinq.Core.ExpressionParsing.FalseExpressionSemantics Instance;
        public override ExRam.Gremlinq.Core.ExpressionParsing.ExpressionSemantics Flip() { }
    }
    public sealed class GreaterThanExpressionSemantics : ExRam.Gremlinq.Core.ExpressionParsing.ObjectExpressionSemantics
    {
        public static readonly ExRam.Gremlinq.Core.ExpressionParsing.GreaterThanExpressionSemantics Instance;
        public override ExRam.Gremlinq.Core.ExpressionParsing.ExpressionSemantics Flip() { }
    }
    public sealed class GreaterThanOrEqualExpressionSemantics : ExRam.Gremlinq.Core.ExpressionParsing.ObjectExpressionSemantics
    {
        public static readonly ExRam.Gremlinq.Core.ExpressionParsing.GreaterThanOrEqualExpressionSemantics Instance;
        public override ExRam.Gremlinq.Core.ExpressionParsing.ExpressionSemantics Flip() { }
    }
    public sealed class HasInfixExpressionSemantics : ExRam.Gremlinq.Core.ExpressionParsing.StringExpressionSemantics
    {
        public static readonly ExRam.Gremlinq.Core.ExpressionParsing.HasInfixExpressionSemantics CaseInsensitive;
        public static readonly ExRam.Gremlinq.Core.ExpressionParsing.HasInfixExpressionSemantics CaseSensitive;
        public override ExRam.Gremlinq.Core.ExpressionParsing.ExpressionSemantics Flip() { }
        public static ExRam.Gremlinq.Core.ExpressionParsing.HasInfixExpressionSemantics Get(System.StringComparison comparison) { }
    }
    public interface IPFactory
    {
        Gremlin.Net.Process.Traversal.P? TryGetP(ExRam.Gremlinq.Core.ExpressionParsing.ExpressionSemantics semantics, object? value, ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment);
    }
    public sealed class IntersectsExpressionSemantics : ExRam.Gremlinq.Core.ExpressionParsing.EnumerableExpressionSemantics
    {
        public static readonly ExRam.Gremlinq.Core.ExpressionParsing.IntersectsExpressionSemantics Instance;
        public override ExRam.Gremlinq.Core.ExpressionParsing.ExpressionSemantics Flip() { }
    }
    public sealed class IsContainedInExpressionSemantics : ExRam.Gremlinq.Core.ExpressionParsing.EnumerableExpressionSemantics
    {
        public static readonly ExRam.Gremlinq.Core.ExpressionParsing.IsContainedInExpressionSemantics Instance;
        public override ExRam.Gremlinq.Core.ExpressionParsing.ExpressionSemantics Flip() { }
    }
    public sealed class IsInfixOfExpressionSemantics : ExRam.Gremlinq.Core.ExpressionParsing.StringExpressionSemantics
    {
        public static readonly ExRam.Gremlinq.Core.ExpressionParsing.IsInfixOfExpressionSemantics CaseInsensitive;
        public static readonly ExRam.Gremlinq.Core.ExpressionParsing.IsInfixOfExpressionSemantics CaseSensitive;
        public override ExRam.Gremlinq.Core.ExpressionParsing.ExpressionSemantics Flip() { }
        public static ExRam.Gremlinq.Core.ExpressionParsing.IsInfixOfExpressionSemantics Get(System.StringComparison comparison) { }
    }
    public sealed class IsPrefixOfExpressionSemantics : ExRam.Gremlinq.Core.ExpressionParsing.StringExpressionSemantics
    {
        public static readonly ExRam.Gremlinq.Core.ExpressionParsing.IsPrefixOfExpressionSemantics CaseInsensitive;
        public static readonly ExRam.Gremlinq.Core.ExpressionParsing.IsPrefixOfExpressionSemantics CaseSensitive;
        public override ExRam.Gremlinq.Core.ExpressionParsing.ExpressionSemantics Flip() { }
        public static ExRam.Gremlinq.Core.ExpressionParsing.IsPrefixOfExpressionSemantics Get(System.StringComparison comparison) { }
    }
    public sealed class IsSuffixOfExpressionSemantics : ExRam.Gremlinq.Core.ExpressionParsing.StringExpressionSemantics
    {
        public static readonly ExRam.Gremlinq.Core.ExpressionParsing.IsSuffixOfExpressionSemantics CaseInsensitive;
        public static readonly ExRam.Gremlinq.Core.ExpressionParsing.IsSuffixOfExpressionSemantics CaseSensitive;
        public override ExRam.Gremlinq.Core.ExpressionParsing.ExpressionSemantics Flip() { }
        public static ExRam.Gremlinq.Core.ExpressionParsing.IsSuffixOfExpressionSemantics Get(System.StringComparison comparison) { }
    }
    public sealed class LowerThanExpressionSemantics : ExRam.Gremlinq.Core.ExpressionParsing.ObjectExpressionSemantics
    {
        public static readonly ExRam.Gremlinq.Core.ExpressionParsing.LowerThanExpressionSemantics Instance;
        public override ExRam.Gremlinq.Core.ExpressionParsing.ExpressionSemantics Flip() { }
    }
    public sealed class LowerThanOrEqualExpressionSemantics : ExRam.Gremlinq.Core.ExpressionParsing.ObjectExpressionSemantics
    {
        public static readonly ExRam.Gremlinq.Core.ExpressionParsing.LowerThanOrEqualExpressionSemantics Instance;
        public override ExRam.Gremlinq.Core.ExpressionParsing.ExpressionSemantics Flip() { }
    }
    public sealed class NotEqualsExpressionSemantics : ExRam.Gremlinq.Core.ExpressionParsing.ObjectExpressionSemantics
    {
        public static readonly ExRam.Gremlinq.Core.ExpressionParsing.NotEqualsExpressionSemantics Instance;
        public override ExRam.Gremlinq.Core.ExpressionParsing.ExpressionSemantics Flip() { }
    }
    public abstract class ObjectExpressionSemantics : ExRam.Gremlinq.Core.ExpressionParsing.ExpressionSemantics
    {
        protected ObjectExpressionSemantics() { }
        public ExRam.Gremlinq.Core.ExpressionParsing.ExpressionSemantics TransformCompareTo(int comparison) { }
    }
    public static class PFactory
    {
        public static readonly ExRam.Gremlinq.Core.ExpressionParsing.IPFactory Default;
        public static readonly ExRam.Gremlinq.Core.GremlinqOption<ExRam.Gremlinq.Core.ExpressionParsing.IPFactory> PFactoryOption;
        public static ExRam.Gremlinq.Core.ExpressionParsing.IPFactory Override(this ExRam.Gremlinq.Core.ExpressionParsing.IPFactory originalFactory, ExRam.Gremlinq.Core.ExpressionParsing.IPFactory overrideFactory) { }
    }
    public sealed class StartsWithExpressionSemantics : ExRam.Gremlinq.Core.ExpressionParsing.StringExpressionSemantics
    {
        public static readonly ExRam.Gremlinq.Core.ExpressionParsing.StartsWithExpressionSemantics CaseInsensitive;
        public static readonly ExRam.Gremlinq.Core.ExpressionParsing.StartsWithExpressionSemantics CaseSensitive;
        public override ExRam.Gremlinq.Core.ExpressionParsing.ExpressionSemantics Flip() { }
        public static ExRam.Gremlinq.Core.ExpressionParsing.StartsWithExpressionSemantics Get(System.StringComparison comparison) { }
    }
    public sealed class StringEqualsExpressionSemantics : ExRam.Gremlinq.Core.ExpressionParsing.StringExpressionSemantics
    {
        public static readonly ExRam.Gremlinq.Core.ExpressionParsing.StringEqualsExpressionSemantics CaseInsensitive;
        public static readonly ExRam.Gremlinq.Core.ExpressionParsing.StringEqualsExpressionSemantics CaseSensitive;
        public override ExRam.Gremlinq.Core.ExpressionParsing.ExpressionSemantics Flip() { }
        public static ExRam.Gremlinq.Core.ExpressionParsing.StringEqualsExpressionSemantics Get(System.StringComparison comparison) { }
    }
    public abstract class StringExpressionSemantics : ExRam.Gremlinq.Core.ExpressionParsing.ExpressionSemantics
    {
        protected StringExpressionSemantics(System.StringComparison comparison) { }
        public System.StringComparison Comparison { get; }
    }
    public sealed class TrueExpressionSemantics : ExRam.Gremlinq.Core.ExpressionParsing.ConstantExpressionSemantics
    {
        public static readonly ExRam.Gremlinq.Core.ExpressionParsing.TrueExpressionSemantics Instance;
        public override ExRam.Gremlinq.Core.ExpressionParsing.ExpressionSemantics Flip() { }
    }
}
namespace ExRam.Gremlinq.Core.GraphElements
{
    public sealed class Path
    {
        public Path() { }
        public string[][] Labels { get; set; }
        public object[] Objects { get; set; }
    }
    public abstract class Property
    {
        protected Property() { }
        public string? Key { get; }
        protected abstract object? GetValue();
        public override string ToString() { }
    }
    public class Property<TValue> : ExRam.Gremlinq.Core.GraphElements.Property
    {
        public Property(TValue value) { }
        public TValue Value { get; set; }
        protected override object? GetValue() { }
        public static ExRam.Gremlinq.Core.GraphElements.Property<TValue> op_Implicit(ExRam.Gremlinq.Core.GraphElements.Property<TValue>[] value) { }
        public static ExRam.Gremlinq.Core.GraphElements.Property<TValue> op_Implicit(TValue value) { }
        public static ExRam.Gremlinq.Core.GraphElements.Property<TValue> op_Implicit(TValue[] value) { }
    }
    public class VertexProperty<TValue> : ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue, System.Collections.Generic.IDictionary<string, object>>
    {
        public VertexProperty(TValue value) { }
        protected override System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, object>> GetProperties(ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment) { }
        public static ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue> op_Implicit(ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue>[] value) { }
        public static ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue> op_Implicit(TValue value) { }
        public static ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue> op_Implicit(TValue[] value) { }
    }
    public class VertexProperty<TValue, TMeta> : ExRam.Gremlinq.Core.GraphElements.Property<TValue>
    {
        public VertexProperty(TValue value) { }
        public object? Id { get; }
        public string? Label { get; }
        public TMeta Properties { get; set; }
        protected virtual System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, object>> GetProperties(ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment) { }
        public override string ToString() { }
        public static ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue, TMeta> op_Implicit(ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue, TMeta>[] value) { }
        public static ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue, TMeta> op_Implicit(TValue value) { }
        public static ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue, TMeta> op_Implicit(TValue[] value) { }
    }
}
namespace ExRam.Gremlinq.Core.Models
{
    public readonly struct ElementMetadata : System.IEquatable<ExRam.Gremlinq.Core.Models.ElementMetadata>
    {
        public ElementMetadata(string label) { }
        public string Label { get; }
        public bool Equals(ExRam.Gremlinq.Core.Models.ElementMetadata other) { }
        public override bool Equals(object? obj) { }
        public override int GetHashCode() { }
        public static bool operator !=(ExRam.Gremlinq.Core.Models.ElementMetadata left, ExRam.Gremlinq.Core.Models.ElementMetadata right) { }
        public static bool operator ==(ExRam.Gremlinq.Core.Models.ElementMetadata left, ExRam.Gremlinq.Core.Models.ElementMetadata right) { }
    }
    public static class GraphElementModel
    {
        public static ExRam.Gremlinq.Core.Models.IGraphElementModel ConfigureElement<TElement>(this ExRam.Gremlinq.Core.Models.IGraphElementModel model, System.Func<ExRam.Gremlinq.Core.Models.IMemberMetadataConfigurator<TElement>, ExRam.Gremlinq.Core.Models.IMemberMetadataConfigurator<TElement>> transformation) { }
        public static ExRam.Gremlinq.Core.Models.IGraphElementModel ConfigureLabels(this ExRam.Gremlinq.Core.Models.IGraphElementModel model, System.Func<System.Type, string, string> overrideTransformation) { }
        public static ExRam.Gremlinq.Core.Models.IGraphElementModel ConfigureMemberNames(this ExRam.Gremlinq.Core.Models.IGraphElementModel model, System.Func<System.Reflection.MemberInfo, string, string> transformation) { }
        public static ExRam.Gremlinq.Core.Models.IGraphElementModel UseCamelCaseLabels(this ExRam.Gremlinq.Core.Models.IGraphElementModel model) { }
        public static ExRam.Gremlinq.Core.Models.IGraphElementModel UseCamelCaseMemberNames(this ExRam.Gremlinq.Core.Models.IGraphElementModel model) { }
        public static ExRam.Gremlinq.Core.Models.IGraphElementModel UseLowerCaseLabels(this ExRam.Gremlinq.Core.Models.IGraphElementModel model) { }
        public static ExRam.Gremlinq.Core.Models.IGraphElementModel UseLowerCaseMemberNames(this ExRam.Gremlinq.Core.Models.IGraphElementModel model) { }
    }
    public static class GraphModel
    {
        public static readonly ExRam.Gremlinq.Core.Models.IGraphModel Invalid;
        public static ExRam.Gremlinq.Core.Models.IGraphModel ConfigureElements(this ExRam.Gremlinq.Core.Models.IGraphModel model, System.Func<ExRam.Gremlinq.Core.Models.IGraphElementModel, ExRam.Gremlinq.Core.Models.IGraphElementModel> transformation) { }
        public static ExRam.Gremlinq.Core.Models.IGraphModel FromBaseTypes<TVertexBaseType, TEdgeBaseType>() { }
    }
    public interface IGraphElementModel
    {
        System.Collections.Immutable.IImmutableSet<System.Type> ElementTypes { get; }
        System.Collections.Immutable.IImmutableSet<System.Reflection.MemberInfo> Members { get; }
        ExRam.Gremlinq.Core.Models.IGraphElementModel AddAssemblies(params System.Reflection.Assembly[] assemblies);
        ExRam.Gremlinq.Core.Models.IGraphElementModel ConfigureMetadata(System.Func<System.Reflection.MemberInfo, ExRam.Gremlinq.Core.Models.MemberMetadata, ExRam.Gremlinq.Core.Models.MemberMetadata> transformation);
        ExRam.Gremlinq.Core.Models.IGraphElementModel ConfigureMetadata(System.Func<System.Type, ExRam.Gremlinq.Core.Models.ElementMetadata, ExRam.Gremlinq.Core.Models.ElementMetadata> metaDataTransformation);
        ExRam.Gremlinq.Core.Models.IGraphElementModel ConfigureMetadata(System.Reflection.MemberInfo member, System.Func<ExRam.Gremlinq.Core.Models.MemberMetadata, ExRam.Gremlinq.Core.Models.MemberMetadata> transformation);
        ExRam.Gremlinq.Core.Models.IGraphElementModel ConfigureMetadata(System.Type elementType, System.Func<ExRam.Gremlinq.Core.Models.ElementMetadata, ExRam.Gremlinq.Core.Models.ElementMetadata> metaDataTransformation);
        ExRam.Gremlinq.Core.Models.MemberMetadata? TryGetMetadata(System.Reflection.MemberInfo memberInfo);
        ExRam.Gremlinq.Core.Models.ElementMetadata? TryGetMetadata(System.Type elementType);
    }
    public interface IGraphModel
    {
        ExRam.Gremlinq.Core.Models.IGraphElementModel EdgesModel { get; }
        ExRam.Gremlinq.Core.Models.IGraphElementModel VerticesModel { get; }
        ExRam.Gremlinq.Core.Models.IGraphModel AddAssemblies(params System.Reflection.Assembly[] assemblies);
        ExRam.Gremlinq.Core.Models.IGraphModel ConfigureEdges(System.Func<ExRam.Gremlinq.Core.Models.IGraphElementModel, ExRam.Gremlinq.Core.Models.IGraphElementModel> transformation);
        ExRam.Gremlinq.Core.Models.IGraphModel ConfigureVertices(System.Func<ExRam.Gremlinq.Core.Models.IGraphElementModel, ExRam.Gremlinq.Core.Models.IGraphElementModel> transformation);
    }
    public interface IMemberMetadataConfigurator<TElement>
    {
        ExRam.Gremlinq.Core.Models.IMemberMetadataConfigurator<TElement> ConfigureName<TProperty>(System.Linq.Expressions.Expression<System.Func<TElement, TProperty>> propertyExpression, string name);
        ExRam.Gremlinq.Core.Models.IMemberMetadataConfigurator<TElement> IgnoreAlways<TProperty>(System.Linq.Expressions.Expression<System.Func<TElement, TProperty>> propertyExpression);
        ExRam.Gremlinq.Core.Models.IMemberMetadataConfigurator<TElement> IgnoreOnAdd<TProperty>(System.Linq.Expressions.Expression<System.Func<TElement, TProperty>> propertyExpression);
        ExRam.Gremlinq.Core.Models.IMemberMetadataConfigurator<TElement> IgnoreOnUpdate<TProperty>(System.Linq.Expressions.Expression<System.Func<TElement, TProperty>> propertyExpression);
        ExRam.Gremlinq.Core.Models.IMemberMetadataConfigurator<TElement> ResetSerializationBehaviour<TProperty>(System.Linq.Expressions.Expression<System.Func<TElement, TProperty>> propertyExpression);
        ExRam.Gremlinq.Core.Models.IGraphElementModel Transform(ExRam.Gremlinq.Core.Models.IGraphElementModel model);
    }
    public readonly struct MemberMetadata : System.IEquatable<ExRam.Gremlinq.Core.Models.MemberMetadata>
    {
        public MemberMetadata(ExRam.Gremlinq.Core.Key key, ExRam.Gremlinq.Core.Models.SerializationBehaviour serializationBehaviour = 0) { }
        public ExRam.Gremlinq.Core.Key Key { get; }
        public ExRam.Gremlinq.Core.Models.SerializationBehaviour SerializationBehaviour { get; }
        public bool Equals(ExRam.Gremlinq.Core.Models.MemberMetadata other) { }
        public override bool Equals(object? obj) { }
        public override int GetHashCode() { }
        public static ExRam.Gremlinq.Core.Models.MemberMetadata Default(string key) { }
        public static bool operator !=(ExRam.Gremlinq.Core.Models.MemberMetadata left, ExRam.Gremlinq.Core.Models.MemberMetadata right) { }
        public static bool operator ==(ExRam.Gremlinq.Core.Models.MemberMetadata left, ExRam.Gremlinq.Core.Models.MemberMetadata right) { }
    }
    [System.Flags]
    public enum SerializationBehaviour
    {
        Default = 0,
        IgnoreOnAdd = 1,
        IgnoreOnUpdate = 2,
        IgnoreAlways = 3,
    }
}
namespace ExRam.Gremlinq.Core.Projections
{
    public sealed class ArrayProjection : ExRam.Gremlinq.Core.Projections.Projection
    {
        public override ExRam.Gremlinq.Core.Projections.Projection Lower() { }
        public override ExRam.Gremlinq.Core.Traversal ToTraversal(ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment) { }
        public ExRam.Gremlinq.Core.Projections.Projection Unfold() { }
    }
    public sealed class EdgeOrVertexProjection : ExRam.Gremlinq.Core.Projections.Projection
    {
        public EdgeOrVertexProjection() { }
        public override ExRam.Gremlinq.Core.Projections.Projection Lower() { }
    }
    public sealed class EdgeProjection : ExRam.Gremlinq.Core.Projections.Projection
    {
        public EdgeProjection() { }
        public override ExRam.Gremlinq.Core.Projections.Projection Lower() { }
        public override ExRam.Gremlinq.Core.Traversal ToTraversal(ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment) { }
    }
    public sealed class ElementProjection : ExRam.Gremlinq.Core.Projections.Projection
    {
        public ElementProjection() { }
        public override ExRam.Gremlinq.Core.Projections.Projection Lower() { }
    }
    public sealed class EmptyProjection : ExRam.Gremlinq.Core.Projections.Projection
    {
        public EmptyProjection() { }
        public override ExRam.Gremlinq.Core.Projections.Projection Lower() { }
    }
    public sealed class GroupProjection : ExRam.Gremlinq.Core.Projections.Projection
    {
        public override ExRam.Gremlinq.Core.Projections.Projection Lower() { }
        public override ExRam.Gremlinq.Core.Traversal ToTraversal(ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment) { }
    }
    public abstract class Projection
    {
        public static readonly ExRam.Gremlinq.Core.Projections.EdgeProjection Edge;
        public static readonly ExRam.Gremlinq.Core.Projections.EdgeOrVertexProjection EdgeOrVertex;
        public static readonly ExRam.Gremlinq.Core.Projections.ElementProjection Element;
        public static readonly ExRam.Gremlinq.Core.Projections.EmptyProjection Empty;
        public static readonly ExRam.Gremlinq.Core.Projections.ValueProjection Value;
        public static readonly ExRam.Gremlinq.Core.Projections.VertexProjection Vertex;
        public static readonly ExRam.Gremlinq.Core.Projections.VertexPropertyProjection VertexProperty;
        protected Projection() { }
        public string Name { get; }
        public ExRam.Gremlinq.Core.Projections.ArrayProjection Fold() { }
        public ExRam.Gremlinq.Core.Projections.GroupProjection Group(ExRam.Gremlinq.Core.Projections.Projection keyProjection, ExRam.Gremlinq.Core.Projections.Projection valueProjection) { }
        public abstract ExRam.Gremlinq.Core.Projections.Projection Lower();
        public ExRam.Gremlinq.Core.Projections.TupleProjection Project(ExRam.Gremlinq.Core.Steps.ProjectStep projectStep, ExRam.Gremlinq.Core.Steps.ProjectStep.ByStep[] bySteps) { }
        public virtual ExRam.Gremlinq.Core.Traversal ToTraversal(ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment) { }
    }
    public sealed class TupleProjection : ExRam.Gremlinq.Core.Projections.Projection
    {
        public override ExRam.Gremlinq.Core.Projections.Projection Lower() { }
        public ExRam.Gremlinq.Core.Projections.Projection Select(System.Collections.Immutable.ImmutableArray<ExRam.Gremlinq.Core.Key> keys) { }
        public override ExRam.Gremlinq.Core.Traversal ToTraversal(ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment) { }
    }
    public sealed class ValueProjection : ExRam.Gremlinq.Core.Projections.Projection
    {
        public ValueProjection() { }
        public override ExRam.Gremlinq.Core.Projections.Projection Lower() { }
    }
    public sealed class VertexProjection : ExRam.Gremlinq.Core.Projections.Projection
    {
        public VertexProjection() { }
        public override ExRam.Gremlinq.Core.Projections.Projection Lower() { }
        public override ExRam.Gremlinq.Core.Traversal ToTraversal(ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment) { }
    }
    public sealed class VertexPropertyProjection : ExRam.Gremlinq.Core.Projections.Projection
    {
        public VertexPropertyProjection() { }
        public override ExRam.Gremlinq.Core.Projections.Projection Lower() { }
        public override ExRam.Gremlinq.Core.Traversal ToTraversal(ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment) { }
    }
}
namespace ExRam.Gremlinq.Core.Serialization
{
    public readonly struct GroovyExpression
    {
        public string Identifier { get; }
        public System.Collections.Immutable.ImmutableArray<Gremlin.Net.Process.Traversal.Instruction> Instructions { get; }
        public static ExRam.Gremlinq.Core.Serialization.GroovyExpression From(string identifier, System.Collections.Immutable.ImmutableArray<Gremlin.Net.Process.Traversal.Instruction> instructions) { }
    }
    public readonly struct GroovyGremlinScript
    {
        [System.Obsolete("Use GroovyGremlinScript.From(...) instead.")]
        public GroovyGremlinScript(string script, System.Collections.Generic.IReadOnlyDictionary<string, object?> bindings) { }
        public System.Collections.Immutable.ImmutableDictionary<string, object?> Bindings { get; }
        public string Script { get; }
        public ExRam.Gremlinq.Core.Serialization.GroovyGremlinScript Bind(string variable, object value) { }
        public override string ToString() { }
        public static ExRam.Gremlinq.Core.Serialization.GroovyGremlinScript From(string script, System.Collections.Immutable.ImmutableDictionary<string, object?>? bindings = null) { }
    }
    public static class Serializer
    {
        public static readonly ExRam.Gremlinq.Core.Transformation.ITransformer Default;
    }
}
namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class AddEStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public AddEStep(string label) { }
        public string Label { get; }
        public sealed class FromLabelStep : ExRam.Gremlinq.Core.Steps.Step
        {
            public FromLabelStep(ExRam.Gremlinq.Core.StepLabel stepLabel) { }
            public ExRam.Gremlinq.Core.StepLabel StepLabel { get; }
        }
        public sealed class FromTraversalStep : ExRam.Gremlinq.Core.Steps.Step
        {
            public FromTraversalStep(ExRam.Gremlinq.Core.Traversal traversal) { }
            public ExRam.Gremlinq.Core.Traversal Traversal { get; }
        }
        public sealed class ToLabelStep : ExRam.Gremlinq.Core.Steps.Step
        {
            public ToLabelStep(ExRam.Gremlinq.Core.StepLabel stepLabel) { }
            public ExRam.Gremlinq.Core.StepLabel StepLabel { get; }
        }
        public sealed class ToTraversalStep : ExRam.Gremlinq.Core.Steps.Step
        {
            public ToTraversalStep(ExRam.Gremlinq.Core.Traversal traversal) { }
            public ExRam.Gremlinq.Core.Traversal Traversal { get; }
        }
    }
    public sealed class AddVStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public AddVStep(string label) { }
        public string Label { get; }
    }
    public sealed class AggregateStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public AggregateStep(Gremlin.Net.Process.Traversal.Scope scope, ExRam.Gremlinq.Core.StepLabel stepLabel) { }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
        public ExRam.Gremlinq.Core.StepLabel StepLabel { get; }
    }
    public sealed class AndStep : ExRam.Gremlinq.Core.Steps.LogicalStep<ExRam.Gremlinq.Core.Steps.AndStep>
    {
        public AndStep(System.Collections.Generic.IEnumerable<ExRam.Gremlinq.Core.Traversal> traversals) { }
    }
    public sealed class AsStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public AsStep(ExRam.Gremlinq.Core.StepLabel stepLabel) { }
        public ExRam.Gremlinq.Core.StepLabel StepLabel { get; }
    }
    public sealed class BarrierStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.BarrierStep Instance;
        public BarrierStep() { }
    }
    public sealed class BothEStep : ExRam.Gremlinq.Core.Steps.DerivedLabelNamesStep
    {
        public static readonly ExRam.Gremlinq.Core.Steps.BothEStep NoLabels;
        public BothEStep() { }
        public BothEStep(System.Collections.Immutable.ImmutableArray<string> labels) { }
    }
    public sealed class BothStep : ExRam.Gremlinq.Core.Steps.DerivedLabelNamesStep
    {
        public static readonly ExRam.Gremlinq.Core.Steps.BothStep NoLabels;
        public BothStep() { }
        public BothStep(System.Collections.Immutable.ImmutableArray<string> labels) { }
    }
    public sealed class BothVStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.BothVStep Instance;
        public BothVStep() { }
    }
    public sealed class CapStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public CapStep(ExRam.Gremlinq.Core.StepLabel stepLabel) { }
        public ExRam.Gremlinq.Core.StepLabel StepLabel { get; }
    }
    public sealed class ChooseOptionTraversalStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public ChooseOptionTraversalStep(ExRam.Gremlinq.Core.Traversal traversal) { }
        public ExRam.Gremlinq.Core.Traversal Traversal { get; }
    }
    public sealed class ChoosePredicateStep : ExRam.Gremlinq.Core.Steps.ChooseStep
    {
        public ChoosePredicateStep(Gremlin.Net.Process.Traversal.P predicate, ExRam.Gremlinq.Core.Traversal thenTraversal, ExRam.Gremlinq.Core.Traversal? elseTraversal = default) { }
        public Gremlin.Net.Process.Traversal.P Predicate { get; }
    }
    public abstract class ChooseStep : ExRam.Gremlinq.Core.Steps.Step
    {
        protected ChooseStep(ExRam.Gremlinq.Core.Traversal thenTraversal, ExRam.Gremlinq.Core.Traversal? elseTraversal = default, ExRam.Gremlinq.Core.Steps.SideEffectSemanticsChange sideEffectSemanticsChange = 1) { }
        public ExRam.Gremlinq.Core.Traversal? ElseTraversal { get; }
        public ExRam.Gremlinq.Core.Traversal ThenTraversal { get; }
    }
    public sealed class ChooseTraversalStep : ExRam.Gremlinq.Core.Steps.ChooseStep
    {
        public ChooseTraversalStep(ExRam.Gremlinq.Core.Traversal ifTraversal, ExRam.Gremlinq.Core.Traversal thenTraversal, ExRam.Gremlinq.Core.Traversal? elseTraversal = default) { }
        public ExRam.Gremlinq.Core.Traversal IfTraversal { get; }
    }
    public sealed class CoalesceStep : ExRam.Gremlinq.Core.Steps.MultiTraversalArgumentStep
    {
        public CoalesceStep(System.Collections.Immutable.ImmutableArray<ExRam.Gremlinq.Core.Traversal> traversals) { }
    }
    public sealed class CoinStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public CoinStep(double probability) { }
        public double Probability { get; }
    }
    public sealed class ConstantStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public ConstantStep(object? value) { }
        public object? Value { get; }
    }
    public sealed class CountStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.CountStep Global;
        public static readonly ExRam.Gremlinq.Core.Steps.CountStep Local;
        public CountStep(Gremlin.Net.Process.Traversal.Scope scope) { }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
    }
    public sealed class CyclicPathStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.CyclicPathStep Instance;
        public CyclicPathStep() { }
    }
    public sealed class DedupStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.DedupStep Global;
        public static readonly ExRam.Gremlinq.Core.Steps.DedupStep Local;
        public DedupStep(Gremlin.Net.Process.Traversal.Scope scope) { }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
    }
    public abstract class DerivedLabelNamesStep : ExRam.Gremlinq.Core.Steps.Step
    {
        protected DerivedLabelNamesStep(System.Collections.Immutable.ImmutableArray<string> labels) { }
        public System.Collections.Immutable.ImmutableArray<string> Labels { get; }
    }
    public sealed class DropStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.DropStep Instance;
        public DropStep() { }
    }
    public sealed class EStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public EStep(System.Collections.Immutable.ImmutableArray<object> ids) { }
        public System.Collections.Immutable.ImmutableArray<object> Ids { get; }
    }
    public sealed class EmitStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.EmitStep Instance;
        public EmitStep() { }
    }
    public sealed class ExplainStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.ExplainStep Instance;
        public ExplainStep() { }
    }
    public sealed class FailStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.FailStep NoMessage;
        public FailStep(string? message = null) { }
        public string? Message { get; }
    }
    public abstract class FilterStep : ExRam.Gremlinq.Core.Steps.Step
    {
        protected FilterStep(ExRam.Gremlinq.Core.Steps.SideEffectSemanticsChange sideEffectSemanticsChange) { }
        public sealed class ByTraversalStep : ExRam.Gremlinq.Core.Steps.Step
        {
            public ByTraversalStep(ExRam.Gremlinq.Core.Traversal traversal) { }
            public ExRam.Gremlinq.Core.Traversal Traversal { get; }
        }
    }
    public sealed class FlatMapStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public FlatMapStep(ExRam.Gremlinq.Core.Traversal traversal) { }
        public ExRam.Gremlinq.Core.Traversal Traversal { get; }
    }
    public sealed class FoldStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.FoldStep Instance;
        public FoldStep() { }
    }
    public sealed class GroupStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.GroupStep Instance;
        public GroupStep() { }
        public sealed class ByKeyStep : ExRam.Gremlinq.Core.Steps.GroupStep.ByStep
        {
            public ByKeyStep(ExRam.Gremlinq.Core.Key key) { }
            public ExRam.Gremlinq.Core.Key Key { get; }
        }
        public abstract class ByStep : ExRam.Gremlinq.Core.Steps.Step
        {
            protected ByStep(ExRam.Gremlinq.Core.Steps.SideEffectSemanticsChange sideEffectSemanticsChange = 0) { }
        }
        public sealed class ByTraversalStep : ExRam.Gremlinq.Core.Steps.GroupStep.ByStep
        {
            public ByTraversalStep(ExRam.Gremlinq.Core.Traversal traversal) { }
            public ExRam.Gremlinq.Core.Traversal Traversal { get; }
        }
    }
    public sealed class HasKeyStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public HasKeyStep(object argument) { }
        public object Argument { get; }
    }
    public sealed class HasLabelStep : ExRam.Gremlinq.Core.Steps.DerivedLabelNamesStep
    {
        public HasLabelStep(System.Collections.Immutable.ImmutableArray<string> labels) { }
    }
    public sealed class HasNotStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public HasNotStep(ExRam.Gremlinq.Core.Key key) { }
        public ExRam.Gremlinq.Core.Key Key { get; }
    }
    public sealed class HasPredicateStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public HasPredicateStep(ExRam.Gremlinq.Core.Key key, Gremlin.Net.Process.Traversal.P predicate) { }
        public ExRam.Gremlinq.Core.Key Key { get; }
        public Gremlin.Net.Process.Traversal.P Predicate { get; }
    }
    public sealed class HasStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public HasStep(ExRam.Gremlinq.Core.Key key) { }
        public ExRam.Gremlinq.Core.Key Key { get; }
    }
    public sealed class HasTraversalStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public HasTraversalStep(ExRam.Gremlinq.Core.Key key, ExRam.Gremlinq.Core.Traversal traversal) { }
        public ExRam.Gremlinq.Core.Key Key { get; }
        public ExRam.Gremlinq.Core.Traversal Traversal { get; }
    }
    public sealed class HasValueStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public HasValueStep(object argument) { }
        public object Argument { get; }
    }
    public interface ISourceStep { }
    public sealed class IdStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.IdStep Instance;
        public IdStep() { }
    }
    public sealed class IdentityStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.IdentityStep Instance;
        public IdentityStep() { }
    }
    public sealed class InEStep : ExRam.Gremlinq.Core.Steps.DerivedLabelNamesStep
    {
        public static readonly ExRam.Gremlinq.Core.Steps.InEStep NoLabels;
        public InEStep() { }
        public InEStep(System.Collections.Immutable.ImmutableArray<string> labels) { }
    }
    public sealed class InStep : ExRam.Gremlinq.Core.Steps.DerivedLabelNamesStep
    {
        public static readonly ExRam.Gremlinq.Core.Steps.InStep NoLabels;
        public InStep() { }
        public InStep(System.Collections.Immutable.ImmutableArray<string> labels) { }
    }
    public sealed class InVStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.InVStep Instance;
        public InVStep() { }
    }
    public sealed class InjectStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public InjectStep(System.Collections.Immutable.ImmutableArray<object> elements) { }
        public System.Collections.Immutable.ImmutableArray<object> Elements { get; }
    }
    public sealed class IsStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public IsStep(Gremlin.Net.Process.Traversal.P predicate) { }
        public Gremlin.Net.Process.Traversal.P Predicate { get; }
    }
    public sealed class KeyStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.KeyStep Instance;
        public KeyStep() { }
    }
    public sealed class LabelStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.LabelStep Instance;
        public LabelStep() { }
    }
    public sealed class LimitStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.LimitStep LimitGlobal1;
        public static readonly ExRam.Gremlinq.Core.Steps.LimitStep LimitLocal1;
        public LimitStep(long count, Gremlin.Net.Process.Traversal.Scope scope) { }
        public long Count { get; }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
    }
    public sealed class LocalStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public LocalStep(ExRam.Gremlinq.Core.Traversal traversal) { }
        public ExRam.Gremlinq.Core.Traversal Traversal { get; }
    }
    public abstract class LogicalStep<TStep> : ExRam.Gremlinq.Core.Steps.Step
        where TStep : ExRam.Gremlinq.Core.Steps.LogicalStep<TStep>
    {
        protected LogicalStep(string name, System.Collections.Generic.IEnumerable<ExRam.Gremlinq.Core.Traversal> traversals) { }
        public string Name { get; }
        public System.Collections.Immutable.ImmutableArray<ExRam.Gremlinq.Core.Traversal> Traversals { get; }
    }
    public sealed class MapStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public MapStep(ExRam.Gremlinq.Core.Traversal traversal) { }
        public ExRam.Gremlinq.Core.Traversal Traversal { get; }
    }
    public sealed class MatchStep : ExRam.Gremlinq.Core.Steps.MultiTraversalArgumentStep
    {
        public MatchStep(System.Collections.Immutable.ImmutableArray<ExRam.Gremlinq.Core.Traversal> traversals) { }
    }
    public sealed class MaxStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.MaxStep Global;
        public static readonly ExRam.Gremlinq.Core.Steps.MaxStep Local;
        public MaxStep(Gremlin.Net.Process.Traversal.Scope scope) { }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
    }
    public sealed class MeanStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.MeanStep Global;
        public static readonly ExRam.Gremlinq.Core.Steps.MeanStep Local;
        public MeanStep(Gremlin.Net.Process.Traversal.Scope scope) { }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
    }
    public sealed class MinStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.MinStep Global;
        public static readonly ExRam.Gremlinq.Core.Steps.MinStep Local;
        public MinStep(Gremlin.Net.Process.Traversal.Scope scope) { }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
    }
    public abstract class MultiTraversalArgumentStep : ExRam.Gremlinq.Core.Steps.Step
    {
        protected MultiTraversalArgumentStep(System.Collections.Immutable.ImmutableArray<ExRam.Gremlinq.Core.Traversal> traversals) { }
        public System.Collections.Immutable.ImmutableArray<ExRam.Gremlinq.Core.Traversal> Traversals { get; }
    }
    public sealed class NoneStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.NoneStep Instance;
        public NoneStep() { }
    }
    public sealed class NotStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public NotStep(ExRam.Gremlinq.Core.Traversal traversal) { }
        public ExRam.Gremlinq.Core.Traversal Traversal { get; }
    }
    public sealed class OptionTraversalStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public OptionTraversalStep(object? guard, ExRam.Gremlinq.Core.Traversal optionTraversal) { }
        public object? Guard { get; }
        public ExRam.Gremlinq.Core.Traversal OptionTraversal { get; }
    }
    public sealed class OptionalStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public OptionalStep(ExRam.Gremlinq.Core.Traversal traversal) { }
        public ExRam.Gremlinq.Core.Traversal Traversal { get; }
    }
    public sealed class OrStep : ExRam.Gremlinq.Core.Steps.LogicalStep<ExRam.Gremlinq.Core.Steps.OrStep>
    {
        public OrStep(System.Collections.Generic.IEnumerable<ExRam.Gremlinq.Core.Traversal> traversals) { }
    }
    public sealed class OrderStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.OrderStep Global;
        public static readonly ExRam.Gremlinq.Core.Steps.OrderStep Local;
        public OrderStep(Gremlin.Net.Process.Traversal.Scope scope) { }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
        public sealed class ByMemberStep : ExRam.Gremlinq.Core.Steps.OrderStep.ByStep
        {
            public ByMemberStep(ExRam.Gremlinq.Core.Key key, Gremlin.Net.Process.Traversal.Order order) { }
            public ExRam.Gremlinq.Core.Key Key { get; }
            public Gremlin.Net.Process.Traversal.Order Order { get; }
        }
        public abstract class ByStep : ExRam.Gremlinq.Core.Steps.Step
        {
            protected ByStep(ExRam.Gremlinq.Core.Steps.SideEffectSemanticsChange sideEffectSemanticsChange = 0) { }
        }
        public sealed class ByTraversalStep : ExRam.Gremlinq.Core.Steps.OrderStep.ByStep
        {
            public ByTraversalStep(ExRam.Gremlinq.Core.Traversal traversal, Gremlin.Net.Process.Traversal.Order order) { }
            public Gremlin.Net.Process.Traversal.Order Order { get; }
            public ExRam.Gremlinq.Core.Traversal Traversal { get; }
        }
    }
    public sealed class OtherVStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.OtherVStep Instance;
        public OtherVStep() { }
    }
    public sealed class OutEStep : ExRam.Gremlinq.Core.Steps.DerivedLabelNamesStep
    {
        public static readonly ExRam.Gremlinq.Core.Steps.OutEStep NoLabels;
        public OutEStep() { }
        public OutEStep(System.Collections.Immutable.ImmutableArray<string> labels) { }
    }
    public sealed class OutStep : ExRam.Gremlinq.Core.Steps.DerivedLabelNamesStep
    {
        public static readonly ExRam.Gremlinq.Core.Steps.OutStep NoLabels;
        public OutStep() { }
        public OutStep(System.Collections.Immutable.ImmutableArray<string> labels) { }
    }
    public sealed class OutVStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.OutVStep Instance;
        public OutVStep() { }
    }
    public sealed class PathStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.PathStep Instance;
        public PathStep() { }
    }
    public sealed class ProfileStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.ProfileStep Instance;
        public ProfileStep() { }
    }
    public sealed class ProjectStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public ProjectStep(System.Collections.Immutable.ImmutableArray<string> projections) { }
        public System.Collections.Immutable.ImmutableArray<string> Projections { get; }
        public sealed class ByKeyStep : ExRam.Gremlinq.Core.Steps.ProjectStep.ByStep
        {
            public ByKeyStep(ExRam.Gremlinq.Core.Key key) { }
            public ExRam.Gremlinq.Core.Key Key { get; }
            public override ExRam.Gremlinq.Core.Steps.ProjectStep.ByTraversalStep ToByTraversalStep() { }
        }
        public abstract class ByStep : ExRam.Gremlinq.Core.Steps.Step
        {
            protected ByStep(ExRam.Gremlinq.Core.Steps.SideEffectSemanticsChange sideEffectSemanticsChange = 0) { }
            public abstract ExRam.Gremlinq.Core.Steps.ProjectStep.ByTraversalStep ToByTraversalStep();
        }
        public sealed class ByTraversalStep : ExRam.Gremlinq.Core.Steps.ProjectStep.ByStep
        {
            public ByTraversalStep(ExRam.Gremlinq.Core.Traversal traversal) { }
            public ExRam.Gremlinq.Core.Traversal Traversal { get; }
            public override ExRam.Gremlinq.Core.Steps.ProjectStep.ByTraversalStep ToByTraversalStep() { }
        }
    }
    public sealed class PropertiesStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public PropertiesStep(System.Collections.Immutable.ImmutableArray<string> keys) { }
        public System.Collections.Immutable.ImmutableArray<string> Keys { get; }
    }
    public abstract class PropertyStep : ExRam.Gremlinq.Core.Steps.Step
    {
        protected PropertyStep(object value, Gremlin.Net.Process.Traversal.Cardinality? cardinality = null) { }
        protected PropertyStep(object value, System.Collections.Immutable.ImmutableArray<System.Collections.Generic.KeyValuePair<string, object>> metaProperties, Gremlin.Net.Process.Traversal.Cardinality? cardinality = null) { }
        public Gremlin.Net.Process.Traversal.Cardinality? Cardinality { get; }
        public System.Collections.Immutable.ImmutableArray<System.Collections.Generic.KeyValuePair<string, object>> MetaProperties { get; }
        public object Value { get; }
        public sealed class ByKeyStep : ExRam.Gremlinq.Core.Steps.PropertyStep
        {
            public ByKeyStep(ExRam.Gremlinq.Core.Key key, object value, Gremlin.Net.Process.Traversal.Cardinality? cardinality = null) { }
            public ByKeyStep(ExRam.Gremlinq.Core.Key key, object value, System.Collections.Immutable.ImmutableArray<System.Collections.Generic.KeyValuePair<string, object>> metaProperties, Gremlin.Net.Process.Traversal.Cardinality? cardinality = null) { }
            public ExRam.Gremlinq.Core.Key Key { get; }
        }
        public sealed class ByTraversalStep : ExRam.Gremlinq.Core.Steps.PropertyStep
        {
            public ByTraversalStep(ExRam.Gremlinq.Core.Traversal traversal, object value, Gremlin.Net.Process.Traversal.Cardinality? cardinality = null) { }
            public ByTraversalStep(ExRam.Gremlinq.Core.Traversal traversal, object value, System.Collections.Immutable.ImmutableArray<System.Collections.Generic.KeyValuePair<string, object>> metaProperties, Gremlin.Net.Process.Traversal.Cardinality? cardinality = null) { }
            public ExRam.Gremlinq.Core.Traversal Traversal { get; }
        }
    }
    public sealed class RangeStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public RangeStep(long lower, long upper, Gremlin.Net.Process.Traversal.Scope scope) { }
        public long Lower { get; }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
        public long Upper { get; }
    }
    public sealed class RepeatStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public RepeatStep(ExRam.Gremlinq.Core.Traversal traversal) { }
        public ExRam.Gremlinq.Core.Traversal Traversal { get; }
    }
    public sealed class SelectColumnStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public SelectColumnStep(Gremlin.Net.Process.Traversal.Column column) { }
        public Gremlin.Net.Process.Traversal.Column Column { get; }
    }
    public sealed class SelectKeysStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public SelectKeysStep(ExRam.Gremlinq.Core.Key key) { }
        public SelectKeysStep(System.Collections.Immutable.ImmutableArray<ExRam.Gremlinq.Core.Key> keys) { }
        public System.Collections.Immutable.ImmutableArray<ExRam.Gremlinq.Core.Key> Keys { get; }
    }
    public sealed class SelectStepLabelStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public SelectStepLabelStep(ExRam.Gremlinq.Core.StepLabel stepLabel) { }
        public SelectStepLabelStep(System.Collections.Immutable.ImmutableArray<ExRam.Gremlinq.Core.StepLabel> stepLabels) { }
        public System.Collections.Immutable.ImmutableArray<ExRam.Gremlinq.Core.StepLabel> StepLabels { get; }
    }
    public enum SideEffectSemantics
    {
        Read = 0,
        Write = 1,
    }
    public enum SideEffectSemanticsChange
    {
        None = 0,
        Write = 1,
    }
    public sealed class SideEffectStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public SideEffectStep(ExRam.Gremlinq.Core.Traversal traversal) { }
        public ExRam.Gremlinq.Core.Traversal Traversal { get; }
    }
    public sealed class SimplePathStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.SimplePathStep Instance;
        public SimplePathStep() { }
    }
    public sealed class SkipStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public SkipStep(long count, Gremlin.Net.Process.Traversal.Scope scope) { }
        public long Count { get; }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
    }
    public abstract class Step
    {
        protected Step(ExRam.Gremlinq.Core.Steps.SideEffectSemanticsChange sideEffectSemanticsChange = 0) { }
        public ExRam.Gremlinq.Core.Steps.SideEffectSemanticsChange SideEffectSemanticsChange { get; }
    }
    public sealed class SumStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.SumStep Global;
        public static readonly ExRam.Gremlinq.Core.Steps.SumStep Local;
        public SumStep(Gremlin.Net.Process.Traversal.Scope scope) { }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
    }
    public sealed class TailStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.TailStep TailGlobal1;
        public static readonly ExRam.Gremlinq.Core.Steps.TailStep TailLocal1;
        public TailStep(long count, Gremlin.Net.Process.Traversal.Scope scope) { }
        public long Count { get; }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
    }
    public sealed class TimesStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public TimesStep(int count) { }
        public int Count { get; }
    }
    public sealed class UnfoldStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.UnfoldStep Instance;
        public UnfoldStep() { }
    }
    public sealed class UnionStep : ExRam.Gremlinq.Core.Steps.MultiTraversalArgumentStep
    {
        public UnionStep(System.Collections.Immutable.ImmutableArray<ExRam.Gremlinq.Core.Traversal> traversals) { }
    }
    public sealed class UntilStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public UntilStep(ExRam.Gremlinq.Core.Traversal traversal) { }
        public ExRam.Gremlinq.Core.Traversal Traversal { get; }
    }
    public sealed class VStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public VStep(System.Collections.Immutable.ImmutableArray<object> ids) { }
        public System.Collections.Immutable.ImmutableArray<object> Ids { get; }
    }
    public sealed class ValueMapStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public ValueMapStep(System.Collections.Immutable.ImmutableArray<string> keys) { }
        public System.Collections.Immutable.ImmutableArray<string> Keys { get; }
    }
    public sealed class ValueStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public static readonly ExRam.Gremlinq.Core.Steps.ValueStep Instance;
        public ValueStep() { }
    }
    public sealed class ValuesStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public ValuesStep(System.Collections.Immutable.ImmutableArray<string> keys) { }
        public System.Collections.Immutable.ImmutableArray<string> Keys { get; }
    }
    public sealed class WherePredicateStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public WherePredicateStep(Gremlin.Net.Process.Traversal.P predicate) { }
        public Gremlin.Net.Process.Traversal.P Predicate { get; }
        public sealed class ByMemberStep : ExRam.Gremlinq.Core.Steps.Step
        {
            public ByMemberStep(ExRam.Gremlinq.Core.Key? key = default) { }
            public ExRam.Gremlinq.Core.Key? Key { get; }
        }
    }
    public sealed class WhereStepLabelAndPredicateStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public WhereStepLabelAndPredicateStep(ExRam.Gremlinq.Core.StepLabel stepLabel, Gremlin.Net.Process.Traversal.P predicate) { }
        public Gremlin.Net.Process.Traversal.P Predicate { get; }
        public ExRam.Gremlinq.Core.StepLabel StepLabel { get; }
    }
    public sealed class WhereTraversalStep : ExRam.Gremlinq.Core.Steps.Step
    {
        public WhereTraversalStep(ExRam.Gremlinq.Core.Traversal traversal) { }
        public ExRam.Gremlinq.Core.Traversal Traversal { get; }
    }
    public sealed class WithSideEffectStep : ExRam.Gremlinq.Core.Steps.Step, ExRam.Gremlinq.Core.Steps.ISourceStep
    {
        public WithSideEffectStep(ExRam.Gremlinq.Core.StepLabel label, object value) { }
        public ExRam.Gremlinq.Core.StepLabel Label { get; }
        public object Value { get; }
    }
}
namespace ExRam.Gremlinq.Core.Transformation
{
    public static class ConverterFactory
    {
        public static ExRam.Gremlinq.Core.Transformation.IConverterFactory Create<TStaticSource, TStaticTarget>(System.Func<TStaticSource, ExRam.Gremlinq.Core.IGremlinQueryEnvironment, ExRam.Gremlinq.Core.Transformation.ITransformer, ExRam.Gremlinq.Core.Transformation.ITransformer, TStaticTarget?> func)
            where TStaticTarget :  struct { }
        public static ExRam.Gremlinq.Core.Transformation.IConverterFactory Create<TStaticSource, TStaticTarget>(System.Func<TStaticSource, ExRam.Gremlinq.Core.IGremlinQueryEnvironment, ExRam.Gremlinq.Core.Transformation.ITransformer, ExRam.Gremlinq.Core.Transformation.ITransformer, TStaticTarget?> func)
            where TStaticTarget :  class { }
    }
    public interface IConverterFactory
    {
        ExRam.Gremlinq.Core.Transformation.IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment);
    }
    public interface IConverter<in TSource, TTarget>
    {
        bool TryConvert(TSource source, ExRam.Gremlinq.Core.Transformation.ITransformer defer, ExRam.Gremlinq.Core.Transformation.ITransformer recurse, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out TTarget? value);
    }
    public interface ITransformer
    {
        ExRam.Gremlinq.Core.Transformation.ITransformer Add(ExRam.Gremlinq.Core.Transformation.IConverterFactory converterFactory);
        bool TryTransform<TSource, TTarget>(TSource source, ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out TTarget? value);
    }
    public static class Transformer
    {
        public static readonly ExRam.Gremlinq.Core.Transformation.ITransformer Empty;
    }
}