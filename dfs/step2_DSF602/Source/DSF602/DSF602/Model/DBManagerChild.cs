using BaseCommon;
using Dapper;
using DSF602.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DSF602.Model
{
    public class DBManagerChild : DBCoreDapper
    {
        public readonly string Name;
        private readonly object _syncDB = new object();

        public DBManagerChild(IDbConnection dbConnection, string strName) : base(dbConnection)
        {
            this.Name = strName;
        }

        public static string GetDBName(int sensorId)
        {
            return "sensor_" + sensorId + ".db";
        }

        public int InsertMeasure(SensorInfo sensor)
        {
            var now = DateTime.Now;
            var dataInsert = new
            {
                SensorId = sensor.SensorId,
                Alarm_Value = sensor.Alarm_Value,
                Start_time = now,
                //End_time = now,
                Measure_Type = (int)clsConst.emMeasureType.AlarmTest,
                Measure_Result = (int)clsConst.emMeasureResult.Pass,
                UserId = AppManager.UserLogin.UserId,
                UpdateTime = now,
                Delete_Flag = 0
            };

            var sqlInsert = GenerateSqlInsert("Measure", dataInsert);

            var sqlGetLastId = " SELECT seq FROM sqlite_sequence where name='Measure'; ";

            var sql = string.Empty;
            sql += " BEGIN; ";
            sql += sqlInsert;
            sql += sqlGetLastId;
            sql += " END; ";

            lock (_syncDB)
            {
                return _dbConnection.QueryFirst<int>(sql, dataInsert);
            }
        }

        public bool UpdateEndTimeMeasure(int measureId, DateTime endTime, int measureResult = -1)
        {
            lock (_syncDB)
            {
                var intRst = measureResult < 0
                        ? _dbConnection.Execute("UPDATE Measure SET End_time = @End_time WHERE MeasureId = @MeasureId", new { MeasureId = measureId, End_time = endTime })
                        : _dbConnection.Execute("UPDATE Measure SET End_time = @End_time, Measure_Result = @Measure_Result WHERE MeasureId = @MeasureId", new { MeasureId = measureId, End_time = endTime, Measure_Result = measureResult });
                return intRst > 0;
            }
        }

        public int InsertMeasureDetail(MeasureDetail detail)
        {
            var sql = string.Empty;
            sql += string.Format(" INSERT INTO Measure_Detail (MeasureId, Actual_Value, Samples_time, Detail_Result) VALUES ({0}, {1}, '{2}', {3}); ",
                                    detail.MeasureId, detail.Actual_Value, detail.Samples_time?.ToString("yyyy-MM-dd HH:mm:ss.fff"), detail.Detail_Result);
            lock (_syncDB)
            {
                return _dbConnection.Execute(sql);
            }
        }

        public int InsertMeasureDetailBulk(List<MeasureDetail> measureDetails)
        {
            //var sql = string.Empty;
            //sql += " BEGIN; ";

            //measureDetails.Where(i => i != null).Select((detail) =>
            //{
            //    sql += string.Format(" INSERT INTO Measure_Detail (MeasureId, Actual_Value, Samples_time, Detail_Result) VALUES ({0}, {1}, '{2}', {3}); ",
            //                        detail.MeasureId, detail.Actual_Value, detail.Samples_time?.ToString("yyyy-MM-dd HH:mm:ss.fff"), detail.Detail_Result);
            //    //sql += string.Format(" UPDATE Measure SET End_time = '{0}', Measure_Result = {1} WHERE MeasureId = {2}; ",
            //    //                    detail.Samples_time?.ToString("yyyy-MM-dd HH:mm:ss.fff"), detail.Result_Measure, detail.MeasureId);
            //    return detail;
            //}).ToList();

            //sql += " END; ";

            var dataInsert = measureDetails != null ? measureDetails.Where(i => i != null).ToList() : null;

            if (dataInsert == null || dataInsert.Count == 0)
            {
                return 0;
            }

            var sql = " INSERT INTO Measure_Detail (MeasureId, Actual_Value, Samples_time, Detail_Result) VALUES ";

            sql += string.Join(",", measureDetails.Select(
                detail => string.Format(" ({0}, {1}, '{2}', {3}) ", detail.MeasureId, detail.Actual_Value, detail.Samples_time?.ToString("yyyy-MM-dd HH:mm:ss.fff"), detail.Detail_Result)
            ).ToArray());

            sql += "; ";

            lock (_syncDB)
            {
                return _dbConnection.Execute(sql);
            }
        }

        public IEnumerable<Measure> GetAllMeasure(string sDate = null, string eDate = null, int? user = null, int? result = null, bool isAlarmReport = true)
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

            if (isAlarmReport)
            {
                strSearch += string.Format("AND measure.Measure_Type = '{0}' OR measure.Measure_Type = '{1}' ", clsConst.MeasureMode_Volt, clsConst.MeasureMode_Ion);
            }
            else
            {
                strSearch += string.Format("AND measure.Measure_Type = '{0}' ", clsConst.MeasureMode_Decay);
            }

            sql += "SELECT * FROM Measure WHERE Delete_Flag = 0 ";

            if (string.IsNullOrEmpty(eDate))
            {
                sql += " measure.End_time IS NOT NULL ";
            }
            else if (strSearch != "")
            {
                sql += strSearch;
            }

            sql += " ORDER BY ";
            sql += "    measure.MeasureId DESC ";

            lock (_syncDB)
            {
                return _dbConnection.Query<Measure>(sql);
            }
        }

        public IEnumerable<MeasureDetail> GetMeasureDetail(string measure_id, bool status, DateTime? dtS = null, DateTime? dtE = null)
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

            sql += "    " + (status ? " AND measure.Alarm_Value <=  abs( measure_detail.Actual_Value )" : " ");

            if (dtS != null)
            {
                var dtTemp = (DateTime)dtS;
                sql += " AND measure_detail.samples_time >= '" + dtTemp.ToString(clsConst.cstrDateTimeFormatNoMiliSecond) + "' ";
            }

            if (dtE != null)
            {
                var dtTemp = (DateTime)dtE;
                //sql += " AND measure_detail.samples_time <= '" + dtTemp.ToString(clsConst.cstrDateTimeFormatMiliSecond) + "' ";
                sql += " AND measure_detail.samples_time <= '" + dtTemp.ToString(clsConst.cstrDateTimeFormatNoMiliSecond + ".999") + "' ";
            }

            sql += " ORDER BY ";
            sql += "    measure_detail.samples_time ASC ";

            lock (_syncDB)
            {
                return _dbConnection.Query<MeasureDetail>(sql);
            }
        }

        public bool DeleteMeasure(int measureId)
        {
            string sql = "UPDATE Measure SET Delete_Flag = 1 WHERE MeasureId = '" + measureId.ToString() + "';";

            lock (_syncDB)
            {
                return _dbConnection.Execute(sql) > 0 ? true : false;
            }
        }
    }
}
