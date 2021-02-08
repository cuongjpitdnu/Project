using BaseCommon;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NoMySql
{
    public partial class Management : BaseForm
    {
        #region Variables

        private const string MSG_SAVE_SETTING_SUCCESS = "Save setting success";
        public bool ChangeSetting { get; set; }

        #endregion

        #region functions

        public Management()
        {
            InitializeComponent();
            this.dvcManagement.EventSave += btnSaveSetting_Click;
        }

        public void InitForm(List<DeviceInfo> lstDeviceInfo)
        {
            this.dvcManagement.SetListDevice(lstDeviceInfo);
        }

        #endregion

        #region events

        private void btnSaveSetting_Click(object sender, EventArgs e)
        {
            var deviceManagement = (DeviceManagement)sender;
            var lstDeviceInfo = deviceManagement.ListDevice;
            clsSuportSerialize.BinSerialize(clsConfig.SQLITE_DB_PATH, lstDeviceInfo);
            this.dvcManagement.SetListDevice(lstDeviceInfo);
            this.ChangeSetting = true;
            ShowMsg(MessageBoxIcon.Information, MSG_SAVE_SETTING_SUCCESS, this.Text);
        }

        #endregion
    }
}
