using ExRam.Gremlinq.Core;
using LanguageExt;

namespace System.Collections.Generic
{
    internal static class EnumeratorExtensions
    {
        private sealed class PebbleEnumerator<TSource> : IPebbleEnumerator<TSource>
        {
            private int _currentIndex = -1;
            private bool _hasBaseEnumeratorElement = true;

            private readonly IEnumerator<TSource> _baseEnumerator;
            private readonly Stack<int> _pebbles = new Stack<int>();
            private readonly List<TSource> _list = new List<TSource>();

            public PebbleEnumerator(IEnumerator<TSource> baseEnumerator)
            {
                _baseEnumerator = baseEnumerator;
            }

            public void DropPebble()
            {
                var pebble = _currentIndex == -1
                    ? _list.Count
                    : _currentIndex;

                _pebbles.Push(pebble);
            }

            public void LiftPebble()
            {
                _pebbles.Pop();

                if (_pebbles.Count == 0 && _currentIndex == -1)
                    _list.Clear();
            }

            public void Return()
            {
                if (_list.Count > 0)
                    _currentIndex = _pebbles.Peek();
            }

            public bool MoveNext()
            {
                if (_currentIndex >= 0)
                {
                    if (++_currentIndex < _list.Count)
                        return true;

                    _currentIndex = -1;

                    if (_pebbles.Count == 0)
                        _list.Clear();

                    return _hasBaseEnumeratorElement;
                }

                if (_hasBaseEnumeratorElement)
                {
                    if (_pebbles.Count > 0)
                        _list.Add(_baseEnumerator.Current);
                    
                    return _hasBaseEnumeratorElement = _baseEnumerator.MoveNext();
                }

                return false;
            }

            void IEnumerator.Reset()
            {
                throw new InvalidOperationException();
            }

            public TSource Current
            {
                get
                {
                    if (_currentIndex >= 0 && _currentIndex < _list.Count)
                        return _list[_currentIndex];

                    return _baseEnumerator.Current;
                }
            }

            object IEnumerator.Current => ((IEnumerator)_baseEnumerator).Current;

            public bool HasPebble => _pebbles.Count > 0;

            public void Dispose()
            {
                _baseEnumerator.Dispose();
            }

            public IEnumerator<TSource> Replay()
            {
                if (_list.Count > 0)
                {
                    for (var i = _pebbles.Peek(); i < _list.Count; i++)
                    {
                        yield return _list[i];
                    }
                }
            }
        }

        public static IPebbleEnumerator<TSource> WithPebbles<TSource>(this IEnumerator<TSource> source)
        {
            return new PebbleEnumerator<TSource>(source);
        }

        public static TSource Last<TSource>(this IEnumerator<TSource> source)
        {
            var last = default(Option<TSource>);

            while (source.MoveNext())
                last = source.Current;

            return last.IfNone(() => throw new InvalidOperationException());
        }

        public static void Iterate<TSource>(this IEnumerator<TSource> source)
        {
            while (source.MoveNext());
        }

    }
}
