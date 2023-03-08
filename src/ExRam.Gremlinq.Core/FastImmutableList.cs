using System.Buffers;
using System.Collections;

namespace ExRam.Gremlinq.Core
{
    internal readonly struct FastImmutableList<T> : IReadOnlyList<T>
        where T : class
    {
        public static readonly FastImmutableList<T> Empty = new(Array.Empty<T>());

        private readonly T?[]? _items;

        internal FastImmutableList(T[] items) : this(items, items.Length)
        {

        }

        internal FastImmutableList(T?[] steps, int count)
        {
            Count = count;
            _items = steps;
        }

        public FastImmutableList<T> Push(params T[] items)
        {
            var ret = EnsureCapacity(Count + items.Length);

            for(var i = 0; i < items.Length; i++)
            {
                ret = ret.Push(items[i]);
            }

            return ret;
        }

        public FastImmutableList<T> Push(T item)
        {
            var steps = Items;

            if (Count < steps.Length)
            {
                if (Interlocked.CompareExchange(ref steps[Count], item, default) != null)
                    return Clone().Push(item);

                return new FastImmutableList<T>(
                    steps,
                    Count + 1);
            }
            else
                return EnsureCapacity(Math.Max(steps.Length * 2, 16)).Push(item);
        }

        public FastImmutableList<T> Pop() => Pop(out _);

        public FastImmutableList<T> Pop(out T poppedItem)
        {
            if (Count == 0)
                throw new InvalidOperationException($"{nameof(Traversal)} is Empty.");

            poppedItem = this[Count - 1];
            return new FastImmutableList<T>(Items, Count - 1);
        }

        public IEnumerator<T> GetEnumerator()
        {
            var steps = Items;

            for (var i = 0; i < Count; i++)
            {
                yield return steps[i]!;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count { get; }

        public T this[int index]
        {
            get => index < 0 || index >= Count
                ? throw new ArgumentOutOfRangeException(nameof(index))
                : Items[index]!;
        }

        public static implicit operator FastImmutableList<T>(T item) => new(new[] { item });

        public static FastImmutableList<T> Create<TState>(int length, TState state, SpanAction<T, TState> action)
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            var steps = new T[length];
            action(steps.AsSpan(), state);

            return new(steps);
        }

        public ReadOnlySpan<T> AsSpan() => Items.AsSpan()[..Count];

        public ReadOnlySpan<T> AsSpan(Range range) => AsSpan()[range];

        public ReadOnlySpan<T> AsSpan(int start, int length) => AsSpan().Slice(start, length);

        public ReadOnlySpan<T> AsSpan(int start) => AsSpan()[start..];


        public ReadOnlyMemory<T> AsMemory() => Items.AsMemory()[..Count];

        public ReadOnlyMemory<T> AsMemory(Range range) => AsMemory()[range];

        public ReadOnlyMemory<T> AsMemory(int start, int length) => AsMemory().Slice(start, length);

        public ReadOnlyMemory<T> AsMemory(int start) => AsMemory()[start..];

        private FastImmutableList<T> EnsureCapacity(int count)
        {
            if (Items.Length < count)
            {
                var newItems = new T[count];
                Array.Copy(Items, newItems, Count);

                return new(newItems, Count);
            }

            return this;
        }

        private FastImmutableList<T> Clone()
        {
            var newItems = new T[Items.Length];
            Array.Copy(Items, newItems, Count);

            return new(newItems, Count);
        }

        private T?[] Items => _items ?? Array.Empty<T?>();

        internal bool IsEmpty { get => Count == 0; }
    }
}
