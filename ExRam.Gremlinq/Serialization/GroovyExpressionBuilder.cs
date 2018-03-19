using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace ExRam.Gremlinq
{
    public struct GroovyExpressionBuilder
    {
        public enum State
        {
            Idle,
            Chaining,
        }

        private readonly State _state;
        private readonly StringBuilder _stringBuilder;
        private readonly IImmutableDictionary<object, string> _variables;
        private readonly IImmutableDictionary<StepLabel, string> _stepLabelMappings;

        public GroovyExpressionBuilder(State state, StringBuilder stringBuilder, IImmutableDictionary<object, string> variables, IImmutableDictionary<StepLabel, string> stepLabelMappings)
        {
            this._state = state;
            this._variables = variables;
            this._stringBuilder = stringBuilder;
            this._stepLabelMappings = stepLabelMappings;
        }

        public GroovyExpressionBuilder AppendIdentifier(string className)
        {
            if (this._state != State.Idle)
                throw new InvalidOperationException();

            this._stringBuilder.Append(className);
            return new GroovyExpressionBuilder(State.Chaining, this._stringBuilder, this._variables, this._stepLabelMappings);
        }

        public GroovyExpressionBuilder AppendLambda(string lambda)
        {
            this._stringBuilder.Append("{");
            this._stringBuilder.Append(lambda);
            this._stringBuilder.Append("}");

            return this;
        }

        public GroovyExpressionBuilder AppendMethod(string methodName, IEnumerable<object> parameters)
        {
            var setComma = false;
            var subMethodBuilder = this;

            if (this._state == State.Chaining)
                this._stringBuilder.Append(".");

            this._stringBuilder.Append(methodName);
            this._stringBuilder.Append("(");
            
            foreach (var parameter in parameters)
            {
                if (setComma)
                    this._stringBuilder.Append(", ");

                subMethodBuilder = new GroovyExpressionBuilder(State.Idle, this._stringBuilder, subMethodBuilder._variables, subMethodBuilder._stepLabelMappings);
                {
                    subMethodBuilder = parameter is IGremlinSerializable serializable
                        ? serializable.Serialize(subMethodBuilder)
                        : subMethodBuilder.AppendConstant(parameter);
                }

                setComma = true;
            }

            this._stringBuilder.Append(")");

            return new GroovyExpressionBuilder(State.Chaining, this._stringBuilder, subMethodBuilder._variables, subMethodBuilder._stepLabelMappings);
        }

        public GroovyExpressionBuilder AppendField(string fieldName)
        {
            if (this._state != State.Chaining)
                throw new InvalidOperationException();

            this._stringBuilder.Append(".");
            this._stringBuilder.Append(fieldName);

            return this;
        }

        public GroovyExpressionBuilder AppendConstant(object constant)
        {
            if (this._state == State.Chaining)
                throw new InvalidOperationException();

            if (constant is Enum)
            {
                return this
                    .AppendIdentifier(constant.GetType().Name)
                    .AppendField(Enum.GetName(constant.GetType(), constant));
            }

            var (newVariables, newStepLabelMappings) = Cache(constant, this._variables, this._stepLabelMappings, out var key);
            this._stringBuilder.Append(key);

            return new GroovyExpressionBuilder(this._state, this._stringBuilder, newVariables, newStepLabelMappings);
        }

        public (string queryString, IDictionary<string, object> parameters) ToExpression()
        {
             return (
                this._stringBuilder.ToString(),
                this._variables
                    .ToDictionary(kvp => kvp.Value, kvp => kvp.Key));

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