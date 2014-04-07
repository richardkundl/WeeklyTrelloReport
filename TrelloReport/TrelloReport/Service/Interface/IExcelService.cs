// -----------------------------------------------------------------------
// <copyright file="IExcelService.cs" company="">
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
    public interface IReportService
    {
        /// <summary>
        /// Generate Card reports
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="lists"></param>
        /// <param name="users"></param>
        /// <returns></returns>
        byte[] GenerateCardReports(List<Card> cards, List<List> lists, List<string> users);
    }
}
