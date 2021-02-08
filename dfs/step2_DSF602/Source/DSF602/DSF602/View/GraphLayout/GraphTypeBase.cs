using DSF602.Class;
using GraphLib;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DSF602.View.GraphLayout
{
    public partial class GraphTypeBase : UserControl
    {
        public DisplaySensor LabelOne { get; set; }
        public DisplaySensor LabelTwo { get; set; }
        public DisplaySensor LabelThree { get; set; }
        public DisplaySensor LabelFour { get; set; }
        public DisplaySensor LabelFive { get; set; }
        public DisplaySensor LabelSix { get; set; }
        public DisplaySensor LabelSeven { get; set; }
        public DisplaySensor LabelEight { get; set; }

        public DisplaySensor[] _arrLabel;

        public PlotterDisplayEx GraphOne { get; set; }
        public PlotterDisplayEx GraphTwo { get; set; }
        public PlotterDisplayEx GraphThree { get; set; }
        public PlotterDisplayEx GraphFour { get; set; }
        public PlotterDisplayEx GraphFive { get; set; }
        public PlotterDisplayEx GraphSix { get; set; }
        public PlotterDisplayEx GraphSeven { get; set; }
        public PlotterDisplayEx GraphEight { get; set; }

        private PlotterDisplayEx[] _arrGraph;

        public GraphTypeBase()
        {
            InitializeComponent();

            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Opaque | ControlStyles.SupportsTransparentBackColor, false);

            this.GraphOne = new PlotterDisplayEx();
            this.GraphTwo = new PlotterDisplayEx();
            this.GraphThree = new PlotterDisplayEx();
            this.GraphFour = new PlotterDisplayEx();
            this.GraphFive = new PlotterDisplayEx();
            this.GraphSix = new PlotterDisplayEx();
            this.GraphSeven = new PlotterDisplayEx();
            this.GraphEight = new PlotterDisplayEx();
            this._arrGraph = new PlotterDisplayEx[] { GraphOne, GraphTwo, GraphThree, GraphFour, GraphFive, GraphSix, GraphSeven, GraphEight };

            var cnn = 1;
            foreach (var graph in this._arrGraph)
            {
                graph.Name = "Graph" + cnn.ToString();
                clsSupportGraph.GraphInit(graph);
                cnn++;
            }

            this.LabelOne = this.CreateLabel();
            this.LabelTwo = this.CreateLabel();
            this.LabelThree = this.CreateLabel();
            this.LabelFour = this.CreateLabel();
            this.LabelFive = this.CreateLabel();
            this.LabelSix = this.CreateLabel();
            this.LabelSeven = this.CreateLabel();
            this.LabelEight = this.CreateLabel();
            this._arrLabel = new DisplaySensor[] { LabelOne, LabelTwo, LabelThree, LabelFour, LabelFive, LabelSix, LabelSeven, LabelEight };
        }

        public DisplaySensor CreateLabel()
        {
            return new DisplaySensor()
            {
                BackColor = Color.FromArgb(0, 15, 33)
            };
        }

        public void ResetNameGraphChild()
        {
            var cnn = 1;
            foreach (var graph in this._arrGraph)
            {
                graph.Name = "Graph" + cnn.ToString();
                cnn++;
            }
        }

        public PlotterDisplayEx GetPlotterDisplayExByIndex(int index)
        {
            if (this._arrGraph != null && index > -1 && index < this._arrGraph.Length)
            {
                return this._arrGraph[index];
            }

            return null;
        }

        public DisplaySensor GetDisplaySensorByIndex(int index)
        {
            if (this._arrLabel != null && index > -1 && index < this._arrLabel.Length)
            {
                return this._arrLabel[index];
            }

            return null;
        }

        public void RefreshGraph()
        {
            if (this._arrGraph == null || this._arrGraph.Length == 0)
            {
                return;
            }

            foreach (var graph in this._arrGraph)
            {
                if (graph.AlowRefesh)
                {
                    clsSupportGraph.RefreshGraph(graph);
                }
            }
        }

        public void InitPanel(List<Panel> lstPanel)
        {
            var indexLbl = 0;

            foreach (var panel in lstPanel)
            {
                panel.AllowDrop = true;

                panel.DragEnter += panel_DragEnter;
                panel.DragDrop += panel_DragDrop;
                panel.MouseDown += panel_MouseDown;

                var graph = this.GetPlotterDisplayExByIndex(indexLbl);
                var displaySensor = this.GetDisplaySensorByIndex(indexLbl);
                displaySensor.Height = 25;
                panel.SizeChanged += ((o, e) =>
                {
                    displaySensor.Location = new Point(25, 0);
                    displaySensor.Width = panel.Width - 25;
                });

                displaySensor.LableSensorName.AllowDrop = true;
                displaySensor.LableSensorName.DragEnter += panel_DragEnter;
                displaySensor.LableSensorName.DragDrop += panel_DragDrop;
                displaySensor.LableSensorName.MouseDown += panel_MouseDown;

                panel.Controls.Add(graph);
                panel.Controls.Add(displaySensor);

                if (graph.Controls.Count > 0)
                {
                    foreach (var control in graph.Controls)
                    {
                        var vBar = control as VScrollBar;

                        if (vBar != null)
                        {
                            displaySensor.Location = new Point(graph.Location.X + vBar.Size.Width - displaySensor.Margin.Left, graph.Location.Y);
                            break;
                        }
                    }
                }

                graph.Dock = DockStyle.Fill;
                displaySensor.BringToFront();
                indexLbl++;
            }
        }

        public void panel_MouseDown(object sender, MouseEventArgs e)
        {
            var label = ((Control)sender).Parent;
            var panel = label.Parent as Panel;
            panel.DoDragDrop(label, DragDropEffects.Move);
        }

        public void panel_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        internal void ShowChargeButton()
        {
            //int[] arrSensorIndex = null;
            //if (this.GetType() == typeof(GraphType1))
            //{
            //    arrSensorIndex = new int[] { 0, 1 };
            //}
            //else if (this.GetType() == typeof(GraphType3))
            //{
            //    arrSensorIndex = new int[] { 0 };
            //}

            for (int i = 0; i < _arrLabel.Length; i++)
            {
                var pn = _arrLabel[i].Parent as Panel;
                if (pn != null && ("" + pn.Tag == "1"))
                {
                    _arrLabel[i].DisplayChargeButton(true);
                }
                else
                {
                    _arrLabel[i].DisplayChargeButton(false);
                }
            }
            //if (arrSensorIndex == null || arrSensorIndex.Length == 0) return;
            //foreach(var i in arrSensorIndex)
            //{
            //    _arrLabel[i].DisplayChargeButton(true);

            //    //var pn = _arrLabel[i].Parent as Panel;
            //    //_arrLabel[i].Width = pn.Width - 25;
            //}
        }

        public void panel_DragDrop(object sender, DragEventArgs e)
        {
            if (sender.GetType() != typeof(Panel)) return;
            var target = (Panel)sender;
            var grapType = target.Parent.Parent.Parent.Name;
            if (e.Data.GetDataPresent(typeof(DisplaySensor)))
            {
                var scr = (DisplaySensor)e.Data.GetData(typeof(DisplaySensor));
                var source = scr.Parent as Panel;
                if (source != target)
                {
                    SwapGraph(source, target, grapType);
                    SwapLocation(source, target);
                }
            }

            ShowChargeButton();
        }

        private void SwapLocation(Panel source, Panel target)
        {
            DisplaySensor sensorSource = null;
            DisplaySensor sensorTartget = null;

            foreach (var control in source.Controls)
            {
                if (control is DisplaySensor)
                {
                    sensorSource = (DisplaySensor)control;
                    break;
                }
            }

            foreach (var control in target.Controls)
            {
                if (control is DisplaySensor)
                {
                    sensorTartget = (DisplaySensor)control;
                    break;
                }
            }

            if (sensorSource == null || sensorTartget == null)
            {
                return;
            }

            var locationTemp = new Point(sensorSource.Location.X, sensorSource.Location.Y);
            sensorSource.Location = new Point(sensorTartget.Location.X, sensorTartget.Location.Y);
            sensorTartget.Location = new Point(locationTemp.X, locationTemp.Y);
        }

        public void SwapGraph(Panel source, Panel target, string graphType)
        {
            var targetCrl = target.Controls;
            var sourceCrl = source.Controls;

            Control[] targetControl = new Control[targetCrl.Count];
            for (int i = 0; i < targetCrl.Count; i++)
            {
                targetControl[i] = targetCrl[i];
            }

            Control[] sourceControl = new Control[sourceCrl.Count];
            for (int i = 0; i < source.Controls.Count; i++)
            {
                sourceControl[i] = source.Controls[i];
            }

            source.Controls.Clear();
            target.Controls.Clear();

            source.Controls.AddRange(targetControl);
            target.Controls.AddRange(sourceControl);

            if (source.Parent.Name == target.Parent.Name)
            {
                return;
            }

            if (source.Parent.Name == "LeftLayout")
            {

                SwapControl(sourceCrl, targetCrl, graphType);

            }
            else
            {
                SwapControl(targetCrl, sourceCrl, graphType);
            }
        }

        public void SwapControl(ControlCollection sourceCrl, ControlCollection targetCrl, string graphType)
        {
            if (sourceCrl.Count > 0)
            {
                foreach (var control in sourceCrl)
                {
                    var plotter = control as PlotterDisplayEx;

                    if (plotter == null)
                    {
                        continue;
                    }

                    if (graphType == "GraphType1")
                    {
                        clsSupportGraph.SetGridDistanceX(plotter, 60);
                        clsSupportGraph.SetGridDistanceY(plotter, 500);

                        clsSupportGraph.AddVScrollBar(plotter, 1500);
                        clsSupportGraph.AddHScrollBar(plotter, 300);
                    }

                    if (graphType == "GraphType3")
                    {
                        clsSupportGraph.SetGridDistanceX(plotter, 60);
                        clsSupportGraph.SetGridDistanceY(plotter, 500);

                        clsSupportGraph.AddVScrollBar(plotter);
                        clsSupportGraph.AddHScrollBar(plotter);
                    }

                    plotter.Height = sourceCrl.Owner.Height;
                }
            }

            if (targetCrl.Count > 0)
            {
                foreach (var control in targetCrl)
                {
                    var plotter = control as PlotterDisplayEx;

                    if (plotter == null)
                    {
                        continue;
                    }

                    SplitContainer splitTemp = null;

                    if (graphType == "GraphType1")
                    {
                        clsSupportGraph.SetGridDistanceX(plotter, 60);
                        clsSupportGraph.SetGridDistanceY(plotter, 500);
                    }

                    if (graphType == "GraphType3")
                    {
                        clsSupportGraph.SetGridDistanceX(plotter, 60);
                        clsSupportGraph.SetGridDistanceY(plotter, 100);
                    }


                    foreach (var controlIteam in plotter.Controls)
                    {
                        if (controlIteam is SplitContainer)
                        {
                            splitTemp = controlIteam as SplitContainer;
                            break;
                        }
                    }
                    if (splitTemp != null)
                    {
                        plotter.Controls.Clear();
                        plotter.Controls.Add(splitTemp);
                    }

                    plotter.Height = targetCrl.Owner.Height;

                }
            }
        }
    }
}