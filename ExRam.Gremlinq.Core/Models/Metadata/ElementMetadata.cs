using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public struct ElementMetadata
    {
        public static readonly ElementMetadata Default = new ElementMetadata(default);

        public ElementMetadata(Option<string> labelOverride)
        {
            LabelOverride = labelOverride;
        }

        public Option<string> LabelOverride { get; }
    }
}
