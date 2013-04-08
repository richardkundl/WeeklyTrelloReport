using System.Linq;
using System.Web.Mvc;
using TrelloReport.Models;
using TrelloReport.Helper;

namespace TrelloReport.Controllers
{
    public class ExportController : BaseController
    {
        public FileContentResult Word(ReportModel model)
        {
            var cards = CardHelper.GetCards(model, TrelloInstance).ToList();
            var result = WordHelper.HelloWord(cards);
            return File(result,
                "application/vnd.ms-word",
                "mytestfile.doc");
        }

        public ActionResult Excel(ReportModel model)
        {
            var result = "";
            return File(new System.Text.UTF8Encoding().GetBytes(result),
                "application/vnd.ms-excel",
                "mytestfile.xls");
        }
    }
}
