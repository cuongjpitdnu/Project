using BaseCommon.Utility;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;

namespace DFS02_ToolSetIP
{
    public class NetworkManagement
    {
        public static IPConfig _ipConfig;
        public static string _NICName;

        public static IPConfig IPv4NetworkInterfaces()
        {
            NetworkInterface[] Interfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface Interface in Interfaces)
            {
                if (Interface.OperationalStatus != OperationalStatus.Up)
                {
                    continue;
                }

                if (Interface.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                {
                    continue;
                }

                IPInterfaceProperties adapterProperties = Interface.GetIPProperties();
                Console.WriteLine(Interface.Name);

                foreach (UnicastIPAddressInformation ip in Interface.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        continue;
                    }

                    _ipConfig = new IPConfig();
                    IPv4InterfaceProperties ipProperties = adapterProperties.GetIPv4Properties();
                    IPAddressCollection dnsServers = adapterProperties.DnsAddresses;

                    var defaultGateway = adapterProperties.GatewayAddresses.Select(i => i?.Address).Where(x => x != null).FirstOrDefault();

                    _ipConfig.IsDhcpEnabled = ipProperties.IsDhcpEnabled;
                    _ipConfig.IsDnsEnabled = DNSAutoOrStatic(Interface.Id);
                    _ipConfig.IpAddress = ip.Address.ToString();
                    _ipConfig.Subnet = ip.IPv4Mask.ToString();
                    _ipConfig.Gateway = defaultGateway.ToString();
                    if (dnsServers.Count > 0)
                    {
                        _ipConfig.DNS = dnsServers[0].ToString();
                    }
                    _ipConfig.NICName = Interface.Description;
                    _NICName = _ipConfig.NICName;
                }
            }

            return _ipConfig;
        }

        public static IPConfig IPv4NetworkInterfaces(string nameInterface)
        {
            NetworkInterface[] Interfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface Interface in Interfaces)
            {
                if (Interface.Name != nameInterface)
                {
                    continue;
                }

                if (Interface.OperationalStatus != OperationalStatus.Up)
                {
                    continue;
                }

                if (Interface.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                {
                    continue;
                }

                IPInterfaceProperties adapterProperties = Interface.GetIPProperties();
                Console.WriteLine(Interface.Name);

                foreach (UnicastIPAddressInformation ip in Interface.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        continue;
                    }

                    _ipConfig = new IPConfig();
                    IPv4InterfaceProperties ipProperties = adapterProperties.GetIPv4Properties();
                    IPAddressCollection dnsServers = adapterProperties.DnsAddresses;

                    var defaultGateway = adapterProperties.GatewayAddresses.Select(i => i?.Address).Where(x => x != null).FirstOrDefault();

                    _ipConfig.IsDhcpEnabled = ipProperties.IsDhcpEnabled;
                    _ipConfig.IsDnsEnabled = DNSAutoOrStatic(Interface.Id);
                    _ipConfig.IpAddress = ConvertHelper.CnvNullToString(ip.Address);
                    _ipConfig.Subnet = ConvertHelper.CnvNullToString(ip.IPv4Mask);
                    _ipConfig.Gateway = ConvertHelper.CnvNullToString(defaultGateway);
                    if (dnsServers.Count > 0)
                    {
                        _ipConfig.DNS = ConvertHelper.CnvNullToString(dnsServers[0]);
                    }
                    _ipConfig.NICName = Interface.Description;
                    _NICName = _ipConfig.NICName;
                }
            }

