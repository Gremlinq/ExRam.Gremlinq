#pragma warning disable CS0109 // Member does not hide an inherited member; new keyword is not required
using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    partial interface IGremlinQueryBase
    {
        IValueGremlinQuery<(T1, T2)> Select<T1, T2>(StepLabel<T1> label1, StepLabel<T2> label2);
        IValueGremlinQuery<(T1, T2, T3)> Select<T1, T2, T3>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3);
        IValueGremlinQuery<(T1, T2, T3, T4)> Select<T1, T2, T3, T4>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4);
        IValueGremlinQuery<(T1, T2, T3, T4, T5)> Select<T1, T2, T3, T4, T5>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5);
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6)> Select<T1, T2, T3, T4, T5, T6>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6);
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7)> Select<T1, T2, T3, T4, T5, T6, T7>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7);
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8)> Select<T1, T2, T3, T4, T5, T6, T7, T8>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8);
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9);
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10);
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11);
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12);
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13);
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13, StepLabel<T14> label14);
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13, StepLabel<T14> label14, StepLabel<T15> label15);
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13, StepLabel<T14> label14, StepLabel<T15> label15, StepLabel<T16> label16);
    }


    public partial interface IGremlinQueryBase<TElement>
    {
        new IGremlinQuery<TElement> As(StepLabel<TElement> stepLabel);
    }
    public partial interface IValueGremlinQueryBase<TElement>
    {
        new IValueGremlinQuery<TElement> As(StepLabel<TElement> stepLabel);
    }
    public partial interface IArrayGremlinQueryBase<TArray, TQuery>
    {
        new IArrayGremlinQuery<TArray, TQuery> As(StepLabel<TArray> stepLabel);
    }
    public partial interface IElementGremlinQueryBase<TElement>
    {
        new IElementGremlinQuery<TElement> As(StepLabel<TElement> stepLabel);
    }
    public partial interface IEdgeOrVertexGremlinQueryBase<TElement>
    {
        new IEdgeOrVertexGremlinQuery<TElement> As(StepLabel<TElement> stepLabel);
    }
    public partial interface IVertexGremlinQueryBase<TVertex>
    {
        new IVertexGremlinQuery<TVertex> As(StepLabel<TVertex> stepLabel);
    }
    public partial interface IEdgeGremlinQueryBase<TEdge>
    {
        new IEdgeGremlinQuery<TEdge> As(StepLabel<TEdge> stepLabel);
    }
    public partial interface IInOrOutEdgeGremlinQueryBase<TEdge, TAdjacentVertex>
    {
        new IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex> As(StepLabel<TEdge> stepLabel);
    }
    public partial interface IBothEdgeGremlinQueryBase<TEdge, TOutVertex, TInVertex>
    {
        new IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> As(StepLabel<TEdge> stepLabel);
    }
    public partial interface IInEdgeGremlinQueryBase<TEdge, TInVertex>
    {
        new IInEdgeGremlinQuery<TEdge, TInVertex> As(StepLabel<TEdge> stepLabel);
    }
    public partial interface IOutEdgeGremlinQueryBase<TEdge, TOutVertex>
    {
        new IOutEdgeGremlinQuery<TEdge, TOutVertex> As(StepLabel<TEdge> stepLabel);
    }
    public partial interface IVertexPropertyGremlinQueryBase<TProperty, TValue>
    {
        new IVertexPropertyGremlinQuery<TProperty, TValue> As(StepLabel<TProperty> stepLabel);
    }
    public partial interface IVertexPropertyGremlinQueryBase<TProperty, TValue, TMeta>
    {
        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> As(StepLabel<TProperty> stepLabel);
    }
    public partial interface IPropertyGremlinQueryBase<TElement>
    {
        new IPropertyGremlinQuery<TElement> As(StepLabel<TElement> stepLabel);
    }


    public partial interface IGremlinQueryBase
    {
        new IGremlinQuery<TResult> Cast<TResult>();
    }
    public partial interface IValueGremlinQueryBase
    {
        new IValueGremlinQuery<TResult> Cast<TResult>();
    }
    public partial interface IElementGremlinQueryBase
    {
        new IElementGremlinQuery<TResult> Cast<TResult>();
    }
    public partial interface IEdgeOrVertexGremlinQueryBase
    {
        new IEdgeOrVertexGremlinQuery<TResult> Cast<TResult>();
    }
    public partial interface IVertexGremlinQueryBase
    {
        new IVertexGremlinQuery<TResult> Cast<TResult>();
    }
    public partial interface IEdgeGremlinQueryBase
    {
        new IEdgeGremlinQuery<TResult> Cast<TResult>();
    }
    public partial interface IPropertyGremlinQueryBase
    {
        new IPropertyGremlinQuery<TResult> Cast<TResult>();
    }


    public partial interface IVertexGremlinQueryBase
    {
        new IVertexGremlinQuery<TTarget> OfType<TTarget>();
    }
    public partial interface IEdgeGremlinQueryBase
    {
        new IEdgeGremlinQuery<TTarget> OfType<TTarget>();
    }

}

#pragma warning restore CS0109 // Member does not hide an inherited member; new keyword is not required
