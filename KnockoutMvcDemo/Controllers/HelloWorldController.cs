using System.Web.Mvc;
using KnockoutMvcDemo.Models;

namespace KnockoutMvcDemo.Controllers
{
  public class HelloWorldController : BaseController
  {
    public ActionResult Index()
    {
      InitializeViewBag("Hello world");
      return View(new HelloWorldModel
      {
        FirstName = "Steve",
        LastName = "Sanderson"
      });
    }
  }
}
