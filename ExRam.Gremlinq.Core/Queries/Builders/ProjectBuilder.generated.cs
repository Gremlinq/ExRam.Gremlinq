#pragma warning disable CS0109 // Member does not hide an inherited member; new keyword is not required
using System;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2> : IProjectResult<(TItem1, TItem2)>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3> By<TItem3>(Func<TSourceQuery, IGremlinQueryBase<TItem3>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3> : IProjectResult<(TItem1, TItem2, TItem3)>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4> By<TItem4>(Func<TSourceQuery, IGremlinQueryBase<TItem4>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4> : IProjectResult<(TItem1, TItem2, TItem3, TItem4)>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5> By<TItem5>(Func<TSourceQuery, IGremlinQueryBase<TItem5>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5> : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5)>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6> By<TItem6>(Func<TSourceQuery, IGremlinQueryBase<TItem6>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6> : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6)>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7> By<TItem7>(Func<TSourceQuery, IGremlinQueryBase<TItem7>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7> : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7)>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8> By<TItem8>(Func<TSourceQuery, IGremlinQueryBase<TItem8>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8> : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8)>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9> By<TItem9>(Func<TSourceQuery, IGremlinQueryBase<TItem9>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9> : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9)>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10> By<TItem10>(Func<TSourceQuery, IGremlinQueryBase<TItem10>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10> : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10)>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11> By<TItem11>(Func<TSourceQuery, IGremlinQueryBase<TItem11>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11> : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11)>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12> By<TItem12>(Func<TSourceQuery, IGremlinQueryBase<TItem12>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12> : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12)>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13> By<TItem13>(Func<TSourceQuery, IGremlinQueryBase<TItem13>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13> : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13)>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14> By<TItem14>(Func<TSourceQuery, IGremlinQueryBase<TItem14>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14> : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14)>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15> By<TItem15>(Func<TSourceQuery, IGremlinQueryBase<TItem15>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15> : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15)>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16> By<TItem16>(Func<TSourceQuery, IGremlinQueryBase<TItem16>> projection);
    }

    public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16> : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16)>
        where TSourceQuery : IGremlinQueryBase
    {
    }



    partial class GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>
    {
        private sealed partial class ProjectBuilderImpl<TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16> :
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement>,
            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1>
        {

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TNewItem2> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1>.By<TNewItem2>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem2>> projection)
            {
                return By<TItem1, TNewItem2, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TNewItem3> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2>.By<TNewItem3>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem3>> projection)
            {
                return By<TItem1, TItem2, TNewItem3, object, object, object, object, object, object, object, object, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TNewItem4> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3>.By<TNewItem4>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem4>> projection)
            {
                return By<TItem1, TItem2, TItem3, TNewItem4, object, object, object, object, object, object, object, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TNewItem5> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4>.By<TNewItem5>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem5>> projection)
            {
                return By<TItem1, TItem2, TItem3, TItem4, TNewItem5, object, object, object, object, object, object, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TNewItem6> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5>.By<TNewItem6>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem6>> projection)
            {
                return By<TItem1, TItem2, TItem3, TItem4, TItem5, TNewItem6, object, object, object, object, object, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TNewItem7> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6>.By<TNewItem7>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem7>> projection)
            {
                return By<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TNewItem7, object, object, object, object, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TNewItem8> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7>.By<TNewItem8>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem8>> projection)
            {
                return By<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TNewItem8, object, object, object, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TNewItem9> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8>.By<TNewItem9>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem9>> projection)
            {
                return By<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TNewItem9, object, object, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TNewItem10> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9>.By<TNewItem10>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem10>> projection)
            {
                return By<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TNewItem10, object, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TNewItem11> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10>.By<TNewItem11>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem11>> projection)
            {
                return By<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TNewItem11, object, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TNewItem12> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11>.By<TNewItem12>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem12>> projection)
            {
                return By<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TNewItem12, object, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TNewItem13> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12>.By<TNewItem13>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem13>> projection)
            {
                return By<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TNewItem13, object, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TNewItem14> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13>.By<TNewItem14>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem14>> projection)
            {
                return By<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TNewItem14, object, object>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TNewItem15> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14>.By<TNewItem15>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem15>> projection)
            {
                return By<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TNewItem15, object>(projection);
            }
        }
    }
}

