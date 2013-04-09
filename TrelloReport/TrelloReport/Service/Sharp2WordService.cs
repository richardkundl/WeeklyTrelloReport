using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TrelloNet;
using TrelloReport.Service.Interface;
using Word.Api.Interfaces;
using Word.Utils;
using Word.W2004;
using Word.W2004.Elements;
using System;

namespace TrelloReport.Service
{
    public static class Sharp2WordServiceHelper
    {
        public static Properties GetProperties()
        {
            return new Properties
                       {
                           Company = "Hvg Online Zrt.",
                           Created = DateTime.Now,
                           Description = "Heti trello riport",
                           Keywords = "Trello, Workflow",
                           Title = "Heti trello riport",
                           Author = "\"Weekly Trello Report\" Application"
                       };
        }

        public static IDocument AddHeader(this IDocument builder, int weekNumber)
        {
            builder.Header.AddEle(Paragraph.With("X. etap - 2012.03.18 - 29 - várható fejlesztések").Create());
            return builder;
        }

        public static IDocument AddHeadline(this IDocument builder, int weekNumber)
        {
            builder.AddEle(Heading1.With(string.Format("{0}. hét utáni státuszriport:", weekNumber)).Create());
            return builder;
        }

        public static IDocument AddCards(this IDocument builder, List<Card> cards)
        {
            foreach (var card in cards)
            {
                builder.AddEle(Paragraph.With(card.Name).Create());
            }

            return builder;
        }

        public static IDocument AddLists(this IDocument builder, List<Card> cards, List<List> lists)
        {
            var groupped = cards.GroupBy(c => c.IdList);

            foreach (var group in groupped)
            {
                var name = lists.FirstOrDefault(l => l.Id == @group.Key).Name;
                builder.AddEle(Heading3.With(string.Format("{0}:", name)).WithStyle().SetBold(false).Create());
                builder.AddCards(group.ToList());
            }

            return builder;
        }

        public static IDocument AddReports(this IDocument builder, List<Card> cards, List<List> lists)
        {
            var comparer = new CardComparer();
            var groupped = cards.GroupBy(c => c.Labels, comparer);

            foreach (var group in groupped)
            {
                // write project name
                var name = "Egyéb";
                var key = group.Key.FirstOrDefault();
                if (key != null)
                {
                    name = key.Name;
                }

                builder.AddEle(new BreakLine(1));
                builder.AddEle(Heading2.With(string.Format("{0}:", name)).WithStyle().Bold().Create());
                builder.AddLists(group.ToList(), lists);
            }

            return builder;
        }

        public static byte[] CreateResponse(this IDocument document)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                {
                    streamWriter.Write(Util.Pretty(document.Content));
                }
                return memoryStream.ToArray();
            }
        }
    }

    public class Sharp2WordService : IWordService
    {
        public byte[] GenerateCardReports(List<Card> cards, List<List> lists)
        {
            IDocument myDoc = new Document2004();
            myDoc.Head.Properties = Sharp2WordServiceHelper.GetProperties();
            myDoc = myDoc.AddHeader(12).AddHeadline(12).AddReports(cards, lists);
            return myDoc.CreateResponse();
        }
    }
}