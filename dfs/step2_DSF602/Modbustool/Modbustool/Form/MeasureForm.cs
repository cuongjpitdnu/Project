// Modbustool.MeasureForm
using Modbus;
using Modbus.Device;
using Modbustool;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

public class MeasureForm : Form
{
    private SerialPort Sp;

    private TcpClient Tc;

    private ModbusMaster Master;

    private int devno;

    private Sensor TargetSensor;

    private FileStream fs;

    private StreamWriter sw;

    private IContainer components;

    private DataGridView dataGridView1;

    private Timer timer1;

    private Button SettingButton;

    private Button ExecButton;

    private Button UpdateButton;

    private CheckBox CyclicCheckBox;

    private CheckBox LoggingCheckBox;

    private TextBox FileTextBox;

    private Button FileSelectButton;

    private Label label1;

    private Label label2;

    private NumericUpDown CyclicPeriod;

    private Button DescriteButton;

    private Button CoilsButton;

    private Button UptimeButton;

    private Button RawButton;

    public MeasureForm(int _devno, SerialPort _sp, TcpClient _tc, Sensor _ts)
    {
        InitializeComponent();
        SettingButton.Click += SettingButton_Click;
        ExecButton.Click += ExecButton_Click;
        UpdateButton.Click += UpdateButton_Click;
        CoilsButton.Click += CoilsButton_Click;
        DescriteButton.Click += DescriteButton_Click;
        UptimeButton.Click += UptimeButton_Click;
        RawButton.Click += RawButton_Click;
        CyclicPeriod.Minimum = 100m;
        CyclicPeriod.Maximum = 10000m;
        CyclicPeriod.Value = 1000m;
        fs = null;
        sw = null;
        DataGridView dataGridView = dataGridView1;
        dataGridView.AutoSize = true;
        dataGridView.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
        dataGridView.RowHeadersVisible = false;
        dataGridView.AllowUserToAddRows = false;
        dataGridView.MultiSelect = false;
        dataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
        dataGridView.ColumnCount = 2;
        dataGridView.Columns[0].HeaderText = "Item";
        dataGridView.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
        dataGridView.Columns[1].HeaderText = "Value";
        dataGridView.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
        TargetSensor = _ts;
        TargetSensor.SensorComponent(dataGridView);
        devno = _devno;
        if (_sp != null)
        {
            Tc = null;
            Sp = _sp;
            Sp.Open();
            Master = ModbusSerialMaster.CreateRtu(_sp);
        }
        else
        {
            Sp = null;
            Tc = _tc;
            Master = ModbusIpMaster.CreateIp(Tc);
        }
        Master.Transport.ReadTimeout = 300;
        dataGridView.CurrentCell = dataGridView[1, 0];
        dataGridView.AllowUserToResizeRows = false;
    }

    private void MeasureForm_Load(object sender, EventArgs e)
    {
    }

