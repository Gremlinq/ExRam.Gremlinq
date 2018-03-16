using System.Text;

namespace ExRam.Gremlinq
{
    internal struct OrP : IGremlinSerializable
    {
        private readonly P[] _predicates;

        public OrP(params P[] predicates)
        {
            this._predicates = predicates;
        }

        public void Serialize(IMethodStringBuilder builder, IParameterCache parameterCache)
        {
            for (var i = 0; i < this._predicates.Length; i++)
            {
                if (i != 0)
                    builder.AppendMethod("or", new object[] { this._predicates[i] }, parameterCache);
                else
                    this._predicates[i].Serialize(builder, parameterCache);
            }
        }
    }
}