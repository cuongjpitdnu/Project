<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmViewImage
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmViewImage))
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtTitle = New System.Windows.Forms.TextBox
        Me.btnOpenFile = New System.Windows.Forms.Button
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.pnlEditImage = New System.Windows.Forms.Panel
        Me.btnClose = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnSetAvatar = New System.Windows.Forms.Button
        Me.pnlFunction = New System.Windows.Forms.Panel
        Me.btnCusBack = New GiaPha.CustomButton
        Me.btnCusNext = New GiaPha.CustomButton
        Me.btnSaveToFolder = New System.Windows.Forms.Button
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlEditImage.SuspendLayout()
        Me.pnlFunction.SuspendLayout()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PictureBox1.Location = New System.Drawing.Point(7, 5)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(799, 550)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(81, 16)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(42, 15)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Mô tả: "
        '
        'txtTitle
        '
        Me.txtTitle.Location = New System.Drawing.Point(125, 14)
        Me.txtTitle.Margin = New System.Windows.Forms.Padding(4)
        Me.txtTitle.Multiline = True
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(473, 20)
        Me.txtTitle.TabIndex = 4
        '
        'btnOpenFile
        '
        Me.btnOpenFile.Location = New System.Drawing.Point(6, 6)
        Me.btnOpenFile.Margin = New System.Windows.Forms.Padding(4)
        Me.btnOpenFile.Name = "btnOpenFile"
        Me.btnOpenFile.Size = New System.Drawing.Size(70, 37)
        Me.btnOpenFile.TabIndex = 3
        Me.btnOpenFile.Text = "Chọn ảnh"
        Me.btnOpenFile.UseVisualStyleBackColor = True
        '
        'pnlEditImage
        '
        Me.pnlEditImage.Controls.Add(Me.Label1)
        Me.pnlEditImage.Controls.Add(Me.btnOpenFile)
        Me.pnlEditImage.Controls.Add(Me.btnClose)
        Me.pnlEditImage.Controls.Add(Me.btnSave)
        Me.pnlEditImage.Controls.Add(Me.txtTitle)
        Me.pnlEditImage.Location = New System.Drawing.Point(7, 616)
        Me.pnlEditImage.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlEditImage.Name = "pnlEditImage"
        Me.pnlEditImage.Size = New System.Drawing.Size(784, 50)
        Me.pnlEditImage.TabIndex = 7
        '
        'btnClose
        '
        Me.btnClose.Image = Global.GiaPha.My.Resources.Resources.exit32
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(695, 6)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(83, 37)
        Me.btnClose.TabIndex = 6
        Me.btnClose.Text = "Đóng"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Image = CType(resources.GetObject("btnSave.Image"), System.Drawing.Image)
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(604, 6)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(83, 37)
        Me.btnSave.TabIndex = 5
        Me.btnSave.Text = "Lưu ảnh"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnSetAvatar
        '
        Me.btnSetAvatar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSetAvatar.Location = New System.Drawing.Point(133, 571)
        Me.btnSetAvatar.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSetAvatar.Name = "btnSetAvatar"
        Me.btnSetAvatar.Size = New System.Drawing.Size(131, 37)
        Me.btnSetAvatar.TabIndex = 2
        Me.btnSetAvatar.Text = "Đặt ảnh thành viên"
        Me.btnSetAvatar.UseVisualStyleBackColor = True
        '
        'pnlFunction
        '
        Me.pnlFunction.Controls.Add(Me.btnCusBack)
        Me.pnlFunction.Controls.Add(Me.btnCusNext)
        Me.pnlFunction.Location = New System.Drawing.Point(284, 564)
        Me.pnlFunction.Name = "pnlFunction"
        Me.pnlFunction.Size = New System.Drawing.Size(226, 50)
        Me.pnlFunction.TabIndex = 9
        Me.pnlFunction.TabStop = True
        '
        'btnCusBack
        '
        Me.btnCusBack.Image = CType(resources.GetObject("btnCusBack.Image"), System.Drawing.Image)
        Me.btnCusBack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCusBack.Location = New System.Drawing.Point(8, 7)
        Me.btnCusBack.Name = "btnCusBack"
        Me.btnCusBack.Size = New System.Drawing.Size(96, 37)
        Me.btnCusBack.TabIndex = 0
        Me.btnCusBack.Text = " Ảnh trước"
        Me.btnCusBack.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCusBack.UseVisualStyleBackColor = True
        '
        'btnCusNext
        '
        Me.btnCusNext.Image = CType(resources.GetObject("btnCusNext.Image"), System.Drawing.Image)
        Me.btnCusNext.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCusNext.Location = New System.Drawing.Point(129, 7)
        Me.btnCusNext.Name = "btnCusNext"
        Me.btnCusNext.Size = New System.Drawing.Size(87, 37)
        Me.btnCusNext.TabIndex = 1
        Me.btnCusNext.Text = "Ảnh sau"
        Me.btnCusNext.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCusNext.UseVisualStyleBackColor = True
        '
        'btnSaveToFolder
        '
        Me.btnSaveToFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveToFolder.Location = New System.Drawing.Point(526, 571)
        Me.btnSaveToFolder.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSaveToFolder.Name = "btnSaveToFolder"
        Me.btnSaveToFolder.Size = New System.Drawing.Size(135, 37)
        Me.btnSaveToFolder.TabIndex = 2
        Me.btnSaveToFolder.Text = "Lưu ảnh vào thư mục"
        Me.btnSaveToFolder.UseVisualStyleBackColor = True
        '
        'frmViewImage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(814, 675)
        Me.Controls.Add(Me.pnlFunction)
        Me.Controls.Add(Me.pnlEditImage)
        Me.Controls.Add(Me.btnSaveToFolder)
        Me.Controls.Add(Me.btnSetAvatar)
        Me.Controls.Add(Me.PictureBox1)
        Me.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmViewImage"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Xem ảnh"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlEditImage.ResumeLayout(False)
        Me.pnlEditImage.PerformLayout()
        Me.pnlFunction.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtTitle As System.Windows.Forms.TextBox
    Friend WithEvents btnOpenFile As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents pnlEditImage As System.Windows.Forms.Panel
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents pnlFunction As System.Windows.Forms.Panel
    Friend WithEvents btnSetAvatar As System.Windows.Forms.Button
    Friend WithEvents btnCusBack As GiaPha.CustomButton
    Friend WithEvents btnCusNext As GiaPha.CustomButton
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnSaveToFolder As System.Windows.Forms.Button
End Class
