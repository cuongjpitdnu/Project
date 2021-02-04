using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMain.Common.Excel
{
    public class InfoMemberForExportExcel
    {
        public int STT { get; set; }
        public string LevelInFamily { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string BirthDay { get; set; }
        public string DeadDay { get; set; }
        public string Father { get; set; }
        public string Mother { get; set; }
        public string Spouse { get; set; }
        public string Child { get; set; }
    }
}
