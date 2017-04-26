using System.Threading;

namespace ExRam.Gremlinq
{
    public sealed class DefaultParameterNameProvider : IParameterNameProvider
    {
        private int _current;

        public string Get()
        {
            return "P" + Interlocked.Increment(ref this._current);
        }
    }
}