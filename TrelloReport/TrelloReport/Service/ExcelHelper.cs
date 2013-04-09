using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrelloNet;
using TrelloReport.Service.Interface;

namespace TrelloReport.Helper
{
    public class NpoiExcelService : IExcelService
    {
        public byte[] GenerateCardReports(List<Card> cards, List<List> lists)
        {
            throw new NotImplementedException();
        }
    }
}