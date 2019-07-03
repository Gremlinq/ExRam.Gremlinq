namespace ExRam.Gremlinq.Core
{
    internal static class GremlinQueryAdmin
    {
        public static IGremlinQuery AddStep(this IGremlinQueryAdmin admin, Step step)
        {
            return admin.InsertStep(admin.Steps.Count, step);
        }

        public static IGremlinQuery AddSteps(this IGremlinQueryAdmin admin, Step[] steps)
        {
            return admin.InsertSteps(admin.Steps.Count, steps);
        }

        public static IGremlinQuery InsertStep(this IGremlinQueryAdmin admin, int index, Step step)
        {
            return admin.InsertSteps(index, new[] { step });
        }
    }
}
