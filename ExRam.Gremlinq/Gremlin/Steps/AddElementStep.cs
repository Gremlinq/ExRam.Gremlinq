namespace ExRam.Gremlinq
{
    public abstract class AddElementStep : Step
    {
        public object Value { get; }

        protected AddElementStep(object value)
        {
            Value = value;
        }
    }
}
