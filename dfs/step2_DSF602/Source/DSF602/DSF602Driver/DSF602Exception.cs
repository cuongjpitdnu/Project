using System;

namespace DSF602Driver
{
    public enum DSF602ExceptionStatusCode
    {
        None,
        Initialization,
        Connect,
        ReceiveData,
        SensorError,
    }

    public class DSF602Exception : Exception
    {
        public DSF602ExceptionStatusCode StatusCode { get; private set; }

        public DSF602Exception(string message, DSF602ExceptionStatusCode statusCode = DSF602ExceptionStatusCode.None)
                : base(string.Format("DSF602Exception: {0}", message))
        {
            this.StatusCode = statusCode;
        }

        public DSF602Exception(string message, Exception innerException, DSF602ExceptionStatusCode statusCode = DSF602ExceptionStatusCode.None)
                : base(string.Format("DSF602Exception: {0}", message), innerException)
        {
            this.StatusCode = statusCode;
        }
    }
}