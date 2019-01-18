using System;
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
            public Within(object[] arguments)
            {
                Arguments = arguments;
            }

            public override void Accept(IGremlinQueryElementVisitor visitor)
            {
                visitor.Visit(this);
            }

            public object[] Arguments { get; }
        }

        public sealed class Without : P
        {
            public Without(object[] arguments)
            {
                Arguments = arguments;
            }

            public override void Accept(IGremlinQueryElementVisitor visitor)
            {
                visitor.Visit(this);
            }

            public object[] Arguments { get; }
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

        public sealed class AndP : P
        {
            public AndP(P operand1, P operand2)
            {
                Operand1 = operand1;
                Operand2 = operand2;
            }

            public override void Accept(IGremlinQueryElementVisitor visitor)
            {
                visitor.Visit(this);
            }

            public P Operand1 { get; }
            public P Operand2 { get; }
        }

        public sealed class OrP : P
        {
            public OrP(P operand1, P operand2)
            {
                Operand1 = operand1;
                Operand2 = operand2;
            }

            public override void Accept(IGremlinQueryElementVisitor visitor)
            {
                visitor.Visit(this);
            }

            public P Operand1 { get; }
            public P Operand2 { get; }
        }
        #endregion

        private P()
        {
        }

        public abstract void Accept(IGremlinQueryElementVisitor visitor);

        public P And(P p)
        {
            return new AndP(this, p);
        }

        public P Or(P p)
        {
            return new OrP(this, p);
        }

        internal static readonly P True = new TrueP();
    }
}
