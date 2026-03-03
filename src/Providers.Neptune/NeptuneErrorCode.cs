using static ExRam.Gremlinq.Core.ExceptionHelper;

namespace ExRam.Gremlinq.Providers.Neptune
{
    /// <summary>
    /// Represents a Neptune-specific error code returned by the server.
    /// </summary>
    public readonly struct NeptuneErrorCode : IEquatable<NeptuneErrorCode>
    {
        /// <summary>Access denied.</summary>
        public static readonly NeptuneErrorCode AccessDeniedException = From(nameof(AccessDeniedException));
        /// <summary>Bad request.</summary>
        public static readonly NeptuneErrorCode BadRequestException = From(nameof(BadRequestException));
        /// <summary>Query cancelled by user.</summary>
        public static readonly NeptuneErrorCode CancelledByUserException = From(nameof(CancelledByUserException));
        /// <summary>Concurrent modification conflict.</summary>
        public static readonly NeptuneErrorCode ConcurrentModificationException = From(nameof(ConcurrentModificationException));
        /// <summary>Constraint violation.</summary>
        public static readonly NeptuneErrorCode ConstraintViolationException = From(nameof(ConstraintViolationException));
        /// <summary>Internal server failure.</summary>
        public static readonly NeptuneErrorCode InternalFailureException = From(nameof(InternalFailureException));
        /// <summary>Invalid numeric data.</summary>
        public static readonly NeptuneErrorCode InvalidNumericDataException = From(nameof(InvalidNumericDataException));
        /// <summary>Invalid parameter.</summary>
        public static readonly NeptuneErrorCode InvalidParameterException = From(nameof(InvalidParameterException));
        /// <summary>Malformed query.</summary>
        public static readonly NeptuneErrorCode MalformedQueryException = From(nameof(MalformedQueryException));
        /// <summary>Memory limit exceeded.</summary>
        public static readonly NeptuneErrorCode MemoryLimitExceededException = From(nameof(MemoryLimitExceededException));
        /// <summary>Method not allowed.</summary>
        public static readonly NeptuneErrorCode MethodNotAllowedException = From(nameof(MethodNotAllowedException));
        /// <summary>Missing parameter.</summary>
        public static readonly NeptuneErrorCode MissingParameterException = From(nameof(MissingParameterException));
        /// <summary>Query limit exceeded.</summary>
        public static readonly NeptuneErrorCode QueryLimitExceededException = From(nameof(QueryLimitExceededException));
        /// <summary>Query limit.</summary>
        public static readonly NeptuneErrorCode QueryLimitException = From(nameof(QueryLimitException));
        /// <summary>Query too large.</summary>
        public static readonly NeptuneErrorCode QueryTooLargeException = From(nameof(QueryTooLargeException));
        /// <summary>Read-only violation.</summary>
        public static readonly NeptuneErrorCode ReadOnlyViolationException = From(nameof(ReadOnlyViolationException));
        /// <summary>Request throttled.</summary>
        public static readonly NeptuneErrorCode ThrottlingException = From(nameof(ThrottlingException));
        /// <summary>Time limit exceeded.</summary>
        public static readonly NeptuneErrorCode TimeLimitExceededException = From(nameof(TimeLimitExceededException));
        /// <summary>Too many requests.</summary>
        public static readonly NeptuneErrorCode TooManyRequestsException = From(nameof(TooManyRequestsException));
        /// <summary>Unsupported operation.</summary>
        public static readonly NeptuneErrorCode UnsupportedOperationException = From(nameof(UnsupportedOperationException));
        /// <summary>Failure by query.</summary>
        public static readonly NeptuneErrorCode FailureByQueryException = From(nameof(FailureByQueryException));

        private readonly string? _code;

        private NeptuneErrorCode(string code)
        {
            if (code.Length == 0)
                throw new ArgumentException($"{nameof(code)} may not be empty.", nameof(code));

            _code = code;
        }

        /// <summary>
        /// Creates a <see cref="NeptuneErrorCode"/> from the specified error code string.
        /// </summary>
        /// <param name="code">The error code string.</param>
        public static NeptuneErrorCode From(string code)
        {
            ArgumentNullException.ThrowIfNull(code);

            return new (code);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is NeptuneErrorCode code && Equals(code);

        /// <inheritdoc />
        public bool Equals(NeptuneErrorCode other) => Code == other._code;

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(_code);

        /// <summary>
        /// Determines whether two <see cref="NeptuneErrorCode"/> values are equal.
        /// </summary>
        public static bool operator ==(NeptuneErrorCode left, NeptuneErrorCode right) => left.Equals(right);

        /// <summary>
        /// Determines whether two <see cref="NeptuneErrorCode"/> values are not equal.
        /// </summary>
        public static bool operator !=(NeptuneErrorCode left, NeptuneErrorCode right) => !(left == right);

        /// <summary>
        /// Gets the error code string.
        /// </summary>
        public string Code => _code ?? throw UninitializedStruct();
    }
}
