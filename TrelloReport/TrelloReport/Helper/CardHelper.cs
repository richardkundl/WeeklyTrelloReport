using System;
using System.Collections.Generic;
using System.Linq;
using Omu.ValueInjecter;
using TrelloNet;
using TrelloReport.Models;
using Action = TrelloNet.Action;

namespace TrelloReport.Helper
{
    /// <summary>
    /// </summary>
    public static class CardHelper
    {
        /// <summary>
        /// Card action types
        /// </summary>
        public static IEnumerable<ActionType> CardActionTypes
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
        /// Get card Id from action(only card action)
        /// </summary>
        /// <param name="action">Trello action</param>
        /// <returns> if exits card Id, else empty string</returns>
        public static string GetCardIdFromAction(Action action)
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
        public static List<string> GetCardIdsFromActions(ITrello trello, string boardId, DateTime startDate, DateTime endDate)
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
        public static IEnumerable<Card> SepareteCardByLabels(IEnumerable<Card> cards)
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
        public static IEnumerable<Card> OrderCards(IEnumerable<Card> cards)
        {
            var comparer = new CardComparer();
            var ordered = cards.OrderBy(c => c.IdList)
                                .ThenBy(c => c.Labels, comparer)
                                .ThenBy(c => c.Pos)
                                .ToList();
            return ordered;
        }

        /// <summary>
        /// Get cards for a report
        /// </summary>
        /// <param name="model">Input model</param>
        /// <param name="instance">Trello instance</param>
        /// <returns>Card list</returns>
        public static IEnumerable<Card> GetCards(ReportModel model, ITrello instance)
        {
            // set query start date 
            var startDate = DateTimeHelper.GetStartDate(model.StartDate, model.ReportIntervalType);

            // set query end date
            var endDate = DateTimeHelper.GetEndDate(startDate, model.ReportIntervalType);

            // query cards
            var cards = instance.Cards.ForBoard(new BoardId(model.BoardId));

            // if interval type is actually, doesn't need activity filter
            if (model.ReportIntervalType != "actually")
            {
                // query card actions
                var changedCards = GetCardIdsFromActions(instance, model.BoardId, startDate, endDate);

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

            return cards;
        }
    }
}