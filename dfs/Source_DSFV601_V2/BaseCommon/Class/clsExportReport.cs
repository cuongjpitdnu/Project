using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.IO;
using static BaseCommon.clsConst;

namespace BaseCommon
{
    public class clsExportReport : IDisposable
    {
        public const int ROW_START_ALARM = 10;
        public const int ROW_START_WALKING = 20;
        public const int ROW_START_WALKING_LIMIT = 11;

        // Cell Address Measure Info
        public const string REPORT_DATE = "E3";
        public const string MEASURE_START = "C4";
        public const string MEASURE_END = "C5";
        public const string MEASURE_TYPE = "E4";
        public const string MEASURE_RESULT = "E5";
        public const string DEVICE_NAME = "E6";
        public const string ALARM_VALUE = "C6";  // Report Alarm Test
        public const string FAIL_LEVEL = "C6";   // Report Walking Test
        public const string PERIOD = "C7";       // Report Walking Test

        // Column Address Measure Detail
        public const string NO = "B";
        public const string TIME = "C";
        public const string VALUE = "D";
        public const string RESULT = "E";

        // Excel
        ExcelPackage _package;
        ExcelWorksheet _excelWorksheet;

        public clsExportReport(string pathReport)
        {
            if (string.IsNullOrEmpty(pathReport) || !File.Exists(pathReport))
            {
                return;
            }

            _package = new ExcelPackage(new FileInfo(pathReport));
            _excelWorksheet = _package.Workbook.Worksheets[1];
        }

        public bool WriteMeasureInfo(MeasureInfo measureInfo, bool isNoMysql = true)
        {
            if (_package == null || _excelWorksheet == null)
            {
                return false;
            }

            try
            {
                _excelWorksheet.Cells[REPORT_DATE].Value = measureInfo.ReportDate?.ToString(cstrDateFomatShow);
                _excelWorksheet.Cells[MEASURE_START].Value = measureInfo.MeasureStart?.ToString(cstrDateTimeFomatShow);
                _excelWorksheet.Cells[MEASURE_END].Value = measureInfo.MeasureEnd?.ToString(cstrDateTimeFomatShow);
                _excelWorksheet.Cells[MEASURE_TYPE].Value = measureInfo.MeasureType == emMeasureType.AlarmTest ? ALARM_TEST : WALKING_TEST;
                _excelWorksheet.Cells[MEASURE_RESULT].Value = measureInfo.MeasureResult;
                _excelWorksheet.Cells[ALARM_VALUE].Value = measureInfo.AlarmValue;
                _excelWorksheet.Cells[DEVICE_NAME].Value = measureInfo.DeviceName;

                if (measureInfo.MeasureType == emMeasureType.WalkingTest)
                {
                    _excelWorksheet.Cells[FAIL_LEVEL].Value = measureInfo.FailLevel;
                    _excelWorksheet.Cells[PERIOD].Value = measureInfo.Period;
                }

                if (!isNoMysql)
                {
                    _excelWorksheet.InsertRow(4, 1, 7);
                    _excelWorksheet.Cells["B4"].Value = "Measure Id";
                    _excelWorksheet.Cells["C4"].Value = measureInfo.MeasureId;
                    _excelWorksheet.Cells["D4"].Value = "User Name";
                    _excelWorksheet.Cells["E4"].Value = measureInfo.UserName;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool WriteMeasureDetail(int rowStart, MeasureDetail measureDetail, emMeasureType MeasureType)
        {
            if (_package == null || _excelWorksheet == null)
            {
                return false;
            }

            try
            {
                _excelWorksheet.Cells[NO + rowStart].Value = measureDetail.No;
                _excelWorksheet.Cells[TIME + rowStart].Value = measureDetail.Time?.ToString(cstrDateTimeFomatShow);
                _excelWorksheet.Cells[VALUE + rowStart].Value = measureDetail.Value;

                var range = string.Format("{0}{2}:{1}{2}", NO, RESULT, rowStart.ToString());

                if (MeasureType == emMeasureType.AlarmTest)
                {
                    _excelWorksheet.Cells[RESULT + rowStart].Value = measureDetail.Result;
                }
                else
                {
                    range = string.Format("{0}{2}:{1}{2}", NO, VALUE, rowStart.ToString());
                }

                _excelWorksheet.Cells[range].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                _excelWorksheet.Cells[range].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                _excelWorksheet.Cells[range].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                _excelWorksheet.Cells[range].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (_package != null)
                    {
                        _package.Save();
                        _package.Dispose();
                        _package = null;
                    }

                    if (_excelWorksheet != null)
                    {
                        _excelWorksheet.Dispose();
                        _excelWorksheet = null;
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~clsExportReport() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
