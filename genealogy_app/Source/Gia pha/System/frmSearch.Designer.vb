<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSearch
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSearch))
        Me.dgvSearchMember = New System.Windows.Forms.DataGridView()
        Me.clmNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clmLevel = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clmFullName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clmGender = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clmContact = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clmHometown = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clmBirthDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clmDeceaseDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clmRemark = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.tbcSearch = New System.Windows.Forms.TabControl()
        Me.tbpBase = New System.Windows.Forms.TabPage()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.btnDieFrom = New System.Windows.Forms.Button()
        Me.dtpDieFrom = New System.Windows.Forms.DateTimePicker()
        Me.lblDeaTo = New System.Windows.Forms.Label()
        Me.lblDeaFrom = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.txtPosition = New System.Windows.Forms.TextBox()
        Me.dtpDieTo = New System.Windows.Forms.DateTimePicker()
        Me.txtOccupt = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.btnDieTo = New System.Windows.Forms.Button()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.lblDieFrom = New System.Windows.Forms.Label()
        Me.btnBirthTo = New System.Windows.Forms.Button()
        Me.lblDieTo = New System.Windows.Forms.Label()
        Me.lblBirthTo = New System.Windows.Forms.Label()
        Me.lblBirthFrom = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.dtpBirthTo = New System.Windows.Forms.DateTimePicker()
        Me.btnDeaTo = New System.Windows.Forms.Button()
        Me.btnDeaFrom = New System.Windows.Forms.Button()
        Me.btnBirthFrom = New System.Windows.Forms.Button()
        Me.chkDie = New System.Windows.Forms.CheckBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.dtpBirthFrom = New System.Windows.Forms.DateTimePicker()
        Me.cbGender = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtKeyword = New System.Windows.Forms.TextBox()
        Me.tbpMoreInfo = New System.Windows.Forms.TabPage()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cboPosLevelName = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.cbPages = New System.Windows.Forms.ComboBox()
        Me.btnLastPage = New System.Windows.Forms.Button()
        Me.btnFirstPage = New System.Windows.Forms.Button()
        Me.btnNextPage = New System.Windows.Forms.Button()
        Me.btnPrePage = New System.Windows.Forms.Button()
        CType(Me.dgvSearchMember, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tbcSearch.SuspendLayout()
        Me.tbpBase.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.tbpMoreInfo.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvSearchMember
        '
        Me.dgvSearchMember.AllowUserToAddRows = False
        Me.dgvSearchMember.AllowUserToDeleteRows = False
        Me.dgvSearchMember.AllowUserToResizeRows = False
        Me.dgvSearchMember.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvSearchMember.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvSearchMember.ColumnHeadersHeight = 25
        Me.dgvSearchMember.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.clmNo, Me.clmLevel, Me.clmFullName, Me.clmGender, Me.clmContact, Me.clmHometown, Me.clmBirthDate, Me.clmDeceaseDate, Me.clmRemark})
        Me.dgvSearchMember.Location = New System.Drawing.Point(12, 341)
        Me.dgvSearchMember.MultiSelect = False
        Me.dgvSearchMember.Name = "dgvSearchMember"
        Me.dgvSearchMember.ReadOnly = True
        Me.dgvSearchMember.RowHeadersVisible = False
        Me.dgvSearchMember.RowHeadersWidth = 30
        Me.dgvSearchMember.RowTemplate.Height = 30
        Me.dgvSearchMember.RowTemplate.ReadOnly = True
        Me.dgvSearchMember.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvSearchMember.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvSearchMember.Size = New System.Drawing.Size(907, 308)
        Me.dgvSearchMember.TabIndex = 18
        '
        'clmNo
        '
        Me.clmNo.DataPropertyName = "STT"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter
        Me.clmNo.DefaultCellStyle = DataGridViewCellStyle2
        Me.clmNo.HeaderText = "STT"
        Me.clmNo.MinimumWidth = 35
        Me.clmNo.Name = "clmNo"
        Me.clmNo.ReadOnly = True
        Me.clmNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.clmNo.Width = 35
        '
        'clmLevel
        '
        Me.clmLevel.DataPropertyName = "GEN"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter
        Me.clmLevel.DefaultCellStyle = DataGridViewCellStyle3
        Me.clmLevel.HeaderText = "Đời"
        Me.clmLevel.MinimumWidth = 40
        Me.clmLevel.Name = "clmLevel"
        Me.clmLevel.ReadOnly = True
        Me.clmLevel.Width = 40
        '
        'clmFullName
        '
        Me.clmFullName.DataPropertyName = "NAME"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.clmFullName.DefaultCellStyle = DataGridViewCellStyle4
        Me.clmFullName.HeaderText = "Họ và tên"
        Me.clmFullName.MinimumWidth = 220
        Me.clmFullName.Name = "clmFullName"
        Me.clmFullName.ReadOnly = True
        Me.clmFullName.Width = 220
        '
        'clmGender
        '
        Me.clmGender.DataPropertyName = "GENDER"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter
        Me.clmGender.DefaultCellStyle = DataGridViewCellStyle5
        Me.clmGender.HeaderText = "Giới tính"
        Me.clmGender.MinimumWidth = 80
        Me.clmGender.Name = "clmGender"
        Me.clmGender.ReadOnly = True
        Me.clmGender.Width = 80
        '
        'clmContact
        '
        Me.clmContact.DataPropertyName = "CONTACT"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.clmContact.DefaultCellStyle = DataGridViewCellStyle6
        Me.clmContact.HeaderText = "Liên lạc"
        Me.clmContact.MinimumWidth = 200
        Me.clmContact.Name = "clmContact"
        Me.clmContact.ReadOnly = True
        Me.clmContact.Width = 200
        '
        'clmHometown
        '
        Me.clmHometown.DataPropertyName = "HOME"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.clmHometown.DefaultCellStyle = DataGridViewCellStyle7
        Me.clmHometown.HeaderText = "Quê quán"
        Me.clmHometown.MinimumWidth = 175
        Me.clmHometown.Name = "clmHometown"
        Me.clmHometown.ReadOnly = True
        Me.clmHometown.Width = 175
        '
        'clmBirthDate
        '
        Me.clmBirthDate.DataPropertyName = "BIRTH"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter
        Me.clmBirthDate.DefaultCellStyle = DataGridViewCellStyle8
        Me.clmBirthDate.HeaderText = "Ngày sinh"
        Me.clmBirthDate.MinimumWidth = 90
        Me.clmBirthDate.Name = "clmBirthDate"
        Me.clmBirthDate.ReadOnly = True
        Me.clmBirthDate.Width = 90
        '
        'clmDeceaseDate
        '
        Me.clmDeceaseDate.DataPropertyName = "DECEASE"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter
        Me.clmDeceaseDate.DefaultCellStyle = DataGridViewCellStyle9
        Me.clmDeceaseDate.HeaderText = "Ngày mất"
        Me.clmDeceaseDate.Name = "clmDeceaseDate"
        Me.clmDeceaseDate.ReadOnly = True
        '
        'clmRemark
        '
        Me.clmRemark.DataPropertyName = "REMARK"
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        DataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.clmRemark.DefaultCellStyle = DataGridViewCellStyle10
        Me.clmRemark.HeaderText = "Ghi chú"
        Me.clmRemark.MinimumWidth = 220
        Me.clmRemark.Name = "clmRemark"
        Me.clmRemark.ReadOnly = True
        Me.clmRemark.Width = 220
        '
        'tbcSearch
        '
        Me.tbcSearch.Controls.Add(Me.tbpBase)
        Me.tbcSearch.Controls.Add(Me.tbpMoreInfo)
        Me.tbcSearch.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        Me.tbcSearch.Location = New System.Drawing.Point(12, 13)
        Me.tbcSearch.Name = "tbcSearch"
        Me.tbcSearch.SelectedIndex = 0
        Me.tbcSearch.Size = New System.Drawing.Size(907, 265)
        Me.tbcSearch.TabIndex = 1
        '
        'tbpBase
        '
        Me.tbpBase.BackColor = System.Drawing.Color.Transparent
        Me.tbpBase.Controls.Add(Me.GroupBox2)
        Me.tbpBase.Location = New System.Drawing.Point(4, 25)
        Me.tbpBase.Name = "tbpBase"
        Me.tbpBase.Padding = New System.Windows.Forms.Padding(3)
        Me.tbpBase.Size = New System.Drawing.Size(899, 236)
        Me.tbpBase.TabIndex = 0
        Me.tbpBase.Text = "Thông tin cơ bản"
        Me.tbpBase.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.btnDieFrom)
        Me.GroupBox2.Controls.Add(Me.dtpDieFrom)
        Me.GroupBox2.Controls.Add(Me.lblDeaTo)
        Me.GroupBox2.Controls.Add(Me.lblDeaFrom)
        Me.GroupBox2.Controls.Add(Me.Label14)
        Me.GroupBox2.Controls.Add(Me.txtPosition)
        Me.GroupBox2.Controls.Add(Me.dtpDieTo)
        Me.GroupBox2.Controls.Add(Me.txtOccupt)
        Me.GroupBox2.Controls.Add(Me.Label13)
        Me.GroupBox2.Controls.Add(Me.Label10)
        Me.GroupBox2.Controls.Add(Me.btnDieTo)
        Me.GroupBox2.Controls.Add(Me.Label18)
        Me.GroupBox2.Controls.Add(Me.lblDieFrom)
        Me.GroupBox2.Controls.Add(Me.btnBirthTo)
        Me.GroupBox2.Controls.Add(Me.lblDieTo)
        Me.GroupBox2.Controls.Add(Me.lblBirthTo)
        Me.GroupBox2.Controls.Add(Me.lblBirthFrom)
        Me.GroupBox2.Controls.Add(Me.Label11)
        Me.GroupBox2.Controls.Add(Me.Label12)
        Me.GroupBox2.Controls.Add(Me.dtpBirthTo)
        Me.GroupBox2.Controls.Add(Me.btnDeaTo)
        Me.GroupBox2.Controls.Add(Me.btnDeaFrom)
        Me.GroupBox2.Controls.Add(Me.btnBirthFrom)
        Me.GroupBox2.Controls.Add(Me.chkDie)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.dtpBirthFrom)
        Me.GroupBox2.Controls.Add(Me.cbGender)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.txtKeyword)
        Me.GroupBox2.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(10, 6)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(881, 221)
        Me.GroupBox2.TabIndex = 12
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Điều kiện tìm kiếm"
        '
        'btnDieFrom
        '
        Me.btnDieFrom.Enabled = False
        Me.btnDieFrom.Location = New System.Drawing.Point(635, 164)
        Me.btnDieFrom.Name = "btnDieFrom"
        Me.btnDieFrom.Size = New System.Drawing.Size(39, 31)
        Me.btnDieFrom.TabIndex = 12
        Me.btnDieFrom.Text = "ÂL"
        Me.btnDieFrom.UseVisualStyleBackColor = True
        Me.btnDieFrom.Visible = False
        '
        'dtpDieFrom
        '
        Me.dtpDieFrom.Checked = False
        Me.dtpDieFrom.CustomFormat = "dd/MM/yyyy"
        Me.dtpDieFrom.Enabled = False
        Me.dtpDieFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpDieFrom.Location = New System.Drawing.Point(525, 167)
        Me.dtpDieFrom.Name = "dtpDieFrom"
        Me.dtpDieFrom.ShowCheckBox = True
        Me.dtpDieFrom.Size = New System.Drawing.Size(108, 22)
        Me.dtpDieFrom.TabIndex = 11
        Me.dtpDieFrom.Visible = False
        '
        'lblDeaTo
        '
        Me.lblDeaTo.AutoSize = True
        Me.lblDeaTo.Enabled = False
        Me.lblDeaTo.Location = New System.Drawing.Point(339, 171)
        Me.lblDeaTo.Name = "lblDeaTo"
        Me.lblDeaTo.Size = New System.Drawing.Size(55, 16)
        Me.lblDeaTo.TabIndex = 44
        Me.lblDeaTo.Text = "Chưa rõ"
        '
        'lblDeaFrom
        '
        Me.lblDeaFrom.AutoSize = True
        Me.lblDeaFrom.Enabled = False
        Me.lblDeaFrom.Location = New System.Drawing.Point(129, 171)
        Me.lblDeaFrom.Name = "lblDeaFrom"
        Me.lblDeaFrom.Size = New System.Drawing.Size(55, 16)
        Me.lblDeaFrom.TabIndex = 44
        Me.lblDeaFrom.Text = "Chưa rõ"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(12, 171)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(63, 16)
        Me.Label14.TabIndex = 44
        Me.Label14.Text = "Ngày giỗ:"
        '
        'txtPosition
        '
        Me.txtPosition.Location = New System.Drawing.Point(728, 67)
        Me.txtPosition.MaxLength = 150
        Me.txtPosition.Name = "txtPosition"
        Me.txtPosition.Size = New System.Drawing.Size(147, 22)
        Me.txtPosition.TabIndex = 9
        '
        'dtpDieTo
        '
        Me.dtpDieTo.Checked = False
        Me.dtpDieTo.CustomFormat = "dd/MM/yyyy"
        Me.dtpDieTo.Enabled = False
        Me.dtpDieTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpDieTo.Location = New System.Drawing.Point(704, 167)
        Me.dtpDieTo.Name = "dtpDieTo"
        Me.dtpDieTo.ShowCheckBox = True
        Me.dtpDieTo.Size = New System.Drawing.Size(109, 22)
        Me.dtpDieTo.TabIndex = 13
        Me.dtpDieTo.Visible = False
        '
        'txtOccupt
        '
        Me.txtOccupt.Location = New System.Drawing.Point(515, 67)
        Me.txtOccupt.MaxLength = 150
        Me.txtOccupt.Name = "txtOccupt"
        Me.txtOccupt.Size = New System.Drawing.Size(143, 22)
        Me.txtOccupt.TabIndex = 8
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Enabled = False
        Me.Label13.Location = New System.Drawing.Point(682, 174)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(16, 16)
        Me.Label13.TabIndex = 48
        Me.Label13.Text = "~"
        Me.Label13.Visible = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(664, 70)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(64, 16)
        Me.Label10.TabIndex = 56
        Me.Label10.Text = "Chức vụ :"
        '
        'btnDieTo
        '
        Me.btnDieTo.Enabled = False
        Me.btnDieTo.Location = New System.Drawing.Point(819, 164)
        Me.btnDieTo.Name = "btnDieTo"
        Me.btnDieTo.Size = New System.Drawing.Size(39, 31)
        Me.btnDieTo.TabIndex = 14
        Me.btnDieTo.Text = "ÂL"
        Me.btnDieTo.UseVisualStyleBackColor = True
        Me.btnDieTo.Visible = False
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(424, 70)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(88, 16)
        Me.Label18.TabIndex = 57
        Me.Label18.Text = "Nghề nghiệp :"
        '
        'lblDieFrom
        '
        Me.lblDieFrom.AutoSize = True
        Me.lblDieFrom.Enabled = False
        Me.lblDieFrom.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        Me.lblDieFrom.ForeColor = System.Drawing.Color.Blue
        Me.lblDieFrom.Location = New System.Drawing.Point(547, 199)
        Me.lblDieFrom.Name = "lblDieFrom"
        Me.lblDieFrom.Size = New System.Drawing.Size(0, 16)
        Me.lblDieFrom.TabIndex = 51
        Me.lblDieFrom.Visible = False
        '
        'btnBirthTo
        '
        Me.btnBirthTo.Location = New System.Drawing.Point(378, 60)
        Me.btnBirthTo.Name = "btnBirthTo"
        Me.btnBirthTo.Size = New System.Drawing.Size(42, 31)
        Me.btnBirthTo.TabIndex = 7
        Me.btnBirthTo.Text = "ÂL"
        Me.btnBirthTo.UseVisualStyleBackColor = True
        '
        'lblDieTo
        '
        Me.lblDieTo.AutoSize = True
        Me.lblDieTo.Enabled = False
        Me.lblDieTo.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        Me.lblDieTo.ForeColor = System.Drawing.Color.Blue
        Me.lblDieTo.Location = New System.Drawing.Point(729, 199)
        Me.lblDieTo.Name = "lblDieTo"
        Me.lblDieTo.Size = New System.Drawing.Size(0, 16)
        Me.lblDieTo.TabIndex = 52
        Me.lblDieTo.Visible = False
        '
        'lblBirthTo
        '
        Me.lblBirthTo.AutoSize = True
        Me.lblBirthTo.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        Me.lblBirthTo.ForeColor = System.Drawing.Color.Blue
        Me.lblBirthTo.Location = New System.Drawing.Point(276, 95)
        Me.lblBirthTo.Name = "lblBirthTo"
        Me.lblBirthTo.Size = New System.Drawing.Size(0, 16)
        Me.lblBirthTo.TabIndex = 50
        '
        'lblBirthFrom
        '
        Me.lblBirthFrom.AutoSize = True
        Me.lblBirthFrom.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        Me.lblBirthFrom.ForeColor = System.Drawing.Color.Blue
        Me.lblBirthFrom.Location = New System.Drawing.Point(97, 95)
        Me.lblBirthFrom.Name = "lblBirthFrom"
        Me.lblBirthFrom.Size = New System.Drawing.Size(0, 16)
        Me.lblBirthFrom.TabIndex = 49
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(271, 171)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(16, 16)
        Me.Label11.TabIndex = 42
        Me.Label11.Text = "~"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(242, 69)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(16, 16)
        Me.Label12.TabIndex = 42
        Me.Label12.Text = "~"
        '
        'dtpBirthTo
        '
        Me.dtpBirthTo.Checked = False
        Me.dtpBirthTo.CustomFormat = "dd/MM/yyyy"
        Me.dtpBirthTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpBirthTo.Location = New System.Drawing.Point(263, 63)
        Me.dtpBirthTo.Name = "dtpBirthTo"
        Me.dtpBirthTo.ShowCheckBox = True
        Me.dtpBirthTo.Size = New System.Drawing.Size(109, 22)
        Me.dtpBirthTo.TabIndex = 6
        '
        'btnDeaTo
        '
        Me.btnDeaTo.Enabled = False
        Me.btnDeaTo.Location = New System.Drawing.Point(293, 164)
        Me.btnDeaTo.Name = "btnDeaTo"
        Me.btnDeaTo.Size = New System.Drawing.Size(39, 31)
        Me.btnDeaTo.TabIndex = 13
        Me.btnDeaTo.Text = "ÂL"
        Me.btnDeaTo.UseVisualStyleBackColor = True
        '
        'btnDeaFrom
        '
        Me.btnDeaFrom.Enabled = False
        Me.btnDeaFrom.Location = New System.Drawing.Point(84, 164)
        Me.btnDeaFrom.Name = "btnDeaFrom"
        Me.btnDeaFrom.Size = New System.Drawing.Size(39, 31)
        Me.btnDeaFrom.TabIndex = 11
        Me.btnDeaFrom.Text = "ÂL"
        Me.btnDeaFrom.UseVisualStyleBackColor = True
        '
        'btnBirthFrom
        '
        Me.btnBirthFrom.Location = New System.Drawing.Point(194, 60)
        Me.btnBirthFrom.Name = "btnBirthFrom"
        Me.btnBirthFrom.Size = New System.Drawing.Size(39, 31)
        Me.btnBirthFrom.TabIndex = 5
        Me.btnBirthFrom.Text = "ÂL"
        Me.btnBirthFrom.UseVisualStyleBackColor = True
        '
        'chkDie
        '
        Me.chkDie.AutoSize = True
        Me.chkDie.Location = New System.Drawing.Point(84, 131)
        Me.chkDie.Name = "chkDie"
        Me.chkDie.Size = New System.Drawing.Size(69, 20)
        Me.chkDie.TabIndex = 10
        Me.chkDie.Text = "Đã mất"
        Me.chkDie.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(7, 66)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(70, 16)
        Me.Label9.TabIndex = 36
        Me.Label9.Text = "Ngày sinh:"
        '
        'dtpBirthFrom
        '
        Me.dtpBirthFrom.Checked = False
        Me.dtpBirthFrom.CustomFormat = "dd/MM/yyyy"
        Me.dtpBirthFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpBirthFrom.Location = New System.Drawing.Point(84, 64)
        Me.dtpBirthFrom.Name = "dtpBirthFrom"
        Me.dtpBirthFrom.ShowCheckBox = True
        Me.dtpBirthFrom.Size = New System.Drawing.Size(108, 22)
        Me.dtpBirthFrom.TabIndex = 4
        '
        'cbGender
        '
        Me.cbGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbGender.FormattingEnabled = True
        Me.cbGender.Items.AddRange(New Object() {"Tất cả", "Nam", "Nữ"})
        Me.cbGender.Location = New System.Drawing.Point(655, 26)
        Me.cbGender.Name = "cbGender"
        Me.cbGender.Size = New System.Drawing.Size(107, 24)
        Me.cbGender.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(585, 29)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(66, 16)
        Me.Label2.TabIndex = 11
        Me.Label2.Text = "Giới tính :"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 16)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Từ khóa :"
        '
        'txtKeyword
        '
        Me.txtKeyword.Location = New System.Drawing.Point(84, 26)
        Me.txtKeyword.MaxLength = 150
        Me.txtKeyword.Name = "txtKeyword"
        Me.txtKeyword.Size = New System.Drawing.Size(480, 22)
        Me.txtKeyword.TabIndex = 2
        '
        'tbpMoreInfo
        '
        Me.tbpMoreInfo.Controls.Add(Me.ComboBox1)
        Me.tbpMoreInfo.Controls.Add(Me.TextBox5)
        Me.tbpMoreInfo.Controls.Add(Me.TextBox4)
        Me.tbpMoreInfo.Controls.Add(Me.TextBox3)
        Me.tbpMoreInfo.Controls.Add(Me.TextBox2)
        Me.tbpMoreInfo.Controls.Add(Me.Label5)
        Me.tbpMoreInfo.Controls.Add(Me.Label8)
        Me.tbpMoreInfo.Controls.Add(Me.Label7)
        Me.tbpMoreInfo.Controls.Add(Me.Label6)
        Me.tbpMoreInfo.Controls.Add(Me.cboPosLevelName)
        Me.tbpMoreInfo.Controls.Add(Me.Label4)
        Me.tbpMoreInfo.Controls.Add(Me.Label3)
        Me.tbpMoreInfo.Location = New System.Drawing.Point(4, 25)
        Me.tbpMoreInfo.Name = "tbpMoreInfo"
        Me.tbpMoreInfo.Padding = New System.Windows.Forms.Padding(3)
        Me.tbpMoreInfo.Size = New System.Drawing.Size(899, 236)
        Me.tbpMoreInfo.TabIndex = 1
        Me.tbpMoreInfo.Text = "Thông tin mở rộng"
        Me.tbpMoreInfo.UseVisualStyleBackColor = True
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Items.AddRange(New Object() {"", "Giáo sư", "Phó giáo sư", "Tiến sĩ", "Thạc sĩ", "Đại học", "Cao đẳng", "Trung cấp", "Phổ thông cơ sở", "Phổ thông trung học"})
        Me.ComboBox1.Location = New System.Drawing.Point(98, 50)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(147, 24)
        Me.ComboBox1.TabIndex = 22
        '
        'TextBox5
        '
        Me.TextBox5.Location = New System.Drawing.Point(586, 53)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.Size = New System.Drawing.Size(147, 22)
        Me.TextBox5.TabIndex = 24
        '
        'TextBox4
        '
        Me.TextBox4.Location = New System.Drawing.Point(366, 53)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(147, 22)
        Me.TextBox4.TabIndex = 23
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(248, 6)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(147, 22)
        Me.TextBox3.TabIndex = 20
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(98, 6)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(44, 22)
        Me.TextBox2.TabIndex = 19
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(178, 10)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(72, 16)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Thuộc chi :"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(522, 57)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(64, 16)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Chức vụ :"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(278, 57)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(88, 16)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Nghề nghiệp :"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(29, 53)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(71, 16)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Bằng cấp :"
        '
        'cboPosLevelName
        '
        Me.cboPosLevelName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPosLevelName.FormattingEnabled = True
        Me.cboPosLevelName.Items.AddRange(New Object() {"Không có", "Trưởng tộc", "Trưởng họ", "Trưởng chi", "Con trưởng", "Dâu trưởng", "Rể trưởng"})
        Me.cboPosLevelName.Location = New System.Drawing.Point(586, 6)
        Me.cboPosLevelName.Name = "cboPosLevelName"
        Me.cboPosLevelName.Size = New System.Drawing.Size(147, 24)
        Me.cboPosLevelName.TabIndex = 21
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(39, 10)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(61, 16)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Đời thứ :"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(454, 10)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(136, 16)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Vai trò trong dòng họ :"
        '
        'btnPrint
        '
        Me.btnPrint.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        Me.btnPrint.Image = Global.GiaPha.My.Resources.Resources.printer24
        Me.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPrint.Location = New System.Drawing.Point(684, 285)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(129, 41)
        Me.btnPrint.TabIndex = 31
        Me.btnPrint.Text = "Xuất ra Excel"
        Me.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        Me.btnCancel.Image = Global.GiaPha.My.Resources.Resources.back_24
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(819, 285)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(96, 41)
        Me.btnCancel.TabIndex = 32
        Me.btnCancel.Text = "Quay lại"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        Me.btnSearch.Image = Global.GiaPha.My.Resources.Resources.MemberSearch24
        Me.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSearch.Location = New System.Drawing.Point(582, 285)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(96, 41)
        Me.btnSearch.TabIndex = 30
        Me.btnSearch.Text = "Tìm kiếm"
        Me.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'cbPages
        '
        Me.cbPages.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbPages.FormattingEnabled = True
        Me.cbPages.Location = New System.Drawing.Point(799, 660)
        Me.cbPages.Name = "cbPages"
        Me.cbPages.Size = New System.Drawing.Size(40, 24)
        Me.cbPages.TabIndex = 35
        '
        'btnLastPage
        '
        Me.btnLastPage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLastPage.Location = New System.Drawing.Point(869, 661)
        Me.btnLastPage.Name = "btnLastPage"
        Me.btnLastPage.Size = New System.Drawing.Size(37, 23)
        Me.btnLastPage.TabIndex = 37
        Me.btnLastPage.Text = ">>"
        Me.btnLastPage.UseVisualStyleBackColor = True
        '
        'btnFirstPage
        '
        Me.btnFirstPage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFirstPage.Location = New System.Drawing.Point(731, 661)
        Me.btnFirstPage.Name = "btnFirstPage"
        Me.btnFirstPage.Size = New System.Drawing.Size(37, 23)
        Me.btnFirstPage.TabIndex = 33
        Me.btnFirstPage.Text = "<<"
        Me.btnFirstPage.UseVisualStyleBackColor = True
        '
        'btnNextPage
        '
        Me.btnNextPage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNextPage.Location = New System.Drawing.Point(845, 661)
        Me.btnNextPage.Name = "btnNextPage"
        Me.btnNextPage.Size = New System.Drawing.Size(18, 23)
        Me.btnNextPage.TabIndex = 36
        Me.btnNextPage.Text = ">"
        Me.btnNextPage.UseVisualStyleBackColor = True
        '
        'btnPrePage
        '
        Me.btnPrePage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPrePage.Location = New System.Drawing.Point(776, 661)
        Me.btnPrePage.Name = "btnPrePage"
        Me.btnPrePage.Size = New System.Drawing.Size(18, 23)
        Me.btnPrePage.TabIndex = 34
        Me.btnPrePage.Text = "<"
        Me.btnPrePage.UseVisualStyleBackColor = True
        '
        'frmSearch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(927, 696)
        Me.Controls.Add(Me.cbPages)
        Me.Controls.Add(Me.btnLastPage)
        Me.Controls.Add(Me.btnFirstPage)
        Me.Controls.Add(Me.btnNextPage)
        Me.Controls.Add(Me.btnPrePage)
        Me.Controls.Add(Me.tbcSearch)
        Me.Controls.Add(Me.dgvSearchMember)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSearch)
        Me.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSearch"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Tìm kiếm thành viên dòng họ"
        CType(Me.dgvSearchMember, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tbcSearch.ResumeLayout(False)
        Me.tbpBase.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.tbpMoreInfo.ResumeLayout(False)
        Me.tbpMoreInfo.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents dgvSearchMember As System.Windows.Forms.DataGridView
    Friend WithEvents tbcSearch As System.Windows.Forms.TabControl
    Friend WithEvents tbpBase As System.Windows.Forms.TabPage
    Friend WithEvents tbpMoreInfo As System.Windows.Forms.TabPage
    Friend WithEvents txtKeyword As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents cboPosLevelName As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cbGender As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dtpBirthFrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents chkDie As System.Windows.Forms.CheckBox
    Friend WithEvents btnBirthFrom As System.Windows.Forms.Button
    Friend WithEvents dtpBirthTo As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents dtpDieTo As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents dtpDieFrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblDieFrom As System.Windows.Forms.Label
    Friend WithEvents lblBirthTo As System.Windows.Forms.Label
    Friend WithEvents lblBirthFrom As System.Windows.Forms.Label
    Friend WithEvents lblDieTo As System.Windows.Forms.Label
    Friend WithEvents btnDieTo As System.Windows.Forms.Button
    Friend WithEvents btnDieFrom As System.Windows.Forms.Button
    Friend WithEvents btnBirthTo As System.Windows.Forms.Button
    Friend WithEvents txtPosition As System.Windows.Forms.TextBox
    Friend WithEvents txtOccupt As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents lblDeaTo As System.Windows.Forms.Label
    Friend WithEvents lblDeaFrom As System.Windows.Forms.Label
    Friend WithEvents btnDeaTo As System.Windows.Forms.Button
    Friend WithEvents btnDeaFrom As System.Windows.Forms.Button
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents clmNo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmLevel As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmFullName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmGender As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmContact As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmHometown As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmBirthDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmDeceaseDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmRemark As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cbPages As System.Windows.Forms.ComboBox
    Friend WithEvents btnLastPage As System.Windows.Forms.Button
    Friend WithEvents btnFirstPage As System.Windows.Forms.Button
    Friend WithEvents btnNextPage As System.Windows.Forms.Button
    Friend WithEvents btnPrePage As System.Windows.Forms.Button
End Class
