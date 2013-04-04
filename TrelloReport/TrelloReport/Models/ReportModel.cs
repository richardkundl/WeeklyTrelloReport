using System.Collections.Generic;

namespace TrelloReport.Models
{
    public class ReportModel
    {
        public string BoardId { get; set; }

        public List<string> ListIds { get; set; }

        public List<string> UserIds { get; set; }

        public string ReportIntervalType { get; set; }

        public string StartDate { get; set; }
    }
}