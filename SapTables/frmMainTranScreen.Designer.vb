<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmMainTranScreen
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
        Me.components = New System.ComponentModel.Container()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.GetControlToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetDescriptionForAllControlToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddForCustomFieldGridToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip1.SuspendLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GetControlToolStripMenuItem, Me.SetDescriptionForAllControlToolStripMenuItem, Me.AddForCustomFieldGridToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(234, 92)
        '
        'GetControlToolStripMenuItem
        '
        Me.GetControlToolStripMenuItem.Name = "GetControlToolStripMenuItem"
        Me.GetControlToolStripMenuItem.Size = New System.Drawing.Size(233, 22)
        Me.GetControlToolStripMenuItem.Text = "Set Description"
        Me.GetControlToolStripMenuItem.Visible = False
        '
        'SetDescriptionForAllControlToolStripMenuItem
        '
        Me.SetDescriptionForAllControlToolStripMenuItem.Name = "SetDescriptionForAllControlToolStripMenuItem"
        Me.SetDescriptionForAllControlToolStripMenuItem.Size = New System.Drawing.Size(233, 22)
        Me.SetDescriptionForAllControlToolStripMenuItem.Text = "Set Description For All Control"
        Me.SetDescriptionForAllControlToolStripMenuItem.Visible = False
        '
        'AddForCustomFieldGridToolStripMenuItem
        '
        Me.AddForCustomFieldGridToolStripMenuItem.Name = "AddForCustomFieldGridToolStripMenuItem"
        Me.AddForCustomFieldGridToolStripMenuItem.Size = New System.Drawing.Size(233, 22)
        Me.AddForCustomFieldGridToolStripMenuItem.Text = "Add For Custom Field Grid"
        '
        'FrmMainTranScreen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 270)
        Me.KeyPreview = True
        Me.Name = "FrmMainTranScreen"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.Text = "FrmMainTranScreen"
        Me.ContextMenuStrip1.ResumeLayout(False)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents GetControlToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SetDescriptionForAllControlToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddForCustomFieldGridToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class

