using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace ExRam.Gremlinq
{
    public struct GroovyExpressionState
    {
        private enum State
        {
            Idle,
            Chaining,
        }

        private readonly State _state;
        private readonly IImmutableDictionary<object, string> _variables;
        private readonly IImmutableDictionary<StepLabel, string> _stepLabelMappings;

        private GroovyExpressionState(State state, IImmutableDictionary<object, string> variables, IImmutableDictionary<StepLabel, string> stepLabelMappings)
        {
            this._state = state;
            this._variables = variables;
            this._stepLabelMappings = stepLabelMappings;
        }

        public GroovyExpressionState AppendIdentifier(StringBuilder builder, string className)
        {
            if (this._state != State.Idle)
                throw new InvalidOperationException();

            builder.Append(className);
            return new GroovyExpressionState(State.Chaining, this._variables, this._stepLabelMappings);
        }

        public GroovyExpressionState AppendLambda(StringBuilder builder, string lambda)
        {
            builder.Append("{");
            builder.Append(lambda);
            builder.Append("}");

            return this;
        }

        public GroovyExpressionState AppendMethod(StringBuilder builder, string methodName, IEnumerable<object> parameters)
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

                subMethodBuilder = new GroovyExpressionState(State.Idle, subMethodBuilder._variables, subMethodBuilder._stepLabelMappings);
                {
                    subMethodBuilder = parameter is IGremlinSerializable serializable
                        ? serializable.Serialize(builder, subMethodBuilder)
                        : subMethodBuilder.AppendConstant(builder, parameter);
                }

                setComma = true;
            }

            builder.Append(")");

            return new GroovyExpressionState(State.Chaining, subMethodBuilder._variables, subMethodBuilder._stepLabelMappings);
        }

        public GroovyExpressionState AppendField(StringBuilder builder, string fieldName)
        {
            if (this._state != State.Chaining)
                throw new InvalidOperationException();

            builder.Append(".");
            builder.Append(fieldName);

            return this;
        }

        public GroovyExpressionState AppendConstant(StringBuilder builder, object constant)
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

            return new GroovyExpressionState(this._state, newVariables, newStepLabelMappings);
        }

        public IDictionary<string, object> GetVariables()
        {
            return this._variables
                .ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        }

        public static GroovyExpressionState FromQuery(IGremlinQuery query)
        {
            return new GroovyExpressionState(State.Idle, ImmutableDictionary<object, string>.Empty, query.StepLabelMappings);
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