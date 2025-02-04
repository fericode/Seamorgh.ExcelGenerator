﻿using ExcelHelper.ReportObjects;
using ExcelHelper.Reports.ExcelReports.PropertyOptions;
using System.Collections.Generic;
using System.Drawing;

namespace ExcelHelper.Reports.ExcelReports.Template
{
    public static class TableTemplates
    {
        public static Table AccountHeader(CellLocation startCellLocation)
        {
            Table table = new();
            ExcelReportBuilder builder = new();
            Border border = new(LineStyle.Thick, Color.Black);
            var row = builder.AddRow(new List<string> { "نام حساب", "کد حساب" }, new RowPropertyOptions(startCellLocation));
            var location = row.NextVerticalCellLocation;
            var emtyrow = builder.EmptyRows(new List<string> { "", "" }, new RowPropertyOptions(location));
            // location = emtyrow.LastOrDefault().NextHorizontalLocation;

            row.BackgroundColor = Color.DarkBlue;
            row.FontColor = Color.White;
            //row.MergedCellsList.Add("A17:A18");
            //row.MergedCellsList.Add("B17:B18");
            table.MergedCells.Add("A17:A18");
            table.MergedCells.Add("B17:B18");
            row.AllBorder = border;
            row.OutsideBorder = border;
            table.Rows.Add(row);
            table.Rows.AddRange(emtyrow);
            return table;
        }

        public static Table Multiplex(List<SummaryAccount> summary, CellLocation currentCellLocation)
        {
            Table table = new();
            List<string> Sumcells = new();
            ExcelReportBuilder builder = new();
            Border border = new(LineStyle.Thick, Color.Black);
            Cell sumColumn;
            var row = builder.AddRow(summary, new RowPropertyOptions(currentCellLocation), 2);
            currentCellLocation = row.NextVerticalCellLocation;
            row.BackgroundColor = Color.DarkBlue;
            Row childrow=new();
            row.FontColor = Color.White;
            row.AllBorder = border;
            row.OutsideBorder = border;
            table.Rows.Add(row);
            foreach (var item in summary)
            {

                foreach (var result in item.Multiplex)
                {
                    var header = builder.AddRow(new List<string> { "قبل از تسهیم", "بعد از تسهیم", "جمع" }, new RowPropertyOptions(currentCellLocation));
                    table.Rows.Add(header);
                    currentCellLocation = header.NextVerticalCellLocation;
                    childrow = builder.AddRow(item.Multiplex, new RowPropertyOptions(currentCellLocation));
                    row.BackgroundColor = Color.DarkBlue;
                    row.FontColor = Color.White;
                    header.BackgroundColor = Color.DarkBlue;
                    header.FontColor = Color.White;

                    // Add Cell For Formulas
                    childrow.Formulas = $"=sum({childrow.GetCell(childrow.StartCellLocation.X).CellLocation.GetName()}:{childrow.GetCell(childrow.EndCellLocation.X).CellLocation.GetName()})";
                    sumColumn = childrow.AddCell();
                    sumColumn.CellType = CellType.Formula;
                    sumColumn.Value = childrow.Formulas;
                    Sumcells.Add(sumColumn.CellLocation.GetName());
                    ////////

                    table.Rows.Add(childrow);
                    currentCellLocation = new CellLocation(childrow.NextHorizontalCellLocation.X, header.EndCellLocation.Y);
                    var avgtitle = header.AddCell();
                    avgtitle.Value = "میانگین";
                }
                var avg = childrow.AddCell();
                string avgstr = "=(";
                for (int i = 0; i < Sumcells.Count; i++)
                {
                    avgstr += Sumcells[i];
                    if (i < Sumcells.Count-1)
                        avgstr += "+";
                }
                avgstr += ")/"+ Sumcells.Count + "";
                avg.Value = avgstr;
                avg.CellType = CellType.Formula;
                
            }
            
            return table;
        }

        public static Table Accounts(List<AccountDto> accounts, CellLocation currentCellLocation)
        {
            Table table = new();
            ExcelReportBuilder builder = new();
            Border border = new(LineStyle.Thick, Color.Black);

            var childrow = builder.AddTable(accounts, new TablePropertyOptions(currentCellLocation));
            table = childrow;
            table.InlineBorder = border;
            table.OutsideBorder = border;
            return table;
        }
    }
}
