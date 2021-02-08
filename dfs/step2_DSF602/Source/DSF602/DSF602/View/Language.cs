using BaseCommon.Core;
using DSF602.Language;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace DSF602.View
{
    public partial class Language : BaseForm
    {
        private Dictionary<string, string> lstFileLanguage = new Dictionary<string, string>();

        public Language()
        {
            InitializeComponent();
            InitForm();
        }

        #region Event Form

        private void btnChangeLanguage_Click(object sender, EventArgs e)
        {
            if (lstbLanguage.GetItemText(lstbLanguage.SelectedItem) == "")
            {
                this.ResultData = false;
                return;
            }
            this.SetModeWaiting();
            try
            {
                string fileNameLanguage = lstFileLanguage[lstbLanguage.GetItemText(lstbLanguage.SelectedItem)];
                LanguageHelper.DefaultLanguageValue = fileNameLanguage;
                LanguageHelper.LoadDataPOFile();

                this.ResultData = true;
                AppManager.OnLanguageChanged?.Invoke(null, null);
                this.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                this.SetModeWaiting(false);
            } 
        }

        private void btnCancelLanguage_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lstbLanguage_DoubleClick(object sender, EventArgs e)
        {
            btnChangeLanguage.PerformClick();
        }

        protected override void SetLanguageControl()
        {
            LanguageHelper.SetValueOf(this, "LANGUAGE_TITLE");
            LanguageHelper.SetValueOf(lblHeader, "LANGUAGE_HEADER");
            LanguageHelper.SetValueOf(btnCancelLanguage, "LANGUAGE_BTN_CANCEL");
            LanguageHelper.SetValueOf(btnChangeLanguage, "LANGUAGE_BTN_AGREE");
        }

        private void Language_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (lstFileLanguage != null)
            {
                lstFileLanguage.Clear();
            }

            lstFileLanguage = null;
        }

        #endregion Event Form

        #region Private Function

        private void InitForm()
        {
            this.Icon = Properties.Resources.language;

            DirectoryInfo dirInfoPO;
            FileInfo[] arrFilePO;

            try
            {
                dirInfoPO = new DirectoryInfo(LanguageHelper.GetPathFilePo());
                arrFilePO = dirInfoPO.GetFiles("*.po");

                if (arrFilePO.Length > 0)
                {
                    foreach (FileInfo filePO in arrFilePO)
                    {
                        var languageName = filePO.Name.Replace('_', '-');
                        languageName = languageName.Replace(".po", "").Trim();
                        CultureInfo ci = new CultureInfo(languageName);

                        lstFileLanguage.Add(ci.DisplayName, filePO.Name);
                        lstbLanguage.Items.Add(ci.DisplayName);
                    }

                    lstbLanguage.SelectedItem = CultureInfo.CurrentCulture.DisplayName;
                }
            }
            catch (Exception ex)
            {
                this.ShowMsg(MessageBoxIcon.Error, ex.Message);
            }
            finally
            {
                dirInfoPO = null;
                arrFilePO = null;
            }
        }

        #endregion Private Function
    }
}
