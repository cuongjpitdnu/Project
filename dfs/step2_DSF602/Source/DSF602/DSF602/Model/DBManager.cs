using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using BaseCommon;
using DSF602.Class;

namespace DSF602.Model
{
    public class DBManager : DBCoreDapper
    {
        public DBManager(IDbConnection dbConnection) : base(dbConnection)
        {
        }

        #region User Model

        public MUser GetUserLoginInfo(string userName, string pass)
        {
            var objUser = _dbConnection.QueryFirstOrDefault<MUser>("SELECT * FROM MUser WHERE UserName = @UserName AND Password = @Password LIMIT 1 ", new MUser
            {
                UserName = userName,
                Password = pass,
            });
            return objUser;
        }

        public List<MUser> GetMUser()
        {
            var listUser = _dbConnection.Query<MUser>(@"SELECT * FROM MUser  ORDER BY Role DESC, UserId ASC").ToList();
            return listUser;
        }

        public bool InserUser(MUser mUser)
        {
            var intRst = Insert("MUser", new
            {
                UserName = mUser.UserName,
                Password = mUser.Password,
                FullName = mUser.FullName,
                Email = mUser.Email,
                UpdateTime = mUser.UpdateTime,
                Role = mUser.Role,
            });

            return intRst > 0;
        }

        public bool UpdateUser(MUser mUser)
        {
            var intRst = Update("MUser", new string[] { "UserId" }, new
            {
                UserId = mUser.UserId,
                UserName = mUser.UserName,
                Password = mUser.Password,
                FullName = mUser.FullName,
                Email = mUser.Email,
                UpdateTime = mUser.UpdateTime,
                Role = mUser.Role,
            });

            return intRst > 0;
        }

        public bool DeleteUserById(int userId)
        {
            var intRst = _dbConnection.Execute("DELETE FROM MUser WHERE UserId = @userId", new MUser
            {
                UserId = userId,
            });
            return intRst > 0;
        }

        public bool ChangePassUser(int userid, string passwordNew)
        {
            if (string.IsNullOrEmpty(passwordNew))
            {
                return false;
            }

            var intRst = Update("MUser", new string[] { "UserId" }, new
            {
                UserId = userid,
                Password = passwordNew,
                UpdateTime = DateTime.Now,
            });

            return intRst > 0;
        }

        public void SetDefaultValues(SettingParam defValues)
        {
            var lstBlock = GetListBlock();
            var lstSensor = GetListSensor();
            foreach(Block bl in lstBlock)
            {
                if (string.IsNullOrEmpty(bl.DefaultParams))
                {
                    var defString = Newtonsoft.Json.JsonConvert.SerializeObject(defValues);
                    string sql = string.Format("UPDATE Block SET DefaultParams = '{0}' WHERE BlockId = '{1}'", defString, bl.BlockId);
                    ExecuteNonQuery(sql);
                }
            }

            foreach(SensorInfo sensor in lstSensor)
            {
                if (sensor.DecayUpperValue == 0)
                {
                    sensor.DecayUpperValue = defValues.UpVal;
                    sensor.DecayLowerValue = defValues.LowVal;
                    sensor.DecayTimeCheck = defValues.DecayTimeCheck;
                    sensor.DecayStopTime = defValues.StopDecayTime;
                    sensor.IonValueCheck = defValues.IonBalanceCheck;
                    sensor.IonTimeCheck = defValues.IonStopTimeCheck;
                    sensor.AutoCheckFlag = 1;
                    sensor.AutoCheckTime = string.Join(",", defValues.AutoCheckTimes);
                    sensor.AutoCheckDays = string.Join(",", defValues.AutoCheckDays);

                    UpdateSensor(sensor, sensor.Ordinal_Display, sensor.Ordinal_Display, false);
                }
            }
        }

        #endregion User Model

        #region Device Model

