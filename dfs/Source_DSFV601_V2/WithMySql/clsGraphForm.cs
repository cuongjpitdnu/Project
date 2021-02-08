using BaseCommon;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using static BaseCommon.clsConst;

namespace WithMySql
{
    class clsGraphForm : IGraphForm
    {
        //Msg
        private const string MSG_ERR_CONNECT_DB = "Cannot connect to database, please check network or connection information!";
        private const string MSG_ERR_HAS_DB = "Has error in insert data to database";

        #region Variables

        public GraphForm FormSetting { get; set; }
        //public emMeasureType MeasureType { get; set; }
        public int RoundValue
        {
            get
            {
                return Config.RoundValue;
            }
        }

        public int AlarmTime
        {
            get
            {
                return Config.AlarmTime;
            }
        }

        public int WalkingTime
        {
            get
            {
                return Config.WalkingTime;
            }
        }

        private clsDBUltity _DBUltity = new clsDBUltity(true);
        //private bool _hasErrDB = false;
        private string _nameErrFileDetail = string.Empty;
        private string _nameErrFileLimit = string.Empty;
        private string _nameErrFileRaw = string.Empty;
        private int _numberRecordWriteDetail = 0;
        private int _numberRecordWriteLimit = 0;
        private int _numberRecordWriteRaw = 0;
        //private int _measureId;
        private List<DeviceInfo> lstDeviceInfo;
        private Thread threadInsert = null;
        private bool isRunning = true;

        private class DataTemp
        {
            public DataSample Data { get; set; }
            public DeviceInfo Device { get; set; }
        }

        private ConcurrentQueue<DataTemp> lstDateTemp = new ConcurrentQueue<DataTemp>();

        #endregion

        #region public functions

        public clsGraphForm()
        {
            threadInsert = new Thread(new ThreadStart(processInsertData));
            threadInsert.Start();
        }

        public void StopInsertDB()
        {
            isRunning = false;
            if (threadInsert != null)
            {
                threadInsert.Abort();
            }
            threadInsert = null;
        }

