namespace DSF602.View.GraphLayout
{
    partial class GraphTypeBase
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();

                if (this._arrGraph != null)
                {
                    foreach (var graph in this._arrGraph)
                    {
                        if (graph != null)
                        {
                            graph.Dispose();
                        }
                    }
                }
            }

            this.GraphOne = null;
            this.GraphTwo = null;
            this.GraphThree = null;
            this.GraphFour = null;
            this.GraphFive = null;
            this.GraphSix = null;
            this.GraphSeven = null;
            this.GraphEight = null;
            this._arrGraph = null;
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        }

        #endregion
    }
}
