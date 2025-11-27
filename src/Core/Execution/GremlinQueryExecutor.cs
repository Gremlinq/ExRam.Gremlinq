using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Core.Execution
{
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

        public static readonly IGremlinQueryExecutor Empty = new EmptyGremlinQueryExecutor();

        public static readonly IGremlinQueryExecutor Invalid = new InvalidGremlinQueryExecutor();

        public static IGremlinQueryExecutor TransformQuery(this IGremlinQueryExecutor baseExecutor, Func<IGremlinQueryBase, IGremlinQueryBase> transformation) => new TransformQueryGremlinQueryExecutor(baseExecutor, transformation);

        public static IGremlinQueryExecutor TransformExecutionException(this IGremlinQueryExecutor executor, Func<GremlinQueryExecutionException, GremlinQueryExecutionException> exceptionTransformation) => new TransformExecutionExceptionGremlinQueryExecutor(executor, exceptionTransformation);

        public static IGremlinQueryExecutor Serialize(this IGremlinQueryExecutor executor) => new SerializingGremlinQueryExecutor(executor);
    }
}
