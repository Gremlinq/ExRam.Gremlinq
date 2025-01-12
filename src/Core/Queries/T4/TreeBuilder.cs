#pragma warning disable CS0109 // Member does not hide an inherited member; new keyword is not required
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public interface ITreeBuilder
    {
        ITreeBuilder<TItem1> Of<TItem1>();
    }

    public interface ITreeBuilder<TItem1>
        //: ITreeResult<(TItem1)>
        {
        ITreeBuilder<TItem1, TItem2> Of<TItem2>();
    }

    public interface ITreeBuilder<TItem1, TItem2>
        //: ITreeResult<(TItem1, TItem2)>
        {
        ITreeBuilder<TItem1, TItem2, TItem3> Of<TItem3>();
    }

    public interface ITreeBuilder<TItem1, TItem2, TItem3>
        //: ITreeResult<(TItem1, TItem2, TItem3)>
        {
        ITreeBuilder<TItem1, TItem2, TItem3, TItem4> Of<TItem4>();
    }

    public interface ITreeBuilder<TItem1, TItem2, TItem3, TItem4>
        //: ITreeResult<(TItem1, TItem2, TItem3, TItem4)>
        {
        ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5> Of<TItem5>();
    }

    public interface ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5>
        //: ITreeResult<(TItem1, TItem2, TItem3, TItem4, TItem5)>
        {
        ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6> Of<TItem6>();
    }

    public interface ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6>
        //: ITreeResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6)>
        {
        ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7> Of<TItem7>();
    }

    public interface ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7>
        //: ITreeResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7)>
        {
        ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8> Of<TItem8>();
    }

    public interface ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8>
        //: ITreeResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8)>
        {
        ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9> Of<TItem9>();
    }

    public interface ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9>
        //: ITreeResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9)>
        {
        ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10> Of<TItem10>();
    }

    public interface ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10>
        //: ITreeResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10)>
        {
        ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11> Of<TItem11>();
    }

    public interface ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11>
        //: ITreeResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11)>
        {
        ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12> Of<TItem12>();
    }

    public interface ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12>
        //: ITreeResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12)>
        {
        ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13> Of<TItem13>();
    }

    public interface ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13>
        //: ITreeResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13)>
        {
        ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14> Of<TItem14>();
    }

    public interface ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14>
        //: ITreeResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14)>
        {
        ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15> Of<TItem15>();
    }

    public interface ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15>
        //: ITreeResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15)>
        {
        ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16> Of<TItem16>();
    }

    public interface ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16>
        //: ITreeResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16)>
        {
    }



    partial class GremlinQuery<T1, T2, T3, T4>
    {
        private sealed partial class TreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16> :
            ITreeBuilder<TItem1>,
            ITreeBuilder<TItem1, TItem2>,
            ITreeBuilder<TItem1, TItem2, TItem3>,
            ITreeBuilder<TItem1, TItem2, TItem3, TItem4>,
            ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5>,
            ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6>,
            ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7>,
            ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8>,
            ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9>,
            ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10>,
            ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11>,
            ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12>,
            ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13>,
            ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14>,
            ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15>,
            ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16>,
            ITreeBuilder

        {

            ITreeBuilder<TItem1,  TNewItem2> ITreeBuilder<TItem1>.Of<TNewItem2>()
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TItem1, TItem2,  TNewItem3> ITreeBuilder<TItem1, TItem2>.Of<TNewItem3>()
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TItem1, TItem2, TItem3,  TNewItem4> ITreeBuilder<TItem1, TItem2, TItem3>.Of<TNewItem4>()
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TItem1, TItem2, TItem3, TItem4,  TNewItem5> ITreeBuilder<TItem1, TItem2, TItem3, TItem4>.Of<TNewItem5>()
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5,  TNewItem6> ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5>.Of<TNewItem6>()
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6,  TNewItem7> ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6>.Of<TNewItem7>()
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7,  TNewItem8> ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7>.Of<TNewItem8>()
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8,  TNewItem9> ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8>.Of<TNewItem9>()
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9,  TNewItem10> ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9>.Of<TNewItem10>()
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10,  TNewItem11> ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10>.Of<TNewItem11>()
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11,  TNewItem12> ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11>.Of<TNewItem12>()
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12,  TNewItem13> ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12>.Of<TNewItem13>()
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13,  TNewItem14> ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13>.Of<TNewItem14>()
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14,  TNewItem15> ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14>.Of<TNewItem15>()
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15,  TNewItem16> ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15>.Of<TNewItem16>()
            {
                throw new NotImplementedException();
            }

            ITreeBuilder<TNewItem> ITreeBuilder.Of<TNewItem>()
            {
                throw new NotImplementedException();
            }

            //IMapGremlinQuery<(TItem1, TItem2)> IProjectTupleResult<(TItem1, TItem2)>.Build() => Build<IMapGremlinQuery<(TItem1, TItem2)>>();
            //IMapGremlinQuery<(TItem1, TItem2, TItem3)> IProjectTupleResult<(TItem1, TItem2, TItem3)>.Build() => Build<IMapGremlinQuery<(TItem1, TItem2, TItem3)>>();
            //IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4)> IProjectTupleResult<(TItem1, TItem2, TItem3, TItem4)>.Build() => Build<IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4)>>();
            //IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5)> IProjectTupleResult<(TItem1, TItem2, TItem3, TItem4, TItem5)>.Build() => Build<IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5)>>();
            //IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6)> IProjectTupleResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6)>.Build() => Build<IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6)>>();
            //IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7)> IProjectTupleResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7)>.Build() => Build<IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7)>>();
            //IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8)> IProjectTupleResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8)>.Build() => Build<IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8)>>();
            //IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9)> IProjectTupleResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9)>.Build() => Build<IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9)>>();
            //IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10)> IProjectTupleResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10)>.Build() => Build<IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10)>>();
            //IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11)> IProjectTupleResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11)>.Build() => Build<IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11)>>();
            //IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12)> IProjectTupleResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12)>.Build() => Build<IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12)>>();
            //IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13)> IProjectTupleResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13)>.Build() => Build<IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13)>>();
            //IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14)> IProjectTupleResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14)>.Build() => Build<IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14)>>();
            //IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15)> IProjectTupleResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15)>.Build() => Build<IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15)>>();
            //IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16)> IProjectTupleResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16)>.Build() => Build<IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16)>>();
        }
    }
}

