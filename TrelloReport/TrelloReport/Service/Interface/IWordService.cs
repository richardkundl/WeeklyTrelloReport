// -----------------------------------------------------------------------
// <copyright file="IWordService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace TrelloReport.Service.Interface
{
    using System.Collections.Generic;
    using TrelloNet;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IWordService
    {
        /// <summary>
        /// Generate Card reports
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="lists"></param>
        /// <returns></returns>
        byte[] GenerateCardReports(List<Card> cards, List<List> lists);
    }
}
