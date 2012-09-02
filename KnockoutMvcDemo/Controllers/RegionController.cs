using System.Collections.Generic;
using System.Web.Mvc;
using KnockoutMvcDemo.Models;

namespace KnockoutMvcDemo.Controllers
{
  public class RegionController : BaseController
  {
    public ActionResult Index()
    {
      InitializeViewBag("Region");
      var model = new RegionModel
      {
        Condition1 = true,
        Condition2 = false,
        Items = new List<string> { "A", "B", "C" },
        ModelName = "Model",
        SubModel = new RegionSubModel
                     {
                       SubModelName = "SubModel",
                       SubSubModel = new RegionSubSubModel { SubSubModelName = "SubSubModel" }
                     }
      };
      return View(model);
    }
  }
}
