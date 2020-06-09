using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    internal sealed class PropertyMetadataConfigurator<TElement> : IPropertyMetadataConfigurator<TElement>
    {
        private readonly IImmutableDictionary<MemberInfo, MemberMetadata> _metadata;

        public PropertyMetadataConfigurator(IImmutableDictionary<MemberInfo, MemberMetadata> metadata)
        {
            _metadata = metadata;
        }

        public IPropertyMetadataConfigurator<TElement> IgnoreOnAdd<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression)
        {
            return SetSerializationBehaviour(
                propertyExpression,
                behaviour => behaviour | SerializationBehaviour.IgnoreOnAdd);
        }

        public IPropertyMetadataConfigurator<TElement> IgnoreOnUpdate<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression)
        {
            return SetSerializationBehaviour(
                propertyExpression,
                behaviour => behaviour | SerializationBehaviour.IgnoreOnUpdate);
        }

        public IPropertyMetadataConfigurator<TElement> IgnoreAlways<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression)
        {
            return SetSerializationBehaviour(
                propertyExpression,
                behaviour => behaviour | SerializationBehaviour.IgnoreAlways);
        }

        public IPropertyMetadataConfigurator<TElement> ConfigureName<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression, string name)
        {
            return Configure(
                propertyExpression,
                metaData => new MemberMetadata(name, metaData.SerializationBehaviour));
        }

        public IPropertyMetadataConfigurator<TElement> SetSerializationBehaviour<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression, Func<SerializationBehaviour, SerializationBehaviour> transformation)
        {
            return Configure(
                propertyExpression,
                metaData => new MemberMetadata(metaData.Name, transformation(metaData.SerializationBehaviour)));
        }

        private IPropertyMetadataConfigurator<TElement> Configure<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression, Func<MemberMetadata, MemberMetadata> transformation)
        {
            var memberInfo = propertyExpression.GetMemberInfo();
            
            return new PropertyMetadataConfigurator<TElement>(_metadata.SetItem(
                memberInfo,
                transformation(_metadata.TryGetValue(memberInfo, out var metadata)
                    ? metadata
                    : new MemberMetadata(memberInfo.Name))));
        }

        #region Explicit
        IEnumerator<KeyValuePair<MemberInfo, MemberMetadata>> IEnumerable<KeyValuePair<MemberInfo, MemberMetadata>>.GetEnumerator()
        {
            return _metadata.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_metadata).GetEnumerator();
        }

        int IReadOnlyCollection<KeyValuePair<MemberInfo, MemberMetadata>>.Count => _metadata.Count;

        bool IReadOnlyDictionary<MemberInfo, MemberMetadata>.ContainsKey(MemberInfo key)
        {
            return _metadata.ContainsKey(key);
        }

        bool IReadOnlyDictionary<MemberInfo, MemberMetadata>.TryGetValue(MemberInfo key, out MemberMetadata value)
        {
            return _metadata.TryGetValue(key, out value);
        }

        MemberMetadata IReadOnlyDictionary<MemberInfo, MemberMetadata>.this[MemberInfo key] => _metadata[key];

        IEnumerable<MemberInfo> IReadOnlyDictionary<MemberInfo, MemberMetadata>.Keys => _metadata.Keys;

        IEnumerable<MemberMetadata> IReadOnlyDictionary<MemberInfo, MemberMetadata>.Values => _metadata.Values;

        IImmutableDictionary<MemberInfo, MemberMetadata> IImmutableDictionary<MemberInfo, MemberMetadata>.Clear()
        {
            return _metadata.Clear();
        }

        IImmutableDictionary<MemberInfo, MemberMetadata> IImmutableDictionary<MemberInfo, MemberMetadata>.Add(MemberInfo key, MemberMetadata value)
        {
            return _metadata.Add(key, value);
        }

        IImmutableDictionary<MemberInfo, MemberMetadata> IImmutableDictionary<MemberInfo, MemberMetadata>.AddRange(IEnumerable<KeyValuePair<MemberInfo, MemberMetadata>> pairs)
        {
            return _metadata.AddRange(pairs);
        }

        IImmutableDictionary<MemberInfo, MemberMetadata> IImmutableDictionary<MemberInfo, MemberMetadata>.SetItem(MemberInfo key, MemberMetadata value)
        {
            return _metadata.SetItem(key, value);
        }

        IImmutableDictionary<MemberInfo, MemberMetadata> IImmutableDictionary<MemberInfo, MemberMetadata>.SetItems(IEnumerable<KeyValuePair<MemberInfo, MemberMetadata>> items)
        {
            return _metadata.SetItems(items);
        }

        IImmutableDictionary<MemberInfo, MemberMetadata> IImmutableDictionary<MemberInfo, MemberMetadata>.RemoveRange(IEnumerable<MemberInfo> keys)
        {
            return _metadata.RemoveRange(keys);
        }

        IImmutableDictionary<MemberInfo, MemberMetadata> IImmutableDictionary<MemberInfo, MemberMetadata>.Remove(MemberInfo key)
        {
            return _metadata.Remove(key);
        }

        bool IImmutableDictionary<MemberInfo, MemberMetadata>.Contains(KeyValuePair<MemberInfo, MemberMetadata> pair)
        {
            return _metadata.Contains(pair);
        }

        bool IImmutableDictionary<MemberInfo, MemberMetadata>.TryGetKey(MemberInfo equalKey, out MemberInfo actualKey)
        {
            return _metadata.TryGetKey(equalKey, out actualKey);
        }
        #endregion
    }
}
