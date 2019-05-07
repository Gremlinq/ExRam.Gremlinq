using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public struct MemberMetadata
    {
        public static readonly MemberMetadata Default = new MemberMetadata(default, default);

        public MemberMetadata(Option<string> identifierOverride, SerializationDirective ignoreDirective)
        {
            IgnoreDirective = ignoreDirective;
            IdentifierOverride = identifierOverride;
        }

        public Option<string> IdentifierOverride { get; }
        public SerializationDirective IgnoreDirective { get; }
    }
}
