namespace ExRam.Gremlinq.Providers.Neptune
{
    /// <summary>
    /// Represents a disabled AWS signer that can be upgraded to a SigV4 signer.
    /// </summary>
    public interface IDisabledAWSSigner : IAWSSigner
    {
        /// <summary>
        /// Creates a new <see cref="ISigV4AWSSigner"/> with default settings.
        /// </summary>
        ISigV4AWSSigner UseSigV4();
    }
}
