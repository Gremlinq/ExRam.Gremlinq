using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public struct PropertyMetadata
    {
        public static readonly PropertyMetadata Default = new PropertyMetadata(default, default);

        public PropertyMetadata(Option<string> identifierOverride, SerializationDirective ignoreDirective)
        {
            IgnoreDirective = ignoreDirective;
            IdentifierOverride = identifierOverride;
        }

        public Option<string> IdentifierOverride { get; }
        public SerializationDirective IgnoreDirective { get; }
    }
}
