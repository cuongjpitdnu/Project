using System;
using System.Linq;
using BaseCommon;
using System.IO;
using System.Windows.Forms;
using static BaseCommon.clsConst;
using System.Data;

namespace WithMySql
{
    class clsGraphForm : IGraphForm
    {
        private const string MSG_ERR_CONNECT_DB = "Cannot connect to database, please check network or connection information!";
        private const string MSG_ERR_HAS_DB = "Has error in insert data to database";

        public GraphForm FormSetting { get; set; }
        public emMeasureType MeasureType { get; set; }
        public DeviceInfo DeviceCurrent { get; set; }
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
        private bool _hasErrDB = false;
        private string _nameErrFileDetail = string.Empty;
        private string _nameErrFileLimit = string.Empty;
        private string _nameErrFileRaw = string.Empty;
        private int _numberRecordWriteDetail = 0;
        private int _numberRecordWriteLimit = 0;
        private int _numberRecordWriteRaw = 0;
        private int _measureId;

        public DeviceInfo[] GetListDevice()
        {
            var listDeviceInfo = new DeviceInfo[2];
            
            using (var objDb = new clsDBUltity())
            {
                var deviceInfo = objDb.GetDeviceList(false);

                if (clsCommon.TableIsNullOrEmpty(deviceInfo))
                {
                    return listDeviceInfo;
                }

                var i = -1;

                foreach (DataRow row in deviceInfo.Rows)
                {
                    i++;
                 
                    if (i > 1)
                    {
                        break;
                    }
                       
                    listDeviceInfo[i].deviceId = clsCommon.CnvNullToInt(row["device_id"]);
                    listDeviceInfo[i].deviceType = clsCommon.CnvNullToInt(row["device_type"]);
                    listDeviceInfo[i].ipAddress = clsCommon.CnvNullToString(row["ip_address"]).Trim();
                    listDeviceInfo[i].port = clsCommon.CnvNullToInt(row["port"]);
                    listDeviceInfo[i].AlarmValue = clsCommon.CnvNullToInt(row["alarm_value"]);
                    listDeviceInfo[i].period = clsCommon.CnvNullToInt(row["period"]);
                    listDeviceInfo[i].failLevel = clsCommon.CnvNullToInt(row["fail_level"]);
                    listDeviceInfo[i].samples = clsCommon.CnvNullToInt(row["samples"]);
                    listDeviceInfo[i].active = clsCommon.CnvNullToInt(row["active"]) > 0;
                }

                return listDeviceInfo;
            }
        }

        public void MeasureStart()
        {
            //Insert measure information
            _measureId = _DBUltity.InsertMeasure(DeviceCurrent.deviceId,
                                                 (int)MeasureType,
                                                 DeviceCurrent.AlarmValue,
                                                 DeviceCurrent.period,
                                                 DeviceCurrent.failLevel,
                                                 DeviceCurrent.samples,
                                                 DateTime.Now);

            if (_measureId < 1)
            {
                FormSetting.ShowMsg(MessageBoxIcon.Error, MSG_ERR_CONNECT_DB);
                return;
            }
        }
        
        public void MeasureProcess(DataSample data)
        {
            if (data.isRaw)
            {
                InsertMeasureRaw(data, _DBUltity);
            } else
            {
                InsertMeasureDetail(data, _DBUltity);

                if (MeasureType == emMeasureType.AlarmTest && Math.Abs(data.actualDelegate) >= Math.Abs(DeviceCurrent.AlarmValue))
                {
                    InsertMeasureLimit(data, _DBUltity);
                }
            }
        }

        public void MeasureEnd()
        {
            if (MeasureType == emMeasureType.WalkingTest && FormSetting.ListMaxValue != null)
            {
                foreach (var data in FormSetting.ListMaxValue)
                {
                    var dtSample = data.dtSample;
                    dtSample.result = (int)emMeasureResult.Fail;
                    InsertMeasureLimit(dtSample, _DBUltity);
                }
            }

            if (!_DBUltity.UpdateEndTimeMeasure(_measureId, FormSetting.MeasureEndTime, FormSetting.MeasureResult))
            {
                _hasErrDB = true;
                WriteFileErrors(clsConfig.MEASURE_NAME_FILE, _DBUltity.GetUpdateEndTimeMeasure(_measureId, FormSetting.MeasureEndTime, FormSetting.MeasureResult));
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

        #region Insert DB

        private void InsertMeasureDetail(DataSample data, clsDBUltity db)
        {
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
                    WriteFileErrors(_nameErrFileDetail, content);
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
                WriteFileErrors(_nameErrFileDetail, content);
                _numberRecordWriteDetail++;
            }
        }

        private void InsertMeasureRaw(DataSample data, clsDBUltity db)
        {
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

            if (MeasureType == emMeasureType.AlarmTest)
            {
                result = DeviceCurrent.AlarmValue > (int)dblDelegate ? (int)clsDBUltity.emMeasureResult.Normal : (int)clsDBUltity.emMeasureResult.Alarm;
            }

            if (!_hasErrDB)
            {
                if (!db.InsertMeasureDetailRaw(data.deviceId, _measureId, data.t, data.strSample, (int)dblMax, (int)dblMin, (int)dblDelegate, result))
                {
                    _hasErrDB = true;
                    _nameErrFileRaw = clsConfig.MEASURE_DETAIL_RAW_NAME_FILE + DateTime.Now.ToString(clsDBUltity.cstrDateTimeFormatNoMiliSecond2);
                    string content = db.GetInsertMeasureDetailRawSql(data.deviceId, _measureId, data.t, data.strSample, (int)dblMax, (int)dblMin, (int)dblDelegate, result);
                    WriteFileErrors(_nameErrFileRaw, content);
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
                string content = db.GetInsertMeasureDetailRawSql(data.deviceId, _measureId, data.t, data.strSample, (int)dblMax, (int)dblMin, (int)dblDelegate, result);
                WriteFileErrors(_nameErrFileRaw, content);
                _numberRecordWriteRaw++;
            }
        }

        private void InsertMeasureLimit(DataSample data, clsDBUltity db)
        {
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
                    WriteFileErrors(_nameErrFileLimit, content);
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
                WriteFileErrors(_nameErrFileLimit, content);
                _numberRecordWriteLimit++;
            }
        }

        #endregion Insert DB

        #region Private Function

        private void WriteFileErrors(string fileName, string content)
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
            } else
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
