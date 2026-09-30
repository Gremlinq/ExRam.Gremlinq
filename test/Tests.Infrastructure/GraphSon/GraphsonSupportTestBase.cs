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

        // A .NET enum is read from the name of one of its values, as it is from the number of one.
        // The name is found however it is spelled, as a member of a type the caller asks for is,
        // and whatever whitespace surrounds it. A name the enum does not have is no value of it,
        // and neither is no name at all.
        [Fact]
        public virtual Task Enum_from_name() => Verify<Gender>("\"Female\"");

        [Fact]
        public virtual Task Enum_from_name_in_other_case() => Verify<Gender>("\"female\"");

        [Fact]
        public virtual Task Enum_from_name_in_whitespace() => Verify<Gender>("\" Female \"");

        [Fact]
        public virtual Task Enum_from_unknown_name() => VerifyAttempt<Gender>("\"Other\"");

        [Fact]
        public virtual Task Enum_from_empty_string() => VerifyAttempt<Gender>("\"\"");

        // Found however it is spelled, a name is found twice in an enum with two values that differ
        // in nothing but case. The one spelled exactly is the one that counts - here the second of
        // the two - and where none is, the first.
        [Fact]
        public virtual Task Enum_from_name_spelled_as_one_of_two() => Verify<EnumWithNamesInTwoCases>("\"VALUE\"");

        [Fact]
        public virtual Task Enum_from_name_spelled_as_none_of_two() => Verify<EnumWithNamesInTwoCases>("\"value\"");

        // A number in a string is the value of that number, as the number itself is - whether the
        // enum has a name for it or not. One that does not fit the type underlying the enum is no
        // value of it.
        [Fact]
        public virtual Task Enum_from_number_in_string() => Verify<Gender>("\"1\"");

        [Fact]
        public virtual Task Enum_from_number_without_name_in_string() => Verify<Gender>("\"42\"");

        [Fact]
        public virtual Task Enum_from_number_out_of_range_in_string() => VerifyAttempt<Gender>("\"4294967296\"");

        // Names separated by commas are their values combined, each name found the way a single one
        // is. One among them that the enum does not have leaves nothing to read, and so does a comma
        // with no name after it.
        [Fact]
        public virtual Task Flags_enum_from_names() => Verify<FlagsEnum>("\"Read, execute\"");

        [Fact]
        public virtual Task Flags_enum_from_names_with_unknown_name() => VerifyAttempt<FlagsEnum>("\"Read, Other\"");

        [Fact]
        public virtual Task Flags_enum_from_names_with_trailing_comma() => VerifyAttempt<FlagsEnum>("\"Read,\"");

        // Whether the enum is [Flags] is not asked, as Enum.Parse does not ask either: Female and
        // NonBinary are 1 and 2, and combine into a 3 that Gender has no name for.
        [Fact]
        public virtual Task Enum_without_Flags_from_names() => Verify<Gender>("\"Female, NonBinary\"");

        // A value with an [EnumMember] is read from the name given there, and still from its own.
        // The name given there may be anything a string can be, so the whole text is looked for
        // among those names before a comma in it separates anything.
        [Fact]
        public virtual Task Enum_from_EnumMember_name() => Verify<EnumWithEnumMembers>("\"not-started\"");

        [Fact]
        public virtual Task Enum_from_EnumMember_name_in_other_case() => Verify<EnumWithEnumMembers>("\"NOT-STARTED\"");

        [Fact]
        public virtual Task Enum_from_own_name_despite_EnumMember() => Verify<EnumWithEnumMembers>("\"NotStarted\"");

        [Fact]
        public virtual Task Enum_from_EnumMember_name_with_comma() => Verify<EnumWithEnumMembers>("\"started, not done\"");

        [Fact]
        public virtual Task Enum_from_EnumMember_name_with_comma_in_other_case() => Verify<EnumWithEnumMembers>("\"STARTED, NOT DONE\"");

        // A nullable enum is read the way the enum is. A name it does not have is not read as null:
        // there is a token to read and the enum cannot read it, which is a decline.
        [Fact]
        public virtual Task Nullable_enum_from_name() => Verify<Gender?>("\"female\"");

        [Fact]
        public virtual Task Nullable_enum_from_unknown_name() => VerifyAttempt<Gender?>("\"Other\"");

        [Fact]
        public virtual Task Nullable_enum_from_empty_string() => VerifyAttempt<Gender?>("\"\"");

        // GraphSON has no type for an enum, so inside a typed value there is a number to read, and
        // it is read as the bare number is.
        [Fact]
        public virtual Task Enum_from_typed_Int32() => Verify<Gender>("""{ "@type": "g:Int32", "@value": 1 }""");

        // An enum is read from a name wherever one is read. As an element of an array, where a name
        // the enum does not have is dropped like any item that cannot be converted, and a null is
        // kept where the item type can hold one.
        [Fact]
        public virtual Task Enums_from_Array_of_names() => Verify<Gender[]>("""[ "Female", "nonbinary", 1, "Other", "0" ]""");

        [Fact]
        public virtual Task Nullable_enums_from_Array_of_names() => Verify<Gender?[]>("""[ "Female", null, "Other", "nonbinary" ]""");

        [Fact]
        public virtual Task Enums_from_typed_List_of_names() => Verify<Gender[]>("""{ "@type": "g:List", "@value": [ "Female", "nonbinary" ] }""");

        // As a value of a dictionary, read from an object or from a g:Map - where an entry whose
        // value is a name the enum does not have is left out, as the array leaves out the item.
        [Fact]
        public virtual Task Enum_dictionary_from_object_with_names() => Verify<Dictionary<string, Gender>>("""{ "first": "Female", "second": "nonbinary", "third": 1 }""");

        [Fact]
        public virtual Task Enum_dictionary_from_map_with_names() => Verify<Dictionary<string, Gender>>("""
            {
              "@type": "g:Map",
              "@value": [ "first", "Female", "second", "nonbinary", "third", { "@type": "g:Int32", "@value": 1 } ]
            }
            """);

        [Fact]
        public virtual Task Enum_dictionary_from_map_with_unknown_name() => Verify<Dictionary<string, Gender>>("""
            {
              "@type": "g:Map",
              "@value": [ "first", "Female", "second", "Other" ]
            }
            """);

        // And as a member of an entity, which is where an enum stored under its name comes back
        // from a graph: as the property of a vertex, however the vertex arrives - plain, typed, or
        // as valueMap() returns it. A name the enum does not have leaves the member as it was, and
        // costs nothing but itself.
        [Fact]
        public virtual Task Person_with_Gender_name() => Verify<Person>("""
            {
              "id": 1,
              "label": "Person",
              "properties": {
                "Gender": [ { "id": 2, "value": "Female" } ]
              }
            }
            """);

        [Fact]
        public virtual Task Person_with_Gender_name_typed() => Verify<Person>("""
            {
              "@type": "g:Vertex",
              "@value": {
                "id": { "@type": "g:Int64", "@value": 1 },
                "label": "Person",
                "properties": {
                  "Gender": [
                    {
                      "@type": "g:VertexProperty",
                      "@value": {
                        "id": { "@type": "g:Int64", "@value": 2 },
                        "value": "female",
                        "label": "Gender"
                      }
                    }
                  ]
                }
              }
            }
            """);

        [Fact]
        public virtual Task Person_from_map_with_Gender_name() => Verify<Person>("""
            {
              "@type": "g:Map",
              "@value": [ "Gender", [ "Female" ] ]
            }
            """);

        [Fact]
        public virtual Task Person_with_unknown_Gender_name() => Verify<Person>("""
            {
              "id": 1,
              "label": "Person",
              "properties": {
                "Age": [ { "id": 2, "value": 36 } ],
                "Gender": [ { "id": 3, "value": "Other" } ]
              }
            }
            """);

        [Fact]
        public virtual Task VertexProperty_with_enum_name() => Verify<VertexProperty<Gender>>("""{ "id": 2, "value": "Female", "label": "Gender" }""");

        // Bug_1884 with the enum under its name: ValueA, where the entity sets ValueB by itself.
        [Fact]
        public virtual Task Bug_1884_with_enum_name() => Verify<Bug_1884_Entity>("""
            {
                "CreatedAt": 123456789,
                "MyEnum1": "ValueA",
                "IsDeleted": true
            }
            """);

        // Constructor arguments are read the same way as members are.
        [Fact]
        public virtual Task Constructor_with_enum_names() => Verify<ClassWithEnumConstructor>("""{ "gender": "Female", "nullableGender": "nonbinary" }""");

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

        // The same map with nothing said about its type. Its keys are not names, so it cannot become
        // a dynamic object the way a string-keyed map does - but it is still a map, and it keeps its
        // entries, keys as they were typed. The snapshot of a dictionary writes an int key and a
        // string key alike, so the types are recorded alongside it: a map that only kept its entries
        // by turning 1 into "1" would not have kept them.
        [Fact]
        public virtual Task Map_of_typed_int_keys_as_object()
        {
            var subject = _environment
                .Deserializer
                .TransformTo<object>()
                .From(CreateNativeToken(Map_of_Typed_Int_Keys_Typed_String_Values), _environment);

            return Verifier
                .Verify(
                    new
                    {
                        Type = subject.GetType(),
                        KeyTypes = subject is IDictionary dictionary
                            ? dictionary.Keys.Cast<object>().Select(static key => key.GetType()).ToArray()
                            : null,
                        Value = subject
                    },
                    sourceFile: _sourceFile)
                .DontScrubDateTimes();
        }

        // Asked for as anything but an object, such a map is read the way a map keyed by names is:
        // the members are looked up by name, and an entry whose key cannot be one is left out. It
        // costs nothing but itself.
        [Fact]
        public virtual Task Constructor_arguments_from_map_with_int_key() => Verify<ClassWithFieldsAndConstructor>(Map_Of_Constructor_Arguments_With_Int_Key);

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

        // The strings above name UTC. One that names an offset is read as the instant it names and
        // handed back in UTC, as a DateTimeOffset too: the offset is what it takes to find the
        // instant, and is not kept. A fraction of a second is, down to the tick.
        [Fact]
        public virtual Task DateTimeOffset_from_string_with_offset() => Verify<DateTimeOffset>("\"2020-01-02T03:04:05+02:00\"");

        [Fact]
        public virtual Task DateTime_from_string_with_offset() => Verify<DateTime>("\"2020-01-02T03:04:05+02:00\"");

        [Fact]
        public virtual Task DateTimeOffset_from_string_with_fraction_of_a_second() => Verify<DateTimeOffset>("\"2020-01-02T03:04:05.1234567+02:00\"");

        // What DateTime_from_double says of a DateTime: a number is milliseconds since 1970, and
        // what is less than one of them is cut off.
        [Fact]
        public virtual Task DateTimeOffset_from_double() => Verify<DateTimeOffset>("123456789.2");

        // A string that names no offset names no instant either. It is taken as the local time of
        // the machine reading it, so the instant it is read as is another one wherever that machine
        // is, and no snapshot can hold it. What these hold instead is that it is that local time:
        // the day's midnight, here. A DateTimeOffset is compared by its instant alone - the offset
        // it comes back with is UTC on one implementation and the machine's own on the other.
        [Fact]
        public virtual Task DateTime_from_date_only_string()
        {
            var subject = _environment
                .Deserializer
                .TransformTo<DateTime>()
                .From(CreateNativeToken("\"2020-01-02\""), _environment);

            return Verifier
                .Verify(
                    new
                    {
                        subject.Kind,
                        IsLocalMidnight = subject == new DateTime(2020, 1, 2, 0, 0, 0, DateTimeKind.Local).ToUniversalTime()
                    },
                    sourceFile: _sourceFile);
        }

        [Fact]
        public virtual Task DateTimeOffset_from_date_only_string()
        {
            var subject = _environment
                .Deserializer
                .TransformTo<DateTimeOffset>()
                .From(CreateNativeToken("\"2020-01-02\""), _environment);

            return Verifier
                .Verify(
                    new
                    {
                        IsLocalMidnight = subject == new DateTimeOffset(new DateTime(2020, 1, 2, 0, 0, 0, DateTimeKind.Local))
                    },
                    sourceFile: _sourceFile);
        }

        // Nothing is a date unless a date is asked for. Asked for as a string or as an object, a
        // string that looks like a date is the string it came as - on its own, in an array and as a
        // value of a map. A snapshot writes a string the way it is spelled, its T and its offset
        // included, and that is how these tell it from a date.
        [Fact]
        public virtual Task String_from_date_like_string() => Verify<string>("\"2020-01-02T03:04:05+02:00\"");

        [Fact]
        public virtual Task Object_from_date_like_string() => Verify<object>("\"2020-01-02T03:04:05Z\"");

        [Fact]
        public virtual Task Object_from_date_like_string_with_offset() => Verify<object>("\"2020-01-02T03:04:05+02:00\"");

        [Fact]
        public virtual Task Object_from_date_only_string() => Verify<object>("\"2020-01-02\"");

        [Fact]
        public virtual Task Objects_from_Array_with_date_like_strings() => Verify<object[]>("""[ "2020-01-02T03:04:05+02:00", "2020-01-02T03:04:05Z" ]""");

        [Fact]
        public virtual Task Object_from_map_with_date_like_string() => Verify<object>("""
            {
              "@type": "g:Map",
              "@value": [ "name", "Bob", "registered", "2020-01-02T03:04:05+02:00" ]
            }
            """);

        // An entity says what each of its properties is. Person.RegistrationDate is a date, and the
        // string is read as one, as it is on its own. Person.Name is a string, and the same string
        // stays what it came as. And a RegistrationDate that is no date costs the property, not
        // the person.
        [Fact]
        public virtual Task Person_with_RegistrationDate_from_string_with_offset() => Verify<Person>("""
            {
              "id": 13,
              "label": "Person",
              "type": "vertex",
              "properties": {
                "RegistrationDate": [ { "id": 1, "value": "2020-01-02T03:04:05+02:00" } ]
              }
            }
            """);

        [Fact]
        public virtual Task Person_with_date_like_Name() => Verify<Person>("""
            {
              "id": 13,
              "label": "Person",
              "type": "vertex",
              "properties": {
                "Name": [ { "id": 1, "value": "2020-01-02T03:04:05+02:00" } ]
              }
            }
            """);

        [Fact]
        public virtual Task Person_with_RegistrationDate_from_invalid_string() => Verify<Person>("""
            {
              "id": 13,
              "label": "Person",
              "type": "vertex",
              "properties": {
                "RegistrationDate": [ { "id": 1, "value": "not a date" } ]
              }
            }
            """);

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

        // A value Gremlin.Net has no static instance for is still a value of that enumeration - a
        // newer server may send one this Gremlin.Net does not know yet. It is read as a Direction
        // with that value, rather than as no Direction at all, or as the bare string when asked for
        // as an object. It equals no Direction Gremlin.Net knows.
        [Fact]
        public virtual Task Direction_from_unknown_typed_Direction() => VerifyAttempt<Direction>("""{ "@type": "g:Direction", "@value": "SIDEWAYS" }""");

        [Fact]
        public virtual Task Object_from_unknown_typed_Direction() => VerifyAttempt<object>("""{ "@type": "g:Direction", "@value": "SIDEWAYS" }""");

        // An enumeration's value is a string. A number is none, so there is no Direction to read.
        [Fact]
        public virtual Task Direction_from_typed_Direction_with_number_value() => VerifyAttempt<Direction>("""{ "@type": "g:Direction", "@value": 1 }""");

        // An enumeration without the constructor taking the value - which the test below says no
        // Gremlin.Net one lacks today - cannot hold a value it has no name for. Asked for one, it
        // declines, as every enumeration did before. So it does where its GetByValue answers null.
        [Fact]
        public virtual Task Enumeration_without_value_constructor_from_its_known_value() => VerifyAttempt<EnumerationWithoutValueConstructor>("\"known\"");

        [Fact]
        public virtual Task Enumeration_without_value_constructor_from_unknown_value() => VerifyAttempt<EnumerationWithoutValueConstructor>("\"unknown\"");

        [Fact]
        public virtual Task Enumeration_without_value_constructor_from_value_answered_with_null() => VerifyAttempt<EnumerationWithoutValueConstructor>("\"none\"");

        [Fact]
        public virtual Task Object_from_typed_Merge() => Verify<object>("""{ "@type": "g:Merge", "@value": "onCreate" }""");

        [Fact]
        public virtual Task Merge_from_typed_Merge() => Verify<Merge>("""{ "@type": "g:Merge", "@value": "onCreate" }""");

        [Fact]
        public virtual Task Object_from_typed_T() => Verify<object>("""{ "@type": "g:T", "@value": "id" }""");

        [Fact]
        public virtual Task T_from_typed_T() => Verify<T>("""{ "@type": "g:T", "@value": "id" }""");

        // The same for a T.
        [Fact]
        public virtual Task T_from_unknown_typed_T() => VerifyAttempt<T>("""{ "@type": "g:T", "@value": "unknown" }""");

        [Fact]
        public virtual Task Object_from_unknown_typed_T() => VerifyAttempt<object>("""{ "@type": "g:T", "@value": "unknown" }""");

        // Both implementations build such a value through the private constructor every Gremlin.Net
        // enumeration has, taking the value. That is not Gremlin.Net's public surface, so this says
        // when an upgrade takes it away: it lists the enumerations that have none.
        [Fact]
        public virtual Task Every_Gremlin_enumeration_can_hold_a_value_it_has_no_name_for() => Verifier
            .Verify(
                typeof(Gremlin.Net.Process.Traversal.EnumWrapper).Assembly
                    .GetTypes()
                    .Where(static type => typeof(Gremlin.Net.Process.Traversal.EnumWrapper).IsAssignableFrom(type) && !type.IsAbstract && type != typeof(Gremlin.Net.Process.Traversal.EnumWrapper))
                    .Where(static type => type.GetConstructor(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, [typeof(string)]) is null)
                    .Select(static type => type.FullName)
                    .ToArray(),
                sourceFile: _sourceFile);

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

        // An envelope is matched exactly, as every other "@type" and "@value" is on both
        // implementations: an object that spells them otherwise is no traverser, and an int array
        // has no place for it. The type name inside is a different matter - "G:TRAVERSER" would
        // still be one, as "G:BULKSET" is a bulk set.
        [Fact]
        public virtual Task Ints_from_Traverser_with_uppercase_envelope() => Verify<int[]>(Array_With_Traverser_With_Uppercase_Envelope);

        // The members inside the envelope are GraphSON's own as well, and matched exactly like it:
        // spelled "Bulk" and "Value", this is no traverser either.
        [Fact]
        public virtual Task Ints_from_Traverser_with_capitalized_members() => Verify<int[]>("""
            [
              {
                "@type": "g:Traverser",
                "@value": { "Bulk": 7, "Value": 42 }
              }
            ]
            """);

        // A bulk that is no number says nothing about how often the value is there, so it is there
        // once, as it is when a traverser has no bulk at all.
        [Fact]
        public virtual Task Ints_from_Traverser_with_bulk_that_is_no_number() => Verify<int[]>("""
            [
              {
                "@type": "g:Traverser",
                "@value": { "bulk": "seven", "value": 42 }
              }
            ]
            """);

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

        // "id", "label" and "properties" are GraphSON's names for an element's parts, and an object
        // is only taken for an element when it spells them that way. This one is a map of the
        // caller's own that happens to have members of those names, and stays one - it is not
        // looked up as a Person.
        [Fact]
        public virtual Task Object_with_capitalized_id_label_and_properties_as_object() => Verify<object>("""
            {
              "Id": 1,
              "Label": "Person",
              "Properties": { "Age": 36 }
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

        // An element map's id and label are the g:T values "id" and "label", spelled the way GraphSON
        // spells them. "ID" and "LABEL" are T values of their own, neither of those, so this map has
        // no id and no label and is no element. Asked for as a Person, it has an Age and nothing to
        // say about the rest - and a T value Gremlin has no name for is no reason to throw.
        [Fact]
        public virtual Task Person_from_element_map_with_uppercase_T_values() => VerifyAttempt<Person>("""
            {
              "@type": "g:Map",
              "@value": [
                { "@type": "g:T", "@value": "ID" },
                { "@type": "g:Int64", "@value": 1 },
                { "@type": "g:T", "@value": "LABEL" },
                "Person",
                "Age",
                36
              ]
            }
            """);

        // The same map asked for as an object. Without an id and a label it is no element, and its keys
        // are not all names - a name is a string, or an element map's id or label - so it is built as
        // the dictionary it is, as a map with any other key is. Its g:T keys stay what they are: the T
        // values T.ID and T.LABEL, not the strings "ID" and "LABEL". The snapshot of a dictionary
        // writes its keys alike whatever their type, so the types are recorded alongside it.
        [Fact]
        public virtual Task Element_map_with_uppercase_T_values_as_object()
        {
            var subject = _environment
                .Deserializer
                .TransformTo<object>()
                .From(CreateNativeToken("""
                    {
                      "@type": "g:Map",
                      "@value": [
                        { "@type": "g:T", "@value": "ID" },
                        { "@type": "g:Int64", "@value": 1 },
                        { "@type": "g:T", "@value": "LABEL" },
                        "SomeUnknownLabel",
                        "SomeProperty",
                        "SomeValue"
                      ]
                    }
                    """), _environment);

            return Verifier
                .Verify(
                    new
                    {
                        Type = subject.GetType(),
                        KeyTypes = subject is IDictionary dictionary
                            ? dictionary.Keys.Cast<object>().Select(static key => key.GetType()).ToArray()
                            : null,
                        Value = subject
                    },
                    sourceFile: _sourceFile)
                .DontScrubDateTimes();
        }

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

        // A string can hold a null where an int cannot, and that changes nothing here: at the top
        // level a null is no string, as it is no int?, no Uri and no Person. It is not the empty
        // string either - that would be a value where there was none.
        [Fact]
        public virtual Task String_from_null() => VerifyAttempt<string>("null");

        [Fact]
        public virtual Task Uri_from_null() => VerifyAttempt<Uri>("null");

        [Fact]
        public virtual Task Person_from_null() => VerifyAttempt<Person>("null");

        // Unwrapping does not change it. A single item array, a typed value, a property - each
        // hands on what it wraps, and what it wraps is still a null with nothing around it to
        // hold it.
        [Fact]
        public virtual Task String_from_single_item_array_with_null() => VerifyAttempt<string>("[ null ]");

        [Fact]
        public virtual Task String_from_typed_value_with_null() => VerifyAttempt<string>("""{ "@type": "g:UUID", "@value": null }""");

        [Fact]
        public virtual Task String_from_Property_with_null_value() => VerifyAttempt<string>("""{ "key": "name", "value": null }""");

        [Fact]
        public virtual Task String_from_VertexProperty_with_null_value() => VerifyAttempt<string>("""{ "id": 1, "label": "name", "value": null }""");

        // Nor does wrapping: a scalar asked for as a property is that property's value, and a null
        // is none.
        [Fact]
        public virtual Task Property_of_string_from_null() => VerifyAttempt<Property<string>>("null");

        [Fact]
        public virtual Task VertexProperty_of_string_from_null() => VerifyAttempt<VertexProperty<string>>("null");

        // Inside an array the null has somewhere to be, and is there - as a null, the way
        // Nullable_null has it for an int?.
        [Fact]
        public virtual Task Strings_from_Array_with_null() => Verify<string[]>("[ \"a\", null ]");

        // An object is somewhere to be as well. A constructor argument that is null is passed as
        // one, and a member that is null is left as it was - neither is handed the empty string. A
        // snapshot leaves out a member that is null and writes one that is empty, so these tell
        // the two apart.
        [Fact]
        public virtual Task Constructor_arguments_from_null() => Verify<ClassWithFieldsAndConstructor>("""
            {
                "stringArg": null,
                "nullableStringArg": null,
                "intArg": 42
            }
            """);

        [Fact]
        public virtual Task Member_from_null() => Verify<ClassWithFieldsAndConstructor>("""
            {
                "stringArg": "stringValue",
                "intArg": 42,
                "settableString": null
            }
            """);

        // The same for the member of a vertex, whose null arrives inside a vertex property.
        [Fact]
        public virtual Task Language_with_null_property_value() => Verify<Language>("""
            {
              "id": 1,
              "label": "Language",
              "type": "vertex",
              "properties": {
                "IetfLanguageTag": [ { "id": 2, "value": null } ]
              }
            }
            """);

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

        // A member of a type the caller asks for is found however its name is spelled, so a name
        // spelled two ways is found twice. The last one in the document is the one that counts - not
        // the one spelled like the member: the second test is the first one reversed, and answers
        // the other value.
        [Fact]
        public virtual Task Person_from_object_with_age_twice_in_different_case() => Verify<Person>("""{ "age": 1, "Age": 2 }""");

        [Fact]
        public virtual Task Person_from_object_with_age_twice_in_different_case_reversed() => Verify<Person>("""{ "Age": 2, "age": 1 }""");

        // Gremlin's property keys are case sensitive, so a vertex can have an "age" and an "Age" -
        // the one place where a server sends a name spelled two ways. The last one counts here too.
        [Fact]
        public virtual Task Person_from_vertex_with_age_property_twice_in_different_case() => Verify<Person>("""
            {
              "id": 1,
              "label": "Person",
              "properties": {
                "age": [ { "id": 2, "value": 1 } ],
                "Age": [ { "id": 3, "value": 2 } ]
              }
            }
            """);

        // The same vertex as valueMap() returns it: a g:Map with both keys.
        [Fact]
        public virtual Task Person_from_map_with_age_twice_in_different_case() => Verify<Person>("""
            {
              "@type": "g:Map",
              "@value": [ "age", 1, "Age", 2 ]
            }
            """);

        // Constructor arguments are found the same way as members are.
        [Fact]
        public virtual Task Constructor_argument_twice_in_different_case() => Verify<ClassWithFieldsAndConstructor>("""{ "stringArg": "a", "StringArg": "b", "intArg": 1 }""");

        // And so are the members of Gremlinq's own Property<T>, when the caller asks for one.
        [Fact]
        public virtual Task Property_with_value_twice_in_different_case() => Verify<Property<int>>("""{ "key": "p", "value": 1, "Value": 2 }""");

        // A name spelled the same way twice is taken like one spelled two ways: from its last
        // occurrence, and nothing throws. That goes for a member of a plain object, for a property
        // of a vertex, and for a key of the g:Map that valueMap() returns.
        [Fact]
        public virtual Task Person_from_object_with_age_twice() => Verify<Person>("""{ "Age": 1, "Age": 2 }""");

        [Fact]
        public virtual Task Person_from_vertex_with_age_property_twice() => Verify<Person>("""
            {
              "id": 1,
              "label": "Person",
              "properties": {
                "Age": [ { "id": 2, "value": 1 } ],
                "Age": [ { "id": 3, "value": 2 } ]
              }
            }
            """);

        [Fact]
        public virtual Task Person_from_map_with_age_twice() => Verify<Person>("""
            {
              "@type": "g:Map",
              "@value": [ "Age", 1, "Age", 2 ]
            }
            """);

        // GraphSON's own names are no different, be it a property's "value" or an envelope's
        // "@value".
        [Fact]
        public virtual Task Property_with_value_twice() => Verify<Property<int>>("""{ "key": "p", "value": 1, "value": 2 }""");

        [Fact]
        public virtual Task Int_from_typed_value_with_value_twice() => Verify<int>("""{ "@type": "g:Int32", "@value": 1, "@value": 2 }""");

        // Nor are the g:T keys of an element map. An id that is there twice is the last one, asked
        // for as a Person or as an object - and of two labels it is the last one that says what the
        // element is: a Person here, not a Language.
        [Fact]
        public virtual Task Person_from_element_map_with_id_twice() => Verify<Person>("""
            {
              "@type": "g:Map",
              "@value": [
                { "@type": "g:T", "@value": "id" },
                { "@type": "g:Int64", "@value": 1 },
                { "@type": "g:T", "@value": "id" },
                { "@type": "g:Int64", "@value": 2 },
                { "@type": "g:T", "@value": "label" },
                "Person",
                "Age",
                36
              ]
            }
            """);

        [Fact]
        public virtual Task Element_map_with_id_twice_as_object() => Verify<object>("""
            {
              "@type": "g:Map",
              "@value": [
                { "@type": "g:T", "@value": "id" },
                { "@type": "g:Int64", "@value": 1 },
                { "@type": "g:T", "@value": "id" },
                { "@type": "g:Int64", "@value": 2 },
                { "@type": "g:T", "@value": "label" },
                "SomeUnknownLabel",
                "SomeProperty",
                "SomeValue"
              ]
            }
            """);

        [Fact]
        public virtual Task Vertex_from_element_map_with_label_twice() => Verify<Vertex>("""
            {
              "@type": "g:Map",
              "@value": [
                { "@type": "g:T", "@value": "id" },
                { "@type": "g:Int64", "@value": 1 },
                { "@type": "g:T", "@value": "label" },
                "Language",
                { "@type": "g:T", "@value": "label" },
                "Person",
                "Age",
                36
              ]
            }
            """);

        // Read as a dictionary, or as an object, a map keeps its entries rather than having members
        // looked up in it - and a key that is there twice must not cost the whole map there either.
        // Its entry gets the value of the last occurrence and keeps the place of the first, which
        // is where a parser that takes the last of two members leaves it. The snapshot of a
        // dictionary says neither in which order its keys come nor of which type they are, so both
        // are recorded alongside it.
        private SettingsTask VerifyWithKeys<T>(string token)
        {
            var subject = _environment
                .Deserializer
                .TransformTo<T>()
                .From(CreateNativeToken(token), _environment);

            var keys = subject switch
            {
                IDictionary dictionary => dictionary.Keys.Cast<object>().ToArray(),
                IDictionary<string, object?> dictionary => [.. dictionary.Keys],
                _ => null
            };

            return Verifier
                .Verify(
                    new
                    {
                        Keys = keys,
                        KeyTypes = keys?.Select(static key => key.GetType()).ToArray(),
                        Value = subject
                    },
                    sourceFile: _sourceFile)
                .DontScrubDateTimes();
        }

        [Fact]
        public virtual Task Map_with_key_twice_as_object() => VerifyWithKeys<object>("""
            {
              "@type": "g:Map",
              "@value": [ "a", 1, "b", 2, "a", 3 ]
            }
            """);

        [Fact]
        public virtual Task Dictionary_from_map_with_key_twice() => VerifyWithKeys<Dictionary<string, int>>("""
            {
              "@type": "g:Map",
              "@value": [ "a", 1, "b", 2, "a", 3 ]
            }
            """);

        // An immutable dictionary has no order to speak of, so there is none to record.
        [Fact]
        public virtual Task ImmutableDictionary_from_map_with_key_twice() => Verify<ImmutableDictionary<string, int>>("""
            {
              "@type": "g:Map",
              "@value": [ "a", 1, "b", 2, "a", 3 ]
            }
            """);

        // The same for a plain object, whose members are the entries.
        [Fact]
        public virtual Task Object_with_member_twice_as_object() => VerifyWithKeys<object>("""{ "a": 1, "b": 2, "a": 3 }""");

        [Fact]
        public virtual Task Dictionary_from_object_with_member_twice() => VerifyWithKeys<Dictionary<string, int>>("""{ "a": 1, "b": 2, "a": 3 }""");

        [Fact]
        public virtual Task ImmutableDictionary_from_object_with_member_twice() => Verify<ImmutableDictionary<string, int>>("""{ "a": 1, "b": 2, "a": 3 }""");

        // Two places where a server's answer ends up in a dictionary without the caller asking for
        // one: the properties of a vertex whose label the model does not know, and the meta
        // properties of a vertex property.
        [Fact]
        public virtual Task Vertex_with_unknown_label_and_property_twice_as_object() => Verify<object>("""
            {
              "id": 1,
              "label": "SomeUnknownLabel",
              "type": "vertex",
              "properties": {
                "SomeProperty": [ { "id": 2, "value": "SomeValue" } ],
                "SomeProperty": [ { "id": 3, "value": "SomeOtherValue" } ]
              }
            }
            """);

        [Fact]
        public virtual Task VertexProperty_with_meta_property_twice() => Verify<VertexProperty<object>>("""{ "id": 166, "value": "bob", "label": "Name", "properties": { "metaKey": "MetaValue", "metaKey": "OtherMetaValue" } }""");

        // A key that is no name is found twice just the same, typed as it was.
        [Fact]
        public virtual Task Map_with_typed_int_key_twice_as_object() => VerifyWithKeys<object>("""
            {
              "@type": "g:Map",
              "@value": [
                { "@type": "g:Int32", "@value": 1 },
                "value1",
                { "@type": "g:Int32", "@value": 2 },
                "value2",
                { "@type": "g:Int32", "@value": 1 },
                "value3"
              ]
            }
            """);

        // Whether two keys are the same is decided by what they are read as. A g:Int32 1 and a
        // g:Int64 1 are two keys of a map read as an object, where each stays what it is, and one
        // key when longs are asked for. The same goes for a "1" and a 1 when strings are asked for.
        [Fact]
        public virtual Task Map_with_int_and_long_key_as_object() => VerifyWithKeys<object>("""
            {
              "@type": "g:Map",
              "@value": [
                { "@type": "g:Int32", "@value": 1 },
                "value1",
                { "@type": "g:Int64", "@value": 1 },
                "value2"
              ]
            }
            """);

        [Fact]
        public virtual Task Dictionary_of_long_keys_from_map_with_int_and_long_key() => VerifyWithKeys<Dictionary<long, string>>("""
            {
              "@type": "g:Map",
              "@value": [
                { "@type": "g:Int32", "@value": 1 },
                "value1",
                { "@type": "g:Int64", "@value": 1 },
                "value2"
              ]
            }
            """);

        [Fact]
        public virtual Task Dictionary_of_string_keys_from_map_with_string_and_number_key() => VerifyWithKeys<Dictionary<string, string>>("""
            {
              "@type": "g:Map",
              "@value": [ "1", "value1", 1, "value2" ]
            }
            """);

        // The last occurrence counts even when its value cannot be read. The map says "a" is
        // something that is no int, so there is no "a" - the 1 it said before is not what it says.
        [Fact]
        public virtual Task Dictionary_from_map_with_key_twice_and_unreadable_last_value() => VerifyWithKeys<Dictionary<string, int>>("""
            {
              "@type": "g:Map",
              "@value": [ "a", 1, "b", 2, "a", "not a number" ]
            }
            """);

        // A tree's entries are keyed as well, and a key that is there twice is its last subtree.
        [Fact]
        public virtual Task Tree_with_key_twice() => Verify<Tree<string>>("""
            {
              "@type": "g:Tree",
              "@value": [
                {
                  "key": "a",
                  "value": {
                    "@type": "g:Tree",
                    "@value": [ { "key": "b", "value": { "@type": "g:Tree", "@value": [] } } ]
                  }
                },
                {
                  "key": "a",
                  "value": {
                    "@type": "g:Tree",
                    "@value": [ { "key": "c", "value": { "@type": "g:Tree", "@value": [] } } ]
                  }
                }
              ]
            }
            """);

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

        // A property is recognised by GraphSON's "key" and "value", spelled exactly. Spelled "Value",
        // this is an object like any other, and nothing reads an int from it.
        [Fact]
        public virtual Task Int_from_object_with_key_and_capitalized_value() => VerifyAttempt<int>("""{ "key": "Property1", "Value": 42 }""");

        // A Property<object> and a map with the same two members snapshot alike, so whether the
        // result is a property is recorded alongside it. A map of the caller's own whose members
        // happen to be named Key and Value is no property.
        [Fact]
        public virtual Task Object_with_capitalized_key_and_value_as_object()
        {
            var subject = _environment
                .Deserializer
                .TransformTo<object>()
                .From(CreateNativeToken("""{ "Key": "Property1", "Value": 42 }"""), _environment);

            return Verifier
                .Verify(
                    new
                    {
                        IsProperty = subject is Property,
                        Value = subject
                    },
                    sourceFile: _sourceFile)
                .DontScrubDateTimes();
        }

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

        // A bool is read from a string the way .NET parses one - in any case, with white space
        // around it - which is what a bool kept in a string property looks like: .NET writes "True".
        // No other string is a bool, neither one that means yes nor a number in a string.
        [Fact]
        public virtual Task Bools_from_strings() => Verify<bool[]>("""[ "true", "True", "TRUE", " true ", "false", "False", "FALSE" ]""");

        [Fact]
        public virtual Task Bool_from_string_that_is_no_bool() => VerifyAttempt<bool>("\"yes\"");

        [Fact]
        public virtual Task Bool_from_numeric_string() => VerifyAttempt<bool>("\"1\"");

        // And from a number: zero is false and every other number is true, whatever its sign and
        // whether it has a fraction. A typed value is the number it holds.
        [Fact]
        public virtual Task Bools_from_numbers() => Verify<bool[]>("[ 0, 1, 2, -1, 1.5, 0.0 ]");

        [Fact]
        public virtual Task Bool_from_typed_Int32() => Verify<bool>("""{ "@type": "g:Int32", "@value": 1 }""");

        // An integer is read from any number, not only from one written as an integer: GraphSON may
        // well carry a whole number as 3.0 or 1e2. A fraction is rounded to the nearest integer and
        // a half to the even one, so 3.5 is 4 where 2.5 is 2 - Int_from_double above has always
        // said so for an int. One test per integer type, as each is read on its own. The two
        // smallest are read into a List: a byte[] is not a collection of bytes to either
        // implementation - see below - and an sbyte[] is snapshotted as a binary file.
        [Fact]
        public virtual Task SBytes_from_numbers_with_fraction_or_exponent() => Verify<List<sbyte>>("[ 3.0, 1e2, 3.5, 2.5, -2.5 ]");

        [Fact]
        public virtual Task Bytes_from_numbers_with_fraction_or_exponent() => Verify<List<byte>>("[ 3.0, 1e2, 3.5, 2.5, -0.4 ]");

        [Fact]
        public virtual Task Shorts_from_numbers_with_fraction_or_exponent() => Verify<short[]>("[ 3.0, 1e2, 3.5, 2.5, -2.5 ]");

        [Fact]
        public virtual Task UShorts_from_numbers_with_fraction_or_exponent() => Verify<ushort[]>("[ 3.0, 1e2, 3.5, 2.5, -0.4 ]");

        [Fact]
        public virtual Task Ints_from_numbers_with_fraction_or_exponent() => Verify<int[]>("[ 3.0, 1e2, 3.5, 2.5, -2.5 ]");

        [Fact]
        public virtual Task UInts_from_numbers_with_fraction_or_exponent() => Verify<uint[]>("[ 3.0, 1e2, 3.5, 2.5, -0.4 ]");

        [Fact]
        public virtual Task Longs_from_numbers_with_fraction_or_exponent() => Verify<long[]>("[ 3.0, 1e2, 3.5, 2.5, -2.5 ]");

        [Fact]
        public virtual Task ULongs_from_numbers_with_fraction_or_exponent() => Verify<ulong[]>("[ 3.0, 1e2, 3.5, 2.5, -0.4 ]");

        // A number out of the requested type's range is no such integer, and the attempt declines
        // rather than throws - so each of these arrays is empty. The last number of each is within
        // the range until it is rounded, or, for the two widest types, until it is read as a double.
        // A long has one more: written as an integer it is out of range, however close the nearest
        // double comes. An int is not among these: asked for one from a number out of its range, the
        // two implementations do not agree yet.
        [Fact]
        public virtual Task SBytes_from_numbers_out_of_range() => Verify<List<sbyte>>("[ 128.0, -129.0, 127.5 ]");

        [Fact]
        public virtual Task Bytes_from_numbers_out_of_range() => Verify<List<byte>>("[ 256.0, -1.0, 255.5 ]");

        [Fact]
        public virtual Task Shorts_from_numbers_out_of_range() => Verify<short[]>("[ 32768.0, -32769.0, 32767.5 ]");

        [Fact]
        public virtual Task UShorts_from_numbers_out_of_range() => Verify<ushort[]>("[ 65536.0, -1.0, 65535.5 ]");

        [Fact]
        public virtual Task UInts_from_numbers_out_of_range() => Verify<uint[]>("[ 4294967296.0, -1.0, 4294967295.5 ]");

        [Fact]
        public virtual Task Longs_from_numbers_out_of_range() => Verify<long[]>("[ 1e19, -1e19, 9223372036854775807.0, -9223372036854775809 ]");

        [Fact]
        public virtual Task ULongs_from_numbers_out_of_range() => Verify<ulong[]>("[ 1e20, -1.0, 18446744073709551615.0 ]");

        // The same through a typed value, which is the number it holds whatever type it names, and
        // into a nullable. Asked for as an object, a typed value is what its type says, so a g:Int64
        // holding 3.5 is the long 4 - where the bare number is the double 3.5, as Object_from_double
        // above says.
        [Fact]
        public virtual Task Long_from_typed_Double() => Verify<long>("""{ "@type": "g:Double", "@value": 3.0 }""");

        [Fact]
        public virtual Task Nullable_long_from_number_with_fraction() => Verify<long?>("3.5");

        [Fact]
        public virtual Task Object_from_typed_Int64_with_fraction() => Verify<object>("""{ "@type": "g:Int64", "@value": 3.5 }""");

        // A number in a string is read the way .NET parses one, whatever the culture of the machine:
        // white space around it is fine, and so is a sign - a plus before an unsigned integer too.
        [Fact]
        public virtual Task SBytes_from_strings() => Verify<List<sbyte>>("""[ "42", " 42 ", "+42", "-42" ]""");

        [Fact]
        public virtual Task Bytes_from_strings() => Verify<List<byte>>("""[ "42", " 42 ", "+42" ]""");

        [Fact]
        public virtual Task Shorts_from_strings() => Verify<short[]>("""[ "42", " 42 ", "+42", "-42" ]""");

        [Fact]
        public virtual Task UShorts_from_strings() => Verify<ushort[]>("""[ "42", " 42 ", "+42" ]""");

        [Fact]
        public virtual Task Ints_from_strings() => Verify<int[]>("""[ "42", " 42 ", "+42", "-42" ]""");

        [Fact]
        public virtual Task UInts_from_strings() => Verify<uint[]>("""[ "42", " 42 ", "+42" ]""");

        [Fact]
        public virtual Task Longs_from_strings() => Verify<long[]>("""[ "42", " 42 ", "+42", "-42" ]""");

        [Fact]
        public virtual Task ULongs_from_strings() => Verify<ulong[]>("""[ "42", " 42 ", "+42" ]""");

        // An integer in a string is written as one, though. A number is rounded, a string is not:
        // none of these is a long.
        [Fact]
        public virtual Task Longs_from_strings_that_are_no_integers() => Verify<long[]>("""[ "3.0", "1e2", "1,000", "0x10", "" ]""");

        [Fact]
        public virtual Task Floats_from_strings() => Verify<float[]>("""[ "1.5", " 1.5 ", "+1.5", "-1.5", "1e2" ]""");

        [Fact]
        public virtual Task Doubles_from_strings() => Verify<double[]>("""[ "1.5", " 1.5 ", "+1.5", "-1.5", "1e2" ]""");

        [Fact]
        public virtual Task Decimals_from_strings() => Verify<decimal[]>("""[ "1.5", " 1.5 ", "+1.5", "-1.5", "1e2" ]""");

        // What is not a number is spelled as .NET spells it, in any case.
        [Fact]
        public virtual Task Doubles_from_strings_naming_what_is_no_number() => Verify<double[]>("""[ "NaN", "nan", "Infinity", "-INFINITY" ]""");

        [Fact]
        public virtual Task Doubles_from_strings_that_are_no_numbers() => Verify<double[]>("""[ "abc", "1.5f", "0x10", "" ]""");

        // A BigInteger is read from any number as well. It has a fraction cut off where the integer
        // types above round it - 3.5 is 3, and -2.5 is -2 either way - each being what .NET makes of
        // a double when it converts one to that type.
        [Fact]
        public virtual Task BigIntegers_from_numbers_with_fraction_or_exponent() => Verify<BigInteger[]>("[ 3.0, 1e2, 3.5, 2.5, -2.5 ]");

        // It is a number, though, or a string of digits, and nothing else: a JSON object is no
        // BigInteger - not zero - and neither is a typed value that holds none.
        [Fact]
        public virtual Task BigInteger_from_object() => VerifyAttempt<BigInteger>("{ }");

        [Fact]
        public virtual Task Nullable_BigInteger_from_object() => VerifyAttempt<BigInteger?>("{ }");

        [Fact]
        public virtual Task BigInteger_from_typed_BigInteger_that_is_no_integer() => VerifyAttempt<BigInteger>("""{ "@type": "gx:BigInteger", "@value": "3.0" }""");

        // A byte[] is a native type, not a collection of bytes: it arrives as a Base64 string, as
        // Person_with_typed_ByteBuffer_image above has it. It is read from an array of numbers too,
        // which is how one looks that was not written as a gx:ByteBuffer - when every item is a
        // number written as an integer. Unlike any other array, it does not drop the items it
        // cannot read: a byte[] short of some of its bytes is another value, so it is none at all.
        [Fact]
        public virtual Task Byte_array_from_numbers() => VerifyAttempt<byte[]>("[ 1, 2, 3 ]");

        [Fact]
        public virtual Task Byte_array_from_empty_array() => VerifyAttempt<byte[]>("[ ]");

        [Fact]
        public virtual Task Byte_array_from_single_number() => VerifyAttempt<byte[]>("[ 1 ]");

        [Fact]
        public virtual Task Byte_array_from_numbers_with_fraction() => VerifyAttempt<byte[]>("[ 1.0, 2 ]");

        [Fact]
        public virtual Task Byte_array_from_numbers_and_strings() => VerifyAttempt<byte[]>("""[ 1, "2" ]""");

        [Fact]
        public virtual Task Byte_array_from_numbers_and_null() => VerifyAttempt<byte[]>("[ 1, null ]");

        [Fact]
        public virtual Task Byte_array_from_typed_List() => VerifyAttempt<byte[]>("""{ "@type": "g:List", "@value": [ 1, 2, 3 ] }""");

        // The items are bare numbers. A typed one is not read, although it holds one.
        [Fact]
        public virtual Task Byte_array_from_typed_List_of_typed_Int32() => VerifyAttempt<byte[]>("""
            {
              "@type": "g:List",
              "@value": [
                { "@type": "g:Int32", "@value": 1 },
                { "@type": "g:Int32", "@value": 2 }
              ]
            }
            """);

        // All of the above as the members of a type the caller asks for, which is where they matter:
        // once from a plain object, once from the properties of a vertex.
        [Fact]
        public virtual Task Scalar_members_from_tokens_of_another_kind() => Verify<ClassWithScalarMembers>("""
            {
              "Bool": "True",
              "Long": 3.5,
              "Double": " 1.5 ",
              "BigInteger": 1e2,
              "Bytes": [ 1, 2, 3 ]
            }
            """);

        [Fact]
        public virtual Task Scalar_members_from_vertex_properties_of_another_kind() => Verify<ClassWithScalarMembers>("""
            {
              "id": 1,
              "label": "ClassWithScalarMembers",
              "properties": {
                "Bool": [ { "id": 2, "value": 1 } ],
                "Long": [ { "id": 3, "value": { "@type": "g:Double", "@value": 3.0 } } ],
                "Double": [ { "id": 4, "value": " 1.5 " } ],
                "BigInteger": [ { "id": 5, "value": 3.0 } ],
                "Bytes": [ { "id": 6, "value": [ 1, 2, 3 ] } ]
              }
            }
            """);

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

        // The same for a vertex property: without a "value" spelled that way, there is none to read.
        [Fact]
        public virtual Task Int_from_object_with_id_label_and_capitalized_value() => VerifyAttempt<int>("""{ "id": 1, "label": "Property1", "Value": 42 }""");

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

        // A tree's entries are GraphSON's "key" and "value" pairs, spelled exactly. Spelled "Key"
        // and "Value", an entry is not one.
        [Fact]
        public virtual Task Tree_with_capitalized_key_and_value() => VerifyAttempt<Tree<string>>("""
            {
              "@type": "g:Tree",
              "@value": [
                {
                  "Key": "3",
                  "Value": { "@type": "g:Tree", "@value": [] }
                }
              ]
            }
            """);
    }
}
