using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GP40Main.Core
{
    public class ReposityLiteTable<T> : IDisposable where T : BaseModel
    {
        private ILiteCollection<T> table;
        private bool disposedValue;

        public ReposityLiteTable(LiteDatabase database)
        {
            table = database.GetCollection<T>();
            table.EnsureIndex(i => i.Id);
        }

        public ILiteQueryable<T> CreateQuery(bool ignoreDataDeleted = true)
        {
            if (ignoreDataDeleted)
            {
                return table.Query().Where(i => !i.DeleteDate.HasValue);
            }

            return table.Query();
        }

        public ILiteQueryable<T> CreateQuery(Expression<Func<T, bool>> filter, bool ignoreDataDeleted = true)
        {
            if (filter == null)
            {
                return CreateQuery(ignoreDataDeleted);
            }

            return CreateQuery(ignoreDataDeleted).Where(filter);
        }

        public IEnumerable<T> AsEnumerable(bool ignoreDataDeleted = true)
        {
            if (ignoreDataDeleted)
            {
                return table.Query().Where(i => !i.DeleteDate.HasValue).ToEnumerable();
            }

            return table.Query().ToEnumerable();
        }

        public Task<bool> DeleteOneAsync(Predicate<T> filter)
        {
            //var dataUpdate = this.AsEnumerable().FirstOrDefault(i => filter(i));

            //if (dataUpdate != null)
            //{
            //    dataUpdate.DeleteDate = DateTime.Now;
            //    return this.UpdateOneAsync(i => i.Id == dataUpdate.Id, dataUpdate);
            //}
            return Task.FromResult(false);
        }

        public Task<bool> InsertOneAsync(T dataInsert, bool autoGenId = true)
        {
            return Task.FromResult(InsertOne(dataInsert, autoGenId));
        }

        public Task<bool> UpdateOneAsync(Predicate<T> filter, T dataUpdate)
        {
            return Task.FromResult(UpdateOne(filter, dataUpdate));
        }

        public bool InsertOne(T dataInsert, bool autoGenId = true)
        {
            if (dataInsert == null)
            {
                return false;
            }

            if (autoGenId)
            {
                dataInsert.Id = LiteDBManager.CreateNewId();
            }

            table.Insert(dataInsert);
            table.EnsureIndex(i => i.Id);

            return true;
        }

        public bool UpdateOne(Predicate<T> filter, T dataUpdate)
        {
            if (dataUpdate == null)
            {
                return false;
            }

            dataUpdate.LastUpdate = DateTime.Now;

            return table.Update(dataUpdate);
        }

        public bool UpdateOne(Expression<Func<T, bool>> filter, Action<T> callback)
        {
            if (filter == null || callback == null)
            {
                return false;
            }

            var dataUpdate = table.Query().Where(filter).FirstOrDefault();

            if (dataUpdate != null)
            {
                callback(dataUpdate);
            }

            return UpdateOne(dataUpdate);
        }

        public bool UpdateOne(T dataUpdate)
        {
            if (dataUpdate == null)
            {
                return false;
            }

            dataUpdate.LastUpdate = DateTime.Now;

            return table.Update(dataUpdate);
        }

        public bool DeleteOne(Predicate<T> filter)
        {
            throw new NotImplementedException();
        }


        public bool InsertBulk(IEnumerable<T> entities, bool autoGenId = true)
        {
            if (entities == null)
            {
                return false;
            }

            if (autoGenId)
            {
                entities = entities.Select((entity) =>
                {
                    entity.Id = LiteDBManager.CreateNewId();
                    return entity;
                }).ToList();
            }

            table.InsertBulk(entities);
            table.EnsureIndex(i => i.Id);

            return true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ReposityLiteTable()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
