using KeyCipher;
using Microsoft.Win32;
using NativeWifi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BaseCommon
{
    public partial class clsCommon
    {
        public const string BASESECUREXMLPROFILE = "<?xml version=\"1.0\"?><WLANProfile xmlns=\"http://www.microsoft.com/networking/WLAN/profile/v1\"><name>{0}</name><SSIDConfig><SSID><hex>{1}</hex><name>{2}</name></SSID>{3}</SSIDConfig><connectionType>ESS</connectionType><connectionMode>auto</connectionMode><autoSwitch>false</autoSwitch><MSM><security><authEncryption><authentication>{4}</authentication><encryption>{5}</encryption><useOneX>false</useOneX></authEncryption><sharedKey><keyType>{6}</keyType><protected>false</protected><keyMaterial>{7}</keyMaterial></sharedKey><keyIndex>0</keyIndex></security></MSM></WLANProfile>";
        public const string BASEOPENXMLPROFILE = "<?xml version=\"1.0\"?><WLANProfile xmlns=\"http://www.microsoft.com/networking/WLAN/profile/v1\"><name>{0}</name><SSIDConfig><SSID><hex>{1}</hex><name>{2}</name></SSID>{3}</SSIDConfig><connectionType>ESS</connectionType><connectionMode>auto</connectionMode><autoSwitch>false</autoSwitch><MSM><security><authEncryption><authentication>{4}</authentication><encryption>{5}</encryption><useOneX>false</useOneX></authEncryption>{6}{7}</security></MSM></WLANProfile>";
        public const string NONBROADCASTTAG = "<nonBroadcast>true</nonBroadcast>";
        private const string KEY_REGISTRY_PATH = "DSF601\\v2";

        public static string GetStringForSSID(Wlan.Dot11Ssid ssid)
        {
            return Encoding.ASCII.GetString(ssid.SSID, 0, (int)ssid.SSIDLength);
        }

        public static string ConvertToHex(string data)
        {
            byte[] byteArr = System.Text.ASCIIEncoding.ASCII.GetBytes(data);
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                result.Append(byteArr[i].ToString("x"));
            }
            return result.ToString();
        }

        public static void ConnectWifi(string username, string password)
        {
            var result = new StringBuilder();
            WlanClient client = new WlanClient();

            try
            {
                //int count = 1;
                foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
                {
                    // Lists all networks with WEP security
                    Wlan.WlanAvailableNetwork[] networks = wlanIface.GetAvailableNetworkList(0);
                    foreach (Wlan.WlanAvailableNetwork network in networks)
                    {
                        string auth = "", cipher = "", keytype = "", key = password, profileName = "", xml = "", hiddenssidtag = "";

                        if (GetStringForSSID(network.dot11Ssid).Contains(username))
                        {
                            bool hiddentag = false;
                            #region dot11DefaultAuthAlgorithm
                            switch (network.dot11DefaultAuthAlgorithm)
                            {
                                case Wlan.Dot11AuthAlgorithm.IEEE80211_Open:
                                    auth = "open";
                                    break;
                                case Wlan.Dot11AuthAlgorithm.IEEE80211_SharedKey:
                                    break;
                                case Wlan.Dot11AuthAlgorithm.RSNA:
                                    auth = "WPA2PSK";
                                    break;
                                case Wlan.Dot11AuthAlgorithm.RSNA_PSK:
                                    auth = "WPA2PSK";
                                    break;
                                case Wlan.Dot11AuthAlgorithm.WPA:
                                    auth = "WPAPSK";
                                    break;
                                case Wlan.Dot11AuthAlgorithm.WPA_None:
                                    auth = "WPAPSK";
                                    break;
                                case Wlan.Dot11AuthAlgorithm.WPA_PSK:
                                    auth = "WPAPSK";
                                    break;
                            }
                            #endregion

                            #region cipher, keytype
                            switch (network.dot11DefaultCipherAlgorithm)
                            {
                                case Wlan.Dot11CipherAlgorithm.CCMP:
                                    cipher = "AES";
                                    keytype = "passPhrase";
                                    break;
                                case Wlan.Dot11CipherAlgorithm.TKIP:
                                    cipher = "TKIP";
                                    keytype = "passPhrase";
                                    break;
                                case Wlan.Dot11CipherAlgorithm.None:
                                    cipher = "none";
                                    keytype = "";
                                    key = "";
                                    break;
                                case Wlan.Dot11CipherAlgorithm.WEP:
                                    cipher = "WEP";
                                    keytype = "networkKey";
                                    break;
                                case Wlan.Dot11CipherAlgorithm.WEP40:
                                    cipher = "WEP";
                                    keytype = "networkKey";
                                    break;
                                case Wlan.Dot11CipherAlgorithm.WEP104:
                                    cipher = "WEP";
                                    keytype = "networkKey";
                                    break;
                            }
                            #endregion

                            if (GetStringForSSID(network.dot11Ssid) == "")
                            {
                                hiddentag = true;
                                hiddenssidtag = NONBROADCASTTAG;
                            }

                            profileName = GetStringForSSID(network.dot11Ssid);
                            if (cipher == "none")
                                xml = BASEOPENXMLPROFILE;
                            else
                                xml = BASESECUREXMLPROFILE;

                            string ssidhex = ConvertToHex(profileName);

                            string profileXml = String.Format(xml, profileName, ssidhex, profileName, hiddenssidtag, auth, cipher, keytype, key);
                            wlanIface.SetProfile(Wlan.WlanProfileFlags.AllUser, profileXml, true);
                            wlanIface.Connect(Wlan.WlanConnectionMode.Profile, Wlan.Dot11BssType.Any, profileName);

                            break;
                        }
                        //txtResult.Text += String.Format("\nFound WEP network with SSID {0}.", GetStringForSSID(network.dot11Ssid)) + "\n";
                    }

                    // Retrieves XML configurations of existing profiles.
                    // This can assist you in constructing your own XML configuration
                    // (that is, it will give you an example to follow).
                    foreach (Wlan.WlanProfileInfo profileInfo in wlanIface.GetProfiles())
                    {
                        string name = profileInfo.profileName; // this is typically the network's SSID
                        string xml = wlanIface.GetProfileXml(profileInfo.profileName);
                    }

                    // Connects to a known network with WEP security
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static bool TableIsNullOrEmpty(DataTable tbl)
        {
            return tbl == null || tbl.Rows.Count == 0;
        }

        public static void TableDispose(ref DataTable tbl)
        {
            if (tbl != null)
            {
                tbl.Dispose();
                tbl = null;
            }
        }

        public static string CnvNullToString(object obj, string defaultValue = "")
        {
            string rst = defaultValue;

            if (obj != null)
            {
                rst = obj.ToString();
            }

            return rst;
        }

        public static int CnvNullToInt(object obj, int defaultValue = -1)
        {
            int rst = defaultValue;
            string temp = CnvNullToString(obj);

            if (!int.TryParse(temp, out rst))
            {
                rst = defaultValue;
            }

            return rst;
        }

        public static long CnvNullToLong(object obj, long defaultValue = -1)
        {
            long rst = defaultValue;
            string temp = CnvNullToString(obj);

            if (!long.TryParse(temp, out rst))
            {
                rst = defaultValue;
            }

            return rst;
        }

        public static double CnvNullToDouble(object obj, int defaultValue = -1)
        {
            double rst = defaultValue;
            string temp = CnvNullToString(obj);

            if (!double.TryParse(temp, out rst))
            {
                rst = defaultValue;
            }

            return rst;
        }

        public static DateTime CnvStringToDateTime(string str, string fomat = "yyyy/MM/dd HH:mm:ss")
        {
            DateTime dtRst;
            DateTime.TryParseExact(str, fomat, null, DateTimeStyles.None, out dtRst);
            return dtRst;
        }

        public static DateTime? CnvStringToDateTimeNull(object obj, DateTime? defaultValue = null, string fomat = "")
        {
            if (obj == null || obj == DBNull.Value)
            {
                return defaultValue;
            }
            try
            {
                if (string.IsNullOrEmpty(fomat))
                {
                    return (DateTime)obj;
                }

                DateTime rst;

                if (!DateTime.TryParseExact(clsCommon.CnvNullToString(obj), fomat, null, DateTimeStyles.None, out rst))
                {
                    return defaultValue;
                }

                return rst;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static bool IsNumberAndThanZero(string input)
        {
            var intInput = CnvNullToInt(input);
            return intInput > 0;
        }

        public static string MeasureResultDisplay(int measureResult)
        {
            switch (measureResult)
            {
                case (int)clsConst.emMeasureResult.Normal:
                    return "Normal";
                case (int)clsConst.emMeasureResult.Alarm:
                    return "Alarm";
                case (int)clsConst.emMeasureResult.Pass:
                    return "Pass";
                case (int)clsConst.emMeasureResult.Fail:
                    return "Fail";
            }

            return string.Empty;
        }

        public static bool CreateFolderHidden(string strPath, bool deleteBefore = false)
        {
            if (Directory.Exists(strPath))
            {
                if (!deleteBefore)
                {
                    return true;
                }

                var objDirInfo = new DirectoryInfo(strPath);
                SetFolderAttr(objDirInfo, FileAttributes.Normal);
                objDirInfo.Delete(true);
                objDirInfo = null;
            }

            Directory.CreateDirectory(strPath);
            SetFolderAttr(new DirectoryInfo(strPath), FileAttributes.Hidden);

            return true;
        }

        public static bool SetFolderAttr(DirectoryInfo objDir, FileAttributes emAttr)
        {
            try
            {
                if (objDir.Exists)
                {

                    // set this folder's attribute
                    objDir.Attributes = emAttr;

                    // set file's attribute
                    foreach (FileInfo objFile in objDir.GetFiles())
                    {
                        objFile.Attributes = emAttr;
                    }

                    // set folder's attribute
                    foreach (DirectoryInfo objFolder in objDir.GetDirectories())
                    {
                        SetFolderAttr(objFolder, emAttr);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string[] GetListSubFolder(string strPath)
        {
            try
            {
                if (Directory.Exists(strPath))
                {
                    return Directory.GetDirectories(strPath);
                }
            }
            catch
            {

            }

            return null;
        }

        /// <summary>
        /// Get the machine code.
        /// </summary>
        /// <returns></returns>
        public static string GetMachineCode()
        {
            var processorId = getMachineInfo("Select * from Win32_processor", "processorID");
            var machineMacAddress = getMachineMacAddress();
            var motherBoardID = getMachineInfo("Select * from Win32_BaseBoard", "SerialNumber");
            return string.Join("", processorId, machineMacAddress, motherBoardID);
        }

        /// <summary>
        /// Gets the machine mac address.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Pings the specified list ip.
        /// </summary>
        /// <param name="lstIp">The LST ip.</param>
        public static void Ping(List<string> lstIp)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = "ping.exe ";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;

                foreach(var ip in lstIp)
                {
                    if (!string.IsNullOrEmpty(ip))
                    {
                        process.StartInfo.Arguments = ip;
                        process.Start();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Gets the list MacIpPair connnect network.
        /// </summary>
        /// <returns></returns>
        public static List<MacIpPair> GetLstDevice()
        {
            List<MacIpPair> lstMacDevice = new List<MacIpPair>();

            try
            {
                System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
                pProcess.StartInfo.FileName = "arp";
                pProcess.StartInfo.Arguments = "-a ";
                pProcess.StartInfo.UseShellExecute = false;
                pProcess.StartInfo.RedirectStandardOutput = true;
                pProcess.StartInfo.CreateNoWindow = true;
                pProcess.Start();

                string cmdOutput = pProcess.StandardOutput.ReadToEnd();
                string pattern = @"(?<ip>([0-9]{1,3}\.?){4})\s*(?<mac>([a-f0-9]{2}-?){6})";

                foreach (Match m in Regex.Matches(cmdOutput, pattern, RegexOptions.IgnoreCase))
                {
                    MacIpPair macIpPair = new MacIpPair();
                    macIpPair.IpAddress = m.Groups["ip"].Value;
                    macIpPair.MacAddress = m.Groups["mac"].Value.Replace("-", ":").ToUpper();
                    lstMacDevice.Add(macIpPair);
                }
            }
            catch (Exception)
            {
            }

            return lstMacDevice;
        }

        /// <summary>
        /// Gets the machine information.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="returnType">Type of the return.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Validates the machine and devices.
        /// </summary>
        /// <param name="lstDeviceInfo">The LST device information.</param>
        /// <param name="keyEncripted">The key encripted.</param>
        /// <returns></returns>
        public static bool ValidateMachineAndDevices(List<DeviceInfo> lstDeviceInfo, string keyEncripted = "")
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
                //var arrStr = keyDecripted.Split(clsConst.KEY_CHAR_SPLIT_SERVER).ToList();
                //var arrMachineInfo = arrStr[0].Split(clsConst.KEY_CHAR_SPLIT_CLIENT).ToList();
                //if (machineCode == arrMachineInfo[0])
                //{
                //    var lstKeyMacServer = arrStr; lstKeyMacServer.RemoveAt(0);

                //    if (validateLstDevice(lstKeyMacServer, lstDeviceInfo))
                //    {
                //        clsCommon.writeKeyToRegistry(keyEncripted);
                //        return true;
                //    }
                //    else
                //    {
                //        return false;
                //    }
                //}
                //else
                //{
                //    return false;
                //}
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Validates the LST device.
        /// </summary>
        /// <param name="lstKeyMacServer">The LST key mac server.</param>
        /// <param name="lstDeviceInfo">The LST device information.</param>
        /// <returns></returns>
        private static bool validateLstDevice(List<string> lstKeyMacServer, List<DeviceInfo> lstDeviceInfo)
        {
            lstKeyMacServer = lstKeyMacServer.Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
            var lstMacDevice = lstDeviceInfo.Select(i => i.MacAddress).ToList();
            lstMacDevice = lstMacDevice.Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();

            var res = lstMacDevice.Except(lstKeyMacServer).ToList();

            if (res == null || res.Count == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Writes the key to registry.
        /// </summary>
        /// <param name="param">The parameter.</param>
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

        /// <summary>
        /// Reads the key from registry.
        /// </summary>
        /// <returns></returns>
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
    }
}
