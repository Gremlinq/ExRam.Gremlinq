using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    internal interface IPebbleEnumerator<out TSource> : IEnumerator<TSource>
    {
        void DropPebble();
        void LiftPebble();
        void Return();
    }
}
