using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DSF602.Model;
using BaseCommon;
using BaseCommon.Utility;
using DSF602.Language;

namespace DSF602.View.GraphLayout
{
    public partial class AlarmReport : UserControl
    {
        private List<Block> lstBlock;
        private List<SensorInfo> lstAllSensor;
        private string SELECT_ALL_SHOW = LanguageHelper.GetValueOf("SELECT_ALL_SHOW");
        private const int SELECT_ALL = -1;
        private bool IsLoading = false;
        private int blockidActive = -1;
        private MainForm frmMain = null;

        public AlarmReport()
        {
            InitializeComponent();
            Init();
            AppManager.OnLanguageChanged += new EventHandler((s, e) => { SetLanguageControl(); });
        }

        private void Init()
        {
            foreach(Form frm in Application.OpenForms)
            {
                if (frm.Name == nameof(MainForm))
                {
                    frmMain = frm as MainForm;
                    break;
                }
            }
            
            lstAllSensor = AppManager.ListSensor;
            lstBlock = AppManager.ListBlock.Select(i => { return i; }).ToList();
            lstBlock.Insert(0, new Block { BlockId = SELECT_ALL, BlockName = SELECT_ALL_SHOW });
            cmbBlock.DataSource = lstBlock;
            cmbBlock.SelectedValue = blockidActive;
        }

        private void SetLanguageControl()
        {
            // Grid master
            LanguageHelper.SetValueOf(colSensorName, "MEASURE_COL_SENSORNAME");
            LanguageHelper.SetValueOf(colBlockName, "MEASURE_COL_BLOCKNAME");
            LanguageHelper.SetValueOf(colAlarmValue, "MEASURE_COL_ALARMVALUE");
            LanguageHelper.SetValueOf(colStartTime, "MEASURE_COL_STARTTIME");
            LanguageHelper.SetValueOf(colEndTime, "MEASURE_COL_ENDTIME");
            LanguageHelper.SetValueOf(colResult, "MEASURE_COL_RESULT");

            // Grid detail
            LanguageHelper.SetValueOf(colDtlTime, "MEASURE_COL_TIMEDETAIL");
            LanguageHelper.SetValueOf(colDtlActualValue, "MEASURE_COL_VALUEDETAIL");

            LanguageHelper.SetValueOf(lblBlock, "MEASURE_LBL_BLOCK");
            LanguageHelper.SetValueOf(lblSensor, "MEASURE_LBL_SENSOR");
            LanguageHelper.SetValueOf(btnSearch, "MEASURE_BTN_SEARCH");

            SELECT_ALL_SHOW = LanguageHelper.GetValueOf("SELECT_ALL_SHOW");

            var lstBl = cmbBlock.DataSource as List<Block>;
            lstBl = lstBl.Select(i =>
            {
                if (i.BlockId == -1)
                {
                    i.BlockName = SELECT_ALL_SHOW;
                }
                return i;
            }).ToList();
            var lstSen = cmbSensor.DataSource as List<SensorInfo>;
            lstSen = lstSen.Select(i =>
            {
                if (i.SensorId == -1)
                {
                    i.SensorName = SELECT_ALL_SHOW;
                }
                return i;
            }).ToList();

            var currentBlock = int.Parse("" + cmbBlock.SelectedValue);
            var currentSensor = int.Parse("" + cmbSensor.SelectedValue);

            cmbBlock.DataSource = lstBl;
            cmbBlock.SelectedValue = currentBlock;

            cmbSensor.DataSource = lstSen;
            cmbSensor.SelectedValue = currentSensor;

            var lstMeasure = dgvMeasure.DataSource as List<DGVMeasureInfo>;
            if (lstMeasure != null)
            {
                lstMeasure = lstMeasure.Select(i =>
                {
                    i.MeasureDisplay = clsCommon.MeasureResultDisplay(1);
                    return i;
                }).ToList();
            }
        }

        public void GetData(int currentBlock = -1)
        {
            try
            {
                if (frmMain != null && blockidActive == -1)
                {
                    blockidActive = frmMain.BlockActiceId;
                }

                cmbBlock.SelectedValue = currentBlock > -1 ? currentBlock : int.Parse("" + cmbBlock.SelectedValue);


                IsLoading = true;
                Cursor = Cursors.WaitCursor;

                dgvMeasure.DataSource = null;
                dgvMeasureDetail.DataSource = null;
                DateTime dtNow = DateTime.Now;
                var dtStart = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, 0, 0, 0);
                var dtEnd = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, 23, 59, 59);