    private void MeasureForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        try
        {
            try
            {
                if (fs != null)
                {
                    sw.Close();
                    fs.Close();
                    sw = null;
                    fs = null;
                }
            }
            catch
            {
            }
            Master.Dispose();
            if (Sp != null)
            {
                Sp.Close();
            }
            else
            {
                Tc.Close();
            }
        }
        catch
        {
        }
        base.Owner.Show();
    }

    private void SettingButton_Click(object sender, EventArgs e)
    {
        try
        {
            using (SettingForm settingForm = new SettingForm(Master, devno, TargetSensor))
            {
                settingForm.ShowInTaskbar = false;
                settingForm.StartPosition = FormStartPosition.CenterParent;
                settingForm.ShowDialog(this);
            }
        }
        catch
        {
        }
    }

    private void ExecButton_Click(object sender, EventArgs e)
    {
        try
        {
            using (ExecForm execForm = new ExecForm(Master, devno, TargetSensor))
            {
                execForm.ShowInTaskbar = false;
                execForm.StartPosition = FormStartPosition.CenterParent;
                execForm.ShowDialog(this);
            }
        }
        catch
        {
        }
    }

    private async void UpdateButton_Click(object sender, EventArgs e)
    {
        UpdateButton.Enabled = false;
        try
        {
            int len = 0;
            foreach (typeBase sampleMap in TargetSensor.SampleMaps)
            {
                len += sampleMap.Len;
            }
            byte[] frame = Utils.FrameToByte(await Master.ReadInputRegistersAsync((byte)devno, (ushort)TargetSensor.SamplingAddress, (ushort)(len / 2)));
            int idx = 0;
            int i = 0;
            foreach (typeBase sampleMap2 in TargetSensor.SampleMaps)
            {
                sampleMap2.FrameTo(frame, idx);
                idx += sampleMap2.Len;
                dataGridView1[1, i].Value = sampleMap2.ToString();
                i++;
            }
            if (CyclicCheckBox.Checked && FileTextBox.Text != "" && LoggingCheckBox.Checked)
            {
                try
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(DateTime.Now.ToString("HH:mm:ss"));
                    foreach (typeBase sampleMap3 in TargetSensor.SampleMaps)
                    {
                        stringBuilder.Append(",");
                        stringBuilder.Append(sampleMap3.ToString());
                    }
                    StoreWrite(stringBuilder.ToString());
                }
                catch
                {
                }
            }
        }
        catch (TimeoutException)
        {
        }
        catch (SlaveException)
        {
        }
        catch (Exception)
        {
        }
        if (!CyclicCheckBox.Checked)
        {
            UpdateButton.Enabled = true;
        }
    }

    private void CyclicCheckBox_Click(object sender, EventArgs e)
    {
        if (CyclicCheckBox.Checked)
        {
            timer1.Stop();
            timer1.Interval = decimal.ToInt32(CyclicPeriod.Value);
            timer1.Start();
            UpdateButton.Enabled = false;
            DescriteButton.Enabled = false;
            CoilsButton.Enabled = false;
            SettingButton.Enabled = false;
            ExecButton.Enabled = false;
            UptimeButton.Enabled = false;
            RawButton.Enabled = false;
        }
        else
        {
            timer1.Stop();
            UpdateButton.Enabled = true;
            DescriteButton.Enabled = true;
            CoilsButton.Enabled = true;
            SettingButton.Enabled = true;
            ExecButton.Enabled = true;
            UptimeButton.Enabled = true;
            RawButton.Enabled = true;
        }
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
        UpdateButton_Click(null, null);
        if (!CyclicCheckBox.Checked)
        {
            DescriteButton.Enabled = true;
            CoilsButton.Enabled = true;
            SettingButton.Enabled = true;
            ExecButton.Enabled = true;
            UptimeButton.Enabled = true;
            RawButton.Enabled = true;
        }
    }

    private void FileSelectButton_Click(object sender, EventArgs e)
    {
        using (SaveFileDialog saveFileDialog = new SaveFileDialog())
        {
            saveFileDialog.Filter = "csv|*.csv|Texe|*.txt";
            saveFileDialog.CheckFileExists = false;
            saveFileDialog.DefaultExt = "csv";
            saveFileDialog.FileName = FileTextBox.Text;
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.Title = "Cycle log file";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileTextBox.Text = saveFileDialog.FileName;
            }
        }
    }

    private void StoreWrite(string record)
    {
        if (fs == null)
        {
            try
            {
                fs = new FileStream(FileTextBox.Text, FileMode.Append, FileAccess.Write, FileShare.Read);
                sw = new StreamWriter(fs);
                if (fs.Length <= 0)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append("Time");
                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        stringBuilder.Append(",");
                        stringBuilder.Append(dataGridView1[0, i].Value.ToString());
                    }
                    sw.WriteLine(stringBuilder.ToString());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        try
        {
            sw.WriteLine(record);
            sw.Flush();
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void LoggingCheckBox_Click(object sender, EventArgs e)
    {
        try
        {
            if (fs != null && !LoggingCheckBox.Checked)
            {
                sw.Close();
                fs.Close();
                sw = null;
                fs = null;
            }
        }
        catch
        {
        }
    }

    private void CoilsButton_Click(object sender, EventArgs e)
    {
        try
        {
            using (CoilsForm coilsForm = new CoilsForm(Master, devno, TargetSensor))
            {
                coilsForm.ShowInTaskbar = false;
                coilsForm.StartPosition = FormStartPosition.CenterParent;
                coilsForm.ShowDialog(this);
            }
        }
        catch
        {
        }
    }

    private void DescriteButton_Click(object sender, EventArgs e)
    {
        try
        {
            using (DescriteForm descriteForm = new DescriteForm(Master, devno, TargetSensor))
            {
                descriteForm.ShowInTaskbar = false;
                descriteForm.StartPosition = FormStartPosition.CenterParent;
                descriteForm.ShowDialog(this);
            }
        }
        catch
        {
        }
    }

    private void UptimeButton_Click(object sender, EventArgs e)
    {
        try
        {
            using (VersionForm versionForm = new VersionForm(Master, devno, TargetSensor))
            {
                versionForm.ShowInTaskbar = false;
                versionForm.StartPosition = FormStartPosition.CenterParent;
                versionForm.ShowDialog(this);
            }
        }
        catch
        {
        }
    }

    private void RawButton_Click(object sender, EventArgs e)
    {
        try
        {
            using (DebugForm debugForm = new DebugForm(Master, devno, TargetSensor))
            {
                debugForm.ShowInTaskbar = false;
                debugForm.StartPosition = FormStartPosition.CenterParent;
                debugForm.ShowDialog(this);
            }
        }
        catch
        {
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
        components = new System.ComponentModel.Container();
        dataGridView1 = new System.Windows.Forms.DataGridView();
        timer1 = new System.Windows.Forms.Timer(components);
        SettingButton = new System.Windows.Forms.Button();
        ExecButton = new System.Windows.Forms.Button();
        UpdateButton = new System.Windows.Forms.Button();
        CyclicCheckBox = new System.Windows.Forms.CheckBox();
        LoggingCheckBox = new System.Windows.Forms.CheckBox();
        FileTextBox = new System.Windows.Forms.TextBox();
        FileSelectButton = new System.Windows.Forms.Button();
        label1 = new System.Windows.Forms.Label();
        label2 = new System.Windows.Forms.Label();
        CyclicPeriod = new System.Windows.Forms.NumericUpDown();
        DescriteButton = new System.Windows.Forms.Button();
        CoilsButton = new System.Windows.Forms.Button();
        UptimeButton = new System.Windows.Forms.Button();
        RawButton = new System.Windows.Forms.Button();
        ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
        ((System.ComponentModel.ISupportInitialize)CyclicPeriod).BeginInit();
        SuspendLayout();
        dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridView1.Location = new System.Drawing.Point(12, 12);
        dataGridView1.Name = "dataGridView1";
        dataGridView1.RowTemplate.Height = 21;
        dataGridView1.Size = new System.Drawing.Size(294, 305);
        dataGridView1.TabIndex = 0;
        timer1.Tick += new System.EventHandler(timer1_Tick);
        SettingButton.Location = new System.Drawing.Point(345, 128);
        SettingButton.Name = "SettingButton";
        SettingButton.Size = new System.Drawing.Size(75, 23);
        SettingButton.TabIndex = 1;
        SettingButton.Text = "Settings";
        SettingButton.UseVisualStyleBackColor = true;
        ExecButton.Location = new System.Drawing.Point(345, 157);
        ExecButton.Name = "ExecButton";
        ExecButton.Size = new System.Drawing.Size(75, 23);
        ExecButton.TabIndex = 2;
        ExecButton.Text = "Exec";
        ExecButton.UseVisualStyleBackColor = true;
        UpdateButton.Location = new System.Drawing.Point(345, 212);
        UpdateButton.Name = "UpdateButton";
        UpdateButton.Size = new System.Drawing.Size(75, 23);
        UpdateButton.TabIndex = 3;
        UpdateButton.Text = "Update";
        UpdateButton.UseVisualStyleBackColor = true;
        CyclicCheckBox.AutoSize = true;
        CyclicCheckBox.Location = new System.Drawing.Point(345, 241);
        CyclicCheckBox.Name = "CyclicCheckBox";
        CyclicCheckBox.Size = new System.Drawing.Size(56, 16);
        CyclicCheckBox.TabIndex = 4;
        CyclicCheckBox.Text = "Cyclic";
        CyclicCheckBox.UseVisualStyleBackColor = true;
        CyclicCheckBox.Click += new System.EventHandler(CyclicCheckBox_Click);
        LoggingCheckBox.AutoSize = true;
        LoggingCheckBox.Location = new System.Drawing.Point(345, 289);
        LoggingCheckBox.Name = "LoggingCheckBox";
        LoggingCheckBox.Size = new System.Drawing.Size(63, 16);
        LoggingCheckBox.TabIndex = 6;
        LoggingCheckBox.Text = "Logging";
        LoggingCheckBox.UseVisualStyleBackColor = true;
        LoggingCheckBox.Click += new System.EventHandler(LoggingCheckBox_Click);
        FileTextBox.Location = new System.Drawing.Point(360, 308);
        FileTextBox.Name = "FileTextBox";
        FileTextBox.Size = new System.Drawing.Size(129, 19);
        FileTextBox.TabIndex = 7;
        FileSelectButton.Location = new System.Drawing.Point(495, 308);
        FileSelectButton.Name = "FileSelectButton";
        FileSelectButton.Size = new System.Drawing.Size(31, 23);
        FileSelectButton.TabIndex = 8;
        FileSelectButton.Text = "･･･";
        FileSelectButton.UseVisualStyleBackColor = true;
        FileSelectButton.Click += new System.EventHandler(FileSelectButton_Click);
        label1.AutoSize = true;
        label1.Location = new System.Drawing.Point(330, 266);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(37, 12);
        label1.TabIndex = 9;
        label1.Text = "Period";
        label2.AutoSize = true;
        label2.Location = new System.Drawing.Point(330, 308);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(24, 12);
        label2.TabIndex = 10;
        label2.Text = "File";
        CyclicPeriod.Location = new System.Drawing.Point(374, 264);
        CyclicPeriod.Name = "CyclicPeriod";
        CyclicPeriod.Size = new System.Drawing.Size(46, 19);
        CyclicPeriod.TabIndex = 11;
        DescriteButton.Location = new System.Drawing.Point(345, 41);
        DescriteButton.Name = "DescriteButton";
        DescriteButton.Size = new System.Drawing.Size(75, 23);
        DescriteButton.TabIndex = 12;
        DescriteButton.Text = "Descrite";
        DescriteButton.UseVisualStyleBackColor = true;
        CoilsButton.Location = new System.Drawing.Point(345, 12);
        CoilsButton.Name = "CoilsButton";
        CoilsButton.Size = new System.Drawing.Size(75, 23);
        CoilsButton.TabIndex = 13;
        CoilsButton.Text = "Coils";
        CoilsButton.UseVisualStyleBackColor = true;
        UptimeButton.Location = new System.Drawing.Point(345, 70);
        UptimeButton.Name = "UptimeButton";
        UptimeButton.Size = new System.Drawing.Size(75, 23);
        UptimeButton.TabIndex = 14;
        UptimeButton.Text = "Uptime";
        UptimeButton.UseVisualStyleBackColor = true;
        RawButton.Location = new System.Drawing.Point(345, 99);
        RawButton.Name = "RawButton";
        RawButton.Size = new System.Drawing.Size(75, 23);
        RawButton.TabIndex = 15;
        RawButton.Text = "RawData";
        RawButton.UseVisualStyleBackColor = true;
        base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
        base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        base.ClientSize = new System.Drawing.Size(538, 421);
        base.Controls.Add(RawButton);
        base.Controls.Add(UptimeButton);
        base.Controls.Add(CoilsButton);
        base.Controls.Add(DescriteButton);
        base.Controls.Add(CyclicPeriod);
        base.Controls.Add(label2);
        base.Controls.Add(label1);
        base.Controls.Add(FileSelectButton);
        base.Controls.Add(FileTextBox);
        base.Controls.Add(LoggingCheckBox);
        base.Controls.Add(CyclicCheckBox);
        base.Controls.Add(UpdateButton);
        base.Controls.Add(ExecButton);
        base.Controls.Add(SettingButton);
        base.Controls.Add(dataGridView1);
        base.MaximizeBox = false;
        base.MinimizeBox = false;
        base.Name = "MeasureForm";
        Text = "Measurement";
        base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(MeasureForm_FormClosing);
        base.Load += new System.EventHandler(MeasureForm_Load);
        ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
        ((System.ComponentModel.ISupportInitialize)CyclicPeriod).EndInit();
        ResumeLayout(performLayout: false);
        PerformLayout();
    }
}
