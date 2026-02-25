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
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2>();
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2, TEdge3>();
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2, TEdge3, TEdge4>();
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>();
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>();
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>();
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>();
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>();
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>();
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>();
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>();
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>();
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>();
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>();
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>();

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
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2>();
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2, TEdge3>();
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2, TEdge3, TEdge4>();
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>();
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>();
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>();
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>();
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>();
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>();
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>();
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>();
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>();
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>();
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>();
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>();

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
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2>();
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2, TEdge3>();
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2, TEdge3, TEdge4>();
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>();
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>();
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>();
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>();
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>();
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>();
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>();
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>();
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>();
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>();
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>();
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>();
    }

    public interface IVertexGremlinQueryBase<TVertex> :
        IVertexGremlinQueryBase,
        IEdgeOrVertexGremlinQueryBase<TVertex>
    {
        new IVertexGremlinQuery<TVertex> Update(TVertex element);

        new IInOrOutEdgeGremlinQuery<TEdge, TVertex> AddE<TEdge>(TEdge edge);
        new IInOrOutEdgeGremlinQuery<TEdge, TVertex> AddE<TEdge>() where TEdge : new();

        new IEdgeGremlinQuery<object, TVertex> BothE();
        new IEdgeGremlinQuery<TEdge, TVertex> BothE<TEdge>();
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2>();
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2, TEdge3>();
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2, TEdge3, TEdge4>();
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>();
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>();
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>();
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>();
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>();
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>();
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>();
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>();
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>();
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>();
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>();
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>();

        new IInEdgeGremlinQuery<object, TVertex> InE();
        new IInEdgeGremlinQuery<TEdge, TVertex> InE<TEdge>();
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2>();
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2, TEdge3>();
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2, TEdge3, TEdge4>();
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>();
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>();
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>();
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>();
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>();
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>();
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>();
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>();
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>();
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>();
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>();
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>();

        new IEdgeOrVertexGremlinQuery<TVertex> Lower();

        new IOutEdgeGremlinQuery<object, TVertex> OutE();
        new IOutEdgeGremlinQuery<TEdge, TVertex> OutE<TEdge>();
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2>();
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2, TEdge3>();
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2, TEdge3, TEdge4>();
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>();
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>();
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>();
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>();
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>();
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>();
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>();
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>();
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>();
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>();
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>();
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>();

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
