using System.Collections.Generic;
using System.Text;

namespace PerpetuumSoft.Knockout
{
    public class KnockoutBingindComplexItem : KnockoutBindingItem
    {
        private readonly List<KnockoutBindingItem> subItems = new List<KnockoutBindingItem>();

        public void Add(KnockoutBindingItem subItem)
        {
            subItems.Add(subItem);
        }

        public override bool IsValid()
        {
            return subItems.Count > 0;
        }

        public override string GetKnockoutExpression(KnockoutExpressionData data)
        {
            var builder = new StringBuilder();

            builder.Append(Name);
            builder.Append(" : {");
            for (int i = 0; i < subItems.Count; i++)
            {
                if (i != 0)
                    builder.Append(",");
                builder.Append(subItems[i].GetKnockoutExpression(data));
            }
            builder.Append('}');

            return builder.ToString();
        }
    }
}