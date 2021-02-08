using BaseCommon;
using BaseCommon.Core;
using BaseCommon.Utility;
using DSF602.Language;
using DSF602.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BaseCommon.clsConst;

namespace DSF602.View
{

    public partial class MeasureManagement : BaseForm
    {
        // Binding Combo
        private const int SELECT_ALL = -1;
        private string SELECT_ALL_SHOW = LanguageHelper.GetValueOf("SELECT_ALL_SHOW");

        private List<MUser> _mUsers;

        protected override void SetLanguageControl()
        {
            LanguageHelper.SetValueOf(this, "MEASURE_TITLE");
            LanguageHelper.SetValueOf(btnSearch, "MEASURE_BTN_SEARCH");
            LanguageHelper.SetValueOf(btnExport, "MEASURE_BTN_EXCEL");
            LanguageHelper.SetValueOf(btnFilter, "MEASURE_BTN_FILTER");
            LanguageHelper.SetValueOf(lblDate, "MEASURE_LBL_DATE");
            LanguageHelper.SetValueOf(lblUsers, "MEASURE_LBL_USERS");
            LanguageHelper.SetValueOf(lblResult, "MEASURE_LBL_RESULT");
            LanguageHelper.SetValueOf(lblType, "MEASURE_LBL_TYPE");
            LanguageHelper.SetValueOf(lblTime, "MEASURE_LBL_TIME");
            LanguageHelper.SetValueOf(chkView, "MEASURE_LBL_TEXT");
            LanguageHelper.SetValueOf(SensorName, "MEASURE_COL_SENSORNAME");
            LanguageHelper.SetValueOf(BlockName, "MEASURE_COL_BLOCKNAME");
            LanguageHelper.SetValueOf(AlarmValue, "MEASURE_COL_ALARMVALUE");
            LanguageHelper.SetValueOf(StartTime, "MEASURE_COL_STARTTIME");
            LanguageHelper.SetValueOf(EndTime, "MEASURE_COL_ENDTIME");
            LanguageHelper.SetValueOf(Result, "MEASURE_COL_RESULT");
            LanguageHelper.SetValueOf(User, "MEASURE_COL_USER");
            LanguageHelper.SetValueOf(TimeDetail, "MEASURE_COL_TIMEDETAIL");
            LanguageHelper.SetValueOf(ValueDetail, "MEASURE_COL_VALUEDETAIL");
            LanguageHelper.SetValueOf(ResultDetail, "MEASURE_COL_RESULTDETAIL");
            LanguageHelper.SetValueOf(lblBlock, "MEASURE_LBL_BLOCK");
            LanguageHelper.SetValueOf(lblSensor, "MEASURE_LBL_SENSOR");

        }

        public MeasureManagement()
        {
            InitializeComponent();

            try
            {
                IntForm();
            }
            catch (Exception ex)
            {
                ShowMsg(MessageBoxIcon.Error, LanguageHelper.GetValueOf("MSG_ERR_PROCESS"));
            }
        }

        #region Event Form

        // Format grid measure
        private void dgvMeasure_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == No.Index)
            {
                e.Value = e.RowIndex + 1;
            }
            else if (e.ColumnIndex == Result.Index)
            {
                e.Value = clsCommon.MeasureResultDisplay(ConvertHelper.CnvNullToInt(e.Value));
            }

