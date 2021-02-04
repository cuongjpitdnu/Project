using GP40Main.Models;
using System;

namespace GP40Main.Core
{
    public interface IDBManager : IDisposable
    {
        IRepositoryTable<T> GetTable<T>() where T : BaseModel;
    }
}
