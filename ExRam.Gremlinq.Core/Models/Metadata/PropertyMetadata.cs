using System;
using System.Collections.Generic;
using System.Text;

namespace ExRam.Gremlinq.Core
{
    public class PropertyMetadata
    {
        public bool IsReadOnly { get; internal set; }

        public bool IsIgnored { get; internal set; }

        public PropertyMetadata()
        {

        }

        public PropertyMetadata(PropertyMetadata source)
        {
            IsReadOnly = source.IsReadOnly;
            IsIgnored = source.IsIgnored;
        }
    }
}
