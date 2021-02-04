<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmConfirmDrawTree
    Inherits System.Windows.Forms.Form

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmConfirmDrawTree))
        Me.chkBoxNomal = New System.Windows.Forms.CheckBox
        Me.chkBoxAdvance = New System.Windows.Forms.CheckBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnClose = New System.Windows.Forms.Button
        Me.lblTitle = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'chkBoxNomal
        '
        Me.chkBoxNomal.AutoSize = True
        Me.chkBoxNomal.Checked = True
        Me.chkBoxNomal.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkBoxNomal.Location = New System.Drawing.Point(38, 36)
        Me.chkBoxNomal.Name = "chkBoxNomal"
        Me.chkBoxNomal.Size = New System.Drawing.Size(15, 14)
        Me.chkBoxNomal.TabIndex = 0
        Me.chkBoxNomal.UseVisualStyleBackColor = True
        '
        'chkBoxAdvance
        '
        Me.chkBoxAdvance.AutoSize = True
        Me.chkBoxAdvance.Location = New System.Drawing.Point(38, 64)
        Me.chkBoxAdvance.Name = "chkBoxAdvance"
        Me.chkBoxAdvance.Size = New System.Drawing.Size(15, 14)
        Me.chkBoxAdvance.TabIndex = 1
        Me.chkBoxAdvance.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(65, 36)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(83, 15)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Thông thường"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(65, 64)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(128, 15)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Nâng cao (Chậm hơn)"
        '
        'btnOK
        '
        Me.btnOK.Image = Global.GiaPha.My.Resources.Resources.task_done
        Me.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnOK.Location = New System.Drawing.Point(68, 98)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(116, 34)
        Me.btnOK.TabIndex = 4
        Me.btnOK.Text = "      Xác nhận"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Image = Global.GiaPha.My.Resources.Resources.back_32
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(190, 98)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(107, 34)
        Me.btnClose.TabIndex = 5
        Me.btnClose.Text = "   Đóng"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(35, 9)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(249, 15)
        Me.lblTitle.TabIndex = 6
        Me.lblTitle.Text = "Hãy lựa chọn chế độ đưa ra file Excel."
        '
        'frmConfirmDrawTree
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(386, 138)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.chkBoxAdvance)
        Me.Controls.Add(Me.chkBoxNomal)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmConfirmDrawTree"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Phần mềm quản lý gia phả"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents chkBoxNomal As System.Windows.Forms.CheckBox
    Friend WithEvents chkBoxAdvance As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents lblTitle As System.Windows.Forms.Label
End Class
