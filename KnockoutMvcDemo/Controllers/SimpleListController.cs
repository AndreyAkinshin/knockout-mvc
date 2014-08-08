using System.Collections.Generic;
using System.Web.Mvc;
using KnockoutMvcDemo.Models;

namespace KnockoutMvcDemo.Controllers
{
    public class SimpleListController : BaseController
    {
        public ActionResult Index()
        {
            InitializeViewBag("Simple list");
            var model = new SimpleListModel { Items = new List<string> { "Alpha", "Beta", "Gamma" } };
            return View(model);
        }

        public ActionResult AddItem(SimpleListModel model)
        {
            model.AddItem();
            return Json(model);
        }
    }
}
