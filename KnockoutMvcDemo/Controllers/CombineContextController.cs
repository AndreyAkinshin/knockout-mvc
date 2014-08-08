using System.Collections.Generic;
using System.Web.Mvc;
using KnockoutMvcDemo.Models;

namespace KnockoutMvcDemo.Controllers
{
    public class CombineContextController : BaseController
    {
        public ActionResult Index()
        {
            InitializeViewBag("Combine context");
            var model = new CombineContextModel
              {
                  Key = "Global",
                  Items = new List<CombineContextItemModel>
            {
              new CombineContextItemModel
                {
                  Caption = "Item 1",
                  SubItems = new List<CombineContextSubItemModel>
                    {
                      new CombineContextSubItemModel {Name = "SubItem1-1"},
                      new CombineContextSubItemModel {Name = "SubItem1-2"},
                      new CombineContextSubItemModel {Name = "SubItem1-3"},
                    }
                },
              new CombineContextItemModel
                {
                  Caption = "Item 2",
                  SubItems = new List<CombineContextSubItemModel>
                    {
                      new CombineContextSubItemModel {Name = "SubItem2-1"},
                      new CombineContextSubItemModel {Name = "SubItem2-2"},
                      new CombineContextSubItemModel {Name = "SubItem2-3"},
                    }
                }
            }
              };
            return View(model);
        }
    }
}
