using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LanguageExt;

namespace ExRam.Gremlinq.Core.Serialization
{
    public class GroovyGremlinQueryElementVisitor : IGremlinQueryElementVisitor<GroovySerializedGremlinQuery>
    {
        public static readonly GremlinqOption<bool> WorkaroundTinkerpop2112 = new GremlinqOption<bool>(false);

        private enum State
        {
            Idle,
            Chaining,
            ChainingSupressIdentifier,
            InMethodBeforeFirstParameter,
            InMethodAfterFirstParameter
        }

        private State _state = State.Idle;
        private IGremlinQueryAdmin _currentAdmin;

        private readonly StringBuilder _builder = new StringBuilder();
        private readonly Stack<State> _stateQueue = new Stack<State>();
        private readonly Dictionary<object, string> _variables = new Dictionary<object, string>();
        private readonly Dictionary<StepLabel, string> _stepLabelMappings = new Dictionary<StepLabel, string>();

        #region Visit
        public virtual void Visit(HasNotStep step)
        {
            Method("hasNot", step.Key);
        }

        public void Visit(ChooseOptionTraversalStep step)
        {
            Method("choose", step.Traversal);
        }

        public void Visit(OptionTraversalStep step)
        {
            Method("option", step.Guard, step.OptionTraversal);
        }

        public virtual void Visit(HasStep step)
        {
            if (step.Value is P p1 && p1.EqualsConstant(false))
                Visit(NoneStep.Instance);
            else
            {
                var stepName = "has";
                var argument = step.Value;

                if (argument is P p2)
                {
                    if (p2 is P.SingleArgumentP singleArgumentP)
                    {
                        if (singleArgumentP.Argument == null)
                        {
                            if (p2 is P.Eq)
                                stepName = "hasNot";
                            else if (p2 is P.Neq)
                                argument = null;
                        }

                        if (p2 is P.Eq)
                            argument = singleArgumentP.Argument;
                    }
                    else if (p2 == P.True)
                        argument = null;
                }

                if (argument != null)
                    Method(stepName, step.Key, argument);
                else
                    Method(stepName, step.Key);
            }
        }
        
        public virtual void Visit(RepeatStep step)
        {
            Visit(step, "repeat");
        }

        public virtual void Visit(SideEffectStep step)
        {
            Visit(step, "sideEffect");
        }

        public virtual void Visit(ToTraversalStep step)
        {
            Visit(step, "to");
        }

        public virtual void Visit(UnionStep step)
        {
            Visit(step, "union");
        }

        public virtual void Visit(UntilStep step)
        {
            Visit(step, "until");
        }

        public virtual void Visit(ValuesStep step)
        {
            Method("values", step.Keys);
        }

        public virtual void Visit(VerticesStep step)
        {
            Visit(step, "vertices");
        }

        public virtual void Visit(WhereTraversalStep step)
        {
            Visit(step, "where");
        }

        public virtual void Visit(WithStrategiesStep step)
        {
            Visit(step, "withStrategies");
        }

        public virtual void Visit(IdStep step)
        {
            Method("id");
        }

        public virtual void Visit(BarrierStep step)
        {
            Method("barrier");
        }

        public virtual void Visit(OrderStep step)
        {
            Method("order");
        }

        public virtual void Visit(CreateStep step)
        {
            Method("create");
        }

        public virtual void Visit(UnfoldStep step)
        {
            Method("unfold");
        }

        public virtual void Visit(IdentityStep step)
        {
            Method("identity");
        }

        public virtual void Visit(EmitStep step)
        {
            Method("emit");
        }

        public virtual void Visit(DedupStep step)
        {
            Method("dedup");
        }

        public virtual void Visit(OutVStep step)
        {
            Method("outV");
        }

        public virtual void Visit(OtherVStep step)
        {
            Method("otherV");
        }

        public virtual void Visit(InVStep step)
        {
            Method("inV");
        }

        public virtual void Visit(BothVStep step)
        {
            Method("bothV");
        }

        public virtual void Visit(DropStep step)
        {
            Method("drop");
        }

        public virtual void Visit(FoldStep step)
        {
            Method("fold");
        }

        public virtual void Visit(ExplainStep step)
        {
            Method("explain");
        }

