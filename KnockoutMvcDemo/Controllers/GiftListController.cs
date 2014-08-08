using System.Collections.Generic;
using System.Web.Mvc;
using KnockoutMvcDemo.Models;

namespace KnockoutMvcDemo.Controllers
{
    public class GiftListController : BaseController
    {
        public ActionResult Index()
        {
            InitializeViewBag("Gift list");
            var model = new GiftListModel
            {
                Gifts = new List<GiftModel>
            {
              new GiftModel {Title = "Tall Hat", Price = 49.95},
              new GiftModel {Title = "Long Cloak", Price = 78.25}
            }
            };
            return View(model);
        }

        public ActionResult AddGift(GiftListModel model)
        {
            model.AddGift();
            return Json(model);
        }

        public ActionResult RemoveGift(GiftListModel model, int index)
        {
            model.RemoveGift(index);
            return Json(model);
        }

        public ActionResult Save(GiftListModel model)
        {
            model.Save();
            return Json(model);
        }
    }
}
