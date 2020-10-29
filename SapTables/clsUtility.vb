''created by Monika 18/03/2015
''======================in this class reverse and recreate financial entry, but if there is already document exist then no new document no. generated for journal voucher, ap and ar invoice
Imports common
Imports System.Data.SqlClient

Public Class clsUtility
#Region "variables"

#End Region

    Public Shared Function DeleteAllRecreateTempTables() As Boolean
        Dim trans As SqlTransaction = clsDBFuncationality.GetTransactin()
        Try
            Dim isSaved As Boolean = True
            isSaved = isSaved AndAlso DeleteAllRecreateTempTables(trans)

            trans.Commit()
            Return isSaved
        Catch ex As Exception
            trans.Rollback()
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function DeleteAllRecreateTempTables(ByVal trans As SqlTransaction) As Boolean
        Try
            Dim isSaved As Boolean = True
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery("delete from TEMP_Created_Adjustment", trans)
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery("delete from TEMP_Delete_Adjustment", trans)

            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery("delete from TEMP_DELETE_PURCHASE_RETURN", trans)
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery("delete from TEMP_CREATE_PURCHASE_RETURN", trans)

            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery("delete from TEMP_DELETE_SRN", trans)
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery("delete from TEMP_CREATE_SRN", trans)

            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery("delete from TEMP_Delete_Tranfer", trans)
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery("delete from TEMP_Created_Tranfer", trans)

            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery("delete from TEMP_DELETE_SHIPMENT", trans)
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery("delete from TEMP_CREATE_SHIPMENT", trans)

            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery("delete from TEMP_DELETE_SALE_RETURN", trans)
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery("delete from TEMP_CREATE_SALE_RETURN", trans)

            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery("delete from TEMP_DELETE_SCRAPINVOICE", trans)
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery("delete from TEMP_CREATE_SCRAPINVOICE", trans)

            Return isSaved
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

#Region "Recreate Adjustment"
    '-----------------------adjustment-------------------------------------------------------------------
    Public Shared Function ReCreateAdjustment(ByVal strAdjNo As String, ByVal strVoucherNo As String) As Boolean
        Dim trans As SqlTransaction = clsDBFuncationality.GetTransactin()
        Try
            Dim isSaved As Boolean = True
            isSaved = isSaved AndAlso ReCreateAdjustment(strAdjNo, strVoucherNo, trans)

            Dim Qry As String = "insert into TEMP_Created_Adjustment select '" + strAdjNo + "','" + strVoucherNo + "'"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(Qry, trans)

            trans.Commit()
            Return isSaved
        Catch ex As Exception
            trans.Rollback()
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function ReCreateAdjustment(ByVal strAdjNo As String, ByVal strVoucherNo As String, ByVal trans As SqlTransaction) As Boolean
        Try
            Dim isSaved As Boolean = True
            Dim Qry As String = "select Posted  from TSPL_ADJUSTMENT_HEADER where Adjustment_No='" + strAdjNo + "'"
            If Not clsCommon.CompairString(clsDBFuncationality.getSingleValue(Qry, trans), "Y") = CompairStringResult.Equal Then
                'Throw New Exception("Transaction status should be posted for reverse and unpost,i.e (" + strAdjNo + ")")
                ''if not posted then no mgs and skip the document and go for next
                Return isSaved
            End If

            '===============reverse the document,but without deleting journal entry-------------------
            Qry = "select InOut,Trans_Type,Item_Code,Item_Desc,Location_Code,case when InOut='I' then -1 else 1 end *Qty as Qty ,UOM,MRP,ItemType,case when InOut='I' then -1 else 1 end* Basic_Cost as Basic_Cost from TSPL_INVENTORY_MOVEMENT where Source_Doc_No='" + strAdjNo + "' and Trans_Type='IC-AD'"
            Dim dt1 As DataTable = clsDBFuncationality.GetDataTable(Qry, trans)
            Dim ArrLocationDetails As List(Of clsItemLocationDetails) = New List(Of clsItemLocationDetails)
            For Each objtr As DataRow In dt1.Rows
                Dim dblConvFac As Double = clsItemMaster.GetConvertionFactor(clsCommon.myCstr(objtr("Item_Code")), clsCommon.myCstr(objtr("UOM")), trans)
                Dim objLocationDetails As New clsItemLocationDetails()
                objLocationDetails.Item_Code = clsCommon.myCstr(objtr("Item_Code"))
                objLocationDetails.Item_Desc = clsCommon.myCstr(objtr("Item_Desc"))
                objLocationDetails.Location_Code = clsCommon.myCstr(objtr("Location_Code"))
                objLocationDetails.Location_Desc = clsLocation.GetName(objLocationDetails.Location_Code, trans)
                objLocationDetails.Item_Qty = clsCommon.myCdbl(objtr("Qty")) / dblConvFac
                objLocationDetails.Amount = clsCommon.myCdbl(objtr("Basic_Cost"))
                objLocationDetails.MRP = clsCommon.myCdbl(objtr("MRP")) * dblConvFac
                objLocationDetails.ItemType = clsCommon.myCstr(objtr("ItemType"))
                ArrLocationDetails.Add(objLocationDetails)
            Next
            Dim strPostDate As String = clsCommon.GetPrintDate(clsCommon.GETSERVERDATE(trans), "dd/MM/yyyy")
            isSaved = isSaved AndAlso clsItemLocationDetails.SaveData(strPostDate, ArrLocationDetails, trans)

            Qry = "delete from tspl_serial_item where Against_Inv_Movement_Trans_Id in (select trans_id from TSPL_INVENTORY_MOVEMENT where Source_Doc_No='" + strAdjNo + "' and Trans_Type='IC-AD')"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(Qry, trans)

            Qry = "delete from TSPL_INVENTORY_MOVEMENT where Source_Doc_No='" + strAdjNo + "' and Trans_Type='IC-AD'"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(Qry, trans)

            Qry = "Update TSPL_ADJUSTMENT_HEADER set Posted = 'N' where adjustment_no='" + strAdjNo + "'"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(Qry, trans)

            Xtra.UpdateAPInvoiceBalanceAmount(strAdjNo, trans)
            '=============================================================================

            Dim strAdjustmentType As String = ClsAdjustments.GetTransactionType(strAdjNo, trans)

            isSaved = isSaved AndAlso ClsAdjustments.PostData(strAdjNo, strAdjustmentType, trans, True, strVoucherNo)


            Return isSaved
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
    '-----------------------end here-------------------------------------------------------------------
