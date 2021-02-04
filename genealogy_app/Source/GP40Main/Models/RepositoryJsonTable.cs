//using GP40Main.Core;
//using GP40Main.Utility;
//using JsonFlatFileDataStore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace GP40Main.Models
//{
//    public class KeyValueModel
//    {
//        public int Id { get; set; }
//        public string Data { get; set; }
//    }

//    /// <summary>
//    /// Link lib: https://github.com/ttu/json-flatfile-datastore
//    /// </summary>
//    /// <typeparam name="T"></typeparam>
//    public class RepositoryJsonTable<T> : IRepositoryTable<T> where T : BaseModel
//    {
//        private DataStore dataStore;
//        private IDocumentCollection<KeyValueModel> collection;
//        private bool disposedValue;

//        public RepositoryJsonTable(string pathDB)
//        {
//            var path = pathDB + typeof(T).Name + ".json";
//            dataStore = new DataStore(path, reloadBeforeGetCollection: true);
//            collection = dataStore.GetCollection<KeyValueModel>();
//        }

//        public IEnumerable<T> AsEnumerable(bool ignoreDataDeleted = true)
//        {
//            var rst = collection.AsQueryable()
//                                .Select(i => i.Data.Decrypt(AppConst.PassEncryptString).ToObject<T>());

//            if (ignoreDataDeleted)
//            {
//                return rst.Where(i => !i.DeleteDate.HasValue);
//            }

//            return rst;
//        }

//        public Task<bool> InsertOneAsync(T dataInsert, bool autoGenId = true)
//        {
//            if (dataInsert != null && autoGenId)
//            {
//                dataInsert.Id = DateTime.Now.ToString("yyyyMMddHHmmss");
//            }

//            return collection.InsertOneAsync(new KeyValueModel()
//            {
//                Data = dataInsert.ToJson().Encrypt(AppConst.PassEncryptString)
//            });
//        }

//        public Task<bool> UpdateOneAsync(Predicate<T> filter, T dataUpdate)
//        {
//            if (dataUpdate != null)
//            {
//                dataUpdate.LastUpdate = DateTime.Now;
//            }

//            return collection.UpdateOneAsync(i => filter(i.Data.Decrypt(AppConst.PassEncryptString).ToObject<T>()), new {
//                Data = dataUpdate.ToJson().Encrypt(AppConst.PassEncryptString)
//            });
//        }

//        public Task<bool> DeleteOneAsync(Predicate<T> filter)
//        {
//            var dataUpdate = this.AsEnumerable().FirstOrDefault(i => filter(i));

//            if (dataUpdate != null)
//            {
//                dataUpdate.DeleteDate = DateTime.Now;
//                return this.UpdateOneAsync(i => i.Id == dataUpdate.Id, dataUpdate);
//            }

//            return Task.FromResult(false);
//        }

//        protected virtual void Dispose(bool disposing)
//        {
//            if (!disposedValue)
//            {
//                if (disposing)
//                {
//                    // TODO: dispose managed state (managed objects)
//                    if (dataStore != null)
//                    {
//                        dataStore.Dispose();
//                    }
//                }

//                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
//                // TODO: set large fields to null
//                dataStore = null;
//                collection = null;
//                disposedValue = true;
//            }
//        }

//        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
//        // ~RepositoryJsonTable()
//        // {
//        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
//        //     Dispose(disposing: false);
//        // }

//        public void Dispose()
//        {
//            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
//            Dispose(disposing: true);
//            GC.SuppressFinalize(this);
//        }

//        public bool InsertOne(T dataInsert, bool autoGenId = true)
//        {
//            if (dataInsert != null && autoGenId)
//            {
//                dataInsert.Id = DateTime.Now.ToString("yyyyMMddHHmmss");
//            }

//            return collection.InsertOne(new KeyValueModel()
//            {
//                Data = dataInsert.ToJson().Encrypt(AppConst.PassEncryptString)
//            });
//        }

//        public bool UpdateOne(Predicate<T> filter, T dataUpdate)
//        {
//            if (dataUpdate != null)
//            {
//                dataUpdate.LastUpdate = DateTime.Now;
//            }

//            return collection.UpdateOne(i => filter(i.Data.Decrypt(AppConst.PassEncryptString).ToObject<T>()), new
//            {
//                Data = dataUpdate.ToJson().Encrypt(AppConst.PassEncryptString)
//            });
//        }

//        public bool DeleteOne(Predicate<T> filter)
//        {
//            var dataUpdate = this.AsEnumerable().FirstOrDefault(i => filter(i));

//            if (dataUpdate != null)
//            {
//                dataUpdate.DeleteDate = DateTime.Now;
//                return this.UpdateOne(i => i.Id == dataUpdate.Id, dataUpdate);
//            }

//            return false;
//        }
//    }
//}
