namespace ExRam.Gremlinq.Core
{
    public abstract class FullScanStep : Step
    {
        protected FullScanStep(object[] ids)
        {
            Ids = ids;
        }

        public object[] Ids { get; }
    }
}
