<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCalendar
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCalendar))
        Me.grbSunCal = New System.Windows.Forms.GroupBox()
        Me.btnSunClear = New System.Windows.Forms.Button()
        Me.txtSunYear = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.calSun = New System.Windows.Forms.MonthCalendar()
        Me.cbSunMon = New System.Windows.Forms.ComboBox()
        Me.cbSunDay = New System.Windows.Forms.ComboBox()
        Me.grbLunCal = New System.Windows.Forms.GroupBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.lblLunYear = New System.Windows.Forms.Label()
        Me.txtLunYear = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cbLunMon = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cbLunDay = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.grbSunCal.SuspendLayout()
        Me.grbLunCal.SuspendLayout()
        Me.SuspendLayout()
        '
        'grbSunCal
        '
        Me.grbSunCal.Controls.Add(Me.btnSunClear)
        Me.grbSunCal.Controls.Add(Me.txtSunYear)
        Me.grbSunCal.Controls.Add(Me.Label3)
        Me.grbSunCal.Controls.Add(Me.Label2)
        Me.grbSunCal.Controls.Add(Me.Label1)
        Me.grbSunCal.Controls.Add(Me.calSun)
        Me.grbSunCal.Controls.Add(Me.cbSunMon)
        Me.grbSunCal.Controls.Add(Me.cbSunDay)
        Me.grbSunCal.Location = New System.Drawing.Point(12, 11)
        Me.grbSunCal.Name = "grbSunCal"
        Me.grbSunCal.Size = New System.Drawing.Size(336, 279)
        Me.grbSunCal.TabIndex = 0
        Me.grbSunCal.TabStop = False
        Me.grbSunCal.Text = "Dương lịch"
        '
        'btnSunClear
        '
        Me.btnSunClear.Location = New System.Drawing.Point(252, 50)
        Me.btnSunClear.Name = "btnSunClear"
        Me.btnSunClear.Size = New System.Drawing.Size(75, 21)
        Me.btnSunClear.TabIndex = 40
        Me.btnSunClear.Text = "Xóa"
        Me.btnSunClear.UseVisualStyleBackColor = True
        '
        'txtSunYear
        '
        Me.txtSunYear.Location = New System.Drawing.Point(139, 50)
        Me.txtSunYear.MaxLength = 4
        Me.txtSunYear.Name = "txtSunYear"
        Me.txtSunYear.Size = New System.Drawing.Size(72, 19)
        Me.txtSunYear.TabIndex = 30
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(136, 31)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(28, 12)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Năm"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(83, 31)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(36, 12)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Tháng"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(33, 31)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(31, 12)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Ngày"
        '
        'calSun
        '
        Me.calSun.Location = New System.Drawing.Point(60, 79)
        Me.calSun.Margin = New System.Windows.Forms.Padding(9, 8, 9, 8)
        Me.calSun.Name = "calSun"
        Me.calSun.TabIndex = 50
        '
        'cbSunMon
        '
        Me.cbSunMon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSunMon.FormattingEnabled = True
        Me.cbSunMon.Items.AddRange(New Object() {"", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"})
        Me.cbSunMon.Location = New System.Drawing.Point(86, 50)
        Me.cbSunMon.Name = "cbSunMon"
        Me.cbSunMon.Size = New System.Drawing.Size(47, 20)
        Me.cbSunMon.TabIndex = 20
        '
        'cbSunDay
        '
        Me.cbSunDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSunDay.FormattingEnabled = True
        Me.cbSunDay.Items.AddRange(New Object() {"", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31"})
        Me.cbSunDay.Location = New System.Drawing.Point(33, 50)
        Me.cbSunDay.Name = "cbSunDay"
        Me.cbSunDay.Size = New System.Drawing.Size(47, 20)
        Me.cbSunDay.TabIndex = 10
        '
        'grbLunCal
        '
        Me.grbLunCal.Controls.Add(Me.Button1)
        Me.grbLunCal.Controls.Add(Me.lblLunYear)
        Me.grbLunCal.Controls.Add(Me.txtLunYear)
        Me.grbLunCal.Controls.Add(Me.Label6)
        Me.grbLunCal.Controls.Add(Me.cbLunMon)
        Me.grbLunCal.Controls.Add(Me.Label5)
        Me.grbLunCal.Controls.Add(Me.cbLunDay)
        Me.grbLunCal.Controls.Add(Me.Label4)
        Me.grbLunCal.Location = New System.Drawing.Point(12, 307)
        Me.grbLunCal.Name = "grbLunCal"
        Me.grbLunCal.Size = New System.Drawing.Size(336, 116)
        Me.grbLunCal.TabIndex = 1
        Me.grbLunCal.TabStop = False
        Me.grbLunCal.Text = "Âm lịch"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(252, 46)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 21)
        Me.Button1.TabIndex = 90
        Me.Button1.Text = "Xóa"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'lblLunYear
        '
        Me.lblLunYear.AutoSize = True
        Me.lblLunYear.Location = New System.Drawing.Point(250, 45)
        Me.lblLunYear.Name = "lblLunYear"
        Me.lblLunYear.Size = New System.Drawing.Size(0, 12)
        Me.lblLunYear.TabIndex = 4
        '
        'txtLunYear
        '
        Me.txtLunYear.Location = New System.Drawing.Point(172, 46)
        Me.txtLunYear.MaxLength = 4
        Me.txtLunYear.Name = "txtLunYear"
        Me.txtLunYear.Size = New System.Drawing.Size(72, 19)
        Me.txtLunYear.TabIndex = 80
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(169, 27)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(28, 12)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Năm"
        '
        'cbLunMon
        '
        Me.cbLunMon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbLunMon.FormattingEnabled = True
        Me.cbLunMon.Items.AddRange(New Object() {"", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"})
        Me.cbLunMon.Location = New System.Drawing.Point(96, 46)
        Me.cbLunMon.Name = "cbLunMon"
        Me.cbLunMon.Size = New System.Drawing.Size(70, 20)
        Me.cbLunMon.TabIndex = 70
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(93, 27)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(36, 12)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Tháng"
        '
        'cbLunDay
        '
        Me.cbLunDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbLunDay.FormattingEnabled = True
        Me.cbLunDay.Items.AddRange(New Object() {"", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30"})
        Me.cbLunDay.Location = New System.Drawing.Point(23, 46)
        Me.cbLunDay.Name = "cbLunDay"
        Me.cbLunDay.Size = New System.Drawing.Size(67, 20)
        Me.cbLunDay.TabIndex = 60
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(20, 27)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(31, 12)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Ngày"
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOK.Image = Global.GiaPha.My.Resources.Resources.task_done
        Me.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnOK.Location = New System.Drawing.Point(72, 435)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(98, 35)
        Me.btnOK.TabIndex = 100
        Me.btnOK.Text = "      Chọn"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Image = Global.GiaPha.My.Resources.Resources.back_32
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(176, 435)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(98, 35)
        Me.btnCancel.TabIndex = 110
        Me.btnCancel.Text = "      Trở về"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'frmCalendar
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(360, 481)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.grbLunCal)
        Me.Controls.Add(Me.grbSunCal)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCalendar"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Lịch"
        Me.grbSunCal.ResumeLayout(False)
        Me.grbSunCal.PerformLayout()
        Me.grbLunCal.ResumeLayout(False)
        Me.grbLunCal.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grbSunCal As System.Windows.Forms.GroupBox
    Friend WithEvents grbLunCal As System.Windows.Forms.GroupBox
    Friend WithEvents calSun As System.Windows.Forms.MonthCalendar
    Friend WithEvents cbSunMon As System.Windows.Forms.ComboBox
    Friend WithEvents cbSunDay As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cbLunMon As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cbLunDay As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtSunYear As System.Windows.Forms.TextBox
    Friend WithEvents txtLunYear As System.Windows.Forms.TextBox
    Friend WithEvents btnSunClear As System.Windows.Forms.Button
    Friend WithEvents lblLunYear As System.Windows.Forms.Label
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
End Class
