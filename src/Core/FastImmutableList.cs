using System.Buffers;

namespace ExRam.Gremlinq.Core
{
    internal readonly struct FastImmutableList<T>
        where T : class
    {
        public static readonly FastImmutableList<T> Empty = new([]);

        private readonly Memory<T?>? _items;

        internal FastImmutableList(T[] items) : this(items, items.Length)
        {

        }

        internal FastImmutableList(Memory<T?> steps, int count)
        {
            Count = count;
            _items = steps;
        }

        public FastImmutableList<T> Push(params ReadOnlySpan<T> newItems)
        {
            if (newItems.Length > 0)
            {
                var newListLength = Count + newItems.Length;
                var newListMemory = EnsureCapacity(Math.Max(newListLength, 16)).Items;
                var targetSpan = newListMemory.Span[Count..];

                if (newListMemory.Equals(Items))
                {
                    //This instance is big enough, we need to guard the first element by Interlocked.
                    if (Interlocked.CompareExchange(ref targetSpan[0], newItems[0], null) != null)
                        return Clone().Push(newItems);

                    newItems = newItems[1..];
                    targetSpan = targetSpan[1..];
                }

                ((ReadOnlySpan<T?>)newItems).CopyTo(targetSpan);
                return new FastImmutableList<T>(newListMemory, newListLength);
            }

            return this;
        }

        public FastImmutableList<T> Push(T item)
        {
            var steps = Items;

            return Count < steps.Length
                ? Interlocked.CompareExchange(ref steps.Span[Count], item, null) != null
                    ? Clone().Push(item)
                    : new FastImmutableList<T>(steps, Count + 1)
                : EnsureCapacity(Math.Max(steps.Length * 2, 16)).Push(item);
        }

        public FastImmutableList<T> Pop(out T poppedItem)
        {
            if (Count == 0)
                throw new InvalidOperationException($"{nameof(Traversal)} is Empty.");

            poppedItem = this[Count - 1];
            return new FastImmutableList<T>(Items, Count - 1);
        }

        public FastImmutableList<T> Slice(int start, int length) => length <= Count - start
            ? new(Items[start..], length)
            : throw new ArgumentOutOfRangeException(nameof(length));

        public int Count { get; }

        public T this[int index] => index < 0 || index >= Count
            ? throw new ArgumentOutOfRangeException(nameof(index))
            : Items.Span[index]!;

        public static FastImmutableList<T> Create<TState>(int length, TState state, SpanAction<T, TState> action)
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            var steps = new T[length];
            action(steps.AsSpan(), state);

            return new(steps);
        }

#pragma warning disable CS8619
        public ReadOnlySpan<T> AsSpan() => Items.Span[..Count];
#pragma warning restore CS8619

        public FastImmutableList<T> EnsureCapacity(int count)
        {
            if (Items.Length < count)
            {
                var newItems = new T[count];

                this
                    .AsSpan()
                    .CopyTo(newItems);

                return new(newItems, Count);
            }

            return this;
        }

        private FastImmutableList<T> Clone()
        {
            var newItems = new T[Items.Length];

            this
                .AsSpan()
                .CopyTo(newItems);

            return new(newItems, Count);
        }

        private Memory<T?> Items => _items ?? Array.Empty<T?>();
    }
}