        public Block GetBlockById(int blockId)
        {
            return _dbConnection.QueryFirstOrDefault<Block>("SELECT * FROM Block WHERE BlockId = @BlockId", new { BlockId = blockId });
        }

        public SensorInfo GetSensorById(int sensorId)
        {
            var sensorInfo = _dbConnection.QueryFirstOrDefault<SensorInfo>("SELECT * FROM Sensors WHERE SensorId =  @SensorId", new SensorInfo { SensorId = sensorId });
            return sensorInfo;
        }

        public List<Block> GetListBlock()
        {
            return _dbConnection.Query<Block>("SELECT * FROM Block ORDER BY BlockId").ToList();
        }

        public IEnumerable<SensorInfo> GetIESensor(int blockId = -1)
        {
            var sql = " SELECT * FROM Sensors ";

            if (blockId > 0)
            {
                sql += " WHERE OfBlock = @BlockId ";
                return _dbConnection.Query<SensorInfo>(sql, new { BlockId = blockId });
            }

            return _dbConnection.Query<SensorInfo>(sql);
        }

        public List<SensorInfo> GetListSensor(int blockId = -1)
        {
            var sql = " SELECT * FROM Sensors ";

            if (blockId > 0)
            {
                sql += " WHERE OfBlock = @BlockId ";
                return _dbConnection.Query<SensorInfo>(sql, new { BlockId = blockId }).ToList();
            }

            return _dbConnection.Query<SensorInfo>(sql).ToList();
        }

        public bool InsertSensor(SensorInfo sensor)
        {
            var intRst = Insert("Sensors", new
            {
                SensorId = sensor.SensorId,
                SensorName = sensor.SensorName,
                Alarm_Value = sensor.Alarm_Value,
                Ordinal_Display = sensor.Ordinal_Display,
                OfBlock = sensor.OfBlock,
                Active = sensor.Active,
                UpdateTime = DateTime.Now,
                UpdateBy = AppManager.UserLogin.UserId,
            });

            return intRst > 0;
        }

        public bool UpdateSensor(SensorInfo sensor)
        {
            if (sensor == null) return true;

            var intRst = Update("Sensors", new string[] { "SensorId" }, new
            {
                SensorId = sensor.SensorId,
                SensorName = sensor.SensorName,
                Alarm_Value = sensor.Alarm_Value,
                Ordinal_Display = sensor.Ordinal_Display,
                OfBlock = sensor.OfBlock,
                Active = sensor.Active,
                UpdateTime = DateTime.Now,
                UpdateBy = AppManager.UserLogin.UserId
            });

            return intRst > 0;
        }

