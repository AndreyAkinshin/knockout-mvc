using System;
using System.Linq.Expressions;

namespace PerpetuumSoft.Knockout
{
    public abstract class KnockoutBindingItem
    {
        public string Name { get; set; }

        public abstract string GetKnockoutExpression(KnockoutExpressionData data);

        public virtual bool IsValid()
        {
            return true;
        }
    }

    public class KnockoutBindingItem<TModel, TResult> : KnockoutBindingItem
    {
        public Expression<Func<TModel, TResult>> Expression { get; set; }

        public override string GetKnockoutExpression(KnockoutExpressionData data)
        {
            string value = KnockoutExpressionConverter.Convert(Expression, data);
            if (string.IsNullOrWhiteSpace(value))
                value = "$data";
            return string.Format("{0} : {1}", Name, value);
        }
    }
}
