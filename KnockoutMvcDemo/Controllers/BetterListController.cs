using System.Collections.Generic;
using System.Web.Mvc;
using KnockoutMvcDemo.Models;

namespace KnockoutMvcDemo.Controllers
{
    public class BetterListController : BaseController
    {
        public ActionResult Index()
        {
            InitializeViewBag("Better list");
            var model = new BetterListModel
            {
                AllItems = new List<string> { "Fries", "Eggs Benedict", "Ham", "Cheese" },
                SelectedItems = new List<string> { "Ham" }
            };
            return View(model);
        }

        public ActionResult AddItem(BetterListModel model)
        {
            model.AddItem();
            return Json(model);
        }

        public ActionResult RemoveSelected(BetterListModel model)
        {
            model.RemoveSelected();
            return Json(model);
        }

        public ActionResult SortItems(BetterListModel model)
        {
            model.SortItems();
            return Json(model);
        }
    }
}
