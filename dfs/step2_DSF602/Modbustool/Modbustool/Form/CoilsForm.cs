// Modbustool.CoilsForm
using Modbus;
using Modbus.Device;
using Modbustool;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

public class CoilsForm : Form
{
    private Sensor TargetSensor;

    private ModbusMaster Master;

    private int devno;

    private IContainer components;

    private SplitContainer splitContainer1;

    private TabControl tabControl1;

    private Button WriteButton;

    private Button ReadButton;

    public CoilsForm(ModbusMaster _master, int _devno, Sensor _ts)
    {
        InitializeComponent();
        Master = _master;
        TargetSensor = _ts;
        devno = _devno;
        ReadButton.Click += ReadButton_Click;
        WriteButton.Click += WriteButton_Click;
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
        dataGridView.AllowUserToResizeRows = false;
        dataGridView.ColumnCount = 2;
        dataGridView.Columns[0].HeaderText = "Item";
        dataGridView.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
        dataGridView.Columns[1].HeaderText = "Value";
        dataGridView.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
        dataGridView.RowCount = TargetSensor.CoilsMaps.Count;
        for (int i = 0; i < dataGridView.Rows.Count; i++)
        {
            dataGridView[0, i].Value = TargetSensor.CoilsMaps[i].Title.ToString();
            dataGridView[0, i].ReadOnly = true;
            if (TargetSensor.CoilsMaps[i].Type == ParameterType.TYPE_SELECT)
            {
                dataGridView[1, i] = new DataGridViewComboBoxCell();
                string[] array = TargetSensor.CoilsMaps[i].SelectItem();
                string[] array2 = array;
                foreach (string item in array2)
                {
                    ((DataGridViewComboBoxCell)dataGridView[1, i]).Items.Add(item);
                }
            }
            if (TargetSensor.CoilsMaps[i].Ratio == 0)
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
            bool[] task = await Master.ReadCoilsAsync((byte)devno, (ushort)TargetSensor.CoilAddress, (ushort)TargetSensor.CoilsMaps.Count);
            int item_idx2 = 0;
            foreach (typeBase coilsMap in TargetSensor.CoilsMaps)
            {
                if (task[item_idx2])
                {
                    ((typeCh)coilsMap).bit = 1;
                }
                else
                {
                    ((typeCh)coilsMap).bit = 0;
                }
                item_idx2++;
            }
            TabPage pg = tabControl1.TabPages[tab_idx];
            DataGridView mdgv = (DataGridView)pg.Controls[0];
            item_idx2 = 0;
            foreach (typeBase coilsMap2 in TargetSensor.CoilsMaps)
            {
                mdgv[1, item_idx2].Value = coilsMap2.ToString();
                item_idx2++;
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
            TabPage pg = tabControl1.TabPages[tab_idx];
            DataGridView mdgv = (DataGridView)pg.Controls[0];
            bool[] bits = new bool[TargetSensor.CoilsMaps.Count];
            foreach (typeBase coilsMap in TargetSensor.CoilsMaps)
            {
                string text = mdgv[1, item_idx].Value.ToString();
                if (text == null || text == "")
                {
                    throw new Exception(mdgv[0, item_idx].Value.ToString() + " parameter empty");
                }
                if (!coilsMap.ToValue(text))
                {
                    throw new Exception(mdgv[0, item_idx].Value.ToString() + " parameter error");
                }
                if (((typeCh)coilsMap).bit == 1)
                {
                    bits[item_idx] = true;
                }
                else
                {
                    bits[item_idx] = false;
                }
                item_idx++;
            }
            await Master.WriteMultipleCoilsAsync((byte)devno, (ushort)TargetSensor.CoilAddress, bits);
            ReadButton_Click(null, null);
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
                MessageBox.Show("Parameter error.");
            }
        }
        UIEnabled(bFlg: true);
    }

    private void UIEnabled(bool bFlg)
    {
        ReadButton.Enabled = bFlg;
        WriteButton.Enabled = bFlg;
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
        base.Name = "CoilsForm";
        Text = "Coils";
        base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(SettingForm_FormClosing);
        base.Load += new System.EventHandler(SettingForm_Load);
        splitContainer1.Panel1.ResumeLayout(performLayout: false);
        splitContainer1.Panel2.ResumeLayout(performLayout: false);
        ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
        splitContainer1.ResumeLayout(performLayout: false);
        ResumeLayout(performLayout: false);
    }
}
