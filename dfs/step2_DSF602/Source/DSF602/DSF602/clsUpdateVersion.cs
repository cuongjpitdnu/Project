using DSF602.Language;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DSF602
{
    public class clsUpdateVersion
    {
        //public const string FILE_ZIP_NAME = "NewVersion.zip";
        //public const string FILE_SETUP_UPDATE = "ToolUpdate.exe";

        //public static string startPath = Application.StartupPath;
        //public static string zipPath = Application.StartupPath + @"\" + FILE_ZIP_NAME;

        //public static string uriFileVersion = Properties.Settings.Default.URI_FileVersion;
        //public static string uriFileUpdate = Properties.Settings.Default.URI_FileUpdate;


        //public static bool CheckNewVersion()
        //{
        //    try
        //    {
        //        WebRequest wr = WebRequest.Create(new Uri(uriFileVersion));
        //        WebResponse ws = wr.GetResponse();
        //        StreamReader sr = new StreamReader(ws.GetResponseStream());

        //        var currentversion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        //        var newversion = sr.ReadToEnd();

        //        return currentversion.Contains(newversion) ? true : false;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        //public static void Download()
        //{
        //    Thread thread = new Thread(() =>
        //    {
        //        try
        //        {
        //            WebClient client = new WebClient();
        //            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
        //            client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
        //            client.DownloadFileAsync(new Uri(uriFileUpdate), zipPath);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message, "Error");
        //        }

        //    });
        //    thread.Start();
        //}

        //private static void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        //{
        //    this.BeginInvoke((MethodInvoker)delegate
        //    {
        //        double bytesIn = double.Parse(e.BytesReceived.ToString());
        //        double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
        //        double percentage = bytesIn / totalBytes * 100;

        //        lblProgress.Text = LanguageHelper.GetValueOf("UPDATE_DOWNLOADED_TEXT") + " " + e.BytesReceived + " / " + e.TotalBytesToReceive + " (" + Convert.ToInt32(percentage) + "%)";
        //        prgDownload.Value = Convert.ToInt32((e.BytesReceived * 100) / e.TotalBytesToReceive);
        //        prgDownload.Update();
        //    });
        //}

        //private static void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        //{
        //    this.BeginInvoke((MethodInvoker)delegate
        //    {
        //        lblProgress.Text = LanguageHelper.GetValueOf("UPDATE_LBL_PROGRESS");
        //        lblContent.Text = LanguageHelper.GetValueOf("UPDATE_LBL_CONTENT_DOWNLOADED");
        //        lblInfo.Visible = false;

        //        var sendObj = "UPDATE";
        //        var jsonData = JsonConvert.SerializeObject(sendObj);
        //        if (e.Error == null)
        //        {
        //            clsCommon.ShowMsg(MessageBoxIcon.Information, "UPDATE_MESSAGEBOX_MSG", "Message");
        //            //MessageBox.Show("Restart and update the application", "Done", MessageBoxButtons.OK);
        //            try
        //            {
        //                using (Process myProcess = new Process())
        //                {
        //                    myProcess.StartInfo.UseShellExecute = false;
        //                    myProcess.StartInfo.FileName = startPath + @"\" + FILE_SETUP_UPDATE;
        //                    myProcess.StartInfo.Arguments = clsCommon.Base64Encode(jsonData);
        //                    myProcess.StartInfo.CreateNoWindow = false;
        //                    myProcess.Start();
        //                }

        //                var lstBlockThread = this.Params as List<BlockThread>;

        //                for (var i = 0; i < lstBlockThread.Count; i++)
        //                {
        //                    lstBlockThread[i].IsRunning = false;

        //                    foreach (var sensor in lstBlockThread[i].ListSensorChild)
        //                    {
        //                        sensor.Alarm = false;
        //                    }
        //                }
        //                Environment.Exit(0);
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message, "Error");
        //            }
        //        }
        //    });
        //}
    }
}
