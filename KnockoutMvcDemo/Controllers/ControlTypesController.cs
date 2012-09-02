using System.Collections.Generic;
using System.Web.Mvc;
using KnockoutMvcDemo.Models;

namespace KnockoutMvcDemo.Controllers
{
  public class ControlTypesController : BaseController
  {
    public ActionResult Index()
    {
      InitializeViewBag("Control types");
      var model = new ControlTypesModel
      {
        StringValue = "Hello",
        PasswordValue = "mypass",
        BooleanValue = true,
        OptionValue = new List<string> { "Alpha", "Beta", "Gamma" },
        SelectedOptionValue = "Gamma",
        MultipleSelectedOptionValues = new List<string> { "Alpha" },
        RadioSelectedOptionValue = "Beta"
      };
      return View(model);
    }
  }
}
