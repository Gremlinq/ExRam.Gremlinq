using System;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core.Tests
{
    public class User : Authority
    {
        public int Age { get; set; }

        public string PhoneNumber { get; set; }

        public Gender Gender { get; set; }

        public DateTimeOffset RegistrationDate { get; set; }

        public VertexProperty<object> SomeObscureProperty { get; set; }
    }
}
