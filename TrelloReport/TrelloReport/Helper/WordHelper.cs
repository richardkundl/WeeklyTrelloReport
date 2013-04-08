using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Foxby.Core;
using Foxby.Core.DocumentBuilder;
using Foxby.Core.MetaObjects;
using TrelloNet;

namespace TrelloReport.Helper
{
    /// <summary>
    /// </summary>
    public static class WordHelper
    {
        private static IDocumentTagContextBuilder AddHeadline(this IDocumentTagContextBuilder builder, int weekNumber)
        {
            return builder.Paragraph(z => z.Bold.Text(String.Format("{0}. hét utáni státuszriport:", weekNumber))).EmptyLine();
        }

        private static IDocumentTagContextBuilder AddCards(this IDocumentTagContextBuilder builder, List<Card> cards)
        {
            foreach (var card in cards)
            {
                builder.Paragraph(z => z.Bold.Text("AZ"));
            }

            return builder;
        }

        public static byte[] HelloWord(List<Card> cards)
        {
            using (var docxDocument = new DocxDocument(SimpleTemplate.EmptyWordFile))
            {
                var builder = new DocxDocumentBuilder(docxDocument);
                builder.Tag(SimpleTemplate.HeaderTagName,
                            x => x.Paragraph(z => z.Bold.Text("X. etap - 2012.03.18 - 29 - várható fejlesztések")));

                builder.Tag(SimpleTemplate.ContentTagName,
                            x => AddHeadline(x, 12).AddCards(cards));

                return docxDocument.ToArray();
            }
        }
    }
}