using BaseCommon;
using BaseCommon.Core;
using DSF602.Language;
using DSF602.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace DSF602
{
    internal class AppManager : AppCore
    {
        public static readonly string AppVersion = Application.ProductVersion;
        public static readonly DateTime AppCreateDate = File.GetCreationTime(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public static readonly string PathReportTemplate = Application.StartupPath + @"\Excel Template\";
        public static readonly string PathFolderData = Application.StartupPath + @"\Data\";

        private static bool _hasConfigure;
        private static string _pathDB = PathFolderData + @"dbMain.db";
        private static Dictionary<string, DBManagerChild> _dbChildContainer = new Dictionary<string, DBManagerChild>();

        public static MUser UserLogin { get; set; }
        public static List<Block> ListBlock { get; set; }
        public static List<SensorInfo> ListSensor { get; set; }

        public static List<int> ActualListSensorIdActive = new List<int>();

        public delegate void OnSensorInfoRefreshDelegate();
        public static OnSensorInfoRefreshDelegate OnSensorInfoRefresh;

        public static EventHandler OnSensorSelected;
        public static EventHandler OnLanguageChanged;
        public static EventHandler OnSettingChanged;

        public static void Configure()
        {
            if (!_hasConfigure)
            {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.ApplicationExit += (sender, e) =>
                {
                    //foreach (var dbChild in _dbChildContainer)
                    //{
                    //    if (dbChild.Value != null)
                    //    {
                    //        dbChild.Value.Dispose();
                    //    }
                    //}

                    //_dbChildContainer.Clear();
                    //_dbChildContainer = null;
                };

                // Config DB           
                SQLiteManager.CreateDatabaseIfNotExists(_pathDB);
                using (var objDB = AppManager.GetConnection())
                {
                    var defValues = GetDefaultSetting();
                    objDB.SetDefaultValues(defValues);
                }
                using (var objDB = AppManager.GetConnection())
                {
                    ListBlock = objDB.GetListBlock();
                    ListSensor = objDB.GetListSensor();
                }

                ListSensor.Select((sensor) => {
                    var pathDBSensor = PathFolderData + DBManagerChild.GetDBName(sensor.SensorId);
                    SQLiteManager.CreateDatabaseIfNotExists2(pathDBSensor);
                    GetDBChildConnection(DBManagerChild.GetDBName(sensor.SensorId));
                    return sensor;
                }).ToList();

                //foreach (var sensor in ListSensor)
                //{
                //    var pathDBSensor = PathFolderData + DBManagerChild.GetDBName(sensor.SensorId);
                //    SQLiteManager.CreateDatabaseIfNotExists2(pathDBSensor);
                //    GetDBChildConnection(DBManagerChild.GetDBName(sensor.SensorId));
                //}

                // Config Log

                // Config Language
                LanguageHelper.LoadConfigDefaultLanguage();

                _hasConfigure = true;
            }
        }

        public static DBManager GetConnection()
        {
            return new DBManager(new SQLiteConnection(SQLiteManager.GetConnection(_pathDB)));
        }

        public static DBManagerChild GetDBChildConnection(string nameDB)
        {
            if (!_dbChildContainer.ContainsKey(nameDB)) {
                var pathDB = PathFolderData + nameDB;
                _dbChildContainer.Add(nameDB, new DBManagerChild(new SQLiteConnection(SQLiteManager.GetConnection(pathDB)), nameDB));
            }

            return _dbChildContainer[nameDB];
        }

        public static SettingParam GetDefaultSetting()
        {
            var pr = new SettingParam()
            {
                VoltAlarmValue = int.Parse(ConfigurationManager.AppSettings["VoltAlarm"]),
                IonAlarmValue = int.Parse(ConfigurationManager.AppSettings["IonAlarm"]),
                UpVal = int.Parse(ConfigurationManager.AppSettings["UpVal"]),
                LowVal = int.Parse(ConfigurationManager.AppSettings["LowVal"]),
                DecayTimeCheck = int.Parse(ConfigurationManager.AppSettings["DecayTimeCheck"]),
                StopDecayTime = int.Parse(ConfigurationManager.AppSettings["StopDecayTime"]),
                IonBalanceCheck = int.Parse(ConfigurationManager.AppSettings["IonValueCheck"]),
                IonStopTimeCheck = int.Parse(ConfigurationManager.AppSettings["IonTimeCheck"]),
                AutoCheckTimes = ConfigurationManager.AppSettings["AutoCheckTimes"],
                AutoCheckDays = ConfigurationManager.AppSettings["AutoCheckDays"],
                IsAuto = ConfigurationManager.AppSettings["IsAuto"] == "1" ? true : false
            };
            return pr;
        }
    }
}
