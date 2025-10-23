#pragma warning disable IDE0003 // Remove qualification

using System.Collections;
using System.Collections.Immutable;

using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    internal readonly struct Bindings
    {
        private sealed class Counter : ICollection<KeyValuePair<object, Label>>, IEnumerator<KeyValuePair<object, Label>>
        {
            private int _count;

            public KeyValuePair<object, Label> this[int index] { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

            int ICollection<KeyValuePair<object, Label>>.Count => _count;

            bool ICollection<KeyValuePair<object, Label>>.IsReadOnly => false;

            KeyValuePair<object, Label> IEnumerator<KeyValuePair<object, Label>>.Current => throw new InvalidOperationException();

            object IEnumerator.Current => throw new InvalidOperationException();

            void ICollection<KeyValuePair<object, Label>>.Add(KeyValuePair<object, Label> item) => _count++;

            void ICollection<KeyValuePair<object, Label>>.Clear() => _count = 0;

            bool ICollection<KeyValuePair<object, Label>>.Contains(KeyValuePair<object, Label> item) => false;

            void ICollection<KeyValuePair<object, Label>>.CopyTo(KeyValuePair<object, Label>[] array, int arrayIndex) => throw new NotSupportedException();

            void IDisposable.Dispose()
            {
            }

            IEnumerator<KeyValuePair<object, Label>> IEnumerable<KeyValuePair<object, Label>>.GetEnumerator() => this;

            bool IEnumerator.MoveNext() => false;

            bool ICollection<KeyValuePair<object, Label>>.Remove(KeyValuePair<object, Label> item) => throw new NotSupportedException();

            void IEnumerator.Reset()
            {
            }

            IEnumerator IEnumerable.GetEnumerator() => this;
        }

        private sealed class BindingsEnumerator : IEnumerator<KeyValuePair<string, object?>>
        {
            private readonly IEnumerator<KeyValuePair<object, Label>> _baseEnumerator;

            public BindingsEnumerator(IEnumerator<KeyValuePair<object, Label>> baseEnumerator)
            {
                _baseEnumerator = baseEnumerator;
            }

            public KeyValuePair<string, object?> Current => new(_baseEnumerator.Current.Value, _baseEnumerator.Current.Key);

            object IEnumerator.Current => Current;

            public void Dispose() => _baseEnumerator.Dispose();

            public bool MoveNext() => _baseEnumerator.MoveNext();

            public void Reset() => _baseEnumerator.Reset();
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

        public IEnumerator<KeyValuePair<string, object?>> GetEnumerator() => _list is { } list
            ? new BindingsEnumerator(list.GetEnumerator())
            : _existing?.GetEnumerator() ?? throw new InvalidOperationException();
    }
}
