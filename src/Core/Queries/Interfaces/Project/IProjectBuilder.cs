using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Core
{
    public interface IProjectDynamicResult
    {
        /// <summary>
        /// Builds and returns the dynamic project query.
        /// </summary>
        IGremlinQuery<dynamic> Build();
    }

    // ReSharper disable once UnusedTypeParameter
    public interface IProjectTupleResult<TTuple>
        where TTuple : ITuple
    {
        /// <summary>
        /// Builds and returns the tuple project query.
        /// </summary>
        IMapGremlinQuery<TTuple> Build();
    }

    // ReSharper disable once UnusedTypeParameter
    public interface IProjectMapResult<TTargetType>
    {
        /// <summary>
        /// Builds and returns the mapped project query.
        /// </summary>
        IMapGremlinQuery<TTargetType> Build();
    }

    public interface IProjectBuilder<out TSourceQuery, TElement>
        where TSourceQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Projects the current element to a tuple.
        /// Corresponds to the Gremlin <c>project()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#project-step">Reference Documentation - Project Step</seealso>
        IProjectTupleBuilder<TSourceQuery, TElement> ToTuple();

        /// <summary>
        /// Projects the current element to a dynamic object.
        /// Corresponds to the Gremlin <c>project()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#project-step">Reference Documentation - Project Step</seealso>
        IProjectDynamicBuilder<TSourceQuery, TElement> ToDynamic();

        /// <summary>
        /// Projects the current element to a strongly-typed object.
        /// Corresponds to the Gremlin <c>project()</c> step.
        /// </summary>
        /// <typeparam name="TTargetType">The target type to project to.</typeparam>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#project-step">Reference Documentation - Project Step</seealso>
        IProjectMapBuilder<TSourceQuery, TElement, TTargetType> To<TTargetType>();

        /// <summary>
        /// Enables empty projection protection for the project builder.
        /// </summary>
        IProjectBuilder<TSourceQuery, TElement> WithEmptyProjectionProtection();
    }

    public interface IProjectMapBuilder<out TSourceQuery, TElement, TTargetType> : IProjectMapResult<TTargetType>
       where TSourceQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Adds a projection from a traversal result to a target property.
        /// Corresponds to the Gremlin <c>by()</c> modulator on a <c>project()</c> step.
        /// </summary>
        /// <typeparam name="TSourceProperty">The type of the source property.</typeparam>
        /// <typeparam name="TTargetProperty">The type of the target property.</typeparam>
        /// <param name="targetExpression">An expression selecting the target property.</param>
        /// <param name="projection">A traversal that produces the source value.</param>
        IProjectMapBuilder<TSourceQuery, TElement, TTargetType> By<TSourceProperty, TTargetProperty>(Expression<Func<TTargetType, TTargetProperty>> targetExpression, Func<TSourceQuery, IGremlinQueryBase<TSourceProperty>> projection)
            where TSourceProperty : TTargetProperty;

        /// <summary>
        /// Adds a projection from a source element property to a target property.
        /// Corresponds to the Gremlin <c>by()</c> modulator on a <c>project()</c> step.
        /// </summary>
        /// <typeparam name="TSourceProperty">The type of the source property.</typeparam>
        /// <typeparam name="TTargetProperty">The type of the target property.</typeparam>
        /// <param name="targetExpression">An expression selecting the target property.</param>
        /// <param name="projection">An expression selecting the source property.</param>
        IProjectMapBuilder<TSourceQuery, TElement, TTargetType> By<TSourceProperty, TTargetProperty>(Expression<Func<TTargetType, TTargetProperty>> targetExpression, Expression<Func<TElement, TSourceProperty>> projection)
            where TSourceProperty : TTargetProperty;
    }

    public interface IProjectDynamicBuilder<out TSourceQuery, TElement> : IProjectDynamicResult
        where TSourceQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Adds an unnamed projection from a traversal.
        /// Corresponds to the Gremlin <c>by()</c> modulator on a <c>project()</c> step.
        /// </summary>
        /// <param name="projection">A traversal that produces the projected value.</param>
        IProjectDynamicBuilder<TSourceQuery, TElement> By(Func<TSourceQuery, IGremlinQueryBase> projection);

        /// <summary>
        /// Adds a named projection from a traversal.
        /// Corresponds to the Gremlin <c>by()</c> modulator on a <c>project()</c> step.
        /// </summary>
        /// <param name="name">The name of the projected value in the result map.</param>
        /// <param name="projection">A traversal that produces the projected value.</param>
        IProjectDynamicBuilder<TSourceQuery, TElement> By(string name, Func<TSourceQuery, IGremlinQueryBase> projection);

        /// <summary>
        /// Adds a named projection from a property expression.
        /// Corresponds to the Gremlin <c>by()</c> modulator on a <c>project()</c> step.
        /// </summary>
        /// <param name="name">The name of the projected value in the result map.</param>
        /// <param name="projection">An expression selecting the property to project.</param>
        IProjectDynamicBuilder<TSourceQuery, TElement> By(string name, Expression<Func<TElement, object>> projection);

        /// <summary>
        /// Adds an unnamed projection from a property expression.
        /// Corresponds to the Gremlin <c>by()</c> modulator on a <c>project()</c> step.
        /// </summary>
        /// <param name="projection">An expression selecting the property to project.</param>
        IProjectDynamicBuilder<TSourceQuery, TElement> By(Expression<Func<TElement, object>> projection);
    }
}
