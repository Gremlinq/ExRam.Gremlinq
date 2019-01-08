using System;

namespace ExRam.Gremlinq.Core
{
    public sealed class GraphModelException : Exception
    {
        public GraphModelException()
        {
        }

        public GraphModelException(string message) : base(message)
        {
        }

        public GraphModelException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}