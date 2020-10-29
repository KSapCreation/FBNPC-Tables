Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports Telerik.WinControls

Public Module myMessages
    Dim message As String = ""
    Public Sub insert()
        message = "Record added Successfully."
        common.clsCommon.MyMessageBoxShow(message)
    End Sub
    Public Sub update()
        message = "Record updated Successfully."
        common.clsCommon.MyMessageBoxShow(message)
    End Sub
    Public Sub delete()
        message = "Record deleted Successfully."
        common.clsCommon.MyMessageBoxShow(message)
    End Sub
    Public Sub post()
        message = "Record posted Successfully."
        common.clsCommon.MyMessageBoxShow(message)
    End Sub
    Public Sub zeroGridRow()
        message = "Please provide atleast one value."
        common.clsCommon.MyMessageBoxShow(message)
    End Sub
    Public Sub blankValue(ByVal control As String)
        message = control + " can not be left blank."
        common.clsCommon.MyMessageBoxShow(message)
    End Sub


    Public Sub myExceptions(ByVal exception As Exception)
        common.clsCommon.MyMessageBoxShow(exception.Message)
    End Sub
    Public Sub myExceptions1(ByVal exception As Exception)
        myExceptions(exception)
    End Sub


    Public Function insertConfirm() As Boolean
        message = "Do you want to add this record."
        If common.clsCommon.MyMessageBoxShow(message, "", MessageBoxButtons.YesNo, RadMessageIcon.Question) = DialogResult.No Then
            Return False
        End If
        Return True
    End Function
    Public Function updateConfirm() As Boolean
        message = "Do you want to update this record."
        If common.clsCommon.MyMessageBoxShow(message, "", MessageBoxButtons.YesNo, RadMessageIcon.Question) = DialogResult.No Then
            Return False
        End If
        Return True
    End Function
    Public Function deleteConfirm() As Boolean
        message = "Do you want to delete this record."
        If common.clsCommon.MyMessageBoxShow(message, "", MessageBoxButtons.YesNo, RadMessageIcon.Question) = DialogResult.No Then
            Return False
        End If
        Return True
    End Function
    Public Function postConfirm() As Boolean
        message = "Do you want to post this record."
        If common.clsCommon.MyMessageBoxShow(message, "", MessageBoxButtons.YesNo, RadMessageIcon.Question) = DialogResult.No Then
            Return False
        End If
        Return True
    End Function
    Public Function cancelConfirm() As Boolean
        message = "Do you want to cancel this record."
        If common.clsCommon.MyMessageBoxShow(message, "", MessageBoxButtons.YesNo, RadMessageIcon.Question) = DialogResult.No Then
            Return False
        End If
        Return True
    End Function

    Public Function ItemchkConfirm() As Boolean
        message = "Do you want to replicate the same Scheme Item with other."
        If common.clsCommon.MyMessageBoxShow(message, "", MessageBoxButtons.YesNo, RadMessageIcon.Question) = DialogResult.No Then
            Return False
        End If
        Return True
    End Function

    Public Function SchemeCloseCheck() As Boolean


        message = "Do you want to close the previous Scheme  of same type ."
        If common.clsCommon.MyMessageBoxShow(message, "", MessageBoxButtons.YesNo, RadMessageIcon.Question) = DialogResult.No Then
            Return False
        End If
        Return True
    End Function


    Public Function invoiceConfirm() As Boolean
        message = "Do you want to create Invoice."
        If common.clsCommon.MyMessageBoxShow(message, "", MessageBoxButtons.YesNo, RadMessageIcon.Question) = DialogResult.No Then
            Return False
        End If
        Return True
    End Function
    Public Function ApprovalConfirm() As Boolean
        message = "Approval is required to post that document due to extra discount."
        If common.clsCommon.MyMessageBoxShow(message, "", MessageBoxButtons.YesNo, RadMessageIcon.Question) = DialogResult.No Then
            Return False
        End If
        Return True
    End Function

    Public Sub checkMaxLength(ByVal control As String, ByVal value As String, ByVal maxLength As Integer)
        If value.Length > maxLength Then
            message = control + " length can not be greater than " + maxLength
            common.clsCommon.MyMessageBoxShow(message)
        End If
    End Sub
    Public Function EnableConfirm() As Boolean
        message = "Do you want to Enable This Screen."
        If common.clsCommon.MyMessageBoxShow(message, "", MessageBoxButtons.YesNo, RadMessageIcon.Question) = DialogResult.No Then
            Return False
        End If
        Return True
    End Function
    Public Function GLAccountRefreshConfirm() As Boolean
        message = "If you change location then all fill gl Account lost.Do you want to change location."
        If common.clsCommon.MyMessageBoxShow(message, "", MessageBoxButtons.YesNo, RadMessageIcon.Question) = DialogResult.No Then
            Return False
        End If
        Return True
    End Function
  

End Module
