<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRelationMem
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRelationMem))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.grpListMem = New System.Windows.Forms.GroupBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnDown1 = New System.Windows.Forms.Button()
        Me.btnUp1 = New System.Windows.Forms.Button()
        Me.dgvRelation = New System.Windows.Forms.DataGridView()
        Me.colGen = New System.Windows.Forms.DataGridViewImageColumn()
        Me.colName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colBirth = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colRelTyp = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clmMemID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnExcelExport = New System.Windows.Forms.Button()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpListMem.SuspendLayout()
        CType(Me.dgvRelation, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.GiaPha.My.Resources.Resources.AddChilds48
        Me.PictureBox1.Location = New System.Drawing.Point(10, 8)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(48, 48)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 7
        Me.PictureBox1.TabStop = False
        '
        'lblTitle
        '
        Me.lblTitle.Font = New System.Drawing.Font("Times New Roman", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        Me.lblTitle.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.lblTitle.Location = New System.Drawing.Point(56, 8)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(471, 70)
        Me.lblTitle.TabIndex = 6
        Me.lblTitle.Text = "Thông tin các thành viên liên quan với : "
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'grpListMem
        '
        Me.grpListMem.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpListMem.Controls.Add(Me.btnSave)
        Me.grpListMem.Controls.Add(Me.btnDown1)
        Me.grpListMem.Controls.Add(Me.btnUp1)
        Me.grpListMem.Controls.Add(Me.dgvRelation)
        Me.grpListMem.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        Me.grpListMem.Location = New System.Drawing.Point(10, 81)
        Me.grpListMem.Name = "grpListMem"
        Me.grpListMem.Size = New System.Drawing.Size(559, 377)
        Me.grpListMem.TabIndex = 8
        Me.grpListMem.TabStop = False
        Me.grpListMem.Text = "Thông tin"
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Enabled = False
        Me.btnSave.Image = CType(resources.GetObject("btnSave.Image"), System.Drawing.Image)
        Me.btnSave.Location = New System.Drawing.Point(520, 98)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(33, 33)
        Me.btnSave.TabIndex = 41
        Me.ToolTip1.SetToolTip(Me.btnSave, "Lưu lại.")
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnDown1
        '
        Me.btnDown1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDown1.Enabled = False
        Me.btnDown1.Image = CType(resources.GetObject("btnDown1.Image"), System.Drawing.Image)
        Me.btnDown1.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnDown1.Location = New System.Drawing.Point(520, 59)
        Me.btnDown1.Name = "btnDown1"
        Me.btnDown1.Size = New System.Drawing.Size(33, 33)
        Me.btnDown1.TabIndex = 41
        Me.ToolTip1.SetToolTip(Me.btnDown1, "Chuyển xuống dưới.")
        Me.btnDown1.UseVisualStyleBackColor = True
        '
        'btnUp1
        '
        Me.btnUp1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnUp1.Enabled = False
        Me.btnUp1.Image = CType(resources.GetObject("btnUp1.Image"), System.Drawing.Image)
        Me.btnUp1.ImageAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnUp1.Location = New System.Drawing.Point(520, 20)
        Me.btnUp1.Name = "btnUp1"
        Me.btnUp1.Size = New System.Drawing.Size(33, 33)
        Me.btnUp1.TabIndex = 40
        Me.ToolTip1.SetToolTip(Me.btnUp1, "Chuyển lên trên.")
        Me.btnUp1.UseVisualStyleBackColor = True
        '
        'dgvRelation
        '
        Me.dgvRelation.AllowUserToAddRows = False
        Me.dgvRelation.AllowUserToDeleteRows = False
        Me.dgvRelation.AllowUserToResizeColumns = False
        Me.dgvRelation.AllowUserToResizeRows = False
        Me.dgvRelation.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvRelation.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvRelation.ColumnHeadersHeight = 30
        Me.dgvRelation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvRelation.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colGen, Me.colName, Me.colBirth, Me.colRelTyp, Me.clmMemID})
        Me.dgvRelation.Location = New System.Drawing.Point(5, 20)
        Me.dgvRelation.MultiSelect = False
        Me.dgvRelation.Name = "dgvRelation"
        Me.dgvRelation.ReadOnly = True
        Me.dgvRelation.RowHeadersVisible = False
        Me.dgvRelation.RowTemplate.Height = 21
        Me.dgvRelation.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvRelation.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgvRelation.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvRelation.Size = New System.Drawing.Size(510, 352)
        Me.dgvRelation.TabIndex = 0
        '
        'colGen
        '
        Me.colGen.DataPropertyName = "GEN"
        Me.colGen.HeaderText = "GT"
        Me.colGen.Name = "colGen"
        Me.colGen.ReadOnly = True
        Me.colGen.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.colGen.Width = 40
        '
        'colName
        '
        Me.colName.DataPropertyName = "NAME"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.colName.DefaultCellStyle = DataGridViewCellStyle2
        Me.colName.HeaderText = "Họ và tên"
        Me.colName.Name = "colName"
        Me.colName.ReadOnly = True
        Me.colName.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.colName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colName.Width = 240
        '
        'colBirth
        '
        Me.colBirth.DataPropertyName = "BIRTH_DAY_NEW"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle3.Format = "dd/MM/yyyy"
        Me.colBirth.DefaultCellStyle = DataGridViewCellStyle3
        Me.colBirth.HeaderText = "Ngày Sinh"
        Me.colBirth.Name = "colBirth"
        Me.colBirth.ReadOnly = True
        Me.colBirth.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colBirth.Width = 115
        '
        'colRelTyp
        '
        Me.colRelTyp.DataPropertyName = "REL_TYPE"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.colRelTyp.DefaultCellStyle = DataGridViewCellStyle4
        Me.colRelTyp.HeaderText = "Quan hệ"
        Me.colRelTyp.Name = "colRelTyp"
        Me.colRelTyp.ReadOnly = True
        Me.colRelTyp.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colRelTyp.Width = 110
        '
        'clmMemID
        '
        Me.clmMemID.DataPropertyName = "MEMBER_ID"
        Me.clmMemID.HeaderText = "MEMBER_ID"
        Me.clmMemID.Name = "clmMemID"
        Me.clmMemID.ReadOnly = True
        Me.clmMemID.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.GiaPha.My.Resources.Resources.back_24
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(327, 464)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(86, 32)
        Me.btnCancel.TabIndex = 9
        Me.btnCancel.Text = "Quay lại"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Trở lại màn hình trước.")
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnExcelExport
        '
        Me.btnExcelExport.Image = Global.GiaPha.My.Resources.Resources.Excel
        Me.btnExcelExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExcelExport.Location = New System.Drawing.Point(167, 464)
        Me.btnExcelExport.Name = "btnExcelExport"
        Me.btnExcelExport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnExcelExport.Size = New System.Drawing.Size(115, 32)
        Me.btnExcelExport.TabIndex = 20
        Me.btnExcelExport.Text = "Xuất ra Excel"
        Me.btnExcelExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExcelExport.UseVisualStyleBackColor = True
        '
        'frmRelationMem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(581, 501)
        Me.Controls.Add(Me.btnExcelExport)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.grpListMem)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.lblTitle)
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.Name = "frmRelationMem"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Danh sách"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpListMem.ResumeLayout(False)
        CType(Me.dgvRelation, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents grpListMem As System.Windows.Forms.GroupBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents dgvRelation As System.Windows.Forms.DataGridView
    Friend WithEvents btnDown1 As System.Windows.Forms.Button
    Friend WithEvents btnUp1 As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents colGen As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents colName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colBirth As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colRelTyp As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmMemID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnExcelExport As System.Windows.Forms.Button
End Class
