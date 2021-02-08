using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ToolUpdate.frmUpdate;

namespace ToolUpdate
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                MessageBox.Show("File transfer error", "Error", MessageBoxButtons.OK);
                return;
            }
            var dataMsg = JsonConvert.DeserializeObject<MsgData>(Base64Decode(args[0]));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmUpdate(dataMsg));
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }

    public class MsgData
    {
        public string KeyCheck { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }
    }
}
