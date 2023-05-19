namespace ExRam.Gremlinq.Tests.Entities
{
    public readonly struct ProjectRecordStruct
    {
        public ProjectRecordStruct(object? @in, object? @out, object? count, object? properties)
        {
            In = @in;
            Out = @out;
            Count = count;
            Properties = properties;
        }

        public object? In { get; }
        public object? Out { get; }
        public object? Count { get; }
        public object? Properties { get; }
    }
}
