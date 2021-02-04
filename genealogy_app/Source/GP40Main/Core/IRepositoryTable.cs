using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GP40Main.Core
{
    public interface IRepositoryTable<T> : IDisposable
    {
        IEnumerable<T> AsEnumerable(bool ignoreDataDeleted = true);

        Task<bool> InsertOneAsync(T dataInsert, bool autoGenId = true);
        Task<bool> UpdateOneAsync(Predicate<T> filter, T dataUpdate);
        Task<bool> DeleteOneAsync(Predicate<T> filter);

        bool InsertOne(T dataInsert, bool autoGenId = true);
        bool UpdateOne(Predicate<T> filter, T dataUpdate);
        bool DeleteOne(Predicate<T> filter);
    }
}
