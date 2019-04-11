using System;
using System.Collections.Generic;
using System.Text;

namespace ExRam.Gremlinq.Core.Attributes
{
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class ReadOnlyAttribute : Attribute
    {
        public ReadOnlyAttribute()
        {
        }
    }
}
