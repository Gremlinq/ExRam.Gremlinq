using System;
using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryAdmin
    {
        public static TTargetQuery AddSteps<TTargetQuery>(this IGremlinQueryAdmin admin, IEnumerable<Step> steps)
            where TTargetQuery : IGremlinQueryBase
        {
            var ret = default(IGremlinQueryBase?);

            foreach (var step in steps)
            {
                ret = admin.AddStep<IGremlinQueryBase>(step);
                admin = ret.AsAdmin();
            }

            return ret == null
                ? throw new ArgumentException($"{nameof(steps)} must contain at least one step.", nameof(steps))
                : ret.AsAdmin().ChangeQueryType<TTargetQuery>();
        }
    }
}
