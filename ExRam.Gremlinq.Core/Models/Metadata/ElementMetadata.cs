using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public struct ElementMetadata
    {
        public static readonly MemberMetadata Default = new MemberMetadata(default, default);

        public ElementMetadata(Option<string> labelOverride)
        {
            LabelOverride = labelOverride;
        }

        public Option<string> LabelOverride { get; }
    }
}