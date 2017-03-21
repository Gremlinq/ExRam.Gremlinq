namespace ExRam.Gremlinq
{
    internal struct SpecialGremlinString
    {
        private readonly string _value;

        public SpecialGremlinString(string value)
        {
            this._value = value;
        }

        public override string ToString()
        {
            return this._value;
        }
    }
}