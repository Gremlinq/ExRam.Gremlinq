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

        public class MethodStep1 : MethodStep
        {
            private readonly object _parameter;

            public MethodStep1(string name, object parameter) : base(name)
            {
                _parameter = parameter;
            }

            protected override IEnumerable<object> ResolveParameters(IGraphModel model)
            {
                yield return _parameter is IGremlinQuery query ? query.Resolve(model) : _parameter;
            }
        }

        public class MethodStep2 : MethodStep
        {
            private readonly object _parameter1;
            private readonly object _parameter2;

            public MethodStep2(string name, object parameter1, object parameter2) : base(name)
            {
                _parameter1 = parameter1;
                _parameter2 = parameter2;
            }

            protected override IEnumerable<object> ResolveParameters(IGraphModel model)
            {
                yield return _parameter1 is IGremlinQuery query1 ? query1.Resolve(model) : _parameter1;
                yield return _parameter2 is IGremlinQuery query2 ? query2.Resolve(model) : _parameter2;
            }
        }

        public class MethodStep3 : MethodStep
        {
            private readonly object _parameter1;
            private readonly object _parameter2;
            private readonly object _parameter3;

            public MethodStep3(string name, object parameter1, object parameter2, object parameter3) : base(name)
            {
                _parameter1 = parameter1;
                _parameter2 = parameter2;
                _parameter3 = parameter3;
            }

            protected override IEnumerable<object> ResolveParameters(IGraphModel model)
            {
                yield return _parameter1 is IGremlinQuery query1 ? query1.Resolve(model) : _parameter1;
                yield return _parameter2 is IGremlinQuery query2 ? query2.Resolve(model) : _parameter2;
                yield return _parameter3 is IGremlinQuery query3 ? query3.Resolve(model) : _parameter3;
            }
        }

        public class MethodStep4 : MethodStep
        {
            private readonly object _parameter1;
            private readonly object _parameter2;
            private readonly object _parameter3;
            private readonly object _parameter4;

            public MethodStep4(string name, object parameter1, object parameter2, object parameter3, object parameter4) : base(name)
            {
                _parameter1 = parameter1;
                _parameter2 = parameter2;
                _parameter3 = parameter3;
                _parameter4 = parameter4;
            }

            protected override IEnumerable<object> ResolveParameters(IGraphModel model)
            {
                yield return _parameter1 is IGremlinQuery query1 ? query1.Resolve(model) : _parameter1;
                yield return _parameter2 is IGremlinQuery query2 ? query2.Resolve(model) : _parameter2;
                yield return _parameter3 is IGremlinQuery query3 ? query3.Resolve(model) : _parameter3;
                yield return _parameter4 is IGremlinQuery query4 ? query4.Resolve(model) : _parameter4;
            }
        }

        public class MethodStepN : MethodStep
        {
            public MethodStepN(string name, object[] parameters) : base(name)
            {
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
