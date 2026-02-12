using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    public interface IVertexGremlinQueryBase :
        IEdgeOrVertexGremlinQueryBase
    {
        IVertexGremlinQuery<object> Both();
        IVertexGremlinQuery<object> Both<TEdge>();
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2>();
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2, TEdge3>();
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2, TEdge3, TEdge4>();
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>();
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>();
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>();
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>();
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>();
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>();
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>();
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>();
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>();
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>();
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>();
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>();

        IEdgeGremlinQuery<object> BothE();
        IEdgeGremlinQuery<TEdge> BothE<TEdge>();

        new IVertexGremlinQuery<TResult> Cast<TResult>();
        
        IVertexGremlinQuery<object> In();
        IVertexGremlinQuery<object> In<TEdge>();
        IVertexGremlinQuery<object> In<TEdge1, TEdge2>();
        IVertexGremlinQuery<object> In<TEdge1, TEdge2, TEdge3>();
        IVertexGremlinQuery<object> In<TEdge1, TEdge2, TEdge3, TEdge4>();
        IVertexGremlinQuery<object> In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>();
        IVertexGremlinQuery<object> In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>();
        IVertexGremlinQuery<object> In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>();
        IVertexGremlinQuery<object> In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>();
        IVertexGremlinQuery<object> In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>();
        IVertexGremlinQuery<object> In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>();
        IVertexGremlinQuery<object> In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>();
        IVertexGremlinQuery<object> In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>();
        IVertexGremlinQuery<object> In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>();
        IVertexGremlinQuery<object> In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>();
        IVertexGremlinQuery<object> In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>();
        IVertexGremlinQuery<object> In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>();

        IEdgeGremlinQuery<object> InE();
        IEdgeGremlinQuery<TEdge> InE<TEdge>();

        new IEdgeOrVertexGremlinQuery<object> Lower();

        IVertexGremlinQuery<TTarget> OfType<TTarget>();

        IVertexGremlinQuery<object> Out();
        IVertexGremlinQuery<object> Out<TEdge>();
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2>();
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2, TEdge3>();
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2, TEdge3, TEdge4>();
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>();
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>();
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>();
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>();
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>();
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>();
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>();
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>();
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>();
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>();
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>();
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>();

        IEdgeGremlinQuery<object> OutE();
        IEdgeGremlinQuery<TEdge> OutE<TEdge>();
    }

    public interface IVertexGremlinQueryBase<TVertex> :
        IVertexGremlinQueryBase,
        IEdgeOrVertexGremlinQueryBase<TVertex>
    {
        new IVertexGremlinQuery<TVertex> Update(TVertex element);

        new IInOrOutEdgeGremlinQuery<TEdge, TVertex> AddE<TEdge>(TEdge edge);
        new IInOrOutEdgeGremlinQuery<TEdge, TVertex> AddE<TEdge>() where TEdge : new();

        new IInEdgeGremlinQuery<TEdge, TVertex> InE<TEdge>();

        new IEdgeOrVertexGremlinQuery<TVertex> Lower();

        new IOutEdgeGremlinQuery<TEdge, TVertex> OutE<TEdge>();

        IVertexPropertyGremlinQuery<VertexProperty<object>, object> Properties();
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>();

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params Expression<Func<TVertex, TValue>>[] projections);
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params ReadOnlySpan<Expression<Func<TVertex, TValue>>> projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params Expression<Func<TVertex, VertexProperty<TValue>>>[] projections);
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<TValue>>>> projections);

        IVertexPropertyGremlinQuery<VertexProperty<object>, object> Properties(params Expression<Func<TVertex, VertexProperty<object>>>[] projections);
        IVertexPropertyGremlinQuery<VertexProperty<object>, object> Properties(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<object>>>> projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params Expression<Func<TVertex, TValue[]>>[] projections);
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params ReadOnlySpan<Expression<Func<TVertex, TValue[]>>> projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params Expression<Func<TVertex, VertexProperty<TValue>[]>>[] projections);
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<TValue>[]>>> projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue, TMeta>, TValue, TMeta> Properties<TValue, TMeta>(params Expression<Func<TVertex, VertexProperty<TValue, TMeta>>>[] projections);
        IVertexPropertyGremlinQuery<VertexProperty<TValue, TMeta>, TValue, TMeta> Properties<TValue, TMeta>(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<TValue, TMeta>>>> projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue, TMeta>, TValue, TMeta> Properties<TValue, TMeta>(params Expression<Func<TVertex, VertexProperty<TValue, TMeta>[]>>[] projections);
        IVertexPropertyGremlinQuery<VertexProperty<TValue, TMeta>, TValue, TMeta> Properties<TValue, TMeta>(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<TValue, TMeta>[]>>> projections);

        new IGremlinQuery<TValue> Values<TValue>(params Expression<Func<TVertex, TValue>>[] projections);
        new IGremlinQuery<TValue> Values<TValue>(params ReadOnlySpan<Expression<Func<TVertex, TValue>>> projections);

        new IGremlinQuery<TValue> Values<TValue>(params Expression<Func<TVertex, TValue[]>>[] projections);
        new IGremlinQuery<TValue> Values<TValue>(params ReadOnlySpan<Expression<Func<TVertex, TValue[]>>> projections);

        IGremlinQuery<TValue> Values<TValue>(params Expression<Func<TVertex, VertexProperty<TValue>>>[] projections);
        IGremlinQuery<TValue> Values<TValue>(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<TValue>>>> projections);

        IGremlinQuery<object> Values(params Expression<Func<TVertex, VertexProperty<object>>>[] projections);
        IGremlinQuery<object> Values(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<object>>>> projections);

        IGremlinQuery<TValue> Values<TValue>(params Expression<Func<TVertex, VertexProperty<TValue>[]>>[] projections);
        IGremlinQuery<TValue> Values<TValue>(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<TValue>[]>>> projections);

        IGremlinQuery<TValue> Values<TValue, TMeta>(params Expression<Func<TVertex, VertexProperty<TValue, TMeta>>>[] projections);
        IGremlinQuery<TValue> Values<TValue, TMeta>(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<TValue, TMeta>>>> projections);

        IGremlinQuery<TValue> Values<TValue, TMeta>(params Expression<Func<TVertex, VertexProperty<TValue, TMeta>[]>>[] projections);
        IGremlinQuery<TValue> Values<TValue, TMeta>(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<TValue, TMeta>[]>>> projections);
    }

    public interface IVertexGremlinQuery<TVertex> :
        IVertexGremlinQueryBase<TVertex>,
        IEdgeOrVertexGremlinQueryBaseRec<TVertex, IVertexGremlinQuery<TVertex>>
    {
        IVertexGremlinQuery<TVertex> Property<TProjectedValue>(Expression<Func<TVertex, TProjectedValue[]>> projection, TProjectedValue value);
    }
}
