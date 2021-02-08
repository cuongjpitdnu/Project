using BaseCommon;
using BaseCommon.Utility;
using DSF602.Language;
using KeyCipher;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace DSF602
{
    public class clsCommon
    {
        private const string KEY_REGISTRY_PATH = "DSF602\\v2";

        public static string AddVersionApp(string title)
        {
            var version = LanguageHelper.GetValueOf("APP_VERSION") + " " + AppManager.AppVersion;
            return string.Format("{0} {1} ({2})", title, version, AppManager.AppCreateDate.ToString("dd/MM/yyyy"));
        }

        public static string MeasureResultDisplay(int measureResult)
        {
            switch (measureResult)
            {
                //case (int)clsConst.emMeasureResult.Normal:
                //    return "Normal";
                //case (int)clsConst.emMeasureResult.Alarm:
                //    return "Alarm";
                case (int)clsConst.emMeasureResult.Pass:
                    return LanguageHelper.GetValueOf("MEASURE_RESULT_PASS");
                case (int)clsConst.emMeasureResult.Fail:
                    return LanguageHelper.GetValueOf("MEASURE_RESULT_FAIL");
            }

            return string.Empty;
        }

        public static bool IsNumberAndThanZero(string input)
        {
            var intInput = ConvertHelper.CnvNullToInt(input);
            return intInput > 0;
        }

        //Cover string to Base64Encode
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        #region Dialog

        public static bool ShowMsg(MessageBoxIcon type, string strMsg, string strTitle = "")
        {
            MessageBox.Show(LanguageHelper.GetValueOf(strMsg), strTitle, MessageBoxButtons.OK, type);

            if (type == MessageBoxIcon.Error || type == MessageBoxIcon.Warning)
            {
                return false;
            }

            return true;
        }

        public static bool ComfirmMsg(string strMsg, string strTitle = "")
        {
            return MessageBox.Show(LanguageHelper.GetValueOf(strMsg), strTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK;
        }

        public static bool ComfirmMsgErr(string strMsg, string strTitle = "")
        {
            return MessageBox.Show(LanguageHelper.GetValueOf(strMsg), strTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }

        public static SaveFileDialog SaveExcelDialog(string defaultFileName = "")
        {
            return new SaveFileDialog
            {
                DefaultExt = "xlsx",
                Filter = "Excel Workbook (*.xls, *.xlsx)|*.xls;*.xlsx",
                AddExtension = true,
                RestoreDirectory = true,
                Title = "Save as",
                FileName = defaultFileName,
                InitialDirectory = Application.StartupPath,
            };
        }

        #endregion Dialog

        public static string GetMachineCode()
        {
            var processorId = getMachineInfo("Select * from Win32_processor", "processorID");
            var machineMacAddress = getMachineMacAddress();
            var motherBoardID = getMachineInfo("Select * from Win32_BaseBoard", "SerialNumber");
            return string.Join("", processorId, machineMacAddress, motherBoardID);
        }

        private static string getMachineInfo(string query, string returnType)
        {
            string result = "0123456789";

            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                foreach (ManagementObject item in searcher.Get())
                {
                    result = item.GetPropertyValue(returnType).ToString();
                }
            }
            catch (Exception)
            {

            }

            return result;
        }

        private static string getMachineMacAddress()
        {
            try
            {
                string firstMacAddress = NetworkInterface.GetAllNetworkInterfaces()
                                                         .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                                                         .Select(nic => nic.GetPhysicalAddress().ToString())
                                                         .FirstOrDefault();

                return firstMacAddress;
            }
            catch (Exception)
            {
            }

            return "0123456789";
        }

        public static bool ValidateMachineAndDevices( string keyEncripted = "")
        {
            try
            {
                if (string.IsNullOrEmpty(keyEncripted)) return false;

                var machineCode = clsCommon.GetMachineCode();
                var keyDecripted = Cipher.DecryptText(keyEncripted);

                if (machineCode != keyDecripted)
                {
                    return false;
                }

                clsCommon.writeKeyToRegistry(keyEncripted);

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void writeKeyToRegistry(string param)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(KEY_REGISTRY_PATH, true))
                {
                    if (key != null)
                    {
                        key.SetValue("KEY", param);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static string readKeyFromRegistry()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(KEY_REGISTRY_PATH))
                {
                    if (key != null)
                    {
                        Object o = key.GetValue("KEY");
                        if (o != null)
                        {
                            return o.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return "";
        }

        public static string FormatValue(int value, int pading = 3)
        {
            if (value == 0) return "0";
            var tmpVal = Math.Abs(value);

            var strVal = tmpVal.ToString();
            strVal = strVal.Length > pading ? strVal : strVal.PadLeft(pading, '0');
            strVal = value > 0 ? "+" + strVal : "-" + strVal;
            return strVal;
        }
    }
}
