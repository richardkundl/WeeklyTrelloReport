using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrelloNet;
using TrelloReport.Service.Interface;
using Word.Api.Interfaces;
using Word.W2004;

namespace TrelloReport.Service
{
    public class Sharp2WordService: IWordService
    {
        public byte[] GenerateCardReports(List<Card> cards, List<List> lists)
        {
            IDocument myDoc = new Document2004();
            throw new NotImplementedException();
        }
    }
}