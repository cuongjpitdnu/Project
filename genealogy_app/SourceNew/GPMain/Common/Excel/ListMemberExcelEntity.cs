using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMain.Common.Excel
{
    public class ListMemberExcelEntity
    {

        public enum Rows
        {
            Title = 1,//Số thứ tự dòng tiêu đề
            DateCreate = 3,//Số thứ tự dòng ngày xuất bản
            DataStart = 6  //Số thứ tự dòng bắt đầu dữ liệu
        }
        public enum Columns
        {
            STT,
            LevelInFamily,
            MemberName,
            Gender,
            BirthDay,
            DeadDay,
            Father,
            Mother,
            Spouse,
            Child
        }
        public static Dictionary<string, string> NameColumns = new Dictionary<string, string>()
        {
            { "STT","STT"},
            { "LevelInFamily","Đời"},
            { "MemberName","Tên thành viên"},
            { "Gender","Giới tính"},
            { "BirthDay","Ngày sinh"},
            { "DeadDay","Ngày mất"},
            { "Father","Tên cha"},
            { "Mother","Tên mẹ"},
            { "Spouse","Tên vợ"},
            { "Child","Tên con"},
        };
    }

}
