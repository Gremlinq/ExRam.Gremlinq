using ExRam.Gremlinq.Tests.Entities;

namespace ExRam.Gremlinq.Tests.Infrastructure.GraphSon.Entities
{
    public class ClassWithEnumConstructor
    {
        public ClassWithEnumConstructor(SomeEnum gender, SomeEnum? nullableGender)
        {
            Gender = gender;
            NullableGender = nullableGender;
        }

        public SomeEnum Gender { get; }
        public SomeEnum? NullableGender { get; }
    }
}
