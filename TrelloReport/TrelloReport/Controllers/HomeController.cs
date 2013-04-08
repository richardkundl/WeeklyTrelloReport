using System.Web.Mvc;
using TrelloReport.Models;

namespace TrelloReport.Controllers
{
    /// <summary>
    /// Home controller
    /// </summary>
    [HandleError]
    public class HomeController : BaseController
    {
        /// <summary>
        /// Index action
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new HomeModel();

            return View(model);
        }

        /// <summary>
        /// About action
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            return View();
        }
    }
}
