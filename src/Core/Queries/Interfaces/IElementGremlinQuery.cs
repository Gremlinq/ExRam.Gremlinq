using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public interface IElementGremlinQueryBase :
        IGremlinQueryBase
    {
        new IElementGremlinQuery<TResult> Cast<TResult>();

        IGremlinQuery<object> Id();

        IGremlinQuery<string> Label();

        IGremlinQuery<TTarget> Values<TTarget>();
        IGremlinQuery<object> Values();

        IGremlinQuery<IDictionary<string, TTarget>> ValueMap<TTarget>();
        IGremlinQuery<IDictionary<string, object>> ValueMap();
    }

    public interface IElementGremlinQueryBaseRec<TSelf> :
        IElementGremlinQueryBase,
        IGremlinQueryBaseRec<TSelf>
        where TSelf : IElementGremlinQueryBaseRec<TSelf>
    {
        TSelf Property(string key, object? value);
        TSelf Property(string key, Func<TSelf, IGremlinQueryBase> valueTransformation);
    }

    public interface IElementGremlinQueryBase<TElement> :
        IElementGremlinQueryBase,
        IGremlinQueryBase<TElement>
    {
        IElementGremlinQuery<TElement> Update(TElement element);

        IGremlinQuery<IDictionary<string, TTarget>> ValueMap<TTarget>(params Expression<Func<TElement, TTarget>>[] keys);

        IGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TElement, TTarget>>[] projections);
        IGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TElement, TTarget[]>>[] projections);
    }

    public interface IElementGremlinQueryBaseRec<TElement, TSelf> :
        IElementGremlinQueryBaseRec<TSelf>,
        IElementGremlinQueryBase<TElement>,
        IGremlinQueryBaseRec<TElement, TSelf>
        where TSelf : IElementGremlinQueryBaseRec<TElement, TSelf>
    {
        TSelf Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQueryBase<TProjection>, IGremlinQueryBase> propertyTraversal);

        TSelf Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, TProjectedValue value);
        TSelf Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, StepLabel<TProjectedValue> stepLabel);
        TSelf Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, Func<TSelf, IGremlinQueryBase<TProjectedValue>> valueTraversal);
    }

    public interface IElementGremlinQuery<TElement> :
        IElementGremlinQueryBaseRec<TElement, IElementGremlinQuery<TElement>>
    {
    }
}
