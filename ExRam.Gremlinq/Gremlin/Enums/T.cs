namespace ExRam.Gremlinq
{
    public sealed class T : GremlinEnum<T>
    {
        public static readonly T Id = new T("id");

        private T(string name) : base(name)
        {
        }

        public static bool operator ==(T obj1, T obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(T obj1, T obj2)
        {
            return !obj1.Equals(obj2);
        }
    }
}
