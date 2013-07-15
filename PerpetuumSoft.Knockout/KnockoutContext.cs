using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Web.Routing;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace PerpetuumSoft.Knockout
{
    public interface IKnockoutContext
    {
        string GetInstanceName();
        string GetIndex();
    }

    public class KnockoutContext<TModel> : IKnockoutContext
    {
        public string ViewModelName = "viewModel";

        private TModel model;


        public TModel Model
        {
            get
            {
                return model;
            }
        }

        protected List<IKnockoutContext> ContextStack { get; set; }

        public KnockoutContext(ViewContext viewContext)
        {
            this.viewContext = viewContext;
            ContextStack = new List<IKnockoutContext>();
        }

        private readonly ViewContext viewContext;

        private bool isInitialized;

        private string GetInitializeData(TModel model, bool needBinding, string wrapperId, bool applyOnDocumentReady)
        {
            if (isInitialized)
                return "";
            isInitialized = true;
            KnockoutUtilities.ConvertData(model);
            this.model = model;

            var sb = new StringBuilder();

            var json = JsonConvert.SerializeObject(model);

            sb.AppendLine(@"<script type=""text/javascript""> ");
            if (applyOnDocumentReady)
            {
                sb.Append("$(document).ready(function() {");
            }
            sb.AppendLine(string.Format("var {0}Js = {1};", ViewModelName, json));
            var mappingData = KnockoutJsModelBuilder.CreateMappingData<TModel>();
            if (mappingData == "{}")
            {
                sb.AppendLine(string.Format("var {0} = ko.mapping.fromJS({0}Js); ", ViewModelName));
            }
            else
            {
                sb.AppendLine(string.Format("var {0}MappingData = {1};", ViewModelName, mappingData));
                sb.AppendLine(string.Format("var {0} = ko.mapping.fromJS({0}Js, {0}MappingData); ", ViewModelName));
            }
            sb.Append(KnockoutJsModelBuilder.AddComputedToModel(model, ViewModelName));
            if (needBinding)
                if (!string.IsNullOrEmpty(wrapperId))
                {
                    sb.AppendLine(string.Format("ko.applyBindings({0}, document.getElementById('{1}'))", ViewModelName, wrapperId));
                }
                else
                {
                    sb.AppendLine(string.Format("ko.applyBindings({0});", ViewModelName));
                }
            if (applyOnDocumentReady)
            {
                sb.Append("});");
            }
            sb.AppendLine(@"</script>");
            return sb.ToString();
        }

        public HtmlString Initialize(TModel model)
        {
            return new HtmlString(GetInitializeData(model, false, string.Empty, false));
        }

        public HtmlString Apply(TModel model, string wrapperId = "", bool applyOnDocumentReady = false)
        {
            if (isInitialized)
            {
                var sb = new StringBuilder();
                sb.AppendLine(@"<script type=""text/javascript"">");
                if (applyOnDocumentReady)
                {
                    sb.AppendLine("$(document).ready(function() {");
                }
                if (!string.IsNullOrEmpty(wrapperId))
                {
                    sb.AppendLine(string.Format("ko.applyBindings({0}, document.getElementById('{1}'))", ViewModelName, wrapperId));
                }
                else
                {
                    sb.AppendLine(string.Format("ko.applyBindings({0});", ViewModelName));
                }
                if (applyOnDocumentReady)
                {
                    sb.AppendLine("});");
                }
                sb.AppendLine(@"</script>");
                return new HtmlString(sb.ToString());
            }
            return new HtmlString(GetInitializeData(model, true, wrapperId, applyOnDocumentReady));
        }

        public HtmlString LazyApply(TModel model, string actionName, string controllerName, string wrapperId = "")
        {
            var sb = new StringBuilder();

            sb.AppendLine(@"<script type=""text/javascript""> ");
            sb.AppendLine("$(document).ready(function() {");

            sb.AppendLine(string.Format("$.ajax({{ url: '{0}', type: 'POST', success: function (data) {{", Url().Action(actionName, controllerName)));

            string mappingData = KnockoutJsModelBuilder.CreateMappingData<TModel>();
            if (mappingData == "{}")
                sb.AppendLine(string.Format("var {0} = ko.mapping.fromJS(data); ", ViewModelName));
            else
            {
                sb.AppendLine(string.Format("var {0}MappingData = {1};", ViewModelName, mappingData));
                sb.AppendLine(string.Format("var {0} = ko.mapping.fromJS(data, {0}MappingData); ", ViewModelName));
            }
            sb.Append(KnockoutJsModelBuilder.AddComputedToModel(model, ViewModelName));
            if (!string.IsNullOrEmpty(wrapperId))
            {
                sb.AppendLine(string.Format("ko.applyBindings({0}, document.getElementById('{1}'))", ViewModelName, wrapperId));
            }
            else
            {
                sb.AppendLine(string.Format("ko.applyBindings({0});", ViewModelName));
            }

            sb.AppendLine("}, error: function (error) { alert('There was an error posting the data to the server: ' + error.responseText); } });");

            sb.AppendLine("});");
            sb.AppendLine(@"</script>");

            return new HtmlString(sb.ToString());
        }

        private int ActiveSubcontextCount
        {
            get
            {
                return ContextStack.Count - 1 - ContextStack.IndexOf(this);
            }
        }

        public KnockoutForeachContext<TItem> Foreach<TItem>(Expression<Func<TModel, IList<TItem>>> binding)
        {
            var expression = KnockoutExpressionConverter.Convert(binding, CreateData());
            var regionContext = new KnockoutForeachContext<TItem>(viewContext, expression);
            regionContext.WriteStart(viewContext.Writer);
            regionContext.ContextStack = ContextStack;
            ContextStack.Add(regionContext);
            return regionContext;
        }

        public KnockoutWithContext<TItem> With<TItem>(Expression<Func<TModel, TItem>> binding)
        {
            var expression = KnockoutExpressionConverter.Convert(binding, CreateData());
            var regionContext = new KnockoutWithContext<TItem>(viewContext, expression);
            regionContext.WriteStart(viewContext.Writer);
            regionContext.ContextStack = ContextStack;
            ContextStack.Add(regionContext);
            return regionContext;
        }

        public KnockoutIfContext<TModel> If(Expression<Func<TModel, bool>> binding)
        {
            var regionContext = new KnockoutIfContext<TModel>(viewContext, KnockoutExpressionConverter.Convert(binding));
            regionContext.InStack = false;
            regionContext.WriteStart(viewContext.Writer);
            return regionContext;
        }

        public string GetInstanceName()
        {
            switch (ActiveSubcontextCount)
            {
                case 0:
                    return "";
                case 1:
                    return "$parent";
                default:
                    return "$parents[" + (ActiveSubcontextCount - 1) + "]";
            }
        }

        private string GetContextPrefix()
        {
            var sb = new StringBuilder();
            int count = ActiveSubcontextCount;
            for (int i = 0; i < count; i++)
                sb.Append("$parentContext.");
            return sb.ToString();
        }

        public string GetIndex()
        {
            return GetContextPrefix() + "$index()";
        }

        public virtual KnockoutExpressionData CreateData()
        {
            return new KnockoutExpressionData { InstanceNames = new[] { GetInstanceName() } };
        }

        public virtual KnockoutBinding<TModel> Bind
        {
            get
            {
                return new KnockoutBinding<TModel>(this, CreateData().InstanceNames, CreateData().Aliases);
            }
        }

        public virtual KnockoutHtml<TModel> Html
        {
            get
            {
                return new KnockoutHtml<TModel>(viewContext, this, CreateData().InstanceNames, CreateData().Aliases);
            }
        }

    //TODO: rewrite
        public MvcHtmlString ServerAction(string actionName, string controllerName, object routeValues = null)
        {
            var url = Url().Action(actionName, controllerName, routeValues);
      url = url.Replace("%28", "(");
      url = url.Replace("%29", ")");
      url = url.Replace("%24", "$");
            string exec = string.Format(@"executeOnServer({0}, '{1}')", ViewModelName, url);
            if (exec.Contains("%24index()"))
            {
                int count = exec.Length / 17 + 1;
                string[] from = new string[count], to = new string[count];
                from[0] = "%24index()";
                to[0] = "$index()";
                for (int i = 1; i < count; i++)
                {
                    from[i] = "%24parentContext." + from[i - 1];
                    to[i] = "$parentContext." + to[i - 1];
                }
                for (int i = count - 1; i >= 0; i--)
                    exec = exec.Replace(from[i], "'+" + to[i] + "+'");
            }
            if (exec.Contains("%24index%28%29")) // For ASP.NET MVC4
            {
                int count = exec.Length / 17 + 1;
                string[] from = new string[count], to = new string[count];
                from[0] = "%24index%28%29";
                to[0] = "$index%28%29";
                for (int i = 1; i < count; i++)
                {
                    from[i] = "%24parentContext." + from[i - 1];
                    to[i] = "$parentContext." + to[i - 1];
          nextPattern = parentPrefix + pattern;
                }
                for (int i = count - 1; i >= 0; i--)
                    exec = exec.Replace(from[i], "'+" + to[i] + "+'");
            }
            return new MvcHtmlString(exec);
        }

    protected UrlHelper Url()
        {
      return new UrlHelper(viewContext.RequestContext);
        }
    }
}
