using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Web;

namespace PerpetuumSoft.Knockout
{
  public class KnockoutBinding<TModel> : KnockoutSubContext<TModel>, IHtmlString
  {
    public KnockoutBinding(KnockoutContext<TModel> context, string[] instanceNames = null, Dictionary<string, string> aliases = null) : base(context, instanceNames, aliases)
    {
    }

    // *** Controlling text and appearance ***

    // Visible
    public KnockoutBinding<TModel> Visible(Expression<Func<TModel, object>> binding)
    {
      Items.Add(new KnockoutBindingItem<TModel, object> { Name = "visible", Expression = binding });
      return this;
    }

    // Text
    public KnockoutBinding<TModel> Text(Expression<Func<TModel, object>> binding)
    {
      Items.Add(new KnockoutBindingItem<TModel, object> { Name = "text", Expression = binding });
      return this;
    }

    public KnockoutBinding<TModel> Text(string binding)
    {
      Items.Add(new KnockoutBindingStringItem("text", binding, false));
      return this;
    }

    // Html
    public KnockoutBinding<TModel> Html(Expression<Func<TModel, string>> binding)
    {
      Items.Add(new KnockoutBindingItem<TModel, string> { Name = "html", Expression = binding });
      return this;
    }

    public KnockoutBinding<TModel> Html(Expression<Func<TModel, Expression<Func<string>>>> binding)
    {
      Items.Add(new KnockoutBindingItem<TModel, Expression<Func<string>>> { Name = "html", Expression = binding });
      return this;
    }

    // *** Working with form fields ***
    public KnockoutBinding<TModel> Value(Expression<Func<TModel, object>> binding)
    {
      Items.Add(new KnockoutBindingItem<TModel, object> { Name = "value", Expression = binding });
      return this;
    }

    public KnockoutBinding<TModel> Disable(Expression<Func<TModel, bool>> binding)
    {
      Items.Add(new KnockoutBindingItem<TModel, bool> { Name = "disable", Expression = binding });
      return this;
    }

    public KnockoutBinding<TModel> Enable(Expression<Func<TModel, bool>> binding)
    {
      Items.Add(new KnockoutBindingItem<TModel, bool> { Name = "enable", Expression = binding });
      return this;
    }

    public KnockoutBinding<TModel> Checked(Expression<Func<TModel, object>> binding)
    {
      Items.Add(new KnockoutBindingItem<TModel, object> { Name = "checked", Expression = binding });
      return this;
    }

    public KnockoutBinding<TModel> Options(Expression<Func<TModel, IEnumerable>> binding)
    {
      Items.Add(new KnockoutBindingItem<TModel, IEnumerable> { Name = "options", Expression = binding });
      return this;
    }

    public KnockoutBinding<TModel> SelectedOptions(Expression<Func<TModel, IEnumerable>> binding)
    {
      Items.Add(new KnockoutBindingItem<TModel, IEnumerable> { Name = "selectedOptions", Expression = binding });
      return this;
    }

    public KnockoutBinding<TModel> OptionsCaption(Expression<Func<TModel, object>> binding)
    {
      Items.Add(new KnockoutBindingItem<TModel, object> { Name = "optionsCaption", Expression = binding });
      return this;
    }

    public KnockoutBinding<TModel> OptionsCaption(string text)
    {
      Items.Add(new KnockoutBindingStringItem("optionsCaption", text));
      return this;
    }

    public KnockoutBinding<TModel> OptionsText(string text)
    {
      Items.Add(new KnockoutBindingStringItem("optionsText", text, false));
      return this;
    }

    public KnockoutBinding<TModel> UniqueName()
    {
      Items.Add(new KnockoutBindingStringItem("uniqueName", "true", false));
      return this;
    }

    public KnockoutBinding<TModel> ValueUpdate(KnockoutValueUpdateKind kind)
    {
      Items.Add(new KnockoutBindingStringItem("valueUpdate", Enum.GetName(typeof(KnockoutValueUpdateKind), kind).ToLower()));
      return this;
    }

    public KnockoutBinding<TModel> HasFocus(Expression<Func<TModel, object>> binding)
    {
      Items.Add(new KnockoutBindingItem<TModel, object> {Name = "hasfocus", Expression = binding});
      return this;
    }

    // *** Complex ***
    public KnockoutBinding<TModel> Css(string name, Expression<Func<TModel, object>> binding)
    {
      ComplexItem("css").Add(new KnockoutBindingItem<TModel, object> { Name = name, Expression = binding });
      return this;
    }

    public KnockoutBinding<TModel> Style(string name, Expression<Func<TModel, object>> binding)
    {
      ComplexItem("style").Add(new KnockoutBindingItem<TModel, object> { Name = name, Expression = binding });
      return this;
    }

    public KnockoutBinding<TModel> Attr(string name, Expression<Func<TModel, object>> binding)
    {
      ComplexItem("attr").Add(new KnockoutBindingItem<TModel, object> { Name = name, Expression = binding });
      return this;
    }

    // *** Events ***
    protected virtual KnockoutBinding<TModel> Event(string eventName, string actionName, string controllerName, object routeValues)
    {
      var sb = new StringBuilder();
      sb.Append("function() {");
      sb.Append(Context.ServerAction(actionName, controllerName, routeValues));
      sb.Append(";}");
      Items.Add(new KnockoutBindingStringItem(eventName, sb.ToString(), false));
      return this;
    }

    public KnockoutBinding<TModel> Click(string actionName, string controllerName, object routeValues = null)
    {
      return Event("click", actionName, controllerName, routeValues);
    }

    public KnockoutBinding<TModel> Submit(string actionName, string controllerName, object routeValues = null)
    {
      return Event("submit", actionName, controllerName, routeValues);
    }

    // *** Custom ***    
    public KnockoutBinding<TModel> Custom(string name, string value)
    {
      Items.Add(new KnockoutBindingStringItem(name, value));
      return this;
    }

    // *** Common ***

    private readonly List<KnockoutBindingItem> items = new List<KnockoutBindingItem>();

    private readonly Dictionary<string, KnockoutBingindComplexItem> complexItems = new Dictionary<string, KnockoutBingindComplexItem>();

    public List<KnockoutBindingItem> Items
    {
      get
      {
        return items;
      }
    }

    private KnockoutBingindComplexItem ComplexItem(string name)
    {
      if (!complexItems.ContainsKey(name))
      {
        complexItems[name] = new KnockoutBingindComplexItem { Name = name };
        items.Add(complexItems[name]);
      }
      return complexItems[name];
    }

    public virtual string ToHtmlString()
    {
      var sb = new StringBuilder();
      sb.Append(@"data-bind=""");
      sb.Append(BindingAttributeContent());
      sb.Append(@"""");
      return sb.ToString();
    }

    public string BindingAttributeContent()
    {
      var sb = new StringBuilder();
      bool first = true;
      foreach (var item in Items)
      {
        if (!item.IsValid())
          continue;
        if (first)
          first = false;
        else
          sb.Append(',');        
        sb.Append(item.GetKnockoutExpression(CreateData()));
      }
      return sb.ToString();
    }
  }
}
