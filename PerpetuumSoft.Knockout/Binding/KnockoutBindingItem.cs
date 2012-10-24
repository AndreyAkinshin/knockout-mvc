using System;
using System.Text;
using System.Linq.Expressions;
using DelegateDecompiler;

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
      string value = KnockoutExpressionConverter.Convert(DecompileExpressionVisitor.Decompile(Expression), data);
      if (string.IsNullOrWhiteSpace(value))
        value = "$data";

      var sb = new StringBuilder();

      sb.Append(Name);
      sb.Append(" : ");      
      sb.Append(value);

      return sb.ToString();
    }
  }  
}
