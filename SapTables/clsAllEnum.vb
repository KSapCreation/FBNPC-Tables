Imports System.Data.SqlClient
Imports common
Imports System.IO
Imports Telerik.WinControls
Imports System.Windows.Forms
Imports Telerik.WinControls.UI
Imports System.Drawing

Public Class Xtra

    '---- Created By Richa Agarwal-----Ticket no. BM00000003242 on 29/07/2014
    Public Shared Function CustomerPermission() As String
        Dim qry As String = ""
        Dim strvalue As String = ""
        qry = "select distinct Cust_Code from TSPL_CUSTOMER_MAPPING where User_Code ='" + objCommonVar.CurrentUserCode + "' and Comp_Code='" + objCommonVar.CurrentCompanyCode + "'"
        Dim dtNew As DataTable = clsDBFuncationality.GetDataTable(qry)

        If dtNew IsNot Nothing AndAlso dtNew.Rows.Count > 0 Then
            For Each dr As DataRow In dtNew.Rows
                strvalue = strvalue & "'" & clsCommon.myCstr(dr("Cust_Code")).Replace("'", "''").ToString() & "'" & ","
            Next

            If strvalue <> "" Then
                strvalue = strvalue.Substring(0, strvalue.Length - 1)
            End If

        End If
        Try

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return strvalue
    End Function

    Public Shared Function UpdateBalanceQtyAndBalanceQtyInBottleOFTransfer(ByVal strTransferNo As String, ByVal trans As SqlTransaction) As Boolean
        Dim qry As String = " update TSPL_TRANSFER_DETAIL set Pending_Qty=xxxxx.BalanceQty,Pending_Balance_In_Bottle=xxxxx.balanceInBottel"
        qry += "  from("
        qry += "  select xxxx.*,(xxxx.BalanceQty*(select Conversion_Factor from TSPL_ITEM_UOM_DETAIL where TSPL_ITEM_UOM_DETAIL.Item_Code=xxxx.Item_Code and TSPL_ITEM_UOM_DETAIL.UOM_Code='FB')) as balanceInBottel from ("
        qry += "  select  Transfer_No,max(Line_No) as Line_No,Item_Code,sum(Item_Qty * case when RI in (1,5) then 1 else case when RI in (2,3,4) then -1 else 0 end end) as BalanceQty,MRP from ("
        qry += "  select TSPL_TRANSFER_DETAIL.Transfer_No,TSPL_TRANSFER_DETAIL.Line_No,TSPL_TRANSFER_DETAIL.Item_Code,TSPL_TRANSFER_DETAIL.Price_Date,TSPL_TRANSFER_DETAIL.MRP,TSPL_TRANSFER_DETAIL.Item_Qty,1 as RI,1 as chk from TSPL_TRANSFER_DETAIL "
        qry += "  left outer join  TSPL_TRANSFER_HEAD on TSPL_TRANSFER_HEAD.Transfer_No=TSPL_TRANSFER_DETAIL.Transfer_No where TSPL_TRANSFER_HEAD.Transfer_Type='LO' and TSPL_TRANSFER_HEAD.Post='Y'"

        If clsCommon.myLen(strTransferNo) > 0 Then
            qry += " and TSPL_TRANSFER_DETAIL.Transfer_No='" + strTransferNo + "'"
        End If

        qry += "  union all"
        qry += "  select TSPL_SHIPMENT_MASTER.Transfer_No as Transfer_No,0 as Line_No,TSPL_SHIPMENT_DETAILS.Item_Code,TSPL_SHIPMENT_DETAILS.Price_Date,MRP_Amt*Conversion_Factor as MRP,TSPL_SHIPMENT_DETAILS.Shipped_Qty /Conversion_Factor as Item_Qty ,2 as RI,0 as chk"
        qry += "  from TSPL_SHIPMENT_DETAILS "
        qry += "  left outer join TSPL_SHIPMENT_MASTER on TSPL_SHIPMENT_MASTER.Shipment_No=TSPL_SHIPMENT_DETAILS.Shipment_No"
        qry += "  left outer join TSPL_ITEM_UOM_DETAIL on TSPL_ITEM_UOM_DETAIL.Item_Code=TSPL_SHIPMENT_DETAILS.Item_Code and TSPL_ITEM_UOM_DETAIL.UOM_Code=TSPL_SHIPMENT_DETAILS.Unit_code"
        qry += "  where TSPL_SHIPMENT_MASTER.Is_Post='Y' and LEN( ISNULL(TSPL_SHIPMENT_MASTER.Transfer_No,''))>0"
        qry += "  union all"
        qry += "  select TSPL_TRANSFER_HEAD.Load_Out_No as Transfer_No,0 as Line_No,TSPL_TRANSFER_DETAIL.Item_Code,TSPL_TRANSFER_DETAIL.Price_Date,MRP*Conversion_Factor as MRP,(ISNULL( TSPL_TRANSFER_DETAIL.Burst,0)+isnull(TSPL_TRANSFER_DETAIL.Leak,0)+isnull(TSPL_TRANSFER_DETAIL.Shortage,0)+TSPL_TRANSFER_DETAIL.LoadIn_Qty) /Conversion_Factor  as Item_Qty  ,4 as RI,0 as chk"
        qry += "  from TSPL_TRANSFER_DETAIL "
        qry += "  left outer join TSPL_TRANSFER_HEAD on TSPL_TRANSFER_DETAIL.Transfer_No=TSPL_TRANSFER_HEAD.Transfer_No"
        qry += "  left outer join TSPL_ITEM_UOM_DETAIL on TSPL_ITEM_UOM_DETAIL.Item_Code=TSPL_TRANSFER_DETAIL.Item_Code and TSPL_ITEM_UOM_DETAIL.UOM_Code=TSPL_TRANSFER_DETAIL.Uom where  Transfer_Type='LI' and len(ISNULL(TSPL_TRANSFER_HEAD.Load_Out_No,''))>0 and TSPL_TRANSFER_HEAD.Post='Y'"
        qry += "  union all"
        qry += "  select TSPL_SHIPMENT_MASTER.Transfer_No as Transfer_No,0 as Line_No,TSPL_SALE_RETURN_DETAIL.Item_Code,TSPL_SALE_RETURN_DETAIL.Price_Date,MRP_Amt*Conversion_Factor as MRP,  TSPL_SALE_RETURN_DETAIL.Return_Qty/Conversion_Factor  as Item_Qty  ,5 as RI,0 as chk from TSPL_SALE_RETURN_DETAIL left outer join TSPL_SALE_RETURN_HEAD on TSPL_SALE_RETURN_HEAD.Sale_Return_No=TSPL_SALE_RETURN_DETAIL.Sale_Return_No left outer join TSPL_SALE_INVOICE_HEAD on TSPL_SALE_INVOICE_HEAD.Sale_Invoice_No=TSPL_SALE_RETURN_HEAD.Invoice_No left outer join TSPL_SHIPMENT_MASTER on TSPL_SHIPMENT_MASTER.Shipment_No=TSPL_SALE_INVOICE_HEAD.Shipment_No left outer join TSPL_ITEM_UOM_DETAIL on TSPL_ITEM_UOM_DETAIL.Item_Code=TSPL_SALE_RETURN_DETAIL.Item_Code and TSPL_ITEM_UOM_DETAIL.UOM_Code=TSPL_SALE_RETURN_DETAIL.Unit_code where  TSPL_SHIPMENT_MASTER.Shipment_Type='Transfer' and LEN(ISNULL(TSPL_SHIPMENT_MASTER.Transfer_No,''))>0 and ISNULL( TSPL_SALE_RETURN_HEAD.Is_Post,'')='Y'"
        qry += "  ) xxx group by Transfer_No,Item_Code,MRP having SUM(chk)>0   "
        qry += "  )xxxx"
        qry += "  )xxxxx "
        qry += "  inner join TSPL_TRANSFER_DETAIL on TSPL_TRANSFER_DETAIL.Transfer_No=xxxxx.Transfer_No and TSPL_TRANSFER_DETAIL.Line_No=xxxxx.Line_No"
        clsDBFuncationality.ExecuteNonQuery(qry, trans)


        Return True
    End Function

    Public Shared Function UpdateSaleInvoiceBalanceAmt() As Boolean
        Dim trans As SqlTransaction = clsDBFuncationality.GetTransactin()
        Try
            UpdateSaleInvoiceBalanceAmt(trans)
            trans.Commit()
        Catch ex As Exception
            trans.Rollback()
            Throw New Exception(ex.Message)
        End Try
        Return True
    End Function

    Public Shared Function UpdateSaleInvoiceBalanceAmt(ByVal trans As SqlTransaction) As Boolean

        Try
            Dim qry As String = "update  TSPL_SALE_INVOICE_HEAD set Balance_Amt=xxxx.Amt"
            qry += " from("
            qry += " select Code,sum(Amt* RI ) as Amt from ("
            qry += " select Sale_Invoice_No as Code, Empty_Value+Total_Invoice_Amt as Amt,1 as RI,1 as Chk   from TSPL_SALE_INVOICE_HEAD where Is_Post='Y'"
            qry += " union all "
            qry += " select TSPL_RECEIPT_DETAIL.Document_No as Code,Applied_Amount as Amt,-1 as RI,0 as chk  from TSPL_RECEIPT_DETAIL"
            qry += " left outer join  TSPL_RECEIPT_HEADER on TSPL_RECEIPT_HEADER.Receipt_No =TSPL_RECEIPT_DETAIL.Receipt_No"
            qry += " where TSPL_RECEIPT_HEADER.Posted='Y'  and LEN(ISNULL(TSPL_RECEIPT_DETAIL.Document_No,''))>0"
            qry += " union all "
            qry += " select TSPL_Receipt_Adjustment_Header.Doc_No as Code,TSPL_Receipt_Adjustment_Detail.Amount as Amt,-1 as RI,0 as Chk from TSPL_Receipt_Adjustment_Detail "
            qry += " left outer join TSPL_Receipt_Adjustment_Header on TSPL_Receipt_Adjustment_Header.Adjustment_No=TSPL_Receipt_Adjustment_Detail.Adjustment_No"
            qry += " where TSPL_Receipt_Adjustment_Header.Is_Post='Y' and LEN(ISNULL(TSPL_Receipt_Adjustment_Header.Doc_No,''))>0"
            qry += " union all "
            qry += " select TSPL_ADJUSTMENT_HEADER.Document_No as Code,TSPL_ADJUSTMENT_DETAIL.Item_Cost as Amt,case when Trans_Type='Out' then 1 else -1 end as RI,0 as Chk from TSPL_ADJUSTMENT_DETAIL"
            qry += " left outer join TSPL_ADJUSTMENT_HEADER on TSPL_ADJUSTMENT_HEADER.Adjustment_No=TSPL_ADJUSTMENT_DETAIL.Adjustment_No"
            qry += " where TSPL_ADJUSTMENT_HEADER.Posted='Y' and TSPL_ADJUSTMENT_HEADER.Reference_Document='Sale Invoice' and TSPL_ADJUSTMENT_HEADER.ItemType='E' "
            qry += " and LEN(ISNULL(TSPL_ADJUSTMENT_HEADER.Document_No,''))>0"
            qry += "  union all "
            qry += " select Invoice_No as Code, Empty_Value+Total_Invoice_Amt as Amt,-1 as RI,0 as Chk   from TSPL_SALE_RETURN_HEAD where Is_Post='Y'"
            qry += " union all "
            qry += " select TSPL_RECEIPT_DETAIL.Document_No,TSPL_RECEIPT_DETAIL.Applied_Amount as Amt,1 as RI,0 as chk from TSPL_BANK_REVERSE"
            qry += " inner join TSPL_RECEIPT_HEADER on TSPL_RECEIPT_HEADER.Bank_Code=TSPL_BANK_REVERSE.Bank_Code and TSPL_RECEIPT_HEADER.Receipt_No=TSPL_BANK_REVERSE.Document_No and TSPL_RECEIPT_HEADER.Cust_Code=TSPL_BANK_REVERSE.Cust_Code"
            qry += " left outer join TSPL_RECEIPT_DETAIL on TSPL_RECEIPT_HEADER.Receipt_No =TSPL_RECEIPT_DETAIL.Receipt_No "
            qry += " where TSPL_BANK_REVERSE.Source_Type='AR' and Reverse_Document='Receipts'  and LEN(ISNULL(TSPL_RECEIPT_DETAIL.Document_No,''))>0 and TSPL_BANK_REVERSE.Post='P'"
            qry += " ) xxx"
            qry += " group by Code having SUM(chk)>0"
            qry += " )xxxx"
            qry += " inner join TSPL_SALE_INVOICE_HEAD on TSPL_SALE_INVOICE_HEAD.Sale_Invoice_No = xxxx.Code"
            clsDBFuncationality.ExecuteNonQuery(qry, trans)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return True
    End Function

    Public Shared Function UpdateAPInvoiceBalanceAmount(ByVal strAPDocumentNo As String, ByVal trans As SqlTransaction) As Boolean
        Dim qry As String = "update TSPL_VENDOR_INVOICE_HEAD set Balance_Amt=xxxx.Amt" + Environment.NewLine
        qry += " from(" + Environment.NewLine
        qry += " select Document_No,SUM(Amt*RI) as Amt from (" + Environment.NewLine
        qry += " select TSPL_VENDOR_INVOICE_HEAD.Document_No,TSPL_VENDOR_INVOICE_HEAD.Document_Total-TSPL_VENDOR_INVOICE_HEAD.TDS_Actual_Amount as Amt,1 as RI,1 as Chk from TSPL_VENDOR_INVOICE_HEAD where Document_Type= 'I'"
        If clsCommon.myLen(strAPDocumentNo) > 0 Then
            qry += " and TSPL_VENDOR_INVOICE_HEAD.Document_No='" + strAPDocumentNo + "'"
        End If
        qry += Environment.NewLine + " union all" + Environment.NewLine
        qry += " select PIVendorInvooiceHead.Document_No,TSPL_VENDOR_INVOICE_HEAD.Document_Total-TSPL_VENDOR_INVOICE_HEAD.TDS_Actual_Amount as Amt,-1 as RI,0 as Chk " + Environment.NewLine
        qry += " from TSPL_PR_HEAD" + Environment.NewLine
        qry += " left outer join TSPL_PR_DETAIL on TSPL_PR_DETAIL.PR_No=TSPL_PR_HEAD.PR_No and TSPL_PR_DETAIL.Line_No=1" + Environment.NewLine
        qry += " left outer join TSPL_VENDOR_INVOICE_HEAD on TSPL_VENDOR_INVOICE_HEAD.Against_PurchaseReturn_No=TSPL_PR_HEAD.PR_No" + Environment.NewLine
        qry += " left outer join TSPL_PI_HEAD on TSPL_PI_HEAD.PI_No=TSPL_PR_DETAIL.PI_Id" + Environment.NewLine
        qry += " left outer join TSPL_VENDOR_INVOICE_HEAD as PIVendorInvooiceHead on PIVendorInvooiceHead.Against_POInvoice_No=TSPL_PI_HEAD.PI_No" + Environment.NewLine
        qry += " where TSPL_VENDOR_INVOICE_HEAD.Document_Type= 'D'"
        If clsCommon.myLen(strAPDocumentNo) > 0 Then
            qry += " and PIVendorInvooiceHead.Document_No='" + strAPDocumentNo + "'"
        End If
        qry += " union all" + Environment.NewLine
        qry += " select APVendorInvoiceNo.Document_No,TSPL_VENDOR_INVOICE_HEAD.Document_Total-TSPL_VENDOR_INVOICE_HEAD.TDS_Actual_Amount as Amt,(case when TSPL_VENDOR_INVOICE_HEAD.Document_Type ='C' then 1 else case when TSPL_VENDOR_INVOICE_HEAD.Document_Type='D' then -1 else 0 end end) as RI,0 as Chk " + Environment.NewLine
        qry += " from TSPL_VENDOR_INVOICE_HEAD " + Environment.NewLine
        qry += " left outer join TSPL_VENDOR_INVOICE_HEAD as APVendorInvoiceNo on APVendorInvoiceNo.Document_No=TSPL_VENDOR_INVOICE_HEAD.RefDocNo" + Environment.NewLine
        qry += " where  TSPL_VENDOR_INVOICE_HEAD.RefDocType='AP'"
        If clsCommon.myLen(strAPDocumentNo) > 0 Then
            qry += "  and APVendorInvoiceNo.Document_No='" + strAPDocumentNo + "'"
        End If
        qry += Environment.NewLine + " union all" + Environment.NewLine
        qry += " select TSPL_PAYMENT_DETAIL.Document_No,TSPL_PAYMENT_DETAIL.Applied_Amount as Amt,-1 as RI ,0 as Chk" + Environment.NewLine
        qry += " from TSPL_PAYMENT_DETAIL" + Environment.NewLine
        qry += " left outer join TSPL_PAYMENT_HEADER on TSPL_PAYMENT_HEADER.Payment_No= TSPL_PAYMENT_DETAIL.Payment_No" + Environment.NewLine
        qry += " where  TSPL_PAYMENT_HEADER.Payment_Type in ('PY','AV') and TSPL_PAYMENT_HEADER.Posted=1 "
        If clsCommon.myLen(strAPDocumentNo) > 0 Then
            qry += " and TSPL_PAYMENT_DETAIL.Document_No='" + strAPDocumentNo + "'"
        End If
        qry += Environment.NewLine + " ) xxx" + Environment.NewLine
        qry += " group by Document_No having sum(chk)>0" + Environment.NewLine
        qry += " )xxxx" + Environment.NewLine
        qry += " inner join TSPL_VENDOR_INVOICE_HEAD on TSPL_VENDOR_INVOICE_HEAD.Document_No=xxxx.Document_No" + Environment.NewLine
        Return clsDBFuncationality.ExecuteNonQuery(qry, trans)
    End Function

    Public Shared Function GetCapexCombo() As DataTable
        Dim dt As DataTable = New DataTable()
        dt.Columns.Add("Code", GetType(String))
        dt.Columns.Add("Name", GetType(String))
        Dim dr As DataRow = dt.NewRow()
        dr("Code") = ""
        dr("Name") = "None"
        dt.Rows.Add(dr)

        dr = dt.NewRow()
        dr("Code") = "Capex"
        dr("Name") = "Capex"
        dt.Rows.Add(dr)

        dr = dt.NewRow()
        dr("Code") = "Regular"
        dr("Name") = "Regular"
        dt.Rows.Add(dr)
        Return dt
    End Function
