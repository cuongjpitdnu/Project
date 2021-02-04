using GPMain.Common;
using GPMain.Common.Navigation;
using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace GPMain.Views
{
    public partial class About : BaseUserControl
    {
        public About(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();
            //var fileName = System.IO.Path.GetFileName(Application.StartupPath + @"\Data\Docs\about.html");
            // SetBrowserFeatureControlKey("FEATURE_BROWSER_EMULATION", fileName, GetBrowserEmulationMode());
            if (!CheckForInternetConnection())
            {
                webBrowser.Url = new Uri(string.Format("file:///{0}",
                                     Application.StartupPath + AppConst.PageAbout));
            }
            else
            {
                if (File.Exists(AppConst.PageDownLoad))
                {
                    File.Delete(AppConst.PageAbout);
                    using (var client = new WebClient())
                    {
                        client.DownloadFile(AppConst.PageDownLoad, AppConst.PageDownLoadName);

                        webBrowser.Url = new Uri(string.Format("file:///{0}",
                                     Application.StartupPath + AppConst.PageAbout));
                    }
                }
                else
                {
                    webBrowser.Url = new Uri(string.Format("file:///{0}",
                                     Application.StartupPath + AppConst.PageAbout));
                }
                
            }
            
        }

        private  bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com"))
                    return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
