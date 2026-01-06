namespace ExRam.Gremlinq.Core
{
    public interface IDateGremlinQuery<TDate> : IGremlinQueryBaseRec<TDate, IDateGremlinQuery<TDate>>
    {
        IDateGremlinQuery<TDate> Add(TimeSpan duration);
    }
}
