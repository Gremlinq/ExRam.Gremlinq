using System;
using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public abstract class MethodStep : NonTerminalStep
    {
        public class MethodStep0 : MethodStep
        {
            public MethodStep0(string name) : base(name)
            {

            }

            protected override IEnumerable<object> ResolveParameters(IGraphModel model)
            {
                yield break;
            }
        }

        public class MethodStepN : MethodStep
        {
            public MethodStepN(string name, object parameter) : this(name, new[] {parameter})
            {

            }

            public MethodStepN(string name, object parameter1, object parameter2) : this(name,
                new[] {parameter1, parameter2})
            {

            }

            public MethodStepN(string name, object parameter1, object parameter2, object parameter3) : this(name,
                new[] {parameter1, parameter2, parameter3})
            {

            }

            public MethodStepN(string name, object parameter1, object parameter2, object parameter3, object parameter4) :
                this(name, new[] {parameter1, parameter2, parameter3, parameter4})
            {

            }

            public MethodStepN(string name, object[] parameters) : base(name)
            {
                Name = name;
                Parameters = parameters;
            }

            protected override IEnumerable<object> ResolveParameters(IGraphModel model)
            {
                foreach (var parameter in Parameters)
                {
                    if (parameter is IGremlinQuery subQuery)
                        yield return subQuery.Resolve(model);
                    else
                        yield return parameter;
                }
            }

            public string Name { get; }
            public object[] Parameters { get; }
        }

        protected MethodStep(string name)
        {
            Name = name;
        }

        public override IEnumerable<Step> Resolve(IGraphModel model)
        {
            yield return new ResolvedMethodStep(Name, ResolveParameters(model));
        }

        protected abstract IEnumerable<object> ResolveParameters(IGraphModel model);

        public string Name { get; }
    }
}
