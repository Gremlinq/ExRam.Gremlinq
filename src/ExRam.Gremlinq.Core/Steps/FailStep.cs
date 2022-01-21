namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class FailStep : Step
    {
        public static readonly FailStep NoMessage = new ();

        public FailStep(string? message = null)
        {
            Message = message;
        }

        public string? Message { get; }
    }
}
