// Modbustool.ExecForm
using Modbus;
using Modbus.Device;
using Modbustool;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

public class ExecForm : Form
{
    private Sensor TargetSensor;

    private ModbusMaster Master;

    private int devno;

    private IContainer components;

    private TabControl tabControl1;

    public ExecForm(ModbusMaster _master, int _devno, Sensor _ts)
    {
        InitializeComponent();
        Master = _master;
        TargetSensor = _ts;
        devno = _devno;
        tabControl1.SuspendLayout();
        tabControl1.SizeMode = TabSizeMode.Fixed;
        TabPage tabPage = new TabPage("実行");
        tabPage.SuspendLayout();
        DataGridView dataGridView = new DataGridView();
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
        DataGridViewButtonColumn dataGridViewColumn = new DataGridViewButtonColumn
        {
            Name = "Button",
            UseColumnTextForButtonValue = true,
            Text = "exec"
        };
        dataGridView.Columns.Add(dataGridViewColumn);
        dataGridView.RowCount = TargetSensor.ExecMaps.Count;
        for (int i = 0; i < TargetSensor.ExecMaps.Count; i++)
        {
            dataGridView[0, i].Value = TargetSensor.ExecMaps[i].Reg.Title.ToString();
            dataGridView[0, i].ReadOnly = true;
            if (TargetSensor.ExecMaps[i].Reg.Type == ParameterType.TYPE_SELECT)
            {
                dataGridView[1, i] = new DataGridViewComboBoxCell();
                string[] array = TargetSensor.ExecMaps[i].Reg.SelectItem();
                string[] array2 = array;
                foreach (string item in array2)
                {
                    ((DataGridViewComboBoxCell)dataGridView[1, i]).Items.Add(item);
                }
            }
            if (TargetSensor.ExecMaps[i].Reg.Ratio == 0)
            {
                dataGridView[1, i].ReadOnly = true;
            }
            dataGridView[1, i].Value = TargetSensor.ExecMaps[i].Reg.ToString();
        }
        dataGridView.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        dataGridView.CurrentCell = dataGridView[2, 0];
        dataGridView.CellContentClick += mdgv_OnCellContentClickAsync;
        tabPage.Controls.Add(dataGridView);
        tabPage.AutoScroll = true;
        tabControl1.Controls.Add(tabPage);
        tabPage.ResumeLayout();
        tabControl1.ResumeLayout();
    }

    private void ExecForm_Load(object sender, EventArgs e)
    {
    }

    private void ExecForm_FormClosing(object sender, FormClosingEventArgs e)
    {
    }

    private async void mdgv_OnCellContentClickAsync(object sender, DataGridViewCellEventArgs args)
    {
        DataGridView mdgv = (DataGridView)sender;
        if (mdgv.Columns[args.ColumnIndex].Name == "Button")
        {
            try
            {
                string param = mdgv[1, args.RowIndex].Value.ToString();
                if (param == null || param == "")
                {
                    throw new Exception(mdgv[0, args.RowIndex].Value.ToString() + " parameter empty");
                }
                if (!TargetSensor.ExecMaps[args.RowIndex].Reg.ToValue(param))
                {
                    throw new Exception(mdgv[0, args.RowIndex].Value.ToString() + " parameter error");
                }
                byte[] v = TargetSensor.ExecMaps[args.RowIndex].Reg.ToFrame();
                await Master.WriteMultipleRegistersAsync((byte)devno, TargetSensor.ExecMaps[args.RowIndex].ExecAddress, Utils.ByteToFrame(v));
            }
            catch (SlaveException)
            {
            }
            catch
            {
            }
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
        tabControl1 = new System.Windows.Forms.TabControl();
        SuspendLayout();
        tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
        tabControl1.Location = new System.Drawing.Point(0, 0);
        tabControl1.Name = "tabControl1";
        tabControl1.SelectedIndex = 0;
        tabControl1.Size = new System.Drawing.Size(473, 336);
        tabControl1.TabIndex = 4;
        base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
        base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        base.ClientSize = new System.Drawing.Size(473, 336);
        base.Controls.Add(tabControl1);
        base.MaximizeBox = false;
        base.MinimizeBox = false;
        base.Name = "ExecForm";
        Text = "Exec";
        base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ExecForm_FormClosing);
        base.Load += new System.EventHandler(ExecForm_Load);
        ResumeLayout(performLayout: false);
    }
}