End Class

Public Class clsEmailSMSConstants
    '----------------complaint detail entry-------------
    Public Const Complnt_code As String = "$comp_id$"
    Public Const Assetcode As String = "$item_code$"
    Public Const outlet As String = "$cust_code$"
    Public Const complnt_date As String = "$comp_date$"
    Public Const SerivceDealer As String = "$Executive_Code$"
    '---------------------------------------------------


    '----------------HR EM Resignation Letter-------------
    Public Const doccode As String = "$doccode$"
    Public Const docdate As String = "$docdate$"
    Public Const EmpCode As String = "$EmpCode$"
    Public Const EmpName As String = "$EmpName$"
    Public Const DepCode As String = "$DepCode$"
    Public Const DepName As String = "$DepName$"
    Public Const ResonOfResignation As String = "$ResonOfResignation$"
    Public Const ResignationDate As String = "$ResignationDate$"
    Public Const Remarks As String = "$Remarks$"
    Public Const HandoverCode As String = "$HandoverCode$"
    Public Const HandoverName As String = "$HandoverName$"
    '---------------------------------------------------

    '----------------Sale Order------------------------
    Public Const SaleOrderNo As String = "$DocNo$"
    Public Const SaleOrderDate As String = "$DocDate$"
    Public Const VendorNo As String = "$VendorNo$"
    Public Const VendorName As String = "$VendorName$"
    Public Const ContactPerson As String = "$ContactPerson$"
    Public Const TotalAmount As String = "$TotalAmount$"
    Public Const RoundOffAmount As String = "$RoundOffAmount$"
    '------------------------------------------------------

    '----------------Delivery Note Fresh Sale------------------------
    Public Const DeliveryNo As String = "$DocNo$"
    Public Const DeliveryDate As String = "$DocDate$"
    Public Const LocationCode As String = "$LocationCode$"
    Public Const LocationName As String = "$LocationName$"
    Public Const BookingNo As String = "$BookingNo$"
    '------------------------------------------------------

    Public Const CustomerNo As String = "$CustomerNo$"
    Public Const CustomerName As String = "$CustomerName$"
    Public Const InvoiceNo As String = "$Purchase InvoiceNo$"

    '---------------Sale register------------------
    Public Const FromDate As String = "$From Date$"
    Public Const ToDate As String = "$To Date$"
    Public Const ReportType As String = "$Summary Or Detail$"
    Public Const InvoiceType As String = "$Invoice Type$"
    '----------------Purchase Requistion------------------------
    Public Const PurchaseRequisitionNo As String = "$PurchaseRequisitionNo$"
    Public Const PurchaseRequisitionDate As String = "$PurchaseRequisitionDate$"

    'Public Const VendorNo As String = "$VendorNo$"
    'Public Const VendorName As String = "$VendorName$"
    'Public Const ContactPerson As String = "$ContactPerson$"
    'Public Const TotalAmount As String = "$TotalAmount$"
    '------------------------------------------------------
    '---------------Quality Check------------------
    Public Const QcNo As String = "$QC No$"
    Public Const inDateTime As String = "$In Date Time$"
    Public Const outDateTime As String = "$Out Date Time$"

    Public Const Form_Code As String = "$FormId$"
    Public Const UserCode As String = "$UserCode$"

    '-------------RFQ---------------------
    Public Const RFQ_No As String = "$DOC No$"
    Public Const RFQ_Date As String = "$DOC DATE$"
    Public Const Request_No As String = "$REQ NO$"
    Public Const Request_Date As String = "$REQ Date$"
    Public Const Request_Amount As String = "$REQ Amt$"
    '-----------------------------------------------------

    '' Anubhooti 25-Aug-2014 BM00000003528
    '-------------Offer Letter HR---------------------
    Public Const App_No As String = "$Applicant No$"
    Public Const Offer_Date As String = "$Offer Date$"
    Public Const DOJ As String = "$DOJ$"
    Public Const Salary As String = "$Salary$"
    Public Const ApplicantName As String = "$Applicant Name$"
    '' Anubhooti 25-Aug-2014 BM00000003528
    '-------------Appointment Letter HR---------------------
    Public Const Appointment_Date As String = "$Appointment Date$"
    '-----------------------------------------------------
    '' Anubhooti 20-Oct-2015 BM00000008219
    '-------------Service Call SW---------------------
    Public Const Call_No As String = "$Service Call No$"
    Public Const Call_Date As String = "$Call Date$"
    Public Const Problem_Type As String = "$Problem_Type$"
    Public Const Subject As String = "$Subject$"
    Public Const ItemPartNo As String = "$Item Part No$"
    Public Const IssueNotice As String = "$Issue Notice$"
    Public Const AssignedTo As String = "$Assigned To$"
    Public Const AssignedBy As String = "$Assigned By$"
    '=================CSA DO====================
    Public Const DOC_NO As String = "$Document No$"
    Public Const DOC_Date As String = "$Document Date$"
    Public Const Cust_Name As String = "$CSA Name$"
    Public Const From_Location As String = "$From Location$"
    Public Const RT_Detail As String = "$RT Rate And UOM$"
    Public Const CSA_Item_Type As String = "$CSA Item Type$"
    Public Const Doc_Amount As String = "$Document Amount$"

    '-------------Leave Application---------------------
    Public Const Leave_App_No As String = "$Application No$"
    Public Const Application_Date As String = "$Application Date$"
    Public Const Leave_From As String = "$Leave From$"
    Public Const Leave_To As String = "$Leave To$"
    Public Const Leave_Type As String = "$Leave Type$"
    Public Const Leave_Days As String = "$Leave Days$"
    Public Const Leave_Reason As String = "$Leave Reason$"
    Public Const Employee_Name As String = "$Employee Name$"
    Public Const EMP_CODE As String = "$Employee Code$"

    '-------------Employeee Master---------------------
    Public Const Birth_Date As String = "$Birth Date$"
    Public Const AnniversaryDate As String = "$Anniversary Date$"
    Public Const ProbPeriodEnDate As String = "$Probation Period End Date$"

    '----------------Milk Shift End------------------------
    Public Const Doc_Code As String = "$DocNo$"
    'Public Const Doc_Date As String = "$DocDate$"
    Public Const Mcc_Code As String = "$Mcc_Code$"
    Public Const Mcc_Name As String = "$Mcc_Name$"
    Public Const Shift As String = "$Shift$"
    Public Const User_Code As String = "$Created_By$"
    Public Const State_Name As String = "$State$"
    Public Const Total_collection As String = "$Total_collection$"

    Public Const FAT As String = "$FAT$"
    Public Const SNF As String = "$SNF$"
    Public Const Rate As String = "$Rate$"
    Public Const Amount As String = "$Amount$"

    Public Const UOM As String = "$UOM$"
    ''richa agarwal against ticket no BM00000008361
    Public Const VLCCode As String = "$VLC_Code$"
    Public Const VLCUploaderCode As String = "$VLCUploaderCode$"
    Public Const VLCName As String = "$VLC_Name$"
    Public Const CowQty As String = "$Cow_Qty$"
    Public Const BuffaloQty As String = "$Buffalo_Qty$"
    Public Const CowFat As String = "$CowFat_%$"
    Public Const BuffaloFat As String = "$BuffaloFat%$"
    Public Const CowSNF As String = "$CowSNF_%$"
    Public Const BuffaloSNF As String = "$BuffaloSNF%$"
    Public Const CowAmount As String = "$Cow_Amount$"
    Public Const BuffaloAmount As String = "$Buffalo_Amount$"

    '----------------MCC Master------------------------
    Public Const Shift_Open_Time As String = "$Shift_Open_Time$"
    Public Const Total_Route As String = "$Total_Route$"
    Public Const Total_Vlc As String = "$Total_Vlc$"
    Public Const Shift_Close_Time As String = "$Shift_Close_Time$"
    '------------------------------------------------------

    Public Const CompanyName As String = "$CompanyName$"
