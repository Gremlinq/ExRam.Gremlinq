using System;
using System.Collections.Generic;
using System.Text;

namespace ExRam.Gremlinq
{
    public abstract class MethodStep : TerminalStep
    {
        public static readonly MethodStep Id = Create("id");
        public static readonly MethodStep Barrier = Create("barrier");
        public static readonly MethodStep Order = Create("order");
        public static readonly MethodStep CreateStep = Create("create");
        public static readonly MethodStep Unfold = Create("unfold");
        public static readonly MethodStep Identity = Create("identity");
        public static readonly MethodStep Emit = Create("emit");
        public static readonly MethodStep Dedup = Create("dedup");
        public static readonly MethodStep OutV = Create("outV");
        public static readonly MethodStep OtherV = Create("otherV");
        public static readonly MethodStep InV = Create("inV");
        public static readonly MethodStep BothV = Create("bothV");
        public static readonly MethodStep Drop = Create("drop");
        public static readonly MethodStep Fold = Create("fold");
        public static readonly MethodStep Explain = Create("explain");
        public static readonly MethodStep Profile = Create("profile");
        public static readonly MethodStep Count = Create("count");
        public static readonly MethodStep Build = Create("build");
        public static readonly MethodStep Limit1 = Create("limit", 1);
        
        public sealed class MethodStep0 : MethodStep
        {
            public MethodStep0(string name) : base(name)
            {
            }

            public override GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state)
            {
                return state.AppendMethod(stringBuilder, Name, Array.Empty<object>());
            }
        }

        public sealed class MethodStep1 : MethodStep
        {
            private readonly object _parameter;

            public MethodStep1(string name, object parameter) : base(name)
            {
                _parameter = parameter;
            }

            public override GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state)
            {
                return state.AppendMethod(stringBuilder, Name, _parameter);
            }
        }

        public sealed class MethodStep2 : MethodStep
        {
            private readonly object _parameter1;
            private readonly object _parameter2;

            public MethodStep2(string name, object parameter1, object parameter2) : base(name)
            {
                _parameter1 = parameter1;
                _parameter2 = parameter2;
            }

            public override GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state)
            {
                return state.AppendMethod(stringBuilder, Name, new[] { _parameter1, _parameter2 });
            }
        }

        public sealed class MethodStep3 : MethodStep
        {
            private readonly object _parameter1;
            private readonly object _parameter2;
            private readonly object _parameter3;

            public MethodStep3(string name, object parameter1, object parameter2, object parameter3) : base(name)
            {
                _parameter1 = parameter1;
                _parameter2 = parameter2;
                _parameter3 = parameter3;
            }

            public override GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state)
            {
                return state.AppendMethod(stringBuilder, Name, new[] { _parameter1, _parameter2, _parameter3 });
            }
        }

        public sealed class MethodStep4 : MethodStep
        {
            private readonly object _parameter1;
            private readonly object _parameter2;
            private readonly object _parameter3;
            private readonly object _parameter4;

            public MethodStep4(string name, object parameter1, object parameter2, object parameter3, object parameter4) : base(name)
            {
                _parameter1 = parameter1;
                _parameter2 = parameter2;
                _parameter3 = parameter3;
                _parameter4 = parameter4;
            }

            public override GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state)
            {
                return state.AppendMethod(stringBuilder, Name, new[] { _parameter1, _parameter2, _parameter3, _parameter4 });
            }
        }

        public sealed class MethodStepN : MethodStep
        {
            private readonly IEnumerable<object> _parameters;

            public MethodStepN(string name, IEnumerable<object> parameters) : base(name)
            {
                _parameters = parameters;
            }

            public override GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state)
            {
                return state.AppendMethod(stringBuilder, Name, _parameters);
            }
        }

        protected MethodStep(string name)
        {
            Name = name;
        }

        public static MethodStep Create(string name)
        {
            return new MethodStep0(name);
        }

        public static MethodStep Create(string name, object parameter)
        {
            return new MethodStep1(name, parameter);
        }

        public static MethodStep Create(string name, object parameter1, object parameter2)
        {
            return new MethodStep2(name, parameter1, parameter2);
        }

        public static MethodStep Create(string name, object parameter1, object parameter2, object parameter3)
        {
            return new MethodStep3(name, parameter1, parameter2, parameter3);
        }

        public static MethodStep Create(string name, object parameter1, object parameter2, object parameter3, object parameter4)
        {
            return new MethodStep4(name, parameter1, parameter2, parameter3, parameter4);
        }

        public static MethodStep Create(string name, IEnumerable<object> parameters)
        {
            return new MethodStepN(name, parameters);
        }

        public string Name { get; }
    }
}
