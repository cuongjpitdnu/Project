using GPMain.Common;
using GPMain.Common.Database;
using GPMain.Common.Navigation;
using GPMain.Views;
using GPMain.Common.Interface;
using GPModels;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TinyIoC;
using GPMain.Common.Dialog;
using GPMain.Views.FamilyInfo;
using GPMain.Common.Helper;
using System.Net;
using ICSharpCode.SharpZipLib.Zip;
using System.Threading;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using GPMain.Views.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Drawing;

namespace GPMain
{
    /// <summary>
    /// Meno        : Manager Application, Resources
    /// Create by   : AKB Nguyen Thanh Tung
    /// </summary>
    internal class AppManager
    {
        private static bool _isConfig = false;
        private static readonly object _syncApplication = new object();
        private static readonly object _syncDatabase = new object();
        private static LiteDBManager _dbManager;
        private static string _uriUpdateVersion = Properties.Settings.Default.URI_UpdateVersion;

        #region Public Property

        // Application Information
        public static string AppName => Application.ProductName;
        public static string AppVersion => Application.ProductVersion;
        public static DateTime AppCreateDate => File.GetCreationTime(Assembly.GetExecutingAssembly().Location);
        public static string AppPathFolder => Environment.CurrentDirectory;

        public static ModeDisplay ModeDisplay = ModeDisplay.BuildTree;

        public static TinyIoCContainer Container
        {
            get
            {
                lock (_syncApplication)
                {
                    return TinyIoCContainer.Current;
                }
            }
        }

        public static INavigation Navigation => Container.Resolve<INavigation>();
        public static IDialog Dialog => Container.Resolve<IDialog>();
        public static ILog LoggerApp => Container.Resolve<ILog>("log_app");
        public static ILog LoggerDB => Container.Resolve<ILog>("log_db");
        public static MenuMemberBuffer MenuMemberBuffer => Container.Resolve<MenuMemberBuffer>();

        public static MUser LoginUser { get; set; }

        public static bool isExecuting { get; set; } = false;

        public static LiteDBManager DBManager
        {
            get
            {
                lock (_syncDatabase)
                {
                    if (_dbManager == null || _dbManager.IsDisposed)
                    {
                        var pathDB = Environment.CurrentDirectory + AppConst.DatabaseJsonPath;

                        if (!Directory.Exists(pathDB))
                        {
                            Directory.CreateDirectory(pathDB);
                        }

                        _dbManager = new LiteDBManager(pathDB + AppConst.DatabaseName, AppConst.DatabasePass);
                    }

                    return _dbManager;
                }
            }
        }

        #endregion Public Property

        /// <summary>
        /// Initialize Application
        /// </summary>
        public static void Init()
        {
            if (_isConfig) return;

            Application.CurrentCulture = new CultureInfo("vi-VN");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("vi-VN");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("vi-VN");

            var pathLogApp = AppPathFolder + AppConst.LogPath + "GP40App_.txt";
            var pathLogDB = AppPathFolder + AppConst.LogPath + "GP40DB_.txt";

            // Register Service
            Container.Register<INavigation, NavigationService>().AsSingleton();
            Container.Register<IDialog, DialogManager>().AsSingleton();
            Container.Register<ILog>(new LogHelper(pathLogApp), "log_app");
            Container.Register<ILog>(new LogHelper(pathLogDB), "log_db");
            Container.Register<MenuMemberBuffer>().AsSingleton();

            GP40Common.clsCommon.InitDataDirectory();
            var pathDBBackup = AppPathFolder + AppConst.BackupDBPath;
            if (!Directory.Exists(pathDBBackup)) Directory.CreateDirectory(pathDBBackup);

            Application.ApplicationExit += (sender, e) =>
            {
                lock (_syncDatabase)
                {
                    if (_dbManager != null)
                    {
                        _dbManager.Dispose();
                    }

                    _dbManager = null;
                }
            };

            Application.ThreadException += (sender, e) =>
            {
                LoggerApp.Error(typeof(Application), e.Exception, "Global Exception");
            };

            _isConfig = true;
        }

        /// <summary>
        /// Run Application
        /// </summary>
        public static void RunAppIfLogin()
        {
            CheckVersion();
            if (!_isConfig) return;

            LoginUser = Navigation.ShowDialogWithParam<LoginPage, MUser>();

            if (LoginUser == null)
            {
                Application.Exit();
                return;
            }

            var objFamilyInfo = DBManager.GetTable<MFamilyInfo>().CreateQuery(i => i.Id == LoginUser.FamilyId).FirstOrDefault();

            if (objFamilyInfo == null && Navigation.ShowDialogWithParam<FamilyInfoNewPage, MFamilyInfo>() == null)
            {
                Application.Exit();
                return;
            }

            Navigation.RunApp<MenuForm>();
        }

