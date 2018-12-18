namespace ExRam.Gremlinq
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