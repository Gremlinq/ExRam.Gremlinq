﻿using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class MinStep : Step
    {
        public static readonly MinStep Local = new(Scope.Local);
        public static readonly MinStep Global = new(Scope.Global);

        public MinStep(Scope scope, QuerySemantics? semantics = default) : base(semantics)
        {
            Scope = scope;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new MinStep(Scope, semantics);

        public Scope Scope { get; }
    }
}
