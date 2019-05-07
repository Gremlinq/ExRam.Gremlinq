using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public struct PropertyMetadata
    {
        public static readonly PropertyMetadata Default = new PropertyMetadata(default, default);

        public PropertyMetadata(Option<string> nameOverride, SerializationDirective ignoreDirective)
        {
            IgnoreDirective = ignoreDirective;
            NameOverride = nameOverride;
        }

        public Option<string> NameOverride { get; }
        public SerializationDirective IgnoreDirective { get; }
    }
}
