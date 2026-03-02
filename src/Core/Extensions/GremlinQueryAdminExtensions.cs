using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryAdminExtensions
    {
        /// <summary>
        /// Adds multiple steps to the query's traversal.
        /// </summary>
        /// <typeparam name="TTargetQuery">The target query type to return.</typeparam>
        /// <param name="admin">The query admin interface.</param>
        /// <param name="steps">The steps to add. Must contain at least one step.</param>
        public static TTargetQuery AddSteps<TTargetQuery>(this IGremlinQueryAdmin admin, IEnumerable<Step> steps)
            where TTargetQuery : IGremlinQueryBase
        {
            ArgumentNullException.ThrowIfNull(admin);
            ArgumentNullException.ThrowIfNull(steps);

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
