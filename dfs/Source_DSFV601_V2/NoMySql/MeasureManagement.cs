using BaseCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using static BaseCommon.clsConst;

namespace NoMySql
{

    public partial class MeasureManagement : BaseForm
    {

        // Msg
        private const string COMFIRM_MSG_DELETE = "Do you want delete this record?";

        #region Event Form

        public MeasureManagement()
        {
            InitializeComponent();

            // Limit resize form
            this.MinimumSize = this.Size;
            this.MaximumSize = Screen.PrimaryScreen.Bounds.Size;
            this.DoubleBuffered = true;

            try
            {
                IntForm();
            }
            catch (Exception ex)
            {
                ShowMsg(MessageBoxIcon.Error, MSG_ERR_PROCESS);
            }
        }

        private void chkView_CheckedChanged(object sender, EventArgs e)
        {
            btnFilter_Click(null, null);
        }

        private void cmbType_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!cmbResult.Enabled || cmbType.SelectedIndex < 0)
            {
                return;
            }

            int selectedValue = clsCommon.CnvNullToInt(cmbType.SelectedValue);

            var tblBinding = new DataTable();
            tblBinding.Columns.Add(DISPLAY, typeof(string));
            tblBinding.Columns.Add(VALUE, typeof(int));

            tblBinding.Rows.Add(new object[] { SELECT_ALL_SHOW, SELECT_ALL });

            if (selectedValue == SELECT_ALL || selectedValue == (int)emMeasureType.AlarmTest)
            {
                tblBinding.Rows.Add(new object[] { clsCommon.MeasureResultDisplay((int)emMeasureResult.Normal), (int)emMeasureResult.Normal });
                tblBinding.Rows.Add(new object[] { clsCommon.MeasureResultDisplay((int)emMeasureResult.Alarm), (int)emMeasureResult.Alarm });
            }

            if (selectedValue == SELECT_ALL || selectedValue == (int)emMeasureType.WalkingTest)
            {
                tblBinding.Rows.Add(new object[] { clsCommon.MeasureResultDisplay((int)emMeasureResult.Pass), (int)emMeasureResult.Pass });
                tblBinding.Rows.Add(new object[] { clsCommon.MeasureResultDisplay((int)emMeasureResult.Fail), (int)emMeasureResult.Fail });
            }

