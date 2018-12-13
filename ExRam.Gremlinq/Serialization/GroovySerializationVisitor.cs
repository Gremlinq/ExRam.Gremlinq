using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExRam.Gremlinq
{
    public sealed class GroovySerializationVisitor : IQueryElementVisitor
    {
        private enum State
        {
            Idle,
            Chaining,
            InMethodBeforeFirstParameter,
            InMethodAfterFirstParameter
        }

        private State _state;

        private readonly Dictionary<object, string> _variables;
        private readonly Stack<State> _stateQueue = new Stack<State>();
        private readonly Dictionary<StepLabel, string> _stepLabelMappings;

        private GroovySerializationVisitor(State state, Dictionary<object, string> variables, Dictionary<StepLabel, string> stepLabelMappings, StringBuilder stringBuilder)
        {
            _state = state;
            _variables = variables;
            _stepLabelMappings = stepLabelMappings;

            Builder = stringBuilder;
        }

        public static GroovySerializationVisitor Create(IGraphModel model)
        {
            return new GroovySerializationVisitor(State.Idle, new Dictionary<object, string>(), new Dictionary<StepLabel, string>(), new StringBuilder());
        }

        public IDictionary<string, object> GetVariables()
        {
            return _variables
                .ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        }

        #region Visit
        public void Visit(HasNotStep step)
        {
            Visit(step, "hasNot");
        }

        public void Visit(HasStep step)
        {
            Visit(step, "has");
        }


        public void Visit(RepeatStep step)
        {
            Visit(step, "repeat");
        }

        public void Visit(SideEffectStep step)
        {
            Visit(step, "sideEffect");
        }

        public void Visit(ToTraversalStep step)
        {
            Visit(step, "to");
        }

        public void Visit(UnionStep step)
        {
            Visit(step, "union");
        }

        public void Visit(UntilStep step)
        {
            Visit(step, "until");
        }

        public void Visit(ValuesStep step)
        {
            var numberOfIdSteps = step.Keys
                .OfType<T>()
                .Count(x => x == T.Id);

            var propertyKeys = step.Keys
                .OfType<string>()
                .Cast<object>()
                .ToArray();

            if (numberOfIdSteps > 1 || numberOfIdSteps > 0 && propertyKeys.Length > 0)
            {
                OpenMethod("union");
                {
                    StartParameter();
                    {
                        Identifier("__");

                        OpenMethod("values");
                        {
                            foreach (var propertyKey in propertyKeys)
                            {
                                StartParameter();
                                Visit(propertyKey);
                                EndParameter();
                            }
                        }
                        CloseMethod();
                    }
                    EndParameter();

                    StartParameter();
                    {
                        Identifier("__");
                        Method("id");
                    }
                    EndParameter();
                }
                CloseMethod();
            }
            else
            {
                if (numberOfIdSteps > 0)
                    IdStep.Instance.Accept(this);
                else
                    Method(
                        "values",
                        propertyKeys);
            }
        }

        public void Visit(VerticesStep step)
        {
            Visit(step, "vertices");
        }

        public void Visit(WhereTraversalStep step)
        {
            Visit(step, "where");
        }

        public void Visit(WithStrategiesStep step)
        {
            Visit(step, "withStrategies");
        }

        public void Visit(IdStep step)
        {
            Method("id");
        }

        public void Visit(BarrierStep step)
        {
            Method("barrier");
        }

        public void Visit(OrderStep step)
        {
            Method("order");
        }

        public void Visit(CreateStep step)
        {
            Method("create");
        }

        public void Visit(UnfoldStep step)
        {
            Method("unfold");
        }

        public void Visit(IdentityStep step)
        {
            Method("identity");
        }

        public void Visit(EmitStep step)
        {
            Method("emit");
        }

        public void Visit(DedupStep step)
        {
            Method("dedup");
        }

        public void Visit(OutVStep step)
        {
            Method("outV");
        }

        public void Visit(OtherVStep step)
        {
            Method("otherV");
        }

        public void Visit(InVStep step)
        {
            Method("inV");
        }

        public void Visit(BothVStep step)
        {
            Method("bothV");
        }

        public void Visit(DropStep step)
        {
            Method("drop");
        }

        public void Visit(FoldStep step)
        {
            Method("fold");
        }

        public void Visit(ExplainStep step)
        {
            Method("explain");
        }

        public void Visit(ProfileStep step)
        {
            Method("profile");
        }

        public void Visit(CountStep step)
        {
            Method("count");
        }

        public void Visit(BuildStep step)
        {
            Method("build");
        }

        public void Visit(SumStep step)
        {
            Method("sum", step.Scope);
        }

        public void Visit(TailStep step)
        {
            Method("tail", step.Count);
        }

        public void Visit(SelectStep step)
        {
            Method("select", step.StepLabels);
        }

        public void Visit(AsStep step)
        {
            Method("as", step.StepLabel);
        }

        public void Visit(FromLabelStep step)
        {
            Method("from", step.StepLabel);
        }

        public void Visit(ToLabelStep step)
        {
            Method("to", step.StepLabel);
        }

        public void Visit(TimesStep step)
        {
            Method("times", step.Count);
        }

        public void Visit(FilterStep step)
        {
            Method("filter", step.Lambda);
        }

        public void Visit(AggregateStep step)
        {
            Method("aggregate", step.StepLabel);
        }

        public void Visit(WherePredicateStep step)
        {
            Method("where", step.Predicate);
        }

        public void Visit(ByLambdaStep step)
        {
            Method("by", step.Lambda);
        }

        public void Visit(SkipStep step)
        {
            Method("skip", step.Count);
        }

        public void Visit(MetaPropertyStep step)
        {
            Method("property", step.Key, step.Value);
        }

        public void Visit(RangeStep step)
        {
            Method("range", step.Lower, step.Upper);
        }

        public void Visit(ByMemberStep step)
        {
            Method("by", step.Member.Name, step.Order);
        }

        public void Visit(PropertiesStep step)
        {
            Method("properties", step.Members.Select(x => x.Name).ToArray());
        }

        public void Visit(MetaPropertiesStep step)
        {
            Method("properties", step.Keys);
        }

        public void Visit(VStep step)
        {
            Method("V", step.Ids);
        }

        public void Visit(EStep step)
        {
            Method("E", step.Ids);
        }

        public void Visit(InjectStep step)
        {
            Method("inject", step.Elements);
        }

        public void Visit(StepLabel stepLabel)
        {
            Constant(stepLabel);
        }

        public void Visit(P.Eq p)
        {
            Visit(p, "eq");
        }

        public void Visit(P.Between p)
        {
            Identifier(nameof(P));
            Method("between", p.Lower, p.Upper);
        }

        public void Visit(P.Gt p)
        {
            Visit(p, "gt");
        }

        public void Visit(P.Gte p)
        {
            Visit(p, "gte");
        }

        public void Visit(P.Lt p)
        {
            Visit(p, "lt");
        }

        public void Visit(P.Lte p)
        {
            Visit(p, "lte");
        }

        public void Visit(P.Neq p)
        {
            Visit(p, "neq");
        }

        public void Visit(P.Within p)
        {
            Identifier(nameof(P));
            Method("within", p.Arguments);
        }

        public void Visit(Lambda lambda)
        {
            Lambda(lambda.LambdaString);
        }

        public void Visit<TEnum>(GremlinEnum<TEnum> gremlinEnum) where TEnum : GremlinEnum<TEnum>
        {
            Identifier(typeof(TEnum).Name);
            Field(gremlinEnum.Name);
        }

        public void Visit(HasValueStep step)
        {
            Method("hasValue",
                step.Argument is P.Eq eq
                    ? eq.Argument
                    : step.Argument);
        }

        public void Visit(AddEStep step)
        {
            Visit(step, "addE");
        }

        public void Visit(AddVStep step)
        {
            Visit(step, "addV");
        }

        public void Visit(AndStep step)
        {
            Visit(step, "and");
        }

        public void Visit(ByTraversalStep step)
        {
            Method("by", step.Traversal, step.Order);
        }

        public void Visit(ChooseStep step)
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

        public void Visit(CoalesceStep step)
        {
            Visit(step, "coalesce");
        }

        public void Visit(BothStep step)
        {
            Visit(step, "both");
        }

        public void Visit(BothEStep step)
        {
            Visit(step, "bothE");
        }

        public void Visit(InStep step)
        {
            Visit(step, "in");
        }

        public void Visit(InEStep step)
        {
            Visit(step, "inE");
        }

        public void Visit(OutStep step)
        {
            Visit(step, "out");
        }

        public void Visit(OutEStep step)
        {
            Visit(step, "outE");
        }

        public void Visit(HasLabelStep step)
        {
            Visit(step, "hasLabel");
        }

        public void Visit(DerivedLabelNamesStep step, string stepName)
        {
            Method(stepName, step.Labels);
        }

        public void Visit(EdgesStep step)
        {
            Visit(step, "edges");
        }

        public void Visit(FromTraversalStep step)
        {
            Visit(step, "from");
        }

        public void Visit(IdentifierStep step)
        {
            Identifier(step.Identifier);
        }

        public void Visit(IsStep step)
        {
            Method("is",
                step.Argument is P.Eq eq
                    ? eq.Argument
                    : step.Argument);
        }

        public void Visit(LimitStep step)
        {
            Method("limit", step.Limit);
        }

        public void Visit(LocalStep step)
        {
            Visit(step, "local");
        }

        public void Visit(MapStep step)
        {
            Visit(step, "map");
        }

        public void Visit(MatchStep step)
        {
            Visit(step, "match");
        }

        public void Visit(NotStep step)
        {
            if (step.Traversal.Steps.Count == 0 || !(step.Traversal.Steps[step.Traversal.Steps.Count - 1] is HasStep hasStep) || hasStep.Value != P.False)
                Visit(step, "not");
        }

        public void Visit(OptionalStep step)
        {
            Visit(step, "optional");
        }

        public void Visit(OrStep step)
        {
            Visit(step, "or");
        }

        public void Visit(PropertyStep step)
        {
            if (step.Value != null)
            {
                if (!step.Type.IsArray || step.Type == typeof(byte[]))
                    Property(Cardinality.Single, step.Key, step.Value);
                else
                {
                    if (step.Type.GetElementType().IsInstanceOfType(step.Value))
                        Property(Cardinality.List, step.Key, step.Value);
                    else
                    {
                        foreach (var item in (IEnumerable)step.Value)
                        {
                            Property(Cardinality.List, step.Key, item);
                        }
                    }
                }
            }
        }

        public void Visit(IGremlinQuery query)
        {
            var steps = query.Steps.AsEnumerable();
            if (query.Steps.Count == 1 && query.Steps[0] is IdentifierStep identifierStep && identifierStep.Identifier == "__")
                steps = new Step[] { identifierStep, IdentityStep.Instance };

            foreach (var map in query.StepLabelMappings)
            {
                _stepLabelMappings[map.Key] = map.Value;
            }

            var beforeState = _state;
            _state = State.Idle;

            foreach (var step in steps.WorkaroundTINKERPOP_2112())
            {
                step.Accept(this);
            }

            _state = beforeState;
        }
        #endregion

        private void Identifier(string className)
        {
            if (_state != State.Idle)
                throw new InvalidOperationException();

            Builder.Append(className);
            _state = State.Chaining;
        }

        private void Lambda(string lambda)
        {
            if (_state != State.Idle)
                throw new InvalidOperationException();

            Builder.Append("{");
            Builder.Append(lambda);
            Builder.Append("}");
        }

        private void OpenMethod(string methodName)
        {
            if (_state != State.Chaining)
                throw new InvalidOperationException();

            Builder.Append(".");
            Builder.Append(methodName);
            Builder.Append("(");

            _stateQueue.Push(_state);
            _state = State.InMethodBeforeFirstParameter;
        }

        private void CloseMethod()
        {
            Builder.Append(")");
            _state = _stateQueue.Pop();
        }

        private void StartParameter()
        {
            if (_state != State.InMethodBeforeFirstParameter && _state != State.InMethodAfterFirstParameter)
                throw new InvalidOperationException();

            if (_state == State.InMethodAfterFirstParameter)
                Builder.Append(", ");

            _stateQueue.Push(State.InMethodAfterFirstParameter);
            _state = State.Idle;
        }

        private void EndParameter()
        {
            _state = _stateQueue.Pop();
        }

        private void Visit(object parameter)
        {
            if (parameter is IQueryElement queryElement)
                queryElement.Accept(this);
            else
            {
                if (parameter is IGremlinQuery subQuery)
                    Visit(subQuery);
                else
                    Constant(parameter);
            }
        }

        #region Method
        private void Method(string methodName)
        {
            OpenMethod(methodName);
            CloseMethod();
        }

        private void Method(string methodName, object parameter)
        {
            OpenMethod(methodName);
            {
                StartParameter();
                Visit(parameter);
                EndParameter();
            }
            CloseMethod();
        }

        private void Method(string methodName, object parameter1, object parameter2)
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

        private void Method(string methodName, object parameter1, object parameter2, object parameter3)
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

        private void Method(string methodName, IEnumerable<object> parameters)
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

        private void Field(string fieldName)
        {
            if (_state != State.Chaining)
                throw new InvalidOperationException();

            Builder.Append(".");
            Builder.Append(fieldName);
        }

        private void Constant(object constant)
        {
            if (_state == State.Chaining)
                throw new InvalidOperationException();

            Builder.Append(Cache(constant));
        }

        private string Cache(object constant)
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

            if (_variables.TryGetValue(constant, out var key))
                return key;

            var next = _variables.Count;

            while (next > 0 || key == null)
            {
                key = (char)('a' + next % 26) + key;
                next = next / 26;
            }

            key = "_" + key;
            _variables.Add(constant, key);

            return key;
        }

        private void Visit(AddElementStep step, string stepName)
        {
            Method(stepName, step.Label);
        }

        private void Visit(MultiTraversalArgumentStep step, string stepName)
        {
            Method(stepName,
                step.Traversals);
        }

        private void Visit(SingleTraversalArgumentStep step, string stepName)
        {
            Method(stepName, step.Traversal);
        }

        private void Visit<TStep>(LogicalStep<TStep> step, string stepName) where TStep : LogicalStep
        {
            Method(stepName,
                step.Traversals
                    .SelectMany(traversal => FlattenTraversals<TStep>(traversal)));
        }

        private void Visit(HasStepBase step, string stepName)
        {
            if (step.Value == P.False)
                new NotStep(GremlinQuery.Anonymous(GraphModel.Empty)).Accept(this);
            else
            {
                if (step.Value == P.True)
                    Method(stepName, step.Key);
                else
                    Method(
                        stepName,
                        step.Key,
                        step.Value is P.Eq eq
                            ? eq.Argument
                            : step.Value);
            }
        }

        private void Property(Cardinality cardinality, object name, object value)
        {
            if (ReferenceEquals(name, T.Id) && cardinality != Cardinality.Single)
                throw new NotSupportedException("Cannot have an id property on non-single cardinality.");

            if (value is IMeta meta)
            {
                var metaProperties = meta.Properties
                    .SelectMany(kvp => new[] { kvp.Key, kvp.Value })
                    .Prepend(meta.Value)
                    .Prepend(name)
                    .Prepend(cardinality);

                Method("property", metaProperties);
            }
            else
            {
                if (ReferenceEquals(name, T.Id))
                    Method("property", name, value);
                else
                    Method("property", cardinality, name, value);
            }
        }

        private void Visit(P.SingleArgumentP p, string name)
        {
            Identifier(nameof(P));
            Method(name, p.Argument);
        }

        private static IEnumerable<IGremlinQuery> FlattenTraversals<TStep>(IGremlinQuery query) where TStep : LogicalStep
        {
            if (query.Steps.Count == 2 && query.Steps[1] is TStep andStep)
            {
                foreach (var subTraversal in andStep.Traversals)
                {
                    foreach (var flattenedSubTraversal in FlattenTraversals<TStep>(subTraversal))
                    {
                        yield return flattenedSubTraversal;
                    }
                }
            }
            else
                yield return query;
        }
        
        public StringBuilder Builder { get; }
    }
}
