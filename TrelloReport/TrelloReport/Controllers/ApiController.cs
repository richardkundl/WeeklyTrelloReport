using System.Linq;
using System.Web.Mvc;
using System.Text;
using TrelloNet;
using System.Collections.Generic;
using TrelloReport.Helper;
using TrelloReport.Models;

namespace TrelloReport.Controllers
{
    // TODO: Ezeket majd az authorize filterre kötni, amit a majd trello belépéshez illesztünk
    /// <summary>
    /// Api controller
    /// </summary>
    public class ApiController : BaseController
    {
        /// <summary>
        /// Create JSON response
        /// </summary>
        /// <param name="data">responsed data</param>
        /// <returns>Complete JSON data</returns>
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

        /// <summary>
        /// User is Trello authenticated
        /// </summary>
        /// <returns>Bool and auth url</returns>
        public ActionResult IsAuthenticated()
        {
            var userKey = GetUserKey();

            // validate
            if (string.IsNullOrEmpty(userKey))
            {
                // generate auth url
                var link = new Trello(TrelloApiKey).GetAuthorizationUrl(TrelloAppName, Scope.ReadOnly, Expiration.Never).ToString();
                return CreateResponse(new { isLogged = false, authUrl = link });
            }

            return CreateResponse(new { isLogged = true, authUrl = string.Empty });
        }

        /// <summary>
        /// Get user boards
        /// </summary>
        /// <returns>User boars</returns>
        public ActionResult GetBoards()
        {
            var boards = TrelloInstance.Boards.ForMe().OrderBy(b => b.Name);
            var retBoards = new List<object>();
            foreach (var board in boards)
            {
                retBoards.Add(new { board.Id, board.Name, board.Closed, board.IdOrganization, board.Desc });
            }

            return CreateResponse(retBoards);
        }

        /// <summary>
        /// Get lists from board
        /// </summary>
        /// <param name="boardId">Board Id</param>
        /// <returns>Actually board lists</returns>
        public ActionResult GetLists(string boardId)
        {
            if (string.IsNullOrEmpty(boardId))
            {
                return CreateResponse(null);
            }

            var lists = TrelloInstance.Lists.ForBoard(new BoardId(boardId)).OrderBy(l => l.Pos);
            return CreateResponse(lists);
        }

        /// <summary>
        /// Get users from board
        /// </summary>
        /// <param name="boardId">Board Id</param>
        /// <returns>Actually users in board</returns>
        public ActionResult GetUsersOnBoard(string boardId)
        {
            if (string.IsNullOrEmpty(boardId))
            {
                return CreateResponse(null);
            }

            var users = TrelloInstance.Members.ForBoard(new BoardId(boardId));
            return CreateResponse(users);
        }

        /// <summary>
        /// Create report preview
        /// </summary>
        /// <param name="model">input model</param>
        /// <returns>filtered and ordered cards</returns>
        [HttpPost]
        public ActionResult ReportPreview(ReportModel model)
        {
            // if input data is empty
            if (model == null || string.IsNullOrEmpty(model.BoardId))
            {
                return CreateResponse(null);
            }

            var cards = CardHelper.GetCards(model, TrelloInstance);
            return CreateResponse(cards);
        }
    }
}
