<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmStatistics
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmStatistics))
        Me.Button1 = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.dgvStats = New System.Windows.Forms.DataGridView
        Me.lblBirth = New System.Windows.Forms.LinkLabel
        Me.lblDecease = New System.Windows.Forms.LinkLabel
        Me.clmTitle = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.clmValue = New System.Windows.Forms.DataGridViewTextBoxColumn
        CType(Me.dgvStats, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Image = Global.GiaPha.My.Resources.Resources.printer24
        Me.Button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button1.Location = New System.Drawing.Point(12, 461)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(111, 36)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "In kết quả"
        Me.Button1.UseVisualStyleBackColor = True
        Me.Button1.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnCancel.Image = Global.GiaPha.My.Resources.Resources.back_24
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(164, 461)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(111, 36)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.TabStop = False
        Me.btnCancel.Text = "    Quay lại"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Font = New System.Drawing.Font("Times New Roman", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(454, 51)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "THỐNG KÊ THÔNG TIN GIA PHẢ"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dgvStats
        '
        Me.dgvStats.AllowUserToAddRows = False
        Me.dgvStats.AllowUserToDeleteRows = False
        Me.dgvStats.AllowUserToResizeColumns = False
        Me.dgvStats.AllowUserToResizeRows = False
        Me.dgvStats.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvStats.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.dgvStats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvStats.ColumnHeadersVisible = False
        Me.dgvStats.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.clmTitle, Me.clmValue})
        Me.dgvStats.Location = New System.Drawing.Point(12, 54)
        Me.dgvStats.MultiSelect = False
        Me.dgvStats.Name = "dgvStats"
        Me.dgvStats.ReadOnly = True
        Me.dgvStats.RowHeadersVisible = False
        Me.dgvStats.RowTemplate.ReadOnly = True
        Me.dgvStats.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvStats.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvStats.Size = New System.Drawing.Size(430, 323)
        Me.dgvStats.TabIndex = 4
        '
        'lblBirth
        '
        Me.lblBirth.AutoSize = True
        Me.lblBirth.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        Me.lblBirth.Location = New System.Drawing.Point(12, 391)
        Me.lblBirth.Name = "lblBirth"
        Me.lblBirth.Size = New System.Drawing.Size(213, 16)
        Me.lblBirth.TabIndex = 5
        Me.lblBirth.TabStop = True
        Me.lblBirth.Text = "Danh sách ngày sinh nhật gần nhất"
        '
        'lblDecease
        '
        Me.lblDecease.AutoSize = True
        Me.lblDecease.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        Me.lblDecease.Location = New System.Drawing.Point(12, 419)
        Me.lblDecease.Name = "lblDecease"
        Me.lblDecease.Size = New System.Drawing.Size(177, 16)
        Me.lblDecease.TabIndex = 5
        Me.lblDecease.TabStop = True
        Me.lblDecease.Text = "Danh sách ngày giỗ gần nhất"
        '
        'clmTitle
        '
        Me.clmTitle.HeaderText = "Thống kê"
        Me.clmTitle.MinimumWidth = 250
        Me.clmTitle.Name = "clmTitle"
        Me.clmTitle.ReadOnly = True
        Me.clmTitle.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.clmTitle.Width = 250
        '
        'clmValue
        '
        Me.clmValue.HeaderText = "Giá trị"
        Me.clmValue.MinimumWidth = 175
        Me.clmValue.Name = "clmValue"
        Me.clmValue.ReadOnly = True
        Me.clmValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.clmValue.Width = 175
        '
        'frmStatistics
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(454, 510)
        Me.Controls.Add(Me.lblDecease)
        Me.Controls.Add(Me.lblBirth)
        Me.Controls.Add(Me.dgvStats)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(163, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmStatistics"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Thống kê thông tin"
        CType(Me.dgvStats, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dgvStats As System.Windows.Forms.DataGridView
    Friend WithEvents lblBirth As System.Windows.Forms.LinkLabel
    Friend WithEvents lblDecease As System.Windows.Forms.LinkLabel
    Friend WithEvents clmTitle As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmValue As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
