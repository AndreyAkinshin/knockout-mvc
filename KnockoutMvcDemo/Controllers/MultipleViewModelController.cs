using System.Web.Mvc;
using KnockoutMvcDemo.Models;

namespace KnockoutMvcDemo.Controllers
{
    public class MultipleViewModelController : BaseController
    {
        public ActionResult Index()
        {
            InitializeViewBag("Multiple ViewModel in the same View");
            var model = new MultipleViewModelModel
                          {
                              ClickCounterModel = new ClickCounterModel(),
                              HelloWorldModel = new HelloWorldModel
                              {
                                  FirstName = "Steve",
                                  LastName = "Sanderson"
                              }
                          };
            return View(model);
        }
    }
}