        private void processInsertData()
        {
            while (isRunning)
            {
                try
                {
                    if(lstDateTemp != null && lstDateTemp.Count > 0)
                    {
                        lstDateTemp.TryDequeue(out DataTemp temp);

                        DataSample data = temp.Data;
                        DeviceInfo device = temp.Device;

                        if (data.isRaw)
                        {
                            InsertMeasureRaw(data, _DBUltity, device);
                        }
                        else
                        {
                            InsertMeasureDetail(data, _DBUltity, device.MeasureId);

                            if (device.MeasureType == emMeasureType.AlarmTest && Math.Abs(data.actualDelegate) >= Math.Abs(device.AlarmValue))
                            {
                                InsertMeasureLimit(data, _DBUltity, device.MeasureId);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        public void UpdateListDevice(List<DeviceInfo> lstDevice)
        {
            using (var objDb = new clsDBUltity())
            {
                try
                {
                    foreach (var device in lstDevice)
                    {
                        if (!string.IsNullOrEmpty(device.MacAddress))
                        {
                            objDb.SaveDevice(device);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }

            FormSetting?.LoadInfoDevice();
        }

        public List<DeviceInfo> GetListDevice()
        {
            lstDeviceInfo = new List<DeviceInfo>();

            using (var objDb = new clsDBUltity())
            {
                var deviceInfo = objDb.GetDeviceList();

                if (clsCommon.TableIsNullOrEmpty(deviceInfo))
                {
                    return lstDeviceInfo;
                }

                foreach (DataRow row in deviceInfo.Rows)
                {
                    var device = new DeviceInfo();
                    device.DeviceId = clsCommon.CnvNullToInt(row["device_id"]);
                    device.DeviceName = clsCommon.CnvNullToString(row["device_name"]);
                    device.IpAddress = clsCommon.CnvNullToString(row["ip_address"]).Trim();
                    device.Port = clsCommon.CnvNullToInt(row["port"]);
                    device.AlarmValue = clsCommon.CnvNullToInt(row["alarm_value"]);
                    device.Period = clsCommon.CnvNullToInt(row["period"]);
                    device.FailLevel = clsCommon.CnvNullToInt(row["fail_level"]);
                    device.Samples = clsCommon.CnvNullToInt(row["samples"]);
                    device.WalkingMode = clsCommon.CnvNullToInt(row["walking_mode"]) > 0;
                    device.OrdinalDisplay = clsCommon.CnvNullToInt(row["ordinal_display"]);
                    device.Active = clsCommon.CnvNullToInt(row["active"]) > 0;
                    device.MacAddress = clsCommon.CnvNullToString(row["mac_address"]);

                    lstDeviceInfo.Add(device);
                }

                return lstDeviceInfo;
            }
        }

        public Response MeasureStart(DeviceInfo device, int measureResult)
        {
            Response res = new Response();
            res.MeasureId = -1;
            res.PathFolderData = "";

            //Insert measure information
            var _measureId = _DBUltity.InsertMeasure(device.DeviceId,
                                                 (int)device.MeasureType,
                                                 device.AlarmValue,
                                                 device.Period,
                                                 device.FailLevel,
                                                 device.Samples,
                                                 DateTime.Now);

            if (_measureId < 1)
            {
                FormSetting.ShowMsg(MessageBoxIcon.Error, MSG_ERR_CONNECT_DB);
                return res;
            }

            res.MeasureId = _measureId;

            return res;
        }

        public void MeasureProcess(DataSample data, DeviceInfo device)
        {
            var dataTemp = new DataTemp
            {
                Data = data,
                Device = device
            };

            lstDateTemp.Enqueue(dataTemp);

            //if (data.isRaw)
            //{
            //    InsertMeasureRaw(data, _DBUltity, device);
            //}
            //else
            //{
            //    InsertMeasureDetail(data, _DBUltity);

            //    if (MeasureType == emMeasureType.AlarmTest && Math.Abs(data.actualDelegate) >= Math.Abs(device.AlarmValue))
            //    {
            //        InsertMeasureLimit(data, _DBUltity);
            //    }
            //}
        }

        public void MeasureEnd(DeviceInfo device, List<MaxValue> lstMaxValue, int measureResult, DateTime? measureEndTime)
        {
            var _hasErrDB = false;

            if (device.MeasureType == emMeasureType.WalkingTest && lstMaxValue != null)
            {
                foreach (var data in lstMaxValue)
                {
                    var dtSample = data.dtSample;
                    dtSample.result = (int)emMeasureResult.Fail;
                    InsertMeasureLimit(dtSample, _DBUltity, device.MeasureId);
                }
            }

            if (!_DBUltity.UpdateEndTimeMeasure(device.MeasureId, measureEndTime.Value, measureResult))
            {
                _hasErrDB = true;
                WriteFileErrors(clsConfig.MEASURE_NAME_FILE, _DBUltity.GetUpdateEndTimeMeasure(device.MeasureId, measureEndTime.Value, measureResult), device.MeasureId);
            }

            if (_hasErrDB)
            {
                FormSetting.ShowMsg(MessageBoxIcon.Error, MSG_ERR_HAS_DB);
            }
        }

        public void ShowManagement(object sender, EventArgs e)
        {
            using (var frmManagement = new Management())
            {
                frmManagement.InitForm(lstDeviceInfo);
                frmManagement.ShowDialog();
                if (frmManagement.ChangeSetting)
                {
                    FormSetting?.LoadInfoDevice();
                }
            }
        }

        public void ShowMeasureManagement(object sender, EventArgs e)
        {
            using (var frmMM = new MeasureManagement())
            {
                frmMM.ShowDialog();
            }
        }

        #endregion

        #region Insert DB

        private void InsertMeasureDetail(DataSample data, clsDBUltity db, int _measureId)
        {
            var _hasErrDB = false;

            if (string.IsNullOrEmpty(data.strSample))
            {
                return;
            }

            if (!_hasErrDB)
            {
                if (!db.InsertMeasureDetail(data.deviceId, _measureId, data.t, data.strSample, data.actualMaxValue, data.actualMinValue, data.actualDelegate, data.result))
                {
                    _hasErrDB = true;
                    _nameErrFileDetail = clsConfig.MEASURE_DETAIL_NAME_FILE + DateTime.Now.ToString(clsDBUltity.cstrDateTimeFormatNoMiliSecond2);
                    string content = db.GetInsertMeasureDetailSql(data.deviceId, _measureId, data.t, data.strSample, data.actualMaxValue, data.actualMinValue, data.actualDelegate, data.result);
                    WriteFileErrors(_nameErrFileDetail, content, _measureId);
                    _numberRecordWriteDetail++;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(_nameErrFileDetail) || _numberRecordWriteDetail == clsConfig.MAX_RECORD_FILE_ERR)
                {
                    _numberRecordWriteDetail = 0;
                    _nameErrFileDetail = clsConfig.MEASURE_DETAIL_NAME_FILE + DateTime.Now.ToString(clsDBUltity.cstrDateTimeFormatNoMiliSecond2);
                }
                string content = db.GetInsertMeasureDetailSql(data.deviceId, _measureId, data.t, data.strSample, data.actualMaxValue, data.actualMinValue, data.actualDelegate, data.result);
                WriteFileErrors(_nameErrFileDetail, content, _measureId);
                _numberRecordWriteDetail++;
            }
        }

        private void InsertMeasureRaw(DataSample data, clsDBUltity db, DeviceInfo device)
        {
            var _hasErrDB = false;

            if (string.IsNullOrEmpty(data.strSample))
            {
                return;
            }

            double dblMax;
            double dblMin;
            double dblDelegate;
            string strValue;
            string[] arrValue;
            var result = (int)clsDBUltity.emMeasureResult.Pass;

            GetDataFromSample(data.strSample, out arrValue, out strValue, out dblMax, out dblMin, out dblDelegate);

            if (device.MeasureType == emMeasureType.AlarmTest)
            {
                result = device.AlarmValue > (int)dblDelegate ? (int)clsDBUltity.emMeasureResult.Normal : (int)clsDBUltity.emMeasureResult.Alarm;
            }

            if (!_hasErrDB)
            {
                if (!db.InsertMeasureDetailRaw(data.deviceId, device.MeasureId, data.t, data.strSample, (int)dblMax, (int)dblMin, (int)dblDelegate, result))
                {
                    _hasErrDB = true;
                    _nameErrFileRaw = clsConfig.MEASURE_DETAIL_RAW_NAME_FILE + DateTime.Now.ToString(clsDBUltity.cstrDateTimeFormatNoMiliSecond2);
                    string content = db.GetInsertMeasureDetailRawSql(data.deviceId, device.MeasureId, data.t, data.strSample, (int)dblMax, (int)dblMin, (int)dblDelegate, result);
                    WriteFileErrors(_nameErrFileRaw, content, device.MeasureId);
                    _numberRecordWriteRaw++;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(_nameErrFileRaw) || _numberRecordWriteRaw == clsConfig.MAX_RECORD_FILE_ERR)
                {
                    _numberRecordWriteRaw = 0;
                    _nameErrFileRaw = clsConfig.MEASURE_DETAIL_RAW_NAME_FILE + DateTime.Now.ToString(clsDBUltity.cstrDateTimeFormatNoMiliSecond2);
                }
                string content = db.GetInsertMeasureDetailRawSql(data.deviceId, device.MeasureId, data.t, data.strSample, (int)dblMax, (int)dblMin, (int)dblDelegate, result);
                WriteFileErrors(_nameErrFileRaw, content, device.MeasureId);
                _numberRecordWriteRaw++;
            }
        }

        private void InsertMeasureLimit(DataSample data, clsDBUltity db, int _measureId)
        {
            var _hasErrDB = false;

            if (string.IsNullOrEmpty(data.strSample))
            {
                return;
            }

            if (!_hasErrDB)
            {
                if (!db.InsertMeasureDetailLimit(data.deviceId, _measureId, data.t, data.strSample, data.actualMaxValue, data.actualMinValue, data.actualDelegate, data.result))
                {
                    _hasErrDB = true;
                    _nameErrFileLimit = clsConfig.MEASURE_DETAIL_LIMIT_NAME_FILE + DateTime.Now.ToString(clsDBUltity.cstrDateTimeFormatNoMiliSecond2);
                    string content = db.GetInsertMeasureDetailLimitSql(data.deviceId, _measureId, data.t, data.strSample, data.actualMaxValue, data.actualMinValue, data.actualDelegate, data.result);
                    WriteFileErrors(_nameErrFileLimit, content, _measureId);
                    _numberRecordWriteLimit++;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(_nameErrFileLimit) || _numberRecordWriteLimit == clsConfig.MAX_RECORD_FILE_ERR)
                {
                    _numberRecordWriteLimit = 0;
                    _nameErrFileLimit = clsConfig.MEASURE_DETAIL_LIMIT_NAME_FILE + DateTime.Now.ToString(clsDBUltity.cstrDateTimeFormatNoMiliSecond2);
                }
                string content = db.GetInsertMeasureDetailLimitSql(data.deviceId, _measureId, data.t, data.strSample, data.actualMaxValue, data.actualMinValue, data.actualDelegate, data.result);
                WriteFileErrors(_nameErrFileLimit, content, _measureId);
                _numberRecordWriteLimit++;
            }
        }

        #endregion Insert DB

        #region Private Function

        private void WriteFileErrors(string fileName, string content, int _measureId)
        {
            string path = clsConfig.PathDataErrors + @"\" + _measureId;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path += @"\" + _measureId + "_" + fileName + @".txt";

            if (!File.Exists(path))
            {
                using (var sw = File.CreateText(path))
                {
                    sw.WriteLine(content);
                    sw.Close();
                }
            }
            else
            {
                using (var sw = new StreamWriter(path, true))
                {
                    sw.WriteLine(content);
                    sw.Close();
                }
            }
        }

        private void GetDataFromSample(string strSample, out string[] arrValue, out string strValue, out double dblMax, out double dblMin, out double dblDelegate)
        {
            // Default Value
            strValue = string.Empty;
            dblMax = 0;
            dblMin = 0;
            dblDelegate = 0;
            arrValue = new string[1] { "" };

            // Get Value From Sample
            strSample = strSample.Replace("\"\"", ",");
            strSample = strSample.Replace("\"", "");

            if (string.IsNullOrEmpty(strSample))
            {
                return;
            }

            arrValue = strSample.Split(',');
            double[] dblValue = arrValue.Select(n => clsCommon.CnvNullToDouble(n.Trim(), 0) * RoundValue).ToArray();

            strValue = string.Join(",", dblValue);
            dblMax = dblValue.Max();
            dblMin = dblValue.Min();
            dblDelegate = Math.Abs(dblMax) >= Math.Abs(dblMin) ? dblMax : dblMin;
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
                    if (_DBUltity != null)
                    {
                        _DBUltity.Dispose();
                        _DBUltity = null;
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~clsGraphForm() {
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
