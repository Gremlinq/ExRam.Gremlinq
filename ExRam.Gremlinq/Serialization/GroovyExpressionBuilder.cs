using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace ExRam.Gremlinq
{
    public struct GroovyExpressionBuilder
    {
        private enum State
        {
            Idle,
            Chaining,
        }

        private readonly State _state;
        private readonly IImmutableDictionary<object, string> _variables;
        private readonly IImmutableDictionary<StepLabel, string> _stepLabelMappings;

        private GroovyExpressionBuilder(State state, IImmutableDictionary<object, string> variables, IImmutableDictionary<StepLabel, string> stepLabelMappings)
        {
            this._state = state;
            this._variables = variables;
            this._stepLabelMappings = stepLabelMappings;
        }

        public GroovyExpressionBuilder AppendIdentifier(StringBuilder builder, string className)
        {
            if (this._state != State.Idle)
                throw new InvalidOperationException();

            builder.Append(className);
            return new GroovyExpressionBuilder(State.Chaining, this._variables, this._stepLabelMappings);
        }

        public GroovyExpressionBuilder AppendLambda(StringBuilder builder, string lambda)
        {
            builder.Append("{");
            builder.Append(lambda);
            builder.Append("}");

            return this;
        }

        public GroovyExpressionBuilder AppendMethod(StringBuilder builder, string methodName, IEnumerable<object> parameters)
        {
            var setComma = false;
            var subMethodBuilder = this;

            if (this._state == State.Chaining)
                builder.Append(".");

            builder.Append(methodName);
            builder.Append("(");
            
            foreach (var parameter in parameters)
            {
                if (setComma)
                    builder.Append(", ");

                subMethodBuilder = new GroovyExpressionBuilder(State.Idle, subMethodBuilder._variables, subMethodBuilder._stepLabelMappings);
                {
                    subMethodBuilder = parameter is IGremlinSerializable serializable
                        ? serializable.Serialize(builder, subMethodBuilder)
                        : subMethodBuilder.AppendConstant(builder, parameter);
                }

                setComma = true;
            }

            builder.Append(")");

            return new GroovyExpressionBuilder(State.Chaining, subMethodBuilder._variables, subMethodBuilder._stepLabelMappings);
        }

        public GroovyExpressionBuilder AppendField(StringBuilder builder, string fieldName)
        {
            if (this._state != State.Chaining)
                throw new InvalidOperationException();

            builder.Append(".");
            builder.Append(fieldName);

            return this;
        }

        public GroovyExpressionBuilder AppendConstant(StringBuilder builder, object constant)
        {
            if (this._state == State.Chaining)
                throw new InvalidOperationException();

            if (constant is Enum)
            {
                return this
                    .AppendIdentifier(builder, constant.GetType().Name)
                    .AppendField(builder, Enum.GetName(constant.GetType(), constant));
            }

            var (newVariables, newStepLabelMappings) = Cache(constant, this._variables, this._stepLabelMappings, out var key);
            builder.Append(key);

            return new GroovyExpressionBuilder(this._state, newVariables, newStepLabelMappings);
        }

        public IDictionary<string, object> GetVariables()
        {
            return this._variables
                .ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        }

        public static GroovyExpressionBuilder FromQuery(IGremlinQuery query)
        {
            return new GroovyExpressionBuilder(State.Idle, ImmutableDictionary<object, string>.Empty, query.StepLabelMappings);
        }

        private static (IImmutableDictionary<object, string> variables, IImmutableDictionary<StepLabel, string> stepLabelMappings) Cache(object constant, IImmutableDictionary<object, string> variables, IImmutableDictionary<StepLabel, string> stepLabelMappings, out string key)
        {
            if (constant is StepLabel stepLabel)
            {
                if (!stepLabelMappings.TryGetValue(stepLabel, out var stepLabelMapping))
                {
                    stepLabelMapping = "l" + (stepLabelMappings.Count + 1);
                    stepLabelMappings = stepLabelMappings.Add(stepLabel, stepLabelMapping);
                }

                return Cache(stepLabelMapping, variables, stepLabelMappings, out key);
            }

            if (variables.TryGetValue(constant, out key))
                return (variables, stepLabelMappings);

            key = "_P" + (variables.Count + 1);
            return (variables.Add(constant, key), stepLabelMappings);
        }
    }
}