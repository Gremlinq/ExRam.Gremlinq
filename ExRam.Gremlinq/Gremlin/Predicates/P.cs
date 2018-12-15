using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public abstract class P : IGremlinQueryElement
    {
        #region Nested
        public abstract class SingleArgumentP : P
        {
            protected SingleArgumentP(object argument)
            {
                Argument = argument;
            }

            public object Argument { get; }
        }

        private sealed class Constant : P
        {
            public override void Accept(IGremlinQueryElementVisitor visitor)
            {
                throw new InvalidOperationException();
            }
        }

        public sealed class Eq : SingleArgumentP
        {
            public Eq(object argument) : base(argument)
            {
            }

            public override void Accept(IGremlinQueryElementVisitor visitor)
            {
                visitor.Visit(this);
            }
        }

        public sealed class Neq : SingleArgumentP
        {
            public Neq(object argument) : base(argument)
            {
            }

            public override void Accept(IGremlinQueryElementVisitor visitor)
            {
                visitor.Visit(this);
            }
        }

        public sealed class Lt : SingleArgumentP
        {
            public Lt(object argument) : base(argument)
            {
            }

            public override void Accept(IGremlinQueryElementVisitor visitor)
            {
                visitor.Visit(this);
            }
        }

        public sealed class Lte : SingleArgumentP
        {
            public Lte(object argument) : base(argument)
            {
            }

            public override void Accept(IGremlinQueryElementVisitor visitor)
            {
                visitor.Visit(this);
            }
        }

        public sealed class Gte : SingleArgumentP
        {
            public Gte(object argument) : base(argument)
            {
            }

            public override void Accept(IGremlinQueryElementVisitor visitor)
            {
                visitor.Visit(this);
            }
        }

        public sealed class Gt : SingleArgumentP
        {
            public Gt(object argument) : base(argument)
            {
            }

            public override void Accept(IGremlinQueryElementVisitor visitor)
            {
                visitor.Visit(this);
            }
        }

        public sealed class Within : P
        {
            public object[] Arguments { get; }

            public Within(object[] arguments)
            {
                Arguments = arguments;
            }

            public override void Accept(IGremlinQueryElementVisitor visitor)
            {
                visitor.Visit(this);
            }
        }

        public sealed class Between : P
        {
            public Between(object lower, object upper)
            {
                Lower = lower;
                Upper = upper;
            }

            public override void Accept(IGremlinQueryElementVisitor visitor)
            {
                visitor.Visit(this);
            }

            public object Lower { get; }
            public object Upper { get; }
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

        private P()
        {
        }

        public abstract void Accept(IGremlinQueryElementVisitor visitor);

        public static P ForExpressionType(ExpressionType expressionType, object argument)
        {
            return SupportedComparisons[expressionType](argument);
        }

        internal static readonly P True = new Constant();
    }
}
