using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMain.Common.Excel
{
    public class EventExcelEntity
    {
        public enum Rows
        {
            Title = 1,//Số thứ tự dòng tiêu đề
            DateCreate = 3,//Số thứ tự dòng ngày xuất bản
            DataStart = 6  //Số thứ tự dòng bắt đầu dữ liệu
        }
        public enum Columns
        {
            STT = 1,
            EventName,
            TimeEventStart,
            TimeEventEnd,
            CalendarType,
            Description
        }
        public static Dictionary<string, string> NameColumns = new Dictionary<string, string>()
        {
            { "STT","STT"},
            { "EventName","Tên sự kiện"},
            { "TimeEventStart","Ngày bắt đầu"},
            { "TimeEventEnd","Ngày kết thúc"},
            { "CalendarType","Loại lịch"},
            { "Description","Mô tả"}
        };
    }
}
