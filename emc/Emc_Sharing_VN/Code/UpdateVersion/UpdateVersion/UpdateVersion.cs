using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UpdateVersion
{
    public partial class frmUpdateVersion : Form
    {
        public const string FILE_ZIP_NAME = "NewVersion.zip";
        public const string FILE_SETUP_UPDATE = "ToolUpdate.exe";

        public string startPath = Application.StartupPath;
        public string zipPath = Application.StartupPath + @"\" + FILE_ZIP_NAME;

        public string uriFileVersion = Properties.Settings.Default.URI_FileVersion;
        public string uriFileUpdate = Properties.Settings.Default.URI_FileUpdate;

        public frmUpdateVersion()
        {
            InitializeComponent();
        }

        #region Event
        private void frmUpdateVersion_Load(object sender, EventArgs e)
        {
            try
            {
                WebRequest wr = WebRequest.Create(new Uri(uriFileVersion));
                WebResponse ws = wr.GetResponse();
                StreamReader sr = new StreamReader(ws.GetResponseStream());

                var currentversion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                var newversion = sr.ReadToEnd();

                if (!currentversion.Contains(newversion))
                {
                    prgDownload.Visible = false;
                    lblProgress.Visible = false;
                    lblContent.Text = "A new version is availble";
                    lblInfo.Text = string.Format("DSF602 {0} is now available (you have {1}). Would you like \n to download the new version now?", newversion, currentversion);
                }
                else
                {
                    prgDownload.Visible = false;
                    lblProgress.Visible = false;
                    btnDownload.Visible = false;
                    btnClose.Visible = false;
                    lblContent.Text = "App is up to date";
                    lblInfo.Text = "You are already the latest version, there are no new updates \n available at this moment.";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warring");
                this.Close();
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            prgDownload.Visible = true;
            lblInfo.Visible = false;
            lblProgress.Visible = true;
            btnDownload.Visible = false;
            btnClose.Visible = false;

            // Let the user know we are connecting to the server
            lblInfo.Text = "Download Starting...";
            // Create a new thread that calls the Download() method
            Thread thread = new Thread(() =>
            {
                try
                {
                    WebClient client = new WebClient();
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                    client.DownloadFileAsync(new Uri(uriFileUpdate), zipPath);
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "Warring");
                }

            });
            thread.Start();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Funtion
        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                double bytesIn = double.Parse(e.BytesReceived.ToString());
                double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
                double percentage = bytesIn / totalBytes * 100;

                lblProgress.Text = "Downloaded " + e.BytesReceived + " of " + e.TotalBytesToReceive + " (" + Convert.ToInt32(percentage) + "%)";
                //prgDownload.Value = int.Parse(Math.Truncate(percentage).ToString());
                prgDownload.Value = Convert.ToInt32((e.BytesReceived * 100) / e.TotalBytesToReceive);
            });
        }

        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                lblProgress.Text = "Completed";
                lblContent.Text = "Download complete";
                lblInfo.Visible = true;
                lblInfo.Text = "Do you want to restart and update the application now?";

                if (e.Error == null)
                {
                    MessageBox.Show("Restart and update the application", "Done", MessageBoxButtons.OK);

                    System.Diagnostics.Process.Start(startPath + @"\" + FILE_SETUP_UPDATE);

                    Environment.Exit(0);

                    //Application.Exit();

                    //BackupVersion();

                    //System.Diagnostics.Process.Start(startPath + @"\ToolUpdate.exe");
                }
            });
        }

        #endregion
    }
}
