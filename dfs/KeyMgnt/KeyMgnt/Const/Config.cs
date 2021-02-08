using System.Windows.Forms;

namespace KeyMgnt.Const
{
    public class Config
    {
        public static string SQL_DATA_PATH = Application.StartupPath;
        public static string SQL_DB_NAME = "KeyMgnt.sqlite";
        public static string SQL_DB_PASSWORD = "@kb1564ltt";

        public static string DB_ADMIN_USERNAME = "admin";
        public static string DB_ADMIN_PASSWORD = "admin";

        public static short ROLE_ADMIN = 0;
        public static short ROLE_USERS = 1;

    }
}
