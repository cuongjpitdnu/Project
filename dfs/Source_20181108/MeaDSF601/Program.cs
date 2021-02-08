using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MeaDSF601
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ApplicationExit += new EventHandler(evenExitApp);

            clsDBCore.getInstance(clsDBUltity.ConnectionString);
            Common.CreateFolderHidden(Common.PathDataErrors);
            deleteMeasureRawOld();

            using(var db = new clsDBUltity())
            {
                db.LoginCheck("admin", "admin");
            }

            Application.Run(new GraphForm());

            //using (var frmLogin = new LoginForm())
            //{
            //    clsDBCore.getInstance(clsDBUltity.ConnectionString);
            //    Common.CreateFolderHidden(Common.PathDataErrors);
            //    deleteMeasureRawOld();

            //    frmLogin.ShowDialog();
            //    if (frmLogin.LoginSusses)
            //    {
            //        Application.Run(new GraphForm());
            //    } else
            //    {
            //        Application.Exit();
            //    }
            //}
        }

        private static void deleteMeasureRawOld()
        {
            using(var db = new clsDBUltity())
            {
                var dateLimit = DateTime.Now.AddDays(-Common.TimeDeleteMeasureDetailRaw);
                var tbl = db.GetTBLMeasure(string.Empty, dateLimit.ToString(clsDBUltity.cstrDateTimeFormatNoMiliSecond));

                if (!Common.TableIsNullOrEmpty(tbl))
                {
                    foreach (System.Data.DataRow row in tbl.Rows)
                    {
                        db.DeleteMeasureRaw(Common.CnvNullToInt(row["measure_id"]));
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
