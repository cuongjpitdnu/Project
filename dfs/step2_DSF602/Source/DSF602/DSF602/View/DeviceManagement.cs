using BaseCommon.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BaseCommon.ControlTemplate;
using DSF602.View.GraphLayout;
using DSF602.Model;

namespace DSF602.View
{
    public partial class DeviceManagement : BaseForm
    {

        private const string FOMAT_NAME_TAB_PAGE = "tabBlock{0}";

        public DeviceManagement()
        {
            InitializeComponent();
            InitForm();
        }

        private void DeviceManagement_Load(object sender, EventArgs e)
        {
            var param = this.Params as SensorInfo;
            if (param != null)
            {
                this.UpdateChildControl(tabMngt, string.Format(FOMAT_NAME_TAB_PAGE, param.OfBlock.ToString()), (c) =>
                {
                    var tab = c as TabPage;
                    if (tab != null)
                    {
                        tabMngt.SelectedTab = tab;
                        ((DeviceLayout)tab.Controls[0]).SelectSensor(param.SensorId);
                    }
                });
            }
        }

        private void DeviceManagement_FormClosed(object sender, FormClosedEventArgs e)
        {
            var objChanged = new ObjChanged();
            objChanged.ListBlockIdChanged = new List<int>();
            objChanged.ListSensorIdChanged = new List<int>();

            foreach (TabPage tab in tabMngt.Controls)
            {
                var device = (tab.Controls.Count > 0 ? tab.Controls[0] : null) as DeviceLayout;

                if (device != null)
                {
                    if (device.BlockChange != null)
                    {
                        objChanged.ListBlockIdChanged.Add(device.BlockChange.BlockId);
                    }

                    if (device.ListSensorChange != null && device.ListSensorChange.Count > 0)
                    {
                        objChanged.ListSensorIdChanged.AddRange(device.ListSensorChange.Select(i => i.SensorId));
                    }
                }
            }

            objChanged.ListBlockIdChanged = objChanged.ListBlockIdChanged.Distinct().ToList();
            objChanged.ListSensorIdChanged = objChanged.ListSensorIdChanged.Distinct().ToList();

            this.ResultData = objChanged;
        }

        private void InitForm()
        {
            //this.Text = DSF602.Language.LanguageHelper.GetValueOf("");
            this.Icon = Properties.Resources.setting;

            foreach (var block in AppManager.ListBlock)
            {
                var tabPageAdd = new TabPage()
                {
                    Name = string.Format(FOMAT_NAME_TAB_PAGE, block.BlockId.ToString()),
                    Text = block.BlockName,
                };

                var objDevice = new DeviceLayout(block.BlockId)
                {
                    Dock = DockStyle.Fill,
                };

                objDevice.EventSavedBlock += (sender, blockSaved) => tabPageAdd.Text = blockSaved.BlockName;
                tabPageAdd.Controls.Add(objDevice);
                tabMngt.Controls.Add(tabPageAdd);
            }
        }
    }

    public class ObjChanged
    {
        public List<int> ListBlockIdChanged { get; set; }
        public List<int> ListSensorIdChanged { get; set; }

    }

}
