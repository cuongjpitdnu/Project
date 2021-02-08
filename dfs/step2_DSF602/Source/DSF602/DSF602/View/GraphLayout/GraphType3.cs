using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DSF602.Class;
using GraphLib;

namespace DSF602.View.GraphLayout
{
    public partial class GraphType3 : GraphTypeBase
    {
        public GraphType3() : base()
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

            clsSupportGraph.SetGridDistanceY(this.GraphTwo, 100);
            clsSupportGraph.SetGridDistanceY(this.GraphThree, 100);
            clsSupportGraph.SetGridDistanceY(this.GraphFour, 100);
            clsSupportGraph.SetGridDistanceY(this.GraphFive, 100);
            clsSupportGraph.SetGridDistanceY(this.GraphSix, 100);
            clsSupportGraph.SetGridDistanceY(this.GraphSeven, 100);
            clsSupportGraph.SetGridDistanceY(this.GraphEight, 100);

            clsSupportGraph.SetGridDistanceX(this.GraphOne, 60);
            clsSupportGraph.SetGridDistanceY(this.GraphOne, 2000);
            clsSupportGraph.AddVScrollBar(this.GraphOne);
            clsSupportGraph.AddHScrollBar(this.GraphOne);

            List<Panel> lstPanel = new List<Panel> { panel1, panel2, panel3, panel4, panel5, panel6, panel7, panel8 };
            InitPanel(lstPanel);

        }
    }
}
