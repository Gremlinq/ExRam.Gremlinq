using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public static class SourceFileName
    {
        public static string Of<T>() where T : ISourceFileNameProvider<T> => T.GetSourceFileName();

        public static string OfThis([CallerFilePath] string? sourceFile = "") => sourceFile ?? throw new InvalidOperationException();
    }
}
