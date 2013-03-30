using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrelloNet;
using TrelloReport.Models;
using System.Configuration;

namespace TrelloReport.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private readonly string _trelloApiKey = ConfigurationManager.AppSettings.Get("TrelloKey");

        private readonly string _trelloAppName = "Trello Weekly Report";

        private string GetUserKey()
        {
            return ConfigurationManager.AppSettings.Get("TrelloUserKey");
        }

        public ActionResult Index()
        {
            var model = new HomeModel { Message = "Welcome to ASP.NET MVC!" };

            var trello = new Trello(_trelloApiKey);
            model.TrelloUserKey = GetUserKey();
            if (string.IsNullOrEmpty(model.TrelloUserKey))
            {
                model.TrelloAuthUrl = trello.GetAuthorizationUrl(_trelloAppName, Scope.ReadOnly, Expiration.Never);
            }
            else
            {
                trello.Authorize(model.TrelloUserKey);
            }
            model.Boards = trello.Boards.ForMe();

            return View(model);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
