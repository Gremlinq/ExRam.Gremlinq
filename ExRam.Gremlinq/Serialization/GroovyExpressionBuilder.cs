using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        private readonly IImmutableDictionary<StepLabel, string> _stepLabelMappings;
        
        public GroovyExpressionBuilder(State state, StringBuilder stringBuilder, IImmutableDictionary<object, string> variables, IImmutableDictionary<StepLabel, string> stepLabelMappings)
        {
            this._state = state;
            this.Variables = variables;
            this._stepLabelMappings = stepLabelMappings;
            this._stringBuilder = stringBuilder;
        }

        public GroovyExpressionBuilder AppendIdentifier(string className)
        {
            if (this._state != State.Idle)
                throw new InvalidOperationException();

            this._stringBuilder.Append(className);
            return new GroovyExpressionBuilder(State.Chaining, this._stringBuilder, this.Variables, this._stepLabelMappings);
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

                subMethodBuilder = new GroovyExpressionBuilder(State.Idle, this._stringBuilder, subMethodBuilder.Variables, subMethodBuilder._stepLabelMappings);
                {
                    subMethodBuilder = parameter is IGremlinSerializable serializable
                        ? serializable.Serialize(subMethodBuilder)
                        : subMethodBuilder.AppendConstant(parameter);
                }

                setComma = true;
            }

            this._stringBuilder.Append(")");

            return new GroovyExpressionBuilder(State.Chaining, this._stringBuilder, subMethodBuilder.Variables, subMethodBuilder._stepLabelMappings);
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

            var (newVariables, newStepLabelMappings) = this.Cache(constant, this.Variables, this._stepLabelMappings, out var key);
            this._stringBuilder.Append(key);

            return new GroovyExpressionBuilder(this._state, this._stringBuilder, newVariables, newStepLabelMappings);
        }

        private (IImmutableDictionary<object, string> variables, IImmutableDictionary<StepLabel, string> stepLabelMappings) Cache(object constant, IImmutableDictionary<object, string> variables, IImmutableDictionary<StepLabel, string> stepLabelMappings, out string key)
        {
            if (constant is StepLabel stepLabel)
            {
                if (!stepLabelMappings.TryGetValue(stepLabel, out var stepLabelMapping))
                {
                    stepLabelMapping = "l" + (stepLabelMappings.Count + 1);
                    stepLabelMappings = stepLabelMappings.Add(stepLabel, stepLabelMapping);
                }

                return this.Cache(stepLabelMapping, variables, stepLabelMappings, out key);
            }

            if (variables.TryGetValue(constant, out key))
                return (variables, stepLabelMappings);

            key = "_P" + (variables.Count + 1);
            return (variables.Add(constant, key), stepLabelMappings);
        }

        public IImmutableDictionary<object, string> Variables { get; }
    }
}