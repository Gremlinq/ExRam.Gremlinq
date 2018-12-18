using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public sealed class TimesStep : Step
    {
        public TimesStep(int count)
        {
            Count = count;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public int Count { get; }
    }
}
