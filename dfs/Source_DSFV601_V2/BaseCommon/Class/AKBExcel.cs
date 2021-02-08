using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Printing;
using System.Linq;
using Microsoft.Win32;

namespace BaseCommon
{
    public class AkbExcelTemplateInfo
    {
        public int HeaderStartRow = 1;
        public int HeaderHeight = 1;

        public int DetailStartRow = 1;
        public int DetailHeight = 1;

        public int FooterStartRow = 1;
        public int FooterHeight = 1;

        public int StartRow = 1;
    }

    public class AkbExcelCellCoordinate
    {
        public int Row;
        public int Column;

        public AkbExcelCellCoordinate(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }

    /// <summary>
    /// AKB Excel Class
    /// </summary>
    public class AkbExcel
    {

        private const int XlTypePdf = 0;
        private const int XlQualityStandard = 0;

        private object _mobjExcel;
        private object _mobjWorkBooks;
        private object _mobjWorkBook;
        private object _mobjWorkSheets;
        private object _mobjWorkSheet;
        private object _mobjWorkSheet1;
        private object[] _parameters;
        private object _mobjRange;
        private object _mobjRange2;
        private object _mobjRange3;
        private string _mstrFile = "";
        private readonly object _missingObj = Type.Missing;

        // ▽ 2017/08/11 AKB Nguyen Thanh Tung --------------------------------
        public enum emXlHAlign
        {
            xlHAlignCenter =-4108,	            //Center.
            xlHAlignCenterAcrossSelection =	7,  //Center across selection.
            xlHAlignDistributed = -4117,        //Distribute.
            xlHAlignFill = 5,                   //Fill.
            xlHAlignGeneral = 1,                //Align according to data type.
            xlHAlignJustify = -4130,            //Justify.
            xlHAlignLeft = -4131,               //Left.
            xlHAlignRight =	-4152,              //Right.
        }

        public enum emBorder
        {
            xlDiagonalDown = 5,
            xlDiagonalUp = 6,
            xlEdgeBottom = 9,
            xlEdgeLeft = 7,
            xlEdgeRight = 10,
            xlEdgeTop = 8,
            xlInsideHorizontal = 12,
            xlInsideVertical = 11
        }

        public enum emLineStyle
        {
            xlContinuous = 1,
            xlDash = -4115,
            xlDashDot = 4,
            xlDashDotDot = 5,
            xlDot = -4118,
            xlDouble = -4119,
            xlLineStyleNone = -4142,
            xlSlantDashDot = 13
        }

        public enum emBorderWeight
        {
            xlHairline = 1,
            xlMedium = -4138,
            xlThick = 4,
            xlThin = 2
        }

        public enum emXlRgbColor
        {
            rgbSkyBlue = 15453831,
            rgbLightSkyBlue = 16436871,
        }

        public enum xlAutoFillType
        {
            xlFillCopy = 1,
            xlFillFormats = 3,
            xlFillDefault = 0,
            xlFillValues = 4,
        }

        private enum emXlCalculation
        {
            xlCalculationAutomatic = -4105,
            xlCalculationManual = -4135,
        }
        // △ 2017/08/11 AKB Nguyen Thanh Tung --------------------------------

        public AkbExcelTemplateInfo TemplateInfo = new AkbExcelTemplateInfo();

        public AkbExcel()
        {
            
        }
        
        #region Public method

        public void CopyHeaderTemplate(int row)
        {
            CopyRow(
                TemplateInfo.HeaderStartRow,
                TemplateInfo.HeaderStartRow + TemplateInfo.HeaderHeight - 1,
                row);
        }

        public void CopyDetailTemplate(int row)
        {
            CopyRow(
                TemplateInfo.DetailStartRow,
                TemplateInfo.DetailStartRow + TemplateInfo.DetailHeight - 1,
                row);
        }

        public void CopyFooterTemplate(int row)
        {
            CopyRow(
                TemplateInfo.FooterStartRow,
                TemplateInfo.FooterStartRow + TemplateInfo.FooterHeight - 1,
                row);
        }

        public void DeleteTemplate()
        {
            DeleteRows(1, TemplateInfo.StartRow - 1);
        }

        public void SetCellValue(AkbExcelCellCoordinate coor, object val)
        {
            SetValue(coor.Row, coor.Column, val);
        }

        public object GetCellValue(AkbExcelCellCoordinate coor)
        {
            return GetValue(coor.Row, coor.Column);
        }

        public void SetCellString(AkbExcelCellCoordinate coor, object val)
        {
            var cellVal = "'" + val;
            SetValue(coor.Row, coor.Column, cellVal);
        }

        public void InsertImage(AkbExcelCellCoordinate cell, Image image)
        {
            try
            {
                SetSelect(cell.Row, cell.Column);
                Clipboard.SetImage(image);
                PasteObject();
            }
            catch (Exception e)
            {
                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);
            }
        }
        
