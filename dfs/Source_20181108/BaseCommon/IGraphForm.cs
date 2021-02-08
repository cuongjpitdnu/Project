using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCommon
{
    public interface IGraphForm : IDisposable
    {
        GraphForm FormSetting { get; set; }
        clsConst.emMeasureType MeasureType { get; set; }
        clsConst.DeviceInfo DeviceCurrent { get; set; }
        int RoundValue { get; }
        int AlarmTime { get; }
        int WalkingTime { get; }

        clsConst.DeviceInfo[] GetListDevice();
        void MeasureStart();
        void MeasureEnd();
        void MeasureProcess(clsConst.DataSample data);
        void ShowManagement(object sender, EventArgs e);
        void ShowMeasureManagement(object sender, EventArgs e);
    }
}
