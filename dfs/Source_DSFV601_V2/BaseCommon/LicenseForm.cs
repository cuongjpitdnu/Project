using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BaseCommon
{
    public partial class LicenseForm : BaseForm
    {
        private const string MSG_KEY_BLANLK = "Please input key active!";
        private const string MSG_ACTIVATE_ERR = "Activate machine fail! Please contact system Admin!";
        private const string MSG_ERR_IP = "IP of device: {0} wrong!";

        private IGraphForm iSetting;
        private GraphForm graphForm;

        public LicenseForm(IGraphForm iSetting, GraphForm graphForm)
        {
            this.iSetting = iSetting;
            this.graphForm = graphForm;
            InitializeComponent();

            txtlMachineCode.Text = string.Join(clsConst.KEY_CHAR_JOIN, clsCommon.GetMachineCode());
            txtKey.Focus();
        }

        private void txtIpAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.Split('.').Length - 1 >= 3))
            {
                e.Handled = true;
            }
        }

        private void BtnLoadFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Application.StartupPath,
                Title = "Browse Text Files",
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "txt",
                Multiselect = false,
                Filter = "txt files (*.txt)|*.txt",
                FilterIndex = 2,
                RestoreDirectory = true,
                ReadOnlyChecked = true,
            })
            {
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                txtKey.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtKey.Text))
            {
                ShowMsg(MessageBoxIcon.Warning, MSG_KEY_BLANLK);
                txtKey.Focus();
                return;
            }

            if (!clsCommon.ValidateMachineAndDevices(iSetting.GetListDevice(), txtKey.Text))
            {
                ShowMsg(MessageBoxIcon.Error, MSG_ACTIVATE_ERR);
                txtKey.Focus();
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
            graphForm?.Show();
        }

        private DeviceInfo getDefaultDevice(int deviceId, string deviceName, int ordinalDisplay, string ipAddress = clsConst.IPADDRESS_DEFAULT, string macAddress = "", int port = clsConst.PORT_DEFAULT)
        {
            return new DeviceInfo
            {
                DeviceId = deviceId,
                DeviceName = deviceName,
                IpAddress = ipAddress,
                Port = port,
                AlarmValue = clsConst.ALARMVALUE_DEFAULT,
                Period = clsConst.PERIOD_DEFAULT,
                FailLevel = clsConst.FAILLEVEL_DEFAULT,
                Samples = clsConst.SAMPLES_DEFAULT,
                WalkingMode = false,
                Active = true,
                OrdinalDisplay = ordinalDisplay,
                MacAddress = macAddress
            };
        }
    }
}
