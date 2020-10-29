Imports System.Data.SqlClient
Imports common
Public Class ClsCancelAfterPosting
#Region "variable"
    Public Program_Code As String
    Public Starting_Date As String
    Public Inactive_Date As String
    Public DTMccSMS As DataTable

#End Region
    Public Shared Function SaveData(ByVal Arr As List(Of ClsCancelAfterPosting)) As Boolean

        Dim trans As SqlTransaction = clsDBFuncationality.GetTransactin()
        Try
            Dim isDeleted As Boolean = False
            For Each obj As ClsCancelAfterPosting In Arr
                If clsCommon.myLen(obj.Starting_Date) > 0 And clsCommon.myLen(obj.Inactive_Date) <= 0 Then
                    'If isDeleted = False Then
                    clsDBFuncationality.ExecuteNonQuery("Delete from TSPL_Cancel_After_Posting_Tables_Details WHERE Form_Id='" + obj.Program_Code + "' and convert(date,starting_date,103)=convert(date,'" & clsCommon.GetPrintDate(obj.Starting_Date, "dd-MMM-yyyy") & "',103)", trans)
                    '    isDeleted = True
                    'End If
                    Dim IsSaved As Boolean = False
                    Dim coll As New Hashtable()
                    clsCommon.AddColumnsForChange(coll, "Form_Id", obj.Program_Code)
                    clsCommon.AddColumnsForChange(coll, "Starting_Date", clsCommon.GetPrintDate(obj.Starting_Date, "dd-MMM-yyyy hh:mm:ss"))
                    'clsCommon.AddColumnsForChange(coll, "Inactive_Date", obj.Inactive_Date)
                    clsCommon.AddColumnsForChange(coll, "Comp_Code", objCommonVar.CurrentCompanyCode)
                    clsCommon.AddColumnsForChange(coll, "Modify_By", objCommonVar.CurrentUserCode)
                    clsCommon.AddColumnsForChange(coll, "Modify_Date", clsCommon.GetPrintDate(clsCommon.GETSERVERDATE(trans), "dd/MM/yyyy"))
                    clsCommon.AddColumnsForChange(coll, "Created_By", objCommonVar.CurrentUserCode)
                    clsCommon.AddColumnsForChange(coll, "Created_Date", clsCommon.GetPrintDate(clsCommon.GETSERVERDATE(trans), "dd/MM/yyyy"))

                    IsSaved = clsCommonFunctionality.UpdateDataTable(coll, "TSPL_Cancel_After_Posting_Tables_Details", OMInsertOrUpdate.Insert, "", trans)
                End If
                If clsCommon.myLen(obj.Inactive_Date) > 0 And clsCommon.myLen(obj.Starting_Date) > 0 Then

                    Dim IsSaved As Boolean = False
                    'Dim coll As New Hashtable()
                    'clsCommon.AddColumnsForChange(coll, "Form_Id", obj.Program_Code)
                    'clsCommon.AddColumnsForChange(coll, "Starting_Date", clsCommon.GetPrintDate(obj.Starting_Date, "dd-MMM-yyyy hh:mm:ss"))
                    'clsCommon.AddColumnsForChange(coll, "Inactive_Date", clsCommon.GetPrintDate(obj.Inactive_Date, "dd-MMM-yyyy hh:mm:ss"))
                    'clsCommon.AddColumnsForChange(coll, "Comp_Code", objCommonVar.CurrentCompanyCode)
                    'clsCommon.AddColumnsForChange(coll, "Modify_By", objCommonVar.CurrentUserCode)
                    'clsCommon.AddColumnsForChange(coll, "Modify_Date", clsCommon.GetPrintDate(clsCommon.GETSERVERDATE(trans), "dd/MM/yyyy"))
                    'clsCommon.AddColumnsForChange(coll, "Created_By", objCommonVar.CurrentUserCode)
                    'clsCommon.AddColumnsForChange(coll, "Created_Date", clsCommon.GetPrintDate(clsCommon.GETSERVERDATE(trans), "dd/MM/yyyy"))

                    'IsSaved = clsCommonFunctionality.UpdateDataTable(coll, "TSPL_Cancel_After_Posting_Tables_Details", OMInsertOrUpdate.Update, " where Form_Id='" + obj.Program_Code + "' and convert(date,starting_date,103)=convert(date,'" & clsCommon.GetPrintDate(obj.Starting_Date, "dd-MMM-yyyy") & "',103)", trans)
                    Dim sQuery As String = "insert into TSPL_Cancel_After_Posting_Tables_Details_History_Data(Form_Id,Starting_Date,Created_By,Created_Date,Modify_By,Modify_Date,Comp_Code," _
                                         & " History_Date,Inactive_Date,History_By,Version_Id) select Form_Id,Starting_Date,Created_By,Created_Date,Modify_By,Modify_Date,Comp_Code" _
                                         & " ,'" & clsCommon.GetPrintDate(clsCommon.GETSERVERDATE(trans), "dd-MMM-yyyy hh:mm:ss") & "','" & clsCommon.GetPrintDate(clsCommon.GETSERVERDATE(trans), "dd-MMM-yyyy hh:mm:ss") & "','" & objCommonVar.CurrentUserCode & "'," _
                                         & " (select coalesce(max(Version_Id),0)+1 from TSPL_Cancel_After_Posting_Tables_Details_History_Data where " _
                                         & " TSPL_Cancel_After_Posting_Tables_Details_History_Data.form_Id=TSPL_Cancel_After_Posting_Tables_Details.Form_Id) " _
                                         & " from TSPL_Cancel_After_Posting_Tables_Details where Form_Id='" & obj.Program_Code & "'"
                    clsDBFuncationality.ExecuteNonQuery(sQuery, trans)
                    sQuery = "delete from TSPL_Cancel_After_Posting_Tables_Details where Form_Id='" & obj.Program_Code & "'"
                    clsDBFuncationality.ExecuteNonQuery(sQuery, trans)
                End If
            Next
            trans.Commit()
        Catch ex As Exception
            trans.Rollback()
            clsCommon.MyMessageBoxShow(ex.Message)

        End Try
        Return True
    End Function

    Public Shared Function DeleteData(ByVal strcode As String, ByVal trans As SqlTransaction) As Boolean
        Try
            If (clsCommon.myLen(strcode >= 0)) Then
                Dim qry As String = "delete from TSPL_Cancel_After_Posting_Tables_Details where Form_Id='" + strcode + "'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
            End If
            trans.Commit()
            Return True
        Catch ex As Exception
            trans.Rollback()
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function GetData(ByVal strcode As String) As List(Of ClsCancelAfterPosting)
        Try
            Dim qry As String = " select TSPL_CANCEL_TABLE_DETAILS.Form_id ,Program_Name,Inactive_Date,starting_Date from " _
            & " TSPL_CANCEL_TABLE_DETAILS left join TSPL_Cancel_After_Posting_Tables_Details on TSPL_CANCEL_TABLE_DETAILS.formId=TSPL_Cancel_After_Posting_Tables_Details.form_Id" _
            & " Left join tspl_program_Master on program_Code=TSPL_Cancel_After_Posting_Tables_Details.form_Id  "
            Dim dt1 As DataTable = clsDBFuncationality.GetDataTable(qry)
            Dim obj As ClsCancelAfterPosting
            Dim objlist As New List(Of ClsCancelAfterPosting)
            For Each row As DataRow In dt1.Rows
                obj = New ClsCancelAfterPosting
                obj.Program_Code = clsCommon.myCstr(row.Item("Form_id"))
                obj.Starting_Date = clsCommon.myCstr(row.Item("Starting_Date"))
                obj.Inactive_Date = clsCommon.myCstr(row.Item("Inactive_Date"))
                objlist.Add(obj)
            Next
            Return objlist
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
End Class
