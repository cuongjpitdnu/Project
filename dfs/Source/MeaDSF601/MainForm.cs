using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GraphLib;
using JCS;


namespace MeaDSF601
{
    public partial class MainForm : Form
    {
        TelnetInterfaceDsf telnetDSF;

        public MainForm()
        {
            InitializeComponent();
            telnetDSF = null;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // no smaller than design time size
            this.MinimumSize = new System.Drawing.Size(this.Width, this.Height);

            // no larger than screen size
            this.MaximumSize = new System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            //this.AutoSize = true;
            //this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //this.Dock = DockStyle.Fill;

            panel1.Anchor = AnchorStyles.Right;
            

            JCS.ToggleSwitch ModernStyleToggleSwitch;

            ModernStyleToggleSwitch = new JCS.ToggleSwitch();
            ModernStyleToggleSwitch.Location = new System.Drawing.Point(626, 188);
            ModernStyleToggleSwitch.Name = "ModernStyleToggleSwitch";
            ModernStyleToggleSwitch.OffFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ModernStyleToggleSwitch.OnFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ModernStyleToggleSwitch.Size = new System.Drawing.Size(50, 19);
            ModernStyleToggleSwitch.TabIndex = 14;

            Controls.Add(ModernStyleToggleSwitch);
            pdeGraph.SetPlayPanelInvisible();
            GraphDraw();
            UpdateColorSchemaMenu();
        }

        private void UpdateColorSchemaMenu()
        {
            Color[] cols = { Color.FromArgb(0,255,0),
                                         Color.FromArgb(0,255,0),
                                         Color.FromArgb(0,255,0),
                                         Color.FromArgb(0,255,0),
                                         Color.FromArgb(0,255,0) ,
                                         Color.FromArgb(0,255,0),
                                         Color.FromArgb(0,255,0) };

            for (int j = 0; j < pdeGraph.DataSources.Count; j++)
            {
                pdeGraph.DataSources[j].GraphColor = cols[j % 7];
            }

            pdeGraph.BackgroundColorTop = Color.FromArgb(0, 64, 0);
            pdeGraph.BackgroundColorBot = Color.FromArgb(0, 64, 0);
            pdeGraph.SolidGridColor = Color.FromArgb(0, 128, 0);
            pdeGraph.DashedGridColor = Color.FromArgb(0, 128, 0);
        }

        private String RenderXLabel(DataSource s, int idx)
        {
            if (s.AutoScaleX)
            {
                //if (idx % 2 == 0)
                {
                    int Value = (int)(s.Samples[idx].x);
                    return "" + Value;
                }                
            }
            else
            {
                int Value = (int)(s.Samples[idx].x / 200);
                String Label = "" + Value + "\"";
                return Label;
            }
            
        }

        private String RenderYLabel(DataSource s, float value)
        {
            return String.Format("{0:0.0}", value);
        }

        private void GraphDraw()
        {
            this.SuspendLayout();

            pdeGraph.DataSources.Clear();
            pdeGraph.SetDisplayRangeX(0, 400);

            int j = 0;

            pdeGraph.DataSources.Add(new DataSource());
            pdeGraph.DataSources[j].Name = "Graph " + (j + 1);
            pdeGraph.DataSources[j].OnRenderXAxisLabel += RenderXLabel;

            this.Text = "Normal Graph";
            //pdeGraph.DataSources[j].Length = 5800;
            pdeGraph.PanelLayout = PlotterGraphPaneEx.LayoutMode.NORMAL;
            pdeGraph.DataSources[j].AutoScaleY = false;
            pdeGraph.DataSources[j].SetDisplayRangeY(-300, 300);
            pdeGraph.DataSources[j].SetGridDistanceY(100);
            pdeGraph.DataSources[j].OnRenderYAxisLabel = RenderYLabel;
            CalcSinusFunction_0(pdeGraph.DataSources[j], j);

        }

        protected void CalcSinusFunction_0(DataSource src, int idx)
        {
            for (int i = 0; i < src.Length; i++)
            {
                src.Samples[i].setX(i);
                src.Samples[i].setY((float)(((float)200 * Math.Sin((idx + 1) * (i + 1.0) * 48 / src.Length))));
            }
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            Management frmMngt;

            frmMngt = new Management();
            frmMngt.Show();

        }
            

        private void btnConnect_Click(object sender, EventArgs e)
        {
            telnetDSF = new TelnetInterfaceDsf();
            telnetDSF.OnDataReceived += new ClientHandlePacketData(client_OnDataReceived);
            telnetDSF.ConnectToServer("192.168.0.105", 10001);
            //telnetDSF = new TelnetInterfaceDsf("192.168.1.62", 10001);
        }
        
        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void SetTextCallback(string text);
        private void SetValueText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtValue.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetValueText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtValue.Text = text;
            }
        }


        //This method is called when the client has received data from the server
        void client_OnDataReceived(byte[] data, int bytesRead)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            string message = encoder.GetString(data, 0, bytesRead);
            SetValueText(message);
            Console.WriteLine("Received a message: " + message);
        }

        private void btnDisConnect_Click(object sender, EventArgs e)
        {
            if (telnetDSF != null)
            {
                if (telnetDSF.IsConnected)
                {
                    telnetDSF.Disconnect();                   
                }
                telnetDSF = null;
            }
        }

        private void btnManagement_Click(object sender, EventArgs e)
        {
            Management frmMgnt = new Management();
            frmMgnt.Show();
        }
    }
}
