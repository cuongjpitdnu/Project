using GP40Common;
using GP40Main.Models;
using GP40Main.Services.Dialog;
using GP40Main.Services.Navigation;
using GP40Main.Themes;
using GP40Main.Utility;
using GP40Main.Views;
using GP40Main.Views.FamilyInfo;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GP40Main.Core
{
    /// <summary>
    /// Meno        : Manager Application, Resources
    /// Create by   : AKB Nguyen Thanh Tung
    /// </summary>
    internal class AppManager
    {
        private static bool _isConfig = false;
        private static readonly object _syncDatabase = new object();
        private static LiteDBManager _dbManager;
        #region Public Property

        // Application Information
        public static string AppName => Application.ProductName;

        public static string AppVersion => Application.ProductVersion;
        public static DateTime AppCreateDate => File.GetCreationTime(Assembly.GetExecutingAssembly().Location);

        // Application Service
        public static NavigationService Navigation { get; private set; }

        public static DialogManager Dialog { get; set; }
        public static LogHelper LoggerApp { get; private set; }
        public static LogHelper LoggerDB { get; private set; }

        public static MUser LoginUser { get; private set; }

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

            clsCommon.InitDataDirectory();

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

            Navigation = new NavigationService();
            Dialog = new DialogManager();
            LoggerApp = new LogHelper(Environment.CurrentDirectory + AppConst.LogPath + "GP40App_.txt");
            LoggerDB = new LogHelper(Environment.CurrentDirectory + AppConst.LogPath + "GP40DB_.txt");
            _isConfig = true;
        }

        /// <summary>
        /// Run Application
        /// </summary>
        public static void RunAppIfLogin()
        {
            if (!_isConfig) return;

            LoginUser = Navigation.ShowDialog<LoginPage, MUser>();

            if (LoginUser == null)
            {
                Application.Exit();
                return;
            }

            var objFamilyInfo = DBManager.GetTable<MFamilyInfo>().CreateQuery(i => i.Id == LoginUser.FamilyId).FirstOrDefault();

            if (objFamilyInfo == null && Navigation.ShowDialog<FamilyInfoNewPage, MFamilyInfo>() == null)
            {
                Application.Exit();
                return;
            }

            Navigation.RunApp<MenuForm>();
        }

        /// <summary>
        /// Check Mode Theme Windows 10
        /// </summary>
        /// <returns>true - Light/false - Dark</returns>
        public static bool IsLightMode()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                {
                    if (key != null)
                    {
                        var appsUseLightTheme = key.GetValue("AppsUseLightTheme");
                        if (appsUseLightTheme != null && !appsUseLightTheme.Equals(1))
                        {
                            return false;
                        }
                    }
                }
            }
            catch
            {
            }

            return true;
        }


        /// <summary>
        /// Struct representing a point.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        /// <summary>
        /// Retrieves the cursor's position, in screen coordinates.
        /// </summary>
        /// <see>See MSDN documentation for further information.</see>
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        public static Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            // NOTE: If you need error handling
            // bool success = GetCursorPos(out lpPoint);
            // if (!success)

            return new Point(lpPoint.X, lpPoint.Y);
        }
    }
}