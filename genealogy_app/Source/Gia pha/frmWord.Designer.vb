<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWord
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmWord))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dgvList = New System.Windows.Forms.DataGridView()
        Me.chkSelectAll = New System.Windows.Forms.CheckBox()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.clmSelect = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.clmSTT = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clmMemID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clmName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clmGeneration = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clmTempLevel = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(165, 12)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Chọn thành viên để xuất dữ liệu:"
        '
        'dgvList
        '
        Me.dgvList.AllowUserToAddRows = False
        Me.dgvList.AllowUserToDeleteRows = False
        Me.dgvList.AllowUserToResizeRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvList.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.clmSelect, Me.clmSTT, Me.clmMemID, Me.clmName, Me.clmGeneration, Me.clmTempLevel})
        Me.dgvList.Location = New System.Drawing.Point(12, 50)
        Me.dgvList.Name = "dgvList"
        Me.dgvList.RowHeadersVisible = False
        Me.dgvList.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvList.Size = New System.Drawing.Size(451, 343)
        Me.dgvList.TabIndex = 10
        '
        'chkSelectAll
        '
        Me.chkSelectAll.AutoSize = True
        Me.chkSelectAll.Checked = True
        Me.chkSelectAll.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSelectAll.Location = New System.Drawing.Point(13, 405)
        Me.chkSelectAll.Name = "chkSelectAll"
        Me.chkSelectAll.Size = New System.Drawing.Size(96, 16)
        Me.chkSelectAll.TabIndex = 20
        Me.chkSelectAll.Text = "Bỏ chọn tất cả"
        Me.chkSelectAll.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Image = Global.GiaPha.My.Resources.Resources.MSWord
        Me.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExport.Location = New System.Drawing.Point(149, 423)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(148, 38)
        Me.btnExport.TabIndex = 30
        Me.btnExport.Text = "        Xuất ra file Word >>"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'clmSelect
        '
        Me.clmSelect.HeaderText = "Chọn"
        Me.clmSelect.MinimumWidth = 35
        Me.clmSelect.Name = "clmSelect"
        Me.clmSelect.TrueValue = "1"
        Me.clmSelect.Width = 35
        '
        'clmSTT
        '
        Me.clmSTT.DataPropertyName = "clmSTT"
        Me.clmSTT.HeaderText = "STT"
        Me.clmSTT.Name = "clmSTT"
        Me.clmSTT.ToolTipText = "Số thứ tự"
        Me.clmSTT.Width = 50
        '
        'clmMemID
        '
        Me.clmMemID.DataPropertyName = "clmMemID"
        Me.clmMemID.HeaderText = "MemberID"
        Me.clmMemID.Name = "clmMemID"
        Me.clmMemID.Visible = False
        '
        'clmName
        '
        Me.clmName.DataPropertyName = "clmName"
        Me.clmName.HeaderText = "Họ Tên"
        Me.clmName.MinimumWidth = 150
        Me.clmName.Name = "clmName"
        Me.clmName.ToolTipText = "Họ và Tên"
        Me.clmName.Width = 270
        '
        'clmGeneration
        '
        Me.clmGeneration.DataPropertyName = "clmGeneration"
        Me.clmGeneration.HeaderText = "Đời thứ"
        Me.clmGeneration.MinimumWidth = 75
        Me.clmGeneration.Name = "clmGeneration"
        Me.clmGeneration.ToolTipText = "Đời trong dòng họ"
        Me.clmGeneration.Width = 75
        '
        'clmTempLevel
        '
        Me.clmTempLevel.DataPropertyName = "clmTempLevel"
        Me.clmTempLevel.HeaderText = "TempLevel"
        Me.clmTempLevel.Name = "clmTempLevel"
        Me.clmTempLevel.Visible = False
        '
        'frmWord
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(475, 468)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.chkSelectAll)
        Me.Controls.Add(Me.dgvList)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmWord"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Xuất dữ liệu"
        CType(Me.dgvList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dgvList As System.Windows.Forms.DataGridView
    Friend WithEvents chkSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents clmSelect As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents clmSTT As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmMemID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmGeneration As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmTempLevel As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
