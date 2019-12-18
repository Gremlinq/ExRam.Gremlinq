using System;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    public partial interface IVertexPropertyGremlinQueryBase : IElementGremlinQueryBase
    {
    }

    public partial interface IVertexPropertyGremlinQueryBaseRec<TSelf> :
        IVertexPropertyGremlinQueryBase,
        IElementGremlinQueryBaseRec<TSelf>
        where TSelf : IVertexPropertyGremlinQueryBaseRec<TSelf>
    {

    }




    public partial interface IVertexPropertyGremlinQueryBase<TProperty, TValue> :
        IVertexPropertyGremlinQueryBase,
        IElementGremlinQueryBase<TProperty>
    {
        IVertexPropertyGremlinQuery<VertexProperty<TValue, TMeta>, TValue, TMeta> Meta<TMeta>() where TMeta : class;

        IPropertyGremlinQuery<Property<TMetaValue>> Properties<TMetaValue>(params string[] keys);
        IPropertyGremlinQuery<Property<object>> Properties(params string[] keys);

        IValueGremlinQuery<TValue> Value();
    }

    public partial interface IVertexPropertyGremlinQueryBaseRec<TProperty, TValue, TSelf> :
        IVertexPropertyGremlinQueryBaseRec<TSelf>,
        IVertexPropertyGremlinQueryBase<TProperty, TValue>,
        IElementGremlinQueryBaseRec<TProperty, TSelf>
        where TSelf : IVertexPropertyGremlinQueryBaseRec<TProperty, TValue, TSelf>
    {

    }

    public partial interface IVertexPropertyGremlinQuery<TProperty, TValue> :
        IVertexPropertyGremlinQueryBaseRec<TProperty, TValue, IVertexPropertyGremlinQuery<TProperty, TValue>>
    {
        
    }



    public partial interface IVertexPropertyGremlinQueryBase<TProperty, TValue, TMeta> :
        IVertexPropertyGremlinQueryBase,
        IElementGremlinQueryBase<TProperty> where TMeta : class
    {
        IPropertyGremlinQuery<Property<TTarget>> Properties<TTarget>(params Expression<Func<TMeta, TTarget>>[] projections);
        IPropertyGremlinQuery<Property<object>> Properties(params string[] keys);

        IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Property<TMetaValue>(Expression<Func<TMeta, TMetaValue>> projection, TMetaValue value);

        IValueGremlinQuery<TValue> Value();
        IValueGremlinQuery<TMetaValue> Values<TMetaValue>(params Expression<Func<TMeta, TMetaValue>>[] projections);
        IGremlinQuery<TMeta> ValueMap();

        IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Where(Expression<Func<VertexProperty<TValue, TMeta>, bool>> predicate);
    }

    public partial interface IVertexPropertyGremlinQueryBaseRec<TProperty, TValue, TMeta, TSelf> :
        IVertexPropertyGremlinQueryBaseRec<TSelf>,
        IVertexPropertyGremlinQueryBase<TProperty, TValue, TMeta>,
        IElementGremlinQueryBaseRec<TProperty, TSelf>
        where TSelf : IVertexPropertyGremlinQueryBaseRec<TProperty, TValue, TMeta, TSelf>
        where TMeta : class
    {

    }

    public partial interface IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> :
        IVertexPropertyGremlinQueryBaseRec<TProperty, TValue, TMeta, IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>>
        where TMeta : class
    {

    }
}
