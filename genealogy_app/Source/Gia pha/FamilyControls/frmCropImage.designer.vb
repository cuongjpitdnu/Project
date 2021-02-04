<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCropImage
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCropImage))
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.pnCropFrame = New System.Windows.Forms.Panel
        Me.pbxPatient = New System.Windows.Forms.PictureBox
        Me.btnSelectImage = New System.Windows.Forms.Button
        Me.dlgOpenImage = New System.Windows.Forms.OpenFileDialog
        Me.pnCropFrame.SuspendLayout()
        CType(Me.pbxPatient, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnCancel.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Image = Global.GiaPha.My.Resources.Resources.back_24
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(473, 512)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(83, 44)
        Me.btnCancel.TabIndex = 20
        Me.btnCancel.Text = "Thoát"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSave.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnSave.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Image = CType(resources.GetObject("btnSave.Image"), System.Drawing.Image)
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(359, 512)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(93, 44)
        Me.btnSave.TabIndex = 10
        Me.btnSave.Text = "Tạo ảnh"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'pnCropFrame
        '
        Me.pnCropFrame.Controls.Add(Me.pbxPatient)
        Me.pnCropFrame.Location = New System.Drawing.Point(4, 6)
        Me.pnCropFrame.Name = "pnCropFrame"
        Me.pnCropFrame.Size = New System.Drawing.Size(787, 500)
        Me.pnCropFrame.TabIndex = 28
        '
        'pbxPatient
        '
        Me.pbxPatient.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pbxPatient.Location = New System.Drawing.Point(0, 0)
        Me.pbxPatient.Name = "pbxPatient"
        Me.pbxPatient.Size = New System.Drawing.Size(784, 500)
        Me.pbxPatient.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbxPatient.TabIndex = 0
        Me.pbxPatient.TabStop = False
        '
        'btnSelectImage
        '
        Me.btnSelectImage.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSelectImage.Image = CType(resources.GetObject("btnSelectImage.Image"), System.Drawing.Image)
        Me.btnSelectImage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSelectImage.Location = New System.Drawing.Point(237, 512)
        Me.btnSelectImage.Name = "btnSelectImage"
        Me.btnSelectImage.Size = New System.Drawing.Size(103, 44)
        Me.btnSelectImage.TabIndex = 29
        Me.btnSelectImage.Text = "Chọn ảnh"
        Me.btnSelectImage.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSelectImage.UseVisualStyleBackColor = True
        '
        'dlgOpenImage
        '
        Me.dlgOpenImage.CheckFileExists = False
        Me.dlgOpenImage.Filter = "JPG (*.jpg)|*.jpg|GIF (*.gif)|*gif|BMP (*.bmp)|*.bmp|PNG (*.png)|*.png|All (*.*)|" & _
            "*.*"
        '
        'frmCropImage
        '
        Me.AcceptButton = Me.btnCancel
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.CancelButton = Me.btnSave
        Me.ClientSize = New System.Drawing.Size(792, 568)
        Me.Controls.Add(Me.btnSelectImage)
        Me.Controls.Add(Me.pnCropFrame)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCropImage"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Thay đổi hình đại diện"
        Me.pnCropFrame.ResumeLayout(False)
        CType(Me.pbxPatient, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Private WithEvents pnCropFrame As System.Windows.Forms.Panel
    Private WithEvents pbxPatient As System.Windows.Forms.PictureBox
    Friend WithEvents btnSelectImage As System.Windows.Forms.Button
    Friend WithEvents dlgOpenImage As System.Windows.Forms.OpenFileDialog
End Class
