using System;

namespace ExRam.Gremlinq.Core
{
    public interface ISerializedGremlinQueryAssembler
    {
        void Identifier(string identifier);

        void Field(string fieldName);

        void Method(string methodName, params Action[] parameters);

        void Lambda(string methodName);

        void Constant(object constant);

        object Assemble();
    }
}
