using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public interface IPebbleEnumerator<out TSource> : IEnumerator<TSource>
    {
        void DropPebble();
        void LiftPebble();
        void Return();
    }
}
