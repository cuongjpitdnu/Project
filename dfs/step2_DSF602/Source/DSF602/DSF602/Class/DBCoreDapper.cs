using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DSF602.Class
{
    public class DBCoreDapper : IDisposable
    {
        protected IDbConnection _dbConnection;

        public DBCoreDapper(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        protected string GenerateSqlInsert<T>(string tableName, T entity)
        {
            if (string.IsNullOrEmpty(tableName) || entity == null)
            {
                return string.Empty;
            }

            var properties = entity.GetType().GetProperties();
            var fields = new List<string>();
            var values = new List<string>();

            foreach (var property in properties)
            {
                fields.Add(property.Name);
                values.Add("@" + property.Name);
            }

            var sql = string.Format(" INSERT INTO {0}({1}) VALUES ({2}); ",
                                    tableName,
                                    string.Join(", ", fields.ToArray()),
                                    string.Join(", ", values.ToArray()));

            return sql;
        }

        protected int Insert<T>(string tableName, T entity)
        {

            var sql = GenerateSqlInsert(tableName, entity);

            if (string.IsNullOrEmpty(sql))
            {
                return 0;
            }

            return _dbConnection.Execute(sql, entity);
        }

        protected int Update<T>(string tableName, string[] primaryKeys, T entity)
        {
            if (string.IsNullOrEmpty(tableName) || primaryKeys == null || primaryKeys.Length == 0 || entity == null)
            {
                return 0;
            }

            var properties = entity.GetType().GetProperties();
            var listCheck = new List<string>();
            var keys = new List<string>();
            var values = new List<string>();

            listCheck.AddRange(primaryKeys);
            listCheck = listCheck.Select(i => i.ToLower()).ToList();

            foreach (var property in properties)
            {

                if (listCheck.Any(i => i == property.Name.ToLower()))
                {
                    keys.Add(property.Name + " = @" + property.Name);
                }
                else
                {
                    values.Add(property.Name + " = @" + property.Name);
                }
            }

            var sql = string.Format(" UPDATE {0} SET {1} WHERE {2}; ",
                                    tableName,
                                    string.Join(", ", values.ToArray()),
                                    string.Join(", ", keys.ToArray()));

            return _dbConnection.Execute(sql, entity);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (_dbConnection != null)
                    {
                        if (_dbConnection.State != ConnectionState.Closed)
                        {
                            _dbConnection.Close();
                        }

                        _dbConnection.Dispose();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                _dbConnection = null;
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DBManager()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
