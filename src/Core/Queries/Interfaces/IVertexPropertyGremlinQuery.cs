using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    public interface IVertexPropertyGremlinQueryBase : IElementGremlinQueryBase
    {
        new IElementGremlinQuery<object> Lower();

        IPropertyGremlinQuery<Property<object>> Properties(params string[] keys);

        new IGremlinQuery<TTarget> Values<TTarget>();
        IGremlinQuery<TTarget> Values<TTarget>(params string[] keys);
        IGremlinQuery<object> Values(params string[] keys);

        new IMapGremlinQuery<IDictionary<string, TValue>> ValueMap<TValue>();
        IMapGremlinQuery<IDictionary<string, TValue>> ValueMap<TValue>(params string[] keys);
        IMapGremlinQuery<IDictionary<string, object>> ValueMap(params string[] keys);
    }

    public interface IVertexPropertyGremlinQueryBase<TProperty, TValue> :
        IVertexPropertyGremlinQueryBase,
        IElementGremlinQueryBase<TProperty>
    {
        new IElementGremlinQuery<TProperty> Lower();

        IVertexPropertyGremlinQuery<VertexProperty<TValue, TMeta>, TValue, TMeta> Meta<TMeta>();

        IPropertyGremlinQuery<Property<TMetaValue>> Properties<TMetaValue>(params string[] keys);

        IGremlinQuery<TValue> Value();
    }

    public interface IVertexPropertyGremlinQuery<TProperty, TValue> :
        IVertexPropertyGremlinQueryBase<TProperty, TValue>,
        IElementGremlinQueryBaseRec<TProperty, IVertexPropertyGremlinQuery<TProperty, TValue>>
    {
        
    }

    public interface IVertexPropertyGremlinQueryBase<TProperty, TValue, TMeta> :
        IVertexPropertyGremlinQueryBase,
        IElementGremlinQueryBase<TProperty>
    {
        new IElementGremlinQuery<TProperty> Lower();

        IPropertyGremlinQuery<Property<TTarget>> Properties<TTarget>(params Expression<Func<TMeta, TTarget>>[] projections);

        IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Property<TMetaValue>(Expression<Func<TMeta, TMetaValue>> projection, TMetaValue value);

        IGremlinQuery<TValue> Value();
        IGremlinQuery<TMetaValue> Values<TMetaValue>(params Expression<Func<TMeta, TMetaValue>>[] projections);
        new IGremlinQuery<TMeta> ValueMap();

        IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Where(Expression<Func<VertexProperty<TValue, TMeta>, bool>> predicate);
    }

    public interface IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> :
        IVertexPropertyGremlinQueryBase<TProperty, TValue, TMeta>,
        IElementGremlinQueryBaseRec<TProperty, IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>>
    {

    }
}
