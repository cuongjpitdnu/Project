using ClosedXML.Excel;
using System.Collections.Generic;
using System.Drawing;
using System;
using System.IO;
using System.Data;

namespace GPMain.Common.Excel
{
    public enum EnumAlignmentHorizontal
    {
        Center = 0,
        CenterContinuous = 1,
        Distributed = 2,
        Fill = 3,
        General = 4,
        Justify = 5,
        Left = 6,
        Right = 7
    }
    public enum EnumAlignmentVertical
    {
        Bottom = 0,
        Center = 1,
        Distributed = 2,
        Justify = 3,
        Top = 4
    }

    public class ExcelCommon : IDisposable
    {
        protected XLWorkbook _objWorkbook;
        protected IXLWorksheet _objWorksheet;
        private XLBorderStyleValues _borderType = XLBorderStyleValues.Thin;
        private string _pathFileToSave;

        public void CreateWorkbook(string tempPath, string pathFile)
        {
            _pathFileToSave = pathFile;
            if (string.IsNullOrEmpty(_pathFileToSave))
            {
                _pathFileToSave = Guid.NewGuid() + ".xlsx";
            }

            LoadOptions loadOptions = new LoadOptions() { EventTracking = XLEventTracking.Disabled, RecalculateAllFormulas = false };

            if (File.Exists(tempPath))
            {
                _objWorkbook = new XLWorkbook(tempPath, loadOptions);
                _objWorkbook.CalculateMode = XLCalculateMode.Manual;
                _objWorkbook.CalculationOnSave = false;
                _objWorksheet = _objWorkbook.Worksheet(1);
            }
            else
            {
                _objWorkbook = new XLWorkbook(loadOptions);
                _objWorkbook.CalculateMode = XLCalculateMode.Manual;
                _objWorkbook.CalculationOnSave = false;
            }
        }


        public void Save()
        {
            _objWorkbook.SaveAs(_pathFileToSave, false);
        }

        XLAlignmentHorizontalValues GetXLAlignmentHorizontal(EnumAlignmentHorizontal alignmentHorizontal)
        {
            return (XLAlignmentHorizontalValues)alignmentHorizontal;
        }
        XLAlignmentVerticalValues GetXLAlignmentVertical(EnumAlignmentVertical alignmentVertical)
        {
            return (XLAlignmentVerticalValues)alignmentVertical;
        }

        public void WriteList<T>(List<T> data, int row, int col)
        {
            SetValue(row, col, data);
        }
        public void AddWorkSheet(string nameWorksheet)
        {
            _objWorksheet = _objWorkbook.Worksheets.Add(nameWorksheet);
        }
        public void SetNameWorkSheet(string newNameWorksheet)
        {
            _objWorksheet.Name = newNameWorksheet;
        }
        #region Merge Cell
        public void MergeCell(int startRow, int endRow, int startColumn, int endColumn)
        {
            _objWorksheet.Range(startRow, startColumn, endRow, endColumn).Merge(false);
        }
        public void MergeColumns(int row, int startColumn, int endColumn)
        {
            MergeCell(row, row, startColumn, endColumn);
        }
        public void MergeCells(int column, int startRow, int endRow)
        {
            MergeCell(startRow, endRow, column, column);
        }

