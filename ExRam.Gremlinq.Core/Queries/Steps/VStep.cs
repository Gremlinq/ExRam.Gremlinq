namespace ExRam.Gremlinq.Core
{
    public sealed class VStep : Step
    {
        public VStep(object[] ids)
        {
            Ids = ids;
        }

        public object[] Ids { get; }
    }
}

