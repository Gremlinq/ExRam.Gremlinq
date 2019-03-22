using System;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Tests.Entities
{
    public class Company : Authority
    {
        public VertexProperty<string, PropertyValidity>[] Name { get; set; }

        public DateTime FoundingDate { get; set; }

        public string[] PhoneNumbers { get; set; }
    }
}
