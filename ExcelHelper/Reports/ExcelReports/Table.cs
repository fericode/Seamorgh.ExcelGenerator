﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;

namespace ExcelHelper.Reports.ExcelReports
{
    public class Table : IValidatableObject
    {
        public Table()
        {
            InlineBorder = new Border(LineStyle.None, Color.Black);
            OutsideBorder = new Border(LineStyle.None, Color.Black);
        }
        public List<Row> Rows { get; set; } = new();
        public CellLocation StartCellLocation
        {
            get
            {
                return Rows.FirstOrDefault().StartCellLocation; ;
            }
        }  //TODO: Discuss with Shahab. The Rows has StartLocation itself, which one should be considered?
        //TODO: StartLocation and EndLocation for Table model are critical and should exist and be exact to create desired result
        public CellLocation EndCellLocation
        {
            get
            {
                return Rows.LastOrDefault().EndCellLocation; ;
            }

        } //TODO: above question
        public Border InlineBorder { get; set; } = new(LineStyle.None, Color.Black);//TODO: What it is? Inside border can be set on cells or columns or rows
        public Border OutsideBorder { get; set; } = new Border(LineStyle.None, Color.Black);
        public bool IsBordered { get; set; } //TODO? What is this? isn't it the default one?
        public List<string> MergedCells { get; set; } = new();
        public int RowsCount => Rows.Count;

        public CellLocation NextHorizontalCellLocation
        {
            get
            {
                var y = Rows.LastOrDefault().EndCellLocation.Y - (Rows.LastOrDefault().EndCellLocation.Y - Rows.LastOrDefault().StartCellLocation.Y);
                return new CellLocation(Rows.LastOrDefault().EndCellLocation.X + 1, y);
            }
        }
        public CellLocation NextVerticalCellLocation
        {
            get
            {
                var x = Rows.LastOrDefault().EndCellLocation.X - (Rows.LastOrDefault().EndCellLocation.X - Rows.LastOrDefault().StartCellLocation.X);
                return new CellLocation(x, Rows.LastOrDefault().EndCellLocation.Y + 1);
            }
        }

        public Cell GetCell(CellLocation cellLocation)
        {
            return Rows[cellLocation.X - 1].Cells[cellLocation.Y - 1];
        }

        public List<Cell> GetCells(CellLocation startCellLocation, CellLocation endCellLocation)
        {
            List<Cell> cells = new();
            for (int i = startCellLocation.Y; i < endCellLocation.Y; i++)
            {
                cells.Add(GetCell(new CellLocation(startCellLocation.X, i)));
            }

            return cells;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (false)
                yield return new ValidationResult("");
            // TODO: Discuess with Shahab. Shouldn't Rows in a Table have common features like Same StartLocation.X and things like
        }
    }
}
