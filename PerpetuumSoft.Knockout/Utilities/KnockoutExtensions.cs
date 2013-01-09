using System.Web.Mvc;

namespace PerpetuumSoft.Knockout
{
  public static class KnockoutExtensions
  {
    public static KnockoutContext<TModel> CreateKnockoutContext<TModel>(this HtmlHelper<TModel> helper)
    {
      return new KnockoutContext<TModel>(helper.ViewContext);
    }

    public static KnockoutContext<TModel> CreateKnockoutContext<TModel>(this HtmlHelper<TModel> helper, string viewModelName)
    {
        var context = helper.CreateKnockoutContext();
        context.ViewModelName = viewModelName;

        return context;
    }
  }
}
