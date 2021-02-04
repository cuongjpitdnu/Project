<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class usrMemberCard3
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
        Me.lblName = New System.Windows.Forms.Label
        Me.lblBirth = New System.Windows.Forms.Label
        Me.lblDeath = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'lblName
        '
        Me.lblName.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblName.BackColor = System.Drawing.Color.Transparent
        Me.lblName.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.Location = New System.Drawing.Point(1, 3)
        Me.lblName.Margin = New System.Windows.Forms.Padding(0)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(64, 160)
        Me.lblName.TabIndex = 1
        Me.lblName.Text = "NGUYỄN" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "MMMMMM" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "MMMMMM" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "MMMMMM" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "MMMMMM" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblBirth
        '
        Me.lblBirth.BackColor = System.Drawing.Color.Transparent
        Me.lblBirth.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBirth.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblBirth.Location = New System.Drawing.Point(35, 89)
        Me.lblBirth.Name = "lblBirth"
        Me.lblBirth.Size = New System.Drawing.Size(28, 80)
        Me.lblBirth.TabIndex = 1
        Me.lblBirth.Text = "10/01/1900"
        Me.lblBirth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblDeath
        '
        Me.lblDeath.BackColor = System.Drawing.Color.Transparent
        Me.lblDeath.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblDeath.Location = New System.Drawing.Point(1, 89)
        Me.lblDeath.Name = "lblDeath"
        Me.lblDeath.Size = New System.Drawing.Size(28, 80)
        Me.lblDeath.TabIndex = 2
        Me.lblDeath.Text = "10/01/1900"
        Me.lblDeath.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'usrMemberCard3
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Transparent
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.lblDeath)
        Me.Controls.Add(Me.lblBirth)
        Me.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "usrMemberCard3"
        Me.Size = New System.Drawing.Size(65, 169)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents lblBirth As System.Windows.Forms.Label
    Friend WithEvents lblDeath As System.Windows.Forms.Label

End Class
