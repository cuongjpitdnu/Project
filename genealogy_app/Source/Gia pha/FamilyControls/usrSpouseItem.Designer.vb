<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class usrSpouseItem
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.picAvatar = New System.Windows.Forms.PictureBox
        Me.lnkName = New System.Windows.Forms.LinkLabel
        Me.lnkSubname = New System.Windows.Forms.LinkLabel
        CType(Me.picAvatar, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'picAvatar
        '
        Me.picAvatar.Image = Global.GiaPha.My.Resources.Resources.Gender_woman16
        Me.picAvatar.InitialImage = Nothing
        Me.picAvatar.Location = New System.Drawing.Point(0, 0)
        Me.picAvatar.Name = "picAvatar"
        Me.picAvatar.Size = New System.Drawing.Size(16, 16)
        Me.picAvatar.TabIndex = 0
        Me.picAvatar.TabStop = False
        '
        'lnkName
        '
        Me.lnkName.AutoSize = True
        Me.lnkName.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lnkName.Location = New System.Drawing.Point(22, 3)
        Me.lnkName.Name = "lnkName"
        Me.lnkName.Size = New System.Drawing.Size(50, 13)
        Me.lnkName.TabIndex = 1
        Me.lnkName.TabStop = True
        Me.lnkName.Text = "Thêm Vợ"
        '
        'lnkSubname
        '
        Me.lnkSubname.AutoSize = True
        Me.lnkSubname.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lnkSubname.Location = New System.Drawing.Point(78, 3)
        Me.lnkSubname.Name = "lnkSubname"
        Me.lnkSubname.Size = New System.Drawing.Size(75, 13)
        Me.lnkSubname.TabIndex = 1
        Me.lnkSubname.TabStop = True
        Me.lnkSubname.Text = "(từ danh sách)"
        Me.lnkSubname.Visible = False
        '
        'usrSpouseItem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me.lnkSubname)
        Me.Controls.Add(Me.lnkName)
        Me.Controls.Add(Me.picAvatar)
        Me.Name = "usrSpouseItem"
        Me.Size = New System.Drawing.Size(167, 20)
        CType(Me.picAvatar, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents picAvatar As System.Windows.Forms.PictureBox
    Friend WithEvents lnkName As System.Windows.Forms.LinkLabel
    Friend WithEvents lnkSubname As System.Windows.Forms.LinkLabel

End Class
