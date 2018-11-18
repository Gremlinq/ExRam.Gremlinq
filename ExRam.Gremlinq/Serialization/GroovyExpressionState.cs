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
            Chaining
        }

        private readonly State _state;
        private readonly IImmutableDictionary<object, string> _variables;
        private readonly IImmutableDictionary<StepLabel, string> _stepLabelMappings;

        private GroovyExpressionState(State state, IImmutableDictionary<object, string> variables, IImmutableDictionary<StepLabel, string> stepLabelMappings)
        {
            _state = state;
            _variables = variables;
            _stepLabelMappings = stepLabelMappings;
        }

        public GroovyExpressionState AppendIdentifier(StringBuilder builder, string className)
        {
            if (_state != State.Idle)
                throw new InvalidOperationException();

            builder.Append(className);
            return new GroovyExpressionState(State.Chaining, _variables, _stepLabelMappings);
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
            var methodExpressionState = new GroovyExpressionState(State.Idle, _variables, _stepLabelMappings);
            
            if (_state == State.Chaining)
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

            if (_state == State.Chaining)
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
            if (_state != State.Chaining)
                throw new InvalidOperationException();

            builder.Append(".");
            builder.Append(fieldName);

            return this;
        }

        public GroovyExpressionState AppendConstant(StringBuilder builder, object constant)
        {
            if (_state == State.Chaining)
                throw new InvalidOperationException();

            var (newVariables, newStepLabelMappings) = Cache(constant, _variables, _stepLabelMappings, out var key);
            builder.Append(key);

            return new GroovyExpressionState(State.Chaining, newVariables, newStepLabelMappings);
        }

        public IDictionary<string, object> GetVariables()
        {
            return _variables
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

                // ReSharper disable once TailRecursiveCall
                return Cache(stepLabelMapping, variables, stepLabelMappings, out key);
            }

            if (variables.TryGetValue(constant, out key))
                return (variables, stepLabelMappings);

            var next = variables.Count;
            
            while (next > 0 || key == null)
            {
                key = (char)('a' + next % 26) + key;
                next = next / 26;
            }

            key = "_" + key;
            return (variables.Add(constant, key), stepLabelMappings);
        }
    }
}
