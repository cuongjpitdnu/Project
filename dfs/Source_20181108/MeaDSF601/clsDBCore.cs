namespace MeaDSF601
{
    using System;
    using System.Collections.Generic;
    using MySql.Data.MySqlClient;
    using System.Data;

    class clsDBCore : IDisposable
    {
        private static clsDBCore _dbCore = null;
        private MySqlConnection _cnn = null;
        private MySqlTransaction _sqlTran = null;
        private long _lastInsertId;
        private string _lastSqlQuery = string.Empty;

        public long InsertLastId
        {
            get
            {
                return _lastInsertId;
            }
        }

        public string LastSqlQuery
        {
            get
            {
                return _lastSqlQuery;
            }
        }

        public static clsDBCore getInstance(string connString = "")
        {
            if (_dbCore == null)
            {
                try
                {
                    _dbCore = new clsDBCore(connString);
                    _dbCore.Open();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("getInstance: " + ex.Message);
                }
            }
            return _dbCore;
        }

        public static void DisposeInstance()
        {
            if (_dbCore != null)
            {
                _dbCore.Close();
                _dbCore.Dispose();
                _dbCore = null;
            }

            //if (_cnn != null)
            //{
            //    _cnn.Dispose();
            //    _cnn = null;
            //}
        }

        public clsDBCore(string connString = "")
        {
            if (string.IsNullOrEmpty(connString))
            {
                return;
            }

            _cnn = new MySqlConnection(connString);
        }

        public bool Open(bool createNew = true)
        {
            if (_cnn == null)
            {
                return false;
            }

            try
            {
                if (_cnn.State == ConnectionState.Closed)
                {
                    _cnn.Open();
                }
                else if (createNew)
                {
                    Close();
                    _cnn.Open();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Open DB: " + ex.Message);
                return false;
            }
        }

        public bool Close()
        {
            if (_cnn == null)
            {
                return false;
            }

            try
            {
                if (_cnn.State != ConnectionState.Closed)
                {
                    _cnn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Open DB: " + ex.Message);
                return false;
            }
            finally
            {
                if (_cnn != null)
                {
                    _cnn.Dispose();
                    _cnn = null;
                }
            }
        }

        public DataTable GetTable(string sql)
        {
            DataTable rst = new DataTable();
            _lastSqlQuery = sql;

            try
            {
                using (var da = new MySqlDataAdapter(sql, _cnn))
                {
                    da.Fill(rst);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetTable: " + ex.Message);
            }

            return rst;
        }

        public bool BeginTransaction()
        {
            if (_sqlTran != null)
            {
                _sqlTran.Dispose();
                _sqlTran = null;
            }

            if (_cnn == null)
            {
                return false;
            }

            try
            {
                _sqlTran = _cnn.BeginTransaction();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Commit()
        {
            if (_cnn == null || _sqlTran == null)
            {
                return false;
            }

            try
            {
                _sqlTran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            } finally
            {
                _sqlTran.Dispose();
                _sqlTran = null;
            }
        }

        public bool Rollback()
        {
            if (_cnn == null || _sqlTran == null)
            {
                return false;
            }

            try
            {
                _sqlTran.Rollback();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                _sqlTran.Dispose();
                _sqlTran = null;
            }
        }

        public bool ExecuteNonQuery(string sql, bool sqlTrans = true)
        {

            if (_cnn == null)
            {
                return false;
            }

            MySqlTransaction objSqlTran = null;
            MySqlCommand cmd = null;

            if (sqlTrans)
            {
                objSqlTran = _cnn.BeginTransaction();
            } else
            {
                objSqlTran = _sqlTran;
            }
            
            try
            {
                cmd = _cnn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Transaction = objSqlTran;
                cmd.ExecuteNonQuery();

                _lastSqlQuery = sql;
                _lastInsertId = cmd.LastInsertedId;
                
                if (sqlTrans && objSqlTran != null)
                {
                    objSqlTran.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ExecuteNonQuery: " + ex.Message);
                if (sqlTrans && objSqlTran != null)
                {
                    try
                    {
                        objSqlTran.Rollback();
                    }
                    catch (Exception)
                    {
                    }
                }
                return false;
            }
            finally
            {
                if (sqlTrans && objSqlTran != null)
                {
                    objSqlTran.Dispose();
                    objSqlTran = null;
                }

                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
            }
        }

        public bool InsertTable(string tableName, Dictionary<string, object> insertData, bool sqlTrans = true)
        {
            return ExecuteNonQuery(CreateInsertSql(tableName, insertData), sqlTrans);
        }

        public bool UpdateTable(string tableName, Dictionary<string, object> updateData, string strWhere = "", bool sqlTrans = true)
        {
            return ExecuteNonQuery(CreateUpdateSql(tableName, updateData, strWhere), sqlTrans);
        }

        public string CreateInsertSql(string tableName, Dictionary<string, object> insertData)
        {
            if (string.IsNullOrEmpty(tableName) || insertData == null || insertData.Count == 0)
            {
                return string.Empty;
            }

            string[] arrKey = new string[insertData.Count];
            string[] arrValue = new string[insertData.Count];

            int i = 0;

            foreach (var obj in insertData)
            {
                string value = string.Empty;

                if (obj.Value != null)
                {
                    value = obj.Value.GetType() == typeof(int) || obj.Value.GetType() == typeof(bool)
                            ? obj.Value.ToString()
                            : "'" + MySqlHelper.EscapeString(obj.Value.ToString()) + "'";
                }

                arrKey[i] = obj.Key;
                arrValue[i] = !string.IsNullOrEmpty(value) && value != "''" ? value : "NULL";
                i++;
            }

            string sql = string.Empty;
            sql += " INSERT INTO " + tableName + " (" + string.Join(", ", arrKey) + ") ";
            sql += " VALUES (" + string.Join(", ", arrValue) + ") ";

            return sql;
        }

        public string CreateUpdateSql(string tableName, Dictionary<string, object> updateData, string strWhere = "")
        {
            if (string.IsNullOrEmpty(tableName) || updateData == null || updateData.Count == 0)
            {
                return string.Empty;
            }

            string[] arrValue = new string[updateData.Count];

            int i = 0;

            foreach (var obj in updateData)
            {
                string value = "NULL";

                if (obj.Value != null)
                {
                    value = obj.Value.GetType() == typeof(int) || obj.Value.GetType() == typeof(bool)
                            ? obj.Value.ToString()
                            : "'" + MySqlHelper.EscapeString(obj.Value.ToString()) + "'";
                }

                arrValue[i] = obj.Key + " = " + value;
                i++;
            }

            string sql = string.Empty;
            sql += " UPDATE " + tableName + " ";
            sql += " SET " + string.Join(", ", arrValue) + " ";
            sql += !string.IsNullOrWhiteSpace(strWhere) ? (" WHERE " + strWhere + " ") : "";

            return sql;
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
                    //Close();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~clsDBCore() {
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
