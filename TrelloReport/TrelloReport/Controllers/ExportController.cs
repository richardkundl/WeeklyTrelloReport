using System.Linq;
using System.Web.Mvc;
using TrelloNet;
using TrelloReport.Models;
using TrelloReport.Helper;
using TrelloReport.Service;
using System;

namespace TrelloReport.Controllers
{
    public class ExportController : BaseController
    {
        public FileContentResult Word(ReportModel model)
        {
            var cards = CardHelper.GetCards(model, TrelloInstance).ToList();
            var lists = TrelloInstance.Lists.ForBoard(new BoardId(model.BoardId)).OrderBy(l => l.Pos).ToList();
            var result = new Sharp2WordService().GenerateCardReports(cards, lists);
            var fileName = string.Format("trello-report-{0}.doc", DateTime.Now.ToString("yyyy-MM-dd"));
            return File(result,
                "application/vnd.ms-word",
                fileName);
        }

        public ActionResult Excel(ReportModel model)
        {
            var cards = CardHelper.GetCards(model, TrelloInstance).ToList();
            var lists = TrelloInstance.Lists.ForBoard(new BoardId(model.BoardId)).OrderBy(l => l.Pos).ToList();
            var result = new NpoiExcelService().GenerateCardReports(cards, lists, model.UserIds);
            var fileName = string.Format("trello-report-{0}.xls", DateTime.Now.ToString("yyyy-MM-dd")); 
            
            return File(result,
                "application/vnd.ms-excel",
                fileName);
        }
    }
}