#End Region

#Region "Recreate Transfer"
    Public Shared Function ReCreateTransfer(ByVal strTransferNo As String, ByVal strVoucherNo As String, ByVal ProvisionAllow As Boolean) As Boolean
        Dim trans As SqlTransaction = clsDBFuncationality.GetTransactin()
        Try
            Dim isSaved As Boolean = True

            isSaved = isSaved AndAlso ReCreateTransfer(strTransferNo, strVoucherNo, ProvisionAllow, trans)

            ''===update
            Dim Qry As String = "insert into TEMP_Created_Tranfer values('" + strTransferNo + "','" + strVoucherNo + "')"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(Qry, trans)

            trans.Commit()
            Return isSaved
        Catch ex As Exception
            trans.Rollback()
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function ReCreateTransfer(ByVal strTransferNo As String, ByVal strVoucherNo As String, ByVal ProvisionAllow As Boolean, ByVal trans As SqlTransaction) As Boolean
        Try
            Dim isSaved As Boolean = True
            Dim Qry As String = "select Status,Transfer_Type,(case when Transfer_Type='O' then (select top 1 inn.Document_No from TSPL_TRANSFER_ORDER_HEAD as inn where inn.TransferOutNo=TSPL_TRANSFER_ORDER_HEAD.Document_No ) else null end) as LoadOutNo from TSPL_TRANSFER_ORDER_HEAD where Document_No='" + strTransferNo + "'"
            Dim dt As DataTable = clsDBFuncationality.GetDataTable(Qry, trans)
            If dt Is Nothing OrElse dt.Rows.Count <= 0 Then
                Return isSaved
            End If

            If Not clsCommon.myCdbl(dt.Rows(0)("Status")) = 1 Then
                'Throw New Exception("Transaction status should be posted for reverse and unpost,i.e (" + strTransferNo + ")")
                Return isSaved
            End If

            If clsCommon.CompairString(clsCommon.myCstr(dt.Rows(0)("Transfer_Type")), "O") = CompairStringResult.Equal Then
                If clsCommon.myLen(dt.Rows(0)("LoadOutNo")) > 0 Then
                    'Throw New Exception("Loadin no -" + clsCommon.myCstr(dt.Rows(0)("Transfer_Type")) + " found")
                    Return isSaved
                End If
            End If

            '===reverse=
            Qry = "delete from tspl_serial_item where Against_Inv_Movement_Trans_Id in (select trans_id from TSPL_INVENTORY_MOVEMENT where Source_Doc_No='" + strTransferNo + "' and Trans_Type='Transfer')"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(Qry, trans)

            Qry = "delete from TSPL_INVENTORY_MOVEMENT where Source_Doc_No='" + strTransferNo + "' and Trans_Type='Transfer'"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(Qry, trans)

            Qry = "Update TSPL_TRANSFER_ORDER_HEAD set Status = 0 where Document_No='" + strTransferNo + "'"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(Qry, trans)

            ''===recreate
            If (clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.TransferJEForLocationMapping, clsFixedParameterCode.TransferJEForLocationMapping, trans)) > 0) Then
                isSaved = isSaved AndAlso clsTransferDCC.postTransfer(strTransferNo, trans, ProvisionAllow, strVoucherNo)
            Else
                Dim obj As clsTransferDCC = clsTransferDCC.GetData(strTransferNo, NavigatorType.Current, trans)
                isSaved = isSaved AndAlso clsTransferDCC.postTransferNew(trans, obj, False, ProvisionAllow, strVoucherNo)
            End If


            Return isSaved
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
#End Region

