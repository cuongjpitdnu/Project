using System;

namespace DSF602Driver.Modbus
{
    public interface IModbus : IDisposable
    {
        event EventHandler<string[]> HandleWhenReceiveDataFromBlock;

        event EventHandler<Exception> ErrorsReceiveDataFromBlock;

        event EventHandler HandleWhenReceiveDataEndFromBlock;

        bool IsRunning { get; }

        void Start();

        void End();
    }
}