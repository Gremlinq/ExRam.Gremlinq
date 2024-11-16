using System.Runtime.CompilerServices;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.GraphElements;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Tests.Entities;
using Path = ExRam.Gremlinq.Core.GraphElements.Path;
using static ExRam.Gremlinq.Tests.Infrastructure.GraphSonStrings;
using System.Collections.Immutable;
using System.Collections.Concurrent;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public abstract class GraphsonSupportTestBase<TNativeToken> : VerifyBase
    {
        private sealed class MetaPoco
        {
            public string? MetaKey { get; set; }
        }

        private sealed class PersonLanguageTuple
        {
            public Person? Key { get; set; }
            public Language? Value { get; set; }
        }

        protected readonly IGremlinQueryEnvironment _environment;

        protected GraphsonSupportTestBase(Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation, [CallerFilePath] string sourceFile = "") : base(sourceFile: sourceFile)
        {
            _environment = GremlinQuerySource.g
                .ConfigureEnvironment(env => environmentTransformation
                    .Invoke(env
                        .UseModel(GraphModel
                            .FromBaseTypes<Vertex, Edge>())))
                .AsAdmin()
                .Environment;
        }

        protected virtual Task Verify<T>(string token, Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation)
        {
            var environment = environmentTransformation
                .Invoke(_environment);

            return Verify(environment
                .Deserializer
                .TransformTo<T[]>()
                .From(CreateNativeToken(token), environment)).DontScrubDateTimes();
        }

        protected Task Verify<T>(string token) => Verify<T>(token, _ => _);

        protected abstract TNativeToken CreateNativeToken(string str);

        [Fact]
        public Task ConcurrentQueue_from_typed_Ints() => Verify<ConcurrentQueue<int>>(Typed_Ints);

        [Fact]
        public Task ConcurrentStack_from_typed_Ints() => Verify<ConcurrentStack<int>>(Typed_Ints);

        [Fact]
        public Task ImmutableQueue_from_typed_Ints() => Verify<ImmutableQueue<int>>(Typed_Ints);

        [Fact]
        public Task ImmutableStack_from_typed_Ints() => Verify<ImmutableStack<int>>(Typed_Ints);

        [Fact]
        public Task IReadOnlyList_from_Ints() => Verify<IReadOnlyList<int>>(Ints);

        [Fact]
        public Task IReadOnlyList_from_Typed_Ints() => Verify<IReadOnlyList<int>>(Typed_Ints);

        [Fact]
        public Task Queue_from_typed_Ints() => Verify<Queue<int>>(Typed_Ints);

        [Fact]
        public Task Stack_from_typed_Ints() => Verify<Stack<int>>(Typed_Ints);

        [Fact]
        public Task Array() => Verify<Language[]>(ArrayOfLanguages);

        [Fact]
        public Task Bulk_set() => Verify<string[]>(BulkSet);

        [Fact]
        public Task Configured_property_name() => Verify<Person>(
            "[ { \"id\": 13, \"label\": \"Person\", \"type\": \"vertex\", \"properties\": { \"replacement\": [ { \"id\": 1, \"value\": \"nameValue\" } ] } } ]",
            env => env
                .ConfigureModel(model => model
                    .ConfigureVertices(_ => _
                        .ConfigureElement<Person>(conf => conf
                            .ConfigureName(x => x.Name, "replacement")))));

        [Fact]
        public Task DateTime_from_double() => Verify<DateTime>("[ 123456789.2 ]");

        [Fact]
        public Task DateTime_from_number() => Verify<DateTime>("[ 123456789 ]");

        [Fact]
        public Task DateTime_from_string() => Verify<DateTime>("[ \"2018-12-17T08:00:00Z\" ]");

        [Fact]
        public Task DateTime_is_UTC() => Verify<Company>(Single_Company);

        [Fact]
        public Task DateTimeOffset_from_number() => Verify<DateTimeOffset>("[ 123456789 ]");

        [Fact]
        public Task DateTimeOffset_from_string() => Verify<DateTimeOffset>("[ \"2018-12-17T08:00:00Z\" ]");

        [Fact]
        public Task DynamicData() => Verify<dynamic>("[ { \"values\": [ ], \"count\": { \"@type\": \"g:Int32\", \"@value\": 36 } } ]");

        [Fact]
        public Task Edge() => Verify<WorksFor>(UntypedEdge);

        [Fact]
        public Task Empty_to_ints() => Verify<(int[] ints, string[] strings)>("[{ \"Item1\": [], \"Item2\": [] }]");

        [Fact]
        public Task Empty1() => Verify<object>("[]");

        [Fact]
        public Task Empty2() => Verify<Person>("[]");

        [Fact]
        public Task Graphson2Path() => Verify<Path>(Graphson2_Paths);

        [Fact]
        public Task GraphSon3_Tuple() => Verify<(Person, Language)>(Graphson3_Tuple_of_Person_Language);

        [Fact]
        public Task Graphson3Path() => Verify<Path>(Graphson3_Paths);

        [Fact]
        public Task GraphSon3ReferenceVertex() => Verify<object>(Graphson3ReferenceVertex);

        [Fact]
        public Task Guid() => Verify<Guid>("[ \"FCE0765A-454F-4D00-83DA-D76790156E29\" ]");

        [Fact]
        public Task IDictionary_string_keys_typed_int_values() => Verify<IDictionary<string, int>>(String_Keys_Typed_Int_Values);

        [Fact]
        public Task IList_typed_ints() => Verify<IList<int>>(Typed_Ints);

        [Fact]
        public Task ImmutableArray() => Verify<ImmutableArray<int>>("[ [ 1, 3, 5 ] ]");

        [Fact]
        public Task ImmutableArray_typed_ints() => Verify<ImmutableArray<int>>(Typed_Ints);

        [Fact]
        public Task ImmutableDictionary_map_of_string_keys_typed_int_values() => Verify<ImmutableDictionary<string, int>>(Map_of_String_Keys_Typed_Int_Values);

        [Fact]
        public Task ImmutableDictionary_string_keys_int_values() => Verify<ImmutableDictionary<string, int>>(String_Keys_Int_Values);

        [Fact]
        public Task ImmutableDictionary_string_keys_typed_int_values() => Verify<ImmutableDictionary<string, int>>(String_Keys_Typed_Int_Values);

        [Fact]
        public Task ImmutableList_ints() => Verify<ImmutableList<int>>(Ints);

        [Fact]
        public Task ImmutableList_typed_ints() => Verify<ImmutableList<int>>(Typed_Ints);

        [Fact]
        public Task Int_Ids() => Verify<object>("[ 1, 2 ]");

        [Fact]
        public Task Ints_from_Traverser() => Verify<int[]>(Array_With_Traverser_With_Ints);

        [Fact]
        public Task List_Of_Ints_from_Traverser() => Verify<List<int>>(Array_With_Traverser_With_Ints);

        [Fact]
        public Task IList_Of_Ints_from_Traverser() => Verify<IList<int>>(Array_With_Traverser_With_Ints);

        [Fact]
        public Task Language_by_vertex_inheritance() => Verify<object>(Single_Language);

        [Fact]
        public Task Language_strongly_typed() => Verify<Language>(Single_Language);

        [Fact]
        public Task Language_to_generic_vertex() => Verify<Vertex>(Single_Language);

        [Fact]
        public Task Language_unknown_type() => Verify<object>(Single_Language);

        [Fact]
        public Task Languages_to_object() => Verify<object>(ArrayOfLanguages);

        [Fact]
        public Task List_ints() => Verify<List<int>>("[ [ 1, 2, 3 ] ]");

        [Fact]
        public Task Meta_Properties() => Verify<Country>(Country_with_meta_properties);

        [Fact]
        public Task MetaProperties() => Verify<Property<object>>(Properties);

        [Fact]
        public Task Mixed_Ids() => Verify<object>("[ 1, \"id2\" ]");

        [Fact]
        public Task NamedTuple() => Verify<PersonLanguageTuple>(Named_tuple_of_Person_Language);

        [Fact]
        public Task Nested_Array() => Verify<Language[][]>(Nested_array_of_Languages);

        [Fact]
        public Task Nullable() => Verify<int?>("[ 42 ]");

        [Fact]
        public Task Nullable_null() => Verify<int?>("[ 42, null ]");

        [Fact]
        public Task Object_from_double() => Verify<object>("[ 1.2 ]");

        [Fact]
        public Task Object_from_true() => Verify<object>("[ true ]");

        [Fact]
        public Task Person_lowercase_strongly_typed() => Verify<Person>(Single_Person_lowercase_properties);

        [Fact]
        public Task Person_StringId() => Verify<Person>(Single_Person_String_Id);

        [Fact]
        public Task Person_strongly_typed() => Verify<Person>(Single_Person);

        [Fact]
        public Task Person_with_null() => Verify<Person>(Single_Person_with_null);

        [Fact]
        public Task Person_without_PhoneNumbers_strongly_typed() => Verify<Person>(Single_Person_without_PhoneNumbers);

        [Fact]
        public Task Property_as_object() => Verify<object>("[ { \"value\": 1540202009475, \"key\": \"Property1\" } ]");

        [Fact]
        public Task Property_from_Scalar() => Verify<Property<int>>("[ 36 ]");

        [Fact]
        public Task Scalar() => Verify<int>("[ 36 ]");

        [Fact]
        public Task Scalar_as_object() => Verify<object>("[ 36 ]");

        [Fact]
        public Task String_Ids() => Verify<object>("[ \"id1\", \"id2\" ]");

        [Fact]
        public Task String_Ids2() => Verify<object>("[ \"1\", \"2\" ]");

        [Fact]
        public Task TimeFrame_strongly_typed() => Verify<TimeFrame>(Single_TimeFrame);

        [Fact]
        public Task TimeSpan_from_double() => Verify<TimeSpan>("[ 123456789.2 ]");

        [Fact]
        public Task TimeSpan_from_integer() => Verify<TimeSpan>("[ 123456789 ]");

        [Fact]
        public Task TimeSpan_from_object() => Verify<TimeSpan>("[ { } ]");
        
        [Fact]
        public Task TimeSpan_from_true() => Verify<TimeSpan>("[ true ]");

        [Fact]
        public Task Tuple() => Verify<(Person, Language)>(Tuple_of_Person_Language);

        [Fact]
        public Task Tuple_vertex_vertex() => Verify<(Vertex, Vertex)>(Tuple_of_Person_Language);

        [Fact]
        public Task VertexProperties() => Verify<VertexProperty<object>>(Vertex_Properties);

        [Fact]
        public Task VertexProperties_with_model() => Verify<VertexProperty<object, MetaPoco>>(Vertex_Properties);

        [Fact]
        public Task VertexProperty_as_object() => Verify<object>("[ { \"value\": 1540202009475, \"id\": 1, \"label\": \"Property1\", \"properties\": { \"metaKey\": \"MetaValue\" } } ]");

        [Fact]
        public Task VertexPropertyWithDateTimeOffset() => Verify<VertexProperty<string, PropertyValidity>>("[ { \"id\": 166, \"value\": \"bob\", \"label\": \"Name\", \"properties\": { \"ValidFrom\": 1548112365431 } } ]");

        [Fact]
        public Task VertexPropertyWithoutProperties() => Verify<VertexProperty<object, object>>("[ { \"id\": 166, \"value\": \"bob\", \"label\": \"Name\" } ]");
    }
}
