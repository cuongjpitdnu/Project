using DSF602Driver.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSF602Driver.Sensor
{
    public class SensorBase
    {
        public int DeviceId;

        public string SensorName;

        public int SamplingAddress;

        public int SamplingRegs;

        public int MeasPramsIdx;

        public List<typeBase> SampleMaps
        {
            get;
            set;
        }

        public int CoilAddress
        {
            get;
            set;
        }

        public List<typeBase> CoilsMaps
        {
            get;
            set;
        }

        public int DescriteAddress
        {
            get;
            set;
        }

        public List<typeBase> DescriteMaps
        {
            get;
            set;
        }

        public int VersionAddress
        {
            get;
            set;
        }

        public List<typeBase> VersionMaps
        {
            get;
            set;
        }

        public int DebugAddress
        {
            get;
            set;
        }

        public List<typeBase> DebugMaps
        {
            get;
            set;
        }

        public List<ModbusParam> RegMaps
        {
            get;
            set;
        }

        public List<ExecRequest> ExecMaps
        {
            get;
            set;
        }

        public SensorBase()
        {
            DeviceId = 0;
            SensorName = "";
            SamplingAddress = 0;
            SamplingRegs = 0;
            MeasPramsIdx = 0;
        }

        //public virtual void SensorComponent(DataGridView mdgv)
        //{
        //    mdgv.RowCount = SampleMaps.Count;
        //    for (int i = 0; i < SampleMaps.Count; i++)
        //    {
        //        mdgv[0, i].Value = SampleMaps[i].Title;
        //        mdgv[0, i].ReadOnly = true;
        //        if (MeasPramsIdx <= i && SampleMaps[i].Ratio != 0)
        //        {
        //            if (SampleMaps[i].Type == ParameterType.TYPE_SELECT)
        //            {
        //                mdgv[1, i] = new DataGridViewComboBoxCell();
        //                string[] array = SampleMaps[i].SelectItem();
        //                string[] array2 = array;
        //                foreach (string item in array2)
        //                {
        //                    ((DataGridViewComboBoxCell)mdgv[1, i]).Items.Add(item);
        //                }
        //            }
        //            mdgv[1, i].Value = SampleMaps[i].ToString();
        //            mdgv[1, i].ReadOnly = false;
        //        }
        //        else
        //        {
        //            mdgv[1, i].ReadOnly = true;
        //        }
        //    }
        //}

        //public virtual void SampleAfter(DataGridView mdgv, byte[] frame, ModbusMaster master)
        //{
        //}

        //public virtual async Task SettingsAfterAsync(ModbusMaster master)
        //{
        //    await Task.Delay(1);
        //}

        //public virtual void SensorSetting()
        //{
        //}

        //public virtual void SensorExec()
        //{
        //}

        //public virtual void CalibExec(DataGridView mdgv, ModbusMaster master)
        //{
        //}

        //public virtual void TempCalExec(DataGridView mdgv, ModbusMaster master)
        //{
        //}
    }
}
