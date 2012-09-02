using System.Text;

namespace PerpetuumSoft.Knockout
{
  public class KnockoutBindingStringItem : KnockoutBindingItem
  {
    public KnockoutBindingStringItem()
    {
      NeedQuotes = true;
    }

    public KnockoutBindingStringItem(string name, string value)
      : this()
    {
      Name = name;
      Value = value;
    }

    public KnockoutBindingStringItem(string name, string value, bool needQuotes = true)
      : this(name, value)
    {
      NeedQuotes = needQuotes;
    }

    public string Value { get; set; }
    public bool NeedQuotes { get; set; }

    public override string GetKnockoutExpression(KnockoutExpressionData data)
    {
      var sb = new StringBuilder();

      sb.Append(Name);
      sb.Append(" : ");
      if (NeedQuotes)
        sb.Append('\'');
      sb.Append(Value);
      if (NeedQuotes)
        sb.Append('\'');

      return sb.ToString();
    }
  }
}