            BindingDataTableToComboBox(cmbResult, tblBinding, DISPLAY, VALUE, SELECT_ALL);
        }

        private void dgvMeasure_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;

            var path = clsCommon.CnvNullToString(dgvMeasure.Rows[e.RowIndex].Cells[MeasureID.Index].Value);

            try
            {
                // btnDelete
                if (e.ColumnIndex == colDelete.Index && e.RowIndex >= 0)
                {
                    if (!ComfirmMsg(COMFIRM_MSG_DELETE, this.Text))
                    {
                        ClearSelectionDgv(dgvMeasure);
                        return;
                    }

                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path, true);
                    }

                    LoadData();
                    return;
                }

                // Load filter
                var strStartTime = clsCommon.CnvNullToString(dgvMeasure.Rows[e.RowIndex].Cells[StartTime.Index].Value);
                var strEndTime = clsCommon.CnvNullToString(dgvMeasure.Rows[e.RowIndex].Cells[EndTime.Index].Value);
                var dtStartTime = clsCommon.CnvStringToDateTime(strStartTime, cstrDateTimeFormatNoMiliSecond2);
                var dtEndTime = clsCommon.CnvStringToDateTime(strEndTime, cstrDateTimeFormatNoMiliSecond2);
                var intMeasureType = clsCommon.CnvNullToInt(dgvMeasure.Rows[e.RowIndex].Cells[MeasureType.Index].Value);

                ResultDetail.Visible = intMeasureType == (int)emMeasureType.AlarmTest;
                pnlFilter.Enabled = true;

                dtpSubDateS.Value = dtStartTime;
                cboSubHourS.SelectedValue = dtStartTime.Hour;
                cboSubMimuteS.SelectedValue = dtStartTime.Minute;
                cboSubSecondS.SelectedValue = dtStartTime.Second;

                dtpSubDateE.Value = dtEndTime;
                cboSubHourE.SelectedValue = dtEndTime.Hour;
                cboSubMimuteE.SelectedValue = dtEndTime.Minute;
                cboSubSecondE.SelectedValue = dtEndTime.Second;

                LoadDataSub(path);
            }
            catch (Exception ex)
            {
                ShowMsg(MessageBoxIcon.Error, MSG_ERR_PROCESS);
            }
        }

        // Format grid measure
        private void dgvMeasure_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == MeasureType.Index)
            {
                var intMeasureType = clsCommon.CnvNullToInt(e.Value);
                e.Value = string.Empty;

                if (intMeasureType == (int)emMeasureType.AlarmTest)
                {
                    e.Value = ALARM_TEST;
                }
                else if (intMeasureType == (int)emMeasureType.WalkingTest)
                {
                    e.Value = WALKING_TEST;
                }
            }
            else if (e.ColumnIndex == StartTime.Index || e.ColumnIndex == EndTime.Index)
            {
                DateTime showDate;
                if (DateTime.TryParseExact(clsCommon.CnvNullToString(e.Value), cstrDateTimeFormatNoMiliSecond2, null, DateTimeStyles.None, out showDate))
                {
                    e.Value = showDate.ToString(cstrDateTimeFomatShow);
                }
                else
                {
                    e.Value = string.Empty;
                }
            }
            else if (e.ColumnIndex == Result.Index)
            {
                e.Value = clsCommon.MeasureResultDisplay(clsCommon.CnvNullToInt(e.Value));
            }
        }

        // Format grid measure detail
        private void dgvMeasureDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == colTime.Index)
            {
                DateTime showDate;
                if (DateTime.TryParseExact(clsCommon.CnvNullToString(e.Value), cstrDateTimeFormatMiliSecond, null, DateTimeStyles.None, out showDate))
                {
                    e.Value = showDate.ToString(cstrDateTimeFomatShow);
                }
            }
            else if (e.ColumnIndex == ResultDetail.Index)
            {
                e.Value = clsCommon.MeasureResultDisplay(clsCommon.CnvNullToInt(e.Value));
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                ShowMsg(MessageBoxIcon.Error, MSG_ERR_PROCESS);
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (dgvMeasure.CurrentCell == null)
            {
                return;
            }
            var pathFolder = clsCommon.CnvNullToString(dgvMeasure.Rows[dgvMeasure.CurrentCell.RowIndex].Cells[MeasureID.Index].Value) + @"\";

            try
            {
                LoadDataSub(pathFolder);
            }
            catch (Exception ex)
            {
                ShowMsg(MessageBoxIcon.Error, MSG_ERR_PROCESS);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dgvMeasure.Rows.Count == 0)
            {
                ShowMsg(MessageBoxIcon.Warning, "Data not found.");
                return;
            }

            if (dgvMeasure.CurrentCell == null)
            {
                ShowMsg(MessageBoxIcon.Warning, "Please select 1 row data to export excel.");
                return;
            }

            var intMeasureType = clsCommon.CnvNullToInt(dgvMeasure.Rows[dgvMeasure.CurrentCell.RowIndex].Cells[MeasureType.Index].Value);
            var emType = intMeasureType == (int)emMeasureType.AlarmTest ? emMeasureType.AlarmTest : emMeasureType.WalkingTest;
            var pathFolder = clsCommon.CnvNullToString(dgvMeasure.Rows[dgvMeasure.CurrentCell.RowIndex].Cells[MeasureID.Index].Value) + @"\";
            var fileNameReport = (intMeasureType == (int)emMeasureType.AlarmTest ? REPORT_NAME_ALARM : REPORT_NAME_WALKING);
            var pathReport = Path.GetTempPath() + @"\" + DateTime.Now.ToString(cstrDateTimeFormatNoMiliSecond2) + ".xlsx";
            var pathReportTemplate = Config.PathReportTemplate + fileNameReport;

            if (!Directory.Exists(pathFolder))
            {
                ShowMsg(MessageBoxIcon.Warning, "Data not found.");
                return;
            }

            if (!File.Exists(pathReportTemplate))
            {
                ShowMsg(MessageBoxIcon.Warning, "Excel template not found.");
                return;
            }

            using (var saveFileDialog = SaveExcelDialog(DateTime.Now.ToString(cstrDateTimeFormatNoMiliSecond2) + "_report"))
            {
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var searchPattern = clsConfig.FILE_NAME_DETAIL + @"*.csv";
                var arrData = Directory.GetFiles(pathFolder, searchPattern);
                var cnn = 0;

                try
                {
                    Array.Sort(arrData);

                    var name = Path.GetFileName(pathFolder.TrimEnd('\\'));
                    var arr = name.Split('_');

                    if (arr.Length < 8)
                    {
                        ShowMsg(MessageBoxIcon.Error, MSG_ERR_PROCESS);
                        return;
                    }

                    File.Copy(pathReportTemplate, pathReport, true);

                    using (var objExport = new clsExportReport(pathReport))
                    {
                        var intType = clsCommon.CnvNullToString(arr[0]) == ALARM_TEST_KEY ? (int)emMeasureType.AlarmTest : (int)emMeasureType.WalkingTest;
                        var intResult = clsCommon.CnvNullToInt(arr[7]);
                        var dtMeasureS = DateTime.MinValue;
                        var dtMeasureE = DateTime.MaxValue;

                        DateTime.TryParseExact(arr[5], cstrDateTimeFormatNoMiliSecond2, null, DateTimeStyles.None, out dtMeasureS);
                        DateTime.TryParseExact(arr[6], cstrDateTimeFormatNoMiliSecond2, null, DateTimeStyles.None, out dtMeasureE);

                        objExport.WriteMeasureInfo(new MeasureInfo
                        {
                            ReportDate = dtMeasureS,
                            MeasureStart = dtMeasureS,
                            MeasureEnd = dtMeasureE,
                            MeasureType = emType,
                            MeasureResult = clsCommon.MeasureResultDisplay(intResult),
                            AlarmValue = clsCommon.CnvNullToInt(arr[2]),
                            FailLevel = clsCommon.CnvNullToInt(arr[3]),
                            Period = clsCommon.CnvNullToInt(arr[4]),
                            //DeviceName = clsCommon.CnvNullToInt(arr[1]) == 1 ? Settings.Default.DeviceName1 : Settings.Default.DeviceName2
                        });

                        var rowStart = (emType == emMeasureType.AlarmTest ? clsExportReport.ROW_START_ALARM : clsExportReport.ROW_START_WALKING);

                        foreach (var p in arrData)
                        {
                            if (!File.Exists(p))
                            {
                                continue;
                            }

                            using (var reader = new StreamReader(p))
                            {
                                while (!reader.EndOfStream)
                                {
                                    var line = reader.ReadLine();
                                    var values = line.TrimStart('"').TrimEnd('"').Split(new string[] { "\",\"" }, StringSplitOptions.None);

                                    if (objExport.WriteMeasureDetail(rowStart, new MeasureDetail
                                    {
                                        No = ++cnn,
                                        Time = clsCommon.CnvStringToDateTimeNull(values[0], null, cstrDateTimeFormatMiliSecond),
                                        Value = clsCommon.CnvNullToInt(values[4]),
                                        Result = clsCommon.MeasureResultDisplay(clsCommon.CnvNullToInt(values[5]))
                                    }, emType))
                                    {
                                        rowStart++;
                                    }
                                }
                            }
                        }

                        if (emType == emMeasureType.WalkingTest)
                        {
                            searchPattern = clsConfig.FILE_NAME_LIMIT + @"*.csv";
                            arrData = Directory.GetFiles(pathFolder, searchPattern);
                            cnn = 0;
                            Array.Sort(arrData);
                            rowStart = clsExportReport.ROW_START_WALKING_LIMIT;

                            foreach (var p in arrData)
                            {
                                if (!File.Exists(p))
                                {
                                    continue;
                                }

                                using (var reader = new StreamReader(p))
                                {
                                    while (!reader.EndOfStream)
                                    {
                                        var line = reader.ReadLine();
                                        var values = line.TrimStart('"').TrimEnd('"').Split(new string[] { "\",\"" }, StringSplitOptions.None);

                                        if (objExport.WriteMeasureDetail(rowStart, new MeasureDetail
                                        {
                                            No = ++cnn,
                                            Time = clsCommon.CnvStringToDateTimeNull(values[0], null, cstrDateTimeFormatMiliSecond),
                                            Value = clsCommon.CnvNullToInt(values[4]),
                                            Result = clsCommon.MeasureResultDisplay(clsCommon.CnvNullToInt(values[5]))
                                        }, emType))
                                        {
                                            rowStart++;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    File.Copy(pathReport, saveFileDialog.FileName, true);

                    if (File.Exists(saveFileDialog.FileName))
                    {
                        if (!ComfirmMsg("Do you want open file report?"))
                        {
                            return;
                        }

                        Process.Start(saveFileDialog.FileName);
                        return;
                    }
                    else
                    {
                        ShowMsg(MessageBoxIcon.Error, "Export Excel erors.", Text);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowMsg(MessageBoxIcon.Error, MSG_ERR_PROCESS);
                }
            }
        }

        #endregion Event Form

        #region Private Function

        private void IntForm()
        {
            dgvMeasure.AutoGenerateColumns = false;
            dgvMeasureDetail.AutoGenerateColumns = false;
            dgvMeasure.DataBindingComplete += DGVDataBindingComplete;
            dgvMeasureDetail.DataBindingComplete += DGVDataBindingComplete;

            BindingDateTimePicker();
            BindingCboType();
            BindingCboDevice();

            BindingCboTime(cboSubHourS, 24);
            BindingCboTime(cboSubMimuteS, 60);
            BindingCboTime(cboSubSecondS, 60);
            BindingCboTime(cboSubHourE, 24);
            BindingCboTime(cboSubMimuteE, 60);
            BindingCboTime(cboSubSecondE, 60);

            LoadData();
        }

        private void BindingDateTimePicker()
        {
            var now = DateTime.Now;
            dtpStart.Value = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            dtpEnd.Value = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
        }

        private void BindingCboType()
        {
            var tblBinding = new DataTable();
            tblBinding.Columns.Add(DISPLAY, typeof(string));
            tblBinding.Columns.Add(VALUE, typeof(int));
            tblBinding.Rows.Add(new object[] { SELECT_ALL_SHOW, SELECT_ALL });
            tblBinding.Rows.Add(new object[] { ALARM_TEST, (int)emMeasureType.AlarmTest });
            tblBinding.Rows.Add(new object[] { WALKING_TEST, (int)emMeasureType.WalkingTest });

            BindingDataTableToComboBox(cmbType, tblBinding, DISPLAY, VALUE, SELECT_ALL);
        }

        private void BindingCboDevice()
        {
            var tblBinding = new DataTable();
            tblBinding.Columns.Add(DISPLAY, typeof(string));
            tblBinding.Columns.Add(VALUE, typeof(int));
            tblBinding.Rows.Add(new object[] { SELECT_ALL_SHOW, SELECT_ALL });

            var lstDeviceInfo = clsSuportSerialize.BinDeserialize<List<DeviceInfo>>(clsConfig.SQLITE_DB_PATH);

            if (lstDeviceInfo != null)
            {
                foreach (var device in lstDeviceInfo)
                {
                    tblBinding.Rows.Add(new object[] { device.DeviceName, device.DeviceId });
                }
            }

            BindingDataTableToComboBox(cboDevice, tblBinding, DISPLAY, VALUE, SELECT_ALL);
        }

        private void LoadData()
        {
            dgvMeasure.DataSource = null;
            dgvMeasureDetail.Rows.Clear();
            pnlFilter.Enabled = false;

            var arrData = clsCommon.GetListSubFolder(clsConfig.PathDataMeasure);

            if (arrData == null || arrData.Length == 0)
            {
                return;
            }

            var tblMeasure = new DataTable();
            tblMeasure.Columns.Add(MeasureID.DataPropertyName, typeof(string));
            tblMeasure.Columns.Add(No.DataPropertyName, typeof(string));
            tblMeasure.Columns.Add(DeviceName.DataPropertyName, typeof(string));
            tblMeasure.Columns.Add(AlarmValue.DataPropertyName, typeof(int));
            tblMeasure.Columns.Add(FailLevel.DataPropertyName, typeof(int));
            tblMeasure.Columns.Add(colPeriod.DataPropertyName, typeof(int));
            tblMeasure.Columns.Add(StartTime.DataPropertyName, typeof(string));
            tblMeasure.Columns.Add(EndTime.DataPropertyName, typeof(string));
            tblMeasure.Columns.Add(MeasureType.DataPropertyName, typeof(int));
            tblMeasure.Columns.Add(Result.DataPropertyName, typeof(int));

            var cnn = 0;

            // Get value for search
            var selectedCboType = cmbType.SelectedIndex > -1 && cmbType.SelectedValue != null ? clsCommon.CnvNullToInt(cmbType.SelectedValue) : SELECT_ALL;
            var selectedCboResult = cmbResult.SelectedIndex > -1 && cmbResult.SelectedValue != null ? clsCommon.CnvNullToInt(cmbResult.SelectedValue) : SELECT_ALL;
            var selectedCboDevice = cboDevice.SelectedIndex > -1 && cboDevice.SelectedValue != null ? clsCommon.CnvNullToInt(cboDevice.SelectedValue) : SELECT_ALL;
            var dtStart = new DateTime(dtpStart.Value.Year, dtpStart.Value.Month, dtpStart.Value.Day, 0, 0, 0);
            var dtEnd = new DateTime(dtpEnd.Value.Year, dtpEnd.Value.Month, dtpEnd.Value.Day, 23, 59, 59);
            var lstDeviceInfo = clsSuportSerialize.BinDeserialize<List<DeviceInfo>>(clsConfig.SQLITE_DB_PATH);

            foreach (var data in arrData)
            {
                if (data.StartsWith(clsConfig.FOLDER_NAME_TEMP))
                {
                    continue;
                }

                var name = Path.GetFileName(data);
                var arr = name.Split('_');

                if (arr.Length < 8)
                {
                    continue;
                }

                var intType = clsCommon.CnvNullToString(arr[0]) == ALARM_TEST_KEY ? (int)emMeasureType.AlarmTest : (int)emMeasureType.WalkingTest;
                var intResult = clsCommon.CnvNullToInt(arr[7]);
                var intDeviceId = clsCommon.CnvNullToInt(arr[1]);
                var dtMeasureS = DateTime.MinValue;
                var dtMeasureE = DateTime.MaxValue;

                DateTime.TryParseExact(arr[5], cstrDateTimeFormatNoMiliSecond2, null, DateTimeStyles.None, out dtMeasureS);
                DateTime.TryParseExact(arr[6], cstrDateTimeFormatNoMiliSecond2, null, DateTimeStyles.None, out dtMeasureE);

                // Filter time
                if (dtMeasureS < dtStart || dtEnd < dtMeasureE)
                {
                    continue;
                }

                // Filter type
                if (selectedCboType != SELECT_ALL && selectedCboType != intType)
                {
                    continue;
                }

                // Filter result
                if (selectedCboResult != SELECT_ALL && selectedCboResult != intResult)
                {
                    continue;
                }

                // Filter Device
                if (selectedCboDevice != SELECT_ALL && selectedCboDevice != intDeviceId)
                {
                    continue;
                }

                tblMeasure.Rows.Add(new object[]
                {
                    data,
                    ++cnn,
                    lstDeviceInfo != null ? lstDeviceInfo.Find(i => i.DeviceId == intDeviceId)?.DeviceName : "",
                    clsCommon.CnvNullToInt(arr[2]),
                    clsCommon.CnvNullToInt(arr[3]),
                    clsCommon.CnvNullToInt(arr[4]),
                    clsCommon.CnvNullToString(arr[5]),
                    clsCommon.CnvNullToString(arr[6]),
                    intType,
                    intResult,
                });
            }

            dgvMeasure.DataSource = tblMeasure;
        }

        private void LoadDataSub(string path)
        {
            dgvMeasureDetail.Rows.Clear();

            if (Directory.Exists(path))
            {
                var dtS = new DateTime(dtpSubDateS.Value.Year,
                                       dtpSubDateS.Value.Month,
                                       dtpSubDateS.Value.Day,
                                       (int)cboSubHourS.SelectedValue,
                                       (int)cboSubMimuteS.SelectedValue,
                                       (int)cboSubSecondS.SelectedValue,
                                       000);
                var dtE = new DateTime(dtpSubDateE.Value.Year,
                                       dtpSubDateE.Value.Month,
                                       dtpSubDateE.Value.Day,
                                       (int)cboSubHourE.SelectedValue,
                                       (int)cboSubMimuteE.SelectedValue,
                                       (int)cboSubSecondE.SelectedValue,
                                       999);
                var searchPattern = (!chkView.Checked ? clsConfig.FILE_NAME_DETAIL : clsConfig.FILE_NAME_LIMIT) + @"*.csv";
                var arrData = Directory.GetFiles(path, searchPattern);
                var cnn = 0;
                Array.Sort(arrData);

                foreach (var p in arrData)
                {
                    if (!File.Exists(p))
                    {
                        continue;
                    }

                    using (var reader = new StreamReader(p))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var values = line.TrimStart('"').TrimEnd('"').Split(new string[] { "\",\"" }, StringSplitOptions.None);
                            var dtTime = clsCommon.CnvStringToDateTime(values[0], cstrDateTimeFormatMiliSecond);

                            if (dtTime < dtS || dtTime > dtE)
                            {
                                continue;
                            }

                            SetGridValue(new object[]
                            {
                                ++cnn,
                                values[0],
                                clsCommon.CnvNullToInt(values[4]),
                                values[5],
                            }, dgvMeasureDetail);
                        }
                    }
                }

                if (dgvMeasure.CurrentCell != null)
                {
                    var intMeasureType = clsCommon.CnvNullToInt(dgvMeasure.Rows[dgvMeasure.CurrentCell.RowIndex].Cells[MeasureType.Index].Value);
                    var intFailLevel = clsCommon.CnvNullToInt(dgvMeasure.Rows[dgvMeasure.CurrentCell.RowIndex].Cells[FailLevel.Index].Value);

                    if (intMeasureType == (int)emMeasureType.WalkingTest && chkView.Checked && dgvMeasureDetail.Rows.Count > 0)
                    {
                        var totalValue = 0;

                        foreach (DataGridViewRow row in dgvMeasureDetail.Rows)
                        {
                            totalValue += Math.Abs(clsCommon.CnvNullToInt(row.Cells[ValueDetail.Index].Value));
                        }

                        SetGridValue(new object[]
                        {
                        ++cnn,
                        "Average",
                        string.Format("{0} ({1})",
                                      Math.Ceiling((float)totalValue / dgvMeasureDetail.Rows.Count).ToString("N0"),
                                      clsCommon.MeasureResultDisplay((int)(totalValue < intFailLevel ? emMeasureResult.Pass : emMeasureResult.Fail))),
                        0,
                        }, dgvMeasureDetail);

                        var indexLastRow = dgvMeasureDetail.Rows.Count - 1;
                        var styleCell = dgvMeasureDetail.Rows[indexLastRow].DefaultCellStyle;
                        styleCell.BackColor = Color.NavajoWhite;
                        styleCell.Font = new Font(styleCell.Font ?? SystemFonts.DefaultFont, FontStyle.Bold);
                        dgvMeasureDetail.Rows[indexLastRow].Cells[ValueDetail.Index].Style.ForeColor = totalValue < intFailLevel ? Color.Green : Color.Red;
                    }
                }
            }
        }

        #endregion Private Function
    }
}
