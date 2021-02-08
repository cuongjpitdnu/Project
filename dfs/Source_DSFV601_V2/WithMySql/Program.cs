using BaseCommon;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace WithMySql
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
            Application.ApplicationExit += new EventHandler(evenExitApp);

            clsDBCore.getInstance(clsDBUltity.ConnectionString);
            clsCommon.CreateFolderHidden(clsConfig.PathDataErrors);
            deleteMeasureRawOld();

#if DEBUG
            Application.Run(new GraphForm(new clsGraphForm(), true));
#else
            Application.Run(new GraphForm(new clsGraphForm(), false));
#endif
        }

        private static void deleteMeasureRawOld()
        {
            using (var db = new clsDBUltity())
            {
                var dateLimit = DateTime.Now.AddDays(-clsConfig.TimeDeleteMeasureDetailRaw);
                var tbl = db.GetTBLMeasure(string.Empty, dateLimit.ToString(clsDBUltity.cstrDateTimeFormatNoMiliSecond));

                if (!clsCommon.TableIsNullOrEmpty(tbl))
                {
                    foreach (System.Data.DataRow row in tbl.Rows)
                    {
                        db.DeleteMeasureRaw(clsCommon.CnvNullToInt(row["measure_id"]));
                    }
                }
            }
        }

        private static void evenExitApp(object sender, EventArgs e)
        {
            clsDBCore.DisposeInstance();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
