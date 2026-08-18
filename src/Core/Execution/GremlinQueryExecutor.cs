using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Core.Execution
{
    /// <summary>Provides factory methods and extensions for <see cref="IGremlinQueryExecutor"/>.</summary>
    public static class GremlinQueryExecutor
    {
        private sealed class InvalidGremlinQueryExecutor : IGremlinQueryExecutor
        {
            public async IAsyncEnumerable<T> Execute<T>(GremlinQueryExecutionContext context)
            {
                throw new InvalidOperationException($"'{nameof(IGremlinQueryExecutor.Execute)}' must not be called on {nameof(GremlinQueryExecutor)}.{nameof(Invalid)}. If you are getting this exception while executing a query, set a proper {nameof(GremlinQueryExecutor)} on the {nameof(GremlinQuerySource)} (e.g. with 'g.UseGremlinServer(...)' for GremlinServer which can be found in the 'ExRam.Gremlinq.Providers.GremlinServer' package).");
#pragma warning disable CS0162 // Unreachable code detected
                yield break;
#pragma warning restore CS0162 // Unreachable code detected
            }
        }

        private sealed class EmptyGremlinQueryExecutor : IGremlinQueryExecutor
        {
            public IAsyncEnumerable<T> Execute<T>(GremlinQueryExecutionContext context) => AsyncEnumerable.Empty<T>();
        }

        private sealed class TransformQueryGremlinQueryExecutor : IGremlinQueryExecutor
        {
            private readonly IGremlinQueryExecutor _baseExecutor;
            private readonly Func<IGremlinQueryBase, IGremlinQueryBase> _transformation;

            public TransformQueryGremlinQueryExecutor(IGremlinQueryExecutor baseExecutor, Func<IGremlinQueryBase, IGremlinQueryBase> transformation)
            {
                _transformation = transformation;
                _baseExecutor = baseExecutor;
            }

            public IAsyncEnumerable<T> Execute<T>(GremlinQueryExecutionContext context)
            {
                return Core(this, context);

                static async IAsyncEnumerable<T> Core(TransformQueryGremlinQueryExecutor @this, GremlinQueryExecutionContext context, [EnumeratorCancellation] CancellationToken ct = default)
                {
                    await foreach (var item in @this._baseExecutor.Execute<T>(context.TransformQuery(@this._transformation)).WithCancellation(ct).ConfigureAwait(false))
                    {
                        yield return item;
                    }
                }
            }
        }

        private sealed class TransformExecutionExceptionGremlinQueryExecutor : IGremlinQueryExecutor
        {
            private readonly IGremlinQueryExecutor _baseExecutor;
            private readonly Func<GremlinQueryExecutionException, GremlinQueryExecutionException> _exceptionTransformation;

            public TransformExecutionExceptionGremlinQueryExecutor(IGremlinQueryExecutor baseExecutor, Func<GremlinQueryExecutionException, GremlinQueryExecutionException> exceptionTransformation)
            {
                _baseExecutor = baseExecutor;
                _exceptionTransformation = exceptionTransformation;
            }

            public IAsyncEnumerable<T> Execute<T>(GremlinQueryExecutionContext context)
            {
                return Core(this, context);

                static async IAsyncEnumerable<T> Core(TransformExecutionExceptionGremlinQueryExecutor @this, GremlinQueryExecutionContext context, [EnumeratorCancellation] CancellationToken ct = default)
                {
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
                    await using (var enumerator = @this._baseExecutor.Execute<T>(context).WithCancellation(ct).ConfigureAwait(false).GetAsyncEnumerator())
#pragma warning restore CA2007
                    {
                        while (true)
                        {
                            try
                            {
                                if (!await enumerator.MoveNextAsync())
                                    yield break;
                            }
                            catch (GremlinQueryExecutionException ex)
                            {
                                throw @this._exceptionTransformation(ex);
                            }

                            yield return enumerator.Current;
                        }
                    }
                }
            }
        }

        private sealed class SerializingGremlinQueryExecutor : IGremlinQueryExecutor
        {
            private readonly SemaphoreSlim _semaphore = new(1);
            private readonly IGremlinQueryExecutor _baseExecutor;

            public SerializingGremlinQueryExecutor(IGremlinQueryExecutor baseExecutor)
            {
                _baseExecutor = baseExecutor;
            }

            public IAsyncEnumerable<T> Execute<T>(GremlinQueryExecutionContext context)
            {
                return Core(context, this);

                static async IAsyncEnumerable<T> Core(GremlinQueryExecutionContext context, SerializingGremlinQueryExecutor @this, [EnumeratorCancellation] CancellationToken ct = default)
                {
                    await @this._semaphore.WaitAsync(ct).ConfigureAwait(false);

                    try
                    {
                        await foreach (var item in @this._baseExecutor.Execute<T>(context).WithCancellation(ct).ConfigureAwait(false))
                        {
                            yield return item;
                        }
                    }
                    finally
                    {
                        @this._semaphore.Release();
                    }
                }
            }
        }

        /// <summary>
        /// An executor that returns empty results for any query.
        /// </summary>
        public static readonly IGremlinQueryExecutor Empty = new EmptyGremlinQueryExecutor();

        /// <summary>
        /// An executor that throws when any query is executed. This is the default executor that signals no provider has been configured.
        /// </summary>
        public static readonly IGremlinQueryExecutor Invalid = new InvalidGremlinQueryExecutor();

        /// <summary>
        /// Wraps the executor so that queries are transformed before execution.
        /// </summary>
        /// <param name="baseExecutor">The base executor.</param>
        /// <param name="transformation">A function that transforms the query before execution.</param>
        public static IGremlinQueryExecutor TransformQuery(this IGremlinQueryExecutor baseExecutor, Func<IGremlinQueryBase, IGremlinQueryBase> transformation)
        {
            ArgumentNullException.ThrowIfNull(baseExecutor);
            ArgumentNullException.ThrowIfNull(transformation);

            return new TransformQueryGremlinQueryExecutor(baseExecutor, transformation);
        }

        /// <summary>
        /// Wraps the executor so that execution exceptions are transformed before being thrown.
        /// </summary>
        /// <param name="executor">The base executor.</param>
        /// <param name="exceptionTransformation">A function that transforms execution exceptions.</param>
        public static IGremlinQueryExecutor TransformExecutionException(this IGremlinQueryExecutor executor, Func<GremlinQueryExecutionException, GremlinQueryExecutionException> exceptionTransformation)
        {
            ArgumentNullException.ThrowIfNull(executor);
            ArgumentNullException.ThrowIfNull(exceptionTransformation);

            return new TransformExecutionExceptionGremlinQueryExecutor(executor, exceptionTransformation);
        }

        /// <summary>
        /// Wraps the executor so that queries are executed serially (one at a time).
        /// </summary>
        /// <param name="executor">The base executor.</param>
        public static IGremlinQueryExecutor Serialize(this IGremlinQueryExecutor executor)
        {
            ArgumentNullException.ThrowIfNull(executor);

            return new SerializingGremlinQueryExecutor(executor);
        }
    }
}
