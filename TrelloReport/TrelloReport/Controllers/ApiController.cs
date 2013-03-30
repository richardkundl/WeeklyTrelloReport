using System.Web.Mvc;
using System.Text;
using TrelloNet;
using System.Collections.Generic;

namespace TrelloReport.Controllers
{
    public class ApiController : BaseController
    {
        public ActionResult IsAuthenticated()
        {
            var userKey = GetUserKey();

            // create base object for the return
            var result = new JsonResult
                       {
                           ContentEncoding = Encoding.UTF8,
                           ContentType = "application/json",
                           JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                       };

            // validate
            if (string.IsNullOrEmpty(userKey))
            {
                // generate aut url
                var link = new Trello(TrelloApiKey).GetAuthorizationUrl(TrelloAppName, Scope.ReadOnly, Expiration.Never).ToString();
                result.Data = new {isLogged = false, authUrl = link};
            }
            else
            {
                result.Data = new { isLogged = true, authUrl = string.Empty };
            }

            return result;
        }

        public ActionResult GetBoards()
        {
            var trello = new Trello(TrelloApiKey);
            var userKey = GetUserKey();
            trello.Authorize(userKey);
            var boards = trello.Boards.ForMe();
            var retBoards = new List<object>();
            foreach (var board in boards)
            {
                retBoards.Add(new {board.Id,board.Name, board.Closed, board.IdOrganization });
            }

            return new JsonResult
                       {
                           ContentEncoding = Encoding.UTF8,
                           ContentType = "application/json",
                           Data = retBoards,
                           JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                       };
        }
    }
}
