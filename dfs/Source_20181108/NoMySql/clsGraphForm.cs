using System;
using BaseCommon;
using System.IO;
using static BaseCommon.clsConst;
using NoMySql.Properties;

namespace NoMySql
{
    class clsGraphForm : IGraphForm
    {

        #region Initialize Variables

        private string _startTime;
        private string _pathFolderData;
        //private string _pathReport;
        //private string _fileNameReport;
        private string _fileNameRaw;
        private string _fileNameDetail;
        private string _fileNameLimit;

        private int _numberRaw;
        private int _numberDetail;
        private int _numberLimit;

        //private int _cnn;
        //private int _rowStart;

        //private clsExportReport _objExport;
        //private MeasureInfo _measureInfo;

        #endregion Initialize Variables

        #region Public Property

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

        #endregion Public Property

        public clsGraphForm()
        {
            //_objExport = new clsExportReport();
        }

        private clsGraphForm(GraphForm frmGraph)
        {
            this.FormSetting = frmGraph;
        }

        private clsGraphForm GetInstance()
        {
            return new clsGraphForm(this.FormSetting);
        }

        #region Public Method

        public DeviceInfo[] GetListDevice()
        {
            var listDeviceInfo = new DeviceInfo[2];

            // Device 1
            listDeviceInfo[0].deviceId = 3;
            listDeviceInfo[0].deviceType = 0;
            listDeviceInfo[0].deviceName = Settings.Default.DeviceName1;
            listDeviceInfo[0].ipAddress = Settings.Default.ipAddress1;
            listDeviceInfo[0].port = Settings.Default.port1;
            listDeviceInfo[0].AlarmValue = Settings.Default.AlarmValue1;
            listDeviceInfo[0].period = Settings.Default.Period1;
            listDeviceInfo[0].failLevel = Settings.Default.FailLevel1;
            listDeviceInfo[0].samples = Settings.Default.Samples1;
            listDeviceInfo[0].active = Settings.Default.Active1;

            // Device 2
            listDeviceInfo[1].deviceId = 4;
            listDeviceInfo[1].deviceType = 1;
            listDeviceInfo[1].deviceName = Settings.Default.DeviceName2;
            listDeviceInfo[1].ipAddress = Settings.Default.ipAddress2;
            listDeviceInfo[1].port = Settings.Default.port2;
            listDeviceInfo[1].AlarmValue = Settings.Default.AlarmValue2;
            listDeviceInfo[1].period = Settings.Default.Period2;
            listDeviceInfo[1].failLevel = Settings.Default.FailLevel2;
            listDeviceInfo[1].samples = Settings.Default.Samples2;
            listDeviceInfo[1].active = Settings.Default.Active2;

            return listDeviceInfo;
        }

        public void MeasureStart()
        {
            var now = DateTime.Now;
            //var fileNameTemplate = MeasureType == emMeasureType.AlarmTest ? REPORT_NAME_ALARM : REPORT_NAME_WALKING;
            //var pathTemplateExcel = clsConfig.PathReportTemplate + fileNameTemplate;

            //_rowStart = MeasureType == emMeasureType.AlarmTest ? clsExportReport.ROW_START_ALARM : clsExportReport.ROW_START_WALKING;
            _startTime = now.ToString(cstrDateTimeFormatNoMiliSecond2);
            _pathFolderData = CreateMeasureFolder();
            //_fileNameReport = fileNameTemplate;
            //_pathReport = _pathFolderData + _fileNameReport;
            //_cnn = 0;
            
            if (!Directory.Exists(_pathFolderData))
            {
                Directory.CreateDirectory(_pathFolderData);
            }

            //if (File.Exists(pathTemplateExcel))
            //{
            //    File.Copy(pathTemplateExcel, _pathReport, true);

            //    _measureInfo = new MeasureInfo
            //    {
            //        ReportDate = now.ToString(cstrDateFomatShow),
            //        MeasureStart = now.ToString(cstrDateTimeFomatShow),
            //        MeasureType = MeasureType == emMeasureType.AlarmTest ? ALARM_TEST : WALKING_TEST,
            //        AlarmValue = DeviceCurrent.AlarmValue.ToString("N0"),
            //        FailLevel = DeviceCurrent.failLevel.ToString("N0"),
            //        Period = DeviceCurrent.period.ToString("N0"),
            //        DeviceName = DeviceCurrent.deviceName
            //    };

            //    _objExport.WriteMeasureInfo(_pathReport, _measureInfo, MeasureType);
            //}
        }