#Region "Recreate SRN"
    Public Shared Function ReCreateSRN(ByVal strSRNno As String, ByVal strVoucherNo As String) As Boolean
        Dim trans As SqlTransaction = clsDBFuncationality.GetTransactin()
        Try
            Dim isSaved As Boolean = True
            isSaved = isSaved AndAlso ReCreateSRN(strSRNno, strVoucherNo, trans)

            ''===update
            Dim Qry As String = "insert into TEMP_Create_SRN values('" + strSRNno + "','" + strVoucherNo + "')"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(Qry, trans)

            trans.Commit()
            Return isSaved
        Catch ex As Exception
            trans.Rollback()
            Throw New Exception(ex.Message)
        End Try
    End Function

   

    Public Shared Function ReCreateSRN(ByVal strSRNno As String, ByVal strVoucherNo As String, ByVal trans As SqlTransaction) As Boolean
        Dim dt As New DataTable()
        Try
            Dim isSaved As Boolean = True
            ''reverse

            Dim Qry As String = "select Status from TSPL_SRN_HEAD where SRN_No='" + strSRNno + "'"
            If Not clsCommon.myCdbl(clsDBFuncationality.getSingleValue(Qry, trans)) = 1 Then
                'Throw New Exception("Transaction status should be posted for reverse and unpost")
                Return isSaved
            End If

            Qry = "select distinct PI_No from TSPL_PI_DETAIL where SRN_Id='" + strSRNno + "'"
            dt = New DataTable()
            dt = clsDBFuncationality.GetDataTable(Qry, trans)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Return isSaved
            End If

            Qry = "select InOut,Trans_Type,Item_Code,Item_Desc,TSPL_INVENTORY_MOVEMENT.Location_Code,case when InOut='I' then -1 else 1 end *Qty_ as Qty ,UOM,MRP,ItemType,case when InOut='I' then -1 else 1 end* Basic_Cost as Basic_Cost from TSPL_INVENTORY_MOVEMENT " _
            & " Left join  (select Item_Code as it_code,case when InOut='I' then -1 else 1 end *sum(Qty) as Qty_  from TSPL_INVENTORY_MOVEMENT  where Source_Doc_No='" + strSRNno + "' " _
            & " and Trans_Type='SRN' group by Item_Code,InOut) tt on tt.it_code=TSPL_INVENTORY_MOVEMENT.Item_Code Inner join tspl_location_maSTER on tspl_location_maSTER.Location_Code" _
            & " =TSPL_INVENTORY_MOVEMENT.Location_Code and Rejected_Type='N' where Source_Doc_No='" + strSRNno + "' and Trans_Type='SRN'"
            dt = clsDBFuncationality.GetDataTable(Qry, trans)
            Dim ArrLocationDetails As List(Of clsItemLocationDetails) = New List(Of clsItemLocationDetails)
            For Each objtr As DataRow In dt.Rows
                Dim dblConvFac As Double = clsItemMaster.GetConvertionFactor(clsCommon.myCstr(objtr("Item_Code")), clsCommon.myCstr(objtr("UOM")), trans)
                Dim objLocationDetails As New clsItemLocationDetails()
                objLocationDetails.Item_Code = clsCommon.myCstr(objtr("Item_Code"))
                objLocationDetails.Item_Desc = clsCommon.myCstr(objtr("Item_Desc"))
                objLocationDetails.Location_Code = clsCommon.myCstr(objtr("Location_Code"))
                objLocationDetails.Location_Desc = clsLocation.GetName(objLocationDetails.Location_Code, trans)
                objLocationDetails.Item_Qty = clsCommon.myCdbl(objtr("Qty")) / dblConvFac
                objLocationDetails.Amount = clsCommon.myCdbl(objtr("Basic_Cost"))
                objLocationDetails.MRP = clsCommon.myCdbl(objtr("MRP")) * dblConvFac
                objLocationDetails.ItemType = clsCommon.myCstr(objtr("ItemType"))
                ArrLocationDetails.Add(objLocationDetails)
            Next
            Dim strPostDate As String = clsCommon.GetPrintDate(clsCommon.GETSERVERDATE(trans), "dd/MM/yyyy")
            isSaved = isSaved AndAlso clsItemLocationDetails.SaveData(strPostDate, ArrLocationDetails, trans)

            Qry = "delete from tspl_serial_item where Against_Inv_Movement_Trans_Id in (select trans_id from TSPL_INVENTORY_MOVEMENT where Source_Doc_No='" + strSRNno + "' and Trans_Type='SRN')"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(Qry, trans)

            Qry = "delete from TSPL_INVENTORY_MOVEMENT where Source_Doc_No='" + strSRNno + "' and Trans_Type='SRN'"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(Qry, trans)

            Qry = "Update TSPL_SRN_HEAD set Status = 0 where SRN_No='" + strSRNno + "'"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(Qry, trans)

            ''recreate
            isSaved = isSaved AndAlso clsSRNHead.PostData(clsUserMgtCode.mbtnSRN, strSRNno, trans, False, strVoucherNo)


            Return isSaved
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Function
#End Region

#Region "Recreate Purchase Return"
    Public Shared Function ReCreatePR(ByVal strPRNo As String, ByVal strVoucherNo As String, ByVal strAPInvNo As String) As Boolean
        Dim trans As SqlTransaction = clsDBFuncationality.GetTransactin()
        Try
            Dim isSaved As Boolean = True
            isSaved = isSaved AndAlso ReCreatePR(strPRNo, strVoucherNo, strAPInvNo, trans)

            ''update
            Dim qry As String = "insert into TEMP_CREATE_PURCHASE_RETURN values ('" + strPRNo + "','" + strVoucherNo + "','" + strPRNo + "')"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(qry, trans)

            trans.Commit()
            Return isSaved
        Catch ex As Exception
            trans.Rollback()
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function ReCreatePR(ByVal strPRNo As String, ByVal strVoucherNo As String, ByVal strAPInvNo As String, ByVal trans As SqlTransaction) As Boolean
        Dim dt As New DataTable()
        Try
            Dim isSaved As Boolean = True
            ''reverse
            Dim qry As String = "select Payment_No from TSPL_PAYMENT_DETAIL  where Document_No='" + strAPInvNo + "'"
            Dim dtAP As DataTable = clsDBFuncationality.GetDataTable(qry, trans)
            If dtAP IsNot Nothing AndAlso dtAP.Rows.Count > 0 Then
                'qry = "AP-Invoice " + docNo + " is used in following Payment -"
                'For Each dr As DataRow In dtAP.Rows
                '    qry += Environment.NewLine + clsCommon.myCstr(dr("Payment_No"))
                'Next
                'Throw New Exception(qry)
                Return isSaved
            End If

            qry = "select InOut,Trans_Type,Item_Code,Item_Desc,Location_Code,case when InOut='I' then -1 else 1 end *Qty as Qty ,UOM,MRP,ItemType,case when InOut='I' then -1 else 1 end* Basic_Cost as Basic_Cost from TSPL_INVENTORY_MOVEMENT where Source_Doc_No='" + strPRNo + "' and Trans_Type='Purchase Return'"
            dt = clsDBFuncationality.GetDataTable(qry, trans)
            Dim ArrLocationDetails As List(Of clsItemLocationDetails) = New List(Of clsItemLocationDetails)
            For Each objtr As DataRow In dt.Rows
                Dim dblConvFac As Double = clsItemMaster.GetConvertionFactor(clsCommon.myCstr(objtr("Item_Code")), clsCommon.myCstr(objtr("UOM")), trans)
                Dim objLocationDetails As New clsItemLocationDetails()
                objLocationDetails.Item_Code = clsCommon.myCstr(objtr("Item_Code"))
                objLocationDetails.Item_Desc = clsCommon.myCstr(objtr("Item_Desc"))
                objLocationDetails.Location_Code = clsCommon.myCstr(objtr("Location_Code"))
                objLocationDetails.Location_Desc = clsLocation.GetName(objLocationDetails.Location_Code, trans)
                objLocationDetails.Item_Qty = clsCommon.myCdbl(objtr("Qty")) / dblConvFac
                objLocationDetails.Amount = clsCommon.myCdbl(objtr("Basic_Cost"))
                objLocationDetails.MRP = clsCommon.myCdbl(objtr("MRP")) * dblConvFac
                objLocationDetails.ItemType = clsCommon.myCstr(objtr("ItemType"))
                ArrLocationDetails.Add(objLocationDetails)
            Next
            Dim strPostDate As String = clsCommon.GetPrintDate(clsCommon.GETSERVERDATE(trans), "dd/MM/yyyy")
            clsItemLocationDetails.SaveData(strPostDate, ArrLocationDetails, trans)

            qry = "delete from tspl_serial_item where Against_Inv_Movement_Trans_Id in (select trans_id from TSPL_INVENTORY_MOVEMENT where Source_Doc_No='" + strPRNo + "' and Trans_Type='Purchase Return')"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(qry, trans)

            qry = "delete from TSPL_INVENTORY_MOVEMENT where Source_Doc_No='" + strPRNo + "' and Trans_Type='Purchase Return'"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(qry, trans)

            qry = "update TSPL_PR_HEAD set Status=0 where PR_No='" + strPRNo + "'"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(qry, trans)

            qry = "update TSPL_VENDOR_INVOICE_HEAD set posting_date=null where document_no='" + strAPInvNo + "'"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(qry, trans)

            ''=recreate
            Dim strAP_GL_NO As String = ""
            strAP_GL_NO = clsCommon.myCstr(clsDBFuncationality.getSingleValue("select voucher_no from TSPL_JOURNAL_MASTER where Source_Doc_No='" + strAPInvNo + "' and Source_Code in ('AP-CN','AP-DN')", trans))

            isSaved = isSaved AndAlso clsPurchasReturnHead.PostData(clsUserMgtCode.mbtnPurchaseReturn, strPRNo, trans, strVoucherNo, strAP_GL_NO, strAPInvNo)

            Return isSaved
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
#End Region

