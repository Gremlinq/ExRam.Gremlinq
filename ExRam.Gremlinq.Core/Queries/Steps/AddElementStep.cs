using System;
using System.Linq;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public abstract class AddElementStep : Step
    {
        protected AddElementStep(IGraphElementModel elementModel, object value)
        {
            //TODO: Cache!
            Label = value.GetType()
                .GetTypeHierarchy()
                .Where(type => !type.IsAbstract)
                .SelectMany(type => elementModel.Labels
                    .TryGetValue(type))
                .FirstOrDefault();
        }

        public string Label { get; }
    }
}
