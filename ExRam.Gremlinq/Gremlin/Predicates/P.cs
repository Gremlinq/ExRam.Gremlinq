using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ExRam.Gremlinq
{
    public abstract class P : IGroovySerializable
    {
        #region Nested
        public abstract class SingleArgumentP : P
        {
            protected SingleArgumentP(string name, object argument) : base(name)
            {
                Argument = argument;
            }

            public override GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state)
            {
                return base.Serialize(stringBuilder, state)
                    .AppendMethod(stringBuilder, Name, Argument);
            }

            public object Argument { get; }
        }

        private sealed class Constant : P
        {
            public Constant() : base("")
            {
            }

            public override GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state)
            {
                throw new InvalidOperationException();
            }
        }

        public sealed class Eq : SingleArgumentP
        {
            public Eq(object argument) : base("eq", argument)
            {
            }
        }

        public sealed class Neq : SingleArgumentP
        {
            public Neq(object argument) : base("neq", argument)
            {
            }
        }

        public sealed class Lt : SingleArgumentP
        {
            public Lt(object argument) : base("lt", argument)
            {
            }
        }

        public sealed class Lte : SingleArgumentP
        {
            public Lte(object argument) : base("lte", argument)
            {
            }
        }

        public sealed class Gte : SingleArgumentP
        {
            public Gte(object argument) : base("gte", argument)
            {
            }
        }

        public sealed class Gt : SingleArgumentP
        {
            public Gt(object argument) : base("gt", argument)
            {
            }
        }

        public sealed class Within : P
        {
            private readonly object[] _arguments;

            public Within(object[] arguments) : base("within")
            {
                _arguments = arguments;
            }

            public override GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state)
            {
                return base.Serialize(stringBuilder, state)
                    .AppendMethod(stringBuilder, Name, _arguments);
            }
        }

        public sealed class Between : P
        {
            private readonly object _lower;
            private readonly object _upper;

            public Between(object lower, object upper) : base("between")
            {
                _lower = lower;
                _upper = upper;
            }

            public override GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state)
            {
                return base.Serialize(stringBuilder, state)
                    .AppendMethod(stringBuilder, Name, new[]{ _lower, _upper });
            }
        }
        #endregion

        private static readonly IReadOnlyDictionary<ExpressionType, Func<object, P>> SupportedComparisons = new Dictionary<ExpressionType, Func<object, P>>
        {
            { ExpressionType.Equal, _ => new Eq(_) },
            { ExpressionType.NotEqual, _ => new Neq(_) },
            { ExpressionType.LessThan, _ => new Lt(_) },
            { ExpressionType.LessThanOrEqual, _ => new Lte(_) },
            { ExpressionType.GreaterThanOrEqual, _ => new Gte(_) },
            { ExpressionType.GreaterThan, _ => new Gt(_) }
        };

        private P(string name)
        {
            Name = name;
        }

        public virtual GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state)
        {
            return state
                .AppendIdentifier(stringBuilder, nameof(P));
        }

        public static P ForExpressionType(ExpressionType expressionType, object argument)
        {
            return SupportedComparisons[expressionType](argument);
        }

        public string Name { get; }

        internal static readonly P True = new Constant();
        internal static readonly P False = new Constant();
    }
}
