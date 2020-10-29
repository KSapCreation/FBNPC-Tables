Imports System.Data.SqlClient
Imports Telerik.WinControls.UI
Imports CrystalDecisions.CrystalReports.Engine
Imports common


Public Class clsERPFuncationality
    Public Shared Function GetGSTStatus(ByVal TransactionDate? As Date) As Boolean
        If objCommonVar.GSTApplicable AndAlso objCommonVar.GSTApplicableDate <= TransactionDate Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Shared Function GetNextCode(ByVal trans As SqlTransaction, ByVal dtDocDate As Date, ByVal strDocType As String, ByVal strTransType As String, ByVal strLocationCode As String) As String
        Return GetNextCode(trans, dtDocDate, strDocType, strTransType, strLocationCode, False)
    End Function
    Public Shared Function GetNextCode(ByVal trans As SqlTransaction, ByVal dtDocDate As Date, ByVal strDocType As String, ByVal strTransType As String, ByVal strLocationCode As String, ByVal isLocationCodeisSegment As Boolean) As String
        Return GetNextCode(trans, dtDocDate, strDocType, strTransType, strLocationCode, isLocationCodeisSegment, True)
    End Function

    Public Shared Function GetNextCode(ByVal trans As SqlTransaction, ByVal dtDocDate As Date, ByVal strDocType As String, ByVal strTransType As String, ByVal strLocationCode As String, ByVal isLocationCodeisSegment As Boolean, ByVal isIncreaseCounter As Boolean) As String
        Return GetNextCode(trans, dtDocDate, strDocType, strTransType, strLocationCode, isLocationCodeisSegment, isIncreaseCounter, False)
    End Function

    Public Shared Function GetNextCode(ByVal trans As SqlTransaction, ByVal dtDocDate As Date, ByVal strDocType As String, ByVal strTransType As String, ByVal strLocationCode As String, ByVal isLocationCodeisSegment As Boolean, ByVal isIncreaseCounter As Boolean, ByVal isLocationCodeisState As Boolean) As String
        Return GetNextCode(trans, dtDocDate, strDocType, strTransType, strLocationCode, isLocationCodeisSegment, isIncreaseCounter, isLocationCodeisState, False)
    End Function
    Public Shared Function GetNextCode(ByVal trans As SqlTransaction, ByVal dtDocDate As Date, ByVal strDocType As String, ByVal strTransType As String, ByVal strLocationCode As String, ByVal isLocationCodeisSegment As Boolean, ByVal isIncreaseCounter As Boolean, ByVal isLocationCodeisState As Boolean, ByRef isMonthlyChange As Boolean) As String
        Dim qry As String = ""
        Dim strRetCode As String = ""
        Dim strLocatinSegmentCode As String = ""
        Dim dt As DataTable
        Dim blnBackLog = False
        Dim strBackLogNextNo = 0
        blnBackLog = IIf(clsCommon.myCdbl(clsDBFuncationality.getSingleValue("select Description from TSPL_FIXED_PARAMETER where Code='" & clsFixedParameterCode.AllowAutoNoForBackLogEntry & "'", trans)) = 0, False, True)

        If blnBackLog Then
            If (clsCommon.CompairString(strDocType, "Fresh Dispatch") = CompairStringResult.Equal OrElse clsCommon.CompairString(strDocType, "Fresh Invoice") = CompairStringResult.Equal OrElse clsCommon.CompairString(strDocType, "Shipment Product Sale") = CompairStringResult.Equal OrElse clsCommon.CompairString(strDocType, "Product Invoice") = CompairStringResult.Equal) Then
                Dim strNextNo As String = clsCommon.myCstr(clsDBFuncationality.getSingleValue("select BackLog_Next_Number from TSPL_DOCPREFIX_BACKLOG where  Doc_Type='" + strDocType + "' and  isnull(Doc_Trans_Type,'')='" + strTransType + "' and isnull(Location_Code,'')='" + strLocationCode + "' and BackLog_Date > '" + clsCommon.GetPrintDate(dtDocDate, "dd/MMM/yyyy") + "'  ", trans))
                If clsCommon.myLen(strNextNo) > 0 Then
                    qry = "update TSPL_DOCPREFIX_BACKLOG set BackLog_Next_Number ='" + clsCommon.incval(strNextNo) + "'  where  Doc_Type='" + strDocType + "' and  isnull(Doc_Trans_Type,'')='" + strTransType + "' and isnull(Location_Code,'')='" + strLocationCode + "' and BackLog_Date > '" + clsCommon.GetPrintDate(dtDocDate, "dd/MMM/yyyy") + "'  "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                    Return strNextNo
                End If
            End If
        End If

        If isLocationCodeisState Then
            qry = "select 1 from TSPL_STATE_MASTER where STATE_CODE='" + strLocationCode + "'"
            dt = clsDBFuncationality.GetDataTable(qry, trans)
            If dt Is Nothing OrElse dt.Rows.Count <= 0 Then
                Throw New Exception(strLocationCode + " is not a state")
            End If
            strLocatinSegmentCode = strLocationCode
        ElseIf isLocationCodeisSegment Then
            qry = "SELECT 1 from TSPL_GL_SEGMENT_CODE where Seg_No='7' and Segment_code='" + strLocationCode + "'"
            dt = clsDBFuncationality.GetDataTable(qry, trans)
            If dt Is Nothing OrElse dt.Rows.Count <= 0 Then
                Throw New Exception(strLocationCode + " is not a Location Segment")
            End If
            strLocatinSegmentCode = strLocationCode
        Else
            If clsCommon.myLen(strLocationCode) > 0 Then
                strLocatinSegmentCode = clsCommon.myCstr(clsDBFuncationality.getSingleValue("SELECT Loc_Segment_Code from TSPL_LOCATION_MASTER WHERE Location_Code='" + strLocationCode + "'", trans))
                If clsCommon.myLen(strLocatinSegmentCode) <= 0 Then
                    Throw New Exception("Location Segment code Not found for Location :" + strLocationCode)
                End If
            End If
        End If

        Dim IntFiscalYear As Integer = dtDocDate.Year
        If dtDocDate.Month >= 1 AndAlso dtDocDate.Month <= 3 Then
            IntFiscalYear -= 1
        End If


        qry = GetQryOFDOCPrefix(dtDocDate, strDocType, strTransType, strLocatinSegmentCode, IntFiscalYear, False)
        dt = clsDBFuncationality.GetDataTable(qry, trans)
        If dt Is Nothing OrElse dt.Rows.Count <= 0 Then
            qry = GetQryOFDOCPrefix(dtDocDate.AddMonths(-1), strDocType, strTransType, strLocatinSegmentCode, IntFiscalYear, True)
            dt = clsDBFuncationality.GetDataTable(qry, trans)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 AndAlso (clsCommon.myCdbl(dt.Rows(0)("Is_Change_monthly")) = 1 OrElse clsCommon.myCdbl(dt.Rows(0)("Is_Change_Daily")) = 1) Then
                Dim coll As New Hashtable()
                clsCommon.AddColumnsForChange(coll, "Doc_Type", strDocType)
                clsCommon.AddColumnsForChange(coll, "Doc_Trans_Type", strTransType)
                clsCommon.AddColumnsForChange(coll, "Location_Code", strLocatinSegmentCode)
                clsCommon.AddColumnsForChange(coll, "Doc_Prfeix", clsCommon.myCstr(dt.Rows(0)("Doc_Prfeix")))
                clsCommon.AddColumnsForChange(coll, "Fin_Year", IntFiscalYear)
                clsCommon.AddColumnsForChange(coll, "Next_Number", 1)
                clsCommon.AddColumnsForChange(coll, "Separator", clsCommon.myCstr(dt.Rows(0)("Separator")))

                clsCommon.AddColumnsForChange(coll, "Comp_Code", objCommonVar.CurrentCompanyCode)
                clsCommon.AddColumnsForChange(coll, "Created_By", objCommonVar.CurrentUserCode)
                clsCommon.AddColumnsForChange(coll, "Created_Date", clsCommon.GetPrintDate(clsCommon.GETSERVERDATE(trans), "dd/MMM/yyyy"))
                clsCommon.AddColumnsForChange(coll, "Modify_By", objCommonVar.CurrentUserCode)
                clsCommon.AddColumnsForChange(coll, "Modify_Date", clsCommon.GetPrintDate(clsCommon.GETSERVERDATE(trans), "dd/MMM/yyyy"))

                clsCommon.AddColumnsForChange(coll, "Is_Change_Monthly", clsCommon.myCdbl(dt.Rows(0)("Is_Change_monthly")))
                If clsCommon.myCdbl(dt.Rows(0)("Is_Change_monthly")) = 1 Then
                    clsCommon.AddColumnsForChange(coll, "Curr_Month", dtDocDate.Month)
                End If
                clsCommon.AddColumnsForChange(coll, "Year_Separator", clsCommon.myCstr(dt.Rows(0)("Year_Separator")))
                clsCommon.AddColumnsForChange(coll, "Is_Change_Daily", clsCommon.myCdbl(dt.Rows(0)("Is_Change_Daily")))
                If clsCommon.myCdbl(dt.Rows(0)("Is_Change_Daily")) = 1 Then
                    clsCommon.AddColumnsForChange(coll, "Curr_Date", clsCommon.GetPrintDate(dtDocDate, "dd/MMM/yyyy"))
                End If
                clsCommon.AddColumnsForChange(coll, "dontDisplayYearInSeries", dt.Rows(0)("dontDisplayYearInSeries"))
                clsCommon.AddColumnsForChange(coll, "MinSizeofSeries", dt.Rows(0)("MinSizeofSeries"))
                clsCommonFunctionality.UpdateDataTable(coll, "TSPL_DOCPREFIX_MASTER", OMInsertOrUpdate.Insert, "", trans)

                qry = GetQryOFDOCPrefix(dtDocDate, strDocType, strTransType, strLocatinSegmentCode, IntFiscalYear, False)
                dt = clsDBFuncationality.GetDataTable(qry, trans)
            End If
        End If

        If dt Is Nothing OrElse dt.Rows.Count <= 0 Then
            Dim strException As String = "Please ask your Administrator to Set the Counter  " + Environment.NewLine + _
             "Transaction - " + strDocType + Environment.NewLine + _
             "Fiscal year - " + clsCommon.myCstr(IntFiscalYear) + Environment.NewLine
            If clsCommon.myLen(strTransType) > 0 Then
                strException += "Transaction Type - " + strTransType + Environment.NewLine
            End If
            If clsCommon.myLen(strLocatinSegmentCode) > 0 Then
                If isLocationCodeisState Then
                    strException += "State - " + strLocatinSegmentCode + Environment.NewLine
                Else
                    strException += "Segment Location - " + strLocatinSegmentCode + Environment.NewLine
                End If
            End If
            Throw New Exception(strException)
        End If
        ''**********Generate Counter ************************
        Dim intCurrCounter As Integer = Convert.ToInt32(clsCommon.myCdbl(dt.Rows(0)("Next_Number")))
        Dim isDailyChange As Boolean = IIf(clsCommon.myCdbl(dt.Rows(0)("Is_Change_Daily")) = 1, True, False)
        Dim intCurrDate As Date? = Nothing
        If isDailyChange Then
            intCurrDate = clsCommon.myCDate(dt.Rows(0)("Curr_Date"))
        End If



        isMonthlyChange = IIf(clsCommon.myCdbl(dt.Rows(0)("Is_Change_monthly")) = 1, True, False)
        Dim intCurrMonth As Integer = Convert.ToInt32(clsCommon.myCdbl(dt.Rows(0)("Curr_Month")))
        Dim strSep = clsCommon.myCstr(dt.Rows(0)("Separator")).Trim()

        Dim strFinYear As String = ""
        If dtDocDate.Month >= 1 AndAlso dtDocDate.Month <= 3 Then
            strFinYear = clsCommon.myCstr(dtDocDate.Year - 1 - 2000)
        Else
            strFinYear = clsCommon.myCstr(dtDocDate.Year - 2000)
        End If

        qry = "select Description from TSPL_FIXED_PARAMETER where Type='" + clsFixedParameterType.CounterFinancialYearStyle + "' and Code='" + clsFixedParameterCode.CounterFinancialYearStyle + "'"
        If (clsCommon.myCdbl(clsDBFuncationality.getSingleValue(qry, trans)) = 1) Then
            Dim intYear As Integer = dtDocDate.Year - 2000
            If dtDocDate.Month >= 1 AndAlso dtDocDate.Month <= 3 Then
                strFinYear = clsCommon.myCstr(intYear - 1) + clsCommon.myCstr(dt.Rows(0)("Year_Separator")).Trim() + clsCommon.myCstr(intYear)
            Else
                strFinYear = clsCommon.myCstr(intYear) + clsCommon.myCstr(dt.Rows(0)("Year_Separator")).Trim() + clsCommon.myCstr(intYear + 1)
            End If
        End If
        If clsERPFuncationality.GetGSTStatus(dtDocDate) Then
            If clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.LinkFinancialYearStyleWithGSTDate, clsFixedParameterType.LinkFinancialYearStyleWithGSTDate, trans)) > 0 Then
                If dtDocDate.Month >= 1 AndAlso dtDocDate.Month <= 3 Then
                    strFinYear = clsCommon.myCstr(dtDocDate.Year - 1 - 2000)
                Else
                    strFinYear = clsCommon.myCstr(dtDocDate.Year - 2000)
                End If
            End If
        End If



        'strRetCode = clsCommon.myCstr(dt.Rows(0)("Doc_Prfeix")).Trim() + strSep + strFinYear + strSep
        If clsCommon.myCBool(dt.Rows(0)("dontDisplayYearInSeries")) = False Then
            strRetCode = clsCommon.myCstr(dt.Rows(0)("Doc_Prfeix")).Trim() + strSep + strFinYear + strSep
        Else
            strRetCode = clsCommon.myCstr(dt.Rows(0)("Doc_Prfeix")).Trim() + strSep
        End If
        Dim intNumPartLen As Integer = clsCommon.myCdbl(dt.Rows(0)("MinSizeofSeries"))
        If isDailyChange Then
            intNumPartLen = clsCommon.myCdbl(dt.Rows(0)("MinSizeofSeries"))
            strRetCode += IIf(intCurrDate.Value.Month < 10, "0", "") + clsCommon.myCstr(intCurrDate.Value.Month).Trim() + strSep + IIf(intCurrDate.Value.Day < 10, "0", "") + clsCommon.myCstr(intCurrDate.Value.Day).Trim() + strSep
        ElseIf isMonthlyChange Then
            intNumPartLen = clsCommon.myCdbl(dt.Rows(0)("MinSizeofSeries"))
            strRetCode += IIf(intCurrMonth < 10, "0", "") + clsCommon.myCstr(intCurrMonth).Trim() + strSep
        End If
        Dim intLen As Integer = clsCommon.myLen(intCurrCounter) ''clsCommon.myLen(dt.Rows(0)("Next_Number"))
        For ii As Integer = 1 To intNumPartLen - intLen
            strRetCode += "0"
        Next
        strRetCode += clsCommon.myCstr(intCurrCounter)
        CheckForValidCounter(strRetCode, strDocType, strTransType, trans)
        'Throw New Exception(strRetCode)
        ''**********Increment Current Counter ************************

        If isIncreaseCounter Then
            intCurrCounter = intCurrCounter + 1
            qry = "update TSPL_DOCPREFIX_MASTER set Next_Number=" + clsCommon.myCstr(intCurrCounter) + ""
            qry += " where Fin_Year='" + clsCommon.myCstr(IntFiscalYear) + "' and Doc_Type='" + strDocType + "' and isnull(Doc_Trans_Type,'')='" + strTransType + "' and isnull(Location_Code,'')='" + strLocatinSegmentCode + "' "
            If isDailyChange Then
                qry += " and Curr_Date='" + clsCommon.GetPrintDate(intCurrDate, "dd/MMM/yyyy") + "'"
            ElseIf isMonthlyChange Then
                qry += " and Curr_Month='" + clsCommon.myCstr(intCurrMonth) + "'"
            End If
            clsDBFuncationality.ExecuteNonQuery(qry, trans)
        End If
        Return strRetCode
    End Function

    'Public Shared Function CheckForValidCounter(ByVal strRetCode As String, ByVal strDocType As String, ByVal strTransType As String, ByVal trans As SqlTransaction) As Boolean
    '    If clsCommon.CompairString(strDocType, clsDocType.SaleInvoice) = CompairStringResult.Equal AndAlso clsCommon.CompairString(strTransType, clsDocTransactionType.SaleInvoiceExcise) = CompairStringResult.Equal Then
    '        Dim qry As String = "select code from("
    '        qry += " select Sale_Invoice_No as Code from TSPL_SALE_INVOICE_HEAD"
    '        qry += " union all"
    '        qry += " select Transfer_No as Code from TSPL_TRANSFER_HEAD"
    '        qry += " union all "
    '        qry += " select Doc_No as Code from TSPL_IssueReturn_HEAD"
    '        qry += " union all "
    '        qry += " select shipment_No as Code from TSPL_SCRAPSALE_HEAD"
    '        qry += " union all "
    '        qry += " select invoice_No as Code from TSPL_SCRAPINVOICE_HEAD) xxx where code='" + strRetCode + "'"
    '        Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry, trans)
    '        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
    '            Throw New Exception("Auto Generated Code " + strRetCode + " is already in use.")
    '        End If
    '    End If
    '    Return True
    'End Function

    Private Shared Function GetQryOFDOCPrefix(ByVal dtDocDate As Date, ByVal strDocType As String, ByVal strTransType As String, ByVal strLocatinSegmentCode As String, ByVal IntFiscalYear As Integer, ByVal isTakingTopOne As Boolean) As String
        Dim qry As String = "select " + IIf(isTakingTopOne, " Top 1", "") + " Doc_Prfeix,Fin_Year,Next_Number,Separator,Is_Change_monthly,Curr_Month,Is_Change_Daily,Curr_Date,dontDisplayYearInSeries,MinSizeofSeries,Year_Separator from TSPL_DOCPREFIX_MASTER where  Doc_Type='" + strDocType + "' and  isnull(Doc_Trans_Type,'')='" + strTransType + "' and isnull(Location_Code,'')='" + strLocatinSegmentCode + "' and Fin_Year='" + clsCommon.myCstr(IntFiscalYear) + "'"
        If isTakingTopOne Then
            qry += " order by case when Is_Change_Daily=1 then  CONVERT(varchar, Curr_Date,112) else  CONVERT(varchar, Curr_Month ) end desc"
        Else
            qry += " and 2=(case when Is_Change_Daily=1 then case when Curr_Date= '" + clsCommon.GetPrintDate(dtDocDate, "dd/MMM/yyyy") + "'  then 2 else 3  end   else   case when Is_Change_monthly=1 then case when Curr_Month= " + clsCommon.myCstr(dtDocDate.Month) + "  then 2 else 3  end else 2 end end)"
        End If
        Return qry
    End Function


    Public Shared Function ChangeGLAccountLocationSegment(ByVal strAccountCode As String, ByVal strLocation As String) As String
        Return ChangeGLAccountLocationSegment(strAccountCode, strLocation, False, Nothing)
    End Function
    Public Shared Function ChangeGLAccountLocationSegment(ByVal strAccountCode As String, ByVal strLocation As String, ByVal trans As SqlTransaction) As String
        Return ChangeGLAccountLocationSegment(strAccountCode, strLocation, False, trans)
    End Function
    ''BM00000007648 add parameter isCheckForUserPermission for not check the Loc segment. 
    Public Shared Function ChangeGLAccountLocationSegment(ByVal strAccountCode As String, ByVal strLocation As String, ByVal isLocationLocationSegment As Boolean, ByVal trans As SqlTransaction) As String
        Return ChangeGLAccountLocationSegment(strAccountCode, strLocation, isLocationLocationSegment, True, trans)
    End Function
    Public Shared Function ChangeGLAccountLocationSegment(ByVal strAccountCode As String, ByVal strLocation As String, ByVal isLocationLocationSegment As Boolean, ByVal isCheckForUserPermission As Boolean, ByVal trans As SqlTransaction) As String
        If clsCommon.myLen(strAccountCode) > 0 Then
            Dim qry As String = ""
            Dim strLocatinSegment As String = ""
            If isLocationLocationSegment Then
                strLocatinSegment = strLocation
            Else
                qry = "select Loc_Segment_Code from TSPL_LOCATION_MASTER where Location_Code='" + strLocation + "'"
                strLocatinSegment = clsCommon.myCstr(clsDBFuncationality.getSingleValue(qry, trans))
            End If

            If clsCommon.myLen(strLocatinSegment) <= 0 Then
                Throw New Exception("Please set the Location Segment For location" + strLocation)
            End If
            If (strAccountCode.Length >= 3) Then
                'Dim strOldSegment = strAccountCode.Substring(strAccountCode.Length - 3, 3)
                'If (IsNumeric(strOldSegment)) Then
                '    Throw New Exception("GL Account should be with location segment.For GL Account" + strAccountCode)
                'End If
                'strAccountCode = strAccountCode.Replace(strOldSegment, strLocatinSegment)
                strAccountCode = strAccountCode.Substring(0, strAccountCode.Length - 3) + strLocatinSegment
                qry = "select 1 from TSPL_GL_ACCOUNTS where Account_Code='" + strAccountCode + "'"
                If clsCommon.myLen(objCommonVar.strCurrUserGLAccount) > 0 AndAlso isCheckForUserPermission Then
                    qry += " and TSPL_GL_ACCOUNTS.Account_Code in (" + objCommonVar.strCurrUserGLAccount + ")"
                End If
                Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry, trans)
                If dt Is Nothing OrElse dt.Rows.Count <= 0 Then
                    Throw New Exception("Account " + strAccountCode + " Not Exist or Available for Current User.")
                End If
            Else
                Throw New Exception("GL should be of segment location Type")
            End If
        End If
        Return strAccountCode
    End Function

    Public Shared Function ChangeGLAccountWithOutLOcSegment(ByVal strAccountCode As String, ByVal strLocation As String, ByVal isLocationLocationSegment As Boolean, ByVal trans As SqlTransaction) As String
        Dim qry As String = ""
        Dim strLocatinSegment As String = ""
        If isLocationLocationSegment Then
            strLocatinSegment = strLocation
        Else
            qry = "select Loc_Segment_Code from TSPL_LOCATION_MASTER where Location_Code='" + strLocation + "'"
            strLocatinSegment = clsCommon.myCstr(clsDBFuncationality.getSingleValue(qry, trans))
        End If

        strAccountCode = strAccountCode + "-" + strLocatinSegment
        Dim dt As DataTable = clsDBFuncationality.GetDataTable("select 1 from TSPL_GL_ACCOUNTS where Account_Code='" + strAccountCode + "'", trans)
        If dt Is Nothing OrElse dt.Rows.Count <= 0 Then
            Throw New Exception("Account Not Exist." + strAccountCode)
        End If
        'If clsCommon.myLen(strLocatinSegment) <= 0 Then
        '    Throw New Exception("Please set the Location Segment For location" + strLocation)
        'End If
        'If (strAccountCode.Length >= 3) Then
        '    Dim strOldSegment = strAccountCode.Substring(strAccountCode.Length - 3, 3)
        '    If (IsNumeric(strOldSegment)) Then
        '        Throw New Exception("GL Account should be with location segment.For GL Account" + strAccountCode)
        '    End If
        '    strAccountCode = strAccountCode.Replace(strOldSegment, strLocatinSegment)
        '    Dim dt As DataTable = clsDBFuncationality.GetDataTable("select 1 from TSPL_GL_ACCOUNTS where Account_Code='" + strAccountCode + "'", trans)
        '    If dt Is Nothing OrElse dt.Rows.Count <= 0 Then
        '        Throw New Exception("Account Not Exist." + strAccountCode)
        '    End If
        'Else
        '    Throw New Exception("GL should be of segment location Type")
        'End If
        Return strAccountCode
    End Function

    Public Shared Function GLGetAccountCode(ByVal strAccountCode As String, ByVal trans As SqlTransaction) As String
        Dim qry As String = ""
        strAccountCode = clsDBFuncationality.getSingleValue("select Account_Code from TSPL_GL_ACCOUNTS where Account_Code='" + strAccountCode + "'", trans)
        If clsCommon.myLen(strAccountCode) <= 0 Then
            Throw New Exception("Account Not Exist." + strAccountCode)
        End If
        Return strAccountCode
    End Function

    Public Shared Sub ValidateLocationSegment(ByVal CompCode As String, ByVal Modulee As String, ByVal transName As String, ByVal Location As String, ByVal DocDate As DateTime, ByVal trans As SqlTransaction)
        Dim Qry As String = ""
        Dim AllowLockTransactionUserwise As Integer = clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.AllowLockTransactionUserwise, clsFixedParameterCode.AllowLockTransactionUserwise, trans))

        Try
            If AllowLockTransactionUserwise = 0 Then
                Qry = "Select (CONVERT(varchar, Start_Date, 103) +'  To  '+ CONVERT(varchar, End_Date, 103)) as DateRange from TSPL_LOCK_LOCATION_SEGMENT Where Comp_Code='" + CompCode + "' AND Module_Name='" + Modulee + "' AND Trans_Name='" + transName + "' AND Location_Segment_Code='" + Location + "'"
                Qry += " AND Is_Locked='1' AND Convert(Date, Start_Date, 103)<=convert(Date, '" + clsCommon.GetPrintDate(DocDate, "dd/MMM/yyyy") + "', 103) AND Convert(Date, End_Date , 103)>=convert(Date, '" + clsCommon.GetPrintDate(DocDate, "dd/MMM/yyyy") + "', 103)"
                Dim DateRange As String = clsCommon.myCstr(clsDBFuncationality.getSingleValue(Qry, trans))
                If clsCommon.myLen(DateRange) > 0 Then
                    Throw New Exception("Transaction is Locked For Location '" + Location + "' from " + DateRange + "")
                End If
            Else
                Qry = "Select (CONVERT(varchar, Start_Date, 103) +'  To  '+ CONVERT(varchar, End_Date, 103)) as DateRange,'' as User_Code from TSPL_LOCK_LOCATION_SEGMENT Where " & _
                " TSPL_LOCK_LOCATION_SEGMENT.Comp_Code='" + CompCode + "' AND TSPL_LOCK_LOCATION_SEGMENT.Module_Name='" + Modulee + "' AND " & _
                "TSPL_LOCK_LOCATION_SEGMENT.Trans_Name='" + transName + "' AND TSPL_LOCK_LOCATION_SEGMENT.Location_Segment_Code='" + Location + "' " & _
                " AND TSPL_LOCK_LOCATION_SEGMENT.Is_Locked='1' AND Convert(Date, Start_Date, 103)<=convert(Date, '" + clsCommon.GetPrintDate(DocDate, "dd/MMM/yyyy") + "', 103) AND Convert(Date, End_Date , 103)>=convert(Date, '" + clsCommon.GetPrintDate(DocDate, "dd/MMM/yyyy") + "', 103) " & _
                "union all " & _
                "Select  (CONVERT(varchar, Start_Date, 103) +'  To  '+ CONVERT(varchar, End_Date, 103)) as DateRange,TSPL_LOCK_LOCATION_SEGMENT_USER.User_Code " & _
                "from TSPL_LOCK_LOCATION_SEGMENT  " & _
                "left outer join TSPL_LOCK_LOCATION_SEGMENT_USER on TSPL_LOCK_LOCATION_SEGMENT.Location_Segment_Code=TSPL_LOCK_LOCATION_SEGMENT_USER.Location_Segment_Code and " & _
                "TSPL_LOCK_LOCATION_SEGMENT.Module_Name=TSPL_LOCK_LOCATION_SEGMENT_USER.Module_Name and TSPL_LOCK_LOCATION_SEGMENT.Trans_Name=TSPL_LOCK_LOCATION_SEGMENT_USER.Trans_Name " & _
                "Where TSPL_LOCK_LOCATION_SEGMENT.Comp_Code='" + CompCode + "' AND TSPL_LOCK_LOCATION_SEGMENT.Module_Name='" + Modulee + "' AND " & _
                "TSPL_LOCK_LOCATION_SEGMENT.Trans_Name='" + transName + "' AND TSPL_LOCK_LOCATION_SEGMENT.Location_Segment_Code='" + Location + "' " & _
                " AND Is_Locked='1' AND Convert(Date, ToDate, 103)<convert(Date, '" + clsCommon.GetPrintDate(DocDate, "dd/MMM/yyyy") + "', 103)  And " & _
                "isnull(TSPL_LOCK_LOCATION_SEGMENT_USER.User_Code,'') = '" & objCommonVar.CurrentUserCode & "'"
                Dim strSql = "select DateRange,max(user_code) as  user_code from ( " & Qry & " ) a group by DateRange "
                Dim dt As DataTable = clsDBFuncationality.GetDataTable(strSql, trans)
                Dim DateRange As String = ""
                Dim strUser As String = ""
                If dt.Rows.Count > 0 Then
                    DateRange = clsCommon.myCstr(dt.Rows(0)("DateRange"))
                    strUser = clsCommon.myCstr(dt.Rows(0)("User_Code"))
                    If clsCommon.myLen(strUser) = 0 Then
                        Dim UserLockDate = clsDBFuncationality.getSingleValue("Select CONVERT(varchar, Todate, 103) from TSPL_LOCK_LOCATION_SEGMENT_USER  " & _
                "where TSPL_LOCK_LOCATION_SEGMENT_USER.Comp_Code='" + CompCode + "' AND TSPL_LOCK_LOCATION_SEGMENT_USER.Module_Name='" + Modulee + "' AND " & _
                "TSPL_LOCK_LOCATION_SEGMENT_USER.Trans_Name='" + transName + "' AND TSPL_LOCK_LOCATION_SEGMENT_USER.Location_Segment_Code='" + Location + "' " & _
                "AND Convert(Date, ToDate, 103)>convert(Date, '" + clsCommon.GetPrintDate(DocDate, "dd/MMM/yyyy") + "', 103)  And " & _
                "isnull(TSPL_LOCK_LOCATION_SEGMENT_USER.User_Code,'') = '" & objCommonVar.CurrentUserCode & "'", trans)
                        If clsCommon.myLen(UserLockDate) = 0 Then
                            Throw New Exception("Transaction is Locked For Location Segment '" + Location + "' from " + DateRange + "")
                        Else
                            Throw New Exception("Transaction is Locked For User '" + objCommonVar.CurrentUserCode + "'  Location Segment '" + Location + "' Till " + UserLockDate + "")
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try


        Try
            'Dim DateRange As String = clsCommon.myCstr(clsDBFuncationality.getSingleValue(Qry, trans))
            'If clsCommon.myLen(DateRange) > 0 Then
            '    Throw New Exception("Transaction is Locked For Location '" + Location + "' from " + DateRange + "")
            'End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Shared Sub ValidateLocationSegmentold(ByVal CompCode As String, ByVal Modulee As String, ByVal transName As String, ByVal Location As String, ByVal DocDate As String, ByVal trans As SqlTransaction)
        Dim Qry As String = "Select (CONVERT(varchar, Start_Date, 103) +'  To  '+ CONVERT(varchar, End_Date, 103)) as DateRange from TSPL_LOCK_LOCATION_SEGMENT Where Comp_Code='" + CompCode + "' AND Module_Name='" + Modulee + "' AND Trans_Name='" + transName + "' AND Location_Segment_Code='" + Location + "'"
        Qry += " AND Is_Locked='1' AND Convert(Date, Start_Date, 103)<=convert(Date, '" + DocDate + "', 103) AND Convert(Date, End_Date , 103)>=convert(Date, '" + DocDate + "', 103)"
        Try
            Dim DateRange As String = clsCommon.myCstr(clsDBFuncationality.getSingleValue(Qry, trans))
            If clsCommon.myLen(DateRange) > 0 Then
                Throw New Exception("Transaction is Locked For Location '" + Location + "' from " + DateRange + "")
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Shared Function GetLocationSegment(ByVal locationCode As String) As String
        Dim sql As String = "SELECT Loc_Segment_Code from TSPL_LOCATION_MASTER WHERE Location_Code='" + locationCode + "'"
        Return connectSql.RunScalar(sql).ToString()
    End Function

    Public Shared Function GetLocationSegment(ByVal locationCode As String, ByVal trans As SqlTransaction) As String
        Dim sql As String = "SELECT Loc_Segment_Code from TSPL_LOCATION_MASTER WHERE Location_Code='" + locationCode + "'"
        Return connectSql.RunScalar(trans, sql).ToString()
    End Function

    Public Shared Sub ValidateLocationCode(ByVal CompCode As String, ByVal Modulee As String, ByVal transName As String, ByVal Location As String, ByVal DocDate As DateTime, ByVal trans As SqlTransaction)
        Dim Qry As String = ""
        Dim AllowLockTransactionUserwise As Integer = clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.AllowLockTransactionUserwise, clsFixedParameterCode.AllowLockTransactionUserwise, trans))
        Try
            If AllowLockTransactionUserwise = 0 Then
                Qry = "Select (CONVERT(varchar, Start_Date, 103) +'  To  '+ CONVERT(varchar, End_Date, 103)) as DateRange from TSPL_LOCK_LOCATION Where TSPL_LOCK_LOCATION.Comp_Code='" + CompCode + "' AND TSPL_LOCK_LOCATION.Module_Name='" + Modulee + "' AND TSPL_LOCK_LOCATION.Trans_Name='" + transName + "' AND TSPL_LOCK_LOCATION.Location_Code='" + Location + "'"
                Qry += " AND TSPL_LOCK_LOCATION.Is_Locked='1' AND Convert(Date, Start_Date, 103)<=convert(Date, '" + clsCommon.GetPrintDate(DocDate, "dd/MMM/yyyy") + "', 103) AND Convert(Date, End_Date , 103)>=convert(Date, '" + clsCommon.GetPrintDate(DocDate, "dd/MMM/yyyy") + "', 103)"
                Dim DateRange As String = clsCommon.myCstr(clsDBFuncationality.getSingleValue(Qry, trans))
                If clsCommon.myLen(DateRange) > 0 Then
                    Throw New Exception("Transaction " + transName + "[" + Modulee + "] is Locked For Location '" + Location + "' from " + DateRange + "")
                End If
            Else
                Qry = "Select (CONVERT(varchar, Start_Date, 103) +'  To  '+ CONVERT(varchar, End_Date, 103)) as DateRange,'' as User_Code from TSPL_LOCK_LOCATION Where " & _
                " TSPL_LOCK_LOCATION.Comp_Code='" + CompCode + "' AND TSPL_LOCK_LOCATION.Module_Name='" + Modulee + "' AND " & _
                "TSPL_LOCK_LOCATION.Trans_Name='" + transName + "' AND TSPL_LOCK_LOCATION.Location_Code='" + Location + "' " & _
                " AND TSPL_LOCK_LOCATION.Is_Locked='1' AND Convert(Date, Start_Date, 103)<=convert(Date, '" + clsCommon.GetPrintDate(DocDate, "dd/MMM/yyyy") + "', 103) AND Convert(Date, End_Date , 103)>=convert(Date, '" + clsCommon.GetPrintDate(DocDate, "dd/MMM/yyyy") + "', 103) " & _
                "union all " & _
                "Select  (CONVERT(varchar, Start_Date, 103) +'  To  '+ CONVERT(varchar, End_Date, 103)) as DateRange,TSPL_LOCK_LOCATION_USER.User_Code " & _
                "from TSPL_LOCK_LOCATION  " & _
                "left outer join TSPL_LOCK_LOCATION_USER on TSPL_LOCK_LOCATION.Location_Code=TSPL_LOCK_LOCATION_USER.Location_Code and " & _
                "TSPL_LOCK_LOCATION.Module_Name=TSPL_LOCK_LOCATION_USER.Module_Name and TSPL_LOCK_LOCATION.Trans_Name=TSPL_LOCK_LOCATION_USER.Trans_Name " & _
                "Where TSPL_LOCK_LOCATION.Comp_Code='" + CompCode + "' AND TSPL_LOCK_LOCATION.Module_Name='" + Modulee + "' AND TSPL_LOCK_LOCATION.Trans_Name='" + transName + "' AND TSPL_LOCK_LOCATION.Location_Code='" + Location + "' " & _
                " AND Is_Locked='1' AND Convert(Date, ToDate, 103)<convert(Date, '" + clsCommon.GetPrintDate(DocDate, "dd/MMM/yyyy") + "', 103)  And " & _
                "isnull(TSPL_LOCK_LOCATION_USER.User_Code,'') = '" & objCommonVar.CurrentUserCode & "'"
                Dim strSql = "select DateRange,max(user_code) as  user_code from ( " & Qry & " ) a group by DateRange "
                Dim dt As DataTable = clsDBFuncationality.GetDataTable(strSql, trans)
                Dim DateRange As String = ""
                Dim strUser As String = ""
                If dt.Rows.Count > 0 Then
                    DateRange = clsCommon.myCstr(dt.Rows(0)("DateRange"))
                    strUser = clsCommon.myCstr(dt.Rows(0)("User_Code"))
                    If clsCommon.myLen(strUser) = 0 Then
                        Dim UserLockDate As String = ""
                        Qry = "Select CONVERT(varchar, Todate, 103) from TSPL_LOCK_LOCATION_USER  " & _
                                      "where TSPL_LOCK_LOCATION_USER.Comp_Code='" + CompCode + "' AND TSPL_LOCK_LOCATION_USER.Module_Name='" + Modulee + "' AND " & _
                                      "TSPL_LOCK_LOCATION_USER.Trans_Name='" + transName + "' AND TSPL_LOCK_LOCATION_USER.Location_Code='" + Location + "' " & _
                                      "AND Convert(Date, ToDate, 103)>convert(Date, '" + clsCommon.GetPrintDate(DocDate, "dd/MMM/yyyy") + "', 103)  And " & _
                                      "isnull(TSPL_LOCK_LOCATION_USER.User_Code,'') = '" & objCommonVar.CurrentUserCode & "'"
                        UserLockDate = clsDBFuncationality.getSingleValue(Qry, trans)
                        If clsCommon.myLen(UserLockDate) = 0 Then
                            Throw New Exception("Transaction " + transName + "[" + Modulee + "] is Locked For Location '" + Location + "' from " + DateRange + "")
                        Else
                            Throw New Exception("Transaction " + transName + "[" + Modulee + "] is Locked For User '" + objCommonVar.CurrentUserCode + "'  Location '" + Location + "' Till " + UserLockDate + "")
                        End If


                    End If
                End If
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Shared Sub ValidateLocationCodeold(ByVal CompCode As String, ByVal Modulee As String, ByVal transName As String, ByVal Location As String, ByVal DocDate As String, ByVal trans As SqlTransaction)
        Dim Qry As String = "Select (CONVERT(varchar, Start_Date, 103) +'  To  '+ CONVERT(varchar, End_Date, 103)) as DateRange from TSPL_LOCK_LOCATION Where Comp_Code='" + CompCode + "' AND Module_Name='" + Modulee + "' AND Trans_Name='" + transName + "' AND Location_Code='" + Location + "'"
        Qry += " AND Is_Locked='1' AND Convert(Date, Start_Date, 103)<=convert(Date, '" + DocDate + "', 103) AND Convert(Date, End_Date , 103)>=convert(Date, '" + DocDate + "', 103)"
        Try
            Dim DateRange As String = clsCommon.myCstr(clsDBFuncationality.getSingleValue(Qry, trans))
            If clsCommon.myLen(DateRange) > 0 Then
                Throw New Exception("Transaction is Locked For Location '" + Location + "' from " + DateRange + "")
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Shared Function closeForm(ByRef f As Object) As Boolean
        f.close()
        f.dispose()
        GC.Collect()
        GC.WaitForPendingFinalizers()
        Return True
    End Function

    Public Shared Function IsDocumentAlreadyPosted(TableName As String, DocumentNoColumnName As String, DocumentNo As String, WhrClsForPostingStatusCheck As String, trans As SqlTransaction) As Boolean   ', PostingStatusColumnName As String, PostingColumnNature As PostingColumnType, ValueWhenPostedInPostingStatusColumn As PostingStatusValueList,
        Dim rValue As Boolean = False
        Dim chk As Integer = 0
        Dim qry As String = ""
        Dim qry1 As String = ""
        Try
            If clsCommon.myLen(TableName) <= 0 Then
                Throw New Exception("Table Name Found Missing When Checking Doucment Posting Status")
            End If

            If clsCommon.myLen(DocumentNoColumnName) <= 0 Then
                Throw New Exception("DocumentNo Column Name Found Missing When Checking Doucment Posting Status")
            End If

            If clsCommon.myLen(DocumentNo) <= 0 Then
                Throw New Exception("DocumentNo  Value  Found Blank When Checking Doucment Posting Status")
            End If

            'If clsCommon.myLen(PostingStatusColumnName) <= 0 Then
            '    Throw New Exception("Posting Status Column Name Found Blank When Checking Doucment Posting Status")
            'End If

            'If PostingColumnNature = PostingColumnType.TEXT Then
            '    If ValueWhenPostedInPostingStatusColumn = PostingStatusValueList.Y OrElse ValueWhenPostedInPostingStatusColumn = PostingStatusValueList.Y Then

            '    Else
            '        Throw New Exception("Posting Status Column value Must be Y or Yes when it is of Text Nature")
            '    End If
            'ElseIf PostingColumnNature = PostingColumnType.NUMBER Then
            '    If ValueWhenPostedInPostingStatusColumn = PostingStatusValueList.ONE Then

            '    Else
            '        Throw New Exception("Posting Status Column value Must be ONE when it is of Number Nature")
            '    End If
            'Else
            '    Throw New Exception("Posting Status Column Nature Found Other than Text and Number When Checking Doucment Posting Status")
            'End If

            qry = "select COUNT(*) from " & TableName & " where " & DocumentNoColumnName & " ='" & DocumentNo & "' " & IIf(clsCommon.myLen(WhrClsForPostingStatusCheck) > 0, " and ", "") & WhrClsForPostingStatusCheck
            'If PostingColumnNature = PostingColumnType.NUMBER Then
            '    qry1 = " AND isnull(" & PostingStatusColumnName & ",0)='" & IIf(ValueWhenPostedInPostingStatusColumn = PostingStatusValueList.ONE, 1, 0) & "'"
            'Else
            '    qry1 = " AND isnull(" & PostingStatusColumnName & ",'')='" & IIf(ValueWhenPostedInPostingStatusColumn = PostingStatusValueList.Y, "Y", "YES") & "'"
            'End If
            'qry = qry & qry1
            chk = clsCommon.myCdbl(clsDBFuncationality.getSingleValue(qry, trans))
            If chk = 1 Then
                Throw New Exception("Doument is Already Posted, Please Reload the Doucment")
                rValue = True
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return rValue
    End Function



    Public Shared Sub ShowAlert(ByVal message As String, Optional ByVal Caption As String = "A new message for you", Optional ByVal isAutoClose As Boolean = False, Optional ByVal alertPosition As AlertScreenPosition = AlertScreenPosition.BottomRight)
        Dim rAlert As RadDesktopAlert = New RadDesktopAlert()
        rAlert.AutoClose = isAutoClose
        rAlert.CaptionText = Caption
        rAlert.ContentText = message
        rAlert.CanMove = True
        rAlert.ScreenPosition = alertPosition
        rAlert.ShowCloseButton = True
        rAlert.ShowPinButton = True
        rAlert.Show()
    End Sub
    Public Shared Function ValidationGSTNO(ByVal StateCode As String, ByVal PanNo As String, ByVal GSTNO As String, ByVal trans As SqlTransaction) As Boolean
        Dim msg As String = ""
        Dim TwoDigitForStateCode As String
        Dim TenDigitForPANNO As String
        'Dim EntityNo As String

        Try
            If clsCommon.myLen(GSTNO) <> 15 Then
                Throw New Exception("Length of GST must be 15 Character.")
                Return False
            End If



            If Not (System.Text.RegularExpressions.Regex.IsMatch(clsCommon.myCstr(GSTNO), "^[a-zA-Z0-9]+$")) Then
                Throw New Exception("GST Number must be alphanumeric.")
                Return False
            End If

            TwoDigitForStateCode = GSTNO.Trim().Substring(0, 2)

            If clsCommon.myLen(StateCode) <> 2 Or StateCode <> TwoDigitForStateCode Or Not IsNumeric(TwoDigitForStateCode) Then
                Throw New Exception("State code must be numeric (First two place).")
                Return False
            End If

            TenDigitForPANNO = GSTNO.Trim().Substring(2, 10)

            If clsCommon.myLen(PanNo) <> 10 Or PanNo <> TenDigitForPANNO Then
                Throw New Exception("Wrong PAN No.")
                Return False
            End If


        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return False
        End Try
        Return True
    End Function
    Public Shared Function CheckPanStructure(ByVal PanValue As String, ByVal PersonName As String) As String
        Dim msg As String = ""
        msg = CheckPanStructure(PanValue, PersonName, Nothing)
        Return msg
    End Function

    Public Shared Function CheckPanStructure(ByVal PanValue As String, ByVal PersonName As String, ByVal trans As SqlTransaction) As String
        Dim msg As String = ""
        Try
            Dim IsValidateCustomerPANwithName As Boolean = True
            IsValidateCustomerPANwithName = IIf(clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.ValidateCustomerPANwithName, clsFixedParameterCode.ValidateCustomerPANwithName, trans)) = 1, True, False)
            PersonName = clsCommon.myCstr(PersonName) '' used to trim person name is space is comming
            Dim checkPan As New System.Text.RegularExpressions.Regex("^([A-Z]){5}([0-9]){4}([A-Z]){1}?$")
            Dim fourth_char As String = ""
            Dim Fifth_Char As String = ""
            Dim First_Char As String = ""
            Dim NameSplit() As String = Nothing

            If clsCommon.myLen(PanValue.Trim()) >= 10 Then
                If checkPan.IsMatch(PanValue.Trim()) Then '=when pan contains first 5 Characters followed by 4digits and 1 character then check further validation.
                    fourth_char = clsCommon.myCstr(PanValue).Trim().Substring(3, 1)
                    Fifth_Char = clsCommon.myCstr(PanValue).Trim().Substring(4, 1)

                    If clsCommon.CompairString(fourth_char, "C") <> CompairStringResult.Equal AndAlso clsCommon.CompairString(fourth_char, "P") <> CompairStringResult.Equal AndAlso clsCommon.CompairString(fourth_char, "H") <> CompairStringResult.Equal AndAlso clsCommon.CompairString(fourth_char, "F") <> CompairStringResult.Equal AndAlso clsCommon.CompairString(fourth_char, "A") <> CompairStringResult.Equal AndAlso clsCommon.CompairString(fourth_char, "T") <> CompairStringResult.Equal AndAlso clsCommon.CompairString(fourth_char, "B") <> CompairStringResult.Equal AndAlso clsCommon.CompairString(fourth_char, "L") <> CompairStringResult.Equal AndAlso clsCommon.CompairString(fourth_char, "J") <> CompairStringResult.Equal AndAlso clsCommon.CompairString(fourth_char, "G") <> CompairStringResult.Equal Then
                        msg = "4th character of PAN number should be in (C,P,H,F,A,T,B,L,J,G)."
                    End If

                    If clsCommon.myLen(msg) <= 0 Then
                        NameSplit = PersonName.Split(" ")
                        Dim counter As Integer = 0
                        If NameSplit IsNot Nothing AndAlso NameSplit.Length > 0 Then
                            For ii As Integer = 0 To NameSplit.Length - 1
                                First_Char = clsCommon.myCstr(NameSplit(ii)).Trim().Substring(0, 1)
                                If clsCommon.CompairString(Fifth_Char, First_Char) = CompairStringResult.Equal Then
                                    counter += 1
                                End If
                            Next
                        End If
                        If IsValidateCustomerPANwithName Then
                            If counter <= 0 Then
                                msg = "5th word of PAN number should be 1st character of Assessee's Last Name/Surname."
                            End If
                        End If
                    End If
                Else
                    msg = "PAN numbers should have 5 characters followed by 4 numbers then a final character" + Environment.NewLine + "4th character should be in (C,P,H,F,A,T,B,L,J,G) "
                    If IsValidateCustomerPANwithName Then
                        msg += " and 5th should be 1st character of Assessee's Last Name/Surname."
                    End If
                End If
            Else
                msg = ""
                If clsCommon.myLen(PanValue.Trim()) = 5 Then
                    Dim checkPan1 As New System.Text.RegularExpressions.Regex("^([A-Z]){5}?$")
                    If Not checkPan1.IsMatch(PanValue.Trim()) Then
                        msg = "PAN numbers should have 5 characters."
                    End If
                End If
                If clsCommon.myLen(PanValue.Trim()) >= 4 Then
                    fourth_char = clsCommon.myCstr(PanValue).Trim().Substring(3, 1)
                    If clsCommon.CompairString(fourth_char, "C") <> CompairStringResult.Equal AndAlso clsCommon.CompairString(fourth_char, "P") <> CompairStringResult.Equal AndAlso clsCommon.CompairString(fourth_char, "H") <> CompairStringResult.Equal AndAlso clsCommon.CompairString(fourth_char, "F") <> CompairStringResult.Equal AndAlso clsCommon.CompairString(fourth_char, "A") <> CompairStringResult.Equal AndAlso clsCommon.CompairString(fourth_char, "T") <> CompairStringResult.Equal AndAlso clsCommon.CompairString(fourth_char, "B") <> CompairStringResult.Equal AndAlso clsCommon.CompairString(fourth_char, "L") <> CompairStringResult.Equal AndAlso clsCommon.CompairString(fourth_char, "J") <> CompairStringResult.Equal AndAlso clsCommon.CompairString(fourth_char, "G") <> CompairStringResult.Equal Then
                        msg = "4th character of PAN number should be in (C,P,H,F,A,T,B,L,J,G)."
                    End If
                End If
                If clsCommon.myLen(PanValue.Trim()) >= 5 Then
                    Fifth_Char = clsCommon.myCstr(PanValue).Trim().Substring(4, 1)

                    NameSplit = PersonName.Split(" ")
                    Dim counter As Integer = 0
                    If NameSplit IsNot Nothing AndAlso NameSplit.Length > 0 Then
                        For ii As Integer = 0 To NameSplit.Length - 1
                            First_Char = clsCommon.myCstr(NameSplit(ii)).Trim().Substring(0, 1)
                            If clsCommon.CompairString(Fifth_Char, First_Char) = CompairStringResult.Equal Then
                                counter += 1
                            End If
                        Next
                    End If
                    If IsValidateCustomerPANwithName Then
                        If counter <= 0 Then
                            msg = "5th word of PAN number should be 1st character of Assessee's Last Name/Surname."
                        End If
                    End If
                End If
                If clsCommon.myLen(PanValue.Trim()) = 9 Then
                    Dim checkPan1 As New System.Text.RegularExpressions.Regex("^([A-Z]){5}([0-9]){4}?$")
                    If Not checkPan1.IsMatch(PanValue.Trim()) Then
                        msg = "PAN numbers should have 5 characters followed by 4 numbers."
                    End If
                End If

            End If
        Catch ex As Exception
            clsCommon.MyMessageBoxShow(ex.Message)
        End Try

        Return msg
    End Function

    Public Shared Function CompanyAddresShowinFooter() As DataTable
        Return clsDBFuncationality.GetDataTable("select TSPL_COMPANY_MASTER.add1 +case when len(TSPL_COMPANY_MASTER.add2)>0 then ', '+TSPL_COMPANY_MASTER.add2 else '' end +case when LEN(isnull(TSPL_COMPANY_MASTER.Add3,''))>0 then ', '+isnull(TSPL_COMPANY_MASTER.Add3,'') else '' end + case when LEN(TSPL_COMPANY_MASTER.City_Code  )>0 then ', '+TSPL_COMPANY_MASTER.City_Code  else '' end + case when len(TSPL_STATE_MASTER.STATE_NAME  )>0 then ', '+TSPL_STATE_MASTER.STATE_NAME  else '' end + case when LEN(TSPL_COMPANY_MASTER.Pincode)>0 then ' - '+TSPL_COMPANY_MASTER.Pincode else '' end as companyaddress,TSPL_COMPANY_MASTER.CINNo  as cin,TSPL_COMPANY_MASTER.Pan_No as pan,TSPL_COMPANY_MASTER.Logo_Img from TSPL_COMPANY_MASTER  left outer join TSPL_STATE_MASTER on TSPL_STATE_MASTER.STATE_CODE =TSPL_COMPANY_MASTER.State")
    End Function
    Public Shared Function CompanyAddresShowinFooterForJakson() As DataTable
        Return clsDBFuncationality.GetDataTable("select TSPL_COMPANY_MASTER.add1 +case when len(TSPL_COMPANY_MASTER.add2)>0 then ', '+TSPL_COMPANY_MASTER.add2 else '' end +case when LEN(isnull(TSPL_COMPANY_MASTER.Add3,''))>0 then ', '+isnull(TSPL_COMPANY_MASTER.Add3,'') else '' end + case when LEN(TSPL_COMPANY_MASTER.City_Code  )>0 then ', '+TSPL_COMPANY_MASTER.City_Code  else '' end + case when len(TSPL_STATE_MASTER.STATE_NAME  )>0 then ', '+TSPL_STATE_MASTER.STATE_NAME  else '' end + case when LEN(TSPL_COMPANY_MASTER.Pincode)>0 then ' - '+TSPL_COMPANY_MASTER.Pincode else '' end +case when LEN(TSPL_COMPANY_MASTER.Fax )>0 then ' ,Fax - '+TSPL_COMPANY_MASTER.Fax else '' end+ Case when len(ISNULL(TSPL_COMPANY_MASTER.Phone1,''))>0 and TSPL_COMPANY_MASTER.Phone1='(+__)__________' then '' else ' ,Phone'+TSPL_COMPANY_MASTER.Phone1 end +Case When   ISNULL(TSPL_COMPANY_MASTER.Phone2,'')<>'(+__)__________' Then '  '+ TSPL_COMPANY_MASTER.Phone2 Else'' end +case when LEN(TSPL_COMPANY_MASTER.Email  )>0 then ' ,Email- '+TSPL_COMPANY_MASTER.Email  else '' end+case when LEN(TSPL_COMPANY_MASTER.Pan_No   )>0 then ' ,PAN- '+TSPL_COMPANY_MASTER.Pan_No  else '' end as companyaddress,TSPL_COMPANY_MASTER.CINNo  as cin,TSPL_COMPANY_MASTER.Pan_No as pan,TSPL_COMPANY_MASTER.Logo_Img from TSPL_COMPANY_MASTER  left outer join TSPL_STATE_MASTER on TSPL_STATE_MASTER.STATE_CODE =TSPL_COMPANY_MASTER.State")
    End Function
    '===shivani
    Public Shared Function CompanyAddresShowinHeader() As DataTable
        Return clsDBFuncationality.GetDataTable("select Comp_Name ,TSPL_COMPANY_MASTER.Logo_Img ,TSPL_COMPANY_MASTER.Logo_Img2  ,TSPL_COMPANY_MASTER.add1 +case when len(TSPL_COMPANY_MASTER.add2)>0 then ', '+TSPL_COMPANY_MASTER.add2 else '' end +case when LEN(isnull(TSPL_COMPANY_MASTER.Add3,''))>0 then ', '+isnull(TSPL_COMPANY_MASTER.Add3,'') else ' '  end  as Loc_Add from TSPL_COMPANY_MASTER ")
    End Function
    Public Shared Function CompanyAddresInvoiceHeader() As DataTable
        Return clsDBFuncationality.GetDataTable("select TSPL_COMPANY_MASTER.Logo_Img ,TSPL_COMPANY_MASTER.Logo_Img2 from TSPL_COMPANY_MASTER  ")
    End Function

    Public Shared Function isValueInUse(ByVal value As String, ByVal tableName As String, ByVal fieldName As String) As Boolean
        Dim rValue As Boolean = False
        Dim tblname As String = ""
        Dim fldname As String = ""
        Dim cnt As Integer = 0
        Dim qry As String = "SELECT R.TABLE_NAME,R.COLUMN_NAME FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE U INNER JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS FK     ON U.CONSTRAINT_CATALOG = FK.UNIQUE_CONSTRAINT_CATALOG     AND U.CONSTRAINT_SCHEMA = FK.UNIQUE_CONSTRAINT_SCHEMA     AND U.CONSTRAINT_NAME = FK.UNIQUE_CONSTRAINT_NAME  INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE R    ON R.CONSTRAINT_CATALOG = FK.CONSTRAINT_CATALOG     AND R.CONSTRAINT_SCHEMA = FK.CONSTRAINT_SCHEMA     AND R.CONSTRAINT_NAME = FK.CONSTRAINT_NAME WHERE U.COLUMN_NAME = '" & fieldName & "'   AND U.TABLE_NAME = '" & tableName & "'    "
        Dim dtbl As DataTable = clsDBFuncationality.GetDataTable(qry)
        If dtbl IsNot Nothing AndAlso dtbl.Rows.Count > 0 Then
            qry = ""
            For i As Integer = 0 To dtbl.Rows.Count - 1
                tblname = clsCommon.myCstr(dtbl.Rows(i)("TABLE_NAME"))
                fldname = clsCommon.myCstr(dtbl.Rows(i)("COLUMN_NAME"))
                qry = qry & " select " & fldname & " from " & tblname & " where " & fldname & " ='" & value & "'"
                If i <> (dtbl.Rows.Count - 1) Then
                    qry = qry & Environment.NewLine & " UNION ALL " & Environment.NewLine
                End If
            Next
            Dim dtbl1 As DataTable = clsDBFuncationality.GetDataTable("select *  from (" & qry & " ) as x")
            If dtbl1.Rows.Count > 0 Then
                rValue = True
            Else
                rValue = False
            End If
        End If
        Return rValue
    End Function

    '' Anubhooti Perfix For Vendor 01-Sep-2014 -------------------------------------------------
    Private Shared Function GetVendorQryOFDOCPrefix(ByVal dtDocDate As Date, ByVal strDocType As String, ByVal strTransType As String, ByVal strLocatinSegmentCode As String, ByVal IntFiscalYear As Integer, ByVal isTakingTopOne As Boolean) As String
        Dim qry As String = "select " + IIf(isTakingTopOne, " Top 1", "") + " Doc_Prfeix,Fin_Year,Next_Number,Separator,Is_Change_monthly,Curr_Month,Is_Change_Daily,Curr_Date,dontDisplayYearInSeries,MinSizeofSeries from TSPL_DOCPREFIX_MASTER where  Doc_Type='" + strDocType + "' and  isnull(Doc_Trans_Type,'')='" + strTransType + "' and isnull(Location_Code,'')='" + strLocatinSegmentCode + "' and Fin_Year='" + clsCommon.myCstr(IntFiscalYear) + "'"
        If isTakingTopOne Then
            qry += " order by case when Is_Change_Daily=1 then  CONVERT(varchar, Curr_Date,112) else  CONVERT(varchar, Curr_Month ) end desc"
        Else
            qry += " and 2=(case when Is_Change_Daily=1 then case when Curr_Date= '" + clsCommon.GetPrintDate(dtDocDate, "dd/MMM/yyyy") + "'  then 2 else 3  end   else   case when Is_Change_monthly=1 then case when Curr_Month= " + clsCommon.myCstr(dtDocDate.Month) + "  then 2 else 3  end else 2 end end)"
        End If
        Return qry
    End Function

    Public Shared Function GetVendorNextCode(ByVal TableName As String, ByVal FieldName As String, ByVal StrVenName As String, ByVal trans As SqlTransaction) As String

        If clsCommon.myLen(StrVenName) <= 0 Then
            Throw New Exception("Please enter Description")
        End If
        StrVenName = StrVenName.Substring(0, 1)
        Dim qry As String = ""
        Dim DigitLen As String = ""
        Dim Digits As Double
        Dim strRetCode As String = ""

        Dim strLocatinSegmentCode As String = ""
        ' Dim dt As DataTable
        If clsCommon.myLen(StrVenName) > 0 Then
            ' Dim dt1 As DataTable
            Dim qry1 As String = "Select COUNT(*) AS Row From " + TableName + "  Where " + FieldName & " like '" + StrVenName + "%'"
            Dim VNameSeries As Double = clsCommon.myCdbl(clsDBFuncationality.getSingleValue(qry1, trans))
            If clsCommon.CompairString(TableName, "TSPL_VENDOR_MASTER") = CompairStringResult.Equal Then
                Digits = clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.AutoGeneratedDigitsForVendor, clsFixedParameterCode.AutoGeneratedDigitsForVendor, trans))
            ElseIf clsCommon.CompairString(TableName, "TSPL_CUSTOMER_MASTER") = CompairStringResult.Equal Then
                Digits = clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.AutoGeneratedDigitsForCustomer, clsFixedParameterCode.AutoGeneratedDigitsForCustomer, trans))
            End If
            Digits -= clsCommon.myLen(VNameSeries)
            If clsCommon.myLen(Digits) > 0 Then
                For dig As Integer = 1 To Digits
                    DigitLen += "0"
                Next
            End If

            If VNameSeries = 0 Then
                VNameSeries = 1
            Else
                VNameSeries = 1 + VNameSeries
            End If

            strRetCode = StrVenName.ToUpper() + DigitLen + clsCommon.myCstr(VNameSeries)
            Dim dt As DataTable = Nothing
            Dim blCondition As Boolean = True
            While blCondition
                If clsCommon.CompairString(TableName, "TSPL_VENDOR_MASTER") = CompairStringResult.Equal Then
                    dt = clsDBFuncationality.GetDataTable("Select 1  From tspl_vendor_master where vendor_code='" + strRetCode + "'", trans)
                ElseIf clsCommon.CompairString(TableName, "TSPL_CUSTOMER_MASTER") = CompairStringResult.Equal Then
                    dt = clsDBFuncationality.GetDataTable("Select 1  From TSPL_CUSTOMER_MASTER where Cust_Code='" + strRetCode + "'", trans)
                End If

                If dt Is Nothing OrElse dt.Rows.Count <= 0 Then
                    blCondition = False
                Else
                    blCondition = True
                    strRetCode = clsCommon.incval(strRetCode)
                End If
            End While

        End If
        Return strRetCode
    End Function

    Public Shared Function DemoGetNextCode(ByVal trans As SqlTransaction, ByVal dtDocDate As Date, ByVal strDocType As String, ByVal strTransType As String, ByVal strLocationCode As String, ByVal strDatabase As String) As String
        Return DemoGetNextCode(trans, dtDocDate, strDocType, strTransType, strLocationCode, False, True, strDatabase)
    End Function


    Public Shared Function DemoGetNextCode(ByVal trans As SqlTransaction, ByVal dtDocDate As Date, ByVal strDocType As String, ByVal strTransType As String, ByVal strLocationCode As String, ByVal isLocationCodeisSegment As Boolean, ByVal isIncreaseCounter As Boolean, ByVal strDatabase As String) As String
        Dim qry As String = ""
        Dim strRetCode As String = ""
        Dim strLocatinSegmentCode As String = ""
        Dim dt As DataTable
        If isLocationCodeisSegment Then
            qry = "SELECT 1 from " + strDatabase + ".dbo.TSPL_GL_SEGMENT_CODE where Seg_No='7' and Segment_code='" + strLocationCode + "'"
            dt = clsDBFuncationality.GetDataTable(qry, trans)
            If dt Is Nothing OrElse dt.Rows.Count <= 0 Then
                Throw New Exception(strLocationCode + " is not a Location Segment")
            End If
            strLocatinSegmentCode = strLocationCode
        Else
            If clsCommon.myLen(strLocationCode) > 0 Then
                strLocatinSegmentCode = clsCommon.myCstr(clsDBFuncationality.getSingleValue("SELECT Loc_Segment_Code from TSPL_LOCATION_MASTER WHERE Location_Code='" + strLocationCode + "'", trans))
                If clsCommon.myLen(strLocatinSegmentCode) <= 0 Then
                    Throw New Exception("Location Segment code Not found for Location :" + strLocationCode)
                End If
            End If
        End If

        Dim IntFiscalYear As Integer = dtDocDate.Year
        If dtDocDate.Month >= 1 AndAlso dtDocDate.Month <= 3 Then
            IntFiscalYear -= 1
        End If


        qry = DemoGetQryOFDOCPrefix(dtDocDate, strDocType, strTransType, strLocatinSegmentCode, IntFiscalYear, False, strDatabase)
        dt = clsDBFuncationality.GetDataTable(qry, trans)
        If dt Is Nothing OrElse dt.Rows.Count <= 0 Then
            qry = DemoGetQryOFDOCPrefix(dtDocDate.AddMonths(-1), strDocType, strTransType, strLocatinSegmentCode, IntFiscalYear, True, strDatabase)
            dt = clsDBFuncationality.GetDataTable(qry, trans)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 AndAlso (clsCommon.myCdbl(dt.Rows(0)("Is_Change_monthly")) = 1 OrElse clsCommon.myCdbl(dt.Rows(0)("Is_Change_Daily")) = 1) Then
                Dim coll As New Hashtable()
                clsCommon.AddColumnsForChange(coll, "Doc_Type", strDocType)
                clsCommon.AddColumnsForChange(coll, "Doc_Trans_Type", strTransType)
                clsCommon.AddColumnsForChange(coll, "Location_Code", strLocatinSegmentCode)
                clsCommon.AddColumnsForChange(coll, "Doc_Prfeix", clsCommon.myCstr(dt.Rows(0)("Doc_Prfeix")))
                clsCommon.AddColumnsForChange(coll, "Fin_Year", IntFiscalYear)
                clsCommon.AddColumnsForChange(coll, "Next_Number", 1)
                clsCommon.AddColumnsForChange(coll, "Separator", clsCommon.myCstr(dt.Rows(0)("Separator")))
                clsCommon.AddColumnsForChange(coll, "Comp_Code", objCommonVar.CurrentCompanyCode)
                clsCommon.AddColumnsForChange(coll, "Created_By", objCommonVar.CurrentUserCode)
                clsCommon.AddColumnsForChange(coll, "Created_Date", clsCommon.GetPrintDate(clsCommon.GETSERVERDATE(trans), "dd/MMM/yyyy"))
                clsCommon.AddColumnsForChange(coll, "Modify_By", objCommonVar.CurrentUserCode)
                clsCommon.AddColumnsForChange(coll, "Modify_Date", clsCommon.GetPrintDate(clsCommon.GETSERVERDATE(trans), "dd/MMM/yyyy"))

                clsCommon.AddColumnsForChange(coll, "Is_Change_Monthly", clsCommon.myCdbl(dt.Rows(0)("Is_Change_monthly")))
                If clsCommon.myCdbl(dt.Rows(0)("Is_Change_monthly")) = 1 Then
                    clsCommon.AddColumnsForChange(coll, "Curr_Month", dtDocDate.Month)
                End If

                clsCommon.AddColumnsForChange(coll, "Is_Change_Daily", clsCommon.myCdbl(dt.Rows(0)("Is_Change_Daily")))
                If clsCommon.myCdbl(dt.Rows(0)("Is_Change_Daily")) = 1 Then
                    clsCommon.AddColumnsForChange(coll, "Curr_Date", clsCommon.GetPrintDate(dtDocDate, "dd/MMM/yyyy"))
                End If
                clsCommon.AddColumnsForChange(coll, "dontDisplayYearInSeries", dt.Rows(0)("dontDisplayYearInSeries"))
                clsCommon.AddColumnsForChange(coll, "MinSizeofSeries", dt.Rows(0)("MinSizeofSeries"))
                clsCommonFunctionality.UpdateDataTable(coll, strDatabase + ".dbo.TSPL_DOCPREFIX_MASTER", OMInsertOrUpdate.Insert, "", trans)

                qry = DemoGetQryOFDOCPrefix(dtDocDate, strDocType, strTransType, strLocatinSegmentCode, IntFiscalYear, False, strDatabase)
                dt = clsDBFuncationality.GetDataTable(qry, trans)
            End If
        End If

        If dt Is Nothing OrElse dt.Rows.Count <= 0 Then
            Dim strException As String = "Please ask your Administrator to Set the Counter for " + strDocType
            If clsCommon.myLen(strLocatinSegmentCode) > 0 Then
                strException += Environment.NewLine + "Segment Location - " + strLocatinSegmentCode
            End If
            If clsCommon.myLen(strTransType) > 0 Then
                strException += Environment.NewLine + "Transaction Type - " + strTransType
            End If
            strException += Environment.NewLine + "For Fiscal year - " + clsCommon.myCstr(IntFiscalYear)
            Throw New Exception(strException)
        End If
        ''**********Generate Counter ************************
        Dim intCurrCounter As Integer = Convert.ToInt32(clsCommon.myCdbl(dt.Rows(0)("Next_Number")))
        Dim isDailyChange As Boolean = IIf(clsCommon.myCdbl(dt.Rows(0)("Is_Change_Daily")) = 1, True, False)
        Dim intCurrDate As Date? = Nothing
        If isDailyChange Then
            intCurrDate = clsCommon.myCDate(dt.Rows(0)("Curr_Date"))
        End If



        Dim isMonthlyChange As Boolean = IIf(clsCommon.myCdbl(dt.Rows(0)("Is_Change_monthly")) = 1, True, False)
        Dim intCurrMonth As Integer = Convert.ToInt32(clsCommon.myCdbl(dt.Rows(0)("Curr_Month")))
        Dim strSep = clsCommon.myCstr(dt.Rows(0)("Separator")).Trim()

        Dim strFinYear As String = clsCommon.myCstr(dtDocDate.Year).Trim() 'clsCommon.myCstr(dt.Rows(0)("Fin_Year")).Trim()
        strFinYear = strFinYear.Substring(clsCommon.myLen(strFinYear) - 2, 2)
        qry = "select Description from " + strDatabase + ".dbo.TSPL_FIXED_PARAMETER where Type='" + clsFixedParameterType.CounterFinancialYearStyle + "' and Code='" + clsFixedParameterCode.CounterFinancialYearStyle + "'"
        If (clsCommon.myCdbl(clsDBFuncationality.getSingleValue(qry, trans)) = 1) Then
            Dim intYear As Integer = clsCommon.myCdbl(strFinYear)
            If dtDocDate.Month >= 1 AndAlso dtDocDate.Month <= 3 Then
                strFinYear = clsCommon.myCstr(intYear - 1) + "-" + clsCommon.myCstr(intYear)
            Else
                strFinYear = clsCommon.myCstr(intYear) + "-" + clsCommon.myCstr(intYear + 1)
            End If
        End If


        'strRetCode = clsCommon.myCstr(dt.Rows(0)("Doc_Prfeix")).Trim() + strSep + strFinYear + strSep
        If clsCommon.myCBool(dt.Rows(0)("dontDisplayYearInSeries")) = False Then
            strRetCode = clsCommon.myCstr(dt.Rows(0)("Doc_Prfeix")).Trim() + strSep + strFinYear + strSep
        Else
            strRetCode = clsCommon.myCstr(dt.Rows(0)("Doc_Prfeix")).Trim() + strSep
        End If
        Dim intNumPartLen As Integer = clsCommon.myCdbl(dt.Rows(0)("MinSizeofSeries"))
        If isDailyChange Then
            intNumPartLen = clsCommon.myCdbl(dt.Rows(0)("MinSizeofSeries"))
            strRetCode += IIf(intCurrDate.Value.Month < 10, "0", "") + clsCommon.myCstr(intCurrDate.Value.Month).Trim() + strSep + IIf(intCurrDate.Value.Day < 10, "0", "") + clsCommon.myCstr(intCurrDate.Value.Day).Trim() + strSep
        ElseIf isMonthlyChange Then
            intNumPartLen = clsCommon.myCdbl(dt.Rows(0)("MinSizeofSeries"))
            strRetCode += IIf(intCurrMonth < 10, "0", "") + clsCommon.myCstr(intCurrMonth).Trim() + strSep
        End If
        Dim intLen As Integer = clsCommon.myLen(intCurrCounter) ''clsCommon.myLen(dt.Rows(0)("Next_Number"))
        For ii As Integer = 1 To intNumPartLen - intLen
            strRetCode += "0"
        Next
        strRetCode += clsCommon.myCstr(intCurrCounter)
        CheckForValidCounter(strRetCode, strDocType, strTransType, trans)
        ''Throw New Exception(strRetCode)
        ''**********Increment Current Counter ************************

        If isIncreaseCounter Then
            intCurrCounter = intCurrCounter + 1
            qry = "update " + strDatabase + ".dbo.TSPL_DOCPREFIX_MASTER set Next_Number=" + clsCommon.myCstr(intCurrCounter) + ""
            qry += " where Fin_Year='" + clsCommon.myCstr(IntFiscalYear) + "' and Doc_Type='" + strDocType + "' and isnull(Doc_Trans_Type,'')='" + strTransType + "' and isnull(Location_Code,'')='" + strLocatinSegmentCode + "' "
            If isDailyChange Then
                qry += " and Curr_Date='" + clsCommon.GetPrintDate(intCurrDate, "dd/MMM/yyyy") + "'"
            ElseIf isMonthlyChange Then
                qry += " and Curr_Month='" + clsCommon.myCstr(intCurrMonth) + "'"
            End If
            clsDBFuncationality.ExecuteNonQuery(qry, trans)
        End If
        Return strRetCode
    End Function

    Public Shared Sub ChangeSalesman(ByVal trans As SqlTransaction, ByVal saleInvoiceNo As String, ByRef shipmentNo As String, ByVal salesmanCode As String, ByRef salesmanName As String)
        Try
            Dim strSaleInv As String = "Update TSPL_SALE_INVOICE_HEAD set Salesman_Code ='" + salesmanCode + "' Where Sale_Invoice_No ='" + saleInvoiceNo + "'"
            clsDBFuncationality.ExecuteNonQuery(strSaleInv, trans)
            Dim strShipment As String = "Update TSPL_SHIPMENT_MASTER set Salesman_Code ='" + salesmanCode + "' Where Shipment_No ='" + shipmentNo + "'"
            clsDBFuncationality.ExecuteNonQuery(strShipment, trans)
            Dim strEmptyTrans As String = "Update TSPL_ADJUSTMENT_HEADER set EMP_CODE='" + salesmanCode + "', EMP_NAME='" + salesmanName + "' Where ItemType='E' AND Reference_Document='Sale Invoice' and Document_No='" + saleInvoiceNo + "'"
            clsDBFuncationality.ExecuteNonQuery(strEmptyTrans, trans)
            Dim strSalesReturn As String = "Update TSPL_SALE_RETURN_HEAD set Salesman_Code ='" + salesmanCode + "' Where Invoice_No ='" + saleInvoiceNo + "'"
            clsDBFuncationality.ExecuteNonQuery(strSalesReturn, trans)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Shared Function CheckForValidCounter(ByVal strRetCode As String, ByVal strDocType As String, ByVal strTransType As String, ByVal trans As SqlTransaction) As Boolean
        'If clsCommon.CompairString(strDocType, clsDocType.SaleInvoice) = CompairStringResult.Equal AndAlso clsCommon.CompairString(strTransType, clsDocTransactionType.SaleInvoiceExcise) = CompairStringResult.Equal Then
        '    Dim qry As String = "select code from("
        '    qry += " select Sale_Invoice_No as Code from TSPL_SALE_INVOICE_HEAD"
        '    qry += " union all"
        '    qry += " select Transfer_No as Code from TSPL_TRANSFER_HEAD"
        '    qry += " union all "
        '    qry += " select Doc_No as Code from TSPL_IssueReturn_HEAD"
        '    qry += " union all "
        '    qry += " select shipment_No as Code from TSPL_SCRAPSALE_HEAD"
        '    qry += " union all "
        '    qry += " select invoice_No as Code from TSPL_SCRAPINVOICE_HEAD) xxx where code='" + strRetCode + "'"
        '    Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry, trans)
        '    If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
        '        Throw New Exception("Auto Generated Code " + strRetCode + " is already in use.")
        '    End If
        'End If
        Return True
    End Function

    Private Shared Function DemoGetQryOFDOCPrefix(ByVal dtDocDate As Date, ByVal strDocType As String, ByVal strTransType As String, ByVal strLocatinSegmentCode As String, ByVal IntFiscalYear As Integer, ByVal isTakingTopOne As Boolean, ByVal strDatabase As String) As String
        Dim qry As String = "select " + IIf(isTakingTopOne, " Top 1", "") + " Doc_Prfeix,Fin_Year,Next_Number,Separator,Is_Change_monthly,Curr_Month,Is_Change_Daily,Curr_Date,dontDisplayYearInSeries,MinSizeofSeries from " + strDatabase + ".dbo.TSPL_DOCPREFIX_MASTER where  Doc_Type='" + strDocType + "' and  isnull(Doc_Trans_Type,'')='" + strTransType + "' and isnull(Location_Code,'')='" + strLocatinSegmentCode + "' and Fin_Year='" + clsCommon.myCstr(IntFiscalYear) + "'"
        If isTakingTopOne Then
            qry += " order by case when Is_Change_Daily=1 then  CONVERT(varchar, Curr_Date,112) else  CONVERT(varchar, Curr_Month ) end desc"
        Else
            qry += " and 2=(case when Is_Change_Daily=1 then case when Curr_Date= '" + clsCommon.GetPrintDate(dtDocDate, "dd/MMM/yyyy") + "'  then 2 else 3  end   else   case when Is_Change_monthly=1 then case when Curr_Month= " + clsCommon.myCstr(dtDocDate.Month) + "  then 2 else 3  end else 2 end end)"
        End If
        Return qry
    End Function

    Public Shared Function UserAvailableAccountQuery() As String
        Return "select Account_Code as Code from TSPL_GL_ACCOUNT_PERMISSION WHERE User_Code='" + objCommonVar.CurrentUserCode + "' "
    End Function

    Public Shared Function UserAvailableLocationQuery() As String
        ''7 is hardcoded to get only location
        If objCommonVar.CurrentUserCode = "ADMIN" Then
            '  Return "Select Distinct LM.Location_Code as Code,LM.Location_Desc as Description,LM.Location_type as 'Location Type',(case LM.Excisable when 'T'then 'Excisable'else 'Non-Excisable'end) as 'Excisable' from TSPL_LOCATION_MASTER as LM INNER JOIN TSPL_GL_SEGMENT_PERMISSION GSP ON LM.Loc_Segment_Code=GSP.Segment_Code"
            Return "Select  LM.Location_Code as Code,LM.Location_Desc as Description,Location_type as 'Location Type',(case LM.Excisable when 'T'then 'Excisable'else 'Non-Excisable'end) as 'Excisable'  from TSPL_LOCATION_MASTER as LM where 2=2 "
        Else
            Return "Select Distinct LM.Location_Code as Code,LM.Location_Desc as Description,LM.Location_type as 'Location Type',(case LM.Excisable when 'T'then 'Excisable'else 'Non-Excisable'end) as 'Excisable' from TSPL_LOCATION_MASTER as LM INNER JOIN TSPL_GL_SEGMENT_PERMISSION GSP ON LM.Loc_Segment_Code=GSP.Segment_Code where GSP.User_Code='" + objCommonVar.CurrentUserCode + "'"

        End If
        '  Return "select TSPL_GL_SEGMENT_PERMISSION.Segment_Code as Code,TSPL_GL_SEGMENT_CODE.Description as Description from TSPL_GL_SEGMENT_PERMISSION left outer join TSPL_GL_SEGMENT_CODE on TSPL_GL_SEGMENT_CODE.Segment_code=TSPL_GL_SEGMENT_PERMISSION.Segment_Code and TSPL_GL_SEGMENT_CODE.Seg_No=TSPL_GL_SEGMENT_PERMISSION.GL_Segment where TSPL_GL_SEGMENT_PERMISSION.User_Code='" + objCommonVar.CurrentUserCode + "' and TSPL_GL_SEGMENT_CODE.Seg_No=7 "
    End Function

    Public Shared Function UserAvailableLocationQuery(ByRef whrClas As String) As String
        whrClas = " 2=2 "
        ''7 is hardcoded to get only location
        If objCommonVar.CurrentUserCode = "ADMIN" Then
            Return "Select  LM.Location_Code as Code,LM.Location_Desc as Description,Location_type as 'Location Type',(case LM.Excisable when 'T'then 'Excisable'else 'Non-Excisable'end) as 'Excisable'  from TSPL_LOCATION_MASTER as LM"
        Else
            Return "Select Distinct LM.Location_Code as Code,LM.Location_Desc as Description,LM.Location_type as 'Location Type',(case LM.Excisable when 'T'then 'Excisable'else 'Non-Excisable'end) as 'Excisable' from TSPL_LOCATION_MASTER as LM INNER JOIN TSPL_GL_SEGMENT_PERMISSION GSP ON LM.Loc_Segment_Code=GSP.Segment_Code "
            whrClas = " and GSP.User_Code='" + objCommonVar.CurrentUserCode + "'"
        End If
    End Function

    Public Shared Function UserAvailableLocationCodeQuery() As String
        ''7 is hardcoded to get only location
        Return "select Segment_Code as Code from TSPL_GL_SEGMENT_PERMISSION where User_Code='" + objCommonVar.CurrentUserCode + "' "
    End Function

    Public Shared Function UserAvailableLocationData() As List(Of String)
        Dim Arr As New List(Of String)
        Dim qry As String = "select TSPL_GL_SEGMENT_PERMISSION.Segment_Code as Code,TSPL_GL_SEGMENT_CODE.Description as Description from TSPL_GL_SEGMENT_PERMISSION left outer join TSPL_GL_SEGMENT_CODE on TSPL_GL_SEGMENT_CODE.Segment_code=TSPL_GL_SEGMENT_PERMISSION.Segment_Code and TSPL_GL_SEGMENT_CODE.Seg_No=TSPL_GL_SEGMENT_PERMISSION.GL_Segment where TSPL_GL_SEGMENT_PERMISSION.User_Code='" + objCommonVar.CurrentUserCode + "' and TSPL_GL_SEGMENT_CODE.Seg_No=7 "
        Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry)
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            For Each dr As DataRow In dt.Rows
                Arr.Add(clsCommon.myCstr(dr("Code")))
            Next
        End If
        Return Arr
    End Function

    Public Shared Function UserAvailableTaxGroup() As String
        If objCommonVar.CurrentUserCode = "ADMIN" Then
            Return "select DISTINCT  M.TAX_Group_Code as 'Code',(CASE WHEN M.Tax_Group_Type='S' THEN 'Sales' ELSE 'Purchase' END) as 'Transaction Type',M.Tax_Group_Desc as Description from TSPL_TAX_GROUP_MASTER M JOIN TSPL_TAX_GROUP_DETAILS D ON M.Tax_Group_Code = D.Tax_Group_Code join TSPL_TAX_MASTER TM ON D.Tax_Code = TM.Tax_Code WHERE 0=0"
        Else
            Return "select DISTINCT  M.TAX_Group_Code as 'Code',(CASE WHEN M.Tax_Group_Type='S' THEN 'Sales' ELSE 'Purchase' END) as 'Transaction Type',M.Tax_Group_Desc as Description from TSPL_TAX_GROUP_MASTER M JOIN TSPL_TAX_GROUP_DETAILS D ON M.Tax_Group_Code = D.Tax_Group_Code join TSPL_TAX_MASTER TM ON D.Tax_Code = TM.Tax_Code WHERE (Substring(TM.Tax_Liability_Account,6,3) in (" + UserAvailableLocationCodeQuery() + ")  OR TM.Tax_Liability_Account in (" + UserAvailableAccountQuery() + "))"
        End If
    End Function
    Public Shared Function UserAvailableTaxGroup(ByRef out As String) As String
        If objCommonVar.CurrentUserCode = "ADMIN" Then
            Return "select DISTINCT  M.TAX_Group_Code as 'Code',(CASE WHEN M.Tax_Group_Type='S' THEN 'Sales' ELSE 'Purchase' END) as 'Transaction Type',M.Tax_Group_Desc as Description from TSPL_TAX_GROUP_MASTER M JOIN TSPL_TAX_GROUP_DETAILS D ON M.Tax_Group_Code = D.Tax_Group_Code join TSPL_TAX_MASTER TM ON D.Tax_Code = TM.Tax_Code "
        Else
            Return "select DISTINCT  M.TAX_Group_Code as 'Code',(CASE WHEN M.Tax_Group_Type='S' THEN 'Sales' ELSE 'Purchase' END) as 'Transaction Type',M.Tax_Group_Desc as Description from TSPL_TAX_GROUP_MASTER M JOIN TSPL_TAX_GROUP_DETAILS D ON M.Tax_Group_Code = D.Tax_Group_Code join TSPL_TAX_MASTER TM ON D.Tax_Code = TM.Tax_Code"
            out = " (Substring(TM.Tax_Liability_Account,6,3) in (" + UserAvailableLocationCodeQuery() + ")  OR TM.Tax_Liability_Account in (" + UserAvailableAccountQuery() + "))"
        End If
    End Function

    Public Shared Function UserAvailableCustomers() As String
        If objCommonVar.CurrentUserCode = "ADMIN" Then
            Return "SELECT M.Cust_Code AS [Code], m.Customer_Name as [Name], m.Route_No as [Route No], m.Price_Code as [Excisable Price Code], m.price_CodeNon as [Non-Excisable Price Code], (case when m.Credit_Customer = 'Y' THEN 'YES' ELSE 'NO' END) AS [Credit Customer], M.Cust_Category_Code AS [Customer Category Code]  FROM TSPL_CUSTOMER_MASTER M JOIN TSPL_CUSTOMER_ACCOUNT_SET A ON M.Cust_Account =A.Cust_Account"
        Else
            Return "SELECT M.Cust_Code AS [Code], m.Customer_Name as [Name],m.Route_No as [Route No], m.Price_Code as [Excisable Price Code], m.price_CodeNon as [Non-Excisable Price Code], (case when m.Credit_Customer = 'Y' THEN 'YES' ELSE 'NO' END) AS [Credit Customer], M.Cust_Category_Code AS [Customer Category Code]  FROM TSPL_CUSTOMER_MASTER M JOIN TSPL_CUSTOMER_ACCOUNT_SET A ON M.Cust_Account =A.Cust_Account where (Substring(a.Receivable_Control_acct,6,3) in (" + UserAvailableLocationCodeQuery() + ") or a.Receivable_Control_acct in (" + UserAvailableAccountQuery() + ")) "
        End If
    End Function

    Public Shared Function UserAvailableCustomers(ByRef whrClas As String) As String
        If objCommonVar.CurrentUserCode = "ADMIN" Then
            Return "SELECT M.Cust_Code AS [Code], m.Customer_Name as [Name], m.Route_No as [Route No], m.Price_Code as [Excisable Price Code], m.price_CodeNon as [Non-Excisable Price Code], (case when m.Credit_Customer = 'Y' THEN 'YES' ELSE 'NO' END) AS [Credit Customer], M.Cust_Category_Code AS [Customer Category Code]  FROM TSPL_CUSTOMER_MASTER M JOIN TSPL_CUSTOMER_ACCOUNT_SET A ON M.Cust_Account =A.Cust_Account"
        Else
            whrClas = "  (Substring(a.Receivable_Control_acct,6,3) in (" + UserAvailableLocationCodeQuery() + ") or a.Receivable_Control_acct in (" + UserAvailableAccountQuery() + "))"
            Return "SELECT M.Cust_Code AS [Code], m.Customer_Name as [Name],m.Route_No as [Route No], m.Price_Code as [Excisable Price Code], m.price_CodeNon as [Non-Excisable Price Code], (case when m.Credit_Customer = 'Y' THEN 'YES' ELSE 'NO' END) AS [Credit Customer], M.Cust_Category_Code AS [Customer Category Code]  FROM TSPL_CUSTOMER_MASTER M JOIN TSPL_CUSTOMER_ACCOUNT_SET A ON M.Cust_Account =A.Cust_Account "
        End If
    End Function

    Public Shared Function glvendorquery() As String
        Dim query As String
        If objCommonVar.CurrentUserCode = "ADMIN" Then
            query = "select M.Vendor_Code AS [Vendor Code], m.Vendor_Name as [Vendor Name] from TSPL_VENDOR_MASTER m join TSPL_VENDOR_ACCOUNT_SET s on m.Vendor_Account =s.Acct_Set_Code"

        Else
            Dim arrlocation As New ArrayList()
            Dim arraccount As New ArrayList()
            arraccount = connectSql.funglaccountmultiple(objCommonVar.CurrentUserCode)
            arrlocation = connectSql.funglsegmentmultiple(objCommonVar.CurrentUserCode)
            Dim countsegment As Integer = arrlocation.Count
            Dim countaccount As Integer = arraccount.Count
            If countaccount <> 0 Then
                If countsegment <> 0 Then
                    Dim valuefirstsegment As String = arrlocation(0)
                    valuefirstsegment = "%" + valuefirstsegment
                    Dim valuefirstacct As String = arraccount(0)
                    query = "select M.Vendor_Code AS [Vendor Code], m.Vendor_Name as [Vendor Name]   from TSPL_VENDOR_MASTER m join TSPL_VENDOR_ACCOUNT_SET s on m.Vendor_Account =s.Acct_Set_Code where s.Payable_Account LIKE '" + valuefirstsegment + "' OR s.Payable_Account='" + valuefirstacct + "'"
                    For count As Integer = 0 To countsegment - 1
                        If count < countsegment - 1 Then
                            Dim value As String = arrlocation(count + 1)
                            value = "%" + value
                            Dim stcode As String = "or s.Payable_Account like'" + value + "'"
                            query = query + stcode
                        End If
                    Next
                    For countacct As Integer = 0 To countaccount
                        If countacct < countaccount - 1 Then
                            Dim value As String = arraccount(countacct + 1)
                            Dim stcode As String = "or s.Payable_Account = '" + value + "'"
                            query = query + stcode
                        End If
                    Next
                Else
                    query = "select top 0  M.Vendor_Code AS [Vendor Code], m.Vendor_Name as [Vendor Name] from TSPL_VENDOR_MASTER m join TSPL_VENDOR_ACCOUNT_SET s on m.Vendor_Account =s.Acct_Set_Code"
                End If
            Else
                If countsegment <> 0 Then
                    Dim valuefirstsegment As String = arrlocation(0)
                    valuefirstsegment = "%" + valuefirstsegment
                    Dim valuefirstacct As String = String.Empty
                    query = "select M.Vendor_Code AS [Vendor Code], m.Vendor_Name as [Vendor Name]   from TSPL_VENDOR_MASTER m join TSPL_VENDOR_ACCOUNT_SET s on m.Vendor_Account =s.Acct_Set_Code where s.Payable_Account LIKE '" + valuefirstsegment + "' OR s.Payable_Account='" + valuefirstacct + "'"
                    For count As Integer = 0 To countsegment - 1
                        If count < countsegment - 1 Then
                            Dim value As String = arrlocation(count + 1)
                            value = "%" + value
                            Dim stcode As String = "or s.Payable_Account like'" + value + "'"
                            query = query + stcode
                        End If
                    Next
                    For countacct As Integer = 0 To countaccount
                        If countacct < countaccount - 1 Then
                            Dim value As String = arraccount(countacct + 1)
                            Dim stcode As String = "or s.Payable_Account = '" + value + "'"
                            query = query + stcode
                        End If
                    Next
                Else
                    query = "select top 0 M.Vendor_Code AS [Vendor Code], m.Vendor_Name as [Vendor Name]   from TSPL_VENDOR_MASTER m join TSPL_VENDOR_ACCOUNT_SET s on m.Vendor_Account =s.Acct_Set_Code"
                End If
            End If
        End If
        Return query
    End Function

    '' Added By Pankal''' on  27/01/20121
    ''
    ''RICHA BM00000009847

    Public Shared Function glvendorqueryNew() As String
        Dim query As String
        ''If objCommonVar.CurrentUserCode = "ADMIN" Then
        'query = "select M.Vendor_Code AS [Code], m.Vendor_Name as [Name] from TSPL_VENDOR_MASTER m join TSPL_VENDOR_ACCOUNT_SET s on m.Vendor_Account =s.Acct_Set_Code"
        query = "select M.Vendor_Code AS [Code], m.Vendor_Name as [Name],ISNULL(m.alies_name,'') As [Alies Name],(m.Add1+(case when m.Add2='' then '' else ',' end)+m.Add2) as [Address],m.Vendor_Group_Code as [Vendor Group Code],m.Vendor_Group_Code_Desc as [Vendor Group Desc],s.Acct_Set_Code as [Vendor Account Set],s.Acct_Set_Desc as [Vendor Account Set Desc] from TSPL_VENDOR_MASTER m join TSPL_VENDOR_ACCOUNT_SET s on m.Vendor_Account =s.Acct_Set_Code"

        ''Else
        ''    Dim arrlocation As New ArrayList()
        ''    Dim arraccount As New ArrayList()
        ''    arraccount = connectSql.funglaccountmultiple(objCommonVar.CurrentUserCode)
        ''    arrlocation = connectSql.funglsegmentmultiple(objCommonVar.CurrentUserCode)
        ''    Dim countsegment As Integer = arrlocation.Count
        ''    Dim countaccount As Integer = arraccount.Count
        ''    If countaccount <> 0 Then
        ''        If countsegment <> 0 Then
        ''            Dim valuefirstsegment As String = arrlocation(0)
        ''            valuefirstsegment = "%" + valuefirstsegment
        ''            Dim valuefirstacct As String = arraccount(0)
        ''            query = "select M.Vendor_Code AS [Code], m.Vendor_Name as [Name]   from TSPL_VENDOR_MASTER m join TSPL_VENDOR_ACCOUNT_SET s on m.Vendor_Account =s.Acct_Set_Code where s.Payable_Account LIKE '" + valuefirstsegment + "' OR s.Payable_Account='" + valuefirstacct + "'"
        ''            For count As Integer = 0 To countsegment - 1
        ''                If count < countsegment - 1 Then
        ''                    Dim value As String = arrlocation(count + 1)
        ''                    value = "%" + value
        ''                    Dim stcode As String = "or s.Payable_Account like'" + value + "'"
        ''                    query = query + stcode
        ''                End If
        ''            Next
        ''            For countacct As Integer = 0 To countaccount
        ''                If countacct < countaccount - 1 Then
        ''                    Dim value As String = arraccount(countacct + 1)
        ''                    Dim stcode As String = "or s.Payable_Account = '" + value + "'"
        ''                    query = query + stcode
        ''                End If
        ''            Next
        ''        Else
        ''            query = "select top 0  M.Vendor_Code AS [Code], m.Vendor_Name as [Name] from TSPL_VENDOR_MASTER m join TSPL_VENDOR_ACCOUNT_SET s on m.Vendor_Account =s.Acct_Set_Code"
        ''        End If
        ''    Else
        ''        If countsegment <> 0 Then
        ''            Dim valuefirstsegment As String = arrlocation(0)
        ''            valuefirstsegment = "%" + valuefirstsegment
        ''            Dim valuefirstacct As String = String.Empty
        ''            query = "select M.Vendor_Code AS [Code], m.Vendor_Name as [Name]   from TSPL_VENDOR_MASTER m join TSPL_VENDOR_ACCOUNT_SET s on m.Vendor_Account =s.Acct_Set_Code where s.Payable_Account LIKE '" + valuefirstsegment + "' OR s.Payable_Account='" + valuefirstacct + "'"
        ''            For count As Integer = 0 To countsegment - 1
        ''                If count < countsegment - 1 Then
        ''                    Dim value As String = arrlocation(count + 1)
        ''                    value = "%" + value
        ''                    Dim stcode As String = "or s.Payable_Account like'" + value + "'"
        ''                    query = query + stcode
        ''                End If
        ''            Next
        ''            For countacct As Integer = 0 To countaccount
        ''                If countacct < countaccount - 1 Then
        ''                    Dim value As String = arraccount(countacct + 1)
        ''                    Dim stcode As String = "or s.Payable_Account = '" + value + "'"
        ''                    query = query + stcode
        ''                End If
        ''            Next
        ''        Else
        ''            query = "select top 0 M.Vendor_Code AS [Code], m.Vendor_Name as [Name]   from TSPL_VENDOR_MASTER m join TSPL_VENDOR_ACCOUNT_SET s on m.Vendor_Account =s.Acct_Set_Code"
        ''        End If
        ''    End If
        ''End If
        Return query
    End Function

    Public Shared Function glCustomerQuery() As String
        Dim query As String
        ''If objCommonVar.CurrentUserCode = "ADMIN" Then
        query = "select M.Cust_Code AS [Code], m.Customer_Name as [Name],ISNULL(m.Alies_Name,'') As [Alies Name] from TSPL_CUSTOMER_MASTER m join TSPL_CUSTOMER_ACCOUNT_SET s on m.Cust_Account=s.Cust_Account"
        ' ''Else
        ' ''    Dim arrlocation As New ArrayList()
        ' ''    Dim arraccount As New ArrayList()
        ' ''    arraccount = connectSql.funglaccountmultiple(objCommonVar.CurrentUserCode)
        ' ''    arrlocation = connectSql.funglsegmentmultiple(objCommonVar.CurrentUserCode)
        ' ''    Dim countsegment As Integer = arrlocation.Count
        ' ''    Dim countaccount As Integer = arraccount.Count
        ' ''    If countaccount <> 0 Then
        ' ''        If countsegment <> 0 Then
        ' ''            Dim valuefirstsegment As String = arrlocation(0)
        ' ''            valuefirstsegment = "%" + valuefirstsegment
        ' ''            Dim valuefirstacct As String = arraccount(0)
        ' ''            query = "select M.Vendor_Code AS [Code], m.Vendor_Name as [Name]   from TSPL_VENDOR_MASTER m join TSPL_VENDOR_ACCOUNT_SET s on m.Vendor_Account =s.Acct_Set_Code where s.Payable_Account LIKE '" + valuefirstsegment + "' OR s.Payable_Account='" + valuefirstacct + "'"
        ' ''            For count As Integer = 0 To countsegment - 1
        ' ''                If count < countsegment - 1 Then
        ' ''                    Dim value As String = arrlocation(count + 1)
        ' ''                    value = "%" + value
        ' ''                    Dim stcode As String = "or s.Payable_Account like'" + value + "'"
        ' ''                    query = query + stcode
        ' ''                End If
        ' ''            Next
        ' ''            For countacct As Integer = 0 To countaccount
        ' ''                If countacct < countaccount - 1 Then
        ' ''                    Dim value As String = arraccount(countacct + 1)
        ' ''                    Dim stcode As String = "or s.Payable_Account = '" + value + "'"
        ' ''                    query = query + stcode
        ' ''                End If
        ' ''            Next
        ' ''        Else
        ' ''            query = "select top 0  M.Vendor_Code AS [Code], m.Vendor_Name as [Name] from TSPL_VENDOR_MASTER m join TSPL_VENDOR_ACCOUNT_SET s on m.Vendor_Account =s.Acct_Set_Code"
        ' ''        End If
        ' ''    Else
        ' ''        If countsegment <> 0 Then
        ' ''            Dim valuefirstsegment As String = arrlocation(0)
        ' ''            valuefirstsegment = "%" + valuefirstsegment
        ' ''            Dim valuefirstacct As String = String.Empty
        ' ''            query = "select M.Vendor_Code AS [Code], m.Vendor_Name as [Name]   from TSPL_VENDOR_MASTER m join TSPL_VENDOR_ACCOUNT_SET s on m.Vendor_Account =s.Acct_Set_Code where s.Payable_Account LIKE '" + valuefirstsegment + "' OR s.Payable_Account='" + valuefirstacct + "'"
        ' ''            For count As Integer = 0 To countsegment - 1
        ' ''                If count < countsegment - 1 Then
        ' ''                    Dim value As String = arrlocation(count + 1)
        ' ''                    value = "%" + value
        ' ''                    Dim stcode As String = "or s.Payable_Account like'" + value + "'"
        ' ''                    query = query + stcode
        ' ''                End If
        ' ''            Next
        ' ''            For countacct As Integer = 0 To countaccount
        ' ''                If countacct < countaccount - 1 Then
        ' ''                    Dim value As String = arraccount(countacct + 1)
        ' ''                    Dim stcode As String = "or s.Payable_Account = '" + value + "'"
        ' ''                    query = query + stcode
        ' ''                End If
        ' ''            Next
        ' ''        Else
        ' ''            query = "select top 0 M.Vendor_Code AS [Code], m.Vendor_Name as [Name]   from TSPL_VENDOR_MASTER m join TSPL_VENDOR_ACCOUNT_SET s on m.Vendor_Account =s.Acct_Set_Code"
        ' ''        End If
        ' ''    End If
        ''End If
        Return query
    End Function

    Public Shared Function glbankquery() As String
        Dim query As String
        If objCommonVar.CurrentUserCode = "ADMIN" Then
            query = "select BANK_CODE as [Bank Code], DESCRIPTION  from TSPL_BANK_MASTER "
        Else
            Dim arrlocation As New ArrayList()
            Dim arraccount As New ArrayList()

            arraccount = connectSql.funglaccountmultiple(objCommonVar.CurrentUserCode)
            arrlocation = connectSql.funglsegmentmultiple(objCommonVar.CurrentUserCode)
            Dim countsegment As Integer = arrlocation.Count
            Dim countaccount As Integer = arraccount.Count
            If countaccount <> 0 Then
                If countsegment <> 0 Then
                    Dim valuefirstsegment As String = arrlocation(0)
                    valuefirstsegment = "%" + valuefirstsegment
                    Dim valuefirstacct As String = arraccount(0)
                    query = "select BANK_CODE as [Bank Code], DESCRIPTION  from TSPL_BANK_MASTER where bankacc like '" + valuefirstsegment + "' OR bankacc ='" + valuefirstacct + "'"
                    For count As Integer = 0 To countsegment - 1
                        If count < countsegment - 1 Then
                            Dim value As String = arrlocation(count + 1)
                            value = "%" + value
                            Dim stcode As String = "or bankacc like'" + value + "'"
                            query = query + stcode
                        End If
                    Next
                    For countacct As Integer = 0 To countaccount
                        If countacct < countaccount - 1 Then
                            Dim value As String = arraccount(countacct + 1)
                            Dim stcode As String = "or bankacc = '" + value + "'"
                            query = query + stcode
                        End If
                    Next
                Else
                    query = "select top 0 BANK_CODE as [Bank Code], DESCRIPTION  from TSPL_BANK_MASTER"

                End If
            Else
                If countsegment <> 0 Then
                    Dim valuefirstsegment As String = arrlocation(0)
                    valuefirstsegment = "%" + valuefirstsegment
                    Dim valuefirstacct As String = String.Empty
                    query = "select BANK_CODE as [Bank Code], DESCRIPTION  from TSPL_BANK_MASTER where bankacc like '" + valuefirstsegment + "' OR bankacc ='" + valuefirstacct + "'"
                    For count As Integer = 0 To countsegment - 1
                        If count < countsegment - 1 Then
                            Dim value As String = arrlocation(count + 1)
                            value = "%" + value
                            Dim stcode As String = "or bankacc like'" + value + "'"
                            query = query + stcode
                        End If
                    Next
                    For countacct As Integer = 0 To countaccount
                        If countacct < countaccount - 1 Then
                            Dim value As String = arraccount(countacct + 1)
                            Dim stcode As String = "or bankacc = '" + value + "'"
                            query = query + stcode
                        End If
                    Next
                Else
                    query = "select top 0 BANK_CODE as [Bank Code], DESCRIPTION  from TSPL_BANK_MASTER"
                End If
            End If
        End If


        Return query
    End Function

    ''' Added By Pankaj''''on 27/01/2012
    ''' Add By Preeti Gupta 22/07/2014---
    Public Shared Function glbankqueryNew(ByRef strWhrClas As String) As String
        Dim Bank_Code As String = FrmMainTranScreen.bankPermission(Nothing)
        strWhrClas = "1=1"
        If clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.PermissionSettingForTransactionWithBank, clsFixedParameterType.PermissionSettingForTransactionWithBank, Nothing)) = 1 Then
            If clsCommon.myLen(objCommonVar.strCurrUserLocations) > 0 Then
                strWhrClas += " AND RIGHT(TSPL_BANK_MASTER.BANKACC,3) in (" + objCommonVar.strCurrUserLocationsSegment + ")"
            End If
        ElseIf clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.PermissionSettingForTransactionWithBank, clsFixedParameterType.PermissionSettingForTransactionWithBank, Nothing)) = 1 Then
            If clsCommon.myLen(Bank_Code) > 0 Then
                strWhrClas += " AND TSPL_BANK_MASTER.Bank_Code in ( " + Bank_Code + " )"
            End If
        End If
        Dim query As String
        If objCommonVar.CurrentUserCode = objCommonVar.CurrentUserCode Then
            query = "select BANK_CODE as [Code], DESCRIPTION,BANKACCNUMBER as [Bank Account No]  from TSPL_BANK_MASTER "
        Else
            Dim arrlocation As New ArrayList()
            Dim arraccount As New ArrayList()

            arraccount = connectSql.funglaccountmultiple(objCommonVar.CurrentUserCode)
            arrlocation = connectSql.funglsegmentmultiple(objCommonVar.CurrentUserCode)
            Dim countsegment As Integer = arrlocation.Count
            Dim countaccount As Integer = arraccount.Count
            If countaccount <> 0 Then
                If countsegment <> 0 Then
                    Dim valuefirstsegment As String = arrlocation(0)
                    valuefirstsegment = "%" + valuefirstsegment
                    Dim valuefirstacct As String = arraccount(0)
                    query = "select BANK_CODE as [Code], DESCRIPTION,BANKACCNUMBER as [Bank Account No]  from TSPL_BANK_MASTER "
                    strWhrClas = " Substring(BANKACC , (LEN(BANKACC)-2), 3) IN (" + clsCommon.GetMulcallString(arrlocation) + ") OR bankacc IN (" + clsCommon.GetMulcallString(arraccount) + ")"
                    If Bank_Code <> "" Then
                        strWhrClas = " and TSPL_BANK_MASTER.bank_code in ( " + Bank_Code + " )"
                    End If
                Else
                    query = "select BANK_CODE as [Code], DESCRIPTION,BANKACCNUMBER as [Bank Account No]  from TSPL_BANK_MASTER"
                    If Bank_Code <> "" Then
                        strWhrClas = " TSPL_BANK_MASTER.bank_code in ( " + Bank_Code + " )"
                    End If
                End If
            Else
                If countsegment <> 0 Then
                    Dim valuefirstsegment As String = arrlocation(0)
                    valuefirstsegment = "%" + valuefirstsegment
                    Dim valuefirstacct As String = String.Empty
                    query = "select BANK_CODE as [Code], DESCRIPTION,BANKACCNUMBER as [Bank Account No]  from TSPL_BANK_MASTER "
                    strWhrClas = " Substring(BANKACC , (LEN(BANKACC)-2), 3) IN (" + clsCommon.GetMulcallString(arrlocation) + ") OR bankacc IN (" + clsCommon.GetMulcallString(arraccount) + ")"
                    If Bank_Code <> "" Then
                        strWhrClas = " TSPL_BANK_MASTER.bank_code in ( " + Bank_Code + " )"
                    End If
                Else
                    query = "select BANK_CODE as [Code], DESCRIPTION,BANKACCNUMBER as [Bank Account No]  from TSPL_BANK_MASTER"
                    If Bank_Code <> "" Then
                        strWhrClas = " TSPL_BANK_MASTER.bank_code in ( " + Bank_Code + " )"
                    End If
                End If
            End If
        End If

        Return query
        ''''Code Ends Here
    End Function

    Public Shared Sub GlLOCandACCArray(ByRef Arrloc As ArrayList, ByRef ArrAcc As ArrayList)
        ArrAcc = connectSql.funglaccountmultiple(objCommonVar.CurrentUserCode)
        Arrloc = connectSql.funglsegmentmultiple(objCommonVar.CurrentUserCode)
    End Sub


    'Public Shared Function glaccountquery() As String
    '    Dim qry As String = ""
    '    Dim whrCls As String = ""
    '    Dim arrlocation As New ArrayList()
    '    Dim arraccount As New ArrayList()
    '    If clsCommon.myCstr(objCommonVar.CurrentUserCode).ToUpper() = "ADMIN" Then
    '        qry = "select  Account_Code , Description  from TSPL_GL_ACCOUNTS"
    '    Else
    '        arraccount = connectSql.funglaccountmultiple(objCommonVar.CurrentUserCode)
    '        arrlocation = connectSql.funglsegmentmultiple(objCommonVar.CurrentUserCode)
    '        Dim straccount As String = connectSql.funglaccount(objCommonVar.CurrentUserCode)
    '        Dim countsegment As Integer = arrlocation.Count
    '        Dim countaccount As Integer = arraccount.Count

    '        If countaccount <> 0 Then
    '            If countsegment <> 0 Then
    '                Dim valuefirstsegment As String = arrlocation(0)
    '                valuefirstsegment = "%" + valuefirstsegment
    '                Dim valuefirstacct As String = arraccount(0)
    '                qry = " select Account_Code , Description  from TSPL_GL_ACCOUNTS"
    '                whrCls = " where Account_Code like '" + valuefirstsegment + "' or Account_Code = '" + valuefirstacct + "' "
    '                For count As Integer = 0 To countsegment - 1
    '                    If count < countsegment - 1 Then
    '                        Dim value As String = arrlocation(count + 1)
    '                        value = "%" + value
    '                        Dim stcode As String = " or Account_Code like'" + value + "'"
    '                        whrCls = whrCls + stcode
    '                    End If
    '                Next
    '                For countacct As Integer = 0 To countaccount
    '                    If countacct < countaccount - 1 Then
    '                        Dim value As String = arraccount(countacct + 1)
    '                        Dim stcode As String = "or Account_Code = '" + value + "'"
    '                        whrCls = whrCls + stcode
    '                    End If
    '                Next
    '                qry = qry + whrCls
    '            Else
    '                qry = "select top 0 Account_Code , Description  from TSPL_GL_ACCOUNTS"
    '            End If

    '        Else
    '            If countsegment <> 0 Then
    '                Dim valuefirst As String = arrlocation(0)
    '                valuefirst = "%" + valuefirst
    '                qry = "select Account_Code , Description  from TSPL_GL_ACCOUNTS"
    '                whrCls = " where Account_Code like  '" + valuefirst + "'  "
    '                For count As Integer = 0 To countsegment - 1
    '                    If count < countsegment - 1 Then
    '                        Dim value As String = arrlocation(count + 1)
    '                        value = "%" + value
    '                        Dim stcode As String = " or Account_Code like'" + value + "'"
    '                        whrCls = whrCls + stcode
    '                    End If
    '                Next

    '                qry = qry + whrCls

    '            Else
    '                qry = "select top 0 Account_Code , Description  from TSPL_GL_ACCOUNTS"
    '            End If
    '        End If
    '    End If
    '    If clsCommon.myLen(whrCls) > 0 Then
    '        qry = qry + whrCls + " and ControlAccount ='N'"
    '    Else
    '        qry = qry + " where ControlAccount ='N'"
    '    End If
    '    Return qry

    'End Function

    'Public Shared Function glaccountquery(ByVal code As String) As ArrayList
    '    Dim arrlist As New ArrayList()
    '    Dim qry As String = ""
    '    Dim whrCls As String = ""
    '    Dim arrlocation As New ArrayList()
    '    Dim arraccount As New ArrayList()
    '    If clsCommon.myCstr(objCommonVar.CurrentUserCode).ToUpper() = "ADMIN" Then
    '        qry = " select Account_Code , Description  from TSPL_GL_ACCOUNTS"
    '        whrCls = ""
    '    Else
    '        arraccount = connectSql.funglaccountmultiple(code)
    '        arrlocation = connectSql.funglsegmentmultiple(code)
    '        Dim straccount As String = connectSql.funglaccount(code)
    '        Dim countsegment As Integer = arrlocation.Count
    '        Dim countaccount As Integer = arraccount.Count
    '        If countaccount <> 0 Then
    '            If countsegment <> 0 Then
    '                Dim valuefirstsegment As String = arrlocation(0)
    '                valuefirstsegment = "%" + valuefirstsegment
    '                Dim valuefirstacct As String = arraccount(0)
    '                qry = " select Account_Code , Description  from TSPL_GL_ACCOUNTS"
    '                whrCls = " (Account_Code like '" + valuefirstsegment + "' or Account_Code = '" + valuefirstacct + "' "
    '                For count As Integer = 0 To countsegment - 1
    '                    If count < countsegment - 1 Then
    '                        Dim value As String = arrlocation(count + 1)
    '                        value = "%" + value
    '                        Dim stcode As String = " or Account_Code like'" + value + "'"
    '                        whrCls = whrCls + stcode
    '                    End If
    '                Next
    '                For countacct As Integer = 0 To countaccount
    '                    If countacct < countaccount - 1 Then
    '                        Dim value As String = arraccount(countacct + 1)
    '                        Dim stcode As String = "or Account_Code = '" + value + "'"
    '                        whrCls = whrCls + stcode
    '                    End If
    '                Next
    '            Else
    '                qry = "select top 0 Account_Code , Description  from TSPL_GL_ACCOUNTS"
    '            End If
    '        Else
    '            If countsegment <> 0 Then
    '                Dim valuefirst As String = arrlocation(0)
    '                valuefirst = "%" + valuefirst
    '                qry = "select Account_Code , Description  from TSPL_GL_ACCOUNTS"
    '                whrCls = " ( Account_Code like  '" + valuefirst + "'  "
    '                For count As Integer = 0 To countsegment - 1
    '                    If count < countsegment - 1 Then
    '                        Dim value As String = arrlocation(count + 1)
    '                        value = "%" + value
    '                        Dim stcode As String = " or Account_Code like'" + value + "'"
    '                        whrCls = whrCls + stcode
    '                    End If
    '                Next
    '            Else
    '                qry = "select top 0 Account_Code , Description  from TSPL_GL_ACCOUNTS"
    '            End If
    '        End If
    '    End If
    '    If clsCommon.myLen(whrCls) > 0 Then
    '        whrCls = whrCls + " ) and ControlAccount ='N'"
    '    Else
    '        whrCls = whrCls + " ControlAccount ='N'"
    '    End If


    '    arrlist.Add(qry)
    '    arrlist.Add(whrCls)
    '    Return arrlist
    'End Function




    '' Anubhooti 06-Nov-2014
    'Public Shared Function glaccountqueryForControlAcc(ByVal code As String) As ArrayList
    '    Dim arrlist As New ArrayList()
    '    Dim qry As String = ""
    '    Dim whrCls As String = ""
    '    Dim arrlocation As New ArrayList()
    '    Dim arraccount As New ArrayList()
    '    If clsCommon.CompairString(objCommonVar.CurrentUserCode, "ADMIN") = CompairStringResult.Equal Then
    '        qry = " select Account_Code , Description  from TSPL_GL_ACCOUNTS"
    '        whrCls = ""
    '    Else
    '        arraccount = connectSql.funglaccountmultiple(code)
    '        arrlocation = connectSql.funglsegmentmultiple(code)
    '        Dim straccount As String = connectSql.funglaccount(code)
    '        Dim countsegment As Integer = arrlocation.Count
    '        Dim countaccount As Integer = arraccount.Count
    '        If countaccount <> 0 Then
    '            If countsegment <> 0 Then
    '                Dim valuefirstsegment As String = arrlocation(0)
    '                valuefirstsegment = "%" + valuefirstsegment
    '                Dim valuefirstacct As String = arraccount(0)
    '                qry = " select Account_Code , Description  from TSPL_GL_ACCOUNTS"
    '                whrCls = "Account_Code like '" + valuefirstsegment + "' or Account_Code = '" + valuefirstacct + "' "
    '                For count As Integer = 0 To countsegment - 1
    '                    If count < countsegment - 1 Then
    '                        Dim value As String = arrlocation(count + 1)
    '                        value = "%" + value
    '                        Dim stcode As String = " or Account_Code like'" + value + "'"
    '                        whrCls = whrCls + stcode
    '                    End If
    '                Next
    '                For countacct As Integer = 0 To countaccount
    '                    If countacct < countaccount - 1 Then
    '                        Dim value As String = arraccount(countacct + 1)
    '                        Dim stcode As String = "or Account_Code = '" + value + "'"
    '                        whrCls = whrCls + stcode
    '                    End If
    '                Next
    '            Else
    '                qry = "select top 0 Account_Code , Description  from TSPL_GL_ACCOUNTS"
    '            End If
    '        Else
    '            If countsegment <> 0 Then
    '                Dim valuefirst As String = arrlocation(0)
    '                valuefirst = "%" + valuefirst
    '                qry = "select Account_Code , Description  from TSPL_GL_ACCOUNTS"
    '                whrCls = " Account_Code like  '" + valuefirst + "'  "
    '                For count As Integer = 0 To countsegment - 1
    '                    If count < countsegment - 1 Then
    '                        Dim value As String = arrlocation(count + 1)
    '                        value = "%" + value
    '                        Dim stcode As String = " or Account_Code like'" + value + "'"
    '                        whrCls = whrCls + stcode
    '                    End If
    '                Next
    '            Else
    '                qry = "select top 0 Account_Code , Description  from TSPL_GL_ACCOUNTS"
    '            End If
    '        End If
    '    End If
    '    If clsCommon.myLen(whrCls) > 0 Then
    '        whrCls = whrCls + " and ControlAccount ='Y'"
    '    Else
    '        whrCls = whrCls + " ControlAccount ='Y'"
    '    End If

    '    arrlist.Add(qry)
    '    arrlist.Add(whrCls)
    '    Return arrlist
    'End Function


    Public Shared Function glaccountquery() As String
        Dim arr As New ArrayList()
        arr = glaccountMainFunction(objCommonVar.CurrentUserCode, False)
        Return arr.Item(0) + " where " + arr.Item(1)
    End Function

    Public Shared Function glaccountquery(ByVal code As String) As ArrayList
        Return glaccountMainFunction(code, False)
    End Function

    Public Shared Function glaccountqueryForControlAcc(ByVal code As String) As ArrayList
        Return glaccountMainFunction(code, True)
    End Function

    Private Shared Function glaccountMainFunction(ByVal code As String, ByVal isControlAccount As Boolean) As ArrayList
        Dim arrlist As New ArrayList()
        Dim qry As String = ""
        Dim whrCls As String = ""
        Dim arrlocation As New ArrayList()
        Dim arraccount As New ArrayList()
        If clsCommon.CompairString(objCommonVar.CurrentUserCode, "ADMIN") = CompairStringResult.Equal Then
            qry = " select Account_Code , Description  from TSPL_GL_ACCOUNTS"
            whrCls = ""
        Else
            arraccount = connectSql.funglaccountmultiple(code)
            arrlocation = connectSql.funglsegmentmultiple(code)
            Dim straccount As String = connectSql.funglaccount(code)
            Dim countsegment As Integer = arrlocation.Count
            Dim countaccount As Integer = arraccount.Count
            If countaccount <> 0 Then
                If countsegment <> 0 Then
                    Dim valuefirstsegment As String = arrlocation(0)
                    valuefirstsegment = "%" + valuefirstsegment
                    Dim valuefirstacct As String = arraccount(0)
                    qry = " select Account_Code , Description  from TSPL_GL_ACCOUNTS"
                    whrCls = "Account_Code like '" + valuefirstsegment + "' or Account_Code = '" + valuefirstacct + "' "
                    For count As Integer = 0 To countsegment - 1
                        If count < countsegment - 1 Then
                            Dim value As String = arrlocation(count + 1)
                            value = "%" + value
                            Dim stcode As String = " or Account_Code like'" + value + "'"
                            whrCls = whrCls + stcode
                        End If
                    Next
                    For countacct As Integer = 0 To countaccount
                        If countacct < countaccount - 1 Then
                            Dim value As String = arraccount(countacct + 1)
                            Dim stcode As String = "or Account_Code = '" + value + "'"
                            whrCls = whrCls + stcode
                        End If
                    Next
                Else
                    qry = "select top 0 Account_Code , Description  from TSPL_GL_ACCOUNTS"
                End If
            Else
                If countsegment <> 0 Then
                    Dim valuefirst As String = arrlocation(0)
                    valuefirst = "%" + valuefirst
                    qry = "select Account_Code , Description  from TSPL_GL_ACCOUNTS"
                    whrCls = " Account_Code like  '" + valuefirst + "'  "
                    For count As Integer = 0 To countsegment - 1
                        If count < countsegment - 1 Then
                            Dim value As String = arrlocation(count + 1)
                            value = "%" + value
                            Dim stcode As String = " or Account_Code like'" + value + "'"
                            whrCls = whrCls + stcode
                        End If
                    Next
                Else
                    qry = "select top 0 Account_Code , Description  from TSPL_GL_ACCOUNTS"
                End If
            End If
        End If

        qry += Environment.NewLine + "left outer join (select TSPL_GL_SEGMENT_CODE.Account_Code as AccCode from TSPL_GL_SEGMENT_CODE where TSPL_GL_SEGMENT_CODE.Seg_No='7' and len(isnull(TSPL_GL_SEGMENT_CODE.Account_Code,''))>0 ) as segTable  on segTable.AccCode=TSPL_GL_ACCOUNTS.Account_Code"
        If clsCommon.myLen(whrCls) <= 0 Then
            whrCls = " 2=2 "
        End If
        whrCls += " and TSPL_GL_ACCOUNTS.Status='Y' and ( segTable.AccCode is null "
        If isControlAccount Then
            whrCls = whrCls + " and ControlAccount ='Y' )"
        Else
            whrCls = whrCls + " and ControlAccount ='N' )"
        End If
        arrlist.Add(qry)
        arrlist.Add(whrCls)
        Return arrlist
    End Function

    Public Shared Function getRandomUserCode(tblName As String, UserCodeColumnName As String, LocCode As String, LocCodeColName As String, Optional trans As SqlTransaction = Nothing) As String
        Dim rValue As String = String.Empty
        Try
            Dim qry As String = "select distinct " & UserCodeColumnName & "  from " & tblName & IIf(clsCommon.myLen(LocCodeColName) > 0, " where " & LocCodeColName & " ='" & LocCode & "'", "")
            Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry, trans)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim i As Integer = Rnd() * (dt.Rows.Count - 1)
                If i >= 0 Then
                    rValue = clsCommon.myCstr(dt.Rows(i)(UserCodeColumnName))
                Else
                    rValue = clsCommon.myCstr(dt.Rows(0)(UserCodeColumnName))
                End If
            Else
                rValue = objCommonVar.CurrentUserCode
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return rValue
    End Function
    

    Public Shared Function GetTableColumnNameForQry(ByVal strTableName As String, ByVal trans As SqlTransaction) As String
        Dim qry As String = "select COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + strTableName + "'"
        Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry, trans)

        Dim strInvColumns As String = ""
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            Dim isFirstTime As Boolean = True
            For Each dr As DataRow In dt.Rows
                If Not isFirstTime Then
                    strInvColumns += ","
                End If
                strInvColumns += clsCommon.myCstr(dr("COLUMN_NAME"))
                isFirstTime = False
            Next
        End If
        Return strInvColumns
    End Function

    Public Shared Function isCurrentUserMCC() As Boolean
        Dim qry As String = "  select count(*) from tspl_mcc_master left outer join tspl_user_master on tspl_user_master.Default_Location=tspl_mcc_master.mcc_code where tspl_user_master.user_code='" & objCommonVar.CurrentUserCode & "' "
        If clsDBFuncationality.getSingleValue(qry) = 0 Then
            Return False
        Else
            Return True
        End If
    End Function
    Public Shared Function isLocationMcc(ByVal strLoc As String) As Boolean
        Dim qry As String = "  select COUNT(*) from TSPL_LOCATION_MASTER where Location_Category='MCC' and Location_Code='" & strLoc & "' "
        If clsDBFuncationality.getSingleValue(qry) = 0 Then
            Return False
        Else
            Return True
        End If
    End Function
    Public Shared Function isLocationMcc(ByVal strLoc As String, trans As SqlTransaction) As Boolean
        Dim qry As String = "  select COUNT(*) from TSPL_LOCATION_MASTER where Location_Category='MCC' and Location_Code='" & strLoc & "' "
        If clsDBFuncationality.getSingleValue(qry, trans) = 0 Then
            Return False
        Else
            Return True
        End If
    End Function










    '    Public Shared Sub openJournalEntry(ByVal refDocNo As String)
    '        Dim qry As String = " select count(*) from TSPL_JOURNAL_MASTER where Source_Doc_No ='" & refDocNo & "' "
    '        If clsCommon.myCdbl(clsDBFuncationality.getSingleValue(qry)) = 0 Then
    '            clsCommon.MyMessageBoxShow("No Journal Entry Found For Current Document")
    '        Else
    '            Dim jNo As String = clsDBFuncationality.getSingleValue(" select Voucher_No  from TSPL_JOURNAL_MASTER where Source_Doc_No ='" & refDocNo & "' ")
    '            Dim frm As New frmJournalEntry(objCommonVar.CurrentUserCode, objCommonVar.CurrentCompanyCode)
    '            frm.strVoucherNo = jNo

    '            frm.WindowState = FormWindowState.Maximized
    '            frm.MdiParent = MDI
    '            frm.Show()
    '        End If
    '    End Sub

    '    Public Shared Sub openAPInvoiceEntry(ByVal refDocNo As String)
    '        Dim qry As String = " select count(*) from TSPL_VENDOR_INVOICE_HEAD where Description like '%" & refDocNo & "%' "
    '        If clsCommon.myCdbl(clsDBFuncationality.getSingleValue(qry)) = 0 Then
    '            clsCommon.MyMessageBoxShow("No AP Invoice Entry Found For Current Document")
    '        Else
    '            Dim ApNo As String = clsDBFuncationality.getSingleValue(" select Document_No  from TSPL_VENDOR_INVOICE_HEAD where Description like'%" & refDocNo & "%' ")
    '            Dim frm As New FrmAPInvoiceEntry()
    '            frm.strAPInvoice = ApNo
    '            frm.WindowState = FormWindowState.Maximized
    '            frm.MdiParent = MDI
    '            frm.Show()
    '        End If
    '    End Sub

    Public Shared Function exportCrystalToPDF(ByVal dt As DataTable, ByVal strReportPath As String, ByVal strSrcReportName As String, ByVal strTrgtReportName As String, ByVal strStartPath As String) As Boolean
        Try
            If dt.Rows.Count > 0 Then
                Dim rpdoc As New ReportDocument()
                Dim strReportFullPath = strReportPath & "\" & strSrcReportName & ".rpt"
                rpdoc.Load(strReportFullPath)
                rpdoc.SetDataSource(dt)
                If Not IO.Directory.Exists(strStartPath & "\pdfTemp") Then
                    IO.Directory.CreateDirectory(strStartPath & "\pdfTemp")
                End If
                rpdoc.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, strStartPath & "\pdfTemp\" & strTrgtReportName & ".pdf")
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            clsCommon.MyMessageBoxShow(ex.Message)
            Return False
        End Try

    End Function


    '#Region "Script Function"
    '    '------------------New Subroutine Make For Script Running-----Done By-Monika-28/05/2014---BM00000003099-------------------------------------------
    '    Private Shared Function CheckPrimaryKey(ByVal table_name As String, ByVal column_name As String, ByVal trans As SqlTransaction, Optional ByVal isDefault_Type As Boolean = False) As Boolean
    '        Dim qry As String = "select count(*) from INFORMATION_SCHEMA.TABLES where table_name='" + table_name + "'"
    '        Dim check As Integer = clsDBFuncationality.getSingleValue(qry, trans)

    '        If check <= 0 Then
    '            Return True
    '        End If

    '        If isDefault_Type = False Then
    '            qry = "select column_name from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE where table_name='" + table_name + "' and column_name='" + column_name + "'"
    '            Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry, trans)

    '            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
    '                Return True
    '            Else
    '                Return False
    '            End If
    '        Else
    '            qry = "Select  SysObjects.[Name] As [Name] From SysObjects Inner Join (Select [Name],[ID] From SysObjects) As Tab On Tab.[ID] = Sysobjects.[Parent_Obj] Inner Join sysconstraints On sysconstraints.Constid = Sysobjects.[ID] Inner Join SysColumns Col On Col.[ColID] = sysconstraints.[ColID] And Col.[ID] = Tab.[ID] where Tab.name='" + table_name + "' and Col.name ='" + column_name + "'"
    '            Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry, trans)

    '            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
    '                Return True
    '            Else
    '                Return False
    '            End If
    '        End If
    '    End Function

    '    Private Shared Sub DropConstraint(ByVal table_name As String, ByVal column_name As String, ByVal trans As SqlTransaction)
    '        Dim qry As String = "select count(*) from INFORMATION_SCHEMA.TABLES where table_name='" + table_name + "'"
    '        Dim check As Integer = clsDBFuncationality.getSingleValue(qry, trans)

    '        If check <= 0 Then
    '            Exit Sub
    '        End If

    '        qry = "select CONSTRAINT_NAME from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE where table_name='" + table_name + "' and column_name='" + column_name + "'"
    '        Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry, trans)

    '        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
    '            For Each dr As DataRow In dt.Rows
    '                qry = "alter table " + table_name + " drop constraint " + clsCommon.myCstr(dr("CONSTRAINT_NAME")) + ""
    '                clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '            Next
    '        Else
    '            'Exit Function
    '        End If

    '        'qry = "select name from sys.objects where type_desc like '%constraint%' and object_name(parent_object_id)='" + table_name + "' and name like '%_" + column_name + "_%'"
    '        'dt = New DataTable()
    '        'dt = clsDBFuncationality.GetDataTable(qry, trans)

    '        'If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
    '        '    For Each dr As DataRow In dt.Rows
    '        '        qry = "alter table " + table_name + " drop constraint " + clsCommon.myCstr(dr("NAME")) + ""
    '        '        clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '        '    Next
    '        'Else
    '        '    ' Exit Function
    '        'End If

    '        ''added by richa agarwal on 29/09/2014
    '        qry = "Select  SysObjects.[Name] As [Name] From SysObjects Inner Join (Select [Name],[ID] From SysObjects) As Tab On Tab.[ID] = Sysobjects.[Parent_Obj] Inner Join sysconstraints On sysconstraints.Constid = Sysobjects.[ID] Inner Join SysColumns Col On Col.[ColID] = sysconstraints.[ColID] And Col.[ID] = Tab.[ID] where Tab.name='" + table_name + "' and Col.name ='" + column_name + "'"
    '        dt = clsDBFuncationality.GetDataTable(qry, trans)

    '        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
    '            For Each dr As DataRow In dt.Rows
    '                qry = "alter table " + table_name + " drop constraint " + clsCommon.myCstr(dr("NAME")) + ""
    '                clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '            Next
    '        Else
    '            Exit Sub
    '        End If
    '        ''========================================
    '    End Sub

    '    'Private Shared Function CheckColumnExist(ByVal table_name As String, ByVal column_name As String, ByVal datatype As String, ByVal trans As SqlTransaction) As Integer
    '    '    Dim qry As String = ""
    '    '    If clsCommon.myLen(datatype) > 0 Then
    '    '        qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='" + datatype + "'"
    '    '    Else
    '    '        qry = "select count(*) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + table_name + "' and COLUMN_NAME='" + column_name + "'"
    '    '    End If
    '    '    Dim check As Integer = clsDBFuncationality.getSingleValue(qry, trans)

    '    '    Return check
    '    'End Function
    '    ''By Balwinder due to not work properly on 2014-10-07
    '    Private Shared Function CheckColumnExist(ByVal table_name As String, ByVal column_name As String, ByVal datatype As DBDataType, ByVal MaxLength As Integer, ByVal ScaleForDecimal As Integer, ByVal trans As SqlTransaction) As Integer
    '        Dim qry As String = ""
    '        'If clsCommon.myLen(datatype) > 0 Then
    '        '    qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='" + datatype + "'"
    '        'Else
    '        '    qry = "select count(*) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + table_name + "' and COLUMN_NAME='" + column_name + "'"
    '        'End If

    '        Select Case datatype
    '            Case DBDataType.image_Type
    '                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='image'"
    '            Case DBDataType.int_Type
    '                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='int'"
    '            Case DBDataType.decimal_Type
    '                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='decimal'"
    '                If MaxLength > 0 Then
    '                    qry += " and NUMERIC_PRECISION='" + clsCommon.myCstr(MaxLength) + "'"
    '                End If
    '                If ScaleForDecimal > 0 Then
    '                    qry += " and NUMERIC_SCALE='" + clsCommon.myCstr(ScaleForDecimal) + "'"
    '                End If
    '            Case DBDataType.varbinary_Type
    '                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='varbinary'"
    '            Case DBDataType.text_Type
    '                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='text' "
    '            Case DBDataType.datetime_Type
    '                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='datetime' "
    '            Case DBDataType.time_Type
    '                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='time' "
    '            Case DBDataType.varchar_Type
    '                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='varchar' "
    '                If MaxLength > 0 Then
    '                    qry += " and CHARACTER_MAXIMUM_LENGTH='" + clsCommon.myCstr(MaxLength) + "'"
    '                End If
    '            Case DBDataType.numeric_Type
    '                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='numeric'"
    '                If MaxLength > 0 Then
    '                    qry += " and NUMERIC_PRECISION='" + clsCommon.myCstr(MaxLength) + "'"
    '                End If
    '                If ScaleForDecimal > 0 Then
    '                    qry += " and NUMERIC_SCALE='" + clsCommon.myCstr(ScaleForDecimal) + "'"
    '                End If
    '            Case DBDataType.nchar_Type
    '                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='nchar'"
    '            Case DBDataType.float_Type
    '                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='float'"
    '            Case DBDataType.date_Type
    '                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='date'"
    '            Case DBDataType.char_Type
    '                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='char'"
    '                If MaxLength > 0 Then
    '                    qry += " and CHARACTER_MAXIMUM_LENGTH='" + clsCommon.myCstr(MaxLength) + "'"
    '                End If
    '            Case DBDataType.bigint_Type
    '                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='bigint'"
    '            Case DBDataType.bit_Type
    '                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='bit'"
    '            Case DBDataType.nvarchar_Type
    '                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='nvarchar'"
    '                If MaxLength > 0 Then
    '                    qry += " and CHARACTER_MAXIMUM_LENGTH='" + clsCommon.myCstr(MaxLength) + "'"
    '                End If
    '            Case Else
    '                qry = "select count(*) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + table_name + "' and COLUMN_NAME='" + column_name + "'"
    '        End Select

    '        Dim check As Integer = clsDBFuncationality.getSingleValue(qry, trans)

    '        Return check
    '    End Function

    '    Public Shared Function CheckTriggerExits(ByVal trg_name As String, ByVal trans As SqlTransaction) As Integer
    '        Try
    '            Dim sQuery = "SELECT count(*) FROM sys.triggers where name='" & trg_name & "'"
    '            Dim check As Integer = clsDBFuncationality.getSingleValue(sQuery, trans)
    '            Return check
    '        Catch ex As Exception
    '            clsCommon.MyMessageBoxShow(ex.ToString)
    '        End Try
    '    End Function

    '    Public Shared Sub Pre_AlterOrUpdateScript(ByVal exeVersion As String)
    '        Dim qry As String = ""
    '        Dim check As Integer = 0
    '        Dim trans As SqlTransaction = clsDBFuncationality.GetTransactin()
    '        Dim dt As DataTable = Nothing
    '        Try
    '            If (clsCommon.CompairString("5.0.0.92", exeVersion) = CompairStringResult.Less Or clsCommon.CompairString(exeVersion, "5.0.0.92") = CompairStringResult.Equal) Then
    '                '------------check already have primary key or not------------------
    '                If clsERPFuncationality.CheckPrimaryKey("tspl_vendor_master", "vendor_code", trans) = True Then
    '                Else
    '                    qry = "alter table tspl_vendor_master add primary key(vendor_code)"
    '                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '                End If
    '                '-----------------------------------------------------------------------------
    '            End If

    '            If (clsCommon.CompairString("5.0.0.98", exeVersion) = CompairStringResult.Less Or clsCommon.CompairString(exeVersion, "5.0.0.98") = CompairStringResult.Equal) Then
    '                If clsERPFuncationality.CheckPrimaryKey("tspl_village_master", "village_code", trans) = True Then
    '                Else
    '                    qry = "alter table tspl_village_master add primary key(village_code)"
    '                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '                End If

    '                If clsERPFuncationality.CheckPrimaryKey("tspl_vlc_master_detail", "village_code", trans) = True Then
    '                Else
    '                    qry = "alter table tspl_vlc_master_detail add FOREIGN KEY(village_code) references TSPL_VILLAGE_MASTER(village_code)"
    '                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '                End If

    '                If clsERPFuncationality.CheckPrimaryKey("TSPL_MCC_MASTER", "city_code", trans) = True Then
    '                Else
    '                    qry = "alter table TSPL_MCC_MASTER add FOREIGN KEY(city_code) references TSPL_CITY_MASTER(city_code)"
    '                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '                End If
    '                If clsERPFuncationality.CheckPrimaryKey("TSPL_MCC_MASTER", "state_code", trans) = True Then
    '                Else
    '                    qry = "alter table TSPL_MCC_MASTER add FOREIGN KEY(state_code) references TSPL_state_MASTER(state_code)"
    '                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '                End If
    '                If clsERPFuncationality.CheckPrimaryKey("TSPL_MCC_MASTER", "country_code", trans) = True Then
    '                Else
    '                    qry = "alter table TSPL_MCC_MASTER add FOREIGN KEY(country_code) references TSPL_country_MASTER(country_code)"
    '                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '                End If
    '            End If
    '            If (clsCommon.CompairString("5.0.1.36", exeVersion) = CompairStringResult.Less Or clsCommon.CompairString("5.0.1.36", exeVersion) = CompairStringResult.Equal) Then
    '                DropConstraint("tspl_location_master", "category_struct_code", trans)
    '                DropConstraint("tspl_vendor_master", "category_struct_code", trans)
    '                DropConstraint("tspl_customer_master", "category_struct_code", trans)
    '                DropConstraint("TSPL_PP_BOM_ITEM_DETAIL", "bom_code", trans)
    '                DropConstraint("TSPL_PP_BOM_STAGE_DETAIL", "bom_code", trans)
    '            End If


    '            If (clsCommon.CompairString("5.0.2.37", exeVersion) = CompairStringResult.Less Or clsCommon.CompairString("5.0.2.37", exeVersion) = CompairStringResult.Equal) Then
    '                'check = CheckColumnExist("tspl_mcc_dispatch_challan", "payment_rate", DBDataType.varchar_Type, -1, 0, trans)
    '                'If check > 0 Then
    '                '    qry = "alter table tspl_mcc_dispatch_challan drop column payment_rate"
    '                '    clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '                'End If

    '                'check = CheckColumnExist("tspl_mcc_dispatch_challan", "tanker_transporter_name", DBDataType.varchar_Type, 30, 0, trans)
    '                'If check > 0 Then
    '                '    qry = "alter table tspl_mcc_dispatch_challan drop column tanker_transporter_name"
    '                '    clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '                'End If


    '                check = CheckColumnExist("tspl_vendor_master", "city_code", DBDataType.varchar_Type, 12, 0, trans)
    '                If check > 0 Then
    '                    qry = "alter table tspl_vendor_master alter column city_code varchar(50)"
    '                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '                End If
    '            End If

    '            If (clsCommon.CompairString("5.0.3.19", exeVersion) = CompairStringResult.Less Or clsCommon.CompairString(exeVersion, "5.0.3.19") = CompairStringResult.Equal) Then

    '                qry = "delete from TSPL_TRANSACTION_REVERSE_LOG where Program_Code in (select Program_Code from  TSPL_PROGRAM_MASTER where Parent_Code in (select Program_Code from TSPL_PROGRAM_MASTER where Parent_Code in(select Program_Code from TSPL_PROGRAM_MASTER where Program_Code='MProduction')))"
    '                clsDBFuncationality.ExecuteNonQuery(qry, trans)

    '                qry = "delete from  TSPL_PROGRAM_MASTER where Parent_Code in (select Program_Code from TSPL_PROGRAM_MASTER where Parent_Code in(select Program_Code from TSPL_PROGRAM_MASTER where Program_Code='MProduction'))"
    '                clsDBFuncationality.ExecuteNonQuery(qry, trans)

    '                qry = "delete from TSPL_PROGRAM_MASTER where Parent_Code=(select Program_Code from TSPL_PROGRAM_MASTER where Program_Code='MProduction')"
    '                clsDBFuncationality.ExecuteNonQuery(qry, trans)

    '                qry = "delete from TSPL_PROGRAM_MASTER where Program_Code='MProduction'"
    '                clsDBFuncationality.ExecuteNonQuery(qry, trans)


    '                '-----------------------------------------------------------------------------
    '            End If
    '            'By pankaj jha to update vendor master delete stored procedure
    '            If (clsCommon.CompairString("5.0.3.58", exeVersion) = CompairStringResult.Less Or clsCommon.CompairString(exeVersion, "5.0.3.58") = CompairStringResult.Equal) Then

    '                qry = " ALTER PROCEDURE [dbo].[sp_TSPL_VENDOR_MASTER_DELETE](@Vendor_Code varchar(12),@form_type varchar(10)) AS  BEGIN  delete from TSPL_MCC_ROUTE_VLC_MAPPING where VLC_CODE=(select VLC_CODE from TSPL_VLC_MASTER_HEad  where VSP_Code=@Vendor_Code ) delete from TSPL_VLC_MASTER_HEAD  where VSP_Code =@Vendor_Code  delete from tSPL_MCC_VSP_ChargeCategory_MAPPING where VSP_CODE =@Vendor_Code  DELETE TSPL_VENDOR_MASTER WHERE Vendor_Code=@Vendor_Code and Form_Type='VSP'  end "
    '                clsDBFuncationality.ExecuteNonQuery(qry, trans)

    '                '-----------------------------------------------------------------------------
    '            End If

    '            'By Rohit to update vendor master delete stored procedure
    '            If (clsCommon.CompairString("5.0.4.65", exeVersion) = CompairStringResult.Less Or clsCommon.CompairString(exeVersion, "5.0.4.65") = CompairStringResult.Equal) Then

    '                qry = "  ALTER PROCEDURE [dbo].[sp_TSPL_VENDOR_MASTER_DELETE](@Vendor_Code varchar(12),@form_type varchar(10)) AS  if (@form_type='VSP')  begin delete from TSPL_MCC_ROUTE_VLC_MAPPING where VLC_CODE=(select VLC_CODE from TSPL_VLC_MASTER_HEad  where VSP_Code=@Vendor_Code )    delete from TSPL_VLC_MASTER_HEAD  where VSP_Code =@Vendor_Code delete from tSPL_MCC_VSP_ChargeCategory_MAPPING where VSP_CODE =@Vendor_Code DELETE TSPL_VENDOR_MASTER WHERE Vendor_Code=@Vendor_Code and Form_Type=@form_type end ELSE begin DELETE TSPL_VENDOR_MASTER WHERE Vendor_Code=@Vendor_Code and Form_Type=@form_type end"
    '                clsDBFuncationality.ExecuteNonQuery(qry, trans)

    '                '-----------------------------------------------------------------------------
    '            End If

    '            'If (clsCommon.CompairString("5.0.3.64", exeVersion) = CompairStringResult.Less Or clsCommon.CompairString(exeVersion, "5.0.3.64") = CompairStringResult.Equal) Then

    '            '    qry = "declare @sql varchar(1000)" & _
    '            '          " SELECT @sql = 'ALTER TABLE ' + 'TSPL_VLC_DATA_UPLOADER' " & _
    '            '          " + ' DROP CONSTRAINT  ' + name + ';' " & _
    '            '          " FROM sys.key_constraints " & _
    '            '          " WHERE [type] = 'PK' " & _
    '            '          " AND [parent_object_id] = OBJECT_ID('TSPL_VLC_DATA_UPLOADER');" & _
    '            '          " EXEC(@sql)"
    '            '    clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '            '    '-----------------------------------------------------------------------------

    '            '    qry = "declare @sql varchar(1000)" & _
    '            '          " SELECT @sql = 'ALTER TABLE ' + 'TSPL_VLC_DATA_UPLOADER_Detail' " & _
    '            '          " + ' DROP CONSTRAINT  ' + name + ';' " & _
    '            '          " FROM sys.key_constraints " & _
    '            '          " WHERE [type] = 'PK' " & _
    '            '          " AND [parent_object_id] = OBJECT_ID('TSPL_VLC_DATA_UPLOADER_Detail');" & _
    '            '          " EXEC(@sql)"
    '            '    clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '            '    '-----------------------------------------------------------------------------

    '            '    qry = "declare @sql varchar(1000)" & _
    '            '          " SELECT @sql = 'ALTER TABLE ' + 'TSPL_INVENTORY_MOVEMENT_NEW' " & _
    '            '          " + ' DROP CONSTRAINT  ' + name + ';' " & _
    '            '          " FROM sys.key_constraints " & _
    '            '          " WHERE [type] = 'PK' " & _
    '            '          " AND [parent_object_id] = OBJECT_ID('TSPL_INVENTORY_MOVEMENT_NEW');" & _
    '            '          " EXEC(@sql)"
    '            '    clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '            '    '-----------------------------------------------------------------------------

    '            '    qry = "declare @sql varchar(1000)" & _
    '            '          " SELECT @sql = 'ALTER TABLE ' + 'TSPL_VENDOR_INVOICE_Detail' " & _
    '            '          " + ' DROP CONSTRAINT  ' + name + ';' " & _
    '            '          " FROM sys.key_constraints " & _
    '            '          " WHERE [type] = 'PK' " & _
    '            '          " AND [parent_object_id] = OBJECT_ID('TSPL_VENDOR_INVOICE_Detail');" & _
    '            '          " EXEC(@sql)"
    '            '    clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '            '    '-----------------------------------------------------------------------------

    '            '    qry = "declare @sql varchar(1000)" & _
    '            '          " SELECT @sql = 'ALTER TABLE ' + 'TSPL_JOURNAL_Details' " & _
    '            '          " + ' DROP CONSTRAINT  ' + name + ';' " & _
    '            '          " FROM sys.key_constraints " & _
    '            '          " WHERE [type] = 'PK' " & _
    '            '          " AND [parent_object_id] = OBJECT_ID('TSPL_JOURNAL_Details');" & _
    '            '          " EXEC(@sql)"
    '            '    clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '            '    '-----------------------------------------------------------------------------

    '            '    qry = "declare @sql varchar(1000)" & _
    '            '          " SELECT @sql = 'ALTER TABLE ' + 'TSPL_MILK_RECEIPT_DETAIL' " & _
    '            '          " + ' DROP CONSTRAINT  ' + name + ';' " & _
    '            '          " FROM sys.key_constraints " & _
    '            '          " WHERE [type] = 'PK' " & _
    '            '          " AND [parent_object_id] = OBJECT_ID('TSPL_MILK_RECEIPT_DETAIL');" & _
    '            '          " EXEC(@sql)"
    '            '    clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '            '    '-----------------------------------------------------------------------------

    '            '    qry = "declare @sql varchar(1000)" & _
    '            '        " SELECT @sql = 'ALTER TABLE ' + 'TSPL_MILK_SAMPLE_DETAIL' " & _
    '            '        " + ' DROP CONSTRAINT  ' + name + ';' " & _
    '            '        " FROM sys.key_constraints " & _
    '            '        " WHERE [type] = 'PK' " & _
    '            '        " AND [parent_object_id] = OBJECT_ID('TSPL_MILK_SAMPLE_DETAIL');" & _
    '            '        " EXEC(@sql)"
    '            '    clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '            '    '-----------------------------------------------------------------------------

    '            '    qry = "declare @sql varchar(1000)" & _
    '            '        " SELECT @sql = 'ALTER TABLE ' + 'TSPL_MILK_SAMPLE_DETAIL_History' " & _
    '            '        " + ' DROP CONSTRAINT  ' + name + ';' " & _
    '            '        " FROM sys.key_constraints " & _
    '            '        " WHERE [type] = 'PK' " & _
    '            '        " AND [parent_object_id] = OBJECT_ID('TSPL_MILK_SAMPLE_DETAIL_History');" & _
    '            '        " EXEC(@sql)"
    '            '    clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '            '    '-----------------------------------------------------------------------------

    '            '    qry = "declare @sql varchar(1000)" & _
    '            '      " SELECT @sql = 'ALTER TABLE ' + 'TSPL_MILK_Shift_End_DETAIL' " & _
    '            '      " + ' DROP CONSTRAINT  ' + name + ';' " & _
    '            '      " FROM sys.key_constraints " & _
    '            '      " WHERE [type] = 'PK' " & _
    '            '      " AND [parent_object_id] = OBJECT_ID('TSPL_MILK_Shift_End_DETAIL');" & _
    '            '      " EXEC(@sql)"
    '            '    clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '            '    '-----------------------------------------------------------------------------

    '            '    qry = "declare @sql varchar(1000)" & _
    '            '      " SELECT @sql = 'ALTER TABLE ' + 'TSPL_MILK_Shift_End_Route_DETAIL' " & _
    '            '      " + ' DROP CONSTRAINT  ' + name + ';' " & _
    '            '      " FROM sys.key_constraints " & _
    '            '      " WHERE [type] = 'PK' " & _
    '            '      " AND [parent_object_id] = OBJECT_ID('TSPL_MILK_Shift_End_Route_DETAIL');" & _
    '            '      " EXEC(@sql)"
    '            '    clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '            '    '-----------------------------------------------------------------------------

    '            '    qry = "declare @sql varchar(1000)" & _
    '            '     " SELECT @sql = 'ALTER TABLE ' + 'TSPL_MILK_SRN_DETAIL' " & _
    '            '     " + ' DROP CONSTRAINT  ' + name + ';' " & _
    '            '     " FROM sys.key_constraints " & _
    '            '     " WHERE [type] = 'PK' " & _
    '            '     " AND [parent_object_id] = OBJECT_ID('TSPL_MILK_SRN_DETAIL');" & _
    '            '     " EXEC(@sql)"
    '            '    clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '            '    '-----------------------------------------------------------------------------

    '            '    qry = "declare @sql varchar(1000)" & _
    '            '    " SELECT @sql = 'ALTER TABLE ' + 'TSPL_MILK_SRN_Price_Charge_Detail' " & _
    '            '    " + ' DROP CONSTRAINT  ' + name + ';' " & _
    '            '    " FROM sys.key_constraints " & _
    '            '    " WHERE [type] = 'PK' " & _
    '            '    " AND [parent_object_id] = OBJECT_ID('TSPL_MILK_SRN_Price_Charge_Detail');" & _
    '            '    " EXEC(@sql)"
    '            '    clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '            '    '-----------------------------------------------------------------------------

    '            '    qry = "declare @sql varchar(1000)" & _
    '            '    " SELECT @sql = 'ALTER TABLE ' + 'TSPL_MILK_SRN_VSP_Charge_Detail' " & _
    '            '    " + ' DROP CONSTRAINT  ' + name + ';' " & _
    '            '    " FROM sys.key_constraints " & _
    '            '    " WHERE [type] = 'PK' " & _
    '            '    " AND [parent_object_id] = OBJECT_ID('TSPL_MILK_SRN_VSP_Charge_Detail');" & _
    '            '    " EXEC(@sql)"
    '            '    clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '            '    '-----------------------------------------------------------------------------

    '            '    qry = "declare @sql varchar(1000)" & _
    '            '    " SELECT @sql = 'ALTER TABLE ' + 'TSPL_MILK_SRN_VSP_Charge_Detail' " & _
    '            '    " + ' DROP CONSTRAINT  ' + name + ';' " & _
    '            '    " FROM sys.key_constraints " & _
    '            '    " WHERE [type] = 'PK' " & _
    '            '    " AND [parent_object_id] = OBJECT_ID('TSPL_MILK_SRN_VSP_Charge_Detail');" & _
    '            '    " EXEC(@sql)"
    '            '    clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '            '    '-----------------------------------------------------------------------------

    '            '    qry = "declare @sql varchar(1000)" & _
    '            '    " SELECT @sql = 'ALTER TABLE ' + 'Tspl_Milk_Truck_Sheet_Detail' " & _
    '            '    " + ' DROP CONSTRAINT  ' + name + ';' " & _
    '            '    " FROM sys.key_constraints " & _
    '            '    " WHERE [type] = 'PK' " & _
    '            '    " AND [parent_object_id] = OBJECT_ID('Tspl_Milk_Truck_Sheet_Detail');" & _
    '            '    " EXEC(@sql)"
    '            '    clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '            '    '-----------------------------------------------------------------------------

    '            '    qry = "declare @sql varchar(1000)" & _
    '            '    " SELECT @sql = 'ALTER TABLE ' + 'TSPL_Milk_Purchase_Invoice_Incentive_Detail' " & _
    '            '    " + ' DROP CONSTRAINT  ' + name + ';' " & _
    '            '    " FROM sys.key_constraints " & _
    '            '    " WHERE [type] = 'PK' " & _
    '            '    " AND [parent_object_id] = OBJECT_ID('TSPL_Milk_Purchase_Invoice_Incentive_Detail');" & _
    '            '    " EXEC(@sql)"
    '            '    clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '            '    '-----------------------------------------------------------------------------

    '            '    qry = "declare @sql varchar(1000)" & _
    '            '    " SELECT @sql = 'ALTER TABLE ' + 'TSPL_MCC_RATE_UPLOADER_MCC' " & _
    '            '    " + ' DROP CONSTRAINT  ' + name + ';' " & _
    '            '    " FROM sys.key_constraints " & _
    '            '    " WHERE [type] = 'PK' " & _
    '            '    " AND [parent_object_id] = OBJECT_ID('TSPL_MCC_RATE_UPLOADER_MCC');" & _
    '            '    " EXEC(@sql)"
    '            '    clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '            '    '-----------------------------------------------------------------------------

    '            '    qry = "declare @sql varchar(1000)" & _
    '            '    " SELECT @sql = 'ALTER TABLE ' + 'TSPL_MCC_RATE_UPLOADER_Detail' " & _
    '            '    " + ' DROP CONSTRAINT  ' + name + ';' " & _
    '            '    " FROM sys.key_constraints " & _
    '            '    " WHERE [type] = 'PK' " & _
    '            '    " AND [parent_object_id] = OBJECT_ID('TSPL_MCC_RATE_UPLOADER_Detail');" & _
    '            '    " EXEC(@sql)"
    '            '    clsDBFuncationality.ExecuteNonQuery(qry, trans)
    '            '    '-----------------------------------------------------------------------------

    '            'End If

    '            If (clsCommon.CompairString("5.0.3.74", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.3.74") = CompairStringResult.Equal) Then
    '                DropConstraint("tspl_physical_stock", "Item_Code", trans)
    '                DropConstraint("tspl_physical_stock", "Location", trans)
    '                DropConstraint("tspl_physical_stock", "MRP", trans)
    '                DropConstraint("tspl_physical_stock", "Stock_Date", trans)
    '            End If
    '            trans.Commit()
    '        Catch ex As Exception
    '            trans.Rollback()
    '            clsCommon.MyMessageBoxShow(ex.Message)
    '        End Try
    '    End Sub




    '    Public Shared Sub UpdateCompCodes(ByVal ParamArray CompCode As String())
    '        Try
    '            If CompCode IsNot Nothing AndAlso CompCode.Length > 0 And clsCommon.myLen(objCommonVar.CurrentCompanyCode) > 0 Then
    '                For Each filedname As String In CompCode
    '                    Dim qry As String = "update " + filedname + " set comp_code='" + objCommonVar.CurrentCompanyCode + "' where isnull(comp_code,'')=''"
    '                    clsDBFuncationality.ExecuteNonQuery(qry)
    '                Next
    '            End If
    '        Catch ex As Exception
    '            clsCommon.MyMessageBoxShow(ex.Message)
    '        End Try
    '    End Sub
    '    '-------------------------End By Monika-------------------------------------


    '#End Region


    Public Shared Function SetCustomizedPaperSize(ByRef rpdoc As ReportDocument, ByVal ePaperSize As EnumTecxpertPaperSize)
        Try
            If ePaperSize <> EnumTecxpertPaperSize.NA Then
                Dim strPaperSize As String = GetTecxpertPaperSizeName(ePaperSize)
                Dim isFound As Boolean = False
                Dim i As Integer
                Dim doctoprint As New System.Drawing.Printing.PrintDocument()
                ''doctoprint.PrinterSettings.PrinterName = "Auto Xerox Phaser 3117 on SERVER"
                Dim rawKind As Integer
                For i = 0 To doctoprint.PrinterSettings.PaperSizes.Count - 1
                    If clsCommon.CompairString(doctoprint.PrinterSettings.PaperSizes(i).PaperName, strPaperSize) = CompairStringResult.Equal Then
                        rawKind = CInt(doctoprint.PrinterSettings.PaperSizes(i).GetType().GetField("kind", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic).GetValue(doctoprint.PrinterSettings.PaperSizes(i)))
                        isFound = True
                        Exit For
                    End If
                Next
                If Not isFound Then
                    Throw New Exception("Paper size " + strPaperSize + " not exist.Please Make it before Print.")
                End If
                rpdoc.PrintOptions.PaperSize = CType(rawKind, CrystalDecisions.Shared.PaperSize)
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return rpdoc
    End Function
    '    Public Shared Function glbankquery(ByRef out As String) As String
    '        out = ""
    '        Dim query As String
    '        If objCommonVar.CurrentUserCode = "ADMIN" Then
    '            query = "select BANK_CODE as [Bank Code], DESCRIPTION,BANKACCNUMBER as [Bank Account No]  from TSPL_BANK_MASTER "
    '        Else
    '            Dim arrlocation As New ArrayList()
    '            Dim arraccount As New ArrayList()

    '            arraccount = connectSql.funglaccountmultiple(objCommonVar.CurrentUserCode)
    '            arrlocation = connectSql.funglsegmentmultiple(objCommonVar.CurrentUserCode)
    '            Dim countsegment As Integer = arrlocation.Count
    '            Dim countaccount As Integer = arraccount.Count
    '            If countaccount <> 0 Then
    '                If countsegment <> 0 Then
    '                    Dim valuefirstsegment As String = arrlocation(0)
    '                    valuefirstsegment = "%" + valuefirstsegment
    '                    Dim valuefirstacct As String = arraccount(0)
    '                    query = "select BANK_CODE as [Bank Code], DESCRIPTION ,BANKACCNUMBER as [Bank Account No] from TSPL_BANK_MASTER "
    '                    out = " bankacc like '" + valuefirstsegment + "' OR bankacc ='" + valuefirstacct + "'"
    '                    For count As Integer = 0 To countsegment - 1
    '                        If count < countsegment - 1 Then
    '                            Dim value As String = arrlocation(count + 1)
    '                            value = "%" + value
    '                            Dim stcode As String = "or bankacc like'" + value + "'"
    '                            out += stcode
    '                        End If
    '                    Next
    '                    For countacct As Integer = 0 To countaccount
    '                        If countacct < countaccount - 1 Then
    '                            Dim value As String = arraccount(countacct + 1)
    '                            Dim stcode As String = "or bankacc = '" + value + "'"
    '                            out += stcode
    '                        End If
    '                    Next
    '                Else
    '                    query = "select top 0 BANK_CODE as [Bank Code], DESCRIPTION, BANKACCNUMBER as [Bank Account No] from TSPL_BANK_MASTER"

    '                End If
    '            Else
    '                If countsegment <> 0 Then
    '                    Dim valuefirstsegment As String = arrlocation(0)
    '                    valuefirstsegment = "%" + valuefirstsegment
    '                    Dim valuefirstacct As String = String.Empty
    '                    query = "select BANK_CODE as [Bank Code], DESCRIPTION,BANKACCNUMBER as [Bank Account No]  from TSPL_BANK_MASTER "
    '                    out = " bankacc like '" + valuefirstsegment + "' OR bankacc ='" + valuefirstacct + "'"
    '                    For count As Integer = 0 To countsegment - 1
    '                        If count < countsegment - 1 Then
    '                            Dim value As String = arrlocation(count + 1)
    '                            value = "%" + value
    '                            Dim stcode As String = "or bankacc like'" + value + "'"
    '                            out += stcode
    '                        End If
    '                    Next
    '                    For countacct As Integer = 0 To countaccount
    '                        If countacct < countaccount - 1 Then
    '                            Dim value As String = arraccount(countacct + 1)
    '                            Dim stcode As String = "or bankacc = '" + value + "'"
    '                            out += stcode
    '                        End If
    '                    Next
    '                Else
    '                    query = "select top 0 BANK_CODE as [Bank Code], DESCRIPTION,BANKACCNUMBER as [Bank Account No]  from TSPL_BANK_MASTER"
    '                End If
    '            End If
    '        End If


    '        Return query
    '    End Function
    Public Shared Function GetTecxpertPaperSizeName(ByVal En As EnumTecxpertPaperSize) As String
        Dim str As String = ""
        Select Case En
            Case EnumTecxpertPaperSize.PaperSize10x12
                str = "Tecxpert 10x12"
            Case EnumTecxpertPaperSize.PaperSize10x6
                str = "Tecxpert 10x6"
            Case EnumTecxpertPaperSize.Guntur10x12
                str = "Guntur 10x12"
            Case EnumTecxpertPaperSize.HalfLegal85x7
                str = "Halflegal 8.5x7"
        End Select
        Return str
    End Function

    Public Shared Function GetConstraint(ByVal strTableName As String, ByVal strColumnName As String) As String
        Dim str As String = "SELECT   dc.name AS DefaultConstraintName " +
" FROM   sys.all_columns c " +
" JOIN sys.tables t ON c.object_id = t.object_id " +
" JOIN sys.schemas s ON t.schema_id = s.schema_id " +
"LEFT JOIN sys.default_constraints dc ON c.default_object_id = dc.object_id LEFT JOIN INFORMATION_SCHEMA.COLUMNS SC ON (SC.TABLE_NAME = t.name AND SC.COLUMN_NAME = c.name) " +
"WHERE  SC.COLUMN_DEFAULT IS NOT NULL and t.name = '" + strTableName + "' and c.name = '" + strColumnName + "'"
        Return clsCommon.myCstr(clsDBFuncationality.getSingleValue(str))
    End Function

    Public Shared Function GetConstraintWorking(ByVal strTableName As String, ByVal strColumnName As String) As String
        Dim str As String = "select * from (" + Environment.NewLine + _
        " SELECT f.name AS ForeignKey, OBJECT_NAME(f.parent_object_id) AS TableName," + Environment.NewLine + _
        " COL_NAME(fc.parent_object_id, fc.parent_column_id) AS ColumnName," + Environment.NewLine + _
        " OBJECT_NAME (f.referenced_object_id) AS ReferenceTableName," + Environment.NewLine + _
        " COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS ReferenceColumnName" + Environment.NewLine + _
        " FROM sys.foreign_keys AS f " + Environment.NewLine + _
        " INNER JOIN sys.foreign_key_columns AS fc" + Environment.NewLine + _
        " ON f.OBJECT_ID = fc.constraint_object_id " + Environment.NewLine + _
        " )xx where TableName='" + strTableName + "' and ColumnName='" + strColumnName + "'"
        Return clsCommon.myCstr(clsDBFuncationality.getSingleValue(str))
    End Function
    'Public Shared Sub GenerateExcelChart(ByVal dt As DataTable, ByVal EnuChartType As Integer, ByVal Title As String, ByVal LabelColumn As String, ByVal ValuColumn1 As String, Optional ByVal ValuColumn2 As String = "")
    '    Try
    '        Dim excel As New Excel.Application
    '        excel.Visible = True
    '        excel.Workbooks.Add()
    '        excel.Range("A1").Value2 = LabelColumn
    '        excel.Range("B1").Value2 = ValuColumn1
    '        If clsCommon.myLen(ValuColumn2) > 0 Then
    '            excel.Range("C1").Value2 = ValuColumn2
    '        End If

    '        Dim ii As Integer = 2
    '        For Each dr As DataRow In dt.Rows
    '            excel.Range("A" & ii).Value2 = dr(LabelColumn)
    '            excel.Range("B" & ii).Value2 = dr(ValuColumn1)
    '            If clsCommon.myLen(ValuColumn2) > 0 Then
    '                excel.Range("C" & ii).Value2 = dr(ValuColumn2)
    '            End If
    '            ii += 1
    '        Next
    '        Dim range As Excel.Range = excel.Range("A1")
    '        Dim chart As Excel.Chart = excel.ActiveWorkbook.Charts.Add()
    '        chart.ChartWizard(Source:=range.CurrentRegion, Title:=Title)
    '        chart.ChartStyle = 21
    '        chart.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xl3DBarStacked100
    '    Catch ex As Exception
    '        Throw New Exception(ex.Message)
    '    End Try
    'End Sub

    Public Shared Function myDclInZeroPointFive(ByVal val As Object) As Decimal
        Dim retVal As Decimal = 0.0
        Try
            retVal = Math.Round(Math.Round(clsCommon.myCdbl(val) * 2, MidpointRounding.ToEven) / 2, 1, MidpointRounding.ToEven)
        Catch ex As Exception
        End Try
        Return retVal
    End Function
    Public Shared Function ShowHistoryData(ByVal Code As String, ByVal PrimaryKeyValue As String, ByVal MasterTable As String, Optional Type As String = Nothing, Optional ByVal trans As SqlTransaction = Nothing) As Boolean
        Dim dt As DataTable = Nothing
        Dim Mainqry As String = ""
        Try
            Dim qry As String = clsDBFuncationality.getSingleValue("select TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_NAME='" & MasterTable + clsCommon.HistTablePostFix & "'")
            If clsCommon.myLen(qry) <= 0 Then
                clsCommon.MyMessageBoxShow("No History Table found")
                Return False
            End If
            Dim strMasterCodeColumn As String = ""
            Dim strMasterCodeColumnAS As String = ""
            Dim dtMasterCategory As DataTable = Nothing
            Dim FinalPrimaryKey As String = clsDBFuncationality.getSingleValue("select replace(upper(left('" + PrimaryKeyValue + "',1)) + upper(substring('" + PrimaryKeyValue + "',2,len('" + PrimaryKeyValue + "'))),'_',' ') as FinalName", trans)
            '' Sequence MasterTable column 
            Dim Masteryqry As String = ""
            Masteryqry = "  SELECT c.name as Name,replace(upper(left(c.name,1)) + upper(substring(c.name,2,len(c.name))),'_',' ') as FinalName "
            Masteryqry += " FROM " & objCommonVar.CurrDatabase & ".sys.tables t"
            Masteryqry += " INNER JOIN " & objCommonVar.CurrDatabase & ".sys.all_columns c "
            Masteryqry += "  ON t.object_id = c.object_id"
            Masteryqry += " INNER JOIN " & objCommonVar.CurrDatabase & ".sys.types ty "
            Masteryqry += "  ON c.system_type_id = ty.system_type_id"
            Masteryqry += " WHERE t.name = '" & MasterTable & "'"
            Masteryqry += " order by c.name asc"
            dtMasterCategory = clsDBFuncationality.GetDataTable(Masteryqry, trans)

            If dtMasterCategory IsNot Nothing AndAlso dtMasterCategory.Rows.Count > 0 Then
                For ii As Integer = 0 To dtMasterCategory.Rows.Count - 1
                    If ii <> 0 Then
                        strMasterCodeColumn += ","
                    End If
                    strMasterCodeColumn += "" + clsCommon.myCstr(dtMasterCategory.Rows(ii)("Name")).Trim() + " as [" + clsCommon.myCstr(dtMasterCategory.Rows(ii)("FinalName")).Trim() + "]"
                Next
            End If
            '' End
            '' =========Final Binding Main Qry=======
            Mainqry = "  select ROW_NUMBER() OVER(ORDER BY Hist_Version desc) AS Version,final.* from "
            Mainqry += " ( "
            Mainqry += " select " & clsCommon.HistTableColHistVersion & "," & clsCommon.HistTableColHistBy & "," & clsCommon.HistTableColHistOn & "," & strMasterCodeColumn & " from " & MasterTable + clsCommon.HistTablePostFix & ""
            Mainqry += " union all "
            Mainqry += " select '' as Version,'Current' as [User By],convert(datetime,GETDATE(),103) as " & clsCommon.HistTableColHistOn & "," & strMasterCodeColumn & " from " & MasterTable & ""
            Mainqry += " )final "
            Mainqry += " where 2=2 and final.[" & FinalPrimaryKey & "]='" & Code & "'"
            'Mainqry += " where 2=2 and final." & PrimaryKeyValue & "='" & Code & "'"
            ''==========End=========
            dt = clsDBFuncationality.GetDataTable(Mainqry)
       

        Catch ex As Exception
            trans.Rollback()
            clsCommon.MyMessageBoxShow(ex.Message)
        End Try
        Return True
    End Function
    Public Shared Function ShowTransHistoryData(ByVal Code As String, ByVal PrimaryKeyValue As String, ByVal HeadTable As String, ByVal DetailTable As String, Optional ByVal trans As SqlTransaction = Nothing) As Boolean
        Dim dt As DataTable = Nothing
        Dim Mainqry As String = ""
        Try
            Dim qry As String = clsDBFuncationality.getSingleValue("select TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_NAME='" & HeadTable + clsCommon.HistTablePostFix & "'")
            If clsCommon.myLen(qry) <= 0 Then
                clsCommon.MyMessageBoxShow("No History Table found")
                Return False
            End If
            Dim strMasterCodeColumn As String = ""
            Dim dtMasterCategory As DataTable = Nothing

            '' Sequence MasterTable column 
            Dim Masteryqry As String = ""
            Masteryqry = "  SELECT  REPLACE( c.name ,'_',' ' ) as Name "
            Masteryqry += " FROM " & objCommonVar.CurrDatabase & ".sys.tables t"
            Masteryqry += " INNER JOIN " & objCommonVar.CurrDatabase & ".sys.all_columns c "
            Masteryqry += "  ON t.object_id = c.object_id"
            Masteryqry += " INNER JOIN " & objCommonVar.CurrDatabase & ".sys.types ty "
            Masteryqry += "  ON c.system_type_id = ty.system_type_id"
            Masteryqry += " WHERE t.name = '" & HeadTable & "'"
            Masteryqry += " order by c.name asc"
            dtMasterCategory = clsDBFuncationality.GetDataTable(Masteryqry, trans)

            If dtMasterCategory IsNot Nothing AndAlso dtMasterCategory.Rows.Count > 0 Then
                For ii As Integer = 0 To dtMasterCategory.Rows.Count - 1
                    If ii <> 0 Then
                        strMasterCodeColumn += ","
                    End If
                    strMasterCodeColumn += "" + clsCommon.myCstr(dtMasterCategory.Rows(ii)("Name")).Trim() + ""
                Next
            End If
            '' End
            '' =========Final Binding Main Qry=======
            Mainqry = "  select ROW_NUMBER() OVER(ORDER BY Hist_Version desc) AS Version,final.* from "
            Mainqry += " ( "
            Mainqry += " select " & clsCommon.HistTableColHistVersion & "," & clsCommon.HistTableColHistBy & "," & clsCommon.HistTableColHistOn & "," & strMasterCodeColumn & " from " & HeadTable + clsCommon.HistTablePostFix & ""
            Mainqry += " union all "
            Mainqry += " select '' as Version,'Current' as [User By],'' as " & clsCommon.HistTableColHistOn & "," & strMasterCodeColumn & " from " & HeadTable & ""
            Mainqry += " )final "
            Mainqry += " where 2=2 and final." & PrimaryKeyValue & "='" & Code & "'"
            ''==========End=========
            dt = clsDBFuncationality.GetDataTable(Mainqry)
          
        Catch ex As Exception
            trans.Rollback()
            clsCommon.MyMessageBoxShow(ex.Message)
        End Try

        Return True
    End Function

End Class
Public Class clsFatSnfRateCalculator
    Public fatR As Double = 0
    Public snfR As Double = 0
    Public FatAmt As Double = 0
    Public snfAmt As Double = 0

    Public Shared Function CalculateIn(Qty As Double, StdFatPer As Double, StdSnfPer As Double, FatPer As Double, SnfPer As Double, StdRate As Double, MilkRate As Double) As clsFatSnfRateCalculator

        Dim rValue As clsFatSnfRateCalculator = New clsFatSnfRateCalculator
        Try
            Dim Row As Integer = 1
            Dim Col As Integer = 2
            Dim Matrix(Row, Col) As Double
            Matrix(0, 0) = (Qty * FatPer / 100)
            Matrix(0, 1) = (Qty * SnfPer / 100)
            Matrix(0, 2) = MilkRate * Qty
            Matrix(1, 0) = (Qty * StdFatPer / 100)
            Matrix(1, 1) = (Qty * StdSnfPer / 100)
            Matrix(1, 2) = StdRate * Qty
            Dim ans() As Double = SolveEquations.SolveLinearEquation(Matrix)
            rValue.FatAmt = ans(0) * (Qty * FatPer / 100)
            rValue.snfAmt = ans(1) * (Qty * SnfPer / 100)
            rValue.fatR = ans(0)
            rValue.snfR = ans(1)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return rValue
    End Function

    Public Shared Function CalculateInonSamePercentage(Qty As Double, StdFatPer As Double, StdSnfPer As Double, FatRatio As Double, SnfRatio As Double, StdRate As Double) As clsFatSnfRateCalculator
        Dim rValue As clsFatSnfRateCalculator = New clsFatSnfRateCalculator
        Try
            rValue.fatR = (FatRatio * StdRate) / (StdFatPer * 100)
            rValue.snfR = (SnfRatio * StdRate) / (StdSnfPer * 100)
            rValue.FatAmt = ((FatRatio * StdRate) / (StdFatPer * 100)) * (StdFatPer * Qty)
            rValue.snfAmt = ((SnfRatio * StdRate) / (StdSnfPer * 100)) * (StdSnfPer * Qty)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return rValue
    End Function


    Public Shared Function CalculateStdFATSNFRate(QtyKG As Double, StdFatPer As Double, StdSNFPer As Double, StdFatWeightage As Double, StdSNFWeightage As Double, StdRate As Double, FatPer As Double, SNFPer As Double) As clsFatSnfRateCalculator
        Dim objReturn As clsFatSnfRateCalculator = New clsFatSnfRateCalculator
        Try
            objReturn.fatR = Math.Round(IIf(StdFatPer = 0, 0, StdRate * StdFatWeightage / StdFatPer), 3, MidpointRounding.AwayFromZero)
            objReturn.snfR = Math.Round(IIf(StdSNFPer = 0, 0, StdRate * StdSNFWeightage / StdSNFPer), 3, MidpointRounding.AwayFromZero)
            objReturn.FatAmt = Math.Round(objReturn.fatR * (QtyKG * FatPer / 100), 2, MidpointRounding.ToEven)
            objReturn.snfAmt = Math.Round(objReturn.snfR * (QtyKG * SNFPer / 100), 2, MidpointRounding.ToEven)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return objReturn
    End Function
End Class