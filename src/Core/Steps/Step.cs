namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Base class for all Gremlin traversal steps.</summary>
    public abstract class Step
    {
        /// <summary>Initializes a new instance of <see cref="Step"/>.</summary>
        /// <param name="sideEffectSemanticsChange">Indicates the side-effect semantics of this step.</param>
        protected Step(SideEffectSemanticsChange sideEffectSemanticsChange = SideEffectSemanticsChange.None)
        {
            SideEffectSemanticsChange = sideEffectSemanticsChange;
        }

        /// <summary>Gets the side-effect semantics change introduced by this step.</summary>
        public SideEffectSemanticsChange SideEffectSemanticsChange { get; }
    }
}