#Region "Recreate Shipment"
    Public Shared Function ReCreateShipment(ByVal strShipmentNo As String, ByVal strVoucherNo As String) As Boolean
        Dim trans As SqlTransaction = clsDBFuncationality.GetTransactin()
        Try
            Dim isSaved As Boolean = True
            isSaved = isSaved AndAlso ReCreateShipment(strShipmentNo, strVoucherNo, trans)

            Dim qry As String = "insert into TEMP_CREATE_SHIPMENT values ('" + strShipmentNo + "','" + strVoucherNo + "')"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(qry, trans)

            trans.Commit()
            Return isSaved
        Catch ex As Exception
            trans.Rollback()
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function ReCreateShipment(ByVal strShipmentNo As String, ByVal strVoucherNo As String, ByVal trans As SqlTransaction) As Boolean
        Dim dt As New DataTable()
        Try
            Dim isSaved As Boolean = True

            Dim Qry As String = "select distinct DOCUMENT_CODE from TSPL_SD_SALE_INVOICE_DETAIL where Shipment_Code='" + strShipmentNo + "'"
            dt = New DataTable()
            dt = clsDBFuncationality.GetDataTable(Qry, trans)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                'Qry = "Current Shipment is used in following Sale invoice -"
                'For Each dr As DataRow In dt.Rows
                '    Qry += Environment.NewLine + clsCommon.myCstr(dr("DOCUMENT_CODE"))
                'Next
                'Throw New Exception(Qry)
                Return True
            End If

            Qry = "select InOut,Trans_Type,Item_Code,Item_Desc,Location_Code,case when InOut='I' then -1 else 1 end *Qty as Qty ,UOM,MRP,ItemType,case when InOut='I' then -1 else 1 end* Basic_Cost as Basic_Cost from TSPL_INVENTORY_MOVEMENT where Source_Doc_No='" + strShipmentNo + "' and Trans_Type='SD-SH'"
            dt = clsDBFuncationality.GetDataTable(Qry, trans)
            Dim ArrLocationDetails As List(Of clsItemLocationDetails) = New List(Of clsItemLocationDetails)
            For Each objtr As DataRow In dt.Rows
                Dim dblConvFac As Double = clsItemMaster.GetConvertionFactor(clsCommon.myCstr(objtr("Item_Code")), clsCommon.myCstr(objtr("UOM")), trans)
                Dim objLocationDetails As New clsItemLocationDetails()
                objLocationDetails.Item_Code = clsCommon.myCstr(objtr("Item_Code"))
                objLocationDetails.Item_Desc = clsCommon.myCstr(objtr("Item_Desc"))
                objLocationDetails.Location_Code = clsCommon.myCstr(objtr("Location_Code"))
                objLocationDetails.Location_Desc = clsLocation.GetName(objLocationDetails.Location_Code, trans)
                objLocationDetails.Item_Qty = clsCommon.myCdbl(objtr("Qty")) / dblConvFac
                objLocationDetails.Amount = clsCommon.myCdbl(objtr("Basic_Cost"))
                objLocationDetails.MRP = clsCommon.myCdbl(objtr("MRP")) * dblConvFac
                objLocationDetails.ItemType = clsCommon.myCstr(objtr("ItemType"))
                ArrLocationDetails.Add(objLocationDetails)
            Next
            Dim strPostDate As String = clsCommon.GetPrintDate(clsCommon.GETSERVERDATE(trans), "dd/MM/yyyy")
            isSaved = isSaved AndAlso clsItemLocationDetails.SaveData(strPostDate, ArrLocationDetails, trans)

            Qry = "delete from tspl_serial_item where Against_Inv_Movement_Trans_Id in (select trans_id from TSPL_INVENTORY_MOVEMENT where Source_Doc_No='" + strShipmentNo + "' and Trans_Type='SD-SH')"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(Qry, trans)

            Qry = "delete from TSPL_INVENTORY_MOVEMENT where Source_Doc_No='" + strShipmentNo + "' and Trans_Type='SD-SH'"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(Qry, trans)

            Qry = "Update TSPL_SD_SHIPMENT_HEAD set Status = 0 where Document_Code='" + strShipmentNo + "' and trans_type='ALL'"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(Qry, trans)

            isSaved = isSaved AndAlso clsSNShipmentHead.PostData(clsUserMgtCode.frmSNShipment, strShipmentNo, trans, strVoucherNo)

            Return isSaved
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Function
#End Region

