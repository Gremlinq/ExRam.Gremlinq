namespace ExRam.Gremlinq
{
    public interface IGremlinSerializable
    {
        MethodStringBuilder Serialize(MethodStringBuilder builder, IParameterCache parameterCache);
    }
}