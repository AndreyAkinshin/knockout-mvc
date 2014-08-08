using System.Web.Mvc;
using KnockoutMvcDemo.Models;

namespace KnockoutMvcDemo.Controllers
{
    public class ComplexBindingController : BaseController
    {
        public ActionResult Index()
        {
            InitializeViewBag("Complex binding");
            var model = new ComplexBindingModel
            {
                Price = -10,
                FontSize = "20px"
            };
            return View(model);
        }
    }
}
