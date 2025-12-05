using System.Collections;
using System.Collections.Immutable;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ExRam.Gremlinq.Providers.Neptune
{
    public static class AWSSigner
    {
        private sealed class AWSV4SignerImpl : IAWSSigner
        {
            private sealed class AWSV4SignerHeaders : IReadOnlyDictionary<string, string>
            {
                private readonly KeyValuePair<string, string>[] _kvps;

                public AWSV4SignerHeaders(DateTimeOffset timestamp, string host, string xAmzDate, string xAmzExpires, string authorization)
                {
                    _kvps = [
                        new KeyValuePair<string, string>("host", host),
                        new KeyValuePair<string, string>("x-amz-date", xAmzDate),
                        new KeyValuePair<string, string>("x-amz-expires", xAmzExpires),
                        new KeyValuePair<string, string>("Authorization", authorization)
                    ];

                    Timestamp = timestamp;
                }

                string IReadOnlyDictionary<string, string>.this[string key]
                {
                    get
                    {
                        if (((IReadOnlyDictionary<string, string>)this).TryGetValue(key, out var value))
                            return value;

                        throw new KeyNotFoundException();
                    }
                }

                public DateTimeOffset Timestamp { get; }

                IEnumerable<string> IReadOnlyDictionary<string, string>.Keys => _kvps.Select(static x => x.Key);

                IEnumerable<string> IReadOnlyDictionary<string, string>.Values => _kvps.Select(static x => x.Value);

                int IReadOnlyCollection<KeyValuePair<string, string>>.Count => 4;

                bool IReadOnlyDictionary<string, string>.ContainsKey(string key) => ((IReadOnlyDictionary<string, string>)this).TryGetValue(key, out _);

                IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator() => ((IEnumerable<KeyValuePair<string, string>>)_kvps).GetEnumerator();

                IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<KeyValuePair<string, string>>)this).GetEnumerator();

                bool IReadOnlyDictionary<string, string>.TryGetValue(string key, out string value)
                {
                    for (var i = 0; i < _kvps.Length; i++)
                    {
                        if (_kvps[i].Key == key)
                        {
                            value = _kvps[i].Value;
                            return true;
                        }
                    }

                    value = null!;
                    return false;
                }
            }

            private const string Service = "neptune-db";
            private const string Algorithm = "AWS4-HMAC-SHA256";
            private const string SignedHeaders = "host;x-amz-date;x-amz-expires";

            private static readonly byte[] ServiceBytes = Encoding.UTF8.GetBytes(Service);
            private static readonly byte[] RequestBytes = Encoding.UTF8.GetBytes("aws4_request");

            private AWSV4SignerHeaders? _latestHeaders;

            private readonly Uri _uri;
            private readonly string _region;
            private readonly TimeSpan _cacheTime;
            private readonly string? _accessKeyId;
            private readonly byte[]? _secretAccessKey;
            private readonly Func<DateTimeOffset, AWSV4SignerHeaders>? _headersFactory;

            public static readonly AWSV4SignerImpl Empty = new (new Uri("ws://localhost:8182"), "us-east-1", null, null, null);

            private AWSV4SignerImpl(Uri uri, string region, string? accessKeyId, byte[]? secretAccessKey, TimeSpan? cacheTime)
            {
                if (string.IsNullOrEmpty(region))
                    throw new ArgumentException("region must not be null or empty.", nameof(region));

                _uri = uri;
                _region = region;
                _accessKeyId = accessKeyId;
                _secretAccessKey = secretAccessKey;
                _cacheTime = cacheTime ?? TimeSpan.FromMinutes(5);

                if (accessKeyId is not null && secretAccessKey is not null)
                {
                    var regionBytes = Encoding.UTF8.GetBytes(region);
                    var canonicalRequestPrefix = Encoding.UTF8.GetBytes($"GET\n{string.Join("/", uri.AbsolutePath.Split('/').Select(Uri.EscapeDataString))}\n{GetCanonicalQueryParams(uri.Query)}\nhost:{uri.Host}\nx-amz-date:");
                    var canonicalRequestPostfix = Encoding.UTF8.GetBytes($"\nx-amz-expires:{(int)_cacheTime.TotalSeconds}\n\n{SignedHeaders}\ne3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855");
                    var stringToSignTemplate = Encoding.UTF8.GetBytes($"{Algorithm}\nyyyyMMddTHHmmssZ\nyyyyMMdd/{region}/{Service}/aws4_request\n{new string('x', 64)}");

                    var authorizationTemplatePrefix = Encoding.UTF8.GetBytes($"{Algorithm} Credential={accessKeyId}/");
                    var authorizationTemplatePostfix = Encoding.UTF8.GetBytes($"/{region}/{Service}/aws4_request, SignedHeaders={SignedHeaders}, Signature={new string('x', 64)}");

                    var cacheTimeHeaderValue = ((int)_cacheTime.TotalSeconds).ToString();

                    _headersFactory = actualTime =>
                    {
                        Span<byte> hashSpan1 = stackalloc byte[32];
                        Span<byte> hashSpan2 = stackalloc byte[32];
                        Span<byte> stringToSignBytes = stackalloc byte[stringToSignTemplate.Length];
                        Span<byte> canonicalRequestBytes = stackalloc byte[canonicalRequestPrefix.Length + canonicalRequestPostfix.Length + 16];
                        Span<byte> authorizationBytes = stackalloc byte[authorizationTemplatePrefix.Length + authorizationTemplatePostfix.Length + 8];

#if NET8_0_OR_GREATER
                        Span<byte> timeSpan = stackalloc byte[16];
                        actualTime.TryFormat(timeSpan, out _, "yyyyMMddTHHmmssZ");
#else
                        Span<byte> timeSpan = Encoding.UTF8.GetBytes(actualTime.ToString("yyyyMMddTHHmmssZ"));
#endif

                        stringToSignTemplate.CopyTo(stringToSignBytes);

                        canonicalRequestPrefix.CopyTo(canonicalRequestBytes);
                        canonicalRequestPostfix.CopyTo(canonicalRequestBytes[(canonicalRequestPrefix.Length + 16)..]);

                        authorizationTemplatePrefix.CopyTo(authorizationBytes);
                        authorizationTemplatePostfix.CopyTo(authorizationBytes[(authorizationTemplatePrefix.Length + 8)..]);

                        timeSpan.CopyTo(canonicalRequestBytes[canonicalRequestPrefix.Length..]);
                        timeSpan.CopyTo(stringToSignBytes[17..]);

                        timeSpan[0..8].CopyTo(stringToSignBytes[34..]);
                        timeSpan[0..8].CopyTo(authorizationBytes[authorizationTemplatePrefix.Length..]);

                        SHA256.HashData(canonicalRequestBytes, hashSpan1);

                        ToHexStringLower(hashSpan1, stringToSignBytes[^64..]);

                        HMACSHA256.HashData(secretAccessKey, timeSpan[0..8], hashSpan1);
                        HMACSHA256.HashData(hashSpan1, regionBytes, hashSpan2);
                        HMACSHA256.HashData(hashSpan2, ServiceBytes, hashSpan1);
                        HMACSHA256.HashData(hashSpan1, RequestBytes, hashSpan2);
                        HMACSHA256.HashData(hashSpan2, stringToSignBytes, hashSpan1);

                        ToHexStringLower(hashSpan1, authorizationBytes[^64..]);

                        return new AWSV4SignerHeaders(actualTime, _uri.Host, actualTime.ToString("yyyyMMddTHHmmssZ"), cacheTimeHeaderValue, Encoding.UTF8.GetString(authorizationBytes));
                    };
                }
            }

            public IAWSSigner ConfigureUri(Func<Uri, Uri> transformation) => new AWSV4SignerImpl(transformation(_uri), _region, _accessKeyId, _secretAccessKey, _cacheTime);

            public IAWSSigner ConfigureRegion(Func<string, string> transformation) => new AWSV4SignerImpl(_uri, transformation(_region), _accessKeyId, _secretAccessKey, _cacheTime);

            public IAWSSigner ConfigureCacheTime(Func<TimeSpan, TimeSpan> transformation) => new AWSV4SignerImpl(_uri, _region, _accessKeyId, _secretAccessKey, transformation(_cacheTime));

            public IAWSSigner WithAccessKeyId(string accessKeyId) => new AWSV4SignerImpl(_uri, _region, accessKeyId, _secretAccessKey, _cacheTime);

            public IAWSSigner WithSecretAccessKey(string secretAccessKey) => new AWSV4SignerImpl(_uri, _region, _accessKeyId, Encoding.UTF8.GetBytes("AWS4" + secretAccessKey), _cacheTime);

            public IReadOnlyDictionary<string, string> GetIAMHeaders(DateTimeOffset? time = null)
            {
                if (_headersFactory is { } authorizationFactory)
                {
                    var actualTime = time ?? DateTimeOffset.UtcNow;
                    actualTime = new DateTimeOffset(actualTime.Ticks - (actualTime.Ticks % _cacheTime.Ticks), actualTime.Offset);

                    if (Volatile.Read(ref _latestHeaders) is { Timestamp: { } latestHeadersTimestamp } latestHeaders && latestHeadersTimestamp <= actualTime && (latestHeadersTimestamp + _cacheTime) > actualTime)
                        return latestHeaders;

                    var headers = _latestHeaders = _headersFactory(actualTime);

                    return headers;
                }

                if (_region is null)
                    throw Throw("Region.");

                if (_accessKeyId is null)
                    throw Throw("AccessKeyId.");

                throw Throw("SecretAccessKey.");
            }

            public static void ToHexStringLower(ReadOnlySpan<byte> source, Span<byte> utf8Destination)
            {
#if NET10_0_OR_GREATER
                Convert.TryToHexStringLower(source, utf8Destination, out _);
#else
                //TODO: Optimize for less allocations
                Encoding.UTF8.GetBytes(Convert.ToHexString(source).ToLowerInvariant()).CopyTo(utf8Destination);
#endif
            }

            private static string GetCanonicalQueryParams(string queryString)
            {
                var parsedQueryString = HttpUtility.ParseQueryString(queryString);
                var values = new SortedDictionary<string, IEnumerable<string>>(StringComparer.Ordinal);

                foreach (var maybeKey in parsedQueryString.AllKeys)
                {
                    var value = parsedQueryString[maybeKey] ?? string.Empty;

                    if (maybeKey is { } key)
                    {
                        var escapedKey = Uri.EscapeDataString(key);

                        values.Add(
                            escapedKey,
                            value.Split(',')
                                .OrderBy(v => v, StringComparer.Ordinal)
                                .Select(v => $"{escapedKey}={Uri.EscapeDataString(v)}"));
                    }
                    else
                    {
                        var escapedValue = Uri.EscapeDataString(value);

                        values.Add(escapedValue, new[] { $"{escapedValue}=" });
                    }
                }

                return string.Join("&", values.SelectMany(kvp => kvp.Value));
            }

            private static InvalidOperationException Throw(string messageDetail) => throw new InvalidOperationException($"Missing Neptune IAM configuration: {messageDetail}");
        }

        private sealed class DisabledAWSSigner : IAWSSigner
        {
            public static IAWSSigner Instance = new DisabledAWSSigner();

            private DisabledAWSSigner()
            {

            }

            public IAWSSigner ConfigureCacheTime(Func<TimeSpan, TimeSpan> transformation) => this;

            public IAWSSigner ConfigureRegion(Func<string, string> transformation) => this;

            public IAWSSigner ConfigureUri(Func<Uri, Uri> transformation) => this;

            public IReadOnlyDictionary<string, string> GetIAMHeaders(DateTimeOffset? time = null) => ImmutableDictionary<string, string>.Empty;

            public IAWSSigner WithAccessKeyId(string accessKeyId) => this;

            public IAWSSigner WithSecretAccessKey(string secretAccessKey) => this;
        }

        public static readonly IAWSSigner EmptyV4 = AWSV4SignerImpl.Empty;
        public static readonly IAWSSigner Disabled = DisabledAWSSigner.Instance;

        public static HttpRequestMessage Sign(this IAWSSigner signer, HttpRequestMessage request, DateTimeOffset? time = null)
        {
            if (request.Method != HttpMethod.Get)
                throw new NotSupportedException($"The {request.Method}-method is not supported.");

            signer.Sign(
                request.Headers,
                time);

            return request;
        }

        public static HttpHeaders Sign(this IAWSSigner signer, HttpHeaders headers, DateTimeOffset? time = null)
        {
            foreach (var kvp in signer.GetIAMHeaders(time))
            {
                headers.TryAddWithoutValidation(kvp.Key, kvp.Value);
            }

            return headers;
        }
    }
}
