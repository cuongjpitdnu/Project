<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmProgressBar
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmProgressBar))
        Me.lblPercent = New System.Windows.Forms.Label
        Me.lblProgress = New System.Windows.Forms.Label
        Me.prgbarsGiapha = New System.Windows.Forms.ProgressBar
        Me.btnCancel = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'lblPercent
        '
        Me.lblPercent.AutoSize = True
        Me.lblPercent.Location = New System.Drawing.Point(359, 49)
        Me.lblPercent.Name = "lblPercent"
        Me.lblPercent.Size = New System.Drawing.Size(0, 13)
        Me.lblPercent.TabIndex = 6
        '
        'lblProgress
        '
        Me.lblProgress.AutoSize = True
        Me.lblProgress.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProgress.Location = New System.Drawing.Point(45, 5)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(261, 16)
        Me.lblProgress.TabIndex = 5
        Me.lblProgress.Text = "Dữ liệu đang được xuất ra file excel..."
        '
        'prgbarsGiapha
        '
        Me.prgbarsGiapha.Location = New System.Drawing.Point(24, 40)
        Me.prgbarsGiapha.Name = "prgbarsGiapha"
        Me.prgbarsGiapha.Size = New System.Drawing.Size(329, 29)
        Me.prgbarsGiapha.TabIndex = 4
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.GiaPha.My.Resources.Resources.Cancel
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(139, 85)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(109, 45)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "      Dừng"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'frmProgressBar
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(391, 140)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.lblPercent)
        Me.Controls.Add(Me.lblProgress)
        Me.Controls.Add(Me.prgbarsGiapha)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmProgressBar"
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Phần mềm quản lý Gia Phả"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblPercent As System.Windows.Forms.Label
    Friend WithEvents lblProgress As System.Windows.Forms.Label
    Friend WithEvents prgbarsGiapha As System.Windows.Forms.ProgressBar
    Friend WithEvents btnCancel As System.Windows.Forms.Button
End Class
