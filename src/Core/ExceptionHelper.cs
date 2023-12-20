namespace ExRam.Gremlinq.Core
{
    internal static class ExceptionHelper
    {
        public static InvalidOperationException UninitializedStruct() => new ("The struct has not been initialized and cannot be accessed.");
    }
}
