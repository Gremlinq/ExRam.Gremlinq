namespace ExRam.Gremlinq.Core
{
    internal static class GremlinQueryAdmin
    {
        public static IGremlinQuery AddStep(this IGremlinQueryAdmin admin, Step step)
        {
            return admin.InsertStep(admin.Steps.Count, step);
        }
    }
}