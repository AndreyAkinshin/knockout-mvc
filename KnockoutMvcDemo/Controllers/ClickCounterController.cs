using System.Web.Mvc;
using KnockoutMvcDemo.Models;

namespace KnockoutMvcDemo.Controllers
{
  public class ClickCounterController : BaseController
  {
    public ActionResult Index()
    {
      InitializeViewBag("Click counter");
      return View(new ClickCounterModel());
    }

    public ActionResult RegisterClick(ClickCounterModel model)
    {
      model.RegisterClick();
      return Json(model);
    }

    public ActionResult ResetClicks(ClickCounterModel model)
    {
      model.ResetClicks();
      return Json(model);
    }
  }
}
