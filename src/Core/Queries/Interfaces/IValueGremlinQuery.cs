namespace ExRam.Gremlinq.Core
{
    public interface IValueGremlinQuery<TElement> :
        IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>,
        IGremlinQueryBase,
        IGremlinQueryBase<TElement>,
        IGremlinQueryBaseRec<TElement, IValueGremlinQuery<TElement>>
    {

    }
}
