﻿using System;
using System.Collections.Generic;
using System.Linq;
using Foxby.Core;
using Foxby.Core.DocumentBuilder;
using Foxby.Core.MetaObjects;
using TrelloNet;
using TrelloReport.Service.Interface;

namespace TrelloReport.Service
{
    public static class FoxbyWordServiceHelper
    {
        public static IDocumentTagContextBuilder AddHeadline(this IDocumentTagContextBuilder builder, int weekNumber)
        {
            return builder.Paragraph(z => z.Bold.Text(String.Format("{0}. hét utáni státuszriport:", weekNumber))).EmptyLine();
        }

        public static IDocumentTagContextBuilder AddCards(this IDocumentTagContextBuilder builder, List<Card> cards)
        {
            foreach (var card in cards)
            {
                builder.Paragraph(z => z.Text(card.Name));
            }

            return builder;
        }

        public static IDocumentTagContextBuilder AddLists(this IDocumentTagContextBuilder builder, List<Card> cards, List<List> lists)
        {
            var groupped = cards.GroupBy(c => c.IdList);

            foreach (var group in groupped)
            {
                var name = lists.FirstOrDefault(l => l.Id == @group.Key).Name;
                builder.Paragraph(z => z.Bold.Text(string.Format("{0}:", name)))
                    .AddCards(group.ToList())
                    .EmptyLine();
            }

            return builder;
        }

        public static IDocumentTagContextBuilder AddReports(this IDocumentTagContextBuilder builder, List<Card> cards, List<List> lists)
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

                builder.Paragraph(z => z.Bold.Text(String.Format("{0}:", name)))
                    .AddLists(group.ToList(), lists)
                    .EmptyLine();

            }

            return builder;
        }
    }

    /// <summary>
    /// </summary>
    public class FoxbyWordService : IWordService
    {
        public byte[] GenerateCardReports(List<Card> cards, List<List> lists)
        {
            using (var docxDocument = new DocxDocument(SimpleTemplate.EmptyWordFile))
            {
                var builder = new DocxDocumentBuilder(docxDocument);
                builder.Tag(SimpleTemplate.HeaderTagName,
                            x => x.Paragraph(z => z.Bold.Text("X. etap - 2012.03.18 - 29 - várható fejlesztések")));

                builder.Tag(SimpleTemplate.ContentTagName,
                            x => FoxbyWordServiceHelper.AddHeadline(x, 12).AddReports(cards, lists));

                return docxDocument.ToArray();
            }
        }
    }
}