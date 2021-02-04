<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class usrPic
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.PicContent = New System.Windows.Forms.PictureBox
        Me.chkSelect = New System.Windows.Forms.CheckBox
        CType(Me.PicContent, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PicContent
        '
        Me.PicContent.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PicContent.Location = New System.Drawing.Point(3, 7)
        Me.PicContent.Name = "PicContent"
        Me.PicContent.Size = New System.Drawing.Size(150, 163)
        Me.PicContent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PicContent.TabIndex = 0
        Me.PicContent.TabStop = False
        '
        'chkSelect
        '
        Me.chkSelect.AutoSize = True
        Me.chkSelect.Location = New System.Drawing.Point(3, 156)
        Me.chkSelect.Name = "chkSelect"
        Me.chkSelect.Size = New System.Drawing.Size(15, 14)
        Me.chkSelect.TabIndex = 1
        Me.chkSelect.UseVisualStyleBackColor = True
        '
        'Pic
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.chkSelect)
        Me.Controls.Add(Me.PicContent)
        Me.Name = "Pic"
        Me.Size = New System.Drawing.Size(171, 194)
        CType(Me.PicContent, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PicContent As System.Windows.Forms.PictureBox
    Friend WithEvents chkSelect As System.Windows.Forms.CheckBox

End Class
