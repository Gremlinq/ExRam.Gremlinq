using System.Reflection;

namespace ExRam.Gremlinq.Core.Models
{
    public interface IAssemblyLookupBuilder
    {
        IAssemblyLookupSet IncludeAssembliesOfBaseTypes();
    }
}
