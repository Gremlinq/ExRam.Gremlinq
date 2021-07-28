#pragma warning disable CS0109 // Member does not hide an inherited member; new keyword is not required
using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public interface IProjectTupleBuilder<out TSourceQuery, TElement >
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1> By<TItem1>(Func<TSourceQuery, IGremlinQueryBase<TItem1>> projection);
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1> By<TItem1>(Expression<Func<TElement, TItem1>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement , TItem1>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2> By<TItem2>(Func<TSourceQuery, IGremlinQueryBase<TItem2>> projection);
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2> By<TItem2>(Expression<Func<TElement, TItem2>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement , TItem1, TItem2>
        : IProjectResult<(TItem1, TItem2)>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3> By<TItem3>(Func<TSourceQuery, IGremlinQueryBase<TItem3>> projection);
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3> By<TItem3>(Expression<Func<TElement, TItem3>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement , TItem1, TItem2, TItem3>
        : IProjectResult<(TItem1, TItem2, TItem3)>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4> By<TItem4>(Func<TSourceQuery, IGremlinQueryBase<TItem4>> projection);
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4> By<TItem4>(Expression<Func<TElement, TItem4>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement , TItem1, TItem2, TItem3, TItem4>
        : IProjectResult<(TItem1, TItem2, TItem3, TItem4)>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5> By<TItem5>(Func<TSourceQuery, IGremlinQueryBase<TItem5>> projection);
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5> By<TItem5>(Expression<Func<TElement, TItem5>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement , TItem1, TItem2, TItem3, TItem4, TItem5>
        : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5)>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6> By<TItem6>(Func<TSourceQuery, IGremlinQueryBase<TItem6>> projection);
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6> By<TItem6>(Expression<Func<TElement, TItem6>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6>
        : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6)>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7> By<TItem7>(Func<TSourceQuery, IGremlinQueryBase<TItem7>> projection);
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7> By<TItem7>(Expression<Func<TElement, TItem7>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7>
        : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7)>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8> By<TItem8>(Func<TSourceQuery, IGremlinQueryBase<TItem8>> projection);
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8> By<TItem8>(Expression<Func<TElement, TItem8>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8>
        : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8)>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9> By<TItem9>(Func<TSourceQuery, IGremlinQueryBase<TItem9>> projection);
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9> By<TItem9>(Expression<Func<TElement, TItem9>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9>
        : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9)>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10> By<TItem10>(Func<TSourceQuery, IGremlinQueryBase<TItem10>> projection);
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10> By<TItem10>(Expression<Func<TElement, TItem10>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10>
        : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10)>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11> By<TItem11>(Func<TSourceQuery, IGremlinQueryBase<TItem11>> projection);
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11> By<TItem11>(Expression<Func<TElement, TItem11>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11>
        : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11)>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12> By<TItem12>(Func<TSourceQuery, IGremlinQueryBase<TItem12>> projection);
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12> By<TItem12>(Expression<Func<TElement, TItem12>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12>
        : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12)>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13> By<TItem13>(Func<TSourceQuery, IGremlinQueryBase<TItem13>> projection);
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13> By<TItem13>(Expression<Func<TElement, TItem13>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13>
        : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13)>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14> By<TItem14>(Func<TSourceQuery, IGremlinQueryBase<TItem14>> projection);
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14> By<TItem14>(Expression<Func<TElement, TItem14>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14>
        : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14)>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15> By<TItem15>(Func<TSourceQuery, IGremlinQueryBase<TItem15>> projection);
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15> By<TItem15>(Expression<Func<TElement, TItem15>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15>
        : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15)>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16> By<TItem16>(Func<TSourceQuery, IGremlinQueryBase<TItem16>> projection);
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16> By<TItem16>(Expression<Func<TElement, TItem16>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16>
        : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16)>
        where TSourceQuery : IGremlinQueryBase
    {
    }



    partial class GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>
    {
        private sealed partial class ProjectBuilder<TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16> :
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1>
        {

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement,  TNewItem1> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement >.By<TNewItem1>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem1>> projection)
            {
                return ByLambda< TNewItem1 , object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement,  TNewItem1> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement >.By<TNewItem1>(Expression<Func<TProjectElement, TNewItem1>> projection)
            {
                return ByExpression< TNewItem1 , object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1,  TNewItem2> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1>.By<TNewItem2>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem2>> projection)
            {
                return ByLambda<TItem1,  TNewItem2 , object, object, object, object, object, object, object, object, object, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1,  TNewItem2> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1>.By<TNewItem2>(Expression<Func<TProjectElement, TNewItem2>> projection)
            {
                return ByExpression<TItem1,  TNewItem2 , object, object, object, object, object, object, object, object, object, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2,  TNewItem3> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1, TItem2>.By<TNewItem3>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem3>> projection)
            {
                return ByLambda<TItem1, TItem2,  TNewItem3 , object, object, object, object, object, object, object, object, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2,  TNewItem3> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1, TItem2>.By<TNewItem3>(Expression<Func<TProjectElement, TNewItem3>> projection)
            {
                return ByExpression<TItem1, TItem2,  TNewItem3 , object, object, object, object, object, object, object, object, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3,  TNewItem4> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1, TItem2, TItem3>.By<TNewItem4>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem4>> projection)
            {
                return ByLambda<TItem1, TItem2, TItem3,  TNewItem4 , object, object, object, object, object, object, object, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3,  TNewItem4> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1, TItem2, TItem3>.By<TNewItem4>(Expression<Func<TProjectElement, TNewItem4>> projection)
            {
                return ByExpression<TItem1, TItem2, TItem3,  TNewItem4 , object, object, object, object, object, object, object, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4,  TNewItem5> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1, TItem2, TItem3, TItem4>.By<TNewItem5>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem5>> projection)
            {
                return ByLambda<TItem1, TItem2, TItem3, TItem4,  TNewItem5 , object, object, object, object, object, object, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4,  TNewItem5> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1, TItem2, TItem3, TItem4>.By<TNewItem5>(Expression<Func<TProjectElement, TNewItem5>> projection)
            {
                return ByExpression<TItem1, TItem2, TItem3, TItem4,  TNewItem5 , object, object, object, object, object, object, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5,  TNewItem6> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1, TItem2, TItem3, TItem4, TItem5>.By<TNewItem6>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem6>> projection)
            {
                return ByLambda<TItem1, TItem2, TItem3, TItem4, TItem5,  TNewItem6 , object, object, object, object, object, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5,  TNewItem6> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1, TItem2, TItem3, TItem4, TItem5>.By<TNewItem6>(Expression<Func<TProjectElement, TNewItem6>> projection)
            {
                return ByExpression<TItem1, TItem2, TItem3, TItem4, TItem5,  TNewItem6 , object, object, object, object, object, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6,  TNewItem7> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6>.By<TNewItem7>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem7>> projection)
            {
                return ByLambda<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6,  TNewItem7 , object, object, object, object, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6,  TNewItem7> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6>.By<TNewItem7>(Expression<Func<TProjectElement, TNewItem7>> projection)
            {
                return ByExpression<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6,  TNewItem7 , object, object, object, object, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7,  TNewItem8> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7>.By<TNewItem8>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem8>> projection)
            {
                return ByLambda<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7,  TNewItem8 , object, object, object, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7,  TNewItem8> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7>.By<TNewItem8>(Expression<Func<TProjectElement, TNewItem8>> projection)
            {
                return ByExpression<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7,  TNewItem8 , object, object, object, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8,  TNewItem9> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8>.By<TNewItem9>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem9>> projection)
            {
                return ByLambda<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8,  TNewItem9 , object, object, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8,  TNewItem9> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8>.By<TNewItem9>(Expression<Func<TProjectElement, TNewItem9>> projection)
            {
                return ByExpression<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8,  TNewItem9 , object, object, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9,  TNewItem10> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9>.By<TNewItem10>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem10>> projection)
            {
                return ByLambda<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9,  TNewItem10 , object, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9,  TNewItem10> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9>.By<TNewItem10>(Expression<Func<TProjectElement, TNewItem10>> projection)
            {
                return ByExpression<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9,  TNewItem10 , object, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10,  TNewItem11> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10>.By<TNewItem11>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem11>> projection)
            {
                return ByLambda<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10,  TNewItem11 , object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10,  TNewItem11> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10>.By<TNewItem11>(Expression<Func<TProjectElement, TNewItem11>> projection)
            {
                return ByExpression<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10,  TNewItem11 , object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11,  TNewItem12> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11>.By<TNewItem12>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem12>> projection)
            {
                return ByLambda<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11,  TNewItem12 , object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11,  TNewItem12> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11>.By<TNewItem12>(Expression<Func<TProjectElement, TNewItem12>> projection)
            {
                return ByExpression<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11,  TNewItem12 , object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12,  TNewItem13> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12>.By<TNewItem13>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem13>> projection)
            {
                return ByLambda<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12,  TNewItem13 , object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12,  TNewItem13> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12>.By<TNewItem13>(Expression<Func<TProjectElement, TNewItem13>> projection)
            {
                return ByExpression<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12,  TNewItem13 , object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13,  TNewItem14> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13>.By<TNewItem14>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem14>> projection)
            {
                return ByLambda<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13,  TNewItem14 , object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13,  TNewItem14> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13>.By<TNewItem14>(Expression<Func<TProjectElement, TNewItem14>> projection)
            {
                return ByExpression<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13,  TNewItem14 , object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14,  TNewItem15> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14>.By<TNewItem15>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem15>> projection)
            {
                return ByLambda<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14,  TNewItem15 , object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14,  TNewItem15> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14>.By<TNewItem15>(Expression<Func<TProjectElement, TNewItem15>> projection)
            {
                return ByExpression<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14,  TNewItem15 , object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15,  TNewItem16> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15>.By<TNewItem16>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem16>> projection)
            {
                return ByLambda<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15,  TNewItem16 >(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15,  TNewItem16> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement , TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15>.By<TNewItem16>(Expression<Func<TProjectElement, TNewItem16>> projection)
            {
                return ByExpression<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15,  TNewItem16 >(projection);
            }
        }
    }
}

