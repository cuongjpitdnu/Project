using ClosedXML.Excel;
using GPModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMain.Common.Excel
{
    public class ExportExcelForFamilyPopup : ExcelCommon
    {
        public bool ExportBirthDayFamily(List<InfoBirthDayFamilyForExportExcel> ListMember, string tempPath, string pathFileToSave, string title)
        {
            CreateWorkbook(tempPath, pathFileToSave);//khởi tạo workbook
            SetNameWorkSheet(title);
            SetValue((int)FamilyPopupExcelEntity.Rows.Title, (int)FamilyPopupExcelEntity.ColumnsTableBirthDay.STT, title);
            SetValue((int)FamilyPopupExcelEntity.Rows.DateCreate, (int)FamilyPopupExcelEntity.ColumnsTableBirthDay.STT, $"Ngày xuất bản: {DateTime.Now.ToString("HH:mm dd/MM/yyyy")}");

            string[] columns = Enum.GetNames(typeof(FamilyPopupExcelEntity.ColumnsTableBirthDay));
            int colIdx = 1;
            foreach (string col in columns)
            {
                string colName = FamilyPopupExcelEntity.NameColumnsTableBirthDay[col];
                SetValue((int)FamilyPopupExcelEntity.Rows.HeadTable, colIdx, colName);
                SetCellBackColor((int)FamilyPopupExcelEntity.Rows.HeadTable, colIdx, Color.FromArgb(198, 224, 180), Color.Black);
                colIdx++;
            }

            MergeColumns((int)FamilyPopupExcelEntity.Rows.Title, 1, columns.Length);
            MergeColumns((int)FamilyPopupExcelEntity.Rows.DateCreate, 1, columns.Length);

            SetBorderRangeCell((int)FamilyPopupExcelEntity.Rows.Title, (int)FamilyPopupExcelEntity.Rows.Title, 1, columns.Length);

            SetValue((int)FamilyPopupExcelEntity.Rows.DataStart, (int)FamilyPopupExcelEntity.ColumnsTableBirthDay.STT, ListMember);

            SetBorderRangeCell((int)FamilyPopupExcelEntity.Rows.HeadTable, (int)FamilyPopupExcelEntity.Rows.DataStart + ListMember.Count - 1, (int)FamilyPopupExcelEntity.ColumnsTableBirthDay.STT, (int)FamilyPopupExcelEntity.ColumnsTableBirthDay.Date);
            SetAutoWidthRangeColumn((int)FamilyPopupExcelEntity.ColumnsTableBirthDay.STT, (int)FamilyPopupExcelEntity.ColumnsTableBirthDay.Date);
            SetAutoWidthColumn((int)FamilyPopupExcelEntity.ColumnsTableBirthDay.Name, (int)FamilyPopupExcelEntity.Rows.DataStart, (int)FamilyPopupExcelEntity.Rows.DataStart + ListMember.Count, 18, 100);
            SetAutoWidthColumn((int)FamilyPopupExcelEntity.ColumnsTableBirthDay.Date, (int)FamilyPopupExcelEntity.Rows.DataStart, (int)FamilyPopupExcelEntity.Rows.DataStart + ListMember.Count, 18, 100);
            Save();
            File.Delete(tempPath);
            return true;
        }
        public bool ExportDeadDayFamily(List<InfoDeadDayFamilyForExportExcel> ListMember, string tempPath, string pathFileToSave, string title)
        {
            CreateWorkbook(tempPath, pathFileToSave);//khởi tạo workbook
            SetNameWorkSheet(title);
            SetValue((int)FamilyPopupExcelEntity.Rows.Title, (int)FamilyPopupExcelEntity.ColumnsTableDeadDay.STT, title);
            SetValue((int)FamilyPopupExcelEntity.Rows.DateCreate, (int)FamilyPopupExcelEntity.ColumnsTableDeadDay.STT, $"Ngày xuất bản: {DateTime.Now.ToString("HH:mm dd/MM/yyyy")}");

            string[] columns = Enum.GetNames(typeof(FamilyPopupExcelEntity.ColumnsTableDeadDay));
            int colIdx = 1;
            foreach (string col in columns)
            {
                string colName = FamilyPopupExcelEntity.NameColumnsTableDeadDay[col];
                SetValue((int)FamilyPopupExcelEntity.Rows.HeadTable, colIdx, colName);
                SetCellBackColor((int)FamilyPopupExcelEntity.Rows.HeadTable, colIdx, Color.FromArgb(198, 224, 180), Color.Black);
                colIdx++;
            }

            MergeColumns((int)FamilyPopupExcelEntity.Rows.Title, 1, columns.Length);
            MergeColumns((int)FamilyPopupExcelEntity.Rows.DateCreate, 1, columns.Length);

            SetBorderRangeCell((int)FamilyPopupExcelEntity.Rows.Title, (int)FamilyPopupExcelEntity.Rows.Title, 1, columns.Length);

            SetValue((int)FamilyPopupExcelEntity.Rows.DataStart, (int)FamilyPopupExcelEntity.ColumnsTableDeadDay.STT, ListMember);

            SetBorderRangeCell((int)FamilyPopupExcelEntity.Rows.HeadTable, (int)FamilyPopupExcelEntity.Rows.DataStart + ListMember.Count - 1, (int)FamilyPopupExcelEntity.ColumnsTableDeadDay.STT, (int)FamilyPopupExcelEntity.ColumnsTableDeadDay.Date);
            SetAutoWidthRangeColumn((int)FamilyPopupExcelEntity.ColumnsTableDeadDay.STT, (int)FamilyPopupExcelEntity.ColumnsTableDeadDay.Date);
            SetAutoWidthColumn((int)FamilyPopupExcelEntity.ColumnsTableDeadDay.Name, (int)FamilyPopupExcelEntity.Rows.DataStart, (int)FamilyPopupExcelEntity.Rows.DataStart + ListMember.Count, 18, 100);
            SetAutoWidthColumn((int)FamilyPopupExcelEntity.ColumnsTableDeadDay.Date, (int)FamilyPopupExcelEntity.Rows.DataStart, (int)FamilyPopupExcelEntity.Rows.DataStart + ListMember.Count, 18, 100);
            Save();
            File.Delete(tempPath);
            return true;
        }
    }

    public class InfoBirthDayFamilyForExportExcel
    {
        public int STT { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string BirthDay { get; set; }
        public string Age { get; set; }
        public string Date { get; set; }
    }
    public class InfoDeadDayFamilyForExportExcel
    {
        public int STT { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string DeadDaySun { get; set; }
        public string DeadDayMoon { get; set; }
        public string Date { get; set; }
    }
}
