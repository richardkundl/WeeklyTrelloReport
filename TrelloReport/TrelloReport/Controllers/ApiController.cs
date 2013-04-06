using System.Linq;
using System.Web.Mvc;
using System.Text;
using Omu.ValueInjecter;
using TrelloNet;
using System.Collections.Generic;
using TrelloReport.Models;
using System;
using Action = TrelloNet.Action;

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

        private static IEnumerable<ActionType> CardActionTypes
        {
            get
            {
                return new List<ActionType>
                              {
                                  ActionType.AddMemberToCard,
                                  ActionType.CreateCard,
                                  ActionType.AddAttachmentToCard,
                                  ActionType.AddChecklistToCard,
                                  ActionType.CommentCard,
                                  ActionType.ConvertToCardFromCheckItem,
                                  ActionType.MoveCardFromBoard,
                                  ActionType.MoveCardToBoard,
                                  ActionType.RemoveChecklistFromCard,
                                  ActionType.RemoveMemberFromCard,
                                  ActionType.UpdateCard,
                                  ActionType.UpdateCheckItemStateOnCard
                              };
            }
        }

        private static string GetCardIdFromAction(Action action)
        {
            if (action is AddMemberToCardAction)
            {
                return (action as AddMemberToCardAction).Data.Card.GetCardId();
            }

            return string.Empty;
        }

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


        public ActionResult GetLists(string boardId)
        {
            if (string.IsNullOrEmpty(boardId))
            {
                return CreateResponse(null);
            }

            var lists = TrelloInstance.Lists.ForBoard(new BoardId(boardId)).OrderBy(l => l.Pos);
            return CreateResponse(lists);
        }

        public ActionResult GetUsersOnBoard(string boardId)
        {
            if (string.IsNullOrEmpty(boardId))
            {
                return CreateResponse(null);
            }

            var users = TrelloInstance.Members.ForBoard(new BoardId(boardId));
            return CreateResponse(users);
        }

        [HttpPost]
        public ActionResult ReportPreview(ReportModel model)
        {
            // if input data is empty
            if (model == null || string.IsNullOrEmpty(model.BoardId))
            {
                return CreateResponse(null);
            }

            // set query start date 
            var startDate = Convert.ToDateTime(model.StartDate).Date;
            if (startDate.Year < DateTime.Now.Year - 1)
            {
                startDate = DateTime.Now.Date;
            }

            // set query end date
            DateTime endDate;
            if (model.ReportIntervalType == "weekly")
            {
                //  need last day 23h59m time  
                endDate = startDate.Date.AddDays(7).AddMinutes(-1);
            }
            else if (model.ReportIntervalType == "daily")
            {
                endDate = startDate.Date.AddDays(1).AddMinutes(-1);
            }
            else
            {
                endDate = DateTime.Now.Date.AddDays(1).AddMinutes(-1);
            }

            // query cards
            var cards = TrelloInstance.Cards.ForBoard(new BoardId(model.BoardId));

            // filter cards by lists
            cards = cards.Where(c => model.ListIds.Contains(c.IdList));

            // filter cards by user
            cards = cards.Where(c => c.Members.Select(m => m.Id).Intersect(model.UserIds).Any());

            // filter cards by date intervall
            var actions = TrelloInstance.Actions.ForBoard(
                            new BoardId(model.BoardId),
                            since: Since.Date(startDate),
                            paging: new Paging(1000, 0),
                            filter: CardActionTypes);
            var changedCardActions = actions.Where(a => a.Date > startDate && a.Date < endDate).ToList();
            var changedCardIds = new List<string>();
            foreach (var changedCardAction in changedCardActions)
            {
                changedCardIds.Add(GetCardIdFromAction(changedCardAction));
            }

            // separated cards by labels
            var separeted = new List<Card>();
            foreach (var card in cards)
            {
                // if more than one label, you should be separately
                if (card.Labels.Count > 1)
                {
                    foreach (var label in card.Labels)
                    {
                        var newCard = new Card();
                        newCard.InjectFrom(card);
                        newCard.Labels = new List<Card.Label> { label };
                        separeted.Add(newCard);
                    }
                }
                else
                {
                    separeted.Add(card);
                }
            }

            // order cards by list->label names->position
            var comparer = new CardComparer();
            var ordered = separeted.OrderBy(c => c.IdList)
                                .ThenBy(c => c.Labels, comparer)
                                .ThenBy(c => c.Pos);

            return CreateResponse(ordered);
        }
    }
}
