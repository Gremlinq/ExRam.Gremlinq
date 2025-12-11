using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Represents a query for map-like results (dictionaries, tuples, etc.).
    /// </summary>
    public interface IMapGremlinQueryBase :
        IGremlinQueryBase;

    /// <summary>
    /// Represents a strongly-typed query for map-like results with projection capabilities.
    /// </summary>
    /// <typeparam name="TElement">The type of the map elements.</typeparam>
    public interface IMapGremlinQueryBase<TElement> :
        IMapGremlinQueryBase,
        IGremlinQueryBase<TElement>
    {
        /// <summary>
        /// Selects a single value from the map using a projection.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to select.</typeparam>
        /// <param name="projection">Expression selecting the value from the map.</param>
        /// <returns>A query that returns the selected values.</returns>
        IGremlinQuery<TValue> Select<TValue>(Expression<Func<TElement, TValue>> projection);

        /// <summary>
        /// Selects two values from the map as a tuple.
        /// </summary>
        IMapGremlinQuery<(TItem1, TItem2)> Select<TItem1, TItem2>(Expression<Func<TElement, TItem1>> projection1, Expression<Func<TElement, TItem2>> projection2);
        
        /// <summary>
        /// Selects three values from the map as a tuple.
        /// </summary>
        IMapGremlinQuery<(TItem1, TItem2, TItem3)> Select<TItem1, TItem2, TItem3>(Expression<Func<TElement, TItem1>> projection1, Expression<Func<TElement, TItem2>> projection2, Expression<Func<TElement, TItem3>> projection3);
        
        /// <summary>
        /// Selects four values from the map as a tuple.
        /// </summary>
        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4)> Select<TItem1, TItem2, TItem3, TItem4>(Expression<Func<TElement, TItem1>> projection1, Expression<Func<TElement, TItem2>> projection2, Expression<Func<TElement, TItem3>> projection3, Expression<Func<TElement, TItem4>> projection4);
        
        /// <summary>
        /// Selects five values from the map as a tuple.
        /// </summary>
        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5)> Select<TItem1, TItem2, TItem3, TItem4, TItem5>(Expression<Func<TElement, TItem1>> projection1, Expression<Func<TElement, TItem2>> projection2, Expression<Func<TElement, TItem3>> projection3, Expression<Func<TElement, TItem4>> projection4, Expression<Func<TElement, TItem5>> projection5);
        
        /// <summary>
        /// Selects six values from the map as a tuple.
        /// </summary>
        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6)> Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6>(Expression<Func<TElement, TItem1>> projection1, Expression<Func<TElement, TItem2>> projection2, Expression<Func<TElement, TItem3>> projection3, Expression<Func<TElement, TItem4>> projection4, Expression<Func<TElement, TItem5>> projection5, Expression<Func<TElement, TItem6>> projection6);
        
        /// <summary>
        /// Selects seven values from the map as a tuple.
        /// </summary>
        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7)> Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7>(Expression<Func<TElement, TItem1>> projection1, Expression<Func<TElement, TItem2>> projection2, Expression<Func<TElement, TItem3>> projection3, Expression<Func<TElement, TItem4>> projection4, Expression<Func<TElement, TItem5>> projection5, Expression<Func<TElement, TItem6>> projection6, Expression<Func<TElement, TItem7>> projection7);
        
        /// <summary>
        /// Selects eight values from the map as a tuple.
        /// </summary>
        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8)> Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8>(Expression<Func<TElement, TItem1>> projection1, Expression<Func<TElement, TItem2>> projection2, Expression<Func<TElement, TItem3>> projection3, Expression<Func<TElement, TItem4>> projection4, Expression<Func<TElement, TItem5>> projection5, Expression<Func<TElement, TItem6>> projection6, Expression<Func<TElement, TItem7>> projection7, Expression<Func<TElement, TItem8>> projection8);
        
        /// <summary>
        /// Selects nine values from the map as a tuple.
        /// </summary>
        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9)> Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9>(Expression<Func<TElement, TItem1>> projection1, Expression<Func<TElement, TItem2>> projection2, Expression<Func<TElement, TItem3>> projection3, Expression<Func<TElement, TItem4>> projection4, Expression<Func<TElement, TItem5>> projection5, Expression<Func<TElement, TItem6>> projection6, Expression<Func<TElement, TItem7>> projection7, Expression<Func<TElement, TItem8>> projection8, Expression<Func<TElement, TItem9>> projection9);
        
        /// <summary>
        /// Selects ten values from the map as a tuple.
        /// </summary>
        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10)> Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10>(Expression<Func<TElement, TItem1>> projection1, Expression<Func<TElement, TItem2>> projection2, Expression<Func<TElement, TItem3>> projection3, Expression<Func<TElement, TItem4>> projection4, Expression<Func<TElement, TItem5>> projection5, Expression<Func<TElement, TItem6>> projection6, Expression<Func<TElement, TItem7>> projection7, Expression<Func<TElement, TItem8>> projection8, Expression<Func<TElement, TItem9>> projection9, Expression<Func<TElement, TItem10>> projection10);
        
        /// <summary>
        /// Selects eleven values from the map as a tuple.
        /// </summary>
        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11)> Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11>(Expression<Func<TElement, TItem1>> projection1, Expression<Func<TElement, TItem2>> projection2, Expression<Func<TElement, TItem3>> projection3, Expression<Func<TElement, TItem4>> projection4, Expression<Func<TElement, TItem5>> projection5, Expression<Func<TElement, TItem6>> projection6, Expression<Func<TElement, TItem7>> projection7, Expression<Func<TElement, TItem8>> projection8, Expression<Func<TElement, TItem9>> projection9, Expression<Func<TElement, TItem10>> projection10, Expression<Func<TElement, TItem11>> projection11);
        
        /// <summary>
        /// Selects twelve values from the map as a tuple.
        /// </summary>
        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12)> Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12>(Expression<Func<TElement, TItem1>> projection1, Expression<Func<TElement, TItem2>> projection2, Expression<Func<TElement, TItem3>> projection3, Expression<Func<TElement, TItem4>> projection4, Expression<Func<TElement, TItem5>> projection5, Expression<Func<TElement, TItem6>> projection6, Expression<Func<TElement, TItem7>> projection7, Expression<Func<TElement, TItem8>> projection8, Expression<Func<TElement, TItem9>> projection9, Expression<Func<TElement, TItem10>> projection10, Expression<Func<TElement, TItem11>> projection11, Expression<Func<TElement, TItem12>> projection12);
        
        /// <summary>
        /// Selects thirteen values from the map as a tuple.
        /// </summary>
        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13)> Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13>(Expression<Func<TElement, TItem1>> projection1, Expression<Func<TElement, TItem2>> projection2, Expression<Func<TElement, TItem3>> projection3, Expression<Func<TElement, TItem4>> projection4, Expression<Func<TElement, TItem5>> projection5, Expression<Func<TElement, TItem6>> projection6, Expression<Func<TElement, TItem7>> projection7, Expression<Func<TElement, TItem8>> projection8, Expression<Func<TElement, TItem9>> projection9, Expression<Func<TElement, TItem10>> projection10, Expression<Func<TElement, TItem11>> projection11, Expression<Func<TElement, TItem12>> projection12, Expression<Func<TElement, TItem13>> projection13);
        
        /// <summary>
        /// Selects fourteen values from the map as a tuple.
        /// </summary>
        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14)> Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14>(Expression<Func<TElement, TItem1>> projection1, Expression<Func<TElement, TItem2>> projection2, Expression<Func<TElement, TItem3>> projection3, Expression<Func<TElement, TItem4>> projection4, Expression<Func<TElement, TItem5>> projection5, Expression<Func<TElement, TItem6>> projection6, Expression<Func<TElement, TItem7>> projection7, Expression<Func<TElement, TItem8>> projection8, Expression<Func<TElement, TItem9>> projection9, Expression<Func<TElement, TItem10>> projection10, Expression<Func<TElement, TItem11>> projection11, Expression<Func<TElement, TItem12>> projection12, Expression<Func<TElement, TItem13>> projection13, Expression<Func<TElement, TItem14>> projection14);
        
        /// <summary>
        /// Selects fifteen values from the map as a tuple.
        /// </summary>
        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15)> Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15>(Expression<Func<TElement, TItem1>> projection1, Expression<Func<TElement, TItem2>> projection2, Expression<Func<TElement, TItem3>> projection3, Expression<Func<TElement, TItem4>> projection4, Expression<Func<TElement, TItem5>> projection5, Expression<Func<TElement, TItem6>> projection6, Expression<Func<TElement, TItem7>> projection7, Expression<Func<TElement, TItem8>> projection8, Expression<Func<TElement, TItem9>> projection9, Expression<Func<TElement, TItem10>> projection10, Expression<Func<TElement, TItem11>> projection11, Expression<Func<TElement, TItem12>> projection12, Expression<Func<TElement, TItem13>> projection13, Expression<Func<TElement, TItem14>> projection14, Expression<Func<TElement, TItem15>> projection15);
        
        /// <summary>
        /// Selects sixteen values from the map as a tuple.
        /// </summary>
        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16)> Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16>(Expression<Func<TElement, TItem1>> projection1, Expression<Func<TElement, TItem2>> projection2, Expression<Func<TElement, TItem3>> projection3, Expression<Func<TElement, TItem4>> projection4, Expression<Func<TElement, TItem5>> projection5, Expression<Func<TElement, TItem6>> projection6, Expression<Func<TElement, TItem7>> projection7, Expression<Func<TElement, TItem8>> projection8, Expression<Func<TElement, TItem9>> projection9, Expression<Func<TElement, TItem10>> projection10, Expression<Func<TElement, TItem11>> projection11, Expression<Func<TElement, TItem12>> projection12, Expression<Func<TElement, TItem13>> projection13, Expression<Func<TElement, TItem14>> projection14, Expression<Func<TElement, TItem15>> projection15, Expression<Func<TElement, TItem16>> projection16);
    }

    /// <summary>
    /// Represents a query for strongly-typed map results with full query operations.
    /// </summary>
    /// <typeparam name="TElement">The type of the map elements.</typeparam>
    public interface IMapGremlinQuery<TElement> :
        IMapGremlinQueryBase<TElement>,
        IGremlinQueryBaseRec<TElement, IMapGremlinQuery<TElement>>;
}
