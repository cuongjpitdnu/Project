<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmImage
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmImage))
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtDesc = New System.Windows.Forms.TextBox
        Me.btnChooseImg = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.grpTools = New System.Windows.Forms.GroupBox
        Me.grpInfo = New System.Windows.Forms.GroupBox
        Me.txtTitle = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.pbImage = New System.Windows.Forms.PictureBox
        Me.grpTools.SuspendLayout()
        Me.grpInfo.SuspendLayout()
        CType(Me.pbImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(277, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 18)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Tiêu đề"
        '
        'txtDesc
        '
        Me.txtDesc.Location = New System.Drawing.Point(364, 42)
        Me.txtDesc.MaxLength = 200
        Me.txtDesc.Multiline = True
        Me.txtDesc.Name = "txtDesc"
        Me.txtDesc.Size = New System.Drawing.Size(483, 79)
        Me.txtDesc.TabIndex = 1
        '
        'btnChooseImg
        '
        Me.btnChooseImg.Location = New System.Drawing.Point(1, 12)
        Me.btnChooseImg.Name = "btnChooseImg"
        Me.btnChooseImg.Size = New System.Drawing.Size(120, 45)
        Me.btnChooseImg.TabIndex = 0
        Me.btnChooseImg.Text = "Đổi hình ảnh"
        Me.btnChooseImg.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(127, 12)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(110, 45)
        Me.btnSave.TabIndex = 0
        Me.btnSave.Text = "Lưu thay đổi"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'grpTools
        '
        Me.grpTools.BackColor = System.Drawing.SystemColors.ActiveBorder
        Me.grpTools.Controls.Add(Me.grpInfo)
        Me.grpTools.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.grpTools.Location = New System.Drawing.Point(0, 545)
        Me.grpTools.Name = "grpTools"
        Me.grpTools.Size = New System.Drawing.Size(998, 135)
        Me.grpTools.TabIndex = 3
        Me.grpTools.TabStop = False
        '
        'grpInfo
        '
        Me.grpInfo.Controls.Add(Me.btnChooseImg)
        Me.grpInfo.Controls.Add(Me.txtTitle)
        Me.grpInfo.Controls.Add(Me.txtDesc)
        Me.grpInfo.Controls.Add(Me.Label2)
        Me.grpInfo.Controls.Add(Me.Label1)
        Me.grpInfo.Controls.Add(Me.btnSave)
        Me.grpInfo.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpInfo.Location = New System.Drawing.Point(71, 2)
        Me.grpInfo.Name = "grpInfo"
        Me.grpInfo.Size = New System.Drawing.Size(862, 127)
        Me.grpInfo.TabIndex = 3
        Me.grpInfo.TabStop = False
        '
        'txtTitle
        '
        Me.txtTitle.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTitle.Location = New System.Drawing.Point(364, 10)
        Me.txtTitle.MaxLength = 200
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(483, 26)
        Me.txtTitle.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(290, 44)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(47, 18)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Mô tả"
        '
        'pbImage
        '
        Me.pbImage.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.pbImage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbImage.Location = New System.Drawing.Point(0, 0)
        Me.pbImage.Name = "pbImage"
        Me.pbImage.Size = New System.Drawing.Size(998, 545)
        Me.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbImage.TabIndex = 4
        Me.pbImage.TabStop = False
        '
        'frmImage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(998, 680)
        Me.Controls.Add(Me.pbImage)
        Me.Controls.Add(Me.grpTools)
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmImage"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Thêm mới hình ảnh"
        Me.grpTools.ResumeLayout(False)
        Me.grpInfo.ResumeLayout(False)
        Me.grpInfo.PerformLayout()
        CType(Me.pbImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtDesc As System.Windows.Forms.TextBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnChooseImg As System.Windows.Forms.Button
    Friend WithEvents grpTools As System.Windows.Forms.GroupBox
    Friend WithEvents grpInfo As System.Windows.Forms.GroupBox
    Friend WithEvents txtTitle As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents pbImage As System.Windows.Forms.PictureBox
End Class
