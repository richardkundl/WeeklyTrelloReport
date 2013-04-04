using System.Web.Mvc;
using TrelloNet;
using TrelloReport.Models;

namespace TrelloReport.Controllers
{
    [HandleError]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var model = new HomeModel();

            return View(model);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
