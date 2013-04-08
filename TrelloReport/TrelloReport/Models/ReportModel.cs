using System.Collections.Generic;

namespace TrelloReport.Models
{
    /// <summary>
    /// Report action input model
    /// </summary>
    public class ReportModel
    {
        /// <summary>
        /// Board Id
        /// </summary>
        public string BoardId { get; set; }

        /// <summary>
        /// List ids filter
        /// </summary>
        public List<string> ListIds { get; set; }

        /// <summary>
        /// User Ids filter
        /// </summary>
        public List<string> UserIds { get; set; }

        /// <summary>
        /// Report interval type
        /// </summary>
        public string ReportIntervalType { get; set; }

        /// <summary>
        /// Report start date
        /// </summary>
        public string StartDate { get; set; }
    }
}