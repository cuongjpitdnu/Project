// Modbustool.TopForm
using iptb;
using Modbustool;
using Modbustool.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO.Ports;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

public class TopForm : Form
{
    private Sensor SensorInstance;

    private IContainer components;

    private Button RTUbutton;

    private Label label8;

    private ComboBox ComSelect;

    private Button TCPButton;

    private GroupBox groupBox1;

    private ComboBox paritySelect;

    private Label label3;

    private ComboBox stopSelect;

    private Label label2;

    private Label label1;

    private ComboBox baudSelect;

    private GroupBox groupBox2;

    private Label label6;

    private Label label7;

    private Label label4;

    private ComboBox addressSelect;

    private TextBox PortNoTextBox;

    private IPTextBox ipTextBox1;

    public TopForm()
    {
        InitializeComponent();
        SensorInstance = new StaticElectricityObject();
    }

    private void TopForm_Load(object sender, EventArgs e)
    {
        ComSelect.Items.Clear();
        int num = 0;
        try
        {
            using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select * from Win32_PnPEntity Where ClassGuid = '{4D36E978-E325-11CE-BFC1-08002BE10318}' And Name Like '%(COM%)'"))
            {
                ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
                foreach (ManagementObject item in managementObjectCollection)
                {
                    string text = (string)item.GetPropertyValue("Caption");
                    if (text.IndexOf("(COM") >= 0)
                    {
                        int startIndex = text.IndexOf("(COM");
                        string text2 = text.Substring(startIndex);
                        text2 = text2.Replace("(COM", "");
                        text2 = text2.Replace(")", "");
                        text2 = "COM" + text2;
                        num = Math.Max(num, TextRenderer.MeasureText("COM255 ", ComSelect.Font).Width + TextRenderer.MeasureText(text, ComSelect.Font).Width);
                        ComSelect.Items.Add(text2 + " " + text);
                    }
                }
            }
            num += 8;
            if (ComSelect.DropDownWidth < num)
            {
                ComSelect.DropDownWidth = num;
            }
            string comPort = Settings.Default.ComPort;
            if (comPort == "")
            {
                Settings.Default.Upgrade();
                comPort = Settings.Default.ComPort;
            }
            ComSelect.SelectedIndex = 0;
            if (comPort != "")
            {
                for (int i = 0; i < ComSelect.Items.Count; i++)
                {
                    if (ComSelect.Items[i].ToString().StartsWith(comPort))
                    {
                        ComSelect.SelectedIndex = i;
                        break;
                    }
                }
            }
            string address = Settings.Default.Address;
            if (address == "")
            {
                Settings.Default.Upgrade();
                address = Settings.Default.Address;
            }
            addressSelect.SelectedIndex = 0;
            if (address != "")
            {
                for (int j = 0; j < addressSelect.Items.Count; j++)
                {
                    if (addressSelect.Items[j].ToString().StartsWith(address))
                    {
                        addressSelect.SelectedIndex = j;
                        break;
                    }
                }
            }
            string baud = Settings.Default.Baud;
            if (baud == "")
            {
                Settings.Default.Upgrade();
                baud = Settings.Default.Baud;
            }
            baudSelect.SelectedIndex = 0;
            if (baud != "")
            {
                for (int k = 0; k < baudSelect.Items.Count; k++)
                {
                    if (baudSelect.Items[k].ToString().StartsWith(baud))
                    {
                        baudSelect.SelectedIndex = k;
                        break;
                    }
                }
            }
            string stopBit = Settings.Default.StopBit;
            if (stopBit == "")
            {
                Settings.Default.Upgrade();
                stopBit = Settings.Default.StopBit;
            }
            stopSelect.SelectedIndex = 0;
            if (stopBit != "")
            {
                for (int l = 0; l < stopSelect.Items.Count; l++)
                {
                    if (stopSelect.Items[l].ToString().StartsWith(stopBit))
                    {
                        stopSelect.SelectedIndex = l;
                        break;
                    }
                }
            }
            string parity = Settings.Default.Parity;
            if (parity == "")
            {
                Settings.Default.Upgrade();
                parity = Settings.Default.Parity;
            }
            paritySelect.SelectedIndex = 0;
            if (parity != "")
            {
                for (int m = 0; m < paritySelect.Items.Count; m++)
                {
                    if (paritySelect.Items[m].ToString().StartsWith(parity))
                    {
                        paritySelect.SelectedIndex = m;
                        break;
                    }
                }
            }
            ipTextBox1.Text = Settings.Default.IP;
            PortNoTextBox.Text = Settings.Default.PortNo;
        }
        catch
        {
        }
    }

    private string GetComString()
    {
        string text = ComSelect.SelectedItem.ToString();
        string[] array = text.Split(' ');
        return array[0];
    }

