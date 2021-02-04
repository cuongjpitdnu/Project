using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SkiaSharp;
using GP40Common;
using System.Collections;
using SkiaSharp.Views.Desktop;
using System.Text.Json;
using System.Drawing.Drawing2D;

namespace GP40Tree
{
    public partial class frmFTree : Form
    {
        private SKControl skTree;        
        private clsTreeDraw objFTree;

        private clsConst.ENUM_MEMBER_TEMPLATE enTemplate = clsConst.ENUM_MEMBER_TEMPLATE.MCTFull;

        Hashtable hasMember;
        private int intMemberCount = 0;
        private int intMaxFLevelCount = 3;

        public frmFTree()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.SetStyle(ControlStyles.DoubleBuffer |
              ControlStyles.UserPaint |
              ControlStyles.AllPaintingInWmPaint,
              true);
            this.UpdateStyles();

            objFTree = new clsTreeDraw();
            skTree = objFTree.TreeDraw;

            lblStart.Text = DateTime.Now.ToString();

            hasMember = objFTree.Family;
            
            panelMiddle.Controls.Add(skTree);
        }

        private void xCreateTree(clsConst.ENUM_MEMBER_TEMPLATE enTemplate)
        {
            lblStart.Text = DateTime.Now.ToString();
            lblEnd.Text = "";

            if (hasMember != null)
            {
                hasMember.Clear();
            }

            clsFamilyMember objMember = xCreateMember(0, 1, enTemplate, clsConst.ENUM_GENDER.Male, null, 0, 1);
            hasMember.Add(objMember.miID, objMember);
            intMemberCount = 1;
            xMakeTreeRelation(0, enTemplate);

            //clsCommon.MakeTreeDraw(0, 10, 10, hasMember);
            objFTree.MakeTreeDrawWithSpouse(0, 10, 10, clsConst.ENUM_FIRSTSPOUSE_POSITION.LeftMember);

            lblCount.Text = intMemberCount.ToString();

            skTree.Invalidate();
        }

        private void btnVe1_Click(object sender, EventArgs e)
        {
            enTemplate = clsConst.ENUM_MEMBER_TEMPLATE.MCTFull;
            objFTree.SelectedMember = null;
            clsCommon.InitDataDirectory();
            xCreateTree(enTemplate);            
            objFTree.CenteringRoot();
        }

        private void btnVe2_Click(object sender, EventArgs e)
        {
            enTemplate = clsConst.ENUM_MEMBER_TEMPLATE.MCTShort;
            objFTree.SelectedMember = null;
            clsCommon.InitDataDirectory();
            xCreateTree(enTemplate);
            objFTree.CenteringRoot();
        }
        private void btnVex3_Click(object sender, EventArgs e)
        {
            enTemplate = clsConst.ENUM_MEMBER_TEMPLATE.MCTVeryShort;
            objFTree.SelectedMember = null;
            clsCommon.InitDataDirectory();
            xCreateTree(enTemplate);
            objFTree.CenteringRoot();
        }

        private clsFamilyMember xCreateMember(int intMemberID, int intFrame,
            clsConst.ENUM_MEMBER_TEMPLATE enTemplate, clsConst.ENUM_GENDER enGender, clsFamilyMember objFather = null, int intIndexChild = 0, int intLevel = 1)
        {
            clsFamilyMember objMember = new clsFamilyMember();

            objMember.miID = intMemberID;
            objMember.InitMemberInfo(enTemplate, enGender);
            //objMember.FrameImage = clsConst.FramePath + intFrame.ToString("0#") + ".png";

            //if (enGender == clsConst.ENUM_GENDER.Unknown)
            //    objMember.Image = clsConst.AvartaPath + "noavatar.jpg";
            //else if(enGender == clsConst.ENUM_GENDER.FMale)
            //    objMember.Image = clsConst.AvartaPath + "female.png";
            //else if (enGender == clsConst.ENUM_GENDER.Male)
            //    objMember.Image = clsConst.AvartaPath + "male.png";                   

            ////objMember.FullName = "Phần Mềm QLGP " + (intMemberID + 1).ToString();
            //objMember.FullName = "PM QLGP " + (intMemberID + 1).ToString();
            //objMember.BDayS = "06";
            //objMember.BMonthS = "10";
            //objMember.BYearS = "1981";

            //objMember.TemplateType = enTemplate;
            if (objFather != null)
            {
                if (intIndexChild != 0)
                {
                    objMember.FLevel = intLevel.ToString() + ".[" + objFather.FLevel.Replace("[", "").Replace("]", "") + "]." + intIndexChild.ToString();
                }
            }
            else
            {
                objMember.FLevel = intLevel.ToString();
                if (intIndexChild != 0)
                {
                    objMember.FLevel = intLevel.ToString() + "." + intIndexChild.ToString();
                }
            }

            objMember.intFLevel = intLevel;
            //objMember.DrawingMember();
            // objMember.DrawingMemberSVG();

            return objMember;
        }

