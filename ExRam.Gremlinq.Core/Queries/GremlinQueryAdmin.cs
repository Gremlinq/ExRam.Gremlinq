using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    internal static class GremlinQueryAdmin
    {
        public static IGremlinQueryBase AddStep(this IGremlinQueryAdmin admin, Step step)
        {
            return admin.InsertStep(admin.Steps.Count, step);
        }

        public static IGremlinQueryBase AddSteps(this IGremlinQueryAdmin admin, IEnumerable<Step> steps)
        {
            return admin.ConfigureSteps(x => x.InsertRange(admin.Steps.Count, steps));
        }

        public static IGremlinQueryBase InsertStep(this IGremlinQueryAdmin admin, int index, Step step)
        {
            return admin.ConfigureSteps(x => x.Insert(index, step));
        }

        public static IGremlinQueryBase InsertSteps(this IGremlinQueryAdmin admin, int index, IEnumerable<Step> steps)
        {
            return admin.ConfigureSteps(x => x.InsertRange(index, steps));
        }
    }
}
