using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DSF602.Model;
using DSF602;
using DSF602.Language;
using BaseCommon.Utility;
using DSF602.View;
using DSF602.View.ControlLayout;

namespace BaseCommon.ControlTemplate
{
    public partial class DeviceLayout : Core.BaseUserControl
    {
        private int _blockId;

        public Block BlockChange { get; set; }
        public List<SensorInfo> ListSensorChange { get; set; }

        public event EventHandler<Block> EventSavedBlock;

        public DeviceLayout(int BlockId)
        {
            InitializeComponent();
            InitForm(BlockId);
        }

        public void SelectSensor(int sensorId)
        {
            var sensor = AppManager.ListSensor.FirstOrDefault(i => i.SensorId == sensorId);

            if (sensor != null)
            {
                dgvDevice_CellClick(dgvDevice, new DataGridViewCellEventArgs(colDeviceName.Index, sensor.Ordinal_Display - 1));
            }
        }

        #region Event Form

        protected void onlyInputNumber(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtIpAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.Split('.').Length - 1 >= 3))
            {
                e.Handled = true;
            }
        }

        private void dgvDevice_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            if (e.ColumnIndex == colActive.Index)
            {
                int intActive = ConvertHelper.CnvNullToInt(e.Value);
                e.Value = intActive == clsConst.ACTIVE ? LanguageHelper.GetValueOf("DEVICE_CBO_ACTIVE") : LanguageHelper.GetValueOf("DEVICE_CBO_DEACTIVE");
            }
        }

        private void btnSaveBlock_Click(object sender, EventArgs e)
        {
            if (!validateBlock())
            {
                return;
            }

            var bockData = new Block()
            {
                BlockId = _blockId,
                BlockName = txtBlockName.Text.Trim(),
                Active = ckbActiveBlock.Checked ? clsConst.ACTIVE : clsConst.NOT_ACTIVE,
                Ip_Address = txtIp.Text,
                Port = ConvertHelper.CnvNullToInt(txtPortBlock.Text),
            };

            this.SetModeWaiting();

            try
            {
                using (var objDB = AppManager.GetConnection())
                {
                    if (objDB.UpdateBlock(bockData))
                    {
                        ShowMsg(MessageBoxIcon.Information, LanguageHelper.GetValueOf("MSG_SAVE_SETTING_SUCCESS"));
                        //AppManager.ListBlock = objDB.GetListBlock();
                        this.BlockChange = bockData;
                        //BindingBlock(_blockId);

                        if (EventSavedBlock != null)
                        {
                            EventSavedBlock.Invoke(this, bockData);
                        }

                        //AppManager.OnBlockSettingChanged?.Invoke(bockData, null);
                        return;
                    }

                    ShowMsg(MessageBoxIcon.Error, LanguageHelper.GetValueOf("MSG_SAVE_SETTING_ERR"));
                }
            }
            catch (Exception ex)
            {
                ShowMsg(MessageBoxIcon.Error, ex.Message);
            }
            finally
            {
                this.SetModeWaiting(false);
            }
        }

        private void btSaveSetting_Click(object sender, EventArgs e)
        {
            SaveSensor();
        }

        private void SaveSensor(SettingParam pr = null)
        {
            var sensor = dgvDevice.GetSelectedData<SensorInfo>();
            if (sensor == null)
            {
                return;
            }

            if (pr == null)
            {
                // User data
                sensor.SensorName = txtSensorName.Text.Trim();
                sensor.Active = ckbActiveSensor.Checked ? clsConst.ACTIVE : clsConst.NOT_ACTIVE;
                sensor.MeasureType = int.Parse("" + cboMeasureType.SelectedValue);
                if (sensor.MeasureType == clsConst.MeasureMode_Volt || sensor.MeasureType == clsConst.MeasureMode_Ion)
                {
                    sensor.Alarm_Value = ConvertHelper.CnvNullToInt(txtAlaimValue.Text.Trim());
                }
                if (sensor.MeasureType == clsConst.MeasureMode_Decay)
                {
                    sensor.DecayUpperValue = ConvertHelper.CnvNullToInt(txtUpVal.Text);
                    sensor.DecayLowerValue = ConvertHelper.CnvNullToInt(txtLowVal.Text);
                    sensor.DecayTimeCheck = ConvertHelper.CnvNullToInt(txtDecayTime.Text);
                    sensor.DecayStopTime = ConvertHelper.CnvNullToInt(txtDecayStopTime.Text);
                    sensor.IonValueCheck = ConvertHelper.CnvNullToInt(txtIonCheck.Text);
                    sensor.IonTimeCheck = ConvertHelper.CnvNullToInt(txtIonStopTimeCheck.Text);
                }
            }
            else
            {
                // Default setting data
                sensor.SensorName = txtSensorName.Text.Trim();
                sensor.Active = clsConst.ACTIVE;
                sensor.MeasureType = int.Parse("" + cboMeasureType.SelectedValue);

                if (sensor.MeasureType == clsConst.MeasureMode_Volt)
                {
                    sensor.Alarm_Value = pr.VoltAlarmValue;
                }
                else if (sensor.MeasureType == clsConst.MeasureMode_Ion)
                {
                    sensor.Alarm_Value = pr.IonAlarmValue;
                }
                
                if (sensor.MeasureType == clsConst.MeasureMode_Decay)
                {
                    sensor.AutoCheckFlag = pr.IsAuto ? 1 : 0;
                    sensor.DecayUpperValue = pr.UpVal;
                    sensor.DecayLowerValue = pr.LowVal;
                    sensor.DecayTimeCheck = pr.DecayTimeCheck;
                    sensor.DecayStopTime = pr.StopDecayTime;
                    sensor.IonValueCheck = pr.IonBalanceCheck;
                    sensor.IonTimeCheck = pr.IonStopTimeCheck;
                }
            }

            if (!validateSensor(sensor))
            {
                return;
            }

            var stOrdinalDisplay = sensor.Ordinal_Display;
            var enOrdinalDisplay = (int)cboOrdinalDisplay.SelectedValue;
            var flag = true;

            this.SetModeWaiting();
            try
            {
                flag = stOrdinalDisplay > enOrdinalDisplay ? true : false;
                using (var objDB = AppManager.GetConnection())
                {
                    if (objDB.UpdateSensor(sensor, stOrdinalDisplay, enOrdinalDisplay, flag))
                    {
                        ShowMsg(MessageBoxIcon.Information, LanguageHelper.GetValueOf("MSG_SAVE_SETTING_SUCCESS"));

                        this.ListSensorChange.Add(sensor);
                        lblSensorSetting.Text = (txtSensorName.Text.Trim() + " " + LanguageHelper.GetValueOf("MSG_SETTING")).Trim();
                        var lst = objDB.GetListSensor().Where(i => i.OfBlock == sensor.OfBlock).OrderBy(i => i.Ordinal_Display).ToList();
                        BindingDataList(lst);
                        this.dgvDevice_CellClick(dgvDevice, new DataGridViewCellEventArgs(colDeviceName.Index, enOrdinalDisplay - 1));
                        //AppManager.OnSensorSettingChanged?.Invoke(sensor, null);
                    }
                    else
                    {
                        var lst = objDB.GetListSensor().Where(i => i.OfBlock == sensor.OfBlock).OrderBy(i => i.Ordinal_Display).ToList();
                        BindingDataList(lst);
                        this.dgvDevice_CellClick(dgvDevice, new DataGridViewCellEventArgs(colDeviceName.Index, stOrdinalDisplay - 1));
                        ShowMsg(MessageBoxIcon.Error, LanguageHelper.GetValueOf("MSG_SAVE_SETTING_ERR"));
                    }
                }

            }
            catch (Exception ex)
            {
            }
            finally
            {
                this.SetModeWaiting(false);
            }
        }

        private void dgvDevice_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            dgvDevice.Rows[e.RowIndex].Selected = true;
            bindingSensor();
        }

        private void dgvDevice_SelectionChanged(object sender, EventArgs e)
        {
            bindingSensor();
        }

        private void btResetDefault_Click(object sender, EventArgs e)
        {
            if (clsCommon.ComfirmMsg("MSG_COMFIRM_RESET"))
            {
                var pr = AppManager.GetDefaultSetting();
                SaveSensor(pr);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (dgvDevice.CurrentCell != null)
            {
                bindingSensor();
            }
        }

        #endregion Event Form

        #region Override Function

        protected override void SetLanguageControl()
        {
            LanguageHelper.SetValueOf(lblBlockName, "DEVICE_LBL_BLOCKNAME");
            LanguageHelper.SetValueOf(grpBlockSetting, "DEVICE_GRP_BLOCKSETTING");
            LanguageHelper.SetValueOf(lblActiveBlock, "DEVICE_LBL_ACTIVE");
            LanguageHelper.SetValueOf(lbl_IpAddress, "DEVICE_LBL_IPADDRESS");
            LanguageHelper.SetValueOf(lblPort, "DEVICE_LBL_PORT");
            LanguageHelper.SetValueOf(btnSaveBlock, "DEVICE_BTN_SAVE");
            LanguageHelper.SetValueOf(lblHeaderText, "DEVICE_LBL_HEADERTEXT");
            LanguageHelper.SetValueOf(lblSensorSetting, "DEVICE_LBL_SENSORSETTING");
            LanguageHelper.SetValueOf(lblSensorName, "DEVICE_LBL_SENSORNAME");
            LanguageHelper.SetValueOf(lblOrdinalDisplay, "DEVICE_LBL_ORDINALDISPLAY");
            LanguageHelper.SetValueOf(lblActiveSensor, "DEVICE_LBL_ACTIVESENSOR");
            LanguageHelper.SetValueOf(lblAlaimValue, "DEVICE_LBL_ALARMVALUE");
            LanguageHelper.SetValueOf(btSaveSetting, "DEVICE_BTN_SAVESETTING");
            LanguageHelper.SetValueOf(btnClear, "DEVICE_BTN_CLEAR");
            LanguageHelper.SetValueOf(btResetDefault, "DEVICE_BTN_RESETDEFAULT");
            LanguageHelper.SetValueOf(colDeviceName, "DEVICE_COL_SENSORNAME");
            LanguageHelper.SetValueOf(colActive, "DEVICE_COL_ACTIVE");
            LanguageHelper.SetValueOf(colAlarmValue, "DEVICE_COL_ALARMVALUE");
            LanguageHelper.SetValueOf(lbMeasureTitle, "MEASURE_TYPE");
            LanguageHelper.SetValueOf(btnSetAllAlarm, "TITLE_DEFAULT_VALUES");

            LanguageHelper.SetValueOf(lbUpVal, "TITLE_DECAY_UPVAL");
            LanguageHelper.SetValueOf(lbLowVal, "TITLE_DECAY_LOWVAL");
            LanguageHelper.SetValueOf(lbDecayTime, "TITLE_DECAY_TIME");
            LanguageHelper.SetValueOf(lbDecayStopTime, "TITLE_DECAY_STOPTIME");
            LanguageHelper.SetValueOf(lbIonBalance, "TITLE_DECAY_IBVAL");
            LanguageHelper.SetValueOf(lbIonStopTime, "TITLE_DECAY_IBSTOPTIME");

            LanguageHelper.SetValueOf(colMeasureTypeShow, "MEASURE_TYPE");
            LanguageHelper.SetValueOf(colDecayTimeCheck, "TITLE_DECAY_TIME");
        }

        #endregion Override Function

        #region Private Function

        private void InitForm(int BlockId)
        {
            this._blockId = BlockId;
            this.ListSensorChange = new List<SensorInfo>();
            pnValueMode.Location = new System.Drawing.Point(4, 127);
            pnValueMode.Width = grpDecay.Width;
            txtPortBlock.KeyPress += onlyInputNumber;
            txtAlaimValue.KeyPress += onlyInputNumber;

            // KhoiPD: display measure type
            var dataBinding = new List<DataBinding<int>>();
            dataBinding.Add(new DataBinding<int>()
            {
                Display = LanguageHelper.GetValueOf("MEASURE_MODE_VOLT"),
                Value = clsConst.MeasureMode_Volt,
            });
            dataBinding.Add(new DataBinding<int>()
            {
                Display = LanguageHelper.GetValueOf("MEASURE_MODE_ION"),
                Value = clsConst.MeasureMode_Ion,
            });
            dataBinding.Add(new DataBinding<int>()
            {
                Display = LanguageHelper.GetValueOf("MEASURE_MODE_DECAY"),
                Value = clsConst.MeasureMode_Decay,
            });
            BindingHelper.Combobox(cboMeasureType, dataBinding);
            cboMeasureType.SelectedIndexChanged += CboMeasureType_SelectedIndexChanged;
            BindingCboOrdinalDisplay();
            BindingBlock(BlockId);
        }

        private void CboMeasureType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var showDecay = int.Parse("" + cboMeasureType.SelectedValue) == clsConst.MeasureMode_Decay;
            grpDecay.Visible = showDecay;
            grpIon.Visible = showDecay;
            pnValueMode.Visible = !showDecay;
        }

        private void BtnSetAllAlarm_Click(object sender, EventArgs e)
        {
            MainForm frmMain = null;
            foreach(Form f in Application.OpenForms)
            {
                if (f.Name == nameof(MainForm))
                {
                    frmMain = f as MainForm;
                    break;
                }
            }

            if (frmMain != null)
            {
                var dlg = AppManager.ShowDialog<DefaultSettingPopup>(_blockId);
                if (dlg == null) return;

                using (var objDB = AppManager.GetConnection())
                {
                    var lst = objDB.GetListSensor().Where(i => i.OfBlock == _blockId).OrderBy(i => i.Ordinal_Display).ToList();
                    BindingDataList(lst);
                }
            }
        }

        private void BindingBlock(int BlockId)
        {
            var block = AppManager.ListBlock.FirstOrDefault(i => i.BlockId == BlockId);

            if (block == null)
            {
                return;
            }

            txtBlockName.Text = block.BlockName;
            txtIp.Text = block.Ip_Address;
            txtPortBlock.Text = block.Port.ToString();
            ckbActiveBlock.Checked = block.Active == clsConst.ACTIVE;
            //lbMeasureTitle.Text = LanguageHelper.GetValueOf("MEASURE_TYPE");
            var lstSensor = AppManager.ListSensor.Where(i => i.OfBlock == BlockId).OrderBy(i => i.Ordinal_Display).ToList();
            BindingDataList(lstSensor);
        }

        private void BindingCboOrdinalDisplay()
        {
            var dataBinding = new List<DataBinding<int>>();

            for (var i = 1; i <= clsConst.MAX_SENSORS; i++)
            {
                dataBinding.Add(new DataBinding<int>()
                {
                    Display = i.ToString(),
                    Value = i,
                });
            }

            BindingHelper.Combobox(cboOrdinalDisplay, dataBinding);
        }

        private bool validateSensor(SensorInfo sensor = null)
        {
            var sensorName = sensor.SensorName;
            var alarmValue = sensor.Alarm_Value;

            if (sensor != null)
            {
                var otherSesorOfBlock = AppManager.ListSensor.Where(i => i.OfBlock == _blockId && i.SensorId != sensor.SensorId).ToList();
                if (otherSesorOfBlock.Any(i => i.SensorName.Equals(sensorName)))
                {
                    txtSensorName.Focus();
                    return ShowMsg(MessageBoxIcon.Warning, LanguageHelper.GetValueOf("FOMAT_MSG_EXIST_SENSORNAME"));
                }
            }


            if (string.IsNullOrEmpty(sensorName))
            {
                txtSensorName.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("FOMAT_MSG_BLANK_USER"), lblSensorName.Text));
            }

            //if (string.IsNullOrEmpty(alarmValue))
            //{
            //    txtAlaimValue.Focus();
            //    return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("FOMAT_MSG_BLANK_USER"), lblAlaimValue.Text));
            //}

            if ((sensor.MeasureType == clsConst.MeasureMode_Volt || sensor.MeasureType == clsConst.MeasureMode_Ion) && alarmValue <= 0)
            {
                txtAlaimValue.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("FOMAT_MSG_INPUT_NUMBER"), lblAlaimValue.Text));
            }

            // Charging value
            if (sensor.MeasureType == clsConst.MeasureMode_Decay)
            {
                // Upper value
                if (sensor.DecayUpperValue < 10 || sensor.DecayUpperValue > 2000)
                {
                    txtUpVal.Focus();
                    return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("MSG_OUTOF_RANGE"), lbUpVal.Text, 10, 2000));
                }
                // Lower value
                if (sensor.DecayLowerValue < 0 || sensor.DecayLowerValue > 1995)
                {
                    txtLowVal.Focus();
                    return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("MSG_OUTOF_RANGE"), lbLowVal.Text, 0, 1995));
                }
                // Decay time check
                if (sensor.DecayTimeCheck < 0 || sensor.DecayTimeCheck > 10)
                {
                    txtDecayTime.Focus();
                    return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("MSG_OUTOF_RANGE"), lbDecayTime.Text, 0, 10));
                }
                // Stop time decay check
                if (sensor.DecayStopTime < 10 || sensor.DecayStopTime > 100)
                {
                    txtDecayStopTime.Focus();
                    return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("MSG_OUTOF_RANGE"), lbDecayStopTime.Text, 10, 100));
                }
                // Ion value check
                if (sensor.IonValueCheck <= 0)
                {
                    txtIonCheck.Focus();
                    return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("FOMAT_MSG_INPUT_NUMBER"), lbIonBalance.Text));
                }
                // Ion time check
                if (sensor.IonTimeCheck <= 0)
                {
                    txtIonStopTimeCheck.Focus();
                    return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("FOMAT_MSG_INPUT_NUMBER"), lbIonStopTime.Text));
                }
            }
            

            return true;
        }

        private bool validateBlock()
        {
            var blockName = txtBlockName.Text.Trim();
            var ipAddress = txtIp.Text.Trim();
            var port = txtPortBlock.Text.Trim();

            var otherBlock = AppManager.ListBlock.FindAll(i => i.BlockId != _blockId);

            if (string.IsNullOrEmpty(blockName))
            {
                txtBlockName.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("FOMAT_MSG_BLANK"), lblBlockName.Text));
            }

            if (blockName.Length > 8)
            {
                txtBlockName.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("FOMAT_MSG_BLOCKNAME_VALIDATE"), lblBlockName.Text));
            }

            if (otherBlock.Any(i => i.BlockName.Equals(blockName)))
            {
                txtBlockName.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("FOMAT_MSG_EXIST_BLOCKNAME"), lblBlockName.Text));
            }

            if (string.IsNullOrEmpty(ipAddress))
            {
                txtIp.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("FOMAT_MSG_BLANK"), lbl_IpAddress.Text));
            }

            if (string.IsNullOrEmpty(port))
            {
                txtPortBlock.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("FOMAT_MSG_BLANK"), lblPort.Text));
            }

            if (!clsCommon.IsNumberAndThanZero(port))
            {
                txtPortBlock.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("FOMAT_MSG_INPUT_NUMBER"), lblPort.Text));
            }

            return true;
        }

        private void bindingSensor()
        {
            var sensor = dgvDevice.GetSelectedData<SensorInfo>();

            if (sensor == null)
            {
                return;
            }

            lblSensorSetting.Text = (sensor.SensorName + " " + LanguageHelper.GetValueOf("MSG_SETTING")).Trim();
            txtSensorName.Text = sensor.SensorName;
            txtAlaimValue.Text = sensor.Alarm_Value.ToString();
            cboOrdinalDisplay.SelectedValue = sensor.Ordinal_Display;
            ckbActiveSensor.Checked = sensor.Active == clsConst.ACTIVE;
            cboMeasureType.SelectedValue = sensor.MeasureType;
            txtUpVal.Text = "" + sensor.DecayUpperValue;
            txtLowVal.Text = "" + sensor.DecayLowerValue;
            txtDecayTime.Text = "" + sensor.DecayTimeCheck;
            txtDecayStopTime.Text = "" + sensor.DecayStopTime;
            txtIonCheck.Text = "" + sensor.IonValueCheck;
            txtIonStopTimeCheck.Text = "" + sensor.IonTimeCheck;
            CboMeasureType_SelectedIndexChanged(null, null);
        }

        private void BindingDataList(List<SensorInfo> data)
        {
            var measureData = cboMeasureType.DataSource as List<DataBinding<int>>;
            data = data.Select(i =>
            {
                i.MeasureTypeShow = measureData.FirstOrDefault(m => m.Value == i.MeasureType).Display;
                return i;
            }).ToList();
            this.dgvDevice.DataSource = data;
        }

        #endregion Private Function

        
    }
}