        public bool UpdateSensor(SensorInfo sensor, int currentOrdinalDisplay, int swapOrdinalDisplay, bool flag, int userId = 1)
        {
            if (sensor == null) return true;

            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
            if (AppManager.UserLogin != null)
            {
                userId = AppManager.UserLogin.UserId;
            }
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    var intRst = Update("Sensors", new string[] { "SensorId" }, new
                    {
                        SensorId = sensor.SensorId,
                        SensorName = sensor.SensorName,
                        Alarm_Value = sensor.Alarm_Value,
                        Ordinal_Display = swapOrdinalDisplay,
                        OfBlock = sensor.OfBlock,
                        Active = sensor.Active,
                        UpdateTime = DateTime.Now,
                        UpdateBy = userId,
                        MeasureType = sensor.MeasureType,
                        DecayUpperValue = sensor.DecayUpperValue,
                        DecayLowerValue = sensor.DecayLowerValue,
                        DecayTimeCheck = sensor.DecayTimeCheck,
                        DecayStopTime = sensor.DecayStopTime,
                        IonValueCheck = sensor.IonValueCheck,
                        IonTimeCheck = sensor.IonTimeCheck,
                        AutoCheckFlag = sensor.AutoCheckFlag,
                        AutoCheckTime = sensor.AutoCheckTime,
                        AutoCheckDays = sensor.AutoCheckDays
                    });


                    var sql = string.Empty;
                    sql += flag
                        ? string.Format("UPDATE Sensors SET  Ordinal_Display = Ordinal_Display + 1 WHERE Ordinal_Display >= {0} AND Ordinal_Display <= {1}  AND SensorId <> {2} AND OfBlock = {3}", swapOrdinalDisplay, currentOrdinalDisplay, sensor.SensorId, sensor.OfBlock)
                        : string.Format("UPDATE Sensors SET  Ordinal_Display = Ordinal_Display - 1 WHERE Ordinal_Display >= {0} AND Ordinal_Display <= {1}  AND SensorId <> {2} AND OfBlock = {3}", currentOrdinalDisplay, swapOrdinalDisplay, sensor.SensorId, sensor.OfBlock);

                    var intResul = _dbConnection.Execute(sql);

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public bool UpdateBlock(Block block)
        {
            if (block == null) return true;

            var intRst = Update("Block", new string[] { "BlockId" }, new
            {
                BlockId = block.BlockId,
                BlockName = block.BlockName,
                Ip_Address = block.Ip_Address,
                Port = block.Port,
                Active = block.Active,
                UpdateTime = DateTime.Now,
                UpdateBy = AppManager.UserLogin.UserId,
            });

            return intRst > 0;
        }

        public bool UpdateBlockDefaultParam(Block block)
        {
            if (block == null) return true;

            var intRst = Update("Block", new string[] { "BlockId" }, new
            {
                BlockId = block.BlockId,
                DefaultParams = block.DefaultParams,
                UpdateTime = DateTime.Now,
                UpdateBy = AppManager.UserLogin.UserId,
            });

            return intRst > 0;
        }


        #endregion Device Model


        #region Tab Data

        public int InsertMeasure(Measure measure)
        {
            var intRst = _dbConnection.Execute("INSERT INTO Measure( SensorId, Measure_Type, Alarm_Value, Start_time, End_time, Measure_Result, UserId, UpdateTime, Delete_Flag) VALUES ( @SensorId, @Measure_Type, @Alarm_Value, @Start_time, @End_time, @Measure_Result, @UserId, @UpdateTime, @Delete_Flag) ",
                new Measure
                {
                    //MeasureId = measure.MeasureId,
                    SensorId = measure.SensorId,
                    Measure_Type = measure.Measure_Type = 0,
                    Alarm_Value = measure.Alarm_Value,
                    Start_time = measure.Start_time,
                    End_time = measure.End_time,
                    Measure_Result = measure.Measure_Result,
                    UserId = measure.UserId,
                    UpdateTime = measure.UpdateTime,
                    Delete_Flag = measure.Delete_Flag = 0,
                });
            var resutl = _dbConnection.Query<int>(@"select seq from sqlite_sequence where name='Measure'").Single();
            return intRst > 0 ? resutl : 0;
        }

        public bool UpdateEndTimeMeasure(int measureId, DateTime endTime, int measureResult = -1)
        {
            var intRst = measureResult < 0
                        ? _dbConnection.Execute("UPDATE Measure SET End_time = @End_time WHERE MeasureId = @MeasureId", new { MeasureId = measureId, End_time = endTime })
                        : _dbConnection.Execute("UPDATE Measure SET End_time = @End_time, Measure_Result = @Measure_Result WHERE MeasureId = @MeasureId", new { MeasureId = measureId, End_time = endTime, Measure_Result = measureResult });
            return intRst > 0;
        }

        public bool InsertDetailMeasure(MeasureDetail measure_detail)
        {
            var intRst = _dbConnection.Execute("INSERT INTO Measure_Detail(MeasureId,Actual_Value, Samples_time, Detail_Result) VALUES (@MeasureId, @Actual_Value, @Samples_time, @Detail_Result) ", new MeasureDetail
            {
                //DetailId = measure_detail.DetailId,
                MeasureId = measure_detail.MeasureId,
                Actual_Value = measure_detail.Actual_Value,
                Samples_time = measure_detail.Samples_time,
                Detail_Result = measure_detail.Detail_Result,
            });
            return intRst > 0;
        }

        public List<DGVMeasureInfo> GetTBLMeasure(string sDate = null, string eDate = null, int? user = null, int? result = null)
        {
            string sql = string.Empty;
            string strSearch = string.Empty;

            if (!string.IsNullOrEmpty(sDate))
            {
                strSearch += "AND measure.Start_time >= '" + sDate + "' ";
            }

            if (!string.IsNullOrEmpty(eDate))
            {
                strSearch += "AND measure.End_time <= '" + eDate + "' ";
            }

            if (user != null && user != -1)
            {
                strSearch += "AND measure.UserId = '" + user + "' ";
            }

            if (result != null && result != -1)
            {
                strSearch += "AND measure.Measure_Result = '" + result + "' ";
            }

            sql += " SELECT ";
            sql += "    measure.*, ";
            sql += "    users.FullName, ";
            sql += "    sensors.SensorName ";
            sql += " FROM ";
            sql += "    Measure AS measure ";
            sql += " INNER JOIN MUser AS users ON measure.UserId = users.UserId ";
            sql += " INNER JOIN Sensors AS sensors ON measure.SensorId = sensors.SensorId ";

            if (string.IsNullOrEmpty(eDate))
            {
                sql += " WHERE Delete_Flag = 0 ";
                sql += " measure.End_time IS NOT NULL ";
            }
            else if (strSearch != "")
            {
                sql += " WHERE Delete_Flag = 0 ";
                sql += strSearch;
            }

            sql += " ORDER BY ";
            sql += "    measure.MeasureId DESC ";

            return _dbConnection.Query<DGVMeasureInfo>(sql).ToList();
        }

        public bool DeleteMeasure(int measureId)
        {
            string sql = "UPDATE Measure SET Delete_Flag = 1 WHERE MeasureId = '" + measureId.ToString() + "';";

            return ExecuteNonQuery(sql);
        }

        public List<MeasureDetail> GetTBLMeasureDetail(string measure_id, bool status, DateTime? dtS = null, DateTime? dtE = null)
        {
            if (string.IsNullOrEmpty(measure_id))
            {
                return null;
            }

            var sql = string.Empty;

            sql += " SELECT ";
            sql += "    measure_detail.* ";
            sql += " FROM ";
            sql += " Measure_Detail AS measure_detail ";
            sql += " INNER JOIN Measure AS measure ON measure.MeasureId = measure_detail.MeasureId ";
            sql += " WHERE ";
            sql += "    measure_detail.MeasureId = " + measure_id;

            sql += "    " + (status ? " AND measure.Alarm_Value <= measure_detail.Actual_Value " : " ");

            if (dtS != null)
            {
                var dtTemp = (DateTime)dtS;
                sql += " AND measure_detail.samples_time >= '" + dtTemp.ToString(clsConst.cstrDateTimeFormatNoMiliSecond) + "' ";
            }

            if (dtE != null)
            {
                var dtTemp = (DateTime)dtE;
                sql += " AND measure_detail.samples_time <= '" + dtTemp.ToString(clsConst.cstrDateTimeFormatMiliSecond) + "' ";
            }

            sql += " ORDER BY ";
            sql += "    measure_detail.samples_time ASC ";

            return _dbConnection.Query<MeasureDetail>(sql).ToList();
        }

        #endregion

        public bool ExecuteNonQuery(string sql, bool sqlTrans = true)
        {

            try
            {
                _dbConnection.Open();

                if (string.IsNullOrEmpty(sql))
                {
                    return false;
                }


                using (var command = _dbConnection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            _dbConnection.Close();
            return true;
        }
    }
}
