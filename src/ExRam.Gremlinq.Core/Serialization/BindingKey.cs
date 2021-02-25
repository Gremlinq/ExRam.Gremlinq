using System;

namespace ExRam.Gremlinq.Core
{
    internal readonly struct BindingKey
    {
        private readonly int? _key;

        public BindingKey(int? key)
        {
            _key = key;
        }

        public static implicit operator BindingKey(int key)
        {
            return new(key);
        }

        public static implicit operator string(BindingKey key)
        {
            if (key._key is { } intKey)
            {
                var ret = string.Empty;
                
                do
                {
                    ret = (char)('a' + intKey % 26) + ret;
                    intKey /= 26;
                } while (intKey > 0);

                return "_" + ret;
            }

            throw new ArgumentException();
        }
    }
}
