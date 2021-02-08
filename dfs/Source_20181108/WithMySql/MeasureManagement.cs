using BaseCommon;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static BaseCommon.clsConst;

namespace WithMySql
{

    public partial class MeasureManagement : BaseForm
    {
        // Msg
        private string MSG_BLANK_FOLDER = "Folder errors is blank";
        private const string COMFIRM_MSG_DELETE = "Do you want delete this record?";
        private string COMFIRM_NEXT_FILE = "You want to next File?";
        private string FIX_ERR_SUCCESS = "Fix Measure Success";
        
        // DB
        private clsDBUltity _objDB = new clsDBUltity();

        #region Event Form

        public MeasureManagement()
        {
            InitializeComponent();

            try
            {
                IntForm();
            }
            catch (Exception ex)
            {
                ShowMsg(MessageBoxIcon.Error, MSG_ERR_PROCESS);
            }
        }

        private void MeasureManagement_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_objDB != null)
            {
                _objDB.Dispose();
                _objDB = null;
            }
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
        
        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cmbStatus.Visible || cmbStatus.SelectedIndex < 0)
            {
                return;
            }

            cmbResult.SelectedValue = SELECT_ALL;
            cmbResult.Enabled = (int)cmbStatus.SelectedValue == (int)emMeasureStatus.Complete;
        }

        // Format grid measure
        private void dgvMeasure_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == No.Index)
            {
                e.Value = e.RowIndex + 1;
            } else if (e.ColumnIndex == MeasureType.Index)
            {
                var intMeasureType = clsCommon.CnvNullToInt(e.Value);
                e.Value = string.Empty;

                if (intMeasureType == (int)clsDBUltity.emMeasureType.AlarmTest)
                {
                    e.Value = ALARM_TEST;
                }
                else if (intMeasureType == (int)clsDBUltity.emMeasureType.WalkingTest)
                {
                    e.Value = WALKING_TEST;
                }
            } else if (e.ColumnIndex == Result.Index)
            {
                e.Value = clsCommon.MeasureResultDisplay(clsCommon.CnvNullToInt(e.Value));
            }
        }

        // Format grid measure detail
        private void dgvMeasureDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == NoDetail.Index)
            {
                e.Value = e.RowIndex + 1;
            }

            if (e.ColumnIndex == ResultDetail.Index)
            {
                e.Value = clsCommon.MeasureResultDisplay(clsCommon.CnvNullToInt(e.Value));
            }
        }

        private void dgvMeasure_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            var measureid = clsCommon.CnvNullToInt(dgvMeasure.Rows[e.RowIndex].Cells[MeasureID.Index].Value);

            try
            {
                if (e.ColumnIndex == dgvMeasure.Columns[colDelete.Index].Index)
                {
                    // Delete
                    if (!ComfirmMsg(COMFIRM_MSG_DELETE))
                    {
                        ClearSelectionDgv(dgvMeasure);
                        return;
                    }

                    _objDB.DeleteMeasure(measureid);
                    LoadData();
                }
                else if (e.ColumnIndex == dgvMeasure.Columns[colEnable.Index].Index)
                {
                    // Enable
                    var pathErrors = clsConfig.PathDataErrors + @"\" + measureid;
                    string[] files = null;

                    if (Directory.Exists(pathErrors))
                    {
                        files = Directory.GetFiles(pathErrors);
                    }
                    else
                    {
                        using (var fbd = new FolderBrowserDialog())
                        {
                            DialogResult result = fbd.ShowDialog();

                            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                            {
                                files = Directory.GetFiles(fbd.SelectedPath);
                            }
                            else
                            {
                                ClearSelectionDgv(dgvMeasure);
                                return;
                            }
                        }
                    }

                    if (files == null || files.Length == 0)
                    {
                        ShowMsg(MessageBoxIcon.Warning, MSG_BLANK_FOLDER);
                    }

                    xExecuteNonQueryFromFile(files, measureid);
                } else
                {
                    // Load filter
                    var dtStartTime = CnvDBToDateTimePickerValue(dgvMeasure.Rows[e.RowIndex].Cells[StartTime.Index].Value);
                    var dtEndTime = CnvDBToDateTimePickerValue(dgvMeasure.Rows[e.RowIndex].Cells[EndTime.Index].Value);
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

                    LoadDataSub(measureid);
                }
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

            var rowIndex = dgvMeasure.CurrentCell.RowIndex;

            try
            {
                LoadDataSub(clsCommon.CnvNullToInt(dgvMeasure.Rows[rowIndex].Cells[MeasureID.Index].Value));
            }
            catch (Exception ex)
            {

                ShowMsg(MessageBoxIcon.Error, MSG_ERR_PROCESS);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if(dgvMeasure.Rows.Count == 0)
            {
                ShowMsg(MessageBoxIcon.Warning, "Data not found.");
                return;
            }

            if (dgvMeasure.CurrentCell == null)
            {
                ShowMsg(MessageBoxIcon.Warning, "Please select 1 row data to export excel.");
                return;
            }

            var strSDate = dgvMeasure.CurrentRow.Cells[StartTime.Index].Value.ToString();
            var strEDate = dgvMeasure.CurrentRow.Cells[EndTime.Index].Value.ToString();
            int intUser = clsCommon.CnvNullToInt(cmbUser.SelectedValue);
            int intType = clsCommon.CnvNullToInt(cmbType.SelectedValue);
            //int intResult = clsCommon.CnvNullToInt(cmbResult.SelectedValue);


            var objMeasureS = dgvMeasure.CurrentRow.Cells[StartTime.Index].Value;
            var objMeasureE = dgvMeasure.CurrentRow.Cells[EndTime.Index].Value;
            var dtMeasureS = objMeasureS != null && objMeasureS != DBNull.Value ? (DateTime?)objMeasureS : null;
            var dtMeasureE = objMeasureE != null && objMeasureE != DBNull.Value ? (DateTime?)objMeasureE : null;
            var intMeasureType = clsCommon.CnvNullToInt(dgvMeasure.CurrentRow.Cells[MeasureType.Index].Value);
            var intAlarmValue = clsCommon.CnvNullToInt(dgvMeasure.CurrentRow.Cells[AlarmValue.Index].Value);
            var intFailLevel = clsCommon.CnvNullToInt(dgvMeasure.CurrentRow.Cells[FailLevel.Index].Value);
            var intPeriod = clsCommon.CnvNullToInt(dgvMeasure.CurrentRow.Cells[colPeriod.Index].Value);
            var intResult = clsCommon.CnvNullToInt(dgvMeasure.CurrentRow.Cells[Result.Index].Value);
            var intMeasureId = clsCommon.CnvNullToInt(dgvMeasure.CurrentRow.Cells[MeasureID.Index].Value);
            var strUserName = clsCommon.CnvNullToString(dgvMeasure.CurrentRow.Cells[User.Index].Value);
            var strDeviceName = clsCommon.CnvNullToString(dgvMeasure.CurrentRow.Cells[DeviceName.Index].Value);
            var strType = intMeasureType == (int)clsDBUltity.emMeasureType.AlarmTest ? ALARM_TEST : WALKING_TEST;
            var emType = intMeasureType == (int)emMeasureType.AlarmTest ? emMeasureType.AlarmTest : emMeasureType.WalkingTest;
            var pathReport = Path.GetTempPath() + @"\" + DateTime.Now.ToString(cstrDateTimeFormatNoMiliSecond2) + "xlsx";
            var fileNameReport = (intMeasureType == (int)emMeasureType.AlarmTest ? REPORT_NAME_ALARM : REPORT_NAME_WALKING);
            var pathReportTemplate = Config.PathReportTemplate + fileNameReport;
            var cnn = 0;

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

                File.Copy(pathReportTemplate, pathReport, true);
                
                using (var objExport = new clsExportReport(pathReport))
                {
                    // Header
                    objExport.WriteMeasureInfo(new MeasureInfo
                    {
                        MeasureId = intMeasureId,
                        UserName = strUserName,
                        ReportDate = dtMeasureS,
                        MeasureStart = dtMeasureS,
                        MeasureEnd = dtMeasureE,
                        MeasureType = emType,
                        MeasureResult = clsCommon.MeasureResultDisplay(intResult),
                        AlarmValue = intAlarmValue,
                        FailLevel = intFailLevel,
                        Period = intPeriod,
                        DeviceName = strDeviceName
                    }, false);

                    // Limit
                    if (emType == emMeasureType.WalkingTest)
                    {
                        var dataLimit = _objDB.GetTBLMeasureDetail(intMeasureId.ToString(), true);
                        var rowStartLimit = clsExportReport.ROW_START_WALKING_LIMIT + 1;
                        cnn = 0;

                        foreach (DataRow row in dataLimit.Rows)
                        {
                            if (objExport.WriteMeasureDetail(rowStartLimit, new MeasureDetail
                            {
                                No = ++cnn,
                                Time = clsCommon.CnvStringToDateTimeNull(row["samples_time"]),
                                Value = clsCommon.CnvNullToInt(row["actual_delegate"]),
                                Result = clsCommon.MeasureResultDisplay(clsCommon.CnvNullToInt(row["result"])),
                            }, emType))
                            {
                                rowStartLimit++;
                            }
                        }
                    }

                    // Detail
                    var dataDetail = _objDB.GetTBLMeasureDetail(intMeasureId.ToString(), false);
                    var rowStarDetail = (emType == emMeasureType.AlarmTest ? clsExportReport.ROW_START_ALARM : clsExportReport.ROW_START_WALKING) + 1;
                    cnn = 0;

                    foreach (DataRow row in dataDetail.Rows)
                    {
                        if (objExport.WriteMeasureDetail(rowStarDetail, new MeasureDetail
                        {
                            No = ++cnn,
                            Time = clsCommon.CnvStringToDateTimeNull(row["samples_time"]),
                            Value = clsCommon.CnvNullToInt(row["actual_delegate"]),
                            Result = clsCommon.MeasureResultDisplay(clsCommon.CnvNullToInt(row["result"])),
                        }, emType))
                        {
                            rowStarDetail++;
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
        }

        #endregion Event Form

        #region Private Function

        private void IntForm()
        {
            dgvMeasure.AutoGenerateColumns = false;
            dgvMeasureDetail.AutoGenerateColumns = false;
            dgvMeasure.DataBindingComplete += DGVDataBindingComplete;
            dgvMeasureDetail.DataBindingComplete += DGVDataBindingComplete;

            if (clsConfig.ModeApp == clsConfig.emModeApp.User)
            {
                lblStatus.Visible = true;
                cmbStatus.Visible = true;
                dgvMeasure.Columns[colDelete.Index].Visible = true;
            }

            BindingDateTimePicker();
            BindingCboTime(cboSubHourS, 24);
            BindingCboTime(cboSubMimuteS, 60);
            BindingCboTime(cboSubSecondS, 60);
            BindingCboTime(cboSubHourE, 24);
            BindingCboTime(cboSubMimuteE, 60);
            BindingCboTime(cboSubSecondE, 60);
            BindingCboUser();
            BindingCboType();
            BindingCboStatus();

            LoadData();
        }
        
        private void LoadData()
        {
            var dtStart = new DateTime(dtpStart.Value.Year, dtpStart.Value.Month, dtpStart.Value.Day, 0, 0, 0);
            var dtEnd = new DateTime(dtpEnd.Value.Year, dtpEnd.Value.Month, dtpEnd.Value.Day, 23, 59, 59);

            var strSDate = dtStart.ToString(clsDBUltity.cstrDateTimeFormatNoMiliSecond);
            var strEDate = dtEnd.ToString(clsDBUltity.cstrDateTimeFormatNoMiliSecond);
            int intUser = cmbUser.SelectedValue != null ? clsCommon.CnvNullToInt(cmbUser.SelectedValue) : SELECT_ALL;
            int intType = cmbType.SelectedValue != null ? clsCommon.CnvNullToInt(cmbType.SelectedValue) : SELECT_ALL;
            int intResult = cmbResult.SelectedValue != null ? clsCommon.CnvNullToInt(cmbResult.SelectedValue) : SELECT_ALL;
            int intStatus = cmbStatus.SelectedValue != null ? clsCommon.CnvNullToInt(cmbStatus.SelectedValue) : SELECT_ALL;

            pnlFilter.Enabled = false;
            dgvMeasureDetail.DataSource = null;
            dgvMeasure.DataSource = _objDB.GetTBLMeasure(strSDate, strEDate, intUser, intType, intResult, intStatus);
            dgvMeasure.Columns[colEnable.Index].Visible = intStatus == (int)emMeasureStatus.Error && clsConfig.ModeApp == clsConfig.emModeApp.Admin;
        }

        private void LoadDataSub(int measureId)
        {
            DataTable dtDetail = null;

            if (pnlFilter.Enabled)
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
                dtDetail = _objDB.GetTBLMeasureDetail(measureId.ToString(), chkView.Checked, dtS, dtE);
            } else
            {
                dtDetail = _objDB.GetTBLMeasureDetail(measureId.ToString(), chkView.Checked);
            }

            dgvMeasureDetail.DataSource = dtDetail;

            if (dgvMeasure.CurrentCell != null)
            {
                var intMeasureType = clsCommon.CnvNullToInt(dgvMeasure.Rows[dgvMeasure.CurrentCell.RowIndex].Cells[MeasureType.Index].Value);
                var intFailLevel = clsCommon.CnvNullToInt(dgvMeasure.Rows[dgvMeasure.CurrentCell.RowIndex].Cells[FailLevel.Index].Value);

                if (intMeasureType == (int)emMeasureType.WalkingTest && chkView.Checked && !clsCommon.TableIsNullOrEmpty(dtDetail))
                {
                    dgvMeasureDetail.DataSource = null;
                    var cnn = 0;
                    foreach (DataRow row in dtDetail.Rows)
                    {
                        SetGridValue(new object[]
                        {
                            ++cnn,
                            row[TimeDetail.DataPropertyName],
                            row[ValueDetail.DataPropertyName],
                            row[ResultDetail.DataPropertyName],
                        }, dgvMeasureDetail);
                    }

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

        private void BindingCboUser()
        {
            var tblUser = _objDB.GetMUser();

            if (tblUser != null)
            {
                var rowFirst = tblUser.NewRow();
                rowFirst["username"] = SELECT_ALL_SHOW;
                rowFirst["userid"] = SELECT_ALL;
                tblUser.Rows.InsertAt(rowFirst, 0);

                BindingDataTableToComboBox(cmbUser, tblUser, "username", "userid", clsConfig.UserLoginId);

                if (cmbUser.Items.Count > 0 && cmbUser.SelectedIndex == -1)
                {
                    cmbUser.SelectedValue = SELECT_ALL;
                }
            }
        }

        private void BindingCboStatus()
        {
            if (cmbStatus.Visible)
            {
                return;
            }

            var tblBinding = new DataTable();
            tblBinding.Columns.Add(DISPLAY, typeof(string));
            tblBinding.Columns.Add(VALUE, typeof(int));
            tblBinding.Rows.Add(new object[] { "Complete", (int)emMeasureStatus.Complete });
            tblBinding.Rows.Add(new object[] { "Error", (int)emMeasureStatus.Error });

            BindingDataTableToComboBox(cmbStatus, tblBinding, DISPLAY, VALUE, (int)emMeasureStatus.Complete);
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

        private void BindingDateTimePicker()
        {
            var now = DateTime.Now;
            dtpStart.Value = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            dtpEnd.Value = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
        }

        private void xExecuteNonQueryFromFile(string[] files, int measureid_selected)
        {
            if (files == null || files.Length == 0)
            {
                ShowMsg(MessageBoxIcon.Warning, "");
                return;
            }

            bool flagCheck = false;
            foreach (var path in files)
            {
                var filename = Path.GetFileName(path);
                var arrTemp = filename.Split('_');

                if (arrTemp.Length == 3)
                {
                    var measureId = clsCommon.CnvNullToInt(arrTemp[0]);

                    if (measureId == measureid_selected)
                    {
                        using (var db = new clsDBUltity())
                        {
                            flagCheck = db.ExecuteNonQueryFromFile(path, false);

                            if (!flagCheck)
                            {
                                ShowMsg(MessageBoxIcon.Error, path);
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (!ComfirmMsgErr(path + Environment.NewLine + COMFIRM_NEXT_FILE))
                        {
                            return;
                        }
                    }
                }
                else
                {
                    if (!ComfirmMsgErr(path + Environment.NewLine + COMFIRM_NEXT_FILE))
                    {
                        return;
                    }
                }
            }

            if (flagCheck)
            {
                LoadData();
                ShowMsg(MessageBoxIcon.Information, FIX_ERR_SUCCESS);
            }
        }

        private DateTime CnvDBToDateTimePickerValue(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return DateTime.Now;
            }

            try
            {
                return (DateTime)obj;
            }
            catch
            {
                return DateTime.Now;
            }
        }

        #endregion Private Function
        
        //private string ValidateExportExcel(string tempFile, string name = "Book1")
        //{
        //    var saveFileDialog1 = new SaveFileDialog
        //    {
        //        DefaultExt = "xlsx",
        //        Filter = "Excel Workbook (*.xls, *.xlsx)|*.xls;*.xlsx",
        //        AddExtension = true,
        //        RestoreDirectory = true,
        //        Title = "Save as",
        //        FileName = name,
        //        InitialDirectory = @"D:\",
        //    };

        //    if (saveFileDialog1.ShowDialog() != DialogResult.OK)
        //    {
        //        return "";
        //    }

        //    var targetPath = saveFileDialog1.FileName;

        //    return targetPath;
        //}
    }
}
