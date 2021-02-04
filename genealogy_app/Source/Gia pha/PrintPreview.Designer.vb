<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PrintPreview
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PrintPreview))
        Me.cboPagesize = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.rbtLand = New System.Windows.Forms.RadioButton()
        Me.rbtPortrait = New System.Windows.Forms.RadioButton()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnEnd = New System.Windows.Forms.Button()
        Me.btnExcelNormal = New System.Windows.Forms.Button()
        Me.btnPDF = New System.Windows.Forms.Button()
        Me.pagePreview = New PdfSharp.Forms.PagePreview()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cboZoom = New System.Windows.Forms.ComboBox()
        Me.btnExcelAdvance = New System.Windows.Forms.Button()
        Me.lblWarnning = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.btSetting = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'cboPagesize
        '
        Me.cboPagesize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPagesize.FormattingEnabled = True
        Me.cboPagesize.Items.AddRange(New Object() {"Tự động", "A0 (1189 x 841 mm)", "A1 (841 x 594 mm)", "A2 (594 x 420 mm)", "A3 (420 x 297 mm)", "A4 (297 x 210 mm)", "A5 (210 x 148 mm)"})
        Me.cboPagesize.Location = New System.Drawing.Point(66, 27)
        Me.cboPagesize.Name = "cboPagesize"
        Me.cboPagesize.Size = New System.Drawing.Size(148, 21)
        Me.cboPagesize.TabIndex = 7
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(45, 13)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Cỡ giấy:"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rbtLand)
        Me.GroupBox1.Controls.Add(Me.rbtPortrait)
        Me.GroupBox1.Location = New System.Drawing.Point(222, 20)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(72, 70)
        Me.GroupBox1.TabIndex = 11
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Chiều giấy"
        '
        'rbtLand
        '
        Me.rbtLand.AutoSize = True
        Me.rbtLand.Checked = True
        Me.rbtLand.Location = New System.Drawing.Point(9, 47)
        Me.rbtLand.Name = "rbtLand"
        Me.rbtLand.Size = New System.Drawing.Size(57, 17)
        Me.rbtLand.TabIndex = 1
        Me.rbtLand.TabStop = True
        Me.rbtLand.Text = "Ngang"
        Me.rbtLand.UseVisualStyleBackColor = True
        '
        'rbtPortrait
        '
        Me.rbtPortrait.AutoSize = True
        Me.rbtPortrait.Location = New System.Drawing.Point(11, 22)
        Me.rbtPortrait.Name = "rbtPortrait"
        Me.rbtPortrait.Size = New System.Drawing.Size(45, 17)
        Me.rbtPortrait.TabIndex = 0
        Me.rbtPortrait.Text = "Dọc"
        Me.rbtPortrait.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Image = Global.GiaPha.My.Resources.Resources.printer32
        Me.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPrint.Location = New System.Drawing.Point(392, 17)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(72, 39)
        Me.btnPrint.TabIndex = 16
        Me.btnPrint.Text = "In   "
        Me.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnEnd
        '
        Me.btnEnd.Image = Global.GiaPha.My.Resources.Resources.back_32
        Me.btnEnd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnEnd.Location = New System.Drawing.Point(890, 27)
        Me.btnEnd.Name = "btnEnd"
        Me.btnEnd.Size = New System.Drawing.Size(90, 39)
        Me.btnEnd.TabIndex = 17
        Me.btnEnd.Text = "Quay lại"
        Me.btnEnd.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnEnd.UseVisualStyleBackColor = True
        '
        'btnExcelNormal
        '
        Me.btnExcelNormal.Image = Global.GiaPha.My.Resources.Resources.Excel
        Me.btnExcelNormal.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExcelNormal.Location = New System.Drawing.Point(470, 17)
        Me.btnExcelNormal.Name = "btnExcelNormal"
        Me.btnExcelNormal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnExcelNormal.Size = New System.Drawing.Size(135, 39)
        Me.btnExcelNormal.TabIndex = 18
        Me.btnExcelNormal.Text = "Excel Tiêu chuẩn"
        Me.btnExcelNormal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExcelNormal.UseVisualStyleBackColor = True
        '
        'btnPDF
        '
        Me.btnPDF.Image = Global.GiaPha.My.Resources.Resources.pdficon
        Me.btnPDF.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPDF.Location = New System.Drawing.Point(392, 61)
        Me.btnPDF.Name = "btnPDF"
        Me.btnPDF.Size = New System.Drawing.Size(72, 39)
        Me.btnPDF.TabIndex = 19
        Me.btnPDF.Text = "PDF"
        Me.btnPDF.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPDF.UseVisualStyleBackColor = True
        '
        'pagePreview
        '
        Me.pagePreview.BackColor = System.Drawing.SystemColors.Control
        Me.pagePreview.DesktopColor = System.Drawing.SystemColors.ControlDark
        Me.pagePreview.Location = New System.Drawing.Point(12, 115)
        Me.pagePreview.Name = "pagePreview"
        Me.pagePreview.PageColor = System.Drawing.Color.GhostWhite
        Me.pagePreview.PageSize = CType(resources.GetObject("pagePreview.PageSize"), PdfSharp.Drawing.XSize)
        Me.pagePreview.PageSizeF = New System.Drawing.Size(5500, 3000)
        Me.pagePreview.Size = New System.Drawing.Size(968, 537)
        Me.pagePreview.TabIndex = 21
        Me.pagePreview.Zoom = PdfSharp.Forms.Zoom.BestFit
        Me.pagePreview.ZoomPercent = 12
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.btSetting)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.cboZoom)
        Me.GroupBox2.Location = New System.Drawing.Point(6, 3)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(380, 103)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(10, 61)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(37, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Zoom:"
        '
        'cboZoom
        '
        Me.cboZoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboZoom.FormattingEnabled = True
        Me.cboZoom.Items.AddRange(New Object() {"800", "600", "400", "200", "150", "100", "75", "50", "25", "Tự động"})
        Me.cboZoom.Location = New System.Drawing.Point(57, 57)
        Me.cboZoom.Name = "cboZoom"
        Me.cboZoom.Size = New System.Drawing.Size(148, 21)
        Me.cboZoom.TabIndex = 11
        '
        'btnExcelAdvance
        '
        Me.btnExcelAdvance.Image = Global.GiaPha.My.Resources.Resources.Excel
        Me.btnExcelAdvance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExcelAdvance.Location = New System.Drawing.Point(470, 61)
        Me.btnExcelAdvance.Name = "btnExcelAdvance"
        Me.btnExcelAdvance.Size = New System.Drawing.Size(135, 39)
        Me.btnExcelAdvance.TabIndex = 22
        Me.btnExcelAdvance.Text = "Excel Nâng cao   "
        Me.btnExcelAdvance.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExcelAdvance.UseVisualStyleBackColor = True
        '
        'lblWarnning
        '
        Me.lblWarnning.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblWarnning.ForeColor = System.Drawing.Color.Red
        Me.lblWarnning.Location = New System.Drawing.Point(611, 74)
        Me.lblWarnning.Name = "lblWarnning"
        Me.lblWarnning.Size = New System.Drawing.Size(271, 28)
        Me.lblWarnning.TabIndex = 23
        Me.lblWarnning.Text = "*Tính năng Excel nâng cao thực thi lâu hơn."
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(614, 24)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(76, 25)
        Me.Button1.TabIndex = 24
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        Me.Button1.Visible = False
        '
        'btSetting
        '
        Me.btSetting.Image = Global.GiaPha.My.Resources.Resources.setting32
        Me.btSetting.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btSetting.Location = New System.Drawing.Point(291, 24)
        Me.btSetting.Name = "btSetting"
        Me.btSetting.Size = New System.Drawing.Size(80, 63)
        Me.btSetting.TabIndex = 25
        Me.btSetting.Text = "        Cài đặt"
        Me.btSetting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btSetting.UseVisualStyleBackColor = True
        '
        'PrintPreview
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(992, 678)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.lblWarnning)
        Me.Controls.Add(Me.btnExcelAdvance)
        Me.Controls.Add(Me.pagePreview)
        Me.Controls.Add(Me.btnPDF)
        Me.Controls.Add(Me.btnExcelNormal)
        Me.Controls.Add(Me.btnEnd)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cboPagesize)
        Me.Controls.Add(Me.GroupBox2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "PrintPreview"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "In phả hệ"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cboPagesize As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rbtLand As System.Windows.Forms.RadioButton
    Friend WithEvents rbtPortrait As System.Windows.Forms.RadioButton
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnEnd As System.Windows.Forms.Button
    Friend WithEvents btnExcelNormal As System.Windows.Forms.Button
    Friend WithEvents btnPDF As System.Windows.Forms.Button
    Private WithEvents pagePreview As PdfSharp.Forms.PagePreview
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cboZoom As System.Windows.Forms.ComboBox
    Friend WithEvents btnExcelAdvance As System.Windows.Forms.Button
    Friend WithEvents lblWarnning As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents btSetting As Button
End Class
