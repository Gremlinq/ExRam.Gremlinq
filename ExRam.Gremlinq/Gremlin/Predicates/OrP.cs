using System.Text;

namespace ExRam.Gremlinq
{
    internal struct OrP : IGroovySerializable
    {
        private readonly P[] _predicates;

        public OrP(params P[] predicates)
        {
            this._predicates = predicates;
        }

        public GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state)
        {
            for (var i = 0; i < this._predicates.Length; i++)
            {
                state = i != 0
                    ? state.AppendMethod(stringBuilder, "or", this._predicates[i]) 
                    : this._predicates[i].Serialize(stringBuilder, state);
            }

            return state;
        }
    }
}