End Class

Public Class AdjustmentEnum
    Public Const strCostTransaction As String = "Store Adjustment"
    Public Const strJWInvetoryTrans As String = "Job Work Inventory"
    Public Const strCostTransactionProductionEntry As String = "Production Entry"
    Public Const strCostTransactionEmpty As String = "Empty Transactions"
End Class

Public Class clsEmailAndSMSRecipients
    Public Const strTransTrype As String = "POS"
End Class

Public Enum EnumTaxCalucationType
    Automatic = 0
    Mannual = 1
End Enum

Public Enum EnumControlType
    CheckBox = 0
    TextBox = 1
    NumericBox = 2
End Enum
Public Enum EnumTecxpertPaperSize
    NA = 0
    PaperSize10x12 = 1
    PaperSize10x6 = 2
    Guntur10x12 = 3
    HalfLegal85x7 = 4
End Enum

Public Enum EnumExportTo
    Excel = 0
    PDF = 1
    Refresh = 2
End Enum

Public Enum EnuChartType
    Bar = 1
    Pie = 2
    Line = 3
    Area = 4
End Enum

Public Enum EnumCustomFieldType
    TextType = 0
    NumberType = 1
    DateType = 2
    CheckType = 3
    FinderType = 4
    ComboListBoxType = 5
    PictureType = 6
    MultilineTextType = 7
    Buttons = 8
    RadioButtonType = 9
    GridType = 10


