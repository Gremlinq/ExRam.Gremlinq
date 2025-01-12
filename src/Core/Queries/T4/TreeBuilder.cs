#pragma warning disable CS0109 // Member does not hide an inherited member; new keyword is not required
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public interface ITreeBuilder
    {
        ITreeBuilder<TNewItem> Of<TNewItem>() where TNewItem : notnull;
    }

    public interface ITreeBuilder<TItem1>
            : ITreeBuilderResult<Tree<TItem1>>
    
            where TItem1 : notnull
    
    {
        ITreeBuilder<TItem1, TItem2> Of<TItem2>() where TItem2 : notnull;
    }

    public interface ITreeBuilder<TItem1, TItem2>
            : ITreeBuilderResult<Tree<TItem1, Tree<TItem2>>>
    
            where TItem1 : notnull
            where TItem2 : notnull
    
    {
        ITreeBuilder<TItem1, TItem2, TItem3> Of<TItem3>() where TItem3 : notnull;
    }

    public interface ITreeBuilder<TItem1, TItem2, TItem3>
            : ITreeBuilderResult<Tree<TItem1, Tree<TItem2, Tree<TItem3>>>>
    
            where TItem1 : notnull
            where TItem2 : notnull
            where TItem3 : notnull
    
    {
        ITreeBuilder<TItem1, TItem2, TItem3, TItem4> Of<TItem4>() where TItem4 : notnull;
    }

    public interface ITreeBuilder<TItem1, TItem2, TItem3, TItem4>
            : ITreeBuilderResult<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4>>>>>
    
            where TItem1 : notnull
            where TItem2 : notnull
            where TItem3 : notnull
            where TItem4 : notnull
    
    {
        ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5> Of<TItem5>() where TItem5 : notnull;
    }

    public interface ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5>
            : ITreeBuilderResult<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5>>>>>>
    
            where TItem1 : notnull
            where TItem2 : notnull
            where TItem3 : notnull
            where TItem4 : notnull
            where TItem5 : notnull
    
    {
        ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6> Of<TItem6>() where TItem6 : notnull;
    }

    public interface ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6>
            : ITreeBuilderResult<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6>>>>>>>
    
            where TItem1 : notnull
            where TItem2 : notnull
            where TItem3 : notnull
            where TItem4 : notnull
            where TItem5 : notnull
            where TItem6 : notnull
    
    {
        ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7> Of<TItem7>() where TItem7 : notnull;
    }

    public interface ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7>
            : ITreeBuilderResult<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7>>>>>>>>
    
            where TItem1 : notnull
            where TItem2 : notnull
            where TItem3 : notnull
            where TItem4 : notnull
            where TItem5 : notnull
            where TItem6 : notnull
            where TItem7 : notnull
    
    {
        ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8> Of<TItem8>() where TItem8 : notnull;
    }

    public interface ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8>
            : ITreeBuilderResult<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8>>>>>>>>>
    
            where TItem1 : notnull
            where TItem2 : notnull
            where TItem3 : notnull
            where TItem4 : notnull
            where TItem5 : notnull
            where TItem6 : notnull
            where TItem7 : notnull
            where TItem8 : notnull
    
    {
        ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9> Of<TItem9>() where TItem9 : notnull;
    }

    public interface ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9>
            : ITreeBuilderResult<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9>>>>>>>>>>
    
            where TItem1 : notnull
            where TItem2 : notnull
            where TItem3 : notnull
            where TItem4 : notnull
            where TItem5 : notnull
            where TItem6 : notnull
            where TItem7 : notnull
            where TItem8 : notnull
            where TItem9 : notnull
    
    {
        ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10> Of<TItem10>() where TItem10 : notnull;
    }

    public interface ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10>
            : ITreeBuilderResult<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9, Tree<TItem10>>>>>>>>>>>
    
            where TItem1 : notnull
            where TItem2 : notnull
            where TItem3 : notnull
            where TItem4 : notnull
            where TItem5 : notnull
            where TItem6 : notnull
            where TItem7 : notnull
            where TItem8 : notnull
            where TItem9 : notnull
            where TItem10 : notnull
    
    {
        ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11> Of<TItem11>() where TItem11 : notnull;
    }

    public interface ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11>
            : ITreeBuilderResult<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9, Tree<TItem10, Tree<TItem11>>>>>>>>>>>>
    
            where TItem1 : notnull
            where TItem2 : notnull
            where TItem3 : notnull
            where TItem4 : notnull
            where TItem5 : notnull
            where TItem6 : notnull
            where TItem7 : notnull
            where TItem8 : notnull
            where TItem9 : notnull
            where TItem10 : notnull
            where TItem11 : notnull
    
    {
        ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12> Of<TItem12>() where TItem12 : notnull;
    }

    public interface ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12>
            : ITreeBuilderResult<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9, Tree<TItem10, Tree<TItem11, Tree<TItem12>>>>>>>>>>>>>
    
            where TItem1 : notnull
            where TItem2 : notnull
            where TItem3 : notnull
            where TItem4 : notnull
            where TItem5 : notnull
            where TItem6 : notnull
            where TItem7 : notnull
            where TItem8 : notnull
            where TItem9 : notnull
            where TItem10 : notnull
            where TItem11 : notnull
            where TItem12 : notnull
    
    {
        ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13> Of<TItem13>() where TItem13 : notnull;
    }

    public interface ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13>
            : ITreeBuilderResult<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9, Tree<TItem10, Tree<TItem11, Tree<TItem12, Tree<TItem13>>>>>>>>>>>>>>
    
            where TItem1 : notnull
            where TItem2 : notnull
            where TItem3 : notnull
            where TItem4 : notnull
            where TItem5 : notnull
            where TItem6 : notnull
            where TItem7 : notnull
            where TItem8 : notnull
            where TItem9 : notnull
            where TItem10 : notnull
            where TItem11 : notnull
            where TItem12 : notnull
            where TItem13 : notnull
    
    {
        ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14> Of<TItem14>() where TItem14 : notnull;
    }

    public interface ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14>
            : ITreeBuilderResult<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9, Tree<TItem10, Tree<TItem11, Tree<TItem12, Tree<TItem13, Tree<TItem14>>>>>>>>>>>>>>>
    
            where TItem1 : notnull
            where TItem2 : notnull
            where TItem3 : notnull
            where TItem4 : notnull
            where TItem5 : notnull
            where TItem6 : notnull
            where TItem7 : notnull
            where TItem8 : notnull
            where TItem9 : notnull
            where TItem10 : notnull
            where TItem11 : notnull
            where TItem12 : notnull
            where TItem13 : notnull
            where TItem14 : notnull
    
    {
        ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15> Of<TItem15>() where TItem15 : notnull;
    }

    public interface ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15>
            : ITreeBuilderResult<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9, Tree<TItem10, Tree<TItem11, Tree<TItem12, Tree<TItem13, Tree<TItem14, Tree<TItem15>>>>>>>>>>>>>>>>
    
            where TItem1 : notnull
            where TItem2 : notnull
            where TItem3 : notnull
            where TItem4 : notnull
            where TItem5 : notnull
            where TItem6 : notnull
            where TItem7 : notnull
            where TItem8 : notnull
            where TItem9 : notnull
            where TItem10 : notnull
            where TItem11 : notnull
            where TItem12 : notnull
            where TItem13 : notnull
            where TItem14 : notnull
            where TItem15 : notnull
    
    {
        ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16> Of<TItem16>() where TItem16 : notnull;
    }

    public interface ITreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16>
            : ITreeBuilderResult<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9, Tree<TItem10, Tree<TItem11, Tree<TItem12, Tree<TItem13, Tree<TItem14, Tree<TItem15, Tree<TItem16>>>>>>>>>>>>>>>>>
    
            where TItem1 : notnull
            where TItem2 : notnull
            where TItem3 : notnull
            where TItem4 : notnull
            where TItem5 : notnull
            where TItem6 : notnull
            where TItem7 : notnull
            where TItem8 : notnull
            where TItem9 : notnull
            where TItem10 : notnull
            where TItem11 : notnull
            where TItem12 : notnull
            where TItem13 : notnull
            where TItem14 : notnull
            where TItem15 : notnull
            where TItem16 : notnull
    
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

                            where TItem1 : notnull
                            where TItem2 : notnull
                            where TItem3 : notnull
                            where TItem4 : notnull
                            where TItem5 : notnull
                            where TItem6 : notnull
                            where TItem7 : notnull
                            where TItem8 : notnull
                            where TItem9 : notnull
                            where TItem10 : notnull
                            where TItem11 : notnull
                            where TItem12 : notnull
                            where TItem13 : notnull
                            where TItem14 : notnull
                            where TItem15 : notnull
                            where TItem16 : notnull
            
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

            IGremlinQuery<Tree<TItem1>> ITreeBuilderResult<Tree<TItem1>>.Build() => throw new NotImplementedException();// Build<IGremlinQuery<Tree<TItem1>>>();
            IGremlinQuery<Tree<TItem1, Tree<TItem2>>> ITreeBuilderResult<Tree<TItem1, Tree<TItem2>>>.Build() => throw new NotImplementedException();// Build<IGremlinQuery<Tree<TItem1, Tree<TItem2>>>>();
            IGremlinQuery<Tree<TItem1, Tree<TItem2, Tree<TItem3>>>> ITreeBuilderResult<Tree<TItem1, Tree<TItem2, Tree<TItem3>>>>.Build() => throw new NotImplementedException();// Build<IGremlinQuery<Tree<TItem1, Tree<TItem2, Tree<TItem3>>>>>();
            IGremlinQuery<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4>>>>> ITreeBuilderResult<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4>>>>>.Build() => throw new NotImplementedException();// Build<IGremlinQuery<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4>>>>>>();
            IGremlinQuery<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5>>>>>> ITreeBuilderResult<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5>>>>>>.Build() => throw new NotImplementedException();// Build<IGremlinQuery<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5>>>>>>>();
            IGremlinQuery<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6>>>>>>> ITreeBuilderResult<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6>>>>>>>.Build() => throw new NotImplementedException();// Build<IGremlinQuery<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6>>>>>>>>();
            IGremlinQuery<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7>>>>>>>> ITreeBuilderResult<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7>>>>>>>>.Build() => throw new NotImplementedException();// Build<IGremlinQuery<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7>>>>>>>>>();
            IGremlinQuery<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8>>>>>>>>> ITreeBuilderResult<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8>>>>>>>>>.Build() => throw new NotImplementedException();// Build<IGremlinQuery<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8>>>>>>>>>>();
            IGremlinQuery<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9>>>>>>>>>> ITreeBuilderResult<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9>>>>>>>>>>.Build() => throw new NotImplementedException();// Build<IGremlinQuery<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9>>>>>>>>>>>();
            IGremlinQuery<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9, Tree<TItem10>>>>>>>>>>> ITreeBuilderResult<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9, Tree<TItem10>>>>>>>>>>>.Build() => throw new NotImplementedException();// Build<IGremlinQuery<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9, Tree<TItem10>>>>>>>>>>>>();
            IGremlinQuery<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9, Tree<TItem10, Tree<TItem11>>>>>>>>>>>> ITreeBuilderResult<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9, Tree<TItem10, Tree<TItem11>>>>>>>>>>>>.Build() => throw new NotImplementedException();// Build<IGremlinQuery<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9, Tree<TItem10, Tree<TItem11>>>>>>>>>>>>>();
            IGremlinQuery<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9, Tree<TItem10, Tree<TItem11, Tree<TItem12>>>>>>>>>>>>> ITreeBuilderResult<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9, Tree<TItem10, Tree<TItem11, Tree<TItem12>>>>>>>>>>>>>.Build() => throw new NotImplementedException();// Build<IGremlinQuery<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9, Tree<TItem10, Tree<TItem11, Tree<TItem12>>>>>>>>>>>>>>();
            IGremlinQuery<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9, Tree<TItem10, Tree<TItem11, Tree<TItem12, Tree<TItem13>>>>>>>>>>>>>> ITreeBuilderResult<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9, Tree<TItem10, Tree<TItem11, Tree<TItem12, Tree<TItem13>>>>>>>>>>>>>>.Build() => throw new NotImplementedException();// Build<IGremlinQuery<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9, Tree<TItem10, Tree<TItem11, Tree<TItem12, Tree<TItem13>>>>>>>>>>>>>>>();
            IGremlinQuery<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9, Tree<TItem10, Tree<TItem11, Tree<TItem12, Tree<TItem13, Tree<TItem14>>>>>>>>>>>>>>> ITreeBuilderResult<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9, Tree<TItem10, Tree<TItem11, Tree<TItem12, Tree<TItem13, Tree<TItem14>>>>>>>>>>>>>>>.Build() => throw new NotImplementedException();// Build<IGremlinQuery<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9, Tree<TItem10, Tree<TItem11, Tree<TItem12, Tree<TItem13, Tree<TItem14>>>>>>>>>>>>>>>>();
            IGremlinQuery<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9, Tree<TItem10, Tree<TItem11, Tree<TItem12, Tree<TItem13, Tree<TItem14, Tree<TItem15>>>>>>>>>>>>>>>> ITreeBuilderResult<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9, Tree<TItem10, Tree<TItem11, Tree<TItem12, Tree<TItem13, Tree<TItem14, Tree<TItem15>>>>>>>>>>>>>>>>.Build() => throw new NotImplementedException();// Build<IGremlinQuery<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9, Tree<TItem10, Tree<TItem11, Tree<TItem12, Tree<TItem13, Tree<TItem14, Tree<TItem15>>>>>>>>>>>>>>>>>();
            IGremlinQuery<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9, Tree<TItem10, Tree<TItem11, Tree<TItem12, Tree<TItem13, Tree<TItem14, Tree<TItem15, Tree<TItem16>>>>>>>>>>>>>>>>> ITreeBuilderResult<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9, Tree<TItem10, Tree<TItem11, Tree<TItem12, Tree<TItem13, Tree<TItem14, Tree<TItem15, Tree<TItem16>>>>>>>>>>>>>>>>>.Build() => throw new NotImplementedException();// Build<IGremlinQuery<Tree<TItem1, Tree<TItem2, Tree<TItem3, Tree<TItem4, Tree<TItem5, Tree<TItem6, Tree<TItem7, Tree<TItem8, Tree<TItem9, Tree<TItem10, Tree<TItem11, Tree<TItem12, Tree<TItem13, Tree<TItem14, Tree<TItem15, Tree<TItem16>>>>>>>>>>>>>>>>>>();
        }
    }
}