            if (e.ColumnIndex == colDelete.Index)
            {
                e.Value = LanguageHelper.GetValueOf("USERS_COL_DELETE");
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
                e.Value = clsCommon.MeasureResultDisplay(ConvertHelper.CnvNullToInt(e.Value));
            }
        }

        private void cmbBlock_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbBlock.SelectedValue == null)
            {
                return;
            }

            if ((int)cmbBlock.SelectedValue < 0)
            {
                cmbSensor.Enabled = false;
            }
            else
            {
                int selectedValue = ConvertHelper.CnvNullToInt(cmbBlock.SelectedValue);
                using (var objDB = AppManager.GetConnection())
                {
                    var lstSensor = objDB.GetListSensor().FindAll(i => i.OfBlock == selectedValue);
                    if (lstSensor != null)
                    {
                        lstSensor.Insert(0, new SensorInfo { SensorId = SELECT_ALL, SensorName = SELECT_ALL_SHOW });
                        cmbSensor.Enabled = true;
                        BindingListToComboBox(cmbSensor, lstSensor, "SensorName", "SensorId");
                    }
                }
            }
        }

        private void dgvMeasure_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void chkView_CheckedChanged(object sender, EventArgs e)
        {
            btnFilter_Click(null, null);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.SetModeWaiting();

            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                ShowMsg(MessageBoxIcon.Error, LanguageHelper.GetValueOf("MSG_ERR_PROCESS"));
            }
            finally
            {
                this.SetModeWaiting(false);
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (dgvMeasure.CurrentCell == null)
            {
                return;
            }

            var rowIndex = dgvMeasure.CurrentCell.RowIndex;
            this.SetModeWaiting();

            try
            {
                LoadDataSub(ConvertHelper.CnvNullToInt(dgvMeasure.Rows[rowIndex].Cells[MeasureID.Index].Value), 
                    ConvertHelper.CnvNullToInt(dgvMeasure.Rows[rowIndex].Cells[SensorID.Index].Value));
            }
            catch (Exception ex)
            {
                ShowMsg(MessageBoxIcon.Error, LanguageHelper.GetValueOf("MSG_ERR_PROCESS"));
            }
            finally
            {
                this.SetModeWaiting(false);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dgvMeasure.Rows.Count == 0)
            {
                ShowMsg(MessageBoxIcon.Warning, LanguageHelper.GetValueOf("MSG_WAR_DATA"));
                return;
            }

            if (dgvMeasure.CurrentCell == null)
            {
                ShowMsg(MessageBoxIcon.Warning, LanguageHelper.GetValueOf("MSG_WAR_EXCEL"));
                return;
            }

            var objMeasureS = dgvMeasure.CurrentRow.Cells[StartTime.Index].Value;
            var objMeasureE = dgvMeasure.CurrentRow.Cells[EndTime.Index].Value;
            var dtMeasureS = objMeasureS != null && objMeasureS != DBNull.Value ? (DateTime?)objMeasureS : null;
            var dtMeasureE = objMeasureE != null && objMeasureE != DBNull.Value ? (DateTime?)objMeasureE : null;

            var intSensorId = ConvertHelper.CnvNullToInt(dgvMeasure.CurrentRow.Cells[SensorID.Index].Value);
            var intMeasureType = ConvertHelper.CnvNullToInt(dgvMeasure.CurrentRow.Cells[MeasureType.Index].Value);
            var intAlarmValue = ConvertHelper.CnvNullToInt(dgvMeasure.CurrentRow.Cells[AlarmValue.Index].Value);
            var intResult = ConvertHelper.CnvNullToInt(dgvMeasure.CurrentRow.Cells[Result.Index].Value);
            var intMeasureId = ConvertHelper.CnvNullToInt(dgvMeasure.CurrentRow.Cells[MeasureID.Index].Value);
            var strUserName = ConvertHelper.CnvNullToString(dgvMeasure.CurrentRow.Cells[User.Index].Value);
            var strDeviceName = ConvertHelper.CnvNullToString(dgvMeasure.CurrentRow.Cells[SensorName.Index].Value);
            var emType = intMeasureType == (int)emMeasureType.AlarmTest ? emMeasureType.AlarmTest : emMeasureType.WalkingTest;

            var pathReport = Path.GetTempPath() + @"\" + DateTime.Now.ToString(cstrDateTimeFormatNoMiliSecond2) + "xlsx";
            var fileNameReport = (intMeasureType == (int)emMeasureType.AlarmTest ? REPORT_NAME_ALARM : REPORT_NAME_WALKING);
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            fileNameReport = currentCulture + "_" + fileNameReport;
            var pathReportTemplate = AppManager.PathReportTemplate + fileNameReport;
            var cnn = 0;

            if (!File.Exists(pathReportTemplate))
            {
                pathReportTemplate = AppManager.PathReportTemplate + REPORT_NAME_ALARM;
                //ShowMsg(MessageBoxIcon.Warning, LanguageHelper.GetValueOf("MSG_WAR_EXCELTEMPLATE"));
                //return;
            }

            var threadExport = new Thread(() =>
            {
                this.SetModeWaiting();
                try
                {
                    using (var saveFileDialog = clsCommon.SaveExcelDialog(DateTime.Now.ToString(cstrDateTimeFormatNoMiliSecond2) + "_report"))
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
                                DeviceName = strDeviceName
                            });

                            // Detail
                            var keyDbName = DBManagerChild.GetDBName(intSensorId);
                            var dataDetail = AppManager.GetDBChildConnection(keyDbName).GetMeasureDetail(intMeasureId.ToString(), false, dtMeasureS, dtMeasureE);
                            var rowStarDetail = (emType == emMeasureType.AlarmTest ? clsExportReport.ROW_START_ALARM : clsExportReport.ROW_START_WALKING);

                            dataDetail.Select((measure) =>
                            {
                                if (objExport.WriteMeasureDetail(rowStarDetail, new clsConst.MeasureDetailExport
                                {
                                    No = ++cnn,
                                    Time = ConvertHelper.CnvStringToDateTimeNull(measure.Samples_time),
                                    Value = ConvertHelper.CnvNullToInt(measure.Actual_Value),
                                    Result = clsCommon.MeasureResultDisplay(measure.Detail_Result),

                                }, emType))
                                {
                                    rowStarDetail++;
                                }
                                return measure;
                            }).ToList();
                        }

                        File.Copy(pathReport, saveFileDialog.FileName, true);

                        if (File.Exists(saveFileDialog.FileName))
                        {
                            if (!ComfirmMsg(LanguageHelper.GetValueOf("MSG_COMFIRM_EXCEL")))
                            {
                                return;
                            }

                            Process.Start(saveFileDialog.FileName);
                            return;
                        }
                        else
                        {
                            ShowMsg(MessageBoxIcon.Error, LanguageHelper.GetValueOf("MSG_ERR_EXCEL"), Text);
                            return;
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error export to excel: " + ex.Message);
                }
                finally
                {
                    this.SetModeWaiting(false);
                }
            });
            threadExport.SetApartmentState(ApartmentState.STA);
            threadExport.Start();

        }

        #endregion Event Form

        #region Private Function

        private void IntForm()
        {
            this.Icon = Properties.Resources.report;
            dgvMeasure.DataBindingComplete += DGVDataBindingComplete;
            dgvMeasureDetail.DataBindingComplete += DGVDataBindingComplete;

            if ((int)UserManagement.emModeApp.User == AppManager.UserLogin.Role)
            {
                dgvMeasure.Columns[colDelete.Index].Visible = false;
            }

            using (var objDB = AppManager.GetConnection())
            {
                _mUsers = objDB.GetMUser();
            }

            BindingDateTimePicker();
            BindingCboTime(cboSubHourS, 24);
            BindingCboTime(cboSubMimuteS, 60);
            BindingCboTime(cboSubSecondS, 60);
            BindingCboTime(cboSubHourE, 24);
            BindingCboTime(cboSubMimuteE, 60);
            BindingCboTime(cboSubSecondE, 60);
            BindingCboUser();
            BindingCboResult();
            BindingCboBlock();

            chkView.Checked = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Params != null)
            {
                var sensorInfo = Params as SensorInfo;
                cmbBlock.SelectedValue = sensorInfo.OfBlock;
                cmbSensor.SelectedValue = sensorInfo.SensorId;
            }

            LoadData();
        }

        private void LoadData()
        {
            var dtStart = new DateTime(dtpStart.Value.Year, dtpStart.Value.Month, dtpStart.Value.Day, 0, 0, 0);
            var dtEnd = new DateTime(dtpEnd.Value.Year, dtpEnd.Value.Month, dtpEnd.Value.Day, 23, 59, 59);

            var strSDate = dtStart.ToString(clsConst.cstrDateTimeFormatNoMiliSecond);
            var strEDate = dtEnd.ToString(clsConst.cstrDateTimeFormatNoMiliSecond);
            int intUser = cmbUser.SelectedValue != null ? ConvertHelper.CnvNullToInt(cmbUser.SelectedValue) : SELECT_ALL;
            int intResult = cmbResult.SelectedValue != null ? ConvertHelper.CnvNullToInt(cmbResult.SelectedValue) : SELECT_ALL;

            pnlFilter.Enabled = false;
            dgvMeasureDetail.DataSource = null;

            var lstAllSensor = AppManager.ListSensor;
            var lstBlock = AppManager.ListBlock;
            List<Measure> lstMeasure = new List<Measure>();

            if ((int)cmbBlock.SelectedValue < 0)
            {
                //foreach (var sensor in lstAllSensor)
                //{
                //    var keyDbName = DBManagerChild.GetDBName(sensor.SensorId);
                //    var measureOfSensor = AppManager.GetDBChildConnection(keyDbName)
                //                                 .GetAllMeasure(strSDate, strEDate, intUser, intResult);
                //    lstMeasure = measureOfSensor.Concat(lstMeasure).ToList();
                //}

                lstAllSensor.Select((sensor) =>
                {
                    var keyDbName = DBManagerChild.GetDBName(sensor.SensorId);
                    lstMeasure.AddRange(AppManager.GetDBChildConnection(keyDbName).GetAllMeasure(strSDate, strEDate, intUser, intResult));
                    return sensor;
                }).ToList();
            }
            else if ((int)cmbSensor.SelectedValue < 0)
            {
                //var lstSensorOfBlock = lstAllSensor.FindAll(i => i.OfBlock == (int)cmbBlock.SelectedValue);

                //foreach (var item in lstSensorOfBlock)
                //{
                //    var keyDbName = DBManagerChild.GetDBName(item.SensorId);
                //    var measureOfSensor = AppManager.GetDBChildConnection(keyDbName)
                //                                 .GetAllMeasure(strSDate, strEDate, intUser, intResult);
                //    lstMeasure = measureOfSensor.Concat(lstMeasure).ToList();
                //}

                lstAllSensor.Where(i => i.OfBlock == (int)cmbBlock.SelectedValue).Select((sensor) =>
                {
                    var keyDbName = DBManagerChild.GetDBName(sensor.SensorId);
                    lstMeasure.AddRange(AppManager.GetDBChildConnection(keyDbName).GetAllMeasure(strSDate, strEDate, intUser, intResult));
                    return sensor;
                }).ToList();
            }
            else
            {
                var keyDbName = DBManagerChild.GetDBName((int)cmbSensor.SelectedValue);
                lstMeasure.AddRange(AppManager.GetDBChildConnection(keyDbName).GetAllMeasure(strSDate, strEDate, intUser, intResult));
            }

            var query = (from m in lstMeasure
                         join u in _mUsers on m.UserId equals u.UserId
                         join s in lstAllSensor on m.SensorId equals s.SensorId
                         join b in lstBlock on s.OfBlock equals b.BlockId
                         select new DGVMeasureInfo
                         {
                             MeasureId = m.MeasureId,
                             SensorId = m.SensorId,
                             Alarm_Value = m.Alarm_Value,
                             FullName = u.UserName,
                             SensorName = s.SensorName,
                             BlockName = b.BlockName,
                             Measure_Result = m.Measure_Result,
                             Start_time = m.Start_time,
                             End_time = m.End_time
                         }).OrderBy(x => x.MeasureId).ToList();

            dgvMeasure.DataSource = query;

            if (dgvMeasure.Rows.Count > 0)
            {
                dgvMeasure.Select();
                dgvMeasure.FirstDisplayedScrollingRowIndex = dgvMeasure.Rows.Count - 1;
                dgvMeasure.Rows[dgvMeasure.Rows.Count - 1].Selected = true;
            }
        }

        private void LoadDataSub(int measureId, int sensorId)
        {
            var keyDbName = DBManagerChild.GetDBName(sensorId);
            DateTime? dtS = null;
            DateTime? dtE = null;

            if (pnlFilter.Visible == true)
            {
                dtS = new DateTime(dtpSubDateS.Value.Year,
                                   dtpSubDateS.Value.Month,
                                   dtpSubDateS.Value.Day,
                                   (int)cboSubHourS.SelectedValue,
                                   (int)cboSubMimuteS.SelectedValue,
                                   (int)cboSubSecondS.SelectedValue,
                                   000);
                dtE = new DateTime(dtpSubDateE.Value.Year,
                                   dtpSubDateE.Value.Month,
                                   dtpSubDateE.Value.Day,
                                   (int)cboSubHourE.SelectedValue,
                                   (int)cboSubMimuteE.SelectedValue,
                                   (int)cboSubSecondE.SelectedValue,
                                   999);
            }

            dgvMeasureDetail.DataSource = AppManager.GetDBChildConnection(keyDbName).GetMeasureDetail(measureId.ToString(), chkView.Checked, dtS, dtE);

            if (dgvMeasureDetail.Rows.Count > 0)
            {
                dgvMeasureDetail.FirstDisplayedScrollingRowIndex = dgvMeasureDetail.Rows.Count - 1;
                dgvMeasureDetail.Rows[dgvMeasureDetail.Rows.Count - 1].Selected = true;
            }
        }

        private void BindingCboUser()
        {
            var lstUser = _mUsers.ToList();
            if (lstUser != null)
            {

                lstUser.Insert(0, new MUser { UserId = SELECT_ALL, UserName = SELECT_ALL_SHOW });

                BindingListToComboBox(cmbUser, lstUser, "UserName", "UserId", AppManager.UserLogin.UserId);

                if (cmbUser.Items.Count > 0 && cmbUser.SelectedIndex == -1)
                {
                    cmbUser.SelectedValue = SELECT_ALL;
                }
            }
        }

        private void BindingCboResult()
        {
            var lstResult = new List<CommonStringObject>();

            lstResult.Add(new CommonStringObject() { Key = SELECT_ALL, Value = SELECT_ALL_SHOW });

            lstResult.Add(new CommonStringObject() { Key = (int)emMeasureResult.Pass, Value = clsCommon.MeasureResultDisplay((int)emMeasureResult.Pass) });
            lstResult.Add(new CommonStringObject() { Key = (int)emMeasureResult.Fail, Value = clsCommon.MeasureResultDisplay((int)emMeasureResult.Fail) });

            BindingListToComboBox(cmbResult, lstResult, "Value", "Key", SELECT_ALL);
        }

        private void BindingCboBlock()
        {
            using (var objDB = AppManager.GetConnection())
            {
                var lstBlock = objDB.GetListBlock();
                if (lstBlock != null)
                {
                    lstBlock.Insert(0, new Block { BlockId = SELECT_ALL, BlockName = SELECT_ALL_SHOW });

                    BindingListToComboBox(cmbBlock, lstBlock, "BlockName", "BlockId", SELECT_ALL);
                }
            }
        }

        private void BindingDateTimePicker()
        {
            var now = DateTime.Now;
            dtpStart.Value = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            dtpEnd.Value = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
        }

        private void BindingCboTime(ComboBox cbo, int max)
        {
            var lstBinding = new List<CommonStringObject>();

            for (var i = 0; i < max; i++)
            {
                lstBinding.Add(new CommonStringObject() { Key = i, Value = i.ToString().PadLeft(2, '0') });
            }

            BindingListToComboBox(cbo, lstBinding, "Value", "Key", SELECT_ALL);

        }

        private bool BindingListToComboBox<T>(ComboBox cbo, List<T> list, string strDisplay, string strValue, object selectValue = null) where T : class
        {
            try
            {
                cbo.DisplayMember = strDisplay;
                cbo.ValueMember = strValue;
                cbo.DataSource = list;

                if (selectValue != null && cbo.Items.Count > 0)
                {
                    cbo.SelectedValue = selectValue;
                }

                return true;
            }
            catch
            { }

            return false;
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

        private void DGVDataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (sender == null)
            {
                return;
            }

            ClearSelectionDgv((DataGridView)sender);
        }

        private void ClearSelectionDgv(DataGridView dgv)
        {
            dgv.ClearSelection();
            dgv.CurrentCell = null;
        }

        #endregion Private Function

        private void dgvMeasure_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMeasure.CurrentRow == null || dgvMeasure.CurrentRow.Index < 0)
            {
                return;
            }
            int rowIndex = dgvMeasure.CurrentRow.Index;
            int colIndex = dgvMeasure.CurrentCell.ColumnIndex;

            var measureid = ConvertHelper.CnvNullToInt(dgvMeasure.Rows[rowIndex].Cells[MeasureID.Index].Value);
            var sensorid = ConvertHelper.CnvNullToInt(dgvMeasure.Rows[rowIndex].Cells[SensorID.Index].Value);

            this.SetModeWaiting();

            try
            {
                if (colIndex == dgvMeasure.Columns[colDelete.Index].Index)
                {
                    // Delete
                    if (!clsCommon.ComfirmMsg("MSG_COMFIRM_DELETE"))
                    {
                        ClearSelectionDgv(dgvMeasure);
                        return;
                    }

                    var keyDbName = DBManagerChild.GetDBName(sensorid);
                    var measureOfSensor = AppManager.GetDBChildConnection(keyDbName)
                                                 .DeleteMeasure(measureid);

                    LoadData();
                }
                else
                {
                    // Load filter
                    var dtStartTime = CnvDBToDateTimePickerValue(dgvMeasure.Rows[rowIndex].Cells[StartTime.Index].Value);
                    var dtEndTime = CnvDBToDateTimePickerValue(dgvMeasure.Rows[rowIndex].Cells[EndTime.Index].Value);
                    var intMeasureType = ConvertHelper.CnvNullToInt(dgvMeasure.Rows[rowIndex].Cells[MeasureType.Index].Value);

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

                    LoadDataSub(measureid, sensorid);
                }
            }
            catch (Exception ex)
            {
                ShowMsg(MessageBoxIcon.Error, LanguageHelper.GetValueOf("MSG_ERR_PROCESS"));
            }
            finally
            {
                this.SetModeWaiting(false);
            }
        }
    }
}
