using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Tests.Infrastructure.GraphSon.Entities
{
    // A dictionary of ints and a dictionary of strings at once, which does not say what its
    // values are.
    public class ClassWithTwoDictionaryInterfaces : Dictionary<string, int>, IDictionary<string, string>
    {
        private readonly Dictionary<string, string> _strings = new();

        string IDictionary<string, string>.this[string key]
        {
            get => _strings[key];
            set => _strings[key] = value;
        }

        ICollection<string> IDictionary<string, string>.Keys => _strings.Keys;

        ICollection<string> IDictionary<string, string>.Values => _strings.Values;

        int ICollection<KeyValuePair<string, string>>.Count => _strings.Count;

        bool ICollection<KeyValuePair<string, string>>.IsReadOnly => false;

        void IDictionary<string, string>.Add(string key, string value) => _strings.Add(key, value);

        void ICollection<KeyValuePair<string, string>>.Add(KeyValuePair<string, string> item) => _strings.Add(item.Key, item.Value);

        void ICollection<KeyValuePair<string, string>>.Clear() => _strings.Clear();

        bool ICollection<KeyValuePair<string, string>>.Contains(KeyValuePair<string, string> item) => ((ICollection<KeyValuePair<string, string>>)_strings).Contains(item);

        bool IDictionary<string, string>.ContainsKey(string key) => _strings.ContainsKey(key);

        void ICollection<KeyValuePair<string, string>>.CopyTo(KeyValuePair<string, string>[] array, int arrayIndex) => ((ICollection<KeyValuePair<string, string>>)_strings).CopyTo(array, arrayIndex);

        IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator() => _strings.GetEnumerator();

        bool IDictionary<string, string>.Remove(string key) => _strings.Remove(key);

        bool ICollection<KeyValuePair<string, string>>.Remove(KeyValuePair<string, string> item) => ((ICollection<KeyValuePair<string, string>>)_strings).Remove(item);

        bool IDictionary<string, string>.TryGetValue(string key, [MaybeNullWhen(false)] out string value) => _strings.TryGetValue(key, out value);
    }
}
