using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.Serialization;
using NullGuard;

namespace ExRam.Gremlinq.Core
{
    public abstract class P : IGremlinQueryAtom
    {
        #region Nested
        // ReSharper disable once InconsistentNaming
        public abstract class SingleArgumentP : P
        {
            protected SingleArgumentP([AllowNull] object argument)
            {
                Argument = argument;
            }

            [AllowNull]
            public object Argument { get; }
        }

        private sealed class FalseP : P
        {
            public override void Accept(IGremlinQueryElementVisitor visitor)
            {
                throw new InvalidOperationException("P.False is not supposed to be serialized to groovy. Something went wrong...");
            }
        }

        private sealed class TrueP : P
        {
            public override void Accept(IGremlinQueryElementVisitor visitor)
            {
                throw new InvalidOperationException("P.True is not supposed to be serialized to groovy. Something went wrong...");
            }
        }

        public sealed class Eq : SingleArgumentP
        {
            public Eq([AllowNull] object argument) : base(argument)
            {
            }

            public override void Accept(IGremlinQueryElementVisitor visitor)
            {
                visitor.Visit(this);
            }
        }

        public sealed class Neq : SingleArgumentP
        {
            public Neq([AllowNull] object argument) : base(argument)
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

            internal static P From(Expression expression)
            {
                if (expression.GetValue() is IEnumerable enumerable)
                    return new P.Within(enumerable.Cast<object>().ToArray());

                throw new ExpressionNotSupportedException(expression);
            }

            public override void Accept(IGremlinQueryElementVisitor visitor)
            {
                visitor.Visit(this);
            }
        }

        public sealed class Without : P
        {
            public object[] Arguments { get; }

            public Without(object[] arguments)
            {
                Arguments = arguments;
            }

            internal static P From(Expression expression)
            {
                if (expression.GetValue() is IEnumerable enumerable)
                    return new P.Without(enumerable.Cast<object>().ToArray());

                throw new ExpressionNotSupportedException(expression);
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

        public sealed class Outside : P
        {
            public Outside(object lower, object upper)
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

        private P()
        {
        }

        public abstract void Accept(IGremlinQueryElementVisitor visitor);

        internal static readonly P True = new TrueP();
        internal static readonly P False = new FalseP();
    }
}
