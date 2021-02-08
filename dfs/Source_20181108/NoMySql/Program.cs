using System;
using System.Windows.Forms;
using BaseCommon;
using System.Globalization;

namespace NoMySql
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InstalledUICulture;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#if DEBUG
            Application.Run(new GraphForm(new clsGraphForm(), true));
#else
            Application.Run(new GraphForm(new clsGraphForm(), false));
#endif
        }
    }
}
