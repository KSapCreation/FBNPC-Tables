'=======BM00000006910=============BM00000007011======BM00000007858==========
Imports common
Imports System.Data.SqlClient

Public Class clsCancelTableClass
    Public Form_Id As String = ""
    Public Tb_Name As String = ""
    Public Column_Name As String = ""
    Public Tb_Name_History As String = ""
    Public Level_Id As String = ""
    Public Validate_Tb_name As String = ""
    Public Validate_Column_name As String = ""
    Public Validate_Form_name As String = ""

    Public Shared Function GetData(ByVal strType As String, ByVal trans As SqlTransaction) As String
        Return clsCommon.myCstr(clsDBFuncationality.getSingleValue("select Description from TSPL_Cancel_Table_Details where Form_Id='" + strType + "'", trans))
    End Function

    Public Shared Function GetDataValicationTables(ByVal strType As String, ByVal trans As SqlTransaction) As String
        Return clsCommon.myCstr(clsDBFuncationality.getSingleValue("select Description from TSPL_Cancel_Table_Validate_Details where Form_Id='" + strType + "'", trans))
    End Function

    '======Created by Rohit Gupta on 23-Jun-2015========================
    Public Shared Function GetCboDataTable(ByVal strType As String, ByVal trans As SqlTransaction) As DataTable
        Dim qry As String = "SELECT * FROM TSPL_Cancel_Table_Details where Type= '" + strType + "' "
        Dim dt_Cbo As DataTable = clsDBFuncationality.GetDataTable(qry, trans)
        Return dt_Cbo
    End Function

    Public Shared Function UpdateCancelTable(ByVal obj As clsCancelTableClass, ByVal trans As SqlTransaction, ByVal isNewEntry As Boolean) As Boolean
        Try
            Dim coll As New Hashtable()
            clsCommon.AddColumnsForChange(coll, "Tb_Name", obj.Tb_Name)
            clsCommon.AddColumnsForChange(coll, "Column_Name", obj.Column_Name)
            clsCommon.AddColumnsForChange(coll, "Tb_Name_History", obj.Tb_Name_History)
            If isNewEntry Then
                clsCommon.AddColumnsForChange(coll, "Form_Id", obj.Form_Id)
                clsCommon.AddColumnsForChange(coll, "Level_Id", obj.Level_Id)
                clsCommonFunctionality.UpdateDataTable(coll, "TSPL_Cancel_Table_Details", OMInsertOrUpdate.Insert, "", trans)
            Else
                clsCommonFunctionality.UpdateDataTable(coll, "TSPL_Cancel_Table_Details", OMInsertOrUpdate.Update, "Form_Id='" & obj.Form_Id & "' AND Level_Id='" & obj.Level_Id & "'", trans)
            End If

            coll = New Hashtable()
            clsCommon.AddColumnsForChange(coll, "Validate_Column_Name", obj.Validate_Column_name)
            clsCommon.AddColumnsForChange(coll, "Validate_Form_Name", obj.Validate_Form_name)
            If isNewEntry Then
                clsCommon.AddColumnsForChange(coll, "Form_Id", obj.Form_Id)
                clsCommon.AddColumnsForChange(coll, "Validate_Tb_Name", obj.Validate_Tb_name)
                clsCommonFunctionality.UpdateDataTable(coll, "TSPL_Cancel_Table_Validate_Details", OMInsertOrUpdate.Insert, "", trans)
            Else
                clsCommonFunctionality.UpdateDataTable(coll, "TSPL_Cancel_Table_Validate_Details", OMInsertOrUpdate.Update, "Form_Id='" & obj.Form_Id & "' AND Validate_Tb_Name='" & obj.Validate_Tb_name & "'", trans)
            End If

            Return True
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function GetCancelTable(ByVal trans As SqlTransaction) As DataTable
        Try
            Dim Qry As String = "select *  from TSPL_Cancel_Table_Details"
            Return clsDBFuncationality.GetDataTable(Qry)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function CancelTableValues() As Boolean
        '==Rohit (Insert SRN in cancel Table)
        InsertCancelTable("1", "PO-SRN", "Tspl_SRN_Head", "SRN_NO", "TSPL_SRN_HEAD_History", "", "", "0")
        InsertCancelTable("2", "PO-SRN", "Tspl_SRN_Detail", "SRN_NO", "TSPL_SRN_Detail_History", "", "", "0")
        InsertCancelTable("3", "PO-SRN", "Tspl_Inventory_Movement", "Source_Doc_No", "Tspl_Inventory_Movement_History", "Trans_Type", "SRN", "0")
        InsertCancelTable("4", "PO-SRN", "TSPL_JOURNAL_MASTER", "Source_Doc_No", "TSPL_JOURNAL_MASTER_History", "Source_Code", "PO-RC", "0")
        InsertCancelTable("5", "PO-SRN", "TSPL_JOURNAL_Details", "Voucher_No", "TSPL_JOURNAL_Details_History", "", "", "0")
        InsertCancelTable("6", "PO-SRN", "TSPL_JOURNAL_MASTER", "Source_Doc_No", "TSPL_JOURNAL_MASTER_History", "Source_Code", "SR-RG", "0")
        '==Rohit (Insert MRN in cancel Table)
        InsertCancelTable("1", "PO-MRN", "Tspl_MRN_Head", "MRN_NO", "TSPL_MRN_HEAD_History", "", "", "0")
        InsertCancelTable("2", "PO-MRN", "Tspl_MRN_Detail", "MRN_NO", "TSPL_MRN_Detail_History", "", "", "0")

        '==Rohit (Insert GRN in cancel Table)
        InsertCancelTable("1", "PO-GRN", "Tspl_GRN_Head", "GRN_NO", "TSPL_GRN_HEAD_History", "", "", "0")
        InsertCancelTable("2", "PO-GRN", "Tspl_GRN_Detail", "GRN_NO", "TSPL_GRN_Detail_History", "", "", "0")
        '==================================================
        '===Rohit(Insert Product sale Invoice Tables(Dispatch))============
        InsertCancelTable("1", "SHIPMENT-PS", "Tspl_SD_Shipment_Head", "DOCUMENT_CODE", "Tspl_SD_Shipment_Head_History", "", "", "0")
        InsertCancelTable("2", "SHIPMENT-PS", "Tspl_SD_Shipment_Detail", "DOCUMENT_CODE", "Tspl_SD_Shipment_Detail_History", "", "", "0")
        InsertCancelTable("3", "SHIPMENT-PS", "Tspl_Inventory_Movement", "Source_Doc_No", "Tspl_Inventory_Movement_History", "Trans_Type", "PS-SH", "0")
        InsertCancelTable("4", "SHIPMENT-PS", "TSPL_JOURNAL_MASTER", "Source_Doc_No", "TSPL_JOURNAL_MASTER_History", "Source_Code", "SD-SH", "0")
        InsertCancelTable("5", "SHIPMENT-PS", "TSPL_JOURNAL_Details", "Voucher_No", "TSPL_JOURNAL_Details_History", "", "", "0")
        InsertCancelTable("6", "SHIPMENT-PS", "TSPL_SD_SALE_INVOICE_Head", "Against_Shipment_No", "TSPL_SD_SALE_INVOICE_Head_History", "Trans_Type", "PS", "0")
        InsertCancelTable("7", "SHIPMENT-PS", "TSPL_SD_SALE_INVOICE_Detail", "DOCUMENT_CODE", "TSPL_SD_SALE_INVOICE_Detail_History", "", "", "0")

        InsertCancelTable("8", "SHIPMENT-PS", "TSPL_Customer_Invoice_Head", "Against_Sale_No", "TSPL_Customer_Invoice_Head_History", "Trans_Type", "PS", "0")
        InsertCancelTable("9", "SHIPMENT-PS", "TSPL_Customer_Invoice_Detail", "Document_No", "TSPL_Customer_Invoice_Detail_History", "", "", "0")

        InsertCancelTable("10", "SHIPMENT-PS", "TSPL_JOURNAL_MASTER", "Source_Doc_No", "TSPL_JOURNAL_MASTER_History", "Source_Code", "AR-IN", "0")
        InsertCancelTable("11", "SHIPMENT-PS", "TSPL_JOURNAL_Details", "Voucher_no", "TSPL_JOURNAL_Details_History", "", "", "0")
        '======================================================

        '===Rohit(Insert Fresh sale Invoice Tables)============
        InsertCancelTable("1", "DISPATCH-FS", "Tspl_SD_Shipment_Head", "DOCUMENT_CODE", "Tspl_SD_Shipment_Head_History", "", "", "0")
        InsertCancelTable("2", "DISPATCH-FS", "Tspl_SD_Shipment_Detail", "DOCUMENT_CODE", "Tspl_SD_Shipment_Detail_History", "", "", "0")
        InsertCancelTable("3", "DISPATCH-FS", "Tspl_Inventory_Movement", "Source_Doc_No", "Tspl_Inventory_Movement_History", "Trans_Type", "FS-SH", "0")
        InsertCancelTable("4", "DISPATCH-FS", "TSPL_JOURNAL_MASTER", "Source_Doc_No", "TSPL_JOURNAL_MASTER_History", "Source_Code", "SD-SH", "0")
        InsertCancelTable("5", "DISPATCH-FS", "TSPL_JOURNAL_Details", "Voucher_No", "TSPL_JOURNAL_Details_History", "", "", "0")
        InsertCancelTable("6", "DISPATCH-FS", "TSPL_SD_SALE_INVOICE_Head", "Against_Shipment_No", "TSPL_SD_SALE_INVOICE_Head_History", "Trans_Type", "FS", "0")
        InsertCancelTable("7", "DISPATCH-FS", "TSPL_SD_SALE_INVOICE_Detail", "DOCUMENT_CODE", "TSPL_SD_SALE_INVOICE_Detail_History", "", "", "0")

        InsertCancelTable("8", "DISPATCH-FS", "TSPL_Customer_Invoice_Head", "Against_Sale_No", "TSPL_Customer_Invoice_Head_History", "Trans_Type", "FS", "0")
        InsertCancelTable("9", "DISPATCH-FS", "TSPL_Customer_Invoice_Detail", "Document_No", "TSPL_Customer_Invoice_Detail_History", "", "", "0")

        InsertCancelTable("10", "DISPATCH-FS", "TSPL_JOURNAL_MASTER", "Source_Doc_No", "TSPL_JOURNAL_MASTER_History", "Source_Code", "AR-IN", "0")
        InsertCancelTable("11", "DISPATCH-FS", "TSPL_JOURNAL_Details", "Voucher_no", "TSPL_JOURNAL_Details_History", "", "", "0")
        '======================================================

        '===Rohit(Insert Misc. sale Invoice Tables)============
        InsertCancelTable("1", "SCRAP-SALE", "tspl_scrapsale_head", "Shipment_No", "tspl_scrapsale_head_History", "", "", "0")
        InsertCancelTable("2", "SCRAP-SALE", "tspl_scrapsale_Detail", "Shipment_No", "tspl_scrapsale_detail_History", "", "", "0")

        InsertCancelTable("3", "SCRAP-SALE", "TSPL_SCRAPINVOICE_HEAD", "Shipment_No", "TSPL_SCRAPINVOICE_HEAD_History", "", "", "0")
        InsertCancelTable("4", "SCRAP-SALE", "TSPL_SCRAPINVOICE_Detail", "Invoice_NO", "TSPL_SCRAPINVOICE_Detail_History", "", "", "0")
        InsertCancelTable("5", "SCRAP-SALE", "Tspl_Inventory_Movement", "Source_Doc_No", "Tspl_Inventory_Movement_History", "Trans_Type", "ScrapIn", "0")

        InsertCancelTable("6", "SCRAP-SALE", "TSPL_Customer_Invoice_Head", "AgainstScrap", "TSPL_Customer_Invoice_Head_History", "Trans_Type", "", "0")
        InsertCancelTable("7", "SCRAP-SALE", "TSPL_Customer_Invoice_Detail", "Document_No", "TSPL_Customer_Invoice_Detail_History", "", "", "0")

        InsertCancelTable("8", "SCRAP-SALE", "TSPL_JOURNAL_MASTER", "Source_Doc_No", "TSPL_JOURNAL_MASTER_History", "Source_Code", "AR-IN", "0")
        InsertCancelTable("9", "SCRAP-SALE", "TSPL_JOURNAL_Details", "Voucher_no", "TSPL_JOURNAL_Details_History", "", "", "0")
        '======================================================
        '===Rohit(Insert MCC sale Invoice Tables)============
        InsertCancelTable("1", "M-Material", "Tspl_SD_Shipment_Head", "DOCUMENT_CODE", "Tspl_SD_Shipment_Head_History", "", "", "0")
        InsertCancelTable("2", "M-Material", "Tspl_SD_Shipment_Detail", "DOCUMENT_CODE", "Tspl_SD_Shipment_Detail_History", "", "", "0")
        InsertCancelTable("3", "M-Material", "Tspl_Inventory_Movement", "Source_Doc_No", "Tspl_Inventory_Movement_History", "Trans_Type", "MCC-MSALE", "0")
        InsertCancelTable("4", "M-Material", "TSPL_JOURNAL_MASTER", "Source_Doc_No", "TSPL_JOURNAL_MASTER_History", "Source_Code", "SD-SH", "0")
        InsertCancelTable("5", "M-Material", "TSPL_JOURNAL_Details", "Voucher_No", "TSPL_JOURNAL_Details_History", "", "", "0")
        InsertCancelTable("6", "M-Material", "TSPL_SD_SALE_INVOICE_Head", "Against_Shipment_No", "TSPL_SD_SALE_INVOICE_Head_History", "Trans_Type", "MCC", "0")
        InsertCancelTable("7", "M-Material", "TSPL_SD_SALE_INVOICE_Detail", "DOCUMENT_CODE", "TSPL_SD_SALE_INVOICE_Detail_History", "", "", "0")

        InsertCancelTable("8", "M-Material", "TSPL_Customer_Invoice_Head", "Against_Sale_No", "TSPL_Customer_Invoice_Head_History", "Trans_Type", "MCC", "0")
        InsertCancelTable("9", "M-Material", "TSPL_Customer_Invoice_Detail", "Document_No", "TSPL_Customer_Invoice_Detail_History", "", "", "0")

        InsertCancelTable("10", "M-Material", "TSPL_JOURNAL_MASTER", "Source_Doc_No", "TSPL_JOURNAL_MASTER_History", "Source_Code", "AR-IN", "0")
        InsertCancelTable("11", "M-Material", "TSPL_JOURNAL_Details", "Voucher_no", "TSPL_JOURNAL_Details_History", "", "", "0")
        '======================================================
        '===Rohit(Insert Bulk sale Invoice Tables)============
        InsertCancelTable("1", "INVOICE-BS", "TSPL_INVOICE_MASTER_BULKSALE", "Document_No", "TSPL_INVOICE_MASTER_BULKSALE_History", "", "", "0")
        InsertCancelTable("2", "INVOICE-BS", "TSPL_INVOICE_DETAIL_BULKSALE", "Document_No", "TSPL_INVOICE_DETAIL_BULKSALE_History", "", "", "0")

        InsertCancelTable("3", "INVOICE-BS", "TSPL_Customer_Invoice_Head", "Against_Sale_No", "TSPL_Customer_Invoice_Head_History", "Trans_Type", "BS", "0")
        InsertCancelTable("4", "INVOICE-BS", "TSPL_Customer_Invoice_Detail", "Document_No", "TSPL_Customer_Invoice_Detail_History", "", "", "0")

        InsertCancelTable("5", "INVOICE-BS", "TSPL_JOURNAL_MASTER", "Source_Doc_No", "TSPL_JOURNAL_MASTER_History", "Source_Code", "AR-IN", "0")
        InsertCancelTable("6", "INVOICE-BS", "TSPL_JOURNAL_Details", "Voucher_no", "TSPL_JOURNAL_Details_History", "", "", "0")
        '======================================================
        Try
            Dim qry As String = " select distinct OtherAssemblyFilePathAndName  from TSPL_PROGRAM_MASTER  where isnull(IsLoadFromOtherAssembly ,0)=1"
            Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim AsmName As String = clsCommon.myCstr(dt.Rows(i)("OtherAssemblyFilePathAndName"))
                    clsCreateAllTables.InvokeMethodSlow(AsmName, "clsCancelTableClassCustom", "CreateAllTable", Nothing)
                Next
            End If
        Catch ex As Exception
        End Try
        Return Nothing
    End Function

    Public Shared Function CancelValidationValues() As Boolean
        '==Rohit (Insert SRN in Validation Table)
        InsertValidationTables("PO-SRN", "Tspl_PI_Detail", "SRN_ID", "Purchase Invoice", "", "", "")
        InsertValidationTables("PO-GRN", "Tspl_MRN_Head", "Against_GRN", "Material Received Note", "", "", "")
        InsertValidationTables("PO-MRN", "Tspl_SRN_Head", "Against_MRN", "Store Received Note", "", "", "")
        InsertValidationTables("SHIPMENT-PS", "TSPL_RECEIPT_DETAIL", "Document_No", "Receipt Entry", "", "", "1")
        InsertValidationTables("SHIPMENT-PS", "TSPL_SD_SALE_RETURN_DETAIL", "Invoice_Code", "Sale Return", "", "", "2")
        InsertValidationTables("DISPATCH-FS", "TSPL_RECEIPT_DETAIL", "Document_No", "Receipt Entry", "", "", "1")
        InsertValidationTables("DISPATCH-FS", "TSPL_SD_SALE_RETURN_DETAIL", "Invoice_Code", "Sale Return", "", "", "2")
        InsertValidationTables("SCRAP-SALE", "TSPL_RECEIPT_DETAIL", "Document_No", "Receipt Entry", "", "", "1")
        InsertValidationTables("SCRAP-SALE", "TSPL_SD_SALE_RETURN_DETAIL", "Invoice_Code", "Sale Return", "", "", "2")
        InsertValidationTables("M-Material", "TSPL_RECEIPT_DETAIL", "Document_No", "Receipt Entry", "", "", "1")
        InsertValidationTables("M-Material", "TSPL_SD_SALE_RETURN_DETAIL", "Invoice_Code", "Sale Return", "", "", "2")
        InsertValidationTables("M-Material", "TSPL_RECEIPT_DETAIL", "Document_No", "Receipt Entry", "", "", "1")
        InsertValidationTables("INVOICE-BS", "TSPL_RECEIPT_DETAIL", "Document_No", "Receipt Entry", "", "", "1")
        Try
            Dim qry As String = " select distinct OtherAssemblyFilePathAndName  from TSPL_PROGRAM_MASTER  where isnull(IsLoadFromOtherAssembly ,0)=1"
            Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim AsmName As String = clsCommon.myCstr(dt.Rows(i)("OtherAssemblyFilePathAndName"))
                    clsCreateAllTables.InvokeMethodSlow(AsmName, "clsCancelTableClassCustom", "CancelValidationValues", Nothing)
                Next
            End If
        Catch ex As Exception
        End Try
        Return Nothing
    End Function

    Public Shared Function CancelConditionTableValues() As Boolean
        InsertConditionTables("PO-SRN", "1", "TSPL_JOURNAL_MASTER", "Voucher_No", "Source_Doc_No", "5", "Source_Code", "PO-RC", "")
        InsertConditionTables("PO-SRN", "2", "TSPL_JOURNAL_MASTER", "Voucher_No", "Source_Doc_No", "5", "Source_Code", "SR-RG", "")
        '==Rohit (Insert Product Sale in Validation Table)
        InsertConditionTables("SHIPMENT-PS", "1", "TSPL_SD_SALE_INVOICE_HEAD", "Document_Code", "Against_Shipment_No", "7", "Trans_Type", "PS", "")
        InsertConditionTables("SHIPMENT-PS", "2", "TSPL_SD_SALE_INVOICE_HEAD", "Document_Code", "Against_Shipment_No", "8", "Trans_Type", "PS", "")
        InsertConditionTables("SHIPMENT-PS", "3", "TSPL_Customer_Invoice_Head", "Document_No", "Against_Sale_No", "9", "", "", "")
        InsertConditionTables("SHIPMENT-PS", "4", "TSPL_SD_SALE_INVOICE_HEAD", "Document_Code", "Against_Shipment_No", "9", "Trans_Type", "PS", "")
        InsertConditionTables("SHIPMENT-PS", "5", "TSPL_Customer_Invoice_Head", "Document_No", "Against_Sale_No", "10", "", "", "")
        InsertConditionTables("SHIPMENT-PS", "6", "TSPL_SD_SALE_INVOICE_HEAD", "Document_Code", "Against_Shipment_No", "10", "Trans_Type", "PS", "")
        InsertConditionTables("SHIPMENT-PS", "8", "TSPL_Customer_Invoice_Head", "Document_No", "Against_Sale_No", "11", "", "", "")
        InsertConditionTables("SHIPMENT-PS", "9", "TSPL_SD_SALE_INVOICE_HEAD", "Document_Code", "Against_Shipment_No", "11", "Trans_Type", "PS", "")
        InsertConditionTables("SHIPMENT-PS", "7", "TSPL_JOURNAL_MASTER", "Voucher_No", "Source_Doc_No", "11", "Source_Code", "AR-IN", "")
        InsertConditionTables("SHIPMENT-PS", "10", "TSPL_Customer_Invoice_Head", "Document_No", "Against_Sale_No", "1", "", "", "1")
        InsertConditionTables("SHIPMENT-PS", "11", "TSPL_SD_SALE_INVOICE_HEAD", "Document_Code", "Against_Shipment_No", "1", "Trans_Type", "PS", "1")
        InsertConditionTables("SHIPMENT-PS", "11", "TSPL_SD_SALE_INVOICE_HEAD", "Document_Code", "Against_Shipment_No", "2", "Trans_Type", "PS", "1")

        '==Rohit (Insert Fresh Sale in Validation Table)
        InsertConditionTables("DISPATCH-FS", "1", "TSPL_SD_SALE_INVOICE_HEAD", "Document_Code", "Against_Shipment_No", "7", "Trans_Type", "FS", "")
        InsertConditionTables("DISPATCH-FS", "2", "TSPL_SD_SALE_INVOICE_HEAD", "Document_Code", "Against_Shipment_No", "8", "Trans_Type", "FS", "")
        InsertConditionTables("DISPATCH-FS", "3", "TSPL_Customer_Invoice_Head", "Document_No", "Against_Sale_No", "9", "", "", "")
        InsertConditionTables("DISPATCH-FS", "4", "TSPL_SD_SALE_INVOICE_HEAD", "Document_Code", "Against_Shipment_No", "9", "Trans_Type", "FS", "")
        InsertConditionTables("DISPATCH-FS", "5", "TSPL_Customer_Invoice_Head", "Document_No", "Against_Sale_No", "10", "", "", "")
        InsertConditionTables("DISPATCH-FS", "6", "TSPL_SD_SALE_INVOICE_HEAD", "Document_Code", "Against_Shipment_No", "10", "Trans_Type", "FS", "")
        InsertConditionTables("DISPATCH-FS", "8", "TSPL_Customer_Invoice_Head", "Document_No", "Against_Sale_No", "11", "", "", "")
        InsertConditionTables("DISPATCH-FS", "9", "TSPL_SD_SALE_INVOICE_HEAD", "Document_Code", "Against_Shipment_No", "11", "Trans_Type", "FS", "")
        InsertConditionTables("DISPATCH-FS", "7", "TSPL_JOURNAL_MASTER", "Voucher_No", "Source_Doc_No", "11", "Source_Code", "AR-IN", "")
        InsertConditionTables("DISPATCH-FS", "10", "TSPL_Customer_Invoice_Head", "Document_No", "Against_Sale_No", "1", "", "", "1")
        InsertConditionTables("DISPATCH-FS", "11", "TSPL_SD_SALE_INVOICE_HEAD", "Document_Code", "Against_Shipment_No", "1", "Trans_Type", "FS", "1")
        InsertConditionTables("DISPATCH-FS", "11", "TSPL_SD_SALE_INVOICE_HEAD", "Document_Code", "Against_Shipment_No", "2", "Trans_Type", "FS", "1")

        '==Rohit (Insert Misc. Sale in Validation Table)
        InsertConditionTables("SCRAP-SALE", "1", "TSPL_SCRAPINVOICE_HEAD", "Invoice_No", "Shipment_No", "4", "", "", "")
        InsertConditionTables("SCRAP-SALE", "2", "TSPL_SCRAPINVOICE_HEAD", "Invoice_No", "Shipment_No", "5", "", "", "")
        InsertConditionTables("SCRAP-SALE", "4", "TSPL_SCRAPINVOICE_HEAD", "Invoice_No", "Shipment_No", "6", "", "", "")
        InsertConditionTables("SCRAP-SALE", "3", "TSPL_Customer_Invoice_Head", "Document_No", "AgainstScrap", "7", "", "", "")
        InsertConditionTables("SCRAP-SALE", "4", "TSPL_SCRAPINVOICE_HEAD", "Invoice_No", "Shipment_No", "7", "", "", "")
        InsertConditionTables("SCRAP-SALE", "5", "TSPL_Customer_Invoice_Head", "Document_No", "AgainstScrap", "8", "", "", "")
        InsertConditionTables("SCRAP-SALE", "6", "TSPL_SCRAPINVOICE_HEAD", "Invoice_No", "Shipment_No", "8", "", "", "")
        InsertConditionTables("SCRAP-SALE", "8", "TSPL_Customer_Invoice_Head", "Document_No", "AgainstScrap", "9", "", "", "")
        InsertConditionTables("SCRAP-SALE", "9", "TSPL_SCRAPINVOICE_HEAD", "Invoice_No", "Shipment_No", "9", "", "", "")
        InsertConditionTables("SCRAP-SALE", "7", "TSPL_JOURNAL_MASTER", "Voucher_No", "Source_Doc_No", "9", "Source_Code", "AR-IN", "")
        InsertConditionTables("SCRAP-SALE", "10", "TSPL_Customer_Invoice_Head", "Document_No", "AgainstScrap", "1", "", "", "1")
        InsertConditionTables("SCRAP-SALE", "11", "TSPL_SCRAPINVOICE_HEAD", "Invoice_No", "Shipment_No", "1", "", "", "1")
        InsertConditionTables("SCRAP-SALE", "12", "TSPL_SCRAPINVOICE_HEAD", "Invoice_No", "Shipment_No", "2", "", "", "1")

        '==Rohit (Insert MCC Sale in Validation Table)
        InsertConditionTables("M-Material", "1", "TSPL_SD_SALE_INVOICE_HEAD", "Document_Code", "Against_Shipment_No", "7", "Trans_Type", "MCC", "")
        InsertConditionTables("M-Material", "2", "TSPL_SD_SALE_INVOICE_HEAD", "Document_Code", "Against_Shipment_No", "8", "Trans_Type", "MCC", "")
        InsertConditionTables("M-Material", "3", "TSPL_Customer_Invoice_Head", "Document_No", "Against_Sale_No", "9", "", "", "")
        InsertConditionTables("M-Material", "4", "TSPL_SD_SALE_INVOICE_HEAD", "Document_Code", "Against_Shipment_No", "9", "Trans_Type", "MCC", "")
        InsertConditionTables("M-Material", "5", "TSPL_Customer_Invoice_Head", "Document_No", "Against_Sale_No", "10", "", "", "")
        InsertConditionTables("M-Material", "6", "TSPL_SD_SALE_INVOICE_HEAD", "Document_Code", "Against_Shipment_No", "10", "Trans_Type", "MCC", "")
        InsertConditionTables("M-Material", "8", "TSPL_Customer_Invoice_Head", "Document_No", "Against_Sale_No", "11", "", "", "")
        InsertConditionTables("M-Material", "9", "TSPL_SD_SALE_INVOICE_HEAD", "Document_Code", "Against_Shipment_No", "11", "Trans_Type", "MCC", "")
        InsertConditionTables("M-Material", "7", "TSPL_JOURNAL_MASTER", "Voucher_No", "Source_Doc_No", "11", "Source_Code", "AR-IN", "")
        InsertConditionTables("M-Material", "10", "TSPL_Customer_Invoice_Head", "Document_No", "Against_Sale_No", "1", "", "", "1")
        InsertConditionTables("M-Material", "11", "TSPL_SD_SALE_INVOICE_HEAD", "Document_Code", "Against_Shipment_No", "1", "Trans_Type", "MCC", "1")
        InsertConditionTables("M-Material", "11", "TSPL_SD_SALE_INVOICE_HEAD", "Document_Code", "Against_Shipment_No", "2", "Trans_Type", "MCC", "1")

        '==Rohit (Insert Bulk Sale in Validation Table)
        InsertConditionTables("INVOICE-BS", "1", "TSPL_Customer_Invoice_Head", "Document_No", "Against_Sale_No", "4", "Trans_Type", "BS", "")
        InsertConditionTables("INVOICE-BS", "1", "TSPL_Customer_Invoice_Head", "Document_No", "Against_Sale_No", "5", "", "", "")
        InsertConditionTables("INVOICE-BS", "2", "TSPL_Customer_Invoice_Head", "Document_No", "Against_Sale_No", "6", "", "", "")
        InsertConditionTables("INVOICE-BS", "1", "TSPL_JOURNAL_MASTER", "Voucher_No", "Source_Doc_No", "6", "Source_Code", "AR-IN", "")
        InsertConditionTables("INVOICE-BS", "1", "TSPL_Customer_Invoice_Head", "Document_No", "Against_Sale_No", "1", "", "", "1")
        'Try
        '    Dim qry As String = " select distinct OtherAssemblyFilePathAndName  from TSPL_PROGRAM_MASTER  where isnull(IsLoadFromOtherAssembly ,0)=1"
        '    Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry)
        '    If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
        '        For i As Integer = 0 To dt.Rows.Count - 1
        '            Dim AsmName As String = clsCommon.myCstr(dt.Rows(i)("OtherAssemblyFilePathAndName"))
        '            clsCreateAllTables.InvokeMethodSlow(AsmName, "clsCancelTableClassCustom", "CancelConditionTableValues", Nothing)
        '        Next
        '    End If
        'Catch ex As Exception
        'End Try
        Return Nothing
    End Function
    
    Private Shared Function InsertCancelTable(ByVal Level_Id As String, ByVal Form_Id As String, ByVal strTbName As String, ByVal strColumnName As String, ByVal strTbNameHistory As String, ByVal strTypecolName As String, ByVal strTypecolvalue As String, ByVal strAllowDeleteAfterPosting As String) As Boolean
        Dim qry As String = "select * from TSPL_Cancel_Table_Details where Form_Id='" + Form_Id + "' and Level_Id ='" + Level_Id + "'"
        Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry)

        Dim coll As New Hashtable()
        clsCommon.AddColumnsForChange(coll, "Form_Id", Form_Id)
        clsCommon.AddColumnsForChange(coll, "Level_Id", Level_Id)
        clsCommon.AddColumnsForChange(coll, "Tb_Name", strTbName)
        clsCommon.AddColumnsForChange(coll, "Tb_Name_History", strTbNameHistory)
        clsCommon.AddColumnsForChange(coll, "Column_Name", strColumnName)
        clsCommon.AddColumnsForChange(coll, "Type_Col_Name", strTypecolName)
        clsCommon.AddColumnsForChange(coll, "Type_Col_Value", strTypecolvalue)
        clsCommon.AddColumnsForChange(coll, "Allow_Delete_After_Posting", strAllowDeleteAfterPosting)

        If dt Is Nothing OrElse dt.Rows.Count <= 0 Then
            clsCommonFunctionality.UpdateDataTable(coll, "TSPL_Cancel_Table_Details", OMInsertOrUpdate.Insert, "")
        End If
        Return True
    End Function

    Private Shared Function InsertValidationTables(ByVal Form_Id As String, ByVal strValidationTbName As String, ByVal strValidationColumnName As String, ByVal strValidationFormName As String, ByVal strTypecolName As String, ByVal strTypecolvalue As String, ByVal strLevel_Id As String) As Boolean
        Dim qry As String = "select * from TSPL_Cancel_Table_Validate_Details where Form_Id='" + Form_Id + "' and Valicate_tb_name ='" + strValidationTbName + "'"
        Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry)

        Dim coll As New Hashtable()
        clsCommon.AddColumnsForChange(coll, "Form_Id", Form_Id)
        clsCommon.AddColumnsForChange(coll, "Valicate_tb_name", strValidationTbName)
        clsCommon.AddColumnsForChange(coll, "Valicate_Column_name", strValidationColumnName)
        clsCommon.AddColumnsForChange(coll, "Valicate_Form_name", strValidationFormName)
        clsCommon.AddColumnsForChange(coll, "Type_Col_Name", strTypecolName)
        clsCommon.AddColumnsForChange(coll, "Type_Col_Value", strTypecolvalue)
        clsCommon.AddColumnsForChange(coll, "Level_Id", strLevel_Id)


        If dt Is Nothing OrElse dt.Rows.Count <= 0 Then
            clsCommonFunctionality.UpdateDataTable(coll, "TSPL_Cancel_Table_Validate_Details", OMInsertOrUpdate.Insert, "")
        End If
        Return True
    End Function

    Private Shared Function InsertConditionTables(ByVal Form_Id As String, ByVal StrConditionLevelId As String, ByVal strConditionTbName As String, ByVal strConditionColumnName As String, ByVal strConditionForeigncolname As String, ByVal Str_Base_Level_Id As String, ByVal strTypecolName As String, ByVal strTypecolvalue As String, ByVal StrIsForValidate As String) As Boolean
        Dim qry As String = "select * from TSPL_Cancel_Condition_Tables_Details where Form_Id='" + Form_Id + "' and Condition_tb_name ='" + strConditionTbName + "' and Base_Level_Id='" & Str_Base_Level_Id & "'"
        Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry)

        Dim coll As New Hashtable()
        clsCommon.AddColumnsForChange(coll, "Form_Id", Form_Id)
        clsCommon.AddColumnsForChange(coll, "Base_Level_Id", Str_Base_Level_Id)
        clsCommon.AddColumnsForChange(coll, "Condition_tb_name", strConditionTbName)
        clsCommon.AddColumnsForChange(coll, "Condition_Col_name", strConditionColumnName)
        clsCommon.AddColumnsForChange(coll, "Condition_Foreign_Col_name", strConditionForeigncolname)
        clsCommon.AddColumnsForChange(coll, "Condition_Level_Id", StrConditionLevelId)
        clsCommon.AddColumnsForChange(coll, "Type_Col_Name", strTypecolName)
        clsCommon.AddColumnsForChange(coll, "Type_Col_Value", strTypecolvalue)
        clsCommon.AddColumnsForChange(coll, "Is_For_Validate", StrIsForValidate)
        If dt Is Nothing OrElse dt.Rows.Count <= 0 Then
            clsCommonFunctionality.UpdateDataTable(coll, "TSPL_Cancel_Condition_Tables_Details", OMInsertOrUpdate.Insert, "")
        End If
        Return True
    End Function
End Class