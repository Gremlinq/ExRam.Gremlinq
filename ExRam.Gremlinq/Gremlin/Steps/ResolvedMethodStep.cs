using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExRam.Gremlinq
{
    public class ResolvedMethodStep : TerminalStep
    {
        public static readonly ResolvedMethodStep Id = new ResolvedMethodStep("id");
        public static readonly ResolvedMethodStep Barrier = new ResolvedMethodStep("barrier");
        public static readonly ResolvedMethodStep Order = new ResolvedMethodStep("order");
        public static readonly ResolvedMethodStep Create = new ResolvedMethodStep("create");
        public static readonly ResolvedMethodStep Unfold = new ResolvedMethodStep("unfold");
        public static readonly ResolvedMethodStep Identity = new ResolvedMethodStep("identity");
        public static readonly ResolvedMethodStep Emit = new ResolvedMethodStep("emit");
        public static readonly ResolvedMethodStep Dedup = new ResolvedMethodStep("dedup");
        public static readonly ResolvedMethodStep OutV = new ResolvedMethodStep("outV");
        public static readonly ResolvedMethodStep OtherV = new ResolvedMethodStep("otherV");
        public static readonly ResolvedMethodStep InV = new ResolvedMethodStep("inV");
        public static readonly ResolvedMethodStep BothV = new ResolvedMethodStep("bothV");
        public static readonly ResolvedMethodStep Drop = new ResolvedMethodStep("drop");
        public static readonly ResolvedMethodStep Fold = new ResolvedMethodStep("fold");
        public static readonly ResolvedMethodStep Explain = new ResolvedMethodStep("explain");
        public static readonly ResolvedMethodStep Profile = new ResolvedMethodStep("profile");
        public static readonly ResolvedMethodStep Count = new ResolvedMethodStep("count");
        public static readonly ResolvedMethodStep Build = new ResolvedMethodStep("build");

        private readonly string _name;
        private readonly IEnumerable<object> _parameters;

        public ResolvedMethodStep(string name) : this(name, Enumerable.Empty<object>())
        {

        }

        public ResolvedMethodStep(string name, params object[] parameters) : this(name, (IEnumerable<object>)parameters)
        {

        }

        public ResolvedMethodStep(string name, IEnumerable<object> parameters)
        {
            _name = name;
            _parameters = parameters;
        }

        public override GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state)
        {
            return state.AppendMethod(stringBuilder, _name, _parameters);
        }
    }
}