            return _ipConfig;
        }

        public static void setIP(string ip_address, string subnet_mask, bool dhcp)
        {
            ManagementClass objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection objMOC = objMC.GetInstances();

            foreach (ManagementObject objMO in objMOC)
            {
                if ((bool)objMO["IPEnabled"])
                {
                    try
                    {
                        ManagementBaseObject setIP;

                        if (dhcp)
                        {
                            setIP = objMO.InvokeMethod("EnableDHCP", null, null);
                        }
                        else
                        {
                            ManagementBaseObject newIP = objMO.GetMethodParameters("EnableStatic");

                            newIP["IPAddress"] = new string[] { ip_address };
                            newIP["SubnetMask"] = new string[] { subnet_mask };
                            setIP = objMO.InvokeMethod("EnableStatic", newIP, null);
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Error");
                    }
                }
            }
        }

        public static void setGateway(string gateway, bool dhcp)
        {
            ManagementClass objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection objMOC = objMC.GetInstances();

            foreach (ManagementObject objMO in objMOC)
            {
                if ((bool)objMO["IPEnabled"])
                {
                    try
                    {
                        ManagementBaseObject setGateway;
                        if (dhcp)
                        {
                            ManagementBaseObject newGateway = objMO.GetMethodParameters("SetGateways");

                            newGateway["DefaultIPGateway"] = null;
                            newGateway["GatewayCostMetric"] = new int[] { 1 };
                            setGateway = objMO.InvokeMethod("SetGateways", newGateway, null);
                        }
                        else
                        {
                            ManagementBaseObject newGateway = objMO.GetMethodParameters("SetGateways");

                            newGateway["DefaultIPGateway"] = new string[] { gateway };
                            newGateway["GatewayCostMetric"] = new int[] { 1 };
                            setGateway = objMO.InvokeMethod("SetGateways", newGateway, null);
                        }

                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Error");
                    }
                }
            }
        }

        public static void setDNS(string NIC, string DNS, bool dnsEnable)
        {
            ManagementClass objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection objMOC = objMC.GetInstances();

            foreach (ManagementObject objMO in objMOC)
            {
                if ((bool)objMO["IPEnabled"])
                {
                    try
                    {
                        // if you are using the System.Net.NetworkInformation.NetworkInterface
                        // you'll need to change this line to
                        // if (objMO["Caption"].ToString().Contains(NIC))
                        // and pass in the Description property instead of the name 
                        if (/*objMO["Caption"].Equals(NIC)*/ objMO["Caption"].ToString().Contains(NIC))
                        {
                            ManagementBaseObject setDNS;

                            if (dnsEnable)
                            {
                                ManagementBaseObject newDNS = objMO.GetMethodParameters("SetDNSServerSearchOrder");
                                newDNS["DNSServerSearchOrder"] = null;

                                setDNS = objMO.InvokeMethod("SetDNSServerSearchOrder", newDNS, null);
                            }
                            else
                            {
                                ManagementBaseObject newDNS = objMO.GetMethodParameters("SetDNSServerSearchOrder");
                                newDNS["DNSServerSearchOrder"] = DNS.Split(',');
                                setDNS = objMO.InvokeMethod("SetDNSServerSearchOrder", newDNS, null);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error");
                    }

                }
            }
        }

        private static bool DNSAutoOrStatic(string NetworkAdapterGUID)
        {
            string path = "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\Tcpip\\Parameters\\Interfaces\\" + NetworkAdapterGUID;
            string ns = (string)Registry.GetValue(path, "NameServer", null);

            return String.IsNullOrEmpty(ns) ? true : false;
        }

        public void setWINS(string NIC, string priWINS, string secWINS)
        {
            ManagementClass objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection objMOC = objMC.GetInstances();

            foreach (ManagementObject objMO in objMOC)
            {
                if ((bool)objMO["IPEnabled"])
                {
                    if (objMO["Caption"].Equals(NIC))
                    {
                        try
                        {
                            ManagementBaseObject setWINS;
                            ManagementBaseObject wins =
                            objMO.GetMethodParameters("SetWINSServer");
                            wins.SetPropertyValue("WINSPrimaryServer", priWINS);
                            wins.SetPropertyValue("WINSSecondaryServer", secWINS);

                            setWINS = objMO.InvokeMethod("SetWINSServer", wins, null);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Error");
                        }

                    }
                }
            }
        }

        public static ArrayList GetNICNames()
        {
            ArrayList nicNames = new ArrayList();

            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["ipEnabled"])
                {
                    nicNames.Add(mo["Caption"]);
                }
            }

            return nicNames;
        }

        public static void setIPDynamic(string ip_address, string subnet_mask)
        {
            ManagementClass objMC =
              new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection objMOC = objMC.GetInstances();

            foreach (ManagementObject objMO in objMOC)
            {
                if ((bool)objMO["IPEnabled"])
                {
                    try
                    {
                        ManagementBaseObject setIP;
                        ManagementBaseObject newIP =
                          objMO.GetMethodParameters("EnableStatic");

                        newIP["IPAddress"] = new string[] { ip_address };
                        newIP["SubnetMask"] = new string[] { subnet_mask };

                        setIP = objMO.InvokeMethod("EnableDHCP", null, null);

                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Error");
                    }

                }
            }
        }

    }

    public class IPConfig
    {
        public string IpAddress { get; set; }
        public string Subnet { get; set; }
        public string Gateway { get; set; }
        public string DNS { get; set; }
        public bool IsDhcpEnabled { get; set; }
        public bool IsDnsEnabled { get; set; }
        public string NICName { get; set; }
    }
}
