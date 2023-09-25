using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core.Models
{
    public interface IMemberMetadataConfigurator<TElement>
    {
        IMemberMetadataConfigurator<TElement> IgnoreOnAdd<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression);

        IMemberMetadataConfigurator<TElement> IgnoreOnUpdate<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression);

        IMemberMetadataConfigurator<TElement> IgnoreAlways<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression);

        IMemberMetadataConfigurator<TElement> ResetSerializationBehaviour<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression);

        IMemberMetadataConfigurator<TElement> ConfigureName<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression, string name);

        IGraphElementPropertyModel Transform(IGraphElementPropertyModel model);
    }
}
