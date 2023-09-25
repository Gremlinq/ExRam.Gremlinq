using System.Reflection;

namespace ExRam.Gremlinq.Core.Models
{
    public interface IAssemblyLookupBuilder
    {
        IAssemblyLookupSet IncludeAssembliesOfBaseTypes();
        IAssemblyLookupSet IncludeAssemblies(params Assembly[] assemblies);
    }
}
