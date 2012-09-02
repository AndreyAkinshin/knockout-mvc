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
      ViewBag.Model = GetModel(modelName);
      ViewBag.Controller = GetController(modelName);
      ViewBag.Razor = GetRazor(modelName);
      ViewBag.Description = new MvcHtmlString(GetDescription(modelName));
      if (!string.IsNullOrWhiteSpace(caption))
      {
        ViewBag.Title = caption;
        ViewBag.Caption = caption;
      }
    }

    private string GetModel(string modelName)
    {
      return GetCode(LoadFile(string.Format("\\Models\\{0}Model.cs", modelName)));
    }

    private string GetController(string modelName)
    {
      return GetCode(LoadFile(string.Format("\\Controllers\\{0}Controller.cs", modelName)));
    }

    private string GetCode(string fileName)
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
      var sb = new StringBuilder();
      foreach (var line in split)
      {
        if (sb.Length > 0)
          sb.Append('\n');
        sb.Append(line.StartsWith("  ") ? line.Substring(2) : line);
      }
      return sb.ToString();
    }

    private string GetRazor(string modelName)
    {
      var razor = LoadFile(string.Format("\\Views\\{0}\\Index.cshtml", modelName));
      return razor;
    }

    private string GetDescription(string modelName)
    {
      string description = LoadFile(string.Format("\\Descriptions\\{0}.html", modelName));
      if (string.IsNullOrWhiteSpace(description))
        description = "No description";
      return description;
    }
  }
}
