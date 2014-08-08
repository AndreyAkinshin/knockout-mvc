using System.Web.Mvc;

namespace PerpetuumSoft.Knockout
{
    public class KnockoutForeachContext<TModel> : KnockoutCommonRegionContext<TModel>
    {
        public KnockoutForeachContext(ViewContext viewContext, string expression)
            : base(viewContext, expression)
        {
        }

        protected override string Keyword
        {
            get
            {
                return "foreach";
            }
        }
    }
}
