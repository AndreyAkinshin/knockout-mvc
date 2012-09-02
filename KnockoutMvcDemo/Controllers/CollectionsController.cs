using System.Collections.Generic;
using System.Web.Mvc;
using KnockoutMvcDemo.Models;

namespace KnockoutMvcDemo.Controllers
{
  public class CollectionsController : BaseController
  {
    public ActionResult Index()
    {
      InitializeViewBag("Collections");
      var model = new CollectionsModel();
      model.People = new List<CollectionsPersonModel>();
      model.People.Add(new CollectionsPersonModel
      {
        Name = "Annabelle",
        Children = new List<string> { "Arnie", "Anders", "Apple" }
      });
      model.People.Add(new CollectionsPersonModel
      {
        Name = "Bertie",
        Children = new List<string> { "Boutros-Boutros", "Brianna", "Barbie", "Bee-bop" }
      });
      model.People.Add(new CollectionsPersonModel
      {
        Name = "Charles",
        Children = new List<string> { "Cayenne", "Cleopatra" }
      });
      return View(model);
    }

    public ActionResult AddChild(CollectionsModel model, int index)
    {
      model.AddChild(index);
      return Json(model);
    }
  }
}
