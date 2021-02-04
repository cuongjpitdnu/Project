using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GP40Common;
using System.Collections;
using System.Text.Json;
using System.Drawing.Drawing2D;

namespace GP40Tree
{
    public partial class frmMain : Form
    {
        private int mintGridWidth;

        public frmMain()
        {
            InitializeComponent();

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.SetStyle(ControlStyles.DoubleBuffer |
              ControlStyles.UserPaint |
              ControlStyles.AllPaintingInWmPaint,
              true);
            this.UpdateStyles();

            mintGridWidth = (int)tableLayoutPanel1.ColumnStyles[1].Width;                     

            panelTopL.BackColor = System.Drawing.ColorTranslator.FromHtml("#e0f3ff");

        }
        private void frmMain_Load(object sender, EventArgs e)
        {

        }
        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void ShowFormOnPanel(Panel objViewPanel, Form objForm)
        {
            objForm.TopLevel = false;
            objViewPanel.Controls.Clear();
            objViewPanel.Controls.Add(objForm);

            //if (blnChangeViewSize)
            //{
            //    objViewPanel.Width = objForm.Width + 10;
            //}

            objForm.BackColor = objViewPanel.BackColor;
            objForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            objForm.Dock = DockStyle.Fill;
            objForm.Show();
        }

        private void Menu_Click(object sender, EventArgs e)
        {
            Control objMenu = (Control)sender;

            if (objMenu.Name == "picMenuHambuger")
            {
                if (tableLayoutPanel1.ColumnStyles[1].Width == 0)
                {
                    dgvMember.Visible = true;
                    tableLayoutPanel1.ColumnStyles[1].Width = mintGridWidth;
                }
                else
                {
                    dgvMember.Visible = false;
                    tableLayoutPanel1.ColumnStyles[1].Width = 0;
                }
                return;
            }

            if (objMenu.Name.Contains("MenuFamilyTree"))
            {
                panelTopR.Controls.Clear();
                ShowFormOnPanel(panelTopR, new frmFTree());
            }

            if (objMenu.Name.Contains("MenuMember"))
            {
                panelTopR.Controls.Clear();
                ShowFormOnPanel(panelTopR, new frmFMemberMgnt());
            }

            if (objMenu.Name.Contains("MenuExit"))
            {
                this.Close();
            }            
        }

        private void Menu_MouseHover(object sender, EventArgs e)
        {
            Control objMenu = (Control)sender;
            Color hoverColor = System.Drawing.ColorTranslator.FromHtml("#3f6ad8");
            objMenu.ForeColor = hoverColor; 
            objMenu.Cursor = Cursors.Hand;
        }

        private void Menu_MouseLeave(object sender, EventArgs e)
        {
            Control objMenu = (Control)sender;
            objMenu.BackColor = Color.Transparent;
            objMenu.ForeColor = Color.Black;
            objMenu.Cursor = Cursors.Default;
        }
        private void tableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            var panel = sender as TableLayoutPanel;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            var rectangle = e.CellBounds;
            using (var pen = new Pen(Color.Black, 1))
            {
                pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

                if (e.Row == (panel.RowCount - 1))
                {
                    rectangle.Height -= 1;
                }

                if (e.Column == (panel.ColumnCount - 1))
                {
                    rectangle.Width -= 1;
                }
                e.Graphics.DrawLine(pen, rectangle.Right, rectangle.Top, rectangle.Right, rectangle.Bottom);
                if (e.Row == 0 & e.Column == 2)
                {
                    e.Graphics.DrawLine(pen, rectangle.Left, rectangle.Bottom, rectangle.Right, rectangle.Bottom);
                }

                if (e.Row == 2 & e.Column == 2)
                {
                    e.Graphics.DrawLine(pen, rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Top);
                }
                //e.Graphics.DrawRectangle(pen, rectangle);
            }
        }
    }
}
