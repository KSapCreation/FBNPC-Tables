'Imports common
Imports System.Reflection
Imports System.Data.SqlClient
Imports System.IO
Imports common
Imports System.Environment
Imports System.Net
Imports Microsoft.Win32
Imports Link.AppShortcut
Imports CgtFpAccessCSD200Dotnet
Imports System.Drawing.Imaging
Imports Telerik.WinControls.UI
Imports Telerik.WinControls
Imports Telerik.WinControls.Primitives




Public Class StartPage


    Private Sub MDI_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

    End Sub

    Private Sub MDI_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

    End Sub


    Private Sub MDI_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
     
    End Sub

   
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        Dim frm As New MDI
        frm.Show()
    End Sub

    Private Sub btnEndProcess_Click(sender As Object, e As EventArgs) Handles btnEnd.Click
        Dim frm As New MDI()
        frm.Close()
        Me.Close()
    End Sub
End Class