        [DllImport("psapi.dll")]
        static extern int EmptyWorkingSet(IntPtr hwProc);

        public static void MinimizeFootprint()
        {
            try
            {
                EmptyWorkingSet(Process.GetCurrentProcess().Handle);
            }
            catch
            {
            }
        }

        /// <summary>
        /// function check version when start app
        /// </summary>
        public static void CheckVersion()
        {
            if (isHasVersion())
            {
                if (!Dialog.Confirm("Phần mềm có phiên bản cập nhật mới.\nBạn có muốn cập nhập phần mềm?"))
                {
                    return;
                }
                UpdateApp();
            }
        }

        /// <summary>
        /// Check has new version
        /// </summary>
        /// <returns>boolean: has new version ->true </returns>
        public static bool isHasVersion()
        {
            var isHasNewVersion = false;
            try
            {
                WebRequest wr = WebRequest.Create(new Uri(_uriUpdateVersion + AppConst.FileVersion));
                wr.Timeout = 1000;
                WebResponse ws = wr.GetResponse();
                StreamReader sr = new StreamReader(ws.GetResponseStream());

                var currentversion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                var newversion = sr.ReadToEnd();
                isHasNewVersion = !currentversion.Contains(newversion) ? true : false;
            }
            catch (Exception ex)
            {
                isHasNewVersion = false;
            }

            return isHasNewVersion;
        }

        /// <summary>
        /// Procces update app
        /// </summary>
        public static void UpdateApp()
        {
            using (WebClient wc = new WebClient())
            {
                bool isSuscess = false;
                FastZip zip = null/* TODO Change to default(_) if this is not a reference type */;
                string pathTemp = Application.StartupPath + @"\\TempUpdate\\";
                string pathFile = pathTemp + "temp.zip";
                string pathUnZip = pathTemp + @"UnZip\\";
                string pathUpdater = pathUnZip + "Updater.exe";
                string pathUpdaterPdb = pathUnZip + "Updater.pdb";

                try
                {
                    FileHepler.CreateFolder(pathTemp);
                    wc.DownloadFile(new Uri(_uriUpdateVersion + AppConst.FileUpdate), pathFile);

                    if (!File.Exists(pathFile))
                        return;

                    FileHepler.CreateFolder(pathUnZip);
                    zip = new FastZip();
                    zip.ExtractZip(pathFile, pathUnZip, "");

                    if (File.Exists(pathUpdater))
                    {
                        File.Copy(pathUpdater, Application.StartupPath + @"\Updater.exe", true);
                        File.Delete(pathUpdater);
                    }

                    if (File.Exists(pathUpdaterPdb))
                    {
                        File.Copy(pathUpdaterPdb, Application.StartupPath + @"\Updater.pdb", true);
                        File.Delete(pathUpdaterPdb);
                    }

                    isSuscess = true;
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    zip = null/* TODO Change to default(_) if this is not a reference type */;

                    if (!File.Exists(Application.StartupPath + @"\\Updater.exe"))
                        isSuscess = false;

                    if (isSuscess)
                    {
                        Dialog.Ok("Cập nhập thành công. Chương trình sẽ khởi động lại.");
                        Process p = new Process();
                        p.StartInfo.FileName = Application.StartupPath + @"\\Updater.exe";
                        p.StartInfo.Arguments = "Update";
                        p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        p.Start();
                        Environment.Exit(0);
                    }
                    else
                    {
                        Dialog.Error("Có lỗi trong quá trình xử lí. Vui vòng thử lại sau!");
                    }
                }
            }
        }

        /// <summary>
        /// function check version when start app
        /// </summary>
        public static void RestoreVersion()
        {
            if (!Dialog.Confirm("Bạn có chắc muốn khôi phục lại phiên bản cũ."))
            {
                return;
            }
            RestoreApp();
        }

        /// <summary>
        /// Procces restore app
        /// </summary>
        private static void RestoreApp()
        {
            using (WebClient wc = new WebClient())
            {
                string pathBackup = Application.StartupPath + @"\\BackUp";

                if (!Directory.Exists(pathBackup))
                {
                    Dialog.Error("Không có phiên bản backup.");
                    return;
                }

                if (FileHepler.isDirectoryEmpty(pathBackup))
                {
                    Dialog.Error("Không có phiên bản backup.");
                    return;
                }

                Dialog.Ok("Khôi phục đã thành công. Chương trình sẽ khởi động lại.");
                Process p = new Process();
                p.StartInfo.FileName = Application.StartupPath + @"\\Updater.exe";
                p.StartInfo.Arguments = "Restore";
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.Start();
                Environment.Exit(0);
            }
        }
    }
}