        public virtual void Visit(ProfileStep step)
        {
            Method("profile");
        }

        public virtual void Visit(CountStep step)
        {
            if (step.Scope.Equals(Scope.Local))
                Method("count", step.Scope);
            else
                Method("count");
        }

        public virtual void Visit(BuildStep step)
        {
            Method("build");
        }

        public virtual void Visit(SumStep step)
        {
            Method("sum", step.Scope);
        }

        public virtual void Visit(TailStep step)
        {
            if (step.Scope.Equals(Scope.Local))
                Method("tail", step.Scope, step.Count);
            else
                Method("tail", step.Count);
        }

        public virtual void Visit(SelectStep step)
        {
            Method("select", step.StepLabels);
        }

        public virtual void Visit(AsStep step)
        {
            Method("as", step.StepLabels);
        }

        public virtual void Visit(FromLabelStep step)
        {
            Method("from", step.StepLabel);
        }

        public virtual void Visit(ToLabelStep step)
        {
            Method("to", step.StepLabel);
        }

        public virtual void Visit(TimesStep step)
        {
            Method("times", step.Count);
        }

        public virtual void Visit(FilterStep step)
        {
            Method("filter", step.Lambda);
        }

        public virtual void Visit(AggregateStep step)
        {
            Method("aggregate", step.StepLabel);
        }

        public virtual void Visit(WherePredicateStep step)
        {
            Method("where", step.Predicate);
        }

        public virtual void Visit(ByLambdaStep step)
        {
            Method("by", step.Lambda);
        }

        public virtual void Visit(SkipStep step)
        {
            Method("skip", step.Count);
        }

        public virtual void Visit(PropertyStep step)
        {
            if (T.Id.Equals(step.Key) && !Cardinality.Single.Equals(step.Cardinality.IfNone(Cardinality.Single)))
                throw new NotSupportedException("Cannot have an id property on non-single cardinality.");

            if (ReferenceEquals(step.Key, T.Id))
                Method("property", step.MetaProperties.Prepend(step.Value).Prepend(step.Key));
            else
            {
                step.Cardinality.Match(
                    c => Method("property", step.MetaProperties.Prepend(step.Value).Prepend(step.Key).Prepend(c)),
                    () => Method("property", step.MetaProperties.Prepend(step.Value).Prepend(step.Key)));
            }
        }

        public virtual void Visit(RangeStep step)
        {
            Method("range", step.Lower, step.Upper);
        }

        public virtual void Visit(ByMemberStep step)
        {
            Method("by", step.Key, step.Order);
        }

        public void Visit(KeyStep step)
        {
            Method("key");
        }

        //public virtual void Visit(PropertiesStep step)
        //{
        //    Method("properties", step.Members.Select(x => x.Name).ToArray());
        //}

        public virtual void Visit(PropertiesStep step)
        {
            Method("properties", step.Keys);
        }

        public virtual void Visit(VStep step)
        {
            Method("V", step.Ids);
        }

        public virtual void Visit(EStep step)
        {
            Method("E", step.Ids);
        }

        public virtual void Visit(InjectStep step)
        {
            Method("inject", step.Elements);
        }

        public virtual void Visit(StepLabel stepLabel)
        {
            Constant(stepLabel);
        }

        public virtual void Visit(P.Eq p)
        {
            Visit(p, "eq");
        }

        public virtual void Visit(P.Between p)
        {
            NoIdentifier();
            Method("between", p.Lower, p.Upper);
        }

        public virtual void Visit(P.Gt p)
        {
            Visit(p, "gt");
        }

        public virtual void Visit(P.Gte p)
        {
            Visit(p, "gte");
        }

        public virtual void Visit(P.Lt p)
        {
            Visit(p, "lt");
        }

        public virtual void Visit(P.Lte p)
        {
            Visit(p, "lte");
        }

        public virtual void Visit(P.Neq p)
        {
            Visit(p, "neq");
        }

        public virtual void Visit(P.Within p)
        {
            NoIdentifier();
            Method("within", p.Arguments);
        }

        public void Visit(P.Without p)
        {
            NoIdentifier();
            Method("without", p.Arguments);
        }

