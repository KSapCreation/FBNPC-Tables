'--- modified By : Manoj Sir 18/10/2012 :12:45 PM

Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationSettings
Imports Telerik.WinControls
Imports System.Globalization
Imports System.Collections
Imports common
Public Module connectSql
    Public strConn As String = clsDBFuncationality.connectionString 'Configuration.ConfigurationSettings.AppSettings("connectionString").ToString()
    Dim sql As String

    Public Function SqlCon() As String
        'Dim stp As String = Configuration.ConfigurationSettings.AppSettings("connectionString").ToString()
        Return clsDBFuncationality.connectionString
    End Function

    Public Function Connection() As SqlConnection
        Return clsDBFuncationality.GetConnnection
    End Function

    Public Function OpenConnection() As SqlConnection
        Dim conn As SqlConnection = Connection()
        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Return conn
    End Function

    Public Function ReturnConvFact(ByVal itemcode As String, ByVal uom As String, Optional ByVal trans As SqlTransaction = Nothing) As Decimal
        Dim convfact As Decimal = 0
        If trans Is Nothing Then
            convfact = connectSql.RunScalar("select Conversion_Factor  from TSPL_ITEM_UOM_DETAIL where Item_Code = '" + itemcode + "' and UOM_Code = '" + uom + "'")
        Else
            convfact = connectSql.RunScalar(trans, "select Conversion_Factor  from TSPL_ITEM_UOM_DETAIL where Item_Code = '" + itemcode + "' and UOM_Code = '" + uom + "'")
        End If
        Return convfact
    End Function

    Public Sub CloseConnection(ByVal cnn As SqlConnection)
        Dim conn As SqlConnection = Connection()
        Try
            If (conn.State And ConnectionState.Open) = ConnectionState.Open Then
                conn.Close()
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function RunSql(ByVal strSQL As String) As Boolean
        Return RunSqlTransaction(Nothing, strSQL)
    End Function

    Public Function RunSqlTransaction(ByVal trans As SqlTransaction, ByVal strSQL As String) As Boolean
        Return clsDBFuncationality.ExecuteNonQuery(strSQL, trans)
    End Function

    Public Function RunScalar(ByVal strSQL As String) As String

        Return RunScalar(Nothing, strSQL)
    End Function

    Public Function RunScalar(ByVal trans As SqlTransaction, ByVal strSQL As String) As String
        Return clsCommon.myCstr(clsDBFuncationality.getSingleValue(strSQL, trans))
    End Function

    Public Function RunSQLReturnDS(ByVal strSql As String) As DataSet
        Return RunSQLReturnDS(Nothing, strSql)
    End Function

    Public Function RunSQLReturnDS(ByVal trans As SqlTransaction, ByVal strSql As String) As DataSet
        Dim ds As New DataSet()
        ds.Tables.Add(clsDBFuncationality.GetDataTable(strSql, trans).Copy())
        Return ds
    End Function

    '------To be remove
    ''Public Function RunSqlReturnDR(ByVal SQL As String) As SqlDataReader
    ''    Dim cmd As SqlCommand
    ''    Dim cn As SqlConnection
    ''    Dim dr As SqlDataReader
    ''    Try

    ''        cn = OpenConnection()

    ''        cmd = New SqlCommand(SQL, cn)
    ''        dr = cmd.ExecuteReader()
    ''        'cmd.Dispose()
    ''        'CloseConnection(cn);
    ''        Return dr
    ''    Catch ex As Exception
    ''        Throw ex
    ''    Finally
    ''        CloseConnection(cn)
    ''        'cmd.Dispose()

    ''    End Try

    ''End Function

    ''Public Function RunSqlReturnDR(ByVal trans As SqlTransaction, ByVal SQL As String) As SqlDataReader
    ''    Dim dr As SqlDataReader
    ''    Dim cmd As New SqlCommand(SQL, trans.Connection)
    ''    cmd.Transaction = trans
    ''    dr = cmd.ExecuteReader()
    ''    'cmd.Dispose()
    ''    'CloseConnection(cn);
    ''    Return dr
    ''End Function
    '------End Of To be remove

    Public Function RunSp(ByVal StrSp As String, ByVal ParamArray CommandParameters As SqlParameter()) As Boolean
        Return RunSpTransaction(Nothing, StrSp, CommandParameters)
    End Function

    Public Function RunSpTransaction(ByVal StrSp As String, ByVal ParamArray CommandParameters As SqlParameter()) As Integer
        Return RunSpTransaction(Nothing, StrSp, CommandParameters)
    End Function

    Public Function RunSpTransaction(ByVal trans As SqlTransaction, ByVal StrSp As String, ByVal ParamArray CommandParameters As SqlParameter()) As Boolean
        Return clsDBFuncationality.SaveAStorePorcedure(trans, StrSp, CommandParameters)
    End Function

    Public Function autoNumber(ByVal tableName As String, ByVal trans As SqlTransaction) As Integer
        Dim tCode As Integer = 0
        Dim i As Integer = CountTableRows(tableName, trans)
        If i = 0 Then
            tCode = 1
        Else
            tCode = i + 1
        End If
        Return tCode
    End Function

    Private Function CountTableRows(ByVal tableName As String, ByVal trans As SqlTransaction) As Integer
        Dim sql As String = "select count(*) as count from " + tableName
        Dim i As Integer = clsCommon.myCdbl(clsDBFuncationality.getSingleValue(sql, trans))
        Return i
    End Function

    ''Public Function CountTransactionRows(ByVal tableName As String, ByVal colName As String, ByVal preLentgth As Integer) As Integer
    ''    Dim dr As SqlDataReader
    ''    Dim sql As String
    ''    sql = "select right('00' + convert(varchar, datepart(month, GetDate())), 2)"
    ''    dr = RunSqlReturnDR(sql)
    ''    dr.Read()
    ''    Dim curMonth As String = dr(0).ToString()
    ''    dr.Close()
    ''    sql = "select YEAR(GETDATE())"
    ''    dr = RunSqlReturnDR(sql)
    ''    dr.Read()
    ''    Dim curYear As String = dr(0).ToString()
    ''    dr.Close()
    ''    sql = "select count(*) as count from " + tableName + " WHERE SUBSTRING(" + colName + "," + preLentgth.ToString() + ",6)=" + curYear + curMonth
    ''    dr = RunSqlReturnDR(sql)
    ''    dr.Read()
    ''    Dim i As Integer = CInt(dr("count").ToString())
    ''    Return i
    ''End Function


    Public Function checkuseraccount(ByVal userCode As String)

        Dim location As String = ""

        Try
            If Not String.IsNullOrEmpty(connectSql.RunScalar("select segment_code from TSPL_GL_SEGMENT_PERMISSION where user_code = '" + userCode + "'")) Then
                location = connectSql.RunScalar("select segment_code from TSPL_GL_SEGMENT_PERMISSION where user_code = '" + userCode + "'")
                location = "%" + location
                'ds = connectSql.RunSQLReturnDS("SELECT Account_Code FROM TSPL_GL_ACCOUNTS WHERE Account_Code LIKE '" + location + "'")
                'dt = ds.Tables(0)
            End If
        Catch ex As Exception
            myMessages.myExceptions(ex)
        End Try
        If Not String.IsNullOrEmpty(location) Then
        Else
            location = "NULL"
        End If

        Return location
    End Function
    ''To check the gl security when user has multiple segment
    Public Function funglsegmentmultiple(ByVal userCode As String) As ArrayList
        Dim location As String
        Dim arrlocation As New ArrayList()
        Dim dr As DataTable
        Try
            location = "select segment_code from TSPL_GL_SEGMENT_PERMISSION where user_code = '" + userCode + "'"
            dr = clsDBFuncationality.GetDataTable(location)
            If dr.Rows.Count > 0 AndAlso dr.Rows IsNot Nothing Then
                For Each row As DataRow In dr.Rows
                    arrlocation.Add(clsCommon.myCstr(row("segment_code")))
                Next
            End If
        Catch ex As Exception
            myMessages.myExceptions(ex)
        End Try
        Return arrlocation
    End Function
    ''To check the gl security when user has select multiple account
    Public Function funglaccountmultiple(ByVal usercode As String) As ArrayList
        Dim straccount As String
        Dim arraccount As New ArrayList()
        Dim dr As DataTable
        Try
            straccount = "select Account_Code  from TSPL_GL_ACCOUNT_PERMISSION  WHERE User_Code = '" + usercode + "'"
            dr = clsDBFuncationality.GetDataTable(straccount)
            If dr.Rows.Count > 0 AndAlso dr.Rows IsNot Nothing Then
                For Each row As DataRow In dr.Rows
                    arraccount.Add(clsCommon.myCstr(row("Account_Code")))
                Next
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message.ToString())
        End Try
        Return arraccount
    End Function

    Public Function funglaccount(ByVal userCode As String) As String

        Dim straccount As String = ""
        Try
            If Not String.IsNullOrEmpty(connectSql.RunScalar("select Account_Code  from TSPL_GL_ACCOUNT_PERMISSION  WHERE User_Code = '" + userCode + "'")) Then
                straccount = connectSql.RunScalar("select Account_Code  from TSPL_GL_ACCOUNT_PERMISSION  WHERE User_Code = '" + userCode + "'")
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message.ToString())
        End Try
        If String.IsNullOrEmpty(straccount) Then
            straccount = "NULL"
        End If
        Return straccount
    End Function


    '''' added by priti on 01/06/11 
    '''' updated by Ajit on 07/06/2011
    '''' 

    Public Function serverDate() As String
        Return serverDate(Nothing)
    End Function

    Public Function serverDate(ByVal trans As SqlTransaction) As String
        Return clsCommon.GetPrintDate(clsCommon.GETSERVERDATE(trans), "dd/MM/yyyy")
    End Function

    Public Function myDate() As Date
        Return myDate(Nothing)
    End Function

    Public Function myDate(ByVal trans As SqlTransaction) As Date
        Return clsCommon.myCDate(serverDate(trans))
    End Function
    ''''' codes end here
End Module
