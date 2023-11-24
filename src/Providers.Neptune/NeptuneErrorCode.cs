using static ExRam.Gremlinq.Core.ExceptionHelper;

namespace ExRam.Gremlinq.Providers.Neptune
{
    public readonly struct NeptuneErrorCode : IEquatable<NeptuneErrorCode>
    {
        public static readonly NeptuneErrorCode AccessDeniedException = From(nameof(AccessDeniedException));
        public static readonly NeptuneErrorCode BadRequestException = From(nameof(BadRequestException));
        public static readonly NeptuneErrorCode CancelledByUserException = From(nameof(CancelledByUserException));
        public static readonly NeptuneErrorCode ConcurrentModificationException = From(nameof(ConcurrentModificationException));
        public static readonly NeptuneErrorCode ConstraintViolationException = From(nameof(ConstraintViolationException));
        public static readonly NeptuneErrorCode InternalFailureException = From(nameof(InternalFailureException));
        public static readonly NeptuneErrorCode InvalidNumericDataException = From(nameof(InvalidNumericDataException));
        public static readonly NeptuneErrorCode InvalidParameterException = From(nameof(InvalidParameterException));
        public static readonly NeptuneErrorCode MalformedQueryException = From(nameof(MalformedQueryException));
        public static readonly NeptuneErrorCode MemoryLimitExceededException = From(nameof(MemoryLimitExceededException));
        public static readonly NeptuneErrorCode MethodNotAllowedException = From(nameof(MethodNotAllowedException));
        public static readonly NeptuneErrorCode MissingParameterException = From(nameof(MissingParameterException));
        public static readonly NeptuneErrorCode QueryLimitExceededException = From(nameof(QueryLimitExceededException));
        public static readonly NeptuneErrorCode QueryLimitException = From(nameof(QueryLimitException));
        public static readonly NeptuneErrorCode QueryTooLargeException = From(nameof(QueryTooLargeException));
        public static readonly NeptuneErrorCode ReadOnlyViolationException = From(nameof(ReadOnlyViolationException));
        public static readonly NeptuneErrorCode ThrottlingException = From(nameof(ThrottlingException));
        public static readonly NeptuneErrorCode TimeLimitExceededException = From(nameof(TimeLimitExceededException));
        public static readonly NeptuneErrorCode TooManyRequestsException = From(nameof(TooManyRequestsException));
        public static readonly NeptuneErrorCode UnsupportedOperationException = From(nameof(UnsupportedOperationException));
        public static readonly NeptuneErrorCode FailureByQueryException = From(nameof(FailureByQueryException));

        private readonly string? _code;

        private NeptuneErrorCode(string code)
        {
            if (code.Length == 0)
                throw new ArgumentException($"{nameof(code)} may not be empty.", nameof(code));

            _code = code;
        }

        public static NeptuneErrorCode From(string code) => new (code);

        public override bool Equals(object? obj) => obj is NeptuneErrorCode code && Equals(code);

        public bool Equals(NeptuneErrorCode other) => Code == other._code;

        public override int GetHashCode() => HashCode.Combine(_code);

        public static bool operator ==(NeptuneErrorCode left, NeptuneErrorCode right) => left.Equals(right);

        public static bool operator !=(NeptuneErrorCode left, NeptuneErrorCode right) => !(left == right);

        public string Code { get => _code ?? throw UninitializedStruct(); }
    }
}
