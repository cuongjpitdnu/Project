using BaseCommon;
using System;
using System.Collections.Generic;
using System.IO;
using static BaseCommon.clsConst;

namespace NoMySql
{
    class clsGraphForm : IGraphForm
    {

        #region Initialize Variables

        private Dictionary<int, string> _dicStartTime = new Dictionary<int, string>();
        //private string _pathFolderData;
        private string _fileNameRaw;
        private string _fileNameDetail;
        private string _fileNameLimit;

        private int _numberRaw;
        private int _numberDetail;
        private int _numberLimit;
        private List<DeviceInfo> lstDeviceInfo { get; set; }

        #endregion Initialize Variables

        #region Public Property

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

        #endregion Public Property

        #region Public Method

        public clsGraphForm()
        {
        }

        public void StopInsertDB()
        {
        }

        public void UpdateListDevice(List<DeviceInfo> lstDevice)
        {
            clsSuportSerialize.BinSerialize(clsConfig.SQLITE_DB_PATH, lstDevice);
            FormSetting?.LoadInfoDevice();
        }

        public List<DeviceInfo> GetListDevice()
        {
            lstDeviceInfo = clsSuportSerialize.BinDeserialize<List<DeviceInfo>>(clsConfig.SQLITE_DB_PATH);

            if (lstDeviceInfo == null)
            {
                lstDeviceInfo = new List<DeviceInfo>();

                lstDeviceInfo.Add(getDefaultDevice(1, "Device 1", 1));
                lstDeviceInfo.Add(getDefaultDevice(2, "Device 2", 2));
                lstDeviceInfo.Add(getDefaultDevice(3, "Device 3", 3));
                lstDeviceInfo.Add(getDefaultDevice(4, "Device 4", 4));
                lstDeviceInfo.Add(getDefaultDevice(5, "Device 5", 5));

                clsSuportSerialize.BinSerialize(clsConfig.SQLITE_DB_PATH, lstDeviceInfo);
            }

            return lstDeviceInfo;
        }

        public Response MeasureStart(DeviceInfo device, int measureResult)
        {
            Response res = new Response();
            res.MeasureId = -1;
            res.PathFolderData = "";
            
            if (!_dicStartTime.ContainsKey(device.DeviceId))
            {
                _dicStartTime.Add(device.DeviceId, string.Empty);
            }

            _dicStartTime[device.DeviceId] = DateTime.Now.ToString(cstrDateTimeFormatNoMiliSecond2);

            var _pathFolderData = CreateMeasureFolder(device, measureResult);

            if (!Directory.Exists(_pathFolderData))
            {
                Directory.CreateDirectory(_pathFolderData);
            }

            res.PathFolderData = _pathFolderData;

            return res;
        }

        public void MeasureProcess(DataSample data, DeviceInfo device)
        {
            try
            {
                var strContent = CreateContentCsv(data);

                if (data.isRaw)
                {
                    WriteFileCsv(ref _numberRaw, ref _fileNameRaw, clsConfig.FILE_NAME_RAW, strContent, device.PathFolderData);
                }
                else
                {
                    WriteFileCsv(ref _numberDetail, ref _fileNameDetail, clsConfig.FILE_NAME_DETAIL, strContent, device.PathFolderData);

                    if (device.MeasureType == emMeasureType.AlarmTest && Math.Abs(data.actualDelegate) >= Math.Abs(device.AlarmValue))
                    {
                        WriteFileCsv(ref _numberLimit, ref _fileNameLimit, clsConfig.FILE_NAME_LIMIT, strContent, device.PathFolderData);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void MeasureEnd(DeviceInfo device, List<MaxValue> lstMaxValue, int measureResult, DateTime? measureEndTime)
        {
            if (Directory.Exists(device.PathFolderData))
            {
                if (device.MeasureType == emMeasureType.WalkingTest && lstMaxValue != null)
                {
                    foreach (var data in lstMaxValue)
                    {

                        var dtSample = data.dtSample;
                        dtSample.result = (int)emMeasureResult.Fail;

                        var strContent = CreateContentCsv(dtSample);
                        WriteFileCsv(ref _numberLimit, ref _fileNameLimit, clsConfig.FILE_NAME_LIMIT, strContent, device.PathFolderData);
                    }
                }
                var pathNew = CreateMeasureFolder(device, measureResult, measureEndTime);
                Directory.Move(device.PathFolderData, pathNew);
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
            using (var frmReport = new MeasureManagement())
            {
                frmReport.ShowDialog();
            }
        }

        #endregion Public Method

        #region Private Function

        private DeviceInfo getDefaultDevice(int deviceId, string deviceName, int ordinalDisplay)
        {
            return new DeviceInfo
            {
                DeviceId = deviceId,
                DeviceName = deviceName,
                IpAddress = deviceId == 1 ? clsConst.IPADDRESS_DEFAULT : "",
                Port = clsConst.PORT_DEFAULT,
                AlarmValue = clsConst.ALARMVALUE_DEFAULT,
                Period = clsConst.PERIOD_DEFAULT,
                FailLevel = clsConst.FAILLEVEL_DEFAULT,
                Samples = clsConst.SAMPLES_DEFAULT,
                WalkingMode = false,
                Active = true,
                OrdinalDisplay = ordinalDisplay
            };
        }

        private string CreateContentCsv(DataSample data)
        {
            var arrContent = new string[] {
                ClearStringCsv(data.t.ToString(cstrDateTimeFormatMiliSecond)),
                ClearStringCsv(data.strSample),
                ClearStringCsv(data.actualMaxValue.ToString()),
                ClearStringCsv(data.actualMinValue.ToString()),
                ClearStringCsv(data.actualDelegate.ToString()),
                ClearStringCsv(data.result.ToString()),
            };

            return string.Join(",", arrContent);
        }

        private void WriteFileCsv(ref int numberRecord, ref string fileName, string fileFomat, string strContent, string _pathFolderData)
        {
            if (string.IsNullOrEmpty(fileName) || numberRecord > clsConfig.MaxRecordCsv - 1)
            {
                fileName = fileFomat + DateTime.Now.ToString(cstrDateTimeFormatNoMiliSecond2);
            }

            numberRecord++;

            var pathFile = string.Format(@"{0}\{1}.csv", _pathFolderData, fileName);

            using (var sw = File.AppendText(pathFile))
            {
                sw.WriteLine(strContent);
                sw.Flush();
            }
        }

        private string CreateMeasureFolder(DeviceInfo device, int measureResult, DateTime? endTime = null)
        {
            var arrTemp = new string[] {
                device.MeasureType == emMeasureType.AlarmTest ? ALARM_TEST_KEY : WALKING_TEST_KEY,
                device.DeviceId.ToString(),
                device.AlarmValue.ToString(),
                device.FailLevel.ToString(),
                device.Period.ToString(),
                _dicStartTime[device.DeviceId],
                endTime != null ? endTime?.ToString(cstrDateTimeFormatNoMiliSecond2) : "NULL",
                measureResult.ToString()
            };

            var nameFolder = string.Join("_", arrTemp);

            if (endTime == null)
            {
                nameFolder = clsConfig.FOLDER_NAME_TEMP + nameFolder;
            }

            return clsConfig.PathDataMeasure + nameFolder + @"\";
        }

        private string ClearStringCsv(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            if (value.IndexOfAny(new char[] { '"', ',' }) != -1)
            {
                value = value.Replace("\"", "\"\"");
            }

            return string.Format("\"{0}\"", value);
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
                    //if (_objExport != null)
                    //{
                    //    _objExport.Dispose();
                    //    _objExport = null;
                    //}
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
