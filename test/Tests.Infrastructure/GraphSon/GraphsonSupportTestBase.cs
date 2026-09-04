using System.Runtime.CompilerServices;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.GraphElements;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Tests.Entities;
using Path = ExRam.Gremlinq.Core.GraphElements.Path;
using static ExRam.Gremlinq.Tests.Infrastructure.GraphSonStrings;
using System.Collections.Immutable;
using System.Collections.Concurrent;
using System.Collections;
using System.Numerics;
using ExRam.Gremlinq.Tests.Infrastructure.GraphSon.Entities;
using Direction = Gremlin.Net.Process.Traversal.Direction;
using Merge = Gremlin.Net.Process.Traversal.Merge;
using T = Gremlin.Net.Process.Traversal.T;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public abstract class GraphsonSupportTestBase<TNativeToken>
    {
        private readonly string _sourceFile;
        protected readonly IGremlinQueryEnvironment _environment;

        protected GraphsonSupportTestBase(Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation, [CallerFilePath] string sourceFile = "")
        {
            _sourceFile = sourceFile;

            _environment = GremlinQuerySource.g
                .ConfigureEnvironment(env => environmentTransformation
                    .Invoke(env
                        .UseModel(GraphModel
                            .FromBaseTypes<Vertex, Edge>())))
                .AsAdmin()
                .Environment;
        }

        // SettingsTask rather than Task: a derived class that adds a test of its own has nowhere to
        // put that test's snapshot, the directory being fixed at construction for the whole class.
        // Handing back the SettingsTask lets it redirect just that one, e.g. through
        // UseSnapshotDirectoryAndNameOf<T>(). Test methods can still declare Task; it converts.
        protected virtual SettingsTask Verify<T>(string token, Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation)
        {
            var environment = environmentTransformation
                .Invoke(_environment);

            var subject = environment
                .Deserializer
                .TransformTo<T>()
                .From(CreateNativeToken(token), environment);

            return Verifier
                .Verify(subject, sourceFile: _sourceFile)
                .DontScrubDateTimes();
        }

        protected SettingsTask Verify<T>(string token) => Verify<T>(token, _ => _);

        // Unlike Verify, this snapshots the outcome of a transformation rather than its result,
        // so that tokens no converter accepts can be asserted on. Verify cannot express those:
        // TransformTo<T>().From(...) throws an InvalidCastException when nothing converts.
        // An escaping exception is an outcome too - a converter that throws instead of declining
        // takes the whole deserialization down with it, so that difference belongs in the snapshot.
        // Only the exception type is recorded, as messages are culture dependent.
        protected virtual SettingsTask VerifyAttempt<T>(string token, Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation)
        {
            var environment = environmentTransformation
                .Invoke(_environment);

            object outcome;

            try
            {
                outcome = environment
                    .Deserializer
                    .TryTransform<TNativeToken, T>(CreateNativeToken(token), environment, out var value)
                        ? new { Success = true, Value = (object?)value }
                        : new { Success = false, Value = (object?)null };
            }
            catch (Exception ex)
            {
                outcome = new { Success = false, Threw = ex.GetType().Name };
            }

            return Verifier
                .Verify(outcome, sourceFile: _sourceFile)
                .DontScrubDateTimes();
        }

        protected SettingsTask VerifyAttempt<T>(string token) => VerifyAttempt<T>(token, _ => _);

        protected abstract TNativeToken CreateNativeToken(string str);

        [Fact]
        public Task Bug_1884() => Verify<Bug_1884_Entity>("""
            {
                "CreatedAt": 123456789,
                "MyEnum1": 0,
                "IsDeleted": true
            }
            """);

        [Fact]
        public Task Constructor_assertion_1() => Verify<ClassWithFieldsAndConstructor>("{ }");

        [Fact]
        public Task Constructor_assertion_2() => Verify<ClassWithFieldsAndConstructor>("""
            {
                "stringArg": "stringValue",
                "intArg": 42
            }
            """);

        [Fact]
        public Task Constructor_assertion_3() => Verify<ClassWithFieldsAndConstructor>("""
            {
                "stringArg": "stringValue",
                "intArg": 42,
                "settableString": "settableValue"
            }
            """);

        [Fact]
        public Task String_from_int() => Verify<string>("42");

        [Fact]
        public Task Id_Key() => Verify<Key>("""
            {
                "@type": "g:T",
                "@value": "id"
            }
            """);

        [Fact]
        public Task Label_Key() => Verify<Key>("""
            {
                "@type": "g:T",
                "@value": "label"
            }
            """);

        [Fact]
        public Task Id_Key_from_string() => Verify<Key>("""
            "someId"
            """);

        [Fact]
        public Task Label_Key_from_string() => Verify<Key>("""
            "someLabel"
            """);

        [Fact]
        public Task Everything() => Verify<EverythingAllAtOnce>(EverythingAllAtOnceData);

        [Fact]
        public Task Int_from_double() => Verify<int>("4.2");

        [Fact]
        public Task Init_property() => Verify<ClassWithInitProperty>("""{ "Property": "Value" }""");

        [Fact]
        public Task Field() => Verify<ClassWithField>("""{ "Property": "Value" }""");

        [Fact]
        public Task IImmutableDictionary_string_keys_typed_int_values() => Verify<IImmutableDictionary<string, int>>(String_Keys_Typed_Int_Values);

        [Fact]
        public Task Dictionary_typed_int_keys_string_values() => Verify<Dictionary<int, string>>(Map_of_Typed_Int_Keys_Typed_String_Values);

        [Fact]
        public Task IUntypedDictionary_string_keys_typed_int_values() => Verify<IDictionary>(String_Keys_Typed_Int_Values);

        [Fact]
        public Task IEnumerable_from_Typed_Ints() => Verify<IEnumerable<int>>(Typed_Ints);

        [Fact]
        public Task Untyped_IEnumerable_from_Typed_Ints() => Verify<IEnumerable>(Typed_Ints);

        [Fact]
        public Task ISet_Typed_Ints() => Verify<ISet<int>>(Typed_Ints);

        [Fact]
        public Task ISet_from_typed_Set() => Verify<ISet<int>>(Typed_Set_of_Ints);

        [Fact]
        public Task IList_Typed_Ints() => Verify<IImmutableList<int>>(Typed_Ints);

        [Fact]
        public Task IImmutableList_Ints() => Verify<IImmutableList<int>>(Typed_Ints);

        [Fact]
        public Task IImmutableQueue_Ints() => Verify<IImmutableQueue<int>>(Typed_Ints);

        [Fact]
        public Task IImmutableSet_Ints() => Verify<IImmutableSet<int>>(Typed_Ints);

        [Fact]
        public Task IImmutableStack_Ints() => Verify<IImmutableStack<int>>(Typed_Ints);

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
        public Task Bulk_set_as_object() => Verify<object>(BulkSet);

        [Fact]
        public Task Configured_property_name() => Verify<Person>(
            "{ \"id\": 13, \"label\": \"Person\", \"type\": \"vertex\", \"properties\": { \"replacement\": [ { \"id\": 1, \"value\": \"nameValue\" } ] } }",
            env => env
                .ConfigureModel(model => model
                    .ConfigureVertices(_ => _
                        .ConfigureElement<Person>(conf => conf
                            .ConfigureName(x => x.Name, "replacement")))));

        [Fact]
        public Task DateTime_from_double() => Verify<DateTime>("123456789.2");

        [Fact]
        public Task DateTime_from_number() => Verify<DateTime>("123456789");

        [Fact]
        public Task DateTime_from_string() => Verify<DateTime>("\"2018-12-17T08:00:00Z\"");

        [Fact]
        public Task DateTime_is_UTC() => Verify<Company>(Single_Company);

        [Fact]
        public Task DateTimeOffset_from_number() => Verify<DateTimeOffset>("123456789");

        [Fact]
        public Task DateTimeOffset_from_string() => Verify<DateTimeOffset>("\"2018-12-17T08:00:00Z\"");

        [Fact]
        public Task DateTime_from_invalid_string() => VerifyAttempt<DateTime>("\"not a date\"");

        [Fact]
        public Task DateTimeOffset_from_invalid_string() => VerifyAttempt<DateTimeOffset>("\"not a date\"");

        [Fact]
        public Task TimeSpan_from_invalid_string() => VerifyAttempt<TimeSpan>("\"not a duration\"");

        // Items that cannot be converted are silently dropped, the rest of the array surviving.
        [Fact]
        public Task Array_with_unconvertible_item() => Verify<DateTime[]>("[ \"2018-12-17T08:00:00Z\", \"not a date\" ]");

        [Fact]
        public Task DynamicData() => Verify<dynamic>("{ \"values\": [ ], \"count\": { \"@type\": \"g:Int32\", \"@value\": 36 } }");

        [Fact]
        public Task Edge() => Verify<WorksFor>(UntypedEdge);

        // Should agree with Edge above: the typed and untyped wire forms must converge.
        [Fact]
        public Task Edge_from_typed_Edge() => Verify<WorksFor>(Graphson3_Edge);

        [Fact]
        public Task Property_from_typed_Property() => Verify<Property<int>>("""
            {
              "@type": "g:Property",
              "@value": {
                "key": "since",
                "value": { "@type": "g:Int32", "@value": 2009 }
              }
            }
            """);

        [Fact]
        public Task Empty_to_ints() => Verify<(int[] ints, string[] strings)>("{ \"Item1\": [], \"Item2\": [] }");

        [Fact]
        public Task Empty1() => Verify<object[]>("[]");

        [Fact]
        public Task Empty2() => Verify<Person[]>("[]");

        [Fact]
        public Task Graphson2Path() => Verify<Path>(Graphson2_Paths);

        [Fact]
        public Task GraphSon3_Tuple() => Verify<(Person, Language)[]>(Graphson3_Tuple_of_Person_Language);

        [Fact]
        public Task Graphson3Path() => Verify<Path>(Graphson3_Paths);

        [Fact]
        public Task GraphSon3ReferenceVertex() => Verify<object>(Graphson3ReferenceVertex);

        [Fact]
        public Task Guid() => Verify<Guid>("\"FCE0765A-454F-4D00-83DA-D76790156E29\"");

        [Fact]
        public Task Guid_from_typed_UUID() => Verify<Guid>("""{ "@type": "g:UUID", "@value": "41d2e28a-20a4-4ab0-b379-d810dede3786" }""");

        // The only test taking the "more specific type" route of TypedValueConverterFactory
        // for a value type, i.e. TransformerExtensions.FluentForType.FromStruct.
        [Fact]
        public Task Object_from_typed_UUID() => Verify<object>("""{ "@type": "g:UUID", "@value": "41d2e28a-20a4-4ab0-b379-d810dede3786" }""");

        [Fact]
        public Task Float_from_typed_value() => Verify<float>("""{ "@type": "g:Float", "@value": 1.5 }""");

        // 16777217 is 2^24 + 1, the smallest positive integer a float cannot represent. A
        // snapshot of 16777216 proves the g:Float row really did route through float.
        [Fact]
        public Task Object_from_typed_float() => Verify<object>("""{ "@type": "g:Float", "@value": 16777217 }""");

        [Fact]
        public Task Object_from_typed_double() => Verify<object>("""{ "@type": "g:Double", "@value": 16777217 }""");

        [Fact]
        public Task DateTimeOffset_from_typed_Date() => Verify<DateTimeOffset>("""{ "@type": "g:Date", "@value": 1657527969000 }""");

        // DateTime is not assignable from DateTimeOffset, so this is the only test taking
        // TypedValueConverterFactory's fall-through, where @value is re-dispatched untyped.
        [Fact]
        public Task DateTime_from_typed_Timestamp() => Verify<DateTime>("""{ "@type": "g:Timestamp", "@value": 1657527969000 }""");

        [Fact]
        public Task Object_from_typed_Direction() => Verify<object>("""{ "@type": "g:Direction", "@value": "OUT" }""");

        [Fact]
        public Task Direction_from_typed_Direction() => Verify<Direction>("""{ "@type": "g:Direction", "@value": "OUT" }""");

        [Fact]
        public Task Direction_from_unknown_typed_Direction() => VerifyAttempt<Direction>("""{ "@type": "g:Direction", "@value": "SIDEWAYS" }""");

        [Fact]
        public Task Object_from_typed_Merge() => Verify<object>("""{ "@type": "g:Merge", "@value": "onCreate" }""");

        [Fact]
        public Task Merge_from_typed_Merge() => Verify<Merge>("""{ "@type": "g:Merge", "@value": "onCreate" }""");

        [Fact]
        public Task Object_from_typed_T() => Verify<object>("""{ "@type": "g:T", "@value": "id" }""");

        [Fact]
        public Task T_from_typed_T() => Verify<T>("""{ "@type": "g:T", "@value": "id" }""");

        [Fact]
        public Task T_from_unknown_typed_T() => VerifyAttempt<T>("""{ "@type": "g:T", "@value": "unknown" }""");

        [Fact]
        public Task Decimal_from_typed_BigDecimal() => Verify<decimal>("""{ "@type": "gx:BigDecimal", "@value": 123.456 }""");

        [Fact]
        public Task Decimal_from_typed_BigDecimal_with_high_precision() => Verify<decimal>("""{ "@type": "gx:BigDecimal", "@value": 0.1234567890123456789012345 }""");

        [Fact]
        public Task BigInteger_from_typed_BigInteger() => Verify<BigInteger>("""{ "@type": "gx:BigInteger", "@value": 123456789987654321123456789987654321 }""");

        [Fact]
        public Task Byte_from_typed_Byte() => Verify<byte>("""{ "@type": "gx:Byte", "@value": 255 }""");

        [Fact]
        public Task Short_from_typed_Int16() => Verify<short>("""{ "@type": "gx:Int16", "@value": 32767 }""");

        [Fact]
        public Task Char_from_typed_Char() => Verify<char>("""{ "@type": "gx:Char", "@value": "x" }""");

        // Nested rather than top level, as Verify snapshots a bare byte[] as a binary file.
        // The nested form is the better coverage anyway - it drives gx:ByteBuffer through the
        // Newtonsoft serializer and back into the pipeline, Person.Image being a byte[].
        [Fact]
        public Task Person_with_typed_ByteBuffer_image() => Verify<Person>("""
            {
              "id": 13,
              "label": "Person",
              "type": "vertex",
              "properties": {
                "Image": [
                  {
                    "id": 1,
                    "value": { "@type": "gx:ByteBuffer", "@value": "c29tZSBieXRlcw==" }
                  }
                ]
              }
            }
            """);

        [Fact]
        public Task TimeSpan_from_typed_Duration() => Verify<TimeSpan>("""{ "@type": "gx:Duration", "@value": "P1DT2H3M4S" }""");

        [Fact]
        public Task IDictionary_string_keys_typed_int_values() => Verify<IDictionary<string, int>>(String_Keys_Typed_Int_Values);

        [Fact]
        public Task IReadOnlyDictionary_string_keys_typed_int_values() => Verify<IReadOnlyDictionary<string, int>>(String_Keys_Typed_Int_Values);

        [Fact]
        public Task IUntypedList_Typed_Ints() => Verify<IList>(Typed_Ints);

        [Fact]
        public Task IUntypedCollection_from_typed_ints() => Verify<ICollection>(Typed_Ints);

        [Fact]
        public Task ICollection_from_typed_ints() => Verify<ICollection<int>>(Typed_Ints);

        [Fact]
        public Task ImmutableArray() => Verify<ImmutableArray<int>>("[ 1, 3, 5 ]");

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
        public Task Int_Ids() => Verify<object[]>("[ 1, 2 ]");

        [Fact]
        public Task Ints_from_Traverser() => Verify<int[]>(Array_With_Traverser_With_Ints);

        [Fact]
        public Task List_Of_Ints_from_Traverser() => Verify<List<int>>(Array_With_Traverser_With_Ints);

        [Fact]
        public Task IList_Of_Ints_from_Traverser() => Verify<IList<int>>(Array_With_Traverser_With_Ints);

        // Traversers are only expanded by EnumerableConverterFactory, i.e. inside an array.
        [Fact]
        public Task Traverser_at_top_level() => Verify<Company>(Traverser);

        [Fact]
        public Task Language_by_vertex_inheritance() => Verify<object>(Single_Language);

        [Fact]
        public Task Language_strongly_typed() => Verify<Language>(Single_Language);

        [Fact]
        public Task Language_to_generic_vertex() => Verify<Vertex>(Single_Language);

        // FromBaseTypes<Company, Edge>() is a valid model whose vertex set excludes Language,
        // so the label lookup misses. Id and Label should still be set, as MemberMetadata
        // maps them to T.Id and T.Label independently of the model.
        [Fact]
        public Task Language_strongly_typed_without_matching_model() => Verify<Language>(
            Single_Language,
            env => env
                .UseModel(GraphModel
                    .FromBaseTypes<Company, Edge>()));

        [Fact]
        public Task Vertex_with_unknown_label_as_object() => Verify<object>("""
            {
              "id": 1,
              "label": "SomeUnknownLabel",
              "type": "vertex",
              "properties": {
                "SomeProperty": [ { "id": 2, "value": "SomeValue" } ]
              }
            }
            """);

        [Fact]
        public Task Language_unknown_type() => Verify<object>(Single_Language);

        [Fact]
        public Task Languages_to_object() => Verify<object>(ArrayOfLanguages);

        [Fact]
        public Task List_ints() => Verify<List<int>>("[ 1, 2, 3 ]");

        [Fact]
        public Task Meta_Properties() => Verify<Country>(Country_with_meta_properties);

        [Fact]
        public Task MetaProperties() => Verify<Property<object>[]>(Properties);

        [Fact]
        public Task Mixed_Ids() => Verify<object[]>("[ 1, \"id2\" ]");

        [Fact]
        public Task NamedTuple() => Verify<PersonLanguageTuple>(Named_tuple_of_Person_Language);

        [Fact]
        public Task Nested_Array() => Verify<Language[][]>(Nested_array_of_Languages);

        [Fact]
        public Task Nullable() => Verify<int?>("42");

        [Fact]
        public Task Nullable_null() => Verify<int?[]>("[ 42, null ]");

        [Fact]
        public Task Object_from_double() => Verify<object>("1.2");

        [Fact]
        public Task Object_from_true() => Verify<object>("true");

        [Fact]
        public Task Person_lowercase_strongly_typed() => Verify<Person>(Single_Person_lowercase_properties);

        [Fact]
        public Task Person_From_ElementMap() => Verify<Person>(Single_Person_ElementMap);

        [Fact]
        public Task Person_From_ElementMap_untyped() => Verify<object>(Single_Person_ElementMap);

        [Fact]
        public Task Person_StringId() => Verify<Person>(Single_Person_String_Id);

        [Fact]
        public Task Person_strongly_typed() => Verify<Person>(Single_Person);

        [Fact]
        public Task Person_with_null() => Verify<Person>(Single_Person_with_null);

        [Fact]
        public Task Person_without_PhoneNumbers_strongly_typed() => Verify<Person>(Single_Person_without_PhoneNumbers);

        [Fact]
        public Task Property_as_object() => Verify<object>("{ \"value\": 1540202009475, \"key\": \"Property1\" }");

        [Fact]
        public Task Property_from_Scalar() => Verify<Property<int>>("36");

        [Fact]
        public Task Scalar() => Verify<int>("36");

        [Fact]
        public Task Scalar_as_object() => Verify<object>("36");

        [Fact]
        public Task String_Ids() => Verify<object[]>("[ \"id1\", \"id2\" ]");

        [Fact]
        public Task String_Ids2() => Verify<object[]>("[ \"1\", \"2\" ]");

        [Fact]
        public Task TimeFrame_strongly_typed() => Verify<TimeFrame>(Single_TimeFrame);

        // Single_TimeFrame carries ISO 8601 durations, this one milliseconds, so together
        // they cover both string and integer arms of TimeSpanConverterFactory.
        [Fact]
        public Task TimeFrame_from_numbers() => Verify<TimeFrame>(Single_TimeFrame_with_numbers);

        [Fact]
        public Task TimeSpan_from_double() => Verify<TimeSpan>("123456789.2");

        [Fact]
        public Task TimeSpan_from_integer() => Verify<TimeSpan>("123456789");

        [Fact]
        public Task Tuple() => Verify<(Person, Language)>(Tuple_of_Person_Language);

        [Fact]
        public Task Tuple_vertex_vertex() => Verify<(Vertex, Vertex)>(Tuple_of_Person_Language);

        [Fact]
        public Task VertexProperties() => Verify<VertexProperty<object>[]>(Vertex_Properties);

        [Fact]
        public Task VertexProperties_with_model() => Verify<VertexProperty<object, MetaPoco>[]>(Vertex_Properties);

        [Fact]
        public Task VertexProperty_as_object() => Verify<object>("{ \"value\": 1540202009475, \"id\": 1, \"label\": \"Property1\", \"properties\": { \"metaKey\": \"MetaValue\" } }");

        [Fact]
        public Task VertexPropertyWithDateTimeOffset() => Verify<VertexProperty<string, PropertyValidity>>("{ \"id\": 166, \"value\": \"bob\", \"label\": \"Name\", \"properties\": { \"ValidFrom\": 1548112365431 } }");

        [Fact]
        public Task VertexPropertyWithoutProperties() => Verify<VertexProperty<object, object>>("{ \"id\": 166, \"value\": \"bob\", \"label\": \"Name\" }");

        [Fact]
        public Task Lifted_Entity() => Verify<IAuthority>("""
            {
              "id": "123",
              "label": "Person",
              "properties":
              {
                "age": 42
              }
            }
            """);

        [Fact]
        public Task Empty_tree_from_untyped_array() => Verify<Tree<object>>("[]");

        [Fact]
        public Task Empty_tree_from_untyped_array_2() => Verify<Tree<object, Tree<object>>>("[]");

        [Fact]
        public Task Empty_tree() => Verify<Tree<object>>(Empty_typed_tree);

        [Fact]
        public Task Empty_tree_2() => Verify<Tree<object, Tree<object>>>(Empty_typed_tree);

        [Fact]
        public Task RootOnly_string_tree() => Verify<Tree<string>>(GraphSonStrings.RootOnly_string_tree);

        [Fact]
        public Task RootOnly_int_tree() => Verify<Tree<int>>(GraphSonStrings.RootOnly_int_tree);

        [Fact]
        public Task RootOnly_int_tree_CosmosDb() => Verify<Tree<int>>(GraphSonStrings.RootOnly_int_tree_CosmosDb);

        [Fact]
        public Task RootOnly_string_tree_2() => Verify<Tree<string, Tree<object>>>(GraphSonStrings.RootOnly_string_tree);

        [Fact]
        public Task Linear_string_tree() => Verify<Tree<string>>(GraphSonStrings.Linear_string_tree);

        [Fact]
        public Task Branching_scalar_tree() => Verify<Tree<int, Tree<string>>>(GraphSonStrings.Branching_scalar_tree);

        [Fact]
        public Task Object_from_Branching_scalar_tree() => Verify<object>(GraphSonStrings.Branching_scalar_tree);

        [Fact]
        public Task Mixed_entity_and_scalar_tree() => Verify<Tree<Person, Tree<int>>>(GraphSonStrings.Mixed_entity_and_scalar_tree);

        [Fact]
        public Task Mixed_entity_and_scalar_as_object_tree() => Verify<Tree<object>>(GraphSonStrings.Mixed_entity_and_scalar_tree);

        [Fact]
        public Task Mixed_entity_and_scalar_tree_CosmosDb() => Verify<Tree<Person, Tree<int>>>(GraphSonStrings.Mixed_entity_and_scalar_tree_CosmosDb);

        [Fact]
        public Task Mixed_entity_and_scalar_as_object_tree_CosmosDb() => Verify<Tree<object>>(GraphSonStrings.Mixed_entity_and_scalar_tree_CosmosDb);
    }
}
