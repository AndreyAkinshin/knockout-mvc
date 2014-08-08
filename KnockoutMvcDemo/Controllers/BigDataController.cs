using System.Web.Mvc;
using KnockoutMvcDemo.Models;

namespace KnockoutMvcDemo.Controllers
{
    public class BigDataController : BaseController
    {
        public ActionResult Index()
        {
            InitializeViewBag("Big data");
            return View();
        }

        public ActionResult InitialData(BigDataModel model)
        {
            model.LoadData();
            return Json(model);
        }
    }
}
