using BaseCommon;
using BaseCommon.Core;
using BaseCommon.Utility;
using DSF602.Language;
using DSF602.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DSF602.View
{
    public partial class DefaultSettingPopup : BaseForm
    {
        SettingParam currentSetting;
        Block currentBlock = null;
        string title;
        public DefaultSettingPopup()
        {
            InitializeComponent();
            title = this.Text;
        }

        protected override void OnLoad(EventArgs e)
        {
            var blockid = (int)this.Params;
            currentBlock = AppManager.ListBlock.FirstOrDefault(b => b.BlockId == blockid);
            SettingParam prDefault = null;
            if (!string.IsNullOrEmpty(currentBlock.DefaultParams))
            {
                prDefault = Newtonsoft.Json.JsonConvert.DeserializeObject<SettingParam>(currentBlock.DefaultParams);
            }

            SetDefaultParam(prDefault);
            //SetLanguageControl();
            base.OnLoad(e);
        }

        private void SetDefaultParam(SettingParam pr = null)
        {
            if (pr == null)
            {
                pr = AppManager.GetDefaultSetting();
            }
            currentSetting = pr;
            txtVoltAlarm.Text = "" + pr.VoltAlarmValue;
            txtIonAlarm.Text = "" + pr.IonAlarmValue;

            txtUpVal.Text = "" + pr.UpVal;
            txtLowVal.Text = "" + pr.LowVal;
            txtDecayTime.Text = "" + pr.DecayTimeCheck;
            txtDecayStopTime.Text = "" + pr.StopDecayTime;
            txtIonCheck.Text = "" + pr.IonBalanceCheck;
            txtIonStopTimeCheck.Text = "" + pr.IonStopTimeCheck;
            chkAutoTime.Checked = pr.IsAuto;

            if (!string.IsNullOrEmpty(pr.AutoCheckTimes))
            {
                var times = pr.AutoCheckTimes.Split(',').OrderBy(i => i);

                dg.Rows.Clear();
                foreach (string tm in times)
                {
                    dg.Rows.Add();
                    dg[colTime.Index, dg.Rows.Count - 1].Value = tm;
                    dg[colNo.Index, dg.Rows.Count - 1].Value = dg.Rows.Count;
                }
            }

            if (!string.IsNullOrEmpty(pr.AutoCheckDays))
            {
                var days = pr.AutoCheckDays.Split(',');
                List<DayOfWeek> lstDays = new List<DayOfWeek>();
                foreach (string item in days)
                {
                    DayOfWeek d = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), item);
                    lstDays.Add(d);
                }
                chkMon.Checked = lstDays.Contains(DayOfWeek.Monday);
                chkTue.Checked = lstDays.Contains(DayOfWeek.Tuesday);
                chkWed.Checked = lstDays.Contains(DayOfWeek.Wednesday);
                chkThu.Checked = lstDays.Contains(DayOfWeek.Thursday);
                chkFri.Checked = lstDays.Contains(DayOfWeek.Friday);
                chkSat.Checked = lstDays.Contains(DayOfWeek.Saturday);
                chkSun.Checked = lstDays.Contains(DayOfWeek.Sunday);
            }

            txtVoltAlarm.Focus();
        }

        protected override void SetLanguageControl()
        {
            base.SetLanguageControl();
            string titleIb = LanguageHelper.GetValueOf("TITLE_IB");
            string titleDecay = LanguageHelper.GetValueOf("TITLE_DECAY");

            title = LanguageHelper.GetValueOf("TITLE_DEFAULT_VALUES");
            this.Text = title + string.Format("({0})", currentBlock.BlockName);
            chkVoltGrp.Text = LanguageHelper.GetValueOf("TITLE_VOLT").ToUpper();
            LanguageHelper.SetValueOf(lbAlarmVolt, "DEVICE_LBL_ALARMVALUE");
            chkIonGrp.Text = titleIb.ToUpper();
            LanguageHelper.SetValueOf(lbAlarmIon, "DEVICE_LBL_ALARMVALUE");
            chkDecayGrp.Text = string.Format("{0} && {1}", titleDecay, titleIb).ToUpper();
            lbDecayParams.Text = titleDecay;
            lbIBParams.Text = titleIb;
            LanguageHelper.SetValueOf(lbUpVal, "TITLE_DECAY_UPVAL");
            LanguageHelper.SetValueOf(lbLowVal, "TITLE_DECAY_LOWVAL");
            LanguageHelper.SetValueOf(lbDecayTime, "TITLE_DECAY_TIME");
            LanguageHelper.SetValueOf(lbDecayStopTime, "TITLE_DECAY_STOPTIME");
            LanguageHelper.SetValueOf(lbIonBalance, "TITLE_DECAY_IBVAL");
            LanguageHelper.SetValueOf(lbIonStopTime, "TITLE_DECAY_IBSTOPTIME");
            LanguageHelper.SetValueOf(chkAutoTime, "TITLE_DECAY_AUTO");
            LanguageHelper.SetValueOf(btResetDefault, "DEVICE_BTN_RESETDEFAULT");
            LanguageHelper.SetValueOf(btSaveSetting, "DEVICE_BTN_SAVESETTING");
            LanguageHelper.SetValueOf(btnCancel, "LANGUAGE_BTN_CANCEL");

            LanguageHelper.SetValueOf(colNo, "TITLE_DECAY_COL_COUNT");
            LanguageHelper.SetValueOf(colTime, "MEASURE_COL_TIMEDETAIL");

            LanguageHelper.SetValueOf(chkMon, "DAY_MON");
            LanguageHelper.SetValueOf(chkTue, "DAY_TUE");
            LanguageHelper.SetValueOf(chkWed, "DAY_WED");
            LanguageHelper.SetValueOf(chkThu, "DAY_THU");
            LanguageHelper.SetValueOf(chkFri, "DAY_FRI");
            LanguageHelper.SetValueOf(chkSat, "DAY_SAT");
            LanguageHelper.SetValueOf(chkSun, "DAY_SUN");
        }

        private bool ValidateData()
        {
            if (chkVoltGrp.Checked && ConvertHelper.CnvNullToInt(txtVoltAlarm.Text) <= 0)
            {
                txtVoltAlarm.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("FOMAT_MSG_INPUT_NUMBER"), chkVoltGrp.Text));
            }
            else if (chkIonGrp.Checked && ConvertHelper.CnvNullToInt(txtIonAlarm.Text) <= 0)
            {
                txtIonAlarm.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("FOMAT_MSG_INPUT_NUMBER"), chkIonGrp.Text));
            }

            // Charging value
            if (chkDecayGrp.Checked)
            {
                // Upper value
                var upVal = ConvertHelper.CnvNullToInt(txtUpVal.Text);
                if (upVal < 10 || upVal > 2000)
                {
                    txtUpVal.Focus();
                    return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("MSG_OUTOF_RANGE"), lbUpVal.Text, 10, 2000));
                }
                // Lower value
                var lowVal = ConvertHelper.CnvNullToInt(txtLowVal.Text);
                if (lowVal < 0 || lowVal > 1995)
                {
                    txtLowVal.Focus();
                    return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("MSG_OUTOF_RANGE"), lbLowVal.Text, 0, 1995));
                }
                // Decay time check
                var decayTime = ConvertHelper.CnvNullToInt(txtDecayTime.Text);
                if (decayTime < 0 || decayTime > 10)
                {
                    txtDecayTime.Focus();
                    return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("MSG_OUTOF_RANGE"), lbDecayTime.Text, 0, 10));
                }
                // Stop time decay check
                var decayStopTime = ConvertHelper.CnvNullToInt(txtDecayStopTime.Text);
                if (decayStopTime < 10 || decayStopTime > 100)
                {
                    txtDecayStopTime.Focus();
                    return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("MSG_OUTOF_RANGE"), lbDecayStopTime.Text, 10, 100));
                }
                // Ion value check
                if (ConvertHelper.CnvNullToInt(txtIonCheck.Text) <= 0)
                {
                    txtIonCheck.Focus();
                    return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("FOMAT_MSG_INPUT_NUMBER"), lbIonBalance.Text));
                }
                // Ion time check
                if (ConvertHelper.CnvNullToInt(txtIonStopTimeCheck.Text) <= 0)
                {
                    txtIonStopTimeCheck.Focus();
                    return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("FOMAT_MSG_INPUT_NUMBER"), lbIonStopTime.Text));
                }
            }

            return true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!ValidateData()) return;
            using (var objDB = AppManager.GetConnection())
            {
                int cnt = 0;
                var lstSensor = objDB.GetListSensor().Where(i => i.OfBlock == currentBlock.BlockId).ToList();

                
                List<string> arrTimes = new List<string>();
                foreach (DataGridViewRow row in dg.Rows)
                {
                    if (ValidateInputTime("" + row.Cells[colTime.Index].Value, false))
                    {
                        arrTimes.Add("" + row.Cells[colTime.Index].Value);
                    }
                    else
                    {
                        MessageBox.Show(LanguageHelper.GetValueOf("TIME_INCORRECT_FORMAT"), LanguageHelper.GetValueOf("MSG_SETTING"), MessageBoxButtons.OK);
                        row.Selected = true;
                        dg.CurrentCell = row.Cells[colTime.Index];
                        dg.BeginEdit(true);
                        return;
                    }
                }

                List<string> arrDays = new List<string>();
                if (chkMon.Checked)
                {
                    arrDays.Add(((int)DayOfWeek.Monday).ToString());
                }
                if (chkTue.Checked)
                {
                    arrDays.Add(((int)DayOfWeek.Tuesday).ToString());
                }
                if (chkWed.Checked)
                {
                    arrDays.Add(((int)DayOfWeek.Wednesday).ToString());
                }
                if (chkThu.Checked)
                {
                    arrDays.Add(((int)DayOfWeek.Thursday).ToString());
                }
                if (chkFri.Checked)
                {
                    arrDays.Add(((int)DayOfWeek.Friday).ToString());
                }
                if (chkSat.Checked)
                {
                    arrDays.Add(((int)DayOfWeek.Saturday).ToString());
                }
                if (chkSun.Checked)
                {
                    arrDays.Add(((int)DayOfWeek.Sunday).ToString());
                }

                // Update block setting
                if (chkVoltGrp.Checked)
                {
                    currentSetting.VoltAlarmValue = int.Parse(txtVoltAlarm.Text);
                }
                if (chkIonGrp.Checked)
                {
                    currentSetting.IonAlarmValue = int.Parse(txtIonAlarm.Text);
                }
                if (chkDecayGrp.Checked)
                {
                    currentSetting.UpVal = int.Parse(txtUpVal.Text);
                    currentSetting.LowVal = int.Parse(txtLowVal.Text);
                    currentSetting.DecayTimeCheck = int.Parse(txtDecayTime.Text);
                    currentSetting.StopDecayTime = int.Parse(txtDecayStopTime.Text);
                    currentSetting.IonBalanceCheck = int.Parse(txtIonCheck.Text);
                    currentSetting.IonStopTimeCheck = int.Parse(txtIonStopTimeCheck.Text);
                    currentSetting.AutoCheckTimes = string.Join(",", arrTimes);
                    currentSetting.AutoCheckDays = string.Join(",", arrDays);
                    currentSetting.IsAuto = chkAutoTime.Checked;
                }                
                currentBlock.DefaultParams = Newtonsoft.Json.JsonConvert.SerializeObject(currentSetting);
                objDB.UpdateBlockDefaultParam(currentBlock);

                // Update sensor setting
                foreach (SensorInfo sensor in lstSensor)
                {
                    if (chkVoltGrp.Checked && sensor.MeasureType == clsConst.MeasureMode_Volt)
                    {
                        sensor.Alarm_Value = int.Parse("" + txtVoltAlarm.Text);
                    }
                    
                    if (chkIonGrp.Checked && sensor.MeasureType == clsConst.MeasureMode_Ion)
                    {
                        sensor.Alarm_Value = int.Parse("" + txtIonAlarm.Text);
                    }

                    if (chkDecayGrp.Checked && sensor.MeasureType == clsConst.MeasureMode_Decay)
                    {
                        sensor.DecayUpperValue = int.Parse(txtUpVal.Text);
                        sensor.DecayLowerValue = int.Parse(txtLowVal.Text);
                        sensor.DecayTimeCheck = int.Parse(txtDecayTime.Text);
                        sensor.DecayStopTime = int.Parse(txtDecayStopTime.Text);
                        sensor.IonValueCheck = int.Parse(txtIonCheck.Text);
                        sensor.IonTimeCheck = int.Parse(txtIonStopTimeCheck.Text);

                        if (chkAutoTime.Checked)
                        {
                            if (dg.Rows.Count == 0)
                            {
                                sensor.AutoCheckFlag = 0;
                                sensor.AutoCheckTime = string.Empty;
                                sensor.AutoCheckDays = string.Empty;
                            }
                            else
                            {
                                sensor.AutoCheckFlag = 1;
                                sensor.AutoCheckTime = string.Join(",", arrTimes);
                                sensor.AutoCheckDays = string.Join(",", arrDays);
                            }
                        }
                        else
                        {
                            sensor.AutoCheckFlag = 0;
                        }
                    }

                    if (objDB.UpdateSensor(sensor, sensor.Ordinal_Display, sensor.Ordinal_Display, false))
                    {
                        cnt++;
                    }
                }
            }

            this.ResultData = true;
            
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.ResultData = null;
            this.Close();
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            SetDefaultParam();
        }

        private void Save()
        {

        }

        
        private void DefaultSettingPopup_Load(object sender, EventArgs e)
        {

        }

        private void dg_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dg_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            
            
        }

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            dg.Rows.Add();
            dg.Rows[dg.Rows.Count - 1].Cells[colNo.Index].Value = dg.Rows.Count;
            dg.Rows[dg.Rows.Count - 1].Selected = true;
            dg.CurrentCell = dg.Rows[dg.Rows.Count - 1].Cells[colTime.Index];
            dg.BeginEdit(true);
        }

        private void btnRemoveRow_Click(object sender, EventArgs e)
        {
            dg.Rows.RemoveAt(dg.CurrentRow.Index);
            for(int i = 0; i < dg.Rows.Count; i++)
            {
                dg.Rows[i].Cells[colNo.Index].Value = i + 1;
            }
        }

        private void dg_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == colTime.Index)
            {
                var preValue = "" + e.FormattedValue;
                if (!ValidateInputTime(preValue))
                {
                    e.Cancel = true;
                    MessageBox.Show(LanguageHelper.GetValueOf("TIME_INCORRECT_FORMAT"), LanguageHelper.GetValueOf("MSG_SETTING"), MessageBoxButtons.OK);
                }
            }
        }

        private bool ValidateInputTime(string preValue, bool allowNull = true)
        {
            if (!allowNull && string.IsNullOrEmpty(preValue)) return false;

            var arr = preValue.Split(':');
            if (arr.Length < 2)
            {
                return false;
            }

            if (!allowNull && (arr[0].Trim().Length < 2 || arr[1].Trim().Length < 2))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(arr[0].Trim()))
            {
                var hour = int.Parse(arr[0]);
                if (hour >= 24)
                {
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(arr[1].Trim()))
            {
                var min = int.Parse(arr[1]);
                if (min >= 60)
                {
                    return false;
                }
            }

            return true;
        }

        private void titleIon_CheckedChanged(object sender, EventArgs e)
        {
            pnIB.Enabled = chkIonGrp.Checked;
            chkIonGrp.Enabled = true;
        }

        private void titleVolt_CheckedChanged(object sender, EventArgs e)
        {
            pnVolt.Enabled = chkVoltGrp.Checked;
            chkVoltGrp.Enabled = true;
        }

        private void titleDecay_CheckedChanged(object sender, EventArgs e)
        {
            pnDecay.Enabled = chkDecayGrp.Checked;
            chkDecayGrp.Enabled = true;
        }

        private void chkAuto_CheckedChanged(object sender, EventArgs e)
        {
            pnAuto.Enabled = chkAutoTime.Checked;
            
        }

        private void pnAuto_EnabledChanged(object sender, EventArgs e)
        {
            //btnAddRow.Visible = btnRemoveRow.Visible = pnAuto.Enabled;
            if (pnAuto.Enabled)
            {
                colTime.DefaultCellStyle.BackColor = Color.White;
            }
            else
            {
                colTime.DefaultCellStyle.BackColor = colNo.DefaultCellStyle.BackColor;
            }
        }
    }
}
