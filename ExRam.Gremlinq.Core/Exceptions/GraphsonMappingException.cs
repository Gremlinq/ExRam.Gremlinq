using System;

namespace ExRam.Gremlinq.Providers
{
    public sealed class GraphsonMappingException : Exception
    {
        public GraphsonMappingException()
        {
        }

        public GraphsonMappingException(string message) : base(message)
        {
        }

        public GraphsonMappingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
