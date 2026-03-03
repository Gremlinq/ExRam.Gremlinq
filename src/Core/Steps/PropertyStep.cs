using System.Collections.Immutable;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Base class for Gremlin <c>property()</c> steps that set properties on elements.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#addproperty-step">Reference Documentation - AddProperty Step</seealso>
    public abstract class PropertyStep : Step
    {
        /// <summary>Represents a <c>property()</c> step that sets a property identified by a key.</summary>
        public sealed class ByKeyStep : PropertyStep
        {
            /// <summary>Initializes a new instance of <see cref="ByKeyStep"/>.</summary>
            /// <param name="key">The property key.</param>
            /// <param name="value">The property value.</param>
            /// <param name="cardinality">The optional cardinality.</param>
            public ByKeyStep(Key key, object value, Cardinality? cardinality = null) : this(key, value, ImmutableArray<KeyValuePair<string, object>>.Empty, cardinality)
            {
                ArgumentNullException.ThrowIfNull(value);

            }

            /// <summary>Initializes a new instance of <see cref="ByKeyStep"/> with meta-properties.</summary>
            /// <param name="key">The property key.</param>
            /// <param name="value">The property value.</param>
            /// <param name="metaProperties">The meta-properties to set on the property.</param>
            /// <param name="cardinality">The optional cardinality.</param>
            public ByKeyStep(Key key, object value, ImmutableArray<KeyValuePair<string, object>> metaProperties, Cardinality? cardinality = null) : base(value, metaProperties, cardinality)
            {
                ArgumentNullException.ThrowIfNull(value);

                Key = key;
            }

            /// <summary>Gets the property key.</summary>
            public Key Key { get; }
        }

        /// <summary>Represents a <c>property()</c> step that sets a property identified by a traversal.</summary>
        public sealed class ByTraversalStep : PropertyStep
        {
            /// <summary>Initializes a new instance of <see cref="ByTraversalStep"/>.</summary>
            /// <param name="traversal">The traversal identifying the property.</param>
            /// <param name="value">The property value.</param>
            /// <param name="cardinality">The optional cardinality.</param>
            public ByTraversalStep(Traversal traversal, object value, Cardinality? cardinality = null) : this(traversal, value, ImmutableArray<KeyValuePair<string, object>>.Empty, cardinality)
            {
                ArgumentNullException.ThrowIfNull(value);

            }

            /// <summary>Initializes a new instance of <see cref="ByTraversalStep"/> with meta-properties.</summary>
            /// <param name="traversal">The traversal identifying the property.</param>
            /// <param name="value">The property value.</param>
            /// <param name="metaProperties">The meta-properties to set on the property.</param>
            /// <param name="cardinality">The optional cardinality.</param>
            public ByTraversalStep(Traversal traversal, object value, ImmutableArray<KeyValuePair<string, object>> metaProperties, Cardinality? cardinality = null) : base(value, metaProperties, cardinality)
            {
                ArgumentNullException.ThrowIfNull(value);

                Traversal = traversal;
            }

            /// <summary>Gets the traversal identifying the property.</summary>
            public Traversal Traversal { get; }
        }

        /// <summary>Initializes a new instance of <see cref="PropertyStep"/>.</summary>
        /// <param name="value">The property value.</param>
        /// <param name="cardinality">The optional cardinality.</param>
        protected PropertyStep(object value, Cardinality? cardinality = null) : this(value, ImmutableArray<KeyValuePair<string, object>>.Empty, cardinality)
        {

        }

        /// <summary>Initializes a new instance of <see cref="PropertyStep"/> with meta-properties.</summary>
        /// <param name="value">The property value.</param>
        /// <param name="metaProperties">The meta-properties to set on the property.</param>
        /// <param name="cardinality">The optional cardinality.</param>
        protected PropertyStep(object value, ImmutableArray<KeyValuePair<string, object>> metaProperties, Cardinality? cardinality = null) : base(SideEffectSemanticsChange.Write)
        {
            Value = value;
            Cardinality = cardinality;
            MetaProperties = metaProperties;
        }

        /// <summary>Gets the property value.</summary>
        public object Value { get; }
        /// <summary>Gets the optional cardinality.</summary>
        public Cardinality? Cardinality { get; }
        /// <summary>Gets the meta-properties.</summary>
        public ImmutableArray<KeyValuePair<string, object>> MetaProperties { get; }
    }
}
