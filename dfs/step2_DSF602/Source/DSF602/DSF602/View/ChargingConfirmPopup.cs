using BaseCommon.Core;
using BaseCommon.Utility;
using DSF602.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DSF602.View
{
    public partial class ChargingConfirmPopup : BaseForm
    {
        public ChargingConfirmPopup()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SettingParam pr = null;
            if (this.Params != null)
            {
                pr = this.Params as SettingParam;
                txtUpVal.Text = "" + pr.UpVal;
                txtLowVal.Text = "" + pr.LowVal;
                txtDecayTime.Text = "" + pr.DecayTimeCheck;
                txtStopTime.Text = "" + pr.StopDecayTime;
                txtIonBalanceCheck.Text = "" + pr.IonBalanceCheck;
                txtStopIonBlance.Text = "" + pr.IonStopTimeCheck;
            }
            

            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.ResultData = null;
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            this.ResultData = true;
            this.Close();
        }
    }
}
