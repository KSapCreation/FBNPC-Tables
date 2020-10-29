Imports common
Imports System.IO
Imports System.Text
Imports System.Security.AccessControl

Public Class FrmServerConfig

    Private Sub RadButton1_Click(sender As Object, e As EventArgs) Handles RadButton1.Click
        Try
            If AllowToSave() Then
                clsCommon.MyMessageBoxShow("Connection is working", Me.Text)
            End If
        Catch ex As Exception
            clsCommon.MyMessageBoxShow(ex.Message, Me.Text)
        End Try
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        Try
            If AllowToSave() Then
              

                ' Create or overwrite the file. 
                Dim fs As FileStream = System.IO.File.Create("config.Txp")

                'Add text to the file. 
                Dim info As Byte() = New UTF8Encoding(True).GetBytes("" + txtServerName.Text + " # " + txtDatabaseName.Text + " # " + txtLogin.Text + " # " + clsCommon.EncryptString(txtPassword.Text) + "")
                fs.Write(info, 0, info.Length)
                fs.Close()
                Me.Close()
                objCommonVar.CurrDatabase = txtDatabaseName.Text
            End If
        Catch ex As Exception
            'Dim strDomName As String = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString()
            'AddDirectorySecurity(Application.StartupPath, strDomName, FileSystemRights.Modify, AccessControlType.Allow)
            If ex.Message.Contains("Access to the path") Then
                clsCommon.MyMessageBoxShow("Please give the modify permission" + Environment.NewLine + "Go to - " + Application.StartupPath + Environment.NewLine + "Right click on folder and select properties" + Environment.NewLine + "Select security Tab and click on Edit button" + Environment.NewLine + "Select login 'Group or user names' and give modify permission")
            Else
                clsCommon.MyMessageBoxShow(ex.Message, Me.Text)
            End If
        End Try
    End Sub

   
    Sub AddDirectorySecurity(ByVal FileName As String, ByVal Account As String, ByVal Rights As FileSystemRights, ByVal ControlType As AccessControlType)
        Try
            ' Create a new DirectoryInfoobject.
            Dim dInfo As New DirectoryInfo(FileName)

            ' Get a DirectorySecurity object that represents the current security settings.
            Dim dSecurity As DirectorySecurity = dInfo.GetAccessControl()

            ' Add the FileSystemAccessRule to the security settings (**following is one line of code**).
            dSecurity.AddAccessRule(New FileSystemAccessRule(Account, Rights, (InheritanceFlags.ContainerInherit +
            InheritanceFlags.ObjectInherit), PropagationFlags.InheritOnly, ControlType))

            ' Set the new access settings.
            dSecurity.SetAccessRuleProtection(True, True)
            dInfo.SetAccessControl(dSecurity)

        Catch ex As Exception
            clsCommon.MyMessageBoxShow(ex.Message, Me.Text)
        End Try
    End Sub

    




    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Function AllowToSave() As Boolean
        If clsCommon.myLen(txtServerName.Text) <= 0 Then
            txtServerName.Focus()
            Throw New Exception("Please enter Server Name")
        End If
        If clsCommon.myLen(txtDatabaseName.Text) <= 0 Then
            txtDatabaseName.Focus()
            Throw New Exception("Please enter Database Name")
        End If
        If clsCommon.myLen(txtLogin.Text) <= 0 Then
            txtLogin.Focus()
            Throw New Exception("Please enter Login")
        End If
        If clsCommon.myLen(txtPassword.Text) <= 0 Then
            txtPassword.Focus()
            Throw New Exception("Please enter password")
        End If
        clsDBFuncationality.SetConnection("server=" + txtServerName.Text + "; database= " + txtDatabaseName.Text + "; user id=" + txtLogin.Text + ";  password=" + txtPassword.Text)
        Dim qry As String = "select top 1 TABLE_NAME from INFORMATION_SCHEMA.Tables  "
        Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry)
        If Not (dt IsNot Nothing AndAlso (dt.Rows.Count > 0 OrElse dt.Columns.Count > 0)) Then
            Throw New Exception("Connection's property is not correct." + Environment.NewLine + "Please Try again...")
        End If
        Return True
    End Function

    Private Sub FrmServerConfig_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown

    End Sub

    Private Sub FrmServerConfig_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtServerName.Focus()
    End Sub

End Class
