<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmServerConfig
    Inherits Telerik.WinControls.UI.RadForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.txtPassword = New common.Controls.MyTextBox()
        Me.MyLabel4 = New common.Controls.MyLabel()
        Me.txtLogin = New common.Controls.MyTextBox()
        Me.MyLabel3 = New common.Controls.MyLabel()
        Me.txtDatabaseName = New common.Controls.MyTextBox()
        Me.MyLabel2 = New common.Controls.MyLabel()
        Me.txtServerName = New common.Controls.MyTextBox()
        Me.MyLabel1 = New common.Controls.MyLabel()
        Me.RadButton1 = New Telerik.WinControls.UI.RadButton()
        Me.btnCancel = New Telerik.WinControls.UI.RadButton()
        Me.btnOK = New Telerik.WinControls.UI.RadButton()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.txtPassword, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MyLabel4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtLogin, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MyLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDatabaseName, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MyLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtServerName, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MyLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadButton1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.btnCancel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.btnOK, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtPassword)
        Me.SplitContainer1.Panel1.Controls.Add(Me.MyLabel4)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtLogin)
        Me.SplitContainer1.Panel1.Controls.Add(Me.MyLabel3)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtDatabaseName)
        Me.SplitContainer1.Panel1.Controls.Add(Me.MyLabel2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtServerName)
        Me.SplitContainer1.Panel1.Controls.Add(Me.MyLabel1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.RadButton1)
        Me.SplitContainer1.Panel2.Controls.Add(Me.btnCancel)
        Me.SplitContainer1.Panel2.Controls.Add(Me.btnOK)
        Me.SplitContainer1.Size = New System.Drawing.Size(356, 161)
        Me.SplitContainer1.SplitterDistance = 125
        Me.SplitContainer1.TabIndex = 0
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(98, 88)
        Me.txtPassword.MendatroryField = False
        Me.txtPassword.MyLinkLable1 = Me.MyLabel4
        Me.txtPassword.MyLinkLable2 = Nothing
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(249, 20)
        Me.txtPassword.TabIndex = 3
        '
        'MyLabel4
        '
        Me.MyLabel4.Location = New System.Drawing.Point(5, 89)
        Me.MyLabel4.Name = "MyLabel4"
        Me.MyLabel4.Size = New System.Drawing.Size(53, 18)
        Me.MyLabel4.TabIndex = 7
        Me.MyLabel4.Text = "Password"
        '
        'txtLogin
        '
        Me.txtLogin.Location = New System.Drawing.Point(98, 62)
        Me.txtLogin.MendatroryField = False
        Me.txtLogin.MyLinkLable1 = Me.MyLabel3
        Me.txtLogin.MyLinkLable2 = Nothing
        Me.txtLogin.Name = "txtLogin"
        Me.txtLogin.Size = New System.Drawing.Size(249, 20)
        Me.txtLogin.TabIndex = 2
        '
        'MyLabel3
        '
        Me.MyLabel3.Location = New System.Drawing.Point(5, 63)
        Me.MyLabel3.Name = "MyLabel3"
        Me.MyLabel3.Size = New System.Drawing.Size(34, 18)
        Me.MyLabel3.TabIndex = 6
        Me.MyLabel3.Text = "Login"
        '
        'txtDatabaseName
        '
        Me.txtDatabaseName.Location = New System.Drawing.Point(98, 36)
        Me.txtDatabaseName.MendatroryField = False
        Me.txtDatabaseName.MyLinkLable1 = Me.MyLabel2
        Me.txtDatabaseName.MyLinkLable2 = Nothing
        Me.txtDatabaseName.Name = "txtDatabaseName"
        Me.txtDatabaseName.Size = New System.Drawing.Size(249, 20)
        Me.txtDatabaseName.TabIndex = 1
        '
        'MyLabel2
        '
        Me.MyLabel2.Location = New System.Drawing.Point(5, 37)
        Me.MyLabel2.Name = "MyLabel2"
        Me.MyLabel2.Size = New System.Drawing.Size(86, 18)
        Me.MyLabel2.TabIndex = 5
        Me.MyLabel2.Text = "Database Name"
        '
        'txtServerName
        '
        Me.txtServerName.Location = New System.Drawing.Point(98, 10)
        Me.txtServerName.MendatroryField = False
        Me.txtServerName.MyLinkLable1 = Me.MyLabel1
        Me.txtServerName.MyLinkLable2 = Nothing
        Me.txtServerName.Name = "txtServerName"
        Me.txtServerName.Size = New System.Drawing.Size(249, 20)
        Me.txtServerName.TabIndex = 0
        '
        'MyLabel1
        '
        Me.MyLabel1.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.MyLabel1.Location = New System.Drawing.Point(5, 11)
        Me.MyLabel1.Name = "MyLabel1"
        Me.MyLabel1.Size = New System.Drawing.Size(70, 18)
        Me.MyLabel1.TabIndex = 4
        Me.MyLabel1.Text = "Server Name"
        '
        'RadButton1
        '
        Me.RadButton1.Location = New System.Drawing.Point(30, 3)
        Me.RadButton1.Name = "RadButton1"
        Me.RadButton1.Size = New System.Drawing.Size(109, 24)
        Me.RadButton1.TabIndex = 0
        Me.RadButton1.Text = "Check Connection"
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(236, 3)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(91, 24)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "Cancel"
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(142, 3)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(91, 24)
        Me.btnOK.TabIndex = 1
        Me.btnOK.Text = "OK"
        '
        'FrmServerConfig
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(356, 161)
        Me.ControlBox = False
        Me.Controls.Add(Me.SplitContainer1)
        Me.KeyPreview = True
        Me.MaximumSize = New System.Drawing.Size(364, 191)
        Me.MinimumSize = New System.Drawing.Size(364, 191)
        Me.Name = "FrmServerConfig"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Server Configuration"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.txtPassword, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MyLabel4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtLogin, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MyLabel3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDatabaseName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MyLabel2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtServerName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MyLabel1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadButton1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.btnCancel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.btnOK, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents txtPassword As common.Controls.MyTextBox
    Friend WithEvents MyLabel4 As common.Controls.MyLabel
    Friend WithEvents txtLogin As common.Controls.MyTextBox
    Friend WithEvents MyLabel3 As common.Controls.MyLabel
    Friend WithEvents txtDatabaseName As common.Controls.MyTextBox
    Friend WithEvents MyLabel2 As common.Controls.MyLabel
    Friend WithEvents txtServerName As common.Controls.MyTextBox
    Friend WithEvents MyLabel1 As common.Controls.MyLabel
    Friend WithEvents RadButton1 As Telerik.WinControls.UI.RadButton
    Friend WithEvents btnCancel As Telerik.WinControls.UI.RadButton
    Friend WithEvents btnOK As Telerik.WinControls.UI.RadButton
End Class

