using System.Collections.Generic;
using System.Linq;

namespace ExRam.Gremlinq.Core
{
    public sealed class GroovyGremlinQuery
    {
        public GroovyGremlinQuery(string script, Dictionary<string, object> bindings)
        {
            Script = script;
            Bindings = bindings;
        }

        public override string ToString()
        {
            return Script;
        }

        public GroovyGremlinQuery Inline()
        {
            var script = Script;
            var newBindings = new Dictionary<string, object>();

            foreach (var kvp in Bindings.OrderByDescending(kvp => kvp.Key))
            {
                switch (kvp.Value)
                {
                    case string str:
                    {
                        script = script.Replace(kvp.Key, $"'{str}'");
                        break;
                    }
                    case int number:
                    {
                        script = script.Replace(kvp.Key, number.ToString());
                        break;
                    }
                    case long number:
                    {
                        script = script.Replace(kvp.Key, number.ToString());
                        break;
                    }
                    case bool boolean:
                    {
                        script = script.Replace(kvp.Key, boolean.ToString().ToLower());
                        break;
                    }
                    case {} obj:
                    {
                        if (obj.GetType().IsEnum && obj.GetType().GetEnumUnderlyingType() == typeof(int))
                            script = script.Replace(kvp.Key, ((int)obj).ToString());
                        else
                            newBindings[kvp.Key] = kvp.Value;

                        break;
                    }
                }
            }

            return new GroovyGremlinQuery(script, newBindings);
        }

        public string Script { get; }
        public Dictionary<string, object> Bindings { get; }
    }
}
