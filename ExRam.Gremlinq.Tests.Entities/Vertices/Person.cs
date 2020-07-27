using System;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Tests.Entities
{
    public class Person : Authority
    {
        public int Age { get; set; }

        public Gender Gender { get; set; }

        public byte[] Image { get; set; }

        public DateTimeOffset? RegistrationDate { get; set; }

        public VertexProperty<string>[]? PhoneNumbers { get; set; }

        public VertexProperty<object>? SomeObscureProperty { get; set; }
    }
}
