<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class usrMemberDetail
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
        Me.components = New System.ComponentModel.Container()
        Me.lblDecease = New System.Windows.Forms.Label()
        Me.lblAlias = New System.Windows.Forms.Label()
        Me.lblBirth = New System.Windows.Forms.Label()
        Me.lblName = New System.Windows.Forms.Label()
        Me.picAvatar = New System.Windows.Forms.PictureBox()
        Me.lblRemark = New System.Windows.Forms.Label()
        Me.picHead = New System.Windows.Forms.PictureBox()
        Me.lblGeneration = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.picAvatar, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picHead, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblDecease
        '
        Me.lblDecease.BackColor = System.Drawing.Color.Transparent
        Me.lblDecease.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDecease.Location = New System.Drawing.Point(0, 74)
        Me.lblDecease.Name = "lblDecease"
        Me.lblDecease.Size = New System.Drawing.Size(177, 18)
        Me.lblDecease.TabIndex = 6
        Me.lblDecease.Text = "Mất: 20/10/2016 (Định Dậu)"
        Me.lblDecease.Visible = False
        '
        'lblAlias
        '
        Me.lblAlias.AutoSize = True
        Me.lblAlias.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAlias.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAlias.Location = New System.Drawing.Point(43, 39)
        Me.lblAlias.Name = "lblAlias"
        Me.lblAlias.Size = New System.Drawing.Size(16, 16)
        Me.lblAlias.TabIndex = 7
        Me.lblAlias.Text = "()"
        '
        'lblBirth
        '
        Me.lblBirth.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBirth.Location = New System.Drawing.Point(0, 58)
        Me.lblBirth.Name = "lblBirth"
        Me.lblBirth.Size = New System.Drawing.Size(180, 15)
        Me.lblBirth.TabIndex = 8
        Me.lblBirth.Text = "Sn: 1988"
        Me.lblBirth.Visible = False
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblName.Location = New System.Drawing.Point(43, 18)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(148, 18)
        Me.lblName.TabIndex = 5
        Me.lblName.Text = "Ông Lê Tiến Quyết"
        '
        'picAvatar
        '
        Me.picAvatar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picAvatar.Image = Global.GiaPha.My.Resources.Resources.no_avatar_m
        Me.picAvatar.InitialImage = Global.GiaPha.My.Resources.Resources.no_avatar_m
        Me.picAvatar.Location = New System.Drawing.Point(0, 0)
        Me.picAvatar.Name = "picAvatar"
        Me.picAvatar.Size = New System.Drawing.Size(36, 44)
        Me.picAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picAvatar.TabIndex = 4
        Me.picAvatar.TabStop = False
        '
        'lblRemark
        '
        Me.lblRemark.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblRemark.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRemark.Location = New System.Drawing.Point(0, 91)
        Me.lblRemark.Margin = New System.Windows.Forms.Padding(0)
        Me.lblRemark.Name = "lblRemark"
        Me.lblRemark.Size = New System.Drawing.Size(180, 24)
        Me.lblRemark.TabIndex = 6
        Me.lblRemark.Text = "Liệt sỹ chống Mỹ"
        Me.lblRemark.Visible = False
        '
        'picHead
        '
        Me.picHead.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picHead.BackColor = System.Drawing.Color.Transparent
        Me.picHead.Image = Global.GiaPha.My.Resources.Resources.medal_red
        Me.picHead.Location = New System.Drawing.Point(164, 0)
        Me.picHead.Name = "picHead"
        Me.picHead.Size = New System.Drawing.Size(16, 15)
        Me.picHead.TabIndex = 9
        Me.picHead.TabStop = False
        Me.ToolTip1.SetToolTip(Me.picHead, "Trưởng họ")
        Me.picHead.Visible = False
        '
        'lblGeneration
        '
        Me.lblGeneration.AutoSize = True
        Me.lblGeneration.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGeneration.Location = New System.Drawing.Point(43, 2)
        Me.lblGeneration.Name = "lblGeneration"
        Me.lblGeneration.Size = New System.Drawing.Size(56, 12)
        Me.lblGeneration.TabIndex = 5
        Me.lblGeneration.Text = "Đời thứ 20"
        '
        'usrMemberDetail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me.picHead)
        Me.Controls.Add(Me.lblRemark)
        Me.Controls.Add(Me.lblDecease)
        Me.Controls.Add(Me.lblAlias)
        Me.Controls.Add(Me.lblBirth)
        Me.Controls.Add(Me.lblGeneration)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.picAvatar)
        Me.Name = "usrMemberDetail"
        Me.Size = New System.Drawing.Size(180, 117)
        CType(Me.picAvatar, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picHead, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblDecease As System.Windows.Forms.Label
    Friend WithEvents lblAlias As System.Windows.Forms.Label
    Friend WithEvents lblBirth As System.Windows.Forms.Label
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents picAvatar As System.Windows.Forms.PictureBox
    Friend WithEvents lblRemark As System.Windows.Forms.Label
    Friend WithEvents picHead As System.Windows.Forms.PictureBox
    Friend WithEvents lblGeneration As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip

End Class
