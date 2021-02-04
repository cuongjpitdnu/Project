using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GPMain.Views.Controls
{
    public partial class DataGridTemplate : DataGridView
    {
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool VirtualMode
        {
            get => base.VirtualMode;
            private set => base.VirtualMode = value;
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool MultiSelect
        {
            get => base.MultiSelect;
            private set => base.MultiSelect = value;
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Color GridColor
        {
            get => base.GridColor;
            private set => base.GridColor = value;
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new DataGridViewCellStyle RowHeadersDefaultCellStyle
        {
            get => base.RowHeadersDefaultCellStyle;
            private set => base.RowHeadersDefaultCellStyle = value;
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new DataGridViewCellStyle ColumnHeadersDefaultCellStyle
        {
            get => base.ColumnHeadersDefaultCellStyle;
            private set => base.ColumnHeadersDefaultCellStyle = value;
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new DataGridViewCellStyle RowsDefaultCellStyle
        {
            get => base.RowsDefaultCellStyle;
            private set => base.RowsDefaultCellStyle = value;
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new DataGridViewCellStyle DefaultCellStyle
        {
            get => base.DefaultCellStyle;
            private set => base.DefaultCellStyle = value;
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new DataGridViewColumnHeadersHeightSizeMode ColumnHeadersHeightSizeMode
        {
            get => base.ColumnHeadersHeightSizeMode;
            set => base.ColumnHeadersHeightSizeMode = value;
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int ColumnHeadersHeight
        {
            get => base.ColumnHeadersHeight;
            set => base.ColumnHeadersHeight = value;
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new DataGridViewCellBorderStyle CellBorderStyle
        {
            get => base.CellBorderStyle;
            private set => base.CellBorderStyle = value;
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new DataGridViewHeaderBorderStyle ColumnHeadersBorderStyle
        {
            get => base.ColumnHeadersBorderStyle;
            private set => base.ColumnHeadersBorderStyle = value;
        }

        DataGridViewCellStyle defaultCellStyle = new DataGridViewCellStyle();
        DataGridViewCellStyle defaultHeaderCellStyle = new DataGridViewCellStyle();

        public DataGridTemplate()
        {
            InitControl();
        }

        public DataGridTemplate(IContainer container)
        {
            container.Add(this);
            InitControl();
        }

        private void InitControl()
        {
            AutoGenerateColumns = false;
            RowHeadersVisible = false;
            EnableHeadersVisualStyles = false;
            VirtualMode = true;
            MultiSelect = false;

            defaultCellStyle.BackColor = Color.White;
            defaultCellStyle.Font = new Font("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Pixel);
            defaultCellStyle.ForeColor = Color.FromArgb(89, 89, 91);
            defaultCellStyle.SelectionBackColor = Color.FromArgb(223, 223, 229);
            defaultCellStyle.SelectionForeColor = defaultCellStyle.ForeColor;
            defaultCellStyle.WrapMode = DataGridViewTriState.True;

            defaultHeaderCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            defaultHeaderCellStyle.BackColor = Color.FromArgb(91, 122, 146);
            defaultHeaderCellStyle.Font = new Font("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Pixel);
            defaultHeaderCellStyle.ForeColor = Color.WhiteSmoke;
            defaultHeaderCellStyle.SelectionBackColor = Color.FromArgb(223, 223, 229);
            defaultHeaderCellStyle.SelectionForeColor = defaultCellStyle.ForeColor;
            defaultHeaderCellStyle.WrapMode = DataGridViewTriState.True;

            DefaultCellStyle = defaultCellStyle;
            RowsDefaultCellStyle = defaultCellStyle;
            RowHeadersDefaultCellStyle = defaultHeaderCellStyle;
            ColumnHeadersDefaultCellStyle = defaultHeaderCellStyle;

            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            CellBorderStyle = DataGridViewCellBorderStyle.RaisedHorizontal;

            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            ColumnHeadersHeight = 25;

            GridColor = Color.FromArgb(248, 248, 255);

            ReadOnly = true;

            //this.Paint += (sender, e) =>
            //{

            //    var g = e.Graphics;

            //    using (GraphicsPath path = RoundedRect(new Rectangle(0, 0, this.Width, this.Height), 0))
            //    {
            //        g.DrawPath(new Pen(new SolidBrush(Color.FromArgb(200, 200, 206)), 2), path);
            //    }
            //};

            this.DataBindingComplete += DataGridTemplate_DataBindingComplete;
        }

        private void DataGridTemplate_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            ClearSelection();
        }

        //public static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        //{
        //    int diameter = radius * 2;
        //    Size size = new Size(diameter, diameter);
        //    Rectangle arc = new Rectangle(bounds.Location, size);
        //    GraphicsPath path = new GraphicsPath();

        //    if (radius == 0)
        //    {
        //        path.AddRectangle(bounds);
        //        return path;
        //    }

        //    // top left arc  
        //    path.AddArc(arc, 180, 90);

        //    // top right arc  
        //    arc.X = bounds.Right - diameter;
        //    path.AddArc(arc, 270, 90);

        //    // bottom right arc  
        //    arc.Y = bounds.Bottom - diameter;
        //    path.AddArc(arc, 0, 90);

        //    // bottom left arc 
        //    arc.X = bounds.Left;
        //    path.AddArc(arc, 90, 90);

        //    path.CloseFigure();
        //    return path;
        //}
    }
}
