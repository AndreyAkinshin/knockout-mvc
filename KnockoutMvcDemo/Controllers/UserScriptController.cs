using System.Web.Mvc;
using KnockoutMvcDemo.Models;

namespace KnockoutMvcDemo.Controllers
{
  public class UserScriptController : BaseController
  {
    public ActionResult Index()
    {
      InitializeViewBag("User script");
      var model = new UserScriptModel { Message = "Knockout" };
      return View(model);
    }

    public ActionResult AddLetter(UserScriptModel model)
    {
      model.AddLetter();
      return Json(model);
    }
  }
}