End Enum

Public Enum EnumConditionType
    StartsWith = 0
    DoesNotStartsWith = 1
    EndsWith = 2
    DoesNotEndsWith = 3
    EqualsTo = 4
    DoesNotEqualsTo = 5
    Contains = 6
    DoesNotContains = 7
    Between = 8
    GreaterThan = 9
    GreaterThanOrEquals = 10
    LessThan = 11
    LessThanOrEquals = 12
End Enum

Public Enum EnumCostingMethod
    NA = 0
    Averege = 1
    FIFO = 2
    LIFO = 3
    AveregeIn = 4
End Enum

Public Enum DBDataType
    image_Type = 0
    int_Type = 1
    decimal_Type = 2
    varbinary_Type = 3
    text_Type = 4
    datetime_Type = 5
    time_Type = 6
    varchar_Type = 7
    numeric_Type = 8
    nchar_Type = 9
    float_Type = 10
    date_Type = 11
    char_Type = 12
    bigint_Type = 13
    bit_Type = 14
    nvarchar_Type = 15
    NotApplicable = 16
End Enum

Public Enum PostingColumnType
    TEXT
    NUMBER
End Enum

Public Enum PostingStatusValueList
    YES
    Y
    ONE
End Enum

Public Enum Exporter
    Excel = 0
    PDF = 1
    Print = 2
    Refresh = 3
