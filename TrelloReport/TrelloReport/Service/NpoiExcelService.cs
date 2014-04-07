using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NPOI.HSSF.UserModel;
using TrelloNet;
using TrelloReport.Service.Interface;
using NPOI.SS.UserModel;

namespace TrelloReport.Service
{
    public static class NpoiExcelServiceHelper
    {
        private static Card RemoveUnusedMembers(this Card card, List<string> memberIds)
        {
            var memberList = card.Members.Where(c => memberIds.Contains(c.Id)).ToList();
            card.Members = memberList;
            return card;
        }

        public static ISheet CreateHeader(this ISheet builder)
        {
            var row = builder.CreateRow(0);

            row.CreateCell(0).SetCellValue("");
            builder.SetColumnWidth(0, 5 * 255);

            row.CreateCell(1).SetCellValue("Megrendelő");
            builder.SetColumnWidth(1, 12 * 255);

            row.CreateCell(2).SetCellValue("Feladat");
            builder.SetColumnWidth(2, 60 * 255);

            row.CreateCell(3).SetCellValue("Részfeladat");
            builder.SetColumnWidth(3, 40 * 255);

            row.CreateCell(4).SetCellValue("Fejlesztő");
            builder.SetColumnWidth(4, 20 * 255);

            row.CreateCell(5).SetCellValue("terv");
            builder.SetColumnWidth(5, 20 * 255);

            return builder;
        }

        public static ISheet AddCard(this ISheet builder, string label, Card card, Member user)
        {
            var userName = "";
            if (user != null)
            {
                userName = user.FullName;
            }

			var row = builder.CreateRow(builder.LastRowNum + 1);
            
            row.CreateCell(0).SetCellValue("");
            
            row.CreateCell(1).SetCellValue(label);

            row.CreateCell(2).SetCellValue(card.Name);
            row.GetCell(2).CellStyle.WrapText = true;
            row.GetCell(2).Hyperlink = new HSSFHyperlink(HyperlinkType.URL) { Address = card.Url };

            row.CreateCell(3).SetCellValue("");
            
            row.CreateCell(4).SetCellValue(userName);
            
            row.CreateCell(5).SetCellValue("");

			return builder;
        }

		public static ISheet AddSubtasks(this ISheet builder, Card card)
		{
			foreach (var checklist in card.Checklists)
			{
				var row = builder.CreateRow(builder.LastRowNum + 1);

				row.CreateCell(0).SetCellValue(string.Empty);
				row.CreateCell(1).SetCellValue(string.Empty);
				row.CreateCell(2).SetCellValue(string.Empty);
				row.CreateCell(3).SetCellValue(checklist.Name);
				var font = builder.Workbook.CreateFont();
				font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
				row.GetCell(3).CellStyle = builder.Workbook.CreateCellStyle();
				row.GetCell(3).CellStyle.SetFont(font);

				row.CreateCell(4).SetCellValue(string.Empty);
				row.CreateCell(5).SetCellValue(string.Empty);
				
				foreach (var items in checklist.CheckItems)
				{
					var subrow = builder.CreateRow(builder.LastRowNum + 1);
					var name = "□ " + items.Name;
					if (items.Checked)
					{
						name = "✓ " + items.Name;
					}

					subrow.CreateCell(0).SetCellValue(string.Empty);
					subrow.CreateCell(1).SetCellValue(string.Empty);
					subrow.CreateCell(2).SetCellValue(string.Empty);
					subrow.CreateCell(3).SetCellValue(name);
					subrow.CreateCell(4).SetCellValue(string.Empty);
					subrow.CreateCell(5).SetCellValue(string.Empty);
				}
			}

			return builder;
		}

	    public static ISheet AddReports(this ISheet builder, List<Card> cards, List<List> lists, List<string> users)
        {
            var comparer = new CardComparer();
            var groupped = cards.GroupBy(c => c.Labels, comparer);
            //var rowNumber = 1;

            foreach (var group in groupped)
            {
                // write project name
                var name = "Egyéb";
                var key = group.Key.FirstOrDefault();
                if (key != null)
                {
                    name = key.Name;
                }

                var tempCards = group.ToList();
                foreach (var grouppedCard in tempCards)
                {
                    var actual = grouppedCard.RemoveUnusedMembers(users);
                    if (actual.Members.Count > 1)
                    {
                        foreach (var member in actual.Members)
                        {
                            builder.AddCard(name, actual, member);
	                        builder.AddSubtasks(actual);
							//rowNumber = rowNumber + 1;
                        }
                    }
                    else
                    {
                        builder.AddCard(name, actual, actual.Members.FirstOrDefault());
						builder.AddSubtasks(actual);
                        //rowNumber = rowNumber + 1;
                    }
                }
            }

            return builder;
        }

        public static byte[] CreateRepsonse(this IWorkbook workbook)
        {
            using (var ms = new MemoryStream())
            {
                workbook.Write(ms);
                return ms.ToArray();
            }
        }
    }

	public class NpoiExcelService : IReportService
    {
        public byte[] GenerateCardReports(List<Card> cards, List<List> lists, List<string> users)
        {
            // Opening the Excel template... 
            using (var fs = new MemoryStream())
            {
                var templateWorkbook = new HSSFWorkbook();
                var sheet = templateWorkbook.CreateSheet("Sheet1");
                sheet.CreateHeader().AddReports(cards, lists, users);
                sheet.ForceFormulaRecalculation = true;
                return templateWorkbook.CreateRepsonse();
            }
        }
    }
}