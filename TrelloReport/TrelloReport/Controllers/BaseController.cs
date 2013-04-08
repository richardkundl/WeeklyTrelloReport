using System.Configuration;
using System.Web.Mvc;
using TrelloNet;

namespace TrelloReport.Controllers
{
    /// <summary>
    /// Base controller
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// Trello Api key
        /// </summary>
        protected readonly string TrelloApiKey = ConfigurationManager.AppSettings.Get("TrelloKey");

        /// <summary>
        /// Trello application name
        /// </summary>
        protected readonly string TrelloAppName = "Trello Weekly Report";

        /// <summary>
        /// Get actually user trello auth key
        /// </summary>
        /// <returns></returns>
        protected string GetUserKey()
        {
            return ConfigurationManager.AppSettings.Get("TrelloUserKey");
        }

        /// <summary>
        /// Trello instance 
        /// </summary>
        private ITrello _trelloInstance { get; set; }

        /// <summary>
        /// Get trello instance
        /// </summary>
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
