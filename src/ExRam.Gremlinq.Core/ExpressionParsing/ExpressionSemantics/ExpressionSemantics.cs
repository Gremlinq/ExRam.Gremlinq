using System;

namespace ExRam.Gremlinq.Core
{
    internal abstract class ExpressionSemantics
    {
        private readonly Func<ExpressionSemantics> _flip;

        protected ExpressionSemantics(Func<ExpressionSemantics> flip)
        {
            _flip = flip;
        }

        public ExpressionSemantics Flip() => _flip();
    }
}
