<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRestore
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRestore))
        Me.txtFileName = New System.Windows.Forms.TextBox
        Me.btnRestore = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.bntBrowse = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'txtFileName
        '
        Me.txtFileName.Location = New System.Drawing.Point(12, 45)
        Me.txtFileName.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtFileName.Name = "txtFileName"
        Me.txtFileName.Size = New System.Drawing.Size(413, 22)
        Me.txtFileName.TabIndex = 1
        '
        'btnRestore
        '
        Me.btnRestore.Image = Global.GiaPha.My.Resources.Resources.DBBackup32
        Me.btnRestore.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRestore.Location = New System.Drawing.Point(133, 143)
        Me.btnRestore.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnRestore.Name = "btnRestore"
        Me.btnRestore.Size = New System.Drawing.Size(112, 44)
        Me.btnRestore.TabIndex = 3
        Me.btnRestore.Text = " Khôi phục"
        Me.btnRestore.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRestore.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.GiaPha.My.Resources.Resources.Cancel
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(321, 143)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(104, 44)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "Đóng   "
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'bntBrowse
        '
        Me.bntBrowse.Image = CType(resources.GetObject("bntBrowse.Image"), System.Drawing.Image)
        Me.bntBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.bntBrowse.Location = New System.Drawing.Point(431, 40)
        Me.bntBrowse.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.bntBrowse.Name = "bntBrowse"
        Me.bntBrowse.Size = New System.Drawing.Size(96, 32)
        Me.bntBrowse.TabIndex = 2
        Me.bntBrowse.Text = "Chọn file"
        Me.bntBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.bntBrowse.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(198, 16)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Chọn file dữ liệu muốn khôi phục"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Red
        Me.Label2.Location = New System.Drawing.Point(12, 81)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(515, 16)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Khi dữ liệu được khôi phục thành công thì dữ liệu hiện tại sẽ bị mất hoàn toàn. "
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Red
        Me.Label3.Location = New System.Drawing.Point(12, 102)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(250, 16)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Hãy chú ý trước khi khôi phục dữ liệu."
        '
        'frmRestore
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(547, 212)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.bntBrowse)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnRestore)
        Me.Controls.Add(Me.txtFileName)
        Me.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmRestore"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Khôi phục dữ liệu"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtFileName As System.Windows.Forms.TextBox
    Friend WithEvents btnRestore As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents bntBrowse As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
