using System.Runtime.CompilerServices;

using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Providers.Core
{
    internal readonly struct FormattableBindings : ISpanFormattable
    {
        private readonly Bindings? _bindings;

        public FormattableBindings(Bindings? bindings)
        {
            _bindings = bindings;
        }

        public override string? ToString() => ToString(null, null);

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        {
            charsWritten = 0;

            if (_bindings is { } bindings)
            {
                var first = true;

                foreach (var kvp in bindings)
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
