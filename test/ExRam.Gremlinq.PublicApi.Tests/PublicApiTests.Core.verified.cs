namespace ExRam.Gremlinq.Core
{
    public sealed class AddEStep : ExRam.Gremlinq.Core.Step
    {
        public AddEStep(string label) { }
        public string Label { get; }
        public sealed class FromLabelStep : ExRam.Gremlinq.Core.Step
        {
            public FromLabelStep(ExRam.Gremlinq.Core.StepLabel stepLabel) { }
            public ExRam.Gremlinq.Core.StepLabel StepLabel { get; }
        }
        public sealed class FromTraversalStep : ExRam.Gremlinq.Core.Step
        {
            public FromTraversalStep(ExRam.Gremlinq.Core.Traversal traversal) { }
            public ExRam.Gremlinq.Core.Traversal Traversal { get; }
        }
        public sealed class ToLabelStep : ExRam.Gremlinq.Core.Step
        {
            public ToLabelStep(ExRam.Gremlinq.Core.StepLabel stepLabel) { }
            public ExRam.Gremlinq.Core.StepLabel StepLabel { get; }
        }
        public sealed class ToTraversalStep : ExRam.Gremlinq.Core.Step
        {
            public ToTraversalStep(ExRam.Gremlinq.Core.Traversal traversal) { }
            public ExRam.Gremlinq.Core.Traversal Traversal { get; }
        }
    }
    public static class AddStepHandler
    {
        public static readonly ExRam.Gremlinq.Core.IAddStepHandler Default;
        public static readonly ExRam.Gremlinq.Core.IAddStepHandler Empty;
    }
    public sealed class AddVStep : ExRam.Gremlinq.Core.Step
    {
        public AddVStep(string label) { }
        public string Label { get; }
    }
    public sealed class AggregateStep : ExRam.Gremlinq.Core.Step
    {
        public AggregateStep(Gremlin.Net.Process.Traversal.Scope scope, ExRam.Gremlinq.Core.StepLabel stepLabel) { }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
        public ExRam.Gremlinq.Core.StepLabel StepLabel { get; }
    }
    public sealed class AndStep : ExRam.Gremlinq.Core.LogicalStep<ExRam.Gremlinq.Core.AndStep>
    {
        public static readonly ExRam.Gremlinq.Core.AndStep Infix;
        public AndStep(System.Collections.Generic.IEnumerable<ExRam.Gremlinq.Core.Traversal> traversals) { }
    }
    public sealed class AsStep : ExRam.Gremlinq.Core.Step
    {
        public AsStep(ExRam.Gremlinq.Core.StepLabel stepLabel) { }
        public ExRam.Gremlinq.Core.StepLabel StepLabel { get; }
    }
    public sealed class BarrierStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.BarrierStep Instance;
        public BarrierStep() { }
    }
    public sealed class BothEStep : ExRam.Gremlinq.Core.DerivedLabelNamesStep
    {
        public static readonly ExRam.Gremlinq.Core.BothEStep NoLabels;
        public BothEStep(System.Collections.Immutable.ImmutableArray<string> labels) { }
    }
    public sealed class BothStep : ExRam.Gremlinq.Core.DerivedLabelNamesStep
    {
        public static readonly ExRam.Gremlinq.Core.BothStep NoLabels;
        public BothStep(System.Collections.Immutable.ImmutableArray<string> labels) { }
    }
    public sealed class BothVStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.BothVStep Instance;
        public BothVStep() { }
    }
    public static class BytecodeExtensions
    {
        public static ExRam.Gremlinq.Core.GroovyGremlinQuery ToGroovy(this Gremlin.Net.Process.Traversal.Bytecode bytecode, ExRam.Gremlinq.Core.GroovyFormatting formatting = 0) { }
    }
    public sealed class CapStep : ExRam.Gremlinq.Core.Step
    {
        public CapStep(ExRam.Gremlinq.Core.StepLabel stepLabel) { }
        public ExRam.Gremlinq.Core.StepLabel StepLabel { get; }
    }
    public sealed class ChooseOptionTraversalStep : ExRam.Gremlinq.Core.Step
    {
        public ChooseOptionTraversalStep(ExRam.Gremlinq.Core.Traversal traversal) { }
        public ExRam.Gremlinq.Core.Traversal Traversal { get; }
    }
    public sealed class ChoosePredicateStep : ExRam.Gremlinq.Core.ChooseStep
    {
        public ChoosePredicateStep(Gremlin.Net.Process.Traversal.P predicate, ExRam.Gremlinq.Core.Traversal thenTraversal, ExRam.Gremlinq.Core.Traversal? elseTraversal = default) { }
        public Gremlin.Net.Process.Traversal.P Predicate { get; }
    }
    public abstract class ChooseStep : ExRam.Gremlinq.Core.Step
    {
        protected ChooseStep(ExRam.Gremlinq.Core.Traversal thenTraversal, ExRam.Gremlinq.Core.Traversal? elseTraversal = default) { }
        public ExRam.Gremlinq.Core.Traversal? ElseTraversal { get; }
        public ExRam.Gremlinq.Core.Traversal ThenTraversal { get; }
    }
    public sealed class ChooseTraversalStep : ExRam.Gremlinq.Core.ChooseStep
    {
        public ChooseTraversalStep(ExRam.Gremlinq.Core.Traversal ifTraversal, ExRam.Gremlinq.Core.Traversal thenTraversal, ExRam.Gremlinq.Core.Traversal? elseTraversal = default) { }
        public ExRam.Gremlinq.Core.Traversal IfTraversal { get; }
    }
    public sealed class CoalesceStep : ExRam.Gremlinq.Core.MultiTraversalArgumentStep
    {
        public CoalesceStep(System.Collections.Immutable.ImmutableArray<ExRam.Gremlinq.Core.Traversal> traversals) { }
    }
    public sealed class CoinStep : ExRam.Gremlinq.Core.Step
    {
        public CoinStep(double probability) { }
        public double Probability { get; }
    }
    public sealed class ConstantStep : ExRam.Gremlinq.Core.Step
    {
        public ConstantStep(object value) { }
        public object Value { get; }
    }
    public sealed class CountStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.CountStep Global;
        public static readonly ExRam.Gremlinq.Core.CountStep Local;
        public CountStep(Gremlin.Net.Process.Traversal.Scope scope) { }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
    }
    public sealed class CyclicPathStep : ExRam.Gremlinq.Core.Step
    {
        public CyclicPathStep() { }
    }
    public sealed class DedupStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.DedupStep Global;
        public static readonly ExRam.Gremlinq.Core.DedupStep Local;
        public DedupStep(Gremlin.Net.Process.Traversal.Scope scope) { }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
    }
    public abstract class DerivedLabelNamesStep : ExRam.Gremlinq.Core.Step
    {
        protected DerivedLabelNamesStep(System.Collections.Immutable.ImmutableArray<string> labels) { }
        public System.Collections.Immutable.ImmutableArray<string> Labels { get; }
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
    }
    public sealed class DropStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.DropStep Instance;
        public DropStep() { }
    }
    public sealed class EStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.EStep Empty;
        public EStep(System.Collections.Immutable.ImmutableArray<object> ids) { }
        public System.Collections.Immutable.ImmutableArray<object> Ids { get; }
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
    public readonly struct ElementMetadata
    {
        public ElementMetadata(string label) { }
        public string Label { get; }
    }
    public sealed class EmitStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.EmitStep Instance;
        public EmitStep() { }
    }
    public sealed class ExplainStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.ExplainStep Instance;
        public ExplainStep() { }
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
    public sealed class FilterStep : ExRam.Gremlinq.Core.Step
    {
        public FilterStep(Gremlin.Net.Process.Traversal.ILambda lambda) { }
        public Gremlin.Net.Process.Traversal.ILambda Lambda { get; }
    }
    public sealed class FlatMapStep : ExRam.Gremlinq.Core.Step
    {
        public FlatMapStep(ExRam.Gremlinq.Core.Traversal traversal) { }
        public ExRam.Gremlinq.Core.Traversal Traversal { get; }
    }
    public sealed class FoldStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.FoldStep Instance;
        public FoldStep() { }
    }
    public static class GraphElementModel
    {
        public static readonly ExRam.Gremlinq.Core.IGraphElementModel Empty;
        public static readonly ExRam.Gremlinq.Core.IGraphElementModel Invalid;
        public static ExRam.Gremlinq.Core.IGraphElementModel ConfigureLabels(this ExRam.Gremlinq.Core.IGraphElementModel model, System.Func<System.Type, string, string> overrideTransformation) { }
        public static ExRam.Gremlinq.Core.IGraphElementModel FromBaseType(System.Type baseType, System.Collections.Generic.IEnumerable<System.Reflection.Assembly>? assemblies) { }
        public static ExRam.Gremlinq.Core.IGraphElementModel FromBaseType<TType>(System.Collections.Generic.IEnumerable<System.Reflection.Assembly>? assemblies) { }
        public static ExRam.Gremlinq.Core.IGraphElementModel FromTypes(System.Collections.Generic.IEnumerable<System.Type> types) { }
        public static System.Collections.Immutable.ImmutableArray<string>? TryGetFilterLabels(this ExRam.Gremlinq.Core.IGraphElementModel model, System.Type type, ExRam.Gremlinq.Core.FilterLabelsVerbosity verbosity) { }
        public static ExRam.Gremlinq.Core.IGraphElementModel UseCamelCaseLabels(this ExRam.Gremlinq.Core.IGraphElementModel model) { }
        public static ExRam.Gremlinq.Core.IGraphElementModel UseLowerCaseLabels(this ExRam.Gremlinq.Core.IGraphElementModel model) { }
    }
    public static class GraphElementPropertyModel
    {
        public static readonly ExRam.Gremlinq.Core.IGraphElementPropertyModel Empty;
        public static readonly ExRam.Gremlinq.Core.IGraphElementPropertyModel Invalid;
        public static ExRam.Gremlinq.Core.IGraphElementPropertyModel ConfigureElement<TElement>(this ExRam.Gremlinq.Core.IGraphElementPropertyModel model, System.Func<ExRam.Gremlinq.Core.IMemberMetadataConfigurator<TElement>, System.Collections.Immutable.IImmutableDictionary<System.Reflection.MemberInfo, ExRam.Gremlinq.Core.MemberMetadata>> transformation) { }
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
    public static class GraphModel
    {
        public static readonly ExRam.Gremlinq.Core.IGraphModel Empty;
        public static readonly ExRam.Gremlinq.Core.IGraphModel Invalid;
        public static ExRam.Gremlinq.Core.IGraphModel ConfigureElements(this ExRam.Gremlinq.Core.IGraphModel model, System.Func<ExRam.Gremlinq.Core.IGraphElementModel, ExRam.Gremlinq.Core.IGraphElementModel> transformation) { }
        public static ExRam.Gremlinq.Core.IGraphModel FromBaseTypes(System.Type vertexBaseType, System.Type edgeBaseType, System.Func<ExRam.Gremlinq.Core.IAssemblyLookupBuilder, ExRam.Gremlinq.Core.IAssemblyLookupSet> assemblyLookupTransformation) { }
        public static ExRam.Gremlinq.Core.IGraphModel FromBaseTypes<TVertex, TEdge>(System.Func<ExRam.Gremlinq.Core.IAssemblyLookupBuilder, ExRam.Gremlinq.Core.IAssemblyLookupSet>? assemblyLookupTransformation = null) { }
        public static ExRam.Gremlinq.Core.IGraphModel FromTypes(System.Type[] vertexTypes, System.Type[] edgeTypes) { }
    }
    public static class GremlinQueryAdmin
    {
        public static TTargetQuery AddSteps<TTargetQuery>(this ExRam.Gremlinq.Core.IGremlinQueryAdmin admin, System.Collections.Generic.IEnumerable<ExRam.Gremlinq.Core.Step> steps)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase { }
    }
    public struct GremlinQueryAwaiter<TElement> : System.Runtime.CompilerServices.ICriticalNotifyCompletion, System.Runtime.CompilerServices.INotifyCompletion
    {
        public bool IsCompleted { get; }
        public TElement[] GetResult() { }
        public void OnCompleted(System.Action continuation) { }
        public void UnsafeOnCompleted(System.Action continuation) { }
    }
    public static class GremlinQueryEnvironment
    {
        public static readonly ExRam.Gremlinq.Core.IGremlinQueryEnvironment Default;
        public static readonly ExRam.Gremlinq.Core.IGremlinQueryEnvironment Empty;
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment EchoGraphsonString(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment) { }
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment EchoGroovyString(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment) { }
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment StoreByteArraysAsBase64String(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment) { }
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment StoreTimeSpansAsNumbers(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment) { }
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment UseAddStepHandler(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment source, ExRam.Gremlinq.Core.IAddStepHandler addStepHandler) { }
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment UseDeserializer(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment, ExRam.Gremlinq.Core.IGremlinQueryExecutionResultDeserializer deserializer) { }
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment UseExecutor(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment, ExRam.Gremlinq.Core.IGremlinQueryExecutor executor) { }
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment UseLogger(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment source, Microsoft.Extensions.Logging.ILogger logger) { }
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment UseModel(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment source, ExRam.Gremlinq.Core.IGraphModel model) { }
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment UseSerializer(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment, ExRam.Gremlinq.Core.IGremlinQuerySerializer serializer) { }
    }
    public static class GremlinQueryExecutionResultDeserializer
    {
        public static readonly ExRam.Gremlinq.Core.IGremlinQueryExecutionResultDeserializer Default;
        public static readonly ExRam.Gremlinq.Core.IGremlinQueryExecutionResultDeserializer Identity;
        public static readonly ExRam.Gremlinq.Core.IGremlinQueryExecutionResultDeserializer Invalid;
    }
    public static class GremlinQueryExecutor
    {
        public static readonly ExRam.Gremlinq.Core.IGremlinQueryExecutor Empty;
        public static readonly ExRam.Gremlinq.Core.IGremlinQueryExecutor Identity;
        public static readonly ExRam.Gremlinq.Core.IGremlinQueryExecutor Invalid;
        public static ExRam.Gremlinq.Core.IGremlinQueryExecutor Create(System.Func<object, ExRam.Gremlinq.Core.IGremlinQueryEnvironment, System.Collections.Generic.IAsyncEnumerable<object>> executor) { }
        public static ExRam.Gremlinq.Core.IGremlinQueryExecutor TransformQuery(this ExRam.Gremlinq.Core.IGremlinQueryExecutor baseExecutor, System.Func<object, object> transformation) { }
        public static ExRam.Gremlinq.Core.IGremlinQueryExecutor TransformResult(this ExRam.Gremlinq.Core.IGremlinQueryExecutor baseExecutor, System.Func<System.Collections.Generic.IAsyncEnumerable<object>, System.Collections.Generic.IAsyncEnumerable<object>> transformation) { }
    }
    public static class GremlinQueryExecutorExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQueryExecutor RetryWithExponentialBackoff(this ExRam.Gremlinq.Core.IGremlinQueryExecutor executor, System.Func<int, Gremlin.Net.Driver.Exceptions.ResponseException, bool> shouldRetry) { }
    }
    public static class GremlinQueryExtensions
    {
        public static System.Threading.Tasks.ValueTask<TElement> FirstAsync<TElement>(this ExRam.Gremlinq.Core.IGremlinQueryBase<TElement> query, System.Threading.CancellationToken ct = default) { }
        public static System.Threading.Tasks.ValueTask<TElement?> FirstOrDefaultAsync<TElement>(this ExRam.Gremlinq.Core.IGremlinQueryBase<TElement> query, System.Threading.CancellationToken ct = default) { }
        public static System.Threading.Tasks.ValueTask<TElement> SingleAsync<TElement>(this ExRam.Gremlinq.Core.IGremlinQueryBase<TElement> query, System.Threading.CancellationToken ct = default) { }
        public static System.Threading.Tasks.ValueTask<TElement?> SingleOrDefaultAsync<TElement>(this ExRam.Gremlinq.Core.IGremlinQueryBase<TElement> query, System.Threading.CancellationToken ct = default) { }
        public static System.Threading.Tasks.ValueTask<TElement[]> ToArrayAsync<TElement>(this ExRam.Gremlinq.Core.IGremlinQueryBase<TElement> query, System.Threading.CancellationToken ct = default) { }
    }
    public static class GremlinQueryFragmentDeserializer
    {
        public static readonly ExRam.Gremlinq.Core.IGremlinQueryFragmentDeserializer Identity;
        public static ExRam.Gremlinq.Core.IGremlinQueryFragmentDeserializer AddNewtonsoftJson(this ExRam.Gremlinq.Core.IGremlinQueryFragmentDeserializer deserializer) { }
        public static ExRam.Gremlinq.Core.IGremlinQueryFragmentDeserializer AddToStringFallback(this ExRam.Gremlinq.Core.IGremlinQueryFragmentDeserializer deserializer) { }
    }
    public delegate object? GremlinQueryFragmentDeserializerDelegate<TSerialized>(TSerialized serializedData, System.Type requestedType, ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment, System.Func<TSerialized, System.Type, ExRam.Gremlinq.Core.IGremlinQueryEnvironment, ExRam.Gremlinq.Core.IGremlinQueryFragmentDeserializer, object?> overridden, ExRam.Gremlinq.Core.IGremlinQueryFragmentDeserializer recurse);
    public static class GremlinQueryFragmentSerializer
    {
        public static readonly ExRam.Gremlinq.Core.IGremlinQueryFragmentSerializer Default;
        public static readonly ExRam.Gremlinq.Core.IGremlinQueryFragmentSerializer Identity;
        public static ExRam.Gremlinq.Core.IGremlinQueryFragmentSerializer UseDefaultGremlinStepSerializationHandlers(this ExRam.Gremlinq.Core.IGremlinQueryFragmentSerializer fragmentSerializer) { }
    }
    public delegate object GremlinQueryFragmentSerializerDelegate<TFragment>(TFragment fragment, ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment, System.Func<TFragment, ExRam.Gremlinq.Core.IGremlinQueryEnvironment, ExRam.Gremlinq.Core.IGremlinQueryFragmentSerializer, object> overridden, ExRam.Gremlinq.Core.IGremlinQueryFragmentSerializer recurse);
    public static class GremlinQuerySerializer
    {
        public static readonly ExRam.Gremlinq.Core.IGremlinQuerySerializer Default;
        public static readonly ExRam.Gremlinq.Core.IGremlinQuerySerializer Identity;
        public static readonly ExRam.Gremlinq.Core.IGremlinQuerySerializer Invalid;
        public static ExRam.Gremlinq.Core.IGremlinQuerySerializer Select(this ExRam.Gremlinq.Core.IGremlinQuerySerializer serializer, System.Func<object, object> projection) { }
        public static ExRam.Gremlinq.Core.IGremlinQuerySerializer ToGroovy(this ExRam.Gremlinq.Core.IGremlinQuerySerializer serializer, ExRam.Gremlinq.Core.GroovyFormatting formatting = 0) { }
    }
    public static class GremlinQuerySource
    {
        public static readonly ExRam.Gremlinq.Core.IConfigurableGremlinQuerySource g;
    }
    public static class GremlinqOption
    {
        public static readonly ExRam.Gremlinq.Core.GremlinqOption<ExRam.Gremlinq.Core.DisabledTextPredicates> DisabledTextPredicates;
        public static readonly ExRam.Gremlinq.Core.GremlinqOption<System.Collections.Immutable.ImmutableArray<ExRam.Gremlinq.Core.Step>> EdgeProjectionSteps;
        public static readonly ExRam.Gremlinq.Core.GremlinqOption<ExRam.Gremlinq.Core.FilterLabelsVerbosity> FilterLabelsVerbosity;
        public static readonly ExRam.Gremlinq.Core.GremlinqOption<ExRam.Gremlinq.Core.StringComparisonTranslationStrictness> StringComparisonTranslationStrictness;
        public static readonly ExRam.Gremlinq.Core.GremlinqOption<System.Collections.Immutable.IImmutableDictionary<Gremlin.Net.Process.Traversal.T, ExRam.Gremlinq.Core.SerializationBehaviour>> TSerializationBehaviourOverrides;
        public static readonly ExRam.Gremlinq.Core.GremlinqOption<System.Collections.Immutable.ImmutableArray<ExRam.Gremlinq.Core.Step>> VertexProjectionSteps;
        public static readonly ExRam.Gremlinq.Core.GremlinqOption<System.Collections.Immutable.ImmutableArray<ExRam.Gremlinq.Core.Step>> VertexProjectionWithoutMetaPropertiesSteps;
    }
    public class GremlinqOption<TValue> : ExRam.Gremlinq.Core.IGremlinqOption
    {
        public GremlinqOption(TValue defaultValue) { }
        public TValue DefaultValue { get; }
    }
    public static class GremlinqOptions
    {
        public static readonly ExRam.Gremlinq.Core.IGremlinqOptions Empty;
    }
    public enum GroovyFormatting
    {
        WithBindings = 0,
        Inline = 1,
    }
    public sealed class GroovyGremlinQuery
    {
        public GroovyGremlinQuery(string script, System.Collections.Generic.IReadOnlyDictionary<string, object> bindings) { }
        public System.Collections.Generic.IReadOnlyDictionary<string, object> Bindings { get; }
        public string Script { get; }
        public ExRam.Gremlinq.Core.GroovyGremlinQuery Inline() { }
        public override string ToString() { }
    }
    public sealed class GroupStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.GroupStep Instance;
        public GroupStep() { }
        public sealed class ByKeyStep : ExRam.Gremlinq.Core.GroupStep.ByStep
        {
            public ByKeyStep(ExRam.Gremlinq.Core.Key key) { }
            public ExRam.Gremlinq.Core.Key Key { get; }
        }
        public abstract class ByStep : ExRam.Gremlinq.Core.Step
        {
            protected ByStep() { }
        }
        public sealed class ByTraversalStep : ExRam.Gremlinq.Core.GroupStep.ByStep
        {
            public ByTraversalStep(ExRam.Gremlinq.Core.Traversal traversal) { }
            public ExRam.Gremlinq.Core.Traversal Traversal { get; }
        }
    }
    public sealed class HasKeyStep : ExRam.Gremlinq.Core.Step
    {
        public HasKeyStep(object argument) { }
        public object Argument { get; }
    }
    public sealed class HasLabelStep : ExRam.Gremlinq.Core.DerivedLabelNamesStep
    {
        public HasLabelStep(System.Collections.Immutable.ImmutableArray<string> labels) { }
    }
    public sealed class HasNotStep : ExRam.Gremlinq.Core.Step
    {
        public HasNotStep(ExRam.Gremlinq.Core.Key key) { }
        public ExRam.Gremlinq.Core.Key Key { get; }
    }
    public sealed class HasPredicateStep : ExRam.Gremlinq.Core.Step
    {
        public HasPredicateStep(ExRam.Gremlinq.Core.Key key, Gremlin.Net.Process.Traversal.P predicate) { }
        public ExRam.Gremlinq.Core.Key Key { get; }
        public Gremlin.Net.Process.Traversal.P Predicate { get; }
    }
    public sealed class HasTraversalStep : ExRam.Gremlinq.Core.Step
    {
        public HasTraversalStep(ExRam.Gremlinq.Core.Key key, ExRam.Gremlinq.Core.Traversal traversal) { }
        public ExRam.Gremlinq.Core.Key Key { get; }
        public ExRam.Gremlinq.Core.Traversal Traversal { get; }
    }
    public sealed class HasValueStep : ExRam.Gremlinq.Core.Step
    {
        public HasValueStep(object argument) { }
        public object Argument { get; }
    }
    public interface IAddStepHandler
    {
        ExRam.Gremlinq.Core.StepStack AddStep<TStep>(ExRam.Gremlinq.Core.StepStack steps, TStep step, ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment)
            where TStep : ExRam.Gremlinq.Core.Step;
        ExRam.Gremlinq.Core.IAddStepHandler Override<TStep>(System.Func<ExRam.Gremlinq.Core.StepStack, TStep, ExRam.Gremlinq.Core.IGremlinQueryEnvironment, System.Func<ExRam.Gremlinq.Core.StepStack, TStep, ExRam.Gremlinq.Core.IGremlinQueryEnvironment, ExRam.Gremlinq.Core.IAddStepHandler, ExRam.Gremlinq.Core.StepStack>, ExRam.Gremlinq.Core.IAddStepHandler, ExRam.Gremlinq.Core.StepStack> addStepHandler)
            where TStep : ExRam.Gremlinq.Core.Step;
    }
    public interface IArrayGremlinQueryBase : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IValueGremlinQuery<object[]> Lower();
        ExRam.Gremlinq.Core.IValueGremlinQuery<object> Unfold();
    }
    public interface IArrayGremlinQueryBase<TArrayItem> : ExRam.Gremlinq.Core.IArrayGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IGremlinQuery<TArrayItem[]> Lower();
        ExRam.Gremlinq.Core.IValueGremlinQuery<TArrayItem> Unfold();
    }
    public interface IArrayGremlinQueryBase<TArray, TArrayItem> : ExRam.Gremlinq.Core.IArrayGremlinQueryBase, ExRam.Gremlinq.Core.IArrayGremlinQueryBase<TArrayItem>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TArray>, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        new ExRam.Gremlinq.Core.IGremlinQuery<TArray> Lower();
    }
    public interface IArrayGremlinQueryBase<TArray, TArrayItem, out TOriginalQuery> : ExRam.Gremlinq.Core.IArrayGremlinQueryBase, ExRam.Gremlinq.Core.IArrayGremlinQueryBase<TArrayItem>, ExRam.Gremlinq.Core.IArrayGremlinQueryBase<TArray, TArrayItem>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TArray>, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        new ExRam.Gremlinq.Core.IGremlinQuery<TArray> Lower();
        TOriginalQuery MaxLocal();
        TOriginalQuery MeanLocal();
        TOriginalQuery MinLocal();
        TOriginalQuery SumLocal();
        TOriginalQuery Unfold();
    }
    public interface IArrayGremlinQuery<TArray, TArrayItem, TOriginalQuery> : ExRam.Gremlinq.Core.IArrayGremlinQueryBase, ExRam.Gremlinq.Core.IArrayGremlinQueryBase<TArrayItem>, ExRam.Gremlinq.Core.IArrayGremlinQueryBase<TArray, TArrayItem>, ExRam.Gremlinq.Core.IArrayGremlinQueryBase<TArray, TArrayItem, TOriginalQuery>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IArrayGremlinQuery<TArray, TArrayItem, TOriginalQuery>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TArray, ExRam.Gremlinq.Core.IArrayGremlinQuery<TArray, TArrayItem, TOriginalQuery>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TArray>, ExRam.Gremlinq.Core.IStartGremlinQuery { }
    public interface IAssemblyLookupBuilder
    {
        ExRam.Gremlinq.Core.IAssemblyLookupSet IncludeAssemblies(System.Collections.Generic.IEnumerable<System.Reflection.Assembly> assemblies);
        ExRam.Gremlinq.Core.IAssemblyLookupSet IncludeAssembliesFromAppDomain();
        ExRam.Gremlinq.Core.IAssemblyLookupSet IncludeAssembliesFromStackTrace();
        ExRam.Gremlinq.Core.IAssemblyLookupSet IncludeAssembliesOfBaseTypes();
    }
    public interface IAssemblyLookupSet : ExRam.Gremlinq.Core.IAssemblyLookupBuilder
    {
        System.Collections.Immutable.IImmutableSet<System.Reflection.Assembly> Assemblies { get; }
    }
    public interface IBothEdgeGremlinQueryBase : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        new ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQuery<object> Lower();
    }
    public interface IBothEdgeGremlinQueryBase<TEdge, TOutVertex, TInVertex> : ExRam.Gremlinq.Core.IBothEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBase<TEdge, TInVertex>, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBase<TEdge, TOutVertex>, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        new ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQuery<TEdge> Lower();
    }
    public interface IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> : ExRam.Gremlinq.Core.IBothEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IBothEdgeGremlinQueryBase<TEdge, TOutVertex, TInVertex>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBaseRec<ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBase<TEdge, TInVertex>, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBaseRec<ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBaseRec<ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBase<TEdge, TOutVertex>, ExRam.Gremlinq.Core.IStartGremlinQuery { }
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
    public interface IConfigurableGremlinQuerySource
    {
        ExRam.Gremlinq.Core.IGremlinQuerySource ConfigureEnvironment(System.Func<ExRam.Gremlinq.Core.IGremlinQueryEnvironment, ExRam.Gremlinq.Core.IGremlinQueryEnvironment> environmentTransformation);
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
        ExRam.Gremlinq.Core.IPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.Property<object>> Properties(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.Property<TValue>> Properties<TValue>(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.Property<TValue>> Properties<TValue>(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TEdge, TInVertex> To<TInVertex>(ExRam.Gremlinq.Core.StepLabel<TInVertex> stepLabel);
        ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TEdge, TInVertex> To<TInVertex>(System.Func<ExRam.Gremlinq.Core.IVertexGremlinQueryBase, ExRam.Gremlinq.Core.IVertexGremlinQueryBase<TInVertex>> toVertexTraversal);
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge> Update(TEdge element);
        ExRam.Gremlinq.Core.IValueGremlinQuery<object> Values(params System.Linq.Expressions.Expression<>[] projections);
        new ExRam.Gremlinq.Core.IValueGremlinQuery<TTarget> Values<TTarget>(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IValueGremlinQuery<TTarget> Values<TTarget>(params System.Linq.Expressions.Expression<>[] projections);
    }
    public interface IEdgeGremlinQuery<TEdge> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge>>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge>>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge>>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge>>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IStartGremlinQuery { }
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
        ExRam.Gremlinq.Core.IValueGremlinQuery<object> Id();
        ExRam.Gremlinq.Core.IValueGremlinQuery<string> Label();
        ExRam.Gremlinq.Core.IValueGremlinQuery<System.Collections.Generic.IDictionary<string, object>> ValueMap();
        ExRam.Gremlinq.Core.IValueGremlinQuery<System.Collections.Generic.IDictionary<string, TTarget>> ValueMap<TTarget>();
        ExRam.Gremlinq.Core.IValueGremlinQuery<object> Values();
        ExRam.Gremlinq.Core.IValueGremlinQuery<TTarget> Values<TTarget>();
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
        TSelf Where<TProjection>(System.Linq.Expressions.Expression<System.Func<TElement, TProjection>> projection, System.Func<ExRam.Gremlinq.Core.IGremlinQueryBase<TProjection>, ExRam.Gremlinq.Core.IGremlinQueryBase> propertyTraversal);
    }
    public interface IElementGremlinQueryBase<TElement> : ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IElementGremlinQuery<TElement> Update(TElement element);
        ExRam.Gremlinq.Core.IValueGremlinQuery<System.Collections.Generic.IDictionary<string, TTarget>> ValueMap<TTarget>(params System.Linq.Expressions.Expression<>[] keys);
        ExRam.Gremlinq.Core.IValueGremlinQuery<TTarget> Values<TTarget>(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IValueGremlinQuery<TTarget> Values<TTarget>(params System.Linq.Expressions.Expression<>[] projections);
    }
    public interface IElementGremlinQuery<TElement> : ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<ExRam.Gremlinq.Core.IElementGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TElement, ExRam.Gremlinq.Core.IElementGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IElementGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TElement, ExRam.Gremlinq.Core.IElementGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IStartGremlinQuery { }
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
    public interface IGraphElementModel
    {
        System.Collections.Immutable.IImmutableDictionary<System.Type, ExRam.Gremlinq.Core.ElementMetadata> Metadata { get; }
    }
    public interface IGraphElementPropertyModel
    {
        System.Collections.Immutable.IImmutableDictionary<System.Reflection.MemberInfo, ExRam.Gremlinq.Core.MemberMetadata> MemberMetadata { get; }
        ExRam.Gremlinq.Core.IGraphElementPropertyModel ConfigureMemberMetadata(System.Func<System.Collections.Immutable.IImmutableDictionary<System.Reflection.MemberInfo, ExRam.Gremlinq.Core.MemberMetadata>, System.Collections.Immutable.IImmutableDictionary<System.Reflection.MemberInfo, ExRam.Gremlinq.Core.MemberMetadata>> transformation);
    }
    public interface IGraphModel
    {
        ExRam.Gremlinq.Core.IGraphElementModel EdgesModel { get; }
        System.Collections.Immutable.IImmutableSet<System.Type> NativeTypes { get; }
        ExRam.Gremlinq.Core.IGraphElementPropertyModel PropertiesModel { get; }
        ExRam.Gremlinq.Core.IGraphElementModel VerticesModel { get; }
        ExRam.Gremlinq.Core.IGraphModel ConfigureEdges(System.Func<ExRam.Gremlinq.Core.IGraphElementModel, ExRam.Gremlinq.Core.IGraphElementModel> transformation);
        ExRam.Gremlinq.Core.IGraphModel ConfigureNativeTypes(System.Func<System.Collections.Immutable.IImmutableSet<System.Type>, System.Collections.Immutable.IImmutableSet<System.Type>> transformation);
        ExRam.Gremlinq.Core.IGraphModel ConfigureProperties(System.Func<ExRam.Gremlinq.Core.IGraphElementPropertyModel, ExRam.Gremlinq.Core.IGraphElementPropertyModel> transformation);
        ExRam.Gremlinq.Core.IGraphModel ConfigureVertices(System.Func<ExRam.Gremlinq.Core.IGraphElementModel, ExRam.Gremlinq.Core.IGraphElementModel> transformation);
    }
    public interface IGremlinQueryAdmin
    {
        System.Type ElementType { get; }
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment Environment { get; }
        ExRam.Gremlinq.Core.QuerySemantics Semantics { get; }
        ExRam.Gremlinq.Core.StepStack Steps { get; }
        TTargetQuery AddStep<TTargetQuery>(ExRam.Gremlinq.Core.Step step)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        TTargetQuery ChangeQueryType<TTargetQuery>()
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        TTargetQuery ConfigureSteps<TTargetQuery>(System.Func<ExRam.Gremlinq.Core.StepStack, ExRam.Gremlinq.Core.StepStack> configurator)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        ExRam.Gremlinq.Core.IGremlinQuerySource GetSource();
        ExRam.Gremlinq.Core.Traversal ToTraversal();
    }
    public interface IGremlinQueryBase : ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IGremlinQueryAdmin AsAdmin();
        ExRam.Gremlinq.Core.IArrayGremlinQuery<TElement, TArrayItem, TOriginalQuery> Cap<TElement, TArrayItem, TOriginalQuery>(ExRam.Gremlinq.Core.StepLabel<ExRam.Gremlinq.Core.IArrayGremlinQuery<TElement, TArrayItem, TOriginalQuery>, TElement> label)
            where TOriginalQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        ExRam.Gremlinq.Core.IGremlinQuery<TResult> Cast<TResult>();
        ExRam.Gremlinq.Core.IValueGremlinQuery<TValue> Constant<TValue>(TValue constant);
        ExRam.Gremlinq.Core.IValueGremlinQuery<long> Count();
        ExRam.Gremlinq.Core.IValueGremlinQuery<long> CountLocal();
        string Debug(ExRam.Gremlinq.Core.GroovyFormatting groovyFormatting = 1, bool indented = false);
        ExRam.Gremlinq.Core.IValueGremlinQuery<object> Drop();
        ExRam.Gremlinq.Core.IValueGremlinQuery<string> Explain();
        System.Runtime.CompilerServices.TaskAwaiter GetAwaiter();
        ExRam.Gremlinq.Core.IGremlinQuery<object> Lower();
        ExRam.Gremlinq.Core.IValueGremlinQuery<ExRam.Gremlinq.Core.GraphElements.Path> Path();
        ExRam.Gremlinq.Core.IValueGremlinQuery<string> Profile();
        ExRam.Gremlinq.Core.IValueGremlinQuery<TStepElement> Select<TStepElement>(ExRam.Gremlinq.Core.StepLabel<TStepElement> label);
        TQuery Select<TQuery, TElement>(ExRam.Gremlinq.Core.StepLabel<TQuery, TElement> label)
            where TQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        ExRam.Gremlinq.Core.IValueGremlinQuery<System.ValueTuple<T1, T2>> Select<T1, T2>(ExRam.Gremlinq.Core.StepLabel<T1> label1, ExRam.Gremlinq.Core.StepLabel<T2> label2);
        ExRam.Gremlinq.Core.IValueGremlinQuery<System.ValueTuple<T1, T2, T3>> Select<T1, T2, T3>(ExRam.Gremlinq.Core.StepLabel<T1> label1, ExRam.Gremlinq.Core.StepLabel<T2> label2, ExRam.Gremlinq.Core.StepLabel<T3> label3);
        ExRam.Gremlinq.Core.IValueGremlinQuery<System.ValueTuple<T1, T2, T3, T4>> Select<T1, T2, T3, T4>(ExRam.Gremlinq.Core.StepLabel<T1> label1, ExRam.Gremlinq.Core.StepLabel<T2> label2, ExRam.Gremlinq.Core.StepLabel<T3> label3, ExRam.Gremlinq.Core.StepLabel<T4> label4);
        ExRam.Gremlinq.Core.IValueGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5>> Select<T1, T2, T3, T4, T5>(ExRam.Gremlinq.Core.StepLabel<T1> label1, ExRam.Gremlinq.Core.StepLabel<T2> label2, ExRam.Gremlinq.Core.StepLabel<T3> label3, ExRam.Gremlinq.Core.StepLabel<T4> label4, ExRam.Gremlinq.Core.StepLabel<T5> label5);
        ExRam.Gremlinq.Core.IValueGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6>> Select<T1, T2, T3, T4, T5, T6>(ExRam.Gremlinq.Core.StepLabel<T1> label1, ExRam.Gremlinq.Core.StepLabel<T2> label2, ExRam.Gremlinq.Core.StepLabel<T3> label3, ExRam.Gremlinq.Core.StepLabel<T4> label4, ExRam.Gremlinq.Core.StepLabel<T5> label5, ExRam.Gremlinq.Core.StepLabel<T6> label6);
        ExRam.Gremlinq.Core.IValueGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7>> Select<T1, T2, T3, T4, T5, T6, T7>(ExRam.Gremlinq.Core.StepLabel<T1> label1, ExRam.Gremlinq.Core.StepLabel<T2> label2, ExRam.Gremlinq.Core.StepLabel<T3> label3, ExRam.Gremlinq.Core.StepLabel<T4> label4, ExRam.Gremlinq.Core.StepLabel<T5> label5, ExRam.Gremlinq.Core.StepLabel<T6> label6, ExRam.Gremlinq.Core.StepLabel<T7> label7);
        ExRam.Gremlinq.Core.IValueGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8>>> Select<T1, T2, T3, T4, T5, T6, T7, T8>(ExRam.Gremlinq.Core.StepLabel<T1> label1, ExRam.Gremlinq.Core.StepLabel<T2> label2, ExRam.Gremlinq.Core.StepLabel<T3> label3, ExRam.Gremlinq.Core.StepLabel<T4> label4, ExRam.Gremlinq.Core.StepLabel<T5> label5, ExRam.Gremlinq.Core.StepLabel<T6> label6, ExRam.Gremlinq.Core.StepLabel<T7> label7, ExRam.Gremlinq.Core.StepLabel<T8> label8);
        ExRam.Gremlinq.Core.IValueGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9>>> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9>(ExRam.Gremlinq.Core.StepLabel<T1> label1, ExRam.Gremlinq.Core.StepLabel<T2> label2, ExRam.Gremlinq.Core.StepLabel<T3> label3, ExRam.Gremlinq.Core.StepLabel<T4> label4, ExRam.Gremlinq.Core.StepLabel<T5> label5, ExRam.Gremlinq.Core.StepLabel<T6> label6, ExRam.Gremlinq.Core.StepLabel<T7> label7, ExRam.Gremlinq.Core.StepLabel<T8> label8, ExRam.Gremlinq.Core.StepLabel<T9> label9);
        ExRam.Gremlinq.Core.IValueGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10>>> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(ExRam.Gremlinq.Core.StepLabel<T1> label1, ExRam.Gremlinq.Core.StepLabel<T2> label2, ExRam.Gremlinq.Core.StepLabel<T3> label3, ExRam.Gremlinq.Core.StepLabel<T4> label4, ExRam.Gremlinq.Core.StepLabel<T5> label5, ExRam.Gremlinq.Core.StepLabel<T6> label6, ExRam.Gremlinq.Core.StepLabel<T7> label7, ExRam.Gremlinq.Core.StepLabel<T8> label8, ExRam.Gremlinq.Core.StepLabel<T9> label9, ExRam.Gremlinq.Core.StepLabel<T10> label10);
        ExRam.Gremlinq.Core.IValueGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11>>> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(ExRam.Gremlinq.Core.StepLabel<T1> label1, ExRam.Gremlinq.Core.StepLabel<T2> label2, ExRam.Gremlinq.Core.StepLabel<T3> label3, ExRam.Gremlinq.Core.StepLabel<T4> label4, ExRam.Gremlinq.Core.StepLabel<T5> label5, ExRam.Gremlinq.Core.StepLabel<T6> label6, ExRam.Gremlinq.Core.StepLabel<T7> label7, ExRam.Gremlinq.Core.StepLabel<T8> label8, ExRam.Gremlinq.Core.StepLabel<T9> label9, ExRam.Gremlinq.Core.StepLabel<T10> label10, ExRam.Gremlinq.Core.StepLabel<T11> label11);
        ExRam.Gremlinq.Core.IValueGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12>>> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(ExRam.Gremlinq.Core.StepLabel<T1> label1, ExRam.Gremlinq.Core.StepLabel<T2> label2, ExRam.Gremlinq.Core.StepLabel<T3> label3, ExRam.Gremlinq.Core.StepLabel<T4> label4, ExRam.Gremlinq.Core.StepLabel<T5> label5, ExRam.Gremlinq.Core.StepLabel<T6> label6, ExRam.Gremlinq.Core.StepLabel<T7> label7, ExRam.Gremlinq.Core.StepLabel<T8> label8, ExRam.Gremlinq.Core.StepLabel<T9> label9, ExRam.Gremlinq.Core.StepLabel<T10> label10, ExRam.Gremlinq.Core.StepLabel<T11> label11, ExRam.Gremlinq.Core.StepLabel<T12> label12);
        ExRam.Gremlinq.Core.IValueGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13>>> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(ExRam.Gremlinq.Core.StepLabel<T1> label1, ExRam.Gremlinq.Core.StepLabel<T2> label2, ExRam.Gremlinq.Core.StepLabel<T3> label3, ExRam.Gremlinq.Core.StepLabel<T4> label4, ExRam.Gremlinq.Core.StepLabel<T5> label5, ExRam.Gremlinq.Core.StepLabel<T6> label6, ExRam.Gremlinq.Core.StepLabel<T7> label7, ExRam.Gremlinq.Core.StepLabel<T8> label8, ExRam.Gremlinq.Core.StepLabel<T9> label9, ExRam.Gremlinq.Core.StepLabel<T10> label10, ExRam.Gremlinq.Core.StepLabel<T11> label11, ExRam.Gremlinq.Core.StepLabel<T12> label12, ExRam.Gremlinq.Core.StepLabel<T13> label13);
        ExRam.Gremlinq.Core.IValueGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13, T14>>> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(ExRam.Gremlinq.Core.StepLabel<T1> label1, ExRam.Gremlinq.Core.StepLabel<T2> label2, ExRam.Gremlinq.Core.StepLabel<T3> label3, ExRam.Gremlinq.Core.StepLabel<T4> label4, ExRam.Gremlinq.Core.StepLabel<T5> label5, ExRam.Gremlinq.Core.StepLabel<T6> label6, ExRam.Gremlinq.Core.StepLabel<T7> label7, ExRam.Gremlinq.Core.StepLabel<T8> label8, ExRam.Gremlinq.Core.StepLabel<T9> label9, ExRam.Gremlinq.Core.StepLabel<T10> label10, ExRam.Gremlinq.Core.StepLabel<T11> label11, ExRam.Gremlinq.Core.StepLabel<T12> label12, ExRam.Gremlinq.Core.StepLabel<T13> label13, ExRam.Gremlinq.Core.StepLabel<T14> label14);
        ExRam.Gremlinq.Core.IValueGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13, T14, System.ValueTuple<T15>>>> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(ExRam.Gremlinq.Core.StepLabel<T1> label1, ExRam.Gremlinq.Core.StepLabel<T2> label2, ExRam.Gremlinq.Core.StepLabel<T3> label3, ExRam.Gremlinq.Core.StepLabel<T4> label4, ExRam.Gremlinq.Core.StepLabel<T5> label5, ExRam.Gremlinq.Core.StepLabel<T6> label6, ExRam.Gremlinq.Core.StepLabel<T7> label7, ExRam.Gremlinq.Core.StepLabel<T8> label8, ExRam.Gremlinq.Core.StepLabel<T9> label9, ExRam.Gremlinq.Core.StepLabel<T10> label10, ExRam.Gremlinq.Core.StepLabel<T11> label11, ExRam.Gremlinq.Core.StepLabel<T12> label12, ExRam.Gremlinq.Core.StepLabel<T13> label13, ExRam.Gremlinq.Core.StepLabel<T14> label14, ExRam.Gremlinq.Core.StepLabel<T15> label15);
        ExRam.Gremlinq.Core.IValueGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13, T14, System.ValueTuple<T15, T16>>>> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
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
        TSelf And(params System.Func<, >[] andTraversals);
        TSelf Barrier();
        ExRam.Gremlinq.Core.IValueGremlinQuery<object> Choose(System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase> traversalPredicate, System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase> trueChoice);
        TSelf Choose(System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase> traversalPredicate, System.Func<TSelf, TSelf> trueChoice);
        TTargetQuery Choose<TTargetQuery>(System.Func<ExRam.Gremlinq.Core.IChooseBuilder<TSelf>, ExRam.Gremlinq.Core.IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        TTargetQuery Choose<TTargetQuery>(System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase> traversalPredicate, System.Func<TSelf, TTargetQuery> trueChoice, System.Func<TSelf, TTargetQuery> falseChoice)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        ExRam.Gremlinq.Core.IValueGremlinQuery<object> Coalesce(params System.Func<, >[] traversals);
        TTargetQuery Coalesce<TTargetQuery>(params System.Func<, >[] traversals)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        TSelf Coin(double probability);
        TSelf CyclicPath();
        TSelf Dedup();
        TSelf DedupLocal();
        TSelf Emit();
        TTargetQuery FlatMap<TTargetQuery>(System.Func<TSelf, TTargetQuery> mapping)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        ExRam.Gremlinq.Core.IValueGremlinQuery<System.Collections.Generic.IDictionary<TNewKey, object>> Group<TNewKey>(System.Func<ExRam.Gremlinq.Core.IGroupBuilder<TSelf>, ExRam.Gremlinq.Core.IGroupBuilderWithKey<TSelf, TNewKey>> groupBuilder);
        ExRam.Gremlinq.Core.IValueGremlinQuery<System.Collections.Generic.IDictionary<TNewKey, TNewValue>> Group<TNewKey, TNewValue>(System.Func<ExRam.Gremlinq.Core.IGroupBuilder<TSelf>, ExRam.Gremlinq.Core.IGroupBuilderWithKeyAndValue<TSelf, TNewKey, TNewValue>> groupBuilder);
        TSelf Identity();
        TSelf Limit(long count);
        TSelf LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(System.Func<TSelf, TTargetQuery> localTraversal)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        TTargetQuery Map<TTargetQuery>(System.Func<TSelf, TTargetQuery> mapping)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        TSelf Mute();
        TSelf None();
        TSelf Not(System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase> notTraversal);
        TSelf Optional(System.Func<TSelf, TSelf> optionalTraversal);
        TSelf Or(params System.Func<, >[] orTraversals);
        TSelf Order(System.Func<ExRam.Gremlinq.Core.IOrderBuilder<TSelf>, ExRam.Gremlinq.Core.IOrderBuilderWithBy<TSelf>> projection);
        TSelf OrderLocal(System.Func<ExRam.Gremlinq.Core.IOrderBuilder<TSelf>, ExRam.Gremlinq.Core.IOrderBuilderWithBy<TSelf>> projection);
        TSelf Range(long low, long high);
        TSelf RangeLocal(long low, long high);
        TSelf Repeat(System.Func<TSelf, TSelf> repeatTraversal);
        TSelf RepeatUntil(System.Func<TSelf, TSelf> repeatTraversal, System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase> untilTraversal);
        TSelf SideEffect(System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase> sideEffectTraversal);
        TSelf SimplePath();
        TSelf Skip(long count);
        TSelf SkipLocal(long count);
        TSelf Tail(long count);
        TSelf TailLocal(long count);
        TSelf Times(int count);
        ExRam.Gremlinq.Core.IValueGremlinQuery<object> Union(params System.Func<, >[] traversals);
        TTargetQuery Union<TTargetQuery>(params System.Func<, >[] unionTraversals)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        TSelf UntilRepeat(System.Func<TSelf, TSelf> repeatTraversal, System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase> untilTraversal);
        TSelf Where(Gremlin.Net.Process.Traversal.ILambda lambda);
        TSelf Where(System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase> filterTraversal);
    }
    public interface IGremlinQueryBaseRec<TElement, TSelf> : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TElement, TSelf>
    {
        TTargetQuery Aggregate<TTargetQuery>(System.Func<TSelf, ExRam.Gremlinq.Core.StepLabel<ExRam.Gremlinq.Core.IArrayGremlinQuery<TElement[], TElement, TSelf>, TElement[]>, TTargetQuery> continuation)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        TTargetQuery AggregateGlobal<TTargetQuery>(System.Func<TSelf, ExRam.Gremlinq.Core.StepLabel<ExRam.Gremlinq.Core.IArrayGremlinQuery<TElement[], TElement, TSelf>, TElement[]>, TTargetQuery> continuation)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        TSelf As(ExRam.Gremlinq.Core.StepLabel<TElement> stepLabel);
        TTargetQuery As<TTargetQuery>(System.Func<TSelf, ExRam.Gremlinq.Core.StepLabel<TSelf, TElement>, TTargetQuery> continuation)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        ExRam.Gremlinq.Core.IValueGremlinQuery<object> Choose(System.Linq.Expressions.Expression<System.Func<TElement, bool>> predicate, System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase> trueChoice);
        TSelf Choose(System.Linq.Expressions.Expression<System.Func<TElement, bool>> predicate, System.Func<TSelf, TSelf> trueChoice);
        TTargetQuery Choose<TTargetQuery>(System.Linq.Expressions.Expression<System.Func<TElement, bool>> predicate, System.Func<TSelf, TTargetQuery> trueChoice, System.Func<TSelf, TTargetQuery> falseChoice)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        ExRam.Gremlinq.Core.IArrayGremlinQuery<TElement[], TElement, TSelf> Fold();
        ExRam.Gremlinq.Core.IArrayGremlinQuery<TElement[], TElement, TSelf> ForceArray();
        TSelf Inject(params TElement[] elements);
        TSelf Order(System.Func<ExRam.Gremlinq.Core.IOrderBuilder<TElement, TSelf>, ExRam.Gremlinq.Core.IOrderBuilderWithBy<TElement, TSelf>> projection);
        TSelf OrderLocal(System.Func<ExRam.Gremlinq.Core.IOrderBuilder<TElement, TSelf>, ExRam.Gremlinq.Core.IOrderBuilderWithBy<TElement, TSelf>> projection);
        [return: System.Runtime.CompilerServices.Dynamic(new bool[] {
                false,
                true})]
        ExRam.Gremlinq.Core.IValueGremlinQuery<object> Project(System.Func<ExRam.Gremlinq.Core.IProjectBuilder<TSelf, TElement>, ExRam.Gremlinq.Core.IProjectResult> continuation);
        ExRam.Gremlinq.Core.IValueTupleGremlinQuery<TResult> Project<TResult>(System.Func<ExRam.Gremlinq.Core.IProjectBuilder<TSelf, TElement>, ExRam.Gremlinq.Core.IProjectResult<TResult>> continuation);
        TSelf Where(System.Linq.Expressions.Expression<System.Func<TElement, bool>> predicate);
    }
    public interface IGremlinQueryBase<TElement> : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IArrayGremlinQuery<TElement[], TElement, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>> ForceArray();
        ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> ForceBothEdge<TOutVertex, TInVertex>();
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TElement> ForceEdge();
        ExRam.Gremlinq.Core.IElementGremlinQuery<TElement> ForceElement();
        ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TElement, TInVertex> ForceInEdge<TInVertex>();
        ExRam.Gremlinq.Core.IOutEdgeGremlinQuery<TElement, TOutVertex> ForceOutEdge<TOutVertex>();
        ExRam.Gremlinq.Core.IPropertyGremlinQuery<TElement> ForceProperty();
        ExRam.Gremlinq.Core.IValueGremlinQuery<TElement> ForceValue();
        ExRam.Gremlinq.Core.IValueTupleGremlinQuery<TElement> ForceValueTuple();
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TElement> ForceVertex();
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TElement, TValue> ForceVertexProperty<TValue>();
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TElement, TValue, TMeta> ForceVertexProperty<TValue, TMeta>();
        ExRam.Gremlinq.Core.GremlinQueryAwaiter<TElement> GetAwaiter();
        ExRam.Gremlinq.Core.IGremlinQuery<TElement> Lower();
        System.Collections.Generic.IAsyncEnumerable<TElement> ToAsyncEnumerable();
    }
    public interface IGremlinQueryEnvironment
    {
        ExRam.Gremlinq.Core.IAddStepHandler AddStepHandler { get; }
        ExRam.Gremlinq.Core.IGremlinQueryExecutionResultDeserializer Deserializer { get; }
        ExRam.Gremlinq.Core.IGremlinQueryExecutor Executor { get; }
        ExRam.Gremlinq.Core.IFeatureSet FeatureSet { get; }
        Microsoft.Extensions.Logging.ILogger Logger { get; }
        ExRam.Gremlinq.Core.IGraphModel Model { get; }
        ExRam.Gremlinq.Core.IGremlinqOptions Options { get; }
        ExRam.Gremlinq.Core.IGremlinQuerySerializer Serializer { get; }
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment ConfigureAddStepHandler(System.Func<ExRam.Gremlinq.Core.IAddStepHandler, ExRam.Gremlinq.Core.IAddStepHandler> handlerTransformation);
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment ConfigureDeserializer(System.Func<ExRam.Gremlinq.Core.IGremlinQueryExecutionResultDeserializer, ExRam.Gremlinq.Core.IGremlinQueryExecutionResultDeserializer> deserializerTransformation);
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment ConfigureExecutor(System.Func<ExRam.Gremlinq.Core.IGremlinQueryExecutor, ExRam.Gremlinq.Core.IGremlinQueryExecutor> executorTransformation);
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment ConfigureFeatureSet(System.Func<ExRam.Gremlinq.Core.IFeatureSet, ExRam.Gremlinq.Core.IFeatureSet> featureSetTransformation);
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment ConfigureLogger(System.Func<Microsoft.Extensions.Logging.ILogger, Microsoft.Extensions.Logging.ILogger> loggerTransformation);
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment ConfigureModel(System.Func<ExRam.Gremlinq.Core.IGraphModel, ExRam.Gremlinq.Core.IGraphModel> modelTransformation);
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment ConfigureOptions(System.Func<ExRam.Gremlinq.Core.IGremlinqOptions, ExRam.Gremlinq.Core.IGremlinqOptions> optionsTransformation);
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment ConfigureSerializer(System.Func<ExRam.Gremlinq.Core.IGremlinQuerySerializer, ExRam.Gremlinq.Core.IGremlinQuerySerializer> serializerTransformation);
    }
    public interface IGremlinQueryExecutionResultDeserializer
    {
        ExRam.Gremlinq.Core.IGremlinQueryExecutionResultDeserializer ConfigureFragmentDeserializer(System.Func<ExRam.Gremlinq.Core.IGremlinQueryFragmentDeserializer, ExRam.Gremlinq.Core.IGremlinQueryFragmentDeserializer> transformation);
        System.Collections.Generic.IAsyncEnumerable<TElement> Deserialize<TElement>(object executionResult, ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment);
    }
    public interface IGremlinQueryExecutor
    {
        System.Collections.Generic.IAsyncEnumerable<object> Execute(object serializedQuery, ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment);
    }
    public interface IGremlinQueryFragmentDeserializer
    {
        ExRam.Gremlinq.Core.IGremlinQueryFragmentDeserializer Override<TSerialized>(ExRam.Gremlinq.Core.GremlinQueryFragmentDeserializerDelegate<TSerialized> deserializer);
        object? TryDeserialize<TSerializedData>(TSerializedData serializedData, System.Type fragmentType, ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment);
    }
    public interface IGremlinQueryFragmentSerializer
    {
        ExRam.Gremlinq.Core.IGremlinQueryFragmentSerializer Override<TFragment>(ExRam.Gremlinq.Core.GremlinQueryFragmentSerializerDelegate<TFragment> serializer);
        object Serialize<TFragment>(TFragment fragment, ExRam.Gremlinq.Core.IGremlinQueryEnvironment gremlinQueryEnvironment);
    }
    public interface IGremlinQuerySerializer
    {
        ExRam.Gremlinq.Core.IGremlinQuerySerializer ConfigureFragmentSerializer(System.Func<ExRam.Gremlinq.Core.IGremlinQueryFragmentSerializer, ExRam.Gremlinq.Core.IGremlinQueryFragmentSerializer> transformation);
        object Serialize(ExRam.Gremlinq.Core.IGremlinQueryBase query);
    }
    public interface IGremlinQuerySource : ExRam.Gremlinq.Core.IConfigurableGremlinQuerySource, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment Environment { get; }
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<object> E(params object[] ids);
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge> E<TEdge>(params object[] ids);
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TNewEdge> ReplaceE<TNewEdge>(TNewEdge edge);
        ExRam.Gremlinq.Core.IGremlinQuerySource WithSideEffect<TSideEffect>(ExRam.Gremlinq.Core.StepLabel<TSideEffect> label, TSideEffect value);
        TQuery WithSideEffect<TSideEffect, TQuery>(TSideEffect value, System.Func<ExRam.Gremlinq.Core.IGremlinQuerySource, ExRam.Gremlinq.Core.StepLabel<TSideEffect>, TQuery> continuation)
            where TQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        ExRam.Gremlinq.Core.IGremlinQuerySource WithoutStrategies(params System.Type[] strategyTypes);
    }
    public interface IGremlinQuerySourceTransformation
    {
        ExRam.Gremlinq.Core.IGremlinQuerySource Transform(ExRam.Gremlinq.Core.IGremlinQuerySource source);
    }
    public interface IGremlinQuery<TElement> : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TElement, ExRam.Gremlinq.Core.IGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IStartGremlinQuery { }
    public interface IGremlinqOption { }
    public interface IGremlinqOptions
    {
        ExRam.Gremlinq.Core.IGremlinqOptions ConfigureValue<TValue>(ExRam.Gremlinq.Core.GremlinqOption<TValue> option, System.Func<TValue, TValue> configuration);
        bool Contains(ExRam.Gremlinq.Core.IGremlinqOption option);
        TValue GetValue<TValue>(ExRam.Gremlinq.Core.GremlinqOption<TValue> option);
        ExRam.Gremlinq.Core.IGremlinqOptions Remove(ExRam.Gremlinq.Core.IGremlinqOption option);
        ExRam.Gremlinq.Core.IGremlinqOptions SetValue<TValue>(ExRam.Gremlinq.Core.GremlinqOption<TValue> option, TValue value);
    }
    public interface IGroupBuilderWithKeyAndValue<out TSourceQuery, TKey, TValue> : ExRam.Gremlinq.Core.IGroupBuilderWithKey<TSourceQuery, TKey>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IGremlinQueryBase<TValue> ValueQuery { get; }
    }
    public interface IGroupBuilderWithKey<out TSourceQuery, TKey>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IGremlinQueryBase<TKey> KeyQuery { get; }
        ExRam.Gremlinq.Core.IGroupBuilderWithKeyAndValue<TSourceQuery, TKey, TValue> ByValue<TValue>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TValue>> valueSelector);
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
        ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> From<TOutVertex>(System.Func<ExRam.Gremlinq.Core.IVertexGremlinQuery<TInVertex>, ExRam.Gremlinq.Core.IVertexGremlinQueryBase<TOutVertex>> fromVertexTraversal);
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TInVertex> InV();
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge> Lower();
    }
    public interface IInEdgeGremlinQuery<TEdge, TInVertex> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TEdge, TInVertex>>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TEdge, TInVertex>>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TEdge, TInVertex>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TEdge, TInVertex>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TEdge, TInVertex>>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TEdge, TInVertex>>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TEdge, TInVertex>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TEdge, TInVertex>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBaseRec<ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TEdge, TInVertex>>, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBaseRec<TEdge, TInVertex, ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TEdge, TInVertex>>, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBase<TEdge, TInVertex>, ExRam.Gremlinq.Core.IStartGremlinQuery { }
    public interface IInOrOutEdgeGremlinQueryBase : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery { }
    public interface IInOrOutEdgeGremlinQueryBaseRec<TSelf> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBaseRec<TSelf> { }
    public interface IInOrOutEdgeGremlinQueryBaseRec<TEdge, TAdjacentVertex, TSelf> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBase<TEdge, TAdjacentVertex>, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBaseRec<TEdge, TAdjacentVertex, TSelf> { }
    public interface IInOrOutEdgeGremlinQueryBase<TEdge, TAdjacentVertex> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TTargetVertex, TAdjacentVertex> From<TTargetVertex>(ExRam.Gremlinq.Core.StepLabel<TTargetVertex> stepLabel);
        ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TTargetVertex, TAdjacentVertex> From<TTargetVertex>(System.Func<ExRam.Gremlinq.Core.IVertexGremlinQuery<TAdjacentVertex>, ExRam.Gremlinq.Core.IVertexGremlinQueryBase<TTargetVertex>> fromVertexTraversal);
        ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TAdjacentVertex, TTargetVertex> To<TTargetVertex>(ExRam.Gremlinq.Core.StepLabel<TTargetVertex> stepLabel);
        ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TAdjacentVertex, TTargetVertex> To<TTargetVertex>(System.Func<ExRam.Gremlinq.Core.IVertexGremlinQuery<TAdjacentVertex>, ExRam.Gremlinq.Core.IVertexGremlinQueryBase<TTargetVertex>> toVertexTraversal);
    }
    public interface IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBaseRec<ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBaseRec<TEdge, TAdjacentVertex, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBase<TEdge, TAdjacentVertex>, ExRam.Gremlinq.Core.IStartGremlinQuery { }
    public interface IMemberMetadataConfigurator<TElement> : System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.Reflection.MemberInfo, ExRam.Gremlinq.Core.MemberMetadata>>, System.Collections.Generic.IReadOnlyCollection<System.Collections.Generic.KeyValuePair<System.Reflection.MemberInfo, ExRam.Gremlinq.Core.MemberMetadata>>, System.Collections.Generic.IReadOnlyDictionary<System.Reflection.MemberInfo, ExRam.Gremlinq.Core.MemberMetadata>, System.Collections.IEnumerable, System.Collections.Immutable.IImmutableDictionary<System.Reflection.MemberInfo, ExRam.Gremlinq.Core.MemberMetadata>
    {
        ExRam.Gremlinq.Core.IMemberMetadataConfigurator<TElement> ConfigureName<TProperty>(System.Linq.Expressions.Expression<System.Func<TElement, TProperty>> propertyExpression, string name);
        ExRam.Gremlinq.Core.IMemberMetadataConfigurator<TElement> IgnoreAlways<TProperty>(System.Linq.Expressions.Expression<System.Func<TElement, TProperty>> propertyExpression);
        ExRam.Gremlinq.Core.IMemberMetadataConfigurator<TElement> IgnoreOnAdd<TProperty>(System.Linq.Expressions.Expression<System.Func<TElement, TProperty>> propertyExpression);
        ExRam.Gremlinq.Core.IMemberMetadataConfigurator<TElement> IgnoreOnUpdate<TProperty>(System.Linq.Expressions.Expression<System.Func<TElement, TProperty>> propertyExpression);
        ExRam.Gremlinq.Core.IMemberMetadataConfigurator<TElement> ResetSerializationBehaviour<TProperty>(System.Linq.Expressions.Expression<System.Func<TElement, TProperty>> propertyExpression);
    }
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
        ExRam.Gremlinq.Core.IOrderBuilderWithBy<TSourceQuery> By(Gremlin.Net.Process.Traversal.ILambda lambda);
        ExRam.Gremlinq.Core.IOrderBuilderWithBy<TSourceQuery> By(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase> traversal);
        ExRam.Gremlinq.Core.IOrderBuilderWithBy<TSourceQuery> ByDescending(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase> traversal);
    }
    public interface IOrderBuilder<TElement, out TSourceQuery> : ExRam.Gremlinq.Core.IOrderBuilder<TSourceQuery>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>
    {
        ExRam.Gremlinq.Core.IOrderBuilderWithBy<TElement, TSourceQuery> By(Gremlin.Net.Process.Traversal.ILambda lambda);
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
        ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> To<TInVertex>(System.Func<ExRam.Gremlinq.Core.IVertexGremlinQuery<TOutVertex>, ExRam.Gremlinq.Core.IVertexGremlinQueryBase<TInVertex>> toVertexTraversal);
    }
    public interface IOutEdgeGremlinQuery<TEdge, TOutVertex> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<ExRam.Gremlinq.Core.IOutEdgeGremlinQuery<TEdge, TOutVertex>>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IOutEdgeGremlinQuery<TEdge, TOutVertex>>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<ExRam.Gremlinq.Core.IOutEdgeGremlinQuery<TEdge, TOutVertex>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IOutEdgeGremlinQuery<TEdge, TOutVertex>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<ExRam.Gremlinq.Core.IOutEdgeGremlinQuery<TEdge, TOutVertex>>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IOutEdgeGremlinQuery<TEdge, TOutVertex>>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IOutEdgeGremlinQuery<TEdge, TOutVertex>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IOutEdgeGremlinQuery<TEdge, TOutVertex>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBaseRec<ExRam.Gremlinq.Core.IOutEdgeGremlinQuery<TEdge, TOutVertex>>, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBaseRec<TEdge, TOutVertex, ExRam.Gremlinq.Core.IOutEdgeGremlinQuery<TEdge, TOutVertex>>, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBase<TEdge, TOutVertex>, ExRam.Gremlinq.Core.IStartGremlinQuery { }
    public interface IProjectBuilder<out TSourceQuery, TElement>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectDynamicBuilder<TSourceQuery, TElement> ToDynamic();
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement> ToTuple();
    }
    public interface IProjectDynamicBuilder<out TSourceQuery, TElement> : ExRam.Gremlinq.Core.IProjectResult
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectDynamicBuilder<TSourceQuery, TElement> By(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase> projection);
        ExRam.Gremlinq.Core.IProjectDynamicBuilder<TSourceQuery, TElement> By(System.Linq.Expressions.Expression<System.Func<TElement, object>> projection);
        ExRam.Gremlinq.Core.IProjectDynamicBuilder<TSourceQuery, TElement> By(string name, System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase> projection);
        ExRam.Gremlinq.Core.IProjectDynamicBuilder<TSourceQuery, TElement> By(string name, System.Linq.Expressions.Expression<System.Func<TElement, object>> projection);
    }
    public interface IProjectResult
    {
        System.Collections.Immutable.IImmutableDictionary<string, ExRam.Gremlinq.Core.ProjectStep.ByStep> Projections { get; }
    }
    public interface IProjectResult<TResult> : ExRam.Gremlinq.Core.IProjectResult { }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8>>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9> By<TItem9>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem9>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9> By<TItem9>(System.Linq.Expressions.Expression<System.Func<TElement, TItem9>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9>>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10> By<TItem10>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem10>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10> By<TItem10>(System.Linq.Expressions.Expression<System.Func<TElement, TItem10>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9, TItem10>>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11> By<TItem11>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem11>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11> By<TItem11>(System.Linq.Expressions.Expression<System.Func<TElement, TItem11>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9, TItem10, TItem11>>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12> By<TItem12>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem12>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12> By<TItem12>(System.Linq.Expressions.Expression<System.Func<TElement, TItem12>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9, TItem10, TItem11, TItem12>>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13> By<TItem13>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem13>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13> By<TItem13>(System.Linq.Expressions.Expression<System.Func<TElement, TItem13>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9, TItem10, TItem11, TItem12, TItem13>>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14> By<TItem14>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem14>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14> By<TItem14>(System.Linq.Expressions.Expression<System.Func<TElement, TItem14>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14>>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15> By<TItem15>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem15>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15> By<TItem15>(System.Linq.Expressions.Expression<System.Func<TElement, TItem15>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, System.ValueTuple<TItem15>>>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16> By<TItem16>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem16>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16> By<TItem16>(System.Linq.Expressions.Expression<System.Func<TElement, TItem16>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, System.ValueTuple<TItem15, TItem16>>>>
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
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3> By<TItem3>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem3>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3> By<TItem3>(System.Linq.Expressions.Expression<System.Func<TElement, TItem3>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2, TItem3>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4> By<TItem4>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem4>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4> By<TItem4>(System.Linq.Expressions.Expression<System.Func<TElement, TItem4>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5> By<TItem5>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem5>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5> By<TItem5>(System.Linq.Expressions.Expression<System.Func<TElement, TItem5>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6> By<TItem6>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem6>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6> By<TItem6>(System.Linq.Expressions.Expression<System.Func<TElement, TItem6>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7> By<TItem7>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem7>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7> By<TItem7>(System.Linq.Expressions.Expression<System.Func<TElement, TItem7>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8> By<TItem8>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem8>> projection);
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8> By<TItem8>(System.Linq.Expressions.Expression<System.Func<TElement, TItem8>> projection);
    }
    public interface IPropertyGremlinQueryBase : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IPropertyGremlinQuery<TResult> Cast<TResult>();
    }
    public interface IPropertyGremlinQueryBase<TElement> : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IPropertyGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IValueGremlinQuery<string> Key();
        ExRam.Gremlinq.Core.IValueGremlinQuery<object> Value();
        ExRam.Gremlinq.Core.IValueGremlinQuery<TValue> Value<TValue>();
    }
    public interface IPropertyGremlinQuery<TElement> : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IPropertyGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TElement, ExRam.Gremlinq.Core.IPropertyGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IPropertyGremlinQueryBase, ExRam.Gremlinq.Core.IPropertyGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IStartGremlinQuery { }
    public interface IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge> AddE<TEdge>()
            where TEdge : new();
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge> AddE<TEdge>(TEdge edge);
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex> AddV<TVertex>()
            where TVertex : new();
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex> AddV<TVertex>(TVertex vertex);
        ExRam.Gremlinq.Core.IValueGremlinQuery<TElement> Inject<TElement>(params TElement[] elements);
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TNewVertex> ReplaceV<TNewVertex>(TNewVertex vertex);
        ExRam.Gremlinq.Core.IVertexGremlinQuery<object> V(params object[] ids);
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex> V<TVertex>(params object[] ids);
    }
    public interface IValueGremlinQueryBase : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IValueGremlinQuery<TResult> Cast<TResult>();
    }
    public interface IValueGremlinQueryBaseRec<TSelf> : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IValueGremlinQueryBase
        where TSelf : ExRam.Gremlinq.Core.IValueGremlinQueryBaseRec<TSelf> { }
    public interface IValueGremlinQueryBaseRec<TElement, TSelf> : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TElement, TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IValueGremlinQueryBase, ExRam.Gremlinq.Core.IValueGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IValueGremlinQueryBase<TElement>
        where TSelf : ExRam.Gremlinq.Core.IValueGremlinQueryBaseRec<TElement, TSelf> { }
    public interface IValueGremlinQueryBase<TElement> : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IValueGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IValueGremlinQuery<TElement> Max();
        ExRam.Gremlinq.Core.IValueGremlinQuery<object> MaxLocal();
        ExRam.Gremlinq.Core.IValueGremlinQuery<TElement> Mean();
        ExRam.Gremlinq.Core.IValueGremlinQuery<object> MeanLocal();
        ExRam.Gremlinq.Core.IValueGremlinQuery<TElement> Min();
        ExRam.Gremlinq.Core.IValueGremlinQuery<object> MinLocal();
        ExRam.Gremlinq.Core.IValueGremlinQuery<TElement> Sum();
        ExRam.Gremlinq.Core.IValueGremlinQuery<object> SumLocal();
    }
    public interface IValueGremlinQuery<TElement> : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IValueGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TElement, ExRam.Gremlinq.Core.IValueGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IValueGremlinQueryBase, ExRam.Gremlinq.Core.IValueGremlinQueryBaseRec<ExRam.Gremlinq.Core.IValueGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IValueGremlinQueryBaseRec<TElement, ExRam.Gremlinq.Core.IValueGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IValueGremlinQueryBase<TElement> { }
    public interface IValueTupleGremlinQueryBase : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery { }
    public interface IValueTupleGremlinQueryBase<TElement> : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IValueTupleGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IValueGremlinQuery<TTargetValue> Select<TTargetValue>(System.Linq.Expressions.Expression<System.Func<TElement, TTargetValue>> projection);
        ExRam.Gremlinq.Core.IValueTupleGremlinQuery<System.ValueTuple<T1, T2>> Select<T1, T2>(System.Linq.Expressions.Expression<System.Func<TElement, T1>> projection1, System.Linq.Expressions.Expression<System.Func<TElement, T2>> projection2);
        ExRam.Gremlinq.Core.IValueTupleGremlinQuery<System.ValueTuple<T1, T2, T3>> Select<T1, T2, T3>(System.Linq.Expressions.Expression<System.Func<TElement, T1>> projection1, System.Linq.Expressions.Expression<System.Func<TElement, T2>> projection2, System.Linq.Expressions.Expression<System.Func<TElement, T3>> projection3);
        ExRam.Gremlinq.Core.IValueTupleGremlinQuery<System.ValueTuple<T1, T2, T3, T4>> Select<T1, T2, T3, T4>(System.Linq.Expressions.Expression<System.Func<TElement, T1>> projection1, System.Linq.Expressions.Expression<System.Func<TElement, T2>> projection2, System.Linq.Expressions.Expression<System.Func<TElement, T3>> projection3, System.Linq.Expressions.Expression<System.Func<TElement, T4>> projection4);
        ExRam.Gremlinq.Core.IValueTupleGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5>> Select<T1, T2, T3, T4, T5>(System.Linq.Expressions.Expression<System.Func<TElement, T1>> projection1, System.Linq.Expressions.Expression<System.Func<TElement, T2>> projection2, System.Linq.Expressions.Expression<System.Func<TElement, T3>> projection3, System.Linq.Expressions.Expression<System.Func<TElement, T4>> projection4, System.Linq.Expressions.Expression<System.Func<TElement, T5>> projection5);
        ExRam.Gremlinq.Core.IValueTupleGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6>> Select<T1, T2, T3, T4, T5, T6>(System.Linq.Expressions.Expression<System.Func<TElement, T1>> projection1, System.Linq.Expressions.Expression<System.Func<TElement, T2>> projection2, System.Linq.Expressions.Expression<System.Func<TElement, T3>> projection3, System.Linq.Expressions.Expression<System.Func<TElement, T4>> projection4, System.Linq.Expressions.Expression<System.Func<TElement, T5>> projection5, System.Linq.Expressions.Expression<System.Func<TElement, T6>> projection6);
        ExRam.Gremlinq.Core.IValueTupleGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7>> Select<T1, T2, T3, T4, T5, T6, T7>(System.Linq.Expressions.Expression<System.Func<TElement, T1>> projection1, System.Linq.Expressions.Expression<System.Func<TElement, T2>> projection2, System.Linq.Expressions.Expression<System.Func<TElement, T3>> projection3, System.Linq.Expressions.Expression<System.Func<TElement, T4>> projection4, System.Linq.Expressions.Expression<System.Func<TElement, T5>> projection5, System.Linq.Expressions.Expression<System.Func<TElement, T6>> projection6, System.Linq.Expressions.Expression<System.Func<TElement, T7>> projection7);
        ExRam.Gremlinq.Core.IValueTupleGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8>>> Select<T1, T2, T3, T4, T5, T6, T7, T8>(System.Linq.Expressions.Expression<System.Func<TElement, T1>> projection1, System.Linq.Expressions.Expression<System.Func<TElement, T2>> projection2, System.Linq.Expressions.Expression<System.Func<TElement, T3>> projection3, System.Linq.Expressions.Expression<System.Func<TElement, T4>> projection4, System.Linq.Expressions.Expression<System.Func<TElement, T5>> projection5, System.Linq.Expressions.Expression<System.Func<TElement, T6>> projection6, System.Linq.Expressions.Expression<System.Func<TElement, T7>> projection7, System.Linq.Expressions.Expression<System.Func<TElement, T8>> projection8);
        ExRam.Gremlinq.Core.IValueTupleGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9>>> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9>(System.Linq.Expressions.Expression<System.Func<TElement, T1>> projection1, System.Linq.Expressions.Expression<System.Func<TElement, T2>> projection2, System.Linq.Expressions.Expression<System.Func<TElement, T3>> projection3, System.Linq.Expressions.Expression<System.Func<TElement, T4>> projection4, System.Linq.Expressions.Expression<System.Func<TElement, T5>> projection5, System.Linq.Expressions.Expression<System.Func<TElement, T6>> projection6, System.Linq.Expressions.Expression<System.Func<TElement, T7>> projection7, System.Linq.Expressions.Expression<System.Func<TElement, T8>> projection8, System.Linq.Expressions.Expression<System.Func<TElement, T9>> projection9);
        ExRam.Gremlinq.Core.IValueTupleGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10>>> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(System.Linq.Expressions.Expression<System.Func<TElement, T1>> projection1, System.Linq.Expressions.Expression<System.Func<TElement, T2>> projection2, System.Linq.Expressions.Expression<System.Func<TElement, T3>> projection3, System.Linq.Expressions.Expression<System.Func<TElement, T4>> projection4, System.Linq.Expressions.Expression<System.Func<TElement, T5>> projection5, System.Linq.Expressions.Expression<System.Func<TElement, T6>> projection6, System.Linq.Expressions.Expression<System.Func<TElement, T7>> projection7, System.Linq.Expressions.Expression<System.Func<TElement, T8>> projection8, System.Linq.Expressions.Expression<System.Func<TElement, T9>> projection9, System.Linq.Expressions.Expression<System.Func<TElement, T10>> projection10);
        ExRam.Gremlinq.Core.IValueTupleGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11>>> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(System.Linq.Expressions.Expression<System.Func<TElement, T1>> projection1, System.Linq.Expressions.Expression<System.Func<TElement, T2>> projection2, System.Linq.Expressions.Expression<System.Func<TElement, T3>> projection3, System.Linq.Expressions.Expression<System.Func<TElement, T4>> projection4, System.Linq.Expressions.Expression<System.Func<TElement, T5>> projection5, System.Linq.Expressions.Expression<System.Func<TElement, T6>> projection6, System.Linq.Expressions.Expression<System.Func<TElement, T7>> projection7, System.Linq.Expressions.Expression<System.Func<TElement, T8>> projection8, System.Linq.Expressions.Expression<System.Func<TElement, T9>> projection9, System.Linq.Expressions.Expression<System.Func<TElement, T10>> projection10, System.Linq.Expressions.Expression<System.Func<TElement, T11>> projection11);
        ExRam.Gremlinq.Core.IValueTupleGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12>>> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(System.Linq.Expressions.Expression<System.Func<TElement, T1>> projection1, System.Linq.Expressions.Expression<System.Func<TElement, T2>> projection2, System.Linq.Expressions.Expression<System.Func<TElement, T3>> projection3, System.Linq.Expressions.Expression<System.Func<TElement, T4>> projection4, System.Linq.Expressions.Expression<System.Func<TElement, T5>> projection5, System.Linq.Expressions.Expression<System.Func<TElement, T6>> projection6, System.Linq.Expressions.Expression<System.Func<TElement, T7>> projection7, System.Linq.Expressions.Expression<System.Func<TElement, T8>> projection8, System.Linq.Expressions.Expression<System.Func<TElement, T9>> projection9, System.Linq.Expressions.Expression<System.Func<TElement, T10>> projection10, System.Linq.Expressions.Expression<System.Func<TElement, T11>> projection11, System.Linq.Expressions.Expression<System.Func<TElement, T12>> projection12);
        ExRam.Gremlinq.Core.IValueTupleGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13>>> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(System.Linq.Expressions.Expression<System.Func<TElement, T1>> projection1, System.Linq.Expressions.Expression<System.Func<TElement, T2>> projection2, System.Linq.Expressions.Expression<System.Func<TElement, T3>> projection3, System.Linq.Expressions.Expression<System.Func<TElement, T4>> projection4, System.Linq.Expressions.Expression<System.Func<TElement, T5>> projection5, System.Linq.Expressions.Expression<System.Func<TElement, T6>> projection6, System.Linq.Expressions.Expression<System.Func<TElement, T7>> projection7, System.Linq.Expressions.Expression<System.Func<TElement, T8>> projection8, System.Linq.Expressions.Expression<System.Func<TElement, T9>> projection9, System.Linq.Expressions.Expression<System.Func<TElement, T10>> projection10, System.Linq.Expressions.Expression<System.Func<TElement, T11>> projection11, System.Linq.Expressions.Expression<System.Func<TElement, T12>> projection12, System.Linq.Expressions.Expression<System.Func<TElement, T13>> projection13);
        ExRam.Gremlinq.Core.IValueTupleGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13, T14>>> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(System.Linq.Expressions.Expression<System.Func<TElement, T1>> projection1, System.Linq.Expressions.Expression<System.Func<TElement, T2>> projection2, System.Linq.Expressions.Expression<System.Func<TElement, T3>> projection3, System.Linq.Expressions.Expression<System.Func<TElement, T4>> projection4, System.Linq.Expressions.Expression<System.Func<TElement, T5>> projection5, System.Linq.Expressions.Expression<System.Func<TElement, T6>> projection6, System.Linq.Expressions.Expression<System.Func<TElement, T7>> projection7, System.Linq.Expressions.Expression<System.Func<TElement, T8>> projection8, System.Linq.Expressions.Expression<System.Func<TElement, T9>> projection9, System.Linq.Expressions.Expression<System.Func<TElement, T10>> projection10, System.Linq.Expressions.Expression<System.Func<TElement, T11>> projection11, System.Linq.Expressions.Expression<System.Func<TElement, T12>> projection12, System.Linq.Expressions.Expression<System.Func<TElement, T13>> projection13, System.Linq.Expressions.Expression<System.Func<TElement, T14>> projection14);
        ExRam.Gremlinq.Core.IValueTupleGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13, T14, System.ValueTuple<T15>>>> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(System.Linq.Expressions.Expression<System.Func<TElement, T1>> projection1, System.Linq.Expressions.Expression<System.Func<TElement, T2>> projection2, System.Linq.Expressions.Expression<System.Func<TElement, T3>> projection3, System.Linq.Expressions.Expression<System.Func<TElement, T4>> projection4, System.Linq.Expressions.Expression<System.Func<TElement, T5>> projection5, System.Linq.Expressions.Expression<System.Func<TElement, T6>> projection6, System.Linq.Expressions.Expression<System.Func<TElement, T7>> projection7, System.Linq.Expressions.Expression<System.Func<TElement, T8>> projection8, System.Linq.Expressions.Expression<System.Func<TElement, T9>> projection9, System.Linq.Expressions.Expression<System.Func<TElement, T10>> projection10, System.Linq.Expressions.Expression<System.Func<TElement, T11>> projection11, System.Linq.Expressions.Expression<System.Func<TElement, T12>> projection12, System.Linq.Expressions.Expression<System.Func<TElement, T13>> projection13, System.Linq.Expressions.Expression<System.Func<TElement, T14>> projection14, System.Linq.Expressions.Expression<System.Func<TElement, T15>> projection15);
        ExRam.Gremlinq.Core.IValueTupleGremlinQuery<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13, T14, System.ValueTuple<T15, T16>>>> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
                    System.Linq.Expressions.Expression<System.Func<TElement, T1>> projection1,
                    System.Linq.Expressions.Expression<System.Func<TElement, T2>> projection2,
                    System.Linq.Expressions.Expression<System.Func<TElement, T3>> projection3,
                    System.Linq.Expressions.Expression<System.Func<TElement, T4>> projection4,
                    System.Linq.Expressions.Expression<System.Func<TElement, T5>> projection5,
                    System.Linq.Expressions.Expression<System.Func<TElement, T6>> projection6,
                    System.Linq.Expressions.Expression<System.Func<TElement, T7>> projection7,
                    System.Linq.Expressions.Expression<System.Func<TElement, T8>> projection8,
                    System.Linq.Expressions.Expression<System.Func<TElement, T9>> projection9,
                    System.Linq.Expressions.Expression<System.Func<TElement, T10>> projection10,
                    System.Linq.Expressions.Expression<System.Func<TElement, T11>> projection11,
                    System.Linq.Expressions.Expression<System.Func<TElement, T12>> projection12,
                    System.Linq.Expressions.Expression<System.Func<TElement, T13>> projection13,
                    System.Linq.Expressions.Expression<System.Func<TElement, T14>> projection14,
                    System.Linq.Expressions.Expression<System.Func<TElement, T15>> projection15,
                    System.Linq.Expressions.Expression<System.Func<TElement, T16>> projection16);
    }
    public interface IValueTupleGremlinQuery<TElement> : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IValueTupleGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TElement, ExRam.Gremlinq.Core.IValueTupleGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IValueTupleGremlinQueryBase, ExRam.Gremlinq.Core.IValueTupleGremlinQueryBase<TElement> { }
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
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.VertexProperty<object>, object> Properties(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue>, TValue> Properties<TValue>();
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue>, TValue> Properties<TValue>(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue>, TValue> Properties<TValue>(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue>, TValue> Properties<TValue>(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue>, TValue> Properties<TValue>(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue, TMeta>, TValue, TMeta> Properties<TValue, TMeta>(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue, TMeta>, TValue, TMeta> Properties<TValue, TMeta>(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex> Update(TVertex element);
        ExRam.Gremlinq.Core.IValueGremlinQuery<object> Values(params System.Linq.Expressions.Expression<>[] projections);
        new ExRam.Gremlinq.Core.IValueGremlinQuery<TTarget> Values<TTarget>(params System.Linq.Expressions.Expression<>[] projections);
        new ExRam.Gremlinq.Core.IValueGremlinQuery<TTarget> Values<TTarget>(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IValueGremlinQuery<TTarget> Values<TTarget>(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IValueGremlinQuery<TTarget> Values<TTarget>(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IValueGremlinQuery<TTarget> Values<TTarget, TMeta>(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IValueGremlinQuery<TTarget> Values<TTarget, TMeta>(params System.Linq.Expressions.Expression<>[] projections);
    }
    public interface IVertexGremlinQuery<TVertex> : ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TVertex, ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TVertex>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex>>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TVertex, ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex>>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TVertex>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TVertex, ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TVertex>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IVertexGremlinQueryBase, ExRam.Gremlinq.Core.IVertexGremlinQueryBase<TVertex>
    {
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex> Property<TProjectedValue>(System.Linq.Expressions.Expression<System.Func<TVertex, TProjectedValue[]>> projection, TProjectedValue value);
    }
    public interface IVertexPropertyGremlinQueryBase : ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IElementGremlinQuery<object> Lower();
        ExRam.Gremlinq.Core.IPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.Property<object>> Properties(params string[] keys);
        ExRam.Gremlinq.Core.IValueGremlinQuery<System.Collections.Generic.IDictionary<string, object>> ValueMap(params string[] keys);
        new ExRam.Gremlinq.Core.IValueGremlinQuery<System.Collections.Generic.IDictionary<string, TTarget>> ValueMap<TTarget>();
        ExRam.Gremlinq.Core.IValueGremlinQuery<System.Collections.Generic.IDictionary<string, TTarget>> ValueMap<TTarget>(params string[] keys);
        ExRam.Gremlinq.Core.IValueGremlinQuery<object> Values(params string[] keys);
        new ExRam.Gremlinq.Core.IValueGremlinQuery<TTarget> Values<TTarget>();
        ExRam.Gremlinq.Core.IValueGremlinQuery<TTarget> Values<TTarget>(params string[] keys);
    }
    public interface IVertexPropertyGremlinQueryBase<TProperty, TValue> : ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TProperty>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TProperty>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IElementGremlinQuery<TProperty> Lower();
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue, TMeta>, TValue, TMeta> Meta<TMeta>();
        ExRam.Gremlinq.Core.IPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.Property<TMetaValue>> Properties<TMetaValue>(params string[] keys);
        ExRam.Gremlinq.Core.IValueGremlinQuery<TValue> Value();
    }
    public interface IVertexPropertyGremlinQueryBase<TProperty, TValue, TMeta> : ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TProperty>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TProperty>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IElementGremlinQuery<TProperty> Lower();
        ExRam.Gremlinq.Core.IPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.Property<TTarget>> Properties<TTarget>(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Property<TMetaValue>(System.Linq.Expressions.Expression<System.Func<TMeta, TMetaValue>> projection, TMetaValue value);
        ExRam.Gremlinq.Core.IValueGremlinQuery<TValue> Value();
        ExRam.Gremlinq.Core.IValueGremlinQuery<TMeta> ValueMap();
        ExRam.Gremlinq.Core.IValueGremlinQuery<TMetaValue> Values<TMetaValue>(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Where(System.Linq.Expressions.Expression<System.Func<ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue, TMeta>, bool>> predicate);
    }
    public interface IVertexPropertyGremlinQuery<TProperty, TValue> : ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue>>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TProperty, ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue>>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TProperty>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TProperty, ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TProperty>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBase, ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBase<TProperty, TValue> { }
    public interface IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> : ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TProperty, ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TProperty>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TProperty, ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TProperty>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBase, ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBase<TProperty, TValue, TMeta> { }
    public sealed class IdStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.IdStep Instance;
        public IdStep() { }
    }
    public sealed class IdentityStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.IdentityStep Instance;
        public IdentityStep() { }
    }
    public static class ImmutableDictionaryExtensions
    {
        public static System.Collections.Immutable.IImmutableDictionary<System.Reflection.MemberInfo, ExRam.Gremlinq.Core.MemberMetadata> UseCamelCaseNames(this System.Collections.Immutable.IImmutableDictionary<System.Reflection.MemberInfo, ExRam.Gremlinq.Core.MemberMetadata> names) { }
        public static System.Collections.Immutable.IImmutableDictionary<System.Reflection.MemberInfo, ExRam.Gremlinq.Core.MemberMetadata> UseLowerCaseNames(this System.Collections.Immutable.IImmutableDictionary<System.Reflection.MemberInfo, ExRam.Gremlinq.Core.MemberMetadata> names) { }
    }
    public sealed class InEStep : ExRam.Gremlinq.Core.DerivedLabelNamesStep
    {
        public static readonly ExRam.Gremlinq.Core.InEStep Empty;
        public InEStep(System.Collections.Immutable.ImmutableArray<string> labels) { }
    }
    public sealed class InStep : ExRam.Gremlinq.Core.DerivedLabelNamesStep
    {
        public static readonly ExRam.Gremlinq.Core.InStep Empty;
        public InStep(System.Collections.Immutable.ImmutableArray<string> labels) { }
    }
    public sealed class InVStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.InVStep Instance;
        public InVStep() { }
    }
    public sealed class InjectStep : ExRam.Gremlinq.Core.Step
    {
        public InjectStep(System.Collections.Immutable.ImmutableArray<object> elements) { }
        public System.Collections.Immutable.ImmutableArray<object> Elements { get; }
    }
    public sealed class IsStep : ExRam.Gremlinq.Core.Step
    {
        public IsStep(Gremlin.Net.Process.Traversal.P predicate) { }
        public Gremlin.Net.Process.Traversal.P Predicate { get; }
    }
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
    public sealed class KeyStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.KeyStep Instance;
        public KeyStep() { }
    }
    public sealed class LabelStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.LabelStep Instance;
        public LabelStep() { }
    }
    public sealed class LimitStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.LimitStep LimitGlobal1;
        public static readonly ExRam.Gremlinq.Core.LimitStep LimitLocal1;
        public LimitStep(long count, Gremlin.Net.Process.Traversal.Scope scope) { }
        public long Count { get; }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
    }
    public sealed class LocalStep : ExRam.Gremlinq.Core.Step
    {
        public LocalStep(ExRam.Gremlinq.Core.Traversal traversal) { }
        public ExRam.Gremlinq.Core.Traversal Traversal { get; }
    }
    public abstract class LogicalStep<TStep> : ExRam.Gremlinq.Core.Step
        where TStep : ExRam.Gremlinq.Core.LogicalStep<TStep>
    {
        protected LogicalStep(string name, System.Collections.Generic.IEnumerable<ExRam.Gremlinq.Core.Traversal> traversals) { }
        public string Name { get; }
        public System.Collections.Immutable.ImmutableArray<ExRam.Gremlinq.Core.Traversal> Traversals { get; }
    }
    public sealed class MapStep : ExRam.Gremlinq.Core.Step
    {
        public MapStep(ExRam.Gremlinq.Core.Traversal traversal) { }
        public ExRam.Gremlinq.Core.Traversal Traversal { get; }
    }
    public sealed class MatchStep : ExRam.Gremlinq.Core.MultiTraversalArgumentStep
    {
        public MatchStep(System.Collections.Immutable.ImmutableArray<ExRam.Gremlinq.Core.Traversal> traversals) { }
    }
    public sealed class MaxStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.MaxStep Global;
        public static readonly ExRam.Gremlinq.Core.MaxStep Local;
        public MaxStep(Gremlin.Net.Process.Traversal.Scope scope) { }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
    }
    public sealed class MeanStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.MeanStep Global;
        public static readonly ExRam.Gremlinq.Core.MeanStep Local;
        public MeanStep(Gremlin.Net.Process.Traversal.Scope scope) { }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
    }
    public readonly struct MemberMetadata
    {
        public MemberMetadata(ExRam.Gremlinq.Core.Key key, ExRam.Gremlinq.Core.SerializationBehaviour serializationBehaviour = 0) { }
        public ExRam.Gremlinq.Core.Key Key { get; }
        public ExRam.Gremlinq.Core.SerializationBehaviour SerializationBehaviour { get; }
    }
    public sealed class MinStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.MinStep Global;
        public static readonly ExRam.Gremlinq.Core.MinStep Local;
        public MinStep(Gremlin.Net.Process.Traversal.Scope scope) { }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
    }
    public abstract class MultiTraversalArgumentStep : ExRam.Gremlinq.Core.Step
    {
        protected MultiTraversalArgumentStep(System.Collections.Immutable.ImmutableArray<ExRam.Gremlinq.Core.Traversal> traversals) { }
        public System.Collections.Immutable.ImmutableArray<ExRam.Gremlinq.Core.Traversal> Traversals { get; }
    }
    public sealed class NoneStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.NoneStep Instance;
        public NoneStep() { }
    }
    public sealed class NotStep : ExRam.Gremlinq.Core.Step
    {
        public NotStep(ExRam.Gremlinq.Core.Traversal traversal) { }
        public ExRam.Gremlinq.Core.Traversal Traversal { get; }
    }
    public sealed class OptionTraversalStep : ExRam.Gremlinq.Core.Step
    {
        public OptionTraversalStep(object? guard, ExRam.Gremlinq.Core.Traversal optionTraversal) { }
        public object? Guard { get; }
        public ExRam.Gremlinq.Core.Traversal OptionTraversal { get; }
    }
    public sealed class OptionalStep : ExRam.Gremlinq.Core.Step
    {
        public OptionalStep(ExRam.Gremlinq.Core.Traversal traversal) { }
        public ExRam.Gremlinq.Core.Traversal Traversal { get; }
    }
    public sealed class OrStep : ExRam.Gremlinq.Core.LogicalStep<ExRam.Gremlinq.Core.OrStep>
    {
        public static readonly ExRam.Gremlinq.Core.OrStep Infix;
        public OrStep(System.Collections.Generic.IEnumerable<ExRam.Gremlinq.Core.Traversal> traversals) { }
    }
    public sealed class OrderStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.OrderStep Global;
        public static readonly ExRam.Gremlinq.Core.OrderStep Local;
        public OrderStep(Gremlin.Net.Process.Traversal.Scope scope) { }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
        public sealed class ByLambdaStep : ExRam.Gremlinq.Core.OrderStep.ByStep
        {
            public ByLambdaStep(Gremlin.Net.Process.Traversal.ILambda lambda) { }
            public Gremlin.Net.Process.Traversal.ILambda Lambda { get; }
        }
        public sealed class ByMemberStep : ExRam.Gremlinq.Core.OrderStep.ByStep
        {
            public ByMemberStep(ExRam.Gremlinq.Core.Key key, Gremlin.Net.Process.Traversal.Order order) { }
            public ExRam.Gremlinq.Core.Key Key { get; }
            public Gremlin.Net.Process.Traversal.Order Order { get; }
        }
        public abstract class ByStep : ExRam.Gremlinq.Core.Step
        {
            protected ByStep() { }
        }
        public sealed class ByTraversalStep : ExRam.Gremlinq.Core.OrderStep.ByStep
        {
            public ByTraversalStep(ExRam.Gremlinq.Core.Traversal traversal, Gremlin.Net.Process.Traversal.Order order) { }
            public Gremlin.Net.Process.Traversal.Order Order { get; }
            public ExRam.Gremlinq.Core.Traversal Traversal { get; }
        }
    }
    public sealed class OtherVStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.OtherVStep Instance;
        public OtherVStep() { }
    }
    public sealed class OutEStep : ExRam.Gremlinq.Core.DerivedLabelNamesStep
    {
        public static readonly ExRam.Gremlinq.Core.OutEStep Empty;
        public OutEStep(System.Collections.Immutable.ImmutableArray<string> labels) { }
    }
    public sealed class OutStep : ExRam.Gremlinq.Core.DerivedLabelNamesStep
    {
        public static readonly ExRam.Gremlinq.Core.OutStep Empty;
        public OutStep(System.Collections.Immutable.ImmutableArray<string> labels) { }
    }
    public sealed class OutVStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.OutVStep Instance;
        public OutVStep() { }
    }
    public sealed class PathStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.PathStep Instance;
        public PathStep() { }
    }
    public sealed class ProfileStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.ProfileStep Instance;
        public ProfileStep() { }
    }
    public sealed class ProjectStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.ProjectStep Empty;
        public ProjectStep(System.Collections.Immutable.ImmutableArray<string> projections) { }
        public System.Collections.Immutable.ImmutableArray<string> Projections { get; }
        public sealed class ByKeyStep : ExRam.Gremlinq.Core.ProjectStep.ByStep
        {
            public ByKeyStep(ExRam.Gremlinq.Core.Key key) { }
            public ExRam.Gremlinq.Core.Key Key { get; }
        }
        public abstract class ByStep : ExRam.Gremlinq.Core.Step
        {
            protected ByStep() { }
        }
        public sealed class ByTraversalStep : ExRam.Gremlinq.Core.ProjectStep.ByStep
        {
            public ByTraversalStep(ExRam.Gremlinq.Core.Traversal traversal) { }
            public ExRam.Gremlinq.Core.Traversal Traversal { get; }
        }
    }
    public sealed class PropertiesStep : ExRam.Gremlinq.Core.Step
    {
        public PropertiesStep(System.Collections.Immutable.ImmutableArray<string> keys) { }
        public System.Collections.Immutable.ImmutableArray<string> Keys { get; }
    }
    public abstract class PropertyStep : ExRam.Gremlinq.Core.Step
    {
        protected PropertyStep(object value, Gremlin.Net.Process.Traversal.Cardinality? cardinality = null) { }
        protected PropertyStep(object value, System.Collections.Immutable.ImmutableArray<System.Collections.Generic.KeyValuePair<string, object>> metaProperties, Gremlin.Net.Process.Traversal.Cardinality? cardinality = null) { }
        public Gremlin.Net.Process.Traversal.Cardinality? Cardinality { get; }
        public System.Collections.Immutable.ImmutableArray<System.Collections.Generic.KeyValuePair<string, object>> MetaProperties { get; }
        public object Value { get; }
        public sealed class ByKeyStep : ExRam.Gremlinq.Core.PropertyStep
        {
            public ByKeyStep(ExRam.Gremlinq.Core.Key key, object value, Gremlin.Net.Process.Traversal.Cardinality? cardinality = null) { }
            public ByKeyStep(ExRam.Gremlinq.Core.Key key, object value, System.Collections.Immutable.ImmutableArray<System.Collections.Generic.KeyValuePair<string, object>> metaProperties, Gremlin.Net.Process.Traversal.Cardinality? cardinality = null) { }
            public ExRam.Gremlinq.Core.Key Key { get; }
        }
        public sealed class ByTraversalStep : ExRam.Gremlinq.Core.PropertyStep
        {
            public ByTraversalStep(ExRam.Gremlinq.Core.Traversal traversal, object value, Gremlin.Net.Process.Traversal.Cardinality? cardinality = null) { }
            public ByTraversalStep(ExRam.Gremlinq.Core.Traversal traversal, object value, System.Collections.Immutable.ImmutableArray<System.Collections.Generic.KeyValuePair<string, object>> metaProperties, Gremlin.Net.Process.Traversal.Cardinality? cardinality = null) { }
            public ExRam.Gremlinq.Core.Traversal Traversal { get; }
        }
    }
    [System.Flags]
    public enum QueryLogVerbosity
    {
        QueryOnly = 0,
        IncludeBindings = 1,
    }
    public readonly struct QuerySemantics : System.IEquatable<ExRam.Gremlinq.Core.QuerySemantics>
    {
        public QuerySemantics(System.Type semantics) { }
        public System.Type QueryType { get; }
        public bool Equals(ExRam.Gremlinq.Core.QuerySemantics other) { }
        public override bool Equals(object? obj) { }
        public override int GetHashCode() { }
        public override string ToString() { }
        public static ExRam.Gremlinq.Core.QuerySemantics op_Implicit(System.Type type) { }
        public static bool operator !=(ExRam.Gremlinq.Core.QuerySemantics left, ExRam.Gremlinq.Core.QuerySemantics right) { }
        public static bool operator ==(ExRam.Gremlinq.Core.QuerySemantics left, ExRam.Gremlinq.Core.QuerySemantics right) { }
    }
    public sealed class RangeStep : ExRam.Gremlinq.Core.Step
    {
        public RangeStep(long lower, long upper, Gremlin.Net.Process.Traversal.Scope scope) { }
        public long Lower { get; }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
        public long Upper { get; }
    }
    public sealed class RepeatStep : ExRam.Gremlinq.Core.Step
    {
        public RepeatStep(ExRam.Gremlinq.Core.Traversal traversal) { }
        public ExRam.Gremlinq.Core.Traversal Traversal { get; }
    }
    public sealed class SelectKeysStep : ExRam.Gremlinq.Core.Step
    {
        public SelectKeysStep(System.Collections.Immutable.ImmutableArray<ExRam.Gremlinq.Core.Key> keys) { }
        public System.Collections.Immutable.ImmutableArray<ExRam.Gremlinq.Core.Key> Keys { get; }
    }
    public sealed class SelectStep : ExRam.Gremlinq.Core.Step
    {
        public SelectStep(System.Collections.Immutable.ImmutableArray<ExRam.Gremlinq.Core.StepLabel> stepLabels) { }
        public System.Collections.Immutable.ImmutableArray<ExRam.Gremlinq.Core.StepLabel> StepLabels { get; }
    }
    [System.Flags]
    public enum SerializationBehaviour
    {
        Default = 0,
        IgnoreOnAdd = 1,
        IgnoreOnUpdate = 2,
        IgnoreAlways = 3,
    }
    public enum SerializationFormat
    {
        GraphSonV2 = 0,
        GraphSonV3 = 1,
    }
    public sealed class SideEffectStep : ExRam.Gremlinq.Core.Step
    {
        public SideEffectStep(ExRam.Gremlinq.Core.Traversal traversal) { }
        public ExRam.Gremlinq.Core.Traversal Traversal { get; }
    }
    public sealed class SimplePathStep : ExRam.Gremlinq.Core.Step
    {
        public SimplePathStep() { }
    }
    public sealed class SkipStep : ExRam.Gremlinq.Core.Step
    {
        public SkipStep(long count, Gremlin.Net.Process.Traversal.Scope scope) { }
        public long Count { get; }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
    }
    public abstract class Step
    {
        protected Step() { }
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
        public static TElement op_Implicit(ExRam.Gremlinq.Core.StepLabel<TElement>? stepLabel) { }
        public new static ExRam.Gremlinq.Core.StepLabel<TElement> op_Implicit(string str) { }
        public static bool operator !=(ExRam.Gremlinq.Core.StepLabel<TElement>? b, TElement? a) { }
        public static bool operator !=(TElement? a, ExRam.Gremlinq.Core.StepLabel<TElement>? b) { }
        public static bool operator ==(ExRam.Gremlinq.Core.StepLabel<TElement>? b, TElement? a) { }
        public static bool operator ==(TElement? a, ExRam.Gremlinq.Core.StepLabel<TElement>? b) { }
    }
    public class StepLabel<TQuery, TElement> : ExRam.Gremlinq.Core.StepLabel<TElement>
        where TQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        public StepLabel() { }
        [System.Runtime.CompilerServices.PreserveBaseOverrides]
        public virtual ExRam.Gremlinq.Core.StepLabel<ExRam.Gremlinq.Core.IValueGremlinQuery<TNewValue>, TNewValue> Cast<TNewValue>() { }
    }
    public readonly struct StepStack : System.Collections.Generic.IEnumerable<ExRam.Gremlinq.Core.Step>, System.Collections.Generic.IReadOnlyCollection<ExRam.Gremlinq.Core.Step>, System.Collections.Generic.IReadOnlyList<ExRam.Gremlinq.Core.Step>, System.Collections.IEnumerable
    {
        public static readonly ExRam.Gremlinq.Core.StepStack Empty;
        public int Count { get; }
        public ExRam.Gremlinq.Core.Step this[int index] { get; }
        public System.Collections.Generic.IEnumerator<ExRam.Gremlinq.Core.Step> GetEnumerator() { }
        public ExRam.Gremlinq.Core.StepStack Pop() { }
        public ExRam.Gremlinq.Core.StepStack Pop(out ExRam.Gremlinq.Core.Step poppedStep) { }
        public ExRam.Gremlinq.Core.StepStack Push(ExRam.Gremlinq.Core.Step step) { }
    }
    public enum StringComparisonTranslationStrictness
    {
        Strict = 0,
        Lenient = 1,
    }
    public sealed class SumStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.SumStep Global;
        public static readonly ExRam.Gremlinq.Core.SumStep Local;
        public SumStep(Gremlin.Net.Process.Traversal.Scope scope) { }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
    }
    public sealed class TailStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.TailStep TailGlobal1;
        public static readonly ExRam.Gremlinq.Core.TailStep TailLocal1;
        public TailStep(long count, Gremlin.Net.Process.Traversal.Scope scope) { }
        public long Count { get; }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
    }
    public sealed class TimesStep : ExRam.Gremlinq.Core.Step
    {
        public TimesStep(int count) { }
        public int Count { get; }
    }
    public readonly struct Traversal : System.Collections.Generic.IEnumerable<ExRam.Gremlinq.Core.Step>, System.Collections.Generic.IReadOnlyCollection<ExRam.Gremlinq.Core.Step>, System.Collections.Generic.IReadOnlyList<ExRam.Gremlinq.Core.Step>, System.Collections.IEnumerable
    {
        public Traversal(System.Collections.Generic.IEnumerable<ExRam.Gremlinq.Core.Step> steps) { }
        public Traversal(System.Collections.Immutable.ImmutableArray<ExRam.Gremlinq.Core.Step> steps) { }
        public int Count { get; }
        public ExRam.Gremlinq.Core.Step this[int index] { get; }
        public System.Collections.Generic.IEnumerator<ExRam.Gremlinq.Core.Step> GetEnumerator() { }
        public static ExRam.Gremlinq.Core.Traversal op_Implicit(ExRam.Gremlinq.Core.Step step) { }
        public static ExRam.Gremlinq.Core.Traversal op_Implicit(ExRam.Gremlinq.Core.Step[] steps) { }
        public static ExRam.Gremlinq.Core.Traversal op_Implicit(System.Collections.Immutable.ImmutableArray<ExRam.Gremlinq.Core.Step> steps) { }
    }
    public sealed class UnfoldStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.UnfoldStep Instance;
        public UnfoldStep() { }
    }
    public sealed class UnionStep : ExRam.Gremlinq.Core.MultiTraversalArgumentStep
    {
        public UnionStep(System.Collections.Immutable.ImmutableArray<ExRam.Gremlinq.Core.Traversal> traversals) { }
    }
    public sealed class UntilStep : ExRam.Gremlinq.Core.Step
    {
        public UntilStep(ExRam.Gremlinq.Core.Traversal traversal) { }
        public ExRam.Gremlinq.Core.Traversal Traversal { get; }
    }
    public sealed class VStep : ExRam.Gremlinq.Core.Step
    {
        public VStep(System.Collections.Immutable.ImmutableArray<object> ids) { }
        public System.Collections.Immutable.ImmutableArray<object> Ids { get; }
    }
    public sealed class ValueMapStep : ExRam.Gremlinq.Core.Step
    {
        public ValueMapStep(System.Collections.Immutable.ImmutableArray<string> keys) { }
        public System.Collections.Immutable.ImmutableArray<string> Keys { get; }
    }
    public sealed class ValueStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.ValueStep Instance;
        public ValueStep() { }
    }
    public sealed class ValuesStep : ExRam.Gremlinq.Core.Step
    {
        public ValuesStep(System.Collections.Immutable.ImmutableArray<string> keys) { }
        public System.Collections.Immutable.ImmutableArray<string> Keys { get; }
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
    public sealed class WherePredicateStep : ExRam.Gremlinq.Core.Step
    {
        public WherePredicateStep(Gremlin.Net.Process.Traversal.P predicate) { }
        public Gremlin.Net.Process.Traversal.P Predicate { get; }
        public sealed class ByMemberStep : ExRam.Gremlinq.Core.Step
        {
            public ByMemberStep(ExRam.Gremlinq.Core.Key? key = default) { }
            public ExRam.Gremlinq.Core.Key? Key { get; }
        }
    }
    public sealed class WhereStepLabelAndPredicateStep : ExRam.Gremlinq.Core.Step
    {
        public WhereStepLabelAndPredicateStep(ExRam.Gremlinq.Core.StepLabel stepLabel, Gremlin.Net.Process.Traversal.P predicate) { }
        public Gremlin.Net.Process.Traversal.P Predicate { get; }
        public ExRam.Gremlinq.Core.StepLabel StepLabel { get; }
    }
    public sealed class WhereTraversalStep : ExRam.Gremlinq.Core.Step
    {
        public WhereTraversalStep(ExRam.Gremlinq.Core.Traversal traversal) { }
        public ExRam.Gremlinq.Core.Traversal Traversal { get; }
    }
    public sealed class WithSideEffectStep : ExRam.Gremlinq.Core.Step
    {
        public WithSideEffectStep(ExRam.Gremlinq.Core.StepLabel label, object value) { }
        public ExRam.Gremlinq.Core.StepLabel Label { get; }
        public object Value { get; }
    }
    public sealed class WithStrategiesStep : ExRam.Gremlinq.Core.Step
    {
        public WithStrategiesStep(ExRam.Gremlinq.Core.Traversal traversal) { }
        public ExRam.Gremlinq.Core.Traversal Traversal { get; }
    }
    public sealed class WithoutStrategiesStep : ExRam.Gremlinq.Core.Step
    {
        public WithoutStrategiesStep(System.Collections.Immutable.ImmutableArray<System.Type> strategyTypes) { }
        public System.Collections.Immutable.ImmutableArray<System.Type> StrategyTypes { get; }
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
        public static ExRam.Gremlinq.Core.GraphElements.Property<TValue> op_Implicit(ExRam.Gremlinq.Core.GraphElements.Property<>[] value) { }
        public static ExRam.Gremlinq.Core.GraphElements.Property<TValue> op_Implicit(TValue value) { }
        public static ExRam.Gremlinq.Core.GraphElements.Property<TValue> op_Implicit(TValue[] value) { }
    }
    public class VertexProperty<TValue> : ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue, System.Collections.Generic.IDictionary<string, object>>
    {
        public VertexProperty(TValue value) { }
        protected override System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, object>> GetProperties(ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment) { }
        public static ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue> op_Implicit(ExRam.Gremlinq.Core.GraphElements.VertexProperty<>[] value) { }
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
        public static ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue, TMeta> op_Implicit(ExRam.Gremlinq.Core.GraphElements.VertexProperty<, >[] value) { }
        public static ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue, TMeta> op_Implicit(TValue value) { }
        public static ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue, TMeta> op_Implicit(TValue[] value) { }
    }
}