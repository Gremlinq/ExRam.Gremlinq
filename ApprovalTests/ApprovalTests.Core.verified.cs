[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("ExRam.Gremlinq.Core.Tests")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.1", FrameworkDisplayName="")]
namespace ExRam.Gremlinq.Core
{
    public sealed class AddEStep : ExRam.Gremlinq.Core.AddElementStep
    {
        public AddEStep(ExRam.Gremlinq.Core.IGraphModel model, object value) { }
        public sealed class ToLabelStep : ExRam.Gremlinq.Core.Step
        {
            public ToLabelStep(ExRam.Gremlinq.Core.StepLabel stepLabel) { }
            public ExRam.Gremlinq.Core.StepLabel StepLabel { get; }
        }
        public sealed class ToTraversalStep : ExRam.Gremlinq.Core.SingleTraversalArgumentStep
        {
            public ToTraversalStep(ExRam.Gremlinq.Core.IGremlinQueryBase traversal) { }
        }
    }
    public abstract class AddElementStep : ExRam.Gremlinq.Core.Step
    {
        protected AddElementStep(ExRam.Gremlinq.Core.IGraphElementModel elementModel, object value) { }
        public string Label { get; }
    }
    public sealed class AddVStep : ExRam.Gremlinq.Core.AddElementStep
    {
        public AddVStep(ExRam.Gremlinq.Core.IGraphModel model, object value) { }
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
        public AndStep(ExRam.Gremlinq.Core.IGremlinQueryBase[] traversals) { }
    }
    public sealed class AsStep : ExRam.Gremlinq.Core.Step
    {
        public AsStep(ExRam.Gremlinq.Core.StepLabel[] stepLabels) { }
        public ExRam.Gremlinq.Core.StepLabel[] StepLabels { get; }
    }
    public sealed class BarrierStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.BarrierStep Instance;
        public BarrierStep() { }
    }
    public sealed class BothEStep : ExRam.Gremlinq.Core.DerivedLabelNamesStep
    {
        public static readonly ExRam.Gremlinq.Core.BothEStep NoLabels;
        public BothEStep(string[] labels) { }
    }
    public sealed class BothStep : ExRam.Gremlinq.Core.DerivedLabelNamesStep
    {
        public static readonly ExRam.Gremlinq.Core.BothStep NoLabels;
        public BothStep(string[] labels) { }
    }
    public sealed class BothVStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.BothVStep Instance;
        public BothVStep() { }
    }
    public static class BytecodeExtensions
    {
        public static ExRam.Gremlinq.Core.GroovyGremlinQuery ToGroovy(this Gremlin.Net.Process.Traversal.Bytecode bytecode) { }
    }
    public sealed class CapStep : ExRam.Gremlinq.Core.Step
    {
        public CapStep(ExRam.Gremlinq.Core.StepLabel stepLabel) { }
        public ExRam.Gremlinq.Core.StepLabel StepLabel { get; }
    }
    public sealed class ChooseOptionTraversalStep : ExRam.Gremlinq.Core.SingleTraversalArgumentStep
    {
        public ChooseOptionTraversalStep(ExRam.Gremlinq.Core.IGremlinQueryBase chooseTraversal) { }
    }
    public sealed class ChoosePredicateStep : ExRam.Gremlinq.Core.ChooseStep
    {
        public ChoosePredicateStep(Gremlin.Net.Process.Traversal.P predicate, ExRam.Gremlinq.Core.IGremlinQueryBase thenTraversal, ExRam.Gremlinq.Core.IGremlinQueryBase? elseTraversal = null) { }
        public Gremlin.Net.Process.Traversal.P Predicate { get; }
    }
    public abstract class ChooseStep : ExRam.Gremlinq.Core.Step
    {
        protected ChooseStep(ExRam.Gremlinq.Core.IGremlinQueryBase thenTraversal, ExRam.Gremlinq.Core.IGremlinQueryBase? elseTraversal = null) { }
        public ExRam.Gremlinq.Core.IGremlinQueryBase? ElseTraversal { get; }
        public ExRam.Gremlinq.Core.IGremlinQueryBase ThenTraversal { get; }
    }
    public sealed class ChooseTraversalStep : ExRam.Gremlinq.Core.ChooseStep
    {
        public ChooseTraversalStep(ExRam.Gremlinq.Core.IGremlinQueryBase ifTraversal, ExRam.Gremlinq.Core.IGremlinQueryBase thenTraversal, ExRam.Gremlinq.Core.IGremlinQueryBase? elseTraversal = null) { }
        public ExRam.Gremlinq.Core.IGremlinQueryBase IfTraversal { get; }
    }
    public sealed class CoalesceStep : ExRam.Gremlinq.Core.MultiTraversalArgumentStep
    {
        public CoalesceStep(ExRam.Gremlinq.Core.IGremlinQueryBase[] traversals) { }
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
    public sealed class DedupStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.DedupStep Global;
        public static readonly ExRam.Gremlinq.Core.DedupStep Local;
        public DedupStep(Gremlin.Net.Process.Traversal.Scope scope) { }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
    }
    public abstract class DerivedLabelNamesStep : ExRam.Gremlinq.Core.Step
    {
        protected DerivedLabelNamesStep(string[] labels) { }
        public string[] Labels { get; }
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
    public sealed class EStep : ExRam.Gremlinq.Core.FullScanStep
    {
        public EStep(object[] ids) { }
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
        All = 255,
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
        BooleanValues = 65536,
        ByteValues = 131072,
        DoubleValues = 262144,
        IntegerValues = 524288,
        All = 255,
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
    public static class EnumerableExtensions { }
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
    public readonly struct FeatureSet
    {
        public static ExRam.Gremlinq.Core.FeatureSet Full;
        public FeatureSet(ExRam.Gremlinq.Core.GraphFeatures graphFeatures, ExRam.Gremlinq.Core.VariableFeatures variableFeatures, ExRam.Gremlinq.Core.VertexFeatures vertexFeatures, ExRam.Gremlinq.Core.VertexPropertyFeatures vertexPropertyFeatures, ExRam.Gremlinq.Core.EdgeFeatures edgeFeatures, ExRam.Gremlinq.Core.EdgePropertyFeatures edgePropertyFeatures) { }
        public ExRam.Gremlinq.Core.EdgeFeatures EdgeFeatures { get; }
        public ExRam.Gremlinq.Core.EdgePropertyFeatures EdgePropertyFeatures { get; }
        public ExRam.Gremlinq.Core.GraphFeatures GraphFeatures { get; }
        public ExRam.Gremlinq.Core.VariableFeatures VariableFeatures { get; }
        public ExRam.Gremlinq.Core.VertexFeatures VertexFeatures { get; }
        public ExRam.Gremlinq.Core.VertexPropertyFeatures VertexPropertyFeatures { get; }
        public ExRam.Gremlinq.Core.FeatureSet ConfigureEdgeFeatures(System.Func<ExRam.Gremlinq.Core.EdgeFeatures, ExRam.Gremlinq.Core.EdgeFeatures> config) { }
        public ExRam.Gremlinq.Core.FeatureSet ConfigureEdgePropertyFeatures(System.Func<ExRam.Gremlinq.Core.EdgePropertyFeatures, ExRam.Gremlinq.Core.EdgePropertyFeatures> config) { }
        public ExRam.Gremlinq.Core.FeatureSet ConfigureGraphFeatures(System.Func<ExRam.Gremlinq.Core.GraphFeatures, ExRam.Gremlinq.Core.GraphFeatures> config) { }
        public ExRam.Gremlinq.Core.FeatureSet ConfigureVariableFeatures(System.Func<ExRam.Gremlinq.Core.VariableFeatures, ExRam.Gremlinq.Core.VariableFeatures> config) { }
        public ExRam.Gremlinq.Core.FeatureSet ConfigureVertexFeatures(System.Func<ExRam.Gremlinq.Core.VertexFeatures, ExRam.Gremlinq.Core.VertexFeatures> config) { }
        public ExRam.Gremlinq.Core.FeatureSet ConfigureVertexPropertyFeatures(System.Func<ExRam.Gremlinq.Core.VertexPropertyFeatures, ExRam.Gremlinq.Core.VertexPropertyFeatures> config) { }
    }
    public static class FeatureSetExtensions
    {
        public static bool Supports(this ExRam.Gremlinq.Core.FeatureSet featureSet, ExRam.Gremlinq.Core.EdgeFeatures edgeFeatures) { }
        public static bool Supports(this ExRam.Gremlinq.Core.FeatureSet featureSet, ExRam.Gremlinq.Core.EdgePropertyFeatures edgePropertyFeatures) { }
        public static bool Supports(this ExRam.Gremlinq.Core.FeatureSet featureSet, ExRam.Gremlinq.Core.GraphFeatures graphFeatures) { }
        public static bool Supports(this ExRam.Gremlinq.Core.FeatureSet featureSet, ExRam.Gremlinq.Core.VariableFeatures variableFeatures) { }
        public static bool Supports(this ExRam.Gremlinq.Core.FeatureSet featureSet, ExRam.Gremlinq.Core.VertexFeatures vertexFeatures) { }
        public static bool Supports(this ExRam.Gremlinq.Core.FeatureSet featureSet, ExRam.Gremlinq.Core.VertexPropertyFeatures vertexPropertyFeatures) { }
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
    public sealed class FlatMapStep : ExRam.Gremlinq.Core.SingleTraversalArgumentStep
    {
        public FlatMapStep(ExRam.Gremlinq.Core.IGremlinQueryBase traversal) { }
    }
    public sealed class FoldStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.FoldStep Instance;
        public FoldStep() { }
    }
    public sealed class FromLabelStep : ExRam.Gremlinq.Core.Step
    {
        public FromLabelStep(ExRam.Gremlinq.Core.StepLabel stepLabel) { }
        public ExRam.Gremlinq.Core.StepLabel StepLabel { get; }
    }
    public sealed class FromTraversalStep : ExRam.Gremlinq.Core.SingleTraversalArgumentStep
    {
        public FromTraversalStep(ExRam.Gremlinq.Core.IGremlinQueryBase traversal) { }
    }
    public abstract class FullScanStep : ExRam.Gremlinq.Core.Step
    {
        protected FullScanStep(object[] ids) { }
        public object[] Ids { get; }
    }
    public static class GraphElementModel
    {
        public static readonly ExRam.Gremlinq.Core.IGraphElementModel Empty;
        public static ExRam.Gremlinq.Core.IGraphElementModel ConfigureLabels(this ExRam.Gremlinq.Core.IGraphElementModel model, System.Func<System.Type, string, string> overrideTransformation) { }
        public static ExRam.Gremlinq.Core.IGraphElementModel FromBaseType(System.Type baseType, System.Collections.Generic.IEnumerable<System.Reflection.Assembly>? assemblies, Microsoft.Extensions.Logging.ILogger? logger) { }
        public static ExRam.Gremlinq.Core.IGraphElementModel FromBaseType<TType>(System.Collections.Generic.IEnumerable<System.Reflection.Assembly>? assemblies, Microsoft.Extensions.Logging.ILogger? logger) { }
        public static ExRam.Gremlinq.Core.IGraphElementModel FromTypes(System.Collections.Generic.IEnumerable<System.Type> types) { }
        public static string[]? TryGetFilterLabels(this ExRam.Gremlinq.Core.IGraphElementModel model, System.Type type, ExRam.Gremlinq.Core.FilterLabelsVerbosity verbosity) { }
        public static ExRam.Gremlinq.Core.IGraphElementModel UseCamelCaseLabels(this ExRam.Gremlinq.Core.IGraphElementModel model) { }
        public static ExRam.Gremlinq.Core.IGraphElementModel UseLowerCaseLabels(this ExRam.Gremlinq.Core.IGraphElementModel model) { }
    }
    public static class GraphElementPropertyModel
    {
        public static readonly ExRam.Gremlinq.Core.IGraphElementPropertyModel Default;
        public static ExRam.Gremlinq.Core.IGraphElementPropertyModel ConfigureElement<TElement>(this ExRam.Gremlinq.Core.IGraphElementPropertyModel model, System.Func<ExRam.Gremlinq.Core.IPropertyMetadataConfigurator<TElement>, System.Collections.Immutable.IImmutableDictionary<System.Reflection.MemberInfo, ExRam.Gremlinq.Core.PropertyMetadata>> action)
            where TElement :  class { }
        public static ExRam.Gremlinq.Core.IGraphElementPropertyModel ConfigureNames(this ExRam.Gremlinq.Core.IGraphElementPropertyModel model, System.Func<System.Reflection.MemberInfo, string, string> overrideTransformation) { }
        public static ExRam.Gremlinq.Core.IGraphElementPropertyModel UseCamelCaseNames(this ExRam.Gremlinq.Core.IGraphElementPropertyModel model) { }
        public static ExRam.Gremlinq.Core.IGraphElementPropertyModel UseLowerCaseNames(this ExRam.Gremlinq.Core.IGraphElementPropertyModel model) { }
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
        All = 255,
    }
    public static class GraphModel
    {
        public static readonly ExRam.Gremlinq.Core.IGraphModel Empty;
        public static ExRam.Gremlinq.Core.IGraphModel ConfigureElements(this ExRam.Gremlinq.Core.IGraphModel model, System.Func<ExRam.Gremlinq.Core.IGraphElementModel, ExRam.Gremlinq.Core.IGraphElementModel> transformation) { }
        public static ExRam.Gremlinq.Core.IGraphModel Default(System.Func<ExRam.Gremlinq.Core.IAssemblyLookupBuilder, ExRam.Gremlinq.Core.IAssemblyLookupSet> assemblyLookupTransformation, Microsoft.Extensions.Logging.ILogger? logger = null) { }
        public static ExRam.Gremlinq.Core.IGraphModel FromBaseTypes(System.Type vertexBaseType, System.Type edgeBaseType, System.Func<ExRam.Gremlinq.Core.IAssemblyLookupBuilder, ExRam.Gremlinq.Core.IAssemblyLookupSet> assemblyLookupTransformation, Microsoft.Extensions.Logging.ILogger? logger = null) { }
        public static ExRam.Gremlinq.Core.IGraphModel FromBaseTypes<TVertex, TEdge>(System.Func<ExRam.Gremlinq.Core.IAssemblyLookupBuilder, ExRam.Gremlinq.Core.IAssemblyLookupSet> assemblyLookupTransformation, Microsoft.Extensions.Logging.ILogger? logger = null) { }
        public static ExRam.Gremlinq.Core.IGraphModel FromTypes(System.Type[] vertexTypes, System.Type[] edgeTypes) { }
    }
    public sealed class GraphsonMappingException : System.Exception
    {
        public GraphsonMappingException() { }
        public GraphsonMappingException(string message) { }
        public GraphsonMappingException(string message, System.Exception innerException) { }
    }
    public enum GraphsonVersion
    {
        V2 = 0,
        V3 = 1,
    }
    public readonly struct GremlinQueryAwaiter<TElement> : System.Runtime.CompilerServices.ICriticalNotifyCompletion, System.Runtime.CompilerServices.INotifyCompletion
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
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment EchoGraphson(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment) { }
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment EchoGroovy(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment) { }
        public static System.Collections.Generic.IAsyncEnumerable<TElement> Execute<TElement>(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement> query) { }
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment UseDeserializer(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment, ExRam.Gremlinq.Core.IGremlinQueryExecutionResultDeserializer deserializer) { }
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment UseExecutor(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment, ExRam.Gremlinq.Core.IGremlinQueryExecutor executor) { }
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment UseLogger(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment source, Microsoft.Extensions.Logging.ILogger logger) { }
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment UseModel(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment source, ExRam.Gremlinq.Core.IGraphModel model) { }
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment UseSerializer(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment, ExRam.Gremlinq.Core.IGremlinQuerySerializer serializer) { }
    }
    public static class GremlinQueryExecutionResultDeserializer
    {
        public static readonly ExRam.Gremlinq.Core.IGremlinQueryExecutionResultDeserializer Empty;
        public static readonly ExRam.Gremlinq.Core.IGremlinQueryExecutionResultDeserializer Graphson;
        public static readonly ExRam.Gremlinq.Core.IGremlinQueryExecutionResultDeserializer Identity;
        public static readonly ExRam.Gremlinq.Core.IGremlinQueryExecutionResultDeserializer Invalid;
        public static readonly ExRam.Gremlinq.Core.IGremlinQueryExecutionResultDeserializer ToGraphson;
        public static readonly ExRam.Gremlinq.Core.IGremlinQueryExecutionResultDeserializer ToString;
        public static ExRam.Gremlinq.Core.IGremlinQueryExecutionResultDeserializer GraphsonWithJsonConverters(params ExRam.Gremlinq.Core.IJTokenConverter[] additionalConverters) { }
    }
    public static class GremlinQueryExecutor
    {
        public static readonly ExRam.Gremlinq.Core.IGremlinQueryExecutor Echo;
        public static readonly ExRam.Gremlinq.Core.IGremlinQueryExecutor Empty;
        public static readonly ExRam.Gremlinq.Core.IGremlinQueryExecutor Invalid;
        public static ExRam.Gremlinq.Core.IGremlinQueryExecutor InterceptQuery(this ExRam.Gremlinq.Core.IGremlinQueryExecutor baseExecutor, System.Func<object, object> transformation) { }
        public static ExRam.Gremlinq.Core.IGremlinQueryExecutor InterceptResult(this ExRam.Gremlinq.Core.IGremlinQueryExecutor baseExecutor, System.Func<System.Collections.Generic.IAsyncEnumerable<object>, System.Collections.Generic.IAsyncEnumerable<object>> transformation) { }
    }
    public static class GremlinQueryExtensions
    {
        public static System.Func<TSourceQuery, TTargetQuery> CreateContinuationFrom<TSourceQuery, TTargetQuery>(this TSourceQuery sourceQuery, TTargetQuery targetQuery)
            where TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase { }
        public static System.Threading.Tasks.ValueTask<TElement> FirstAsync<TElement>(this ExRam.Gremlinq.Core.IGremlinQueryBase<TElement> query, System.Threading.CancellationToken ct = default) { }
        public static System.Threading.Tasks.ValueTask<TElement> FirstOrDefaultAsync<TElement>(this ExRam.Gremlinq.Core.IGremlinQueryBase<TElement> query, System.Threading.CancellationToken ct = default) { }
        public static System.Threading.Tasks.ValueTask<TElement> SingleAsync<TElement>(this ExRam.Gremlinq.Core.IGremlinQueryBase<TElement> query, System.Threading.CancellationToken ct = default) { }
        public static System.Threading.Tasks.ValueTask<TElement> SingleOrDefaultAsync<TElement>(this ExRam.Gremlinq.Core.IGremlinQueryBase<TElement> query, System.Threading.CancellationToken ct = default) { }
        public static System.Threading.Tasks.ValueTask<TElement[]> ToArrayAsync<TElement>(this ExRam.Gremlinq.Core.IGremlinQueryBase<TElement> query, System.Threading.CancellationToken ct = default) { }
    }
    public static class GremlinQuerySerializer
    {
        public static readonly ExRam.Gremlinq.Core.IGremlinQuerySerializer Default;
        public static readonly ExRam.Gremlinq.Core.IGremlinQuerySerializer Identity;
        public static readonly ExRam.Gremlinq.Core.IGremlinQuerySerializer Invalid;
        public static ExRam.Gremlinq.Core.IGremlinQuerySerializer Select(this ExRam.Gremlinq.Core.IGremlinQuerySerializer serializer, System.Func<object, object> projection) { }
        public static ExRam.Gremlinq.Core.IGremlinQuerySerializer ToGroovy(this ExRam.Gremlinq.Core.IGremlinQuerySerializer serializer) { }
        public static ExRam.Gremlinq.Core.IGremlinQuerySerializer UseDefaultGremlinStepSerializationHandlers(this ExRam.Gremlinq.Core.IGremlinQuerySerializer serializer) { }
    }
    public static class GremlinQuerySource
    {
        public static readonly ExRam.Gremlinq.Core.IConfigurableGremlinQuerySource g;
    }
    public class GremlinqOption
    {
        public static ExRam.Gremlinq.Core.GremlinqOption<ExRam.Gremlinq.Core.DisabledTextPredicates> DisabledTextPredicates;
        public static ExRam.Gremlinq.Core.GremlinqOption<System.Collections.Immutable.IImmutableList<ExRam.Gremlinq.Core.Step>> EdgeProjectionSteps;
        public static ExRam.Gremlinq.Core.GremlinqOption<ExRam.Gremlinq.Core.FilterLabelsVerbosity> FilterLabelsVerbosity;
        public static ExRam.Gremlinq.Core.GremlinqOption<System.Collections.Immutable.IImmutableList<ExRam.Gremlinq.Core.Step>> VertexProjectionSteps;
        public GremlinqOption() { }
    }
    public class GremlinqOption<TValue> : ExRam.Gremlinq.Core.GremlinqOption
    {
        public GremlinqOption(TValue defaultValue) { }
        public TValue DefaultValue { get; }
    }
    public readonly struct GremlinqOptions
    {
        public GremlinqOptions(System.Collections.Immutable.IImmutableDictionary<ExRam.Gremlinq.Core.GremlinqOption, object> dictionary) { }
        public ExRam.Gremlinq.Core.GremlinqOptions ConfigureValue<TValue>(ExRam.Gremlinq.Core.GremlinqOption<TValue> option, System.Func<TValue, TValue> configuration) { }
        public bool Contains(ExRam.Gremlinq.Core.GremlinqOption option) { }
        public TValue GetValue<TValue>(ExRam.Gremlinq.Core.GremlinqOption<TValue> option) { }
        public ExRam.Gremlinq.Core.GremlinqOptions Remove(ExRam.Gremlinq.Core.GremlinqOption option) { }
        public ExRam.Gremlinq.Core.GremlinqOptions SetValue<TValue>(ExRam.Gremlinq.Core.GremlinqOption<TValue> option, TValue value) { }
    }
    public sealed class GroovyGremlinQuery
    {
        public GroovyGremlinQuery(string script, System.Collections.Generic.Dictionary<string, object> bindings) { }
        public System.Collections.Generic.Dictionary<string, object> Bindings { get; }
        public string Script { get; }
        public override string ToString() { }
    }
    public sealed class GroupStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.GroupStep Instance;
        public sealed class ByKeyStep : ExRam.Gremlinq.Core.Step
        {
            public ByKeyStep(object key) { }
            public object Key { get; }
        }
        public sealed class ByStepsStep : ExRam.Gremlinq.Core.Step
        {
            public ByStepsStep(ExRam.Gremlinq.Core.Step[] steps) { }
            public ExRam.Gremlinq.Core.Step[] Steps { get; }
        }
        public sealed class ByTraversalStep : ExRam.Gremlinq.Core.SingleTraversalArgumentStep
        {
            public ByTraversalStep(ExRam.Gremlinq.Core.IGremlinQueryBase traversal) { }
        }
    }
    public sealed class HasKeyStep : ExRam.Gremlinq.Core.Step
    {
        public HasKeyStep(object argument) { }
        public object Argument { get; }
    }
    public sealed class HasLabelStep : ExRam.Gremlinq.Core.DerivedLabelNamesStep
    {
        public HasLabelStep(string[] labels) { }
    }
    public sealed class HasNotStep : ExRam.Gremlinq.Core.Step
    {
        public HasNotStep(object key) { }
        public object Key { get; }
    }
    public sealed class HasPredicateStep : ExRam.Gremlinq.Core.Step
    {
        public HasPredicateStep(object key, Gremlin.Net.Process.Traversal.P? predicate = null) { }
        public object Key { get; }
        public Gremlin.Net.Process.Traversal.P? Predicate { get; }
    }
    public sealed class HasTraversalStep : ExRam.Gremlinq.Core.Step
    {
        public HasTraversalStep(object key, ExRam.Gremlinq.Core.IGremlinQueryBase traversal) { }
        public object Key { get; }
        public ExRam.Gremlinq.Core.IGremlinQueryBase Traversal { get; }
    }
    public sealed class HasValueStep : ExRam.Gremlinq.Core.Step
    {
        public HasValueStep(object argument) { }
        public object Argument { get; }
    }
    public interface IArrayGremlinQueryBase : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IValueGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IValueGremlinQuery<object[]> Lower();
    }
    public interface IArrayGremlinQueryBaseRec<TSelf> : ExRam.Gremlinq.Core.IArrayGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IValueGremlinQueryBase, ExRam.Gremlinq.Core.IValueGremlinQueryBaseRec<TSelf>
        where TSelf : ExRam.Gremlinq.Core.IArrayGremlinQueryBaseRec<TSelf> { }
    public interface IArrayGremlinQueryBaseRec<TArray, out TQuery, TSelf> : ExRam.Gremlinq.Core.IArrayGremlinQueryBase, ExRam.Gremlinq.Core.IArrayGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IArrayGremlinQueryBase<TArray, TQuery>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TArray, TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase<TArray>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IValueGremlinQueryBase, ExRam.Gremlinq.Core.IValueGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IValueGremlinQueryBaseRec<TArray, TSelf>, ExRam.Gremlinq.Core.IValueGremlinQueryBase<TArray>
        where TSelf : ExRam.Gremlinq.Core.IArrayGremlinQueryBaseRec<TArray, TQuery, TSelf> { }
    public interface IArrayGremlinQueryBase<TArray, out TQuery> : ExRam.Gremlinq.Core.IArrayGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TArray>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IValueGremlinQueryBase, ExRam.Gremlinq.Core.IValueGremlinQueryBase<TArray>
    {
        ExRam.Gremlinq.Core.IValueGremlinQuery<TArray> Lower();
        TQuery MaxLocal();
        TQuery MeanLocal();
        TQuery MinLocal();
        TQuery SumLocal();
        TQuery Unfold();
    }
    public interface IArrayGremlinQuery<TArray, TQuery> : ExRam.Gremlinq.Core.IArrayGremlinQueryBase, ExRam.Gremlinq.Core.IArrayGremlinQueryBaseRec<ExRam.Gremlinq.Core.IArrayGremlinQuery<TArray, TQuery>>, ExRam.Gremlinq.Core.IArrayGremlinQueryBaseRec<TArray, TQuery, ExRam.Gremlinq.Core.IArrayGremlinQuery<TArray, TQuery>>, ExRam.Gremlinq.Core.IArrayGremlinQueryBase<TArray, TQuery>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IArrayGremlinQuery<TArray, TQuery>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TArray, ExRam.Gremlinq.Core.IArrayGremlinQuery<TArray, TQuery>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TArray>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IValueGremlinQueryBase, ExRam.Gremlinq.Core.IValueGremlinQueryBaseRec<ExRam.Gremlinq.Core.IArrayGremlinQuery<TArray, TQuery>>, ExRam.Gremlinq.Core.IValueGremlinQueryBaseRec<TArray, ExRam.Gremlinq.Core.IArrayGremlinQuery<TArray, TQuery>>, ExRam.Gremlinq.Core.IValueGremlinQueryBase<TArray> { }
    public interface IAssemblyLookupBuilder
    {
        ExRam.Gremlinq.Core.IAssemblyLookupSet IncludeAssemblies(System.Collections.Generic.IEnumerable<System.Reflection.Assembly> assemblies);
        ExRam.Gremlinq.Core.IAssemblyLookupSet IncludeAssembliesFromAppDomain();
        ExRam.Gremlinq.Core.IAssemblyLookupSet IncludeAssembliesFromStackTrace();
        ExRam.Gremlinq.Core.IAssemblyLookupSet IncludeAssembliesOfBaseTypes();
    }
    public interface IAssemblyLookupSet : ExRam.Gremlinq.Core.IAssemblyLookupBuilder
    {
        System.Collections.Immutable.IImmutableList<System.Reflection.Assembly> Assemblies { get; }
    }
    public interface IBothEdgeGremlinQueryBase : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery { }
    public interface IBothEdgeGremlinQueryBase<TEdge, TOutVertex, TInVertex> : ExRam.Gremlinq.Core.IBothEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBase<TEdge, TInVertex>, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBase<TEdge, TOutVertex>, ExRam.Gremlinq.Core.IStartGremlinQuery { }
    public interface IBothEdgeGremlinQueryRec<TSelf> : ExRam.Gremlinq.Core.IBothEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IBothEdgeGremlinQueryRec<TSelf> { }
    public interface IBothEdgeGremlinQueryRec<TEdge, TOutVertex, TInVertex, TSelf> : ExRam.Gremlinq.Core.IBothEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IBothEdgeGremlinQueryBase<TEdge, TOutVertex, TInVertex>, ExRam.Gremlinq.Core.IBothEdgeGremlinQueryRec<TSelf>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBase<TEdge, TInVertex>, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBase<TEdge, TOutVertex>, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IBothEdgeGremlinQueryRec<TEdge, TOutVertex, TInVertex, TSelf> { }
    public interface IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> : ExRam.Gremlinq.Core.IBothEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IBothEdgeGremlinQueryBase<TEdge, TOutVertex, TInVertex>, ExRam.Gremlinq.Core.IBothEdgeGremlinQueryRec<ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IBothEdgeGremlinQueryRec<TEdge, TOutVertex, TInVertex, ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBaseRec<ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBase<TEdge, TInVertex>, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBaseRec<ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBaseRec<ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBase<TEdge, TOutVertex>, ExRam.Gremlinq.Core.IStartGremlinQuery { }
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
        ExRam.Gremlinq.Core.IOutEdgeGremlinQuery<TEdge, TOutVertex> From<TOutVertex>(System.Func<ExRam.Gremlinq.Core.IVertexGremlinQueryBase, ExRam.Gremlinq.Core.IVertexGremlinQuery<TOutVertex>> fromVertexTraversal);
        ExRam.Gremlinq.Core.IPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.Property<object>> Properties(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.Property<TValue>> Properties<TValue>(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.Property<TValue>> Properties<TValue>(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TEdge, TInVertex> To<TInVertex>(ExRam.Gremlinq.Core.StepLabel<TInVertex> stepLabel);
        ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TEdge, TInVertex> To<TInVertex>(System.Func<ExRam.Gremlinq.Core.IVertexGremlinQueryBase, ExRam.Gremlinq.Core.IVertexGremlinQuery<TInVertex>> toVertexTraversal);
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge> Update(TEdge element);
        ExRam.Gremlinq.Core.IValueGremlinQuery<object> Values(params System.Linq.Expressions.Expression<>[] projections);
        new ExRam.Gremlinq.Core.IValueGremlinQuery<TTarget> Values<TTarget>(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IValueGremlinQuery<TTarget> Values<TTarget>(params System.Linq.Expressions.Expression<>[] projections);
    }
    public interface IEdgeGremlinQuery<TEdge> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge>>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge>>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge>>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge>>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IStartGremlinQuery { }
    public interface IEdgeOrVertexGremlinQueryBase : ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQuery<TResult> Cast<TResult>();
    }
    public interface IEdgeOrVertexGremlinQueryBaseRec<TSelf> : ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf> { }
    public interface IEdgeOrVertexGremlinQueryBaseRec<TElement, TSelf> : ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TElement, TSelf>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TElement, TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TElement, TSelf> { }
    public interface IEdgeOrVertexGremlinQueryBase<TElement> : ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IStartGremlinQuery { }
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
        TSelf Property(string key, object value);
    }
    public interface IElementGremlinQueryBaseRec<TElement, TSelf> : ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TElement, TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TElement, TSelf>
    {
        TSelf Order(System.Func<ExRam.Gremlinq.Core.IOrderBuilder<TElement, TSelf>, ExRam.Gremlinq.Core.IOrderBuilderWithBy<TElement, TSelf>> projection);
        TSelf OrderLocal(System.Func<ExRam.Gremlinq.Core.IOrderBuilder<TElement, TSelf>, ExRam.Gremlinq.Core.IOrderBuilderWithBy<TElement, TSelf>> projection);
        TSelf Property<TProjectedValue>(System.Linq.Expressions.Expression<System.Func<TElement, TProjectedValue>> projection, TProjectedValue value);
        TSelf Where(System.Linq.Expressions.Expression<System.Func<TElement, bool>> predicate);
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
    public interface IGraphElementModel
    {
        System.Collections.Immutable.IImmutableDictionary<System.Type, ExRam.Gremlinq.Core.ElementMetadata> Metadata { get; }
    }
    public interface IGraphElementPropertyModel
    {
        System.Collections.Immutable.IImmutableDictionary<System.Reflection.MemberInfo, ExRam.Gremlinq.Core.PropertyMetadata> Metadata { get; }
        System.Collections.Immutable.IImmutableDictionary<string, Gremlin.Net.Process.Traversal.T> SpecialNames { get; }
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
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment Environment { get; }
        System.Collections.Immutable.IImmutableStack<ExRam.Gremlinq.Core.Step> Steps { get; }
        TTargetQuery ChangeQueryType<TTargetQuery>()
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        ExRam.Gremlinq.Core.IGremlinQuery<object> ConfigureSteps(System.Func<System.Collections.Immutable.IImmutableStack<ExRam.Gremlinq.Core.Step>, System.Collections.Immutable.IImmutableStack<ExRam.Gremlinq.Core.Step>> configurator);
    }
    public interface IGremlinQueryBase : ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IGremlinQueryAdmin AsAdmin();
        ExRam.Gremlinq.Core.IArrayGremlinQuery<TElement, TQuery> Cap<TQuery, TElement>(ExRam.Gremlinq.Core.StepLabel<ExRam.Gremlinq.Core.IArrayGremlinQuery<TElement, TQuery>, TElement> label)
            where TQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
        ;
        ExRam.Gremlinq.Core.IGremlinQuery<TResult> Cast<TResult>();
        ExRam.Gremlinq.Core.IValueGremlinQuery<TValue> Constant<TValue>(TValue constant);
        ExRam.Gremlinq.Core.IValueGremlinQuery<long> Count();
        ExRam.Gremlinq.Core.IValueGremlinQuery<long> CountLocal();
        ExRam.Gremlinq.Core.IValueGremlinQuery<object> Drop();
        ExRam.Gremlinq.Core.IValueGremlinQuery<string> Explain();
        System.Runtime.CompilerServices.TaskAwaiter GetAwaiter();
        ExRam.Gremlinq.Core.IGremlinQuery<object> Lower();
        ExRam.Gremlinq.Core.IValueGremlinQuery<string> Profile();
        ExRam.Gremlinq.Core.IValueGremlinQuery<TStepElement> Select<TStepElement>(ExRam.Gremlinq.Core.StepLabel<TStepElement> label);
        TQuery Select<TQuery, TElement>(ExRam.Gremlinq.Core.StepLabel<TQuery, TElement> label)
            where TQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
        ;
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
        TTargetQuery Choose<TTargetQuery>(System.Func<ExRam.Gremlinq.Core.IChooseBuilder<TSelf>, ExRam.Gremlinq.Core.IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        TTargetQuery Choose<TTargetQuery>(System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase> traversalPredicate, System.Func<TSelf, TTargetQuery> trueChoice)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        TTargetQuery Choose<TTargetQuery>(System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase> traversalPredicate, System.Func<TSelf, TTargetQuery> trueChoice, System.Func<TSelf, TTargetQuery> falseChoice)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        TTargetQuery Coalesce<TTargetQuery>(params System.Func<, >[] traversals)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        TSelf Coin(double probability);
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
        TSelf None();
        TSelf Not(System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase> notTraversal);
        TSelf Optional(System.Func<TSelf, TSelf> optionalTraversal);
        TSelf Or(params System.Func<, >[] orTraversals);
        TSelf Range(long low, long high);
        TSelf RangeLocal(long low, long high);
        TSelf Repeat(System.Func<TSelf, TSelf> repeatTraversal);
        TSelf RepeatUntil(System.Func<TSelf, TSelf> repeatTraversal, System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase> untilTraversal);
        TSelf SideEffect(System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase> sideEffectTraversal);
        TSelf Skip(long count);
        TSelf SkipLocal(long count);
        TSelf Tail(long count);
        TSelf TailLocal(long count);
        TSelf Times(int count);
        TTargetQuery Union<TTargetQuery>(params System.Func<, >[] unionTraversals)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        TSelf UntilRepeat(System.Func<TSelf, TSelf> repeatTraversal, System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase> untilTraversal);
        TSelf Where(Gremlin.Net.Process.Traversal.ILambda lambda);
        TSelf Where(System.Func<TSelf, ExRam.Gremlinq.Core.IGremlinQueryBase> filterTraversal);
    }
    public interface IGremlinQueryBaseRec<TElement, TSelf> : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TElement, TSelf>
    {
        TTargetQuery Aggregate<TTargetQuery>(System.Func<TSelf, ExRam.Gremlinq.Core.StepLabel<ExRam.Gremlinq.Core.IArrayGremlinQuery<TElement[], TSelf>, TElement[]>, TTargetQuery> continuation)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        TTargetQuery AggregateGlobal<TTargetQuery>(System.Func<TSelf, ExRam.Gremlinq.Core.StepLabel<ExRam.Gremlinq.Core.IArrayGremlinQuery<TElement[], TSelf>, TElement[]>, TTargetQuery> continuation)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        TSelf As(ExRam.Gremlinq.Core.StepLabel<TElement> stepLabel);
        TTargetQuery As<TTargetQuery>(System.Func<TSelf, ExRam.Gremlinq.Core.StepLabel<TSelf, TElement>, TTargetQuery> continuation)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        ExRam.Gremlinq.Core.IArrayGremlinQuery<TElement[], TSelf> Fold();
        TSelf Inject(params TElement[] elements);
        [return: System.Runtime.CompilerServices.Dynamic(new bool[] {
                false,
                true})]
        ExRam.Gremlinq.Core.IValueGremlinQuery<object> Project(System.Func<ExRam.Gremlinq.Core.IProjectBuilder<TSelf, TElement>, ExRam.Gremlinq.Core.IProjectResult> continuation);
        ExRam.Gremlinq.Core.IValueGremlinQuery<TResult> Project<TResult>(System.Func<ExRam.Gremlinq.Core.IProjectBuilder<TSelf, TElement>, ExRam.Gremlinq.Core.IProjectResult<TResult>> continuation);
    }
    public interface IGremlinQueryBase<TElement> : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.GremlinQueryAwaiter<TElement> GetAwaiter();
        ExRam.Gremlinq.Core.IGremlinQuery<TElement> Lower();
        System.Collections.Generic.IAsyncEnumerable<TElement> ToAsyncEnumerable();
    }
    public interface IGremlinQueryEnvironment
    {
        ExRam.Gremlinq.Core.IGremlinQueryExecutionResultDeserializer Deserializer { get; }
        ExRam.Gremlinq.Core.IGremlinQueryExecutor Executor { get; }
        ExRam.Gremlinq.Core.FeatureSet FeatureSet { get; }
        Microsoft.Extensions.Logging.ILogger Logger { get; }
        ExRam.Gremlinq.Core.IGraphModel Model { get; }
        ExRam.Gremlinq.Core.GremlinqOptions Options { get; }
        ExRam.Gremlinq.Core.IGremlinQuerySerializer Serializer { get; }
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment ConfigureDeserializer(System.Func<ExRam.Gremlinq.Core.IGremlinQueryExecutionResultDeserializer, ExRam.Gremlinq.Core.IGremlinQueryExecutionResultDeserializer> deserializerTransformation);
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment ConfigureExecutor(System.Func<ExRam.Gremlinq.Core.IGremlinQueryExecutor, ExRam.Gremlinq.Core.IGremlinQueryExecutor> executorTransformation);
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment ConfigureFeatureSet(System.Func<ExRam.Gremlinq.Core.FeatureSet, ExRam.Gremlinq.Core.FeatureSet> featureSetTransformation);
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment ConfigureLogger(System.Func<Microsoft.Extensions.Logging.ILogger, Microsoft.Extensions.Logging.ILogger> loggerTransformation);
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment ConfigureModel(System.Func<ExRam.Gremlinq.Core.IGraphModel, ExRam.Gremlinq.Core.IGraphModel> modelTransformation);
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment ConfigureOptions(System.Func<ExRam.Gremlinq.Core.GremlinqOptions, ExRam.Gremlinq.Core.GremlinqOptions> optionsTransformation);
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment ConfigureSerializer(System.Func<ExRam.Gremlinq.Core.IGremlinQuerySerializer, ExRam.Gremlinq.Core.IGremlinQuerySerializer> serializerTransformation);
    }
    public interface IGremlinQueryExecutionResultDeserializer
    {
        System.Collections.Generic.IAsyncEnumerable<TElement> Deserialize<TElement>(object executionResult, ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment);
    }
    public interface IGremlinQueryExecutor
    {
        System.Collections.Generic.IAsyncEnumerable<object> Execute(object serializedQuery);
    }
    public interface IGremlinQuerySerializer
    {
        ExRam.Gremlinq.Core.IGremlinQuerySerializer ConfigureFragmentSerializer(System.Func<ExRam.Gremlinq.Core.IQueryFragmentSerializer, ExRam.Gremlinq.Core.IQueryFragmentSerializer> transformation);
        object? Serialize(ExRam.Gremlinq.Core.IGremlinQueryBase query);
    }
    public interface IGremlinQuerySource : ExRam.Gremlinq.Core.IConfigurableGremlinQuerySource, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment Environment { get; }
        ExRam.Gremlinq.Core.IGremlinQuerySource RemoveStrategies(params System.Type[] strategyTypes);
    }
    public interface IGremlinQuery<TElement> : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TElement, ExRam.Gremlinq.Core.IGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IStartGremlinQuery { }
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
    public interface IInEdgeGremlinQueryBase : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery { }
    public interface IInEdgeGremlinQueryBaseRec<TSelf> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IInEdgeGremlinQueryBaseRec<TSelf> { }
    public interface IInEdgeGremlinQueryBaseRec<TEdge, TInVertex, TSelf> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBase<TEdge, TInVertex>, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IInEdgeGremlinQueryBaseRec<TEdge, TInVertex, TSelf> { }
    public interface IInEdgeGremlinQueryBase<TEdge, TInVertex> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IInEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> From<TOutVertex>(System.Func<ExRam.Gremlinq.Core.IVertexGremlinQuery<TInVertex>, ExRam.Gremlinq.Core.IGremlinQuery<TOutVertex>> fromVertexTraversal);
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TInVertex> InV();
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
        ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TTargetVertex, TAdjacentVertex> From<TTargetVertex>(System.Func<ExRam.Gremlinq.Core.IVertexGremlinQuery<TAdjacentVertex>, ExRam.Gremlinq.Core.IVertexGremlinQuery<TTargetVertex>> fromVertexTraversal);
        ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TAdjacentVertex, TTargetVertex> To<TTargetVertex>(ExRam.Gremlinq.Core.StepLabel<TTargetVertex> stepLabel);
        ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TAdjacentVertex, TTargetVertex> To<TTargetVertex>(System.Func<ExRam.Gremlinq.Core.IVertexGremlinQuery<TAdjacentVertex>, ExRam.Gremlinq.Core.IVertexGremlinQuery<TTargetVertex>> toVertexTraversal);
    }
    public interface IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TEdge, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBaseRec<ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBaseRec<TEdge, TAdjacentVertex, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>, ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQueryBase<TEdge, TAdjacentVertex>, ExRam.Gremlinq.Core.IStartGremlinQuery { }
    public interface IJTokenConverter
    {
        bool TryConvert(Newtonsoft.Json.Linq.JToken jToken, System.Type objectType, ExRam.Gremlinq.Core.IJTokenConverter recurse, out object? value);
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
    public interface IOutEdgeGremlinQueryBase : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery { }
    public interface IOutEdgeGremlinQueryBaseRec<TSelf> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBaseRec<TSelf> { }
    public interface IOutEdgeGremlinQueryBaseRec<TEdge, TOutVertex, TSelf> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TEdge, TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBase<TEdge, TOutVertex>, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBaseRec<TEdge, TOutVertex, TSelf> { }
    public interface IOutEdgeGremlinQueryBase<TEdge, TOutVertex> : ExRam.Gremlinq.Core.IEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TEdge>, ExRam.Gremlinq.Core.IOutEdgeGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TOutVertex> OutV();
        ExRam.Gremlinq.Core.IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> To<TInVertex>(System.Func<ExRam.Gremlinq.Core.IVertexGremlinQuery<TOutVertex>, ExRam.Gremlinq.Core.IGremlinQuery<TInVertex>> toVertexTraversal);
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
        ExRam.Gremlinq.Core.IProjectDynamicBuilder<TSourceQuery, TElement> By(string name, System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase> projection);
    }
    public interface IProjectResult
    {
        System.Collections.Immutable.IImmutableDictionary<string, ExRam.Gremlinq.Core.IGremlinQueryBase> Projections { get; }
    }
    public interface IProjectResult<TResult> : ExRam.Gremlinq.Core.IProjectResult { }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8>>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9> By<TItem9>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem9>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9>>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10> By<TItem10>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem10>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9, TItem10>>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11> By<TItem11>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem11>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9, TItem10, TItem11>>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12> By<TItem12>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem12>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9, TItem10, TItem11, TItem12>>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13> By<TItem13>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem13>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9, TItem10, TItem11, TItem12, TItem13>>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14> By<TItem14>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem14>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14>>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15> By<TItem15>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem15>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, System.ValueTuple<TItem15>>>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16> By<TItem16>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem16>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, System.ValueTuple<TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, System.ValueTuple<TItem15, TItem16>>>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase { }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1> By<TItem1>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem1>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2> By<TItem2>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem2>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3> By<TItem3>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem3>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2, TItem3>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4> By<TItem4>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem4>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5> By<TItem5>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem5>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6> By<TItem6>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem6>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7> By<TItem7>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem7>> projection);
    }
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7> : ExRam.Gremlinq.Core.IProjectResult, ExRam.Gremlinq.Core.IProjectResult<System.ValueTuple<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7>>
        where out TSourceQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8> By<TItem8>(System.Func<TSourceQuery, ExRam.Gremlinq.Core.IGremlinQueryBase<TItem8>> projection);
    }
    public interface IPropertyGremlinQueryBase : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IPropertyGremlinQuery<TResult> Cast<TResult>();
    }
    public interface IPropertyGremlinQueryBaseRec<TSelf> : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IPropertyGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IPropertyGremlinQueryBaseRec<TSelf> { }
    public interface IPropertyGremlinQueryBaseRec<TElement, TSelf> : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TElement, TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IPropertyGremlinQueryBase, ExRam.Gremlinq.Core.IPropertyGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IPropertyGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IStartGremlinQuery
        where TSelf : ExRam.Gremlinq.Core.IPropertyGremlinQueryBaseRec<TElement, TSelf> { }
    public interface IPropertyGremlinQueryBase<TElement> : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IPropertyGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IValueGremlinQuery<string> Key();
        ExRam.Gremlinq.Core.IValueGremlinQuery<object> Value();
        ExRam.Gremlinq.Core.IValueGremlinQuery<TValue> Value<TValue>();
        ExRam.Gremlinq.Core.IPropertyGremlinQuery<TElement> Where(System.Linq.Expressions.Expression<System.Func<TElement, bool>> predicate);
    }
    public interface IPropertyGremlinQuery<TElement> : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IPropertyGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TElement, ExRam.Gremlinq.Core.IPropertyGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IPropertyGremlinQueryBase, ExRam.Gremlinq.Core.IPropertyGremlinQueryBaseRec<ExRam.Gremlinq.Core.IPropertyGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IPropertyGremlinQueryBaseRec<TElement, ExRam.Gremlinq.Core.IPropertyGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IPropertyGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IStartGremlinQuery { }
    public interface IPropertyMetadataConfigurator<TElement> : System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.Reflection.MemberInfo, ExRam.Gremlinq.Core.PropertyMetadata>>, System.Collections.Generic.IReadOnlyCollection<System.Collections.Generic.KeyValuePair<System.Reflection.MemberInfo, ExRam.Gremlinq.Core.PropertyMetadata>>, System.Collections.Generic.IReadOnlyDictionary<System.Reflection.MemberInfo, ExRam.Gremlinq.Core.PropertyMetadata>, System.Collections.IEnumerable, System.Collections.Immutable.IImmutableDictionary<System.Reflection.MemberInfo, ExRam.Gremlinq.Core.PropertyMetadata>
    {
        ExRam.Gremlinq.Core.IPropertyMetadataConfigurator<TElement> ConfigureName<TProperty>(System.Linq.Expressions.Expression<System.Func<TElement, TProperty>> propertyExpression, string name);
        ExRam.Gremlinq.Core.IPropertyMetadataConfigurator<TElement> IgnoreAlways<TProperty>(System.Linq.Expressions.Expression<System.Func<TElement, TProperty>> propertyExpression);
        ExRam.Gremlinq.Core.IPropertyMetadataConfigurator<TElement> IgnoreOnAdd<TProperty>(System.Linq.Expressions.Expression<System.Func<TElement, TProperty>> propertyExpression);
        ExRam.Gremlinq.Core.IPropertyMetadataConfigurator<TElement> IgnoreOnUpdate<TProperty>(System.Linq.Expressions.Expression<System.Func<TElement, TProperty>> propertyExpression);
    }
    public interface IQueryFragmentSerializer
    {
        ExRam.Gremlinq.Core.IQueryFragmentSerializer Override<TFragment>(ExRam.Gremlinq.Core.QueryFragmentSerializer<TFragment> serializer);
        object Serialize<TFragment>(TFragment fragment);
    }
    public interface IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge> AddE<TEdge>()
            where TEdge : new();
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge> AddE<TEdge>(TEdge edge);
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex> AddV<TVertex>()
            where TVertex : new();
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex> AddV<TVertex>(TVertex vertex);
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<object> E(params object[] ids);
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge> E<TEdge>(params object[] ids);
        ExRam.Gremlinq.Core.IGremlinQuery<TElement> Inject<TElement>(params TElement[] elements);
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TNewEdge> ReplaceE<TNewEdge>(TNewEdge edge);
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
        where TSelf : ExRam.Gremlinq.Core.IValueGremlinQueryBaseRec<TElement, TSelf>
    {
        TSelf Order(System.Func<ExRam.Gremlinq.Core.IOrderBuilder<TSelf>, ExRam.Gremlinq.Core.IOrderBuilderWithBy<TSelf>> projection);
        TSelf OrderLocal(System.Func<ExRam.Gremlinq.Core.IOrderBuilder<TSelf>, ExRam.Gremlinq.Core.IOrderBuilderWithBy<TSelf>> projection);
    }
    public interface IValueGremlinQueryBase<TElement> : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IValueGremlinQueryBase
    {
        TTargetQuery Choose<TTargetQuery>(System.Linq.Expressions.Expression<System.Func<TElement, bool>> predicate, System.Func<ExRam.Gremlinq.Core.IValueGremlinQuery<TElement>, TTargetQuery> trueChoice)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        TTargetQuery Choose<TTargetQuery>(System.Linq.Expressions.Expression<System.Func<TElement, bool>> predicate, System.Func<ExRam.Gremlinq.Core.IValueGremlinQuery<TElement>, TTargetQuery> trueChoice, System.Func<ExRam.Gremlinq.Core.IValueGremlinQuery<TElement>, TTargetQuery> falseChoice)
            where TTargetQuery : ExRam.Gremlinq.Core.IGremlinQueryBase;
        ExRam.Gremlinq.Core.IValueGremlinQuery<TElement> Max();
        ExRam.Gremlinq.Core.IValueGremlinQuery<object> MaxLocal();
        ExRam.Gremlinq.Core.IValueGremlinQuery<TElement> Mean();
        ExRam.Gremlinq.Core.IValueGremlinQuery<object> MeanLocal();
        ExRam.Gremlinq.Core.IValueGremlinQuery<TElement> Min();
        ExRam.Gremlinq.Core.IValueGremlinQuery<object> MinLocal();
        ExRam.Gremlinq.Core.IValueGremlinQuery<TElement> Sum();
        ExRam.Gremlinq.Core.IValueGremlinQuery<object> SumLocal();
        ExRam.Gremlinq.Core.IValueGremlinQuery<TElement> Where(System.Linq.Expressions.Expression<System.Func<TElement, bool>> predicate);
    }
    public interface IValueGremlinQuery<TElement> : ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IValueGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TElement, ExRam.Gremlinq.Core.IValueGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TElement>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IValueGremlinQueryBase, ExRam.Gremlinq.Core.IValueGremlinQueryBaseRec<ExRam.Gremlinq.Core.IValueGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IValueGremlinQueryBaseRec<TElement, ExRam.Gremlinq.Core.IValueGremlinQuery<TElement>>, ExRam.Gremlinq.Core.IValueGremlinQueryBase<TElement> { }
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
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TTarget> OfType<TTarget>();
        ExRam.Gremlinq.Core.IVertexGremlinQuery<object> Out();
        ExRam.Gremlinq.Core.IVertexGremlinQuery<object> Out<TEdge>();
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<object> OutE();
        ExRam.Gremlinq.Core.IEdgeGremlinQuery<TEdge> OutE<TEdge>();
    }
    public interface IVertexGremlinQueryBaseRec<TSelf> : ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IVertexGremlinQueryBase
        where TSelf : ExRam.Gremlinq.Core.IVertexGremlinQueryBaseRec<TSelf> { }
    public interface IVertexGremlinQueryBaseRec<TVertex, TSelf> : ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TVertex, TSelf>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TVertex>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TVertex, TSelf>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TVertex>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TVertex, TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase<TVertex>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IVertexGremlinQueryBase, ExRam.Gremlinq.Core.IVertexGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IVertexGremlinQueryBase<TVertex>
        where TSelf : ExRam.Gremlinq.Core.IVertexGremlinQueryBaseRec<TVertex, TSelf>
    {
        TSelf Property<TProjectedValue>(System.Linq.Expressions.Expression<System.Func<TVertex, TProjectedValue[]>> projection, TProjectedValue value);
    }
    public interface IVertexGremlinQueryBase<TVertex> : ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TVertex>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TVertex>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TVertex>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IVertexGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TVertex> AddE<TEdge>()
            where TEdge : new();
        ExRam.Gremlinq.Core.IInOrOutEdgeGremlinQuery<TEdge, TVertex> AddE<TEdge>(TEdge edge);
        ExRam.Gremlinq.Core.IInEdgeGremlinQuery<TEdge, TVertex> InE<TEdge>();
        ExRam.Gremlinq.Core.IOutEdgeGremlinQuery<TEdge, TVertex> OutE<TEdge>();
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.VertexProperty<object>, object> Properties();
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.VertexProperty<object>, object> Properties(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue>, TValue> Properties<TValue>();
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue>, TValue> Properties<TValue>(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue>, TValue> Properties<TValue>(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue>, TValue> Properties<TValue>(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue>, TValue> Properties<TValue>(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue, TMeta>, TValue, TMeta> Properties<TValue, TMeta>(params System.Linq.Expressions.Expression<>[] projections)
            where TMeta :  class;
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue, TMeta>, TValue, TMeta> Properties<TValue, TMeta>(params System.Linq.Expressions.Expression<>[] projections)
            where TMeta :  class;
        ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex> Update(TVertex element);
        ExRam.Gremlinq.Core.IValueGremlinQuery<object> Values(params System.Linq.Expressions.Expression<>[] projections);
        new ExRam.Gremlinq.Core.IValueGremlinQuery<TTarget> Values<TTarget>(params System.Linq.Expressions.Expression<>[] projections);
        new ExRam.Gremlinq.Core.IValueGremlinQuery<TTarget> Values<TTarget>(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IValueGremlinQuery<TTarget> Values<TTarget>(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IValueGremlinQuery<TTarget> Values<TTarget>(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IValueGremlinQuery<TTarget> Values<TTarget, TMeta>(params System.Linq.Expressions.Expression<>[] projections)
            where TMeta :  class;
        ExRam.Gremlinq.Core.IValueGremlinQuery<TTarget> Values<TTarget, TMeta>(params System.Linq.Expressions.Expression<>[] projections)
            where TMeta :  class;
    }
    public interface IVertexGremlinQuery<TVertex> : ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBaseRec<TVertex, ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex>>, ExRam.Gremlinq.Core.IEdgeOrVertexGremlinQueryBase<TVertex>, ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex>>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TVertex, ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex>>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TVertex>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TVertex, ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TVertex>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IVertexGremlinQueryBase, ExRam.Gremlinq.Core.IVertexGremlinQueryBaseRec<ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex>>, ExRam.Gremlinq.Core.IVertexGremlinQueryBaseRec<TVertex, ExRam.Gremlinq.Core.IVertexGremlinQuery<TVertex>>, ExRam.Gremlinq.Core.IVertexGremlinQueryBase<TVertex> { }
    public interface IVertexPropertyGremlinQueryBase : ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IStartGremlinQuery
    {
        ExRam.Gremlinq.Core.IPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.Property<object>> Properties(params string[] keys);
        ExRam.Gremlinq.Core.IValueGremlinQuery<System.Collections.Generic.IDictionary<string, object>> ValueMap(params string[] keys);
        new ExRam.Gremlinq.Core.IValueGremlinQuery<System.Collections.Generic.IDictionary<string, TTarget>> ValueMap<TTarget>();
        ExRam.Gremlinq.Core.IValueGremlinQuery<System.Collections.Generic.IDictionary<string, TTarget>> ValueMap<TTarget>(params string[] keys);
        ExRam.Gremlinq.Core.IValueGremlinQuery<object> Values(params string[] keys);
        new ExRam.Gremlinq.Core.IValueGremlinQuery<TTarget> Values<TTarget>();
        ExRam.Gremlinq.Core.IValueGremlinQuery<TTarget> Values<TTarget>(params string[] keys);
    }
    public interface IVertexPropertyGremlinQueryBaseRec<TSelf> : ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBase
        where TSelf : ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBaseRec<TSelf> { }
    public interface IVertexPropertyGremlinQueryBaseRec<TProperty, TValue, TSelf> : ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TProperty, TSelf>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TProperty>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TProperty, TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase<TProperty>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBase, ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBase<TProperty, TValue>
        where TSelf : ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBaseRec<TProperty, TValue, TSelf> { }
    public interface IVertexPropertyGremlinQueryBaseRec<TProperty, TValue, TMeta, TSelf> : ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TProperty, TSelf>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TProperty>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TProperty, TSelf>, ExRam.Gremlinq.Core.IGremlinQueryBase<TProperty>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBase, ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBaseRec<TSelf>, ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBase<TProperty, TValue, TMeta>
        where TMeta :  class
        where TSelf : ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBaseRec<TProperty, TValue, TMeta, TSelf> { }
    public interface IVertexPropertyGremlinQueryBase<TProperty, TValue> : ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TProperty>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TProperty>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBase
    {
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue, TMeta>, TValue, TMeta> Meta<TMeta>()
            where TMeta :  class;
        ExRam.Gremlinq.Core.IPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.Property<TMetaValue>> Properties<TMetaValue>(params string[] keys);
        ExRam.Gremlinq.Core.IValueGremlinQuery<TValue> Value();
    }
    public interface IVertexPropertyGremlinQueryBase<TProperty, TValue, TMeta> : ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TProperty>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBase<TProperty>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBase
        where TMeta :  class
    {
        ExRam.Gremlinq.Core.IPropertyGremlinQuery<ExRam.Gremlinq.Core.GraphElements.Property<TTarget>> Properties<TTarget>(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Property<TMetaValue>(System.Linq.Expressions.Expression<System.Func<TMeta, TMetaValue>> projection, TMetaValue value);
        ExRam.Gremlinq.Core.IValueGremlinQuery<TValue> Value();
        ExRam.Gremlinq.Core.IValueGremlinQuery<TMeta> ValueMap();
        ExRam.Gremlinq.Core.IValueGremlinQuery<TMetaValue> Values<TMetaValue>(params System.Linq.Expressions.Expression<>[] projections);
        ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Where(System.Linq.Expressions.Expression<System.Func<ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue, TMeta>, bool>> predicate);
    }
    public interface IVertexPropertyGremlinQuery<TProperty, TValue> : ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue>>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TProperty, ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue>>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TProperty>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TProperty, ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TProperty>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBase, ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBaseRec<ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue>>, ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBaseRec<TProperty, TValue, ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue>>, ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBase<TProperty, TValue> { }
    public interface IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> : ExRam.Gremlinq.Core.IElementGremlinQueryBase, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>>, ExRam.Gremlinq.Core.IElementGremlinQueryBaseRec<TProperty, ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>>, ExRam.Gremlinq.Core.IElementGremlinQueryBase<TProperty>, ExRam.Gremlinq.Core.IGremlinQueryBase, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>>, ExRam.Gremlinq.Core.IGremlinQueryBaseRec<TProperty, ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>>, ExRam.Gremlinq.Core.IGremlinQueryBase<TProperty>, ExRam.Gremlinq.Core.IStartGremlinQuery, ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBase, ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBaseRec<ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>>, ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBaseRec<TProperty, TValue, TMeta, ExRam.Gremlinq.Core.IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>>, ExRam.Gremlinq.Core.IVertexPropertyGremlinQueryBase<TProperty, TValue, TMeta>
        where TMeta :  class { }
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
    public static class ImmutableDictionaryExtensions { }
    public sealed class InEStep : ExRam.Gremlinq.Core.DerivedLabelNamesStep
    {
        public static readonly ExRam.Gremlinq.Core.InEStep NoLabels;
        public InEStep(string[] labels) { }
    }
    public sealed class InStep : ExRam.Gremlinq.Core.DerivedLabelNamesStep
    {
        public static readonly ExRam.Gremlinq.Core.InStep NoLabels;
        public InStep(string[] labels) { }
    }
    public sealed class InVStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.InVStep Instance;
        public InVStep() { }
    }
    public sealed class InjectStep : ExRam.Gremlinq.Core.Step
    {
        public InjectStep(object[] elements) { }
        public object[] Elements { get; }
    }
    public sealed class IsStep : ExRam.Gremlinq.Core.Step
    {
        public IsStep(Gremlin.Net.Process.Traversal.P predicate) { }
        public Gremlin.Net.Process.Traversal.P Predicate { get; }
    }
    public sealed class KeyStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.KeyStep Instance;
    }
    public sealed class LabelStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.LabelStep Instance;
    }
    public sealed class LimitStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.LimitStep LimitGlobal1;
        public static readonly ExRam.Gremlinq.Core.LimitStep LimitLocal1;
        public LimitStep(long count, Gremlin.Net.Process.Traversal.Scope scope) { }
        public long Count { get; }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
    }
    public sealed class LocalStep : ExRam.Gremlinq.Core.SingleTraversalArgumentStep
    {
        public LocalStep(ExRam.Gremlinq.Core.IGremlinQueryBase traversal) { }
    }
    public abstract class LogicalStep<TStep> : ExRam.Gremlinq.Core.Step
        where TStep : ExRam.Gremlinq.Core.LogicalStep<TStep>
    {
        protected LogicalStep(string name, ExRam.Gremlinq.Core.IGremlinQueryBase[] traversals) { }
        public string Name { get; }
        public ExRam.Gremlinq.Core.IGremlinQueryBase[] Traversals { get; }
    }
    public sealed class MapStep : ExRam.Gremlinq.Core.SingleTraversalArgumentStep
    {
        public MapStep(ExRam.Gremlinq.Core.IGremlinQueryBase traversal) { }
    }
    public sealed class MatchStep : ExRam.Gremlinq.Core.MultiTraversalArgumentStep
    {
        public MatchStep(ExRam.Gremlinq.Core.IGremlinQueryBase[] traversals) { }
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
    public sealed class MinStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.MinStep Global;
        public static readonly ExRam.Gremlinq.Core.MinStep Local;
        public MinStep(Gremlin.Net.Process.Traversal.Scope scope) { }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
    }
    public abstract class MultiTraversalArgumentStep : ExRam.Gremlinq.Core.Step
    {
        protected MultiTraversalArgumentStep(ExRam.Gremlinq.Core.IGremlinQueryBase[] traversals) { }
        public ExRam.Gremlinq.Core.IGremlinQueryBase[] Traversals { get; }
    }
    public sealed class NoneStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.NoneStep Instance;
    }
    public sealed class NotStep : ExRam.Gremlinq.Core.SingleTraversalArgumentStep
    {
        public NotStep(ExRam.Gremlinq.Core.IGremlinQueryBase traversal) { }
    }
    public sealed class OptionTraversalStep : ExRam.Gremlinq.Core.Step
    {
        public OptionTraversalStep(object? guard, ExRam.Gremlinq.Core.IGremlinQueryBase optionTraversal) { }
        public object? Guard { get; }
        public ExRam.Gremlinq.Core.IGremlinQueryBase OptionTraversal { get; }
    }
    public sealed class OptionalStep : ExRam.Gremlinq.Core.SingleTraversalArgumentStep
    {
        public OptionalStep(ExRam.Gremlinq.Core.IGremlinQueryBase traversal) { }
    }
    public sealed class OrStep : ExRam.Gremlinq.Core.LogicalStep<ExRam.Gremlinq.Core.OrStep>
    {
        public static readonly ExRam.Gremlinq.Core.OrStep Infix;
        public OrStep(ExRam.Gremlinq.Core.IGremlinQueryBase[] traversals) { }
    }
    public sealed class OrderStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.OrderStep Global;
        public static readonly ExRam.Gremlinq.Core.OrderStep Local;
        public OrderStep(Gremlin.Net.Process.Traversal.Scope scope) { }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
        public sealed class ByLambdaStep : ExRam.Gremlinq.Core.Step
        {
            public ByLambdaStep(Gremlin.Net.Process.Traversal.ILambda lambda) { }
            public Gremlin.Net.Process.Traversal.ILambda Lambda { get; }
        }
        public sealed class ByMemberStep : ExRam.Gremlinq.Core.Step
        {
            public ByMemberStep(object key, Gremlin.Net.Process.Traversal.Order order) { }
            public object Key { get; }
            public Gremlin.Net.Process.Traversal.Order Order { get; }
        }
        public sealed class ByTraversalStep : ExRam.Gremlinq.Core.SingleTraversalArgumentStep
        {
            public ByTraversalStep(ExRam.Gremlinq.Core.IGremlinQueryBase traversal, Gremlin.Net.Process.Traversal.Order order) { }
            public Gremlin.Net.Process.Traversal.Order Order { get; }
        }
    }
    public sealed class OtherVStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.OtherVStep Instance;
        public OtherVStep() { }
    }
    public sealed class OutEStep : ExRam.Gremlinq.Core.DerivedLabelNamesStep
    {
        public static readonly ExRam.Gremlinq.Core.OutEStep NoLabels;
        public OutEStep(string[] labels) { }
    }
    public sealed class OutStep : ExRam.Gremlinq.Core.DerivedLabelNamesStep
    {
        public static readonly ExRam.Gremlinq.Core.OutStep NoLabels;
        public OutStep(string[] labels) { }
    }
    public sealed class OutVStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.OutVStep Instance;
        public OutVStep() { }
    }
    public sealed class ProfileStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.ProfileStep Instance;
    }
    public static class ProjectBuilderExtensions
    {
        public static ExRam.Gremlinq.Core.IProjectDynamicBuilder<TSourceQuery, TElement> By<TSourceQuery, TElement>(this ExRam.Gremlinq.Core.IProjectDynamicBuilder<TSourceQuery, TElement> projectBuilder, System.Linq.Expressions.Expression<System.Func<TElement, object>> projection)
            where TSourceQuery : ExRam.Gremlinq.Core.IElementGremlinQueryBase<TElement> { }
        public static ExRam.Gremlinq.Core.IProjectDynamicBuilder<TSourceQuery, TElement> By<TSourceQuery, TElement>(this ExRam.Gremlinq.Core.IProjectDynamicBuilder<TSourceQuery, TElement> projectBuilder, string name, System.Linq.Expressions.Expression<System.Func<TElement, object>> projection)
            where TSourceQuery : ExRam.Gremlinq.Core.IElementGremlinQueryBase<TElement> { }
    }
    public sealed class ProjectStep : ExRam.Gremlinq.Core.Step
    {
        public ProjectStep(params string[] projections) { }
        public string[] Projections { get; }
        public sealed class ByKeyStep : ExRam.Gremlinq.Core.Step
        {
            public ByKeyStep(object key) { }
            public object Key { get; }
        }
        public sealed class ByStepsStep : ExRam.Gremlinq.Core.Step
        {
            public ByStepsStep(System.Collections.Generic.IEnumerable<ExRam.Gremlinq.Core.Step> steps) { }
            public System.Collections.Generic.IEnumerable<ExRam.Gremlinq.Core.Step> Steps { get; }
        }
        public sealed class ByTraversalStep : ExRam.Gremlinq.Core.SingleTraversalArgumentStep
        {
            public ByTraversalStep(ExRam.Gremlinq.Core.IGremlinQueryBase traversal) { }
        }
    }
    public sealed class PropertiesStep : ExRam.Gremlinq.Core.Step
    {
        public PropertiesStep(string[] keys) { }
        public string[] Keys { get; }
    }
    public readonly struct PropertyMetadata
    {
        public PropertyMetadata(string name, ExRam.Gremlinq.Core.SerializationBehaviour serializationBehaviour = 0) { }
        public string Name { get; }
        public ExRam.Gremlinq.Core.SerializationBehaviour SerializationBehaviour { get; }
    }
    public sealed class PropertyStep : ExRam.Gremlinq.Core.Step
    {
        public PropertyStep(object key, object value, Gremlin.Net.Process.Traversal.Cardinality? cardinality = null) { }
        public PropertyStep(object key, object value, object[] metaProperties, Gremlin.Net.Process.Traversal.Cardinality? cardinality = null) { }
        public Gremlin.Net.Process.Traversal.Cardinality? Cardinality { get; }
        public object Key { get; }
        public object[] MetaProperties { get; }
        public object Value { get; }
    }
    public static class QueryFragmentSerializer
    {
        public static readonly ExRam.Gremlinq.Core.IQueryFragmentSerializer Identity;
    }
    public delegate object QueryFragmentSerializer<TFragment>(TFragment fragment, System.Func<TFragment, object> baseSerializer, ExRam.Gremlinq.Core.IQueryFragmentSerializer recurse);
    public sealed class RangeStep : ExRam.Gremlinq.Core.Step
    {
        public RangeStep(long lower, long upper, Gremlin.Net.Process.Traversal.Scope scope) { }
        public long Lower { get; }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
        public long Upper { get; }
    }
    public sealed class RepeatStep : ExRam.Gremlinq.Core.SingleTraversalArgumentStep
    {
        public RepeatStep(ExRam.Gremlinq.Core.IGremlinQueryBase traversal) { }
    }
    public sealed class SelectStep : ExRam.Gremlinq.Core.Step
    {
        public SelectStep(params ExRam.Gremlinq.Core.StepLabel[] stepLabels) { }
        public ExRam.Gremlinq.Core.StepLabel[] StepLabels { get; }
    }
    [System.Flags]
    public enum SerializationBehaviour
    {
        Default = 0,
        IgnoreOnAdd = 1,
        IgnoreOnUpdate = 2,
        IgnoreAlways = 3,
    }
    public sealed class SideEffectStep : ExRam.Gremlinq.Core.SingleTraversalArgumentStep
    {
        public SideEffectStep(ExRam.Gremlinq.Core.IGremlinQueryBase traversal) { }
    }
    public abstract class SingleTraversalArgumentStep : ExRam.Gremlinq.Core.Step
    {
        protected SingleTraversalArgumentStep(ExRam.Gremlinq.Core.IGremlinQueryBase traversal) { }
        public ExRam.Gremlinq.Core.IGremlinQueryBase Traversal { get; }
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
    public abstract class StepLabel
    {
        protected StepLabel() { }
    }
    public class StepLabel<TElement> : ExRam.Gremlinq.Core.StepLabel
    {
        public StepLabel() { }
        public TElement Value { get; }
        public static bool !=(ExRam.Gremlinq.Core.StepLabel<TElement> b, TElement a) { }
        public static bool !=(TElement a, ExRam.Gremlinq.Core.StepLabel<TElement> b) { }
        public static bool ==(ExRam.Gremlinq.Core.StepLabel<TElement> b, TElement a) { }
        public static bool ==(TElement a, ExRam.Gremlinq.Core.StepLabel<TElement> b) { }
        public static TElement op_Implicit(ExRam.Gremlinq.Core.StepLabel<TElement> stepLabel) { }
    }
    public class StepLabel<TQuery, TElement> : ExRam.Gremlinq.Core.StepLabel<TElement>
        where TQuery : ExRam.Gremlinq.Core.IGremlinQueryBase
    {
        public StepLabel() { }
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
        public TailStep(long count, Gremlin.Net.Process.Traversal.Scope scope) { }
        public long Count { get; }
        public Gremlin.Net.Process.Traversal.Scope Scope { get; }
    }
    public sealed class TimesStep : ExRam.Gremlinq.Core.Step
    {
        public TimesStep(int count) { }
        public int Count { get; }
    }
    public sealed class UnfoldStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.UnfoldStep Instance;
        public UnfoldStep() { }
    }
    public sealed class UnionStep : ExRam.Gremlinq.Core.MultiTraversalArgumentStep
    {
        public UnionStep(ExRam.Gremlinq.Core.IGremlinQueryBase[] traversals) { }
    }
    public sealed class UntilStep : ExRam.Gremlinq.Core.SingleTraversalArgumentStep
    {
        public UntilStep(ExRam.Gremlinq.Core.IGremlinQueryBase traversal) { }
    }
    public sealed class VStep : ExRam.Gremlinq.Core.FullScanStep
    {
        public VStep(object[] ids) { }
    }
    public sealed class ValueMapStep : ExRam.Gremlinq.Core.Step
    {
        public ValueMapStep(string[] keys) { }
        public string[] Keys { get; }
    }
    public sealed class ValueStep : ExRam.Gremlinq.Core.Step
    {
        public static readonly ExRam.Gremlinq.Core.ValueStep Instance;
    }
    public sealed class ValuesStep : ExRam.Gremlinq.Core.Step
    {
        public ValuesStep(string[] keys) { }
        public string[] Keys { get; }
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
        All = 255,
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
        All = 255,
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
        All = 255,
    }
    public sealed class WherePredicateStep : ExRam.Gremlinq.Core.Step
    {
        public WherePredicateStep(Gremlin.Net.Process.Traversal.P predicate) { }
        public Gremlin.Net.Process.Traversal.P Predicate { get; }
        public sealed class ByMemberStep : ExRam.Gremlinq.Core.Step
        {
            public ByMemberStep(object? key = null) { }
            public object? Key { get; }
        }
    }
    public sealed class WhereStepLabelAndPredicateStep : ExRam.Gremlinq.Core.Step
    {
        public WhereStepLabelAndPredicateStep(ExRam.Gremlinq.Core.StepLabel stepLabel, Gremlin.Net.Process.Traversal.P predicate) { }
        public Gremlin.Net.Process.Traversal.P Predicate { get; }
        public ExRam.Gremlinq.Core.StepLabel StepLabel { get; }
    }
    public sealed class WhereTraversalStep : ExRam.Gremlinq.Core.SingleTraversalArgumentStep
    {
        public WhereTraversalStep(ExRam.Gremlinq.Core.IGremlinQueryBase traversal) { }
    }
    public sealed class WithStrategiesStep : ExRam.Gremlinq.Core.SingleTraversalArgumentStep
    {
        public WithStrategiesStep(ExRam.Gremlinq.Core.IGremlinQueryBase traversal) { }
    }
    public sealed class WithoutStrategiesStep : ExRam.Gremlinq.Core.Step
    {
        public WithoutStrategiesStep(System.Type[] strategyTypes) { }
        public System.Type[] StrategyTypes { get; }
    }
}
namespace ExRam.Gremlinq.Core.GraphElements
{
    public interface IEdge : ExRam.Gremlinq.Core.GraphElements.IElement { }
    public interface IElement
    {
        object? Id { get; set; }
    }
    public interface IVertex : ExRam.Gremlinq.Core.GraphElements.IElement { }
    public interface IVertexProperty : ExRam.Gremlinq.Core.GraphElements.IElement { }
    public abstract class Property
    {
        protected Property() { }
        public string? Key { get; set; }
        public override string ToString() { }
    }
    public class Property<TValue> : ExRam.Gremlinq.Core.GraphElements.Property
    {
        public Property(TValue value) { }
        public TValue Value { get; }
        public static ExRam.Gremlinq.Core.GraphElements.Property<TValue> op_Implicit(ExRam.Gremlinq.Core.GraphElements.Property<>[] value) { }
        public static ExRam.Gremlinq.Core.GraphElements.Property<TValue> op_Implicit(TValue value) { }
        public static ExRam.Gremlinq.Core.GraphElements.Property<TValue> op_Implicit(TValue[] value) { }
    }
    public class VertexProperty<TValue> : ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue, System.Collections.Generic.IDictionary<string, object>>
    {
        public VertexProperty(TValue value) { }
        public System.Collections.Generic.IDictionary<string, object> Properties { get; set; }
        public static ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue> op_Implicit(ExRam.Gremlinq.Core.GraphElements.VertexProperty<>[] value) { }
        public static ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue> op_Implicit(TValue value) { }
        public static ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue> op_Implicit(TValue[] value) { }
    }
    public class VertexProperty<TValue, TMeta> : ExRam.Gremlinq.Core.GraphElements.Property<TValue>, ExRam.Gremlinq.Core.GraphElements.IElement, ExRam.Gremlinq.Core.GraphElements.IVertexProperty
        where TMeta :  class
    {
        public VertexProperty(TValue value) { }
        public object? Id { get; set; }
        public string? Label { get; set; }
        public TMeta Properties { get; set; }
        public override string ToString() { }
        public static ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue, TMeta> op_Implicit(ExRam.Gremlinq.Core.GraphElements.VertexProperty<, >[] value) { }
        public static ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue, TMeta> op_Implicit(TValue value) { }
        public static ExRam.Gremlinq.Core.GraphElements.VertexProperty<TValue, TMeta> op_Implicit(TValue[] value) { }
    }
}