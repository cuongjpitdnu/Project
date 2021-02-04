<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFamilyInfo
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
        Me.components = New System.ComponentModel.Container
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFamilyInfo))
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.lblHeadName = New System.Windows.Forms.Label
        Me.dgvFamilyHead = New System.Windows.Forms.DataGridView
        Me.clmNo = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.clmName = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.clmBirth = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.clmStart = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.clmEnd = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.clmRemark = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblHeadLevel = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.pnImageBar = New System.Windows.Forms.Panel
        Me.pnImageList = New System.Windows.Forms.Panel
        Me.btnNext = New System.Windows.Forms.Button
        Me.btnPrevious = New System.Windows.Forms.Button
        Me.mnuContext = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mniViewPic = New System.Windows.Forms.ToolStripMenuItem
        Me.mniAddPic = New System.Windows.Forms.ToolStripMenuItem
        Me.mniChangePic = New System.Windows.Forms.ToolStripMenuItem
        Me.mniDeletePic = New System.Windows.Forms.ToolStripMenuItem
        Me.tabFamily = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.btnEditFInfo = New System.Windows.Forms.Button
        Me.lblFamilyHometown = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.lblFamilyAnni = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.lblFamilyName = New System.Windows.Forms.Label
        Me.lblFamilyInitGeneration = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.btnClear = New System.Windows.Forms.Button
        Me.btnCreate = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnBrowse = New System.Windows.Forms.Button
        Me.txtTitle = New System.Windows.Forms.TextBox
        Me.txtContent = New System.Windows.Forms.TextBox
        Me.lblTitle = New System.Windows.Forms.Label
        Me.txtFile = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.dgvDocs = New System.Windows.Forms.DataGridView
        Me.clmSTT = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.clmTitle = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.clmContent = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.clmID = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.clmDirectory = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.clmDel = New System.Windows.Forms.DataGridViewButtonColumn
        Me.Label4 = New System.Windows.Forms.Label
        Me.tabFamilyAlbum = New System.Windows.Forms.TabPage
        Me.chkCheckAll = New System.Windows.Forms.CheckBox
        Me.lblFName = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.btnDelImage = New System.Windows.Forms.Button
        Me.btnSaveToFile = New System.Windows.Forms.Button
        Me.btnAddImage = New System.Windows.Forms.Button
        Me.Label10 = New System.Windows.Forms.Label
        Me.flpanelAlbum = New System.Windows.Forms.FlowLayoutPanel
        Me.tabAlbum = New System.Windows.Forms.TabPage
        Me.btnDelete = New System.Windows.Forms.Button
        Me.btnEdit = New System.Windows.Forms.Button
        Me.btnAddNew = New System.Windows.Forms.Button
        Me.btnPreviewNext = New System.Windows.Forms.Button
        Me.btnPreviewBack = New System.Windows.Forms.Button
        Me.picPreview = New System.Windows.Forms.PictureBox
        Me.ToolTipFamily = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.dgvFamilyHead, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnImageBar.SuspendLayout()
        Me.mnuContext.SuspendLayout()
        Me.tabFamily.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        CType(Me.dgvDocs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabFamilyAlbum.SuspendLayout()
        Me.tabAlbum.SuspendLayout()
        CType(Me.picPreview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblHeadName
        '
        Me.lblHeadName.AutoSize = True
        Me.lblHeadName.Location = New System.Drawing.Point(31, 212)
        Me.lblHeadName.Name = "lblHeadName"
        Me.lblHeadName.Size = New System.Drawing.Size(63, 16)
        Me.lblHeadName.TabIndex = 7
        Me.lblHeadName.Text = "Không có"
        '
        'dgvFamilyHead
        '
        Me.dgvFamilyHead.AllowUserToAddRows = False
        Me.dgvFamilyHead.AllowUserToDeleteRows = False
        Me.dgvFamilyHead.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvFamilyHead.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvFamilyHead.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvFamilyHead.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.clmNo, Me.clmName, Me.clmBirth, Me.clmStart, Me.clmEnd, Me.clmRemark})
        Me.dgvFamilyHead.Location = New System.Drawing.Point(6, 266)
        Me.dgvFamilyHead.MultiSelect = False
        Me.dgvFamilyHead.Name = "dgvFamilyHead"
        Me.dgvFamilyHead.ReadOnly = True
        Me.dgvFamilyHead.RowHeadersVisible = False
        Me.dgvFamilyHead.RowTemplate.Height = 21
        Me.dgvFamilyHead.RowTemplate.ReadOnly = True
        Me.dgvFamilyHead.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvFamilyHead.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvFamilyHead.Size = New System.Drawing.Size(577, 230)
        Me.dgvFamilyHead.TabIndex = 6
        '
        'clmNo
        '
        Me.clmNo.HeaderText = "STT"
        Me.clmNo.MinimumWidth = 35
        Me.clmNo.Name = "clmNo"
        Me.clmNo.ReadOnly = True
        Me.clmNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.clmNo.Width = 35
        '
        'clmName
        '
        Me.clmName.HeaderText = "Họ và tên"
        Me.clmName.MinimumWidth = 205
        Me.clmName.Name = "clmName"
        Me.clmName.ReadOnly = True
        Me.clmName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.clmName.Width = 205
        '
        'clmBirth
        '
        Me.clmBirth.HeaderText = "Ngày sinh"
        Me.clmBirth.MinimumWidth = 110
        Me.clmBirth.Name = "clmBirth"
        Me.clmBirth.ReadOnly = True
        Me.clmBirth.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.clmBirth.Width = 110
        '
        'clmStart
        '
        Me.clmStart.HeaderText = "Bắt đầu"
        Me.clmStart.Name = "clmStart"
        Me.clmStart.ReadOnly = True
        Me.clmStart.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.clmStart.Visible = False
        Me.clmStart.Width = 75
        '
        'clmEnd
        '
        Me.clmEnd.HeaderText = "Kết thúc"
        Me.clmEnd.Name = "clmEnd"
        Me.clmEnd.ReadOnly = True
        Me.clmEnd.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.clmEnd.Visible = False
        Me.clmEnd.Width = 75
        '
        'clmRemark
        '
        Me.clmRemark.HeaderText = "Ghi chú"
        Me.clmRemark.MinimumWidth = 224
        Me.clmRemark.Name = "clmRemark"
        Me.clmRemark.ReadOnly = True
        Me.clmRemark.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.clmRemark.Width = 224
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        Me.Label2.Location = New System.Drawing.Point(19, 235)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(175, 16)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Danh sách các trưởng họ :"
        '
        'lblHeadLevel
        '
        Me.lblHeadLevel.AutoSize = True
        Me.lblHeadLevel.Location = New System.Drawing.Point(217, 212)
        Me.lblHeadLevel.Name = "lblHeadLevel"
        Me.lblHeadLevel.Size = New System.Drawing.Size(0, 16)
        Me.lblHeadLevel.TabIndex = 0
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(158, 212)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(61, 16)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Đời thứ :"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        Me.Label1.Location = New System.Drawing.Point(19, 184)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(127, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Trưởng họ hiện tại"
        '
        'pnImageBar
        '
        Me.pnImageBar.Controls.Add(Me.pnImageList)
        Me.pnImageBar.Controls.Add(Me.btnNext)
        Me.pnImageBar.Controls.Add(Me.btnPrevious)
        Me.pnImageBar.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnImageBar.Location = New System.Drawing.Point(0, 459)
        Me.pnImageBar.Name = "pnImageBar"
        Me.pnImageBar.Size = New System.Drawing.Size(593, 70)
        Me.pnImageBar.TabIndex = 5
        '
        'pnImageList
        '
        Me.pnImageList.BackColor = System.Drawing.SystemColors.Control
        Me.pnImageList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnImageList.Location = New System.Drawing.Point(20, 0)
        Me.pnImageList.Name = "pnImageList"
        Me.pnImageList.Size = New System.Drawing.Size(553, 70)
        Me.pnImageList.TabIndex = 3
        '
        'btnNext
        '
        Me.btnNext.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnNext.Location = New System.Drawing.Point(573, 0)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(20, 70)
        Me.btnNext.TabIndex = 7
        Me.btnNext.Text = ">"
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'btnPrevious
        '
        Me.btnPrevious.Dock = System.Windows.Forms.DockStyle.Left
        Me.btnPrevious.Location = New System.Drawing.Point(0, 0)
        Me.btnPrevious.Name = "btnPrevious"
        Me.btnPrevious.Size = New System.Drawing.Size(20, 70)
        Me.btnPrevious.TabIndex = 6
        Me.btnPrevious.Text = "<"
        Me.btnPrevious.UseVisualStyleBackColor = True
        '
        'mnuContext
        '
        Me.mnuContext.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mniViewPic, Me.mniAddPic, Me.mniChangePic, Me.mniDeletePic})
        Me.mnuContext.Name = "mnContext"
        Me.mnuContext.Size = New System.Drawing.Size(122, 92)
        '
        'mniViewPic
        '
        Me.mniViewPic.Name = "mniViewPic"
        Me.mniViewPic.Size = New System.Drawing.Size(121, 22)
        Me.mniViewPic.Text = "Xem ảnh"
        '
        'mniAddPic
        '
        Me.mniAddPic.Name = "mniAddPic"
        Me.mniAddPic.Size = New System.Drawing.Size(121, 22)
        Me.mniAddPic.Text = "Thêm ảnh"
        '
        'mniChangePic
        '
        Me.mniChangePic.Name = "mniChangePic"
        Me.mniChangePic.Size = New System.Drawing.Size(121, 22)
        Me.mniChangePic.Text = "Đổi ảnh"
        '
        'mniDeletePic
        '
        Me.mniDeletePic.Name = "mniDeletePic"
        Me.mniDeletePic.Size = New System.Drawing.Size(121, 22)
        Me.mniDeletePic.Text = "Xóa ảnh"
        '
        'tabFamily
        '
        Me.tabFamily.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabFamily.Controls.Add(Me.TabPage1)
        Me.tabFamily.Controls.Add(Me.TabPage2)
        Me.tabFamily.Controls.Add(Me.tabFamilyAlbum)
        Me.tabFamily.Controls.Add(Me.tabAlbum)
        Me.tabFamily.Location = New System.Drawing.Point(12, 12)
        Me.tabFamily.Name = "tabFamily"
        Me.tabFamily.SelectedIndex = 0
        Me.tabFamily.Size = New System.Drawing.Size(601, 558)
        Me.tabFamily.TabIndex = 6
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.GroupBox1)
        Me.TabPage1.Controls.Add(Me.lblHeadName)
        Me.TabPage1.Controls.Add(Me.dgvFamilyHead)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Controls.Add(Me.Label3)
        Me.TabPage1.Controls.Add(Me.lblHeadLevel)
        Me.TabPage1.Location = New System.Drawing.Point(4, 25)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(593, 529)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Thông tin dòng họ"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnEditFInfo)
        Me.GroupBox1.Controls.Add(Me.lblFamilyHometown)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.lblFamilyAnni)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.lblFamilyName)
        Me.GroupBox1.Controls.Add(Me.lblFamilyInitGeneration)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Location = New System.Drawing.Point(6, 6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(577, 157)
        Me.GroupBox1.TabIndex = 8
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Thông tin chung"
        '
        'btnEditFInfo
        '
        Me.btnEditFInfo.Image = CType(resources.GetObject("btnEditFInfo.Image"), System.Drawing.Image)
        Me.btnEditFInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnEditFInfo.Location = New System.Drawing.Point(496, 128)
        Me.btnEditFInfo.Name = "btnEditFInfo"
        Me.btnEditFInfo.Size = New System.Drawing.Size(75, 23)
        Me.btnEditFInfo.TabIndex = 1
        Me.btnEditFInfo.Text = "    Sửa"
        Me.btnEditFInfo.UseVisualStyleBackColor = True
        '
        'lblFamilyHometown
        '
        Me.lblFamilyHometown.Location = New System.Drawing.Point(102, 93)
        Me.lblFamilyHometown.Name = "lblFamilyHometown"
        Me.lblFamilyHometown.Size = New System.Drawing.Size(388, 51)
        Me.lblFamilyHometown.TabIndex = 0
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(13, 93)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(88, 16)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "Nguyên quán:"
        '
        'lblFamilyAnni
        '
        Me.lblFamilyAnni.Location = New System.Drawing.Point(102, 61)
        Me.lblFamilyAnni.Name = "lblFamilyAnni"
        Me.lblFamilyAnni.Size = New System.Drawing.Size(469, 16)
        Me.lblFamilyAnni.TabIndex = 0
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(23, 61)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(78, 16)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Ngày giỗ tổ:"
        '
        'lblFamilyName
        '
        Me.lblFamilyName.Location = New System.Drawing.Point(102, 29)
        Me.lblFamilyName.Name = "lblFamilyName"
        Me.lblFamilyName.Size = New System.Drawing.Size(277, 16)
        Me.lblFamilyName.TabIndex = 0
        '
        'lblFamilyInitGeneration
        '
        Me.lblFamilyInitGeneration.AutoSize = True
        Me.lblFamilyInitGeneration.Location = New System.Drawing.Point(456, 29)
        Me.lblFamilyInitGeneration.Name = "lblFamilyInitGeneration"
        Me.lblFamilyInitGeneration.Size = New System.Drawing.Size(0, 16)
        Me.lblFamilyInitGeneration.TabIndex = 0
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(385, 29)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(65, 16)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Đời thứ : "
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(18, 29)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(83, 16)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Tên dòng họ:"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.btnClear)
        Me.TabPage2.Controls.Add(Me.btnCreate)
        Me.TabPage2.Controls.Add(Me.btnSave)
        Me.TabPage2.Controls.Add(Me.btnBrowse)
        Me.TabPage2.Controls.Add(Me.txtTitle)
        Me.TabPage2.Controls.Add(Me.txtContent)
        Me.TabPage2.Controls.Add(Me.lblTitle)
        Me.TabPage2.Controls.Add(Me.txtFile)
        Me.TabPage2.Controls.Add(Me.Label6)
        Me.TabPage2.Controls.Add(Me.Label5)
        Me.TabPage2.Controls.Add(Me.dgvDocs)
        Me.TabPage2.Controls.Add(Me.Label4)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(593, 532)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Ghi chú chung về dòng họ"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClear.Location = New System.Drawing.Point(23, 169)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(86, 34)
        Me.btnClear.TabIndex = 7
        Me.btnClear.Text = "&Xóa trắng"
        Me.btnClear.UseVisualStyleBackColor = True
        Me.btnClear.Visible = False
        '
        'btnCreate
        '
        Me.btnCreate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCreate.Location = New System.Drawing.Point(128, 169)
        Me.btnCreate.Name = "btnCreate"
        Me.btnCreate.Size = New System.Drawing.Size(78, 34)
        Me.btnCreate.TabIndex = 5
        Me.btnCreate.Text = "&Tạo mới"
        Me.btnCreate.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(493, 169)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(78, 34)
        Me.btnSave.TabIndex = 6
        Me.btnSave.Text = "&Ghi"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnBrowse
        '
        Me.btnBrowse.Image = CType(resources.GetObject("btnBrowse.Image"), System.Drawing.Image)
        Me.btnBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnBrowse.Location = New System.Drawing.Point(435, 53)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(136, 23)
        Me.btnBrowse.TabIndex = 2
        Me.btnBrowse.Text = "    &Chọn đường dẫn"
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'txtTitle
        '
        Me.txtTitle.Location = New System.Drawing.Point(128, 81)
        Me.txtTitle.MaxLength = 150
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(443, 22)
        Me.txtTitle.TabIndex = 3
        '
        'txtContent
        '
        Me.txtContent.Location = New System.Drawing.Point(128, 109)
        Me.txtContent.MaxLength = 150
        Me.txtContent.Multiline = True
        Me.txtContent.Name = "txtContent"
        Me.txtContent.Size = New System.Drawing.Size(443, 54)
        Me.txtContent.TabIndex = 4
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(30, 84)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(92, 16)
        Me.lblTitle.TabIndex = 2
        Me.lblTitle.Text = "Tiêu đề tài liệu"
        '
        'txtFile
        '
        Me.txtFile.Enabled = False
        Me.txtFile.Location = New System.Drawing.Point(128, 53)
        Me.txtFile.Name = "txtFile"
        Me.txtFile.Size = New System.Drawing.Size(283, 22)
        Me.txtFile.TabIndex = 1
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 112)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(116, 16)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Nội dung khái quát"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(48, 56)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(74, 16)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Đường dẫn"
        '
        'dgvDocs
        '
        Me.dgvDocs.AllowUserToAddRows = False
        Me.dgvDocs.AllowUserToDeleteRows = False
        Me.dgvDocs.AllowUserToResizeRows = False
        Me.dgvDocs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvDocs.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.dgvDocs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDocs.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.clmSTT, Me.clmTitle, Me.clmContent, Me.clmID, Me.clmDirectory, Me.clmDel})
        Me.dgvDocs.Location = New System.Drawing.Point(6, 209)
        Me.dgvDocs.MultiSelect = False
        Me.dgvDocs.Name = "dgvDocs"
        Me.dgvDocs.ReadOnly = True
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvDocs.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.dgvDocs.RowHeadersVisible = False
        Me.dgvDocs.RowTemplate.ReadOnly = True
        Me.dgvDocs.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvDocs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvDocs.Size = New System.Drawing.Size(581, 257)
        Me.dgvDocs.TabIndex = 8
        '
        'clmSTT
        '
        Me.clmSTT.HeaderText = "STT"
        Me.clmSTT.MinimumWidth = 35
        Me.clmSTT.Name = "clmSTT"
        Me.clmSTT.ReadOnly = True
        Me.clmSTT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.clmSTT.Width = 35
        '
        'clmTitle
        '
        Me.clmTitle.HeaderText = "Tiêu đề tài liệu"
        Me.clmTitle.MinimumWidth = 218
        Me.clmTitle.Name = "clmTitle"
        Me.clmTitle.ReadOnly = True
        Me.clmTitle.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.clmTitle.Width = 218
        '
        'clmContent
        '
        Me.clmContent.HeaderText = "Nội dung khái quát"
        Me.clmContent.MinimumWidth = 260
        Me.clmContent.Name = "clmContent"
        Me.clmContent.ReadOnly = True
        Me.clmContent.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.clmContent.Width = 260
        '
        'clmID
        '
        Me.clmID.HeaderText = "ID"
        Me.clmID.Name = "clmID"
        Me.clmID.ReadOnly = True
        Me.clmID.Visible = False
        '
        'clmDirectory
        '
        Me.clmDirectory.HeaderText = "Nơi lưu trữ"
        Me.clmDirectory.Name = "clmDirectory"
        Me.clmDirectory.ReadOnly = True
        Me.clmDirectory.Visible = False
        Me.clmDirectory.Width = 160
        '
        'clmDel
        '
        Me.clmDel.HeaderText = "Xóa"
        Me.clmDel.MinimumWidth = 65
        Me.clmDel.Name = "clmDel"
        Me.clmDel.ReadOnly = True
        Me.clmDel.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.clmDel.Text = "Xóa"
        Me.clmDel.UseColumnTextForButtonValue = True
        Me.clmDel.Width = 65
        '
        'Label4
        '
        Me.Label4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        Me.Label4.Location = New System.Drawing.Point(82, 19)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(438, 18)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "DANH SÁCH CÁC VĂN BẢN GHI CHÚ CHUNG CỦA DÒNG HỌ"
        '
        'tabFamilyAlbum
        '
        Me.tabFamilyAlbum.Controls.Add(Me.btnDelImage)
        Me.tabFamilyAlbum.Controls.Add(Me.chkCheckAll)
        Me.tabFamilyAlbum.Controls.Add(Me.lblFName)
        Me.tabFamilyAlbum.Controls.Add(Me.Label12)
        Me.tabFamilyAlbum.Controls.Add(Me.btnSaveToFile)
        Me.tabFamilyAlbum.Controls.Add(Me.btnAddImage)
        Me.tabFamilyAlbum.Controls.Add(Me.Label10)
        Me.tabFamilyAlbum.Controls.Add(Me.flpanelAlbum)
        Me.tabFamilyAlbum.Location = New System.Drawing.Point(4, 25)
        Me.tabFamilyAlbum.Name = "tabFamilyAlbum"
        Me.tabFamilyAlbum.Size = New System.Drawing.Size(593, 529)
        Me.tabFamilyAlbum.TabIndex = 3
        Me.tabFamilyAlbum.Text = "Bộ sưu tập ảnh"
        Me.tabFamilyAlbum.UseVisualStyleBackColor = True
        '
        'chkCheckAll
        '
        Me.chkCheckAll.AutoSize = True
        Me.chkCheckAll.Font = New System.Drawing.Font("Arial Narrow", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCheckAll.Location = New System.Drawing.Point(451, 35)
        Me.chkCheckAll.Name = "chkCheckAll"
        Me.chkCheckAll.Size = New System.Drawing.Size(121, 20)
        Me.chkCheckAll.TabIndex = 20
        Me.chkCheckAll.Text = "Chọn toàn bộ ảnh."
        Me.chkCheckAll.UseVisualStyleBackColor = True
        '
        'lblFName
        '
        Me.lblFName.AutoSize = True
        Me.lblFName.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold)
        Me.lblFName.Location = New System.Drawing.Point(314, 18)
        Me.lblFName.Name = "lblFName"
        Me.lblFName.Size = New System.Drawing.Size(63, 18)
        Me.lblFName.TabIndex = 19
        Me.lblFName.Text = "Label13"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold)
        Me.Label12.Location = New System.Drawing.Point(126, 18)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(181, 18)
        Me.Label12.TabIndex = 18
        Me.Label12.Text = "ALBUM ẢNH DÒNG HỌ: "
        '
        'btnDelImage
        '
        Me.btnDelImage.Font = New System.Drawing.Font("Arial", 9.75!)
        Me.btnDelImage.Image = CType(resources.GetObject("btnDelImage.Image"), System.Drawing.Image)
        Me.btnDelImage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDelImage.Location = New System.Drawing.Point(539, 475)
        Me.btnDelImage.Name = "btnDelImage"
        Me.btnDelImage.Size = New System.Drawing.Size(33, 35)
        Me.btnDelImage.TabIndex = 17
        Me.btnDelImage.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTipFamily.SetToolTip(Me.btnDelImage, "Xóa ảnh")
        Me.btnDelImage.UseVisualStyleBackColor = True
        '
        'btnSaveToFile
        '
        Me.btnSaveToFile.Font = New System.Drawing.Font("Arial", 9.75!)
        Me.btnSaveToFile.Image = CType(resources.GetObject("btnSaveToFile.Image"), System.Drawing.Image)
        Me.btnSaveToFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveToFile.Location = New System.Drawing.Point(360, 475)
        Me.btnSaveToFile.Name = "btnSaveToFile"
        Me.btnSaveToFile.Size = New System.Drawing.Size(171, 35)
        Me.btnSaveToFile.TabIndex = 15
        Me.btnSaveToFile.Text = "Lưu ảnh vào thư mục"
        Me.btnSaveToFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSaveToFile.UseVisualStyleBackColor = True
        '
        'btnAddImage
        '
        Me.btnAddImage.Font = New System.Drawing.Font("Arial", 9.75!)
        Me.btnAddImage.Image = CType(resources.GetObject("btnAddImage.Image"), System.Drawing.Image)
        Me.btnAddImage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAddImage.Location = New System.Drawing.Point(317, 475)
        Me.btnAddImage.Name = "btnAddImage"
        Me.btnAddImage.Size = New System.Drawing.Size(33, 35)
        Me.btnAddImage.TabIndex = 16
        Me.btnAddImage.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTipFamily.SetToolTip(Me.btnAddImage, "Thêm ảnh")
        Me.btnAddImage.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Arial", 9.75!)
        Me.Label10.ForeColor = System.Drawing.Color.Red
        Me.Label10.Location = New System.Drawing.Point(15, 472)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(290, 16)
        Me.Label10.TabIndex = 14
        Me.Label10.Text = "*Nháy đúp lên ảnh để xem ảnh đúng kích thước."
        '
        'flpanelAlbum
        '
        Me.flpanelAlbum.AutoScroll = True
        Me.flpanelAlbum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.flpanelAlbum.Location = New System.Drawing.Point(18, 61)
        Me.flpanelAlbum.Name = "flpanelAlbum"
        Me.flpanelAlbum.Size = New System.Drawing.Size(554, 408)
        Me.flpanelAlbum.TabIndex = 13
        '
        'tabAlbum
        '
        Me.tabAlbum.Controls.Add(Me.btnDelete)
        Me.tabAlbum.Controls.Add(Me.btnEdit)
        Me.tabAlbum.Controls.Add(Me.btnAddNew)
        Me.tabAlbum.Controls.Add(Me.btnPreviewNext)
        Me.tabAlbum.Controls.Add(Me.btnPreviewBack)
        Me.tabAlbum.Controls.Add(Me.picPreview)
        Me.tabAlbum.Controls.Add(Me.pnImageBar)
        Me.tabAlbum.Location = New System.Drawing.Point(4, 25)
        Me.tabAlbum.Name = "tabAlbum"
        Me.tabAlbum.Size = New System.Drawing.Size(593, 529)
        Me.tabAlbum.TabIndex = 2
        Me.tabAlbum.Text = "Ảnh dòng họ"
        Me.tabAlbum.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Image = CType(resources.GetObject("btnDelete.Image"), System.Drawing.Image)
        Me.btnDelete.Location = New System.Drawing.Point(567, 70)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(23, 21)
        Me.btnDelete.TabIndex = 3
        Me.ToolTipFamily.SetToolTip(Me.btnDelete, "Xóa ảnh")
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnEdit
        '
        Me.btnEdit.Image = CType(resources.GetObject("btnEdit.Image"), System.Drawing.Image)
        Me.btnEdit.Location = New System.Drawing.Point(567, 43)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(23, 21)
        Me.btnEdit.TabIndex = 2
        Me.ToolTipFamily.SetToolTip(Me.btnEdit, "Thay đổi ảnh")
        Me.btnEdit.UseVisualStyleBackColor = True
        '
        'btnAddNew
        '
        Me.btnAddNew.Image = Global.GiaPha.My.Resources.Resources.edit_add
        Me.btnAddNew.Location = New System.Drawing.Point(567, 18)
        Me.btnAddNew.Name = "btnAddNew"
        Me.btnAddNew.Size = New System.Drawing.Size(23, 21)
        Me.btnAddNew.TabIndex = 1
        Me.ToolTipFamily.SetToolTip(Me.btnAddNew, "Thêm ảnh mới")
        Me.btnAddNew.UseVisualStyleBackColor = True
        '
        'btnPreviewNext
        '
        Me.btnPreviewNext.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnPreviewNext.Location = New System.Drawing.Point(567, 115)
        Me.btnPreviewNext.Name = "btnPreviewNext"
        Me.btnPreviewNext.Size = New System.Drawing.Size(23, 85)
        Me.btnPreviewNext.TabIndex = 5
        Me.btnPreviewNext.Text = ">"
        Me.btnPreviewNext.UseVisualStyleBackColor = True
        '
        'btnPreviewBack
        '
        Me.btnPreviewBack.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.btnPreviewBack.Location = New System.Drawing.Point(3, 115)
        Me.btnPreviewBack.Name = "btnPreviewBack"
        Me.btnPreviewBack.Size = New System.Drawing.Size(23, 85)
        Me.btnPreviewBack.TabIndex = 4
        Me.btnPreviewBack.Text = "<"
        Me.btnPreviewBack.UseVisualStyleBackColor = True
        '
        'picPreview
        '
        Me.picPreview.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picPreview.Location = New System.Drawing.Point(32, 3)
        Me.picPreview.Name = "picPreview"
        Me.picPreview.Size = New System.Drawing.Size(529, 378)
        Me.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picPreview.TabIndex = 6
        Me.picPreview.TabStop = False
        '
        'frmFamilyInfo
        '
        Me.ClientSize = New System.Drawing.Size(626, 585)
        Me.Controls.Add(Me.tabFamily)
        Me.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmFamilyInfo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Thông tin dòng họ"
        CType(Me.dgvFamilyHead, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnImageBar.ResumeLayout(False)
        Me.mnuContext.ResumeLayout(False)
        Me.tabFamily.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        CType(Me.dgvDocs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabFamilyAlbum.ResumeLayout(False)
        Me.tabFamilyAlbum.PerformLayout()
        Me.tabAlbum.ResumeLayout(False)
        CType(Me.picPreview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dgvFamilyHead As System.Windows.Forms.DataGridView
    Friend WithEvents lblHeadLevel As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblHeadName As System.Windows.Forms.Label
    Friend WithEvents pnImageBar As System.Windows.Forms.Panel
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents btnPrevious As System.Windows.Forms.Button
    Friend WithEvents mnuContext As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mniViewPic As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mniChangePic As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnImageList As System.Windows.Forms.Panel
    Friend WithEvents mniDeletePic As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mniAddPic As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabFamily As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents tabAlbum As System.Windows.Forms.TabPage
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnPreviewNext As System.Windows.Forms.Button
    Friend WithEvents btnPreviewBack As System.Windows.Forms.Button
    Friend WithEvents picPreview As System.Windows.Forms.PictureBox
    Friend WithEvents dgvDocs As System.Windows.Forms.DataGridView
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents txtContent As System.Windows.Forms.TextBox
    Friend WithEvents txtFile As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtTitle As System.Windows.Forms.TextBox
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents clmNo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmBirth As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmStart As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmEnd As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmRemark As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmSTT As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmTitle As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmContent As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmDirectory As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmDel As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnAddNew As System.Windows.Forms.Button
    Friend WithEvents ToolTipFamily As System.Windows.Forms.ToolTip
    Friend WithEvents btnEdit As System.Windows.Forms.Button
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents btnCreate As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lblFamilyHometown As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents lblFamilyAnni As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents lblFamilyName As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnEditFInfo As System.Windows.Forms.Button
    Friend WithEvents lblFamilyInitGeneration As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents tabFamilyAlbum As System.Windows.Forms.TabPage
    Friend WithEvents chkCheckAll As System.Windows.Forms.CheckBox
    Friend WithEvents lblFName As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents btnDelImage As System.Windows.Forms.Button
    Friend WithEvents btnSaveToFile As System.Windows.Forms.Button
    Friend WithEvents btnAddImage As System.Windows.Forms.Button
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents flpanelAlbum As System.Windows.Forms.FlowLayoutPanel

End Class