End Enum

Public Enum CrystalReportFolder
    Purchase = 1
    FixedAssets = 2
    CommonServices = 3
    GeneralLedger = 4
    HRPayroll = 5
    HumanResource = 6
    InventoryReport = 7
    KwalitySalesReport = 8
    MilkProcurement = 9
    NewSalesReports = 10
    PRODUCTION = 11
    PurchaseOrder = 12
    SalesReport = 13
    ServiceReport = 14
    TDS = 15
    UtilityReports = 16
End Enum


Public Class clsItemRowType
    Public Const RowTypeItem As String = "Item"
    Public Const RowTypeMisc As String = "Misc"
End Class
Public Class MIlkComponentType
    Public FAT_Per As Decimal = 0
    Public SNF_Per As Decimal = 0
    Public FAT_Cost As Decimal = 0
    Public SNF_Cost As Decimal = 0
    Public FAT_Kg As Decimal = 0
    Public SNF_Kg As Decimal = 0
End Class

Public Enum EnumTransType
    JournalEntry
    RcptEntry
    PymntEntry
    BankTransfer
    LoadOut
    SaleInvoice
    PurchaseInvoice
    SRN
    ICAdj
    MMTrans
    BankRvrs
    RcptADJ
    APInvoice
    SaleOrder
    SaleReturn
    SaleReturnInter
    ARInvoice
    PurchaseReturn
    ScrapInvoice
    ScrapShipment
    IssueReturnTransfer
    NRGP
    RGP
    SDShipment
    SDSaleReturn
    SaleInvoiceDemo
    Assemblies
    WareHouseBreakage
    VCGL
    APInvoiceTDS
    SaleQuotation
    GLAccount
    frmSalesmanTarget
    PuchaseOrder
    PurchaseIndent
    ExpiredItemEntry
    MilkSRN
    MilkPI
    productionEntry
    transfer
    SDCSATrans
    SDCSASale
    SDCSADO
    Bank_Guarantee_Master
    RICE_PROC
    RICE_MIX
    PP_ISSUE
    MCC_Material
    EXPORT_SO
    EXPORT_QUOTATION
    EXPORT_PROFORMA
    DispChallan
    MilkTransferIn
    Fresh_Sale
    Sale_Return
    Product_Invoice
    Product_Return_Sale
    CSA_Invoice
    EXPORT_Invoice
    Bulk_Invoice
    Bulk_Return
    CrateReceived
    InvoiceFreshSale
    MCCMaterialFrm
    SD_CSATRANS_RETURN
    MCCMaterialSaleReturn
    Export_Commercial_Inv
    PP_STDN
    PP_SP
    BulkSRNTrade
    DispatchBSTrade
    DispatchBS
    BulkSRN
    PRD_STG_PROC
    VSPTRAN
    PROD_ENTRY
    Export_Sale_Return
    FS_SH
    PS_SH
    MCC_MSRN
    MT_Sale_Order
    MT_Proforma
    MT_Comm_Inv
    MT_Sale_Inv
    MT_Sale_Ret
    MT_Sale_Qu
    MilkReceipt
    Mcc_transfer
    Bulk_Purchase
    GRN
    MRN
    VendorServiceCharge
    ComplaintDetailEntry
    DeliveryOrderPS
    Bulk_Purchase_Return
    Jobwork_Transfer_Milk
    Jobwork_Transfer_Other
    Production_Return
    TransferReturn
    MilkTransferInReturn
    PaymentAdjustmentEntry
    JobWork_SRN
    JobWork_SRN_RETURN
    Jobwork_Transfer_Other_Return
    Jobwork_Transfer_Milk_Return
    CanSale
