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
        /// Card action types
        /// </summary>
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

        /// <summary>
        /// Calculate end date with interval
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="interval">Report interval(eg: weekly, daily)</param>
        /// <returns>Calculated end date</returns>
        private static DateTime GetEndDate(DateTime startDate, string interval)
        {
            var endDate = DateTime.Now.Date.AddDays(1).AddMinutes(-1);
            if (interval == "weekly")
            {
                //  need last day 23h59m time  
                endDate = startDate.Date.AddDays(7).AddMinutes(-1);
            }
            else if (interval == "daily")
            {
                endDate = startDate.Date.AddDays(1).AddMinutes(-1);
            }
            else if (interval == "actually")
            {
                // do nothing, is setted
            }

            return endDate;
        }

        /// <summary>
        /// Calculate start date
        /// </summary>
        /// <param name="startdate">input start date</param>
        /// <param name="interval">Report interval</param>
        /// <returns></returns>
        private static DateTime GetStartDate(string startdate, string interval)
        {
            var startDate = Convert.ToDateTime(startdate).Date;
            if (startDate.Year < DateTime.Now.Year - 1)
            {
                startDate = DateTime.Now.Date;
            }
            if (interval == "actually")
            {
                startDate = DateTime.Now.Date;
            }

            return startDate;
        }

        /// <summary>
        /// Get card Id from action(only card action)
        /// </summary>
        /// <param name="action">Trello action</param>
        /// <returns> if exits card Id, else empty string</returns>
        private static string GetCardIdFromAction(Action action)
        {
            if (action is AddMemberToCardAction)
            {
                return (action as AddMemberToCardAction).Data.Card.GetCardId();
            }
            if (action is CreateCardAction)
            {
                return (action as CreateCardAction).Data.Card.GetCardId();
            }
            if (action is AddAttachmentToCardAction)
            {
                return (action as AddAttachmentToCardAction).Data.Card.GetCardId();
            }
            if (action is AddChecklistToCardAction)
            {
                return (action as AddChecklistToCardAction).Data.Card.GetCardId();
            }
            if (action is CommentCardAction)
            {
                return (action as CommentCardAction).Data.Card.GetCardId();
            }
            if (action is ConvertToCardFromCheckItemAction)
            {
                return (action as ConvertToCardFromCheckItemAction).Data.Card.GetCardId();
            }
            if (action is MoveCardFromBoardAction)
            {
                return (action as MoveCardFromBoardAction).Data.Card.GetCardId();
            }
            if (action is MoveCardToBoardAction)
            {
                return (action as MoveCardToBoardAction).Data.Card.GetCardId();
            }
            if (action is RemoveChecklistFromCardAction)
            {
                return (action as RemoveChecklistFromCardAction).Data.Card.GetCardId();
            }
            if (action is RemoveMemberFromCardAction)
            {
                return (action as RemoveMemberFromCardAction).Data.Card.GetCardId();
            }
            if (action is UpdateCardAction)
            {
                return (action as UpdateCardAction).Data.Card.GetCardId();
            }
            if (action is UpdateCheckItemStateOnCardAction)
            {
                return (action as UpdateCheckItemStateOnCardAction).Data.Card.GetCardId();
            }

            return string.Empty;
        }

        /// <summary>
        /// Get card Ids from all action
        /// </summary>
        /// <param name="trello">trello interface</param>
        /// <param name="boardId">board Id</param>
        /// <param name="startDate">filter start date</param>
        /// <param name="endDate">filter end date</param>
        /// <returns>List of card ids</returns>
        private static List<string> GetCardIdsFromActions(ITrello trello, string boardId, DateTime startDate, DateTime endDate)
        {
            var actions = trello.Actions.ForBoard(
                            new BoardId(boardId),
                            since: Since.Date(startDate),
                            paging: new Paging(1000, 0),
                            filter: CardActionTypes);
            var changedCardActions = actions.Where(a => a.Date > startDate && a.Date < endDate).ToList();
            var changedCardIds = new List<string>();
            foreach (var changedCardAction in changedCardActions)
            {
                // get only cards
                var cardId = GetCardIdFromAction(changedCardAction);

                // modified card
                if (!string.IsNullOrEmpty(cardId))
                {
                    // card id isnt exist
                    if (!changedCardIds.Contains(cardId))
                    {
                        changedCardIds.Add(cardId);
                    }
                }
            }

            return changedCardIds;
        }

        /// <summary>
        /// Separate <paramref name="cards"/> by card labels
        /// </summary>
        /// <param name="cards">Unsepareted cards</param>
        /// <returns>Separeted cards</returns>
        private static IEnumerable<Card> SepareteCardByLabels(IEnumerable<Card> cards)
        {
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

            return separeted;
        }

        /// <summary>
        /// Order cards by list->label names->position
        /// </summary>
        /// <param name="cards">Unordered cards</param>
        /// <returns>Ordered cards</returns>
        private static IEnumerable<Card> OrderCards(IEnumerable<Card> cards)
        {
            var comparer = new CardComparer();
            var ordered = cards.OrderBy(c => c.IdList)
                                .ThenBy(c => c.Labels, comparer)
                                .ThenBy(c => c.Pos)
                                .ToList();
            return ordered;
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

            // set query start date 
            var startDate = GetStartDate(model.StartDate, model.ReportIntervalType);

            // set query end date
            var endDate = GetEndDate(startDate, model.ReportIntervalType);

            // query cards
            var cards = TrelloInstance.Cards.ForBoard(new BoardId(model.BoardId));

            // if interval type is actually, doesn't need activity filter
            if (model.ReportIntervalType != "actually")
            {
                // query card actions
                var changedCards = GetCardIdsFromActions(TrelloInstance, model.BoardId, startDate, endDate);

                // filter cards by date interval
                cards = cards.Where(c => changedCards.Contains(c.Id));
            }

            // filter cards by lists
            cards = cards.Where(c => model.ListIds.Contains(c.IdList));

            // filter cards by user
            cards = cards.Where(c => c.Members.Select(m => m.Id).Intersect(model.UserIds).Any());

            // separated cards by labels
            cards = SepareteCardByLabels(cards);

            // order cards 
            cards = OrderCards(cards);

            return CreateResponse(cards);
        }
    }
}
