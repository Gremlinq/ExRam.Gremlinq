using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public sealed class HasStep : NonTerminalGremlinStep
    {
        private static string Name = "has";

        private readonly string _key;
        private readonly Option<object> _value;

        public HasStep(string key, Option<object> value = default(Option<object>))
        {
            this._key = key;
            this._value = value;
        }

        public override IEnumerable<TerminalGremlinStep> Resolve(IGraphModel model)
        {
            var key = this._key == model.IdPropertyName
                ? new SpecialGremlinString("T.id")
                : (object)this._key;

            yield return this._value.Match(
                value => new TerminalGremlinStep(Name, key, value),
                () => new TerminalGremlinStep(Name, key));
        }

        internal static HasStep FromExpression(Expression expression, Option<object> value = default(Option<object>))
        {
            string name;

            if (expression is MemberExpression leftMemberExpression)
                name = leftMemberExpression.Member.Name;
            else if (expression is ParameterExpression leftParameterExpression)
                name = leftParameterExpression.Name;
            else
                throw new NotSupportedException();

            return new HasStep(name, value);
        }
    }
}