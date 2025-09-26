using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Providers.Core
{
    internal sealed class FormattableBindings : ISpanFormattable
    {
        private readonly IEnumerable<KeyValuePair<string, object?>>? _value;

        public FormattableBindings(IEnumerable<KeyValuePair<string, object?>>? bindings)
        {
            _value = bindings;
        }

        public override string? ToString() => ToString(null, null);

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        {
            charsWritten = 0;

            if (_value is { } value)
            {
                var first = true;

                foreach (var kvp in value)
                {
                    var entryCharsWritten = 0;

                    var success = first
                        ? destination.TryWrite(provider, $"[{kvp.Key}, {kvp.Value}]", out entryCharsWritten)
                        : destination.TryWrite(provider, $", [{kvp.Key}, {kvp.Value}]", out entryCharsWritten);

                    first = false;

                    if (!success)
                    {
                        charsWritten = 0;

                        return false;
                    }

                    charsWritten += entryCharsWritten;
                    destination = destination[entryCharsWritten..];
                }
            }

            return true;
        }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            var handler = new DefaultInterpolatedStringHandler(0, 1);

            handler
                .AppendFormatted(this, format: format);

            return handler
                .ToString();
        }
    }
}
