<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmPersonalAnniversary
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPersonalAnniversary))
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.dgvMemberList = New System.Windows.Forms.DataGridView()
        Me.clmMemberID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clmIcon = New System.Windows.Forms.DataGridViewImageColumn()
        Me.clmStt = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clmName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clmBirthDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clmAge = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clmEventDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnExcelExport = New System.Windows.Forms.Button()
        CType(Me.dgvMemberList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblTitle
        '
        Me.lblTitle.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        Me.lblTitle.ForeColor = System.Drawing.Color.Blue
        Me.lblTitle.Location = New System.Drawing.Point(10, 23)
        Me.lblTitle.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(650, 20)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "DANH SÁCH SINH NHẬT GẦN NHẤT"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dgvMemberList
        '
        Me.dgvMemberList.AllowUserToAddRows = False
        Me.dgvMemberList.AllowUserToDeleteRows = False
        Me.dgvMemberList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvMemberList.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvMemberList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvMemberList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.clmMemberID, Me.clmIcon, Me.clmStt, Me.clmName, Me.clmBirthDate, Me.clmAge, Me.clmEventDate})
        Me.dgvMemberList.Location = New System.Drawing.Point(10, 75)
        Me.dgvMemberList.MultiSelect = False
        Me.dgvMemberList.Name = "dgvMemberList"
        Me.dgvMemberList.ReadOnly = True
        Me.dgvMemberList.RowHeadersVisible = False
        Me.dgvMemberList.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvMemberList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvMemberList.Size = New System.Drawing.Size(976, 362)
        Me.dgvMemberList.TabIndex = 2
        '
        'clmMemberID
        '
        Me.clmMemberID.HeaderText = "MemberID"
        Me.clmMemberID.Name = "clmMemberID"
        Me.clmMemberID.ReadOnly = True
        Me.clmMemberID.Visible = False
        '
        'clmIcon
        '
        Me.clmIcon.HeaderText = "GT"
        Me.clmIcon.MinimumWidth = 35
        Me.clmIcon.Name = "clmIcon"
        Me.clmIcon.ReadOnly = True
        Me.clmIcon.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.clmIcon.Width = 35
        '
        'clmStt
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.clmStt.DefaultCellStyle = DataGridViewCellStyle2
        Me.clmStt.HeaderText = "STT"
        Me.clmStt.MinimumWidth = 35
        Me.clmStt.Name = "clmStt"
        Me.clmStt.ReadOnly = True
        Me.clmStt.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.clmStt.Width = 35
        '
        'clmName
        '
        Me.clmName.HeaderText = "Họ tên"
        Me.clmName.MinimumWidth = 180
        Me.clmName.Name = "clmName"
        Me.clmName.ReadOnly = True
        Me.clmName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.clmName.Width = 185
        '
        'clmBirthDate
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.clmBirthDate.DefaultCellStyle = DataGridViewCellStyle3
        Me.clmBirthDate.HeaderText = "Ngày sinh"
        Me.clmBirthDate.MinimumWidth = 100
        Me.clmBirthDate.Name = "clmBirthDate"
        Me.clmBirthDate.ReadOnly = True
        Me.clmBirthDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.clmBirthDate.Width = 135
        '
        'clmAge
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.clmAge.DefaultCellStyle = DataGridViewCellStyle4
        Me.clmAge.HeaderText = "Tuổi"
        Me.clmAge.MinimumWidth = 75
        Me.clmAge.Name = "clmAge"
        Me.clmAge.ReadOnly = True
        Me.clmAge.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.clmAge.Width = 75
        '
        'clmEventDate
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.clmEventDate.DefaultCellStyle = DataGridViewCellStyle5
        Me.clmEventDate.HeaderText = "Ngày kỷ niệm"
        Me.clmEventDate.MinimumWidth = 165
        Me.clmEventDate.Name = "clmEventDate"
        Me.clmEventDate.ReadOnly = True
        Me.clmEventDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.clmEventDate.Width = 340
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.GiaPha.My.Resources.Resources.back_24
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(523, 458)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(97, 43)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Quay lại"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnExcelExport
        '
        Me.btnExcelExport.Image = Global.GiaPha.My.Resources.Resources.Excel
        Me.btnExcelExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExcelExport.Location = New System.Drawing.Point(316, 458)
        Me.btnExcelExport.Name = "btnExcelExport"
        Me.btnExcelExport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnExcelExport.Size = New System.Drawing.Size(135, 43)
        Me.btnExcelExport.TabIndex = 19
        Me.btnExcelExport.Text = "Xuất ra Excel"
        Me.btnExcelExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExcelExport.UseVisualStyleBackColor = True
        '
        'frmPersonalAnniversary
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(997, 521)
        Me.Controls.Add(Me.btnExcelExport)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.dgvMemberList)
        Me.Controls.Add(Me.lblTitle)
        Me.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.Name = "frmPersonalAnniversary"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Danh sách các sinh nhật gần nhất"
        CType(Me.dgvMemberList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents dgvMemberList As System.Windows.Forms.DataGridView
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents clmMemberID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmIcon As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents clmStt As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmBirthDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmAge As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmEventDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents btnExcelExport As System.Windows.Forms.Button
End Class
