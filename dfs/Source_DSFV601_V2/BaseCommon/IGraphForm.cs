using System;
using System.Collections.Generic;
using static BaseCommon.clsConst;

namespace BaseCommon
{
    public interface IGraphForm : IDisposable
    {
        GraphForm FormSetting { get; set; }
        //clsConst.emMeasureType MeasureType { get; set; }
        int RoundValue { get; }
        int AlarmTime { get; }
        int WalkingTime { get; }
        List<DeviceInfo> GetListDevice();
        Response MeasureStart(DeviceInfo device, int measureResult);
        void MeasureEnd(DeviceInfo device, List<MaxValue> lstMaxValue, int measureResult, DateTime? _measureEndTime);
        void MeasureProcess(clsConst.DataSample data, DeviceInfo device);
        void ShowManagement(object sender, EventArgs e);
        void ShowMeasureManagement(object sender, EventArgs e);
        void UpdateListDevice(List<DeviceInfo> lstDevice);
        void StopInsertDB();
    }
}
