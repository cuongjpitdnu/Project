// Modbustool.VersionForm
using Modbus;
using Modbus.Device;
using Modbustool;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

public class VersionForm : Form
{
    private Sensor TargetSensor;

    private ModbusMaster Master;

    private int devno;

    private IContainer components;

    private SplitContainer splitContainer1;

    private TabControl tabControl1;

    private Button ReadButton;

    public VersionForm(ModbusMaster _master, int _devno, Sensor _ts)
    {
        InitializeComponent();
        Master = _master;
        TargetSensor = _ts;
        devno = _devno;
        ReadButton.Click += ReadButton_Click;
        tabControl1.SuspendLayout();
        tabControl1.SizeMode = TabSizeMode.Normal;
        TabPage tabPage = new TabPage("Coils");
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
        dataGridView.ColumnCount = 2;
        dataGridView.AllowUserToResizeRows = false;
        dataGridView.Columns[0].HeaderText = "Item";
        dataGridView.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
        dataGridView.Columns[1].HeaderText = "Value";
        dataGridView.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
        dataGridView.RowCount = TargetSensor.VersionMaps.Count;
        for (int i = 0; i < dataGridView.Rows.Count; i++)
        {
            dataGridView[0, i].Value = TargetSensor.VersionMaps[i].Title.ToString();
            dataGridView[0, i].ReadOnly = true;
            dataGridView[1, i].ReadOnly = true;
        }
        dataGridView.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        dataGridView.CurrentCell = dataGridView[1, 0];
        tabPage.Controls.Add(dataGridView);
        tabPage.AutoScroll = true;
        tabControl1.Controls.Add(tabPage);
        tabPage.ResumeLayout();
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
            int len = 0;
            foreach (typeBase versionMap in TargetSensor.VersionMaps)
            {
                len += versionMap.Len;
            }
            byte[] frame = Utils.FrameToByte(await Master.ReadInputRegistersAsync((byte)devno, (ushort)TargetSensor.VersionAddress, (ushort)(len / 2)));
            TabPage pg = tabControl1.TabPages[tab_idx];
            DataGridView mdgv = (DataGridView)pg.Controls[0];
            int item_idx = 0;
            for (int i = 0; i < TargetSensor.VersionMaps.Count; i++)
            {
                TargetSensor.VersionMaps[i].FrameTo(frame, item_idx);
                item_idx += TargetSensor.VersionMaps[i].Len;
                mdgv[1, i].Value = TargetSensor.VersionMaps[i].ToString();
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

    private void UIEnabled(bool bFlg)
    {
        ReadButton.Enabled = bFlg;
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
        base.Name = "VersionForm";
        base.ShowIcon = false;
        base.ShowInTaskbar = false;
        Text = "Version";
        base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(SettingForm_FormClosing);
        base.Load += new System.EventHandler(SettingForm_Load);
        splitContainer1.Panel1.ResumeLayout(performLayout: false);
        splitContainer1.Panel2.ResumeLayout(performLayout: false);
        ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
        splitContainer1.ResumeLayout(performLayout: false);
        ResumeLayout(performLayout: false);
    }
}
