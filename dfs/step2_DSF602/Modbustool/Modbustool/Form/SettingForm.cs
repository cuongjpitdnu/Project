// Modbustool.SettingForm
using Modbus;
using Modbus.Device;
using Modbustool;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

public class SettingForm : Form
{
    private Sensor TargetSensor;

    private ModbusMaster Master;

    private int devno;

    private IContainer components;

    private SplitContainer splitContainer1;

    private TabControl tabControl1;

    private Button WriteButton;

    private Button ReadButton;

    private Button allReadButton;

    private Button allWriteButton;

    private Button LoadRegsbutton;

    private Button SaveRegsbutton;

    public SettingForm(ModbusMaster _master, int _devno, Sensor _ts)
    {
        InitializeComponent();
        Master = _master;
        TargetSensor = _ts;
        devno = _devno;
        ReadButton.Click += ReadButton_Click;
        WriteButton.Click += WriteButton_Click;
        tabControl1.SuspendLayout();
        tabControl1.SizeMode = TabSizeMode.Normal;
        foreach (ModbusParam regMap in TargetSensor.RegMaps)
        {
            TabPage tabPage = new TabPage(regMap.BlockTitle);
            tabPage.SuspendLayout();
            DataGridView dataGridView = new DataGridView();
            dataGridView.DataError += DataGridView_DataError;
            dataGridView.AutoSize = true;
            dataGridView.ReadOnly = false;
            dataGridView.MultiSelect = false;
            dataGridView.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.RowHeadersVisible = false;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.ColumnCount = 2;
            dataGridView.Columns[0].HeaderText = "Item";
            dataGridView.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView.Columns[1].HeaderText = "Value";
            dataGridView.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView.RowCount = regMap.Params.Count;
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                dataGridView[0, i].Value = regMap.Params[i].Title.ToString();
                dataGridView[0, i].ReadOnly = true;
                if (regMap.Params[i].Type == ParameterType.TYPE_SELECT)
                {
                    dataGridView[1, i] = new DataGridViewComboBoxCell();
                    string[] array = regMap.Params[i].SelectItem();
                    string[] array2 = array;
                    foreach (string item in array2)
                    {
                        ((DataGridViewComboBoxCell)dataGridView[1, i]).Items.Add(item);
                    }
                }
                if (regMap.Params[i].Ratio == 0)
                {
                    dataGridView[1, i].ReadOnly = true;
                }
            }
            dataGridView.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView.CurrentCell = dataGridView[1, 0];
            for (int k = 0; k < dataGridView.Columns.Count; k++)
            {
                for (int l = 0; l < dataGridView.Rows.Count; l++)
                {
                    if (dataGridView[k, l].ReadOnly)
                    {
                        dataGridView[k, l].Style.BackColor = Color.FromKnownColor(KnownColor.Control);
                    }
                }
            }
            tabPage.Controls.Add(dataGridView);
            tabPage.AutoScroll = true;
            tabControl1.Controls.Add(tabPage);
            tabPage.ResumeLayout();
        }
        tabControl1.ResumeLayout();
    }

    private void SettingForm_Load(object sender, EventArgs e)
    {
    }

    private void SettingForm_FormClosing(object sender, FormClosingEventArgs e)
    {
    }

    private async void ReadButton_Click(object sender, EventArgs e)
    {
        UIEnabled(bFlg: false);
        try
        {
            int tab_idx = tabControl1.SelectedIndex;
            ModbusParam p = TargetSensor.RegMaps[tab_idx];
            byte[] frame = Utils.FrameToByte(await Master.ReadHoldingRegistersAsync((byte)devno, (ushort)p.RegisterAddress, (ushort)(p.ParameterLength / 2)));
            int frame_idx = 0;
            foreach (typeBase param in p.Params)
            {
                param.FrameTo(frame, frame_idx);
                frame_idx += param.Len;
            }
            TabPage pg = tabControl1.TabPages[tab_idx];
            DataGridView mdgv = (DataGridView)pg.Controls[0];
            int item_idx = 0;
            foreach (typeBase param2 in p.Params)
            {
                mdgv[1, item_idx].Value = param2.ToString();
                item_idx++;
            }
        }
        catch (SlaveException)
        {
            MessageBox.Show("Device error response.");
        }
        catch (Exception ex2)
        {
            if (ex2.Message != null)
            {
                MessageBox.Show(ex2.Message);
            }
            else
            {
                MessageBox.Show("Error");
            }
        }
        UIEnabled(bFlg: true);
    }

    private async void WriteButton_Click(object sender, EventArgs e)
    {
        UIEnabled(bFlg: false);
        int item_idx = 0;
        try
        {
            int tab_idx = tabControl1.SelectedIndex;
            ModbusParam p = TargetSensor.RegMaps[tab_idx];
            TabPage pg = tabControl1.TabPages[tab_idx];
            DataGridView mdgv = (DataGridView)pg.Controls[0];
            byte[] pdu = new byte[p.ParameterLength];
            int frame_idx = 0;
            foreach (typeBase param in p.Params)
            {
                string text = mdgv[1, item_idx].Value.ToString();
                if (text == null || text == "")
                {
                    throw new Exception(mdgv[0, item_idx].Value.ToString() + " parameter empty");
                }
                if (!param.ToValue(text))
                {
                    throw new Exception(mdgv[0, item_idx].Value.ToString() + " parameter error");
                }
                byte[] src = param.ToFrame();
                Buffer.BlockCopy(src, 0, pdu, frame_idx, param.Len);
                frame_idx += param.Len;
                item_idx++;
            }
            await Master.WriteMultipleRegistersAsync((byte)devno, (ushort)p.RegisterAddress, Utils.ByteToFrame(pdu));
        }
        catch (SlaveException ex)
        {
            MessageBox.Show("Device error response.");
        }
        catch (Exception ex2)
        {
            if (ex2.Message != null)
            {
                MessageBox.Show(ex2.Message);
            }
            else
            {
                MessageBox.Show("Parameter error.");
            }
        }
        UIEnabled(bFlg: true);
    }

    private async void allReadButton_Click_1(object sender, EventArgs e)
    {
        UIEnabled(bFlg: false);
        try
        {
            int tab_idx = 0;
            foreach (ModbusParam p in TargetSensor.RegMaps)
            {
                byte[] frame = Utils.FrameToByte(await Master.ReadHoldingRegistersAsync((byte)devno, (ushort)p.RegisterAddress, (ushort)(p.ParameterLength / 2)));
                int frame_idx = 0;
                foreach (typeBase param in p.Params)
                {
                    param.FrameTo(frame, frame_idx);
                    frame_idx += param.Len;
                }
                TabPage pg = tabControl1.TabPages[tab_idx];
                DataGridView mdgv = (DataGridView)pg.Controls[0];
                int item_idx = 0;
                foreach (typeBase param2 in p.Params)
                {
                    mdgv[1, item_idx].Value = param2.ToString();
                    item_idx++;
                }
                tab_idx++;
            }
        }
        catch (SlaveException)
        {
            MessageBox.Show("Device error response.");
        }
        catch (Exception ex2)
        {
            if (ex2.Message != null)
            {
                MessageBox.Show(ex2.Message);
            }
            else
            {
                MessageBox.Show("Error");
            }
        }
        UIEnabled(bFlg: true);
    }

    private async void allWriteButton_Click(object sender, EventArgs e)
    {
        allWriteButton.Enabled = false;
        try
        {
            int num = default(int);
            int num2 = num;
            try
            {
                for (int tab_idx = 0; tab_idx < tabControl1.TabPages.Count; tab_idx++)
                {
                    TabPage pg = tabControl1.TabPages[tab_idx];
                    ModbusParam p = TargetSensor.RegMaps[tab_idx];
                    DataGridView mdgv = (DataGridView)pg.Controls[0];
                    byte[] pdu = new byte[p.ParameterLength];
                    int frame_idx = 0;
                    int item_idx = 0;
                    foreach (typeBase param in p.Params)
                    {
                        string text = mdgv[1, item_idx].Value.ToString();
                        if (text == null || text == "")
                        {
                            throw new Exception(mdgv[0, item_idx].Value.ToString() + " parameter empty");
                        }
                        if (!param.ToValue(text))
                        {
                            throw new Exception(mdgv[0, item_idx].Value.ToString() + " parameter error");
                        }
                        byte[] src = param.ToFrame();
                        Buffer.BlockCopy(src, 0, pdu, frame_idx, param.Len);
                        frame_idx += param.Len;
                        item_idx++;
                    }
                    await Master.WriteMultipleRegistersAsync((byte)devno, (ushort)p.RegisterAddress, Utils.ByteToFrame(pdu));
                }
            }
            catch (SlaveException)
            {
                MessageBox.Show("Device error response.");
            }
            catch (Exception ex2)
            {
                if (ex2.Message != null)
                {
                    MessageBox.Show(ex2.Message);
                }
                else
                {
                    MessageBox.Show("Error");
                }
            }
        }
        finally
        {
            allWriteButton.Enabled = true;
        }
    }

    private void SaveRegsbutton_Click(object sender, EventArgs e)
    {
        SaveRegsbutton.Enabled = false;
        using (SaveFileDialog saveFileDialog = new SaveFileDialog())
        {
            saveFileDialog.Filter = "csv|*.csv|Text|*.txt";
            saveFileDialog.CheckFileExists = false;
            saveFileDialog.DefaultExt = "csv";
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.Title = "Save register file";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write, FileShare.Read))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(stream, Encoding.GetEncoding("Shift_JIS")))
                        {
                            StringBuilder stringBuilder = new StringBuilder();
                            stringBuilder.Append(TargetSensor.SensorName);
                            stringBuilder.Append("\r\n");
                            stringBuilder.Append("\r\n");
                            foreach (TabPage tabPage in tabControl1.TabPages)
                            {
                                stringBuilder.Append("[" + tabPage.Text.ToString() + "]\r\n");
                                DataGridView dataGridView = (DataGridView)tabPage.Controls[0];
                                for (int i = 0; i < dataGridView.RowCount; i++)
                                {
                                    stringBuilder.Append(dataGridView[0, i].Value.ToString());
                                    stringBuilder.Append("\t");
                                    stringBuilder.Append(dataGridView[1, i].Value.ToString());
                                    stringBuilder.Append("\r\n");
                                }
                                stringBuilder.Append("\r\n");
                            }
                            streamWriter.WriteLine(stringBuilder.ToString());
                        }
                    }
                }
                catch
                {
                }
            }
        }
        UIEnabled(bFlg: true);
    }

    private void LoadRegsbutton_Click(object sender, EventArgs e)
    {
        using (OpenFileDialog openFileDialog = new OpenFileDialog())
        {
            openFileDialog.Filter = "csv|*.csv|Text|*.txt|すべてのファイル(*.*)|*.*";
            openFileDialog.CheckFileExists = true;
            openFileDialog.DefaultExt = "csv";
            openFileDialog.Title = "Load register file";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                UIEnabled(bFlg: false);
                try
                {
                    using (FileStream stream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(stream, Encoding.GetEncoding("Shift_JIS")))
                        {
                            string a = streamReader.ReadLine();
                            if (a != TargetSensor.SensorName)
                            {
                                throw new Exception("Sensor type is different");
                            }
                            foreach (TabPage tabPage in tabControl1.TabPages)
                            {
                                a = streamReader.ReadLine();
                                if (a != "")
                                {
                                    throw new Exception("Format type is different");
                                }
                                a = streamReader.ReadLine();
                                string a2 = a.Substring(1, tabPage.Text.ToString().Length);
                                if (a2 != tabPage.Text.ToString())
                                {
                                    throw new Exception("Format type is different");
                                }
                                DataGridView dataGridView = (DataGridView)tabPage.Controls[0];
                                for (int i = 0; i < dataGridView.RowCount; i++)
                                {
                                    a = streamReader.ReadLine();
                                    string[] array = a.Split('\t');
                                    if (array.Length != 2)
                                    {
                                        throw new Exception("Format type is different");
                                    }
                                    if (array[0] != dataGridView[0, i].Value.ToString())
                                    {
                                        throw new Exception("Format type is different");
                                    }
                                    dataGridView[1, i].Value = array[1];
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
                UIEnabled(bFlg: true);
            }
        }
    }

    private void UIEnabled(bool bFlg)
    {
        ReadButton.Enabled = bFlg;
        WriteButton.Enabled = bFlg;
        LoadRegsbutton.Enabled = bFlg;
        SaveRegsbutton.Enabled = bFlg;
        allWriteButton.Enabled = bFlg;
        allReadButton.Enabled = bFlg;
    }

    private void DataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
        if (e.Exception != null)
        {
            ((DataGridView)sender)[e.ColumnIndex, e.RowIndex].Value = null;
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
        splitContainer1 = new System.Windows.Forms.SplitContainer();
        tabControl1 = new System.Windows.Forms.TabControl();
        allWriteButton = new System.Windows.Forms.Button();
        LoadRegsbutton = new System.Windows.Forms.Button();
        SaveRegsbutton = new System.Windows.Forms.Button();
        allReadButton = new System.Windows.Forms.Button();
        WriteButton = new System.Windows.Forms.Button();
        ReadButton = new System.Windows.Forms.Button();
        ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
        splitContainer1.Panel1.SuspendLayout();
        splitContainer1.Panel2.SuspendLayout();
        splitContainer1.SuspendLayout();
        SuspendLayout();
        splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
        splitContainer1.Location = new System.Drawing.Point(0, 0);
        splitContainer1.Name = "splitContainer1";
        splitContainer1.Panel1.Controls.Add(tabControl1);
        splitContainer1.Panel2.Controls.Add(allWriteButton);
        splitContainer1.Panel2.Controls.Add(LoadRegsbutton);
        splitContainer1.Panel2.Controls.Add(SaveRegsbutton);
        splitContainer1.Panel2.Controls.Add(allReadButton);
        splitContainer1.Panel2.Controls.Add(WriteButton);
        splitContainer1.Panel2.Controls.Add(ReadButton);
        splitContainer1.Size = new System.Drawing.Size(532, 317);
        splitContainer1.SplitterDistance = 439;
        splitContainer1.TabIndex = 3;
        tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
        tabControl1.Location = new System.Drawing.Point(0, 0);
        tabControl1.Name = "tabControl1";
        tabControl1.SelectedIndex = 0;
        tabControl1.Size = new System.Drawing.Size(439, 317);
        tabControl1.TabIndex = 2;
        allWriteButton.Location = new System.Drawing.Point(2, 112);
        allWriteButton.Name = "allWriteButton";
        allWriteButton.Size = new System.Drawing.Size(75, 23);
        allWriteButton.TabIndex = 8;
        allWriteButton.Text = "All Write";
        allWriteButton.UseVisualStyleBackColor = true;
        allWriteButton.Visible = false;
        allWriteButton.Click += new System.EventHandler(allWriteButton_Click);
        LoadRegsbutton.Location = new System.Drawing.Point(2, 183);
        LoadRegsbutton.Name = "LoadRegsbutton";
        LoadRegsbutton.Size = new System.Drawing.Size(75, 23);
        LoadRegsbutton.TabIndex = 7;
        LoadRegsbutton.Text = "Load File";
        LoadRegsbutton.UseVisualStyleBackColor = true;
        LoadRegsbutton.Visible = false;
        LoadRegsbutton.Click += new System.EventHandler(LoadRegsbutton_Click);
        SaveRegsbutton.Location = new System.Drawing.Point(2, 154);
        SaveRegsbutton.Name = "SaveRegsbutton";
        SaveRegsbutton.Size = new System.Drawing.Size(75, 23);
        SaveRegsbutton.TabIndex = 6;
        SaveRegsbutton.Text = "Regs Save";
        SaveRegsbutton.UseVisualStyleBackColor = true;
        SaveRegsbutton.Visible = false;
        SaveRegsbutton.Click += new System.EventHandler(SaveRegsbutton_Click);
        allReadButton.Location = new System.Drawing.Point(2, 83);
        allReadButton.Name = "allReadButton";
        allReadButton.Size = new System.Drawing.Size(75, 23);
        allReadButton.TabIndex = 5;
        allReadButton.Text = "All Read";
        allReadButton.UseVisualStyleBackColor = true;
        allReadButton.Click += new System.EventHandler(allReadButton_Click_1);
        WriteButton.Location = new System.Drawing.Point(3, 41);
        WriteButton.Name = "WriteButton";
        WriteButton.Size = new System.Drawing.Size(75, 23);
        WriteButton.TabIndex = 4;
        WriteButton.Text = "Write";
        WriteButton.UseVisualStyleBackColor = true;
        ReadButton.Location = new System.Drawing.Point(3, 12);
        ReadButton.Name = "ReadButton";
        ReadButton.Size = new System.Drawing.Size(75, 23);
        ReadButton.TabIndex = 3;
        ReadButton.Text = "Read";
        ReadButton.UseVisualStyleBackColor = true;
        base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
        base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        base.ClientSize = new System.Drawing.Size(532, 317);
        base.Controls.Add(splitContainer1);
        base.MaximizeBox = false;
        base.MinimizeBox = false;
        base.Name = "SettingForm";
        Text = "Settings";
        base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(SettingForm_FormClosing);
        base.Load += new System.EventHandler(SettingForm_Load);
        splitContainer1.Panel1.ResumeLayout(performLayout: false);
        splitContainer1.Panel2.ResumeLayout(performLayout: false);
        ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
        splitContainer1.ResumeLayout(performLayout: false);
        ResumeLayout(performLayout: false);
    }
}
