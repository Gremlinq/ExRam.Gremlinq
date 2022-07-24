using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Core
{
    public interface IProjectDynamicResult
    {
        IValueGremlinQuery<dynamic> Build();
    }

    // ReSharper disable once UnusedTypeParameter
    public interface IProjectTupleResult<TTuple>
        where TTuple : ITuple
    {
        IValueTupleGremlinQuery<TTuple> Build();
    }

    // ReSharper disable once UnusedTypeParameter
    public interface IProjectTypeResult<TTargetType>
    {
        IValueGremlinQuery<TTargetType> Build();
    }

    public interface IProjectBuilder<out TSourceQuery, TElement>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement> ToTuple();
        IProjectDynamicBuilder<TSourceQuery, TElement> ToDynamic();
        IProjectTypeBuilder<TSourceQuery, TElement, TTargetType> To<TTargetType>();

        IProjectBuilder<TSourceQuery, TElement> WithEmptyProjectionProtection();
    }

    public interface IProjectTypeBuilder<out TSourceQuery, TElement, TTargetType> : IProjectTypeResult<TTargetType>
       where TSourceQuery : IGremlinQueryBase
    {
        IProjectTypeBuilder<TSourceQuery, TElement, TTargetType> By<TSourceProperty, TTargetProperty>(Expression<Func<TTargetType, TTargetProperty>> targetExpression, Func<TSourceQuery, IGremlinQueryBase<TSourceProperty>> projection)
            where TSourceProperty : TTargetProperty;

        IProjectTypeBuilder<TSourceQuery, TElement, TTargetType> By<TSourceProperty, TTargetProperty>(Expression<Func<TTargetType, TTargetProperty>> targetExpression, Expression<Func<TElement, TSourceProperty>> projection)
            where TSourceProperty : TTargetProperty;
    }

    public interface IProjectDynamicBuilder<out TSourceQuery, TElement> : IProjectDynamicResult
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectDynamicBuilder<TSourceQuery, TElement> By(Func<TSourceQuery, IGremlinQueryBase> projection);
        IProjectDynamicBuilder<TSourceQuery, TElement> By(string name, Func<TSourceQuery, IGremlinQueryBase> projection);
        IProjectDynamicBuilder<TSourceQuery, TElement> By(string name, Expression<Func<TElement, object>> projection);
        IProjectDynamicBuilder<TSourceQuery, TElement> By(Expression<Func<TElement, object>> projection);
    }
}