        public void Visit(P.Outside p)
        {
            NoIdentifier();
            Method("outside", p.Lower, p.Upper);
        }

        public void Visit(P.AndP p)
        {
            Visit(p.Operand1);
            Method("and", p.Operand2);
        }

        public void Visit(P.OrP p)
        {
            Visit(p.Operand1);
            Method("or", p.Operand2);
        }

        public void Visit(TextP.StartingWith p)
        {
            NoIdentifier();
            Method("startingWith", p.Value);
        }

        public void Visit(TextP.EndingWith p)
        {
            NoIdentifier();
            Method("endingWith", p.Value);
        }

        public void Visit(TextP.Containing p)
        {
            NoIdentifier();
            Method("containing", p.Value);
        }

        public virtual void Visit(Lambda lambda)
        {
            Lambda(lambda.LambdaString);
        }

        public virtual void Visit<TEnum>(GremlinEnum<TEnum> gremlinEnum) where TEnum : GremlinEnum<TEnum>
        {
            NoIdentifier();
            Field(gremlinEnum.Name);
        }

        public virtual void Visit(HasValueStep step)
        {
            Method("hasValue",
                step.Argument is P.Eq eq
                    ? eq.Argument
                    : step.Argument);
        }

        public virtual void Visit(AddEStep step)
        {
            Visit(step, "addE");
        }

        public virtual void Visit(AddVStep step)
        {
            Visit(step, "addV");
        }

        public virtual void Visit(AndStep step)
        {
            VisitLogicalStep(step, "and");
        }

        public virtual void Visit(ByTraversalStep step)
        {
            Method("by", step.Traversal, step.Order);
        }

        public virtual void Visit(ChooseTraversalStep step)
        {
            step.ElseTraversal.Match(
                elseTraversal => Method(
                    "choose",
                    step.IfTraversal,
                    step.ThenTraversal,
                    elseTraversal),
                () => Method(
                    "choose",
                    step.IfTraversal,
                    step.ThenTraversal));
        }

        public virtual void Visit(ChoosePredicateStep step)
        {
            step.ElseTraversal.Match(
                elseTraversal => Method(
                    "choose",
                    step.Predicate,
                    step.ThenTraversal,
                    elseTraversal),
                () => Method(
                    "choose",
                    step.Predicate,
                    step.ThenTraversal));
        }

        public virtual void Visit(CoalesceStep step)
        {
            Visit(step, "coalesce");
        }

        public void Visit(CoinStep step)
        {
            Method("coin", step.Probability);
        }

        public virtual void Visit(ConstantStep step)
        {
            Method("constant", step.Value);
        }

        public virtual void Visit(BothStep step)
        {
            Visit(step, "both");
        }

        public virtual void Visit(BothEStep step)
        {
            Visit(step, "bothE");
        }

        public virtual void Visit(InStep step)
        {
            Visit(step, "in");
        }

        public virtual void Visit(InEStep step)
        {
            Visit(step, "inE");
        }

        public virtual void Visit(OutStep step)
        {
            Visit(step, "out");
        }

        public virtual void Visit(OutEStep step)
        {
            Visit(step, "outE");
        }

        public virtual void Visit(HasLabelStep step)
        {
            Visit(step, "hasLabel");
        }

        public void Visit(LabelStep step)
        {
            Method("label");
        }

        public virtual void Visit(DerivedLabelNamesStep step, string stepName)
        {
            Method(stepName, step.Labels);
        }

        public virtual void Visit(EdgesStep step)
        {
            Visit(step, "edges");
        }

        public virtual void Visit(FromTraversalStep step)
        {
            Visit(step, "from");
        }

        public virtual void Visit(IdentifierStep step)
        {
            Identifier(step.Identifier);
        }

        public virtual void Visit(IsStep step)
        {
            Method("is",
                step.Argument is P.Eq eq
                    ? eq.Argument
                    : step.Argument);
        }

        public virtual void Visit(LimitStep step)
        {
            if (step.Scope.Equals(Scope.Local))
                Method("limit", step.Scope, step.Count);
            else
                Method("limit", step.Count);
        }

        public virtual void Visit(LocalStep step)
        {
            Visit(step, "local");
        }

