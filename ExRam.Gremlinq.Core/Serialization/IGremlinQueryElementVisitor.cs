namespace ExRam.Gremlinq.Core.Serialization
{
    public interface IGremlinQueryElementVisitor<out TSerializedQuery> : IGremlinQueryElementVisitor
    {
        TSerializedQuery Build();
    }

    public interface IGremlinQueryElementVisitor
    {
        void Visit(IGremlinQuery query);

        void Visit(HasNotStep step);
        void Visit(HasStep step);
        void Visit(HasValueStep step);
        void Visit(AddEStep step);
        void Visit(AddVStep step);
        void Visit(AndStep step);
        void Visit(ByTraversalStep step);
        void Visit(ChooseTraversalStep step);
        void Visit(ChoosePredicateStep step);
        void Visit(CoalesceStep step);
        void Visit(CoinStep step);
        void Visit(ConstantStep step);
        void Visit(BothStep step);
        void Visit(BothEStep step);
        void Visit(InStep step);
        void Visit(InEStep step);
        void Visit(OutStep step);
        void Visit(OutEStep step);
        void Visit(HasLabelStep step);

        void Visit(EdgesStep step);
        void Visit(FromTraversalStep step);
        void Visit(IdentifierStep step);
        void Visit(IsStep step);
        void Visit(LimitStep step);
        void Visit(LocalStep step);
        void Visit(MapStep step);
        void Visit(FlatMapStep step);
        void Visit(MatchStep step);
        void Visit(NotStep step);
        void Visit(OptionalStep step);
        void Visit(OrStep step);
        void Visit(PropertyStep step);
        void Visit(RepeatStep step);
        void Visit(SideEffectStep step);
        void Visit(ToTraversalStep step);
        void Visit(UnionStep step);
        void Visit(UntilStep step);
        void Visit(ValuesStep step);
        void Visit(VerticesStep step);
        void Visit(WhereTraversalStep step);
        void Visit(WithStrategiesStep step);
        void Visit(IdStep step);
        void Visit(BarrierStep step);
        void Visit(OrderStep step);
        void Visit(CreateStep step);
        void Visit(UnfoldStep step);
        void Visit(IdentityStep step);
        void Visit(EmitStep step);
        void Visit(DedupStep step);
        void Visit(OutVStep step);
        void Visit(OtherVStep step);
        void Visit(InVStep step);
        void Visit(BothVStep step);
        void Visit(DropStep step);
        void Visit(FoldStep step);
        void Visit(ExplainStep step);
        void Visit(ProfileStep step);
        void Visit(CountStep step);
        void Visit(BuildStep step);
        void Visit(SumStep step);
        void Visit(TailStep step);
        void Visit(SelectStep step);
        void Visit(AsStep step);
        void Visit(FromLabelStep step);
        void Visit(ToLabelStep step);
        void Visit(TimesStep step);
        void Visit(FilterStep step);
        void Visit(AggregateStep step);
        void Visit(WherePredicateStep step);
        void Visit(ByLambdaStep step);
        void Visit(SkipStep step);
        void Visit(MetaPropertyStep step);
        void Visit(RangeStep step);
        void Visit(ByMemberStep step);
        void Visit(PropertiesStep step);
        void Visit(MetaPropertiesStep step);
        void Visit(VStep step);
        void Visit(EStep step);
        void Visit(InjectStep step);
        void Visit(ValueMapStep step);

        void Visit(StepLabel stepLabel);

        void Visit(P.Eq p);
        void Visit(P.Between p);
        void Visit(P.Gt p);
        void Visit(P.Gte p);
        void Visit(P.Lt p);
        void Visit(P.Lte p);
        void Visit(P.Neq p);
        void Visit(P.Within p);
        void Visit(P.Without p);
        void Visit(P.Outside p);

        void Visit(Lambda lambda);
        void Visit<TEnum>(GremlinEnum<TEnum> gremlinEnum) where TEnum : GremlinEnum<TEnum>;
    }
}
