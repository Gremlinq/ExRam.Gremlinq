namespace ExRam.Gremlinq.Core.Steps;

public class PartitionStrategyStep : Step
{
    public string PartitionKey { get; }

    public PartitionStrategyStep(string partitionKey)
    {
        PartitionKey = partitionKey;
    }
}
