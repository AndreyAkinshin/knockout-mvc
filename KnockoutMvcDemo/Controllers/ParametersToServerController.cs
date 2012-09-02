using System.Web.Mvc;
using KnockoutMvcDemo.Models;

namespace KnockoutMvcDemo.Controllers
{
  public class ParametersToServerController : BaseController
  {
    public ActionResult Index()
    {
      InitializeViewBag("Parameters to server");
      return View(new ParametersToServerModel());
    }

    public ActionResult Increment(ParametersToServerModel model, int value)
    {
      model.Increment(value);
      return Json(model);
    }

    public ActionResult AddToString(ParametersToServerModel model, char letter, int count)
    {
      model.AddToString(letter + "", count);
      return Json(model);
    }
  }
}
