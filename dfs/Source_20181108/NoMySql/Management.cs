using BaseCommon;
using NoMySql.Properties;
using System;
using System.Data;
using System.Windows.Forms;

namespace NoMySql
{
    public partial class Management : BaseForm
    {
        private const string DEVICE1 = "Device 1";
        private const string DEVICE2 = "Device 2";
                
        private const string MSG_COMFIRM_RESET = "Are you sure you want to reset device current?";
        private const string MSG_SAVE_SETTING_SUCCESS = "Save setting success";
        
        private const string FOMAT_MSG_INPUT_NUMBER = "Please input {0} is a number than 0";
        private const string FOMAT_MSG_BLANK = "Please input {0}";
        
        private int _deviceSelected = (int)clsConst.emDeviceType.Device1;

        public bool ChangeSetting { get; set; }

        #region Event Form

        public Management()
        {
            InitializeComponent();
            IntForm();
        }
                
        private void grvDevice_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }
            if (e.ColumnIndex == colDevice.Index)
            {
                var deviceType = clsCommon.CnvNullToInt(e.Value);
                e.Value = xGetDeviceShow(deviceType);
            }
        }

        private void grvDevice_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            grvDevice.Rows[e.RowIndex].Selected = true;

            _deviceSelected = clsCommon.CnvNullToInt(grvDevice.Rows[e.RowIndex].Cells[colDevice.Index].Value);
            lblDevice.Text = (xGetDeviceShow(_deviceSelected) + " Setting").Trim();
            txtDeviceName.Text = clsCommon.CnvNullToString(grvDevice.Rows[e.RowIndex].Cells[colDeviceName.Index].Value);
            txtIpAddress.Text = clsCommon.CnvNullToString(grvDevice.Rows[e.RowIndex].Cells[colIpAdress.Index].Value);
            txtPort.Text = clsCommon.CnvNullToString(grvDevice.Rows[e.RowIndex].Cells[colPort.Index].Value);
            txtAlarmValue.Text = clsCommon.CnvNullToString(grvDevice.Rows[e.RowIndex].Cells[colAlarmValue.Index].Value);
            txtPeriod.Text = clsCommon.CnvNullToString(grvDevice.Rows[e.RowIndex].Cells[colPeriod.Index].Value);
            txtFailLevel.Text = clsCommon.CnvNullToString(grvDevice.Rows[e.RowIndex].Cells[colFailLevel.Index].Value);
            txtSamples.Text = clsCommon.CnvNullToString(grvDevice.Rows[e.RowIndex].Cells[colSamples.Index].Value);
            chkActive.Checked = (bool)grvDevice.Rows[e.RowIndex].Cells[colActive.Index].Value;
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
        
        private void btSaveSetting_Click(object sender, EventArgs e)
        {
            if (!xValidateInputDevice())
            {
                return;
            }
            
            if (_deviceSelected == (int)clsConst.emDeviceType.Device1)
            {
                Settings.Default.DeviceName1 = clsCommon.CnvNullToString(txtDeviceName.Text);
                Settings.Default.ipAddress1 = clsCommon.CnvNullToString(txtIpAddress.Text);
                Settings.Default.port1 = clsCommon.CnvNullToInt(txtPort.Text);
                Settings.Default.AlarmValue1 = clsCommon.CnvNullToInt(txtAlarmValue.Text);
                Settings.Default.FailLevel1 = clsCommon.CnvNullToInt(txtFailLevel.Text);
                Settings.Default.Period1 = clsCommon.CnvNullToInt(txtPeriod.Text);
                Settings.Default.Samples1 = clsCommon.CnvNullToInt(txtSamples.Text);
                Settings.Default.Active1 = chkActive.Checked;
            } else
            {
                Settings.Default.DeviceName2 = clsCommon.CnvNullToString(txtDeviceName.Text);
                Settings.Default.ipAddress2 = clsCommon.CnvNullToString(txtIpAddress.Text);
                Settings.Default.port2 = clsCommon.CnvNullToInt(txtPort.Text);
                Settings.Default.AlarmValue2 = clsCommon.CnvNullToInt(txtAlarmValue.Text);
                Settings.Default.FailLevel2 = clsCommon.CnvNullToInt(txtFailLevel.Text);
                Settings.Default.Period2 = clsCommon.CnvNullToInt(txtPeriod.Text);
                Settings.Default.Samples2 = clsCommon.CnvNullToInt(txtSamples.Text);
                Settings.Default.Active2 = chkActive.Checked;
            }

            Settings.Default.Save();
            LoadListDevice();
            ChangeSetting = ShowMsg(MessageBoxIcon.Information, MSG_SAVE_SETTING_SUCCESS, this.Text);
        }

        private void btResetDefault_Click(object sender, EventArgs e)
        {
            if (ComfirmMsg(MSG_COMFIRM_RESET))
            {
                txtDeviceName.Text = (_deviceSelected == (int)clsConst.emDeviceType.Device1 ? DEVICE1 : DEVICE2);
                txtIpAddress.Text = Settings.Default.ipAddressDefault;
                txtPort.Text = clsCommon.CnvNullToString(Settings.Default.portDefault);
                txtAlarmValue.Text = clsCommon.CnvNullToString(Settings.Default.AlarmValueDefault);
                txtFailLevel.Text = clsCommon.CnvNullToString(Settings.Default.FailLevelDefault);
                txtPeriod.Text = clsCommon.CnvNullToString(Settings.Default.PeriodDefault);
                txtSamples.Text = clsCommon.CnvNullToString(Settings.Default.SamplesDefault);
                chkActive.Checked = true;              
                btSaveSetting.PerformClick();
            }
        }

        #endregion Event Form

        #region Private Function

        private void IntForm()
        {
            // Limit resize form
            this.MinimumSize = this.Size;
            this.MaximumSize = Screen.PrimaryScreen.Bounds.Size;

            grvDevice.AutoGenerateColumns = false;
            txtPort.KeyPress += onlyInputNumber;
            txtAlarmValue.KeyPress += onlyInputNumber;
            txtPeriod.KeyPress += onlyInputNumber;
            txtFailLevel.KeyPress += onlyInputNumber;
            txtSamples.KeyPress += onlyInputNumber;

            LoadListDevice();
        }

        private void LoadListDevice()
        {
            var tblDevice = new DataTable();
            tblDevice.Columns.Add(colDeviceName.DataPropertyName, typeof(string));
            tblDevice.Columns.Add(colIpAdress.DataPropertyName, typeof(string));
            tblDevice.Columns.Add(colPort.DataPropertyName, typeof(int));
            tblDevice.Columns.Add(colAlarmValue.DataPropertyName, typeof(int));
            tblDevice.Columns.Add(colFailLevel.DataPropertyName, typeof(int));
            tblDevice.Columns.Add(colPeriod.DataPropertyName, typeof(int));
            tblDevice.Columns.Add(colSamples.DataPropertyName, typeof(int));
            tblDevice.Columns.Add(colActive.DataPropertyName, typeof(bool));
            tblDevice.Columns.Add(colDevice.DataPropertyName, typeof(int));

            tblDevice.Rows.Add(new object[] {
                DEVICE1,
                Settings.Default.ipAddress1,
                Settings.Default.port1,
                Settings.Default.AlarmValue1,
                Settings.Default.FailLevel1,
                Settings.Default.Period1,
                Settings.Default.Samples1,
                Settings.Default.Active1,
                (int)clsConst.emDeviceType.Device1,
            });

            tblDevice.Rows.Add(new object[] {
                DEVICE2,
                Settings.Default.ipAddress2,
                Settings.Default.port2,
                Settings.Default.AlarmValue2,
                Settings.Default.FailLevel2,
                Settings.Default.Period2,
                Settings.Default.Samples2,
                Settings.Default.Active2,
                (int)clsConst.emDeviceType.Device2,
            });

            grvDevice.DataSource = tblDevice;
            grvDevice.ClearSelection();

            if (_deviceSelected == (int)clsConst.emDeviceType.Device1)
            {
                grvDevice_CellClick(grvDevice, new DataGridViewCellEventArgs(0, 0));
            } else
            {
                grvDevice_CellClick(grvDevice, new DataGridViewCellEventArgs(0, 1));
            }
        }

        private bool xValidateInputDevice()
        {
            txtDeviceName.Text = txtDeviceName.Text.Trim();
            txtIpAddress.Text = txtIpAddress.Text.Trim();
            txtPort.Text = txtPort.Text.Trim();
            txtAlarmValue.Text = txtAlarmValue.Text.Trim();
            txtPeriod.Text = txtPeriod.Text.Trim();
            txtFailLevel.Text = txtFailLevel.Text.Trim();
            txtSamples.Text = txtSamples.Text.Trim();
            
            if (string.IsNullOrEmpty(txtIpAddress.Text))
            {
                txtIpAddress.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_BLANK, "Ip Address"));
            }

            if (string.IsNullOrEmpty(txtPort.Text))
            {
                txtPort.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_BLANK, "Port"));
            }

            if (!clsCommon.IsNumberAndThanZero(txtAlarmValue.Text))
            {
                txtAlarmValue.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_INPUT_NUMBER, "Alarm Value"));
            }

            if (!clsCommon.IsNumberAndThanZero(txtPeriod.Text))
            {
                txtPeriod.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_INPUT_NUMBER, "Period"));
            }

            if (!clsCommon.IsNumberAndThanZero(txtFailLevel.Text))
            {
                txtFailLevel.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_INPUT_NUMBER, "Fail Level"));
            }

            if (!clsCommon.IsNumberAndThanZero(txtSamples.Text))
            {
                txtSamples.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_INPUT_NUMBER, "Samples"));
            }
            
            return true;
        }
        
        private string xGetDeviceShow(int deviceType)
        {
            if (deviceType == (int)clsConst.emDeviceType.Device1)
            {
                return DEVICE1;
            }
            else if (deviceType == (int)clsConst.emDeviceType.Device2)
            {
                return DEVICE2;
            }

            return string.Empty;
        }

        #endregion Private Function
    }
}
