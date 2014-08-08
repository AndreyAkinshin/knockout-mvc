using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using PerpetuumSoft.Knockout;

namespace KnockoutMvcDemo.Controllers
{
    public abstract class BaseController : KnockoutController
    {
        protected string LoadFile(string fileName)
        {
            string filepath = Server.MapPath(fileName);
            try
            {
                var stream = new StreamReader(filepath);
                return stream.ReadToEnd();
            }
            catch (Exception)
            {
                return "";
            }
        }

        protected void InitializeViewBag(string caption = "")
        {
            var modelName = GetType().Name.Substring(0, GetType().Name.Length - "Controller".Length);
            ViewBag.Model = FormatModelCode(modelName);
            ViewBag.Controller = FormatControllerCode(modelName);
            ViewBag.Razor = FormatRazorCode(modelName);
            ViewBag.Description = new MvcHtmlString(FormatDescription(modelName));
            if (!string.IsNullOrWhiteSpace(caption))
            {
                ViewBag.Title = caption;
                ViewBag.Caption = caption;
            }
        }

        private string FormatModelCode(string modelName)
        {
            return FormatSourceCode(LoadFile(string.Format("\\Models\\{0}Model.cs", modelName)));
        }

        private string FormatControllerCode(string modelName)
        {
            return FormatSourceCode(LoadFile(string.Format("\\Controllers\\{0}Controller.cs", modelName)));
        }

        private string FormatSourceCode(string fileName)
        {
            var split = fileName.Split(new[] { '\n' }).ToList();
            while (split.Count > 0)
            {
                string first = split.First().Trim();
                if (first.Length == 0 || first.StartsWith("using") || first.StartsWith("namespace") || first.StartsWith("{"))
                    split.RemoveAt(0);
                else
                    break;
            }
            while (split.Count > 0 && split.Last().Trim().Length == 0)
                split.RemoveAt(split.Count - 1);
            if (split.Count > 0 && split.Last().Trim() == "}")
                split.RemoveAt(split.Count - 1);
            var builder = new StringBuilder();
            foreach (var line in split)
            {
                if (builder.Length > 0)
                    builder.Append('\n');
                builder.Append(line.StartsWith("    ") ? line.Substring(4) : line);
            }
            return builder.ToString();
        }

        private string FormatRazorCode(string modelName)
        {
            var razor = LoadFile(string.Format("\\Views\\{0}\\Index.cshtml", modelName));
            return razor;
        }

        private string FormatDescription(string modelName)
        {
            string description = LoadFile(string.Format("\\Descriptions\\{0}.html", modelName));
            if (string.IsNullOrWhiteSpace(description))
                description = "No description";
            return description;
        }
    }
}