        public bool OpenExcel(string filename, bool blnVisible, bool blnReadyOnly, string temp)
        {
            bool flag = false;
            try
            {
                Type type = Type.GetTypeFromProgID("Excel.Application");

                if (type != null)
                {
                    _mobjExcel = Activator.CreateInstance(type);

                    _parameters = new object[1];
                    _parameters[0] = blnVisible;

                    //Set the Visible property 
                    SetProperty(_mobjExcel, "Visible", _parameters);
                    // ▽ 2017/08/16 AKB Nguyen Thanh Tung --------------------------------
                    _parameters[0] = false;
                    SetProperty(_mobjExcel, "UserControl", _parameters);
                    // △ 2017/08/16 AKB Nguyen Thanh Tung --------------------------------

                    _mobjWorkBooks = GetProperty(_mobjExcel, "Workbooks");

                    //Parameters = new object[15] { temp, mobjNullObj, mobjNullObj, mobjNullObj, mobjNullObj, mobjNullObj,
                    //                                mobjNullObj, mobjNullObj, mobjNullObj, mobjNullObj, mobjNullObj,
                    //                                mobjNullObj, mobjNullObj, mobjNullObj, mobjNullObj };

                    _parameters = new object[1];
                    _parameters[0] = temp;
                    InvokeMethod(_mobjWorkBooks, "Open", _parameters);

                    //get the workbook.  
                    _parameters = new object[1];
                    _parameters[0] = 1;
                    _mobjWorkBook = GetProperty(_mobjWorkBooks, "Item", _parameters);

                    //Get the worksheets collection.
                    _mobjWorkSheets = GetProperty(_mobjWorkBook, "Worksheets");

                    //Get the first worksheet.
                    _parameters = new object[1];
                    _parameters[0] = 1;
                    _mobjWorkSheet = GetProperty(_mobjWorkSheets, "Item", _parameters);

                    _mstrFile = filename;

                    // ▽ 2017/08/16 AKB Nguyen Thanh Tung --------------------------------
                    SetModeAppExcel(false);
                    // △ 2017/08/16 AKB Nguyen Thanh Tung --------------------------------
                }

                flag = true;
            }
            catch (Exception e)
            {
                // ▽ 2017/08/16 AKB Nguyen Thanh Tung --------------------------------
                CloseExcel();
                // △ 2017/08/16 AKB Nguyen Thanh Tung --------------------------------
                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);
            }

            return flag;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strNameSheet"></param>
        /// <returns></returns>
        public int CheckExitsSheets(string strNameSheet)
        {
            try
            {
                var Count = GetProperty(_mobjWorkSheets, "Count");
                var length = clsCommon.CnvNullToInt(Count);
                object objSheet = null;

                _parameters = new object[1];

                for (var i = 1; i <= length; i++)
                {

                    _parameters[0] = i;
                    objSheet = GetProperty(_mobjWorkSheets, "Item", _parameters);
                    var Name = GetProperty(objSheet, "Name");
                    MRComObject(ref objSheet);

                    if (Name.ToString().ToLower() == strNameSheet.ToLower()) return i;
                }
            }
            catch (Exception e)
            {
                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);
            }

            return -1;
        }

        public void ActiveSheet(int index)
        {
            try
            {
                ChangeSheet(index);
                GetProperty(_mobjWorkSheet, "Activate");
            }
            catch (Exception e)
            {
                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool ChangeSheet(int index)
        {
            try
            {
                _parameters = new object[1];
                _parameters[0] = index;
                _mobjWorkSheet = GetProperty(_mobjWorkSheets, "Item", _parameters);
                return true;
            }
            catch (Exception e)
            {
                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);
                return false;
            }
        }

        public bool SetNameSheet(string Name)
        {

            if (string.IsNullOrEmpty(Name) || CheckExitsSheets(Name) > -1) return false;

            try
            {
                _parameters = new object[1];
                _parameters[0] = Name;

                SetProperty(_mobjWorkSheet, "Name", _parameters);

                return true;
            }
            catch (Exception e)
            {
                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);
                return false;
            }
        }

