<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPersonList
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPersonList))
        Me.grpSearch = New System.Windows.Forms.GroupBox
        Me.btnSearch = New System.Windows.Forms.Button
        Me.rdAll = New System.Windows.Forms.RadioButton
        Me.rdFemale = New System.Windows.Forms.RadioButton
        Me.rdMale = New System.Windows.Forms.RadioButton
        Me.txtSearch = New System.Windows.Forms.TextBox
        Me.lblSearch = New System.Windows.Forms.Label
        Me.dgvMemberList = New System.Windows.Forms.DataGridView
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.lblResultInfo = New System.Windows.Forms.Label
        Me.cbPages = New System.Windows.Forms.ComboBox
        Me.btnLastPage = New System.Windows.Forms.Button
        Me.btnFirstPage = New System.Windows.Forms.Button
        Me.btnNextPage = New System.Windows.Forms.Button
        Me.btnPrePage = New System.Windows.Forms.Button
        Me.clmMemberID = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.clmIcon = New System.Windows.Forms.DataGridViewImageColumn
        Me.clmStt = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.clmName = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.clmBirthDate = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.clmRel = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.grpSearch.SuspendLayout()
        CType(Me.dgvMemberList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpSearch
        '
        Me.grpSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSearch.Controls.Add(Me.btnSearch)
        Me.grpSearch.Controls.Add(Me.rdAll)
        Me.grpSearch.Controls.Add(Me.rdFemale)
        Me.grpSearch.Controls.Add(Me.rdMale)
        Me.grpSearch.Controls.Add(Me.txtSearch)
        Me.grpSearch.Controls.Add(Me.lblSearch)
        Me.grpSearch.Location = New System.Drawing.Point(12, 13)
        Me.grpSearch.Name = "grpSearch"
        Me.grpSearch.Size = New System.Drawing.Size(485, 92)
        Me.grpSearch.TabIndex = 0
        Me.grpSearch.TabStop = False
        Me.grpSearch.Text = "Tìm kiếm thành viên"
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearch.Image = Global.GiaPha.My.Resources.Resources.MemberSearch16
        Me.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSearch.Location = New System.Drawing.Point(381, 21)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(89, 24)
        Me.btnSearch.TabIndex = 3
        Me.btnSearch.Text = "Tìm kiếm"
        Me.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'rdAll
        '
        Me.rdAll.AutoSize = True
        Me.rdAll.Checked = True
        Me.rdAll.Location = New System.Drawing.Point(101, 58)
        Me.rdAll.Name = "rdAll"
        Me.rdAll.Size = New System.Drawing.Size(55, 18)
        Me.rdAll.TabIndex = 2
        Me.rdAll.TabStop = True
        Me.rdAll.Text = "Tất cả"
        Me.rdAll.UseVisualStyleBackColor = True
        '
        'rdFemale
        '
        Me.rdFemale.AutoSize = True
        Me.rdFemale.Location = New System.Drawing.Point(294, 58)
        Me.rdFemale.Name = "rdFemale"
        Me.rdFemale.Size = New System.Drawing.Size(39, 18)
        Me.rdFemale.TabIndex = 2
        Me.rdFemale.Text = "Nữ"
        Me.rdFemale.UseVisualStyleBackColor = True
        '
        'rdMale
        '
        Me.rdMale.AutoSize = True
        Me.rdMale.Location = New System.Drawing.Point(198, 58)
        Me.rdMale.Name = "rdMale"
        Me.rdMale.Size = New System.Drawing.Size(46, 18)
        Me.rdMale.TabIndex = 2
        Me.rdMale.Text = "Nam"
        Me.rdMale.UseVisualStyleBackColor = True
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(99, 23)
        Me.txtSearch.MaxLength = 150
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(276, 20)
        Me.txtSearch.TabIndex = 1
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSearch.Location = New System.Drawing.Point(6, 26)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(87, 15)
        Me.lblSearch.TabIndex = 0
        Me.lblSearch.Text = "Tên thành viên"
        '
        'dgvMemberList
        '
        Me.dgvMemberList.AllowUserToAddRows = False
        Me.dgvMemberList.AllowUserToDeleteRows = False
        Me.dgvMemberList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvMemberList.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvMemberList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvMemberList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.clmMemberID, Me.clmIcon, Me.clmStt, Me.clmName, Me.clmBirthDate, Me.clmRel})
        Me.dgvMemberList.Location = New System.Drawing.Point(12, 111)
        Me.dgvMemberList.MultiSelect = False
        Me.dgvMemberList.Name = "dgvMemberList"
        Me.dgvMemberList.ReadOnly = True
        Me.dgvMemberList.RowHeadersVisible = False
        Me.dgvMemberList.RowTemplate.ReadOnly = True
        Me.dgvMemberList.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvMemberList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvMemberList.Size = New System.Drawing.Size(485, 420)
        Me.dgvMemberList.TabIndex = 1
        '
        'btnOk
        '
        Me.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOk.Image = Global.GiaPha.My.Resources.Resources.task_done
        Me.btnOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnOk.Location = New System.Drawing.Point(114, 584)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(113, 41)
        Me.btnOk.TabIndex = 2
        Me.btnOk.Text = "      Hoàn tất"
        Me.btnOk.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Image = Global.GiaPha.My.Resources.Resources.back_32
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(270, 584)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(117, 41)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "      Quay lại"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lblResultInfo
        '
        Me.lblResultInfo.AutoSize = True
        Me.lblResultInfo.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.lblResultInfo.Location = New System.Drawing.Point(16, 540)
        Me.lblResultInfo.Name = "lblResultInfo"
        Me.lblResultInfo.Size = New System.Drawing.Size(0, 14)
        Me.lblResultInfo.TabIndex = 3
        '
        'cbPages
        '
        Me.cbPages.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbPages.FormattingEnabled = True
        Me.cbPages.Location = New System.Drawing.Point(381, 538)
        Me.cbPages.Name = "cbPages"
        Me.cbPages.Size = New System.Drawing.Size(40, 22)
        Me.cbPages.TabIndex = 27
        '
        'btnLastPage
        '
        Me.btnLastPage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLastPage.Location = New System.Drawing.Point(451, 538)
        Me.btnLastPage.Name = "btnLastPage"
        Me.btnLastPage.Size = New System.Drawing.Size(37, 23)
        Me.btnLastPage.TabIndex = 29
        Me.btnLastPage.Text = ">>"
        Me.btnLastPage.UseVisualStyleBackColor = True
        '
        'btnFirstPage
        '
        Me.btnFirstPage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFirstPage.Location = New System.Drawing.Point(313, 538)
        Me.btnFirstPage.Name = "btnFirstPage"
        Me.btnFirstPage.Size = New System.Drawing.Size(37, 23)
        Me.btnFirstPage.TabIndex = 25
        Me.btnFirstPage.Text = "<<"
        Me.btnFirstPage.UseVisualStyleBackColor = True
        '
        'btnNextPage
        '
        Me.btnNextPage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNextPage.Location = New System.Drawing.Point(427, 538)
        Me.btnNextPage.Name = "btnNextPage"
        Me.btnNextPage.Size = New System.Drawing.Size(18, 23)
        Me.btnNextPage.TabIndex = 28
        Me.btnNextPage.Text = ">"
        Me.btnNextPage.UseVisualStyleBackColor = True
        '
        'btnPrePage
        '
        Me.btnPrePage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPrePage.Location = New System.Drawing.Point(358, 538)
        Me.btnPrePage.Name = "btnPrePage"
        Me.btnPrePage.Size = New System.Drawing.Size(18, 23)
        Me.btnPrePage.TabIndex = 26
        Me.btnPrePage.Text = "<"
        Me.btnPrePage.UseVisualStyleBackColor = True
        '
        'clmMemberID
        '
        Me.clmMemberID.DataPropertyName = "MEMBER_ID"
        Me.clmMemberID.HeaderText = "MemberID"
        Me.clmMemberID.Name = "clmMemberID"
        Me.clmMemberID.ReadOnly = True
        Me.clmMemberID.Visible = False
        '
        'clmIcon
        '
        Me.clmIcon.DataPropertyName = "GENDER"
        Me.clmIcon.HeaderText = "GT"
        Me.clmIcon.MinimumWidth = 35
        Me.clmIcon.Name = "clmIcon"
        Me.clmIcon.ReadOnly = True
        Me.clmIcon.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.clmIcon.Width = 35
        '
        'clmStt
        '
        Me.clmStt.DataPropertyName = "STT"
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
        Me.clmName.DataPropertyName = "FULL_NAME"
        Me.clmName.HeaderText = "Họ tên"
        Me.clmName.MinimumWidth = 180
        Me.clmName.Name = "clmName"
        Me.clmName.ReadOnly = True
        Me.clmName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.clmName.Width = 205
        '
        'clmBirthDate
        '
        Me.clmBirthDate.DataPropertyName = "BDATE"
        Me.clmBirthDate.HeaderText = "Ngày sinh"
        Me.clmBirthDate.MinimumWidth = 90
        Me.clmBirthDate.Name = "clmBirthDate"
        Me.clmBirthDate.ReadOnly = True
        Me.clmBirthDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.clmBirthDate.Width = 90
        '
        'clmRel
        '
        Me.clmRel.DataPropertyName = "RELATIONSHIP"
        Me.clmRel.HeaderText = "Quan hệ"
        Me.clmRel.MinimumWidth = 100
        Me.clmRel.Name = "clmRel"
        Me.clmRel.ReadOnly = True
        Me.clmRel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'frmPersonList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(509, 637)
        Me.Controls.Add(Me.cbPages)
        Me.Controls.Add(Me.btnLastPage)
        Me.Controls.Add(Me.btnFirstPage)
        Me.Controls.Add(Me.btnNextPage)
        Me.Controls.Add(Me.btnPrePage)
        Me.Controls.Add(Me.lblResultInfo)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.dgvMemberList)
        Me.Controls.Add(Me.grpSearch)
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmPersonList"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Danh sách thành viên"
        Me.grpSearch.ResumeLayout(False)
        Me.grpSearch.PerformLayout()
        CType(Me.dgvMemberList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents grpSearch As System.Windows.Forms.GroupBox
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents lblSearch As System.Windows.Forms.Label
    Friend WithEvents rdFemale As System.Windows.Forms.RadioButton
    Friend WithEvents rdMale As System.Windows.Forms.RadioButton
    Friend WithEvents dgvMemberList As System.Windows.Forms.DataGridView
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents rdAll As System.Windows.Forms.RadioButton
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents lblResultInfo As System.Windows.Forms.Label
    Friend WithEvents cbPages As System.Windows.Forms.ComboBox
    Friend WithEvents btnLastPage As System.Windows.Forms.Button
    Friend WithEvents btnFirstPage As System.Windows.Forms.Button
    Friend WithEvents btnNextPage As System.Windows.Forms.Button
    Friend WithEvents btnPrePage As System.Windows.Forms.Button
    Friend WithEvents clmMemberID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmIcon As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents clmStt As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmBirthDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmRel As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
