#nullable enable
using System.Linq.Expressions;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    partial class GremlinQuery<T1, T2, T3, T4, T5, T6>
    {
        IMapGremlinQuery<(TItem1, TItem2)> IGremlinQueryBase.Select<TItem1, TItem2>(StepLabel<TItem1> label1, StepLabel<TItem2> label2) => Project<(TItem1, TItem2)>(p => p.ToTuple().By(__ => __.Select(label1)).By(__ => __.Select(label2)));

        IMapGremlinQuery<(TItem1, TItem2)> IMapGremlinQueryBase<T1>.Select<TItem1, TItem2>(Expression<Func<T1, TItem1>> projection1, Expression<Func<T1, TItem2>> projection2) => Select<IMapGremlinQuery<(TItem1, TItem2)>>(projection1, projection2);
        IMapGremlinQuery<(TItem1, TItem2, TItem3)> IGremlinQueryBase.Select<TItem1, TItem2, TItem3>(StepLabel<TItem1> label1, StepLabel<TItem2> label2, StepLabel<TItem3> label3) => Project<(TItem1, TItem2, TItem3)>(p => p.ToTuple().By(__ => __.Select(label1)).By(__ => __.Select(label2)).By(__ => __.Select(label3)));

        IMapGremlinQuery<(TItem1, TItem2, TItem3)> IMapGremlinQueryBase<T1>.Select<TItem1, TItem2, TItem3>(Expression<Func<T1, TItem1>> projection1, Expression<Func<T1, TItem2>> projection2, Expression<Func<T1, TItem3>> projection3) => Select<IMapGremlinQuery<(TItem1, TItem2, TItem3)>>(projection1, projection2, projection3);
        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4)> IGremlinQueryBase.Select<TItem1, TItem2, TItem3, TItem4>(StepLabel<TItem1> label1, StepLabel<TItem2> label2, StepLabel<TItem3> label3, StepLabel<TItem4> label4) => Project<(TItem1, TItem2, TItem3, TItem4)>(p => p.ToTuple().By(__ => __.Select(label1)).By(__ => __.Select(label2)).By(__ => __.Select(label3)).By(__ => __.Select(label4)));

        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4)> IMapGremlinQueryBase<T1>.Select<TItem1, TItem2, TItem3, TItem4>(Expression<Func<T1, TItem1>> projection1, Expression<Func<T1, TItem2>> projection2, Expression<Func<T1, TItem3>> projection3, Expression<Func<T1, TItem4>> projection4) => Select<IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4)>>(projection1, projection2, projection3, projection4);
        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5)> IGremlinQueryBase.Select<TItem1, TItem2, TItem3, TItem4, TItem5>(StepLabel<TItem1> label1, StepLabel<TItem2> label2, StepLabel<TItem3> label3, StepLabel<TItem4> label4, StepLabel<TItem5> label5) => Project<(TItem1, TItem2, TItem3, TItem4, TItem5)>(p => p.ToTuple().By(__ => __.Select(label1)).By(__ => __.Select(label2)).By(__ => __.Select(label3)).By(__ => __.Select(label4)).By(__ => __.Select(label5)));

        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5)> IMapGremlinQueryBase<T1>.Select<TItem1, TItem2, TItem3, TItem4, TItem5>(Expression<Func<T1, TItem1>> projection1, Expression<Func<T1, TItem2>> projection2, Expression<Func<T1, TItem3>> projection3, Expression<Func<T1, TItem4>> projection4, Expression<Func<T1, TItem5>> projection5) => Select<IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5)>>(projection1, projection2, projection3, projection4, projection5);
        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6)> IGremlinQueryBase.Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6>(StepLabel<TItem1> label1, StepLabel<TItem2> label2, StepLabel<TItem3> label3, StepLabel<TItem4> label4, StepLabel<TItem5> label5, StepLabel<TItem6> label6) => Project<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6)>(p => p.ToTuple().By(__ => __.Select(label1)).By(__ => __.Select(label2)).By(__ => __.Select(label3)).By(__ => __.Select(label4)).By(__ => __.Select(label5)).By(__ => __.Select(label6)));

        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6)> IMapGremlinQueryBase<T1>.Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6>(Expression<Func<T1, TItem1>> projection1, Expression<Func<T1, TItem2>> projection2, Expression<Func<T1, TItem3>> projection3, Expression<Func<T1, TItem4>> projection4, Expression<Func<T1, TItem5>> projection5, Expression<Func<T1, TItem6>> projection6) => Select<IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6)>>(projection1, projection2, projection3, projection4, projection5, projection6);
        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7)> IGremlinQueryBase.Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7>(StepLabel<TItem1> label1, StepLabel<TItem2> label2, StepLabel<TItem3> label3, StepLabel<TItem4> label4, StepLabel<TItem5> label5, StepLabel<TItem6> label6, StepLabel<TItem7> label7) => Project<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7)>(p => p.ToTuple().By(__ => __.Select(label1)).By(__ => __.Select(label2)).By(__ => __.Select(label3)).By(__ => __.Select(label4)).By(__ => __.Select(label5)).By(__ => __.Select(label6)).By(__ => __.Select(label7)));

        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7)> IMapGremlinQueryBase<T1>.Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7>(Expression<Func<T1, TItem1>> projection1, Expression<Func<T1, TItem2>> projection2, Expression<Func<T1, TItem3>> projection3, Expression<Func<T1, TItem4>> projection4, Expression<Func<T1, TItem5>> projection5, Expression<Func<T1, TItem6>> projection6, Expression<Func<T1, TItem7>> projection7) => Select<IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7)>>(projection1, projection2, projection3, projection4, projection5, projection6, projection7);
        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8)> IGremlinQueryBase.Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8>(StepLabel<TItem1> label1, StepLabel<TItem2> label2, StepLabel<TItem3> label3, StepLabel<TItem4> label4, StepLabel<TItem5> label5, StepLabel<TItem6> label6, StepLabel<TItem7> label7, StepLabel<TItem8> label8) => Project<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8)>(p => p.ToTuple().By(__ => __.Select(label1)).By(__ => __.Select(label2)).By(__ => __.Select(label3)).By(__ => __.Select(label4)).By(__ => __.Select(label5)).By(__ => __.Select(label6)).By(__ => __.Select(label7)).By(__ => __.Select(label8)));

        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8)> IMapGremlinQueryBase<T1>.Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8>(Expression<Func<T1, TItem1>> projection1, Expression<Func<T1, TItem2>> projection2, Expression<Func<T1, TItem3>> projection3, Expression<Func<T1, TItem4>> projection4, Expression<Func<T1, TItem5>> projection5, Expression<Func<T1, TItem6>> projection6, Expression<Func<T1, TItem7>> projection7, Expression<Func<T1, TItem8>> projection8) => Select<IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8)>>(projection1, projection2, projection3, projection4, projection5, projection6, projection7, projection8);
        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9)> IGremlinQueryBase.Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9>(StepLabel<TItem1> label1, StepLabel<TItem2> label2, StepLabel<TItem3> label3, StepLabel<TItem4> label4, StepLabel<TItem5> label5, StepLabel<TItem6> label6, StepLabel<TItem7> label7, StepLabel<TItem8> label8, StepLabel<TItem9> label9) => Project<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9)>(p => p.ToTuple().By(__ => __.Select(label1)).By(__ => __.Select(label2)).By(__ => __.Select(label3)).By(__ => __.Select(label4)).By(__ => __.Select(label5)).By(__ => __.Select(label6)).By(__ => __.Select(label7)).By(__ => __.Select(label8)).By(__ => __.Select(label9)));

        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9)> IMapGremlinQueryBase<T1>.Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9>(Expression<Func<T1, TItem1>> projection1, Expression<Func<T1, TItem2>> projection2, Expression<Func<T1, TItem3>> projection3, Expression<Func<T1, TItem4>> projection4, Expression<Func<T1, TItem5>> projection5, Expression<Func<T1, TItem6>> projection6, Expression<Func<T1, TItem7>> projection7, Expression<Func<T1, TItem8>> projection8, Expression<Func<T1, TItem9>> projection9) => Select<IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9)>>(projection1, projection2, projection3, projection4, projection5, projection6, projection7, projection8, projection9);
        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10)> IGremlinQueryBase.Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10>(StepLabel<TItem1> label1, StepLabel<TItem2> label2, StepLabel<TItem3> label3, StepLabel<TItem4> label4, StepLabel<TItem5> label5, StepLabel<TItem6> label6, StepLabel<TItem7> label7, StepLabel<TItem8> label8, StepLabel<TItem9> label9, StepLabel<TItem10> label10) => Project<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10)>(p => p.ToTuple().By(__ => __.Select(label1)).By(__ => __.Select(label2)).By(__ => __.Select(label3)).By(__ => __.Select(label4)).By(__ => __.Select(label5)).By(__ => __.Select(label6)).By(__ => __.Select(label7)).By(__ => __.Select(label8)).By(__ => __.Select(label9)).By(__ => __.Select(label10)));

        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10)> IMapGremlinQueryBase<T1>.Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10>(Expression<Func<T1, TItem1>> projection1, Expression<Func<T1, TItem2>> projection2, Expression<Func<T1, TItem3>> projection3, Expression<Func<T1, TItem4>> projection4, Expression<Func<T1, TItem5>> projection5, Expression<Func<T1, TItem6>> projection6, Expression<Func<T1, TItem7>> projection7, Expression<Func<T1, TItem8>> projection8, Expression<Func<T1, TItem9>> projection9, Expression<Func<T1, TItem10>> projection10) => Select<IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10)>>(projection1, projection2, projection3, projection4, projection5, projection6, projection7, projection8, projection9, projection10);
        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11)> IGremlinQueryBase.Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11>(StepLabel<TItem1> label1, StepLabel<TItem2> label2, StepLabel<TItem3> label3, StepLabel<TItem4> label4, StepLabel<TItem5> label5, StepLabel<TItem6> label6, StepLabel<TItem7> label7, StepLabel<TItem8> label8, StepLabel<TItem9> label9, StepLabel<TItem10> label10, StepLabel<TItem11> label11) => Project<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11)>(p => p.ToTuple().By(__ => __.Select(label1)).By(__ => __.Select(label2)).By(__ => __.Select(label3)).By(__ => __.Select(label4)).By(__ => __.Select(label5)).By(__ => __.Select(label6)).By(__ => __.Select(label7)).By(__ => __.Select(label8)).By(__ => __.Select(label9)).By(__ => __.Select(label10)).By(__ => __.Select(label11)));

        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11)> IMapGremlinQueryBase<T1>.Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11>(Expression<Func<T1, TItem1>> projection1, Expression<Func<T1, TItem2>> projection2, Expression<Func<T1, TItem3>> projection3, Expression<Func<T1, TItem4>> projection4, Expression<Func<T1, TItem5>> projection5, Expression<Func<T1, TItem6>> projection6, Expression<Func<T1, TItem7>> projection7, Expression<Func<T1, TItem8>> projection8, Expression<Func<T1, TItem9>> projection9, Expression<Func<T1, TItem10>> projection10, Expression<Func<T1, TItem11>> projection11) => Select<IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11)>>(projection1, projection2, projection3, projection4, projection5, projection6, projection7, projection8, projection9, projection10, projection11);
        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12)> IGremlinQueryBase.Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12>(StepLabel<TItem1> label1, StepLabel<TItem2> label2, StepLabel<TItem3> label3, StepLabel<TItem4> label4, StepLabel<TItem5> label5, StepLabel<TItem6> label6, StepLabel<TItem7> label7, StepLabel<TItem8> label8, StepLabel<TItem9> label9, StepLabel<TItem10> label10, StepLabel<TItem11> label11, StepLabel<TItem12> label12) => Project<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12)>(p => p.ToTuple().By(__ => __.Select(label1)).By(__ => __.Select(label2)).By(__ => __.Select(label3)).By(__ => __.Select(label4)).By(__ => __.Select(label5)).By(__ => __.Select(label6)).By(__ => __.Select(label7)).By(__ => __.Select(label8)).By(__ => __.Select(label9)).By(__ => __.Select(label10)).By(__ => __.Select(label11)).By(__ => __.Select(label12)));

        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12)> IMapGremlinQueryBase<T1>.Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12>(Expression<Func<T1, TItem1>> projection1, Expression<Func<T1, TItem2>> projection2, Expression<Func<T1, TItem3>> projection3, Expression<Func<T1, TItem4>> projection4, Expression<Func<T1, TItem5>> projection5, Expression<Func<T1, TItem6>> projection6, Expression<Func<T1, TItem7>> projection7, Expression<Func<T1, TItem8>> projection8, Expression<Func<T1, TItem9>> projection9, Expression<Func<T1, TItem10>> projection10, Expression<Func<T1, TItem11>> projection11, Expression<Func<T1, TItem12>> projection12) => Select<IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12)>>(projection1, projection2, projection3, projection4, projection5, projection6, projection7, projection8, projection9, projection10, projection11, projection12);
        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13)> IGremlinQueryBase.Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13>(StepLabel<TItem1> label1, StepLabel<TItem2> label2, StepLabel<TItem3> label3, StepLabel<TItem4> label4, StepLabel<TItem5> label5, StepLabel<TItem6> label6, StepLabel<TItem7> label7, StepLabel<TItem8> label8, StepLabel<TItem9> label9, StepLabel<TItem10> label10, StepLabel<TItem11> label11, StepLabel<TItem12> label12, StepLabel<TItem13> label13) => Project<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13)>(p => p.ToTuple().By(__ => __.Select(label1)).By(__ => __.Select(label2)).By(__ => __.Select(label3)).By(__ => __.Select(label4)).By(__ => __.Select(label5)).By(__ => __.Select(label6)).By(__ => __.Select(label7)).By(__ => __.Select(label8)).By(__ => __.Select(label9)).By(__ => __.Select(label10)).By(__ => __.Select(label11)).By(__ => __.Select(label12)).By(__ => __.Select(label13)));

        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13)> IMapGremlinQueryBase<T1>.Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13>(Expression<Func<T1, TItem1>> projection1, Expression<Func<T1, TItem2>> projection2, Expression<Func<T1, TItem3>> projection3, Expression<Func<T1, TItem4>> projection4, Expression<Func<T1, TItem5>> projection5, Expression<Func<T1, TItem6>> projection6, Expression<Func<T1, TItem7>> projection7, Expression<Func<T1, TItem8>> projection8, Expression<Func<T1, TItem9>> projection9, Expression<Func<T1, TItem10>> projection10, Expression<Func<T1, TItem11>> projection11, Expression<Func<T1, TItem12>> projection12, Expression<Func<T1, TItem13>> projection13) => Select<IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13)>>(projection1, projection2, projection3, projection4, projection5, projection6, projection7, projection8, projection9, projection10, projection11, projection12, projection13);
        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14)> IGremlinQueryBase.Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14>(StepLabel<TItem1> label1, StepLabel<TItem2> label2, StepLabel<TItem3> label3, StepLabel<TItem4> label4, StepLabel<TItem5> label5, StepLabel<TItem6> label6, StepLabel<TItem7> label7, StepLabel<TItem8> label8, StepLabel<TItem9> label9, StepLabel<TItem10> label10, StepLabel<TItem11> label11, StepLabel<TItem12> label12, StepLabel<TItem13> label13, StepLabel<TItem14> label14) => Project<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14)>(p => p.ToTuple().By(__ => __.Select(label1)).By(__ => __.Select(label2)).By(__ => __.Select(label3)).By(__ => __.Select(label4)).By(__ => __.Select(label5)).By(__ => __.Select(label6)).By(__ => __.Select(label7)).By(__ => __.Select(label8)).By(__ => __.Select(label9)).By(__ => __.Select(label10)).By(__ => __.Select(label11)).By(__ => __.Select(label12)).By(__ => __.Select(label13)).By(__ => __.Select(label14)));

        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14)> IMapGremlinQueryBase<T1>.Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14>(Expression<Func<T1, TItem1>> projection1, Expression<Func<T1, TItem2>> projection2, Expression<Func<T1, TItem3>> projection3, Expression<Func<T1, TItem4>> projection4, Expression<Func<T1, TItem5>> projection5, Expression<Func<T1, TItem6>> projection6, Expression<Func<T1, TItem7>> projection7, Expression<Func<T1, TItem8>> projection8, Expression<Func<T1, TItem9>> projection9, Expression<Func<T1, TItem10>> projection10, Expression<Func<T1, TItem11>> projection11, Expression<Func<T1, TItem12>> projection12, Expression<Func<T1, TItem13>> projection13, Expression<Func<T1, TItem14>> projection14) => Select<IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14)>>(projection1, projection2, projection3, projection4, projection5, projection6, projection7, projection8, projection9, projection10, projection11, projection12, projection13, projection14);
        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15)> IGremlinQueryBase.Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15>(StepLabel<TItem1> label1, StepLabel<TItem2> label2, StepLabel<TItem3> label3, StepLabel<TItem4> label4, StepLabel<TItem5> label5, StepLabel<TItem6> label6, StepLabel<TItem7> label7, StepLabel<TItem8> label8, StepLabel<TItem9> label9, StepLabel<TItem10> label10, StepLabel<TItem11> label11, StepLabel<TItem12> label12, StepLabel<TItem13> label13, StepLabel<TItem14> label14, StepLabel<TItem15> label15) => Project<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15)>(p => p.ToTuple().By(__ => __.Select(label1)).By(__ => __.Select(label2)).By(__ => __.Select(label3)).By(__ => __.Select(label4)).By(__ => __.Select(label5)).By(__ => __.Select(label6)).By(__ => __.Select(label7)).By(__ => __.Select(label8)).By(__ => __.Select(label9)).By(__ => __.Select(label10)).By(__ => __.Select(label11)).By(__ => __.Select(label12)).By(__ => __.Select(label13)).By(__ => __.Select(label14)).By(__ => __.Select(label15)));

        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15)> IMapGremlinQueryBase<T1>.Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15>(Expression<Func<T1, TItem1>> projection1, Expression<Func<T1, TItem2>> projection2, Expression<Func<T1, TItem3>> projection3, Expression<Func<T1, TItem4>> projection4, Expression<Func<T1, TItem5>> projection5, Expression<Func<T1, TItem6>> projection6, Expression<Func<T1, TItem7>> projection7, Expression<Func<T1, TItem8>> projection8, Expression<Func<T1, TItem9>> projection9, Expression<Func<T1, TItem10>> projection10, Expression<Func<T1, TItem11>> projection11, Expression<Func<T1, TItem12>> projection12, Expression<Func<T1, TItem13>> projection13, Expression<Func<T1, TItem14>> projection14, Expression<Func<T1, TItem15>> projection15) => Select<IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15)>>(projection1, projection2, projection3, projection4, projection5, projection6, projection7, projection8, projection9, projection10, projection11, projection12, projection13, projection14, projection15);
        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16)> IGremlinQueryBase.Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16>(StepLabel<TItem1> label1, StepLabel<TItem2> label2, StepLabel<TItem3> label3, StepLabel<TItem4> label4, StepLabel<TItem5> label5, StepLabel<TItem6> label6, StepLabel<TItem7> label7, StepLabel<TItem8> label8, StepLabel<TItem9> label9, StepLabel<TItem10> label10, StepLabel<TItem11> label11, StepLabel<TItem12> label12, StepLabel<TItem13> label13, StepLabel<TItem14> label14, StepLabel<TItem15> label15, StepLabel<TItem16> label16) => Project<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16)>(p => p.ToTuple().By(__ => __.Select(label1)).By(__ => __.Select(label2)).By(__ => __.Select(label3)).By(__ => __.Select(label4)).By(__ => __.Select(label5)).By(__ => __.Select(label6)).By(__ => __.Select(label7)).By(__ => __.Select(label8)).By(__ => __.Select(label9)).By(__ => __.Select(label10)).By(__ => __.Select(label11)).By(__ => __.Select(label12)).By(__ => __.Select(label13)).By(__ => __.Select(label14)).By(__ => __.Select(label15)).By(__ => __.Select(label16)));

        IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16)> IMapGremlinQueryBase<T1>.Select<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16>(Expression<Func<T1, TItem1>> projection1, Expression<Func<T1, TItem2>> projection2, Expression<Func<T1, TItem3>> projection3, Expression<Func<T1, TItem4>> projection4, Expression<Func<T1, TItem5>> projection5, Expression<Func<T1, TItem6>> projection6, Expression<Func<T1, TItem7>> projection7, Expression<Func<T1, TItem8>> projection8, Expression<Func<T1, TItem9>> projection9, Expression<Func<T1, TItem10>> projection10, Expression<Func<T1, TItem11>> projection11, Expression<Func<T1, TItem12>> projection12, Expression<Func<T1, TItem13>> projection13, Expression<Func<T1, TItem14>> projection14, Expression<Func<T1, TItem15>> projection15, Expression<Func<T1, TItem16>> projection16) => Select<IMapGremlinQuery<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16)>>(projection1, projection2, projection3, projection4, projection5, projection6, projection7, projection8, projection9, projection10, projection11, projection12, projection13, projection14, projection15, projection16);

        IGremlinQuery<TResult> IGremlinQueryBase.Cast<TResult>() => Cast<TResult>();
        IElementGremlinQuery<TResult> IElementGremlinQueryBase.Cast<TResult>() => Cast<TResult>();
        IEdgeOrVertexGremlinQuery<TResult> IEdgeOrVertexGremlinQueryBase.Cast<TResult>() => Cast<TResult>();
        IVertexGremlinQuery<TResult> IVertexGremlinQueryBase.Cast<TResult>() => Cast<TResult>();
        IEdgeGremlinQuery<TResult> IEdgeGremlinQueryBase.Cast<TResult>() => Cast<TResult>();
        IPropertyGremlinQuery<TResult> IPropertyGremlinQueryBase.Cast<TResult>() => Cast<TResult>();

        TTargetQuery IGremlinQueryBaseRec<T1, IGremlinQuery<T1>>.Aggregate<TTargetQuery>(Func<IGremlinQuery<T1>, StepLabel<IArrayGremlinQuery<T1[], T1, IGremlinQuery<T1>>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, continuation);
        TTargetQuery IGremlinQueryBaseRec<T1, IGremlinQuery<T1>>.AggregateLocal<TTargetQuery>(Func<IGremlinQuery<T1>, StepLabel<IArrayGremlinQuery<T1[], T1, IGremlinQuery<T1>>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, continuation);

        IGremlinQuery<T1> IGremlinQueryBaseRec<T1, IGremlinQuery<T1>>.Aggregate(StepLabel<IArrayGremlinQuery<T1[], T1, IGremlinQuery<T1>>, T1[]> stepLabel) => Aggregate(Scope.Global, stepLabel);
        IGremlinQuery<T1> IGremlinQueryBaseRec<T1, IGremlinQuery<T1>>.AggregateLocal(StepLabel<IArrayGremlinQuery<T1[], T1, IGremlinQuery<T1>>, T1[]> stepLabel) => Aggregate(Scope.Local, stepLabel);

        TTargetQuery IGremlinQueryBaseRec<T1, IGremlinQuery<T1>>.As<TTargetQuery>(Func<IGremlinQuery<T1>, StepLabel<IGremlinQuery<T1>, T1>, TTargetQuery> continuation) => As<StepLabel<IGremlinQuery<T1>, T1>, TTargetQuery>(continuation);
        IGremlinQuery<T1> IGremlinQueryBaseRec<T1, IGremlinQuery<T1>>.As(StepLabel<T1> stepLabel) => As(stepLabel);

        IGremlinQuery<T1> IGremlinQueryBaseRec<IGremlinQuery<T1>>.And(params Func<IGremlinQuery<T1>, IGremlinQueryBase>[] andTraversals) => And(andTraversals);

        TTargetQuery IGremlinQueryBaseRec<IGremlinQuery<T1>>.As<TTargetQuery>(Func<IGremlinQuery<T1>, StepLabel<IGremlinQuery<T1>, object>, TTargetQuery> continuation) => As<StepLabel<IGremlinQuery<T1>, object>, TTargetQuery>(continuation);

        IGremlinQuery<T1> IGremlinQueryBaseRec<IGremlinQuery<T1>>.Barrier() => Barrier();

        TTargetQuery IGremlinQueryBaseRec<IGremlinQuery<T1>>.Choose<TTargetQuery>(Func<IGremlinQuery<T1>, IGremlinQueryBase> traversalPredicate, Func<IGremlinQuery<T1>, TTargetQuery> trueChoice, Func<IGremlinQuery<T1>, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(traversalPredicate, trueChoice, falseChoice);
        IGremlinQuery<T1> IGremlinQueryBaseRec<IGremlinQuery<T1>>.Choose(Func<IGremlinQuery<T1>, IGremlinQueryBase> traversalPredicate, Func<IGremlinQuery<T1>, IGremlinQuery<T1>> trueChoice) => Choose<IGremlinQuery<T1>, IGremlinQuery<T1>, IGremlinQuery<T1>>(traversalPredicate, trueChoice);
        IGremlinQuery<object> IGremlinQueryBaseRec<IGremlinQuery<T1>>.Choose(Func<IGremlinQuery<T1>, IGremlinQueryBase> traversalPredicate, Func<IGremlinQuery<T1>, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(traversalPredicate, trueChoice);
        
        TTargetQuery IGremlinQueryBaseRec<IGremlinQuery<T1>>.Choose<TTargetQuery>(Func<IChooseBuilder<IGremlinQuery<T1>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQueryBaseRec<T1, IGremlinQuery<T1>>.Choose<TTargetQuery>(Expression<Func<T1, bool>> predicate, Func<IGremlinQuery<T1>, TTargetQuery> trueChoice, Func<IGremlinQuery<T1>, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(predicate, trueChoice, falseChoice);
        IGremlinQuery<T1> IGremlinQueryBaseRec<T1, IGremlinQuery<T1>>.Choose(Expression<Func<T1, bool>> predicate, Func<IGremlinQuery<T1>, IGremlinQuery<T1>> trueChoice) => Choose<IGremlinQuery<T1>, IGremlinQuery<T1>, IGremlinQuery<T1>>(predicate, trueChoice);
        IGremlinQuery<object> IGremlinQueryBaseRec<T1, IGremlinQuery<T1>>.Choose(Expression<Func<T1, bool>> predicate, Func<IGremlinQuery<T1>, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(predicate, trueChoice);

        TTargetQuery IGremlinQueryBaseRec<IGremlinQuery<T1>>.Coalesce<TTargetQuery>(params Func<IGremlinQuery<T1>, TTargetQuery>[] traversals) => Coalesce<TTargetQuery, TTargetQuery>(traversals);
        IGremlinQuery<object> IGremlinQueryBaseRec<IGremlinQuery<T1>>.Coalesce(params Func<IGremlinQuery<T1>, IGremlinQueryBase>[] traversals) => Coalesce<IGremlinQueryBase, IGremlinQuery<object>>(traversals);

        IGremlinQuery<T1> IGremlinQueryBaseRec<IGremlinQuery<T1>>.Coin(double probability) => Coin(probability);

        IGremlinQuery<T1> IGremlinQueryBaseRec<IGremlinQuery<T1>>.CyclicPath() => CyclicPath();

        IGremlinQuery<T1> IGremlinQueryBaseRec<IGremlinQuery<T1>>.Dedup() => DedupGlobal();

        IGremlinQuery<T1> IGremlinQueryBaseRec<IGremlinQuery<T1>>.DedupLocal() => DedupLocal();

        TTargetQuery IGremlinQueryBaseRec<IGremlinQuery<T1>>.FlatMap<TTargetQuery>(Func<IGremlinQuery<T1>, TTargetQuery> mapping) => FlatMap(mapping);

        IArrayGremlinQuery<T1[], T1, IGremlinQuery<T1>> IGremlinQueryBaseRec<T1, IGremlinQuery<T1>>.ForceArray() => CloneAs<IArrayGremlinQuery<T1[], T1, IGremlinQuery<T1>>>();

        IArrayGremlinQuery<T1[], T1, IGremlinQuery<T1>> IGremlinQueryBaseRec<T1, IGremlinQuery<T1>>.Fold() => Fold<IGremlinQuery<T1>>();

        IMapGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<T1, IGremlinQuery<T1>>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IGremlinQuery<T1>>, IGroupBuilderWithKeyAndValue<TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IMapGremlinQuery<IDictionary<TNewKey, T1[]>> IGremlinQueryBaseRec<T1, IGremlinQuery<T1>>.Group<TNewKey>(Func<IGroupBuilder<IGremlinQuery<T1>>, IGroupBuilderWithKey<IGremlinQuery<T1>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IGremlinQuery<T1> IGremlinQueryBaseRec<IGremlinQuery<T1>>.Identity() => Identity();

        IGremlinQuery<T1> IGremlinQueryBaseRec<T1, IGremlinQuery<T1>>.Inject(params T1[] elements) => Inject(elements);
        
        IGremlinQuery<T1> IGremlinQueryBaseRec<IGremlinQuery<T1>>.Limit(long count) => LimitGlobal(count);

        TTargetQuery IGremlinQueryBaseRec<IGremlinQuery<T1>>.Local<TTargetQuery>(Func<IGremlinQuery<T1> , TTargetQuery> localTraversal) => Local(localTraversal);

        IGremlinQuery<T1> IGremlinQueryBaseRec<IGremlinQuery<T1>>.Loop(Func<IStartLoopBuilder<IGremlinQuery<T1>>, IFinalLoopBuilder<IGremlinQuery<T1>>> loopBuilderTransformation) => Loop(loopBuilderTransformation);

        TTargetQuery IGremlinQueryBaseRec<IGremlinQuery<T1>>.Map<TTargetQuery>(Func<IGremlinQuery<T1>, TTargetQuery> mapping) => Map(mapping);

        IGremlinQuery<T1> IGremlinQueryBaseRec<IGremlinQuery<T1>>.Max() => MaxGlobal();

        IGremlinQuery<T1> IGremlinQueryBaseRec<IGremlinQuery<T1>>.Mean() => MeanGlobal();

        IGremlinQuery<T1> IGremlinQueryBaseRec<IGremlinQuery<T1>>.Min() => MinGlobal();

        IGremlinQuery<T1> IGremlinQueryBaseRec<IGremlinQuery<T1>>.Not(Func<IGremlinQuery<T1>, IGremlinQueryBase> notTraversal) => Not(static (__, notTraversal) => notTraversal(__), notTraversal);

        IGremlinQuery<T1> IGremlinQueryBaseRec<IGremlinQuery<T1>>.None() => None();

        IGremlinQuery<T1> IGremlinQueryBaseRec<IGremlinQuery<T1>>.Optional(Func<IGremlinQuery<T1>, IGremlinQuery<T1>> optionalTraversal) => Optional(optionalTraversal);

        IGremlinQuery<T1> IGremlinQueryBaseRec<IGremlinQuery<T1>>.Or(params Func<IGremlinQuery<T1>, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);

        IGremlinQuery<T1> IGremlinQueryBaseRec<T1, IGremlinQuery<T1>>.Order(Func<IOrderBuilder<T1, IGremlinQuery<T1>>, IOrderBuilderWithBy<T1, IGremlinQuery<T1>>> projection) => OrderGlobal(projection);
        IGremlinQuery<T1> IGremlinQueryBaseRec<T1, IGremlinQuery<T1>>.OrderLocal(Func<IOrderBuilder<T1, IGremlinQuery<T1>>, IOrderBuilderWithBy<T1, IGremlinQuery<T1>>> projection) => OrderLocal(projection);

        IGremlinQuery<T1> IGremlinQueryBaseRec<IGremlinQuery<T1>>.Order(Func<IOrderBuilder<IGremlinQuery<T1>>, IOrderBuilderWithBy<IGremlinQuery<T1>>> projection) => OrderGlobal(projection);
        IGremlinQuery<T1> IGremlinQueryBaseRec<IGremlinQuery<T1>>.OrderLocal(Func<IOrderBuilder<IGremlinQuery<T1>>, IOrderBuilderWithBy<IGremlinQuery<T1>>> projection) => OrderLocal(projection);

        IGremlinQuery<dynamic> IGremlinQueryBaseRec<T1, IGremlinQuery<T1>>.Project(Func<IProjectBuilder<IGremlinQuery<T1>, T1>, IProjectDynamicResult> continuation) => Project(continuation);
        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, IGremlinQuery<T1>>.Project<TResult>(Func<IProjectBuilder<IGremlinQuery<T1>, T1>, IProjectMapResult<TResult>> continuation) => Project(continuation);
        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, IGremlinQuery<T1>>.Project<TResult>(Func<IProjectBuilder<IGremlinQuery<T1>, T1>, IProjectTupleResult<TResult>> continuation) => Project<TResult>(continuation);

        IGremlinQuery<T1> IGremlinQueryBaseRec<IGremlinQuery<T1>>.Range(long low, long high) => RangeGlobal(low, high);

        IGremlinQuery<T1> IGremlinQueryBaseRec<IGremlinQuery<T1>>.SideEffect(Func<IGremlinQuery<T1>, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IGremlinQuery<T1> IGremlinQueryBaseRec<IGremlinQuery<T1>>.SimplePath() => SimplePath();

        IGremlinQuery<T1> IGremlinQueryBaseRec<IGremlinQuery<T1>>.Skip(long count) => Skip(count, Scope.Global);

        IGremlinQuery<T1> IGremlinQueryBaseRec<IGremlinQuery<T1>>.Sum() => SumGlobal();

        IGremlinQuery<T1> IGremlinQueryBaseRec<IGremlinQuery<T1>>.Tail(long count) => TailGlobal(count);

        TTargetQuery IGremlinQueryBaseRec<IGremlinQuery<T1>>.Union<TTargetQuery>(params Func<IGremlinQuery<T1>, TTargetQuery>[] unionTraversals) => Union<TTargetQuery, TTargetQuery>(unionTraversals);
        IGremlinQuery<object> IGremlinQueryBaseRec<IGremlinQuery<T1>>.Union(params Func<IGremlinQuery<T1>, IGremlinQueryBase>[] unionTraversals) => Union<IGremlinQueryBase, IGremlinQuery<object>>(unionTraversals);

        IGremlinQuery<T1> IGremlinQueryBaseRec<T1, IGremlinQuery<T1>>.Where(Expression<Func<T1, bool>> predicate) => Where(predicate);
        IGremlinQuery<T1> IGremlinQueryBaseRec<IGremlinQuery<T1>>.Where(Func<IGremlinQuery<T1>, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);
        TTargetQuery IGremlinQueryBaseRec<T1, IMapGremlinQuery<T1>>.Aggregate<TTargetQuery>(Func<IMapGremlinQuery<T1>, StepLabel<IArrayGremlinQuery<T1[], T1, IMapGremlinQuery<T1>>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, continuation);
        TTargetQuery IGremlinQueryBaseRec<T1, IMapGremlinQuery<T1>>.AggregateLocal<TTargetQuery>(Func<IMapGremlinQuery<T1>, StepLabel<IArrayGremlinQuery<T1[], T1, IMapGremlinQuery<T1>>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, continuation);

        IMapGremlinQuery<T1> IGremlinQueryBaseRec<T1, IMapGremlinQuery<T1>>.Aggregate(StepLabel<IArrayGremlinQuery<T1[], T1, IMapGremlinQuery<T1>>, T1[]> stepLabel) => Aggregate(Scope.Global, stepLabel);
        IMapGremlinQuery<T1> IGremlinQueryBaseRec<T1, IMapGremlinQuery<T1>>.AggregateLocal(StepLabel<IArrayGremlinQuery<T1[], T1, IMapGremlinQuery<T1>>, T1[]> stepLabel) => Aggregate(Scope.Local, stepLabel);

        TTargetQuery IGremlinQueryBaseRec<T1, IMapGremlinQuery<T1>>.As<TTargetQuery>(Func<IMapGremlinQuery<T1>, StepLabel<IMapGremlinQuery<T1>, T1>, TTargetQuery> continuation) => As<StepLabel<IMapGremlinQuery<T1>, T1>, TTargetQuery>(continuation);
        IMapGremlinQuery<T1> IGremlinQueryBaseRec<T1, IMapGremlinQuery<T1>>.As(StepLabel<T1> stepLabel) => As(stepLabel);

        IMapGremlinQuery<T1> IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.And(params Func<IMapGremlinQuery<T1>, IGremlinQueryBase>[] andTraversals) => And(andTraversals);

        TTargetQuery IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.As<TTargetQuery>(Func<IMapGremlinQuery<T1>, StepLabel<IMapGremlinQuery<T1>, object>, TTargetQuery> continuation) => As<StepLabel<IMapGremlinQuery<T1>, object>, TTargetQuery>(continuation);

        IMapGremlinQuery<T1> IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.Barrier() => Barrier();

        TTargetQuery IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.Choose<TTargetQuery>(Func<IMapGremlinQuery<T1>, IGremlinQueryBase> traversalPredicate, Func<IMapGremlinQuery<T1>, TTargetQuery> trueChoice, Func<IMapGremlinQuery<T1>, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(traversalPredicate, trueChoice, falseChoice);
        IMapGremlinQuery<T1> IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.Choose(Func<IMapGremlinQuery<T1>, IGremlinQueryBase> traversalPredicate, Func<IMapGremlinQuery<T1>, IMapGremlinQuery<T1>> trueChoice) => Choose<IMapGremlinQuery<T1>, IMapGremlinQuery<T1>, IMapGremlinQuery<T1>>(traversalPredicate, trueChoice);
        IGremlinQuery<object> IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.Choose(Func<IMapGremlinQuery<T1>, IGremlinQueryBase> traversalPredicate, Func<IMapGremlinQuery<T1>, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(traversalPredicate, trueChoice);
        
        TTargetQuery IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.Choose<TTargetQuery>(Func<IChooseBuilder<IMapGremlinQuery<T1>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQueryBaseRec<T1, IMapGremlinQuery<T1>>.Choose<TTargetQuery>(Expression<Func<T1, bool>> predicate, Func<IMapGremlinQuery<T1>, TTargetQuery> trueChoice, Func<IMapGremlinQuery<T1>, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(predicate, trueChoice, falseChoice);
        IMapGremlinQuery<T1> IGremlinQueryBaseRec<T1, IMapGremlinQuery<T1>>.Choose(Expression<Func<T1, bool>> predicate, Func<IMapGremlinQuery<T1>, IMapGremlinQuery<T1>> trueChoice) => Choose<IMapGremlinQuery<T1>, IMapGremlinQuery<T1>, IMapGremlinQuery<T1>>(predicate, trueChoice);
        IGremlinQuery<object> IGremlinQueryBaseRec<T1, IMapGremlinQuery<T1>>.Choose(Expression<Func<T1, bool>> predicate, Func<IMapGremlinQuery<T1>, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(predicate, trueChoice);

        TTargetQuery IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.Coalesce<TTargetQuery>(params Func<IMapGremlinQuery<T1>, TTargetQuery>[] traversals) => Coalesce<TTargetQuery, TTargetQuery>(traversals);
        IGremlinQuery<object> IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.Coalesce(params Func<IMapGremlinQuery<T1>, IGremlinQueryBase>[] traversals) => Coalesce<IGremlinQueryBase, IGremlinQuery<object>>(traversals);

        IMapGremlinQuery<T1> IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.Coin(double probability) => Coin(probability);

        IMapGremlinQuery<T1> IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.CyclicPath() => CyclicPath();

        IMapGremlinQuery<T1> IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.Dedup() => DedupGlobal();

        IMapGremlinQuery<T1> IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.DedupLocal() => DedupLocal();

        TTargetQuery IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.FlatMap<TTargetQuery>(Func<IMapGremlinQuery<T1>, TTargetQuery> mapping) => FlatMap(mapping);

        IArrayGremlinQuery<T1[], T1, IMapGremlinQuery<T1>> IGremlinQueryBaseRec<T1, IMapGremlinQuery<T1>>.ForceArray() => CloneAs<IArrayGremlinQuery<T1[], T1, IMapGremlinQuery<T1>>>();

        IArrayGremlinQuery<T1[], T1, IMapGremlinQuery<T1>> IGremlinQueryBaseRec<T1, IMapGremlinQuery<T1>>.Fold() => Fold<IMapGremlinQuery<T1>>();

        IMapGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<T1, IMapGremlinQuery<T1>>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IMapGremlinQuery<T1>>, IGroupBuilderWithKeyAndValue<TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IMapGremlinQuery<IDictionary<TNewKey, T1[]>> IGremlinQueryBaseRec<T1, IMapGremlinQuery<T1>>.Group<TNewKey>(Func<IGroupBuilder<IMapGremlinQuery<T1>>, IGroupBuilderWithKey<IMapGremlinQuery<T1>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IMapGremlinQuery<T1> IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.Identity() => Identity();

        IMapGremlinQuery<T1> IGremlinQueryBaseRec<T1, IMapGremlinQuery<T1>>.Inject(params T1[] elements) => Inject(elements);
        
        IMapGremlinQuery<T1> IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.Limit(long count) => LimitGlobal(count);

        TTargetQuery IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.Local<TTargetQuery>(Func<IMapGremlinQuery<T1> , TTargetQuery> localTraversal) => Local(localTraversal);

        IMapGremlinQuery<T1> IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.Loop(Func<IStartLoopBuilder<IMapGremlinQuery<T1>>, IFinalLoopBuilder<IMapGremlinQuery<T1>>> loopBuilderTransformation) => Loop(loopBuilderTransformation);

        TTargetQuery IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.Map<TTargetQuery>(Func<IMapGremlinQuery<T1>, TTargetQuery> mapping) => Map(mapping);

        IMapGremlinQuery<T1> IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.Max() => MaxGlobal();

        IMapGremlinQuery<T1> IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.Mean() => MeanGlobal();

        IMapGremlinQuery<T1> IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.Min() => MinGlobal();

        IMapGremlinQuery<T1> IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.Not(Func<IMapGremlinQuery<T1>, IGremlinQueryBase> notTraversal) => Not(static (__, notTraversal) => notTraversal(__), notTraversal);

        IMapGremlinQuery<T1> IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.None() => None();

        IMapGremlinQuery<T1> IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.Optional(Func<IMapGremlinQuery<T1>, IMapGremlinQuery<T1>> optionalTraversal) => Optional(optionalTraversal);

        IMapGremlinQuery<T1> IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.Or(params Func<IMapGremlinQuery<T1>, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);

        IMapGremlinQuery<T1> IGremlinQueryBaseRec<T1, IMapGremlinQuery<T1>>.Order(Func<IOrderBuilder<T1, IMapGremlinQuery<T1>>, IOrderBuilderWithBy<T1, IMapGremlinQuery<T1>>> projection) => OrderGlobal(projection);
        IMapGremlinQuery<T1> IGremlinQueryBaseRec<T1, IMapGremlinQuery<T1>>.OrderLocal(Func<IOrderBuilder<T1, IMapGremlinQuery<T1>>, IOrderBuilderWithBy<T1, IMapGremlinQuery<T1>>> projection) => OrderLocal(projection);

        IMapGremlinQuery<T1> IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.Order(Func<IOrderBuilder<IMapGremlinQuery<T1>>, IOrderBuilderWithBy<IMapGremlinQuery<T1>>> projection) => OrderGlobal(projection);
        IMapGremlinQuery<T1> IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.OrderLocal(Func<IOrderBuilder<IMapGremlinQuery<T1>>, IOrderBuilderWithBy<IMapGremlinQuery<T1>>> projection) => OrderLocal(projection);

        IGremlinQuery<dynamic> IGremlinQueryBaseRec<T1, IMapGremlinQuery<T1>>.Project(Func<IProjectBuilder<IMapGremlinQuery<T1>, T1>, IProjectDynamicResult> continuation) => Project(continuation);
        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, IMapGremlinQuery<T1>>.Project<TResult>(Func<IProjectBuilder<IMapGremlinQuery<T1>, T1>, IProjectMapResult<TResult>> continuation) => Project(continuation);
        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, IMapGremlinQuery<T1>>.Project<TResult>(Func<IProjectBuilder<IMapGremlinQuery<T1>, T1>, IProjectTupleResult<TResult>> continuation) => Project<TResult>(continuation);

        IMapGremlinQuery<T1> IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.Range(long low, long high) => RangeGlobal(low, high);

        IMapGremlinQuery<T1> IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.SideEffect(Func<IMapGremlinQuery<T1>, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IMapGremlinQuery<T1> IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.SimplePath() => SimplePath();

        IMapGremlinQuery<T1> IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.Skip(long count) => Skip(count, Scope.Global);

        IMapGremlinQuery<T1> IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.Sum() => SumGlobal();

        IMapGremlinQuery<T1> IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.Tail(long count) => TailGlobal(count);

        TTargetQuery IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.Union<TTargetQuery>(params Func<IMapGremlinQuery<T1>, TTargetQuery>[] unionTraversals) => Union<TTargetQuery, TTargetQuery>(unionTraversals);
        IGremlinQuery<object> IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.Union(params Func<IMapGremlinQuery<T1>, IGremlinQueryBase>[] unionTraversals) => Union<IGremlinQueryBase, IGremlinQuery<object>>(unionTraversals);

        IMapGremlinQuery<T1> IGremlinQueryBaseRec<T1, IMapGremlinQuery<T1>>.Where(Expression<Func<T1, bool>> predicate) => Where(predicate);
        IMapGremlinQuery<T1> IGremlinQueryBaseRec<IMapGremlinQuery<T1>>.Where(Func<IMapGremlinQuery<T1>, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);
        TTargetQuery IGremlinQueryBaseRec<T1, IArrayGremlinQuery<T1, T2, T3>>.Aggregate<TTargetQuery>(Func<IArrayGremlinQuery<T1, T2, T3>, StepLabel<IArrayGremlinQuery<T1[], T1, IArrayGremlinQuery<T1, T2, T3>>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, continuation);
        TTargetQuery IGremlinQueryBaseRec<T1, IArrayGremlinQuery<T1, T2, T3>>.AggregateLocal<TTargetQuery>(Func<IArrayGremlinQuery<T1, T2, T3>, StepLabel<IArrayGremlinQuery<T1[], T1, IArrayGremlinQuery<T1, T2, T3>>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, continuation);

        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<T1, IArrayGremlinQuery<T1, T2, T3>>.Aggregate(StepLabel<IArrayGremlinQuery<T1[], T1, IArrayGremlinQuery<T1, T2, T3>>, T1[]> stepLabel) => Aggregate(Scope.Global, stepLabel);
        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<T1, IArrayGremlinQuery<T1, T2, T3>>.AggregateLocal(StepLabel<IArrayGremlinQuery<T1[], T1, IArrayGremlinQuery<T1, T2, T3>>, T1[]> stepLabel) => Aggregate(Scope.Local, stepLabel);

        TTargetQuery IGremlinQueryBaseRec<T1, IArrayGremlinQuery<T1, T2, T3>>.As<TTargetQuery>(Func<IArrayGremlinQuery<T1, T2, T3>, StepLabel<IArrayGremlinQuery<T1, T2, T3>, T1>, TTargetQuery> continuation) => As<StepLabel<IArrayGremlinQuery<T1, T2, T3>, T1>, TTargetQuery>(continuation);
        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<T1, IArrayGremlinQuery<T1, T2, T3>>.As(StepLabel<T1> stepLabel) => As(stepLabel);

        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.And(params Func<IArrayGremlinQuery<T1, T2, T3>, IGremlinQueryBase>[] andTraversals) => And(andTraversals);

        TTargetQuery IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.As<TTargetQuery>(Func<IArrayGremlinQuery<T1, T2, T3>, StepLabel<IArrayGremlinQuery<T1, T2, T3>, object>, TTargetQuery> continuation) => As<StepLabel<IArrayGremlinQuery<T1, T2, T3>, object>, TTargetQuery>(continuation);

        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.Barrier() => Barrier();

        TTargetQuery IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.Choose<TTargetQuery>(Func<IArrayGremlinQuery<T1, T2, T3>, IGremlinQueryBase> traversalPredicate, Func<IArrayGremlinQuery<T1, T2, T3>, TTargetQuery> trueChoice, Func<IArrayGremlinQuery<T1, T2, T3>, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(traversalPredicate, trueChoice, falseChoice);
        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.Choose(Func<IArrayGremlinQuery<T1, T2, T3>, IGremlinQueryBase> traversalPredicate, Func<IArrayGremlinQuery<T1, T2, T3>, IArrayGremlinQuery<T1, T2, T3>> trueChoice) => Choose<IArrayGremlinQuery<T1, T2, T3>, IArrayGremlinQuery<T1, T2, T3>, IArrayGremlinQuery<T1, T2, T3>>(traversalPredicate, trueChoice);
        IGremlinQuery<object> IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.Choose(Func<IArrayGremlinQuery<T1, T2, T3>, IGremlinQueryBase> traversalPredicate, Func<IArrayGremlinQuery<T1, T2, T3>, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(traversalPredicate, trueChoice);
        
        TTargetQuery IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.Choose<TTargetQuery>(Func<IChooseBuilder<IArrayGremlinQuery<T1, T2, T3>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQueryBaseRec<T1, IArrayGremlinQuery<T1, T2, T3>>.Choose<TTargetQuery>(Expression<Func<T1, bool>> predicate, Func<IArrayGremlinQuery<T1, T2, T3>, TTargetQuery> trueChoice, Func<IArrayGremlinQuery<T1, T2, T3>, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(predicate, trueChoice, falseChoice);
        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<T1, IArrayGremlinQuery<T1, T2, T3>>.Choose(Expression<Func<T1, bool>> predicate, Func<IArrayGremlinQuery<T1, T2, T3>, IArrayGremlinQuery<T1, T2, T3>> trueChoice) => Choose<IArrayGremlinQuery<T1, T2, T3>, IArrayGremlinQuery<T1, T2, T3>, IArrayGremlinQuery<T1, T2, T3>>(predicate, trueChoice);
        IGremlinQuery<object> IGremlinQueryBaseRec<T1, IArrayGremlinQuery<T1, T2, T3>>.Choose(Expression<Func<T1, bool>> predicate, Func<IArrayGremlinQuery<T1, T2, T3>, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(predicate, trueChoice);

        TTargetQuery IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.Coalesce<TTargetQuery>(params Func<IArrayGremlinQuery<T1, T2, T3>, TTargetQuery>[] traversals) => Coalesce<TTargetQuery, TTargetQuery>(traversals);
        IGremlinQuery<object> IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.Coalesce(params Func<IArrayGremlinQuery<T1, T2, T3>, IGremlinQueryBase>[] traversals) => Coalesce<IGremlinQueryBase, IGremlinQuery<object>>(traversals);

        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.Coin(double probability) => Coin(probability);

        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.CyclicPath() => CyclicPath();

        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.Dedup() => DedupGlobal();

        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.DedupLocal() => DedupLocal();

        TTargetQuery IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.FlatMap<TTargetQuery>(Func<IArrayGremlinQuery<T1, T2, T3>, TTargetQuery> mapping) => FlatMap(mapping);

        IArrayGremlinQuery<T1[], T1, IArrayGremlinQuery<T1, T2, T3>> IGremlinQueryBaseRec<T1, IArrayGremlinQuery<T1, T2, T3>>.ForceArray() => CloneAs<IArrayGremlinQuery<T1[], T1, IArrayGremlinQuery<T1, T2, T3>>>();

        IArrayGremlinQuery<T1[], T1, IArrayGremlinQuery<T1, T2, T3>> IGremlinQueryBaseRec<T1, IArrayGremlinQuery<T1, T2, T3>>.Fold() => Fold<IArrayGremlinQuery<T1, T2, T3>>();

        IMapGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<T1, IArrayGremlinQuery<T1, T2, T3>>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IArrayGremlinQuery<T1, T2, T3>>, IGroupBuilderWithKeyAndValue<TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IMapGremlinQuery<IDictionary<TNewKey, T1[]>> IGremlinQueryBaseRec<T1, IArrayGremlinQuery<T1, T2, T3>>.Group<TNewKey>(Func<IGroupBuilder<IArrayGremlinQuery<T1, T2, T3>>, IGroupBuilderWithKey<IArrayGremlinQuery<T1, T2, T3>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.Identity() => Identity();

        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<T1, IArrayGremlinQuery<T1, T2, T3>>.Inject(params T1[] elements) => Inject(elements);
        
        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.Limit(long count) => LimitGlobal(count);

        TTargetQuery IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.Local<TTargetQuery>(Func<IArrayGremlinQuery<T1, T2, T3> , TTargetQuery> localTraversal) => Local(localTraversal);

        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.Loop(Func<IStartLoopBuilder<IArrayGremlinQuery<T1, T2, T3>>, IFinalLoopBuilder<IArrayGremlinQuery<T1, T2, T3>>> loopBuilderTransformation) => Loop(loopBuilderTransformation);

        TTargetQuery IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.Map<TTargetQuery>(Func<IArrayGremlinQuery<T1, T2, T3>, TTargetQuery> mapping) => Map(mapping);

        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.Max() => MaxGlobal();

        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.Mean() => MeanGlobal();

        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.Min() => MinGlobal();

        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.Not(Func<IArrayGremlinQuery<T1, T2, T3>, IGremlinQueryBase> notTraversal) => Not(static (__, notTraversal) => notTraversal(__), notTraversal);

        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.None() => None();

        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.Optional(Func<IArrayGremlinQuery<T1, T2, T3>, IArrayGremlinQuery<T1, T2, T3>> optionalTraversal) => Optional(optionalTraversal);

        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.Or(params Func<IArrayGremlinQuery<T1, T2, T3>, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);

        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<T1, IArrayGremlinQuery<T1, T2, T3>>.Order(Func<IOrderBuilder<T1, IArrayGremlinQuery<T1, T2, T3>>, IOrderBuilderWithBy<T1, IArrayGremlinQuery<T1, T2, T3>>> projection) => OrderGlobal(projection);
        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<T1, IArrayGremlinQuery<T1, T2, T3>>.OrderLocal(Func<IOrderBuilder<T1, IArrayGremlinQuery<T1, T2, T3>>, IOrderBuilderWithBy<T1, IArrayGremlinQuery<T1, T2, T3>>> projection) => OrderLocal(projection);

        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.Order(Func<IOrderBuilder<IArrayGremlinQuery<T1, T2, T3>>, IOrderBuilderWithBy<IArrayGremlinQuery<T1, T2, T3>>> projection) => OrderGlobal(projection);
        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.OrderLocal(Func<IOrderBuilder<IArrayGremlinQuery<T1, T2, T3>>, IOrderBuilderWithBy<IArrayGremlinQuery<T1, T2, T3>>> projection) => OrderLocal(projection);

        IGremlinQuery<dynamic> IGremlinQueryBaseRec<T1, IArrayGremlinQuery<T1, T2, T3>>.Project(Func<IProjectBuilder<IArrayGremlinQuery<T1, T2, T3>, T1>, IProjectDynamicResult> continuation) => Project(continuation);
        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, IArrayGremlinQuery<T1, T2, T3>>.Project<TResult>(Func<IProjectBuilder<IArrayGremlinQuery<T1, T2, T3>, T1>, IProjectMapResult<TResult>> continuation) => Project(continuation);
        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, IArrayGremlinQuery<T1, T2, T3>>.Project<TResult>(Func<IProjectBuilder<IArrayGremlinQuery<T1, T2, T3>, T1>, IProjectTupleResult<TResult>> continuation) => Project<TResult>(continuation);

        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.Range(long low, long high) => RangeGlobal(low, high);

        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.SideEffect(Func<IArrayGremlinQuery<T1, T2, T3>, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.SimplePath() => SimplePath();

        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.Skip(long count) => Skip(count, Scope.Global);

        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.Sum() => SumGlobal();

        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.Tail(long count) => TailGlobal(count);

        TTargetQuery IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.Union<TTargetQuery>(params Func<IArrayGremlinQuery<T1, T2, T3>, TTargetQuery>[] unionTraversals) => Union<TTargetQuery, TTargetQuery>(unionTraversals);
        IGremlinQuery<object> IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.Union(params Func<IArrayGremlinQuery<T1, T2, T3>, IGremlinQueryBase>[] unionTraversals) => Union<IGremlinQueryBase, IGremlinQuery<object>>(unionTraversals);

        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<T1, IArrayGremlinQuery<T1, T2, T3>>.Where(Expression<Func<T1, bool>> predicate) => Where(predicate);
        IArrayGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T3>>.Where(Func<IArrayGremlinQuery<T1, T2, T3>, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);
        TTargetQuery IGremlinQueryBaseRec<T1, IElementGremlinQuery<T1>>.Aggregate<TTargetQuery>(Func<IElementGremlinQuery<T1>, StepLabel<IArrayGremlinQuery<T1[], T1, IElementGremlinQuery<T1>>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, continuation);
        TTargetQuery IGremlinQueryBaseRec<T1, IElementGremlinQuery<T1>>.AggregateLocal<TTargetQuery>(Func<IElementGremlinQuery<T1>, StepLabel<IArrayGremlinQuery<T1[], T1, IElementGremlinQuery<T1>>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, continuation);

        IElementGremlinQuery<T1> IGremlinQueryBaseRec<T1, IElementGremlinQuery<T1>>.Aggregate(StepLabel<IArrayGremlinQuery<T1[], T1, IElementGremlinQuery<T1>>, T1[]> stepLabel) => Aggregate(Scope.Global, stepLabel);
        IElementGremlinQuery<T1> IGremlinQueryBaseRec<T1, IElementGremlinQuery<T1>>.AggregateLocal(StepLabel<IArrayGremlinQuery<T1[], T1, IElementGremlinQuery<T1>>, T1[]> stepLabel) => Aggregate(Scope.Local, stepLabel);

        TTargetQuery IGremlinQueryBaseRec<T1, IElementGremlinQuery<T1>>.As<TTargetQuery>(Func<IElementGremlinQuery<T1>, StepLabel<IElementGremlinQuery<T1>, T1>, TTargetQuery> continuation) => As<StepLabel<IElementGremlinQuery<T1>, T1>, TTargetQuery>(continuation);
        IElementGremlinQuery<T1> IGremlinQueryBaseRec<T1, IElementGremlinQuery<T1>>.As(StepLabel<T1> stepLabel) => As(stepLabel);

        IElementGremlinQuery<T1> IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.And(params Func<IElementGremlinQuery<T1>, IGremlinQueryBase>[] andTraversals) => And(andTraversals);

        TTargetQuery IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.As<TTargetQuery>(Func<IElementGremlinQuery<T1>, StepLabel<IElementGremlinQuery<T1>, object>, TTargetQuery> continuation) => As<StepLabel<IElementGremlinQuery<T1>, object>, TTargetQuery>(continuation);

        IElementGremlinQuery<T1> IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Barrier() => Barrier();

        TTargetQuery IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Choose<TTargetQuery>(Func<IElementGremlinQuery<T1>, IGremlinQueryBase> traversalPredicate, Func<IElementGremlinQuery<T1>, TTargetQuery> trueChoice, Func<IElementGremlinQuery<T1>, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(traversalPredicate, trueChoice, falseChoice);
        IElementGremlinQuery<T1> IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Choose(Func<IElementGremlinQuery<T1>, IGremlinQueryBase> traversalPredicate, Func<IElementGremlinQuery<T1>, IElementGremlinQuery<T1>> trueChoice) => Choose<IElementGremlinQuery<T1>, IElementGremlinQuery<T1>, IElementGremlinQuery<T1>>(traversalPredicate, trueChoice);
        IGremlinQuery<object> IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Choose(Func<IElementGremlinQuery<T1>, IGremlinQueryBase> traversalPredicate, Func<IElementGremlinQuery<T1>, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(traversalPredicate, trueChoice);
        
        TTargetQuery IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Choose<TTargetQuery>(Func<IChooseBuilder<IElementGremlinQuery<T1>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQueryBaseRec<T1, IElementGremlinQuery<T1>>.Choose<TTargetQuery>(Expression<Func<T1, bool>> predicate, Func<IElementGremlinQuery<T1>, TTargetQuery> trueChoice, Func<IElementGremlinQuery<T1>, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(predicate, trueChoice, falseChoice);
        IElementGremlinQuery<T1> IGremlinQueryBaseRec<T1, IElementGremlinQuery<T1>>.Choose(Expression<Func<T1, bool>> predicate, Func<IElementGremlinQuery<T1>, IElementGremlinQuery<T1>> trueChoice) => Choose<IElementGremlinQuery<T1>, IElementGremlinQuery<T1>, IElementGremlinQuery<T1>>(predicate, trueChoice);
        IGremlinQuery<object> IGremlinQueryBaseRec<T1, IElementGremlinQuery<T1>>.Choose(Expression<Func<T1, bool>> predicate, Func<IElementGremlinQuery<T1>, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(predicate, trueChoice);

        TTargetQuery IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Coalesce<TTargetQuery>(params Func<IElementGremlinQuery<T1>, TTargetQuery>[] traversals) => Coalesce<TTargetQuery, TTargetQuery>(traversals);
        IGremlinQuery<object> IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Coalesce(params Func<IElementGremlinQuery<T1>, IGremlinQueryBase>[] traversals) => Coalesce<IGremlinQueryBase, IGremlinQuery<object>>(traversals);

        IElementGremlinQuery<T1> IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Coin(double probability) => Coin(probability);

        IElementGremlinQuery<T1> IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.CyclicPath() => CyclicPath();

        IElementGremlinQuery<T1> IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Dedup() => DedupGlobal();

        IElementGremlinQuery<T1> IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.DedupLocal() => DedupLocal();

        TTargetQuery IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.FlatMap<TTargetQuery>(Func<IElementGremlinQuery<T1>, TTargetQuery> mapping) => FlatMap(mapping);

        IArrayGremlinQuery<T1[], T1, IElementGremlinQuery<T1>> IGremlinQueryBaseRec<T1, IElementGremlinQuery<T1>>.ForceArray() => CloneAs<IArrayGremlinQuery<T1[], T1, IElementGremlinQuery<T1>>>();

        IArrayGremlinQuery<T1[], T1, IElementGremlinQuery<T1>> IGremlinQueryBaseRec<T1, IElementGremlinQuery<T1>>.Fold() => Fold<IElementGremlinQuery<T1>>();

        IMapGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<T1, IElementGremlinQuery<T1>>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IElementGremlinQuery<T1>>, IGroupBuilderWithKeyAndValue<TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IMapGremlinQuery<IDictionary<TNewKey, T1[]>> IGremlinQueryBaseRec<T1, IElementGremlinQuery<T1>>.Group<TNewKey>(Func<IGroupBuilder<IElementGremlinQuery<T1>>, IGroupBuilderWithKey<IElementGremlinQuery<T1>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IElementGremlinQuery<T1> IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Identity() => Identity();

        IElementGremlinQuery<T1> IGremlinQueryBaseRec<T1, IElementGremlinQuery<T1>>.Inject(params T1[] elements) => Inject(elements);
        
        IElementGremlinQuery<T1> IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Limit(long count) => LimitGlobal(count);

        TTargetQuery IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Local<TTargetQuery>(Func<IElementGremlinQuery<T1> , TTargetQuery> localTraversal) => Local(localTraversal);

        IElementGremlinQuery<T1> IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Loop(Func<IStartLoopBuilder<IElementGremlinQuery<T1>>, IFinalLoopBuilder<IElementGremlinQuery<T1>>> loopBuilderTransformation) => Loop(loopBuilderTransformation);

        TTargetQuery IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Map<TTargetQuery>(Func<IElementGremlinQuery<T1>, TTargetQuery> mapping) => Map(mapping);

        IElementGremlinQuery<T1> IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Max() => MaxGlobal();

        IElementGremlinQuery<T1> IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Mean() => MeanGlobal();

        IElementGremlinQuery<T1> IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Min() => MinGlobal();

        IElementGremlinQuery<T1> IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Not(Func<IElementGremlinQuery<T1>, IGremlinQueryBase> notTraversal) => Not(static (__, notTraversal) => notTraversal(__), notTraversal);

        IElementGremlinQuery<T1> IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.None() => None();

        IElementGremlinQuery<T1> IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Optional(Func<IElementGremlinQuery<T1>, IElementGremlinQuery<T1>> optionalTraversal) => Optional(optionalTraversal);

        IElementGremlinQuery<T1> IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Or(params Func<IElementGremlinQuery<T1>, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);

        IElementGremlinQuery<T1> IGremlinQueryBaseRec<T1, IElementGremlinQuery<T1>>.Order(Func<IOrderBuilder<T1, IElementGremlinQuery<T1>>, IOrderBuilderWithBy<T1, IElementGremlinQuery<T1>>> projection) => OrderGlobal(projection);
        IElementGremlinQuery<T1> IGremlinQueryBaseRec<T1, IElementGremlinQuery<T1>>.OrderLocal(Func<IOrderBuilder<T1, IElementGremlinQuery<T1>>, IOrderBuilderWithBy<T1, IElementGremlinQuery<T1>>> projection) => OrderLocal(projection);

        IElementGremlinQuery<T1> IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Order(Func<IOrderBuilder<IElementGremlinQuery<T1>>, IOrderBuilderWithBy<IElementGremlinQuery<T1>>> projection) => OrderGlobal(projection);
        IElementGremlinQuery<T1> IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.OrderLocal(Func<IOrderBuilder<IElementGremlinQuery<T1>>, IOrderBuilderWithBy<IElementGremlinQuery<T1>>> projection) => OrderLocal(projection);

        IGremlinQuery<dynamic> IGremlinQueryBaseRec<T1, IElementGremlinQuery<T1>>.Project(Func<IProjectBuilder<IElementGremlinQuery<T1>, T1>, IProjectDynamicResult> continuation) => Project(continuation);
        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, IElementGremlinQuery<T1>>.Project<TResult>(Func<IProjectBuilder<IElementGremlinQuery<T1>, T1>, IProjectMapResult<TResult>> continuation) => Project(continuation);
        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, IElementGremlinQuery<T1>>.Project<TResult>(Func<IProjectBuilder<IElementGremlinQuery<T1>, T1>, IProjectTupleResult<TResult>> continuation) => Project<TResult>(continuation);

        IElementGremlinQuery<T1> IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Range(long low, long high) => RangeGlobal(low, high);

        IElementGremlinQuery<T1> IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.SideEffect(Func<IElementGremlinQuery<T1>, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IElementGremlinQuery<T1> IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.SimplePath() => SimplePath();

        IElementGremlinQuery<T1> IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Skip(long count) => Skip(count, Scope.Global);

        IElementGremlinQuery<T1> IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Sum() => SumGlobal();

        IElementGremlinQuery<T1> IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Tail(long count) => TailGlobal(count);

        TTargetQuery IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Union<TTargetQuery>(params Func<IElementGremlinQuery<T1>, TTargetQuery>[] unionTraversals) => Union<TTargetQuery, TTargetQuery>(unionTraversals);
        IGremlinQuery<object> IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Union(params Func<IElementGremlinQuery<T1>, IGremlinQueryBase>[] unionTraversals) => Union<IGremlinQueryBase, IGremlinQuery<object>>(unionTraversals);

        IElementGremlinQuery<T1> IGremlinQueryBaseRec<T1, IElementGremlinQuery<T1>>.Where(Expression<Func<T1, bool>> predicate) => Where(predicate);
        IElementGremlinQuery<T1> IGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Where(Func<IElementGremlinQuery<T1>, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);
        TTargetQuery IGremlinQueryBaseRec<T1, IEdgeOrVertexGremlinQuery<T1>>.Aggregate<TTargetQuery>(Func<IEdgeOrVertexGremlinQuery<T1>, StepLabel<IArrayGremlinQuery<T1[], T1, IEdgeOrVertexGremlinQuery<T1>>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, continuation);
        TTargetQuery IGremlinQueryBaseRec<T1, IEdgeOrVertexGremlinQuery<T1>>.AggregateLocal<TTargetQuery>(Func<IEdgeOrVertexGremlinQuery<T1>, StepLabel<IArrayGremlinQuery<T1[], T1, IEdgeOrVertexGremlinQuery<T1>>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, continuation);

        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<T1, IEdgeOrVertexGremlinQuery<T1>>.Aggregate(StepLabel<IArrayGremlinQuery<T1[], T1, IEdgeOrVertexGremlinQuery<T1>>, T1[]> stepLabel) => Aggregate(Scope.Global, stepLabel);
        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<T1, IEdgeOrVertexGremlinQuery<T1>>.AggregateLocal(StepLabel<IArrayGremlinQuery<T1[], T1, IEdgeOrVertexGremlinQuery<T1>>, T1[]> stepLabel) => Aggregate(Scope.Local, stepLabel);

        TTargetQuery IGremlinQueryBaseRec<T1, IEdgeOrVertexGremlinQuery<T1>>.As<TTargetQuery>(Func<IEdgeOrVertexGremlinQuery<T1>, StepLabel<IEdgeOrVertexGremlinQuery<T1>, T1>, TTargetQuery> continuation) => As<StepLabel<IEdgeOrVertexGremlinQuery<T1>, T1>, TTargetQuery>(continuation);
        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<T1, IEdgeOrVertexGremlinQuery<T1>>.As(StepLabel<T1> stepLabel) => As(stepLabel);

        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.And(params Func<IEdgeOrVertexGremlinQuery<T1>, IGremlinQueryBase>[] andTraversals) => And(andTraversals);

        TTargetQuery IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.As<TTargetQuery>(Func<IEdgeOrVertexGremlinQuery<T1>, StepLabel<IEdgeOrVertexGremlinQuery<T1>, object>, TTargetQuery> continuation) => As<StepLabel<IEdgeOrVertexGremlinQuery<T1>, object>, TTargetQuery>(continuation);

        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Barrier() => Barrier();

        TTargetQuery IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Choose<TTargetQuery>(Func<IEdgeOrVertexGremlinQuery<T1>, IGremlinQueryBase> traversalPredicate, Func<IEdgeOrVertexGremlinQuery<T1>, TTargetQuery> trueChoice, Func<IEdgeOrVertexGremlinQuery<T1>, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(traversalPredicate, trueChoice, falseChoice);
        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Choose(Func<IEdgeOrVertexGremlinQuery<T1>, IGremlinQueryBase> traversalPredicate, Func<IEdgeOrVertexGremlinQuery<T1>, IEdgeOrVertexGremlinQuery<T1>> trueChoice) => Choose<IEdgeOrVertexGremlinQuery<T1>, IEdgeOrVertexGremlinQuery<T1>, IEdgeOrVertexGremlinQuery<T1>>(traversalPredicate, trueChoice);
        IGremlinQuery<object> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Choose(Func<IEdgeOrVertexGremlinQuery<T1>, IGremlinQueryBase> traversalPredicate, Func<IEdgeOrVertexGremlinQuery<T1>, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(traversalPredicate, trueChoice);
        
        TTargetQuery IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Choose<TTargetQuery>(Func<IChooseBuilder<IEdgeOrVertexGremlinQuery<T1>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQueryBaseRec<T1, IEdgeOrVertexGremlinQuery<T1>>.Choose<TTargetQuery>(Expression<Func<T1, bool>> predicate, Func<IEdgeOrVertexGremlinQuery<T1>, TTargetQuery> trueChoice, Func<IEdgeOrVertexGremlinQuery<T1>, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(predicate, trueChoice, falseChoice);
        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<T1, IEdgeOrVertexGremlinQuery<T1>>.Choose(Expression<Func<T1, bool>> predicate, Func<IEdgeOrVertexGremlinQuery<T1>, IEdgeOrVertexGremlinQuery<T1>> trueChoice) => Choose<IEdgeOrVertexGremlinQuery<T1>, IEdgeOrVertexGremlinQuery<T1>, IEdgeOrVertexGremlinQuery<T1>>(predicate, trueChoice);
        IGremlinQuery<object> IGremlinQueryBaseRec<T1, IEdgeOrVertexGremlinQuery<T1>>.Choose(Expression<Func<T1, bool>> predicate, Func<IEdgeOrVertexGremlinQuery<T1>, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(predicate, trueChoice);

        TTargetQuery IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Coalesce<TTargetQuery>(params Func<IEdgeOrVertexGremlinQuery<T1>, TTargetQuery>[] traversals) => Coalesce<TTargetQuery, TTargetQuery>(traversals);
        IGremlinQuery<object> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Coalesce(params Func<IEdgeOrVertexGremlinQuery<T1>, IGremlinQueryBase>[] traversals) => Coalesce<IGremlinQueryBase, IGremlinQuery<object>>(traversals);

        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Coin(double probability) => Coin(probability);

        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.CyclicPath() => CyclicPath();

        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Dedup() => DedupGlobal();

        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.DedupLocal() => DedupLocal();

        TTargetQuery IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.FlatMap<TTargetQuery>(Func<IEdgeOrVertexGremlinQuery<T1>, TTargetQuery> mapping) => FlatMap(mapping);

        IArrayGremlinQuery<T1[], T1, IEdgeOrVertexGremlinQuery<T1>> IGremlinQueryBaseRec<T1, IEdgeOrVertexGremlinQuery<T1>>.ForceArray() => CloneAs<IArrayGremlinQuery<T1[], T1, IEdgeOrVertexGremlinQuery<T1>>>();

        IArrayGremlinQuery<T1[], T1, IEdgeOrVertexGremlinQuery<T1>> IGremlinQueryBaseRec<T1, IEdgeOrVertexGremlinQuery<T1>>.Fold() => Fold<IEdgeOrVertexGremlinQuery<T1>>();

        IMapGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<T1, IEdgeOrVertexGremlinQuery<T1>>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IEdgeOrVertexGremlinQuery<T1>>, IGroupBuilderWithKeyAndValue<TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IMapGremlinQuery<IDictionary<TNewKey, T1[]>> IGremlinQueryBaseRec<T1, IEdgeOrVertexGremlinQuery<T1>>.Group<TNewKey>(Func<IGroupBuilder<IEdgeOrVertexGremlinQuery<T1>>, IGroupBuilderWithKey<IEdgeOrVertexGremlinQuery<T1>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Identity() => Identity();

        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<T1, IEdgeOrVertexGremlinQuery<T1>>.Inject(params T1[] elements) => Inject(elements);
        
        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Limit(long count) => LimitGlobal(count);

        TTargetQuery IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Local<TTargetQuery>(Func<IEdgeOrVertexGremlinQuery<T1> , TTargetQuery> localTraversal) => Local(localTraversal);

        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Loop(Func<IStartLoopBuilder<IEdgeOrVertexGremlinQuery<T1>>, IFinalLoopBuilder<IEdgeOrVertexGremlinQuery<T1>>> loopBuilderTransformation) => Loop(loopBuilderTransformation);

        TTargetQuery IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Map<TTargetQuery>(Func<IEdgeOrVertexGremlinQuery<T1>, TTargetQuery> mapping) => Map(mapping);

        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Max() => MaxGlobal();

        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Mean() => MeanGlobal();

        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Min() => MinGlobal();

        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Not(Func<IEdgeOrVertexGremlinQuery<T1>, IGremlinQueryBase> notTraversal) => Not(static (__, notTraversal) => notTraversal(__), notTraversal);

        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.None() => None();

        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Optional(Func<IEdgeOrVertexGremlinQuery<T1>, IEdgeOrVertexGremlinQuery<T1>> optionalTraversal) => Optional(optionalTraversal);

        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Or(params Func<IEdgeOrVertexGremlinQuery<T1>, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);

        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<T1, IEdgeOrVertexGremlinQuery<T1>>.Order(Func<IOrderBuilder<T1, IEdgeOrVertexGremlinQuery<T1>>, IOrderBuilderWithBy<T1, IEdgeOrVertexGremlinQuery<T1>>> projection) => OrderGlobal(projection);
        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<T1, IEdgeOrVertexGremlinQuery<T1>>.OrderLocal(Func<IOrderBuilder<T1, IEdgeOrVertexGremlinQuery<T1>>, IOrderBuilderWithBy<T1, IEdgeOrVertexGremlinQuery<T1>>> projection) => OrderLocal(projection);

        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Order(Func<IOrderBuilder<IEdgeOrVertexGremlinQuery<T1>>, IOrderBuilderWithBy<IEdgeOrVertexGremlinQuery<T1>>> projection) => OrderGlobal(projection);
        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.OrderLocal(Func<IOrderBuilder<IEdgeOrVertexGremlinQuery<T1>>, IOrderBuilderWithBy<IEdgeOrVertexGremlinQuery<T1>>> projection) => OrderLocal(projection);

        IGremlinQuery<dynamic> IGremlinQueryBaseRec<T1, IEdgeOrVertexGremlinQuery<T1>>.Project(Func<IProjectBuilder<IEdgeOrVertexGremlinQuery<T1>, T1>, IProjectDynamicResult> continuation) => Project(continuation);
        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, IEdgeOrVertexGremlinQuery<T1>>.Project<TResult>(Func<IProjectBuilder<IEdgeOrVertexGremlinQuery<T1>, T1>, IProjectMapResult<TResult>> continuation) => Project(continuation);
        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, IEdgeOrVertexGremlinQuery<T1>>.Project<TResult>(Func<IProjectBuilder<IEdgeOrVertexGremlinQuery<T1>, T1>, IProjectTupleResult<TResult>> continuation) => Project<TResult>(continuation);

        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Range(long low, long high) => RangeGlobal(low, high);

        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.SideEffect(Func<IEdgeOrVertexGremlinQuery<T1>, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.SimplePath() => SimplePath();

        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Skip(long count) => Skip(count, Scope.Global);

        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Sum() => SumGlobal();

        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Tail(long count) => TailGlobal(count);

        TTargetQuery IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Union<TTargetQuery>(params Func<IEdgeOrVertexGremlinQuery<T1>, TTargetQuery>[] unionTraversals) => Union<TTargetQuery, TTargetQuery>(unionTraversals);
        IGremlinQuery<object> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Union(params Func<IEdgeOrVertexGremlinQuery<T1>, IGremlinQueryBase>[] unionTraversals) => Union<IGremlinQueryBase, IGremlinQuery<object>>(unionTraversals);

        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<T1, IEdgeOrVertexGremlinQuery<T1>>.Where(Expression<Func<T1, bool>> predicate) => Where(predicate);
        IEdgeOrVertexGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Where(Func<IEdgeOrVertexGremlinQuery<T1>, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);
        TTargetQuery IGremlinQueryBaseRec<T1, IVertexGremlinQuery<T1>>.Aggregate<TTargetQuery>(Func<IVertexGremlinQuery<T1>, StepLabel<IArrayGremlinQuery<T1[], T1, IVertexGremlinQuery<T1>>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, continuation);
        TTargetQuery IGremlinQueryBaseRec<T1, IVertexGremlinQuery<T1>>.AggregateLocal<TTargetQuery>(Func<IVertexGremlinQuery<T1>, StepLabel<IArrayGremlinQuery<T1[], T1, IVertexGremlinQuery<T1>>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, continuation);

        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<T1, IVertexGremlinQuery<T1>>.Aggregate(StepLabel<IArrayGremlinQuery<T1[], T1, IVertexGremlinQuery<T1>>, T1[]> stepLabel) => Aggregate(Scope.Global, stepLabel);
        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<T1, IVertexGremlinQuery<T1>>.AggregateLocal(StepLabel<IArrayGremlinQuery<T1[], T1, IVertexGremlinQuery<T1>>, T1[]> stepLabel) => Aggregate(Scope.Local, stepLabel);

        TTargetQuery IGremlinQueryBaseRec<T1, IVertexGremlinQuery<T1>>.As<TTargetQuery>(Func<IVertexGremlinQuery<T1>, StepLabel<IVertexGremlinQuery<T1>, T1>, TTargetQuery> continuation) => As<StepLabel<IVertexGremlinQuery<T1>, T1>, TTargetQuery>(continuation);
        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<T1, IVertexGremlinQuery<T1>>.As(StepLabel<T1> stepLabel) => As(stepLabel);

        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.And(params Func<IVertexGremlinQuery<T1>, IGremlinQueryBase>[] andTraversals) => And(andTraversals);

        TTargetQuery IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.As<TTargetQuery>(Func<IVertexGremlinQuery<T1>, StepLabel<IVertexGremlinQuery<T1>, object>, TTargetQuery> continuation) => As<StepLabel<IVertexGremlinQuery<T1>, object>, TTargetQuery>(continuation);

        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Barrier() => Barrier();

        TTargetQuery IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Choose<TTargetQuery>(Func<IVertexGremlinQuery<T1>, IGremlinQueryBase> traversalPredicate, Func<IVertexGremlinQuery<T1>, TTargetQuery> trueChoice, Func<IVertexGremlinQuery<T1>, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(traversalPredicate, trueChoice, falseChoice);
        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Choose(Func<IVertexGremlinQuery<T1>, IGremlinQueryBase> traversalPredicate, Func<IVertexGremlinQuery<T1>, IVertexGremlinQuery<T1>> trueChoice) => Choose<IVertexGremlinQuery<T1>, IVertexGremlinQuery<T1>, IVertexGremlinQuery<T1>>(traversalPredicate, trueChoice);
        IGremlinQuery<object> IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Choose(Func<IVertexGremlinQuery<T1>, IGremlinQueryBase> traversalPredicate, Func<IVertexGremlinQuery<T1>, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(traversalPredicate, trueChoice);
        
        TTargetQuery IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Choose<TTargetQuery>(Func<IChooseBuilder<IVertexGremlinQuery<T1>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQueryBaseRec<T1, IVertexGremlinQuery<T1>>.Choose<TTargetQuery>(Expression<Func<T1, bool>> predicate, Func<IVertexGremlinQuery<T1>, TTargetQuery> trueChoice, Func<IVertexGremlinQuery<T1>, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(predicate, trueChoice, falseChoice);
        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<T1, IVertexGremlinQuery<T1>>.Choose(Expression<Func<T1, bool>> predicate, Func<IVertexGremlinQuery<T1>, IVertexGremlinQuery<T1>> trueChoice) => Choose<IVertexGremlinQuery<T1>, IVertexGremlinQuery<T1>, IVertexGremlinQuery<T1>>(predicate, trueChoice);
        IGremlinQuery<object> IGremlinQueryBaseRec<T1, IVertexGremlinQuery<T1>>.Choose(Expression<Func<T1, bool>> predicate, Func<IVertexGremlinQuery<T1>, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(predicate, trueChoice);

        TTargetQuery IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Coalesce<TTargetQuery>(params Func<IVertexGremlinQuery<T1>, TTargetQuery>[] traversals) => Coalesce<TTargetQuery, TTargetQuery>(traversals);
        IGremlinQuery<object> IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Coalesce(params Func<IVertexGremlinQuery<T1>, IGremlinQueryBase>[] traversals) => Coalesce<IGremlinQueryBase, IGremlinQuery<object>>(traversals);

        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Coin(double probability) => Coin(probability);

        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.CyclicPath() => CyclicPath();

        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Dedup() => DedupGlobal();

        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.DedupLocal() => DedupLocal();

        TTargetQuery IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.FlatMap<TTargetQuery>(Func<IVertexGremlinQuery<T1>, TTargetQuery> mapping) => FlatMap(mapping);

        IArrayGremlinQuery<T1[], T1, IVertexGremlinQuery<T1>> IGremlinQueryBaseRec<T1, IVertexGremlinQuery<T1>>.ForceArray() => CloneAs<IArrayGremlinQuery<T1[], T1, IVertexGremlinQuery<T1>>>();

        IArrayGremlinQuery<T1[], T1, IVertexGremlinQuery<T1>> IGremlinQueryBaseRec<T1, IVertexGremlinQuery<T1>>.Fold() => Fold<IVertexGremlinQuery<T1>>();

        IMapGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<T1, IVertexGremlinQuery<T1>>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IVertexGremlinQuery<T1>>, IGroupBuilderWithKeyAndValue<TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IMapGremlinQuery<IDictionary<TNewKey, T1[]>> IGremlinQueryBaseRec<T1, IVertexGremlinQuery<T1>>.Group<TNewKey>(Func<IGroupBuilder<IVertexGremlinQuery<T1>>, IGroupBuilderWithKey<IVertexGremlinQuery<T1>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Identity() => Identity();

        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<T1, IVertexGremlinQuery<T1>>.Inject(params T1[] elements) => Inject(elements);
        
        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Limit(long count) => LimitGlobal(count);

        TTargetQuery IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Local<TTargetQuery>(Func<IVertexGremlinQuery<T1> , TTargetQuery> localTraversal) => Local(localTraversal);

        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Loop(Func<IStartLoopBuilder<IVertexGremlinQuery<T1>>, IFinalLoopBuilder<IVertexGremlinQuery<T1>>> loopBuilderTransformation) => Loop(loopBuilderTransformation);

        TTargetQuery IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Map<TTargetQuery>(Func<IVertexGremlinQuery<T1>, TTargetQuery> mapping) => Map(mapping);

        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Max() => MaxGlobal();

        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Mean() => MeanGlobal();

        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Min() => MinGlobal();

        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Not(Func<IVertexGremlinQuery<T1>, IGremlinQueryBase> notTraversal) => Not(static (__, notTraversal) => notTraversal(__), notTraversal);

        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.None() => None();

        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Optional(Func<IVertexGremlinQuery<T1>, IVertexGremlinQuery<T1>> optionalTraversal) => Optional(optionalTraversal);

        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Or(params Func<IVertexGremlinQuery<T1>, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);

        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<T1, IVertexGremlinQuery<T1>>.Order(Func<IOrderBuilder<T1, IVertexGremlinQuery<T1>>, IOrderBuilderWithBy<T1, IVertexGremlinQuery<T1>>> projection) => OrderGlobal(projection);
        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<T1, IVertexGremlinQuery<T1>>.OrderLocal(Func<IOrderBuilder<T1, IVertexGremlinQuery<T1>>, IOrderBuilderWithBy<T1, IVertexGremlinQuery<T1>>> projection) => OrderLocal(projection);

        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Order(Func<IOrderBuilder<IVertexGremlinQuery<T1>>, IOrderBuilderWithBy<IVertexGremlinQuery<T1>>> projection) => OrderGlobal(projection);
        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.OrderLocal(Func<IOrderBuilder<IVertexGremlinQuery<T1>>, IOrderBuilderWithBy<IVertexGremlinQuery<T1>>> projection) => OrderLocal(projection);

        IGremlinQuery<dynamic> IGremlinQueryBaseRec<T1, IVertexGremlinQuery<T1>>.Project(Func<IProjectBuilder<IVertexGremlinQuery<T1>, T1>, IProjectDynamicResult> continuation) => Project(continuation);
        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, IVertexGremlinQuery<T1>>.Project<TResult>(Func<IProjectBuilder<IVertexGremlinQuery<T1>, T1>, IProjectMapResult<TResult>> continuation) => Project(continuation);
        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, IVertexGremlinQuery<T1>>.Project<TResult>(Func<IProjectBuilder<IVertexGremlinQuery<T1>, T1>, IProjectTupleResult<TResult>> continuation) => Project<TResult>(continuation);

        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Range(long low, long high) => RangeGlobal(low, high);

        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.SideEffect(Func<IVertexGremlinQuery<T1>, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.SimplePath() => SimplePath();

        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Skip(long count) => Skip(count, Scope.Global);

        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Sum() => SumGlobal();

        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Tail(long count) => TailGlobal(count);

        TTargetQuery IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Union<TTargetQuery>(params Func<IVertexGremlinQuery<T1>, TTargetQuery>[] unionTraversals) => Union<TTargetQuery, TTargetQuery>(unionTraversals);
        IGremlinQuery<object> IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Union(params Func<IVertexGremlinQuery<T1>, IGremlinQueryBase>[] unionTraversals) => Union<IGremlinQueryBase, IGremlinQuery<object>>(unionTraversals);

        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<T1, IVertexGremlinQuery<T1>>.Where(Expression<Func<T1, bool>> predicate) => Where(predicate);
        IVertexGremlinQuery<T1> IGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Where(Func<IVertexGremlinQuery<T1>, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);
        TTargetQuery IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1>>.Aggregate<TTargetQuery>(Func<IEdgeGremlinQuery<T1>, StepLabel<IArrayGremlinQuery<T1[], T1, IEdgeGremlinQuery<T1>>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, continuation);
        TTargetQuery IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1>>.AggregateLocal<TTargetQuery>(Func<IEdgeGremlinQuery<T1>, StepLabel<IArrayGremlinQuery<T1[], T1, IEdgeGremlinQuery<T1>>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, continuation);

        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1>>.Aggregate(StepLabel<IArrayGremlinQuery<T1[], T1, IEdgeGremlinQuery<T1>>, T1[]> stepLabel) => Aggregate(Scope.Global, stepLabel);
        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1>>.AggregateLocal(StepLabel<IArrayGremlinQuery<T1[], T1, IEdgeGremlinQuery<T1>>, T1[]> stepLabel) => Aggregate(Scope.Local, stepLabel);

        TTargetQuery IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1>>.As<TTargetQuery>(Func<IEdgeGremlinQuery<T1>, StepLabel<IEdgeGremlinQuery<T1>, T1>, TTargetQuery> continuation) => As<StepLabel<IEdgeGremlinQuery<T1>, T1>, TTargetQuery>(continuation);
        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1>>.As(StepLabel<T1> stepLabel) => As(stepLabel);

        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.And(params Func<IEdgeGremlinQuery<T1>, IGremlinQueryBase>[] andTraversals) => And(andTraversals);

        TTargetQuery IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.As<TTargetQuery>(Func<IEdgeGremlinQuery<T1>, StepLabel<IEdgeGremlinQuery<T1>, object>, TTargetQuery> continuation) => As<StepLabel<IEdgeGremlinQuery<T1>, object>, TTargetQuery>(continuation);

        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Barrier() => Barrier();

        TTargetQuery IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Choose<TTargetQuery>(Func<IEdgeGremlinQuery<T1>, IGremlinQueryBase> traversalPredicate, Func<IEdgeGremlinQuery<T1>, TTargetQuery> trueChoice, Func<IEdgeGremlinQuery<T1>, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(traversalPredicate, trueChoice, falseChoice);
        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Choose(Func<IEdgeGremlinQuery<T1>, IGremlinQueryBase> traversalPredicate, Func<IEdgeGremlinQuery<T1>, IEdgeGremlinQuery<T1>> trueChoice) => Choose<IEdgeGremlinQuery<T1>, IEdgeGremlinQuery<T1>, IEdgeGremlinQuery<T1>>(traversalPredicate, trueChoice);
        IGremlinQuery<object> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Choose(Func<IEdgeGremlinQuery<T1>, IGremlinQueryBase> traversalPredicate, Func<IEdgeGremlinQuery<T1>, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(traversalPredicate, trueChoice);
        
        TTargetQuery IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Choose<TTargetQuery>(Func<IChooseBuilder<IEdgeGremlinQuery<T1>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1>>.Choose<TTargetQuery>(Expression<Func<T1, bool>> predicate, Func<IEdgeGremlinQuery<T1>, TTargetQuery> trueChoice, Func<IEdgeGremlinQuery<T1>, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(predicate, trueChoice, falseChoice);
        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1>>.Choose(Expression<Func<T1, bool>> predicate, Func<IEdgeGremlinQuery<T1>, IEdgeGremlinQuery<T1>> trueChoice) => Choose<IEdgeGremlinQuery<T1>, IEdgeGremlinQuery<T1>, IEdgeGremlinQuery<T1>>(predicate, trueChoice);
        IGremlinQuery<object> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1>>.Choose(Expression<Func<T1, bool>> predicate, Func<IEdgeGremlinQuery<T1>, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(predicate, trueChoice);

        TTargetQuery IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Coalesce<TTargetQuery>(params Func<IEdgeGremlinQuery<T1>, TTargetQuery>[] traversals) => Coalesce<TTargetQuery, TTargetQuery>(traversals);
        IGremlinQuery<object> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Coalesce(params Func<IEdgeGremlinQuery<T1>, IGremlinQueryBase>[] traversals) => Coalesce<IGremlinQueryBase, IGremlinQuery<object>>(traversals);

        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Coin(double probability) => Coin(probability);

        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.CyclicPath() => CyclicPath();

        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Dedup() => DedupGlobal();

        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.DedupLocal() => DedupLocal();

        TTargetQuery IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.FlatMap<TTargetQuery>(Func<IEdgeGremlinQuery<T1>, TTargetQuery> mapping) => FlatMap(mapping);

        IArrayGremlinQuery<T1[], T1, IEdgeGremlinQuery<T1>> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1>>.ForceArray() => CloneAs<IArrayGremlinQuery<T1[], T1, IEdgeGremlinQuery<T1>>>();

        IArrayGremlinQuery<T1[], T1, IEdgeGremlinQuery<T1>> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1>>.Fold() => Fold<IEdgeGremlinQuery<T1>>();

        IMapGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1>>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IEdgeGremlinQuery<T1>>, IGroupBuilderWithKeyAndValue<TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IMapGremlinQuery<IDictionary<TNewKey, T1[]>> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1>>.Group<TNewKey>(Func<IGroupBuilder<IEdgeGremlinQuery<T1>>, IGroupBuilderWithKey<IEdgeGremlinQuery<T1>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Identity() => Identity();

        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1>>.Inject(params T1[] elements) => Inject(elements);
        
        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Limit(long count) => LimitGlobal(count);

        TTargetQuery IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Local<TTargetQuery>(Func<IEdgeGremlinQuery<T1> , TTargetQuery> localTraversal) => Local(localTraversal);

        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Loop(Func<IStartLoopBuilder<IEdgeGremlinQuery<T1>>, IFinalLoopBuilder<IEdgeGremlinQuery<T1>>> loopBuilderTransformation) => Loop(loopBuilderTransformation);

        TTargetQuery IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Map<TTargetQuery>(Func<IEdgeGremlinQuery<T1>, TTargetQuery> mapping) => Map(mapping);

        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Max() => MaxGlobal();

        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Mean() => MeanGlobal();

        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Min() => MinGlobal();

        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Not(Func<IEdgeGremlinQuery<T1>, IGremlinQueryBase> notTraversal) => Not(static (__, notTraversal) => notTraversal(__), notTraversal);

        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.None() => None();

        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Optional(Func<IEdgeGremlinQuery<T1>, IEdgeGremlinQuery<T1>> optionalTraversal) => Optional(optionalTraversal);

        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Or(params Func<IEdgeGremlinQuery<T1>, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);

        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1>>.Order(Func<IOrderBuilder<T1, IEdgeGremlinQuery<T1>>, IOrderBuilderWithBy<T1, IEdgeGremlinQuery<T1>>> projection) => OrderGlobal(projection);
        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1>>.OrderLocal(Func<IOrderBuilder<T1, IEdgeGremlinQuery<T1>>, IOrderBuilderWithBy<T1, IEdgeGremlinQuery<T1>>> projection) => OrderLocal(projection);

        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Order(Func<IOrderBuilder<IEdgeGremlinQuery<T1>>, IOrderBuilderWithBy<IEdgeGremlinQuery<T1>>> projection) => OrderGlobal(projection);
        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.OrderLocal(Func<IOrderBuilder<IEdgeGremlinQuery<T1>>, IOrderBuilderWithBy<IEdgeGremlinQuery<T1>>> projection) => OrderLocal(projection);

        IGremlinQuery<dynamic> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1>>.Project(Func<IProjectBuilder<IEdgeGremlinQuery<T1>, T1>, IProjectDynamicResult> continuation) => Project(continuation);
        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1>>.Project<TResult>(Func<IProjectBuilder<IEdgeGremlinQuery<T1>, T1>, IProjectMapResult<TResult>> continuation) => Project(continuation);
        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1>>.Project<TResult>(Func<IProjectBuilder<IEdgeGremlinQuery<T1>, T1>, IProjectTupleResult<TResult>> continuation) => Project<TResult>(continuation);

        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Range(long low, long high) => RangeGlobal(low, high);

        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.SideEffect(Func<IEdgeGremlinQuery<T1>, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.SimplePath() => SimplePath();

        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Skip(long count) => Skip(count, Scope.Global);

        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Sum() => SumGlobal();

        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Tail(long count) => TailGlobal(count);

        TTargetQuery IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Union<TTargetQuery>(params Func<IEdgeGremlinQuery<T1>, TTargetQuery>[] unionTraversals) => Union<TTargetQuery, TTargetQuery>(unionTraversals);
        IGremlinQuery<object> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Union(params Func<IEdgeGremlinQuery<T1>, IGremlinQueryBase>[] unionTraversals) => Union<IGremlinQueryBase, IGremlinQuery<object>>(unionTraversals);

        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1>>.Where(Expression<Func<T1, bool>> predicate) => Where(predicate);
        IEdgeGremlinQuery<T1> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Where(Func<IEdgeGremlinQuery<T1>, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);
        TTargetQuery IGremlinQueryBaseRec<T1, IInOrOutEdgeGremlinQuery<T1, T2>>.Aggregate<TTargetQuery>(Func<IInOrOutEdgeGremlinQuery<T1, T2>, StepLabel<IArrayGremlinQuery<T1[], T1, IInOrOutEdgeGremlinQuery<T1, T2>>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, continuation);
        TTargetQuery IGremlinQueryBaseRec<T1, IInOrOutEdgeGremlinQuery<T1, T2>>.AggregateLocal<TTargetQuery>(Func<IInOrOutEdgeGremlinQuery<T1, T2>, StepLabel<IArrayGremlinQuery<T1[], T1, IInOrOutEdgeGremlinQuery<T1, T2>>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, continuation);

        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<T1, IInOrOutEdgeGremlinQuery<T1, T2>>.Aggregate(StepLabel<IArrayGremlinQuery<T1[], T1, IInOrOutEdgeGremlinQuery<T1, T2>>, T1[]> stepLabel) => Aggregate(Scope.Global, stepLabel);
        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<T1, IInOrOutEdgeGremlinQuery<T1, T2>>.AggregateLocal(StepLabel<IArrayGremlinQuery<T1[], T1, IInOrOutEdgeGremlinQuery<T1, T2>>, T1[]> stepLabel) => Aggregate(Scope.Local, stepLabel);

        TTargetQuery IGremlinQueryBaseRec<T1, IInOrOutEdgeGremlinQuery<T1, T2>>.As<TTargetQuery>(Func<IInOrOutEdgeGremlinQuery<T1, T2>, StepLabel<IInOrOutEdgeGremlinQuery<T1, T2>, T1>, TTargetQuery> continuation) => As<StepLabel<IInOrOutEdgeGremlinQuery<T1, T2>, T1>, TTargetQuery>(continuation);
        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<T1, IInOrOutEdgeGremlinQuery<T1, T2>>.As(StepLabel<T1> stepLabel) => As(stepLabel);

        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.And(params Func<IInOrOutEdgeGremlinQuery<T1, T2>, IGremlinQueryBase>[] andTraversals) => And(andTraversals);

        TTargetQuery IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.As<TTargetQuery>(Func<IInOrOutEdgeGremlinQuery<T1, T2>, StepLabel<IInOrOutEdgeGremlinQuery<T1, T2>, object>, TTargetQuery> continuation) => As<StepLabel<IInOrOutEdgeGremlinQuery<T1, T2>, object>, TTargetQuery>(continuation);

        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Barrier() => Barrier();

        TTargetQuery IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Choose<TTargetQuery>(Func<IInOrOutEdgeGremlinQuery<T1, T2>, IGremlinQueryBase> traversalPredicate, Func<IInOrOutEdgeGremlinQuery<T1, T2>, TTargetQuery> trueChoice, Func<IInOrOutEdgeGremlinQuery<T1, T2>, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(traversalPredicate, trueChoice, falseChoice);
        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Choose(Func<IInOrOutEdgeGremlinQuery<T1, T2>, IGremlinQueryBase> traversalPredicate, Func<IInOrOutEdgeGremlinQuery<T1, T2>, IInOrOutEdgeGremlinQuery<T1, T2>> trueChoice) => Choose<IInOrOutEdgeGremlinQuery<T1, T2>, IInOrOutEdgeGremlinQuery<T1, T2>, IInOrOutEdgeGremlinQuery<T1, T2>>(traversalPredicate, trueChoice);
        IGremlinQuery<object> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Choose(Func<IInOrOutEdgeGremlinQuery<T1, T2>, IGremlinQueryBase> traversalPredicate, Func<IInOrOutEdgeGremlinQuery<T1, T2>, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(traversalPredicate, trueChoice);
        
        TTargetQuery IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Choose<TTargetQuery>(Func<IChooseBuilder<IInOrOutEdgeGremlinQuery<T1, T2>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQueryBaseRec<T1, IInOrOutEdgeGremlinQuery<T1, T2>>.Choose<TTargetQuery>(Expression<Func<T1, bool>> predicate, Func<IInOrOutEdgeGremlinQuery<T1, T2>, TTargetQuery> trueChoice, Func<IInOrOutEdgeGremlinQuery<T1, T2>, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(predicate, trueChoice, falseChoice);
        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<T1, IInOrOutEdgeGremlinQuery<T1, T2>>.Choose(Expression<Func<T1, bool>> predicate, Func<IInOrOutEdgeGremlinQuery<T1, T2>, IInOrOutEdgeGremlinQuery<T1, T2>> trueChoice) => Choose<IInOrOutEdgeGremlinQuery<T1, T2>, IInOrOutEdgeGremlinQuery<T1, T2>, IInOrOutEdgeGremlinQuery<T1, T2>>(predicate, trueChoice);
        IGremlinQuery<object> IGremlinQueryBaseRec<T1, IInOrOutEdgeGremlinQuery<T1, T2>>.Choose(Expression<Func<T1, bool>> predicate, Func<IInOrOutEdgeGremlinQuery<T1, T2>, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(predicate, trueChoice);

        TTargetQuery IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Coalesce<TTargetQuery>(params Func<IInOrOutEdgeGremlinQuery<T1, T2>, TTargetQuery>[] traversals) => Coalesce<TTargetQuery, TTargetQuery>(traversals);
        IGremlinQuery<object> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Coalesce(params Func<IInOrOutEdgeGremlinQuery<T1, T2>, IGremlinQueryBase>[] traversals) => Coalesce<IGremlinQueryBase, IGremlinQuery<object>>(traversals);

        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Coin(double probability) => Coin(probability);

        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.CyclicPath() => CyclicPath();

        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Dedup() => DedupGlobal();

        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.DedupLocal() => DedupLocal();

        TTargetQuery IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.FlatMap<TTargetQuery>(Func<IInOrOutEdgeGremlinQuery<T1, T2>, TTargetQuery> mapping) => FlatMap(mapping);

        IArrayGremlinQuery<T1[], T1, IInOrOutEdgeGremlinQuery<T1, T2>> IGremlinQueryBaseRec<T1, IInOrOutEdgeGremlinQuery<T1, T2>>.ForceArray() => CloneAs<IArrayGremlinQuery<T1[], T1, IInOrOutEdgeGremlinQuery<T1, T2>>>();

        IArrayGremlinQuery<T1[], T1, IInOrOutEdgeGremlinQuery<T1, T2>> IGremlinQueryBaseRec<T1, IInOrOutEdgeGremlinQuery<T1, T2>>.Fold() => Fold<IInOrOutEdgeGremlinQuery<T1, T2>>();

        IMapGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<T1, IInOrOutEdgeGremlinQuery<T1, T2>>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IInOrOutEdgeGremlinQuery<T1, T2>>, IGroupBuilderWithKeyAndValue<TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IMapGremlinQuery<IDictionary<TNewKey, T1[]>> IGremlinQueryBaseRec<T1, IInOrOutEdgeGremlinQuery<T1, T2>>.Group<TNewKey>(Func<IGroupBuilder<IInOrOutEdgeGremlinQuery<T1, T2>>, IGroupBuilderWithKey<IInOrOutEdgeGremlinQuery<T1, T2>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Identity() => Identity();

        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<T1, IInOrOutEdgeGremlinQuery<T1, T2>>.Inject(params T1[] elements) => Inject(elements);
        
        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Limit(long count) => LimitGlobal(count);

        TTargetQuery IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Local<TTargetQuery>(Func<IInOrOutEdgeGremlinQuery<T1, T2> , TTargetQuery> localTraversal) => Local(localTraversal);

        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Loop(Func<IStartLoopBuilder<IInOrOutEdgeGremlinQuery<T1, T2>>, IFinalLoopBuilder<IInOrOutEdgeGremlinQuery<T1, T2>>> loopBuilderTransformation) => Loop(loopBuilderTransformation);

        TTargetQuery IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Map<TTargetQuery>(Func<IInOrOutEdgeGremlinQuery<T1, T2>, TTargetQuery> mapping) => Map(mapping);

        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Max() => MaxGlobal();

        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Mean() => MeanGlobal();

        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Min() => MinGlobal();

        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Not(Func<IInOrOutEdgeGremlinQuery<T1, T2>, IGremlinQueryBase> notTraversal) => Not(static (__, notTraversal) => notTraversal(__), notTraversal);

        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.None() => None();

        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Optional(Func<IInOrOutEdgeGremlinQuery<T1, T2>, IInOrOutEdgeGremlinQuery<T1, T2>> optionalTraversal) => Optional(optionalTraversal);

        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Or(params Func<IInOrOutEdgeGremlinQuery<T1, T2>, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);

        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<T1, IInOrOutEdgeGremlinQuery<T1, T2>>.Order(Func<IOrderBuilder<T1, IInOrOutEdgeGremlinQuery<T1, T2>>, IOrderBuilderWithBy<T1, IInOrOutEdgeGremlinQuery<T1, T2>>> projection) => OrderGlobal(projection);
        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<T1, IInOrOutEdgeGremlinQuery<T1, T2>>.OrderLocal(Func<IOrderBuilder<T1, IInOrOutEdgeGremlinQuery<T1, T2>>, IOrderBuilderWithBy<T1, IInOrOutEdgeGremlinQuery<T1, T2>>> projection) => OrderLocal(projection);

        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Order(Func<IOrderBuilder<IInOrOutEdgeGremlinQuery<T1, T2>>, IOrderBuilderWithBy<IInOrOutEdgeGremlinQuery<T1, T2>>> projection) => OrderGlobal(projection);
        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.OrderLocal(Func<IOrderBuilder<IInOrOutEdgeGremlinQuery<T1, T2>>, IOrderBuilderWithBy<IInOrOutEdgeGremlinQuery<T1, T2>>> projection) => OrderLocal(projection);

        IGremlinQuery<dynamic> IGremlinQueryBaseRec<T1, IInOrOutEdgeGremlinQuery<T1, T2>>.Project(Func<IProjectBuilder<IInOrOutEdgeGremlinQuery<T1, T2>, T1>, IProjectDynamicResult> continuation) => Project(continuation);
        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, IInOrOutEdgeGremlinQuery<T1, T2>>.Project<TResult>(Func<IProjectBuilder<IInOrOutEdgeGremlinQuery<T1, T2>, T1>, IProjectMapResult<TResult>> continuation) => Project(continuation);
        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, IInOrOutEdgeGremlinQuery<T1, T2>>.Project<TResult>(Func<IProjectBuilder<IInOrOutEdgeGremlinQuery<T1, T2>, T1>, IProjectTupleResult<TResult>> continuation) => Project<TResult>(continuation);

        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Range(long low, long high) => RangeGlobal(low, high);

        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.SideEffect(Func<IInOrOutEdgeGremlinQuery<T1, T2>, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.SimplePath() => SimplePath();

        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Skip(long count) => Skip(count, Scope.Global);

        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Sum() => SumGlobal();

        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Tail(long count) => TailGlobal(count);

        TTargetQuery IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Union<TTargetQuery>(params Func<IInOrOutEdgeGremlinQuery<T1, T2>, TTargetQuery>[] unionTraversals) => Union<TTargetQuery, TTargetQuery>(unionTraversals);
        IGremlinQuery<object> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Union(params Func<IInOrOutEdgeGremlinQuery<T1, T2>, IGremlinQueryBase>[] unionTraversals) => Union<IGremlinQueryBase, IGremlinQuery<object>>(unionTraversals);

        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<T1, IInOrOutEdgeGremlinQuery<T1, T2>>.Where(Expression<Func<T1, bool>> predicate) => Where(predicate);
        IInOrOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Where(Func<IInOrOutEdgeGremlinQuery<T1, T2>, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);
        TTargetQuery IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1, T2, T3>>.Aggregate<TTargetQuery>(Func<IEdgeGremlinQuery<T1, T2, T3>, StepLabel<IArrayGremlinQuery<T1[], T1, IEdgeGremlinQuery<T1, T2, T3>>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, continuation);
        TTargetQuery IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1, T2, T3>>.AggregateLocal<TTargetQuery>(Func<IEdgeGremlinQuery<T1, T2, T3>, StepLabel<IArrayGremlinQuery<T1[], T1, IEdgeGremlinQuery<T1, T2, T3>>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, continuation);

        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1, T2, T3>>.Aggregate(StepLabel<IArrayGremlinQuery<T1[], T1, IEdgeGremlinQuery<T1, T2, T3>>, T1[]> stepLabel) => Aggregate(Scope.Global, stepLabel);
        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1, T2, T3>>.AggregateLocal(StepLabel<IArrayGremlinQuery<T1[], T1, IEdgeGremlinQuery<T1, T2, T3>>, T1[]> stepLabel) => Aggregate(Scope.Local, stepLabel);

        TTargetQuery IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1, T2, T3>>.As<TTargetQuery>(Func<IEdgeGremlinQuery<T1, T2, T3>, StepLabel<IEdgeGremlinQuery<T1, T2, T3>, T1>, TTargetQuery> continuation) => As<StepLabel<IEdgeGremlinQuery<T1, T2, T3>, T1>, TTargetQuery>(continuation);
        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1, T2, T3>>.As(StepLabel<T1> stepLabel) => As(stepLabel);

        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.And(params Func<IEdgeGremlinQuery<T1, T2, T3>, IGremlinQueryBase>[] andTraversals) => And(andTraversals);

        TTargetQuery IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.As<TTargetQuery>(Func<IEdgeGremlinQuery<T1, T2, T3>, StepLabel<IEdgeGremlinQuery<T1, T2, T3>, object>, TTargetQuery> continuation) => As<StepLabel<IEdgeGremlinQuery<T1, T2, T3>, object>, TTargetQuery>(continuation);

        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Barrier() => Barrier();

        TTargetQuery IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Choose<TTargetQuery>(Func<IEdgeGremlinQuery<T1, T2, T3>, IGremlinQueryBase> traversalPredicate, Func<IEdgeGremlinQuery<T1, T2, T3>, TTargetQuery> trueChoice, Func<IEdgeGremlinQuery<T1, T2, T3>, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(traversalPredicate, trueChoice, falseChoice);
        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Choose(Func<IEdgeGremlinQuery<T1, T2, T3>, IGremlinQueryBase> traversalPredicate, Func<IEdgeGremlinQuery<T1, T2, T3>, IEdgeGremlinQuery<T1, T2, T3>> trueChoice) => Choose<IEdgeGremlinQuery<T1, T2, T3>, IEdgeGremlinQuery<T1, T2, T3>, IEdgeGremlinQuery<T1, T2, T3>>(traversalPredicate, trueChoice);
        IGremlinQuery<object> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Choose(Func<IEdgeGremlinQuery<T1, T2, T3>, IGremlinQueryBase> traversalPredicate, Func<IEdgeGremlinQuery<T1, T2, T3>, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(traversalPredicate, trueChoice);
        
        TTargetQuery IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Choose<TTargetQuery>(Func<IChooseBuilder<IEdgeGremlinQuery<T1, T2, T3>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1, T2, T3>>.Choose<TTargetQuery>(Expression<Func<T1, bool>> predicate, Func<IEdgeGremlinQuery<T1, T2, T3>, TTargetQuery> trueChoice, Func<IEdgeGremlinQuery<T1, T2, T3>, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(predicate, trueChoice, falseChoice);
        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1, T2, T3>>.Choose(Expression<Func<T1, bool>> predicate, Func<IEdgeGremlinQuery<T1, T2, T3>, IEdgeGremlinQuery<T1, T2, T3>> trueChoice) => Choose<IEdgeGremlinQuery<T1, T2, T3>, IEdgeGremlinQuery<T1, T2, T3>, IEdgeGremlinQuery<T1, T2, T3>>(predicate, trueChoice);
        IGremlinQuery<object> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1, T2, T3>>.Choose(Expression<Func<T1, bool>> predicate, Func<IEdgeGremlinQuery<T1, T2, T3>, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(predicate, trueChoice);

        TTargetQuery IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Coalesce<TTargetQuery>(params Func<IEdgeGremlinQuery<T1, T2, T3>, TTargetQuery>[] traversals) => Coalesce<TTargetQuery, TTargetQuery>(traversals);
        IGremlinQuery<object> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Coalesce(params Func<IEdgeGremlinQuery<T1, T2, T3>, IGremlinQueryBase>[] traversals) => Coalesce<IGremlinQueryBase, IGremlinQuery<object>>(traversals);

        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Coin(double probability) => Coin(probability);

        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.CyclicPath() => CyclicPath();

        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Dedup() => DedupGlobal();

        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.DedupLocal() => DedupLocal();

        TTargetQuery IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.FlatMap<TTargetQuery>(Func<IEdgeGremlinQuery<T1, T2, T3>, TTargetQuery> mapping) => FlatMap(mapping);

        IArrayGremlinQuery<T1[], T1, IEdgeGremlinQuery<T1, T2, T3>> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1, T2, T3>>.ForceArray() => CloneAs<IArrayGremlinQuery<T1[], T1, IEdgeGremlinQuery<T1, T2, T3>>>();

        IArrayGremlinQuery<T1[], T1, IEdgeGremlinQuery<T1, T2, T3>> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1, T2, T3>>.Fold() => Fold<IEdgeGremlinQuery<T1, T2, T3>>();

        IMapGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1, T2, T3>>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IEdgeGremlinQuery<T1, T2, T3>>, IGroupBuilderWithKeyAndValue<TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IMapGremlinQuery<IDictionary<TNewKey, T1[]>> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1, T2, T3>>.Group<TNewKey>(Func<IGroupBuilder<IEdgeGremlinQuery<T1, T2, T3>>, IGroupBuilderWithKey<IEdgeGremlinQuery<T1, T2, T3>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Identity() => Identity();

        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1, T2, T3>>.Inject(params T1[] elements) => Inject(elements);
        
        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Limit(long count) => LimitGlobal(count);

        TTargetQuery IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Local<TTargetQuery>(Func<IEdgeGremlinQuery<T1, T2, T3> , TTargetQuery> localTraversal) => Local(localTraversal);

        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Loop(Func<IStartLoopBuilder<IEdgeGremlinQuery<T1, T2, T3>>, IFinalLoopBuilder<IEdgeGremlinQuery<T1, T2, T3>>> loopBuilderTransformation) => Loop(loopBuilderTransformation);

        TTargetQuery IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Map<TTargetQuery>(Func<IEdgeGremlinQuery<T1, T2, T3>, TTargetQuery> mapping) => Map(mapping);

        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Max() => MaxGlobal();

        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Mean() => MeanGlobal();

        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Min() => MinGlobal();

        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Not(Func<IEdgeGremlinQuery<T1, T2, T3>, IGremlinQueryBase> notTraversal) => Not(static (__, notTraversal) => notTraversal(__), notTraversal);

        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.None() => None();

        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Optional(Func<IEdgeGremlinQuery<T1, T2, T3>, IEdgeGremlinQuery<T1, T2, T3>> optionalTraversal) => Optional(optionalTraversal);

        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Or(params Func<IEdgeGremlinQuery<T1, T2, T3>, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);

        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1, T2, T3>>.Order(Func<IOrderBuilder<T1, IEdgeGremlinQuery<T1, T2, T3>>, IOrderBuilderWithBy<T1, IEdgeGremlinQuery<T1, T2, T3>>> projection) => OrderGlobal(projection);
        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1, T2, T3>>.OrderLocal(Func<IOrderBuilder<T1, IEdgeGremlinQuery<T1, T2, T3>>, IOrderBuilderWithBy<T1, IEdgeGremlinQuery<T1, T2, T3>>> projection) => OrderLocal(projection);

        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Order(Func<IOrderBuilder<IEdgeGremlinQuery<T1, T2, T3>>, IOrderBuilderWithBy<IEdgeGremlinQuery<T1, T2, T3>>> projection) => OrderGlobal(projection);
        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.OrderLocal(Func<IOrderBuilder<IEdgeGremlinQuery<T1, T2, T3>>, IOrderBuilderWithBy<IEdgeGremlinQuery<T1, T2, T3>>> projection) => OrderLocal(projection);

        IGremlinQuery<dynamic> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1, T2, T3>>.Project(Func<IProjectBuilder<IEdgeGremlinQuery<T1, T2, T3>, T1>, IProjectDynamicResult> continuation) => Project(continuation);
        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1, T2, T3>>.Project<TResult>(Func<IProjectBuilder<IEdgeGremlinQuery<T1, T2, T3>, T1>, IProjectMapResult<TResult>> continuation) => Project(continuation);
        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1, T2, T3>>.Project<TResult>(Func<IProjectBuilder<IEdgeGremlinQuery<T1, T2, T3>, T1>, IProjectTupleResult<TResult>> continuation) => Project<TResult>(continuation);

        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Range(long low, long high) => RangeGlobal(low, high);

        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.SideEffect(Func<IEdgeGremlinQuery<T1, T2, T3>, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.SimplePath() => SimplePath();

        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Skip(long count) => Skip(count, Scope.Global);

        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Sum() => SumGlobal();

        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Tail(long count) => TailGlobal(count);

        TTargetQuery IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Union<TTargetQuery>(params Func<IEdgeGremlinQuery<T1, T2, T3>, TTargetQuery>[] unionTraversals) => Union<TTargetQuery, TTargetQuery>(unionTraversals);
        IGremlinQuery<object> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Union(params Func<IEdgeGremlinQuery<T1, T2, T3>, IGremlinQueryBase>[] unionTraversals) => Union<IGremlinQueryBase, IGremlinQuery<object>>(unionTraversals);

        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1, T2, T3>>.Where(Expression<Func<T1, bool>> predicate) => Where(predicate);
        IEdgeGremlinQuery<T1, T2, T3> IGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Where(Func<IEdgeGremlinQuery<T1, T2, T3>, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);
        TTargetQuery IGremlinQueryBaseRec<T1, IInEdgeGremlinQuery<T1, T3>>.Aggregate<TTargetQuery>(Func<IInEdgeGremlinQuery<T1, T3>, StepLabel<IArrayGremlinQuery<T1[], T1, IInEdgeGremlinQuery<T1, T3>>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, continuation);
        TTargetQuery IGremlinQueryBaseRec<T1, IInEdgeGremlinQuery<T1, T3>>.AggregateLocal<TTargetQuery>(Func<IInEdgeGremlinQuery<T1, T3>, StepLabel<IArrayGremlinQuery<T1[], T1, IInEdgeGremlinQuery<T1, T3>>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, continuation);

        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<T1, IInEdgeGremlinQuery<T1, T3>>.Aggregate(StepLabel<IArrayGremlinQuery<T1[], T1, IInEdgeGremlinQuery<T1, T3>>, T1[]> stepLabel) => Aggregate(Scope.Global, stepLabel);
        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<T1, IInEdgeGremlinQuery<T1, T3>>.AggregateLocal(StepLabel<IArrayGremlinQuery<T1[], T1, IInEdgeGremlinQuery<T1, T3>>, T1[]> stepLabel) => Aggregate(Scope.Local, stepLabel);

        TTargetQuery IGremlinQueryBaseRec<T1, IInEdgeGremlinQuery<T1, T3>>.As<TTargetQuery>(Func<IInEdgeGremlinQuery<T1, T3>, StepLabel<IInEdgeGremlinQuery<T1, T3>, T1>, TTargetQuery> continuation) => As<StepLabel<IInEdgeGremlinQuery<T1, T3>, T1>, TTargetQuery>(continuation);
        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<T1, IInEdgeGremlinQuery<T1, T3>>.As(StepLabel<T1> stepLabel) => As(stepLabel);

        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.And(params Func<IInEdgeGremlinQuery<T1, T3>, IGremlinQueryBase>[] andTraversals) => And(andTraversals);

        TTargetQuery IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.As<TTargetQuery>(Func<IInEdgeGremlinQuery<T1, T3>, StepLabel<IInEdgeGremlinQuery<T1, T3>, object>, TTargetQuery> continuation) => As<StepLabel<IInEdgeGremlinQuery<T1, T3>, object>, TTargetQuery>(continuation);

        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Barrier() => Barrier();

        TTargetQuery IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Choose<TTargetQuery>(Func<IInEdgeGremlinQuery<T1, T3>, IGremlinQueryBase> traversalPredicate, Func<IInEdgeGremlinQuery<T1, T3>, TTargetQuery> trueChoice, Func<IInEdgeGremlinQuery<T1, T3>, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(traversalPredicate, trueChoice, falseChoice);
        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Choose(Func<IInEdgeGremlinQuery<T1, T3>, IGremlinQueryBase> traversalPredicate, Func<IInEdgeGremlinQuery<T1, T3>, IInEdgeGremlinQuery<T1, T3>> trueChoice) => Choose<IInEdgeGremlinQuery<T1, T3>, IInEdgeGremlinQuery<T1, T3>, IInEdgeGremlinQuery<T1, T3>>(traversalPredicate, trueChoice);
        IGremlinQuery<object> IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Choose(Func<IInEdgeGremlinQuery<T1, T3>, IGremlinQueryBase> traversalPredicate, Func<IInEdgeGremlinQuery<T1, T3>, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(traversalPredicate, trueChoice);
        
        TTargetQuery IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Choose<TTargetQuery>(Func<IChooseBuilder<IInEdgeGremlinQuery<T1, T3>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQueryBaseRec<T1, IInEdgeGremlinQuery<T1, T3>>.Choose<TTargetQuery>(Expression<Func<T1, bool>> predicate, Func<IInEdgeGremlinQuery<T1, T3>, TTargetQuery> trueChoice, Func<IInEdgeGremlinQuery<T1, T3>, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(predicate, trueChoice, falseChoice);
        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<T1, IInEdgeGremlinQuery<T1, T3>>.Choose(Expression<Func<T1, bool>> predicate, Func<IInEdgeGremlinQuery<T1, T3>, IInEdgeGremlinQuery<T1, T3>> trueChoice) => Choose<IInEdgeGremlinQuery<T1, T3>, IInEdgeGremlinQuery<T1, T3>, IInEdgeGremlinQuery<T1, T3>>(predicate, trueChoice);
        IGremlinQuery<object> IGremlinQueryBaseRec<T1, IInEdgeGremlinQuery<T1, T3>>.Choose(Expression<Func<T1, bool>> predicate, Func<IInEdgeGremlinQuery<T1, T3>, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(predicate, trueChoice);

        TTargetQuery IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Coalesce<TTargetQuery>(params Func<IInEdgeGremlinQuery<T1, T3>, TTargetQuery>[] traversals) => Coalesce<TTargetQuery, TTargetQuery>(traversals);
        IGremlinQuery<object> IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Coalesce(params Func<IInEdgeGremlinQuery<T1, T3>, IGremlinQueryBase>[] traversals) => Coalesce<IGremlinQueryBase, IGremlinQuery<object>>(traversals);

        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Coin(double probability) => Coin(probability);

        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.CyclicPath() => CyclicPath();

        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Dedup() => DedupGlobal();

        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.DedupLocal() => DedupLocal();

        TTargetQuery IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.FlatMap<TTargetQuery>(Func<IInEdgeGremlinQuery<T1, T3>, TTargetQuery> mapping) => FlatMap(mapping);

        IArrayGremlinQuery<T1[], T1, IInEdgeGremlinQuery<T1, T3>> IGremlinQueryBaseRec<T1, IInEdgeGremlinQuery<T1, T3>>.ForceArray() => CloneAs<IArrayGremlinQuery<T1[], T1, IInEdgeGremlinQuery<T1, T3>>>();

        IArrayGremlinQuery<T1[], T1, IInEdgeGremlinQuery<T1, T3>> IGremlinQueryBaseRec<T1, IInEdgeGremlinQuery<T1, T3>>.Fold() => Fold<IInEdgeGremlinQuery<T1, T3>>();

        IMapGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<T1, IInEdgeGremlinQuery<T1, T3>>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IInEdgeGremlinQuery<T1, T3>>, IGroupBuilderWithKeyAndValue<TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IMapGremlinQuery<IDictionary<TNewKey, T1[]>> IGremlinQueryBaseRec<T1, IInEdgeGremlinQuery<T1, T3>>.Group<TNewKey>(Func<IGroupBuilder<IInEdgeGremlinQuery<T1, T3>>, IGroupBuilderWithKey<IInEdgeGremlinQuery<T1, T3>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Identity() => Identity();

        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<T1, IInEdgeGremlinQuery<T1, T3>>.Inject(params T1[] elements) => Inject(elements);
        
        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Limit(long count) => LimitGlobal(count);

        TTargetQuery IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Local<TTargetQuery>(Func<IInEdgeGremlinQuery<T1, T3> , TTargetQuery> localTraversal) => Local(localTraversal);

        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Loop(Func<IStartLoopBuilder<IInEdgeGremlinQuery<T1, T3>>, IFinalLoopBuilder<IInEdgeGremlinQuery<T1, T3>>> loopBuilderTransformation) => Loop(loopBuilderTransformation);

        TTargetQuery IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Map<TTargetQuery>(Func<IInEdgeGremlinQuery<T1, T3>, TTargetQuery> mapping) => Map(mapping);

        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Max() => MaxGlobal();

        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Mean() => MeanGlobal();

        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Min() => MinGlobal();

        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Not(Func<IInEdgeGremlinQuery<T1, T3>, IGremlinQueryBase> notTraversal) => Not(static (__, notTraversal) => notTraversal(__), notTraversal);

        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.None() => None();

        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Optional(Func<IInEdgeGremlinQuery<T1, T3>, IInEdgeGremlinQuery<T1, T3>> optionalTraversal) => Optional(optionalTraversal);

        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Or(params Func<IInEdgeGremlinQuery<T1, T3>, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);

        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<T1, IInEdgeGremlinQuery<T1, T3>>.Order(Func<IOrderBuilder<T1, IInEdgeGremlinQuery<T1, T3>>, IOrderBuilderWithBy<T1, IInEdgeGremlinQuery<T1, T3>>> projection) => OrderGlobal(projection);
        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<T1, IInEdgeGremlinQuery<T1, T3>>.OrderLocal(Func<IOrderBuilder<T1, IInEdgeGremlinQuery<T1, T3>>, IOrderBuilderWithBy<T1, IInEdgeGremlinQuery<T1, T3>>> projection) => OrderLocal(projection);

        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Order(Func<IOrderBuilder<IInEdgeGremlinQuery<T1, T3>>, IOrderBuilderWithBy<IInEdgeGremlinQuery<T1, T3>>> projection) => OrderGlobal(projection);
        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.OrderLocal(Func<IOrderBuilder<IInEdgeGremlinQuery<T1, T3>>, IOrderBuilderWithBy<IInEdgeGremlinQuery<T1, T3>>> projection) => OrderLocal(projection);

        IGremlinQuery<dynamic> IGremlinQueryBaseRec<T1, IInEdgeGremlinQuery<T1, T3>>.Project(Func<IProjectBuilder<IInEdgeGremlinQuery<T1, T3>, T1>, IProjectDynamicResult> continuation) => Project(continuation);
        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, IInEdgeGremlinQuery<T1, T3>>.Project<TResult>(Func<IProjectBuilder<IInEdgeGremlinQuery<T1, T3>, T1>, IProjectMapResult<TResult>> continuation) => Project(continuation);
        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, IInEdgeGremlinQuery<T1, T3>>.Project<TResult>(Func<IProjectBuilder<IInEdgeGremlinQuery<T1, T3>, T1>, IProjectTupleResult<TResult>> continuation) => Project<TResult>(continuation);

        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Range(long low, long high) => RangeGlobal(low, high);

        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.SideEffect(Func<IInEdgeGremlinQuery<T1, T3>, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.SimplePath() => SimplePath();

        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Skip(long count) => Skip(count, Scope.Global);

        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Sum() => SumGlobal();

        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Tail(long count) => TailGlobal(count);

        TTargetQuery IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Union<TTargetQuery>(params Func<IInEdgeGremlinQuery<T1, T3>, TTargetQuery>[] unionTraversals) => Union<TTargetQuery, TTargetQuery>(unionTraversals);
        IGremlinQuery<object> IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Union(params Func<IInEdgeGremlinQuery<T1, T3>, IGremlinQueryBase>[] unionTraversals) => Union<IGremlinQueryBase, IGremlinQuery<object>>(unionTraversals);

        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<T1, IInEdgeGremlinQuery<T1, T3>>.Where(Expression<Func<T1, bool>> predicate) => Where(predicate);
        IInEdgeGremlinQuery<T1, T3> IGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Where(Func<IInEdgeGremlinQuery<T1, T3>, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);
        TTargetQuery IGremlinQueryBaseRec<T1, IOutEdgeGremlinQuery<T1, T2>>.Aggregate<TTargetQuery>(Func<IOutEdgeGremlinQuery<T1, T2>, StepLabel<IArrayGremlinQuery<T1[], T1, IOutEdgeGremlinQuery<T1, T2>>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, continuation);
        TTargetQuery IGremlinQueryBaseRec<T1, IOutEdgeGremlinQuery<T1, T2>>.AggregateLocal<TTargetQuery>(Func<IOutEdgeGremlinQuery<T1, T2>, StepLabel<IArrayGremlinQuery<T1[], T1, IOutEdgeGremlinQuery<T1, T2>>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, continuation);

        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<T1, IOutEdgeGremlinQuery<T1, T2>>.Aggregate(StepLabel<IArrayGremlinQuery<T1[], T1, IOutEdgeGremlinQuery<T1, T2>>, T1[]> stepLabel) => Aggregate(Scope.Global, stepLabel);
        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<T1, IOutEdgeGremlinQuery<T1, T2>>.AggregateLocal(StepLabel<IArrayGremlinQuery<T1[], T1, IOutEdgeGremlinQuery<T1, T2>>, T1[]> stepLabel) => Aggregate(Scope.Local, stepLabel);

        TTargetQuery IGremlinQueryBaseRec<T1, IOutEdgeGremlinQuery<T1, T2>>.As<TTargetQuery>(Func<IOutEdgeGremlinQuery<T1, T2>, StepLabel<IOutEdgeGremlinQuery<T1, T2>, T1>, TTargetQuery> continuation) => As<StepLabel<IOutEdgeGremlinQuery<T1, T2>, T1>, TTargetQuery>(continuation);
        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<T1, IOutEdgeGremlinQuery<T1, T2>>.As(StepLabel<T1> stepLabel) => As(stepLabel);

        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.And(params Func<IOutEdgeGremlinQuery<T1, T2>, IGremlinQueryBase>[] andTraversals) => And(andTraversals);

        TTargetQuery IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.As<TTargetQuery>(Func<IOutEdgeGremlinQuery<T1, T2>, StepLabel<IOutEdgeGremlinQuery<T1, T2>, object>, TTargetQuery> continuation) => As<StepLabel<IOutEdgeGremlinQuery<T1, T2>, object>, TTargetQuery>(continuation);

        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Barrier() => Barrier();

        TTargetQuery IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Choose<TTargetQuery>(Func<IOutEdgeGremlinQuery<T1, T2>, IGremlinQueryBase> traversalPredicate, Func<IOutEdgeGremlinQuery<T1, T2>, TTargetQuery> trueChoice, Func<IOutEdgeGremlinQuery<T1, T2>, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(traversalPredicate, trueChoice, falseChoice);
        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Choose(Func<IOutEdgeGremlinQuery<T1, T2>, IGremlinQueryBase> traversalPredicate, Func<IOutEdgeGremlinQuery<T1, T2>, IOutEdgeGremlinQuery<T1, T2>> trueChoice) => Choose<IOutEdgeGremlinQuery<T1, T2>, IOutEdgeGremlinQuery<T1, T2>, IOutEdgeGremlinQuery<T1, T2>>(traversalPredicate, trueChoice);
        IGremlinQuery<object> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Choose(Func<IOutEdgeGremlinQuery<T1, T2>, IGremlinQueryBase> traversalPredicate, Func<IOutEdgeGremlinQuery<T1, T2>, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(traversalPredicate, trueChoice);
        
        TTargetQuery IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Choose<TTargetQuery>(Func<IChooseBuilder<IOutEdgeGremlinQuery<T1, T2>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQueryBaseRec<T1, IOutEdgeGremlinQuery<T1, T2>>.Choose<TTargetQuery>(Expression<Func<T1, bool>> predicate, Func<IOutEdgeGremlinQuery<T1, T2>, TTargetQuery> trueChoice, Func<IOutEdgeGremlinQuery<T1, T2>, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(predicate, trueChoice, falseChoice);
        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<T1, IOutEdgeGremlinQuery<T1, T2>>.Choose(Expression<Func<T1, bool>> predicate, Func<IOutEdgeGremlinQuery<T1, T2>, IOutEdgeGremlinQuery<T1, T2>> trueChoice) => Choose<IOutEdgeGremlinQuery<T1, T2>, IOutEdgeGremlinQuery<T1, T2>, IOutEdgeGremlinQuery<T1, T2>>(predicate, trueChoice);
        IGremlinQuery<object> IGremlinQueryBaseRec<T1, IOutEdgeGremlinQuery<T1, T2>>.Choose(Expression<Func<T1, bool>> predicate, Func<IOutEdgeGremlinQuery<T1, T2>, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(predicate, trueChoice);

        TTargetQuery IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Coalesce<TTargetQuery>(params Func<IOutEdgeGremlinQuery<T1, T2>, TTargetQuery>[] traversals) => Coalesce<TTargetQuery, TTargetQuery>(traversals);
        IGremlinQuery<object> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Coalesce(params Func<IOutEdgeGremlinQuery<T1, T2>, IGremlinQueryBase>[] traversals) => Coalesce<IGremlinQueryBase, IGremlinQuery<object>>(traversals);

        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Coin(double probability) => Coin(probability);

        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.CyclicPath() => CyclicPath();

        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Dedup() => DedupGlobal();

        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.DedupLocal() => DedupLocal();

        TTargetQuery IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.FlatMap<TTargetQuery>(Func<IOutEdgeGremlinQuery<T1, T2>, TTargetQuery> mapping) => FlatMap(mapping);

        IArrayGremlinQuery<T1[], T1, IOutEdgeGremlinQuery<T1, T2>> IGremlinQueryBaseRec<T1, IOutEdgeGremlinQuery<T1, T2>>.ForceArray() => CloneAs<IArrayGremlinQuery<T1[], T1, IOutEdgeGremlinQuery<T1, T2>>>();

        IArrayGremlinQuery<T1[], T1, IOutEdgeGremlinQuery<T1, T2>> IGremlinQueryBaseRec<T1, IOutEdgeGremlinQuery<T1, T2>>.Fold() => Fold<IOutEdgeGremlinQuery<T1, T2>>();

        IMapGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<T1, IOutEdgeGremlinQuery<T1, T2>>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IOutEdgeGremlinQuery<T1, T2>>, IGroupBuilderWithKeyAndValue<TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IMapGremlinQuery<IDictionary<TNewKey, T1[]>> IGremlinQueryBaseRec<T1, IOutEdgeGremlinQuery<T1, T2>>.Group<TNewKey>(Func<IGroupBuilder<IOutEdgeGremlinQuery<T1, T2>>, IGroupBuilderWithKey<IOutEdgeGremlinQuery<T1, T2>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Identity() => Identity();

        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<T1, IOutEdgeGremlinQuery<T1, T2>>.Inject(params T1[] elements) => Inject(elements);
        
        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Limit(long count) => LimitGlobal(count);

        TTargetQuery IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Local<TTargetQuery>(Func<IOutEdgeGremlinQuery<T1, T2> , TTargetQuery> localTraversal) => Local(localTraversal);

        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Loop(Func<IStartLoopBuilder<IOutEdgeGremlinQuery<T1, T2>>, IFinalLoopBuilder<IOutEdgeGremlinQuery<T1, T2>>> loopBuilderTransformation) => Loop(loopBuilderTransformation);

        TTargetQuery IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Map<TTargetQuery>(Func<IOutEdgeGremlinQuery<T1, T2>, TTargetQuery> mapping) => Map(mapping);

        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Max() => MaxGlobal();

        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Mean() => MeanGlobal();

        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Min() => MinGlobal();

        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Not(Func<IOutEdgeGremlinQuery<T1, T2>, IGremlinQueryBase> notTraversal) => Not(static (__, notTraversal) => notTraversal(__), notTraversal);

        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.None() => None();

        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Optional(Func<IOutEdgeGremlinQuery<T1, T2>, IOutEdgeGremlinQuery<T1, T2>> optionalTraversal) => Optional(optionalTraversal);

        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Or(params Func<IOutEdgeGremlinQuery<T1, T2>, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);

        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<T1, IOutEdgeGremlinQuery<T1, T2>>.Order(Func<IOrderBuilder<T1, IOutEdgeGremlinQuery<T1, T2>>, IOrderBuilderWithBy<T1, IOutEdgeGremlinQuery<T1, T2>>> projection) => OrderGlobal(projection);
        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<T1, IOutEdgeGremlinQuery<T1, T2>>.OrderLocal(Func<IOrderBuilder<T1, IOutEdgeGremlinQuery<T1, T2>>, IOrderBuilderWithBy<T1, IOutEdgeGremlinQuery<T1, T2>>> projection) => OrderLocal(projection);

        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Order(Func<IOrderBuilder<IOutEdgeGremlinQuery<T1, T2>>, IOrderBuilderWithBy<IOutEdgeGremlinQuery<T1, T2>>> projection) => OrderGlobal(projection);
        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.OrderLocal(Func<IOrderBuilder<IOutEdgeGremlinQuery<T1, T2>>, IOrderBuilderWithBy<IOutEdgeGremlinQuery<T1, T2>>> projection) => OrderLocal(projection);

        IGremlinQuery<dynamic> IGremlinQueryBaseRec<T1, IOutEdgeGremlinQuery<T1, T2>>.Project(Func<IProjectBuilder<IOutEdgeGremlinQuery<T1, T2>, T1>, IProjectDynamicResult> continuation) => Project(continuation);
        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, IOutEdgeGremlinQuery<T1, T2>>.Project<TResult>(Func<IProjectBuilder<IOutEdgeGremlinQuery<T1, T2>, T1>, IProjectMapResult<TResult>> continuation) => Project(continuation);
        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, IOutEdgeGremlinQuery<T1, T2>>.Project<TResult>(Func<IProjectBuilder<IOutEdgeGremlinQuery<T1, T2>, T1>, IProjectTupleResult<TResult>> continuation) => Project<TResult>(continuation);

        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Range(long low, long high) => RangeGlobal(low, high);

        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.SideEffect(Func<IOutEdgeGremlinQuery<T1, T2>, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.SimplePath() => SimplePath();

        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Skip(long count) => Skip(count, Scope.Global);

        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Sum() => SumGlobal();

        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Tail(long count) => TailGlobal(count);

        TTargetQuery IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Union<TTargetQuery>(params Func<IOutEdgeGremlinQuery<T1, T2>, TTargetQuery>[] unionTraversals) => Union<TTargetQuery, TTargetQuery>(unionTraversals);
        IGremlinQuery<object> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Union(params Func<IOutEdgeGremlinQuery<T1, T2>, IGremlinQueryBase>[] unionTraversals) => Union<IGremlinQueryBase, IGremlinQuery<object>>(unionTraversals);

        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<T1, IOutEdgeGremlinQuery<T1, T2>>.Where(Expression<Func<T1, bool>> predicate) => Where(predicate);
        IOutEdgeGremlinQuery<T1, T2> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Where(Func<IOutEdgeGremlinQuery<T1, T2>, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);
        TTargetQuery IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4>>.Aggregate<TTargetQuery>(Func<IVertexPropertyGremlinQuery<T1, T4>, StepLabel<IArrayGremlinQuery<T1[], T1, IVertexPropertyGremlinQuery<T1, T4>>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, continuation);
        TTargetQuery IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4>>.AggregateLocal<TTargetQuery>(Func<IVertexPropertyGremlinQuery<T1, T4>, StepLabel<IArrayGremlinQuery<T1[], T1, IVertexPropertyGremlinQuery<T1, T4>>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, continuation);

        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4>>.Aggregate(StepLabel<IArrayGremlinQuery<T1[], T1, IVertexPropertyGremlinQuery<T1, T4>>, T1[]> stepLabel) => Aggregate(Scope.Global, stepLabel);
        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4>>.AggregateLocal(StepLabel<IArrayGremlinQuery<T1[], T1, IVertexPropertyGremlinQuery<T1, T4>>, T1[]> stepLabel) => Aggregate(Scope.Local, stepLabel);

        TTargetQuery IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4>>.As<TTargetQuery>(Func<IVertexPropertyGremlinQuery<T1, T4>, StepLabel<IVertexPropertyGremlinQuery<T1, T4>, T1>, TTargetQuery> continuation) => As<StepLabel<IVertexPropertyGremlinQuery<T1, T4>, T1>, TTargetQuery>(continuation);
        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4>>.As(StepLabel<T1> stepLabel) => As(stepLabel);

        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.And(params Func<IVertexPropertyGremlinQuery<T1, T4>, IGremlinQueryBase>[] andTraversals) => And(andTraversals);

        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.As<TTargetQuery>(Func<IVertexPropertyGremlinQuery<T1, T4>, StepLabel<IVertexPropertyGremlinQuery<T1, T4>, object>, TTargetQuery> continuation) => As<StepLabel<IVertexPropertyGremlinQuery<T1, T4>, object>, TTargetQuery>(continuation);

        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Barrier() => Barrier();

        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Choose<TTargetQuery>(Func<IVertexPropertyGremlinQuery<T1, T4>, IGremlinQueryBase> traversalPredicate, Func<IVertexPropertyGremlinQuery<T1, T4>, TTargetQuery> trueChoice, Func<IVertexPropertyGremlinQuery<T1, T4>, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(traversalPredicate, trueChoice, falseChoice);
        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Choose(Func<IVertexPropertyGremlinQuery<T1, T4>, IGremlinQueryBase> traversalPredicate, Func<IVertexPropertyGremlinQuery<T1, T4>, IVertexPropertyGremlinQuery<T1, T4>> trueChoice) => Choose<IVertexPropertyGremlinQuery<T1, T4>, IVertexPropertyGremlinQuery<T1, T4>, IVertexPropertyGremlinQuery<T1, T4>>(traversalPredicate, trueChoice);
        IGremlinQuery<object> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Choose(Func<IVertexPropertyGremlinQuery<T1, T4>, IGremlinQueryBase> traversalPredicate, Func<IVertexPropertyGremlinQuery<T1, T4>, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(traversalPredicate, trueChoice);
        
        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Choose<TTargetQuery>(Func<IChooseBuilder<IVertexPropertyGremlinQuery<T1, T4>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4>>.Choose<TTargetQuery>(Expression<Func<T1, bool>> predicate, Func<IVertexPropertyGremlinQuery<T1, T4>, TTargetQuery> trueChoice, Func<IVertexPropertyGremlinQuery<T1, T4>, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(predicate, trueChoice, falseChoice);
        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4>>.Choose(Expression<Func<T1, bool>> predicate, Func<IVertexPropertyGremlinQuery<T1, T4>, IVertexPropertyGremlinQuery<T1, T4>> trueChoice) => Choose<IVertexPropertyGremlinQuery<T1, T4>, IVertexPropertyGremlinQuery<T1, T4>, IVertexPropertyGremlinQuery<T1, T4>>(predicate, trueChoice);
        IGremlinQuery<object> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4>>.Choose(Expression<Func<T1, bool>> predicate, Func<IVertexPropertyGremlinQuery<T1, T4>, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(predicate, trueChoice);

        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Coalesce<TTargetQuery>(params Func<IVertexPropertyGremlinQuery<T1, T4>, TTargetQuery>[] traversals) => Coalesce<TTargetQuery, TTargetQuery>(traversals);
        IGremlinQuery<object> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Coalesce(params Func<IVertexPropertyGremlinQuery<T1, T4>, IGremlinQueryBase>[] traversals) => Coalesce<IGremlinQueryBase, IGremlinQuery<object>>(traversals);

        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Coin(double probability) => Coin(probability);

        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.CyclicPath() => CyclicPath();

        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Dedup() => DedupGlobal();

        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.DedupLocal() => DedupLocal();

        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.FlatMap<TTargetQuery>(Func<IVertexPropertyGremlinQuery<T1, T4>, TTargetQuery> mapping) => FlatMap(mapping);

        IArrayGremlinQuery<T1[], T1, IVertexPropertyGremlinQuery<T1, T4>> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4>>.ForceArray() => CloneAs<IArrayGremlinQuery<T1[], T1, IVertexPropertyGremlinQuery<T1, T4>>>();

        IArrayGremlinQuery<T1[], T1, IVertexPropertyGremlinQuery<T1, T4>> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4>>.Fold() => Fold<IVertexPropertyGremlinQuery<T1, T4>>();

        IMapGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4>>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IVertexPropertyGremlinQuery<T1, T4>>, IGroupBuilderWithKeyAndValue<TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IMapGremlinQuery<IDictionary<TNewKey, T1[]>> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4>>.Group<TNewKey>(Func<IGroupBuilder<IVertexPropertyGremlinQuery<T1, T4>>, IGroupBuilderWithKey<IVertexPropertyGremlinQuery<T1, T4>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Identity() => Identity();

        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4>>.Inject(params T1[] elements) => Inject(elements);
        
        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Limit(long count) => LimitGlobal(count);

        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Local<TTargetQuery>(Func<IVertexPropertyGremlinQuery<T1, T4> , TTargetQuery> localTraversal) => Local(localTraversal);

        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Loop(Func<IStartLoopBuilder<IVertexPropertyGremlinQuery<T1, T4>>, IFinalLoopBuilder<IVertexPropertyGremlinQuery<T1, T4>>> loopBuilderTransformation) => Loop(loopBuilderTransformation);

        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Map<TTargetQuery>(Func<IVertexPropertyGremlinQuery<T1, T4>, TTargetQuery> mapping) => Map(mapping);

        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Max() => MaxGlobal();

        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Mean() => MeanGlobal();

        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Min() => MinGlobal();

        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Not(Func<IVertexPropertyGremlinQuery<T1, T4>, IGremlinQueryBase> notTraversal) => Not(static (__, notTraversal) => notTraversal(__), notTraversal);

        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.None() => None();

        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Optional(Func<IVertexPropertyGremlinQuery<T1, T4>, IVertexPropertyGremlinQuery<T1, T4>> optionalTraversal) => Optional(optionalTraversal);

        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Or(params Func<IVertexPropertyGremlinQuery<T1, T4>, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);

        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4>>.Order(Func<IOrderBuilder<T1, IVertexPropertyGremlinQuery<T1, T4>>, IOrderBuilderWithBy<T1, IVertexPropertyGremlinQuery<T1, T4>>> projection) => OrderGlobal(projection);
        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4>>.OrderLocal(Func<IOrderBuilder<T1, IVertexPropertyGremlinQuery<T1, T4>>, IOrderBuilderWithBy<T1, IVertexPropertyGremlinQuery<T1, T4>>> projection) => OrderLocal(projection);

        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Order(Func<IOrderBuilder<IVertexPropertyGremlinQuery<T1, T4>>, IOrderBuilderWithBy<IVertexPropertyGremlinQuery<T1, T4>>> projection) => OrderGlobal(projection);
        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.OrderLocal(Func<IOrderBuilder<IVertexPropertyGremlinQuery<T1, T4>>, IOrderBuilderWithBy<IVertexPropertyGremlinQuery<T1, T4>>> projection) => OrderLocal(projection);

        IGremlinQuery<dynamic> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4>>.Project(Func<IProjectBuilder<IVertexPropertyGremlinQuery<T1, T4>, T1>, IProjectDynamicResult> continuation) => Project(continuation);
        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4>>.Project<TResult>(Func<IProjectBuilder<IVertexPropertyGremlinQuery<T1, T4>, T1>, IProjectMapResult<TResult>> continuation) => Project(continuation);
        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4>>.Project<TResult>(Func<IProjectBuilder<IVertexPropertyGremlinQuery<T1, T4>, T1>, IProjectTupleResult<TResult>> continuation) => Project<TResult>(continuation);

        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Range(long low, long high) => RangeGlobal(low, high);

        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.SideEffect(Func<IVertexPropertyGremlinQuery<T1, T4>, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.SimplePath() => SimplePath();

        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Skip(long count) => Skip(count, Scope.Global);

        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Sum() => SumGlobal();

        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Tail(long count) => TailGlobal(count);

        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Union<TTargetQuery>(params Func<IVertexPropertyGremlinQuery<T1, T4>, TTargetQuery>[] unionTraversals) => Union<TTargetQuery, TTargetQuery>(unionTraversals);
        IGremlinQuery<object> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Union(params Func<IVertexPropertyGremlinQuery<T1, T4>, IGremlinQueryBase>[] unionTraversals) => Union<IGremlinQueryBase, IGremlinQuery<object>>(unionTraversals);

        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4>>.Where(Expression<Func<T1, bool>> predicate) => Where(predicate);
        IVertexPropertyGremlinQuery<T1, T4> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Where(Func<IVertexPropertyGremlinQuery<T1, T4>, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);
        TTargetQuery IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4, T5>>.Aggregate<TTargetQuery>(Func<IVertexPropertyGremlinQuery<T1, T4, T5>, StepLabel<IArrayGremlinQuery<T1[], T1, IVertexPropertyGremlinQuery<T1, T4, T5>>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, continuation);
        TTargetQuery IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4, T5>>.AggregateLocal<TTargetQuery>(Func<IVertexPropertyGremlinQuery<T1, T4, T5>, StepLabel<IArrayGremlinQuery<T1[], T1, IVertexPropertyGremlinQuery<T1, T4, T5>>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, continuation);

        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4, T5>>.Aggregate(StepLabel<IArrayGremlinQuery<T1[], T1, IVertexPropertyGremlinQuery<T1, T4, T5>>, T1[]> stepLabel) => Aggregate(Scope.Global, stepLabel);
        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4, T5>>.AggregateLocal(StepLabel<IArrayGremlinQuery<T1[], T1, IVertexPropertyGremlinQuery<T1, T4, T5>>, T1[]> stepLabel) => Aggregate(Scope.Local, stepLabel);

        TTargetQuery IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4, T5>>.As<TTargetQuery>(Func<IVertexPropertyGremlinQuery<T1, T4, T5>, StepLabel<IVertexPropertyGremlinQuery<T1, T4, T5>, T1>, TTargetQuery> continuation) => As<StepLabel<IVertexPropertyGremlinQuery<T1, T4, T5>, T1>, TTargetQuery>(continuation);
        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4, T5>>.As(StepLabel<T1> stepLabel) => As(stepLabel);

        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.And(params Func<IVertexPropertyGremlinQuery<T1, T4, T5>, IGremlinQueryBase>[] andTraversals) => And(andTraversals);

        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.As<TTargetQuery>(Func<IVertexPropertyGremlinQuery<T1, T4, T5>, StepLabel<IVertexPropertyGremlinQuery<T1, T4, T5>, object>, TTargetQuery> continuation) => As<StepLabel<IVertexPropertyGremlinQuery<T1, T4, T5>, object>, TTargetQuery>(continuation);

        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Barrier() => Barrier();

        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Choose<TTargetQuery>(Func<IVertexPropertyGremlinQuery<T1, T4, T5>, IGremlinQueryBase> traversalPredicate, Func<IVertexPropertyGremlinQuery<T1, T4, T5>, TTargetQuery> trueChoice, Func<IVertexPropertyGremlinQuery<T1, T4, T5>, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(traversalPredicate, trueChoice, falseChoice);
        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Choose(Func<IVertexPropertyGremlinQuery<T1, T4, T5>, IGremlinQueryBase> traversalPredicate, Func<IVertexPropertyGremlinQuery<T1, T4, T5>, IVertexPropertyGremlinQuery<T1, T4, T5>> trueChoice) => Choose<IVertexPropertyGremlinQuery<T1, T4, T5>, IVertexPropertyGremlinQuery<T1, T4, T5>, IVertexPropertyGremlinQuery<T1, T4, T5>>(traversalPredicate, trueChoice);
        IGremlinQuery<object> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Choose(Func<IVertexPropertyGremlinQuery<T1, T4, T5>, IGremlinQueryBase> traversalPredicate, Func<IVertexPropertyGremlinQuery<T1, T4, T5>, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(traversalPredicate, trueChoice);
        
        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Choose<TTargetQuery>(Func<IChooseBuilder<IVertexPropertyGremlinQuery<T1, T4, T5>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4, T5>>.Choose<TTargetQuery>(Expression<Func<T1, bool>> predicate, Func<IVertexPropertyGremlinQuery<T1, T4, T5>, TTargetQuery> trueChoice, Func<IVertexPropertyGremlinQuery<T1, T4, T5>, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(predicate, trueChoice, falseChoice);
        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4, T5>>.Choose(Expression<Func<T1, bool>> predicate, Func<IVertexPropertyGremlinQuery<T1, T4, T5>, IVertexPropertyGremlinQuery<T1, T4, T5>> trueChoice) => Choose<IVertexPropertyGremlinQuery<T1, T4, T5>, IVertexPropertyGremlinQuery<T1, T4, T5>, IVertexPropertyGremlinQuery<T1, T4, T5>>(predicate, trueChoice);
        IGremlinQuery<object> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4, T5>>.Choose(Expression<Func<T1, bool>> predicate, Func<IVertexPropertyGremlinQuery<T1, T4, T5>, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(predicate, trueChoice);

        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Coalesce<TTargetQuery>(params Func<IVertexPropertyGremlinQuery<T1, T4, T5>, TTargetQuery>[] traversals) => Coalesce<TTargetQuery, TTargetQuery>(traversals);
        IGremlinQuery<object> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Coalesce(params Func<IVertexPropertyGremlinQuery<T1, T4, T5>, IGremlinQueryBase>[] traversals) => Coalesce<IGremlinQueryBase, IGremlinQuery<object>>(traversals);

        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Coin(double probability) => Coin(probability);

        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.CyclicPath() => CyclicPath();

        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Dedup() => DedupGlobal();

        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.DedupLocal() => DedupLocal();

        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.FlatMap<TTargetQuery>(Func<IVertexPropertyGremlinQuery<T1, T4, T5>, TTargetQuery> mapping) => FlatMap(mapping);

        IArrayGremlinQuery<T1[], T1, IVertexPropertyGremlinQuery<T1, T4, T5>> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4, T5>>.ForceArray() => CloneAs<IArrayGremlinQuery<T1[], T1, IVertexPropertyGremlinQuery<T1, T4, T5>>>();

        IArrayGremlinQuery<T1[], T1, IVertexPropertyGremlinQuery<T1, T4, T5>> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4, T5>>.Fold() => Fold<IVertexPropertyGremlinQuery<T1, T4, T5>>();

        IMapGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4, T5>>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IVertexPropertyGremlinQuery<T1, T4, T5>>, IGroupBuilderWithKeyAndValue<TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IMapGremlinQuery<IDictionary<TNewKey, T1[]>> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4, T5>>.Group<TNewKey>(Func<IGroupBuilder<IVertexPropertyGremlinQuery<T1, T4, T5>>, IGroupBuilderWithKey<IVertexPropertyGremlinQuery<T1, T4, T5>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Identity() => Identity();

        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4, T5>>.Inject(params T1[] elements) => Inject(elements);
        
        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Limit(long count) => LimitGlobal(count);

        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Local<TTargetQuery>(Func<IVertexPropertyGremlinQuery<T1, T4, T5> , TTargetQuery> localTraversal) => Local(localTraversal);

        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Loop(Func<IStartLoopBuilder<IVertexPropertyGremlinQuery<T1, T4, T5>>, IFinalLoopBuilder<IVertexPropertyGremlinQuery<T1, T4, T5>>> loopBuilderTransformation) => Loop(loopBuilderTransformation);

        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Map<TTargetQuery>(Func<IVertexPropertyGremlinQuery<T1, T4, T5>, TTargetQuery> mapping) => Map(mapping);

        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Max() => MaxGlobal();

        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Mean() => MeanGlobal();

        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Min() => MinGlobal();

        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Not(Func<IVertexPropertyGremlinQuery<T1, T4, T5>, IGremlinQueryBase> notTraversal) => Not(static (__, notTraversal) => notTraversal(__), notTraversal);

        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.None() => None();

        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Optional(Func<IVertexPropertyGremlinQuery<T1, T4, T5>, IVertexPropertyGremlinQuery<T1, T4, T5>> optionalTraversal) => Optional(optionalTraversal);

        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Or(params Func<IVertexPropertyGremlinQuery<T1, T4, T5>, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);

        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4, T5>>.Order(Func<IOrderBuilder<T1, IVertexPropertyGremlinQuery<T1, T4, T5>>, IOrderBuilderWithBy<T1, IVertexPropertyGremlinQuery<T1, T4, T5>>> projection) => OrderGlobal(projection);
        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4, T5>>.OrderLocal(Func<IOrderBuilder<T1, IVertexPropertyGremlinQuery<T1, T4, T5>>, IOrderBuilderWithBy<T1, IVertexPropertyGremlinQuery<T1, T4, T5>>> projection) => OrderLocal(projection);

        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Order(Func<IOrderBuilder<IVertexPropertyGremlinQuery<T1, T4, T5>>, IOrderBuilderWithBy<IVertexPropertyGremlinQuery<T1, T4, T5>>> projection) => OrderGlobal(projection);
        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.OrderLocal(Func<IOrderBuilder<IVertexPropertyGremlinQuery<T1, T4, T5>>, IOrderBuilderWithBy<IVertexPropertyGremlinQuery<T1, T4, T5>>> projection) => OrderLocal(projection);

        IGremlinQuery<dynamic> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4, T5>>.Project(Func<IProjectBuilder<IVertexPropertyGremlinQuery<T1, T4, T5>, T1>, IProjectDynamicResult> continuation) => Project(continuation);
        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4, T5>>.Project<TResult>(Func<IProjectBuilder<IVertexPropertyGremlinQuery<T1, T4, T5>, T1>, IProjectMapResult<TResult>> continuation) => Project(continuation);
        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4, T5>>.Project<TResult>(Func<IProjectBuilder<IVertexPropertyGremlinQuery<T1, T4, T5>, T1>, IProjectTupleResult<TResult>> continuation) => Project<TResult>(continuation);

        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Range(long low, long high) => RangeGlobal(low, high);

        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.SideEffect(Func<IVertexPropertyGremlinQuery<T1, T4, T5>, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.SimplePath() => SimplePath();

        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Skip(long count) => Skip(count, Scope.Global);

        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Sum() => SumGlobal();

        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Tail(long count) => TailGlobal(count);

        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Union<TTargetQuery>(params Func<IVertexPropertyGremlinQuery<T1, T4, T5>, TTargetQuery>[] unionTraversals) => Union<TTargetQuery, TTargetQuery>(unionTraversals);
        IGremlinQuery<object> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Union(params Func<IVertexPropertyGremlinQuery<T1, T4, T5>, IGremlinQueryBase>[] unionTraversals) => Union<IGremlinQueryBase, IGremlinQuery<object>>(unionTraversals);

        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4, T5>>.Where(Expression<Func<T1, bool>> predicate) => Where(predicate);
        IVertexPropertyGremlinQuery<T1, T4, T5> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Where(Func<IVertexPropertyGremlinQuery<T1, T4, T5>, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);
        TTargetQuery IGremlinQueryBaseRec<T1, IPropertyGremlinQuery<T1>>.Aggregate<TTargetQuery>(Func<IPropertyGremlinQuery<T1>, StepLabel<IArrayGremlinQuery<T1[], T1, IPropertyGremlinQuery<T1>>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, continuation);
        TTargetQuery IGremlinQueryBaseRec<T1, IPropertyGremlinQuery<T1>>.AggregateLocal<TTargetQuery>(Func<IPropertyGremlinQuery<T1>, StepLabel<IArrayGremlinQuery<T1[], T1, IPropertyGremlinQuery<T1>>, T1[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, continuation);

        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<T1, IPropertyGremlinQuery<T1>>.Aggregate(StepLabel<IArrayGremlinQuery<T1[], T1, IPropertyGremlinQuery<T1>>, T1[]> stepLabel) => Aggregate(Scope.Global, stepLabel);
        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<T1, IPropertyGremlinQuery<T1>>.AggregateLocal(StepLabel<IArrayGremlinQuery<T1[], T1, IPropertyGremlinQuery<T1>>, T1[]> stepLabel) => Aggregate(Scope.Local, stepLabel);

        TTargetQuery IGremlinQueryBaseRec<T1, IPropertyGremlinQuery<T1>>.As<TTargetQuery>(Func<IPropertyGremlinQuery<T1>, StepLabel<IPropertyGremlinQuery<T1>, T1>, TTargetQuery> continuation) => As<StepLabel<IPropertyGremlinQuery<T1>, T1>, TTargetQuery>(continuation);
        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<T1, IPropertyGremlinQuery<T1>>.As(StepLabel<T1> stepLabel) => As(stepLabel);

        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.And(params Func<IPropertyGremlinQuery<T1>, IGremlinQueryBase>[] andTraversals) => And(andTraversals);

        TTargetQuery IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.As<TTargetQuery>(Func<IPropertyGremlinQuery<T1>, StepLabel<IPropertyGremlinQuery<T1>, object>, TTargetQuery> continuation) => As<StepLabel<IPropertyGremlinQuery<T1>, object>, TTargetQuery>(continuation);

        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.Barrier() => Barrier();

        TTargetQuery IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.Choose<TTargetQuery>(Func<IPropertyGremlinQuery<T1>, IGremlinQueryBase> traversalPredicate, Func<IPropertyGremlinQuery<T1>, TTargetQuery> trueChoice, Func<IPropertyGremlinQuery<T1>, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(traversalPredicate, trueChoice, falseChoice);
        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.Choose(Func<IPropertyGremlinQuery<T1>, IGremlinQueryBase> traversalPredicate, Func<IPropertyGremlinQuery<T1>, IPropertyGremlinQuery<T1>> trueChoice) => Choose<IPropertyGremlinQuery<T1>, IPropertyGremlinQuery<T1>, IPropertyGremlinQuery<T1>>(traversalPredicate, trueChoice);
        IGremlinQuery<object> IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.Choose(Func<IPropertyGremlinQuery<T1>, IGremlinQueryBase> traversalPredicate, Func<IPropertyGremlinQuery<T1>, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(traversalPredicate, trueChoice);
        
        TTargetQuery IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.Choose<TTargetQuery>(Func<IChooseBuilder<IPropertyGremlinQuery<T1>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQueryBaseRec<T1, IPropertyGremlinQuery<T1>>.Choose<TTargetQuery>(Expression<Func<T1, bool>> predicate, Func<IPropertyGremlinQuery<T1>, TTargetQuery> trueChoice, Func<IPropertyGremlinQuery<T1>, TTargetQuery> falseChoice) => Choose<TTargetQuery, TTargetQuery, TTargetQuery>(predicate, trueChoice, falseChoice);
        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<T1, IPropertyGremlinQuery<T1>>.Choose(Expression<Func<T1, bool>> predicate, Func<IPropertyGremlinQuery<T1>, IPropertyGremlinQuery<T1>> trueChoice) => Choose<IPropertyGremlinQuery<T1>, IPropertyGremlinQuery<T1>, IPropertyGremlinQuery<T1>>(predicate, trueChoice);
        IGremlinQuery<object> IGremlinQueryBaseRec<T1, IPropertyGremlinQuery<T1>>.Choose(Expression<Func<T1, bool>> predicate, Func<IPropertyGremlinQuery<T1>, IGremlinQueryBase> trueChoice) => Choose<IGremlinQueryBase, IGremlinQueryBase, IGremlinQuery<object>>(predicate, trueChoice);

        TTargetQuery IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.Coalesce<TTargetQuery>(params Func<IPropertyGremlinQuery<T1>, TTargetQuery>[] traversals) => Coalesce<TTargetQuery, TTargetQuery>(traversals);
        IGremlinQuery<object> IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.Coalesce(params Func<IPropertyGremlinQuery<T1>, IGremlinQueryBase>[] traversals) => Coalesce<IGremlinQueryBase, IGremlinQuery<object>>(traversals);

        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.Coin(double probability) => Coin(probability);

        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.CyclicPath() => CyclicPath();

        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.Dedup() => DedupGlobal();

        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.DedupLocal() => DedupLocal();

        TTargetQuery IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.FlatMap<TTargetQuery>(Func<IPropertyGremlinQuery<T1>, TTargetQuery> mapping) => FlatMap(mapping);

        IArrayGremlinQuery<T1[], T1, IPropertyGremlinQuery<T1>> IGremlinQueryBaseRec<T1, IPropertyGremlinQuery<T1>>.ForceArray() => CloneAs<IArrayGremlinQuery<T1[], T1, IPropertyGremlinQuery<T1>>>();

        IArrayGremlinQuery<T1[], T1, IPropertyGremlinQuery<T1>> IGremlinQueryBaseRec<T1, IPropertyGremlinQuery<T1>>.Fold() => Fold<IPropertyGremlinQuery<T1>>();

        IMapGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<T1, IPropertyGremlinQuery<T1>>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IPropertyGremlinQuery<T1>>, IGroupBuilderWithKeyAndValue<TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IMapGremlinQuery<IDictionary<TNewKey, T1[]>> IGremlinQueryBaseRec<T1, IPropertyGremlinQuery<T1>>.Group<TNewKey>(Func<IGroupBuilder<IPropertyGremlinQuery<T1>>, IGroupBuilderWithKey<IPropertyGremlinQuery<T1>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.Identity() => Identity();

        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<T1, IPropertyGremlinQuery<T1>>.Inject(params T1[] elements) => Inject(elements);
        
        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.Limit(long count) => LimitGlobal(count);

        TTargetQuery IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.Local<TTargetQuery>(Func<IPropertyGremlinQuery<T1> , TTargetQuery> localTraversal) => Local(localTraversal);

        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.Loop(Func<IStartLoopBuilder<IPropertyGremlinQuery<T1>>, IFinalLoopBuilder<IPropertyGremlinQuery<T1>>> loopBuilderTransformation) => Loop(loopBuilderTransformation);

        TTargetQuery IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.Map<TTargetQuery>(Func<IPropertyGremlinQuery<T1>, TTargetQuery> mapping) => Map(mapping);

        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.Max() => MaxGlobal();

        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.Mean() => MeanGlobal();

        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.Min() => MinGlobal();

        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.Not(Func<IPropertyGremlinQuery<T1>, IGremlinQueryBase> notTraversal) => Not(static (__, notTraversal) => notTraversal(__), notTraversal);

        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.None() => None();

        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.Optional(Func<IPropertyGremlinQuery<T1>, IPropertyGremlinQuery<T1>> optionalTraversal) => Optional(optionalTraversal);

        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.Or(params Func<IPropertyGremlinQuery<T1>, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);

        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<T1, IPropertyGremlinQuery<T1>>.Order(Func<IOrderBuilder<T1, IPropertyGremlinQuery<T1>>, IOrderBuilderWithBy<T1, IPropertyGremlinQuery<T1>>> projection) => OrderGlobal(projection);
        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<T1, IPropertyGremlinQuery<T1>>.OrderLocal(Func<IOrderBuilder<T1, IPropertyGremlinQuery<T1>>, IOrderBuilderWithBy<T1, IPropertyGremlinQuery<T1>>> projection) => OrderLocal(projection);

        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.Order(Func<IOrderBuilder<IPropertyGremlinQuery<T1>>, IOrderBuilderWithBy<IPropertyGremlinQuery<T1>>> projection) => OrderGlobal(projection);
        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.OrderLocal(Func<IOrderBuilder<IPropertyGremlinQuery<T1>>, IOrderBuilderWithBy<IPropertyGremlinQuery<T1>>> projection) => OrderLocal(projection);

        IGremlinQuery<dynamic> IGremlinQueryBaseRec<T1, IPropertyGremlinQuery<T1>>.Project(Func<IProjectBuilder<IPropertyGremlinQuery<T1>, T1>, IProjectDynamicResult> continuation) => Project(continuation);
        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, IPropertyGremlinQuery<T1>>.Project<TResult>(Func<IProjectBuilder<IPropertyGremlinQuery<T1>, T1>, IProjectMapResult<TResult>> continuation) => Project(continuation);
        IMapGremlinQuery<TResult> IGremlinQueryBaseRec<T1, IPropertyGremlinQuery<T1>>.Project<TResult>(Func<IProjectBuilder<IPropertyGremlinQuery<T1>, T1>, IProjectTupleResult<TResult>> continuation) => Project<TResult>(continuation);

        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.Range(long low, long high) => RangeGlobal(low, high);

        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.SideEffect(Func<IPropertyGremlinQuery<T1>, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.SimplePath() => SimplePath();

        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.Skip(long count) => Skip(count, Scope.Global);

        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.Sum() => SumGlobal();

        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.Tail(long count) => TailGlobal(count);

        TTargetQuery IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.Union<TTargetQuery>(params Func<IPropertyGremlinQuery<T1>, TTargetQuery>[] unionTraversals) => Union<TTargetQuery, TTargetQuery>(unionTraversals);
        IGremlinQuery<object> IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.Union(params Func<IPropertyGremlinQuery<T1>, IGremlinQueryBase>[] unionTraversals) => Union<IGremlinQueryBase, IGremlinQuery<object>>(unionTraversals);

        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<T1, IPropertyGremlinQuery<T1>>.Where(Expression<Func<T1, bool>> predicate) => Where(predicate);
        IPropertyGremlinQuery<T1> IGremlinQueryBaseRec<IPropertyGremlinQuery<T1>>.Where(Func<IPropertyGremlinQuery<T1>, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);


        IVertexGremlinQuery<TTarget> IVertexGremlinQueryBase.OfType<TTarget>() => OfType<TTarget>(Environment.Model.VerticesModel);
        IEdgeGremlinQuery<TTarget> IEdgeGremlinQueryBase.OfType<TTarget>() => OfType<TTarget>(Environment.Model.EdgesModel);


        IElementGremlinQuery<T1> IElementGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Property(string key, object? value) => Property(key, value);
        IElementGremlinQuery<T1> IElementGremlinQueryBaseRec<IElementGremlinQuery<T1>>.Property(string key, Func<IElementGremlinQuery<T1>, IGremlinQueryBase> valueTransformation) => Property(key, valueTransformation);

        IElementGremlinQuery<T1> IElementGremlinQueryBaseRec<T1, IElementGremlinQuery<T1>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, TProjectedValue value) => Property(projection, value);
        IElementGremlinQuery<T1> IElementGremlinQueryBaseRec<T1, IElementGremlinQuery<T1>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, StepLabel<TProjectedValue> stepLabel) => Property(projection, __ => __.Select(stepLabel));
        IElementGremlinQuery<T1> IElementGremlinQueryBaseRec<T1, IElementGremlinQuery<T1>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, Func<IElementGremlinQuery<T1>, IGremlinQueryBase<TProjectedValue>> valueTraversal) => Property(projection, valueTraversal);

        IElementGremlinQuery<T1> IElementGremlinQueryBaseRec<T1, IElementGremlinQuery<T1>>.Where<TProjection>(Expression<Func<T1, TProjection>> projection, Func<IGremlinQueryBase<TProjection>, IGremlinQueryBase> propertyTraversal) => Where(projection, propertyTraversal);
        IEdgeOrVertexGremlinQuery<T1> IElementGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Property(string key, object? value) => Property(key, value);
        IEdgeOrVertexGremlinQuery<T1> IElementGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<T1>>.Property(string key, Func<IEdgeOrVertexGremlinQuery<T1>, IGremlinQueryBase> valueTransformation) => Property(key, valueTransformation);

        IEdgeOrVertexGremlinQuery<T1> IElementGremlinQueryBaseRec<T1, IEdgeOrVertexGremlinQuery<T1>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, TProjectedValue value) => Property(projection, value);
        IEdgeOrVertexGremlinQuery<T1> IElementGremlinQueryBaseRec<T1, IEdgeOrVertexGremlinQuery<T1>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, StepLabel<TProjectedValue> stepLabel) => Property(projection, __ => __.Select(stepLabel));
        IEdgeOrVertexGremlinQuery<T1> IElementGremlinQueryBaseRec<T1, IEdgeOrVertexGremlinQuery<T1>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, Func<IEdgeOrVertexGremlinQuery<T1>, IGremlinQueryBase<TProjectedValue>> valueTraversal) => Property(projection, valueTraversal);

        IEdgeOrVertexGremlinQuery<T1> IElementGremlinQueryBaseRec<T1, IEdgeOrVertexGremlinQuery<T1>>.Where<TProjection>(Expression<Func<T1, TProjection>> projection, Func<IGremlinQueryBase<TProjection>, IGremlinQueryBase> propertyTraversal) => Where(projection, propertyTraversal);
        IVertexGremlinQuery<T1> IElementGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Property(string key, object? value) => Property(key, value);
        IVertexGremlinQuery<T1> IElementGremlinQueryBaseRec<IVertexGremlinQuery<T1>>.Property(string key, Func<IVertexGremlinQuery<T1>, IGremlinQueryBase> valueTransformation) => Property(key, valueTransformation);

        IVertexGremlinQuery<T1> IElementGremlinQueryBaseRec<T1, IVertexGremlinQuery<T1>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, TProjectedValue value) => Property(projection, value);
        IVertexGremlinQuery<T1> IElementGremlinQueryBaseRec<T1, IVertexGremlinQuery<T1>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, StepLabel<TProjectedValue> stepLabel) => Property(projection, __ => __.Select(stepLabel));
        IVertexGremlinQuery<T1> IElementGremlinQueryBaseRec<T1, IVertexGremlinQuery<T1>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, Func<IVertexGremlinQuery<T1>, IGremlinQueryBase<TProjectedValue>> valueTraversal) => Property(projection, valueTraversal);

        IVertexGremlinQuery<T1> IElementGremlinQueryBaseRec<T1, IVertexGremlinQuery<T1>>.Where<TProjection>(Expression<Func<T1, TProjection>> projection, Func<IGremlinQueryBase<TProjection>, IGremlinQueryBase> propertyTraversal) => Where(projection, propertyTraversal);
        IEdgeGremlinQuery<T1> IElementGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Property(string key, object? value) => Property(key, value);
        IEdgeGremlinQuery<T1> IElementGremlinQueryBaseRec<IEdgeGremlinQuery<T1>>.Property(string key, Func<IEdgeGremlinQuery<T1>, IGremlinQueryBase> valueTransformation) => Property(key, valueTransformation);

        IEdgeGremlinQuery<T1> IElementGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, TProjectedValue value) => Property(projection, value);
        IEdgeGremlinQuery<T1> IElementGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, StepLabel<TProjectedValue> stepLabel) => Property(projection, __ => __.Select(stepLabel));
        IEdgeGremlinQuery<T1> IElementGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, Func<IEdgeGremlinQuery<T1>, IGremlinQueryBase<TProjectedValue>> valueTraversal) => Property(projection, valueTraversal);

        IEdgeGremlinQuery<T1> IElementGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1>>.Where<TProjection>(Expression<Func<T1, TProjection>> projection, Func<IGremlinQueryBase<TProjection>, IGremlinQueryBase> propertyTraversal) => Where(projection, propertyTraversal);
        IInOrOutEdgeGremlinQuery<T1, T2> IElementGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Property(string key, object? value) => Property(key, value);
        IInOrOutEdgeGremlinQuery<T1, T2> IElementGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<T1, T2>>.Property(string key, Func<IInOrOutEdgeGremlinQuery<T1, T2>, IGremlinQueryBase> valueTransformation) => Property(key, valueTransformation);

        IInOrOutEdgeGremlinQuery<T1, T2> IElementGremlinQueryBaseRec<T1, IInOrOutEdgeGremlinQuery<T1, T2>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, TProjectedValue value) => Property(projection, value);
        IInOrOutEdgeGremlinQuery<T1, T2> IElementGremlinQueryBaseRec<T1, IInOrOutEdgeGremlinQuery<T1, T2>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, StepLabel<TProjectedValue> stepLabel) => Property(projection, __ => __.Select(stepLabel));
        IInOrOutEdgeGremlinQuery<T1, T2> IElementGremlinQueryBaseRec<T1, IInOrOutEdgeGremlinQuery<T1, T2>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, Func<IInOrOutEdgeGremlinQuery<T1, T2>, IGremlinQueryBase<TProjectedValue>> valueTraversal) => Property(projection, valueTraversal);

        IInOrOutEdgeGremlinQuery<T1, T2> IElementGremlinQueryBaseRec<T1, IInOrOutEdgeGremlinQuery<T1, T2>>.Where<TProjection>(Expression<Func<T1, TProjection>> projection, Func<IGremlinQueryBase<TProjection>, IGremlinQueryBase> propertyTraversal) => Where(projection, propertyTraversal);
        IEdgeGremlinQuery<T1, T2, T3> IElementGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Property(string key, object? value) => Property(key, value);
        IEdgeGremlinQuery<T1, T2, T3> IElementGremlinQueryBaseRec<IEdgeGremlinQuery<T1, T2, T3>>.Property(string key, Func<IEdgeGremlinQuery<T1, T2, T3>, IGremlinQueryBase> valueTransformation) => Property(key, valueTransformation);

        IEdgeGremlinQuery<T1, T2, T3> IElementGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1, T2, T3>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, TProjectedValue value) => Property(projection, value);
        IEdgeGremlinQuery<T1, T2, T3> IElementGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1, T2, T3>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, StepLabel<TProjectedValue> stepLabel) => Property(projection, __ => __.Select(stepLabel));
        IEdgeGremlinQuery<T1, T2, T3> IElementGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1, T2, T3>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, Func<IEdgeGremlinQuery<T1, T2, T3>, IGremlinQueryBase<TProjectedValue>> valueTraversal) => Property(projection, valueTraversal);

        IEdgeGremlinQuery<T1, T2, T3> IElementGremlinQueryBaseRec<T1, IEdgeGremlinQuery<T1, T2, T3>>.Where<TProjection>(Expression<Func<T1, TProjection>> projection, Func<IGremlinQueryBase<TProjection>, IGremlinQueryBase> propertyTraversal) => Where(projection, propertyTraversal);
        IInEdgeGremlinQuery<T1, T3> IElementGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Property(string key, object? value) => Property(key, value);
        IInEdgeGremlinQuery<T1, T3> IElementGremlinQueryBaseRec<IInEdgeGremlinQuery<T1, T3>>.Property(string key, Func<IInEdgeGremlinQuery<T1, T3>, IGremlinQueryBase> valueTransformation) => Property(key, valueTransformation);

        IInEdgeGremlinQuery<T1, T3> IElementGremlinQueryBaseRec<T1, IInEdgeGremlinQuery<T1, T3>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, TProjectedValue value) => Property(projection, value);
        IInEdgeGremlinQuery<T1, T3> IElementGremlinQueryBaseRec<T1, IInEdgeGremlinQuery<T1, T3>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, StepLabel<TProjectedValue> stepLabel) => Property(projection, __ => __.Select(stepLabel));
        IInEdgeGremlinQuery<T1, T3> IElementGremlinQueryBaseRec<T1, IInEdgeGremlinQuery<T1, T3>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, Func<IInEdgeGremlinQuery<T1, T3>, IGremlinQueryBase<TProjectedValue>> valueTraversal) => Property(projection, valueTraversal);

        IInEdgeGremlinQuery<T1, T3> IElementGremlinQueryBaseRec<T1, IInEdgeGremlinQuery<T1, T3>>.Where<TProjection>(Expression<Func<T1, TProjection>> projection, Func<IGremlinQueryBase<TProjection>, IGremlinQueryBase> propertyTraversal) => Where(projection, propertyTraversal);
        IOutEdgeGremlinQuery<T1, T2> IElementGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Property(string key, object? value) => Property(key, value);
        IOutEdgeGremlinQuery<T1, T2> IElementGremlinQueryBaseRec<IOutEdgeGremlinQuery<T1, T2>>.Property(string key, Func<IOutEdgeGremlinQuery<T1, T2>, IGremlinQueryBase> valueTransformation) => Property(key, valueTransformation);

        IOutEdgeGremlinQuery<T1, T2> IElementGremlinQueryBaseRec<T1, IOutEdgeGremlinQuery<T1, T2>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, TProjectedValue value) => Property(projection, value);
        IOutEdgeGremlinQuery<T1, T2> IElementGremlinQueryBaseRec<T1, IOutEdgeGremlinQuery<T1, T2>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, StepLabel<TProjectedValue> stepLabel) => Property(projection, __ => __.Select(stepLabel));
        IOutEdgeGremlinQuery<T1, T2> IElementGremlinQueryBaseRec<T1, IOutEdgeGremlinQuery<T1, T2>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, Func<IOutEdgeGremlinQuery<T1, T2>, IGremlinQueryBase<TProjectedValue>> valueTraversal) => Property(projection, valueTraversal);

        IOutEdgeGremlinQuery<T1, T2> IElementGremlinQueryBaseRec<T1, IOutEdgeGremlinQuery<T1, T2>>.Where<TProjection>(Expression<Func<T1, TProjection>> projection, Func<IGremlinQueryBase<TProjection>, IGremlinQueryBase> propertyTraversal) => Where(projection, propertyTraversal);
        IVertexPropertyGremlinQuery<T1, T4> IElementGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Property(string key, object? value) => Property(key, value);
        IVertexPropertyGremlinQuery<T1, T4> IElementGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4>>.Property(string key, Func<IVertexPropertyGremlinQuery<T1, T4>, IGremlinQueryBase> valueTransformation) => Property(key, valueTransformation);

        IVertexPropertyGremlinQuery<T1, T4> IElementGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, TProjectedValue value) => Property(projection, value);
        IVertexPropertyGremlinQuery<T1, T4> IElementGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, StepLabel<TProjectedValue> stepLabel) => Property(projection, __ => __.Select(stepLabel));
        IVertexPropertyGremlinQuery<T1, T4> IElementGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, Func<IVertexPropertyGremlinQuery<T1, T4>, IGremlinQueryBase<TProjectedValue>> valueTraversal) => Property(projection, valueTraversal);

        IVertexPropertyGremlinQuery<T1, T4> IElementGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4>>.Where<TProjection>(Expression<Func<T1, TProjection>> projection, Func<IGremlinQueryBase<TProjection>, IGremlinQueryBase> propertyTraversal) => Where(projection, propertyTraversal);
        IVertexPropertyGremlinQuery<T1, T4, T5> IElementGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Property(string key, object? value) => Property(key, value);
        IVertexPropertyGremlinQuery<T1, T4, T5> IElementGremlinQueryBaseRec<IVertexPropertyGremlinQuery<T1, T4, T5>>.Property(string key, Func<IVertexPropertyGremlinQuery<T1, T4, T5>, IGremlinQueryBase> valueTransformation) => Property(key, valueTransformation);

        IVertexPropertyGremlinQuery<T1, T4, T5> IElementGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4, T5>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, TProjectedValue value) => Property(projection, value);
        IVertexPropertyGremlinQuery<T1, T4, T5> IElementGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4, T5>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, StepLabel<TProjectedValue> stepLabel) => Property(projection, __ => __.Select(stepLabel));
        IVertexPropertyGremlinQuery<T1, T4, T5> IElementGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4, T5>>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue>> projection, Func<IVertexPropertyGremlinQuery<T1, T4, T5>, IGremlinQueryBase<TProjectedValue>> valueTraversal) => Property(projection, valueTraversal);

        IVertexPropertyGremlinQuery<T1, T4, T5> IElementGremlinQueryBaseRec<T1, IVertexPropertyGremlinQuery<T1, T4, T5>>.Where<TProjection>(Expression<Func<T1, TProjection>> projection, Func<IGremlinQueryBase<TProjection>, IGremlinQueryBase> propertyTraversal) => Where(projection, propertyTraversal);
   }
}

