using GPModels;
using LiteDB;
using System;

namespace GPMain.Common.Database
{
    public class LiteDBManager : IDisposable
    {
        private LiteDatabase database;
        private bool disposedValue;

        public bool IsDisposed => disposedValue;

        public static string CreateNewId()
        {
            return ObjectId.NewObjectId().ToString();
        }

        public LiteDBManager(string pathDB, string password)
        {
            database = new LiteDatabase(string.Format(@"Filename={0};Password={1}", pathDB, password));
        }

        public ReposityLiteTable<T> GetTable<T>() where T : BaseModel
        {
            return new ReposityLiteTable<T>(database);
        }

        public bool DropTable<T>() where T : BaseModel
        {
            if (!database.CollectionExists(typeof(T).Name))
            {
                return true;
            }

            return database.DropCollection(typeof(T).Name);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    if (database != null)
                    {
                        database.Dispose();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                database = null;
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~LiteDBManager()
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