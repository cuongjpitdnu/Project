<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSpouseList
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSpouseList))
        Me.lblSearch = New System.Windows.Forms.Label
        Me.txtSearch = New System.Windows.Forms.TextBox
        Me.dgvMemberList = New System.Windows.Forms.DataGridView
        Me.clmHID = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.clmWid = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.clmStt = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.clmHusband = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.clmWife = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.lblResultInfo = New System.Windows.Forms.Label
        Me.btnSearch = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.cbPages = New System.Windows.Forms.ComboBox
        Me.btnLastPage = New System.Windows.Forms.Button
        Me.btnFirstPage = New System.Windows.Forms.Button
        Me.btnNextPage = New System.Windows.Forms.Button
        Me.btnPrePage = New System.Windows.Forms.Button
        CType(Me.dgvMemberList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSearch.Location = New System.Drawing.Point(6, 30)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(91, 16)
        Me.lblSearch.TabIndex = 0
        Me.lblSearch.Text = "Tên thành viên"
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(107, 29)
        Me.txtSearch.MaxLength = 150
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(249, 20)
        Me.txtSearch.TabIndex = 1
        '
        'dgvMemberList
        '
        Me.dgvMemberList.AllowUserToAddRows = False
        Me.dgvMemberList.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvMemberList.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvMemberList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvMemberList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.clmHID, Me.clmWid, Me.clmStt, Me.clmHusband, Me.clmWife})
        Me.dgvMemberList.Location = New System.Drawing.Point(12, 89)
        Me.dgvMemberList.MultiSelect = False
        Me.dgvMemberList.Name = "dgvMemberList"
        Me.dgvMemberList.ReadOnly = True
        Me.dgvMemberList.RowHeadersVisible = False
        Me.dgvMemberList.RowTemplate.ReadOnly = True
        Me.dgvMemberList.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvMemberList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvMemberList.Size = New System.Drawing.Size(478, 424)
        Me.dgvMemberList.TabIndex = 2
        '
        'clmHID
        '
        Me.clmHID.DataPropertyName = "HID"
        Me.clmHID.HeaderText = "HID"
        Me.clmHID.Name = "clmHID"
        Me.clmHID.ReadOnly = True
        Me.clmHID.Visible = False
        '
        'clmWid
        '
        Me.clmWid.DataPropertyName = "WID"
        Me.clmWid.HeaderText = "WID"
        Me.clmWid.Name = "clmWid"
        Me.clmWid.ReadOnly = True
        Me.clmWid.Visible = False
        '
        'clmStt
        '
        Me.clmStt.DataPropertyName = "STT"
        Me.clmStt.HeaderText = "STT"
        Me.clmStt.MinimumWidth = 35
        Me.clmStt.Name = "clmStt"
        Me.clmStt.ReadOnly = True
        Me.clmStt.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.clmStt.Width = 35
        '
        'clmHusband
        '
        Me.clmHusband.DataPropertyName = "HUSBAND"
        Me.clmHusband.HeaderText = "Chồng"
        Me.clmHusband.MinimumWidth = 220
        Me.clmHusband.Name = "clmHusband"
        Me.clmHusband.ReadOnly = True
        Me.clmHusband.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.clmHusband.Width = 220
        '
        'clmWife
        '
        Me.clmWife.DataPropertyName = "WIFE"
        Me.clmWife.HeaderText = "Vợ"
        Me.clmWife.MinimumWidth = 220
        Me.clmWife.Name = "clmWife"
        Me.clmWife.ReadOnly = True
        Me.clmWife.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.clmWife.Width = 220
        '
        'btnOk
        '
        Me.btnOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOk.Image = Global.GiaPha.My.Resources.Resources.task_done
        Me.btnOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnOk.Location = New System.Drawing.Point(117, 561)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(115, 37)
        Me.btnOk.TabIndex = 3
        Me.btnOk.Text = "         Hoàn tất"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Image = Global.GiaPha.My.Resources.Resources.back_32
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(283, 561)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(115, 37)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "        Quay lại"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lblResultInfo
        '
        Me.lblResultInfo.AutoSize = True
        Me.lblResultInfo.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.lblResultInfo.Location = New System.Drawing.Point(16, 526)
        Me.lblResultInfo.Name = "lblResultInfo"
        Me.lblResultInfo.Size = New System.Drawing.Size(0, 14)
        Me.lblResultInfo.TabIndex = 4
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        Me.btnSearch.Image = Global.GiaPha.My.Resources.Resources.MemberSearch16
        Me.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSearch.Location = New System.Drawing.Point(362, 27)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(103, 25)
        Me.btnSearch.TabIndex = 5
        Me.btnSearch.Text = "   Tìm kiếm"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnSearch)
        Me.GroupBox1.Controls.Add(Me.lblSearch)
        Me.GroupBox1.Controls.Add(Me.txtSearch)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(478, 71)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Tìm kiếm thành vên"
        '
        'cbPages
        '
        Me.cbPages.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbPages.FormattingEnabled = True
        Me.cbPages.Location = New System.Drawing.Point(374, 518)
        Me.cbPages.Name = "cbPages"
        Me.cbPages.Size = New System.Drawing.Size(40, 22)
        Me.cbPages.TabIndex = 32
        '
        'btnLastPage
        '
        Me.btnLastPage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLastPage.Location = New System.Drawing.Point(444, 518)
        Me.btnLastPage.Name = "btnLastPage"
        Me.btnLastPage.Size = New System.Drawing.Size(37, 23)
        Me.btnLastPage.TabIndex = 34
        Me.btnLastPage.Text = ">>"
        Me.btnLastPage.UseVisualStyleBackColor = True
        '
        'btnFirstPage
        '
        Me.btnFirstPage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFirstPage.Location = New System.Drawing.Point(306, 518)
        Me.btnFirstPage.Name = "btnFirstPage"
        Me.btnFirstPage.Size = New System.Drawing.Size(37, 23)
        Me.btnFirstPage.TabIndex = 30
        Me.btnFirstPage.Text = "<<"
        Me.btnFirstPage.UseVisualStyleBackColor = True
        '
        'btnNextPage
        '
        Me.btnNextPage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNextPage.Location = New System.Drawing.Point(420, 518)
        Me.btnNextPage.Name = "btnNextPage"
        Me.btnNextPage.Size = New System.Drawing.Size(18, 23)
        Me.btnNextPage.TabIndex = 33
        Me.btnNextPage.Text = ">"
        Me.btnNextPage.UseVisualStyleBackColor = True
        '
        'btnPrePage
        '
        Me.btnPrePage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPrePage.Location = New System.Drawing.Point(351, 518)
        Me.btnPrePage.Name = "btnPrePage"
        Me.btnPrePage.Size = New System.Drawing.Size(18, 23)
        Me.btnPrePage.TabIndex = 31
        Me.btnPrePage.Text = "<"
        Me.btnPrePage.UseVisualStyleBackColor = True
        '
        'frmSpouseList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(506, 614)
        Me.Controls.Add(Me.cbPages)
        Me.Controls.Add(Me.btnLastPage)
        Me.Controls.Add(Me.btnFirstPage)
        Me.Controls.Add(Me.btnNextPage)
        Me.Controls.Add(Me.btnPrePage)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lblResultInfo)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.dgvMemberList)
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmSpouseList"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Danh sách cha mẹ"
        CType(Me.dgvMemberList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblSearch As System.Windows.Forms.Label
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents dgvMemberList As System.Windows.Forms.DataGridView
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblResultInfo As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cbPages As System.Windows.Forms.ComboBox
    Friend WithEvents btnLastPage As System.Windows.Forms.Button
    Friend WithEvents btnFirstPage As System.Windows.Forms.Button
    Friend WithEvents btnNextPage As System.Windows.Forms.Button
    Friend WithEvents btnPrePage As System.Windows.Forms.Button
    Friend WithEvents clmHID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmWid As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmStt As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmHusband As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmWife As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
