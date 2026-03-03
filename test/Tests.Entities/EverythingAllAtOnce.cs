using ExRam.Gremlinq.Core.GraphElements;
using Path = ExRam.Gremlinq.Core.GraphElements.Path;
using System.Collections.Immutable;
using System.Collections.Concurrent;
using System.Collections;

namespace ExRam.Gremlinq.Tests.Entities
{ 
    public sealed class EverythingAllAtOnce
    {
        public int Int_from_double { get; set; }
        public IImmutableDictionary<string, int>? IImmutableDictionary_string_keys_typed_int_values { get; set; }
        public Dictionary<int, string>? Dictionary_typed_int_keys_string_values { get; set; }
        public IDictionary? IUntypedDictionary_string_keys_typed_int_values { get; set; }
        public IEnumerable<int>? IEnumerable_from_Typed_Ints { get; set; }
        public IEnumerable? Untyped_IEnumerable_from_Typed_Ints { get; set; }
        public ISet<int>? ISet_Typed_Ints { get; set; }
        public IImmutableList<int>? IList_Typed_Ints { get; set; }
        public IImmutableList<int>? IImmutableList_Ints { get; set; }
        public IImmutableQueue<int>? IImmutableQueue_Ints { get; set; }
        public IImmutableSet<int>? IImmutableSet_Ints { get; set; }
        public IImmutableStack<int>? IImmutableStack_Ints { get; set; }
        public ConcurrentQueue<int>? ConcurrentQueue_from_typed_Ints { get; set; }
        public ConcurrentStack<int>? ConcurrentStack_from_typed_Ints { get; set; }
        public ImmutableQueue<int>? ImmutableQueue_from_typed_Ints { get; set; }
        public ImmutableStack<int>? ImmutableStack_from_typed_Ints { get; set; }
        public IReadOnlyList<int>? IReadOnlyList_from_Ints { get; set; }
        public IReadOnlyList<int>? IReadOnlyList_from_Typed_Ints { get; set; }
        public Queue<int>? Queue_from_typed_Ints { get; set; }
        public Stack<int>? Stack_from_typed_Ints { get; set; }
        public Language[]? Array { get; set; }
        public string[]? Bulk_set { get; set; }
        public DateTime DateTime_from_double { get; set; }
        public DateTime DateTime_from_number { get; set; }
        public DateTime DateTime_from_string { get; set; }
        public Company? DateTime_is_UTC { get; set; }
        public DateTimeOffset DateTimeOffset_from_number { get; set; }
        public DateTimeOffset DateTimeOffset_from_string { get; set; }
        public dynamic? DynamicData { get; set; }
        public WorksFor? Edge { get; set; }
        public (int[] ints, string[] strings) Empty_to_ints { get; set; }
        public object[]? Empty1 { get; set; }
        public Person[]? Empty2 { get; set; }
        public Path? Graphson2Path { get; set; }
        public (Person, Language)[]? GraphSon3_Tuple { get; set; }
        public Path? Graphson3Path { get; set; }
        public object? GraphSon3ReferenceVertex { get; set; }
        public Guid Guid { get; set; }
        public IDictionary<string, int>? IDictionary_string_keys_typed_int_values { get; set; }
        public IReadOnlyDictionary<string, int>? IReadOnlyDictionary_string_keys_typed_int_values { get; set; }
        public IList? IUntypedList_Typed_Ints { get; set; }
        public ICollection? IUntypedCollection_from_typed_ints { get; set; }
        public ICollection<int>? ICollection_from_typed_ints { get; set; }
        public ImmutableArray<int> ImmutableArray { get; set; }
        public ImmutableArray<int> ImmutableArray_typed_ints { get; set; }
        public ImmutableDictionary<string, int>? ImmutableDictionary_map_of_string_keys_typed_int_values { get; set; }
        public ImmutableDictionary<string, int>? ImmutableDictionary_string_keys_int_values { get; set; }
        public ImmutableDictionary<string, int>? ImmutableDictionary_string_keys_typed_int_values { get; set; }
        public ImmutableList<int>? ImmutableList_ints { get; set; }
        public ImmutableList<int>? ImmutableList_typed_ints { get; set; }
        public object[]? Int_Ids { get; set; }
        public int[]? Ints_from_Traverser { get; set; }
        public List<int>? List_Of_Ints_from_Traverser { get; set; }
        public IList<int>? IList_Of_Ints_from_Traverser { get; set; }
        public object? Language_by_vertex_inheritance { get; set; }
        public Language? Language_strongly_typed { get; set; }
        public Vertex? Language_to_generic_vertex { get; set; }
        public object? Language_unknown_type { get; set; }
        public object? Languages_to_object { get; set; }
        public List<int>? List_ints { get; set; }
        public Country? Meta_Properties { get; set; }
        public Property<object>[]? MetaProperties { get; set; }
        public object[]? Mixed_Ids { get; set; }
        public PersonLanguageTuple? NamedTuple { get; set; }
        public Language[][]? Nested_Array { get; set; }
        public int? Nullable { get; set; }
        public int?[]? Nullable_null { get; set; }
        public object? Object_from_double { get; set; }
        public object? Object_from_true { get; set; }
        public Person? Person_lowercase_strongly_typed { get; set; }
        public Person? Person_StringId { get; set; }
        public Person? Person_strongly_typed { get; set; }
        public Person? Person_with_null { get; set; }
        public Person? Person_without_PhoneNumbers_strongly_typed { get; set; }
        public object? Property_as_object { get; set; }
        public Property<int>? Property_from_Scalar { get; set; }
        public int Scalar { get; set; }
        public object? Scalar_as_object { get; set; }
        public object[]? String_Ids { get; set; }
        public object[]? String_Ids2 { get; set; }
        public TimeFrame? TimeFrame_strongly_typed { get; set; }
        public TimeSpan TimeSpan_from_double { get; set; }
        public TimeSpan TimeSpan_from_integer { get; set; }
        public (Person, Language) Tuple { get; set; }
        public (Vertex, Vertex) Tuple_vertex_vertex { get; set; }
        public VertexProperty<object>[]? VertexProperties { get; set; }
        public VertexProperty<object, MetaPoco>[]? VertexProperties_with_model { get; set; }
        public object? VertexProperty_as_object { get; set; }
        public VertexProperty<string, PropertyValidity>? VertexPropertyWithDateTimeOffset { get; set; }
        public VertexProperty<object, object>? VertexPropertyWithoutProperties { get; set; }
    }
}
