//using GP40Main.Core;
//using System.Collections.Generic;
//using System.IO;

//namespace GP40Main.Models
//{
//    public class DBManagerJson : IDBManager
//    {
//        private string pathDB;
//        private bool disposedValue;

//        public DBManagerJson(string connection)
//        {
//            pathDB = connection;

//            if (!Directory.Exists(pathDB))
//            {
//                Directory.CreateDirectory(pathDB);
//            }
//        }

//        public IRepositoryTable<T> GetTable<T>() where T : BaseModel
//        {
//            return new RepositoryJsonTable<T>(pathDB);
//        }

//        protected virtual void Dispose(bool disposing)
//        {
//            if (!disposedValue)
//            {
//                if (disposing)
//                {
//                    // TODO: dispose managed state (managed objects)
//                }

//                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
//                // TODO: set large fields to null
//                disposedValue = true;
//            }
//        }

//        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
//        // ~DBManagerJson()
//        // {
//        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
//        //     Dispose(disposing: false);
//        // }

//        public void Dispose()
//        {
//            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
//            Dispose(disposing: true);
//            System.GC.SuppressFinalize(this);
//        }
//    }
//}