                var strSDate = dtStart.ToString(clsConst.cstrDateTimeFormatNoMiliSecond);
                var strEDate = dtEnd.ToString(clsConst.cstrDateTimeFormatNoMiliSecond);
                List<MUser> _mUsers = new List<MUser>();
                using (var objDB = AppManager.GetConnection())
                {
                    _mUsers = objDB.GetMUser();
                }
                int blockSelected = int.Parse("" + cmbBlock.SelectedValue);
                int sensorSelected = int.Parse("" + cmbSensor.SelectedValue);
                var lstSen = new List<SensorInfo>();

                List<Measure> lstMeasure = new List<Measure>();
                if (cmbBlock.SelectedValue == null || int.Parse("" + cmbBlock.SelectedValue) == -1)
                {
                    lstSen = lstAllSensor;
                }
                else
                {
                    if (cmbSensor.SelectedValue == null || int.Parse("" + cmbSensor.SelectedValue) == -1)
                    {
                        lstSen = cmbSensor.DataSource as List<SensorInfo>;
                    }
                    else
                    {
                        lstSen.Add(cmbSensor.SelectedItem as SensorInfo);
                    }
                }

                lstSen = lstSen.Where(i => i.SensorId != -1).ToList();
                foreach (SensorInfo sensor in lstSen)
                {
                    var keyDbName = DBManagerChild.GetDBName(sensor.SensorId);
                    lstMeasure.AddRange(AppManager.GetDBChildConnection(keyDbName).GetAllMeasure(strSDate, strEDate, AppManager.UserLogin.UserId, 1));
                }
                var cnt = 1;
                var lstFail = (from max in (lstMeasure.GroupBy(g => new { g.SensorId, g.Measure_Result })
                                .Select(grp => new
                                {
                                    grp.Key.SensorId,
                                    grp.Key.Measure_Result,
                                    MeasureId = grp.Max(i => i.MeasureId)
                                })).ToList()
                               join sensor in lstSen on max.SensorId equals sensor.SensorId
                               join bl in lstBlock on sensor.OfBlock equals bl.BlockId
                               where (blockSelected == -1 || bl.BlockId == blockSelected)
                               && (sensorSelected == -1 || sensor.SensorId == sensorSelected)
                               select new DGVMeasureInfo
                               {
                                   No = cnt++,
                                   MeasureId = max.MeasureId,
                                   BlockName = bl.BlockName,
                                   SensorId = max.SensorId,
                                   SensorName = sensor.SensorName,
                                   Alarm_Value = lstMeasure.FirstOrDefault(i => i.MeasureId == max.MeasureId).Alarm_Value,
                                   Measure_Result = max.Measure_Result,
                                   MeasureDisplay = clsCommon.MeasureResultDisplay(1),
                                   Start_time = lstMeasure.FirstOrDefault(i => i.MeasureId == max.MeasureId).Start_time,
                                   End_time = lstMeasure.FirstOrDefault(i => i.MeasureId == max.MeasureId).End_time
                               }).ToList();
                dgvMeasure.DataSource = lstFail;
            }
            finally
            {
                Cursor = Cursors.Default;
                IsLoading = false;
                if (dgvMeasure.Rows.Count > 0)
                {
                    dgvMeasure.Select();
                    dgvMeasure.FirstDisplayedScrollingRowIndex = 0;
                    dgvMeasure.Rows[0].Selected = true;
                }
            }
        }

        private void dgvMeasure_SelectionChanged(object sender, EventArgs e)
        {
            if (IsLoading) return;
            if (dgvMeasure.CurrentRow == null || dgvMeasure.CurrentRow.Index < 0) return;

            dgvMeasureDetail.DataSource = null;
            this.Cursor = Cursors.WaitCursor;
            DGVMeasureInfo item = dgvMeasure.CurrentRow.DataBoundItem as DGVMeasureInfo;
            var keyDbName = DBManagerChild.GetDBName(item.SensorId);
            var lst = AppManager.GetDBChildConnection(keyDbName).GetMeasureDetail(item.MeasureId.ToString(), true);

            int cnt = 1;
            lst = lst.Select(i =>
            {
                i.No = cnt++;
                return i;
            }).ToList();
            dgvMeasureDetail.DataSource = lst;
            //if (dgvMeasureDetail.Rows.Count > 0)
            //{
            //    dgvMeasureDetail.Select();
            //    dgvMeasureDetail.FirstDisplayedScrollingRowIndex = 0;
            //    dgvMeasureDetail.Rows[0].Selected = true;
            //}
            this.Cursor = Cursors.Default;
        }

        private void cmbBlock_SelectedIndexChanged(object sender, EventArgs e)
        {
            int blockId = int.Parse("" + cmbBlock.SelectedValue);
            var lstSen = lstAllSensor.Where(i => i.OfBlock == blockId).ToList();
            lstSen.Insert(0, new SensorInfo { SensorId = SELECT_ALL, SensorName = SELECT_ALL_SHOW });
            cmbSensor.DataSource = lstSen;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }
    }
}
