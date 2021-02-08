using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using BaseCommon.Properties;

namespace BaseCommon
{
    public partial class DeviceManagement : UserControl
    {
        #region const

        private const string MSG_COMFIRM_RESET = "Are you sure you want to reset device current?";
        private const string MSG_SAVE_SETTING_SUCCESS = "Save setting success";

        private const string FOMAT_MSG_INPUT_NUMBER = "Please input {0} is a number than 0";
        private const string FOMAT_MSG_BLANK = "Please input {0}";

        #endregion

        #region variables

        public List<DeviceInfo> ListDevice { get; set; }

        public DeviceInfo GridDeviceSelected { get; set; }

        public DeviceInfo SwapDeviceSelected { get; set; }

        public DeviceInfo SwapOrdinalDisplayDevice{ get; set; }

        #endregion

        #region events delegate

        public event EventHandler EventSave;

        #endregion

        #region functions

        public DeviceManagement()
        {
            InitializeComponent();
            bindingCombobox();
            dgvDevice.AutoGenerateColumns = false;
            txtPort.KeyPress += onlyInputNumber;
            txtAlarmValue.KeyPress += onlyInputNumber;
            txtPeriod.KeyPress += onlyInputNumber;
            txtFailLevel.KeyPress += onlyInputNumber;
            txtSamples.KeyPress += onlyInputNumber;
        }

        public void SetListDevice(List<DeviceInfo> lstDevice)
        {
            lstDevice = lstDevice.OrderBy(i => i.OrdinalDisplay).ToList();
            ListDevice = lstDevice;
            bindingDataGrid(lstDevice);  
        }

        private void bindingCombobox()
        {
            List<CommonObject> cboOrdinalDisplayData = new List<CommonObject>();
            var obj1 = new CommonObject { Key = 1, Value = 1 };
            cboOrdinalDisplayData.Add(obj1);

            var obj2 = new CommonObject { Key = 2, Value = 2 };
            cboOrdinalDisplayData.Add(obj2);

            var obj3 = new CommonObject { Key = 3, Value = 3 };
            cboOrdinalDisplayData.Add(obj3);

            var obj4 = new CommonObject { Key = 4, Value = 4 };
            cboOrdinalDisplayData.Add(obj4);

            var obj5 = new CommonObject { Key = 5, Value = 5 };
            cboOrdinalDisplayData.Add(obj5);

            cboOrdinalDisplay.DisplayMember = "Value"; cboOrdinalDisplay.ValueMember = "Key";
            cboOrdinalDisplay.DataSource = cboOrdinalDisplayData;
        }

        private void bindingDataGrid(List<DeviceInfo> lstDevice)
        {
            this.dgvDevice.DataSource = lstDevice;

            if (lstDevice.Count > 0)
            {
                bindingDevice();
            }
        }

        /// <summary>
        /// Sets the walking mode for only device.
        /// </summary>
        private void setWalkingMode()
        {
            if (GridDeviceSelected.WalkingMode != chkWalkingMode.Checked)
            {
                SwapOrdinalDisplayDevice = ListDevice.FirstOrDefault(i => i.WalkingMode == true);
                ListDevice = ListDevice.Select(c => { c.WalkingMode = false; return c; }).ToList();
                ListDevice.FirstOrDefault(i => i.DeviceId == GridDeviceSelected.DeviceId).WalkingMode = chkWalkingMode.Checked;
            }
        }

        /// <summary>
        /// Sets the ordinal display.
        /// </summary>
        private void setOrdinalDisplay()
        {
            var ordinalDisplayCbo = int.Parse(cboOrdinalDisplay.SelectedValue.ToString());

            if (GridDeviceSelected.OrdinalDisplay != ordinalDisplayCbo)
            {
                var ordinalDisplayGrid = GridDeviceSelected.OrdinalDisplay;
                var lstDevice = ListDevice.OrderBy(i => i.OrdinalDisplay).ToList();

                var selectIndex = ListDevice.FindIndex(i => i.OrdinalDisplay == ordinalDisplayCbo);
                var selectItem = ListDevice[selectIndex];
                lstDevice[selectIndex] = GridDeviceSelected; lstDevice[selectIndex].OrdinalDisplay = ordinalDisplayCbo;

                var oldIndex = ListDevice.FindIndex(i => i.OrdinalDisplay == GridDeviceSelected.OrdinalDisplay);
                lstDevice[oldIndex] = selectItem; lstDevice[oldIndex].OrdinalDisplay = ordinalDisplayGrid;
                SwapDeviceSelected = lstDevice[selectIndex];

                ListDevice = lstDevice;
                bindingDataGrid(ListDevice);
            }
        }

        /// <summary>
        /// Updates the device information.
        /// </summary>
        private void updateDeviceInfo()
        {
            var deviceName = txtDeviceName.Text.Trim();
            var ipAddress = txtIpAddress.Text.Trim();
            var port = txtPort.Text.Trim();
            var alarmValue = txtAlarmValue.Text.Trim();
            var period = txtPeriod.Text.Trim();
            var failLevel = txtFailLevel.Text.Trim();
            var samples = txtSamples.Text.Trim();
            var ordinalDisplay = cboOrdinalDisplay.SelectedValue;

            GridDeviceSelected.DeviceName = deviceName;
            GridDeviceSelected.IpAddress = ipAddress;
            GridDeviceSelected.Port = clsCommon.CnvNullToInt(port);
            GridDeviceSelected.AlarmValue = clsCommon.CnvNullToInt(alarmValue);
            GridDeviceSelected.Period = clsCommon.CnvNullToInt(period);
            GridDeviceSelected.FailLevel = clsCommon.CnvNullToInt(failLevel);
            GridDeviceSelected.Samples = clsCommon.CnvNullToInt(samples);
            GridDeviceSelected.Active = chkActive.Checked;
        }