End Enum

Public Class clsOpenTransactionForm

    Dim strRvalue As String = ""
    Function getNavigatorValue(ByRef formname As FrmMainTranScreen, Optional ByVal contrl As Control = Nothing) As String
        If clsCommon.myLen(strRvalue) > 0 Then
            Return strRvalue
            Exit Function
        End If

        If IsNothing(contrl) Then
            For Each ctrl As Control In formname.Controls
                If ctrl.HasChildren = True Then
                    'getNavigatorValue(Me, ctrl)
                End If
                If TypeOf (ctrl) Is common.UserControls.txtNavigator Then
                    Try
                        strRvalue = clsCommon.myCstr(CType(ctrl, common.UserControls.txtNavigator).Value)
                    Catch ex As Exception
                        MessageBox.Show(ex.ToString)
                    End Try
                End If
            Next
        Else
            For Each ctrl As Control In contrl.Controls
                If ctrl.HasChildren = True Then
                    ' getNavigatorValue(Me, ctrl)
                End If
                If TypeOf (ctrl) Is common.UserControls.txtNavigator Then
                    Try
                        strRvalue = clsCommon.myCstr(CType(ctrl, common.UserControls.txtNavigator).Value)
                    Catch ex As Exception
                        MessageBox.Show(ex.ToString)
                    End Try
                End If
            Next
        End If
        If clsCommon.myLen(strRvalue) > 0 Then
            Return strRvalue
            Exit Function
        End If
        Return ""
    End Function

    

End Class