        public virtual void Visit(MapStep step)
        {
            Visit(step, "map");
        }

        public virtual void Visit(NoneStep step)
        {
            Method("none");
        }

        public virtual void Visit(FlatMapStep step)
        {
            Visit(step, "flatMap");
        }

        public virtual void Visit(MatchStep step)
        {
            Visit(step, "match");
        }

        public virtual void Visit(NotStep step)
        {
            var traversalSteps = step.Traversal.AsAdmin().Steps;

            if (!(traversalSteps.Count != 0 && traversalSteps[traversalSteps.Count - 1] is HasStep hasStep && hasStep.Value is P p && p.EqualsConstant(false)))
                Visit(step, "not");
        }

        public virtual void Visit(OptionalStep step)
        {
            Visit(step, "optional");
        }

        public virtual void Visit(OrStep step)
        {
            VisitLogicalStep(step, "or");
        }


        public virtual void Visit(ValueStep step)
        {
            Method("value");
        }

        public virtual void Visit(ValueMapStep step)
        {
            Method("valueMap", step.Keys);
        }

        public virtual void Visit(IGremlinQuery query)
        {
            var beforeState = _state;
            var beforeAdmin = _currentAdmin;
            {
                _state = State.Idle;
                _currentAdmin = query.AsAdmin();

                var steps = _currentAdmin.Steps.HandleAnonymousQueries();
                if (_currentAdmin.Options.GetValue(WorkaroundTinkerpop2112))
                    steps = _currentAdmin.Steps.WorkaroundTINKERPOP_2112();

                foreach (var step in steps)
                {
                    step.Accept(this);
                }
            }

            _state = beforeState;
            _currentAdmin = beforeAdmin;
        }

        #endregion

        public GroovySerializedGremlinQuery Build()
        {
            return new GroovySerializedGremlinQuery(
                _builder.ToString(),
                _variables
                    .ToDictionary(kvp => kvp.Value, kvp => kvp.Key));
        }

        protected void NoIdentifier()
        {
            if (_state != State.Idle)
                throw new InvalidOperationException();

            _state = State.ChainingSupressIdentifier;
        }

        protected void Identifier(string className)
        {
            if (_state != State.Idle)
                throw new InvalidOperationException();

            _builder.Append(className);
            _state = State.Chaining;
        }

        protected void Lambda(string lambda)
        {
            if (_state != State.Idle)
                throw new InvalidOperationException();

            _builder.Append("{");
            _builder.Append(lambda);
            _builder.Append("}");
        }

        protected void OpenMethod(string methodName)
        {
            if (_state != State.Chaining && _state != State.ChainingSupressIdentifier)
                throw new InvalidOperationException();

            if (_state == State.Chaining)
                _builder.Append(".");

            _builder.Append(methodName);
            _builder.Append("(");

            _stateQueue.Push(State.Chaining);
            _state = State.InMethodBeforeFirstParameter;
        }

        protected void CloseMethod()
        {
            _builder.Append(")");
            _state = _stateQueue.Pop();
        }

        protected void StartParameter()
        {
            if (_state != State.InMethodBeforeFirstParameter && _state != State.InMethodAfterFirstParameter)
                throw new InvalidOperationException();

            if (_state == State.InMethodAfterFirstParameter)
                _builder.Append(", ");

            _stateQueue.Push(State.InMethodAfterFirstParameter);
            _state = State.Idle;
        }

        protected void EndParameter()
        {
            _state = _stateQueue.Pop();
        }

        protected virtual void Visit(object parameter)
        {
            switch (parameter)
            {
                case IGremlinQueryAtom queryElement:
                {
                    queryElement.Accept(this);
                    break;
                }
                case IGremlinQuery subQuery:
                {
                    Visit(subQuery);
                    break;
                }
                default:
                {
                    Constant(parameter);
                    break;
                }
            }
        }

        #region Method
        protected virtual void Method(string methodName)
        {
            OpenMethod(methodName);
            CloseMethod();
        }

        protected virtual void Method(string methodName, object parameter)
        {
            OpenMethod(methodName);
            {
                StartParameter();
                Visit(parameter);
                EndParameter();
            }
            CloseMethod();
        }