        /// <summary>
        /// Validates the device.
        /// </summary>
        /// <returns></returns>
        private bool validateDevice()
        {
            var deviceName = txtDeviceName.Text.Trim();
            var ipAddress = txtIpAddress.Text.Trim();
            var port = txtPort.Text.Trim();
            var alarmValue = txtAlarmValue.Text.Trim();
            var period = txtPeriod.Text.Trim();
            var failLevel = txtFailLevel.Text.Trim();
            var samples = txtSamples.Text.Trim();
            var ordinalDisplay = cboOrdinalDisplay.SelectedValue;

            //if (string.IsNullOrEmpty(ipAddress))
            //{
            //    txtIpAddress.Focus();
            //    return ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_BLANK, "Ip Address"));
            //}

            if (string.IsNullOrEmpty(port))
            {
                txtPort.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_BLANK, "Port"));
            }

            if (!clsCommon.IsNumberAndThanZero(port))
            {
                txtPort.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_INPUT_NUMBER, "Port"));
            }

            if (!clsCommon.IsNumberAndThanZero(alarmValue))
            {
                txtAlarmValue.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_INPUT_NUMBER, "Alarm Value"));
            }

            if (!clsCommon.IsNumberAndThanZero(period))
            {
                txtPeriod.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_INPUT_NUMBER, "Period"));
            }

            if (!clsCommon.IsNumberAndThanZero(failLevel))
            {
                txtFailLevel.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_INPUT_NUMBER, "Fail Level"));
            }

            if (!clsCommon.IsNumberAndThanZero(samples))
            {
                txtSamples.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_INPUT_NUMBER, "Samples"));
            }

            return true;
        }

        /// <summary>
        /// Bindings the device.
        /// </summary>
        private void bindingDevice()
        {
            var device = (DeviceInfo)dgvDevice.CurrentRow.DataBoundItem;
            GridDeviceSelected = device;
            lblDevice.Text = (device.DeviceName + " Setting").Trim();
            txtDeviceName.Text = device?.DeviceName?.Trim();
            txtIpAddress.Text = device?.IpAddress?.Trim();
            txtPort.Text = device.Port + "";
            txtAlarmValue.Text = device.AlarmValue + "";
            txtPeriod.Text = device.Period + "";
            txtFailLevel.Text = device.FailLevel + "";
            txtSamples.Text = device.Samples + "";
            cboOrdinalDisplay.SelectedValue = device.OrdinalDisplay;
            chkWalkingMode.Checked = device.WalkingMode;
            chkActive.Checked = device.Active;
        }

        private bool ShowMsg(MessageBoxIcon type, string strMsg, string strTitle = "")
        {
            MessageBox.Show(strMsg, strTitle, MessageBoxButtons.OK, type);

            if (type == MessageBoxIcon.Error || type == MessageBoxIcon.Warning)
            {
                return false;
            }

            return true;
        }

        public bool ComfirmMsg(string strMsg, string strTitle = "")
        {
            return MessageBox.Show(strMsg, strTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK;
        }

        #endregion

        #region events

        protected void onlyInputNumber(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btSaveSetting_Click(object sender, EventArgs e)
        {
            if (!validateDevice())
            {
                return;
            }

            updateDeviceInfo();
            setWalkingMode();
            setOrdinalDisplay();

            EventSave?.Invoke(this, e);
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

        private void dgvDevice_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            dgvDevice.Rows[e.RowIndex].Selected = true;
            bindingDevice();
        }

        private void btResetDefault_Click(object sender, EventArgs e)
        {
            if (ComfirmMsg(MSG_COMFIRM_RESET))
            {
                txtIpAddress.Text = clsConst.IPADDRESS_DEFAULT;
                txtPort.Text = clsConst.PORT_DEFAULT + "";
                txtAlarmValue.Text = clsConst.ALARMVALUE_DEFAULT + "";
                txtFailLevel.Text = clsConst.FAILLEVEL_DEFAULT + "";
                txtPeriod.Text = clsConst.PERIOD_DEFAULT + "";
                txtSamples.Text = clsConst.SAMPLES_DEFAULT + "";
                chkWalkingMode.Checked = false;
                chkActive.Checked = true;

                GridDeviceSelected.IpAddress = clsConst.IPADDRESS_DEFAULT;
                GridDeviceSelected.Port = clsConst.PORT_DEFAULT;
                GridDeviceSelected.AlarmValue = clsConst.ALARMVALUE_DEFAULT;
                GridDeviceSelected.FailLevel = clsConst.FAILLEVEL_DEFAULT;
                GridDeviceSelected.Period = clsConst.PERIOD_DEFAULT;
                GridDeviceSelected.Samples = clsConst.SAMPLES_DEFAULT;
                GridDeviceSelected.WalkingMode = false;
                GridDeviceSelected.Active = true;

                EventSave?.Invoke(this, e);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (dgvDevice.CurrentCell != null)
            {
                bindingDevice();
            }
        }

        #endregion
    }
}
