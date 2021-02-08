using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace DSF602.Model
{
    public class SQLiteManager : IDisposable
    {
        public static string GetConnection(string pathDB, string passDB = "")
        {
            var connStringBuilder = new SQLiteConnectionStringBuilder();
            connStringBuilder.DataSource = pathDB;
            connStringBuilder.JournalMode = SQLiteJournalModeEnum.Persist;
            connStringBuilder.SyncMode = SynchronizationModes.Normal;
            connStringBuilder.Version = 3;
            connStringBuilder.Pooling = true;
            connStringBuilder.ReadOnly = false;

            if (!string.IsNullOrEmpty(passDB))
            {
                connStringBuilder.Password = passDB;
            }

            return connStringBuilder.ToString();
        }

        public static void CreateDatabaseIfNotExists(string pathDB, string passDB = "")
        {
            if (!File.Exists(pathDB))
            {
                var pathFolder = Path.GetDirectoryName(pathDB);

                if (!Directory.Exists(pathFolder))
                {
                    Directory.CreateDirectory(pathFolder);
                }

                SQLiteConnection.CreateFile(pathDB);

                using (var objSQLiteManager = GetSQLiteManager(pathDB, string.Empty, false))
                {
                    objSQLiteManager.SetPassword(passDB);
                    objSQLiteManager.Open();
                    objSQLiteManager.ChangePassword(passDB);
                }

                using (var objSQLiteManager = GetSQLiteManager(pathDB, passDB))
                {
                    var script = Properties.Resources.queryfist;
                    objSQLiteManager.ExcuteNonQuery(script);
                }
            }
        }

        public static void CreateDatabaseIfNotExists2(string pathDB, string passDB = "")
        {
            if (!File.Exists(pathDB))
            {
                var pathFolder = Path.GetDirectoryName(pathDB);

                if (!Directory.Exists(pathFolder))
                {
                    Directory.CreateDirectory(pathFolder);
                }

                SQLiteConnection.CreateFile(pathDB);

                using (var objSQLiteManager = GetSQLiteManager(pathDB, string.Empty, false))
                {
                    objSQLiteManager.SetPassword(passDB);
                    objSQLiteManager.Open();
                    objSQLiteManager.ChangePassword(passDB);
                }

                using (var objSQLiteManager = GetSQLiteManager(pathDB, passDB))
                {
                    var script = string.Empty;
                    script += " BEGIN; ";
                    script += " CREATE TABLE IF NOT EXISTS Measure(MeasureId INTEGER PRIMARY KEY AUTOINCREMENT, SensorId INTEGER, Measure_Type INTEGER, Alarm_Value INTEGER, Start_time DATETIME, End_time DATETIME, Measure_Result INTEGER, UserId INTEGER, UpdateTime DATETIME, Delete_Flag INTEGER); ";
                    script += " CREATE TABLE IF NOT EXISTS Measure_Detail(DetailId INTEGER PRIMARY KEY AUTOINCREMENT, MeasureId INTEGER, Actual_Value INTEGER, Samples_time DATETIME, Detail_Result INTEGER); ";
                    script += " END; ";
                    objSQLiteManager.ExcuteNonQuery(script);
                }
            }
        }

        public static SQLiteManager GetSQLiteManager(string pathDB, string passDB = "", bool autoOpen = true)
        {
            return new SQLiteManager(pathDB, passDB, autoOpen);
        }

        private SQLiteConnection _dbConnection;

        private SQLiteManager(string pathDB, string passDB = "", bool autoOpen = true)
        {
            _dbConnection = new SQLiteConnection(GetConnection(pathDB, passDB));

            if (autoOpen)
            {
                this.Open();
            }
        }

        public void Open()
        {
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
        }

        public void SetPassword(string pass)
        {
            _dbConnection.SetPassword(pass);
        }

        public void ChangePassword(string pass)
        {
            _dbConnection.ChangePassword(pass);
        }

        public int ExcuteNonQuery(string sql)
        {
            if (string.IsNullOrEmpty(sql))
            {
                return -1;
            }

            using (var command = _dbConnection.CreateCommand())
            {
                command.CommandText = sql;
                return command.ExecuteNonQuery();
            }
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (_dbConnection != null && _dbConnection.State != ConnectionState.Closed)
                    {
                        if (_dbConnection.State != ConnectionState.Closed)
                        {
                            _dbConnection.Close();
                        }
                        _dbConnection.Dispose();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                _dbConnection = null;
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SQLiteManager()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}