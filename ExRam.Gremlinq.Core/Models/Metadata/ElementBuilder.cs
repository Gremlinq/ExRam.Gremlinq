using System;
using System.Collections.Generic;
using System.Text;

namespace ExRam.Gremlinq.Core
{
    internal class ElementBuilder : IElementBuilder
    {
        private readonly IMetadataStore _metadataStore;

        public ElementBuilder(IMetadataStore metadataStore)
        {
            _metadataStore = metadataStore;
        }

        public IMetadataBuilder<TElement> Element<TElement>()
        {
            return new MetadataBuilder<TElement>(_metadataStore);
        }
    }
}
