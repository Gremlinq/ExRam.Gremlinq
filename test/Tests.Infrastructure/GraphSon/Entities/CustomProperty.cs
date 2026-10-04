using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Tests.Infrastructure.GraphSon.Entities
{
    public class CustomProperty : Property<string>
    {
        public CustomProperty(string value) : base(value)
        {
        }
    }
}
