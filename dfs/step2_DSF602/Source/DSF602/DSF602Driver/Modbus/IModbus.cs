using System;

namespace DSF602Driver.Modbus
{
    public interface IModbus : IDisposable
    {
        event EventHandler<string[]> HandleWhenReceiveDataFromBlock;

        event EventHandler<DSF602Exception> ErrorsReceiveDataFromBlock;

        event EventHandler HandleWhenReceiveDataEndFromBlock;

        bool IsRunning { get; }

        void Start();

        void End();

        void SelectSensor(ushort sensorId);
        void Charge(ushort sensorId, int type);
        
        void StopCharge(ushort sensorId, int type);

        void ConnectGround(ushort sensorId, bool allway = false);

        void ResetAdj(ushort sensorId);
    }
}