#Region "Sale Return"
    Public Shared Function ReCreateSaleReturn(ByVal strReturnNo As String, ByVal strVoucherNo As String, ByVal strArInvNo As String) As Boolean
        Dim trans As SqlTransaction = clsDBFuncationality.GetTransactin()
        Try
            Dim isSaved As Boolean = True
            isSaved = isSaved AndAlso ReCreateSaleReturn(strReturnNo, strVoucherNo, strArInvNo, trans)

            Dim qry As String = "insert into TEMP_CREATE_SALE_RETURN values ('" + strReturnNo + "','" + strVoucherNo + "','" + strArInvNo + "')"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(qry, trans)

            trans.Commit()
            Return isSaved
        Catch ex As Exception
            trans.Rollback()
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function ReCreateSaleReturn(ByVal strReturnNo As String, ByVal strVoucherNo As String, ByVal strArInvNo As String, ByVal trans As SqlTransaction) As Boolean
        Dim dt As New DataTable()
        Try
            Dim isSaved As Boolean = True

            ''reverse
            Dim qry As String = "select Receipt_No from TSPL_RECEIPT_DETAIL where Document_No='" + strArInvNo + "'"
            Dim dtAP As DataTable = clsDBFuncationality.GetDataTable(qry, trans)
            If dtAP IsNot Nothing AndAlso dtAP.Rows.Count > 0 Then
                'qry = "AP-Invoice " + docNo + " is used in following Payment -"
                'For Each dr As DataRow In dtAP.Rows
                '    qry += Environment.NewLine + clsCommon.myCstr(dr("Payment_No"))
                'Next
                'Throw New Exception(qry)
                Return isSaved
            End If

            qry = "select InOut,Trans_Type,Item_Code,Item_Desc,Location_Code,case when InOut='I' then -1 else 1 end *Qty as Qty ,UOM,MRP,ItemType,case when InOut='I' then -1 else 1 end* Basic_Cost as Basic_Cost from TSPL_INVENTORY_MOVEMENT where Source_Doc_No='" + strReturnNo + "' and Trans_Type='Sale Return'"
            dt = New DataTable()
            dt = clsDBFuncationality.GetDataTable(qry, trans)
            Dim ArrLocationDetails As List(Of clsItemLocationDetails) = New List(Of clsItemLocationDetails)
            For Each objtr As DataRow In dt.Rows
                Dim dblConvFac As Double = clsItemMaster.GetConvertionFactor(clsCommon.myCstr(objtr("Item_Code")), clsCommon.myCstr(objtr("UOM")), trans)
                Dim objLocationDetails As New clsItemLocationDetails()
                objLocationDetails.Item_Code = clsCommon.myCstr(objtr("Item_Code"))
                objLocationDetails.Item_Desc = clsCommon.myCstr(objtr("Item_Desc"))
                objLocationDetails.Location_Code = clsCommon.myCstr(objtr("Location_Code"))
                objLocationDetails.Location_Desc = clsLocation.GetName(objLocationDetails.Location_Code, trans)
                objLocationDetails.Item_Qty = clsCommon.myCdbl(objtr("Qty")) / dblConvFac
                objLocationDetails.Amount = clsCommon.myCdbl(objtr("Basic_Cost"))
                objLocationDetails.MRP = clsCommon.myCdbl(objtr("MRP")) * dblConvFac
                objLocationDetails.ItemType = clsCommon.myCstr(objtr("ItemType"))
                ArrLocationDetails.Add(objLocationDetails)
            Next
            Dim strPostDate As String = clsCommon.GetPrintDate(clsCommon.GETSERVERDATE(trans), "dd/MM/yyyy")
            clsItemLocationDetails.SaveData(strPostDate, ArrLocationDetails, trans)

            qry = "delete from tspl_serial_item where Against_Inv_Movement_Trans_Id in (select trans_id from TSPL_INVENTORY_MOVEMENT where Source_Doc_No='" + strReturnNo + "' and Trans_Type='Sale Return')"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(qry, trans)

            qry = "delete from TSPL_INVENTORY_MOVEMENT where Source_Doc_No='" + strReturnNo + "' and Trans_Type='Sale Return'"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(qry, trans)

            qry = "update TSPL_SD_SALE_RETURN_HEAD set Status=0 where document_code='" + strReturnNo + "'"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(qry, trans)

            qry = "update TSPL_CUSTOMER_INVOICE_HEAD set status=0 where document_no='" + strArInvNo + "'"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(qry, trans)

            ''recreated

            isSaved = isSaved AndAlso clsSNSalesReturnHead.PostData(clsUserMgtCode.frmSNSaleReturn, strReturnNo, trans, strArInvNo, strVoucherNo)

            Return isSaved
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Function
#End Region

