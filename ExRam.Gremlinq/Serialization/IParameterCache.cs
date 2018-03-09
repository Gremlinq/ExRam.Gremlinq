using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public interface IParameterCache
    {
        string Cache(object parameter);
        IDictionary<string, object> GetDictionary();
    }
}