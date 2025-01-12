// ReSharper disable ArrangeThisQualifier
using System.Collections.Immutable;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    partial class GremlinQuery<T1, T2, T3, T4>
    {
        private sealed partial class TreeBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16>
        {
            private readonly GremlinQuery<T1, T2, T3, T4> _sourceQuery;

            public TreeBuilder(GremlinQuery<T1, T2, T3, T4> sourceQuery)
            {
                _sourceQuery = sourceQuery;
            }

            private IGremlinQuery<TTree> Build<TTree>() 
            {
                return _sourceQuery
                    .Continue()
                    .Build(
                        static (builder, state) =>
                        {
                            return builder
                                .As<IGremlinQuery<TTree>>()
                                .Build();
                        },
                        0);
            }
        }
    }
}
