<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPdfOption
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPdfOption))
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.grbFrame = New System.Windows.Forms.GroupBox()
        Me.cboLocation = New System.Windows.Forms.ComboBox()
        Me.lblLocation = New System.Windows.Forms.Label()
        Me.chkBorder = New System.Windows.Forms.CheckBox()
        Me.txtCreateMember = New System.Windows.Forms.TextBox()
        Me.chkCreateMember = New System.Windows.Forms.CheckBox()
        Me.txtRootInfo = New System.Windows.Forms.TextBox()
        Me.txtFamilyAnniInfo = New System.Windows.Forms.TextBox()
        Me.txtFamilyInfo = New System.Windows.Forms.TextBox()
        Me.dtpDate = New System.Windows.Forms.DateTimePicker()
        Me.chkRootInfo = New System.Windows.Forms.CheckBox()
        Me.chkFamilyAnniInfo = New System.Windows.Forms.CheckBox()
        Me.chkFamilyInfo = New System.Windows.Forms.CheckBox()
        Me.chkDate = New System.Windows.Forms.CheckBox()
        Me.grbModeExport = New System.Windows.Forms.GroupBox()
        Me.txtGeneration = New System.Windows.Forms.MaskedTextBox()
        Me.rdoGeneration = New System.Windows.Forms.RadioButton()
        Me.rdoAllGeneration = New System.Windows.Forms.RadioButton()
        Me.grbFrame.SuspendLayout()
        Me.grbModeExport.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Image = Global.GiaPha.My.Resources.Resources.back_32
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(299, 281)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(87, 40)
        Me.btnCancel.TabIndex = 70
        Me.btnCancel.Text = "       Thoát"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOK.Image = Global.GiaPha.My.Resources.Resources.pdficon
        Me.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnOK.Location = New System.Drawing.Point(160, 281)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(93, 40)
        Me.btnOK.TabIndex = 60
        Me.btnOK.Text = "     Tạo file"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'grbFrame
        '
        Me.grbFrame.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.grbFrame.Controls.Add(Me.cboLocation)
        Me.grbFrame.Controls.Add(Me.lblLocation)
        Me.grbFrame.Controls.Add(Me.chkBorder)
        Me.grbFrame.Controls.Add(Me.txtCreateMember)
        Me.grbFrame.Controls.Add(Me.chkCreateMember)
        Me.grbFrame.Controls.Add(Me.txtRootInfo)
        Me.grbFrame.Controls.Add(Me.txtFamilyAnniInfo)
        Me.grbFrame.Controls.Add(Me.txtFamilyInfo)
        Me.grbFrame.Controls.Add(Me.dtpDate)
        Me.grbFrame.Controls.Add(Me.chkRootInfo)
        Me.grbFrame.Controls.Add(Me.chkFamilyAnniInfo)
        Me.grbFrame.Controls.Add(Me.chkFamilyInfo)
        Me.grbFrame.Controls.Add(Me.chkDate)
        Me.grbFrame.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grbFrame.Location = New System.Drawing.Point(12, 7)
        Me.grbFrame.Name = "grbFrame"
        Me.grbFrame.Size = New System.Drawing.Size(523, 213)
        Me.grbFrame.TabIndex = 0
        Me.grbFrame.TabStop = False
        Me.grbFrame.Text = "Chọn các thông tin muốn hiển thị"
        '
        'cboLocation
        '
        Me.cboLocation.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboLocation.FormattingEnabled = True
        Me.cboLocation.Location = New System.Drawing.Point(361, 59)
        Me.cboLocation.Name = "cboLocation"
        Me.cboLocation.Size = New System.Drawing.Size(150, 22)
        Me.cboLocation.TabIndex = 82
        '
        'lblLocation
        '
        Me.lblLocation.AutoSize = True
        Me.lblLocation.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLocation.Location = New System.Drawing.Point(322, 62)
        Me.lblLocation.Name = "lblLocation"
        Me.lblLocation.Size = New System.Drawing.Size(32, 14)
        Me.lblLocation.TabIndex = 81
        Me.lblLocation.Text = "Vị trí"
        '
        'chkBorder
        '
        Me.chkBorder.AutoSize = True
        Me.chkBorder.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkBorder.Location = New System.Drawing.Point(411, 179)
        Me.chkBorder.Name = "chkBorder"
        Me.chkBorder.Size = New System.Drawing.Size(105, 18)
        Me.chkBorder.TabIndex = 75
        Me.chkBorder.Text = "Hiện khung vẽ"
        Me.chkBorder.UseVisualStyleBackColor = True
        '
        'txtCreateMember
        '
        Me.txtCreateMember.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCreateMember.Location = New System.Drawing.Point(119, 59)
        Me.txtCreateMember.Name = "txtCreateMember"
        Me.txtCreateMember.Size = New System.Drawing.Size(188, 22)
        Me.txtCreateMember.TabIndex = 80
        '
        'chkCreateMember
        '
        Me.chkCreateMember.AutoSize = True
        Me.chkCreateMember.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCreateMember.Location = New System.Drawing.Point(12, 61)
        Me.chkCreateMember.Name = "chkCreateMember"
        Me.chkCreateMember.Size = New System.Drawing.Size(77, 18)
        Me.chkCreateMember.TabIndex = 79
        Me.chkCreateMember.Text = "Người lập"
        Me.chkCreateMember.UseVisualStyleBackColor = True
        '
        'txtRootInfo
        '
        Me.txtRootInfo.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRootInfo.Location = New System.Drawing.Point(119, 177)
        Me.txtRootInfo.Name = "txtRootInfo"
        Me.txtRootInfo.Size = New System.Drawing.Size(281, 22)
        Me.txtRootInfo.TabIndex = 78
        '
        'txtFamilyAnniInfo
        '
        Me.txtFamilyAnniInfo.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFamilyAnniInfo.Location = New System.Drawing.Point(119, 137)
        Me.txtFamilyAnniInfo.Name = "txtFamilyAnniInfo"
        Me.txtFamilyAnniInfo.Size = New System.Drawing.Size(392, 22)
        Me.txtFamilyAnniInfo.TabIndex = 77
        '
        'txtFamilyInfo
        '
        Me.txtFamilyInfo.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFamilyInfo.Location = New System.Drawing.Point(119, 98)
        Me.txtFamilyInfo.Name = "txtFamilyInfo"
        Me.txtFamilyInfo.Size = New System.Drawing.Size(392, 22)
        Me.txtFamilyInfo.TabIndex = 76
        '
        'dtpDate
        '
        Me.dtpDate.CustomFormat = "dd/MM/yyyy"
        Me.dtpDate.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpDate.Location = New System.Drawing.Point(119, 24)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Size = New System.Drawing.Size(94, 22)
        Me.dtpDate.TabIndex = 75
        '
        'chkRootInfo
        '
        Me.chkRootInfo.AutoSize = True
        Me.chkRootInfo.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRootInfo.Location = New System.Drawing.Point(12, 179)
        Me.chkRootInfo.Name = "chkRootInfo"
        Me.chkRootInfo.Size = New System.Drawing.Size(111, 18)
        Me.chkRootInfo.TabIndex = 74
        Me.chkRootInfo.Text = "Thành viên gốc"
        Me.chkRootInfo.UseVisualStyleBackColor = True
        '
        'chkFamilyAnniInfo
        '
        Me.chkFamilyAnniInfo.AutoSize = True
        Me.chkFamilyAnniInfo.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkFamilyAnniInfo.Location = New System.Drawing.Point(12, 139)
        Me.chkFamilyAnniInfo.Name = "chkFamilyAnniInfo"
        Me.chkFamilyAnniInfo.Size = New System.Drawing.Size(80, 18)
        Me.chkFamilyAnniInfo.TabIndex = 73
        Me.chkFamilyAnniInfo.Text = "Quê quán"
        Me.chkFamilyAnniInfo.UseVisualStyleBackColor = True
        '
        'chkFamilyInfo
        '
        Me.chkFamilyInfo.AutoSize = True
        Me.chkFamilyInfo.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkFamilyInfo.Location = New System.Drawing.Point(12, 100)
        Me.chkFamilyInfo.Name = "chkFamilyInfo"
        Me.chkFamilyInfo.Size = New System.Drawing.Size(98, 18)
        Me.chkFamilyInfo.TabIndex = 72
        Me.chkFamilyInfo.Text = "Tên dòng họ"
        Me.chkFamilyInfo.UseVisualStyleBackColor = True
        '
        'chkDate
        '
        Me.chkDate.AutoSize = True
        Me.chkDate.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDate.Location = New System.Drawing.Point(12, 28)
        Me.chkDate.Name = "chkDate"
        Me.chkDate.Size = New System.Drawing.Size(75, 18)
        Me.chkDate.TabIndex = 71
        Me.chkDate.Text = "Ngày tạo"
        Me.chkDate.UseVisualStyleBackColor = True
        '
        'grbModeExport
        '
        Me.grbModeExport.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.grbModeExport.Controls.Add(Me.txtGeneration)
        Me.grbModeExport.Controls.Add(Me.rdoGeneration)
        Me.grbModeExport.Controls.Add(Me.rdoAllGeneration)
        Me.grbModeExport.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grbModeExport.Location = New System.Drawing.Point(12, 230)
        Me.grbModeExport.Name = "grbModeExport"
        Me.grbModeExport.Size = New System.Drawing.Size(523, 46)
        Me.grbModeExport.TabIndex = 71
        Me.grbModeExport.TabStop = False
        Me.grbModeExport.Text = "Chọn chế độ in"
        '
        'txtGeneration
        '
        Me.txtGeneration.Culture = New System.Globalization.CultureInfo("vi-VN")
        Me.txtGeneration.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGeneration.Location = New System.Drawing.Point(470, 16)
        Me.txtGeneration.Mask = "000"
        Me.txtGeneration.Name = "txtGeneration"
        Me.txtGeneration.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.txtGeneration.Size = New System.Drawing.Size(41, 22)
        Me.txtGeneration.TabIndex = 2
        Me.txtGeneration.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'rdoGeneration
        '
        Me.rdoGeneration.AutoSize = True
        Me.rdoGeneration.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rdoGeneration.Location = New System.Drawing.Point(206, 16)
        Me.rdoGeneration.Name = "rdoGeneration"
        Me.rdoGeneration.Size = New System.Drawing.Size(258, 18)
        Me.rdoGeneration.TabIndex = 1
        Me.rdoGeneration.TabStop = True
        Me.rdoGeneration.Text = "Chia nhỏ cây theo số đời trong một nhánh"
        Me.rdoGeneration.UseVisualStyleBackColor = True
        '
        'rdoAllGeneration
        '
        Me.rdoAllGeneration.AutoSize = True
        Me.rdoAllGeneration.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rdoAllGeneration.Location = New System.Drawing.Point(129, 16)
        Me.rdoAllGeneration.Name = "rdoAllGeneration"
        Me.rdoAllGeneration.Size = New System.Drawing.Size(71, 18)
        Me.rdoAllGeneration.TabIndex = 0
        Me.rdoAllGeneration.TabStop = True
        Me.rdoAllGeneration.Text = "Toàn bộ"
        Me.rdoAllGeneration.UseVisualStyleBackColor = True
        '
        'frmPdfOption
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(547, 332)
        Me.Controls.Add(Me.grbModeExport)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.grbFrame)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPdfOption"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Tùy chọn tạo file PDF"
        Me.grbFrame.ResumeLayout(False)
        Me.grbFrame.PerformLayout()
        Me.grbModeExport.ResumeLayout(False)
        Me.grbModeExport.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents grbFrame As System.Windows.Forms.GroupBox
    Friend WithEvents chkDate As System.Windows.Forms.CheckBox
    Friend WithEvents chkRootInfo As System.Windows.Forms.CheckBox
    Friend WithEvents chkFamilyAnniInfo As System.Windows.Forms.CheckBox
    Friend WithEvents chkFamilyInfo As System.Windows.Forms.CheckBox
    Friend WithEvents txtRootInfo As System.Windows.Forms.TextBox
    Friend WithEvents txtFamilyAnniInfo As System.Windows.Forms.TextBox
    Friend WithEvents txtFamilyInfo As System.Windows.Forms.TextBox
    Friend WithEvents dtpDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents txtCreateMember As System.Windows.Forms.TextBox
    Friend WithEvents chkCreateMember As System.Windows.Forms.CheckBox
    Friend WithEvents chkBorder As System.Windows.Forms.CheckBox
    Friend WithEvents grbModeExport As System.Windows.Forms.GroupBox
    Friend WithEvents txtGeneration As System.Windows.Forms.MaskedTextBox
    Friend WithEvents rdoGeneration As System.Windows.Forms.RadioButton
    Friend WithEvents rdoAllGeneration As System.Windows.Forms.RadioButton
    Friend WithEvents cboLocation As System.Windows.Forms.ComboBox
    Friend WithEvents lblLocation As System.Windows.Forms.Label
End Class
