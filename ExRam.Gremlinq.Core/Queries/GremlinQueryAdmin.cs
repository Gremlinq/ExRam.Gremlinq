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
            return admin.ConfigureSteps(x => x.InsertRange(admin.Steps.Count, steps));
        }

        public static IGremlinQuery InsertStep(this IGremlinQueryAdmin admin, int index, Step step)
        {
            return admin.ConfigureSteps(x => x.Insert(index, step));
        }

        public static IGremlinQuery InsertSteps(this IGremlinQueryAdmin admin, int index, Step[] steps)
        {
            return admin.ConfigureSteps(x => x.InsertRange(index, steps));
        }
    }
}
