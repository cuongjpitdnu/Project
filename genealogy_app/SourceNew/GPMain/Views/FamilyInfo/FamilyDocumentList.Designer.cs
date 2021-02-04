namespace GPMain.Views.FamilyInfo
{
    partial class FamilyDocumentList
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDownloadAll = new MaterialSkin.Controls.MaterialButton();
            this.btnAddDoc = new MaterialSkin.Controls.MaterialButton();
            this.lblHeader = new MaterialSkin.Controls.MaterialLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgv_lisDocument = new GPMain.Views.Controls.DataGridTemplate(this.components);
            this.STT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.docName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.docIntro = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.download = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_lisDocument)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 117F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(767, 647);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnDownloadAll);
            this.panel1.Controls.Add(this.btnAddDoc);
            this.panel1.Controls.Add(this.lblHeader);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(761, 111);
            this.panel1.TabIndex = 0;
            // 
            // btnDownloadAll
            // 
            this.btnDownloadAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDownloadAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnDownloadAll.Depth = 0;
            this.btnDownloadAll.DrawShadows = true;
            this.btnDownloadAll.HighEmphasis = true;
            this.btnDownloadAll.Icon = null;
            this.btnDownloadAll.Location = new System.Drawing.Point(129, 69);
            this.btnDownloadAll.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnDownloadAll.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnDownloadAll.Name = "btnDownloadAll";
            this.btnDownloadAll.Size = new System.Drawing.Size(151, 36);
            this.btnDownloadAll.TabIndex = 10;
            this.btnDownloadAll.Text = "Download tất cả";
            this.btnDownloadAll.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnDownloadAll.UseAccentColor = false;
            this.btnDownloadAll.UseVisualStyleBackColor = true;
            this.btnDownloadAll.Click += new System.EventHandler(this.btnDownloadAll_Click);
            // 
            // btnAddDoc
            // 
            this.btnAddDoc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddDoc.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAddDoc.Depth = 0;
            this.btnAddDoc.DrawShadows = true;
            this.btnAddDoc.HighEmphasis = true;
            this.btnAddDoc.Icon = null;
            this.btnAddDoc.Location = new System.Drawing.Point(4, 69);
            this.btnAddDoc.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnAddDoc.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnAddDoc.Name = "btnAddDoc";
            this.btnAddDoc.Size = new System.Drawing.Size(91, 36);
            this.btnAddDoc.TabIndex = 1;
            this.btnAddDoc.Text = "Thêm mới";
            this.btnAddDoc.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnAddDoc.UseAccentColor = false;
            this.btnAddDoc.UseVisualStyleBackColor = true;
            this.btnAddDoc.Click += new System.EventHandler(this.btnAddDoc_Click);
            // 
            // lblHeader
            // 
            this.lblHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHeader.Depth = 0;
            this.lblHeader.Font = new System.Drawing.Font("Roboto", 34F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblHeader.FontType = MaterialSkin.MaterialSkinManager.fontType.H4;
            this.lblHeader.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblHeader.Location = new System.Drawing.Point(0, 3);
            this.lblHeader.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(761, 54);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Danh sách Tài liệu";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgv_lisDocument);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 120);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(761, 524);
            this.panel2.TabIndex = 1;
            // 
            // dgv_lisDocument
            // 
            this.dgv_lisDocument.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.STT,
            this.docName,
            this.docIntro});
            this.dgv_lisDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_lisDocument.EnableHeadersVisualStyles = false;
            this.dgv_lisDocument.Location = new System.Drawing.Point(0, 0);
            this.dgv_lisDocument.Name = "dgv_lisDocument";
            this.dgv_lisDocument.ReadOnly = true;
            this.dgv_lisDocument.RowHeadersVisible = false;
            this.dgv_lisDocument.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_lisDocument.Size = new System.Drawing.Size(761, 524);
            this.dgv_lisDocument.TabIndex = 1;
            // 
            // STT
            // 
            this.STT.DataPropertyName = "STT";
            this.STT.HeaderText = "STT";
            this.STT.Name = "STT";
            this.STT.ReadOnly = true;
            this.STT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.STT.Width = 45;
            // 
            // docName
            // 
            this.docName.DataPropertyName = "FileName";
            this.docName.HeaderText = "Tên tài liệu";
            this.docName.Name = "docName";
            this.docName.ReadOnly = true;
            this.docName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.docName.Width = 200;
            // 
            // docIntro
            // 
            this.docIntro.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.docIntro.DataPropertyName = "FileIntroduce";
            this.docIntro.HeaderText = "Giới thiệu tài liệu";
            this.docIntro.Name = "docIntro";
            this.docIntro.ReadOnly = true;
            this.docIntro.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // download
            // 
            this.download.HeaderText = "DownLoad";
            this.download.Name = "download";
            this.download.ReadOnly = true;
            this.download.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.download.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.download.Text = "DownLoad";
            this.download.UseColumnTextForButtonValue = true;
            // 
            // FamilyDocumentList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FamilyDocumentList";
            this.Size = new System.Drawing.Size(767, 647);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_lisDocument)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private MaterialSkin.Controls.MaterialLabel lblHeader;
        private MaterialSkin.Controls.MaterialButton btnAddDoc;
        private Controls.DataGridTemplate dgv_lisDocument;
        private MaterialSkin.Controls.MaterialButton btnDownloadAll;
        private System.Windows.Forms.DataGridViewTextBoxColumn STT;
        private System.Windows.Forms.DataGridViewTextBoxColumn docName;
        private System.Windows.Forms.DataGridViewTextBoxColumn docIntro;
        private System.Windows.Forms.DataGridViewButtonColumn download;
    }
}
