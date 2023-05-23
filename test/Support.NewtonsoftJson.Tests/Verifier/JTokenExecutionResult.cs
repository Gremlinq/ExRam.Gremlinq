using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Support.NewtonsoftJson.Tests.Verifier
{
    public readonly struct JTokenExecutionResult
    {
        private readonly JToken? _result;

        public JTokenExecutionResult(JToken result)
        {
            _result = result;
        }

        public JToken Result => _result ?? throw new InvalidOperationException();
    }
}
