<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmOption
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOption))
        Me.dlgColor = New System.Windows.Forms.ColorDialog()
        Me.MainLayout = New System.Windows.Forms.TableLayoutPanel()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.tabGeneral = New System.Windows.Forms.TabPage()
        Me.grbShowDieDay = New System.Windows.Forms.GroupBox()
        Me.rdDeadMoonCalendarShow = New System.Windows.Forms.RadioButton()
        Me.rdDeadSunCalendarShow = New System.Windows.Forms.RadioButton()
        Me.grbFomat = New System.Windows.Forms.GroupBox()
        Me.chkShowUnknownBirthDay = New System.Windows.Forms.CheckBox()
        Me.chkBackgroupColorDie = New System.Windows.Forms.CheckBox()
        Me.lblBackgroupColorDie = New System.Windows.Forms.Label()
        Me.lblTextColor = New System.Windows.Forms.Label()
        Me.lblBackgroupColor = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.lblBackgroup = New System.Windows.Forms.Label()
        Me.cboFont = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblVerCm = New System.Windows.Forms.Label()
        Me.lblHozCm = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.nudVerBuffer = New System.Windows.Forms.NumericUpDown()
        Me.nudHozBuffer = New System.Windows.Forms.NumericUpDown()
        Me.grbShowImg = New System.Windows.Forms.GroupBox()
        Me.rdFrameCompact = New System.Windows.Forms.RadioButton()
        Me.rdFrameFull = New System.Windows.Forms.RadioButton()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.nudGeneration = New System.Windows.Forms.NumericUpDown()
        Me.lblNote = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cboTypeShowTree = New System.Windows.Forms.ComboBox()
        Me.grbFrame = New System.Windows.Forms.GroupBox()
        Me.rdoShortVer = New System.Windows.Forms.RadioButton()
        Me.rdoShortHoz = New System.Windows.Forms.RadioButton()
        Me.cboFrameType = New System.Windows.Forms.ComboBox()
        Me.usrMemCard2 = New GiaPha.usrMemberCard2()
        Me.usrMemCard1 = New GiaPha.usrMemberCard1()
        Me.rdCard2 = New System.Windows.Forms.RadioButton()
        Me.rdCard1 = New System.Windows.Forms.RadioButton()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.grbTypeTextDisplay = New System.Windows.Forms.GroupBox()
        Me.rdoTextTypeRight = New System.Windows.Forms.RadioButton()
        Me.rdoTexTypeLeft = New System.Windows.Forms.RadioButton()
        Me.rdoTextTypeNormal = New System.Windows.Forms.RadioButton()
        Me.grbInfoCard = New System.Windows.Forms.GroupBox()
        Me.chkShowGender = New System.Windows.Forms.CheckBox()
        Me.chkShowLevel = New System.Windows.Forms.CheckBox()
        Me.grbCardSize = New System.Windows.Forms.GroupBox()
        Me.lblVerSizeCm = New System.Windows.Forms.Label()
        Me.lblHozSizeCm = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.nudVerSize = New System.Windows.Forms.NumericUpDown()
        Me.nudHozSize = New System.Windows.Forms.NumericUpDown()
        Me.plnControl = New System.Windows.Forms.Panel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.MainLayout.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.grbShowDieDay.SuspendLayout()
        Me.grbFomat.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.nudVerBuffer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudHozBuffer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grbShowImg.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.nudGeneration, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grbFrame.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.grbTypeTextDisplay.SuspendLayout()
        Me.grbInfoCard.SuspendLayout()
        Me.grbCardSize.SuspendLayout()
        CType(Me.nudVerSize, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudHozSize, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.plnControl.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainLayout
        '
        Me.MainLayout.ColumnCount = 1
        Me.MainLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.MainLayout.Controls.Add(Me.tabMain, 0, 0)
        Me.MainLayout.Controls.Add(Me.plnControl, 0, 1)
        Me.MainLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MainLayout.Location = New System.Drawing.Point(0, 0)
        Me.MainLayout.Name = "MainLayout"
        Me.MainLayout.RowCount = 2
        Me.MainLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.MainLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52.0!))
        Me.MainLayout.Size = New System.Drawing.Size(548, 436)
        Me.MainLayout.TabIndex = 0
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabGeneral)
        Me.tabMain.Controls.Add(Me.TabPage2)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(3, 3)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(542, 378)
        Me.tabMain.TabIndex = 1
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.grbShowDieDay)
        Me.tabGeneral.Controls.Add(Me.grbFomat)
        Me.tabGeneral.Controls.Add(Me.GroupBox2)
        Me.tabGeneral.Controls.Add(Me.grbShowImg)
        Me.tabGeneral.Controls.Add(Me.GroupBox1)
        Me.tabGeneral.Controls.Add(Me.lblNote)
        Me.tabGeneral.Controls.Add(Me.Label6)
        Me.tabGeneral.Controls.Add(Me.cboTypeShowTree)
        Me.tabGeneral.Controls.Add(Me.grbFrame)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 22)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(534, 352)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "Chung"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'grbShowDieDay
        '
        Me.grbShowDieDay.Controls.Add(Me.rdDeadMoonCalendarShow)
        Me.grbShowDieDay.Controls.Add(Me.rdDeadSunCalendarShow)
        Me.grbShowDieDay.Location = New System.Drawing.Point(272, 204)
        Me.grbShowDieDay.Name = "grbShowDieDay"
        Me.grbShowDieDay.Size = New System.Drawing.Size(259, 45)
        Me.grbShowDieDay.TabIndex = 14
        Me.grbShowDieDay.TabStop = False
        Me.grbShowDieDay.Text = "Cách hiện ngày mất"
        '
        'rdDeadMoonCalendarShow
        '
        Me.rdDeadMoonCalendarShow.AutoSize = True
        Me.rdDeadMoonCalendarShow.Checked = True
        Me.rdDeadMoonCalendarShow.Location = New System.Drawing.Point(139, 19)
        Me.rdDeadMoonCalendarShow.Name = "rdDeadMoonCalendarShow"
        Me.rdDeadMoonCalendarShow.Size = New System.Drawing.Size(86, 17)
        Me.rdDeadMoonCalendarShow.TabIndex = 16
        Me.rdDeadMoonCalendarShow.TabStop = True
        Me.rdDeadMoonCalendarShow.Text = "Ngày âm lịch"
        Me.rdDeadMoonCalendarShow.UseVisualStyleBackColor = True
        '
        'rdDeadSunCalendarShow
        '
        Me.rdDeadSunCalendarShow.AutoSize = True
        Me.rdDeadSunCalendarShow.Location = New System.Drawing.Point(18, 19)
        Me.rdDeadSunCalendarShow.Name = "rdDeadSunCalendarShow"
        Me.rdDeadSunCalendarShow.Size = New System.Drawing.Size(102, 17)
        Me.rdDeadSunCalendarShow.TabIndex = 15
        Me.rdDeadSunCalendarShow.Text = "Ngày dương lịch"
        Me.rdDeadSunCalendarShow.UseVisualStyleBackColor = True
        '
        'grbFomat
        '
        Me.grbFomat.Controls.Add(Me.chkShowUnknownBirthDay)
        Me.grbFomat.Controls.Add(Me.chkBackgroupColorDie)
        Me.grbFomat.Controls.Add(Me.lblBackgroupColorDie)
        Me.grbFomat.Controls.Add(Me.lblTextColor)
        Me.grbFomat.Controls.Add(Me.lblBackgroupColor)
        Me.grbFomat.Controls.Add(Me.Label9)
        Me.grbFomat.Controls.Add(Me.lblBackgroup)
        Me.grbFomat.Controls.Add(Me.cboFont)
        Me.grbFomat.Controls.Add(Me.Label5)
        Me.grbFomat.Location = New System.Drawing.Point(272, 64)
        Me.grbFomat.Name = "grbFomat"
        Me.grbFomat.Size = New System.Drawing.Size(259, 134)
        Me.grbFomat.TabIndex = 10
        Me.grbFomat.TabStop = False
        Me.grbFomat.Text = "Định dạng"
        '
        'chkShowUnknownBirthDay
        '
        Me.chkShowUnknownBirthDay.AutoSize = True
        Me.chkShowUnknownBirthDay.Checked = True
        Me.chkShowUnknownBirthDay.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowUnknownBirthDay.Location = New System.Drawing.Point(10, 110)
        Me.chkShowUnknownBirthDay.Name = "chkShowUnknownBirthDay"
        Me.chkShowUnknownBirthDay.Size = New System.Drawing.Size(226, 17)
        Me.chkShowUnknownBirthDay.TabIndex = 13
        Me.chkShowUnknownBirthDay.Text = "Hiện KHÔNG RÕ nếu không có ngày sinh"
        Me.chkShowUnknownBirthDay.UseVisualStyleBackColor = True
        '
        'chkBackgroupColorDie
        '
        Me.chkBackgroupColorDie.AutoSize = True
        Me.chkBackgroupColorDie.Location = New System.Drawing.Point(10, 87)
        Me.chkBackgroupColorDie.Name = "chkBackgroupColorDie"
        Me.chkBackgroupColorDie.Size = New System.Drawing.Size(181, 17)
        Me.chkBackgroupColorDie.TabIndex = 12
        Me.chkBackgroupColorDie.Text = "Màu nền cho thành viên đã mất:"
        Me.chkBackgroupColorDie.UseVisualStyleBackColor = True
        '
        'lblBackgroupColorDie
        '
        Me.lblBackgroupColorDie.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblBackgroupColorDie.Location = New System.Drawing.Point(203, 83)
        Me.lblBackgroupColorDie.Name = "lblBackgroupColorDie"
        Me.lblBackgroupColorDie.Size = New System.Drawing.Size(27, 23)
        Me.lblBackgroupColorDie.TabIndex = 77
        Me.lblBackgroupColorDie.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTextColor
        '
        Me.lblTextColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblTextColor.Location = New System.Drawing.Point(71, 52)
        Me.lblTextColor.Name = "lblTextColor"
        Me.lblTextColor.Size = New System.Drawing.Size(27, 23)
        Me.lblTextColor.TabIndex = 77
        Me.lblTextColor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblBackgroupColor
        '
        Me.lblBackgroupColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblBackgroupColor.Location = New System.Drawing.Point(203, 52)
        Me.lblBackgroupColor.Name = "lblBackgroupColor"
        Me.lblBackgroupColor.Size = New System.Drawing.Size(27, 23)
        Me.lblBackgroupColor.TabIndex = 77
        Me.lblBackgroupColor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(7, 57)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(55, 13)
        Me.Label9.TabIndex = 77
        Me.Label9.Text = "Màu chữ: "
        '
        'lblBackgroup
        '
        Me.lblBackgroup.AutoSize = True
        Me.lblBackgroup.Location = New System.Drawing.Point(136, 57)
        Me.lblBackgroup.Name = "lblBackgroup"
        Me.lblBackgroup.Size = New System.Drawing.Size(55, 13)
        Me.lblBackgroup.TabIndex = 77
        Me.lblBackgroup.Text = "Màu nền: "
        '
        'cboFont
        '
        Me.cboFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFont.FormattingEnabled = True
        Me.cboFont.Location = New System.Drawing.Point(71, 23)
        Me.cboFont.Name = "cboFont"
        Me.cboFont.Size = New System.Drawing.Size(159, 21)
        Me.cboFont.TabIndex = 11
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 26)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(59, 13)
        Me.Label5.TabIndex = 75
        Me.Label5.Text = "Chọn Font:"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblVerCm)
        Me.GroupBox2.Controls.Add(Me.lblHozCm)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.nudVerBuffer)
        Me.GroupBox2.Controls.Add(Me.nudHozBuffer)
        Me.GroupBox2.Location = New System.Drawing.Point(272, 255)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(259, 91)
        Me.GroupBox2.TabIndex = 17
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Khoảng cách tổi thiểu (điểm ảnh) giữa 2 khung"
        '
        'lblVerCm
        '
        Me.lblVerCm.AutoSize = True
        Me.lblVerCm.Location = New System.Drawing.Point(153, 61)
        Me.lblVerCm.Name = "lblVerCm"
        Me.lblVerCm.Size = New System.Drawing.Size(76, 13)
        Me.lblVerCm.TabIndex = 38
        Me.lblVerCm.Text = "Khoảng ?? cm"
        '
        'lblHozCm
        '
        Me.lblHozCm.AutoSize = True
        Me.lblHozCm.Location = New System.Drawing.Point(153, 27)
        Me.lblHozCm.Name = "lblHozCm"
        Me.lblHozCm.Size = New System.Drawing.Size(76, 13)
        Me.lblHozCm.TabIndex = 37
        Me.lblHozCm.Text = "Khoảng ?? cm"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 61)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(88, 13)
        Me.Label4.TabIndex = 36
        Me.Label4.Text = "Theo Chiều Dọc:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 27)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(100, 13)
        Me.Label3.TabIndex = 35
        Me.Label3.Text = "Theo Chiều Ngang:"
        '
        'nudVerBuffer
        '
        Me.nudVerBuffer.Location = New System.Drawing.Point(112, 59)
        Me.nudVerBuffer.Minimum = New Decimal(New Integer() {5, 0, 0, 0})
        Me.nudVerBuffer.Name = "nudVerBuffer"
        Me.nudVerBuffer.Size = New System.Drawing.Size(38, 20)
        Me.nudVerBuffer.TabIndex = 19
        Me.nudVerBuffer.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'nudHozBuffer
        '
        Me.nudHozBuffer.Location = New System.Drawing.Point(112, 25)
        Me.nudHozBuffer.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudHozBuffer.Name = "nudHozBuffer"
        Me.nudHozBuffer.Size = New System.Drawing.Size(38, 20)
        Me.nudHozBuffer.TabIndex = 18
        Me.nudHozBuffer.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'grbShowImg
        '
        Me.grbShowImg.Controls.Add(Me.rdFrameCompact)
        Me.grbShowImg.Controls.Add(Me.rdFrameFull)
        Me.grbShowImg.Location = New System.Drawing.Point(4, 6)
        Me.grbShowImg.Name = "grbShowImg"
        Me.grbShowImg.Size = New System.Drawing.Size(259, 52)
        Me.grbShowImg.TabIndex = 3
        Me.grbShowImg.TabStop = False
        Me.grbShowImg.Text = "Ảnh"
        '
        'rdFrameCompact
        '
        Me.rdFrameCompact.AutoSize = True
        Me.rdFrameCompact.Location = New System.Drawing.Point(126, 20)
        Me.rdFrameCompact.Name = "rdFrameCompact"
        Me.rdFrameCompact.Size = New System.Drawing.Size(114, 17)
        Me.rdFrameCompact.TabIndex = 5
        Me.rdFrameCompact.Text = "Không hiển thị ảnh"
        Me.rdFrameCompact.UseVisualStyleBackColor = True
        '
        'rdFrameFull
        '
        Me.rdFrameFull.AutoSize = True
        Me.rdFrameFull.Checked = True
        Me.rdFrameFull.Location = New System.Drawing.Point(19, 20)
        Me.rdFrameFull.Name = "rdFrameFull"
        Me.rdFrameFull.Size = New System.Drawing.Size(82, 17)
        Me.rdFrameFull.TabIndex = 4
        Me.rdFrameFull.TabStop = True
        Me.rdFrameFull.Text = "Hiển thị ảnh"
        Me.rdFrameFull.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.nudGeneration)
        Me.GroupBox1.Location = New System.Drawing.Point(272, 6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(259, 52)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Số thế hệ (đời) hiển thị tối đa"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(7, 22)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(73, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Hiển thị tối đa"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(128, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(41, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Thế hệ"
        '
        'nudGeneration
        '
        Me.nudGeneration.Location = New System.Drawing.Point(87, 20)
        Me.nudGeneration.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudGeneration.Name = "nudGeneration"
        Me.nudGeneration.Size = New System.Drawing.Size(38, 20)
        Me.nudGeneration.TabIndex = 7
        Me.nudGeneration.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'lblNote
        '
        Me.lblNote.AutoSize = True
        Me.lblNote.ForeColor = System.Drawing.Color.Red
        Me.lblNote.Location = New System.Drawing.Point(460, 70)
        Me.lblNote.Name = "lblNote"
        Me.lblNote.Size = New System.Drawing.Size(17, 13)
        Me.lblNote.TabIndex = 90
        Me.lblNote.Text = "(*)"
        Me.lblNote.Visible = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(271, 21)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(99, 13)
        Me.Label6.TabIndex = 89
        Me.Label6.Text = "Hiển thị thành viên:"
        Me.Label6.Visible = False
        '
        'cboTypeShowTree
        '
        Me.cboTypeShowTree.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTypeShowTree.FormattingEnabled = True
        Me.cboTypeShowTree.Location = New System.Drawing.Point(372, 18)
        Me.cboTypeShowTree.Name = "cboTypeShowTree"
        Me.cboTypeShowTree.Size = New System.Drawing.Size(82, 21)
        Me.cboTypeShowTree.TabIndex = 80
        Me.cboTypeShowTree.Visible = False
        '
        'grbFrame
        '
        Me.grbFrame.Controls.Add(Me.rdoShortVer)
        Me.grbFrame.Controls.Add(Me.rdoShortHoz)
        Me.grbFrame.Controls.Add(Me.cboFrameType)
        Me.grbFrame.Controls.Add(Me.usrMemCard2)
        Me.grbFrame.Controls.Add(Me.usrMemCard1)
        Me.grbFrame.Controls.Add(Me.rdCard2)
        Me.grbFrame.Controls.Add(Me.rdCard1)
        Me.grbFrame.Location = New System.Drawing.Point(4, 64)
        Me.grbFrame.Name = "grbFrame"
        Me.grbFrame.Size = New System.Drawing.Size(259, 282)
        Me.grbFrame.TabIndex = 8
        Me.grbFrame.TabStop = False
        Me.grbFrame.Text = "Chọn khung ảnh"
        '
        'rdoShortVer
        '
        Me.rdoShortVer.AutoSize = True
        Me.rdoShortVer.Location = New System.Drawing.Point(9, 450)
        Me.rdoShortVer.Name = "rdoShortVer"
        Me.rdoShortVer.Size = New System.Drawing.Size(111, 17)
        Me.rdoShortVer.TabIndex = 3
        Me.rdoShortVer.TabStop = True
        Me.rdoShortVer.Text = "Rút gọn - chữ dọc"
        Me.rdoShortVer.UseVisualStyleBackColor = True
        '
        'rdoShortHoz
        '
        Me.rdoShortHoz.AutoSize = True
        Me.rdoShortHoz.Location = New System.Drawing.Point(9, 427)
        Me.rdoShortHoz.Name = "rdoShortHoz"
        Me.rdoShortHoz.Size = New System.Drawing.Size(123, 17)
        Me.rdoShortHoz.TabIndex = 2
        Me.rdoShortHoz.TabStop = True
        Me.rdoShortHoz.Text = "Rút gọn - chữ ngang"
        Me.rdoShortHoz.UseVisualStyleBackColor = True
        '
        'cboFrameType
        '
        Me.cboFrameType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFrameType.FormattingEnabled = True
        Me.cboFrameType.Location = New System.Drawing.Point(19, 23)
        Me.cboFrameType.Name = "cboFrameType"
        Me.cboFrameType.Size = New System.Drawing.Size(221, 21)
        Me.cboFrameType.TabIndex = 9
        '
        'usrMemCard2
        '
        Me.usrMemCard2.BackColor = System.Drawing.Color.Transparent
        Me.usrMemCard2.CardCoor = Nothing
        Me.usrMemCard2.CardID = 0
        Me.usrMemCard2.CardLevel = 0
        Me.usrMemCard2.CardMouseDown = False
        Me.usrMemCard2.CardSelected = False
        Me.usrMemCard2.CardSize = GiaPha.clsEnum.emCardSize.LARGE
        Me.usrMemCard2.CardXCoor = 0
        Me.usrMemCard2.CardYCoor = 0
        Me.usrMemCard2.DrawLv = 0
        Me.usrMemCard2.Enabled = False
        Me.usrMemCard2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.usrMemCard2.Location = New System.Drawing.Point(30, 312)
        Me.usrMemCard2.Name = "usrMemCard2"
        Me.usrMemCard2.ParentID = -1
        Me.usrMemCard2.ShowAlias = True
        Me.usrMemCard2.ShowBirth = True
        Me.usrMemCard2.ShowDecease = True
        Me.usrMemCard2.ShowImage = True
        Me.usrMemCard2.ShowRemark = True
        Me.usrMemCard2.Size = New System.Drawing.Size(188, 135)
        Me.usrMemCard2.SpouseID = -1
        Me.usrMemCard2.TabIndex = 1
        '
        'usrMemCard1
        '
        Me.usrMemCard1.AliveStatus = False
        Me.usrMemCard1.AllowDrop = True
        Me.usrMemCard1.AutoValidate = System.Windows.Forms.AutoValidate.Disable
        Me.usrMemCard1.BackColor = System.Drawing.Color.White
        Me.usrMemCard1.BackgroundImage = Global.GiaPha.My.Resources.Resources.pic_frame
        Me.usrMemCard1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.usrMemCard1.CardBackground = Global.GiaPha.My.Resources.Resources.pic_frame
        Me.usrMemCard1.CardCoor = Nothing
        Me.usrMemCard1.CardGender = 0
        Me.usrMemCard1.CardID = 0
        Me.usrMemCard1.CardImage = Nothing
        Me.usrMemCard1.CardImageLocation = Nothing
        Me.usrMemCard1.CardMouseDown = False
        Me.usrMemCard1.CardName = "Ten thanh vien (ten thuong goi) 1900 Mất: 20/10/2012"
        Me.usrMemCard1.CardSelected = False
        Me.usrMemCard1.CardSize = GiaPha.clsEnum.emCardSize.LARGE
        Me.usrMemCard1.CardXCoor = 0
        Me.usrMemCard1.CardYCoor = 0
        Me.usrMemCard1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.usrMemCard1.DrawLv = 0
        Me.usrMemCard1.Enabled = False
        Me.usrMemCard1.Location = New System.Drawing.Point(70, 57)
        Me.usrMemCard1.Name = "usrMemCard1"
        Me.usrMemCard1.ParentID = -1
        Me.usrMemCard1.Size = New System.Drawing.Size(119, 169)
        Me.usrMemCard1.SpouseID = -1
        Me.usrMemCard1.TabIndex = 0
        Me.usrMemCard1.UseRotateText = False
        Me.usrMemCard1.Visible = False
        '
        'rdCard2
        '
        Me.rdCard2.AutoSize = True
        Me.rdCard2.Location = New System.Drawing.Point(11, 290)
        Me.rdCard2.Name = "rdCard2"
        Me.rdCard2.Size = New System.Drawing.Size(60, 17)
        Me.rdCard2.TabIndex = 1
        Me.rdCard2.Text = "Đầy đủ"
        Me.rdCard2.UseVisualStyleBackColor = True
        '
        'rdCard1
        '
        Me.rdCard1.AutoSize = True
        Me.rdCard1.Checked = True
        Me.rdCard1.Location = New System.Drawing.Point(19, 296)
        Me.rdCard1.Name = "rdCard1"
        Me.rdCard1.Size = New System.Drawing.Size(77, 17)
        Me.rdCard1.TabIndex = 4
        Me.rdCard1.TabStop = True
        Me.rdCard1.Text = "Khung ảnh"
        Me.rdCard1.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.grbTypeTextDisplay)
        Me.TabPage2.Controls.Add(Me.grbInfoCard)
        Me.TabPage2.Controls.Add(Me.grbCardSize)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(534, 352)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Cây rút gọn"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'grbTypeTextDisplay
        '
        Me.grbTypeTextDisplay.Controls.Add(Me.rdoTextTypeRight)
        Me.grbTypeTextDisplay.Controls.Add(Me.rdoTexTypeLeft)
        Me.grbTypeTextDisplay.Controls.Add(Me.rdoTextTypeNormal)
        Me.grbTypeTextDisplay.Location = New System.Drawing.Point(6, 99)
        Me.grbTypeTextDisplay.Name = "grbTypeTextDisplay"
        Me.grbTypeTextDisplay.Size = New System.Drawing.Size(522, 45)
        Me.grbTypeTextDisplay.TabIndex = 26
        Me.grbTypeTextDisplay.TabStop = False
        Me.grbTypeTextDisplay.Text = "Kiểu chữ hiển thị"
        '
        'rdoTextTypeRight
        '
        Me.rdoTextTypeRight.AutoSize = True
        Me.rdoTextTypeRight.Location = New System.Drawing.Point(363, 17)
        Me.rdoTextTypeRight.Name = "rdoTextTypeRight"
        Me.rdoTextTypeRight.Size = New System.Drawing.Size(66, 17)
        Me.rdoTextTypeRight.TabIndex = 29
        Me.rdoTextTypeRight.Text = "Xoay trái"
        Me.rdoTextTypeRight.UseVisualStyleBackColor = True
        '
        'rdoTexTypeLeft
        '
        Me.rdoTexTypeLeft.AutoSize = True
        Me.rdoTexTypeLeft.Location = New System.Drawing.Point(233, 17)
        Me.rdoTexTypeLeft.Name = "rdoTexTypeLeft"
        Me.rdoTexTypeLeft.Size = New System.Drawing.Size(72, 17)
        Me.rdoTexTypeLeft.TabIndex = 28
        Me.rdoTexTypeLeft.Text = "Xoay phải"
        Me.rdoTexTypeLeft.UseVisualStyleBackColor = True
        '
        'rdoTextTypeNormal
        '
        Me.rdoTextTypeNormal.AutoSize = True
        Me.rdoTextTypeNormal.Checked = True
        Me.rdoTextTypeNormal.Location = New System.Drawing.Point(87, 17)
        Me.rdoTextTypeNormal.Name = "rdoTextTypeNormal"
        Me.rdoTextTypeNormal.Size = New System.Drawing.Size(82, 17)
        Me.rdoTextTypeNormal.TabIndex = 27
        Me.rdoTextTypeNormal.TabStop = True
        Me.rdoTextTypeNormal.Text = "Bình thường"
        Me.rdoTextTypeNormal.UseVisualStyleBackColor = True
        '
        'grbInfoCard
        '
        Me.grbInfoCard.Controls.Add(Me.chkShowGender)
        Me.grbInfoCard.Controls.Add(Me.chkShowLevel)
        Me.grbInfoCard.Location = New System.Drawing.Point(6, 6)
        Me.grbInfoCard.Name = "grbInfoCard"
        Me.grbInfoCard.Size = New System.Drawing.Size(257, 91)
        Me.grbInfoCard.TabIndex = 20
        Me.grbInfoCard.TabStop = False
        Me.grbInfoCard.Text = "Nội dung hiển thị"
        '
        'chkShowGender
        '
        Me.chkShowGender.AutoSize = True
        Me.chkShowGender.Checked = True
        Me.chkShowGender.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowGender.Location = New System.Drawing.Point(10, 57)
        Me.chkShowGender.Name = "chkShowGender"
        Me.chkShowGender.Size = New System.Drawing.Size(180, 17)
        Me.chkShowGender.TabIndex = 22
        Me.chkShowGender.Text = "Hiển thị giới tính trên cây rút gọn"
        Me.chkShowGender.UseVisualStyleBackColor = True
        '
        'chkShowLevel
        '
        Me.chkShowLevel.AutoSize = True
        Me.chkShowLevel.Checked = True
        Me.chkShowLevel.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowLevel.Location = New System.Drawing.Point(10, 26)
        Me.chkShowLevel.Name = "chkShowLevel"
        Me.chkShowLevel.Size = New System.Drawing.Size(157, 17)
        Me.chkShowLevel.TabIndex = 21
        Me.chkShowLevel.Text = "Hiển thị đời trên cây rút gọn"
        Me.chkShowLevel.UseVisualStyleBackColor = True
        '
        'grbCardSize
        '
        Me.grbCardSize.Controls.Add(Me.lblVerSizeCm)
        Me.grbCardSize.Controls.Add(Me.lblHozSizeCm)
        Me.grbCardSize.Controls.Add(Me.Label10)
        Me.grbCardSize.Controls.Add(Me.Label11)
        Me.grbCardSize.Controls.Add(Me.nudVerSize)
        Me.grbCardSize.Controls.Add(Me.nudHozSize)
        Me.grbCardSize.Location = New System.Drawing.Point(269, 6)
        Me.grbCardSize.Name = "grbCardSize"
        Me.grbCardSize.Size = New System.Drawing.Size(257, 91)
        Me.grbCardSize.TabIndex = 23
        Me.grbCardSize.TabStop = False
        Me.grbCardSize.Text = "Kích thước khung"
        '
        'lblVerSizeCm
        '
        Me.lblVerSizeCm.AutoSize = True
        Me.lblVerSizeCm.Location = New System.Drawing.Point(128, 61)
        Me.lblVerSizeCm.Name = "lblVerSizeCm"
        Me.lblVerSizeCm.Size = New System.Drawing.Size(76, 13)
        Me.lblVerSizeCm.TabIndex = 38
        Me.lblVerSizeCm.Text = "Khoảng ?? cm"
        '
        'lblHozSizeCm
        '
        Me.lblHozSizeCm.AutoSize = True
        Me.lblHozSizeCm.Location = New System.Drawing.Point(128, 27)
        Me.lblHozSizeCm.Name = "lblHozSizeCm"
        Me.lblHozSizeCm.Size = New System.Drawing.Size(76, 13)
        Me.lblHozSizeCm.TabIndex = 37
        Me.lblHozSizeCm.Text = "Khoảng ?? cm"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(6, 61)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(60, 13)
        Me.Label10.TabIndex = 36
        Me.Label10.Text = "Chiều Dọc:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(6, 27)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(72, 13)
        Me.Label11.TabIndex = 35
        Me.Label11.Text = "Chiều Ngang:"
        '
        'nudVerSize
        '
        Me.nudVerSize.Location = New System.Drawing.Point(84, 59)
        Me.nudVerSize.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
        Me.nudVerSize.Minimum = New Decimal(New Integer() {30, 0, 0, 0})
        Me.nudVerSize.Name = "nudVerSize"
        Me.nudVerSize.Size = New System.Drawing.Size(38, 20)
        Me.nudVerSize.TabIndex = 25
        Me.nudVerSize.Value = New Decimal(New Integer() {30, 0, 0, 0})
        '
        'nudHozSize
        '
        Me.nudHozSize.Location = New System.Drawing.Point(84, 25)
        Me.nudHozSize.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
        Me.nudHozSize.Minimum = New Decimal(New Integer() {5, 0, 0, 0})
        Me.nudHozSize.Name = "nudHozSize"
        Me.nudHozSize.Size = New System.Drawing.Size(38, 20)
        Me.nudHozSize.TabIndex = 24
        Me.nudHozSize.Value = New Decimal(New Integer() {30, 0, 0, 0})
        '
        'plnControl
        '
        Me.plnControl.Controls.Add(Me.btnCancel)
        Me.plnControl.Controls.Add(Me.btnOK)
        Me.plnControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.plnControl.Location = New System.Drawing.Point(3, 387)
        Me.plnControl.Name = "plnControl"
        Me.plnControl.Size = New System.Drawing.Size(542, 46)
        Me.plnControl.TabIndex = 30
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Image = Global.GiaPha.My.Resources.Resources.back_32
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(305, 3)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(106, 40)
        Me.btnCancel.TabIndex = 32
        Me.btnCancel.Text = "     Thoát"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Image = Global.GiaPha.My.Resources.Resources.task_done
        Me.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnOK.Location = New System.Drawing.Point(132, 3)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(106, 40)
        Me.btnOK.TabIndex = 31
        Me.btnOK.Text = "Hoàn thành"
        Me.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'frmOption
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(548, 436)
        Me.Controls.Add(Me.MainLayout)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmOption"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Tùy chọn vẽ cây"
        Me.MainLayout.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.tabGeneral.PerformLayout()
        Me.grbShowDieDay.ResumeLayout(False)
        Me.grbShowDieDay.PerformLayout()
        Me.grbFomat.ResumeLayout(False)
        Me.grbFomat.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.nudVerBuffer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudHozBuffer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grbShowImg.ResumeLayout(False)
        Me.grbShowImg.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.nudGeneration, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grbFrame.ResumeLayout(False)
        Me.grbFrame.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.grbTypeTextDisplay.ResumeLayout(False)
        Me.grbTypeTextDisplay.PerformLayout()
        Me.grbInfoCard.ResumeLayout(False)
        Me.grbInfoCard.PerformLayout()
        Me.grbCardSize.ResumeLayout(False)
        Me.grbCardSize.PerformLayout()
        CType(Me.nudVerSize, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudHozSize, System.ComponentModel.ISupportInitialize).EndInit()
        Me.plnControl.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dlgColor As ColorDialog
    Friend WithEvents MainLayout As TableLayoutPanel
    Friend WithEvents tabMain As TabControl
    Friend WithEvents tabGeneral As TabPage
    Friend WithEvents grbShowDieDay As GroupBox
    Friend WithEvents rdDeadMoonCalendarShow As RadioButton
    Friend WithEvents rdDeadSunCalendarShow As RadioButton
    Friend WithEvents grbFomat As GroupBox
    Friend WithEvents chkBackgroupColorDie As CheckBox
    Friend WithEvents lblBackgroupColorDie As Label
    Friend WithEvents lblTextColor As Label
    Friend WithEvents lblBackgroupColor As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents lblBackgroup As Label
    Friend WithEvents cboFont As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents lblVerCm As Label
    Friend WithEvents lblHozCm As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents nudVerBuffer As NumericUpDown
    Friend WithEvents nudHozBuffer As NumericUpDown
    Friend WithEvents grbShowImg As GroupBox
    Friend WithEvents rdFrameCompact As RadioButton
    Friend WithEvents rdFrameFull As RadioButton
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents nudGeneration As NumericUpDown
    Friend WithEvents lblNote As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents cboTypeShowTree As ComboBox
    Friend WithEvents grbFrame As GroupBox
    Friend WithEvents rdoShortVer As RadioButton
    Friend WithEvents rdoShortHoz As RadioButton
    Friend WithEvents cboFrameType As ComboBox
    Friend WithEvents usrMemCard2 As usrMemberCard2
    Friend WithEvents usrMemCard1 As usrMemberCard1
    Friend WithEvents rdCard2 As RadioButton
    Friend WithEvents rdCard1 As RadioButton
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents plnControl As Panel
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnOK As Button
    Friend WithEvents grbCardSize As GroupBox
    Friend WithEvents lblVerSizeCm As Label
    Friend WithEvents lblHozSizeCm As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents nudVerSize As NumericUpDown
    Friend WithEvents nudHozSize As NumericUpDown
    Friend WithEvents chkShowUnknownBirthDay As CheckBox
    Friend WithEvents grbInfoCard As GroupBox
    Friend WithEvents chkShowGender As CheckBox
    Friend WithEvents chkShowLevel As CheckBox
    Friend WithEvents grbTypeTextDisplay As GroupBox
    Friend WithEvents rdoTextTypeRight As RadioButton
    Friend WithEvents rdoTexTypeLeft As RadioButton
    Friend WithEvents rdoTextTypeNormal As RadioButton
End Class
