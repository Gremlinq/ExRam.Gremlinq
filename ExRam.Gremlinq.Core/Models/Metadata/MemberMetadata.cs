using System;

namespace ExRam.Gremlinq.Core
{
    [Flags]
    public enum SerializationDirective
    {
        Default = 0,

        IgnoreOnAdd = 1,
        IgnoreOnUpdate = 2,

        IgnoreAlways = 3
    }

    public sealed class MemberMetadata
    {
        public MemberMetadata(string identifier, SerializationDirective ignoreDirective)
        {
            Identifier = identifier;
            IgnoreDirective = ignoreDirective;
        }

        public string Identifier { get; }
        public SerializationDirective IgnoreDirective { get; }
    }
}
