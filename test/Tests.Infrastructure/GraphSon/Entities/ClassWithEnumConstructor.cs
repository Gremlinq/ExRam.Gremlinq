using ExRam.Gremlinq.Tests.Entities;

namespace ExRam.Gremlinq.Tests.Infrastructure.GraphSon.Entities
{
    public class ClassWithEnumConstructor
    {
        public ClassWithEnumConstructor(Gender gender, Gender? nullableGender)
        {
            Gender = gender;
            NullableGender = nullableGender;
        }

        public Gender Gender { get; }
        public Gender? NullableGender { get; }
    }
}