        public void MeasureProcess(DataSample data)
        {
            var strContent = CreateContentCsv(data);

            if (data.isRaw)
            {
                WriteFileCsv(ref _numberRaw, ref _fileNameRaw, clsConfig.FILE_NAME_RAW, strContent);
            }
            else
            {
                WriteFileCsv(ref _numberDetail, ref _fileNameDetail, clsConfig.FILE_NAME_DETAIL, strContent);

                if (MeasureType == emMeasureType.AlarmTest && Math.Abs(data.actualDelegate) >= Math.Abs(DeviceCurrent.AlarmValue))
                {
                    WriteFileCsv(ref _numberLimit, ref _fileNameLimit, clsConfig.FILE_NAME_LIMIT, strContent);
                }
                                
                //if (_objExport.WriteMeasureDetail(_pathReport, _rowStart, new MeasureDetail
                //{
                //    No = ++_cnn,
                //    Time = data.t.ToString(cstrDateTimeFormatMiliSecond),
                //    Value = MeasureType == emMeasureType.AlarmTest ? data.actualDelegate.ToString() : data.actualValue,
                //    Result = clsCommon.MeasureResultDisplay(data.result),
                //}, MeasureType))
                //{
                //    _rowStart++;
                //}
            }
        }

        public void MeasureEnd()
        {

            if (Directory.Exists(_pathFolderData))
            {
                if (MeasureType == emMeasureType.WalkingTest && FormSetting.ListMaxValue != null)
                {
                    //var rowStart = clsExportReport.ROW_START_WALKING_LIMIT;
                    //var cnn = 0;

                    foreach (var data in FormSetting.ListMaxValue)
                    {
                        
                        var dtSample = data.dtSample;
                        dtSample.result = (int)emMeasureResult.Fail;

                        var strContent = CreateContentCsv(dtSample);
                        WriteFileCsv(ref _numberLimit, ref _fileNameLimit, clsConfig.FILE_NAME_LIMIT, strContent);

                        //if (_objExport.WriteMeasureDetail(_pathReport, rowStart, new MeasureDetail
                        //{
                        //    No = ++cnn,
                        //    Time = dtSample.t.ToString(cstrDateTimeFormatMiliSecond),
                        //    Value = MeasureType == emMeasureType.AlarmTest ? dtSample.actualDelegate.ToString() : dtSample.actualValue,
                        //    Result = clsCommon.MeasureResultDisplay(dtSample.result),
                        //}, MeasureType))
                        //{
                        //    rowStart++;
                        //}
                    }
                }

                //_measureInfo.MeasureEnd = FormSetting.MeasureEndTime?.ToString(cstrDateTimeFomatShow);
                //_measureInfo.MeasureResult = clsCommon.MeasureResultDisplay(FormSetting.MeasureResult);
                //_objExport.WriteMeasureInfo(_pathReport, _measureInfo, MeasureType);

                var pathNew = CreateMeasureFolder(FormSetting.MeasureEndTime);
                //var pathReport = pathNew + _fileNameReport;
                Directory.Move(_pathFolderData, pathNew);

                //if (File.Exists(pathReport))
                //{
                //    if (FormSetting.InvokeRequired)
                //    {
                //        FormSetting.Invoke(new Action(() =>
                //        {
                //            SaveReport(pathReport);
                //        }));
                //    }
                //    else
                //    {
                //        SaveReport(pathReport);
                //    }
                //}
            }
        }

        //public void SaveReport(string pathReport)
        //{
        //    if (!File.Exists(pathReport))
        //    {
        //        FormSetting.ShowMsg(MessageBoxIcon.Error, "Export Excel erors.", FormSetting.Text);
        //        return;
        //    }
                        
        //    using (var saveFileDialog = FormSetting.SaveExcelDialog(DateTime.Now.ToString(cstrDateTimeFormatNoMiliSecond2) + "_" + _fileNameReport))
        //    {
        //        if (saveFileDialog.ShowDialog() != DialogResult.OK)
        //        {
        //            return;
        //        }

        //        File.Copy(pathReport, saveFileDialog.FileName, true);

        //        if (File.Exists(saveFileDialog.FileName))
        //        {
        //            if (!FormSetting.ComfirmMsg("Do you want open file report?"))
        //            {
        //                return;
        //            }

        //            Process.Start(saveFileDialog.FileName);
        //            return;
        //        }
        //        else
        //        {
        //            FormSetting.ShowMsg(MessageBoxIcon.Error, "Export Excel erors.", FormSetting.Text);
        //            return;
        //        }
        //    }
        //}

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
            using (var frmReport = new MeasureManagement())
            {
                frmReport.ShowDialog();
            }
        }

        #endregion Public Method

        #region Private Function

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

        private void WriteFileCsv(ref int numberRecord, ref string fileName, string fileFomat, string strContent)
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

        private string CreateMeasureFolder(DateTime? endTime = null)
        {
            var arrTemp = new string[] {
                MeasureType == emMeasureType.AlarmTest ? ALARM_TEST_KEY : WALKING_TEST_KEY,
                (DeviceCurrent.deviceType + 1).ToString(),
                DeviceCurrent.AlarmValue.ToString(),
                DeviceCurrent.failLevel.ToString(),
                DeviceCurrent.period.ToString(),
                _startTime,
                endTime != null ? endTime?.ToString(cstrDateTimeFormatNoMiliSecond2) : "NULL",
                FormSetting.MeasureResult.ToString()
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
