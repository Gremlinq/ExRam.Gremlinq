using System;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Tests.Entities
{
    public class Person : Authority
    {
        public VertexProperty<string>[] PhoneNumbers  { get; set; }

        public int Age { get; set; }

        public Gender Gender { get; set; }

        public DateTimeOffset RegistrationDate { get; set; }

        public VertexProperty<object> SomeObscureProperty { get; set; }
    }
}
