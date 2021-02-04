<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class usrMemberCard1
    Inherits usrMemCardBase

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
        Me.picMember = New System.Windows.Forms.PictureBox()
        Me.lblName = New System.Windows.Forms.Label()
        CType(Me.picMember, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'picMember
        '
        Me.picMember.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picMember.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picMember.InitialImage = Nothing
        Me.picMember.Location = New System.Drawing.Point(29, 9)
        Me.picMember.Name = "picMember"
        Me.picMember.Size = New System.Drawing.Size(48, 22)
        Me.picMember.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picMember.TabIndex = 0
        Me.picMember.TabStop = False
        '
        'lblName
        '
        Me.lblName.BackColor = System.Drawing.Color.Transparent
        Me.lblName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.Location = New System.Drawing.Point(6, 91)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(110, 69)
        Me.lblName.TabIndex = 1
        Me.lblName.Text = "Ten thanh vien (ten thuong goi)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "1990" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Mất: 10/01/1900" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Nam"
        Me.lblName.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'usrMemberCard1
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.GiaPha.My.Resources.Resources.pic_frame
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.picMember)
        Me.Cursor = System.Windows.Forms.Cursors.Hand
        Me.DoubleBuffered = True
        Me.Name = "usrMemberCard1"
        Me.Size = New System.Drawing.Size(122, 160)
        CType(Me.picMember, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents picMember As System.Windows.Forms.PictureBox
    Friend WithEvents lblName As System.Windows.Forms.Label

End Class
