<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFamilyInital
    Inherits FamilyForm

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFamilyInital))
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtFamilyName = New System.Windows.Forms.TextBox
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtHomeTown = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtAnni = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtGeneration = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtCreateMember = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Font = New System.Drawing.Font("Times New Roman", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(412, 51)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "THÔNG TIN DÒNG HỌ"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 61)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(83, 16)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Tên dòng họ:"
        '
        'txtFamilyName
        '
        Me.txtFamilyName.Location = New System.Drawing.Point(12, 85)
        Me.txtFamilyName.MaxLength = 70
        Me.txtFamilyName.Name = "txtFamilyName"
        Me.txtFamilyName.Size = New System.Drawing.Size(209, 22)
        Me.txtFamilyName.TabIndex = 10
        '
        'btnOK
        '
        Me.btnOK.Image = Global.GiaPha.My.Resources.Resources.task_done
        Me.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnOK.Location = New System.Drawing.Point(81, 330)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(103, 38)
        Me.btnOK.TabIndex = 50
        Me.btnOK.Text = "        Tiếp tục"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.GiaPha.My.Resources.Resources.Cancel
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(229, 330)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(103, 38)
        Me.btnCancel.TabIndex = 60
        Me.btnCancel.Text = "         Đóng"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 169)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(88, 16)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Nguyên quán:"
        '
        'txtHomeTown
        '
        Me.txtHomeTown.Location = New System.Drawing.Point(12, 193)
        Me.txtHomeTown.MaxLength = 200
        Me.txtHomeTown.Multiline = True
        Me.txtHomeTown.Name = "txtHomeTown"
        Me.txtHomeTown.Size = New System.Drawing.Size(384, 58)
        Me.txtHomeTown.TabIndex = 40
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 115)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(78, 16)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Ngày giỗ tổ:"
        '
        'txtAnni
        '
        Me.txtAnni.Location = New System.Drawing.Point(12, 139)
        Me.txtAnni.MaxLength = 70
        Me.txtAnni.Name = "txtAnni"
        Me.txtAnni.Size = New System.Drawing.Size(384, 22)
        Me.txtAnni.TabIndex = 30
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(246, 88)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(61, 16)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "Đời thứ :"
        '
        'txtGeneration
        '
        Me.txtGeneration.Location = New System.Drawing.Point(313, 85)
        Me.txtGeneration.MaxLength = 70
        Me.txtGeneration.Name = "txtGeneration"
        Me.txtGeneration.Size = New System.Drawing.Size(83, 22)
        Me.txtGeneration.TabIndex = 20
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(12, 264)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(116, 16)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "Người lập gia phả:"
        '
        'txtCreateMember
        '
        Me.txtCreateMember.Location = New System.Drawing.Point(12, 288)
        Me.txtCreateMember.MaxLength = 200
        Me.txtCreateMember.Name = "txtCreateMember"
        Me.txtCreateMember.Size = New System.Drawing.Size(384, 22)
        Me.txtCreateMember.TabIndex = 45
        '
        'frmFamilyInital
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.ClientSize = New System.Drawing.Size(412, 385)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.txtCreateMember)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtAnni)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtHomeTown)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtGeneration)
        Me.Controls.Add(Me.txtFamilyName)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.DoubleBuffered = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmFamilyInital"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Thông tin dòng họ"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtFamilyName As System.Windows.Forms.TextBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtHomeTown As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtAnni As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtGeneration As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtCreateMember As System.Windows.Forms.TextBox

End Class