        protected virtual void Method(string methodName, object parameter1, object parameter2)
        {
            OpenMethod(methodName);
            {
                StartParameter();
                Visit(parameter1);
                EndParameter();

                StartParameter();
                Visit(parameter2);
                EndParameter();
            }
            CloseMethod();
        }

        protected virtual void Method(string methodName, object parameter1, object parameter2, object parameter3)
        {
            OpenMethod(methodName);
            {
                StartParameter();
                Visit(parameter1);
                EndParameter();

                StartParameter();
                Visit(parameter2);
                EndParameter();

                StartParameter();
                Visit(parameter3);
                EndParameter();
            }
            CloseMethod();
        }

        protected virtual void Method(string methodName, IEnumerable<object> parameters)
        {
            OpenMethod(methodName);
            {
                foreach (var parameter in parameters)
                {
                    StartParameter();
                    Visit(parameter);
                    EndParameter();
                }
            }
            CloseMethod();
        }
        #endregion

        protected virtual void Field(string fieldName)
        {
            if (_state != State.Chaining && _state != State.ChainingSupressIdentifier)
                throw new InvalidOperationException();

            if (_state != State.ChainingSupressIdentifier)
                _builder.Append(".");

            _builder.Append(fieldName);
        }

        protected void Constant(object constant)
        {
            if (_state == State.Chaining || _state == State.ChainingSupressIdentifier)
                throw new InvalidOperationException();

            _builder.Append(Cache(constant));
        }

        protected string Cache(object constant)
        {
            if (constant is StepLabel stepLabel)
            {
                if (!_stepLabelMappings.TryGetValue(stepLabel, out var stepLabelMapping))
                {
                    stepLabelMapping = "l" + (_stepLabelMappings.Count + 1);
                    _stepLabelMappings.Add(stepLabel, stepLabelMapping);
                }

                // ReSharper disable once TailRecursiveCall
                return Cache(stepLabelMapping);
            }

            if (constant is IOptional optional)
                return optional.MatchUntyped(Cache, () => "none");

            if (_variables.TryGetValue(constant, out var key))
                return key;

            var next = _variables.Count;

            do
            {
                key = (char)('a' + next % 26) + key;
                next = next / 26;
            }
            while (next > 0);

            key = "_" + key;
            _variables.Add(constant, key);

            return key;
        }

        protected virtual void Visit(AddElementStep step, string stepName)
        {
            Method(stepName, step.Label);
        }

        protected virtual void Visit(MultiTraversalArgumentStep step, string stepName)
        {
            Method(stepName,
                step.Traversals);
        }

        protected virtual void Visit(SingleTraversalArgumentStep step, string stepName)
        {
            Method(stepName, step.Traversal);
        }

        protected virtual void VisitLogicalStep<TStep>(TStep step, string stepName) where TStep : LogicalStep
        {
            Method(stepName,
                step.Traversals
                    .SelectMany(FlattenLogicalTraversals<TStep>));
        }

        protected virtual void Visit(P.SingleArgumentP p, string name)
        {
            NoIdentifier();
            Method(name, p.Argument);
        }

        private static IEnumerable<IGremlinQuery> FlattenLogicalTraversals<TStep>(IGremlinQuery query) where TStep : LogicalStep
        {
            var steps = query.AsAdmin().Steps;

            if (steps.Count == 2 && steps[1] is TStep otherStep)
            {
                foreach (var subTraversal in otherStep.Traversals)
                {
                    foreach (var flattenedSubTraversal in FlattenLogicalTraversals<TStep>(subTraversal))
                    {
                        yield return flattenedSubTraversal;
                    }
                }
            }
            else
                yield return query;
        }

        public void Visit(WithoutStrategiesStep step)
        {
            OpenMethod("withoutStrategies");

            foreach (var className in step.ClassNames)
            {
                StartParameter();
                Identifier(className);
                EndParameter();
            }

            CloseMethod();
        }

        public void Visit(ProjectStep.ByTraversalStep byTraversalStep)
        {
            Method("by", byTraversalStep.Traversal);
        }

        public void Visit(ProjectStep projectStep)
        {
            Method("project", projectStep.Projections);
        }
    }
}
