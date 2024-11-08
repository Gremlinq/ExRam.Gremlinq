using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public static class ThisFile
    {
        public static string GetName([CallerFilePath] string? sourceFile = "") => sourceFile ?? throw new InvalidOperationException();
    }
}