    private void RTUbutton_Click(object sender, EventArgs e)
    {
        string comString = GetComString();
        int devno = int.Parse(addressSelect.Text);
        int baudRate = int.Parse(baudSelect.Text);
        StopBits stopBits = (stopSelect.Text == "1 stop") ? StopBits.One : ((!(stopSelect.Text == "2 stop")) ? StopBits.One : StopBits.Two);
        Parity parity = (!(paritySelect.Text == "None")) ? ((paritySelect.Text == "Odd") ? Parity.Odd : ((!(paritySelect.Text == "Even")) ? Parity.Even : Parity.Even)) : Parity.None;
        try
        {
            using (SerialPort sp = new SerialPort(comString, baudRate, parity, 8, stopBits))
            {
                using (MeasureForm measureForm = new MeasureForm(devno, sp, null, SensorInstance))
                {
                    measureForm.ShowInTaskbar = false;
                    measureForm.StartPosition = FormStartPosition.CenterParent;
                    measureForm.ShowDialog(this);
                }
            }
            Settings.Default.ComPort = comString;
            Settings.Default.Address = addressSelect.Text;
            Settings.Default.StopBit = stopSelect.Text;
            Settings.Default.Parity = paritySelect.Text;
            Settings.Default.Baud = baudSelect.Text;
            Settings.Default.Save();
        }
        catch
        {
        }
    }

