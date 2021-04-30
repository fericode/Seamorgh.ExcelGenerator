﻿using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ExcelHelper.Reports.ExcelReports
{
    public class Table
    {
        public DataTable Data { get; set; }
        public List<Row> Rows { get; set; } = new();
        public Location StartLocation { get; set; }
        public Location EndLocation { get; set; }
        public Border InLineBorder { get; set; }
        public Border OutLineBorder { get; set; }
        public bool IsBordered { get; set; }
        public List<string> MergedCells { get; set; } = new();
        public int RowsCount => Rows.Count;

        public Location NextHorizontalLocation
        {
            get
            {
                var y = Rows.LastOrDefault().EndLocation.Y - (Rows.LastOrDefault().EndLocation.Y - Rows.LastOrDefault().StartLocation.Y);
                return new Location(Rows.LastOrDefault().EndLocation.X + 1, y);
            }
        }
        public Location NextVerticalLocation
        {
            get
            {
                var x = Rows.LastOrDefault().EndLocation.X - (Rows.LastOrDefault().EndLocation.X - Rows.LastOrDefault().StartLocation.X);
                return new Location(x, Rows.LastOrDefault().EndLocation.Y + 1);
            }
        }

        public Column GetColumn(Location location)
        {
            return Rows[location.X - 1].Columns[location.Y - 1];
        }

        public List<Column> GetColumns(Location startLocation, Location endLocation)
        {
            List<Column> columns = new();
            for (int i = startLocation.Y; i < endLocation.Y; i++)
            {
                columns.Add(GetColumn(new Location(startLocation.X, i)));
            }

            return columns;
        }
    }
}
