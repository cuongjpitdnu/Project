using DSF602.Class;
using GraphLib;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DSF602.View.GraphLayout
{
    public partial class GraphType1 : GraphTypeBase
    {
        public GraphType1() : base()
        {
            InitializeComponent();

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            this.GraphOne.Margin = new Padding(1);
            this.GraphTwo.Margin = new Padding(1);
            this.GraphThree.Margin = new Padding(1);
            this.GraphFour.Margin = new Padding(1);
            this.GraphFive.Margin = new Padding(1);
            this.GraphSix.Margin = new Padding(1);
            this.GraphSeven.Margin = new Padding(1);
            this.GraphEight.Margin = new Padding(1);

            clsSupportGraph.SetGridDistanceX(this.GraphOne, 60);
            clsSupportGraph.SetGridDistanceX(this.GraphTwo, 60);
            clsSupportGraph.SetGridDistanceY(this.GraphOne, 1500);
            clsSupportGraph.SetGridDistanceY(this.GraphTwo, 1500);

            clsSupportGraph.AddVScrollBar(this.GraphOne, 1500);
            clsSupportGraph.AddVScrollBar(this.GraphTwo, 1500);

            clsSupportGraph.AddHScrollBar(this.GraphOne, 300);
            clsSupportGraph.AddHScrollBar(this.GraphTwo, 300);


            List<Panel> lstPanel = new List<Panel> { panel1, panel2, panel3, panel4, panel5, panel6, panel7, panel8 };
            InitPanel(lstPanel);
        }

    }
}

