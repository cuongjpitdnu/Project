using DSF602Driver.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSF602Driver.Sensor
{
    public class ExecRequest
    {
        public ushort ExecAddress
        {
            get;
            private set;
        }

        public typeBase Reg
        {
            get;
            private set;
        }

        public ExecRequest(ushort _addr, typeBase _reg)
        {
            ExecAddress = _addr;
            Reg = _reg;
        }
    }
}
