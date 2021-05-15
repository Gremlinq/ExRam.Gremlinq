using System.Collections.Generic;
using System.Reflection;

namespace ExRam.Gremlinq.Core.Models
{
    public interface IAssemblyLookupBuilder
    {
        IAssemblyLookupSet IncludeAssembliesOfBaseTypes();
        IAssemblyLookupSet IncludeAssembliesFromStackTrace();
        IAssemblyLookupSet IncludeAssembliesFromAppDomain();
        IAssemblyLookupSet IncludeAssemblies(IEnumerable<Assembly> assemblies);
    }
}
