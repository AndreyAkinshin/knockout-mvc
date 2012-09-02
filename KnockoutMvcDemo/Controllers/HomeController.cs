using System.Web.Mvc;

namespace KnockoutMvcDemo.Controllers
{
  public class HomeController : System.Web.Mvc.Controller
  {
    //
    // GET: /Home/

    public ActionResult Index()
    {      
      return View();
    }

    //
    // GET: /BrowserSupport/

    public ActionResult Links()
    {
      return View();
    }

    //
    // GET: /QuickStart/

    public ActionResult QuickStart()
    {
      return View();
    }

    //
    // GET: /Documentation/

    public ActionResult Documentation()
    {
      return View();
    }

    //
    // GET: /Intro/

    public ActionResult Introduction()
    {
      return View();
    }

    //
    // GET: /Downloads/

    public ActionResult Downloads()
    {
      return View();
    }

    //
    // GET: /Roadmap/

    public ActionResult Roadmap()
    {
      return View();
    }
  }
}
