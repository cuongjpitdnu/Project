using BaseCommon.Core;
using DSF602Driver.Modbus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DFS02_ToolSetIP
{
    public partial class ManagerInternetPotocol : BaseForm
    {
        private const string IP_DEFAULT = "192.168.0.1";
        private const string IP_CHANGE_DEFAULT = "192.168.0.";
        public ManagerInternetPotocol()
        {
            InitializeComponent();
        }

        private void ManagerInternetPotocol_Load(object sender, EventArgs e)
        {
            NetworkInterface[] Interfaces = NetworkInterface.GetAllNetworkInterfaces();
            var lstNetwork = Interfaces.Where(x => x.OperationalStatus == OperationalStatus.Up && x.NetworkInterfaceType != NetworkInterfaceType.Loopback).Select(x => x.Name).ToList();

            if (lstNetwork == null || lstNetwork.Count() == 0)
            {
                return;
            }

            foreach (var item in lstNetwork)
            {
                cboNetwork.Items.Add(item);
            }
            cboNetwork.SelectedIndex = 0;
        }

        private void btnDefaul_Click(object sender, EventArgs e)
        {
            var ipConfig = new IPConfig()
            {
                IpAddress = "192.168.0.100",
                Subnet = "255.255.255.0",
                Gateway = "192.168.0.1",
                DNS = "8.8.8.8",
                IsDhcpEnabled = false,
                IsDnsEnabled = false
            };

            NetworkManagement.setIP(ipConfig.IpAddress, ipConfig.Subnet, ipConfig.IsDhcpEnabled);
            NetworkManagement.setGateway(ipConfig.Gateway, ipConfig.IsDhcpEnabled);
            NetworkManagement.setDNS(NetworkManagement._NICName, ipConfig.DNS, ipConfig.IsDnsEnabled);

            txtIpAddress.Text = ipConfig.IpAddress;
            txtSubMask.Text = ipConfig.Subnet;
            txtGateway.Text = ipConfig.Gateway;
            txtDNS.Text = ipConfig.DNS;

            txtCurrentIp.Text = IP_DEFAULT;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            NetworkManagement.setIP(NetworkManagement._ipConfig.IpAddress, NetworkManagement._ipConfig.Subnet, NetworkManagement._ipConfig.IsDhcpEnabled);

            NetworkManagement.setGateway(NetworkManagement._ipConfig.Gateway, NetworkManagement._ipConfig.IsDhcpEnabled);

            NetworkManagement.setDNS(NetworkManagement._NICName, NetworkManagement._ipConfig.DNS, NetworkManagement._ipConfig.IsDnsEnabled);

            cboNetwork_SelectedIndexChanged(null, null);
        }

        public bool ValidateIPv4(string ipString)
        {
            if (String.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            byte tempForParsing;

            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }

        public IPAddress GetDefaultGateway()
        {
            return NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up)
                .Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .SelectMany(n => n.GetIPProperties()?.GatewayAddresses)
                .Select(g => g?.Address)
                .Where(a => a != null)
                // .Where(a => a.AddressFamily == AddressFamily.InterNetwork)
                // .Where(a => Array.FindIndex(a.GetAddressBytes(), b => b != 0) >= 0)
                .FirstOrDefault();
        }

        private void txtIpAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.Split('.').Length - 1 >= 3))
            {
                e.Handled = true;
            }
        }

        public static void DisplayIPv4NetworkInterfaces()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            Console.WriteLine("IPv4 interface information for {0}.{1}",
               properties.HostName, properties.DomainName);
            Console.WriteLine();

            foreach (NetworkInterface adapter in nics)
            {
                // Only display informatin for interfaces that support IPv4.
                if (adapter.Supports(NetworkInterfaceComponent.IPv4) == false)
                {
                    continue;
                }
                Console.WriteLine(adapter.Description);
                // Underline the description.
                Console.WriteLine(String.Empty.PadLeft(adapter.Description.Length, '='));
                IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                // Try to get the IPv4 interface properties.
                IPv4InterfaceProperties p = adapterProperties.GetIPv4Properties();

                if (p == null)
                {
                    Console.WriteLine("No IPv4 information is available for this interface.");
                    Console.WriteLine();
                    continue;
                }
                // Display the IPv4 specific data.
                Console.WriteLine("  Index ............................. : {0}", p.Index);
                Console.WriteLine("  MTU ............................... : {0}", p.Mtu);
                Console.WriteLine("  APIPA active....................... : {0}",
                    p.IsAutomaticPrivateAddressingActive);
                Console.WriteLine("  APIPA enabled...................... : {0}",
                    p.IsAutomaticPrivateAddressingEnabled);
                Console.WriteLine("  Forwarding enabled................. : {0}",
                    p.IsForwardingEnabled);
                Console.WriteLine("  Uses WINS ......................... : {0}",
                    p.UsesWins);
                Console.WriteLine("  DHPC enable ......................... : {0}",
                p.IsDhcpEnabled);
                Console.WriteLine();
            }
        }

        private void cboNetwork_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboNetwork.SelectedItem == null)
            {
                return;
            }

            var obj = NetworkManagement.IPv4NetworkInterfaces(cboNetwork.SelectedItem.ToString());

            if (obj == null)
            {
                return;
            }

            txtIpAddress.Text = obj.IpAddress;
            txtSubMask.Text = obj.Subnet;
            txtGateway.Text = obj.Gateway;
            txtDNS.Text = obj.DNS;

            btnDefaul.Focus();

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                IModbus objModbus = new ModbusTCP(txtCurrentIp.Text);
                objModbus.Start();

                ShowMsg(MessageBoxIcon.Information, "Connection Success!");
            }
            catch (Exception ex)
            {
                ShowMsg(MessageBoxIcon.Error, "Connection Fail!");
            }     
        }
    }
}