#Region "Scrap Sale"
    Public Shared Function ReCreateScrapSale(ByVal strInvoiceNo As String, ByVal strShipmentNo As String, ByVal strArInvno As String, ByVal strVoucherNo As String, ByVal strAR_Voucher_NO As String) As Boolean
        Dim trans As SqlTransaction = clsDBFuncationality.GetTransactin()
        Try
            Dim isSaved As Boolean = True
            isSaved = isSaved AndAlso ReCreateScrapSale(strInvoiceNo, strShipmentNo, strArInvno, strVoucherNo, strAR_Voucher_NO, trans)

            Dim qry As String = "insert into TEMP_CREATE_SCRAPINVOICE values ('" + strShipmentNo + "','" + strInvoiceNo + "','" + strVoucherNo + "','" + strArInvno + "','" + strAR_Voucher_NO + "')"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(qry, trans)

            trans.Commit()
            Return isSaved
        Catch ex As Exception
            trans.Rollback()
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function ReCreateScrapSale(ByVal strInvoiceNo As String, ByVal strShipmentNo As String, ByVal strArInvno As String, ByVal strVoucherNo As String, ByVal strAR_Voucher_NO As String, ByVal trans As SqlTransaction) As Boolean
        Dim dt As New DataTable()
        Try
            Dim isSaved As Boolean = True

            ''reverse
            Dim Qry As String = "select distinct Receipt_No  from TSPL_RECEIPT_DETAIL where Document_No in (select Document_No from TSPL_Customer_Invoice_Head where AgainstScrap in ('" + strInvoiceNo + "') ) "
            dt = New DataTable()
            dt = clsDBFuncationality.GetDataTable(Qry, trans)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                'Qry = "Current Scrap Invoice is used in following Receipt -"
                'For Each dr As DataRow In dt.Rows
                '    Qry += Environment.NewLine + clsCommon.myCstr(dr("Receipt_No"))
                'Next
                'Throw New Exception(Qry)
                Return True
            End If

            Qry = "select InOut,Trans_Type,Item_Code,Item_Desc,Location_Code,case when InOut='I' then -1 else 1 end *Qty as Qty ,UOM,MRP,ItemType,case when InOut='I' then -1 else 1 end* Basic_Cost as Basic_Cost from TSPL_INVENTORY_MOVEMENT where Source_Doc_No='" + strInvoiceNo + "' and Trans_Type='ScrapIn'"
            dt = New DataTable()
            dt = clsDBFuncationality.GetDataTable(Qry, trans)
            Dim ArrLocationDetails As List(Of clsItemLocationDetails) = New List(Of clsItemLocationDetails)
            For Each objtr As DataRow In dt.Rows
                Dim dblConvFac As Double = clsItemMaster.GetConvertionFactor(clsCommon.myCstr(objtr("Item_Code")), clsCommon.myCstr(objtr("UOM")), trans)
                Dim objLocationDetails As New clsItemLocationDetails()
                objLocationDetails.Item_Code = clsCommon.myCstr(objtr("Item_Code"))
                objLocationDetails.Item_Desc = clsCommon.myCstr(objtr("Item_Desc"))
                objLocationDetails.Location_Code = clsCommon.myCstr(objtr("Location_Code"))
                objLocationDetails.Location_Desc = clsLocation.GetName(objLocationDetails.Location_Code, trans)
                objLocationDetails.Item_Qty = clsCommon.myCdbl(objtr("Qty")) / dblConvFac
                objLocationDetails.Amount = clsCommon.myCdbl(objtr("Basic_Cost"))
                objLocationDetails.MRP = clsCommon.myCdbl(objtr("MRP")) * dblConvFac
                objLocationDetails.ItemType = clsCommon.myCstr(objtr("ItemType"))
                ArrLocationDetails.Add(objLocationDetails)
            Next
            Dim strPostDate As String = clsCommon.GetPrintDate(clsCommon.GETSERVERDATE(trans), "dd/MM/yyyy")
            clsItemLocationDetails.SaveData(strPostDate, ArrLocationDetails, trans)

            Qry = "delete from tspl_serial_item where Against_Inv_Movement_Trans_Id in (select trans_id from TSPL_INVENTORY_MOVEMENT where Source_Doc_No='" + strInvoiceNo + "' and Trans_Type='ScrapIn')"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(Qry, trans)

            Qry = "delete from TSPL_INVENTORY_MOVEMENT where Source_Doc_No='" + strInvoiceNo + "' and Trans_Type='ScrapIn'"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(Qry, trans)

            Qry = "update TSPL_CUSTOMER_INVOICE_HEAD set status=0 where AgainstScrap='" + strInvoiceNo + "'"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(Qry, trans)

            Qry = "Update TSPL_SCRAPINVOICE_HEAD set ispost = 0 where invoice_No='" + strInvoiceNo + "'"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(Qry, trans)

            Qry = "Update TSPL_SCRAPSALE_HEAD set ispost = 0 where shipment_No='" + strShipmentNo + "'"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(Qry, trans)

            ''recreated
            isSaved = isSaved AndAlso ClsScrapInvoiceHead.PostData(strInvoiceNo, True, trans, strVoucherNo, strArInvno, strAR_Voucher_NO)

            Return True
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Function
#End Region

