namespace ExRam.Gremlinq
{
    public class EdgeSchemaInfo
    {
        public EdgeTypeInfo TypeInfo { get; }

        public EdgeSchemaInfo(EdgeTypeInfo typeInfo)
        {
            this.TypeInfo = typeInfo;
        }
    }
}