<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FamilyCard
    Inherits FamilyBaseControl

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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FamilyCard))
        Me.picUserCard = New System.Windows.Forms.PictureBox()
        Me.lnkName = New System.Windows.Forms.LinkLabel()
        Me.lblStatusCap = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.lnkChildMan = New System.Windows.Forms.LinkLabel()
        Me.lblChildsList = New System.Windows.Forms.Label()
        Me.lblTextBirth = New System.Windows.Forms.Label()
        Me.lblTextBirthPlace = New System.Windows.Forms.Label()
        Me.lblTextHometown = New System.Windows.Forms.Label()
        Me.lblBirth = New System.Windows.Forms.Label()
        Me.lblLunarBirth = New System.Windows.Forms.Label()
        Me.lblBirthPlace = New System.Windows.Forms.Label()
        Me.lblBirthDie = New System.Windows.Forms.Label()
        Me.lblHometown = New System.Windows.Forms.Label()
        Me.lblRelation = New System.Windows.Forms.Label()
        Me.lblAlias = New System.Windows.Forms.Label()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnDelSpouseRel = New System.Windows.Forms.Button()
        CType(Me.picUserCard, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'picUserCard
        '
        Me.picUserCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picUserCard.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picUserCard.Location = New System.Drawing.Point(8, 7)
        Me.picUserCard.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.picUserCard.Name = "picUserCard"
        Me.picUserCard.Size = New System.Drawing.Size(106, 146)
        Me.picUserCard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picUserCard.TabIndex = 0
        Me.picUserCard.TabStop = False
        Me.ToolTip.SetToolTip(Me.picUserCard, "Thay đổi hình đại diện")
        '
        'lnkName
        '
        Me.lnkName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lnkName.BackColor = System.Drawing.Color.Transparent
        Me.lnkName.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        Me.lnkName.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.lnkName.Location = New System.Drawing.Point(127, 7)
        Me.lnkName.Name = "lnkName"
        Me.lnkName.Size = New System.Drawing.Size(241, 19)
        Me.lnkName.TabIndex = 3
        Me.lnkName.TabStop = True
        Me.lnkName.Text = "Tên thành viên"
        '
        'lblStatusCap
        '
        Me.lblStatusCap.AutoSize = True
        Me.lblStatusCap.Location = New System.Drawing.Point(6, 62)
        Me.lblStatusCap.Name = "lblStatusCap"
        Me.lblStatusCap.Size = New System.Drawing.Size(130, 16)
        Me.lblStatusCap.TabIndex = 5
        Me.lblStatusCap.Text = "Tình trạng hôn nhân :"
        Me.lblStatusCap.Visible = False
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(142, 62)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(60, 16)
        Me.lblStatus.TabIndex = 6
        Me.lblStatus.Text = "Độc thân"
        Me.lblStatus.Visible = False
        '
        'lnkChildMan
        '
        Me.lnkChildMan.AutoSize = True
        Me.lnkChildMan.Location = New System.Drawing.Point(142, 35)
        Me.lnkChildMan.Name = "lnkChildMan"
        Me.lnkChildMan.Size = New System.Drawing.Size(120, 16)
        Me.lnkChildMan.TabIndex = 7
        Me.lnkChildMan.TabStop = True
        Me.lnkChildMan.Text = "Danh sách các con"
        Me.lnkChildMan.Visible = False
        '
        'lblChildsList
        '
        Me.lblChildsList.AutoSize = True
        Me.lblChildsList.Location = New System.Drawing.Point(6, 35)
        Me.lblChildsList.Name = "lblChildsList"
        Me.lblChildsList.Size = New System.Drawing.Size(106, 16)
        Me.lblChildsList.TabIndex = 8
        Me.lblChildsList.Text = "Có 99 người con"
        Me.lblChildsList.Visible = False
        '
        'lblTextBirth
        '
        Me.lblTextBirth.AutoSize = True
        Me.lblTextBirth.BackColor = System.Drawing.Color.Transparent
        Me.lblTextBirth.ForeColor = System.Drawing.Color.Navy
        Me.lblTextBirth.Location = New System.Drawing.Point(125, 77)
        Me.lblTextBirth.Name = "lblTextBirth"
        Me.lblTextBirth.Size = New System.Drawing.Size(74, 16)
        Me.lblTextBirth.TabIndex = 9
        Me.lblTextBirth.Text = "Ngày sinh :"
        '
        'lblTextBirthPlace
        '
        Me.lblTextBirthPlace.AutoSize = True
        Me.lblTextBirthPlace.BackColor = System.Drawing.Color.Transparent
        Me.lblTextBirthPlace.ForeColor = System.Drawing.Color.Navy
        Me.lblTextBirthPlace.Location = New System.Drawing.Point(134, 123)
        Me.lblTextBirthPlace.Name = "lblTextBirthPlace"
        Me.lblTextBirthPlace.Size = New System.Drawing.Size(65, 16)
        Me.lblTextBirthPlace.TabIndex = 9
        Me.lblTextBirthPlace.Text = "Nơi sinh :"
        '
        'lblTextHometown
        '
        Me.lblTextHometown.AutoSize = True
        Me.lblTextHometown.BackColor = System.Drawing.Color.Transparent
        Me.lblTextHometown.ForeColor = System.Drawing.Color.Navy
        Me.lblTextHometown.Location = New System.Drawing.Point(125, 158)
        Me.lblTextHometown.Name = "lblTextHometown"
        Me.lblTextHometown.Size = New System.Drawing.Size(72, 16)
        Me.lblTextHometown.TabIndex = 9
        Me.lblTextHometown.Text = "Quê quán :"
        '
        'lblBirth
        '
        Me.lblBirth.AutoSize = True
        Me.lblBirth.BackColor = System.Drawing.Color.Transparent
        Me.lblBirth.ForeColor = System.Drawing.Color.Navy
        Me.lblBirth.Location = New System.Drawing.Point(205, 77)
        Me.lblBirth.Name = "lblBirth"
        Me.lblBirth.Size = New System.Drawing.Size(0, 16)
        Me.lblBirth.TabIndex = 9
        '
        'lblLunarBirth
        '
        Me.lblLunarBirth.AutoSize = True
        Me.lblLunarBirth.BackColor = System.Drawing.Color.Transparent
        Me.lblLunarBirth.ForeColor = System.Drawing.Color.Navy
        Me.lblLunarBirth.Location = New System.Drawing.Point(205, 101)
        Me.lblLunarBirth.Name = "lblLunarBirth"
        Me.lblLunarBirth.Size = New System.Drawing.Size(0, 16)
        Me.lblLunarBirth.TabIndex = 9
        '
        'lblBirthPlace
        '
        Me.lblBirthPlace.BackColor = System.Drawing.Color.Transparent
        Me.lblBirthPlace.ForeColor = System.Drawing.Color.Navy
        Me.lblBirthPlace.Location = New System.Drawing.Point(197, 123)
        Me.lblBirthPlace.Name = "lblBirthPlace"
        Me.lblBirthPlace.Size = New System.Drawing.Size(163, 32)
        Me.lblBirthPlace.TabIndex = 9
        Me.lblBirthPlace.Text = "xa quang chau - tinh hung yen"
        '
        'lblBirthDie
        '
        Me.lblBirthDie.AutoSize = True
        Me.lblBirthDie.BackColor = System.Drawing.Color.Transparent
        Me.lblBirthDie.ForeColor = System.Drawing.Color.Navy
        Me.lblBirthDie.Location = New System.Drawing.Point(11, 159)
        Me.lblBirthDie.Name = "lblBirthDie"
        Me.lblBirthDie.Size = New System.Drawing.Size(0, 16)
        Me.lblBirthDie.TabIndex = 9
        '
        'lblHometown
        '
        Me.lblHometown.BackColor = System.Drawing.Color.Transparent
        Me.lblHometown.ForeColor = System.Drawing.Color.Navy
        Me.lblHometown.Location = New System.Drawing.Point(197, 158)
        Me.lblHometown.Name = "lblHometown"
        Me.lblHometown.Size = New System.Drawing.Size(163, 32)
        Me.lblHometown.TabIndex = 9
        Me.lblHometown.Text = "ha noi"
        '
        'lblRelation
        '
        Me.lblRelation.AutoSize = True
        Me.lblRelation.BackColor = System.Drawing.Color.Transparent
        Me.lblRelation.ForeColor = System.Drawing.Color.Red
        Me.lblRelation.Location = New System.Drawing.Point(49, 48)
        Me.lblRelation.Name = "lblRelation"
        Me.lblRelation.Size = New System.Drawing.Size(0, 16)
        Me.lblRelation.TabIndex = 10
        '
        'lblAlias
        '
        Me.lblAlias.AutoSize = True
        Me.lblAlias.BackColor = System.Drawing.Color.Transparent
        Me.lblAlias.ForeColor = System.Drawing.Color.Navy
        Me.lblAlias.Location = New System.Drawing.Point(128, 35)
        Me.lblAlias.Name = "lblAlias"
        Me.lblAlias.Size = New System.Drawing.Size(0, 16)
        Me.lblAlias.TabIndex = 11
        '
        'btnDelSpouseRel
        '
        Me.btnDelSpouseRel.Image = CType(resources.GetObject("btnDelSpouseRel.Image"), System.Drawing.Image)
        Me.btnDelSpouseRel.Location = New System.Drawing.Point(342, 3)
        Me.btnDelSpouseRel.Name = "btnDelSpouseRel"
        Me.btnDelSpouseRel.Size = New System.Drawing.Size(26, 23)
        Me.btnDelSpouseRel.TabIndex = 12
        Me.ToolTip.SetToolTip(Me.btnDelSpouseRel, "Hủy quan hệ hôn nhân")
        Me.btnDelSpouseRel.UseVisualStyleBackColor = True
        Me.btnDelSpouseRel.Visible = False
        '
        'FamilyCard
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.lblHometown)
        Me.Controls.Add(Me.btnDelSpouseRel)
        Me.Controls.Add(Me.lblAlias)
        Me.Controls.Add(Me.lblRelation)
        Me.Controls.Add(Me.lblBirth)
        Me.Controls.Add(Me.lblTextBirth)
        Me.Controls.Add(Me.lnkName)
        Me.Controls.Add(Me.lblTextBirthPlace)
        Me.Controls.Add(Me.lblLunarBirth)
        Me.Controls.Add(Me.lblTextHometown)
        Me.Controls.Add(Me.lblChildsList)
        Me.Controls.Add(Me.lblBirthDie)
        Me.Controls.Add(Me.lblBirthPlace)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.lblStatusCap)
        Me.Controls.Add(Me.lnkChildMan)
        Me.Controls.Add(Me.picUserCard)
        Me.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Name = "FamilyCard"
        Me.Size = New System.Drawing.Size(372, 193)
        CType(Me.picUserCard, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents picUserCard As System.Windows.Forms.PictureBox
    Friend WithEvents lnkName As System.Windows.Forms.LinkLabel
    Friend WithEvents lblStatusCap As System.Windows.Forms.Label
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents lnkChildMan As System.Windows.Forms.LinkLabel
    Friend WithEvents lblChildsList As System.Windows.Forms.Label
    Friend WithEvents lblTextBirth As System.Windows.Forms.Label
    Friend WithEvents lblTextBirthPlace As System.Windows.Forms.Label
    Friend WithEvents lblTextHometown As System.Windows.Forms.Label
    Friend WithEvents lblBirth As System.Windows.Forms.Label
    Friend WithEvents lblLunarBirth As System.Windows.Forms.Label
    Friend WithEvents lblBirthPlace As System.Windows.Forms.Label
    Friend WithEvents lblBirthDie As System.Windows.Forms.Label
    Friend WithEvents lblHometown As System.Windows.Forms.Label
    Friend WithEvents lblRelation As System.Windows.Forms.Label
    Friend WithEvents lblAlias As System.Windows.Forms.Label
    Friend WithEvents ToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents btnDelSpouseRel As System.Windows.Forms.Button

End Class
