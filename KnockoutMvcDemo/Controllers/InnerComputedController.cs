using System.Collections.Generic;
using System.Web.Mvc;
using KnockoutMvcDemo.Models;

namespace KnockoutMvcDemo.Controllers
{
    public class InnerComputedController : BaseController
    {
        public ActionResult Index()
        {
            InitializeViewBag("Inner computed properties");
            var model = new InnerComputedModel
              {
                  Items = new List<InnerComputedItemModel>
            {
              new InnerComputedItemModel {FirstName = "Annabelle", LastName = "Arnie"},
              new InnerComputedItemModel {FirstName = "Bertie", LastName = "Brianna"},
              new InnerComputedItemModel {FirstName = "Charles", LastName = "Cayenne"},
            },
                  SubModel = new InnerComputedSubModel
                    {
                        Caption = "Count",
                        Value = 3
                    }
              };
            return View(model);
        }
    }
}
