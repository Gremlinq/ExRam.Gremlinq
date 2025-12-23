using System.Collections;
using System.Collections.Immutable;

using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    internal readonly struct Bindings
#if ExRam_Gremlinq_Providers_Core
        : ISpanFormattable
#endif
    {
        private sealed class Counter : ICollection<KeyValuePair<object, Label>>
        {
            private int _count;

            int ICollection<KeyValuePair<object, Label>>.Count => _count;

            bool ICollection<KeyValuePair<object, Label>>.IsReadOnly => false;

            void ICollection<KeyValuePair<object, Label>>.Add(KeyValuePair<object, Label> item) => _count++;

            void ICollection<KeyValuePair<object, Label>>.Clear() => _count = 0;

            bool ICollection<KeyValuePair<object, Label>>.Contains(KeyValuePair<object, Label> item) => false;

            void ICollection<KeyValuePair<object, Label>>.CopyTo(KeyValuePair<object, Label>[] array, int arrayIndex) => throw new NotSupportedException();

            IEnumerator<KeyValuePair<object, Label>> IEnumerable<KeyValuePair<object, Label>>.GetEnumerator() => throw new NotSupportedException();

            bool ICollection<KeyValuePair<object, Label>>.Remove(KeyValuePair<object, Label> item) => throw new NotSupportedException();

            IEnumerator IEnumerable.GetEnumerator() => throw new NotSupportedException();
        }

        private readonly ICollection<KeyValuePair<object, Label>>? _list;
        private readonly IEnumerable<KeyValuePair<string, object?>>? _existing;

        private Bindings(ICollection<KeyValuePair<object, Label>> list)
        {
            _list = list;
        }

        private Bindings(IEnumerable<KeyValuePair<string, object?>> existing)
        {
            _existing = existing;
        }

        public static Bindings CreateDictionary() => new(new Dictionary<object, Label>());

        public static Bindings CreateList() => new(new List<KeyValuePair<object, Label>>());

        public static Bindings CreateCounter() => new(new Counter());

        public static Bindings From(IEnumerable<KeyValuePair<string, object?>> existing) => new (existing);

        public Label GetOrAdd(object obj)
        {
            if (_list is IDictionary<object, Label> dictionary)
            {
                if (!dictionary.TryGetValue(obj, out var bindingKey))
                {
                    bindingKey = dictionary.Count;
                    dictionary.Add(obj, bindingKey);
                }

                return bindingKey;
            }

            if (_list is { } list)
            {
                var bindingKey = list.Count;
                list.Add(new KeyValuePair<object, Label>(obj, bindingKey));

                return bindingKey;
            }

            throw new InvalidOperationException();
        }

        public ImmutableDictionary<string, object?> ToImmutableDictionary() => _list is { } list
            ? list.ToImmutableDictionary(static kvp => (string)kvp.Value, static kvp => (object?)kvp.Key)
            : _existing is { } existing
                ? existing.ToImmutableDictionary()
                : throw new InvalidOperationException();

#if ExRam_Gremlinq_Providers_Core
        public override string ToString() => ToString(null, null);

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        {
            var first = true;

            charsWritten = 0;

            static bool TryWrite(ref Span<char> destination, ref bool first, ref int charsWritten, KeyValuePair<string, object?> kvp, IFormatProvider? provider)
            {
                var success = first
                     ? destination.TryWrite(provider, $"[{kvp.Key}, {kvp.Value}]", out var entryCharsWritten)
                     : destination.TryWrite(provider, $", [{kvp.Key}, {kvp.Value}]", out entryCharsWritten);

                first = false;

                if (success)
                {
                    charsWritten += entryCharsWritten;
                    destination = destination[entryCharsWritten..];
                }
                else
                {
                    charsWritten = 0;
                }

                return success;
            }

            if (_list is Dictionary<object, Label> dict)
            {
                foreach (var kvp in dict)
                {
                    if (!TryWrite(ref destination, ref first, ref charsWritten, new KeyValuePair<string, object?>(kvp.Value, kvp.Key), provider))
                        return false;
                }
            }
            else if (_list is List<KeyValuePair<object, Label>> list)
            {
                foreach (var kvp in list)
                {
                    if (!TryWrite(ref destination, ref first, ref charsWritten, new KeyValuePair<string, object?>(kvp.Value, kvp.Key), provider))
                        return false;
                }
            }
            else if (_existing is ImmutableDictionary<string, object?> existing)
            {
                foreach (var kvp in existing)
                {
                    if (!TryWrite(ref destination, ref first, ref charsWritten, kvp, provider))
                        return false;
                }
            }

            return true;
        }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            var handler = new System.Runtime.CompilerServices.DefaultInterpolatedStringHandler(0, 1);

            handler
                .AppendFormatted(this, format: format);

            return handler
                .ToString();
        }
#endif
    }
}
