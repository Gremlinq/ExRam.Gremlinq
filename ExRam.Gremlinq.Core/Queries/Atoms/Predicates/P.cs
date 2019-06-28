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

            internal override bool ContainsSingleStepLabel() => Argument is StepLabel;
        }

        private sealed class TrueP : P
        {
            public override void Accept(IGremlinQueryElementVisitor visitor)
            {
                throw new InvalidOperationException("P.True is not supposed to be serialized to groovy. Something went wrong...");
            }

            public override bool EqualsConstant(bool value) => value;

            internal override bool ContainsSingleStepLabel() => false;
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

            public override bool EqualsConstant(bool value) => !value && Arguments.Length == 0;

            internal override bool ContainsSingleStepLabel() => Arguments.Length == 1 && Arguments[0] is StepLabel;

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

            public override bool EqualsConstant(bool value) => value && Arguments.Length == 0;

            internal override bool ContainsSingleStepLabel() => Arguments.Length == 1 && Arguments[0] is StepLabel;

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

            internal override bool ContainsSingleStepLabel() => false;

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

            internal override bool ContainsSingleStepLabel() => false;
            
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

            public override bool EqualsConstant(bool value)
            {
                return value
                    ? Operand1.EqualsConstant(true) && Operand2.EqualsConstant(true)
                    : Operand1.EqualsConstant(false) || Operand2.EqualsConstant(false);
            }

            internal override bool ContainsSingleStepLabel() => Operand1.ContainsSingleStepLabel() && Operand2.ContainsSingleStepLabel();

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

            public override bool EqualsConstant(bool value)
            {
                return value
                    ? Operand1.EqualsConstant(true) || Operand2.EqualsConstant(true)
                    : Operand1.EqualsConstant(false) && Operand2.EqualsConstant(false);
            }

            internal override bool ContainsSingleStepLabel() => Operand1.ContainsSingleStepLabel() && Operand2.ContainsSingleStepLabel();
            
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

        public virtual bool EqualsConstant(bool value) => false;

        internal abstract bool ContainsSingleStepLabel();

        internal static readonly P True = new TrueP();
    }
}
