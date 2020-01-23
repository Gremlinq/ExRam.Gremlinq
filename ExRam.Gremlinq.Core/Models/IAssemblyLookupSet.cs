using System.Collections.Immutable;
using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    public interface IAssemblyLookupSet : IAssemblyLookupBuilder
    {
        IImmutableList<Assembly> Assemblies { get; }
    }
}