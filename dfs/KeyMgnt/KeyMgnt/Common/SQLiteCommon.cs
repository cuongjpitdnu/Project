using KeyMgnt.Const;
using KeyMgnt.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace KeyMgnt.Common
{
    public class SQLiteCommon
    {
        private static SQLiteConnection dbConnection;
        private static SQLiteTransaction dbTransaction;

        public static void InitDB()
        {
            try
            {
                string sqlFilePath = Config.SQL_DATA_PATH + "\\" + Config.SQL_DB_NAME;

                if (File.Exists(sqlFilePath))
                {
                    if (connectSQLite())
                    {
                        string sql = string.Format("SELECT * FROM M_USERS WHERE USERID='{0}' AND PASSWORD='{1}'", Config.DB_ADMIN_USERNAME, Config.DB_ADMIN_PASSWORD);
                        var result = ExecuteSqlWithResult(sql);
                        if (result == null || !result.HasRows)
                        {
                            sql = "INSERT INTO M_USERS (USERID, PASSWORD, FULLNAME, EMAIL, MOBILE, ROLE) VALUES ('admin', 'admin', 'Nguyễn Văn Hiếu', 'hieu@akb.com.vn', '0968901256', 0);";
                            ExecuteSqlNonResult(sql);
                        }
                    }
                }
                else
                {
                    SQLiteConnection.CreateFile(Config.SQL_DB_NAME);
                    if (connectSQLite())
                    {
                        string sqlInitDB = File.ReadAllText(Config.SQL_DATA_PATH + "\\Resources\\initDB.sql");
                        ExecuteSqlNonResult(sqlInitDB);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<MUser> GetALlUsers()
        {
            List<MUser> lstUser = new List<MUser>();

            try
            {
                string sql = "SELECT * FROM M_USERS";
                var result = SQLiteCommon.ExecuteSqlWithResult(sql);

                if (result != null && result.HasRows)
                {
                    while (result.Read())
                    {
                        MUser user = new MUser();
                        user.UserId = result["USERID"]?.ToString();
                        user.FullName = result["FULLNAME"]?.ToString();
                        user.CreateBy = result["CREATEBY"]?.ToString();
                        user.Email = result["EMAIL"]?.ToString();
                        user.Mobile = result["MOBILE"]?.ToString();
                        user.Password = result["PASSWORD"]?.ToString();
                        user.Role = short.Parse(result["ROLE"]?.ToString());
                        user.RoleDisplay = user.Role == 0 ? "ADMIN" : "USERS";
                        user.CreateDate = Convert.ToDateTime(result["CREATEDATE"]?.ToString());

                        lstUser.Add(user);
                    }
                }

                return lstUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<MCustomer> GetALlCustomers()
        {
            List<MCustomer> lstCustomer = new List<MCustomer>();

            try
            {
                string sql = "SELECT * FROM M_CUSTOMERS";
                var result = SQLiteCommon.ExecuteSqlWithResult(sql);

                if (result != null && result.HasRows)
                {
                    while (result.Read())
                    {
                        MCustomer customer = new MCustomer();
                        customer.ID = long.Parse(result["ID"]?.ToString());
                        customer.CompanyName = result["COMPANYNAME"]?.ToString();
                        customer.CompanyAddress = result["COMPANYADDRESS"]?.ToString();
                        customer.CompanyMobile = result["COMPANYMOBILE"]?.ToString();
                        customer.CusName = result["CUSNAME"]?.ToString();
                        customer.CusEmail = result["CUSEMAIL"]?.ToString();
                        customer.CusMobile = result["CUSMOBILE"]?.ToString();
                        customer.CreateDate = Convert.ToDateTime(result["CREATEDATE"]?.ToString());

                        lstCustomer.Add(customer);
                    }
                }

                return lstCustomer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<MDevice> GetALlDevices()
        {
            List<MDevice> lstDevice = new List<MDevice>();

            try
            {
                string sql = "SELECT * FROM M_DEVICES";
                var result = SQLiteCommon.ExecuteSqlWithResult(sql);

                if (result != null && result.HasRows)
                {
                    while (result.Read())
                    {
                        MDevice device = new MDevice();
                        device.ID = long.Parse(result["ID"]?.ToString());
                        device.DeviceName = result["DEVICENAME"]?.ToString();
                        device.MacAddress = result["MACADDRESS"]?.ToString();
                        device.UserId = result["USERID"]?.ToString();  
                        device.CreateDate = Convert.ToDateTime(result["CREATEDATE"]?.ToString());

                        lstDevice.Add(device);
                    }
                }

                return lstDevice;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CustomerKey> GetALlCustomerKeys()
        {
            List<CustomerKey> lstCusKeys = new List<CustomerKey>();

            try
            {
                string sql = "SELECT * FROM CUSTOMERKEYS";
                var result = SQLiteCommon.ExecuteSqlWithResult(sql);

                if (result != null && result.HasRows)
                {
                    while (result.Read())
                    {
                        CustomerKey cusKey = new CustomerKey();
                        cusKey.CusId = long.Parse(result["CUSID"]?.ToString());
                        cusKey.KeyCode = result["KEYCODE"]?.ToString();
                        cusKey.UserId = result["USERID"]?.ToString();
                        cusKey.CreateDate = Convert.ToDateTime(result["CREATEDATE"]?.ToString());

                        lstCusKeys.Add(cusKey);
                    }
                }

                return lstCusKeys;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CustomerKey GetCustomerKeyByKeyCode(string keyCode)
        {
            CustomerKey cusKey = new CustomerKey();
            List<string> listMacs = new List<string>();
            listMacs.AddRange(new string[] { "", "", "", "", "" });

            try
            {
                string sql = string.Format("SELECT * FROM KEYDEVICES WHERE KEYCODE='{0}'", keyCode);
                var result = SQLiteCommon.ExecuteSqlWithResult(sql);

                if (result != null && result.HasRows)
                {
                    int i = 0;
                    while (result.Read())
                    {
                        cusKey.MachineCode = result["MACHINECODE"]?.ToString();
                        cusKey.KeyCode = result["KEYCODE"]?.ToString();
                        var macAddress = result["MACADDRESS"]?.ToString();
                        listMacs[i] = macAddress;
                        i++;
                    }
                }

                cusKey.ListMacAddress = listMacs;

                return cusKey;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CustomerKey> GetALlKeysByCusId(long customerId)
        {
            List<CustomerKey> lstCusKeys = new List<CustomerKey>();

            try
            {
                string sql = string.Format("SELECT * FROM CUSTOMERKEYS INNER JOIN KEYDEVICES ON CUSTOMERKEYS.KEYCODE = KEYDEVICES.KEYCODE WHERE CUSID= {0} ORDER BY CREATEDATE DESC", customerId);
                var result = SQLiteCommon.ExecuteSqlWithResult(sql);

                if (result != null && result.HasRows)
                {
                    while (result.Read())
                    {
                        CustomerKey cusKey = new CustomerKey();
                        cusKey.CusId = long.Parse(result["CUSID"]?.ToString());
                        cusKey.KeyCode = result["KEYCODE"]?.ToString();
                        cusKey.UserId = result["USERID"]?.ToString();
                        cusKey.MachineCode = result["MACHINECODE"]?.ToString();
                        cusKey.MacAddress = result["MACADDRESS"]?.ToString();
                        cusKey.CreateDate = Convert.ToDateTime(result["CREATEDATE"]?.ToString());

                        lstCusKeys.Add(cusKey);
                    }
                }

                return lstCusKeys;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static List<KeyDevice> GetALlKeyDevices()
        {
            List<KeyDevice> lstKeyDevice = new List<KeyDevice>();

            try
            {
                string sql = "SELECT * FROM KEYDEVICES";
                var result = SQLiteCommon.ExecuteSqlWithResult(sql);

                if (result != null && result.HasRows)
                {
                    while (result.Read())
                    {
                        KeyDevice keyDevice = new KeyDevice();
                        keyDevice.KeyCode = result["KEYCODE"]?.ToString();
                        keyDevice.MachineCode = result["MACHINECODE"]?.ToString();
                        keyDevice.MacAddress = result["MACADDRESS"]?.ToString();
                        keyDevice.UserId = result["USERID"]?.ToString();
                        keyDevice.CreateDate = Convert.ToDateTime(result["CREATEDATE"]?.ToString());

                        lstKeyDevice.Add(keyDevice);
                    }
                }

                return lstKeyDevice;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CloseDB()
        {
            try
            {
                dbConnection?.Close();
                dbConnection = null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static SQLiteDataReader ExecuteSqlWithResult(string sql)
        {
            using (SQLiteCommand command = new SQLiteCommand(sql, dbConnection))
            {
                try
                {
                    SQLiteDataReader result = command.ExecuteReader();

                    return result;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public static void ExecuteSqlNonResult(string sql)
        {
            using (SQLiteCommand command = new SQLiteCommand(sql, dbConnection, dbTransaction))
            {
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private static bool _useSqlTran = false;

        public static void BeginTran()
        {
            if (dbTransaction == null)
            {
                dbTransaction = dbConnection.BeginTransaction();
                _useSqlTran = true;
            }
        }

        public static void CommitTran()
        {
            if (_useSqlTran)
            {
                dbTransaction.Commit();
                dbTransaction.Dispose();
                dbTransaction = null;
                _useSqlTran = false;
            }
        }

        public static void RollBackTran()
        {
            if (_useSqlTran)
            {
                dbTransaction.Rollback();
                dbTransaction.Dispose();
                dbTransaction = null;
                _useSqlTran = false;
            }
        }

        private static bool connectSQLite()
        {
            bool isConnected = false;

            try
            {
                dbConnection = new SQLiteConnection(string.Format(@"Data Source={0};Version=3;Password={1};", Config.SQL_DB_NAME, Config.SQL_DB_PASSWORD));
                dbConnection.Open();

                isConnected = true;
            }
            catch (Exception)
            {

                throw;
            }

            return isConnected;
        }
    }
}
