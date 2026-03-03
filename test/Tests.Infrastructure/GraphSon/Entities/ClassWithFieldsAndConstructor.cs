namespace ExRam.Gremlinq.Tests.Infrastructure.GraphSon.Entities
{
    public class ClassWithFieldsAndConstructor
    {
        public ClassWithFieldsAndConstructor(string stringArg, string? nullableStringArg, int intArg, int? nullableIntArg)
        {
            StringArg = stringArg;
            NullableStringArg = nullableStringArg;
            IntArg = intArg;
            NullableIntArg = nullableIntArg;
        }

        public string StringArg { get; }
        public string? NullableStringArg { get; }
        public int IntArg { get; }
        public int? NullableIntArg { get; }

        public string? SettableString { get; set; }
    }
}
