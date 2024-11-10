using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public static class FileName
    {
        public static string OfThis([CallerFilePath] string? sourceFile = "") => sourceFile ?? throw new InvalidOperationException();
    }
}
