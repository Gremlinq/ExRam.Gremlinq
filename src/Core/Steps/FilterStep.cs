namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Base class for filter steps that reduce the traversal stream.</summary>
    public abstract class FilterStep : Step, IFilterStep
    {
        /// <summary>Represents the <c>by()</c> modulator with a traversal argument applied to a filter step.</summary>
        public sealed class ByTraversalStep : Step
        {
            /// <summary>Initializes a new instance of <see cref="ByTraversalStep"/>.</summary>
            /// <param name="traversal">The by-modulator traversal.</param>
            public ByTraversalStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
            {
                Traversal = traversal;
            }

            /// <summary>Gets the by-modulator traversal.</summary>
            public Traversal Traversal { get; }
        }

        /// <summary>Initializes a new instance of <see cref="FilterStep"/>.</summary>
        /// <param name="sideEffectSemanticsChange">The side-effect semantics change.</param>
        protected FilterStep(SideEffectSemanticsChange sideEffectSemanticsChange) : base(sideEffectSemanticsChange)
        {

        }
    }
}
