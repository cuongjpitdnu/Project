using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GP40Common;

namespace GP40Tree
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            clsCommon.InitDataDirectory();
            //Application.Run(new frmTree());
            
            Application.Run(new frmCalendarVN());
            //Application.Run(new frmMain());            
        }
    }
}
