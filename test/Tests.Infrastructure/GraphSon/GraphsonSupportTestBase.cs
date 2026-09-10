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
    // These tests are the contract both serializer implementations verify against - hence
    // virtual: one that fulfils a test differently, reading a gx:BigDecimal without losing its
    // digits say, overrides that one and redirects its snapshot, rather than the shared file
    // having to hold two answers at once.
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
        public virtual Task Bug_1884() => Verify<Bug_1884_Entity>("""
            {
                "CreatedAt": 123456789,
                "MyEnum1": 0,
                "IsDeleted": true
            }
            """);

        [Fact]
        public virtual Task Constructor_assertion_1() => Verify<ClassWithFieldsAndConstructor>("{ }");

        [Fact]
        public virtual Task Constructor_assertion_2() => Verify<ClassWithFieldsAndConstructor>("""
            {
                "stringArg": "stringValue",
                "intArg": 42
            }
            """);

        [Fact]
        public virtual Task Constructor_assertion_3() => Verify<ClassWithFieldsAndConstructor>("""
            {
                "stringArg": "stringValue",
                "intArg": 42,
                "settableString": "settableValue"
            }
            """);

        [Fact]
        public virtual Task String_from_int() => Verify<string>("42");

        [Fact]
        public virtual Task Id_Key() => Verify<Key>("""
            {
                "@type": "g:T",
                "@value": "id"
            }
            """);

        [Fact]
        public virtual Task Label_Key() => Verify<Key>("""
            {
                "@type": "g:T",
                "@value": "label"
            }
            """);

        [Fact]
        public virtual Task Id_Key_from_string() => Verify<Key>("""
            "someId"
            """);

        [Fact]
        public virtual Task Label_Key_from_string() => Verify<Key>("""
            "someLabel"
            """);

        [Fact]
        public virtual Task Everything() => Verify<EverythingAllAtOnce>(EverythingAllAtOnceData);

        [Fact]
        public virtual Task Int_from_double() => Verify<int>("4.2");

        [Fact]
        public virtual Task Init_property() => Verify<ClassWithInitProperty>("""{ "Property": "Value" }""");

        [Fact]
        public virtual Task Field() => Verify<ClassWithField>("""{ "Property": "Value" }""");

        [Fact]
        public virtual Task IImmutableDictionary_string_keys_typed_int_values() => Verify<IImmutableDictionary<string, int>>(String_Keys_Typed_Int_Values);

        [Fact]
        public virtual Task Dictionary_typed_int_keys_string_values() => Verify<Dictionary<int, string>>(Map_of_Typed_Int_Keys_Typed_String_Values);

        [Fact]
        public virtual Task IUntypedDictionary_string_keys_typed_int_values() => Verify<IDictionary>(String_Keys_Typed_Int_Values);

        [Fact]
        public virtual Task IImmutableDictionary_typed_int_keys_string_values() => Verify<IImmutableDictionary<int, string>>(Map_of_Typed_Int_Keys_Typed_String_Values);

        // The same tolerance the bulk set gets, for the same reason: every other GraphSON type name
        // is matched case insensitively, so a map that shouts is still a map. There are two roads
        // into a map and each is pinned: into a dictionary, where the keys may be anything, and into
        // an object whose members are looked up by name, where they have to be strings - hence the
        // second pair, which reads Constructor_assertion_2's arguments out of a map instead of out
        // of a plain object, once as g:Map and once as G:MAP.
        [Fact]
        public virtual Task Map_with_uppercase_type() => Verify<Dictionary<int, string>>(Map_With_Uppercase_Type);

        [Fact]
        public virtual Task Constructor_arguments_from_map() => Verify<ClassWithFieldsAndConstructor>(Map_Of_Constructor_Arguments);

        [Fact]
        public virtual Task Constructor_arguments_from_map_with_uppercase_type() => Verify<ClassWithFieldsAndConstructor>(Map_Of_Constructor_Arguments_With_Uppercase_Type);

        [Fact]
        public virtual Task IEnumerable_from_Typed_Ints() => Verify<IEnumerable<int>>(Typed_Ints);

        [Fact]
        public virtual Task Untyped_IEnumerable_from_Typed_Ints() => Verify<IEnumerable>(Typed_Ints);

        [Fact]
        public virtual Task ISet_Typed_Ints() => Verify<ISet<int>>(Typed_Ints);

        [Fact]
        public virtual Task ISet_from_typed_Set() => Verify<ISet<int>>(Typed_Set_of_Ints);

        [Fact]
        public virtual Task IList_Typed_Ints() => Verify<IImmutableList<int>>(Typed_Ints);

        [Fact]
        public virtual Task IImmutableList_Ints() => Verify<IImmutableList<int>>(Typed_Ints);

        [Fact]
        public virtual Task IImmutableQueue_Ints() => Verify<IImmutableQueue<int>>(Typed_Ints);

        [Fact]
        public virtual Task IImmutableSet_Ints() => Verify<IImmutableSet<int>>(Typed_Ints);

        [Fact]
        public virtual Task IImmutableStack_Ints() => Verify<IImmutableStack<int>>(Typed_Ints);

        [Fact]
        public virtual Task ConcurrentQueue_from_typed_Ints() => Verify<ConcurrentQueue<int>>(Typed_Ints);

        [Fact]
        public virtual Task ConcurrentStack_from_typed_Ints() => Verify<ConcurrentStack<int>>(Typed_Ints);

        [Fact]
        public virtual Task ImmutableQueue_from_typed_Ints() => Verify<ImmutableQueue<int>>(Typed_Ints);

        [Fact]
        public virtual Task ImmutableStack_from_typed_Ints() => Verify<ImmutableStack<int>>(Typed_Ints);

        [Fact]
        public virtual Task IReadOnlyList_from_Ints() => Verify<IReadOnlyList<int>>(Ints);

        [Fact]
        public virtual Task IReadOnlyList_from_Typed_Ints() => Verify<IReadOnlyList<int>>(Typed_Ints);

        [Fact]
        public virtual Task Queue_from_typed_Ints() => Verify<Queue<int>>(Typed_Ints);

        [Fact]
        public virtual Task Stack_from_typed_Ints() => Verify<Stack<int>>(Typed_Ints);

        [Fact]
        public virtual Task ArrayList_from_typed_Ints() => Verify<ArrayList>(Typed_Ints);

        [Fact]
        public virtual Task Untyped_Queue_from_typed_Ints() => Verify<Queue>(Typed_Ints);

        [Fact]
        public virtual Task Untyped_Stack_from_typed_Ints() => Verify<Stack>(Typed_Ints);

        [Fact]
        public virtual Task Array() => Verify<Language[]>(ArrayOfLanguages);

        [Fact]
        public virtual Task Bulk_set() => Verify<string[]>(BulkSet);

        [Fact]
        public virtual Task Bulk_set_as_object() => Verify<object>(BulkSet);

        // Every other GraphSON type name in either implementation is matched case insensitively, so
        // the bulk set is expected to be too - and it has to be for the same reason the array does:
        // the converter that unwraps a typed value refuses g:BulkSet insensitively, so a spelling
        // only one of the two recognises falls between them and is answered by neither.
        [Fact]
        public virtual Task Bulk_set_with_uppercase_type() => Verify<string[]>(BulkSet_With_Uppercase_Type);

        [Fact]
        public virtual Task Bulk_set_with_uppercase_type_as_object() => Verify<object>(BulkSet_With_Uppercase_Type);

        // An array is not the only thing a bulk set can be read into. One test per way of building a
        // collection, as the traversers have below - and the answer is the expanded sequence every
        // time, 10 once, 20 twice, 30 three times, exactly as Ints_from_Bulk_set gets it.
        [Fact]
        public virtual Task Ints_from_Bulk_set() => Verify<int[]>(Typed_BulkSet);

        [Fact]
        public virtual Task List_Of_Ints_from_Bulk_set() => Verify<List<int>>(Typed_BulkSet);

        [Fact]
        public virtual Task IEnumerable_Of_Ints_from_Bulk_set() => Verify<IEnumerable<int>>(Typed_BulkSet);

        [Fact]
        public virtual Task ISet_Of_Ints_from_Bulk_set() => Verify<ISet<int>>(Typed_BulkSet);

        [Fact]
        public virtual Task ImmutableList_Of_Ints_from_Bulk_set() => Verify<ImmutableList<int>>(Typed_BulkSet);

        [Fact]
        public virtual Task Queue_Of_Ints_from_Bulk_set() => Verify<Queue<int>>(Typed_BulkSet);

        // The other half of the same sweep: the collections that are not generic. They are built
        // differently - from an ICollection, or as an object[] standing in for an interface - and so
        // are reached by a different road, but the answer is the same expanded sequence, reversed
        // only where a stack reverses it. Each mirrors the plain array test of the same shape.
        [Fact]
        public virtual Task ArrayList_from_Bulk_set() => Verify<ArrayList>(Typed_BulkSet);

        [Fact]
        public virtual Task Untyped_Queue_from_Bulk_set() => Verify<Queue>(Typed_BulkSet);

        [Fact]
        public virtual Task Untyped_Stack_from_Bulk_set() => Verify<Stack>(Typed_BulkSet);

        [Fact]
        public virtual Task Untyped_IEnumerable_from_Bulk_set() => Verify<IEnumerable>(Typed_BulkSet);

        [Fact]
        public virtual Task IUntypedList_from_Bulk_set() => Verify<IList>(Typed_BulkSet);

        [Fact]
        public virtual Task IUntypedCollection_from_Bulk_set() => Verify<ICollection>(Typed_BulkSet);

        // And a bulk set is not a scalar. Asked for one, the typed value unwrapper leaves it alone
        // rather than handing on the pair array, so nothing converts and the attempt declines -
        // which is the honest answer for a collection of six asked to be a single int.
        [Fact]
        public virtual Task Int_from_Bulk_set() => VerifyAttempt<int>(Typed_BulkSet);

        // A null in a bulk set is a null item repeated by its bulk, the same rule an array follows
        // and for the same reason - TryTransform reporting a conversion to null as a failure, which
        // is indistinguishable from a converter declining. Three, because the item type decides what
        // the null becomes: null where one fits, and the cost of it where none does.
        [Fact]
        public virtual Task Nullable_Ints_from_Bulk_set_with_null() => Verify<int?[]>(Typed_BulkSet_With_Null);

        [Fact]
        public virtual Task Ints_from_Bulk_set_with_null() => Verify<int[]>(Typed_BulkSet_With_Null);

        // Requested as object, the null used to come back as the JValue token itself, that being
        // assignable to object and so taken by the transformer's own fallback before any converter
        // could look at it. It is a null now, like it is for every other item type.
        [Fact]
        public virtual Task Objects_from_Bulk_set_with_null() => Verify<object[]>(Typed_BulkSet_With_Null);

        // The other way out of that condition, and the one a null no longer takes: an element the
        // requested type cannot read is still skipped, its bulk with it. Keeping a null and dropping
        // this are the same decision seen from either side - a converter that declines has said
        // there is no item here, where a null token says there is one and it is null.
        [Fact]
        public virtual Task Ints_from_Bulk_set_with_unreadable_element() => Verify<int[]>(Typed_BulkSet_With_Unreadable_Element);

        // The bulk half of the same question, and the same answer: a bulk nothing can read is not a
        // bulk of one, it is an unknown count, and an item repeated an unknown number of times
        // cannot be repeated at all. The pair goes, element included. Note this is deliberately
        // unlike a traverser, whose bulk defaults to 1 when absent or unreadable - there the value
        // stands on its own and the bulk only multiplies it, where here the two arrive as a pair and
        // neither half means anything without the other.
        [Fact]
        public virtual Task Ints_from_Bulk_set_with_unreadable_bulk() => Verify<int[]>(Typed_BulkSet_With_Unreadable_Bulk);

        // A bulk set is read in pairs, so an odd number of entries leaves a last element with
        // nothing to say how often it occurs. Both implementations used to reach past the end of
        // the array for that missing bulk and throw, which is the one outcome a converter must not
        // have: it takes the whole result set down rather than costing the one item it cannot read.
        // The well formed prefix is the answer, and the dangling element goes the way of any other
        // half a converter cannot make sense of.
        [Fact]
        public virtual Task Ints_from_Bulk_set_with_odd_length() => Verify<int[]>(Typed_BulkSet_With_Odd_Length);

        [Fact]
        public virtual Task Configured_property_name() => Verify<Person>(
            "{ \"id\": 13, \"label\": \"Person\", \"type\": \"vertex\", \"properties\": { \"replacement\": [ { \"id\": 1, \"value\": \"nameValue\" } ] } }",
            env => env
                .ConfigureModel(model => model
                    .ConfigureVertices(_ => _
                        .ConfigureElement<Person>(conf => conf
                            .ConfigureName(x => x.Name, "replacement")))));

        [Fact]
        public virtual Task DateTime_from_double() => Verify<DateTime>("123456789.2");

        [Fact]
        public virtual Task DateTime_from_number() => Verify<DateTime>("123456789");

        [Fact]
        public virtual Task DateTime_from_string() => Verify<DateTime>("\"2018-12-17T08:00:00Z\"");

        [Fact]
        public virtual Task DateTime_is_UTC() => Verify<Company>(Single_Company);

        [Fact]
        public virtual Task DateTimeOffset_from_number() => Verify<DateTimeOffset>("123456789");

        [Fact]
        public virtual Task DateTimeOffset_from_string() => Verify<DateTimeOffset>("\"2018-12-17T08:00:00Z\"");

        [Fact]
        public virtual Task DateTime_from_invalid_string() => VerifyAttempt<DateTime>("\"not a date\"");

        [Fact]
        public virtual Task DateTimeOffset_from_invalid_string() => VerifyAttempt<DateTimeOffset>("\"not a date\"");

        [Fact]
        public virtual Task TimeSpan_from_invalid_string() => VerifyAttempt<TimeSpan>("\"not a duration\"");

        // Items that cannot be converted are silently dropped, the rest of the array surviving.
        [Fact]
        public virtual Task Array_with_unconvertible_item() => Verify<DateTime[]>("[ \"2018-12-17T08:00:00Z\", \"not a date\" ]");

        [Fact]
        public virtual Task DynamicData() => Verify<dynamic>("{ \"values\": [ ], \"count\": { \"@type\": \"g:Int32\", \"@value\": 36 } }");

        [Fact]
        public virtual Task Edge() => Verify<WorksFor>(UntypedEdge);

        // Should agree with Edge above: the typed and untyped wire forms must converge.
        [Fact]
        public virtual Task Edge_from_typed_Edge() => Verify<WorksFor>(Graphson3_Edge);

        [Fact]
        public virtual Task Property_from_typed_Property() => Verify<Property<int>>("""
            {
              "@type": "g:Property",
              "@value": {
                "key": "since",
                "value": { "@type": "g:Int32", "@value": 2009 }
              }
            }
            """);

        [Fact]
        public virtual Task Empty_to_ints() => Verify<(int[] ints, string[] strings)>("{ \"Item1\": [], \"Item2\": [] }");

        [Fact]
        public virtual Task Empty1() => Verify<object[]>("[]");

        [Fact]
        public virtual Task Empty2() => Verify<Person[]>("[]");

        [Fact]
        public virtual Task Graphson2Path() => Verify<Path>(Graphson2_Paths);

        [Fact]
        public virtual Task GraphSon3_Tuple() => Verify<(Person, Language)[]>(Graphson3_Tuple_of_Person_Language);

        [Fact]
        public virtual Task Graphson3Path() => Verify<Path>(Graphson3_Paths);

        [Fact]
        public virtual Task GraphSon3ReferenceVertex() => Verify<object>(Graphson3ReferenceVertex);

        [Fact]
        public virtual Task Guid() => Verify<Guid>("\"FCE0765A-454F-4D00-83DA-D76790156E29\"");

        [Fact]
        public virtual Task Guid_from_typed_UUID() => Verify<Guid>("""{ "@type": "g:UUID", "@value": "41d2e28a-20a4-4ab0-b379-d810dede3786" }""");

        // The only test taking the "more specific type" route of TypedValueConverterFactory
        // for a value type, i.e. TransformerExtensions.FluentForType.FromStruct.
        [Fact]
        public virtual Task Object_from_typed_UUID() => Verify<object>("""{ "@type": "g:UUID", "@value": "41d2e28a-20a4-4ab0-b379-d810dede3786" }""");

        [Fact]
        public virtual Task Float_from_typed_value() => Verify<float>("""{ "@type": "g:Float", "@value": 1.5 }""");

        // 16777217 is 2^24 + 1, the smallest positive integer a float cannot represent. A
        // snapshot of 16777216 proves the g:Float row really did route through float.
        [Fact]
        public virtual Task Object_from_typed_float() => Verify<object>("""{ "@type": "g:Float", "@value": 16777217 }""");

        [Fact]
        public virtual Task Object_from_typed_double() => Verify<object>("""{ "@type": "g:Double", "@value": 16777217 }""");

        [Fact]
        public virtual Task DateTimeOffset_from_typed_Date() => Verify<DateTimeOffset>("""{ "@type": "g:Date", "@value": 1657527969000 }""");

        // DateTime is not assignable from DateTimeOffset, so this is the only test taking
        // TypedValueConverterFactory's fall-through, where @value is re-dispatched untyped.
        [Fact]
        public virtual Task DateTime_from_typed_Timestamp() => Verify<DateTime>("""{ "@type": "g:Timestamp", "@value": 1657527969000 }""");

        [Fact]
        public virtual Task Object_from_typed_Direction() => Verify<object>("""{ "@type": "g:Direction", "@value": "OUT" }""");

        [Fact]
        public virtual Task Direction_from_typed_Direction() => Verify<Direction>("""{ "@type": "g:Direction", "@value": "OUT" }""");

        [Fact]
        public virtual Task Direction_from_unknown_typed_Direction() => VerifyAttempt<Direction>("""{ "@type": "g:Direction", "@value": "SIDEWAYS" }""");

        [Fact]
        public virtual Task Object_from_typed_Merge() => Verify<object>("""{ "@type": "g:Merge", "@value": "onCreate" }""");

        [Fact]
        public virtual Task Merge_from_typed_Merge() => Verify<Merge>("""{ "@type": "g:Merge", "@value": "onCreate" }""");

        [Fact]
        public virtual Task Object_from_typed_T() => Verify<object>("""{ "@type": "g:T", "@value": "id" }""");

        [Fact]
        public virtual Task T_from_typed_T() => Verify<T>("""{ "@type": "g:T", "@value": "id" }""");

        [Fact]
        public virtual Task T_from_unknown_typed_T() => VerifyAttempt<T>("""{ "@type": "g:T", "@value": "unknown" }""");

        [Fact]
        public virtual Task Decimal_from_typed_BigDecimal() => Verify<decimal>("""{ "@type": "gx:BigDecimal", "@value": 123.456 }""");

        [Fact]
        public virtual Task Decimal_from_typed_BigDecimal_with_high_precision() => Verify<decimal>("""{ "@type": "gx:BigDecimal", "@value": 0.1234567890123456789012345 }""");

        [Fact]
        public virtual Task BigInteger_from_typed_BigInteger() => Verify<BigInteger>("""{ "@type": "gx:BigInteger", "@value": 123456789987654321123456789987654321 }""");

        [Fact]
        public virtual Task Byte_from_typed_Byte() => Verify<byte>("""{ "@type": "gx:Byte", "@value": 255 }""");

        [Fact]
        public virtual Task Short_from_typed_Int16() => Verify<short>("""{ "@type": "gx:Int16", "@value": 32767 }""");

        [Fact]
        public virtual Task Char_from_typed_Char() => Verify<char>("""{ "@type": "gx:Char", "@value": "x" }""");

        // Nested rather than top level, as Verify snapshots a bare byte[] as a binary file.
        // The nested form is the better coverage anyway - it drives gx:ByteBuffer through the
        // Newtonsoft serializer and back into the pipeline, Person.Image being a byte[].
        [Fact]
        public virtual Task Person_with_typed_ByteBuffer_image() => Verify<Person>("""
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
        public virtual Task TimeSpan_from_typed_Duration() => Verify<TimeSpan>("""{ "@type": "gx:Duration", "@value": "P1DT2H3M4S" }""");

        [Fact]
        public virtual Task IDictionary_string_keys_typed_int_values() => Verify<IDictionary<string, int>>(String_Keys_Typed_Int_Values);

        [Fact]
        public virtual Task IReadOnlyDictionary_string_keys_typed_int_values() => Verify<IReadOnlyDictionary<string, int>>(String_Keys_Typed_Int_Values);

        [Fact]
        public virtual Task IUntypedList_Typed_Ints() => Verify<IList>(Typed_Ints);

        [Fact]
        public virtual Task IUntypedCollection_from_typed_ints() => Verify<ICollection>(Typed_Ints);

        [Fact]
        public virtual Task ICollection_from_typed_ints() => Verify<ICollection<int>>(Typed_Ints);

        [Fact]
        public virtual Task ImmutableArray() => Verify<ImmutableArray<int>>("[ 1, 3, 5 ]");

        [Fact]
        public virtual Task ImmutableArray_typed_ints() => Verify<ImmutableArray<int>>(Typed_Ints);

        [Fact]
        public virtual Task ImmutableDictionary_map_of_string_keys_typed_int_values() => Verify<ImmutableDictionary<string, int>>(Map_of_String_Keys_Typed_Int_Values);

        [Fact]
        public virtual Task ImmutableDictionary_string_keys_int_values() => Verify<ImmutableDictionary<string, int>>(String_Keys_Int_Values);

        [Fact]
        public virtual Task ImmutableDictionary_string_keys_typed_int_values() => Verify<ImmutableDictionary<string, int>>(String_Keys_Typed_Int_Values);

        [Fact]
        public virtual Task ImmutableList_ints() => Verify<ImmutableList<int>>(Ints);

        [Fact]
        public virtual Task ImmutableList_typed_ints() => Verify<ImmutableList<int>>(Typed_Ints);

        [Fact]
        public virtual Task Int_Ids() => Verify<object[]>("[ 1, 2 ]");

        // Every traverser test is array-shaped because that is the only shape there is. A response
        // carries its results as a g:List of traversers, so a traverser is always an element of an
        // array, and bulk is applied at the terminal step, never deeper in the payload.
        [Fact]
        public virtual Task Ints_from_Traverser() => Verify<int[]>(Array_With_Traverser_With_Ints);

        [Fact]
        public virtual Task List_Of_Ints_from_Traverser() => Verify<List<int>>(Array_With_Traverser_With_Ints);

        [Fact]
        public virtual Task IList_Of_Ints_from_Traverser() => Verify<IList<int>>(Array_With_Traverser_With_Ints);

        // The three above are assignable from List<int> and are the only shapes expansion used to
        // reach. Everything below fell through to Newtonsoft, which builds the collection but
        // expands no bulk - and, the traverser converting to nothing an int collection will take,
        // threw rather than returning a short answer. One test per way of building a collection.
        [Fact]
        public virtual Task ImmutableList_Of_Ints_from_Traverser() => Verify<ImmutableList<int>>(Array_With_Traverser_With_Ints);

        [Fact]
        public virtual Task IImmutableList_Of_Ints_from_Traverser() => Verify<IImmutableList<int>>(Array_With_Traverser_With_Ints);

        [Fact]
        public virtual Task ImmutableArray_Of_Ints_from_Traverser() => Verify<ImmutableArray<int>>(Array_With_Traverser_With_Ints);

        [Fact]
        public virtual Task IImmutableSet_Of_Ints_from_Traverser() => Verify<IImmutableSet<int>>(Array_With_Traverser_With_Ints);

        [Fact]
        public virtual Task ImmutableSortedSet_Of_Ints_from_Traverser() => Verify<ImmutableSortedSet<int>>(Array_With_Traverser_With_Ints);

        [Fact]
        public virtual Task ImmutableQueue_Of_Ints_from_Traverser() => Verify<ImmutableQueue<int>>(Array_With_Traverser_With_Ints);

        [Fact]
        public virtual Task ImmutableStack_Of_Ints_from_Traverser() => Verify<ImmutableStack<int>>(Array_With_Traverser_With_Ints);

        // A set collapses the seven 42s into one, so these two pin down that the value survives at
        // all, not that the bulk does.
        [Fact]
        public virtual Task ISet_Of_Ints_from_Traverser() => Verify<ISet<int>>(Array_With_Traverser_With_Ints);

        [Fact]
        public virtual Task Queue_Of_Ints_from_Traverser() => Verify<Queue<int>>(Array_With_Traverser_With_Ints);

        [Fact]
        public virtual Task Stack_Of_Ints_from_Traverser() => Verify<Stack<int>>(Array_With_Traverser_With_Ints);

        [Fact]
        public virtual Task ConcurrentQueue_Of_Ints_from_Traverser() => Verify<ConcurrentQueue<int>>(Array_With_Traverser_With_Ints);

        // Traversers standing among plain values, twice over, so that what comes before the first
        // one survives and the second one is expanded too.
        [Fact]
        public virtual Task ImmutableList_from_Array_with_Traversers_and_plain_values() => Verify<ImmutableList<int>>(Array_With_Traversers_Among_Plain_Ints);

        // A traverser wrapping null is null as often as its bulk says - the one place a null
        // survives into a deserialized array.
        [Fact]
        public virtual Task ImmutableList_Of_Nullable_Ints_from_Traverser_with_null() => Verify<ImmutableList<int?>>(Array_With_Traverser_With_Null);

        // An expanded traverser runs the ordinary pipeline, vertex heuristics included - a bulk of
        // 3 around a vertex is three companies.
        [Fact]
        public virtual Task ImmutableList_Of_Companies_from_Traverser() => Verify<ImmutableList<Company>>(Array_With_Traverser_With_Company);

        [Fact]
        public virtual Task Language_by_vertex_inheritance() => Verify<object>(Single_Language);

        [Fact]
        public virtual Task Language_strongly_typed() => Verify<Language>(Single_Language);

        [Fact]
        public virtual Task Language_to_generic_vertex() => Verify<Vertex>(Single_Language);

        // FromBaseTypes<Company, Edge>() is a valid model whose vertex set excludes Language,
        // so the label lookup misses. Id and Label should still be set, as MemberMetadata
        // maps them to T.Id and T.Label independently of the model.
        [Fact]
        public virtual Task Language_strongly_typed_without_matching_model() => Verify<Language>(
            Single_Language,
            env => env
                .UseModel(GraphModel
                    .FromBaseTypes<Company, Edge>()));

        [Fact]
        public virtual Task Vertex_with_unknown_label_as_object() => Verify<object>("""
            {
              "id": 1,
              "label": "SomeUnknownLabel",
              "type": "vertex",
              "properties": {
                "SomeProperty": [ { "id": 2, "value": "SomeValue" } ]
              }
            }
            """);

        // The same vertex as an elementMap() returns it: a g:Map whose id and label arrive under g:T
        // keys rather than as named properties. Its label is unknown to the model, so nothing builds
        // an entity from it, and asked for as an object it is still an element - id and label kept,
        // everything else under properties, the shape the plain vertex above keeps as well.
        [Fact]
        public virtual Task Element_map_with_unknown_label_as_object() => Verify<object>("""
            {
              "@type": "g:Map",
              "@value": [
                { "@type": "g:T", "@value": "id" },
                { "@type": "g:Int64", "@value": 1 },
                { "@type": "g:T", "@value": "label" },
                "SomeUnknownLabel",
                "SomeProperty",
                "SomeValue"
              ]
            }
            """);

        [Fact]
        public virtual Task Language_unknown_type() => Verify<object>(Single_Language);

        [Fact]
        public virtual Task Languages_to_object() => Verify<object>(ArrayOfLanguages);

        [Fact]
        public virtual Task List_ints() => Verify<List<int>>("[ 1, 2, 3 ]");

        [Fact]
        public virtual Task Meta_Properties() => Verify<Country>(Country_with_meta_properties);

        [Fact]
        public virtual Task MetaProperties() => Verify<Property<object>[]>(Properties);

        [Fact]
        public virtual Task Mixed_Ids() => Verify<object[]>("[ 1, \"id2\" ]");

        [Fact]
        public virtual Task NamedTuple() => Verify<PersonLanguageTuple>(Named_tuple_of_Person_Language);

        [Fact]
        public virtual Task Nested_Array() => Verify<Language[][]>(Nested_array_of_Languages);

        [Fact]
        public virtual Task Nullable() => Verify<int?>("42");

        [Fact]
        public virtual Task Nullable_null() => Verify<int?[]>("[ 42, null ]");

        // The other side of the rule that lets a null survive inside an array: at the top level
        // there is no enclosing structure to hold it, so the null would have to be the
        // transformation's own result, and TryTransform cannot report one. NullableConverter
        // answers with a successful null and it is discarded, being indistinguishable from a
        // decline. Which is the whole reason an array has to decide for itself.
        [Fact]
        public virtual Task Nullable_null_at_top_level() => VerifyAttempt<int?>("null");

        // The other way a nullable can come to nothing, and a different one: here the token is
        // there to be read and the requested type simply cannot read it.
        [Fact]
        public virtual Task Nullable_from_invalid_string() => VerifyAttempt<int?>("\"not a number\"");

        // What default! means when the item type cannot hold a null. The alternative is to drop the
        // element, which is worse: an array's length is an answer of its own, and shortening it
        // silently reports fewer results than came back.
        [Fact]
        public virtual Task Ints_from_Array_with_null() => Verify<int[]>("[ 1, null, 3 ]");

        // Requested as object, a null element used to come back as the JValue token itself, that
        // being assignable to object and so accepted by the Newtonsoft converter before anything
        // else could look at it. It is a null now, like it is for every other item type.
        [Fact]
        public virtual Task Objects_from_Array_with_null() => Verify<object[]>("[ 1, null, 3 ]");

        [Fact]
        public virtual Task Object_from_double() => Verify<object>("1.2");

        [Fact]
        public virtual Task Object_from_true() => Verify<object>("true");

        [Fact]
        public virtual Task Person_lowercase_strongly_typed() => Verify<Person>(Single_Person_lowercase_properties);

        [Fact]
        public virtual Task Person_From_ElementMap() => Verify<Person>(Single_Person_ElementMap);

        [Fact]
        public virtual Task Person_From_ElementMap_untyped() => Verify<object>(Single_Person_ElementMap);

        [Fact]
        public virtual Task Person_StringId() => Verify<Person>(Single_Person_String_Id);

        [Fact]
        public virtual Task Person_strongly_typed() => Verify<Person>(Single_Person);

        [Fact]
        public virtual Task Person_with_null() => Verify<Person>(Single_Person_with_null);

        [Fact]
        public virtual Task Person_without_PhoneNumbers_strongly_typed() => Verify<Person>(Single_Person_without_PhoneNumbers);

        [Fact]
        public virtual Task Property_as_object() => Verify<object>("{ \"value\": 1540202009475, \"key\": \"Property1\" }");

        [Fact]
        public virtual Task Property_from_Scalar() => Verify<Property<int>>("36");

        [Fact]
        public virtual Task Scalar() => Verify<int>("36");

        [Fact]
        public virtual Task Scalar_as_object() => Verify<object>("36");

        [Fact]
        public virtual Task String_Ids() => Verify<object[]>("[ \"id1\", \"id2\" ]");

        [Fact]
        public virtual Task String_Ids2() => Verify<object[]>("[ \"1\", \"2\" ]");

        [Fact]
        public virtual Task TimeFrame_strongly_typed() => Verify<TimeFrame>(Single_TimeFrame);

        // Single_TimeFrame carries ISO 8601 durations, this one milliseconds, so together
        // they cover both string and integer arms of TimeSpanConverterFactory.
        [Fact]
        public virtual Task TimeFrame_from_numbers() => Verify<TimeFrame>(Single_TimeFrame_with_numbers);

        [Fact]
        public virtual Task TimeSpan_from_double() => Verify<TimeSpan>("123456789.2");

        [Fact]
        public virtual Task TimeSpan_from_integer() => Verify<TimeSpan>("123456789");

        [Fact]
        public virtual Task Tuple() => Verify<(Person, Language)>(Tuple_of_Person_Language);

        [Fact]
        public virtual Task Tuple_vertex_vertex() => Verify<(Vertex, Vertex)>(Tuple_of_Person_Language);

        [Fact]
        public virtual Task VertexProperties() => Verify<VertexProperty<object>[]>(Vertex_Properties);

        [Fact]
        public virtual Task VertexProperties_with_model() => Verify<VertexProperty<object, MetaPoco>[]>(Vertex_Properties);

        [Fact]
        public virtual Task VertexProperty_as_object() => Verify<object>("{ \"value\": 1540202009475, \"id\": 1, \"label\": \"Property1\", \"properties\": { \"metaKey\": \"MetaValue\" } }");

        [Fact]
        public virtual Task VertexPropertyWithDateTimeOffset() => Verify<VertexProperty<string, PropertyValidity>>("{ \"id\": 166, \"value\": \"bob\", \"label\": \"Name\", \"properties\": { \"ValidFrom\": 1548112365431 } }");

        [Fact]
        public virtual Task VertexPropertyWithoutProperties() => Verify<VertexProperty<object, object>>("{ \"id\": 166, \"value\": \"bob\", \"label\": \"Name\" }");

        [Fact]
        public virtual Task Lifted_Entity() => Verify<IAuthority>("""
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
        public virtual Task Empty_tree_from_untyped_array() => Verify<Tree<object>>("[]");

        [Fact]
        public virtual Task Empty_tree_from_untyped_array_2() => Verify<Tree<object, Tree<object>>>("[]");

        [Fact]
        public virtual Task Empty_tree() => Verify<Tree<object>>(Empty_typed_tree);

        [Fact]
        public virtual Task Empty_tree_2() => Verify<Tree<object, Tree<object>>>(Empty_typed_tree);

        [Fact]
        public virtual Task RootOnly_string_tree() => Verify<Tree<string>>(GraphSonStrings.RootOnly_string_tree);

        [Fact]
        public virtual Task RootOnly_int_tree() => Verify<Tree<int>>(GraphSonStrings.RootOnly_int_tree);

        [Fact]
        public virtual Task RootOnly_int_tree_CosmosDb() => Verify<Tree<int>>(GraphSonStrings.RootOnly_int_tree_CosmosDb);

        [Fact]
        public virtual Task RootOnly_string_tree_2() => Verify<Tree<string, Tree<object>>>(GraphSonStrings.RootOnly_string_tree);

        [Fact]
        public virtual Task Linear_string_tree() => Verify<Tree<string>>(GraphSonStrings.Linear_string_tree);

        [Fact]
        public virtual Task Branching_scalar_tree() => Verify<Tree<int, Tree<string>>>(GraphSonStrings.Branching_scalar_tree);

        [Fact]
        public virtual Task Object_from_Branching_scalar_tree() => Verify<object>(GraphSonStrings.Branching_scalar_tree);

        [Fact]
        public virtual Task Mixed_entity_and_scalar_tree() => Verify<Tree<Person, Tree<int>>>(GraphSonStrings.Mixed_entity_and_scalar_tree);

        [Fact]
        public virtual Task Mixed_entity_and_scalar_as_object_tree() => Verify<Tree<object>>(GraphSonStrings.Mixed_entity_and_scalar_tree);

        [Fact]
        public virtual Task Mixed_entity_and_scalar_tree_CosmosDb() => Verify<Tree<Person, Tree<int>>>(GraphSonStrings.Mixed_entity_and_scalar_tree_CosmosDb);

        [Fact]
        public virtual Task Mixed_entity_and_scalar_as_object_tree_CosmosDb() => Verify<Tree<object>>(GraphSonStrings.Mixed_entity_and_scalar_tree_CosmosDb);
    }
}
