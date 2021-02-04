using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPMain.Common.Events;

namespace GPMain.Common.Excel
{
    public class ExportExcelForEvent : ExcelCommon
    {
        public bool Export(List<EventForShowGridView> listData, string title, string tempPath, string pathFileToSave)
        {
            CreateWorkbook(tempPath, pathFileToSave);
            SetValue((int)EventExcelEntity.Rows.Title, (int)EventExcelEntity.Columns.STT, title);
            SetValue((int)EventExcelEntity.Rows.DateCreate, (int)EventExcelEntity.Columns.STT, DateTime.Now.ToString("HH:mm - dd/MM/yyyy"));
            int rowdata = (int)EventExcelEntity.Rows.DataStart;
            foreach (var even in listData)
            {
                SetValue(rowdata, (int)EventExcelEntity.Columns.STT, even.STT);
                SetValue(rowdata, (int)EventExcelEntity.Columns.EventName, even.EventName);
                SetValue(rowdata, (int)EventExcelEntity.Columns.Description, even.Desciption);
                SetValue(rowdata, (int)EventExcelEntity.Columns.CalendarType, even.CalendarType);
                SetValue(rowdata, (int)EventExcelEntity.Columns.TimeEventStart, even.TimeEventStart);
                SetValue(rowdata, (int)EventExcelEntity.Columns.TimeEventEnd, even.TimeEventEnd);
                SetAutoRowHeight(rowdata);
                if (even.Important)
                {
                    SetRowBackColor(rowdata, (int)EventExcelEntity.Columns.STT, (int)EventExcelEntity.Columns.Description, Color.FromArgb(248, 184, 98), Color.Black);
                }
                rowdata++;
            }
            SetBorderRangeCell((int)EventExcelEntity.Rows.DataStart, rowdata - 1, (int)EventExcelEntity.Columns.STT, (int)EventExcelEntity.Columns.Description);
            SetAutoWidthRangeColumn((int)EventExcelEntity.Columns.STT, (int)EventExcelEntity.Columns.Description);
            Save();
            File.Delete(tempPath);
            return true;
        }
    }
}
