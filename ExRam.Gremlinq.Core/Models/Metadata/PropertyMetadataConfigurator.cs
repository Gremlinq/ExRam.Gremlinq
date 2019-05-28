using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    internal sealed class PropertyMetadataConfigurator<TElement> : IPropertyMetadataConfigurator<TElement>
    {
        private readonly IImmutableDictionary<MemberInfo, PropertyMetadata> _metadata;

        public PropertyMetadataConfigurator(IImmutableDictionary<MemberInfo, PropertyMetadata> metadata)
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
            var memberInfo = propertyExpression.GetMemberInfo();

            return new PropertyMetadataConfigurator<TElement>(_metadata.SetItem(
                memberInfo,
                _metadata
                    .TryGetValue(memberInfo)
                    .Map(metaData => new PropertyMetadata(name, metaData.SerializationBehaviour))
                    .IfNone(new PropertyMetadata(name, SerializationBehaviour.Default))));
        }

        public IPropertyMetadataConfigurator<TElement> SetSerializationBehaviour<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression, Func<SerializationBehaviour, SerializationBehaviour> transformation)
        {
            var memberInfo = propertyExpression.GetMemberInfo();

            return new PropertyMetadataConfigurator<TElement>(_metadata.SetItem(
                memberInfo,
                _metadata
                    .TryGetValue(memberInfo)
                    .Map(metaData => new PropertyMetadata(metaData.NameOverride, transformation(metaData.SerializationBehaviour)))
                    .IfNone(new PropertyMetadata(default, transformation(default)))));
        }

        #region Explicit
        IEnumerator<KeyValuePair<MemberInfo, PropertyMetadata>> IEnumerable<KeyValuePair<MemberInfo, PropertyMetadata>>.GetEnumerator()
        {
            return _metadata.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_metadata).GetEnumerator();
        }

        int IReadOnlyCollection<KeyValuePair<MemberInfo, PropertyMetadata>>.Count => _metadata.Count;

        bool IReadOnlyDictionary<MemberInfo, PropertyMetadata>.ContainsKey(MemberInfo key)
        {
            return _metadata.ContainsKey(key);
        }

        bool IReadOnlyDictionary<MemberInfo, PropertyMetadata>.TryGetValue(MemberInfo key, out PropertyMetadata value)
        {
            return _metadata.TryGetValue(key, out value);
        }

        PropertyMetadata IReadOnlyDictionary<MemberInfo, PropertyMetadata>.this[MemberInfo key] => _metadata[key];

        IEnumerable<MemberInfo> IReadOnlyDictionary<MemberInfo, PropertyMetadata>.Keys => _metadata.Keys;

        IEnumerable<PropertyMetadata> IReadOnlyDictionary<MemberInfo, PropertyMetadata>.Values => _metadata.Values;

        IImmutableDictionary<MemberInfo, PropertyMetadata> IImmutableDictionary<MemberInfo, PropertyMetadata>.Clear()
        {
            return _metadata.Clear();
        }

        IImmutableDictionary<MemberInfo, PropertyMetadata> IImmutableDictionary<MemberInfo, PropertyMetadata>.Add(MemberInfo key, PropertyMetadata value)
        {
            return _metadata.Add(key, value);
        }

        IImmutableDictionary<MemberInfo, PropertyMetadata> IImmutableDictionary<MemberInfo, PropertyMetadata>.AddRange(IEnumerable<KeyValuePair<MemberInfo, PropertyMetadata>> pairs)
        {
            return _metadata.AddRange(pairs);
        }

        IImmutableDictionary<MemberInfo, PropertyMetadata> IImmutableDictionary<MemberInfo, PropertyMetadata>.SetItem(MemberInfo key, PropertyMetadata value)
        {
            return _metadata.SetItem(key, value);
        }

        IImmutableDictionary<MemberInfo, PropertyMetadata> IImmutableDictionary<MemberInfo, PropertyMetadata>.SetItems(IEnumerable<KeyValuePair<MemberInfo, PropertyMetadata>> items)
        {
            return _metadata.SetItems(items);
        }

        IImmutableDictionary<MemberInfo, PropertyMetadata> IImmutableDictionary<MemberInfo, PropertyMetadata>.RemoveRange(IEnumerable<MemberInfo> keys)
        {
            return _metadata.RemoveRange(keys);
        }

        IImmutableDictionary<MemberInfo, PropertyMetadata> IImmutableDictionary<MemberInfo, PropertyMetadata>.Remove(MemberInfo key)
        {
            return _metadata.Remove(key);
        }

        bool IImmutableDictionary<MemberInfo, PropertyMetadata>.Contains(KeyValuePair<MemberInfo, PropertyMetadata> pair)
        {
            return _metadata.Contains(pair);
        }

        bool IImmutableDictionary<MemberInfo, PropertyMetadata>.TryGetKey(MemberInfo equalKey, out MemberInfo actualKey)
        {
            return _metadata.TryGetKey(equalKey, out actualKey);
        }
        #endregion
    }
}
