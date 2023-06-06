namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class ReleaseOnlyFactAttribute : FactAttribute
    {
        public ReleaseOnlyFactAttribute()
        {
#if DEBUG
            Skip = "Skipped in Debug mode";
#endif
        }

    }
}
