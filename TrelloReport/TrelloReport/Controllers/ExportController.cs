using System.Linq;
using System.Web.Mvc;
using TrelloNet;
using TrelloReport.Models;
using TrelloReport.Helper;
using TrelloReport.Service;
using System;
using TrelloReport.Service.Interface;

namespace TrelloReport.Controllers
{
    public class ExportController : BaseController
    {
		private byte[] GetExportContent(ReportModel model, IReportService service)
		{
			var cards = CardHelper.GetCards(model, TrelloInstance).ToList();
			var lists = TrelloInstance.Lists.ForBoard(new BoardId(model.BoardId)).OrderBy(l => l.Pos).ToList();
			var result = service.GenerateCardReports(cards, lists, model.UserIds);
			return result;
		}

	    public FileContentResult Word(ReportModel model)
	    {
		    var result = GetExportContent(model, new Sharp2WordService());
            var fileName = string.Format("trello-report-{0}.doc", DateTime.Now.ToString("yyyy-MM-dd"));
            return File(result, "application/vnd.ms-word", fileName);
        }

        public ActionResult Excel(ReportModel model)
        {
			var result = GetExportContent(model, new NpoiExcelService());
            var fileName = string.Format("trello-report-{0}.xls", DateTime.Now.ToString("yyyy-MM-dd")); 
            return File(result, "application/vnd.ms-excel", fileName);
        }
    }
}
