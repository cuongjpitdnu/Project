<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChildsManager
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmChildsManager))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.dtpBirth = New System.Windows.Forms.DateTimePicker()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.grpList = New System.Windows.Forms.GroupBox()
        Me.dgvChildren = New System.Windows.Forms.DataGridView()
        Me.ID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NameChild = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BirthDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Relation = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.btnFindMember = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.grpList.SuspendLayout()
        CType(Me.dgvChildren, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtName)
        Me.GroupBox1.Controls.Add(Me.dtpBirth)
        Me.GroupBox1.Controls.Add(Me.Label15)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 402)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(470, 91)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Thông tin về con đang được chọn"
        Me.GroupBox1.Visible = False
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(129, 20)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(323, 22)
        Me.txtName.TabIndex = 1
        '
        'dtpBirth
        '
        Me.dtpBirth.Location = New System.Drawing.Point(129, 48)
        Me.dtpBirth.Name = "dtpBirth"
        Me.dtpBirth.Size = New System.Drawing.Size(130, 22)
        Me.dtpBirth.TabIndex = 3
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        Me.Label15.Location = New System.Drawing.Point(55, 52)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(68, 15)
        Me.Label15.TabIndex = 2
        Me.Label15.Text = "Ngày sinh :"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(58, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(65, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Họ và tên :"
        '
        'grpList
        '
        Me.grpList.Controls.Add(Me.dgvChildren)
        Me.grpList.Location = New System.Drawing.Point(12, 74)
        Me.grpList.Name = "grpList"
        Me.grpList.Size = New System.Drawing.Size(470, 322)
        Me.grpList.TabIndex = 1
        Me.grpList.TabStop = False
        Me.grpList.Text = "Danh sách thông tin các con"
        '
        'dgvChildren
        '
        Me.dgvChildren.AllowUserToAddRows = False
        Me.dgvChildren.AllowUserToDeleteRows = False
        Me.dgvChildren.AllowUserToResizeRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvChildren.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvChildren.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvChildren.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ID, Me.NameChild, Me.BirthDate, Me.Relation})
        Me.dgvChildren.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvChildren.EnableHeadersVisualStyles = False
        Me.dgvChildren.Location = New System.Drawing.Point(3, 18)
        Me.dgvChildren.Name = "dgvChildren"
        Me.dgvChildren.ReadOnly = True
        Me.dgvChildren.RowHeadersVisible = False
        Me.dgvChildren.RowTemplate.Height = 21
        Me.dgvChildren.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvChildren.Size = New System.Drawing.Size(464, 301)
        Me.dgvChildren.TabIndex = 0
        '
        'ID
        '
        Me.ID.HeaderText = "ID"
        Me.ID.Name = "ID"
        Me.ID.ReadOnly = True
        Me.ID.Width = 45
        '
        'NameChild
        '
        Me.NameChild.HeaderText = "Họ và tên"
        Me.NameChild.Name = "NameChild"
        Me.NameChild.ReadOnly = True
        Me.NameChild.Width = 170
        '
        'BirthDate
        '
        Me.BirthDate.HeaderText = "Ngày sinh"
        Me.BirthDate.Name = "BirthDate"
        Me.BirthDate.ReadOnly = True
        Me.BirthDate.Width = 126
        '
        'Relation
        '
        Me.Relation.HeaderText = "Quan hệ"
        Me.Relation.Name = "Relation"
        Me.Relation.ReadOnly = True
        '
        'lblTitle
        '
        Me.lblTitle.Font = New System.Drawing.Font("Times New Roman", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.lblTitle.Location = New System.Drawing.Point(166, 15)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(215, 48)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Thông tin về con cái"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.GiaPha.My.Resources.Resources.AddChilds48
        Me.PictureBox1.Location = New System.Drawing.Point(113, 15)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(48, 48)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 5
        Me.PictureBox1.TabStop = False
        '
        'btnFindMember
        '
        Me.btnFindMember.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnFindMember.Location = New System.Drawing.Point(197, 503)
        Me.btnFindMember.Name = "btnFindMember"
        Me.btnFindMember.Size = New System.Drawing.Size(100, 34)
        Me.btnFindMember.TabIndex = 4
        Me.btnFindMember.Text = "Tìm kiếm"
        Me.btnFindMember.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnFindMember.UseVisualStyleBackColor = True
        Me.btnFindMember.Visible = False
        '
        'btnOK
        '
        Me.btnOK.Image = Global.GiaPha.My.Resources.Resources.ok24
        Me.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnOK.Location = New System.Drawing.Point(91, 503)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(100, 34)
        Me.btnOK.TabIndex = 3
        Me.btnOK.Text = "Hoàn thành"
        Me.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnOK.UseVisualStyleBackColor = True
        Me.btnOK.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.GiaPha.My.Resources.Resources.back_24
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(303, 503)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 34)
        Me.btnCancel.TabIndex = 5
        Me.btnCancel.Text = "Quay lại"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancel.UseVisualStyleBackColor = True
        Me.btnCancel.Visible = False
        '
        'frmChildsManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(494, 547)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.grpList)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.btnFindMember)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnCancel)
        Me.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmChildsManager"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Quản lý thông tin con cái"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.grpList.ResumeLayout(False)
        CType(Me.dgvChildren, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents dtpBirth As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents grpList As System.Windows.Forms.GroupBox
    Friend WithEvents dgvChildren As System.Windows.Forms.DataGridView
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents btnFindMember As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents ChildName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NameChild As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents BirthDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Relation As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
