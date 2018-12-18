namespace ExRam.Gremlinq.Core
{
#pragma warning disable 660,661
    public sealed class T : GremlinEnum<T>
#pragma warning restore 660,661
    {
        public static readonly T Id = new T("id");

        private T(string name) : base(name)
        {
        }

        public static bool operator ==(T left, T right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(T left, T right)
        {
            return !Equals(left, right);
        }
    }
}
