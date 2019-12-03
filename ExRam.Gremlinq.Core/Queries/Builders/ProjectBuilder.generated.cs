#pragma warning disable CS0109 // Member does not hide an inherited member; new keyword is not required
using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using Gremlin.Net.Process.Traversal;
using System.Collections.Immutable;
using System.Linq.Expressions;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public static partial class ProjectBuilder
    {
        private sealed partial class ProjectBuilderImpl<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16> :
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2>,
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3>,
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4>,
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5>,
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6>,
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7>,
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8>,
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9>,
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10>,
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11>,
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12>,
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13>,
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14>,
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15>,
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16>,
            IProjectTupleBuilder<TSourceQuery, TElement>,
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1>
        {
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TNewItem2> IProjectTupleBuilder<TSourceQuery, TElement, TItem1>.By<TNewItem2>(Func<TSourceQuery, IGremlinQuery<TNewItem2>> projection)
            {
                return By<TItem1, TNewItem2, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit>(projection);
            }
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TNewItem3> IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2>.By<TNewItem3>(Func<TSourceQuery, IGremlinQuery<TNewItem3>> projection)
            {
                return By<TItem1, TItem2, TNewItem3, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit>(projection);
            }
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TNewItem4> IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3>.By<TNewItem4>(Func<TSourceQuery, IGremlinQuery<TNewItem4>> projection)
            {
                return By<TItem1, TItem2, TItem3, TNewItem4, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit>(projection);
            }
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TNewItem5> IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4>.By<TNewItem5>(Func<TSourceQuery, IGremlinQuery<TNewItem5>> projection)
            {
                return By<TItem1, TItem2, TItem3, TItem4, TNewItem5, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit>(projection);
            }
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TNewItem6> IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5>.By<TNewItem6>(Func<TSourceQuery, IGremlinQuery<TNewItem6>> projection)
            {
                return By<TItem1, TItem2, TItem3, TItem4, TItem5, TNewItem6, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit>(projection);
            }
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TNewItem7> IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6>.By<TNewItem7>(Func<TSourceQuery, IGremlinQuery<TNewItem7>> projection)
            {
                return By<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TNewItem7, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit>(projection);
            }
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TNewItem8> IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7>.By<TNewItem8>(Func<TSourceQuery, IGremlinQuery<TNewItem8>> projection)
            {
                return By<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TNewItem8, Unit, Unit, Unit, Unit, Unit, Unit, Unit, Unit>(projection);
            }
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TNewItem9> IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8>.By<TNewItem9>(Func<TSourceQuery, IGremlinQuery<TNewItem9>> projection)
            {
                return By<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TNewItem9, Unit, Unit, Unit, Unit, Unit, Unit, Unit>(projection);
            }
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TNewItem10> IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9>.By<TNewItem10>(Func<TSourceQuery, IGremlinQuery<TNewItem10>> projection)
            {
                return By<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TNewItem10, Unit, Unit, Unit, Unit, Unit, Unit>(projection);
            }
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TNewItem11> IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10>.By<TNewItem11>(Func<TSourceQuery, IGremlinQuery<TNewItem11>> projection)
            {
                return By<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TNewItem11, Unit, Unit, Unit, Unit, Unit>(projection);
            }
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TNewItem12> IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11>.By<TNewItem12>(Func<TSourceQuery, IGremlinQuery<TNewItem12>> projection)
            {
                return By<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TNewItem12, Unit, Unit, Unit, Unit>(projection);
            }
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TNewItem13> IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12>.By<TNewItem13>(Func<TSourceQuery, IGremlinQuery<TNewItem13>> projection)
            {
                return By<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TNewItem13, Unit, Unit, Unit>(projection);
            }
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TNewItem14> IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13>.By<TNewItem14>(Func<TSourceQuery, IGremlinQuery<TNewItem14>> projection)
            {
                return By<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TNewItem14, Unit, Unit>(projection);
            }
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TNewItem15> IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14>.By<TNewItem15>(Func<TSourceQuery, IGremlinQuery<TNewItem15>> projection)
            {
                return By<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TNewItem15, Unit>(projection);
            }
        }
    }
}