        #endregion
        #region SetColor
        public void SetCellBackColor(int row, int column, Color backColor, Color foreColor)
        {
            _objWorksheet.Cell(row, column).Style.Fill.SetPatternColor(XLColor.FromColor(foreColor));
            _objWorksheet.Cell(row, column).Style.Fill.SetBackgroundColor(XLColor.FromColor(backColor));
        }
        public void SetRowBackColor(int row, int startColumn, int endColumn, Color backColor, Color foreColor)
        {
            for (int col = startColumn; col <= endColumn; col++)
            {
                SetCellBackColor(row, col, backColor, foreColor);
            }
        }
        public void SetColumnBackColor(int column, int startRow, int endRow, Color backColor, Color foreColor)
        {
            for (int row = startRow; row <= endRow; row++)
            {
                SetCellBackColor(row, column, backColor, foreColor);
            }
        }
        #endregion
        #region Set Alignment
        public void SetHorizontalAlignmentOfCell(int row, int column, EnumAlignmentHorizontal alignment)
        {
            _objWorksheet.Cell(row, column).Style.Alignment.Horizontal = GetXLAlignmentHorizontal(alignment);

        }
        public void SetVerticalAlignmentOfCell(int row, int column, EnumAlignmentVertical alignment)
        {
            _objWorksheet.Cell(row, column).Style.Alignment.Vertical = GetXLAlignmentVertical(alignment);
        }
        public void SetHorizontalAlignmentOfRange(int startRow, int endRow, int startColumn, int endColumn, EnumAlignmentHorizontal alignment)
        {
            _objWorksheet.Range(startRow, startRow, endRow, endColumn).Style.Alignment.Horizontal = GetXLAlignmentHorizontal(alignment);
        }
        public void SetHorizontalAlignmentOfRange(string nameRange, EnumAlignmentHorizontal alignment)
        {
            _objWorksheet.NamedRange(nameRange).Ranges.Style.Alignment.Horizontal = GetXLAlignmentHorizontal(alignment);
        }
        public void SetVerticalAlignmentOfRange(int startRow, int endRow, int startColumn, int endColumn, EnumAlignmentVertical alignment)
        {
            _objWorksheet.Range(startRow, startRow, endRow, endColumn).Style.Alignment.Vertical = GetXLAlignmentVertical(alignment);
        }
        public void SetVerticalAlignmentOfRange(string nameRange, EnumAlignmentVertical alignment)
        {
            _objWorksheet.NamedRange(nameRange).Ranges.Style.Alignment.Vertical = GetXLAlignmentVertical(alignment);
        }
        #endregion
        #region Set Visible
        public void VisibleColumn(int column, bool visible)
        {
            if (visible)
            {
                _objWorksheet.Column(column).Unhide();
            }
            else
            {
                _objWorksheet.Column(column).Hide();
            }
        }
        public void VisibleRow(int row, bool visible)
        {
            if (visible)
            {
                _objWorksheet.Row(row).Unhide();
            }
            else
            {
                _objWorksheet.Row(row).Hide();
            }
        }
        public bool VisibleGridLine
        {
            set
            {
                _objWorksheet.ShowGridLines = value;
            }
        }
        #endregion
        #region Set Indent
        public void IndentCell(int row, int column, int indent)
        {
            _objWorksheet.Cell(row, column).Style.Alignment.Indent = indent;
        }
        public void IndentColumn(int column, int indent)
        {
            _objWorksheet.Column(column).Style.Alignment.Indent = indent;
        }
        public void IndentRow(int row, int indent)
        {
            _objWorksheet.Row(row).Style.Alignment.Indent = indent;
        }
        #endregion
        #region Get, Set Value
        public string GetValue(int row, int column)
        {
            return _objWorksheet.Cell(row, column).Value.ToString();
        }
        public void SetValue(int row, int column, object value)
        {
            _objWorksheet.Cell(row, column).Value = value;
        }

        public void SetHeader(int row, string[] columns)
        {
            int col = 1;
            foreach (string colName in columns)
            {
                _objWorksheet.Cell(row, ++col).Value = colName;
            }
        }

        public void InsertDataTable(int rowStart, int colStart, DataTable table)
        {
            _objWorksheet.Cell(rowStart, colStart).InsertTable(table.AsEnumerable(), false);
        }
        #endregion
        public void SetBorderRangeCell(int startRow, int endRow, int startColumn, int endColumn)
        {
            _objWorksheet.Range(startRow, startColumn, endRow, endColumn)
                         .Style
                         .Border.SetTopBorder(_borderType)
                         .Border.SetRightBorder(_borderType)
                         .Border.SetBottomBorder(_borderType)
                         .Border.SetLeftBorder(_borderType);
        }
        public void SetBoldStyleRangeCell(int startRow, int endRow, int startColumn, int endColumn)
        {
            _objWorksheet.Range(startRow, startColumn, endRow, endColumn).Style.Font.Bold = true;
        }

        public void SetAutoRowHeight(int row)
        {
            _objWorksheet.Row(row).Style.Alignment.WrapText = true;
        }
        public void SetColumnAutoHeight(int column)
        {
            _objWorksheet.Column(column).Style.Alignment.WrapText = true;
        }
        public void SetAutoWidthColumn(int column, int startRow, int endRow, int minWidth, int maxWidth)
        {
            _objWorksheet.Column(column).AdjustToContents(startRow, endRow, minWidth, maxWidth);
        }
        public void SetAutoWidthColumn(int column)
        {
            _objWorksheet.Column(column).AdjustToContents();
        }
        public void SetAutoWidthRangeColumn(int startColumn, int endColumn)
        {
            for (int col = startColumn; col <= endColumn; col++)
            {
                SetAutoWidthColumn(col);
            }
        }
        #region Dispose
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ExcelCommon()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
