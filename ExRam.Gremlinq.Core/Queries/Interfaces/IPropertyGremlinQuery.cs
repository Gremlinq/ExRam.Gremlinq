namespace ExRam.Gremlinq.Core
{
    public interface IPropertyGremlinQueryBase : IGremlinQueryBase
    {
        new IPropertyGremlinQuery<TResult> Cast<TResult>();
    }

    public interface IPropertyGremlinQueryBase<TElement> :
        IPropertyGremlinQueryBase,
        IGremlinQueryBase<TElement>
    {
        IValueGremlinQuery<string> Key();

        IValueGremlinQuery<object> Value();
        IValueGremlinQuery<TValue> Value<TValue>();
    }

    public interface IPropertyGremlinQuery<TElement> :
        IPropertyGremlinQueryBase<TElement>,
        IGremlinQueryBaseRec<TElement, IPropertyGremlinQuery<TElement>>
    {

    }
}
