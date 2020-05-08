using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    public interface IVertexPropertyGremlinQueryBase : IElementGremlinQueryBase
    {
        new IElementGremlinQuery<object> Lower();

        IPropertyGremlinQuery<Property<object>> Properties(params string[] keys);

        new IValueGremlinQuery<TTarget> Values<TTarget>();
        IValueGremlinQuery<TTarget> Values<TTarget>(params string[] keys);
        IValueGremlinQuery<object> Values(params string[] keys);

        new IValueGremlinQuery<IDictionary<string, TTarget>> ValueMap<TTarget>();
        IValueGremlinQuery<IDictionary<string, TTarget>> ValueMap<TTarget>(params string[] keys);
        IValueGremlinQuery<IDictionary<string, object>> ValueMap(params string[] keys);
    }

    public interface IVertexPropertyGremlinQueryBaseRec<TSelf> :
        IVertexPropertyGremlinQueryBase,
        IElementGremlinQueryBaseRec<TSelf>
        where TSelf : IVertexPropertyGremlinQueryBaseRec<TSelf>
    {

    }




    public interface IVertexPropertyGremlinQueryBase<TProperty, TValue> :
        IVertexPropertyGremlinQueryBase,
        IElementGremlinQueryBase<TProperty>
    {
        new IElementGremlinQuery<TProperty> Lower();

        IVertexPropertyGremlinQuery<VertexProperty<TValue, TMeta>, TValue, TMeta> Meta<TMeta>() where TMeta : class;

        IPropertyGremlinQuery<Property<TMetaValue>> Properties<TMetaValue>(params string[] keys);

        IValueGremlinQuery<TValue> Value();
    }

    public interface IVertexPropertyGremlinQueryBaseRec<TProperty, TValue, TSelf> :
        IVertexPropertyGremlinQueryBaseRec<TSelf>,
        IVertexPropertyGremlinQueryBase<TProperty, TValue>,
        IElementGremlinQueryBaseRec<TProperty, TSelf>
        where TSelf : IVertexPropertyGremlinQueryBaseRec<TProperty, TValue, TSelf>
    {

    }

    public interface IVertexPropertyGremlinQuery<TProperty, TValue> :
        IVertexPropertyGremlinQueryBaseRec<TProperty, TValue, IVertexPropertyGremlinQuery<TProperty, TValue>>
    {
        
    }



    public interface IVertexPropertyGremlinQueryBase<TProperty, TValue, TMeta> :
        IVertexPropertyGremlinQueryBase,
        IElementGremlinQueryBase<TProperty> where TMeta : class
    {
        new IElementGremlinQuery<TProperty> Lower();

        IPropertyGremlinQuery<Property<TTarget>> Properties<TTarget>(params Expression<Func<TMeta, TTarget>>[] projections);

        IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Property<TMetaValue>(Expression<Func<TMeta, TMetaValue>> projection, TMetaValue value);

        IValueGremlinQuery<TValue> Value();
        IValueGremlinQuery<TMetaValue> Values<TMetaValue>(params Expression<Func<TMeta, TMetaValue>>[] projections);
        new IValueGremlinQuery<TMeta> ValueMap();

        IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Where(Expression<Func<VertexProperty<TValue, TMeta>, bool>> predicate);
    }

    public interface IVertexPropertyGremlinQueryBaseRec<TProperty, TValue, TMeta, TSelf> :
        IVertexPropertyGremlinQueryBaseRec<TSelf>,
        IVertexPropertyGremlinQueryBase<TProperty, TValue, TMeta>,
        IElementGremlinQueryBaseRec<TProperty, TSelf>
        where TSelf : IVertexPropertyGremlinQueryBaseRec<TProperty, TValue, TMeta, TSelf>
        where TMeta : class
    {

    }

    public interface IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> :
        IVertexPropertyGremlinQueryBaseRec<TProperty, TValue, TMeta, IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>>
        where TMeta : class
    {

    }
}
