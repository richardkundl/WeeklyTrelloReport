using System.Web.Mvc;
using System.Text;
using TrelloNet;
using System.Collections.Generic;

namespace TrelloReport.Controllers
{
    // TODO: Ezeket majd az authorize filterre kötni, amit a majd trello belépéshez illesztünk
    public class ApiController : BaseController
    {
        private JsonResult CreateResponse(object data)
        {
             var result = new JsonResult
                       {
                           ContentEncoding = Encoding.UTF8,
                           ContentType = "application/json",
                           Data = data,
                           JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                       };
            return result;
        }

        public ActionResult IsAuthenticated()
        {
            var userKey = GetUserKey();

            // validate
            if (string.IsNullOrEmpty(userKey))
            {
                // generate auth url
                var link = new Trello(TrelloApiKey).GetAuthorizationUrl(TrelloAppName, Scope.ReadOnly, Expiration.Never).ToString();
                return CreateResponse(new {isLogged = false, authUrl = link});
            }
            return CreateResponse(new { isLogged = true, authUrl = string.Empty });
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
            return CreateResponse(retBoards);
        }


        public ActionResult GetLists(string boardId)
        {
            var trello = new Trello(TrelloApiKey);
            var userKey = GetUserKey();
            trello.Authorize(userKey);
            var lists = trello.Lists.ForBoard(new BoardId(boardId));
            var retBLists = new List<object>();
            foreach (var list in lists)
            {
                retBLists.Add(new { list.Id, list.Name, list.Closed, list.Pos });
            }
            return CreateResponse(retBLists);
        }
    }
}