#Region "MCC Material Sale"
    Public Shared Function ReCreateMCCSale(ByVal strShipmentNo As String, ByVal strVoucherNo As String) As Boolean
        Dim trans As SqlTransaction = clsDBFuncationality.GetTransactin()
        Try
            Dim isSaved As Boolean = True
            isSaved = isSaved AndAlso ReCreateShipment(strShipmentNo, strVoucherNo, trans)

            Dim qry As String = "insert into TEMP_CREATE_SHIPMENT values ('" + strShipmentNo + "','" + strVoucherNo + "')"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(qry, trans)

            trans.Commit()
            Return isSaved
        Catch ex As Exception
            trans.Rollback()
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function ReCreateMCCSale(ByVal strShipmentNo As String, ByVal strVoucherNo As String, ByVal trans As SqlTransaction) As Boolean
        Dim dt As New DataTable()
        Try
            Dim isSaved As Boolean = True

            Dim Qry As String = "select distinct DOCUMENT_CODE from TSPL_SD_SALE_INVOICE_DETAIL where Shipment_Code='" + strShipmentNo + "'"
            dt = New DataTable()
            dt = clsDBFuncationality.GetDataTable(Qry, trans)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                'Qry = "Current Shipment is used in following Sale invoice -"
                'For Each dr As DataRow In dt.Rows
                '    Qry += Environment.NewLine + clsCommon.myCstr(dr("DOCUMENT_CODE"))
                'Next
                'Throw New Exception(Qry)
                Return True
            End If

            Qry = "select InOut,Trans_Type,Item_Code,Item_Desc,Location_Code,case when InOut='I' then -1 else 1 end *Qty as Qty ,UOM,MRP,ItemType,case when InOut='I' then -1 else 1 end* Basic_Cost as Basic_Cost from TSPL_INVENTORY_MOVEMENT where Source_Doc_No='" + strShipmentNo + "' and Trans_Type='SD-SH'"
            dt = clsDBFuncationality.GetDataTable(Qry, trans)
            Dim ArrLocationDetails As List(Of clsItemLocationDetails) = New List(Of clsItemLocationDetails)
            For Each objtr As DataRow In dt.Rows
                Dim dblConvFac As Double = clsItemMaster.GetConvertionFactor(clsCommon.myCstr(objtr("Item_Code")), clsCommon.myCstr(objtr("UOM")), trans)
                Dim objLocationDetails As New clsItemLocationDetails()
                objLocationDetails.Item_Code = clsCommon.myCstr(objtr("Item_Code"))
                objLocationDetails.Item_Desc = clsCommon.myCstr(objtr("Item_Desc"))
                objLocationDetails.Location_Code = clsCommon.myCstr(objtr("Location_Code"))
                objLocationDetails.Location_Desc = clsLocation.GetName(objLocationDetails.Location_Code, trans)
                objLocationDetails.Item_Qty = clsCommon.myCdbl(objtr("Qty")) / dblConvFac
                objLocationDetails.Amount = clsCommon.myCdbl(objtr("Basic_Cost"))
                objLocationDetails.MRP = clsCommon.myCdbl(objtr("MRP")) * dblConvFac
                objLocationDetails.ItemType = clsCommon.myCstr(objtr("ItemType"))
                ArrLocationDetails.Add(objLocationDetails)
            Next
            Dim strPostDate As String = clsCommon.GetPrintDate(clsCommon.GETSERVERDATE(trans), "dd/MM/yyyy")
            isSaved = isSaved AndAlso clsItemLocationDetails.SaveData(strPostDate, ArrLocationDetails, trans)

            Qry = "delete from tspl_serial_item where Against_Inv_Movement_Trans_Id in (select trans_id from TSPL_INVENTORY_MOVEMENT where Source_Doc_No='" + strShipmentNo + "' and Trans_Type='SD-SH')"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(Qry, trans)

            Qry = "delete from TSPL_INVENTORY_MOVEMENT where Source_Doc_No='" + strShipmentNo + "' and Trans_Type='SD-SH'"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(Qry, trans)

            Qry = "Update TSPL_SD_SHIPMENT_HEAD set Status = 0 where Document_Code='" + strShipmentNo + "' and trans_type='MCC'"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(Qry, trans)

            isSaved = isSaved AndAlso clsMCCMaterialSale.PostData(clsUserMgtCode.frmMCCMaterial, strShipmentNo, trans, strVoucherNo)

            Return isSaved
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Function
#End Region

#Region "Product Dispatch"
    Public Shared Function ReCreateProductDispatch(ByVal strShipmentNo As String, ByVal strVoucherNo As String) As Boolean
        Dim trans As SqlTransaction = clsDBFuncationality.GetTransactin()
        Try
            Dim isSaved As Boolean = True
            isSaved = isSaved AndAlso ReCreateProductDispatch(strShipmentNo, strVoucherNo, trans)

            Dim qry As String = "insert into TEMP_CREATE_SHIPMENT values ('" + strShipmentNo + "','" + strVoucherNo + "')"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(qry, trans)

            trans.Commit()
            Return isSaved
        Catch ex As Exception
            trans.Rollback()
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function ReCreateProductDispatch(ByVal strShipmentNo As String, ByVal strVoucherNo As String, ByVal trans As SqlTransaction) As Boolean
        Dim dt As New DataTable()
        Try
            Dim isSaved As Boolean = True

            Dim Qry As String = "select distinct DOCUMENT_CODE from TSPL_SD_SALE_INVOICE_DETAIL where Shipment_Code='" + strShipmentNo + "'"
            dt = New DataTable()
            dt = clsDBFuncationality.GetDataTable(Qry, trans)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                'Qry = "Current Shipment is used in following Sale invoice -"
                'For Each dr As DataRow In dt.Rows
                '    Qry += Environment.NewLine + clsCommon.myCstr(dr("DOCUMENT_CODE"))
                'Next
                'Throw New Exception(Qry)
                Return True
            End If

            Qry = "select InOut,Trans_Type,Item_Code,Item_Desc,Location_Code,case when InOut='I' then -1 else 1 end *Qty as Qty ,UOM,MRP,ItemType,case when InOut='I' then -1 else 1 end* Basic_Cost as Basic_Cost from TSPL_INVENTORY_MOVEMENT where Source_Doc_No='" + strShipmentNo + "' and Trans_Type='SD-SH'"
            dt = clsDBFuncationality.GetDataTable(Qry, trans)
            Dim ArrLocationDetails As List(Of clsItemLocationDetails) = New List(Of clsItemLocationDetails)
            For Each objtr As DataRow In dt.Rows
                Dim dblConvFac As Double = clsItemMaster.GetConvertionFactor(clsCommon.myCstr(objtr("Item_Code")), clsCommon.myCstr(objtr("UOM")), trans)
                Dim objLocationDetails As New clsItemLocationDetails()
                objLocationDetails.Item_Code = clsCommon.myCstr(objtr("Item_Code"))
                objLocationDetails.Item_Desc = clsCommon.myCstr(objtr("Item_Desc"))
                objLocationDetails.Location_Code = clsCommon.myCstr(objtr("Location_Code"))
                objLocationDetails.Location_Desc = clsLocation.GetName(objLocationDetails.Location_Code, trans)
                objLocationDetails.Item_Qty = clsCommon.myCdbl(objtr("Qty")) / dblConvFac
                objLocationDetails.Amount = clsCommon.myCdbl(objtr("Basic_Cost"))
                objLocationDetails.MRP = clsCommon.myCdbl(objtr("MRP")) * dblConvFac
                objLocationDetails.ItemType = clsCommon.myCstr(objtr("ItemType"))
                ArrLocationDetails.Add(objLocationDetails)
            Next
            Dim strPostDate As String = clsCommon.GetPrintDate(clsCommon.GETSERVERDATE(trans), "dd/MM/yyyy")
            isSaved = isSaved AndAlso clsItemLocationDetails.SaveData(strPostDate, ArrLocationDetails, trans)

            Qry = "delete from tspl_serial_item where Against_Inv_Movement_Trans_Id in (select trans_id from TSPL_INVENTORY_MOVEMENT where Source_Doc_No='" + strShipmentNo + "' and Trans_Type='SD-SH')"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(Qry, trans)

            Qry = "delete from TSPL_INVENTORY_MOVEMENT where Source_Doc_No='" + strShipmentNo + "' and Trans_Type='SD-SH'"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(Qry, trans)

            Qry = "Update TSPL_SD_SHIPMENT_HEAD set Status = 0 where Document_Code='" + strShipmentNo + "' and trans_type='PS'"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(Qry, trans)

            isSaved = isSaved AndAlso clsPSShipmentHead.PostData(clsUserMgtCode.frmShipmentProductSale, strShipmentNo, trans, strVoucherNo)

            Return isSaved
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Function
#End Region

