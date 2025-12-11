using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Represents a project builder result that produces dynamic objects.
    /// </summary>
    public interface IProjectDynamicResult
    {
        /// <summary>
        /// Builds and returns a query that produces dynamic objects.
        /// </summary>
        /// <returns>A query returning dynamic objects with the projected properties.</returns>
        IGremlinQuery<dynamic> Build();
    }

    /// <summary>
    /// Represents a project builder result that produces tuples.
    /// </summary>
    /// <typeparam name="TTuple">The tuple type.</typeparam>
    // ReSharper disable once UnusedTypeParameter
    public interface IProjectTupleResult<TTuple>
        where TTuple : ITuple
    {
        /// <summary>
        /// Builds and returns a map query that produces tuples.
        /// </summary>
        /// <returns>A map query returning tuples with the projected values.</returns>
        IMapGremlinQuery<TTuple> Build();
    }

    /// <summary>
    /// Represents a project builder result that produces mapped objects of a target type.
    /// </summary>
    /// <typeparam name="TTargetType">The target type to project to.</typeparam>
    // ReSharper disable once UnusedTypeParameter
    public interface IProjectMapResult<TTargetType>
    {
        /// <summary>
        /// Builds and returns a map query that produces the target type.
        /// </summary>
        /// <returns>A map query returning objects of the target type.</returns>
        IMapGremlinQuery<TTargetType> Build();
    }

    /// <summary>
    /// Builds projection specifications for query results.
    /// </summary>
    /// <typeparam name="TSourceQuery">The source query type.</typeparam>
    /// <typeparam name="TElement">The source element type.</typeparam>
    public interface IProjectBuilder<out TSourceQuery, TElement>
        where TSourceQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Configures the projection to produce tuples.
        /// </summary>
        /// <returns>A tuple projection builder.</returns>
        IProjectTupleBuilder<TSourceQuery, TElement> ToTuple();
        
        /// <summary>
        /// Configures the projection to produce dynamic objects.
        /// </summary>
        /// <returns>A dynamic projection builder.</returns>
        IProjectDynamicBuilder<TSourceQuery, TElement> ToDynamic();
        
        /// <summary>
        /// Configures the projection to produce objects of a specific target type.
        /// </summary>
        /// <typeparam name="TTargetType">The target type to project to.</typeparam>
        /// <returns>A map projection builder for the target type.</returns>
        IProjectMapBuilder<TSourceQuery, TElement, TTargetType> To<TTargetType>();

        /// <summary>
        /// Enables protection against empty projections that would otherwise cause runtime errors.
        /// </summary>
        /// <returns>The project builder with empty projection protection enabled.</returns>
        IProjectBuilder<TSourceQuery, TElement> WithEmptyProjectionProtection();
    }

    /// <summary>
    /// Builds projections that map query results to a specific target type.
    /// </summary>
    /// <typeparam name="TSourceQuery">The source query type.</typeparam>
    /// <typeparam name="TElement">The source element type.</typeparam>
    /// <typeparam name="TTargetType">The target type to project to.</typeparam>
    public interface IProjectMapBuilder<out TSourceQuery, TElement, TTargetType> : IProjectMapResult<TTargetType>
       where TSourceQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Maps a source property to a target property using a traversal.
        /// </summary>
        /// <typeparam name="TSourceProperty">The source property type.</typeparam>
        /// <typeparam name="TTargetProperty">The target property type.</typeparam>
        /// <param name="targetExpression">Expression selecting the target property.</param>
        /// <param name="projection">A traversal that produces the source property value.</param>
        /// <returns>The map builder with the property mapping added.</returns>
        IProjectMapBuilder<TSourceQuery, TElement, TTargetType> By<TSourceProperty, TTargetProperty>(Expression<Func<TTargetType, TTargetProperty>> targetExpression, Func<TSourceQuery, IGremlinQueryBase<TSourceProperty>> projection)
            where TSourceProperty : TTargetProperty;

        /// <summary>
        /// Maps a source property to a target property using an expression.
        /// </summary>
        /// <typeparam name="TSourceProperty">The source property type.</typeparam>
        /// <typeparam name="TTargetProperty">The target property type.</typeparam>
        /// <param name="targetExpression">Expression selecting the target property.</param>
        /// <param name="projection">Expression selecting the source property.</param>
        /// <returns>The map builder with the property mapping added.</returns>
        IProjectMapBuilder<TSourceQuery, TElement, TTargetType> By<TSourceProperty, TTargetProperty>(Expression<Func<TTargetType, TTargetProperty>> targetExpression, Expression<Func<TElement, TSourceProperty>> projection)
            where TSourceProperty : TTargetProperty;
    }

    /// <summary>
    /// Builds projections that produce dynamic objects.
    /// </summary>
    /// <typeparam name="TSourceQuery">The source query type.</typeparam>
    /// <typeparam name="TElement">The source element type.</typeparam>
    public interface IProjectDynamicBuilder<out TSourceQuery, TElement> : IProjectDynamicResult
        where TSourceQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Adds a projected value to the dynamic result using a traversal.
        /// </summary>
        /// <param name="projection">A traversal that produces the value to project.</param>
        /// <returns>The dynamic builder with the projection added.</returns>
        IProjectDynamicBuilder<TSourceQuery, TElement> By(Func<TSourceQuery, IGremlinQueryBase> projection);
        
        /// <summary>
        /// Adds a named projected value to the dynamic result using a traversal.
        /// </summary>
        /// <param name="name">The property name in the dynamic result.</param>
        /// <param name="projection">A traversal that produces the value to project.</param>
        /// <returns>The dynamic builder with the projection added.</returns>
        IProjectDynamicBuilder<TSourceQuery, TElement> By(string name, Func<TSourceQuery, IGremlinQueryBase> projection);
        
        /// <summary>
        /// Adds a named projected value to the dynamic result using an expression.
        /// </summary>
        /// <param name="name">The property name in the dynamic result.</param>
        /// <param name="projection">Expression selecting the value to project.</param>
        /// <returns>The dynamic builder with the projection added.</returns>
        IProjectDynamicBuilder<TSourceQuery, TElement> By(string name, Expression<Func<TElement, object>> projection);
        
        /// <summary>
        /// Adds a projected value to the dynamic result using an expression.
        /// </summary>
        /// <param name="projection">Expression selecting the value to project.</param>
        /// <returns>The dynamic builder with the projection added.</returns>
        IProjectDynamicBuilder<TSourceQuery, TElement> By(Expression<Func<TElement, object>> projection);
    }
}
