﻿using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class HasPredicateStep : Step, IFilterStep
    {
        public HasPredicateStep(Key key, P predicate)
        {
            Key = key;
            Predicate = predicate;
        }

        public Key Key { get; }
        public P Predicate { get; }
    }
}
