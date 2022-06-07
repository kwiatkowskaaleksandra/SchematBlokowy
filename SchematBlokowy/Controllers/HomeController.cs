using SchematBlokowy.Utils;
using System.Web.Mvc;

namespace SchematBlokowy.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "SchematBlokowy", new { area = AreaNames.SchematBlokowy_Area });
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public bool KeepSession()
        {
            return true;
        }
    }
}