namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// An immutable dictionary of <see cref="GremlinqOption{TValue}"/> values that control query behavior.
    /// </summary>
    public interface IGremlinqOptions
    {
        /// <summary>
        /// Gets the value of the specified option, or its default if not set.
        /// </summary>
        /// <typeparam name="TValue">The type of the option value.</typeparam>
        /// <param name="option">The option to retrieve.</param>
        TValue GetValue<TValue>(GremlinqOption<TValue> option);

        /// <summary>
        /// Determines whether the specified option has been explicitly set.
        /// </summary>
        /// <typeparam name="TValue">The type of the option value.</typeparam>
        /// <param name="option">The option to check.</param>
        bool Contains<TValue>(GremlinqOption<TValue> option);

        /// <summary>
        /// Configures the value of the specified option by applying a transformation to its current value.
        /// </summary>
        /// <typeparam name="TValue">The type of the option value.</typeparam>
        /// <param name="option">The option to configure.</param>
        /// <param name="configuration">A function that transforms the current value.</param>
        IGremlinqOptions ConfigureValue<TValue>(GremlinqOption<TValue> option, Func<TValue, TValue> configuration);

        /// <summary>
        /// Sets the value of the specified option.
        /// </summary>
        /// <typeparam name="TValue">The type of the option value.</typeparam>
        /// <param name="option">The option to set.</param>
        /// <param name="value">The value to set.</param>
        IGremlinqOptions SetValue<TValue>(GremlinqOption<TValue> option, TValue value);

        /// <summary>
        /// Removes the specified option, causing it to return its default value.
        /// </summary>
        /// <typeparam name="TValue">The type of the option value.</typeparam>
        /// <param name="option">The option to remove.</param>
        IGremlinqOptions Remove<TValue>(GremlinqOption<TValue> option);
    }
}
