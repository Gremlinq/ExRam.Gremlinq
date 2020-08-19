using System.Collections.Generic;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Tests.Entities
{
    public class Country : Vertex
    {
        public VertexProperty<string>? Name { get; set; }

        public string[]? Languages { get; set; }

        public string? CountryCallingCode { get; set; }
        
        public VertexProperty<object, IDictionary<string, string>>? LocalizableDescription { get; set; }
    }
}
