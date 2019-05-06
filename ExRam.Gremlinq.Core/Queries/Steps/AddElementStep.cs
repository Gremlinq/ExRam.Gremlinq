using System;
using System.Linq;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public abstract class AddElementStep : Step
    {
        protected AddElementStep(IGraphElementModel elementModel, object value)
        {
            var valueType = value.GetType();

            //TODO: Cache!
            Label = valueType
                .GetTypeHierarchy()
                .Where(type => !type.IsAbstract)
                .Select(type => elementModel.Labels
                    .TryGetValue(type)
                    .IfNone(valueType.Name))
                .FirstOrDefault();
        }

        public string Label { get; }
    }
}
