using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExRam.Gremlinq
{
    public interface IGraphSchemaCreator
    {
        Task CreateSchema(IGraphModel model, CancellationToken ct);
    }
}
