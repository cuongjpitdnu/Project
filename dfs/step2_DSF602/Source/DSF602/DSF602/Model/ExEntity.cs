using GraphLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BaseCommon.clsConst;

namespace DSF602.Model
{
    public class MUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int Role { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

    }

    public class Block
    {
        public int BlockId { get; set; }
        public string BlockName { get; set; }
        public string Ip_Address { get; set; }
        public int Port { get; set; }
        public int Active { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int UpdateBy { get; set; }

        public string ProductName { get; set; }
        

        public string DefaultParams { get; set; }

    }

    public class SensorInfo
    {
        public int SensorId { get; set; }
        public int MeasureId { get; set; }
        public string SensorName { get; set; }
        public int Alarm_Value { get; set; }
        public int Ordinal_Display { get; set; }
        public int OfBlock { get; set; }
        public int Active { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int UpdateBy { get; set; }
        public DataSource GraphData { get; set; }

        public int Result_Measure { get; set; }

        public string GraphName { get; set; }

        public bool Alarm { get; set; }

        public int ActualValue { get; set; }

        public int MeasureType { get; set; }

        public string MeasureTypeShow { get; set; }

        public int DecayUpperValue { get; set; }

        public int DecayLowerValue { get; set; }

        public int DecayTimeCheck { get; set; }

        public int DecayStopTime { get; set; }

        public int IonValueCheck { get; set; }

        public int IonTimeCheck { get; set; }

        public int AutoCheckFlag { get; set; }

        public string AutoCheckTime { get; set; }

        public string AutoCheckDays { get; set; }

        public bool AlarmStatus { get; set; }

        public MeasureState MeasureState { get; set; }

        public double Actual_DecayPositive { get; set; }

        public double Actual_DecayNegative { get; set; }

        public double Actual_IonCheck { get; set; }

        public List<int> PositivePoint { get; set; }

        public List<int> NegativePoint { get; set; }

        public double ActualDecayPositiveTime { get; set; }

        public double ActualDecayNegativeTime { get; set; }

        public double ActualIBMax { get; set; }
    }

    public class Measure
    {
        public int MeasureId { get; set; }
        public int SensorId { get; set; }
        public int Measure_Type { get; set; }
        public int Alarm_Value { get; set; }
        public DateTime? Start_time { get; set; }
        public DateTime? End_time { get; set; }
        public int Measure_Result { get; set; }
        public int UserId { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int Delete_Flag { get; set; }

    }

    public class DGVMeasureInfo
    {
        public int No { get; set; }
        public int MeasureId { get; set; }
        public int SensorId { get; set; }
        public int Measure_Type { get; set; }
        public int Alarm_Value { get; set; }
        public DateTime? Start_time { get; set; }
        public DateTime? End_time { get; set; }
        public int Measure_Result { get; set; }

        public string MeasureDisplay { get; set; }
        public int UserId { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int Delete_Flag { get; set; }

        public string SensorName { get; set; }
        public string FullName { get; set; }
        public string BlockName { get; set; }

    }

    public class MeasureDetail
    {
        public int No { get; set; }
        public  int DetailId { get; set; }
        public int MeasureId { get; set; }
        public int Actual_Value { get; set; }
        public DateTime? Samples_time { get; set; }
        public int Detail_Result { get; set; }
        public string DBName { get; set; }
        public int SensorId { get; set; }
    }

    public class CommonObject
    {
        public int Key { get; set; }

        public int Value { get; set; }
    }

    public class CommonStringObject
    {
        public int Key { get; set; }

        public string Value { get; set; }
    }

    public class MsgData
    {
        public string KeyCheck { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }

    public class SettingParam
    {
        public int VoltAlarmValue { get; set; }

        public int IonAlarmValue { get; set; }

        public int UpVal { get; set; }
        public int LowVal { get; set; }
        public int DecayTimeCheck { get; set; }
        public int StopDecayTime { get; set; }
        public int IonBalanceCheck { get; set; }
        public int IonStopTimeCheck { get; set; }

        public string AutoCheckTimes { get; set; }

        public string AutoCheckDays { get; set; }

        public bool IsAuto { get; set; }
    }
}
