﻿using System;

namespace ExRam.Gremlinq.Core.GraphElements
{
    public abstract class Property : IProperty
    {
        public override string ToString()
        {
            return $"p[{Key}->{GetValue()}]";
        }

        protected abstract object GetValue();

        public string? Key { get; set; }

        object? IProperty.Value { get => GetValue(); }
    }

    public class Property<TValue> : Property
    {
        public Property(TValue value)
        {
            //TODO: Null?!?!
            Value = value;
        }

        public static implicit operator Property<TValue>(TValue value) => new Property<TValue>(value);
        public static implicit operator Property<TValue>(TValue[] value) => throw new NotSupportedException("This conversion is only intended to be used in expressions. It can't be executed reasonably.");
        public static implicit operator Property<TValue>(Property<TValue>[] value) => throw new NotSupportedException("This conversion is only intended to be used in expressions. It can't be executed reasonably.");

        protected override object GetValue() => Value!;

        public TValue Value { get; }
    }
}
