using System.Configuration;
using System.Web.Mvc;
using TrelloNet;

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

        private ITrello _trelloInstance { get; set; }

        protected ITrello TrelloInstance
        {
            get
            {
                if (_trelloInstance == null)
                {
                    _trelloInstance = new Trello(TrelloApiKey);
                    var userKey = GetUserKey();
                    _trelloInstance.Authorize(userKey);
                }

                return _trelloInstance;
            }
        }
    }
}
