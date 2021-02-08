using DSF602Driver.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSF602Driver.Sensor
{
    public class ModbusParam
    {
        private int modbus_frame_parameter_length;

        private List<typeBase> parameters;

        public string BlockTitle
        {
            get;
            set;
        }

        public int RegisterAddress
        {
            get;
            set;
        }

        public int ParameterLength => modbus_frame_parameter_length;

        public List<typeBase> Params
        {
            get
            {
                return parameters;
            }
            set
            {
                parameters.Clear();
                modbus_frame_parameter_length = 0;
                foreach (typeBase item in value)
                {
                    modbus_frame_parameter_length += item.Len;
                }
                parameters = value;
            }
        }

        public ModbusParam(string title, int address)
        {
            BlockTitle = title;
            RegisterAddress = address;
            parameters = new List<typeBase>();
            modbus_frame_parameter_length = 0;
        }

        public override int GetHashCode()
        {
            return BlockTitle.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            ModbusParam modbusParam = obj as ModbusParam;
            if (modbusParam == null)
            {
                return false;
            }
            return BlockTitle == modbusParam.BlockTitle;
        }
    }
}