        public bool AddNewSheet(string Name)
        {
            if (CheckExitsSheets(Name) > -1) return false;

            try
            {
                _parameters = new object[1];
                _parameters[0] = Name;

                _mobjWorkSheet = GetProperty(_mobjWorkSheets, "Add");
                SetProperty(_mobjWorkSheet, "Name", _parameters);

                return true;
            }
            catch (Exception e)
            {
                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);
                return false;
            }
            finally
            {
                FreeMemory();
            }
        }

        public int GetCountSheets()
        {
            try
            {

                return clsCommon.CnvNullToInt(GetProperty(_mobjWorkSheets, "Count"));

            }
            catch (Exception e)
            {
                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);
                return -1;
            }
        }

        public bool DeleteSheet(int indexSheet)
        {
            if (indexSheet < 1 || indexSheet > GetCountSheets()) return false;

            try
            {
                _parameters = new object[1];
                _parameters[0] = indexSheet;
                _mobjRange = GetProperty(_mobjWorkSheets, "Item", _parameters);
                GetProperty(_mobjRange, "Delete");

                return true;
            }
            catch (Exception e)
            {
                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);
                return false;
            }
        }

        public bool ActiveSheetClone(string Name)
        {
            if (CheckExitsSheets(Name) > -1) return false;

            try
            {
                _parameters = new object[2];
                _parameters[0] = _missingObj;
                _parameters[1] = _mobjWorkSheet;

                var flag = (bool) GetProperty(_mobjWorkSheet, "Copy", _parameters);

                if (!flag) return false;

                int CountSheet = GetCountSheets();
                ChangeSheet(CountSheet);

                _parameters = new object[1];
                _parameters[0] = Name;
                SetProperty(_mobjWorkSheet, "Name", _parameters);

                return true;
            }
            catch (Exception e)
            {
                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);
                return false;
            }
            finally
            {
                FreeMemory();
            }
        }

        public int GetLastRowHasValueOfColumn(int index)
        {
            var nameCol = GetExcelColumnName(index);
            return GetLastRowHasValueOfColumn(nameCol);
        }

        /// <summary>
        ///     Meno: Get index last rows has value of a column
        ///     Create by: 2017.10.13 AKB Nguyen Thanh Tung
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public int GetLastRowHasValueOfColumn(string strName)
        {

            var xlUp = -4162;

            try
            {
                _mobjRange = GetProperty(_mobjWorkSheet, "Rows");
                _mobjRange = GetProperty(_mobjRange, "Count");

                _parameters = new object[2];
                _parameters[0] = _mobjRange;
                _parameters[1] = strName;

                _mobjRange = GetProperty(_mobjWorkSheet, "Cells", _parameters);

                _parameters = new object[1];
                _parameters[0] = xlUp;
                _mobjRange = GetProperty(_mobjRange, "End", _parameters);
                _mobjRange = GetProperty(_mobjRange, "Row");

                return clsCommon.CnvNullToInt(_mobjRange);
            }
            catch (Exception e)
            {
                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);
            }
            finally
            {
                FreeMemory();
            }

            return -1;
        }

        /// <summary>
        ///     Meno: Get value range
        ///     Create by: 2017.10.13 AKB Nguyen Thanh Tung     
        /// </summary>
        /// <param name="strRange"></param>
        /// <returns></returns>
        public object[,] GetRangeValue(string strRange)
        {
            try
            {
                _parameters = new object[1];
                _parameters[0] = strRange;
                _mobjRange = GetProperty(_mobjWorkSheet, "Range", _parameters);
                _mobjRange = GetProperty(_mobjRange, "Value");

                return (_mobjRange as object [,]);
            }
            catch (Exception e)
            {
                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);
            }
            finally
            {
                FreeMemory();
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strNameSheet"></param>
        /// <returns></returns>
        public int GetRowsCount()
        {
            try
            {
                _mobjRange = InvokeMethod(_mobjWorkSheet, "UsedRange");
                _mobjRange2 = GetProperty(_mobjRange, "Rows");

                var rowsCount = GetProperty(_mobjRange2, "Count");
                return clsCommon.CnvNullToInt(rowsCount);
            }
            catch (Exception e)
            {
                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);
            }
            finally
            {
                FreeMemory();
            }

            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strNameSheet"></param>
        /// <returns></returns>
        public int GetColumnsCount()
        {
            try
            {
                _mobjRange = InvokeMethod(_mobjWorkSheet, "UsedRange");
                _mobjRange2 = GetProperty(_mobjRange, "Columns");

                var rowsCount = GetProperty(_mobjRange2, "Count");
                return clsCommon.CnvNullToInt(rowsCount);
            }
            catch (Exception e)
            {
                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);
            }
            finally
            {
                FreeMemory();
            }

            return -1;
        }

        /// <summary>
        ///     2017/08/16 AKB Nguyen Thanh Tung
        /// </summary>
        public void CloseExcel()
        {

            int hWnd = 0;
            bool blnHWnd = false;

            try
            {

                SetModeAppExcel(false);

                if (_mobjWorkBook != null)
                {
                    InvokeMethod(_mobjWorkBook, "Close");
                }

                if (_mobjExcel != null)
                {
                    try
                    {
                        hWnd = clsCommon.CnvNullToInt(GetProperty(_mobjExcel, "Hwnd"));
                        blnHWnd = true;
                    }
                    catch
                    {

                    }
                    ModePerformance(false);
                    InvokeMethod(_mobjExcel, "Quit");
                }

                MRComObject(ref _mobjRange);
                MRComObject(ref _mobjRange2);
                MRComObject(ref _mobjWorkSheet);
                MRComObject(ref _mobjWorkSheets);
                MRComObject(ref _mobjWorkBook);
                MRComObject(ref _mobjWorkBooks);
                MRComObject(ref _mobjExcel);
                _parameters = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
                //GC.WaitForPendingFinalizers();

                if (blnHWnd) SendMessage((IntPtr)hWnd, 0x10, IntPtr.Zero, IntPtr.Zero);
            }
            catch (Exception e)
            {

                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);

            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        // ▽ 2017/08/11 AKB Nguyen Thanh Tung --------------------------------
        //public void Save()
        public bool Save()
        // △ 2017/08/11 AKB Nguyen Thanh Tung --------------------------------
        {
            try
            {

                SetModeAppExcel(true);
                SetModeAppExcel(false);

                _parameters = new object[1];
                _parameters[0] = _mstrFile;
                // ブックを保存
                InvokeMethod(_mobjWorkBook, "SaveAs", _parameters);

                // ▽ 2017/08/11 AKB Nguyen Thanh Tung ----------------------------
                return true;
                // △ 2017/08/11 AKB Nguyen Thanh Tung ----------------------------
            }
            catch (Exception e)
            {

                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);

                // ▽ 2017/08/11 AKB Nguyen Thanh Tung ----------------------------
                return false;
                // △ 2017/08/11 AKB Nguyen Thanh Tung ----------------------------
            }
        }
        
        public void Print(int copyNumber = 1)
        {
            _parameters = new object[8];
            _parameters[0] = _missingObj;
            _parameters[1] = _missingObj;
            _parameters[2] = copyNumber;
            _parameters[3] = _missingObj;
            _parameters[4] = _missingObj;
            _parameters[5] = _missingObj;
            _parameters[6] = _missingObj;
            _parameters[7] = _missingObj;

            //Printing the sheet
            _mobjWorkBook.GetType().InvokeMember("PrintOut", BindingFlags.InvokeMethod,
            null,
            _mobjWorkBook,
            _parameters);

            //InvokeMethod(_mobjWorkBook, "PrintOut", _parameters);
        }

        public void ExportToPdf(string strSavePath, bool blnOpen)
        {

            bool paramOpenAfterPublish = blnOpen;
            bool paramIncludeDocProps = true;
            bool paramIgnorePrintAreas = false;

            try
            {

                if (_mobjWorkBook != null)
                {
                    _parameters = new object[9];
                    _parameters[0] = XlTypePdf;
                    _parameters[1] = strSavePath;
                    _parameters[2] = XlQualityStandard;
                    _parameters[3] = paramIncludeDocProps;
                    _parameters[4] = paramIgnorePrintAreas;
                    _parameters[5] = _missingObj;
                    _parameters[6] = _missingObj;
                    _parameters[7] = paramOpenAfterPublish;
                    _parameters[8] = _missingObj;

                    InvokeMethod(_mobjWorkBook, "ExportAsFixedFormat", _parameters);
                }
            }
            catch (Exception e)
            {

                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);

            }

        }

        /// <summary>
        ///     2017/08/11 AKB Nguyen Thanh Tung
        ///     Merge Cell
        /// </summary>
        /// <param name="blnMode">True - Merge/False - UnMerge</param>
        /// <param name="strRange">Range</param>
        public void SetMergeCell(bool blnMode, string strRange)
        {
            _mobjRange = null;

            try
            {
                _parameters = new object[1];
                _parameters[0] = strRange;
                _mobjRange = GetProperty(_mobjWorkSheet, "Range", _parameters);

                if (blnMode)
                {
                    _parameters[0] = Type.Missing;
                    InvokeMethod(_mobjRange, "Merge", _parameters);
                }
                else
                {
                    InvokeMethod(_mobjRange, "UnMerge");
                }

            }
            catch (Exception e)
            {

                XWriteLogs(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), e.Message);

            }
            finally
            {
                FreeMemory();
            }
        }

        /// <summary>
        ///     2017/08/11 AKB Nguyen Thanh Tung
        ///     Get name column from index column
        /// </summary>
        /// <param name="columnNumber"></param>
        /// <returns></returns>
        public string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

        public void ModePerformance(bool blnMode = true)
        {

            _parameters = new Object[1];

            try
            {
                if (!SetModeAppExcel(!blnMode)) return;
                
                _parameters[0] = !blnMode;

                _mobjRange = GetProperty(_mobjExcel, "ErrorCheckingOptions");
                SetProperty(_mobjRange, "BackgroundChecking", _parameters);

                try
                {
                    SetProperty(_mobjExcel, "PrintCommunication", _parameters);
                }
                catch { }

                if (_mobjWorkBook != null) {
                    SetProperty(_mobjWorkBook, "EnableAutoRecover", _parameters);
                    _parameters[0] = blnMode ? emXlCalculation.xlCalculationManual : emXlCalculation.xlCalculationAutomatic;
                    SetProperty(_mobjExcel, "Calculation", _parameters);
                }
            }
            catch (Exception e)
            {
                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);
            }
            finally
            {
                MRComObject(ref _mobjRange);
                _parameters = null;
            }
        }

        public void ActivePrinter(string printerName)
        {
            try
            {

                var lstPrinterInSystems = PrinterSettings.InstalledPrinters.Cast<string>().ToList();
                var selectedPrinter = string.Empty;

                //Check printer is installed on the system
                if (string.IsNullOrEmpty(printerName) || !lstPrinterInSystems.Contains(printerName))
                {
                    var printerDefault = new PrinterSettings();
                    selectedPrinter = printerDefault.IsValid ? printerDefault.PrinterName : string.Empty;
                } else
                {
                    selectedPrinter = printerName;
                }

                if (string.IsNullOrEmpty(selectedPrinter)) return;

                //Set printer
                var port = string.Empty;
                var path = @"Software\Microsoft\Windows NT\CurrentVersion\Devices";

                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(path))
                {
                    if (key != null)
                    {
                        object value = key.GetValue(selectedPrinter);
                        if (value != null)
                        {
                            string[] values = value.ToString().Split(',');
                            if (values.Length >= 2) port = values[1];
                        }
                    }
                }

                var currentPrinter = GetProperty(_mobjExcel, "ActivePrinter").ToString();

                if (!currentPrinter.StartsWith(selectedPrinter))
                {

                    //Get current concatenation string
                    var split = currentPrinter.Split(' ');

                    if (split.Length >= 3)
                    {

                        _parameters = new object[1];
                        _parameters[0] = string.Format("{0} {1} {2}",
                                                        selectedPrinter,
                                                        split[split.Length - 2],
                                                        port);

                        SetProperty(_mobjExcel, "ActivePrinter", _parameters);
                    }
                }
            }
            catch (Exception e)
            {
                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);
            }
            finally
            {
                _parameters = null;
            }
        }

        public void WriteFileCSV(List<List<object>> lstData, string filePath)
        {

            var csv = new System.Text.StringBuilder();

            foreach (var row in lstData)
            {

                var newLine = string.Join(",", row.ToArray());
                csv.AppendLine(newLine);

            }

            if (File.Exists(filePath)) File.Delete(filePath);
            File.WriteAllText(filePath, csv.ToString());

        }

        #region Work with Range

        /// <summary>
        ///     2017/08/11 AKB Nguyen Thanh Tung
        ///     Set All Boder for Range
        /// </summary>
        /// <param name="strRange">Range</param>
        public void SetBorderAllRange(string strRange, Color color, emLineStyle lineStyle = emLineStyle.xlContinuous, emBorderWeight weight = emBorderWeight.xlThin)
        {
            try
            {
                SetBorderRange(strRange, emBorder.xlEdgeTop, color, lineStyle, weight);
                SetBorderRange(strRange, emBorder.xlEdgeBottom, color, lineStyle, weight);
                SetBorderRange(strRange, emBorder.xlEdgeLeft, color, lineStyle, weight);
                SetBorderRange(strRange, emBorder.xlEdgeRight, color, lineStyle, weight);
                SetBorderRange(strRange, emBorder.xlInsideHorizontal, color, lineStyle, weight);
                SetBorderRange(strRange, emBorder.xlInsideVertical, color, lineStyle, weight);
            }
            catch (Exception e)
            {

                XWriteLogs(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), e.Message);

            }
            finally
            {
                FreeMemory();
            }
        }

        /// <summary>
        ///     2017/08/11 AKB Nguyen Thanh Tung
        ///     Set Border for Range
        /// </summary>
        /// <param name="strRange">Range</param>
        /// <param name="border">Border</param>
        public void SetBorderRange(string strRange, emBorder border, Color color, emLineStyle lineStyle = emLineStyle.xlContinuous, emBorderWeight weight = emBorderWeight.xlThin)
        {
            try
            {
                _parameters = new object[1];
                _parameters[0] = strRange;
                _mobjRange = GetProperty(_mobjWorkSheet, "Range", _parameters);

                _parameters[0] = border;
                _mobjRange2 = GetProperty(_mobjRange, "Borders", _parameters);

                _parameters[0] = weight;
                SetProperty(_mobjRange2, "Weight", _parameters);

                _parameters[0] = color;
                SetProperty(_mobjRange2, "Color", _parameters);

                _parameters[0] = lineStyle;
                SetProperty(_mobjRange2, "LineStyle", _parameters);
            }
            catch (Exception e)
            {

                XWriteLogs(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), e.Message);

            }
            finally
            {
                FreeMemory();
            }
        }

        /// <summary>
        ///     2017/08/11 AKB Nguyen Thanh Tung
        ///     Set Backgroup to Range
        /// </summary>
        /// <param name="strRange"></param>
        /// <param name="color"></param>
        public void SetBackgroundRange(string strRange, emXlRgbColor color)
        {
            try
            {
                _parameters = new object[1];

                _parameters[0] = strRange;
                _mobjRange = GetProperty(_mobjWorkSheet, "Range", _parameters);

                _mobjRange2 = GetProperty(_mobjRange, "Interior");

                _parameters[0] = color;
                SetProperty(_mobjRange2, "Color", _parameters);

            }
            catch (Exception e)
            {
                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);
            }
            finally
            {
                FreeMemory();
            }
        }

        /// <summary>
        ///     2017/08/11 AKB Nguyen Thanh Tung
        ///     Fill RangeSource to RangeFill
        /// </summary>
        /// <param name="rangeSource"></param>
        /// <param name="rangeFill"></param>
        /// <param name="fillType"></param>
        public void RangeAutoFill(string rangeSource, string rangeFill, xlAutoFillType fillType = xlAutoFillType.xlFillFormats)
        {
            try
            {
                _parameters = new object[1];

                _parameters[0] = rangeSource;
                _mobjRange = GetProperty(_mobjWorkSheet, "Range", _parameters);
                _parameters[0] = rangeFill;
                _mobjRange2 = GetProperty(_mobjWorkSheet, "Range", _parameters);

                _parameters = new object[2];
                _parameters[0] = _mobjRange2;
                _parameters[1] = fillType;
                InvokeMethod(_mobjRange, "AutoFill", _parameters);
            }
            catch (Exception e)
            {
                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);
            }
            finally
            {
                FreeMemory();
            }
        }

        public void RangeShrinkToFit(string strRange, bool blnShrinkToFit = true)
        {
            try
            {
                _parameters = new object[1];

                _parameters[0] = strRange;
                _mobjRange = GetProperty(_mobjWorkSheet, "Range", _parameters);

                _parameters[0] = blnShrinkToFit;
                SetProperty(_mobjRange, "ShrinkToFit", _parameters);
            }
            catch (Exception e)
            {
                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);
            }
            finally
            {
                FreeMemory();
            }
        }

        /// <summary>
        ///     2017/08/11 AKB Nguyen Thanh Tung
        /// </summary>
        /// <param name="rangeOld"></param>
        /// <param name="rangeNew"></param>
        public void RangeCut(string rangeOld, string rangeNew)
        {
            try
            {
                _parameters = new object[1];

                _parameters[0] = rangeOld;
                _mobjRange = GetProperty(_mobjWorkSheet, "Range", _parameters);
                _parameters[0] = rangeNew;
                _mobjRange2 = GetProperty(_mobjWorkSheet, "Range", _parameters);

                _parameters[0] = _mobjRange2;
                InvokeMethod(_mobjRange, "Cut", _parameters);
            }
            catch (Exception e)
            {
                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);
            }
            finally
            {
                FreeMemory();
            }
        }

        /// <summary>
        ///     2017/08/11 AKB Nguyen Thanh Tung
        /// </summary>
        /// <param name="strRange"></param>
        /// <param name="align"></param>
        public void SetAlignRange(string strRange, emXlHAlign align = emXlHAlign.xlHAlignCenter)
        {
            try
            {
                _parameters = new object[1];

                _parameters[0] = strRange;
                _mobjRange = GetProperty(_mobjWorkSheet, "Range", _parameters);

                _parameters[0] = align;
                SetProperty(_mobjRange, "HorizontalAlignment", _parameters);
            }
            catch (Exception e)
            {
                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);
            }
            finally
            {
                FreeMemory();
            }
        }

        /// <summary>
        ///     2017/08/11 AKB Nguyen Thanh Tung
        /// </summary>
        /// <param name="strRange"></param>
        /// <param name="width"></param>
        public void SetCoumnWidthRange(string strRange, int width)
        {
            try
            {
                _parameters = new object[1];

                _parameters[0] = strRange;
                _mobjRange = GetProperty(_mobjWorkSheet, "Range", _parameters);

                _parameters[0] = width;
                SetProperty(_mobjRange, "ColumnWidth", _parameters);
            }
            catch (Exception e)
            {
                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);
            }
            finally
            {
                FreeMemory();
            }
        }

        /// <summary>
        ///     2017/08/11 AKB Nguyen Thanh Tung
        /// </summary>
        /// <param name="cellStart"></param>
        /// <param name="objValue"></param>
        /// <returns></returns>
        public string SetRangeValue(AkbExcelCellCoordinate cellStart, object[,] objValue)
        {
            string strRange = string.Empty;

            try
            {
                if (objValue == null) return null;

                int countRow = objValue.GetLength(0) - 1;
                int countCol = objValue.GetLength(1) - 1;
                string NameColStart = GetExcelColumnName(cellStart.Column);
                string NameColEnd = GetExcelColumnName(cellStart.Column + countCol);
                int rowEnd = cellStart.Row + countRow;

                strRange = NameColStart + cellStart.Row + ":" + NameColEnd + rowEnd;

                _parameters = new Object[1];
                _parameters[0] = strRange;

                _mobjRange = GetProperty(_mobjWorkSheet, "Range", _parameters);
                SetProperty(_mobjRange, "Value2", objValue);
            }
            catch (Exception e)
            {
                strRange = string.Empty;
                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);
            }
            finally
            {
                FreeMemory();
            }

            return strRange;
        }

        /// <summary>
        ///     2017/08/11 AKB Nguyen Thanh Tung
        /// </summary>
        /// <param name="strRange"></param>
        /// <param name="objValue"></param>
        public void SetRangeValue(string strRange, object[,] objValue)
        {
            try
            {
                if (objValue == null) return;

                _parameters = new object[1];
                _parameters[0] = strRange;

                _mobjRange = GetProperty(_mobjWorkSheet, "Range", _parameters);
                SetProperty(_mobjRange, "Value2", objValue);
            }
            catch (Exception e)
            {
                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);
            }
            finally
            {
                MRComObject(ref _mobjRange);
            }
        }

        public void SetRangeFontBold(string strRange, bool bold)
        {
            try
            {
                if (string.IsNullOrEmpty(strRange)) return;

                _parameters = new object[1];
                _parameters[0] = strRange;

                _mobjRange = GetProperty(_mobjWorkSheet, "Range", _parameters);
                _mobjRange2 = GetProperty(_mobjRange, "Font");

                _parameters[0] = bold;

                SetProperty(_mobjRange2, "Bold", _parameters);
            }
            catch (Exception e)
            {
                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);
            }
            finally
            {
                MRComObject(ref _mobjRange);
            }
        }

        /// <summary>
        ///     AKB DucNH
        /// </summary>
        public void SetRowHeightRange(string strRange, int height)
        {
            try
            {
                _parameters = new object[1];

                _parameters[0] = strRange;
                _mobjRange = GetProperty(_mobjWorkSheet, "Range", _parameters);

                _parameters[0] = height;
                SetProperty(_mobjRange, "RowHeight", _parameters);
            }
            catch (Exception e)
            {
                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);
            }
            finally
            {
                FreeMemory();
            }
        }
        #endregion Work with Range

        #endregion

        #region Private methods

        private void FreeMemory()
        {
            MRComObject(ref _mobjRange);
            MRComObject(ref _mobjRange2);
            MRComObject(ref _mobjRange3);

            _parameters = null;
        }

        public bool SetModeAppExcel(bool blMode)
        {

            try
            {
                if (_mobjExcel == null) return false;

                _parameters = new Object[1];
                _parameters[0] = blMode;

                //Mode Binding Data
                SetProperty(_mobjExcel, "DisplayAlerts", _parameters);
                SetProperty(_mobjExcel, "Interactive", _parameters);
                SetProperty(_mobjExcel, "ScreenUpdating", _parameters);
                SetProperty(_mobjExcel, "EnableEvents", _parameters);

                return true;
            }
            catch (Exception e)
            {
                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);
                return false;
            }
        }

        public void SetSelect(int intRow, int intCol)
        {
            _mobjRange = null;

            try
            {

                _parameters = new Object[2];
                _parameters[0] = intRow;
                _parameters[1] = intCol;

                _mobjRange = GetProperty(_mobjWorkSheet, "Cells", _parameters);
                // ▽ 2017/08/31 AKB Nguyen Thanh Tung --------------------------------
                InvokeMethod(_mobjRange, "Select");
                //InvokeMethod(_mobjRange, "Activate");
                // △ 2017/08/31 AKB Nguyen Thanh Tung --------------------------------
            }
            catch (Exception e)
            {

                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);

            }
            finally
            {
                FreeMemory();
            }
        }

        public object SelectRows(int startRow, int endRow)
        {
            return GetProperty(_mobjWorkSheet, "Rows",
                new object[]
                {
                    string.Format("{0}:{1}", startRow, endRow)
                });
        }

        private void SelectObject(object obj)
        {
            _parameters = new Object[0];
            InvokeMethod(obj, "Select", _parameters);
        }

        private void CopyObject(object obj)
        {
            _parameters = new Object[0];
            InvokeMethod(obj, "Copy", _parameters);
        }

        private void DeleteObject(object obj)
        {
            _parameters = new Object[0];
            InvokeMethod(obj, "Delete", _parameters);
        }

        private void PasteObject()
        {
            _parameters = new Object[0];
            InvokeMethod(_mobjWorkSheet, "Paste", _parameters);
        }

        public void CopyRow(int fromRow, int toRow, int pasteTo)
        {
            try
            {

                var mobjRow = SelectRows(fromRow, toRow);
                SelectObject(mobjRow);
                CopyObject(mobjRow);

                mobjRow = SelectRows(pasteTo, pasteTo);
                SelectObject(mobjRow);
                PasteObject();

            }
            catch (Exception e)
            {

                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);

            }
        }

        public void InsertImage(int row, int col, Image image)
        {
            try
            {

                SetSelect(row, col);
                Clipboard.SetImage(image);
                PasteObject();

            }
            catch (Exception e)
            {

                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);

            }
        }

        public void DeleteRows(int fromRow, int toRow)
        {
            try
            {

                var mobjRow = SelectRows(fromRow, toRow);
                SelectObject(mobjRow);
                DeleteObject(mobjRow);

            }
            catch (Exception e)
            {

                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);

            }
        }

        private bool XWriteLogs(string strNameFunction, string strContentErr)
        {
            try
            {
                if (!string.IsNullOrEmpty(strNameFunction) && !string.IsNullOrEmpty(strContentErr))
                {
                    //Remove new line
                    strContentErr = strContentErr.Replace("\n", String.Empty);
                    strContentErr = strContentErr.Replace("\r", String.Empty);

                    string strPath = "C:/Log/";
                    string strLogs = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").Trim()
                        + ", " + strNameFunction + ", " + strContentErr;

                    strNameFunction = null;
                    strContentErr = null;

                    if (!Directory.Exists(strPath))
                    {
                        Directory.CreateDirectory(strPath);
                    }

                    strPath = strPath + "log.txt";

                    if (!File.Exists(strPath))
                    {
                        File.CreateText(strPath).Close();
                    }

                    FileStream fsFile = new FileStream(strPath, FileMode.Append, FileAccess.Write);
                    StreamWriter swWrite = new StreamWriter(fsFile);
                    swWrite.WriteLine(strLogs);
                    swWrite.Close();
                    fsFile.Close();
                    swWrite = null;
                    fsFile = null;
                }

                return true;
            }
            catch
            {
                return false;
            }

        }

        #endregion

        #region Refactor objects

        private object GetProperty(object obj, string strProperty)
        {
            return obj.GetType().InvokeMember(strProperty, BindingFlags.GetProperty, null, obj, null);
        }

        private object GetProperty(object obj, string strProperty, object[] parameters)
        {
            return obj.GetType().InvokeMember(strProperty, BindingFlags.GetProperty, null, obj, parameters);
        }

        private void SetProperty(object obj, string strProperty, object oValue)
        {
            _parameters = new object[1];
            _parameters[0] = oValue;
            obj.GetType().InvokeMember(strProperty, BindingFlags.SetProperty, null, obj, _parameters);
        }

        private void SetProperty(object obj, string strProperty, object[] parameters)
        {

            obj.GetType().InvokeMember(strProperty, BindingFlags.SetProperty, null, obj, parameters);
        }

        private object InvokeMethod(object obj, string strProperty, object[] oParam)
        {
            return obj.GetType().InvokeMember(strProperty, BindingFlags.InvokeMethod, null, obj, oParam);
        }

        private object InvokeMethod(object obj, string strProperty)
        {
            return obj.GetType().InvokeMember
            (strProperty, BindingFlags.InvokeMethod, null, obj, null);
        }

        private void MRComObject(ref object objCom)
        {
            //COM オブジェクトの使用後、明示的に COM オブジェクトへの参照を解放する
            try
            {
                if (objCom != null)
                {
                    //提供されたランタイム呼び出し可能ラッパーの参照カウントをデクリメントします
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(objCom);
                }
            }
            catch (Exception e)
            {

                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);

            }
            finally
            {
                //参照を解除する
                objCom = null;
            }
        }

        public void SetValue(int intRow, int intCol, object objValue)
        {
            try
            {
                _parameters = new Object[2];
                _parameters[0] = intRow;
                _parameters[1] = intCol;

                _mobjRange = GetProperty(_mobjWorkSheet, "Cells", _parameters);
                SetProperty(_mobjRange, "Value", objValue);

            }
            catch (Exception e)
            {
                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);
            }
        }

        public object GetValue(int intRow, int intCol)
        {
            try
            {
                _parameters = new Object[2];
                _parameters[0] = intRow;
                _parameters[1] = intCol;

                _mobjRange = GetProperty(_mobjWorkSheet, "Cells", _parameters);
                return GetProperty(_mobjRange, "Value");

            }
            catch (Exception e)
            {
                XWriteLogs(MethodBase.GetCurrentMethod().Name, e.Message);
                return null;
            }
        }

        #endregion

    }

}
