using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using LanguageExt;

namespace ExRam.Gremlinq.Dse
{
    public static class DseGraphSchemaExtensions
    {
        public static DseGraphSchema ToGraphSchema(this IGraphModel model)
        {
            return new DseGraphSchema(model.EdgeConnectionClosure());
        }
    }
}