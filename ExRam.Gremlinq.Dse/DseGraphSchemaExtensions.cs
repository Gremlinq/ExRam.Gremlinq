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
        public static DseGraphModel ToGraphSchema(this IGraphModel model)
        {
            model = model.EdgeConnectionClosure();

            return new DseGraphModel(model.VertexTypes, model.EdgeTypes, model.Connections);
        }
    }
}