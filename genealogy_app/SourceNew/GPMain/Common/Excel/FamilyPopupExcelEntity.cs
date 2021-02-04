using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMain.Common.Excel
{
    public class FamilyPopupExcelEntity
    {
        public enum Rows
        {
            Title = 1,//Số thứ tự dòng tiêu đề
            DateCreate = 3,//Số thứ tự dòng ngày xuất bản
            HeadTable = 5,
            DataStart = 6  //Số thứ tự dòng bắt đầu dữ liệu
        }
        public enum ColumnsTableBirthDay
        {
            STT = 1,
            Name,
            Gender,
            BirthDay,
            Age,
            Date
        }
        public enum ColumnsTableDeadDay
        {
            STT = 1,
            Name,
            Gender,
            DeadDaySun,
            DeadDayMoon,
            Date
        }
        public static Dictionary<string, string> NameColumnsTableBirthDay = new Dictionary<string, string>()
        {
            { "Gender","Giới tính"},
            { "STT","STT"},
            { "Name","Họ tên"},
            { "BirthDay","Ngày sinh (Dương lịch)"},
            { "Age","Tuổi"},
            { "Date","Ngày kỷ niệm"}
        };
        public static Dictionary<string, string> NameColumnsTableDeadDay = new Dictionary<string, string>()
        {
            { "Gender","Giới tính"},
            { "STT","STT"},
            { "Name","Họ tên"},
            { "DeadDaySun","Ngày mất (Dương lịch)"},
            { "DeadDayMoon","Ngày mất (Âm lịch)"},
            { "Date","Ngày kỷ niệm"}
        };
    }
}
