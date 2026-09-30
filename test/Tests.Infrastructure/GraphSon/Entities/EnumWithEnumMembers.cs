using System.Runtime.Serialization;

namespace ExRam.Gremlinq.Tests.Infrastructure.GraphSon.Entities
{
    // What an [EnumMember] calls a value may be anything a string can be - a comma included,
    // which is otherwise what separates one name from the next.
    public enum EnumWithEnumMembers
    {
        [EnumMember(Value = "not-started")]
        NotStarted,

        [EnumMember(Value = "started, not done")]
        InProgress,

        Done
    }
}
