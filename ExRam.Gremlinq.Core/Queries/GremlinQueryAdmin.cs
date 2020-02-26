using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    internal static class GremlinQueryAdmin
    {
        public static IGremlinQueryBase AddStep(this IGremlinQueryAdmin admin, Step step)
        {
            return admin.ConfigureSteps(steps => steps.Push(step));
        }

        public static IGremlinQueryBase AddSteps(this IGremlinQueryAdmin admin, IEnumerable<Step> steps)
        {
            return admin.ConfigureSteps(existingSteps =>
            {
                foreach (var step in steps)
                {
                    existingSteps = existingSteps.Push(step);
                }

                return existingSteps;
            });
        }
    }
}
