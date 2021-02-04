<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class usrSpouseList
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
        Me.lblTitle = New System.Windows.Forms.Label
        Me.lnkSpouseList = New System.Windows.Forms.LinkLabel
        Me.SuspendLayout()
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(4, 4)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(188, 13)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Thành viên là vợ của thành viên ABC:"
        '
        'lnkSpouseList
        '
        Me.lnkSpouseList.AutoSize = True
        Me.lnkSpouseList.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.lnkSpouseList.Location = New System.Drawing.Point(198, 4)
        Me.lnkSpouseList.Name = "lnkSpouseList"
        Me.lnkSpouseList.Size = New System.Drawing.Size(63, 13)
        Me.lnkSpouseList.TabIndex = 1
        Me.lnkSpouseList.TabStop = True
        Me.lnkSpouseList.Text = "(danh sách)"
        '
        'usrSpouseList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me.lnkSpouseList)
        Me.Controls.Add(Me.lblTitle)
        Me.Name = "usrSpouseList"
        Me.Size = New System.Drawing.Size(375, 146)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lnkSpouseList As System.Windows.Forms.LinkLabel

End Class
