using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace PerpetuumSoft.Knockout
{
    public class KnockoutFormContext<TModel> : KnockoutRegionContext<TModel>
    {
        private readonly KnockoutContext<TModel> context;
        private readonly string[] instanceNames;
        private readonly Dictionary<string, string> aliases;

        private readonly string actionName;
        private readonly string controllerName;
        private readonly object routeValues;
        private readonly object htmlAttributes;

        public KnockoutFormContext(
          ViewContext viewContext,
          KnockoutContext<TModel> context, string[] instanceNames, Dictionary<string, string> aliases,
          string actionName, string controllerName, object routeValues, object htmlAttributes)
            : base(viewContext)
        {
            this.context = context;
            this.instanceNames = instanceNames;
            this.aliases = aliases;
            this.actionName = actionName;
            this.controllerName = controllerName;
            this.routeValues = routeValues;
            this.htmlAttributes = htmlAttributes;
            InStack = false;
        }

        public override void WriteStart(TextWriter writer)
        {
            var tagBuilder = new KnockoutTagBuilder<TModel>(context, "form", instanceNames, aliases);
            tagBuilder.ApplyAttributes(htmlAttributes);
            tagBuilder.Submit(actionName, controllerName, routeValues);
            tagBuilder.TagRenderMode = TagRenderMode.StartTag;
            writer.WriteLine(tagBuilder.ToHtmlString());
        }

        protected override void WriteEnd(TextWriter writer)
        {
            var tagBuilder = new TagBuilder("form");
            writer.WriteLine(new MvcHtmlString(tagBuilder.ToString(TagRenderMode.EndTag)));
        }
    }
}