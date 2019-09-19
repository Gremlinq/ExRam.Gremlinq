namespace ExRam.Gremlinq.Core
{
    public interface ISerializedGremlinQueryAssembler
    {
        void Identifier(string identifier);

        void Field(string fieldName);

        void OpenMethod(string methodName);
        void CloseMethod();

        void StartParameter();
        void EndParameter();

        void Lambda(string methodName);

        void Constant(object constant);

        object Assemble();
    }
}
