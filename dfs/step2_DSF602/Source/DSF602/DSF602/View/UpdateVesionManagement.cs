using BaseCommon;
using BaseCommon.Core;
using DSF602.Language;
using DSF602.Model;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace DSF602.View
{
    public partial class UpdateVersion : BaseForm
    {
        public string zipPath = Application.StartupPath + @"\" + clsConst.FILE_ZIP_NAME;

        public string uriFileVersion = Properties.Settings.Default.URI_FileVersion;
        public string uriFileUpdate = Properties.Settings.Default.URI_FileUpdate;

        public UpdateVersion()
        {
            InitializeComponent();
        }

        #region Event
        private void UpdateVersion_Load(object sender, EventArgs e)
        {
            prgDownload.Visible = false;
            lblProgress.Visible = false;
            btnRestore.Visible = true;
            btnClose.Visible = true;
            btnCheckVersion.Visible = true;
            btnUpload.Visible = false;
            btnCheckVersion.Location = btnUpload.Location;

            lblContent.Text = LanguageHelper.GetValueOf("UPDATE_CHECK_CONTENT");

            var currentversion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            lblInfo.Text = string.Format(LanguageHelper.GetValueOf("UPDATE_CHECK_INFO"), currentversion);

        }

        private void btnCheckVersion_Click(object sender, EventArgs e)
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
                    btnRestore.Visible = true;
                    btnClose.Visible = true;
                    btnCheckVersion.Visible = false;
                    btnUpload.Visible = true;

                    lblContent.Text = LanguageHelper.GetValueOf("UPDATE_LBL_CONTENT");
                    lblInfo.Text = string.Format(LanguageHelper.GetValueOf("UPDATE_LBL_INFO"), newversion, currentversion);
                }
                else
                {
                    prgDownload.Visible = false;
                    lblProgress.Visible = false;
                    btnUpload.Visible = false;
                    btnClose.Visible = true;
                    btnRestore.Visible = true;
                    btnRestore.Location = btnUpload.Location;


                    lblContent.Text = LanguageHelper.GetValueOf("UPDATE_LBL_CONTENT_NOVERSION");
                    lblInfo.Text = LanguageHelper.GetValueOf("UPDATE_LBL_INFO_NOVERSION");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(LanguageHelper.GetValueOf("MSG_ERROR_CHECKCONNECTION"), LanguageHelper.GetValueOf("MSG_ERROR_TITLE"));
                //this.Close();
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {

            prgDownload.Visible = true;
            lblInfo.Visible = false;
            lblProgress.Visible = true;
            btnUpload.Visible = false;
            btnClose.Visible = false;
            btnRestore.Visible = false;
            btnCheckVersion.Visible = false;

            lblInfo.Text = LanguageHelper.GetValueOf("UPDATE_LBL_INFO_DOWNLOAD");
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
                    MessageBox.Show(ex.Message, LanguageHelper.GetValueOf("MSG_ERROR_TITLE"));
                }

            });
            thread.Start();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            AppManager.ShowDialog<RestoreVersion>(null, this);
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

                lblProgress.Text = LanguageHelper.GetValueOf("UPDATE_DOWNLOADED_TEXT") + " " + e.BytesReceived + " / " + e.TotalBytesToReceive + " (" + Convert.ToInt32(percentage) + "%)";
                prgDownload.Value = Convert.ToInt32((e.BytesReceived * 100) / e.TotalBytesToReceive);
                prgDownload.Update();
            });
        }

        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                lblProgress.Text = LanguageHelper.GetValueOf("UPDATE_LBL_PROGRESS");
                lblContent.Text = LanguageHelper.GetValueOf("UPDATE_LBL_CONTENT_DOWNLOADED");
                lblInfo.Visible = false;

                var dataMsg = new MsgData
                {
                    KeyCheck = "UPDATE",
                    Title = LanguageHelper.GetValueOf("UPDATE_TITLE_SENDER"),
                    Message = LanguageHelper.GetValueOf("UPDATE_MESSAGE_SENDER"),
                };

                if (e.Error == null)
                {
                    clsCommon.ShowMsg(MessageBoxIcon.Information, "UPDATE_MESSAGEBOX_MSG", "Message");

                    this.Hide();
                    this.Close();
                    var parentForm = this.FormParent as MainForm;
                    parentForm.IsStop = true;
                    parentForm.MainForm_FormClosed(dataMsg, null);

                }
            });
        }

        #endregion

        protected override void SetLanguageControl()
        {
            LanguageHelper.SetValueOf(this, "UPDATEVERSION_TITLE");
            LanguageHelper.SetValueOf(btnClose, "UPDATE_BTN_CLOSE");
            LanguageHelper.SetValueOf(btnUpload, "UPDATE_BTN_UPLOAD");
            LanguageHelper.SetValueOf(btnRestore, "UPDATE_BTN_RESTORE");
            LanguageHelper.SetValueOf(btnCheckVersion, "UPDATE_BTN_CHECK");
        }

    }
}
