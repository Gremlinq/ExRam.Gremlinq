using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public interface IProjectResult
    {
        IImmutableDictionary<string, Step> Projections { get; }
    }

    // ReSharper disable once UnusedTypeParameter
    public interface IProjectResult<TResult> : IProjectResult
    {
        
    }

    public interface IProjectBuilder<out TSourceQuery, TElement>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement> ToTuple();
        IProjectDynamicBuilder<TSourceQuery, TElement> ToDynamic();
    }

    public interface IProjectDynamicBuilder<out TSourceQuery, TElement> : IProjectResult
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectDynamicBuilder<TSourceQuery, TElement> By(Func<TSourceQuery, IGremlinQueryBase> projection);
        IProjectDynamicBuilder<TSourceQuery, TElement> By(string name, Func<TSourceQuery, IGremlinQueryBase> projection);
    }
}
