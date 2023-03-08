#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryDebugger
    {
        public static readonly IGremlinQueryDebugger Groovy = new GroovyGremlinQueryDebugger();
    }
}
