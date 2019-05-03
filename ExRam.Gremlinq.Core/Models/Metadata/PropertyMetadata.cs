using System;
using System.Collections.Generic;
using System.Text;

namespace ExRam.Gremlinq.Core
{
    public class PropertyMetadata
    {
        public bool IsIgnoredOnUpdate { get; internal set; }

        public bool IsIgnoredAlways { get; internal set; }

        public PropertyMetadata()
        {

        }

        public PropertyMetadata(PropertyMetadata source)
        {
            IsIgnoredOnUpdate = source.IsIgnoredOnUpdate;
            IsIgnoredAlways = source.IsIgnoredAlways;
        }
    }
}
