using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExRam.Gremlinq.Core.Serialization;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public static class SerializedGremlinQueryAssemblerFactory
    {
        private sealed class InvalidSerializedGremlinQueryAssemblerFactory : ISerializedGremlinQueryAssemblerFactory
        {
            private sealed class InvalidSerializedGremlinQueryAssembler : ISerializedGremlinQueryAssembler
            {
                public void Identifier(string identifier)
                {
                    throw new InvalidOperationException();
                }

                public void Field(string fieldName)
                {
                    throw new InvalidOperationException();
                }

                public void OpenMethod(string methodName)
                {
                    throw new InvalidOperationException();
                }

                public void CloseMethod()
                {
                    throw new InvalidOperationException();
                }

                public void StartParameter()
                {
                    throw new InvalidOperationException();
                }

                public void EndParameter()
                {
                    throw new InvalidOperationException();
                }

                public void Lambda(string methodName)
                {
                    throw new InvalidOperationException();
                }

                public void Constant(object constant)
                {
                    throw new InvalidOperationException();
                }

                public object Assemble()
                {
                    throw new InvalidOperationException();
                }
            }

            private static readonly ISerializedGremlinQueryAssembler Instance = new InvalidSerializedGremlinQueryAssembler();

            public ISerializedGremlinQueryAssembler Create()
            {
                return Instance;
            }
        }

        private sealed class UnitSerializedGremlinQueryAssemblerFactory : ISerializedGremlinQueryAssemblerFactory
        {
            private sealed class UnitSerializedGremlinQueryAssembler : ISerializedGremlinQueryAssembler
            {
                public void Identifier(string identifier)
                {
                    
                }

                public void Field(string fieldName)
                {
                }

                public void OpenMethod(string methodName)
                {
                }

                public void CloseMethod()
                {
                }

                public void StartParameter()
                {
                }

                public void EndParameter()
                {
                }

                public void Lambda(string methodName)
                {
                }

                public void Constant(object constant)
                {
                }

                public object Assemble()
                {
                    return LanguageExt.Unit.Default;
                }
            }

            private static readonly ISerializedGremlinQueryAssembler Instance = new UnitSerializedGremlinQueryAssembler();

            public ISerializedGremlinQueryAssembler Create()
            {
                return Instance;
            }
        }

        private sealed class GroovySerializedGremlinQueryAssemblerFactory : ISerializedGremlinQueryAssemblerFactory
        {
            private sealed class GroovySerializedGremlinQueryAssembler : ISerializedGremlinQueryAssembler
            {
                private enum State
                {
                    Idle,
                    Chaining,
                    InMethodBeforeFirstParameter,
                    InMethodAfterFirstParameter
                }

                private State _state = State.Idle;

                private readonly StringBuilder _builder = new StringBuilder();
                private readonly Stack<State> _stateStack = new Stack<State>();
                private readonly Dictionary<object, string> _variables = new Dictionary<object, string>();
                private readonly Dictionary<StepLabel, string> _stepLabelNames = new Dictionary<StepLabel, string>();

                public void Field(string fieldName)
                {
                    if (_state != State.Idle && _state != State.Chaining)
                        throw new InvalidOperationException();

                    if (_state == State.Chaining)
                        _builder.Append(".");

                    _builder.Append(fieldName);
                }

                public void Lambda(string lambda)
                {
                    if (_state != State.Idle)
                        throw new InvalidOperationException();

                    _builder.Append("{");
                    _builder.Append(lambda);
                    _builder.Append("}");
                }

                public void Constant(object constant)
                {
                    if (_state == State.Chaining)
                        throw new InvalidOperationException();

                    _builder.Append(Cache(constant));
                }

                private string Cache(object constant)
                {
                    if (constant is StepLabel stepLabel)
                    {
                        if (!_stepLabelNames.TryGetValue(stepLabel, out var stepLabelMapping))
                        {
                            stepLabelMapping = "l" + (_stepLabelNames.Count + 1);
                            _stepLabelNames.Add(stepLabel, stepLabelMapping);
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

                public void Identifier(string identifier)
                {
                    if (_state != State.Idle)
                        throw new InvalidOperationException();

                    _builder.Append(identifier);
                    _state = State.Chaining;
                }

                public void OpenMethod(string methodName)
                {
                    if (_state != State.Idle && _state != State.Chaining)
                        throw new InvalidOperationException();

                    if (_state == State.Chaining)
                        _builder.Append(".");

                    _builder.Append(methodName);
                    _builder.Append("(");

                    _stateStack.Push(State.Chaining);
                    _state = State.InMethodBeforeFirstParameter;
                }

                public void CloseMethod()
                {
                    _builder.Append(")");
                    _state = _stateStack.Pop();
                }

                public void StartParameter()
                {
                    if (_state != State.InMethodBeforeFirstParameter && _state != State.InMethodAfterFirstParameter)
                        throw new InvalidOperationException();

                    if (_state == State.InMethodAfterFirstParameter)
                        _builder.Append(", ");

                    _stateStack.Push(State.InMethodAfterFirstParameter);
                    _state = State.Idle;
                }

                public void EndParameter()
                {
                    _state = _stateStack.Pop();
                }

                public object Assemble()
                {
                    return new GroovySerializedGremlinQuery(
                        _builder.ToString(),
                        _variables
                            .ToDictionary(kvp => kvp.Value, kvp => kvp.Key));
                }
            }

            public ISerializedGremlinQueryAssembler Create()
            {
                return new GroovySerializedGremlinQueryAssembler();
            }
        }

        public static readonly ISerializedGremlinQueryAssemblerFactory Unit = new UnitSerializedGremlinQueryAssemblerFactory();

        public static readonly ISerializedGremlinQueryAssemblerFactory Invalid = new InvalidSerializedGremlinQueryAssemblerFactory();

        public static readonly ISerializedGremlinQueryAssemblerFactory Groovy = new GroovySerializedGremlinQueryAssemblerFactory();
    }
}
