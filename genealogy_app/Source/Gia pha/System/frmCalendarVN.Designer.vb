<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCalendarVN
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCalendarVN))
        Me.btnChoose = New System.Windows.Forms.Button
        Me.btnBack = New System.Windows.Forms.Button
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.txtYear = New System.Windows.Forms.TextBox
        Me.cbDay = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblVnCal = New System.Windows.Forms.Label
        Me.cbMonth = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.lblEnCal = New System.Windows.Forms.Label
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnChoose
        '
        Me.btnChoose.Image = Global.GiaPha.My.Resources.Resources.ok24
        Me.btnChoose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnChoose.Location = New System.Drawing.Point(116, 221)
        Me.btnChoose.Name = "btnChoose"
        Me.btnChoose.Size = New System.Drawing.Size(96, 41)
        Me.btnChoose.TabIndex = 4
        Me.btnChoose.Text = "     Chọn"
        Me.btnChoose.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Image = Global.GiaPha.My.Resources.Resources.back_24
        Me.btnBack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnBack.Location = New System.Drawing.Point(237, 221)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(96, 41)
        Me.btnBack.TabIndex = 5
        Me.btnBack.Text = "     Quay lại"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtYear)
        Me.GroupBox2.Controls.Add(Me.cbDay)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.lblVnCal)
        Me.GroupBox2.Controls.Add(Me.cbMonth)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 15)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(443, 101)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Âm lịch"
        '
        'txtYear
        '
        Me.txtYear.Location = New System.Drawing.Point(75, 21)
        Me.txtYear.MaxLength = 4
        Me.txtYear.Name = "txtYear"
        Me.txtYear.Size = New System.Drawing.Size(60, 22)
        Me.txtYear.TabIndex = 1
        '
        'cbDay
        '
        Me.cbDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbDay.Enabled = False
        Me.cbDay.FormattingEnabled = True
        Me.cbDay.Location = New System.Drawing.Point(319, 21)
        Me.cbDay.Name = "cbDay"
        Me.cbDay.Size = New System.Drawing.Size(47, 24)
        Me.cbDay.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(277, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Ngày"
        '
        'lblVnCal
        '
        Me.lblVnCal.Font = New System.Drawing.Font("Arial", 12.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVnCal.ForeColor = System.Drawing.Color.Red
        Me.lblVnCal.Location = New System.Drawing.Point(2, 62)
        Me.lblVnCal.Name = "lblVnCal"
        Me.lblVnCal.Size = New System.Drawing.Size(435, 22)
        Me.lblVnCal.TabIndex = 6
        Me.lblVnCal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cbMonth
        '
        Me.cbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbMonth.Enabled = False
        Me.cbMonth.FormattingEnabled = True
        Me.cbMonth.Location = New System.Drawing.Point(196, 21)
        Me.cbMonth.Name = "cbMonth"
        Me.cbMonth.Size = New System.Drawing.Size(60, 24)
        Me.cbMonth.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(30, 24)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(39, 16)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Năm:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(145, 24)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(47, 16)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Tháng:"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblEnCal)
        Me.GroupBox1.Location = New System.Drawing.Point(11, 132)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(444, 73)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Dương lịch"
        '
        'lblEnCal
        '
        Me.lblEnCal.Font = New System.Drawing.Font("Arial", 12.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEnCal.ForeColor = System.Drawing.Color.Blue
        Me.lblEnCal.Location = New System.Drawing.Point(1, 29)
        Me.lblEnCal.Name = "lblEnCal"
        Me.lblEnCal.Size = New System.Drawing.Size(437, 22)
        Me.lblEnCal.TabIndex = 7
        Me.lblEnCal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmCalendarVN
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(467, 283)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnBack)
        Me.Controls.Add(Me.btnChoose)
        Me.Controls.Add(Me.GroupBox2)
        Me.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCalendarVN"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Chuyển đổi lịch"
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnChoose As System.Windows.Forms.Button
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cbMonth As System.Windows.Forms.ComboBox
    Friend WithEvents cbDay As System.Windows.Forms.ComboBox
    Friend WithEvents lblVnCal As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtYear As System.Windows.Forms.TextBox
    Friend WithEvents lblEnCal As System.Windows.Forms.Label
End Class
