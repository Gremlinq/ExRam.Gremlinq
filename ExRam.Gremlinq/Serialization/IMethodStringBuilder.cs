using System;
using System.Collections.Generic;
using System.Text;

namespace ExRam.Gremlinq
{
    public interface IMethodStringBuilder : IDisposable //TODO: internal?
    {
        void AppendIdentifier(string className);
        void AppendField(string fieldName);
        void AppendLambda(string lambda);
        void AppendMethod(string methodName, IEnumerable<object> parameters, IParameterCache parameterCache);
        void AppendConstant(object constant, IParameterCache parameterCache);
    }
}
