using System;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Tests.Entities
{
    public class Company : Authority
    {
        public DateTime FoundingDate { get; set; }

        public string[] PhoneNumbers { get; set; }

        public VertexProperty<string, PropertyValidity>[] Locations { get; set; }
    }
}
