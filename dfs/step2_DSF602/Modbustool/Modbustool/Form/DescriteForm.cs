// Modbustool.DescriteForm
using Modbus;
using Modbus.Device;
using Modbustool;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

public class DescriteForm : Form
{
    private Sensor TargetSensor;

    private ModbusMaster Master;

    private int devno;

    private IContainer components;

    private SplitContainer splitContainer1;

    private TabControl tabControl1;

    private Button ReadButton;

    public DescriteForm(ModbusMaster _master, int _devno, Sensor _ts)
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
        dataGridView.AllowUserToResizeRows = false;
        dataGridView.ColumnCount = 2;
        dataGridView.Columns[0].HeaderText = "Item";
        dataGridView.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
        dataGridView.Columns[1].HeaderText = "Value";
        dataGridView.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
        dataGridView.RowCount = TargetSensor.DescriteMaps.Count;
        for (int i = 0; i < dataGridView.Rows.Count; i++)
        {
            dataGridView[0, i].Value = TargetSensor.DescriteMaps[i].Title.ToString();
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
            bool[] task = await Master.ReadInputsAsync((byte)devno, (ushort)TargetSensor.DescriteAddress, (ushort)TargetSensor.DescriteMaps.Count);
            int item_idx2 = 0;
            foreach (typeBase descriteMap in TargetSensor.DescriteMaps)
            {
                if (task[item_idx2])
                {
                    ((typeCh)descriteMap).bit = 1;
                }
                else
                {
                    ((typeCh)descriteMap).bit = 0;
                }
                item_idx2++;
            }
            TabPage pg = tabControl1.TabPages[tab_idx];
            DataGridView mdgv = (DataGridView)pg.Controls[0];
            item_idx2 = 0;
            foreach (typeBase descriteMap2 in TargetSensor.DescriteMaps)
            {
                mdgv[1, item_idx2].Value = descriteMap2.ToString();
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
        base.MaximizeBox = false;
        base.MinimizeBox = false;
        base.Name = "DescriteForm";
        Text = "Descrite";
        base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(SettingForm_FormClosing);
        base.Load += new System.EventHandler(SettingForm_Load);
        splitContainer1.Panel1.ResumeLayout(performLayout: false);
        splitContainer1.Panel2.ResumeLayout(performLayout: false);
        ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
        splitContainer1.ResumeLayout(performLayout: false);
        ResumeLayout(performLayout: false);
    }
}
