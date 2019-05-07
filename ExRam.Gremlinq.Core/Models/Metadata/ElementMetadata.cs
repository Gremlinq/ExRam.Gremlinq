using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public struct ElementMetadata
    {
        public static readonly PropertyMetadata Default = new PropertyMetadata(default, default);

        public ElementMetadata(Option<string> labelOverride)
        {
            LabelOverride = labelOverride;
        }

        public Option<string> LabelOverride { get; }
    }
}