#Region "Fresh Dispatch"
    Public Shared Function ReCreateFreshDispatch(ByVal strShipmentNo As String, ByVal strVoucherNo As String) As Boolean
        Dim trans As SqlTransaction = clsDBFuncationality.GetTransactin()
        Try
            Dim isSaved As Boolean = True
            isSaved = isSaved AndAlso ReCreateFreshDispatch(strShipmentNo, strVoucherNo, trans)

            Dim qry As String = "insert into TEMP_CREATE_SHIPMENT values ('" + strShipmentNo + "','" + strVoucherNo + "')"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(qry, trans)

            trans.Commit()
            Return isSaved
        Catch ex As Exception
            trans.Rollback()
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function ReCreateFreshDispatch(ByVal strShipmentNo As String, ByVal strVoucherNo As String, ByVal trans As SqlTransaction) As Boolean
        Dim dt As New DataTable()
        Try
            Dim isSaved As Boolean = True

            Dim Qry As String = "select distinct DOCUMENT_CODE from TSPL_SD_SALE_INVOICE_DETAIL where Shipment_Code='" + strShipmentNo + "'"
            dt = New DataTable()
            dt = clsDBFuncationality.GetDataTable(Qry, trans)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                'Qry = "Current Shipment is used in following Sale invoice -"
                'For Each dr As DataRow In dt.Rows
                '    Qry += Environment.NewLine + clsCommon.myCstr(dr("DOCUMENT_CODE"))
                'Next
                'Throw New Exception(Qry)
                Return True
            End If

            Qry = "select InOut,Trans_Type,Item_Code,Item_Desc,Location_Code,case when InOut='I' then -1 else 1 end *Qty as Qty ,UOM,MRP,ItemType,case when InOut='I' then -1 else 1 end* Basic_Cost as Basic_Cost from TSPL_INVENTORY_MOVEMENT where Source_Doc_No='" + strShipmentNo + "' and Trans_Type='SD-SH'"
            dt = clsDBFuncationality.GetDataTable(Qry, trans)
            Dim ArrLocationDetails As List(Of clsItemLocationDetails) = New List(Of clsItemLocationDetails)
            For Each objtr As DataRow In dt.Rows
                Dim dblConvFac As Double = clsItemMaster.GetConvertionFactor(clsCommon.myCstr(objtr("Item_Code")), clsCommon.myCstr(objtr("UOM")), trans)
                Dim objLocationDetails As New clsItemLocationDetails()
                objLocationDetails.Item_Code = clsCommon.myCstr(objtr("Item_Code"))
                objLocationDetails.Item_Desc = clsCommon.myCstr(objtr("Item_Desc"))
                objLocationDetails.Location_Code = clsCommon.myCstr(objtr("Location_Code"))
                objLocationDetails.Location_Desc = clsLocation.GetName(objLocationDetails.Location_Code, trans)
                objLocationDetails.Item_Qty = clsCommon.myCdbl(objtr("Qty")) / dblConvFac
                objLocationDetails.Amount = clsCommon.myCdbl(objtr("Basic_Cost"))
                objLocationDetails.MRP = clsCommon.myCdbl(objtr("MRP")) * dblConvFac
                objLocationDetails.ItemType = clsCommon.myCstr(objtr("ItemType"))
                ArrLocationDetails.Add(objLocationDetails)
            Next
            Dim strPostDate As String = clsCommon.GetPrintDate(clsCommon.GETSERVERDATE(trans), "dd/MM/yyyy")
            isSaved = isSaved AndAlso clsItemLocationDetails.SaveData(strPostDate, ArrLocationDetails, trans)

            Qry = "delete from tspl_serial_item where Against_Inv_Movement_Trans_Id in (select trans_id from TSPL_INVENTORY_MOVEMENT where Source_Doc_No='" + strShipmentNo + "' and Trans_Type='SD-SH')"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(Qry, trans)

            Qry = "delete from TSPL_INVENTORY_MOVEMENT where Source_Doc_No='" + strShipmentNo + "' and Trans_Type='SD-SH'"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(Qry, trans)

            Qry = "Update TSPL_SD_SHIPMENT_HEAD set Status = 0 where Document_Code='" + strShipmentNo + "' and trans_type='FS'"
            isSaved = isSaved AndAlso clsDBFuncationality.ExecuteNonQuery(Qry, trans)

            isSaved = isSaved AndAlso clsDispatchNoteFreshSale.PostData(clsUserMgtCode.FrmDispatchFreshSale, strShipmentNo, trans, strVoucherNo)

            Return isSaved
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Function
#End Region
End Class

