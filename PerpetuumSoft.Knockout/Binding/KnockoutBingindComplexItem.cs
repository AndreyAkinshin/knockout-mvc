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
      var sb = new StringBuilder();

      sb.Append(Name);
      sb.Append(" : {");
      for (int i = 0; i < subItems.Count; i++)
      {
        if (i != 0)
          sb.Append(",");
        sb.Append(subItems[i].GetKnockoutExpression(data));
      }
      sb.Append('}');

      return sb.ToString();
    }
  }
}