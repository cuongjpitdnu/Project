namespace GPMain.Views.FamilyInfo
{
    partial class FamilyDocument
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
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSaveFile = new MaterialSkin.Controls.MaterialButton();
            this.btnUploadFile = new MaterialSkin.Controls.MaterialButton();
            this.txtDocName = new System.Windows.Forms.TextBox();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.txtIntroDoc = new System.Windows.Forms.TextBox();
            this.materialLabel3 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(714, 252);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.MintCream;
            this.panel2.Controls.Add(this.btnSaveFile);
            this.panel2.Controls.Add(this.btnUploadFile);
            this.panel2.Controls.Add(this.txtDocName);
            this.panel2.Controls.Add(this.txtPath);
            this.panel2.Controls.Add(this.txtIntroDoc);
            this.panel2.Controls.Add(this.materialLabel3);
            this.panel2.Controls.Add(this.materialLabel2);
            this.panel2.Controls.Add(this.materialLabel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.MinimumSize = new System.Drawing.Size(708, 294);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(708, 294);
            this.panel2.TabIndex = 1;
            // 
            // btnSaveFile
            // 
            this.btnSaveFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveFile.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSaveFile.Depth = 0;
            this.btnSaveFile.DrawShadows = true;
            this.btnSaveFile.HighEmphasis = true;
            this.btnSaveFile.Icon = null;
            this.btnSaveFile.Location = new System.Drawing.Point(177, 177);
            this.btnSaveFile.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnSaveFile.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnSaveFile.Name = "btnSaveFile";
            this.btnSaveFile.Size = new System.Drawing.Size(75, 36);
            this.btnSaveFile.TabIndex = 10;
            this.btnSaveFile.Text = "Lưu lại ";
            this.btnSaveFile.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnSaveFile.UseAccentColor = false;
            this.btnSaveFile.UseVisualStyleBackColor = true;
            this.btnSaveFile.Click += new System.EventHandler(this.btnSaveFile_Click);
            // 
            // btnUploadFile
            // 
            this.btnUploadFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUploadFile.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnUploadFile.Depth = 0;
            this.btnUploadFile.DrawShadows = true;
            this.btnUploadFile.HighEmphasis = true;
            this.btnUploadFile.Icon = null;
            this.btnUploadFile.Location = new System.Drawing.Point(606, 15);
            this.btnUploadFile.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnUploadFile.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnUploadFile.Name = "btnUploadFile";
            this.btnUploadFile.Size = new System.Drawing.Size(78, 36);
            this.btnUploadFile.TabIndex = 8;
            this.btnUploadFile.Text = "Upload";
            this.btnUploadFile.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnUploadFile.UseAccentColor = false;
            this.btnUploadFile.UseVisualStyleBackColor = true;
            this.btnUploadFile.Click += new System.EventHandler(this.btnUploadFile_Click);
            // 
            // txtDocName
            // 
            this.txtDocName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDocName.BackColor = System.Drawing.Color.MintCream;
            this.txtDocName.Location = new System.Drawing.Point(177, 55);
            this.txtDocName.Multiline = true;
            this.txtDocName.Name = "txtDocName";
            this.txtDocName.Size = new System.Drawing.Size(425, 35);
            this.txtDocName.TabIndex = 7;
            // 
            // txtPath
            // 
            this.txtPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPath.BackColor = System.Drawing.Color.MintCream;
            this.txtPath.Location = new System.Drawing.Point(177, 14);
            this.txtPath.Multiline = true;
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(425, 35);
            this.txtPath.TabIndex = 6;
            // 
            // txtIntroDoc
            // 
            this.txtIntroDoc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtIntroDoc.BackColor = System.Drawing.Color.MintCream;
            this.txtIntroDoc.Location = new System.Drawing.Point(177, 97);
            this.txtIntroDoc.Multiline = true;
            this.txtIntroDoc.Name = "txtIntroDoc";
            this.txtIntroDoc.Size = new System.Drawing.Size(425, 71);
            this.txtIntroDoc.TabIndex = 4;
            // 
            // materialLabel3
            // 
            this.materialLabel3.AutoSize = true;
            this.materialLabel3.Depth = 0;
            this.materialLabel3.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel3.Location = new System.Drawing.Point(15, 98);
            this.materialLabel3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel3.Name = "materialLabel3";
            this.materialLabel3.Size = new System.Drawing.Size(135, 19);
            this.materialLabel3.TabIndex = 3;
            this.materialLabel3.Text = "Nội dung khái quát";
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel2.Location = new System.Drawing.Point(41, 56);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(109, 19);
            this.materialLabel2.TabIndex = 2;
            this.materialLabel2.Text = "Tiêu đề  tài liệu";
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel1.Location = new System.Drawing.Point(70, 15);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(80, 19);
            this.materialLabel1.TabIndex = 0;
            this.materialLabel1.Text = "Đường dẫn";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // FamilyDocument
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(714, 252);
            this.Name = "FamilyDocument";
            this.Sizable = false;
            this.Size = new System.Drawing.Size(714, 252);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtIntroDoc;
        private MaterialSkin.Controls.MaterialLabel materialLabel3;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox txtPath;
        private MaterialSkin.Controls.MaterialButton btnUploadFile;
        private System.Windows.Forms.TextBox txtDocName;
        private MaterialSkin.Controls.MaterialButton btnSaveFile;
    }
}
