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

        public GroovyExpressionState AppendMethod(StringBuilder stringBuilder, string methodName, object parameter)
        {
            var methodExpressionState = new GroovyExpressionState(State.Idle, this._variables, this._stepLabelMappings);
            
            if (this._state == State.Chaining)
                stringBuilder.Append(".");

            stringBuilder.Append(methodName);
            stringBuilder.Append("(");

            methodExpressionState = parameter is IGroovySerializable serializable
                ? serializable.Serialize(stringBuilder, methodExpressionState)
                : methodExpressionState.AppendConstant(stringBuilder, parameter);

            stringBuilder.Append(")");

            return new GroovyExpressionState(State.Chaining, methodExpressionState._variables, methodExpressionState._stepLabelMappings);
        }

        public GroovyExpressionState AppendMethod(StringBuilder stringBuilder, string methodName, IEnumerable<object> parameters)
        {
            var setComma = false;
            var methodExpressionState = this;

            if (this._state == State.Chaining)
                stringBuilder.Append(".");

            stringBuilder.Append(methodName);
            stringBuilder.Append("(");
            
            foreach (var parameter in parameters)
            {
                if (setComma)
                    stringBuilder.Append(", ");

                methodExpressionState = new GroovyExpressionState(State.Idle, methodExpressionState._variables, methodExpressionState._stepLabelMappings);
                
                methodExpressionState = parameter is IGroovySerializable serializable
                    ? serializable.Serialize(stringBuilder, methodExpressionState)
                    : methodExpressionState.AppendConstant(stringBuilder, parameter);
                
                setComma = true;
            }

            stringBuilder.Append(")");

            return new GroovyExpressionState(State.Chaining, methodExpressionState._variables, methodExpressionState._stepLabelMappings);
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

            var (newVariables, newStepLabelMappings) = Cache(constant, this._variables, this._stepLabelMappings, out var key);
            builder.Append(key);

            return new GroovyExpressionState(State.Chaining, newVariables, newStepLabelMappings);
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