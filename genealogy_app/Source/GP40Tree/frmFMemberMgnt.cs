using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using GP40Common;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace GP40Tree
{
    public partial class frmFMemberMgnt : Form
    {
        private SKControl skTree;
        private clsFInputTree objFTree;

        private clsConst.ENUM_MEMBER_TEMPLATE enTemplate = clsConst.ENUM_MEMBER_TEMPLATE.MCTInput;

        Hashtable hasMember;      

        public frmFMemberMgnt()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.SetStyle(ControlStyles.DoubleBuffer |
              ControlStyles.UserPaint |
              ControlStyles.AllPaintingInWmPaint,
              true);
            this.UpdateStyles();

            objFTree = new clsFInputTree();            
            skTree = objFTree.InputTree;

            lblStart.Text = DateTime.Now.ToString();
            panelMiddle.Controls.Add(skTree);
        }       

        private void frmFMemberMgnt_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (hasMember != null) hasMember.Clear();
            hasMember = null;
            objFTree = null;
        }
    }
}
