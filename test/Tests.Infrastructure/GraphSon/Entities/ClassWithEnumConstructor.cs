using ExRam.Gremlinq.Tests.Entities;

namespace ExRam.Gremlinq.Tests.Infrastructure.GraphSon.Entities
{
    public class ClassWithEnumConstructor
    {
        public ClassWithEnumConstructor(SomeEnum @enum, SomeEnum? nullableEnum)
        {
            Enum = @enum;
            NullableEnum = nullableEnum;
        }

        public SomeEnum Enum { get; }
        public SomeEnum? NullableEnum { get; }
    }
}
