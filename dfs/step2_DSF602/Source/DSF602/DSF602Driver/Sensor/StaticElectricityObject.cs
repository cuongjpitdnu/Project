using DSF602Driver.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSF602Driver.Sensor
{
    public class StaticElectricityObject : SensorBase
    {
        public StaticElectricityObject()
        {
            base.SampleMaps = new List<typeBase>();
            SamplingAddress = 0;
            base.SampleMaps.Add(new typeMeas("CH1", 0, 1));
            base.SampleMaps.Add(new typeMeas("CH2", 0, 1));
            base.SampleMaps.Add(new typeMeas("CH3", 0, 1));
            base.SampleMaps.Add(new typeMeas("CH4", 0, 1));
            base.SampleMaps.Add(new typeMeas("CH5", 0, 1));
            base.SampleMaps.Add(new typeMeas("CH6", 0, 1));
            base.SampleMaps.Add(new typeMeas("CH7", 0, 1));
            base.SampleMaps.Add(new typeMeas("CH8", 0, 1));
            base.SampleMaps.Add(new typeMeas("Temp", 0, 1));
            base.SampleMaps.Add(new typeMeas("Humi", 0, 1));
            base.SampleMaps.Add(new typeStatus("Status", 0, 1));
            base.SampleMaps.Add(new typeBinary("Meas bit", 0, 1));
            base.SampleMaps.Add(new typeRange("Range", 0, 1));
            base.SampleMaps.Add(new typeBinary("Detect bit", 0, 1));
            base.SampleMaps.Add(new typeBinary("Pulse bit", 0, 1));
            base.CoilsMaps = new List<typeBase>();
            base.CoilAddress = 0;
            base.CoilsMaps.Add(new typeCh("高圧CH1", 0, 1));
            base.CoilsMaps.Add(new typeCh("高圧CH2", 0, 1));
            base.CoilsMaps.Add(new typeCh("高圧CH3", 0, 1));
            base.CoilsMaps.Add(new typeCh("高圧CH4", 0, 1));
            base.CoilsMaps.Add(new typeCh("高圧CH5", 0, 1));
            base.CoilsMaps.Add(new typeCh("高圧CH6", 0, 1));
            base.CoilsMaps.Add(new typeCh("高圧CH7", 0, 1));
            base.CoilsMaps.Add(new typeCh("高圧CH8", 0, 1));
            base.CoilsMaps.Add(new typeCh("リレ\u30fcCH1", 0, 1));
            base.CoilsMaps.Add(new typeCh("リレ\u30fcCH2", 0, 1));
            base.CoilsMaps.Add(new typeCh("リレ\u30fcCH3", 0, 1));
            base.CoilsMaps.Add(new typeCh("リレ\u30fcCH4", 0, 1));
            base.CoilsMaps.Add(new typeCh("リレ\u30fcCH5", 0, 1));
            base.CoilsMaps.Add(new typeCh("リレ\u30fcCH6", 0, 1));
            base.CoilsMaps.Add(new typeCh("リレ\u30fcCH7", 0, 1));
            base.CoilsMaps.Add(new typeCh("リレ\u30fcCH8", 0, 1));
            base.CoilsMaps.Add(new typeCh("0Adj CH1", 0, 1));
            base.CoilsMaps.Add(new typeCh("0Adj CH2", 0, 1));
            base.CoilsMaps.Add(new typeCh("0Adj CH3", 0, 1));
            base.CoilsMaps.Add(new typeCh("0Adj CH4", 0, 1));
            base.CoilsMaps.Add(new typeCh("0Adj CH5", 0, 1));
            base.CoilsMaps.Add(new typeCh("0Adj CH6", 0, 1));
            base.CoilsMaps.Add(new typeCh("0Adj CH7", 0, 1));
            base.CoilsMaps.Add(new typeCh("0Adj CH8", 0, 1));
            base.CoilsMaps.Add(new typeCh("予約", 0, 0));
            base.CoilsMaps.Add(new typeCh("予約", 0, 0));
            base.CoilsMaps.Add(new typeCh("予約", 0, 0));
            base.CoilsMaps.Add(new typeCh("予約", 0, 0));
            base.CoilsMaps.Add(new typeCh("予約", 0, 0));
            base.CoilsMaps.Add(new typeCh("予約", 0, 0));
            base.CoilsMaps.Add(new typeCh("予約", 0, 0));
            base.CoilsMaps.Add(new typeCh("ALARM", 0, 1));
            base.DescriteMaps = new List<typeBase>();
            base.DescriteAddress = 0;
            base.DescriteMaps.Add(new typeCh("CH1検出", 0, 0));
            base.DescriteMaps.Add(new typeCh("CH2検出", 0, 0));
            base.DescriteMaps.Add(new typeCh("CH3検出", 0, 0));
            base.DescriteMaps.Add(new typeCh("CH4検出", 0, 0));
            base.DescriteMaps.Add(new typeCh("CH5検出", 0, 0));
            base.DescriteMaps.Add(new typeCh("CH6検出", 0, 0));
            base.DescriteMaps.Add(new typeCh("CH7検出", 0, 0));
            base.DescriteMaps.Add(new typeCh("CH8検出", 0, 0));
            base.DescriteMaps.Add(new typeCh("CH1パルス", 0, 0));
            base.DescriteMaps.Add(new typeCh("CH2パルス", 0, 0));
            base.DescriteMaps.Add(new typeCh("CH3パルス", 0, 0));
            base.DescriteMaps.Add(new typeCh("CH4パルス", 0, 0));
            base.DescriteMaps.Add(new typeCh("CH5パルス", 0, 0));
            base.DescriteMaps.Add(new typeCh("CH6パルス", 0, 0));
            base.DescriteMaps.Add(new typeCh("CH7パルス", 0, 0));
            base.DescriteMaps.Add(new typeCh("CH8パルス", 0, 0));
            base.VersionMaps = new List<typeBase>();
            base.VersionAddress = 30;
            base.VersionMaps.Add(new typeVersion("Ver", 0, 0));
            base.VersionMaps.Add(new typeUptime("Uptime", 0, 0));
            base.DebugMaps = new List<typeBase>();
            base.DebugAddress = 100;
            base.DebugMaps.Add(new typeUshort("Raw CH1", 0, 1));
            base.DebugMaps.Add(new typeUshort("Raw CH2", 0, 1));
            base.DebugMaps.Add(new typeUshort("Raw CH3", 0, 1));
            base.DebugMaps.Add(new typeUshort("Raw CH4", 0, 1));
            base.DebugMaps.Add(new typeUshort("Raw CH5", 0, 1));
            base.DebugMaps.Add(new typeUshort("Raw CH6", 0, 1));
            base.DebugMaps.Add(new typeUshort("Raw CH7", 0, 1));
            base.DebugMaps.Add(new typeUshort("Raw CH8", 0, 1));
            base.DebugMaps.Add(new typeUshort("0Adj CH1", 0, 1));
            base.DebugMaps.Add(new typeUshort("0Adj CH2", 0, 1));
            base.DebugMaps.Add(new typeUshort("0Adj CH3", 0, 1));
            base.DebugMaps.Add(new typeUshort("0Adj CH4", 0, 1));
            base.DebugMaps.Add(new typeUshort("0Adj CH5", 0, 1));
            base.DebugMaps.Add(new typeUshort("0Adj CH6", 0, 1));
            base.DebugMaps.Add(new typeUshort("0Adj CH7", 0, 1));
            base.DebugMaps.Add(new typeUshort("0Adj CH8", 0, 1));
            base.DebugMaps.Add(new typeUshort("R-SW", 0, 1));
            base.DebugMaps.Add(new typeUshort("DIPSW", 0, 1));
            base.RegMaps = new List<ModbusParam>();
            ModbusParam item = new ModbusParam("接点", 0)
            {
                Params = new List<typeBase>
            {
                new typeBinary("ALARM", 0, 1),
                new typeBinary("高圧電源", 0, 1),
                new typeBinary("高圧リレ\u30fc", 0, 1),
                new typeBinary("0Adj", 0, 1)
            }
            };
            base.RegMaps.Add(item);
            ModbusParam item2 = new ModbusParam("ネットワ\u30fcク", 10)
            {
                Params = new List<typeBase>
            {
                new typeIP("IPアドレス", 0, 1),
                new typeIP("サブネットマスク", 0, 1),
                new typeIP("ゲ\u30fcトウェイ", 0, 1),
                new typeUshort("ポ\u30fcト", 0, 1),
                new typeUshort("DHCP", 0, 1),
                new typeExec("設定", 0, 1)
            }
            };
            base.RegMaps.Add(item2);
            ModbusParam item3 = new ModbusParam("シリアル", 20)
            {
                Params = new List<typeBase>
            {
                new typeBaud("ボ\u30fcレ\u30fcト", 0, 1),
                new typeStop("Stop", 0, 1),
                new typeParity("Parity", 0, 1),
                new typeUshort("遅延時間", 0, 1),
                new typeExec("設定", 0, 1)
            }
            };
            base.RegMaps.Add(item3);
            ModbusParam item4 = new ModbusParam("MAC", 100)
            {
                Params = new List<typeBase>
            {
                new typeMAC("MAC", 0, 1),
                new typeExec("設定", 0, 1)
            }
            };
            base.RegMaps.Add(item4);
            base.ExecMaps = new List<ExecRequest>();
            base.ExecMaps.Add(new ExecRequest(30, new typeExec("Factory Init", 255, 1)));
            base.ExecMaps.Add(new ExecRequest(31, new typeExec("Soft Reset", 255, 0)));
        }
    }
}
