// ReSharper disable HeapView.PossibleBoxingAllocation
using System;

using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Core.AspNet
{
    internal sealed class ProviderSetupInfo<TConfigurator>
        where TConfigurator : IProviderConfigurator<TConfigurator>
    {
        public ProviderSetupInfo(string sectionName, Func<IConfigurableGremlinQuerySource, Func<TConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource> providerChoice)
        {
            SectionName = sectionName;
            ProviderChoice = providerChoice;
        }

        public string SectionName { get; }
        public Func<IConfigurableGremlinQuerySource, Func<TConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource> ProviderChoice { get; }
    }
}
