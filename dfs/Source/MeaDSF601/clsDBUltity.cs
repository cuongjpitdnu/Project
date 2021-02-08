namespace MeaDSF601
{
    using MySql.Data.MySqlClient;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.IO;

    class clsDBUltity : IDisposable
    {
        const string userGetDeviceList = "99";
        public const string cstrDateTimeFormatNoMiliSecond = "yyyy-MM-dd HH:mm:ss";
        public const string cstrDateTimeFormatMiliSecond = "yyyy-MM-dd HH:mm:ss.fff";
        public const string cstrDateTimeFormatNoMiliSecond2 = "yyyyMMddHHmmss";

        private clsDBCore _db;
        private bool _blnCreateNew;

        public enum emDeviceType
        {
            None,
            Device1,
            Device2,
        }

        public enum emMeasureType
        {
            AlarmTest,
            WalkingTest,
        }

        public enum emMeasureResult
        {
            Pass,
            Fail,
            Normal,
            Alarm
        }

        public enum emMeasureStatus
        {
            Complete,
            Error,
        }

        private string _lastSqlQuery = string.Empty;

        public string LastSqlQuery
        {
            get { return _lastSqlQuery; }
        }

        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["connectionString"];
            }
        }

        public clsDBUltity(bool blnCreateNew = false)
        {
            _blnCreateNew = blnCreateNew;

            if (!blnCreateNew)
            {
                _db = clsDBCore.getInstance();
            } else
            {
                _db = new clsDBCore(ConnectionString);
                _db.Open();
            }
        }

        public bool LoginCheck(string username, string password)
        {
            string sql = string.Empty;

            sql += " SELECT ";
            sql += "    * ";
            sql += " FROM ";
            sql += "    m_user ";
            sql += " WHERE ";
            sql += "    username = '" + MySqlHelper.EscapeString(username) + "' ";
            sql += "    AND password = '" + MySqlHelper.EscapeString(password) + "' ";
            sql += " LIMIT 1 ";

            DataTable rst = null;

            try
            {
                rst = _db.GetTable(sql);

                if (!Common.TableIsNullOrEmpty(rst))
                {
                    var role = Common.CnvNullToInt(rst.Rows[0]["role"]);
                    var userId = Common.CnvNullToInt(rst.Rows[0]["userid"]);

                    Common.UserLoginId = userId;

                    switch (role)
                    {
                        case (int)Common.emModeApp.Admin:
                            Common.ModeApp = Common.emModeApp.Admin;
                            break;
                        case (int)Common.emModeApp.User:
                            Common.ModeApp = Common.emModeApp.User;
                            break;
                    }

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                _lastSqlQuery = _db.LastSqlQuery;
                Common.TableDispose(ref rst);
            }
        }

        public DataTable GetMUser()
        {
            string sql = string.Empty;

            sql += " SELECT ";
            sql += "    * ";
            sql += " FROM ";
            sql += "    m_user ";
            sql += " ORDER BY ";
            sql += "    role ASC ";
            sql += "   ,userid ASC ";

            return xGetTable(sql);
        }

        public bool DeleteUserById(int userId)
        {
            string sql = "DELETE FROM m_user WHERE userid = '" + MySqlHelper.EscapeString(userId.ToString()) + "'";
            return xExecuteNonQuery(sql);
        }

        public bool InsertUser(string username, string password, string fullname, string email, int role)
        {
            Dictionary<string, object> insertData = new Dictionary<string, object>();
            insertData.Add("username", username);
            insertData.Add("password", password);
            insertData.Add("fullname", fullname);
            insertData.Add("email", email);
            insertData.Add("role", role);

            try
            {
                return _db.InsertTable("m_user", insertData);
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                _lastSqlQuery = _db.LastSqlQuery;
                insertData = null;
            }
        }

        public bool UpdateUser(int userid, string username = "", string password = "", string fullname = "", string email = "", int role = -1)
        {
            Dictionary<string, object> updateData = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(username))
            {
                updateData.Add("username", username);
            }

            if (!string.IsNullOrEmpty(password))
            {
                updateData.Add("password", password);
            }

            if (!string.IsNullOrEmpty(fullname))
            {
                updateData.Add("fullname", fullname);
            }

            if (!string.IsNullOrEmpty(email))
            {
                updateData.Add("email", email);
            }

            if (role > -1)
            {
                updateData.Add("role", role);
            }

            try
            {
                return _db.UpdateTable("m_user", updateData, ("userid = " + userid));
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                _lastSqlQuery = _db.LastSqlQuery;
                updateData = null;
            }
        }

        public bool ChangePasswordUser(int userid, string passwordCurrent, string passwordNew)
        {
            if (string.IsNullOrEmpty(passwordCurrent) || string.IsNullOrEmpty(passwordNew))
            {
                return false;
            }

            Dictionary<string, object> updateData = new Dictionary<string, object>();
            updateData.Add("password", passwordNew);

            try
            {
                return _db.UpdateTable("m_user", updateData, ("userid = " + userid + " AND password = '" + MySqlHelper.EscapeString(passwordCurrent) + "'"));
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                _lastSqlQuery = _db.LastSqlQuery;
                updateData = null;
            }
        }
        
        public bool ResetDefauleDevice(int deviceid, int devicetype)
        {
            string sql = string.Empty;

            sql += " SELECT ";
            sql += "    * ";
            sql += " FROM ";
            sql += "    m_device ";
            sql += " WHERE ";
            sql += "    userid IS NULL ";
            sql += "    AND device_type = " + devicetype;
            sql += " LIMIT 1 ";

            DataTable defaultDevice = null;
            Dictionary<string, object> deviceData = null;

            try
            {
                defaultDevice = _db.GetTable(sql);

                if (Common.TableIsNullOrEmpty(defaultDevice))
                {
                    return false;
                }

                deviceData = new Dictionary<string, object>();
                deviceData.Add("device_name", Common.CnvNullToString(defaultDevice.Rows[0]["device_name"]));
                deviceData.Add("ip_address", Common.CnvNullToString(defaultDevice.Rows[0]["ip_address"]));
                deviceData.Add("port", Common.CnvNullToInt(defaultDevice.Rows[0]["port"]));
                deviceData.Add("alarm_value", Common.CnvNullToInt(defaultDevice.Rows[0]["alarm_value"]));
                deviceData.Add("period", Common.CnvNullToInt(defaultDevice.Rows[0]["period"]));
                deviceData.Add("fail_level", Common.CnvNullToInt(defaultDevice.Rows[0]["fail_level"]));
                deviceData.Add("samples", Common.CnvNullToInt(defaultDevice.Rows[0]["samples"]));
                deviceData.Add("active", Common.CnvNullToInt(defaultDevice.Rows[0]["active"]));

                return _db.UpdateTable("m_device", deviceData, " device_id = " + deviceid);
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                _lastSqlQuery = _db.LastSqlQuery;
                deviceData = null;
                Common.TableDispose(ref defaultDevice);
            }
        }

        public DataTable GetDeviceList(bool getAll = true)
        {
            string sql = string.Empty;
            
            sql += " SELECT ";
            sql += "    * ";
            sql += " FROM ";
            sql += "    m_device ";
            sql += " WHERE ";
            sql += "    {0} " + (!getAll ? " AND device_type > 0" : "");
            sql += " ORDER BY ";
            sql += "    device_type ASC ";

            DataTable rst = null;
            DataTable defaultDevice = null;
            Dictionary<string, object> deviceData = null;

            try
            {
                rst = _db.GetTable(string.Format(sql, " userid = " + userGetDeviceList));

                if (Common.TableIsNullOrEmpty(rst) && getAll)
                {
                    defaultDevice = _db.GetTable(string.Format(sql, " userid IS NULL "));

                    if (!Common.TableIsNullOrEmpty(defaultDevice))
                    {
                        _db.BeginTransaction();

                        foreach (DataRow row in defaultDevice.Rows)
                        {
                            deviceData = new Dictionary<string, object>();
                            deviceData.Add("device_name", Common.CnvNullToString(row["device_name"]));
                            deviceData.Add("ip_address", Common.CnvNullToString(row["ip_address"]));
                            deviceData.Add("port", Common.CnvNullToInt(row["port"]));
                            deviceData.Add("alarm_value", Common.CnvNullToInt(row["alarm_value"]));
                            deviceData.Add("period", Common.CnvNullToInt(row["period"]));
                            deviceData.Add("fail_level", Common.CnvNullToInt(row["fail_level"]));
                            deviceData.Add("samples", Common.CnvNullToInt(row["samples"]));
                            deviceData.Add("active", Common.CnvNullToInt(row["active"]));
                            deviceData.Add("device_type", Common.CnvNullToInt(row["device_type"]));
                            deviceData.Add("userid", userGetDeviceList);

                            if (!_db.InsertTable("m_device", deviceData, false))
                            {
                                _db.Rollback();
                                return rst;
                            }
                        }

                        _db.Commit();

                        rst = _db.GetTable(string.Format(sql, " userid = " + userGetDeviceList));
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _lastSqlQuery = _db.LastSqlQuery;
                Common.TableDispose(ref defaultDevice);
                deviceData = null;
            }

            return rst;
        }

        public bool SaveDevice(int deviceId, 
                               string deviceName = "", 
                               string ipAddress = "", 
                               int port = -1, 
                               int alarmValue = -1,
                               int period = -1,
                               int failLevel = -1,
                               int samples = -1,
                               bool active = true,
                               emDeviceType deviceType = emDeviceType.None)
        {
            string sql = " SELECT device_id FROM m_device WHERE device_id = " + deviceId + " AND userid = " + userGetDeviceList + " LIMIT 1 ";
            Dictionary<string, object> deviceData = new Dictionary<string, object>();

            deviceData.Add("device_name", deviceName);
            deviceData.Add("ip_address", ipAddress);
            deviceData.Add("port", port);
            deviceData.Add("alarm_value", alarmValue);
            deviceData.Add("period", period);
            deviceData.Add("fail_level", failLevel);
            deviceData.Add("samples", samples);
            deviceData.Add("active", active);
            deviceData.Add("device_type", (int)deviceType);
            deviceData.Add("userid", userGetDeviceList);

            DataTable checkExist = null;

            try
            {
                checkExist = deviceId > 0 ? _db.GetTable(sql) : null;

                if (Common.TableIsNullOrEmpty(checkExist))
                {
                    return _db.InsertTable("m_device", deviceData);
                }
                else
                {
                    return _db.UpdateTable("m_device", deviceData, " device_id = " + deviceId);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                _lastSqlQuery = _db.LastSqlQuery;
                Common.TableDispose(ref checkExist);
            }
        }

        public int InsertMeasure(int deviceId, int measureType, int alarmValue, int period, int failLevel, int samples, DateTime startTime)
        {
            int intRst = 0;

            Dictionary<string, object> insertData = new Dictionary<string, object>();
            insertData.Add("device_id", deviceId);
            insertData.Add("measure_type", measureType);
            insertData.Add("alarm_value", alarmValue);
            insertData.Add("period", period);
            insertData.Add("fail_level", failLevel);
            insertData.Add("samples", samples);
            insertData.Add("start_time", startTime.ToString(cstrDateTimeFormatNoMiliSecond));
            insertData.Add("end_time", string.Empty);
            insertData.Add("userid", Common.UserLoginId);

            try
            {
                return _db.InsertTable("tbl_measure", insertData) ? (int)_db.InsertLastId : intRst;
            }
            catch (Exception ex)
            {
                return intRst;
            }
            finally
            {
                _lastSqlQuery = _db.LastSqlQuery;
                insertData = null;
            }
        }
        
        public bool UpdateEndTimeMeasure(int measureId, DateTime endTime, int measureResult)
        {
            Dictionary<string, object> updateData = new Dictionary<string, object>();
            updateData.Add("end_time", endTime.ToString(cstrDateTimeFormatNoMiliSecond));
            updateData.Add("result", measureResult);

            try
            {
                return _db.UpdateTable("tbl_measure", updateData, " measure_id = " + measureId.ToString());
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                _lastSqlQuery = _db.LastSqlQuery;
                updateData = null;
            }
        }

        public string GetUpdateEndTimeMeasure(int measureId, DateTime endTime, int measureResult)
        {
            Dictionary<string, object> updateData = new Dictionary<string, object>();
            updateData.Add("end_time", endTime.ToString(cstrDateTimeFormatNoMiliSecond));
            updateData.Add("result", measureResult);

            return _db.CreateUpdateSql("tbl_measure", updateData, " measure_id = " + measureId.ToString());
        }

        public string GetInsertMeasureDetailSql(int deviceId, int measureId, DateTime samplesTime, string actualValue, int actualMaxValue, int actualMinValue, int actualDelegate, int result)
        {
            Dictionary<string, object> insertData = new Dictionary<string, object>();
            insertData.Add("measure_id", measureId);
            insertData.Add("samples_time", samplesTime.ToString(cstrDateTimeFormatNoMiliSecond));
            insertData.Add("device_id", deviceId);
            insertData.Add("actual_value", actualValue);
            insertData.Add("actual_max_value", actualMaxValue);
            insertData.Add("actual_min_value", actualMinValue);
            insertData.Add("actual_delegate", actualDelegate);
            insertData.Add("result", result);

            return _db.CreateInsertSql("tbl_measure_detail", insertData);
        }

        public string GetInsertMeasureDetailLimitSql(int deviceId, int measureId, DateTime samplesTime, string actualValue, int actualMaxValue, int actualMinValue, int actualDelegate, int result)
        {
            Dictionary<string, object> insertData = new Dictionary<string, object>();
            insertData.Add("measure_id", measureId);
            insertData.Add("samples_time", samplesTime.ToString(result == (int)emMeasureResult.Pass ? cstrDateTimeFormatMiliSecond : cstrDateTimeFormatNoMiliSecond));
            insertData.Add("device_id", deviceId);
            insertData.Add("actual_value", actualValue);
            insertData.Add("actual_max_value", actualMaxValue);
            insertData.Add("actual_min_value", actualMinValue);
            insertData.Add("actual_delegate", actualDelegate);
            insertData.Add("result", result);

            return _db.CreateInsertSql("tbl_measure_detail_limit", insertData);
        }

        public string GetInsertMeasureDetailRawSql(int deviceId, int measureId, DateTime samplesTime, string actualValue, int actualMaxValue, int actualMinValue, int actualDelegate, int result)
        {
            Dictionary<string, object> insertData = new Dictionary<string, object>();
            insertData.Add("measure_id", measureId);
            insertData.Add("samples_time", samplesTime.ToString(cstrDateTimeFormatNoMiliSecond));
            insertData.Add("device_id", deviceId);
            insertData.Add("actual_value", actualValue);
            insertData.Add("actual_max_value", actualMaxValue);
            insertData.Add("actual_min_value", actualMinValue);
            insertData.Add("actual_delegate", actualDelegate);
            insertData.Add("result", result);

            return _db.CreateInsertSql("tbl_measure_detail_raw", insertData);
        }

        public bool InsertMeasureDetail(int deviceId, int measureId, DateTime samplesTime, string actualValue, int actualMaxValue, int actualMinValue, int actualDelegate, int result)
        {
            return xInsertMeasureDetail("tbl_measure_detail", deviceId, measureId, samplesTime, actualValue, actualMaxValue, actualMinValue, actualDelegate, result);
        }

        public bool InsertMeasureDetailLimit(int deviceId, int measureId, DateTime samplesTime, string actualValue, int actualMaxValue, int actualMinValue, int actualDelegate, int result)
        {
            return xInsertMeasureDetail("tbl_measure_detail_limit", deviceId, measureId, samplesTime, actualValue, actualMaxValue, actualMinValue, actualDelegate, result);
        }

        public bool InsertMeasureDetailRaw(int deviceId, int measureId, DateTime samplesTime, string actualValue, int actualMaxValue, int actualMinValue, int actualDelegate, int result)
        {
            return xInsertMeasureDetail("tbl_measure_detail_raw", deviceId, measureId, samplesTime, actualValue, actualMaxValue, actualMinValue, actualDelegate, result);
        }

        public bool ExecuteNonQueryFromFile(string path, bool deleteFile = true)
        {
            try
            {
                if (!File.Exists(path))
                {
                    return false;
                }

                _db.BeginTransaction();

                using (var read = new StreamReader(path, System.Text.Encoding.UTF8, true, 128))
                {
                    string line = string.Empty;

                    while ((line = read.ReadLine()) != null)
                    {
                        if (!_db.ExecuteNonQuery(line, false))
                        {
                            _db.Rollback();
                            return false;
                        }
                    }
                }

                _db.Commit();

                if (deleteFile)
                {
                    try
                    {
                        File.Delete(path);
                    }
                    catch (Exception)
                    {
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }    
        }

        public DataTable GetTBLMeasure(string sDate = null, string eDate = null, int? user = null, int? type = null, int? result = null, int? status = null)
        {
            string sql = string.Empty;
            string strSearch = string.Empty;

            if(!string.IsNullOrEmpty(sDate))
            {
                strSearch += "AND measure.start_time >= '"+sDate+"' ";
            }

            if (!string.IsNullOrEmpty(eDate) && status != 1)
            {
                strSearch += "AND measure.end_time <= '" + eDate + "' ";
            }

            if (user != null && user != -1)
            {
                strSearch += "AND measure.userid = '" + user + "' ";
            }

            if (type != null && type != -1)
            {
                strSearch += "AND measure.measure_type = '" + type + "' ";
            }

            if (result != null && result != -1)
            {
                strSearch += "AND measure.result = '" + result + "' ";
            }

            sql += " SELECT ";
            sql += "    measure.*, ";
            sql += "    user.fullname, ";
            sql += "    device.device_name ";
            sql += " FROM ";
            sql += "    tbl_measure AS measure ";
            sql += " INNER JOIN m_user AS user ON measure.userid = user.userid ";
            sql += " INNER JOIN m_device AS device ON measure.device_id = device.device_id ";
            
            if(status == 0)
            {
                if (string.IsNullOrEmpty(eDate))
                {
                    sql += " WHERE ";
                    sql += " measure.end_time IS NOT NULL ";
                }
                else if (strSearch != "")
                {
                    sql += " WHERE ";
                    sql += strSearch.Substring(3);
                }
            } else
            {
                sql += " WHERE ";                
                if (strSearch != "")
                {
                    sql += strSearch.Substring(3);
                }
                sql += "AND measure.end_time IS NULL ";
            }
            

            sql += " ORDER BY ";
            sql += "    measure.measure_id DESC ";

            return xGetTable(sql);
        }

        public DataTable GetTBLMeasureDetail(string measure_id, bool status)
        {
            if (measure_id == "") return null;

            string sql = string.Empty;

            sql += " SELECT ";
            sql += "    measure_detail.* ";
            sql += " FROM ";
            sql += "    tbl_measure_detail AS measure_detail ";
            sql += " WHERE measure_detail.measure_id = " + measure_id;
            if(status)
            {
                sql += string.Format(" AND (result = '{0}' OR result = '{1}') ", (int)emMeasureResult.Fail, (int)emMeasureResult.Alarm);
            }
            sql += " ORDER BY ";
            sql += "    measure_detail.measure_id DESC ";

            return xGetTable(sql);
        }

        public DataTable GetTBLMeasureDetailLimit(string measure_id)
        {
            if (measure_id == "") return null;

            string sql = string.Empty;

            sql += " SELECT ";
            sql += "    measure_detail_limit.* ";
            sql += " FROM ";
            sql += "    tbl_measure_detail_limit AS measure_detail_limit ";
            sql += " WHERE measure_detail_limit.measure_id = " + measure_id;
            sql += " ORDER BY ";
            sql += "    measure_detail_limit.samples_time";

            return xGetTable(sql);
        }

        public bool DeleteMeasure(int measureId)
        {
            string sql = "DELETE FROM tbl_measure WHERE measure_id = '" + MySqlHelper.EscapeString(measureId.ToString()) + "';";
            sql += "DELETE FROM tbl_measure_detail WHERE measure_id = '" + MySqlHelper.EscapeString(measureId.ToString()) + "';";
            sql += "DELETE FROM tbl_measure_detail_limit WHERE measure_id = '" + MySqlHelper.EscapeString(measureId.ToString()) + "';";
            sql += "DELETE FROM tbl_measure_detail_raw WHERE measure_id = '" + MySqlHelper.EscapeString(measureId.ToString()) + "';";
            
            return xExecuteNonQuery(sql);
        }

        public bool DeleteMeasureRaw(int measureId)
        {
            if (measureId < 0)
            {
                return false;
            }

            string sql = "DELETE FROM tbl_measure_detail_raw WHERE measure_id = '" + MySqlHelper.EscapeString(measureId.ToString()) + "'";

            return xExecuteNonQuery(sql);
        }

        #region Private Function

        private bool xExecuteNonQuery(string sql)
        {
            try
            {
                return _db.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                _lastSqlQuery = _db.LastSqlQuery;
            }
        }

        private DataTable xGetTable(string sql)
        {
            DataTable rst = null;
            try
            {
                rst = _db.GetTable(sql);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _lastSqlQuery = _db.LastSqlQuery;
            }
            return rst;
        }

        private bool xInsertMeasureDetail(string tableName, int deviceId, int measureId, DateTime samplesTime, string actualValue, int actualMaxValue, int actualMinValue, int actualDelegate, int result)
        {
            Dictionary<string, object> insertData = new Dictionary<string, object>();
            insertData.Add("measure_id", measureId);
            // insertData.Add("samples_time", samplesTime.ToString(tableName == "tbl_measure_detail_raw" || result == (int)clsDBUltity.emMeasureResult.Pass ? cstrDateTimeFormatMiliSecond : cstrDateTimeFormatNoMiliSecond));
            insertData.Add("samples_time", samplesTime.ToString(cstrDateTimeFormatMiliSecond));
            insertData.Add("device_id", deviceId);
            insertData.Add("actual_value", actualValue);
            insertData.Add("actual_max_value", actualMaxValue);
            insertData.Add("actual_min_value", actualMinValue);
            insertData.Add("actual_delegate", actualDelegate);
            insertData.Add("result", result);

            try
            {
                return _db.InsertTable(tableName, insertData);
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                _lastSqlQuery = _db.LastSqlQuery;
                insertData = null;
            }
        }

        #endregion Private Function

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (_db != null)
                    {
                        if (_blnCreateNew)
                        {
                            _db.Close();
                        }
                        _db.Dispose();
                        _db = null;
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~clsDBUltity() {
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
        #endregion
    }
}
