using System;

namespace ExRam.Gremlinq.Core
{
    [Flags]
    public enum IgnoreDirective
    {
        Never = 0,

        OnAdd = 1,
        OnUpdate = 2,

        Always = 3
    }

    public class PropertyMetadata
    {
        public PropertyMetadata(IgnoreDirective ignoreDirective)
        {
            IgnoreDirective = ignoreDirective;
        }

        public IgnoreDirective IgnoreDirective { get; }
    }
}