        //Automactic Make a Tree by intMaxFLevelCount, number of children is random
        private void xMakeTreeRelation(int intMember, clsConst.ENUM_MEMBER_TEMPLATE enTemplate)
        {
            clsFamilyMember objMember = (clsFamilyMember)hasMember[intMember];
            if (objMember.intFLevel >= intMaxFLevelCount)
            {
                return;
            }

            Random r = new Random();
            int rChild = r.Next(1, 5); //Number of child randmom
            
            rChild = 5;
            for (int i = 0; i < rChild; i++)
            {
                clsFamilyMember objMemberChild = xCreateMember(intMemberCount, 1, enTemplate, clsConst.ENUM_GENDER.Male, objMember, i + 1, objMember.intFLevel + 1);
                //objMemberChild.FLevel = objMember.FLevel + "." + (i + 1).ToString();
                objMember.lstChild.Add(objMemberChild.miID);
                hasMember.Add(objMemberChild.miID, objMemberChild);
                intMemberCount++;
            }

            int rSpouse = r.Next(1, 2); //Number of Spouse Random
            rSpouse = 3;
            for (int i = 0; i < rSpouse; i++)
            {
                clsFamilyMember objMemberSpouse = xCreateMember(intMemberCount, 1, enTemplate, clsConst.ENUM_GENDER.FMale, null, 0, objMember.intFLevel);
                objMember.lstSpouse.Add(objMemberSpouse.miID);
                hasMember.Add(objMemberSpouse.miID, objMemberSpouse);
                intMemberCount++;
            }

            for (int i = 0; i < rChild; i++)
            {
                xMakeTreeRelation(objMember.lstChild[i], enTemplate);
            }

        }

        private void btnPDF2_Click(object sender, EventArgs e)
        {
            lblStart.Text = DateTime.Now.ToString();
            lblEnd.Text = "";

            //xMakeTree();
            if (hasMember.Count <= 0) xCreateTree(enTemplate);

            float dpi = 300;
            // create the document
            SKWStream stream = SKFileWStream.OpenStream("FamilyTree.pdf");
            SKDocument document = SKDocument.CreatePdf(stream, dpi);

            //SKDocument document = SKDocument.CreatePdf("FamilyTree.pdf");

            // get the canvas from the page
            var canvas = document.BeginPage(46.8f * dpi, 33.1f * dpi);

            // draw on the canvas ...
            objFTree.DrawTree(canvas);

            canvas.Dispose();
            // end the page and document
            document.EndPage();
            //Thread.Sleep(100);
            document.Close();
            document.Dispose();

            stream.Flush();
            stream.Dispose();

            lblEnd.Text = DateTime.Now.ToString();
        }

        private void btnSvg_Click(object sender, EventArgs e)
        {
            lblStart.Text = DateTime.Now.ToString();
            lblEnd.Text = "";

            //xMakeTree();
            if (hasMember.Count <= 0) xCreateTree(enTemplate);

            // create the canvas
            var stream = SKFileWStream.OpenStream("FamilyTree.svg");
            //var writer = new SKXmlStreamWriter(stream);
            float dpi = 300;
            var canvas = SKSvgCanvas.Create(SKRect.Create(46.8f * dpi, 33.1f * dpi), stream);


            // draw on the canvas ...
            objFTree.DrawTree(canvas);

            canvas.Dispose();
            //writer.Dispose();
            stream.Flush();
            stream.Dispose();

            lblEnd.Text = DateTime.Now.ToString();
            //skTree.Invalidate();
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            if (objFTree.ZoomLevel > 0.1f)
            {
                objFTree.ZoomLevel = objFTree.ZoomLevel - 0.1f;
            }
            lblZoomLevel.Text = objFTree.ZoomLevel.ToString("0.#");
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            if (objFTree.ZoomLevel < 2f)
            {
                objFTree.ZoomLevel = objFTree.ZoomLevel + 0.1f;
            }
            lblZoomLevel.Text = objFTree.ZoomLevel.ToString("0.#");
        }

        private void btnRoot_Click(object sender, EventArgs e)
        {
            objFTree.CenteringRoot();
        }


        private void btnJSON_Click(object sender, EventArgs e)
        {
            if (objFTree.SelectedMember == null) return;

            var options = new JsonSerializerOptions
            {
                IgnoreReadOnlyProperties = true,
                WriteIndented = true
            };
            string jsonString = JsonSerializer.Serialize(objFTree.SelectedMember, options);

            var obj = JsonSerializer.Deserialize<clsFamilyMember>(jsonString);

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

        private void frmFTree_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (hasMember != null) hasMember.Clear();
            hasMember = null;
            objFTree = null;
        }

        private void btnFIT_Click(object sender, EventArgs e)
        {
            objFTree.SetFit();
        }
    }

}

