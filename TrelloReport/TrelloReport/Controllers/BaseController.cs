using System.Configuration;
using System.Web.Mvc;

namespace TrelloReport.Controllers
{
    public class BaseController : Controller
    {
        protected readonly string TrelloApiKey = ConfigurationManager.AppSettings.Get("TrelloKey");

        protected readonly string TrelloAppName = "Trello Weekly Report";

        protected string GetUserKey()
        {
            return ConfigurationManager.AppSettings.Get("TrelloUserKey");
        }
    }
}
