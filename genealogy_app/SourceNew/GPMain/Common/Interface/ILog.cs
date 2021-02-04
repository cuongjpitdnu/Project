using System;
using System.Runtime.CompilerServices;

namespace GPMain.Common.Interface
{
    public interface ILog
    {
        void Info(Type type, string msg = "", [CallerMemberName] string memberName = "");
        void Debug(Type type, string msg = "", [CallerMemberName] string memberName = "");
        void Error(Type type, Exception ex, string msg = "", [CallerMemberName] string memberName = "");
    }
}
