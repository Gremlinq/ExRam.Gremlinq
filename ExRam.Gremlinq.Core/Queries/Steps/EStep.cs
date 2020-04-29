namespace ExRam.Gremlinq.Core
{
    public sealed class EStep : Step
    {
        public EStep(object[] ids)
        {
            Ids = ids;
        }

        public object[] Ids { get; }
    }
}