    private void TCPButton_Click(object sender, EventArgs e)
    {
        try
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(ipTextBox1.Text), int.Parse(PortNoTextBox.Text));
            using (TcpClient tcpClient = new TcpClient())
            {
                tcpClient.Connect(remoteEP);
                using (MeasureForm measureForm = new MeasureForm(0, null, tcpClient, SensorInstance))
                {
                    measureForm.ShowInTaskbar = false;
                    measureForm.StartPosition = FormStartPosition.CenterParent;
                    measureForm.ShowDialog(this);
                }
            }
            Settings.Default.IP = ipTextBox1.Text;
            Settings.Default.PortNo = PortNoTextBox.Text;
            Settings.Default.Save();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        RTUbutton = new System.Windows.Forms.Button();
        label8 = new System.Windows.Forms.Label();
        ComSelect = new System.Windows.Forms.ComboBox();
        TCPButton = new System.Windows.Forms.Button();
        groupBox1 = new System.Windows.Forms.GroupBox();
        label4 = new System.Windows.Forms.Label();
        addressSelect = new System.Windows.Forms.ComboBox();
        paritySelect = new System.Windows.Forms.ComboBox();
        label3 = new System.Windows.Forms.Label();
        stopSelect = new System.Windows.Forms.ComboBox();
        label2 = new System.Windows.Forms.Label();
        label1 = new System.Windows.Forms.Label();
        baudSelect = new System.Windows.Forms.ComboBox();
        groupBox2 = new System.Windows.Forms.GroupBox();
        PortNoTextBox = new System.Windows.Forms.TextBox();
        ipTextBox1 = new iptb.IPTextBox();
        label6 = new System.Windows.Forms.Label();
        label7 = new System.Windows.Forms.Label();
        groupBox1.SuspendLayout();
        groupBox2.SuspendLayout();
        SuspendLayout();
        RTUbutton.Location = new System.Drawing.Point(12, 183);
        RTUbutton.Name = "RTUbutton";
        RTUbutton.Size = new System.Drawing.Size(85, 23);
        RTUbutton.TabIndex = 0;
        RTUbutton.Text = "RTU";
        RTUbutton.UseVisualStyleBackColor = true;
        RTUbutton.Click += new System.EventHandler(RTUbutton_Click);
        label8.AutoSize = true;
        label8.Location = new System.Drawing.Point(6, 26);
        label8.Name = "label8";
        label8.Size = new System.Drawing.Size(30, 12);
        label8.TabIndex = 14;
        label8.Text = "COM";
        ComSelect.FormattingEnabled = true;
        ComSelect.Location = new System.Drawing.Point(59, 23);
        ComSelect.Name = "ComSelect";
        ComSelect.Size = new System.Drawing.Size(121, 20);
        ComSelect.Sorted = true;
        ComSelect.TabIndex = 13;
        TCPButton.Location = new System.Drawing.Point(226, 183);
        TCPButton.Name = "TCPButton";
        TCPButton.Size = new System.Drawing.Size(85, 23);
        TCPButton.TabIndex = 16;
        TCPButton.Text = "TCP";
        TCPButton.UseVisualStyleBackColor = true;
        TCPButton.Click += new System.EventHandler(TCPButton_Click);
        groupBox1.Controls.Add(label4);
        groupBox1.Controls.Add(addressSelect);
        groupBox1.Controls.Add(paritySelect);
        groupBox1.Controls.Add(label3);
        groupBox1.Controls.Add(stopSelect);
        groupBox1.Controls.Add(label2);
        groupBox1.Controls.Add(label1);
        groupBox1.Controls.Add(baudSelect);
        groupBox1.Controls.Add(ComSelect);
        groupBox1.Controls.Add(label8);
        groupBox1.Location = new System.Drawing.Point(11, 12);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new System.Drawing.Size(209, 165);
        groupBox1.TabIndex = 17;
        groupBox1.TabStop = false;
        groupBox1.Text = "Serial";
        label4.AutoSize = true;
        label4.Location = new System.Drawing.Point(6, 134);
        label4.Name = "label4";
        label4.Size = new System.Drawing.Size(47, 12);
        label4.TabIndex = 22;
        label4.Text = "Address";
        addressSelect.FormattingEnabled = true;
        addressSelect.Items.AddRange(new object[9]
        {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"
        });
        addressSelect.Location = new System.Drawing.Point(59, 131);
        addressSelect.Name = "addressSelect";
        addressSelect.Size = new System.Drawing.Size(121, 20);
        addressSelect.TabIndex = 21;
        paritySelect.FormattingEnabled = true;
        paritySelect.Items.AddRange(new object[3]
        {
            "None",
            "Odd",
            "Even"
        });
        paritySelect.Location = new System.Drawing.Point(59, 105);
        paritySelect.Name = "paritySelect";
        paritySelect.Size = new System.Drawing.Size(121, 20);
        paritySelect.TabIndex = 20;
        label3.AutoSize = true;
        label3.Location = new System.Drawing.Point(6, 108);
        label3.Name = "label3";
        label3.Size = new System.Drawing.Size(34, 12);
        label3.TabIndex = 19;
        label3.Text = "parity";
        stopSelect.FormattingEnabled = true;
        stopSelect.Items.AddRange(new object[2]
        {
            "1 stop",
            "2 stop"
        });
        stopSelect.Location = new System.Drawing.Point(59, 77);
        stopSelect.Name = "stopSelect";
        stopSelect.Size = new System.Drawing.Size(121, 20);
        stopSelect.TabIndex = 18;
        label2.AutoSize = true;
        label2.Location = new System.Drawing.Point(6, 80);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(27, 12);
        label2.TabIndex = 17;
        label2.Text = "stop";
        label1.AutoSize = true;
        label1.Location = new System.Drawing.Point(7, 52);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(29, 12);
        label1.TabIndex = 16;
        label1.Text = "baud";
        baudSelect.FormattingEnabled = true;
        baudSelect.Items.AddRange(new object[5]
        {
            "4800",
            "9600",
            "19200",
            "38400",
            "57600"
        });
        baudSelect.Location = new System.Drawing.Point(59, 49);
        baudSelect.Name = "baudSelect";
        baudSelect.Size = new System.Drawing.Size(121, 20);
        baudSelect.TabIndex = 15;
        groupBox2.Controls.Add(PortNoTextBox);
        groupBox2.Controls.Add(ipTextBox1);
        groupBox2.Controls.Add(label6);
        groupBox2.Controls.Add(label7);
        groupBox2.Location = new System.Drawing.Point(226, 12);
        groupBox2.Name = "groupBox2";
        groupBox2.Size = new System.Drawing.Size(212, 165);
        groupBox2.TabIndex = 18;
        groupBox2.TabStop = false;
        groupBox2.Text = "TCP";
        PortNoTextBox.Location = new System.Drawing.Point(55, 47);
        PortNoTextBox.Name = "PortNoTextBox";
        PortNoTextBox.Size = new System.Drawing.Size(59, 19);
        PortNoTextBox.TabIndex = 22;
        PortNoTextBox.Text = "502";
        ipTextBox1.Location = new System.Drawing.Point(54, 23);
        ipTextBox1.Name = "ipTextBox1";
        ipTextBox1.Size = new System.Drawing.Size(128, 18);
        ipTextBox1.TabIndex = 21;
        ipTextBox1.ToolTipText = "";
        label6.AutoSize = true;
        label6.Location = new System.Drawing.Point(7, 52);
        label6.Name = "label6";
        label6.Size = new System.Drawing.Size(42, 12);
        label6.TabIndex = 16;
        label6.Text = "PortNo.";
        label7.AutoSize = true;
        label7.Location = new System.Drawing.Point(6, 26);
        label7.Name = "label7";
        label7.Size = new System.Drawing.Size(15, 12);
        label7.TabIndex = 14;
        label7.Text = "IP";
        base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
        base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        base.ClientSize = new System.Drawing.Size(452, 220);
        base.Controls.Add(groupBox2);
        base.Controls.Add(groupBox1);
        base.Controls.Add(TCPButton);
        base.Controls.Add(RTUbutton);
        base.Name = "TopForm";
        base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Text = "Modbus test tool";
        base.Load += new System.EventHandler(TopForm_Load);
        groupBox1.ResumeLayout(performLayout: false);
        groupBox1.PerformLayout();
        groupBox2.ResumeLayout(performLayout: false);
        groupBox2.PerformLayout();
        ResumeLayout(performLayout: false);
    }
}